package com.sand.table.Bluetooth;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothProfile;
import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;

import java.util.Arrays;
import java.util.UUID;

public final class Device {
    private static final String TAG = "Device";

    private static final UUID ServiceId = UUID.fromString("0000ffe0-0000-1000-8000-00805f9b34fb");
    private static final UUID ReadCharacteristicId = UUID.fromString(("0000ffe4-0000-1000-8000-00805f9b34fb"));
    private static final UUID WriteCharacteristicId = UUID.fromString("0000ffe9-0000-1000-8000-00805f9b34fb");
    private static final UUID ClientCharacteristicConfigurationDescriptorId = UUID.fromString("00002902-0000-1000-8000-00805f9b34fb");

    private final Context _context;
    private final String _address;
    private final Handler _handler = new Handler(Looper.myLooper());

    private Listener _listener;

    private BluetoothGatt _gatt;
    private BluetoothGattCharacteristic _readCharacteristic;
    private BluetoothGattCharacteristic _writeCharacteristic;

    public interface Listener {
        void onIndication(byte[] data);
    }

    public Device(Context context, String address) {
        _context = context;
        _address = address;
    }

    public void setListener(Listener listener) {
        _listener = listener;
    }

    public void connect() throws SecurityException {
        Log.d(TAG, "connecting");

        BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
        BluetoothDevice device = adapter.getRemoteDevice(_address);

        _gatt = device.connectGatt(_context, true, new GattCallback(), BluetoothDevice.TRANSPORT_LE);
    }

    public void write(byte[] message) throws SecurityException {
        Log.d(TAG, String.format("writing %s", Arrays.toString(message)));
        _writeCharacteristic.setValue(message);
        _writeCharacteristic.setWriteType(BluetoothGattCharacteristic.WRITE_TYPE_DEFAULT);
        _gatt.writeCharacteristic(_writeCharacteristic);
    }

    private class GattCallback extends BluetoothGattCallback {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            _handler.post(() -> {
                try {
                    Log.d(TAG, String.format("onConnectionStateChanged %d", newState));
                    if (newState == BluetoothProfile.STATE_CONNECTED) {
                        gatt.discoverServices();
                    }
                } catch (SecurityException e) {
                    e.printStackTrace();
                }
            });
        }

        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            _handler.post(() -> {
                try {
                    Log.d(TAG, String.format("onServicesDiscovered %d", status));

                    BluetoothGattService service = gatt.getService(ServiceId);
                    _readCharacteristic = service.getCharacteristic(ReadCharacteristicId);
                    _writeCharacteristic = service.getCharacteristic(WriteCharacteristicId);

                    // Enable indications from the read characteristic
                    gatt.setCharacteristicNotification(_readCharacteristic, true);
                    BluetoothGattDescriptor descriptor = _readCharacteristic.getDescriptor(ClientCharacteristicConfigurationDescriptorId);
                    descriptor.setValue(BluetoothGattDescriptor.ENABLE_INDICATION_VALUE);
                    gatt.writeDescriptor(descriptor);
                } catch (SecurityException e) {
                    e.printStackTrace();
                }
            });
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            if (characteristic.getUuid().equals(ReadCharacteristicId)) {
                byte[] data = characteristic.getValue();
                _handler.post(() -> {
                    Log.d(TAG, String.format("reading %s", Arrays.toString(data)));
                    _listener.onIndication(data);
                });
            }
        }

    }
}
