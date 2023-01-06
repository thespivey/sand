using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace Sand.Messages.Serialization
{
    internal class Frame
    {
        private const byte Magic = 123;
        private const byte AckBit = 0x80;
        private const byte TopicMask = 0x7F;

        public Frame(Topic topic, Verb? verb, bool ack, IBuffer data)
        {
            Topic = topic;
            Verb = verb;
            Ack = ack;
            Data = data;
        }

        public Topic Topic { get; }
        public Verb? Verb { get; }
        public bool Ack { get; }
        public IBuffer Data { get; }

        public static Frame Parse(IBuffer buffer)
        {
            var msg = DataReader.FromBuffer(buffer);

            var magic = msg.ReadByte();
            if (magic != Magic)
            {
                throw new Exception("Invalid message: magic value = {magic}");
            }

            uint len = msg.ReadByte();
            if (len != buffer.Length)
            {
                throw new Exception("Invalid message: real length = {buffer.Length} specified length = {len}");
            }

            var ackAndTopic = msg.ReadByte();
            bool ack = (ackAndTopic & AckBit) == AckBit;
            Topic topic = Topic.LookupTopic((byte)(ackAndTopic & TopicMask));

            Verb? verb = null;
            if (!ack)
            {
                verb = topic.LookupVerb(msg.ReadByte());
            }

            var data = msg.ReadBuffer(msg.UnconsumedBufferLength - 1);

            _ = msg.ReadByte(); // checksum (ignored)

            return new Frame(topic, verb, ack, data);
        }

        public IBuffer Serialize()
        {
            // Calculate the message size
            var messageSize = Data.Length + 4; /* magic, length, topic, checksum */
            if (Verb != null)
            {
                messageSize++;
            }

            // Write the message
            DataWriter messageWriter = new();
            messageWriter.WriteByte(Magic);
            messageWriter.WriteByte((byte)messageSize);
            messageWriter.WriteByte((byte)(Topic.Value | (Ack ? AckBit : 0)));
            if (Verb != null)
            {
                messageWriter.WriteByte(Verb.Value);
            }
            messageWriter.WriteBuffer(Data);

            // Calculate the message checksum
            var message = messageWriter.DetachBuffer();
            uint checksum = 0;
            foreach (byte b in message.ToArray())
            {
                checksum += b;
            }

            // Write the message with the checksum
            DataWriter checksumWriter = new();
            checksumWriter.WriteBuffer(message);
            checksumWriter.WriteByte((byte)checksum);
            return checksumWriter.DetachBuffer();
        }
    }
}
