using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace Sand.Bluetooth
{
    internal class Device
    {
        private static readonly Guid ServiceId = new Guid("0000ffe0-0000-1000-8000-00805f9b34fb");
        private static readonly Guid ReadCharacteristicId = new Guid("0000ffe4-0000-1000-8000-00805f9b34fb");
        private static readonly Guid WriteCharacteristicId = new Guid("0000ffe9-0000-1000-8000-00805f9b34fb");

        private BluetoothLEDevice? device;
        private GattDeviceService? service;
        private GattCharacteristic? readCharacteristic;
        private GattCharacteristic? writeCharacteristic;
        private DeviceWatcher? deviceWatcher;

        public event Action<IBuffer>? MessageReceived;

        public void Connect()
        {
            deviceWatcher = DeviceInformation.CreateWatcher(
                "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")",
                new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" },
                DeviceInformationKind.AssociationEndpoint);
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Start();
            Console.WriteLine("Searching for device");
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            if (deviceInfo.Name.StartsWith("KS"))
            {
                Console.WriteLine($"Found device {deviceInfo.Name.Trim()}");

                device = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
                device.ConnectionStatusChanged += (_, _) => Console.WriteLine($"Connection status {device.ConnectionStatus}");

                Console.WriteLine("Querying for service");
                GattDeviceServicesResult serviceResult;
                do
                {
                    serviceResult = await device.GetGattServicesForUuidAsync(ServiceId, BluetoothCacheMode.Uncached);
                } while (serviceResult.Services.Count == 0);
                service = serviceResult.Services[0];

                Console.WriteLine("Querying for read characteristic");
                var readCharacteristicResult = await service.GetCharacteristicsForUuidAsync(ReadCharacteristicId, BluetoothCacheMode.Uncached);
                readCharacteristic = readCharacteristicResult.Characteristics[0];

                Console.WriteLine("Querying for write characteristic");
                var writeCharacteristicResult = await service.GetCharacteristicsForUuidAsync(WriteCharacteristicId, BluetoothCacheMode.Uncached);
                writeCharacteristic = writeCharacteristicResult.Characteristics[0];

                Console.WriteLine("Monitoring for changes");
                readCharacteristic.ValueChanged += ReadCharacteristic_ValueChanged;
                await readCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            }
        }

        private void ReadCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            MessageReceived?.Invoke(args.CharacteristicValue);
        }

        public async void SendMessage(IBuffer message)
        {
            await writeCharacteristic.WriteValueAsync(message);
        }
    }
}
