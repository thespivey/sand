using Windows.Storage.Streams;

namespace Sand.Messages.Serialization;

internal class Writer
{
    private DataWriter _writer = new();

    public Writer()
    {
        _writer.ByteOrder = ByteOrder.LittleEndian;
    }

    public void Write(bool value) => _writer.WriteBoolean(value);

    public void Write(byte value) => _writer.WriteByte(value);

    public void Write(ushort value) => _writer.WriteUInt16(value);

    public void Write(uint value) => _writer.WriteUInt32(value);

    public void Write(Color value)
    {
        _writer.WriteByte(value.r);
        _writer.WriteByte(value.g);
        _writer.WriteByte(value.b);
    }

    public void Write(Point point)
    {
        _writer.WriteUInt16((ushort)(point.Angle * 1000 / 6));  // Scaled from 0-360 to 0-60000
        _writer.WriteUInt16((ushort)(point.Radius * 100000 / 2)); // Scaled from 0-1 to 0-50000
    }

    public IBuffer DetachBuffer() => _writer.DetachBuffer();
}
