using System.Collections.Generic;

namespace Sand.Protocol.Serialization;

internal class Topic
{
    private static readonly Dictionary<byte, Topic> _topics = new();

    public static readonly Topic AskConnection = new(79, "AskConnection", Verb.AllCompletesData);
    public static readonly Topic AskNoConnectionData = new(80, "AskNoConnectionData", Verb.WaitForConnectionData, Verb.ConfirmConnectionData);
    public static readonly Topic AskReceptionStatus = new(78, "AskReceptionStatus", Verb.DataReceptionIsComplete, Verb.DataReceptionFailed);
    public static readonly Topic ReceiveFavoriteListData = new(54, "ReceiveFavoriteListData", Verb.StartSendingListOfFavorites, Verb.SendingListOfFavoritesIsComplete, Verb.FavoriteListData);
    public static readonly Topic ReceiveFileTransferAndStatus = new(52, "ReceiveFileTransferAndStatus", Verb.FileIsMissing, Verb.FileStartsToReceive, Verb.FileReceiptCompleted, Verb.FileWasDeletedSuccessfully, Verb.FileDownloadFailed, Verb.ThereIsNotEnoughStorageSpace);
    public static readonly Topic ReceiveIapUpdateInstructions = new(56, "ReceiveIapUpdateInstructions", Verb.RequestASliceAppProgramToUpdatePacket, Verb.CompleteReceiptOfASliceAppProgram, Verb.AChipAppProgramFailedToDownload, Verb.RequestBSliceAppProgramToUpdatePacket, Verb.CompleteReceiptOfBSliceAppProgram, Verb.BSliceAppProgramFailedToDownload);
    public static readonly Topic ReceiveLightingAndColor = new(49, "ReceiveLightingAndColor", Verb.StartSendingLightColorData, Verb.EndSendingLightColorData, Verb.LightColorData, Verb.SendLightSettings);
    public static readonly Topic ReceiveListInfo = new(48, "ReceiveListInfo", Verb.StartSendingListData, Verb.ListDataIsSentAtEnd, Verb.ListData, Verb.FavoriteData);
    public static readonly Topic ReceivePicture = new(51, "ReceivePicture", Verb.StartSendingPictureMessages, Verb.PictureMessageIsSentAtEnd, Verb.PictureInformation);
    public static readonly Topic ReceiveSandPaintingRunData = new(53, "ReceiveSandPaintingRunData", Verb.AllStateDataDuringTheRun);
    public static readonly Topic ReceiveSleepAndTime = new(50, "ReceiveSleepAndTime", Verb.GetSleepTimeAndHibernationStatus, Verb.GetCurrentTimeFromApp);
    public static readonly Topic ReceiveSystemAlarm = new(55, "ReceiveSystemAlarm", Verb.FatalError, Verb.ErrorsCanBeRecovered, Verb.Warning);
    public static readonly Topic SendsBigFileDataIsSent = new(83, "SendsBigFileDataIsSent", Verb.ContentsOfFileSent);
    public static readonly Topic SendsData = new(16, "SendsData", Verb.AddList, Verb.DeleteList, Verb.ModifyList, Verb.AddFilesToList, Verb.RemoveFileFromList, Verb.AddFilesToFavorite, Verb.DeleteFilesFromFavorites, Verb.Playlist);
    public static readonly Topic SendsHibernateSettings = new(18, "SendsHibernateSettings", Verb.SetSleepTimeAndHibernationStatus, Verb.SendTheCurrentTime);
    public static readonly Topic SendsIapUpgradeActionDirective = new(21, "SendsIapUpgradeActionDirective", Verb.StartDownloadingASliceAppFile, Verb.ASliceAppFileData, Verb.StartDownloadingBSliceAppFile, Verb.BSliceAppFileData, Verb.StartAnAppProgramUpdate);
    public static readonly Topic SendsLargeFileOperations = new(19, "SendsLargeFileOperations", Verb.StartDownloadingFile, Verb.EndDownloadingFile, Verb.DeleteFile);
    public static readonly Topic SendsLightingAndColor = new(17, "SendsLightingAndColor", Verb.StartModifyingColorData, Verb.EndModifyingColorData, Verb.ColorData, Verb.UpdateOtherLightControlStates, Verb.RequestColors);
    public static readonly Topic SendsUserActionsInstructions = new(20, "SendsUserActionsInstructions", Verb.SinglePlotDrawingOperation, Verb.DrawListPicture, Verb.InitiateOrPauseDrawing, Verb.NextPicture, Verb.PreviousPicture, Verb.SetDrawingSpeed, Verb.SetBrightness, Verb.ColorPreview, Verb.SleepModeTotalSwitchSettings, Verb.RandomModeSwitchSettings, Verb.CycleModeSwitchSettings, Verb.RestoreFactorySettings, Verb.ColorSettings, Verb.OnOrOffAromatherapy);

    public static Topic LookupTopic(byte value)
    {
        if (_topics.TryGetValue(value, out var result))
        {
            return result;
        }
        return new Topic(value, $"INVALID ({value})");
    }

    private readonly Dictionary<byte, Verb> _verbs = new();

    private Topic(byte value, string name, params Verb[] verbs)
    {
        Value = value;
        Name = name;
        foreach (var verb in verbs)
        {
            _verbs.Add(verb.Value, verb);
        }
        _topics.Add(value, this);
    }

    public byte Value { get; }
    public string Name { get; }

    public Verb LookupVerb(byte value)
    {
        if (_verbs.TryGetValue(value, out var result))
        {
            return result;
        }
        return new Verb(value, $"INVALID ({value})");
    }
}
