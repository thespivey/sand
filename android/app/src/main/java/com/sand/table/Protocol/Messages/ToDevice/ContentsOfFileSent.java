package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Model.Point;
import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

import java.util.List;

public class ContentsOfFileSent extends MessageToDevice {
    private final short _page;
    private final Point[] _points;

    public ContentsOfFileSent(short page, Point[] points) {
        super(Topic.SendsBigFileDataIsSent, Verb.ContentsOfFileSent);
        _page = page;
        _points = points;
    }

    @Override
    protected void write(Writer writer) {
        writer.writeByte(_points.length);
        writer.writeShort(_page);
        for (Point point : _points) {
            writer.writePoint(point);
        }
    }
}
