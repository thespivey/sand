package com.sand.table.Model;

import android.util.Log;

import com.sand.table.Bluetooth.Device;
import com.sand.table.Bluetooth.ProtocolAdapter;
import com.sand.table.Protocol.Messages.FromDevice.AckFromDevice;
import com.sand.table.Protocol.Messages.FromDevice.MessageFromDevice;
import com.sand.table.Protocol.Messages.FromDevice.UnknownMessage;
import com.sand.table.Protocol.Messages.FromDevice.WaitForConnectionData;
import com.sand.table.Protocol.Messages.ToDevice.AckToDevice;
import com.sand.table.Protocol.Messages.ToDevice.ConfirmConnectionData;
import com.sand.table.Protocol.Messages.ToDevice.ContentsOfFileSent;
import com.sand.table.Protocol.Messages.ToDevice.EndDownloadingFile;
import com.sand.table.Protocol.Messages.ToDevice.MessageToDevice;
import com.sand.table.Protocol.Messages.ToDevice.StartDownloadingFile;
import com.sand.table.Protocol.Messages.ToDevice.UpdateOtherLightControlStates;
import com.sand.table.Protocol.Serialization.Frame;
import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;

import java.util.ArrayList;
import java.util.List;

public final class Table {
    private static final String TAG = "Table";
    private final ProtocolAdapter _protocol;

    private boolean _colorShift = true;
    private short _colorShiftInterval = 5;
    private boolean _breath = true;
    private float _brightness = 1;

    public Table(Device device) {
        _protocol = new ProtocolAdapter(device, new ProtocolListener());
        device.connect();
    }

    public void setBrightness(float value) {
        _brightness = value;
        // TODO: I guess I should have some kind of Light model that does this.  Table.getLight().setBrightness(1)...
        sendMessage(new UpdateOtherLightControlStates(_colorShift, _colorShiftInterval, _breath, (byte)Math.round(_brightness * 100)));
    }

    private static final short _pattern = 511; // TODO: Support a list of patterns. Download from some web service?
    private static final Point[] _points  = new Point[] {
        new Point(0, 0),
        new Point(0, 1),
        new Point(0, 0),
        new Point(90, 0),
        new Point(90, 1),
        new Point(90, 0),
        new Point(180, 0),
        new Point(180, 1),
        new Point(180, 0),
        new Point(270, 0),
        new Point(270, 1),
        new Point(270, 0)
    };
    public void startPattern() {
        // TODO: this is not a model, just for testing.  Need to be able to set up some kind of flow (wait for ack, wait for response, notice errors) for multi-command sequencing.
        sendMessage(new StartDownloadingFile(_pattern, (short)1));
    }

    public void endPattern() {
        sendMessage(new EndDownloadingFile(_pattern, (short)1));
    }
    public void sendPatternData() {
        sendMessage(new ContentsOfFileSent((short)1, _points));
    }

    private void sendMessage(MessageToDevice message) {
        Log.d(TAG, String.format("sending %s", message));
        _protocol.sendMessage(message);
    }

    private final class ProtocolListener implements ProtocolAdapter.Listener {
        @Override
        public void onMessage(MessageFromDevice message) {
            Log.d(TAG, String.format("received %s", message));

            MessageToDevice reply = null;
            if (message instanceof WaitForConnectionData) {
                reply = new ConfirmConnectionData();
            } else if (message instanceof AckFromDevice) {
                reply = new AckToDevice();
            } else if (message instanceof UnknownMessage) {
                UnknownMessage unk = (UnknownMessage) message;
                if (unk.getTopic() == Topic.ReceiveSystemAlarm) {
                    reply = new AckToDevice();
                }
            }

            if (reply != null) {
                if (!(reply instanceof AckToDevice)) {
                    Log.d(TAG, String.format("sending %s", reply));
                }
                _protocol.sendMessage(reply);
            }
        }
    }
}
