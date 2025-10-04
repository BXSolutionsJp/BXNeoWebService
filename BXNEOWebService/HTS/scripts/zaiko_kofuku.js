function KeyCode_Changed() {
    var KeyCode = document.getElementById("KeyCode").value;
    

    
    var url = BXWebServiceUrl;
    
    var pl = new SOAPClientParameters();
    pl.add("KEY", KeyCode);
    


    try {
        Ht.CallWebMethod(url, "getKofukuInfo", pl, false, KeyCode_Changed_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }

}

function KeyCode_Changed_callBack(r) {


    if(r!=null){
        document.getElementById("KeyCode").value = r["KYK_SHN_CD"];        
        document.getElementById("JanCode").value = r["JAN_CD"];
        document.getElementById("konpo_w").value = r["KONPO_W"];
        document.getElementById("konpo_h").value = r["KONPO_D"];
        document.getElementById("konpo_d").value = r["KONPO_H"];
        document.getElementById("konpo_wght").value = r["KONPO_WGHT"];
        document.getElementById("konpo_w").focus();
    } else {
        if(isChrome){
            window.alert("商品が見つかりませんでした。");
        } else {
            HT.ShowErrorMessage("商品が見つかりませんでした。");
        }
        initView();
    }

}

function konpo_w_change() {
    var control = document.getElementById("konpo_w")
    var value = control.value;

    if (isNaN(value)) {        
        if (isChrome) {
            window.alert("数値で入力してください。");
        } else {
            HT.ShowErrorMessage("数値で入力してください。");
        }
        control.value = "";
        control.focus();
    } else {
        var nextcontrol = document.getElementById("konpo_d");
        nextcontrol.focus();
    }
}

function konpo_d_change() {
    var control = document.getElementById("konpo_d")
    var value = control.value;

    if (isNaN(value)) {
        if (isChrome) {
            window.alert("数値で入力してください。");
        } else {
            HT.ShowErrorMessage("数値で入力してください。");
        }
        control.value = "";
        control.focus();
    } else {
        var nextcontrol = document.getElementById("konpo_h");
        nextcontrol.focus();
    }
}

function konpo_h_change() {
    var control = document.getElementById("konpo_h")
    var value = control.value;

    if (isNaN(value)) {
        if (isChrome) {
            window.alert("数値で入力してください。");
        } else {
            HT.ShowErrorMessage("数値で入力してください。");
        }
        control.value = "";
        control.focus();
    } else {
        var nextcontrol = document.getElementById("konpo_wght");
        nextcontrol.focus();
    }
}

function konpo_wght_Changed() {
    var control = document.getElementById("konpo_wght")
    var value = control.value;

    if (isNaN(value)) {
        if (isChrome) {
            window.alert("数値で入力してください。");
        } else {
            HT.ShowErrorMessage("数値で入力してください。");
        }
        control.value = "";
        control.focus();
    } else {
        var nextcontrol = document.getElementById("SubmitButton");
        nextcontrol.focus();
    }
}

function Kofuku_Reg_Send() {
    var KeyCode = document.getElementById("KeyCode").value;
    var JanCode = document.getElementById("JanCode").value;
    var konpo_w = document.getElementById("konpo_w").value;
    var konpo_h = document.getElementById("konpo_h").value;
    var konpo_d = document.getElementById("konpo_d").value;
    var konpo_wght = document.getElementById("konpo_wght").value;


    //var msg = "KYK_SHN_CD=" + KeyCode + "\n";
    //msg += "KONPO_W=" + konpo_w + "\n";
    //msg += "KONPO_D=" + konpo_d + "\n";
    //msg += "KONPO_H=" + konpo_h + "\n";
    //msg += "KONPO_WGHT=" + konpo_wght + "\n";
    //window.alert(msg);

    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("KYK_SHN_CD", KeyCode);
    pl.add("KONPO_W", konpo_w);
    pl.add("KONPO_D", konpo_d);
    pl.add("KONPO_H", konpo_h);
    pl.add("KONPO_WGHT", konpo_wght);



    try {
        Ht.CallWebMethod(url, "RegKofukuInfo", pl, false, Kofuku_Reg_Send_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }

}

function Kofuku_Reg_Send_callBack(r) {
    if (r != null) {
        if (isChrome) {
            window.alert("登録しました....");
        } else {
            Ht.ShowErrorMessage("登録しました....");
        }
        initView();
    } else {
        if (isChrome){
            window.alert("登録時にエラーが発生しました。");
        } else {
            Ht.ShowErrorMessage("登録時にエラーが発生しました。");
        }
        initView();
    }
}

function initView() {
    document.getElementById("KeyCode").value = "";
    document.getElementById("JanCode").value = "";
    document.getElementById("konpo_w").value = "";
    document.getElementById("konpo_h").value = "";
    document.getElementById("konpo_d").value = "";    
    document.getElementById("konpo_wght").value = "";

    document.getElementById("KeyCode").focus();

}