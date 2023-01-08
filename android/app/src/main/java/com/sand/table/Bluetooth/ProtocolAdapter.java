package com.sand.table.Bluetooth;

import com.sand.table.Protocol.Messages.FromDevice.MessageFromDevice;
import com.sand.table.Protocol.Messages.ToDevice.MessageToDevice;

import java.io.IOException;

public class ProtocolAdapter {
    private final Device _device;

    public interface Listener {
        void onMessage(MessageFromDevice message);
    }

    public ProtocolAdapter(Device device, Listener listener) {
        _device = device;
        device.setListener(new DeviceListener(listener));
    }

    public void sendMessage(MessageToDevice message) {
        _device.write(message.serialize());
    }

    private class DeviceListener implements Device.Listener {
        private final Listener _listener;
        private byte[] _pendingData;

        public DeviceListener(Listener listener) {
            _listener = listener;
        }

        @Override
        public void onIndication(byte[] data) {
            if (_pendingData != null) {
                byte[] newData = new byte[_pendingData.length + data.length];
                System.arraycopy(_pendingData, 0, newData, 0, _pendingData.length);
                System.arraycopy(data, 0, newData, _pendingData.length, data.length);
                _pendingData = null;
                data = newData;
            }

            MessageFromDevice result;
            try {
                result = MessageFromDevice.parse(data);
            } catch (IOException e) {
                e.printStackTrace();
                return;
            }

            if (result == null) {
                _pendingData = data;
            } else {
                _listener.onMessage(result);
            }
        }
    }
}
