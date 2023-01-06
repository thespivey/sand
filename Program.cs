using Sand;
using Sand.Bluetooth;
using Sand.Messages;

// TODO: Factor out device discovery & see if we can connect faster by persisting the id
Device device = new();

device.MessageReceived += (data) =>
{
    MessageFromDevice message;
    try
    {
        message = MessageFromDevice.Parse(data);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex}");
        return;
    }

    Console.WriteLine($"Received {message}");

    MessageToDevice? reply = message switch
    {
        WaitForConnectionData _ => new ConfirmConnectionData(),
        AckFromDevice _ => null,
        _ => new AckToDevice()
    };

    if (reply != null)
    {
        Console.WriteLine($"Replying {reply}");
        device.SendMessage(reply.Serialize());
    }
};

device.Connect();

while (true)
{
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Enter)
    {
        break;
    }


    MessageToDevice? message = key.Key switch
    {
        ConsoleKey.F => new UpdateOtherLightControlStates(true, 5, false, 0),
        ConsoleKey.N => new UpdateOtherLightControlStates(true, 5, false, 100),
        _ => null
    };

    if (message != null)
    {
        Console.WriteLine($"Sending {message}");
        device.SendMessage(message.Serialize());
    }
}
