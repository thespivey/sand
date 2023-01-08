package com.sand.table.Protocol.Serialization;

public class Verb {
        public static final Verb Ack = new Verb(0, "Ack");
        public static final Verb AddFilesToFavorite = new Verb(6, "AddFilesToFavorite");
        public static final Verb AddFilesToList = new Verb(4, "AddFilesToList");
        public static final Verb AddList = new Verb(1, "AddList");
        public static final Verb AllCompletesData = new Verb(1, "AllCompletesData");
        public static final Verb AllStateDataDuringTheRun = new Verb(1, "AllStateDataDuringTheRun");
        public static final Verb AChipAppProgramFailedToDownload = new Verb(3, "AChipAppProgramFailedToDownload");
        public static final Verb ASliceAppFileData = new Verb(2, "ASliceAppFileData");
        public static final Verb BSliceAppFileData = new Verb(4, "BSliceAppFileData");
        public static final Verb BSliceAppProgramFailedToDownload = new Verb(6, "BSliceAppProgramFailedToDownload");
        public static final Verb ColorData = new Verb(3, "ColorData");
        public static final Verb ColorPreview = new Verb(8, "ColorPreview");
        public static final Verb ColorSettings = new Verb(13, "ColorSettings");
        public static final Verb CompleteReceiptOfASliceAppProgram = new Verb(2, "CompleteReceiptOfASliceAppProgram");
        public static final Verb CompleteReceiptOfBSliceAppProgram = new Verb(5, "CompleteReceiptOfBSliceAppProgram");
        public static final Verb ConfirmConnectionData = new Verb(2, "ConfirmConnectionData");
        public static final Verb ContentsOfFileSent = new Verb(1, "ContentsOfFileSent");
        public static final Verb CycleModeSwitchSettings = new Verb(11, "CycleModeSwitchSettings");
        public static final Verb DataReceptionFailed = new Verb(2, "DataReceptionFailed");
        public static final Verb DataReceptionIsComplete = new Verb(1, "DataReceptionIsComplete");
        public static final Verb DeleteFile = new Verb(3, "DeleteFile");
        public static final Verb DeleteFilesFromFavorites = new Verb(7, "DeleteFilesFromFavorites");
        public static final Verb DeleteList = new Verb(2, "DeleteList");
        public static final Verb DrawListPicture = new Verb(2, "DrawListPicture");
        public static final Verb EndDownloadingFile = new Verb(2, "EndDownloadingFile");
        public static final Verb EndModifyingColorData = new Verb(2, "EndModifyingColorData");
        public static final Verb EndSendingLightColorData = new Verb(2, "EndSendingLightColorData");
        public static final Verb ErrorsCanBeRecovered = new Verb(2, "ErrorsCanBeRecovered");
        public static final Verb FatalError = new Verb(1, "FatalError");
        public static final Verb FavoriteData = new Verb(4, "FavoriteData");
        public static final Verb FavoriteListData = new Verb(3, "FavoriteListData");
        public static final Verb FileDownloadFailed = new Verb(5, "FileDownloadFailed");
        public static final Verb FileIsMissing = new Verb(1, "FileIsMissing");
        public static final Verb FileReceiptCompleted = new Verb(3, "FileReceiptCompleted");
        public static final Verb FileStartsToReceive = new Verb(2, "FileStartsToReceive");
        public static final Verb FileWasDeletedSuccessfully = new Verb(4, "FileWasDeletedSuccessfully");
        public static final Verb GetCurrentTimeFromApp = new Verb(2, "GetCurrentTimeFromApp");
        public static final Verb GetSleepTimeAndHibernationStatus = new Verb(1, "GetSleepTimeAndHibernationStatus");
        public static final Verb InitiateOrPauseDrawing = new Verb(3, "InitiateOrPauseDrawing");
        public static final Verb LightColorData = new Verb(3, "LightColorData");
        public static final Verb ListData = new Verb(3, "ListData");
        public static final Verb ListDataIsSentAtEnd = new Verb(2, "ListDataIsSentAtEnd");
        public static final Verb ModifyList = new Verb(3, "ModifyList");
        public static final Verb NextPicture = new Verb(4, "NextPicture");
        public static final Verb OnOrOffAromatherapy = new Verb(14, "OnOrOffAromatherapy");
        public static final Verb PictureInformation = new Verb(3, "PictureInformation");
        public static final Verb PictureMessageIsSentAtEnd = new Verb(2, "PictureMessageIsSentAtEnd");
        public static final Verb Playlist = new Verb(8, "Playlist");
        public static final Verb PreviousPicture = new Verb(5, "PreviousPicture");
        public static final Verb RandomModeSwitchSettings = new Verb(10, "RandomModeSwitchSettings");
        public static final Verb RemoveFileFromList = new Verb(5, "RemoveFileFromList");
        public static final Verb RequestASliceAppProgramToUpdatePacket = new Verb(1, "RequestASliceAppProgramToUpdatePacket");
        public static final Verb RequestBSliceAppProgramToUpdatePacket = new Verb(4, "RequestBSliceAppProgramToUpdatePacket");
        public static final Verb RequestColors = new Verb(5, "RequestColors");
        public static final Verb RestoreFactorySettings = new Verb(12, "RestoreFactorySettings");
        public static final Verb SendingListOfFavoritesIsComplete = new Verb(2, "SendingListOfFavoritesIsComplete");
        public static final Verb SendLightSettings = new Verb(4, "SendLightSettings");
        public static final Verb SendTheCurrentTime = new Verb(2, "SendTheCurrentTime");
        public static final Verb SetBrightness = new Verb(7, "SetBrightness");
        public static final Verb SetDrawingSpeed = new Verb(6, "SetDrawingSpeed");
        public static final Verb SetSleepTimeAndHibernationStatus = new Verb(1, "SetSleepTimeAndHibernationStatus");
        public static final Verb SinglePlotDrawingOperation = new Verb(1, "SinglePlotDrawingOperation");
        public static final Verb SleepModeTotalSwitchSettings = new Verb(9, "SleepModeTotalSwitchSettings");
        public static final Verb StartAnAppProgramUpdate = new Verb(5, "StartAnAppProgramUpdate");
        public static final Verb StartDownloadingASliceAppFile = new Verb(1, "StartDownloadingASliceAppFile");
        public static final Verb StartDownloadingBSliceAppFile = new Verb(3, "StartDownloadingBSliceAppFile");
        public static final Verb StartDownloadingFile = new Verb(1, "StartDownloadingFile");
        public static final Verb StartModifyingColorData = new Verb(1, "StartModifyingColorData");
        public static final Verb StartSendingLightColorData = new Verb(1, "StartSendingLightColorData");
        public static final Verb StartSendingListData = new Verb(1, "StartSendingListData");
        public static final Verb StartSendingListOfFavorites = new Verb(1, "StartSendingListOfFavorites");
        public static final Verb StartSendingPictureMessages = new Verb(1, "StartSendingPictureMessages");
        public static final Verb ThereIsNotEnoughStorageSpace = new Verb(6, "ThereIsNotEnoughStorageSpace");
        public static final Verb UpdateOtherLightControlStates = new Verb(4, "UpdateOtherLightControlStates");
        public static final Verb WaitForConnectionData = new Verb(1, "WaitForConnectionData");
        public static final Verb Warning = new Verb(3, "Warning");

        private byte _value;
        private String _name;

        public Verb(int value, String name) {
                _value = (byte)value;
                _name = name;
        }

        public byte getValue() {
                return _value;
        }

        public String getName() {
                return _name;
        }
}
