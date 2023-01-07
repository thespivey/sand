using System.Runtime.InteropServices.WindowsRuntime;
using Sand.Messages.Serialization;
using Windows.Storage.Streams;

namespace Sand.Messages;

internal abstract class MessageFromDevice
{
    public static MessageFromDevice Parse(IBuffer buffer)
    {
        var frame = Frame.Parse(buffer);
        var data = new Reader(frame.Data);

        return frame.Verb switch
        {
            Verb v when v == Verb.Ack => new AckFromDevice(frame.Topic, data),
            Verb v when v == Verb.WaitForConnectionData => new WaitForConnectionData(),
            Verb v when v == Verb.AllStateDataDuringTheRun => new AllStateDataDuringTheRun(data),
            _ => new UnknownMessage(frame.Topic, frame.Verb, data)
        };
    }
}

internal class UnknownMessage : MessageFromDevice
{
    private readonly IBuffer _data;

    public UnknownMessage(Topic topic, Verb verb, Reader data)
    {
        Topic = topic;
        Verb = verb;
        _data = data.DetachBuffer();
    }

    public Topic Topic { get; }
    public Verb Verb { get; }

    public override string ToString()
    {
        return $"{Topic.Name} {Verb.Name} {string.Join(" ", _data.ToArray())}";
    }
}

internal class AckFromDevice : MessageFromDevice
{
    private readonly Topic _topic;
    private readonly IBuffer _data;

    public AckFromDevice(Topic topic, Reader data)
    {
        _topic = topic;
        _data = data.DetachBuffer();
    }

    public override string ToString()
    {
        return $"Ack {_topic.Name} {string.Join(" ", _data.ToArray())}";
    }
}

internal class WaitForConnectionData : MessageFromDevice
{
}

internal class AllStateDataDuringTheRun : MessageFromDevice
{
    private const float MaxProgress = 10000;

    private readonly byte _brightness;
    private readonly Color _color;
    private readonly byte _speed;
    private readonly string _playlistId;
    private readonly ushort _patternId;
    private readonly ushort _patternProgress;
    private readonly byte _status;
    private readonly bool _randomMode;
    private readonly bool _loopMode;
    private readonly bool _sleepEnabled;
    private readonly bool _breathMode;
    private readonly bool _shiftingMode;
    private readonly ushort _shiftTime;

    public AllStateDataDuringTheRun(Reader reader)
    {
        reader.Read(out _brightness);
        reader.Read(out string _);
        reader.Read(out _color);
        reader.Read(out _speed);
        reader.Read(out _playlistId);
        reader.Read(out _patternId);
        reader.Read(out _patternProgress);
        reader.Read(out _status);
        reader.Read(out _randomMode);
        reader.Read(out _loopMode);
        reader.Read(out _sleepEnabled);
        reader.Read(out _breathMode);
        reader.Read(out _shiftingMode);
        reader.Read(out _shiftTime);
        reader.Read(out string _);
        reader.Read(out bool _);
        reader.Read(out byte _);
        reader.Read(out string _);
        reader.Read(out bool _);
        reader.Read(out byte _);
        reader.Read(out bool _);
    }

    public override string ToString()
    {
        return $"brightness {_brightness} " +
            $"color {_color.r} {_color.g} {_color.b} " +
            $"speed {_speed} " +
            $"playlist {_playlistId} pattern {_patternId} {_patternProgress/MaxProgress:0.##}% " +
            $"status {_status} " +
            (_randomMode ? "random " : "") +
            (_loopMode ? "loop " : "") +
            (_sleepEnabled ? "sleep " : "") +
            (_breathMode ? "breath " : "") +
            (_shiftingMode ? $"shift {_shiftTime} " : "");
    }
}
