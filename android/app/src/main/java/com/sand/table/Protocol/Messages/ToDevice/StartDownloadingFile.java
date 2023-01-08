package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public class StartDownloadingFile extends MessageToDevice {
    private final short _pattern;
    private final int _pages;

    public StartDownloadingFile(short pattern, int pages) {
        super(Topic.SendsLargeFileOperations, Verb.StartDownloadingFile);
        _pattern = pattern;
        _pages = pages;
    }

    @Override
    protected void write(Writer writer) {
        writer.writeShort(_pattern);
        writer.writeInt(_pages);
    }
}
