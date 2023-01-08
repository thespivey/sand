package com.sand.table.Model;

public class Point {

    private final float _angle;  // distance, 0 (center) - 1 (edge)
    private final float _radius; // degrees, 0 (north) - 360

    public Point(float angle, float radius) {
        _angle = angle;
        _radius = radius;
    }

    public float getAngle() {
        return _angle;
    }

    public float getRadius() {
        return _radius;
    }
}
