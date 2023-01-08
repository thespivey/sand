package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public class UpdateOtherLightControlStates extends MessageToDevice {
    private final boolean _colorShift;
    private final short _colorShiftInterval;
    private final boolean _breath;
    private final byte _brightness;

    public UpdateOtherLightControlStates(boolean colorShift, short colorShiftInterval, boolean breath, byte brightness) {
        super(Topic.SendsLightingAndColor, Verb.UpdateOtherLightControlStates);
        _colorShift = colorShift;
        _colorShiftInterval = colorShiftInterval;
        _breath = breath;
        _brightness = brightness;
    }

    @Override
    protected void write(Writer writer) {
        writer.writeBoolean(_colorShift);
        writer.writeShort(_colorShiftInterval);
        writer.writeBoolean(_breath);
        writer.writeByte(_brightness);
    }
}
