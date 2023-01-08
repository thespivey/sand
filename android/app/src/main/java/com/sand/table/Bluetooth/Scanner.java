package com.sand.table.Bluetooth;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanFilter;
import android.bluetooth.le.ScanResult;
import android.bluetooth.le.ScanSettings;
import android.os.Handler;
import android.os.Looper;
import android.os.ParcelUuid;
import android.util.Log;

import java.util.Arrays;
import java.util.List;

public class Scanner {
    private static final String TAG = "Scanner";

    private final Handler _handler = new Handler(Looper.myLooper());
    private final Callback _callback = new Callback();
    private Listener _listener;
    private BluetoothLeScanner _scanner;

    public interface Listener {
        void onDeviceFound(String address);
    }

    public Scanner(Listener listener) throws SecurityException {
        _listener = listener;

        BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
        _scanner = adapter.getBluetoothLeScanner();

        Log.d(TAG, "Starting scan");
        List<ScanFilter> filters = Arrays.asList();
        ScanSettings settings = new ScanSettings.Builder()
                .setCallbackType(ScanSettings.CALLBACK_TYPE_ALL_MATCHES)
                .setScanMode(ScanSettings.SCAN_MODE_LOW_LATENCY)
                .build();
        _scanner.startScan(filters, settings, _callback);
    }

    public void dispose() {
        _listener = null;
        if (_scanner != null) {
            try {
                _scanner.stopScan(_callback);
            } catch (SecurityException e) {
                e.printStackTrace();
            }
            _scanner = null;
        }
    }

    private class Callback extends ScanCallback {
        @Override
        public void onScanResult(int callbackType, ScanResult result) {
            _handler.post(() -> {
                try {
                    BluetoothDevice device = result.getDevice();
                    String name = device.getName();
                    String address = device.getAddress();
                    Log.i(TAG, String.format("device: %s %s", name, address));

                    if (name != null && name.startsWith("KS")) {
                        if (_listener != null) {
                            _listener.onDeviceFound(result.getDevice().getAddress());
                        }
                    }
                } catch (SecurityException e) {
                    e.printStackTrace();
                }
            });
        }
    }
}
