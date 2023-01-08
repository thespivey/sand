using Sand.Model;
using Windows.Storage.Streams;

namespace Sand.Protocol.Serialization;

internal class Reader
{
    private DataReader _reader;

    public Reader(IBuffer data)
    {
        _reader = DataReader.FromBuffer(data);
        _reader.ByteOrder = ByteOrder.LittleEndian;
    }

    public void Read(out bool value) => value = _reader.ReadBoolean();

    public void Read(out byte value) => value = _reader.ReadByte();

    public void Read(out ushort value) => value = _reader.ReadUInt16();

    public void Read(out Color value)
    {
        value.r = _reader.ReadByte();
        value.g = _reader.ReadByte();
        value.b = _reader.ReadByte();
    }

    public void Read(out string value)
    {
        value = "";
        while (true)
        {
            var b = _reader.ReadByte();
            if (b == 0)
            {
                return;
            }

            value += (char)b;
        }
    }

    public IBuffer DetachBuffer() => _reader.DetachBuffer();
}
