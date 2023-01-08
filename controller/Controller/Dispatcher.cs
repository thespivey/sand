using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Sand.Controller
{
    internal class Dispatcher
    {
        private readonly Channel<Func<Task?>> _queue = Channel.CreateUnbounded<Func<Task?>>();

        public async Task RunAsync(CancellationToken cancel)
        {
            var reader = _queue.Reader;
            while (await reader.WaitToReadAsync(cancel))
            {
                await foreach (var func in reader.ReadAllAsync(cancel))
                {
                    await (func.Invoke() ?? Task.CompletedTask);
                }
            }
        }

        public void Post(Func<Task?> func)
        {
            if (!_queue.Writer.TryWrite(func))
            {
                Console.Error.WriteLine("ERROR: Channel failure");
            }
        }
    }
}
