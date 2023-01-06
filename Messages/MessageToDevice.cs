using Sand.Messages.Serialization;
using Windows.Storage.Streams;

namespace Sand.Messages
{
    internal abstract class MessageToDevice
    {
        private readonly Topic _topic;
        private readonly Verb? _verb;
        private readonly bool _ack;

        public MessageToDevice(Topic topic, Verb? verb, bool ack = false)
        {
            _topic = topic;
            _verb = verb;
            _ack = ack;
        }

        protected virtual void Write(Writer data) { }

        public IBuffer Serialize()
        {
            Writer data = new();
            Write(data);

            Frame messageFrame = new(_topic, _verb, _ack, data.DetachBuffer());
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
            base.Write(data);
            data.WriteBool(true);
        }

    }

    internal class AckToDevice : MessageToDevice
    {
        // TODO: Is this right?  Do we pass an verb with the ack?  Do we ack blindly?
        public AckToDevice()
            : base(Topic.AskNoConnectionData, Verb.ConfirmConnectionData, ack: true)
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
            base.Write(data);
            data.WriteBool(_shift);
            data.WriteUShort(_shiftTime);
            data.WriteBool(_breath);
            data.WriteByte(_brightness);
        }
    }
}
