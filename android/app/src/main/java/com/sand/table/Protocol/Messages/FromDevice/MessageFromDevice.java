package com.sand.table.Protocol.Messages.FromDevice;

import com.sand.table.Protocol.Serialization.Frame;
import com.sand.table.Protocol.Serialization.Reader;
import com.sand.table.Protocol.Serialization.Verb;

import java.io.IOException;

public abstract class MessageFromDevice {
    public static MessageFromDevice parse(byte[] buffer) throws IOException {
        Frame frame = Frame.parse(buffer);
        if (frame == null) {
            return null;
        }

        Reader reader = new Reader(frame.getData());

        if (Verb.Ack.equals(frame.getVerb())) {
            return new AckFromDevice(frame.getTopic(), reader);
        } else if (Verb.WaitForConnectionData.equals(frame.getVerb())) {
            return new WaitForConnectionData();
        } else if (Verb.AllStateDataDuringTheRun.equals(frame.getVerb())) {
            return new AllStateDataDuringTheRun(reader);
        } else {
            return new UnknownMessage(frame.getTopic(), frame.getVerb(), reader);
        }
    }

    @Override
    public String toString() {
        return getClass().getSimpleName();
    }
}

