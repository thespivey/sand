package com.sand.table.Protocol.Messages.FromDevice;

import com.sand.table.Protocol.Serialization.Reader;
import com.sand.table.Protocol.Serialization.Topic;

import java.util.Arrays;

public class AckFromDevice extends MessageFromDevice {
    private final Topic _topic;
    private final byte[] _data;

    public AckFromDevice(Topic topic, Reader reader) {
        _topic = topic;
        _data = reader.readBytes();
    }

    @Override
    public String toString() {
        return String.format("Ack %s %s", _topic.getName(), Arrays.toString(_data));
    }
}
