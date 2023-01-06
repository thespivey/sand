using Windows.Storage.Streams;

namespace Sand.Messages.Serialization
{
    internal class Reader
    {
        private DataReader _reader;

        public Reader(IBuffer data)
        {
            _reader = DataReader.FromBuffer(data);
            _reader.ByteOrder = ByteOrder.LittleEndian;
        }

        public bool ReadBool() => _reader.ReadBoolean();

        public byte ReadByte() => _reader.ReadByte();

        public ushort ReadUShort() => _reader.ReadUInt16();

        public Color ReadColor()
        {
            var r = _reader.ReadByte();
            var g = _reader.ReadByte();
            var b = _reader.ReadByte();
            return new Color { r = r, g = g, b = b };
        }

        public string ReadString()
        {
            string s = "";
            while (true)
            {
                var b = _reader.ReadByte();
                if (b == 0)
                {
                    return s;
                }

                s += (char)b;
            }
        }

        public IBuffer DetachBuffer() => _reader.DetachBuffer();
    }
}
