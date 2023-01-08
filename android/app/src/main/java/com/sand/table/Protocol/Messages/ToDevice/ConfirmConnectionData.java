package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public class ConfirmConnectionData extends MessageToDevice {
    public ConfirmConnectionData() {
        super(Topic.AskNoConnectionData, Verb.ConfirmConnectionData);
    }

    @Override
    protected void write(Writer writer) {
        writer.writeBoolean(true);
    }
}
