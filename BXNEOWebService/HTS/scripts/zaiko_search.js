

function GetZaikoList() {
    var KEY_CD = document.getElementById("ShnCd").value;

    if (KEY_CD == "") {        
        return;
    } else {
        //window.alert("推奨ロケを表示します。\n 担当者" + tantocd+"\n 商品コード"+KEY_CD);
        GetStockInfo(KEY_CD, tantocd);
    }
}

function GetStockInfo(shnCd, tanto) {
    if (shnCd == "") {        
        return false;
    }    
    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("KEY_CD", shnCd);
    pl.add("TANTO", tanto);    
    try {        
        Ht.CallWebMethod(url, "GETSUISHOLOC", pl, false, GetStockInfo_callBack);    }
    catch (e) {        
    }
}

function GetStockInfo_callBack(r) {
    try {
        var result = "";

        if (r.length > 0) {
            
            var childTemplate = "<dd class=\"w39 clear\">{0}</dd></dd><dd class=\"w25 right\">{1}</dd><dd class=\"w25 right\">{2}</dd>";
            for (var i = 0; i < r.length; i++) {
                
                              
                result += Format(childTemplate, r[i].VAL_1, r[i].VAL_5, r[i].VAL_6);
            }

            result = "<dt class=\"w39\">ﾛｹｰｼｮﾝ</dt><dt class=\"w25\">在庫</dt><dt class=\"25\">引当</dt>" + result;
        }
        else {
            result = "<p class=\"note\">在庫は存在しません。</p>";
        }

        document.getElementById("ZaikoList").innerHTML = result;
    }
    catch (e) {
        if (isChrome) {
            Ht.ShowErrorMessage("在庫は存在しません。");
        } else{
            window.alert("在庫は存在しません。");
        }
        document.getElementById("ShnCd").focus();
    }

    return;
}

function InitView() {
    document.getElementById("ZaikoList").innerHTML = "<p class=\"note\">商品コードを入力します。</p>";

    document.getElementById("ShnCd").focus();
}