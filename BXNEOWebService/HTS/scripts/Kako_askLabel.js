function getItemInfo() {
    var ItemCode = document.getElementById("ItemCode").value;
    if (ItemCode == "") return;
    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("ItemCode", ItemCode);
    try {
        Ht.CallWebMethod(url, "GETITEMINFO", pl, false, ItemCode_callBack);
    }
    catch (e) {
        Ht.ShowErrorMessage("無効な商品コードです。");
        initView();
    }
}

function ItemCode_callBack(r) {
    try {

        

        if ( r["VAL_2"] != null) {            
            result = "<dt class=\"100%\">商品名</dt>";
            result += "<dd>" + r["VAL_2"] + "</dd>";
            document.getElementById("ItemInfo").innerHTML = result;
            document.getElementById("SubmitButton").disabled = false;
            document.getElementById("SubmitButton").focus();
            
            
        } else    {
            if (isChrome) {
                window.alert("商品が取得できません。");
            } else {
                Ht.ShowErrorMessage("商品が取得できません。");
            }
            
            document.getElementById("SubmitButton").disabled = true;
            initView();
        }
    }
    catch (e) {
        if (isChrome) {
            window.alert("無効な商品コードです。");
            window.alert(e.message);
        } else {
            Ht.ShowErrorMessage("無効な商品コードです。");
        }
        document.getElementById("SubmitButton").disabled = true;
        initView();
    }

    
}



function AskLabelRegister() {
    var PrtLocID = document.getElementById("PrtLocID").value;
    var PrtCnt = document.getElementById("PrtCnt").value;
    var LabelType = document.getElementById("LabelType").value;
    var ItemCode = document.getElementById("ItemCode").value;
    var msg = "";

    if (PrtLocID=="0") {
        msg = "印刷ロケを指定してください。";
        if (isChrome) {
            window.alert(msg);
        } else{
            HT.ShowErrorMessage(msg);
        }
        return;
        
    }
    if (PrtCnt == "" || isNaN(PrtCnt)) {
        msg = "印刷枚数を数値で指定してください。";
        if (isChrome) {
            window.alert(msg);
        } else {
            HT.ShowErrorMessage(msg);
        }
        document.getElementById("PrtCnt").value = "100";
        document.getElementById("PrtCnt").focus();
        return;
    }

    if (LabelType == "0") {
        msg = "種別を指定してください。";
        if (isChrome) {
            window.alert(msg);
        } else {
            HT.ShowErrorMessage(msg);
        }
        return;
    }

    if (ItemCode == "") {
        msg = "商品コードを入力してください。";
        if (isChrome) {
            window.alert(msg);
        } else {
            HT.ShowErrorMessage(msg);
        }
        return;
    }
    //msg = ItemCode + '/' + PrtLocID + '/' + PrtCnt + '/' + LabelType;
    //if (isChrome) {        
    //    window.alert(msg);
    //} else {
    //    HT.ShowErrorMessage(msg);
    //}
    
    
    var url = BXWebServiceUrl;

    
    
    
    var pl = new SOAPClientParameters();
    pl.add("ItemCode", ItemCode);
    pl.add("PrtLocID", PrtLocID);
    pl.add("PrtCnt", PrtCnt);
    pl.add("LabelType", LabelType);

    try {        
        Ht.CallWebMethod(url, "RegASKLabel", pl, false, AskLabelRegister_callBack);
        
    }
    catch (e) {
        Ht.ShowErrorMessage("処理中にエラーが発生しました．．．");
        
    }

}




function AskLabelRegister_callBack(r) {

    
    var msg = "";
    var isErr = false;

    
    if (r != null) {        
        switch (r[0]) {
            case "0":
                isErr = false;
                msg = "登録しました。";
                break;
            case "2":
                isErr = true;
                msg = "指定した種別は印刷ロケに設置されていません。";
                break;
            default:
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
    //document.getElementById("ItemCode").value = "";
    document.getElementById("ItemInfo").innerHTML = "";
    document.getElementById("ItemCode").focus();
}