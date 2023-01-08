namespace Sand.Protocol.Serialization;

internal class Verb
{
    public static readonly Verb AddFilesToFavorite = new(6, "AddFilesToFavorite");
    public static readonly Verb AddFilesToList = new(4, "AddFilesToList");
    public static readonly Verb AddList = new(1, "AddList");
    public static readonly Verb AllCompletesData = new(1, "AllCompletesData");
    public static readonly Verb AllStateDataDuringTheRun = new(1, "AllStateDataDuringTheRun");
    public static readonly Verb AChipAppProgramFailedToDownload = new(3, "AChipAppProgramFailedToDownload");
    public static readonly Verb ASliceAppFileData = new(2, "ASliceAppFileData");
    public static readonly Verb BSliceAppFileData = new(4, "BSliceAppFileData");
    public static readonly Verb BSliceAppProgramFailedToDownload = new(6, "BSliceAppProgramFailedToDownload");
    public static readonly Verb ColorData = new(3, "ColorData");
    public static readonly Verb ColorPreview = new(8, "ColorPreview");
    public static readonly Verb ColorSettings = new(13, "ColorSettings");
    public static readonly Verb CompleteReceiptOfASliceAppProgram = new(2, "CompleteReceiptOfASliceAppProgram");
    public static readonly Verb CompleteReceiptOfBSliceAppProgram = new(5, "CompleteReceiptOfBSliceAppProgram");
    public static readonly Verb ConfirmConnectionData = new(2, "ConfirmConnectionData");
    public static readonly Verb ContentsOfFileSent = new(1, "ContentsOfFileSent");
    public static readonly Verb CycleModeSwitchSettings = new(11, "CycleModeSwitchSettings");
    public static readonly Verb DataReceptionFailed = new(2, "DataReceptionFailed");
    public static readonly Verb DataReceptionIsComplete = new(1, "DataReceptionIsComplete");
    public static readonly Verb DeleteFile = new(3, "DeleteFile");
    public static readonly Verb DeleteFilesFromFavorites = new(7, "DeleteFilesFromFavorites");
    public static readonly Verb DeleteList = new(2, "DeleteList");
    public static readonly Verb DrawListPicture = new(2, "DrawListPicture");
    public static readonly Verb EndDownloadingFile = new(2, "EndDownloadingFile");
    public static readonly Verb EndModifyingColorData = new(2, "EndModifyingColorData");
    public static readonly Verb EndSendingLightColorData = new(2, "EndSendingLightColorData");
    public static readonly Verb ErrorsCanBeRecovered = new(2, "ErrorsCanBeRecovered");
    public static readonly Verb FatalError = new(1, "FatalError");
    public static readonly Verb FavoriteData = new(4, "FavoriteData");
    public static readonly Verb FavoriteListData = new(3, "FavoriteListData");
    public static readonly Verb FileDownloadFailed = new(5, "FileDownloadFailed");
    public static readonly Verb FileIsMissing = new(1, "FileIsMissing");
    public static readonly Verb FileReceiptCompleted = new(3, "FileReceiptCompleted");
    public static readonly Verb FileStartsToReceive = new(2, "FileStartsToReceive");
    public static readonly Verb FileWasDeletedSuccessfully = new(4, "FileWasDeletedSuccessfully");
    public static readonly Verb GetCurrentTimeFromApp = new(2, "GetCurrentTimeFromApp");
    public static readonly Verb GetSleepTimeAndHibernationStatus = new(1, "GetSleepTimeAndHibernationStatus");
    public static readonly Verb InitiateOrPauseDrawing = new(3, "InitiateOrPauseDrawing");
    public static readonly Verb LightColorData = new(3, "LightColorData");
    public static readonly Verb ListData = new(3, "ListData");
    public static readonly Verb ListDataIsSentAtEnd = new(2, "ListDataIsSentAtEnd");
    public static readonly Verb ModifyList = new(3, "ModifyList");
    public static readonly Verb NextPicture = new(4, "NextPicture");
    public static readonly Verb OnOrOffAromatherapy = new(14, "OnOrOffAromatherapy");
    public static readonly Verb PictureInformation = new(3, "PictureInformation");
    public static readonly Verb PictureMessageIsSentAtEnd = new(2, "PictureMessageIsSentAtEnd");
    public static readonly Verb Playlist = new(8, "Playlist");
    public static readonly Verb PreviousPicture = new(5, "PreviousPicture");
    public static readonly Verb RandomModeSwitchSettings = new(10, "RandomModeSwitchSettings");
    public static readonly Verb RemoveFileFromList = new(5, "RemoveFileFromList");
    public static readonly Verb RequestASliceAppProgramToUpdatePacket = new(1, "RequestASliceAppProgramToUpdatePacket");
    public static readonly Verb RequestBSliceAppProgramToUpdatePacket = new(4, "RequestBSliceAppProgramToUpdatePacket");
    public static readonly Verb RequestColors = new(5, "RequestColors");
    public static readonly Verb RestoreFactorySettings = new(12, "RestoreFactorySettings");
    public static readonly Verb SendingListOfFavoritesIsComplete = new(2, "SendingListOfFavoritesIsComplete");
    public static readonly Verb SendLightSettings = new(4, "SendLightSettings");
    public static readonly Verb SendTheCurrentTime = new(2, "SendTheCurrentTime");
    public static readonly Verb SetBrightness = new(7, "SetBrightness");
    public static readonly Verb SetDrawingSpeed = new(6, "SetDrawingSpeed");
    public static readonly Verb SetSleepTimeAndHibernationStatus = new(1, "SetSleepTimeAndHibernationStatus");
    public static readonly Verb SinglePlotDrawingOperation = new(1, "SinglePlotDrawingOperation");
    public static readonly Verb SleepModeTotalSwitchSettings = new(9, "SleepModeTotalSwitchSettings");
    public static readonly Verb StartAnAppProgramUpdate = new(5, "StartAnAppProgramUpdate");
    public static readonly Verb StartDownloadingASliceAppFile = new(1, "StartDownloadingASliceAppFile");
    public static readonly Verb StartDownloadingBSliceAppFile = new(3, "StartDownloadingBSliceAppFile");
    public static readonly Verb StartDownloadingFile = new(1, "StartDownloadingFile");
    public static readonly Verb StartModifyingColorData = new(1, "StartModifyingColorData");
    public static readonly Verb StartSendingLightColorData = new(1, "StartSendingLightColorData");
    public static readonly Verb StartSendingListData = new(1, "StartSendingListData");
    public static readonly Verb StartSendingListOfFavorites = new(1, "StartSendingListOfFavorites");
    public static readonly Verb StartSendingPictureMessages = new(1, "StartSendingPictureMessages");
    public static readonly Verb ThereIsNotEnoughStorageSpace = new(6, "ThereIsNotEnoughStorageSpace");
    public static readonly Verb UpdateOtherLightControlStates = new(4, "UpdateOtherLightControlStates");
    public static readonly Verb WaitForConnectionData = new(1, "WaitForConnectionData");
    public static readonly Verb Warning = new(3, "Warning");
    public static readonly Verb None = new(0, "None");
    public Verb(byte value, string name)
    {
        Value = value;
        Name = name;
    }

    public byte Value { get; }
    public string Name { get; }
}
