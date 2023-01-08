package com.sand.table.Protocol.Messages.FromDevice;

import android.annotation.SuppressLint;

import com.sand.table.Model.Color;
import com.sand.table.Protocol.Serialization.Reader;

public class AllStateDataDuringTheRun extends MessageFromDevice {
    private static final float MaxProgress = 10000;

    private final byte _brightness;
    private final Color _color;
    private final byte _speed;
    private final String _playlist;
    private final short _pattern;
    private final short _progress;
    private final byte _status;
    private final boolean _shuffle;
    private final boolean _loop;
    private final boolean _breath;
    private final boolean _colorShift;
    private final short _colorShiftInterval;

    public AllStateDataDuringTheRun(Reader reader) {
        _brightness = reader.readByte();
        reader.readString();
        _color = reader.readColor();
        _speed = reader.readByte();
        _playlist = reader.readString();
        _pattern = reader.readShort();
        _progress = reader.readShort();
        _status = reader.readByte();
        _shuffle = reader.readBoolean();
        _loop = reader.readBoolean();
        reader.readBoolean();
        _breath = reader.readBoolean();
        _colorShift = reader.readBoolean();
        _colorShiftInterval = reader.readShort();
        reader.readString();
        reader.readBoolean();
        reader.readByte();
        reader.readString();
        reader.readBoolean();
        reader.readByte();
        reader.readBoolean();
    }

    @SuppressLint("DefaultLocale")
    @Override
    public String toString() {
        return String.format(
                "AllStateDataDuringTheRun brightness %d%% color %s speed %d playlist %s pattern %d %.2f%% status %d %s %s %s %s",
                _brightness,
                _color,
                _speed,
                _playlist,
                _pattern,
                _progress / MaxProgress * 100,
                _status,
                _shuffle ? "random " : "",
                _loop ? "loop " : "",
                _breath ? "breath " : "",
                _colorShift ? String.format("shift %d", _colorShiftInterval) : ""
        );
    }
}
