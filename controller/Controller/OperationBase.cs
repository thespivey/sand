using Sand.Bluetooth;
using Sand.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Sand.Controller
{
    internal abstract class OperationBase
    {
        private readonly CancellationTokenSource _cancel = new();
        private readonly Dispatcher _dispatcher = new();
        private readonly Device _device;
        private Func<MessageFromDevice, Task>? _messageHandler;

        public OperationBase(Device device)
        {
            _device = device;
            _device.MessageReceived += Device_MessageReceived;
        }

        public async Task RunAsync()
        {
            await _dispatcher.RunAsync(_cancel.Token);
        }

        protected virtual async Task HandleMessageAsync(MessageFromDevice message)
        {
            if (message is WaitForConnectionData)
            {
                Console.WriteLine("Connecting...");
                await SendMessageAsync(new ConfirmConnectionData());
            }
            else
            {
                await SendMessageAsync(new Ack());
            }

            if (_messageHandler != null)
            {
                await _messageHandler.Invoke(message);
            }
        }

        protected void SetMessageHandler(Action<MessageFromDevice> handler)
        {
            _messageHandler = message =>
            {
                handler(message);
                return Task.CompletedTask;
            };
        }

        protected void SetMessageHandler(Func<MessageFromDevice, Task> handler)
        {
            _messageHandler = handler;
        }

        protected async Task SendMessageAsync(MessageToDevice message)
        {
            await _device.SendMessageAsync(message.Serialize());
        }

        protected void Exit()
        {
            _messageHandler = null;
            _cancel.Cancel();
        }

        private void Device_MessageReceived(IBuffer buffer)
        {
            _dispatcher.Post(async () =>
            {
                MessageFromDevice message;
                try
                {
                    message = MessageFromDevice.Parse(buffer);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Parse error: {e}");
                    return;
                }

                await HandleMessageAsync(message);
            });
        }
    }
}
