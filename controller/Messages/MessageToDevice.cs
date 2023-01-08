using Sand.Protocol.Serialization;
using Sand.Model;
using Windows.Storage.Streams;
using System.Collections.Generic;
using System;

namespace Sand.Protocol;

internal abstract class MessageToDevice
{
    private readonly Topic _topic;
    private readonly Verb _verb;
    private readonly bool _ack;

    public MessageToDevice(Topic topic, Verb verb, bool ack = false)
    {
        _topic = topic;
        _verb = verb;
        _ack = ack;
    }

    protected virtual void Write(Writer writer) { }

    public IBuffer Serialize()
    {
        Writer writer = new();
        Write(writer);

        Frame frame = new(_topic, _verb, _ack, writer.DetachBuffer());
        return frame.Serialize();
    }
}

internal class ConfirmConnectionData : MessageToDevice
{
    public ConfirmConnectionData()
      : base(Topic.AskNoConnectionData, Verb.ConfirmConnectionData)
    { }

    protected override void Write(Writer writer)
    {
        writer.Write(true);
    }

}

internal class Ack : MessageToDevice
{
    public Ack()
        : base(Topic.AskNoConnectionData, Verb.None, ack: true)
    { }
}

internal class UpdateOtherLightControlStates : MessageToDevice
{
    private readonly LightSettings _settings;

    public UpdateOtherLightControlStates(LightSettings settings)
        : base(Topic.SendsLightingAndColor, Verb.UpdateOtherLightControlStates)
    {
        _settings = settings;
    }

    protected override void Write(Writer writer)
    {
        writer.Write(_settings.EnableColorShift);
        writer.Write((ushort)Math.Round(_settings.ColorShiftInterval.TotalSeconds));
        writer.Write(_settings.EnableBreath);
        writer.Write((byte)Math.Round(_settings.Brightness * 100));
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

    protected override void Write(Writer writer)
    {
        writer.Write(_patternId);
        writer.Write(_pages);
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

    protected override void Write(Writer writer)
    {
        writer.Write(_patternId);
        writer.Write(_pages);
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

    protected override void Write(Writer writer)
    {
        writer.Write((byte)_points.Count);
        writer.Write(_pageIndex);
        foreach (var point in _points)
        {
            writer.Write(point);
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

    protected override void Write(Writer writer)
    {
        writer.Write(_patternId);
    }
}

internal class InitiateOrPauseDrawing : MessageToDevice
{
    public InitiateOrPauseDrawing()
        : base(Topic.SendsUserActionsInstructions, Verb.InitiateOrPauseDrawing)
    { }
}
