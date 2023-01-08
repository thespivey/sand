package com.sand.table.Protocol.Serialization;

import com.sand.table.Model.Point;

import java.io.ByteArrayOutputStream;

public class Writer {
    private final ByteArrayOutputStream _data = new ByteArrayOutputStream();

    public byte[] getData() {
        return _data.toByteArray();
    }

    public void writeByte(int value) {
        _data.write(value & 0xff);
    }

    public void writeBoolean(boolean value) {
        writeByte((byte)(value ? 1 : 0));
    }

    public void writeShort(int value) {
        int high = (value >> 8) & 0xff;
        int low = value & 0xff;
        writeByte(low);
        writeByte(high);
    }

    public void writeInt(int value) {
        int high = value >> 16 & 0xffff;
        int low = value & 0xffff;
        writeShort(low);
        writeShort(high);
    }

    public void writePoint(Point point) {
        int angle = (int)(point.getAngle() * (1000.0f / 6.0f)); // Angle is 0-360, map to 0 - 60000
        int radius = (int)(point.getRadius() * 50000);    // Radius is 0-1, map to 0 - 50000
        writeShort(angle & 0xffff);
        writeShort(radius & 0xffff);
    }
}
