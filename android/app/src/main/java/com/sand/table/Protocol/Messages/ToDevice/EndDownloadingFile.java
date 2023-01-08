package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;
import com.sand.table.Protocol.Serialization.Writer;

public class EndDownloadingFile extends MessageToDevice {
    private final short _pattern;
    private final int _pages;

    public EndDownloadingFile(short pattern, int pages) {
        super(Topic.SendsLargeFileOperations, Verb.EndDownloadingFile);

        _pattern = pattern;
        _pages = pages;
    }

    @Override
    protected void write(Writer writer) {
        writer.writeShort(_pattern);
        writer.writeInt(_pages);
    }
}
