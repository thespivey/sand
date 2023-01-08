package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;

public class AckToDevice extends MessageToDevice {
    public AckToDevice() {
        super(Topic.AskNoConnectionData, Verb.Ack);
    }
}
