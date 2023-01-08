package com.sand.table;

import android.Manifest;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;

import com.sand.table.Bluetooth.Device;
import com.sand.table.Bluetooth.Scanner;
import com.sand.table.Model.Table;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "MainActivity";

    private Scanner _scanner;
    private Table _table;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_main);
        bindControls();

        ActivityResultLauncher<String[]> requestPermissionLauncher = registerForActivityResult(new ActivityResultContracts.RequestMultiplePermissions(), granted -> start());
        requestPermissionLauncher.launch(new String[]{
                Manifest.permission.ACCESS_FINE_LOCATION,
                Manifest.permission.ACCESS_COARSE_LOCATION,
                Manifest.permission.BLUETOOTH,
                Manifest.permission.BLUETOOTH_ADMIN,
                Manifest.permission.BLUETOOTH_CONNECT,
                Manifest.permission.BLUETOOTH_SCAN
        });
    }

    private void start() {
        _scanner = new Scanner(address -> {
            connect(address);
            _scanner.dispose();
            _scanner = null;
        });
    }

    private void connect(String address) {
        ((TextView)findViewById(R.id.status)).setText(address);
        Device device = new Device(this, address);
        _table = new Table(device);
    }

    private void bindControls() {
        ((Button)findViewById(R.id.light_off)).setOnClickListener(v -> _table.setBrightness(0));
        ((Button)findViewById(R.id.light_on)).setOnClickListener(v -> _table.setBrightness(1));
        ((Button)findViewById(R.id.pattern_start)).setOnClickListener(v -> _table.startPattern());
        ((Button)findViewById(R.id.pattern_data)).setOnClickListener(v -> _table.sendPatternData());
        ((Button)findViewById(R.id.pattern_end)).setOnClickListener(v -> _table.endPattern());
    }
}
