package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Frame;
import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public abstract class MessageToDevice {
    private final Topic _topic;
    private final Verb _verb;

    public MessageToDevice(Topic topic, Verb verb) {
        _topic = topic;
        _verb = verb;
    }

    public byte[] serialize() {
        Writer writer = new Writer();
        write(writer);

        Frame frame = new Frame(_topic, _verb, writer.getData());
        return frame.serialize();
    }

    @Override
    public String toString() {
        return getClass().getSimpleName();
    }

    protected void write(Writer writer) {
    }
}

