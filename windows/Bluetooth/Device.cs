using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Intrinsics.Arm;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace Sand.Bluetooth;

internal class Device
{
    private static readonly Guid ServiceId = new Guid("0000ffe0-0000-1000-8000-00805f9b34fb");
    private static readonly Guid ReadCharacteristicId = new Guid("0000ffe4-0000-1000-8000-00805f9b34fb");
    private static readonly Guid WriteCharacteristicId = new Guid("0000ffe9-0000-1000-8000-00805f9b34fb");

    private BluetoothLEDevice? _device;
    private GattDeviceService? _service;
    private GattCharacteristic? _readCharacteristic;
    private GattCharacteristic? _writeCharacteristic;

    public event Action<BluetoothConnectionStatus>? ConnectionStatusChanged;
    public event Action<IBuffer>? MessageReceived;

    public async Task ConnectAsync(string id)
    {
        _device = await BluetoothLEDevice.FromIdAsync(id);
        _device.ConnectionStatusChanged += (_, _) => ConnectionStatusChanged?.Invoke(_device.ConnectionStatus);
        
        GattDeviceServicesResult serviceResult;
        do
        {
            serviceResult = await _device.GetGattServicesForUuidAsync(ServiceId, BluetoothCacheMode.Uncached);
        } while (serviceResult.Services.Count == 0);
        _service = serviceResult.Services[0];

        _readCharacteristic = await GetCharacteristic(ReadCharacteristicId);
        _writeCharacteristic = await GetCharacteristic(WriteCharacteristicId);

        await _readCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Indicate);
        _readCharacteristic.ValueChanged += ReadCharacteristic_ValueChanged;
    }

    private async Task<GattCharacteristic> GetCharacteristic(Guid id)
    {
        GattCharacteristicsResult result;
        do
        {
            result = await _service.GetCharacteristicsForUuidAsync(id, BluetoothCacheMode.Uncached);
        } while (result.Characteristics.Count == 0);
        return result.Characteristics[0];
    }

    private void ReadCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
    {
        MessageReceived?.Invoke(args.CharacteristicValue);
    }
    public async void SendMessage(IBuffer message)
    {
        var status = await _writeCharacteristic.WriteValueAsync(message, GattWriteOption.WriteWithResponse);
        if (status != GattCommunicationStatus.Success)
        {
            throw new Exception($"Write error {status}");
        }
    }
}
