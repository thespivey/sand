using Windows.Storage.Streams;

namespace Sand.Messages.Serialization
{
    internal class Writer
    {
        private DataWriter _writer = new();

        public Writer()
        {
            _writer.ByteOrder = ByteOrder.LittleEndian;
        }

        public void WriteBool(bool value) => _writer.WriteBoolean(value);

        public void WriteByte(byte value) => _writer.WriteByte(value);

        public void WriteUShort(ushort value) => _writer.WriteUInt16(value);

        public void WriteColor(Color value)
        {
            _writer.WriteByte(value.r);
            _writer.WriteByte(value.g);
            _writer.WriteByte(value.b);
        }

        public void WriteString(String value) => throw new NotImplementedException();

        public IBuffer DetachBuffer() => _writer.DetachBuffer();
    }
}
