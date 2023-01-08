using Sand.Bluetooth;
using Sand.Model;
using Sand.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sand.Controller
{
    internal class PatternOperation : OperationBase
    {
        private const int PointsPerPage = 40;

        private readonly Pattern _pattern;
        private uint _numPages;
        private DateTime _timeout;

        public PatternOperation(Device device, Pattern pattern)
            : base(device)
        {
            _pattern = pattern;
            _numPages = (uint)((_pattern.Points.Count + PointsPerPage - 1) / PointsPerPage);
            SetMessageHandler(WaitForConnection);
        }


        private List<Point> GetPage(int page)
        {
            var result = new List<Point>();
            var baseIndex = page * PointsPerPage;
            var numPoints = Math.Min(PointsPerPage, _pattern.Points.Count - baseIndex);
            for (int i = 0; i < numPoints; i++)
            {
                result.Add(_pattern.Points[baseIndex + i]);
            }
            return result;
        }

        private async void WaitForConnection(MessageFromDevice message)
        {
            if (message is AllCompletesData)
            {
                Console.WriteLine("Connected");
                await StartSendingAsync();
            }
        }

        private async Task StartSendingAsync()
        {
            _timeout = DateTime.Now + TimeSpan.FromSeconds(5);
            await SendMessageAsync(new StartDownloadingFile(_pattern.PatternId, _numPages));
            SetMessageHandler(AfterStart);
        }

        private async Task AfterStart(MessageFromDevice message)
        {
            if (message is FileStartsToReceive s && s.PatternId == _pattern.PatternId)
            {
                for (int i =0; i < _numPages; i++)
                {
                    // TOOD: For a large number of pages I think we will fail here.
                    // Need to yield to incoming messages, maybe even to dispatcher idle.
                    Console.WriteLine($"Sending page {i}");
                    await SendMessageAsync(new ContentsOfFileSent((ushort)(i + 1), GetPage(i)));
                }
                await SendMessageAsync(new EndDownloadingFile(_pattern.PatternId, _numPages));
                SetMessageHandler(AfterSend);
            }
            else if (_timeout < DateTime.Now)
            {
                await StartSendingAsync();
            }
        }

        private async void AfterSend(MessageFromDevice message)
        {
            if (message is FileReceiptCompleted c && c.PatternId == _pattern.PatternId)
            {
                Console.WriteLine($"Drawing");
                await SendMessageAsync(new SinglePlotDrawingOperation(_pattern.PatternId));
                Exit();
            }
            else if (message is FileIsMissing missing)
            {
                // TODO: Replay these pages
                Console.Error.WriteLine($"Missing pages {string.Join(" ", missing.MissingPages)}");
            }

            // TODO: timeout
        }
    }
}
