using Sand.Messages.Serialization;
using System.CommandLine;
using System.Data;
using Windows.Storage.Streams;

namespace Sand.Messages;

internal abstract class MessageToDevice
{
    private readonly Topic _topic;
    private readonly Verb _verb;

    public MessageToDevice(Topic topic, Verb verb)
    {
        _topic = topic;
        _verb = verb;
    }

    protected virtual void Write(Writer data) { }

    public IBuffer Serialize()
    {
        Writer data = new();
        Write(data);

        Frame messageFrame = new(_topic, _verb, data.DetachBuffer());
        return messageFrame.Serialize();
    }
}

internal class ConfirmConnectionData : MessageToDevice
{
    public ConfirmConnectionData()
      : base(Topic.AskNoConnectionData, Verb.ConfirmConnectionData)
    { }

    protected override void Write(Writer data)
    {
        data.Write(true);
    }

}

internal class AckToDevice : MessageToDevice
{
    public AckToDevice()
        : base(Topic.AskNoConnectionData, Verb.Ack)
    { }
}

internal class UpdateOtherLightControlStates : MessageToDevice
{
    private readonly bool _shift;
    private readonly ushort _shiftTime;
    private readonly bool _breath;
    private readonly byte _brightness;

    public UpdateOtherLightControlStates(bool shift, ushort shiftTime, bool breath, byte brightness)
        : base(Topic.SendsLightingAndColor, Verb.UpdateOtherLightControlStates)
    {
        _shift = shift;
        _shiftTime = shiftTime;
        _breath = breath;
        _brightness = brightness;
    }

    protected override void Write(Writer data)
    {
        data.Write(_shift);
        data.Write(_shiftTime);
        data.Write(_breath);
        data.Write(_brightness);
    }
}

internal class StartDownloadingFile : MessageToDevice
{
    private readonly ushort _patternId;
    private readonly uint _pages;

    public StartDownloadingFile(ushort patternId, uint pages)
        : base(Topic.SendsLargeFileOperations, Verb.StartDownloadingFile)
    {
        _patternId = patternId;
        _pages = pages;
    }

    protected override void Write(Writer data)
    {
        data.Write(_patternId);
        data.Write(_pages);
    }
}

internal class EndDownloadingFile : MessageToDevice
{
    private readonly ushort _patternId;
    private readonly uint _pages;

    public EndDownloadingFile(ushort patternId, uint pages)
        : base(Topic.SendsLargeFileOperations, Verb.EndDownloadingFile)
    {
        _patternId = patternId;
        _pages = pages;
    }

    protected override void Write(Writer data)
    {
        data.Write(_patternId);
        data.Write(_pages);
    }
}

internal class ContentsOfFileSent : MessageToDevice
{
    private readonly ushort _pageIndex;
    private readonly List<Point> _points;

    public ContentsOfFileSent(ushort pageIndex, List<Point> points)
        : base(Topic.SendsBigFileDataIsSent, Verb.ContentsOfFileSent)
    {
        _pageIndex = pageIndex;
        _points = points;
    }

    protected override void Write(Writer data)
    {
        data.Write((byte)_points.Count);
        data.Write(_pageIndex);
        foreach (var point in _points)
        {
            data.Write(point);
        }
    }
}

internal class SinglePlotDrawingOperation : MessageToDevice
{
    private ushort _patternId;

    public SinglePlotDrawingOperation(ushort patternId)
        : base(Topic.SendsUserActionsInstructions, Verb.SinglePlotDrawingOperation)
    {
        _patternId = patternId;
    }

    protected override void Write(Writer data)
    {
        data.Write(_patternId);
    }
}

internal class InitiateOrPauseDrawing : MessageToDevice
{
    public InitiateOrPauseDrawing()
        : base(Topic.SendsUserActionsInstructions, Verb.InitiateOrPauseDrawing)
    { }
}
