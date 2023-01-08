package com.sand.table.Protocol.Messages.FromDevice;

import com.sand.table.Protocol.Serialization.Reader;
import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;

import java.util.Arrays;

public class UnknownMessage extends MessageFromDevice {
    private final Topic _topic;
    private final Verb _verb;
    private final byte[] _data;

    public UnknownMessage(Topic topic, Verb verb, Reader reader) {
        _topic = topic;
        _verb = verb;
        _data = reader.readBytes();
    }

    @Override
    public String toString() {
        return String.format("%s %s %s", _topic.getName(), _verb.getName(), Arrays.toString(_data));
    }

    public Topic getTopic() {
        return _topic;
    }
}
