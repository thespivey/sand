using System.Runtime.InteropServices.WindowsRuntime;
using Sand.Protocol.Serialization;
using Sand.Model;
using Windows.Storage.Streams;
using System;

namespace Sand.Protocol;

internal abstract class MessageFromDevice
{
    public static MessageFromDevice Parse(IBuffer buffer)
    {
        var frame = Frame.Parse(buffer);
        var data = new Reader(frame.Data);

        return frame.Verb switch
        {
            Verb v when v == Verb.WaitForConnectionData => new WaitForConnectionData(data),
            Verb v when v == Verb.AllStateDataDuringTheRun => new AllStateDataDuringTheRun(data),
            Verb v when v == Verb.StartSendingListData=> new StartSendingListData(data),
            Verb v when v == Verb.ListData  => new ListData  (data),
            Verb v when v == Verb.ListDataIsSentAtEnd => new ListDataIsSentAtEnd (data),
            Verb v when v == Verb.StartSendingListOfFavorites  => new StartSendingListOfFavorites  (data),
            Verb v when v == Verb.FavoriteListData  => new FavoriteListData  (data),
            Verb v when v == Verb.SendingListOfFavoritesIsComplete  => new SendingListOfFavoritesIsComplete  (data),
            Verb v when v == Verb.StartSendingLightColorData => new StartSendingLightColorData (data),
            Verb v when v == Verb.SendLightSettings  => new SendLightSettings  (data),
            Verb v when v == Verb.EndSendingLightColorData => new EndSendingLightColorData (data),
            Verb v when v == Verb.FatalError  => new FatalError  (data),
            Verb v when v == Verb.StartSendingPictureMessages => new StartSendingPictureMessages (data),
            Verb v when v == Verb.PictureInformation  => new PictureInformation  (data),
            Verb v when v == Verb.PictureMessageIsSentAtEnd => new PictureMessageIsSentAtEnd (data),
            Verb v when v == Verb.GetSleepTimeAndHibernationStatus  => new GetSleepTimeAndHibernationStatus  (data),
            Verb v when v == Verb.GetCurrentTimeFromApp => new GetCurrentTimeFromApp (data),
            Verb v when v == Verb.AllCompletesData => new AllCompletesData(data),
            Verb v when v == Verb.FileStartsToReceive => new FileStartsToReceive(data),
            Verb v when v == Verb.FileReceiptCompleted => new FileReceiptCompleted(data),
            Verb v when v == Verb.FileIsMissing => new FileIsMissing(data),
            Verb v when v == Verb.FileDownloadFailed => new FileDownloadFailed(data),
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

internal class WaitForConnectionData : MessageFromDevice
{
    public WaitForConnectionData(Reader data)
    { }
}

internal class AllStateDataDuringTheRun : MessageFromDevice
{
    private const float MaxProgress = 10000;

    private readonly LightSettings _light;
    private readonly Color _color;
    private readonly byte _speed;
    private readonly string _playlistId;
    private readonly ushort _patternId;
    private readonly ushort _progress;
    private readonly byte _status;
    private readonly bool _shuffle;
    private readonly bool _loop;

    public AllStateDataDuringTheRun(Reader reader)
    {
        reader.Read(out byte brightness);
        reader.Read(out string _);
        reader.Read(out _color);
        reader.Read(out _speed);
        reader.Read(out _playlistId);
        reader.Read(out _patternId);
        reader.Read(out _progress);
        reader.Read(out _status);
        reader.Read(out _shuffle);
        reader.Read(out _loop);
        reader.Read(out bool _);
        reader.Read(out bool enableBreath);
        reader.Read(out bool enableColorShift);
        reader.Read(out ushort colorShiftInterval);
        reader.Read(out string _);
        reader.Read(out bool _);
        reader.Read(out byte _);
        reader.Read(out string _);
        reader.Read(out bool _);
        reader.Read(out byte _);
        reader.Read(out bool _);

        _light.Brightness = brightness / 100.0f;
        _light.EnableColorShift = enableColorShift;
        _light.ColorShiftInterval = TimeSpan.FromSeconds(colorShiftInterval);
        _light.EnableBreath = enableBreath;
    }

    public LightSettings Light => _light;

    public override string ToString()
    {
        return $"AllStateDataDuringTheRun " +
            $"Status {_status} " +
            $"{_light} " +
            $"Color ({_color.r} {_color.g} {_color.b}) " +
            $"Drawing (pattern={_patternId} speed={_speed} progress={_progress / MaxProgress:0.##}%) " +
            $"Playlist (id={_playlistId}" +
                (_shuffle ? " shuffle" : "") +
                (_loop ? " loop" : "") +
            ")";
    }
}

internal class StartSendingListData : MessageFromDevice
{
    public StartSendingListData(Reader reader)
    { }
}

internal class ListData : MessageFromDevice
{
    public ListData(Reader reader)
    {
        // 1 1 68 101 109 111 0 1 4 0
    }
}

internal class ListDataIsSentAtEnd : MessageFromDevice
{
    public ListDataIsSentAtEnd(Reader reader)
    { }
}

internal class StartSendingListOfFavorites : MessageFromDevice
{
    public StartSendingListOfFavorites(Reader reader)
    {
        // 1
    }
}

internal class FavoriteListData : MessageFromDevice
{
    public FavoriteListData(Reader reader)
    {
        // 1 1 4 0
    }
}
internal class SendingListOfFavoritesIsComplete : MessageFromDevice
{
    public SendingListOfFavoritesIsComplete(Reader reader)
    {
        // 1
    }
}

internal class StartSendingLightColorData : MessageFromDevice
{
    public StartSendingLightColorData(Reader reader)
    { }
}

internal class SendLightSettings : MessageFromDevice
{
    public SendLightSettings(Reader reader)
    {
        // 0 60 0 0 70
    }
}

internal class EndSendingLightColorData : MessageFromDevice
{
    public EndSendingLightColorData(Reader reader)
    { }
}

internal class FatalError : MessageFromDevice
{
    public FatalError(Reader reader)
    {
        // 1 13 0 
    }
}

internal class StartSendingPictureMessages : MessageFromDevice
{
    public StartSendingPictureMessages(Reader reader)
    { }
}

internal class PictureInformation : MessageFromDevice
{
    public PictureInformation(Reader reader)
    {
        // 1 6 4 0 159 0 25 2 32 2 255 1 254 1
    }
}

internal class PictureMessageIsSentAtEnd : MessageFromDevice
{
    public PictureMessageIsSentAtEnd(Reader reader)
    { }
}

internal class GetSleepTimeAndHibernationStatus : MessageFromDevice
{
    public GetSleepTimeAndHibernationStatus(Reader reader)
    {
        //   0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
    }
}

internal class GetCurrentTimeFromApp : MessageFromDevice
{
    public GetCurrentTimeFromApp(Reader reader)
    { }
}

internal class AllCompletesData : MessageFromDevice
{
    public AllCompletesData(Reader reader)
    { }
}

internal abstract class PatternMessage : MessageFromDevice
{
    private readonly ushort _patternId;

    public PatternMessage(Reader reader)
    {
        reader.Read(out _patternId);
    }

    public ushort PatternId => _patternId;
}

internal class FileStartsToReceive : PatternMessage
{
    public FileStartsToReceive(Reader reader)
        : base(reader)
    { }
}

internal class FileReceiptCompleted : PatternMessage
{
    public FileReceiptCompleted(Reader reader)
        : base(reader)
    { }
}

internal class FileIsMissing : MessageFromDevice
{
    private readonly ushort[] _missingPages;

    public FileIsMissing(Reader reader)
    {
        reader.Read(out byte length);
        _missingPages = new ushort[length];
        for (int i=0; i < length; i++)
        {
            reader.Read(out _missingPages[i]);
        }
    }

    public ushort[] MissingPages => _missingPages;
}

internal class FileDownloadFailed : PatternMessage
{
    public FileDownloadFailed(Reader reader)
        : base(reader)
    { }
}

