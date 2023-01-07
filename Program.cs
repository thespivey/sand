using Sand.Bluetooth;
using Sand.Messages;
using Sand.Messages.Serialization;
using System.CommandLine;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Sand;

public static class Program
{
    private const ushort PatternId = 510;
    private static readonly List<Point> Pattern = new() {
        new Point{ Angle=0, Radius=0 },
        new Point{ Angle=0, Radius=1 },
        new Point{ Angle=0, Radius=0 },
        new Point{ Angle=90, Radius=0 },
        new Point{ Angle=90, Radius=1 },
        new Point{ Angle=90, Radius=0 },
        new Point{ Angle=180, Radius=0 },
        new Point{ Angle=180, Radius=1 },
        new Point{ Angle=180, Radius=0 },
        new Point{ Angle=270, Radius=0 },
        new Point{ Angle=270, Radius=1 },
        new Point{ Angle=270, Radius=0 },
    };

    public static async Task Main(string[] args)
    {
        var root = new RootCommand();

        var find = new Command("find");
        find.SetHandler(Find);
        root.AddCommand(find);

        var connect = new Command("connect");
        var device = new Argument<string>("device");
        connect.AddArgument(device);
        connect.SetHandler(Connect, device);
        root.AddCommand(connect);

        await root.InvokeAsync(args);
    }

    private static async Task Find()
    {
        Console.WriteLine("Searching for device");
        Discovery discovery = new();
        var deviceId = await discovery.WaitForDeviceAsync();
        Console.WriteLine($"Found device: {deviceId}");
    }

    private static async Task Connect(string deviceId)
    {
        Device device = new();
        device.ConnectionStatusChanged += status => Console.WriteLine($"Status: {status}");
        device.MessageReceived += data =>
        {
            MessageFromDevice message;
            try
            {
                message = MessageFromDevice.Parse(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERR   {ex}");
                return;
            }

            Console.WriteLine($"RECV  {message}");

            MessageToDevice? reply = message switch
            {
                WaitForConnectionData _ => new ConfirmConnectionData(),
                AckFromDevice _ => null,
                _ => new AckToDevice()
            };

            if (reply != null)
            {
                Console.WriteLine($"REPLY {reply.GetType().Name}");
                device.SendMessage(reply.Serialize());
            }
        };

        await device.ConnectAsync(deviceId);

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            MessageToDevice? message = key.Key switch
            {
                ConsoleKey.F => new UpdateOtherLightControlStates(shift: true, shiftTime: 5, breath: true, brightness: 0),
                ConsoleKey.N => new UpdateOtherLightControlStates(shift: true, shiftTime: 5, breath: true, brightness: 100),
                ConsoleKey.S => new StartDownloadingFile(PatternId, 1),
                ConsoleKey.E => new EndDownloadingFile(PatternId, 1),
                ConsoleKey.D => new ContentsOfFileSent(1, Pattern),
                ConsoleKey.X => new SinglePlotDrawingOperation(PatternId),
                ConsoleKey.P => new InitiateOrPauseDrawing(),
                _ => null
            };

            if (message != null)
            {
                Console.WriteLine($"SEND  {message.GetType().Name}");
                device.SendMessage(message.Serialize());
            }
        }
    }
}
