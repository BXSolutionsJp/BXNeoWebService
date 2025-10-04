
function GetNyukoList() {
    var shnCd = document.getElementById("ShnCd").value;
    
    if (shnCd == "") {
        if (document.getElementById("NyukoList").innerHTML == "") {
            Ht.ShowErrorMessage("商品コードを入力してください。");
        }
        else {
            InitView();
        }
        return false;
    }
    var url = BXWebServiceUrl;    
    var pl = new SOAPClientParameters();
    pl.add("shnCd", shnCd);    
    try {
        Ht.CallWebMethod(url, "GetNyukoList", pl, false, GetNyukoList_callBack);
    }
    catch (e) {
        Ht.ShowErrorMessage("無効な商品コードです。");
        document.getElementById("ShnCd").value = "";
    }
}

function GetNyukoList_callBack(r) {    
    try {
        var result = "";

        if (r.length > 0) {
            
            var childTemplate = "<dd class=\"w25 clear\">{0}</dd><dd class=\"w64\"><a href=\"{1}\">{2}</a></dd><dd class=\"w25 right\">{3}</dd>";
            for (var i = 0; i < r.length; i++) {
                //var url = baseUrl + Format(storeUrlFormat,tantocd, r[i].HKAT_SHIJI_NO);
                var url = baseUrl + "HTS/Nyka_Store.aspx?TANTO_CD=" + tantocd + "&hkat_shiji_no=" + r[i].HKAT_SHIJI_NO;                
                result += Format(childTemplate, r[i].NYSHK_DATE, url, r[i].OKURIJO_NO, r[i].QTY);                
            }

            result = "<dt class=\"w25\">日付</dt><dt class=\"w64\">発注番号</dt><dt class=\"w25\">数量</dt>" + result;
        }
        else {
            result = "<p class=\"note\">入荷予定はありません。</p>";
        }

        document.getElementById("NyukoList").innerHTML = result;
    }
    catch (e) {
        Ht.ShowErrorMessage("無効な商品コードです。");
        document.getElementById("ShnCd").focus();
    }

    return;
}

function InitView() {
    document.getElementById("NyukoList").innerHTML = "<p class=\"note\">商品コードを入力します。</p>";

    document.getElementById("ShnCd").focus();
}