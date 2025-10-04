function ShnCdChange() {
    
    var shnCd = document.getElementById("ShnCd").value;
    
    
    if (shnCd = "") {
        return;
    } else {
        document.getElementById("qty").focus();
    }
}

function qtychanged() {
    var qty = document.getElementById("qty").value;
    if (qty == "") {
        return;
    }
    if (isNaN(qty)) {
        if (isChrome) {
            window.alert("数量は数値で入力してください。")
        } else {
            Ht.ShowErrorMessage("数量は数値で入力してください。");
        }
        document.getElementById("qty").value = "";
        document.getElementById("qty").focus();

        return;
    } else {
        document.getElementById("sendButton").focus();        
    }


}

function sendRegHaiki() {    
    var shnCd = document.getElementById("ShnCd").value;    
    var qty = document.getElementById("qty").value;    
    if (shnCd == "") {        
        return;
    }

    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    var JanCode;
    var ItemCode;
    var Qty;
    
    if (shnCd.length == 13) {
        JanCode = shnCd;
        ItemCode = "";
    } else {
        ItemCode = shnCd;
        JanCode = "";
    }

    
    var msg = "soko=" + soko + "\n";
    msg += "ItemCode=" + ItemCode + "\n";
    msg += "JanCode=" + JanCode + "\n";
    msg += "qty=" + qty + "\n";

    //window.alert(msg);


    pl.add("SOKO", soko)
    pl.add("ItemCode", ItemCode);
    pl.add("JAN_CD", JanCode);
    pl.add("QTY", qty);
    try {
        Ht.CallWebMethod(url, "REG_HAIKI", pl, false, sendRegHaiki_callBack);
    }
    catch (e) {
        Ht.ShowErrorMessage("無効な商品コードです。");
        document.getElementById("ShnCd").value = "";
    }
}


function sendRegHaiki_callBack(r) {

    try {
        var result = "";

        if (r.length > 0) {

            var childTemplate = "<dd class=\"w30 clear\">{0}</dd><dd class=\"w55\">{1}</dd><dd class=\"w25 right\">{2}</dd>";
            for (var i = 0; i < r.length; i++) {                
                
                result += Format(childTemplate, r[i]["VAL_1"],r[i]["VAL_2"], r[i]["VAL_3"]);
            }

            result = "<dt class=\"w30\">コード</dt><dt class=\"w55\">JanCode</dt><dt class=\"w25\">数量</dt>" + result;
        }
        else {
            result = "<p class=\"note\">廃棄登録はされていません。</p>";
        }

        document.getElementById("HikiList").innerHTML = result;

        initView();
    }
    catch (e) {
        Ht.ShowErrorMessage("無効な商品コードです。");
        
    }

    return;
}

function initView() {
    document.getElementById("ShnCd").value = "";
    document.getElementById("qty").value = "";
    document.getElementById("ShnCd").focus();
}