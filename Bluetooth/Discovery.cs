using Windows.Devices.Enumeration;

namespace Sand.Bluetooth;

internal class Discovery
{
    private TaskCompletionSource<string> _deviceFound = new(); 
    private DeviceWatcher? _deviceWatcher;

    public Discovery()
    {
        _deviceWatcher = DeviceInformation.CreateWatcher(
            "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")",
            new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" },
            DeviceInformationKind.AssociationEndpoint);
        _deviceWatcher.Added += DeviceWatcher_Added;
        _deviceWatcher.Start();
    }

    public async Task<string> WaitForDeviceAsync()
    {
        return await _deviceFound.Task;
    }

    private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
    {
        if (deviceInfo.Name.StartsWith("KS"))
        {
            _deviceFound.TrySetResult(deviceInfo.Id);
        }
    }
}
