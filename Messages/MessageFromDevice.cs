using System.Runtime.InteropServices.WindowsRuntime;
using Sand.Messages.Serialization;
using Windows.Storage.Streams;

namespace Sand.Messages
{
    internal abstract class MessageFromDevice
    {
        public static MessageFromDevice Parse(IBuffer buffer)
        {
            var frame = Frame.Parse(buffer);
            var data = new Reader(frame.Data);

            if (frame.Ack)
            {
                return new AckFromDevice(frame.Topic, data);
            }

            return frame.Verb switch
            {
                Verb v when v == Verb.WaitForConnectionData => new WaitForConnectionData(),
                Verb v when v == Verb.AllStateDataDuringTheRun => new AllStateDataDuringTheRun(data),
                _ => new UnknownMessage(frame.Topic, frame.Verb, data)
            };
        }
    }

    internal class UnknownMessage : MessageFromDevice
    {
        private readonly Topic _topic;
        private readonly Verb _verb;
        private readonly IBuffer _data;

        public UnknownMessage(Topic topic, Verb verb, Reader data)
        {
            _topic = topic;
            _verb = verb;
            _data = data.DetachBuffer();
        }

        public override string ToString()
        {
            return $"{_topic.Name} {_verb.Name} {string.Join(" ", _data.ToArray())}";
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
            _brightness = reader.ReadByte();
            _ = reader.ReadString();
            _color = reader.ReadColor();
            _speed = reader.ReadByte();
            _playlistId = reader.ReadString();
            _patternId = reader.ReadUShort();
            _patternProgress = reader.ReadUShort();
            _status = reader.ReadByte();
            _randomMode = reader.ReadBool();
            _loopMode = reader.ReadBool();
            _sleepEnabled = reader.ReadBool();
            _breathMode = reader.ReadBool();
            _shiftingMode = reader.ReadBool();
            _shiftTime = reader.ReadUShort();
            _ = reader.ReadString();
            _ = reader.ReadBool();
            _ = reader.ReadByte();
            _ = reader.ReadString();
            _ = reader.ReadBool();
            _ = reader.ReadByte();
            _ = reader.ReadBool();
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
}
