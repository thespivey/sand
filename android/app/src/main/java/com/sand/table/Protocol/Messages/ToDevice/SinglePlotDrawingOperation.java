package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public class SinglePlotDrawingOperation extends MessageToDevice {
    private short _pattern;

    public SinglePlotDrawingOperation(short pattern) {
        super(Topic.SendsUserActionsInstructions, Verb.SinglePlotDrawingOperation);
        _pattern = pattern;
    }

    @Override
    protected void write(Writer writer) {
        writer.writeShort(_pattern);
    }
}
