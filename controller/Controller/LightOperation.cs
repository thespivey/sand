using Sand.Bluetooth;
using Sand.Model;
using Sand.Protocol;
using System;
using System.Threading.Tasks;

namespace Sand.Controller
{
    internal class LightOperation : OperationBase
    {
        private readonly float _brightness;
        public LightOperation(Device device, float brightness)
            : base(device)
        {
            _brightness = brightness;
            SetMessageHandler(WaitForConnection);
        }

        private void WaitForConnection(MessageFromDevice message)
        {
            if (message is AllCompletesData)
            {
                Console.WriteLine("Connected");
                SetMessageHandler(WaitForLightData);
            }
        }

        private async Task WaitForLightData(MessageFromDevice message)
        {
            if (message is AllStateDataDuringTheRun current)
            {
                LightSettings settings = current.Light;
                settings.Brightness = _brightness;

                Console.WriteLine($"Updating {settings}");
                await SendMessageAsync(new UpdateOtherLightControlStates(settings));

                Exit();
            }
        }
    }
}
