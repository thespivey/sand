using Sand.Bluetooth;
using Sand.Controller;
using Sand.Model;
using System.CommandLine;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sand;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var root = new RootCommand();

        root.AddCommand(Find());
        root.AddCommand(Light());
        root.AddCommand(Draw());

        await root.InvokeAsync(args);
    }

    private static Command Find()
    {
        var command = new Command("find");
        command.SetHandler(async () =>
        {
            Discovery discovery = new();
            var deviceId = await discovery.WaitForDeviceAsync();
            Console.WriteLine($"Found device: {deviceId}");
        });
        return command;
    }

    private static Command Light()
    {
        var command = new Command("light");
        var deviceId = new Argument<string>("deviceId");
        var brightness = new Argument<float>("brightness");
        command.AddArgument(deviceId);
        command.AddArgument(brightness);
        command.SetHandler(async (deviceId, brightness) =>
        {
            Device device = new();
            LightOperation operation = new(device, brightness);

            device.ConnectionStatusChanged += status => Console.WriteLine($"Bluetooth {status}");

            await device.ConnectAsync(deviceId);
            await operation.RunAsync();
        }, deviceId, brightness);
        return command;
    }

    private static Command Draw()
    {
        var command = new Command("draw");
        var deviceId = new Argument<string>("deviceId");
        var file = new Argument<FileInfo>("file");
        command.AddArgument(deviceId);
        command.AddArgument(file);
        command.SetHandler(async (deviceId, file) =>
        {
            var pattern = await PatternLoader.LoadThr(file);

            Device device = new();
            PatternOperation operation = new(device, pattern);

            device.ConnectionStatusChanged += status => Console.WriteLine($"Bluetooth {status}");

            await device.ConnectAsync(deviceId);
            await operation.RunAsync();
        }, deviceId, file);
        return command;
    }

}
