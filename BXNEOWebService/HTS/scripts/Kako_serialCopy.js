function SerialCopyRegister() {
    var PrtLocID = document.getElementById("PrtLocID").value;
    var PrtCnt = document.getElementById("PrtCnt").value;
    var SerialNo = document.getElementById("SerialNo").value;

   
    var url = BXWebServiceUrl;
    
    var pl = new SOAPClientParameters();
    pl.add("PrtLocID", PrtLocID);
    pl.add("PrtCnt", PrtCnt);
    pl.add("SerialNo", SerialNo);

    try {
        Ht.CallWebMethod(url, "RegSeraialCopy", pl, false, SerialCopyRegister_callBack);
        
    }
    catch (e) {
        Ht.ShowErrorMessage("処理中にエラーが発生しました．．．");
        
    }

}




function SerialCopyRegister_callBack(r) {

    var msg = "";
    var isErr = false;

    if (r != null) {        
        if (r[0]=="0"){
            isErr = false;
            msg = "登録しました。";
        } else {
            isErr = true;
            msg = "値に異常値がありました。";
        }
    } else {
        isErr = true;
        msg = "登録できませんでした";
    
    }

    if (isChrome) {
        
        window.alert(msg);
    }
    else{
        if (isErr){
            HT.ShowErrorMessage(msg);
        } else {
            HT.ShowMessage(msg);
        }
    }
    initView();
}


function initView() {
    document.getElementById("SerialNo").value = "";
    document.getElementById("PrtCnt").value = "1";
    document.getElementById("SerialNo").focus();
}