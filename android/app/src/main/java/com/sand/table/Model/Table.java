package com.sand.table.Model;

import android.content.Context;
import android.util.Log;

import com.sand.table.Bluetooth.Device;
import com.sand.table.Bluetooth.ProtocolAdapter;
import com.sand.table.Protocol.Frame;
import com.sand.table.Protocol.Topic;
import com.sand.table.Protocol.Verb;

public final class Table {
    private static final String TAG = "Table";
    private final ProtocolAdapter _protocol;

    public Table(Device device) {
        _protocol = new ProtocolAdapter(device, new ProtocolListener());
        device.connect();
    }

    private final class ProtocolListener implements ProtocolAdapter.Listener {
        @Override
        public void onMessage(Frame frame) {
            Log.d(TAG, String.format("received %s %s", frame.getTopic().getName(), frame.getVerb().getName()));

            Frame reply;
            if (frame.getTopic() == Topic.AskNoConnectionData && frame.getVerb() == Verb.WaitForConnectionData) {
                reply = new Frame(Topic.AskNoConnectionData, Verb.ConfirmConnectionData, new byte[]{1});
            } else {
                reply = new Frame(Topic.AskNoConnectionData, Verb.Ack, new byte[0]);
            }

            Log.d(TAG, String.format("sending %s %s", reply.getTopic().getName(), reply.getVerb().getName()));
            _protocol.sendMessage(reply);
        }
    }
}
