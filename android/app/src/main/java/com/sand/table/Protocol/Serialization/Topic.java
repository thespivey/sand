package com.sand.table.Protocol.Serialization;

import java.util.HashMap;
import java.util.Map;

public class Topic {
    private static final Map<Byte, Topic> _topics = new HashMap<>();

    public static final Topic AskConnection = new Topic(79, "AskConnection", Verb.AllCompletesData);
    public static final Topic AskNoConnectionData = new Topic(80, "AskNoConnectionData", Verb.WaitForConnectionData, Verb.ConfirmConnectionData);
    public static final Topic AskReceptionStatus = new Topic(78, "AskReceptionStatus", Verb.DataReceptionIsComplete, Verb.DataReceptionFailed);
    public static final Topic ReceiveFavoriteListData = new Topic(54, "ReceiveFavoriteListData", Verb.StartSendingListOfFavorites, Verb.SendingListOfFavoritesIsComplete, Verb.FavoriteListData);
    public static final Topic ReceiveFileTransferAndStatus = new Topic(52, "ReceiveFileTransferAndStatus", Verb.FileIsMissing, Verb.FileStartsToReceive, Verb.FileReceiptCompleted, Verb.FileWasDeletedSuccessfully, Verb.FileDownloadFailed, Verb.ThereIsNotEnoughStorageSpace);
    public static final Topic ReceiveIapUpdateInstructions = new Topic(56, "ReceiveIapUpdateInstructions", Verb.RequestASliceAppProgramToUpdatePacket, Verb.CompleteReceiptOfASliceAppProgram, Verb.AChipAppProgramFailedToDownload, Verb.RequestBSliceAppProgramToUpdatePacket, Verb.CompleteReceiptOfBSliceAppProgram, Verb.BSliceAppProgramFailedToDownload);
    public static final Topic ReceiveLightingAndColor = new Topic(49, "ReceiveLightingAndColor", Verb.StartSendingLightColorData, Verb.EndSendingLightColorData, Verb.LightColorData, Verb.SendLightSettings);
    public static final Topic ReceiveListInfo = new Topic(48, "ReceiveListInfo", Verb.StartSendingListData, Verb.ListDataIsSentAtEnd, Verb.ListData, Verb.FavoriteData);
    public static final Topic ReceivePicture = new Topic(51, "ReceivePicture", Verb.StartSendingPictureMessages, Verb.PictureMessageIsSentAtEnd, Verb.PictureInformation);
    public static final Topic ReceiveSandPaintingRunData = new Topic(53, "ReceiveSandPaintingRunData", Verb.AllStateDataDuringTheRun);
    public static final Topic ReceiveSleepAndTime = new Topic(50, "ReceiveSleepAndTime", Verb.GetSleepTimeAndHibernationStatus, Verb.GetCurrentTimeFromApp);
    public static final Topic ReceiveSystemAlarm = new Topic(55, "ReceiveSystemAlarm", Verb.FatalError, Verb.ErrorsCanBeRecovered, Verb.Warning);
    public static final Topic SendsBigFileDataIsSent = new Topic(83, "SendsBigFileDataIsSent", Verb.ContentsOfFileSent);
    public static final Topic SendsData = new Topic(16, "SendsData", Verb.AddList, Verb.DeleteList, Verb.ModifyList, Verb.AddFilesToList, Verb.RemoveFileFromList, Verb.AddFilesToFavorite, Verb.DeleteFilesFromFavorites, Verb.Playlist);
    public static final Topic SendsHibernateSettings = new Topic(18, "SendsHibernateSettings", Verb.SetSleepTimeAndHibernationStatus, Verb.SendTheCurrentTime);
    public static final Topic SendsIapUpgradeActionDirective = new Topic(21, "SendsIapUpgradeActionDirective", Verb.StartDownloadingASliceAppFile, Verb.ASliceAppFileData, Verb.StartDownloadingBSliceAppFile, Verb.BSliceAppFileData, Verb.StartAnAppProgramUpdate);
    public static final Topic SendsLargeFileOperations = new Topic(19, "SendsLargeFileOperations", Verb.StartDownloadingFile, Verb.EndDownloadingFile, Verb.DeleteFile);
    public static final Topic SendsLightingAndColor = new Topic(17, "SendsLightingAndColor", Verb.StartModifyingColorData, Verb.EndModifyingColorData, Verb.ColorData, Verb.UpdateOtherLightControlStates, Verb.RequestColors);
    public static final Topic SendsUserActionsInstructions = new Topic(20, "SendsUserActionsInstructions", Verb.SinglePlotDrawingOperation, Verb.DrawListPicture, Verb.InitiateOrPauseDrawing, Verb.NextPicture, Verb.PreviousPicture, Verb.SetDrawingSpeed, Verb.SetBrightness, Verb.ColorPreview, Verb.SleepModeTotalSwitchSettings, Verb.RandomModeSwitchSettings, Verb.CycleModeSwitchSettings, Verb.RestoreFactorySettings, Verb.ColorSettings, Verb.OnOrOffAromatherapy);

    public static Topic lookupTopic(byte value) {
        Topic result = _topics.get(value);
        if (result == null) {
            result = new Topic(value, String.format("INVALID (%d)", value));
        }
        return result;
    }

    private final Map<Byte, Verb> _verbs = new HashMap<>();
    private final Byte _value;
    private final String _name;

    private Topic(int value, String name, Verb... verbs) {
        _value = (byte)value;
        _name = name;
        for (Verb verb : verbs) {
            _verbs.put(verb.getValue(), verb);
        }
        _topics.put((byte)value, this);
    }

    public byte getValue() {
        return _value;
    }

    public String getName() {
        return _name;
    }

    public Verb lookupVerb(byte value) {
        Verb result = _verbs.get(value);
        if (result == null) {
            result = new Verb(value, String.format("INVALID (%d)", value));
        }
        return result;
    }
}