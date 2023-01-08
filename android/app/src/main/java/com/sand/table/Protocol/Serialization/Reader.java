package com.sand.table.Protocol.Serialization;

import com.sand.table.Model.Color;

import java.util.Arrays;

public class Reader {
    private final byte[] _data;
    private int _index = 0;

    public Reader(byte[] data) {
        _data = data;
    }

    public byte readByte() {
        return _data[_index++];
    }

    public byte[] readBytes() {
        return readBytes(_data.length - _index);
    }

    public byte[] readBytes(int length) {
        byte[] result = Arrays.copyOfRange(_data, _index, length);
        _index += length;
        return result;
    }

    public Color readColor() {
        return new Color(readByte(), readByte(), readByte());
    }

    public boolean readBoolean() {
        return readByte() != 0;
    }

    public short readShort() {
        int low = readByte() & 0xff;
        int high = readByte() & 0xff;
        return (short)(high << 8 | low);
    }

    public String readString() {
        StringBuilder result = new StringBuilder();
        while (true) {
            byte b = readByte();
            if (b == 0) {
                return result.toString();
            }

            result.append((char) b);
        }
    }
}
