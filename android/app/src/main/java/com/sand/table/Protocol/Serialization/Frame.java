package com.sand.table.Protocol.Serialization;

import android.util.Log;

import java.io.IOException;
import java.util.Arrays;

public class Frame {
    private static final String TAG = "Frame";
    private static final byte Magic = (byte)123;
    private static final byte AckBit = (byte)0x80;
    private static final byte TopicMask = (byte)0x7F;

    private final Topic _topic;
    private final Verb _verb;
    private final byte[] _data;

    public Frame(Topic topic, Verb verb, byte[] data) {
        _topic = topic;
        _verb = verb;
        _data = data;
    }

    public Topic getTopic() { return _topic; }
    public Verb getVerb() { return _verb; }
    public byte[] getData() { return _data; }

    public static Frame parse(byte[] buffer) throws IOException {
        int offset = 0;
        byte magic = buffer[offset++];
        if (magic != Magic) {
            throw new IOException(String.format("Invalid message: magic value = %d", magic));
        }

        int length = buffer[offset++] & 0xff;
        if (length > buffer.length) {
            return null;
        }

        byte checksum = computeChecksum(buffer);
        byte expectedChecksum = buffer[buffer.length - 1];
        if (checksum != expectedChecksum) {
            Log.d("Device", String.format("BAD CHECKSUM: expected length %d, actual length %d", length, buffer.length));
            throw new IOException(String.format("Invalid checksum. Actual=%d Expected=%d", checksum & 0xff, expectedChecksum & 0xff));
        }

        byte ackAndTopic = buffer[offset++];
        boolean ack = (ackAndTopic & AckBit) == AckBit;
        Topic topic = Topic.lookupTopic((byte) (ackAndTopic & TopicMask));
        Verb verb = ack ? Verb.Ack : topic.lookupVerb(buffer[offset++]);

        byte[] data = Arrays.copyOfRange(buffer, offset, buffer.length - 1);

        return new Frame(topic, verb, data);
    }

    public byte[] serialize() {
        int messageSize = _data.length + 4; /* magic, length, topic, checksum */
        if (_verb != Verb.Ack) {
            messageSize++; /* verb */
        }

        // Write the message
        byte[] message = new byte[messageSize];
        int offset = 0;
        message[offset++] = Magic;
        message[offset++] = (byte)messageSize;
        if (_verb == Verb.Ack) {
            message[offset++] = (byte)((_topic.getValue() & 0xff) | (AckBit & 0xff));
        } else {
            message[offset++] = _topic.getValue();
            message[offset++] = _verb.getValue();
        }

        System.arraycopy(_data, 0, message, offset, _data.length);
        offset += _data.length;

        // Calculate the message checksum
        message[offset++] = computeChecksum(message);

        return message;
    }

    private static byte computeChecksum(byte[] message) {
        int checksum = 0;
        for (int i = 0; i < message.length - 1; i++) {
            checksum = checksum + (message[i] & 0xff);
        }
        return (byte)(checksum & 0xff);
    }
}
