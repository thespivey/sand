package com.sand.table.Protocol.Messages.ToDevice;

import com.sand.table.Protocol.Serialization.Topic;
import com.sand.table.Protocol.Serialization.Verb;

public class InitiateOrPauseDrawing extends MessageToDevice {
    public InitiateOrPauseDrawing() {
        super(Topic.SendsUserActionsInstructions, Verb.InitiateOrPauseDrawing);
    }

}
