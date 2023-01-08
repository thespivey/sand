package com.sand.table.Model;

public class Color {
    private final byte _r;
    private final byte _g;
    private final byte _b;

    public Color(byte r, byte g, byte b) {
        _r = r;
        _g = g;
        _b = b;
    }

    @Override
    public String toString() {
        return String.format("(%X %X %X)", _r, _g, _b);
    }
}
