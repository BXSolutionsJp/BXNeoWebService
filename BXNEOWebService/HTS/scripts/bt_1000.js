
function Bt_1000() { }

Bt_1000.ShowErrorMessage = function (message) {
    try {
        BtBuzzer.Exec();
        BtVibration.Initialize();
        BtVibration.onTime = 2;
        BtVibration.offTime = 2;
        BtVibration.loopCount = 2;
        BtVibration.Exec();
        BtHandy.ShowMessageBox(message, "ok");
    }
    catch (ex) {
    }
}

Bt_1000.ShowMessage = function (message) {
    try {
        BtHandy.ShowMessageBox(message, "ok");
    }
    catch (ex) {
    }

}

Bt_1000.ShowConfirmDialog = function (message) {
    try {
        BtHandy.ShowMessageBox(message, "confirm");
    }
    catch (ex) {
    }
}

Bt_1000.ShowYesNoDialog = function (message, defaultButton) {
    try {
        defaultButton = defaultButton || "yes";

        return BtHandy.ShowMessageBox(message, "yesno|" + defaultButton);
    }
    catch (ex) {
    }
}

Bt_1000.ShowOkCancelDialog = function (message, defaultButton) {
    try {
        if (defaultButton == null)
            defaultButton = "ok";
        return BtHandy.ShowMessageBox(message, "okcancel|" + defaultButton);
    }
    catch (ex) {
    }
}

Bt_1000.CallWebMethod = function (url, method, parameters, async, callback) {
    try {
        BtHandy.ShowAsyncMessageBox("Please wait...", true);
    }
    catch (ex) { }

    SOAPClient.invoke(url, method, parameters, async, callback);

}

Bt_1000.LoadPage = function () {
    try {
        BtHandy.ShowAsyncMessageBox("Loading...", true);
    }
    catch (ex) {
    }
}

Bt_1000.Back = function () {
    //    try {
    //        BtHandy.ShowAsyncMessageBox("Loading...", true);
    //    }
    //    catch (ex) { }
    history.back();
}

Bt_1000.Next = function (href) {
    window.alert(href);

}

Bt_1000.LogOff = function () {
    try {
        BtHandy.ShowAsyncMessageBox("Loading...", true);
    }
    catch (ex) { }
    location.href = logOffUrl;
}

Bt_1000.Close = function () { }

Bt_1000.F2Click = function () { }