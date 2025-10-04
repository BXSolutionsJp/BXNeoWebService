Ht.F2Click = function () {
    var action = GetFKeyAction("f2Action");

    if (action == "search") {        
        var KEY_CD = document.getElementById("KykShnCd").value;

        if (KEY_CD == "") {
            window.alert("コードが入力されていませんので中断します。");
            return;
        } else {
            //window.alert("推奨ロケを表示します。\n 担当者" + tantocd+"\n 商品コード"+KEY_CD);
            GetSuishoLoc(KEY_CD, tantocd);
        }

    }
}

Ht.Close = function () {
    document.getElementById("popup").dispose;

    var objPopup = document.getElementById("popup");
    var objBody = document.getElementsByTagName("body").item(0);

    objBody.removeChild(objPopup);

    SetFKeyAction('f1Action', 'back');
    SetFKeyAction('f2Action', 'search');
}

function GetMoveData() {
    
    var kykShnCd = document.getElementById("KykShnCd").value;
    var sokoKanriCd = document.getElementById("SOKO_KANRI_CD").value;
    var locNo = document.getElementById("MotLocNo").value;
    var koza = document.getElementById("KOZA").value;
       
    //var msg = "kykShnCd=" + kykShnCd +"\n";
    //msg += "sokoKanriCd=" + sokoKanriCd + "\n";
    //msg += "locNo=" + locNo + "\n";
    //msg += "koza=" + koza + "\n";

    //window.alert(msg);

    if (kykShnCd == "") {
        if (isChrome) {
            window.alert("商品コードを入力してください。");
        } else {
            Ht.ShowErrorMessage("商品コードを入力してください。");
        }
        InitView();
        return false;
    }
    
    if (locNo == "") {
        if (isChrome) {
            window.alert("移動元を入力して下さい。");
        } else {
            Ht.ShowErrorMessage("移動元を入力して下さい。");
        }        
        InitView();
        return false;
    }

    

    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("SOKO_KANRI_CD", sokoKanriCd);
    pl.add("SHN_CD", kykShnCd);
    pl.add("LOC_NO", locNo);
    pl.add("KOZA", koza);

    try {
        Ht.CallWebMethod(url, "GetMoveData", pl, false, GetMoveData_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }
}

function GetMoveData_callBack(r) {

    

    
    if (r["TANA_ZK_QTY"] > 0) {
        document.getElementById("KykShnCd").value = r["KYK_SHN_CD"];
        document.getElementById("ZkKanri").value = r["ZK_KANRI"];
        document.getElementById("GnpnShnCd").value = r["GNPN_SHN_CD"];
        document.getElementById("DispQty").value = r["TANA_ZK_QTY"];

        document.getElementById("SakiLocNo").focus();
    }
    else {
        if (isChrome) {
            window.alert("移動可能な商品はありません。");
        } else {
            Ht.ShowErrorMessage("移動可能な商品はありません。");        
        }
        InitView();
    }

    return;
}

//public ProcedureResult MoveZaiko(string gnpn_shn_cd, string koza, string zk_kanri, string motoloc_no, string sakiloc_no, decimal qty, string hkat_flg, string dict_flg, string tanto_cd)

function MoveZaiko() {
    
    var kykShnCd = document.getElementById("KykShnCd").value;    
    var gnpnShnCd = document.getElementById("GnpnShnCd").value;    
    var koza = document.getElementById("KOZA").value;    
    var unit = document.getElementById("Unit").value;    
    var zkKanri = document.getElementById("ZkKanri").value;    
    var motLocNo = document.getElementById("MotLocNo").value;    
    var sakiLocNo = document.getElementById("SakiLocNo").value;    
    var qty = document.getElementById("Qty").value;    
    var hkatFlg = document.getElementById("HkatFlg").value;    
    var dictFlg = document.getElementById("DictFlg").value;
    
    

    //window.alert(gnpnShnCd + "_" + koza + "_" + unit + "_" + zkKanri + "_" + motLocNo + "_" + sakiLocNo + "_" + qty + "_" + hkatFlg + "_" + dictFlg + "_" + tantocd);

    if (gnpnShnCd == "") {
        if (isChrome) {
            window.alert("移動可能な商品はありません。");
        } else {
            Ht.ShowErrorMessage("移動可能な商品はありません。");
        }
        InitView();
        return false;
    }
    
    if (motLocNo == "") {
        if (isChrome) {
            window.alert("移動元ロケーションを入力してください。");
        } else {
            Ht.ShowErrorMessage("移動元ロケーションを入力してください。");
        }
        return false;
    }
    
    if (sakiLocNo == "") {
        if (isChrome) {
            window.alert("移動先ロケーションを入力してください。");
        } else {
            Ht.ShowErrorMessage("移動先ロケーションを入力してください。");
        }
        return false;
    }
    
    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
        
    pl.add("GNPN_SHN_CD", gnpnShnCd);
    pl.add("KYK_SHN_CD", kykShnCd);
    pl.add("KOZA", koza);    
    pl.add("MOTO_LOC", motLocNo);
    pl.add("SAKI_LOC", sakiLocNo);
    pl.add("QTY", qty);
    pl.add("TANTO_CD", tantocd);    
    

    try {        
        Ht.CallWebMethod(url, "MoveZaikoFIFO", pl, false, MoveZaiko_callBack);
    }
    catch (e) {
        if (isChrome) {
            window.alert("在庫移動エラー\r\n"+e.message);

        } else {
            Ht.ShowErrorMessage("在庫移動エラー");
        }
        InitView();
    }
}

function MoveZaiko_callBack(r) {
    if (r==null) {
        if (isChrome) {
            window.alert("在庫移動エラー");
        } else {
            Ht.ShowErrorMessage("在庫移動エラー");
        }
    } else {
        if (r["RTN"] == null) {
            if (isChrome) {
                window.alert("棚移動異常...");
            } else {
                Ht.ShowErrorMessage("棚移動異常...");
            }
        } else {
            var messageFormat = "棚移動完了\r\n\r\n棚出:{0}\r\n棚入:{1}\r\n商品:{2}\r\n数量:{3}";
            var kykShnCd = r["KYK_SHN_CD"];
            var motLocNo = r["MOT_LOC"];
            var sakiLocNo = r["SAKI_LOC"];
            var qty = r["RTN_QTY"];

            var message = Format(messageFormat, motLocNo, sakiLocNo, kykShnCd, qty);
            if (isChrome) {
                window.alert(message);
            }
            else {
                Ht.ShowErrorMessage(message);
            }
        }
    }

    InitView();

    return;
}

function InitView() {
    //document.getElementById("SOKO_KANRI_CD").selectedIndex = 0;
    document.getElementById("GnpnShnCd").value = "";
    document.getElementById("ShnCd").value = "";
    document.getElementById("KykShnCd").value = "";
    document.getElementById("MotLocNo").value = "";
    document.getElementById("SakiLocNo").value = "";
    document.getElementById("Qty").value = "";
    document.getElementById("DispQty").value = "";
    document.getElementById("ZkKanri").value = "";
    //document.getElementById("Koza").selectedIndex = 0;

    document.getElementById("ShnCd").focus();
}

function ShnCdChanged() {
    var value = document.getElementById("ShnCd").value;

    if (value == "") {
        //        Ht.ShowErrorMessage("数量を入力してください。");
        //        document.getElementById("Qty").focus();
        //        document.getElementById("SubmitButton").disabled = true;
        return;
    } else {
        if (value.length == 13) {

            var url = BXWebServiceUrl;
            var pl = new SOAPClientParameters();
            pl.add("JanCode", value);
            try {
                Ht.CallWebMethod(url, "strJanToKykShnCd", pl, false, getKykShnCd_callBack);
            }
            catch (e) {
                Ht.ShowErrorMessage("SOAPエラー");
                InitView();

            }
        } else {
            document.getElementById("KykShnCd").value = value;
            document.getElementById("KOZA").focus();

        }
    }
}
function getKykShnCd_callBack(r) {
    if (r == null) {
        Ht.ShowErrorMessage("JANコードから商品コードの変換ができませんでした。");
        InitView();
    }
    else {
        document.getElementById("KykShnCd").value = r;
        document.getElementById("KOZA").focus();
    }

    return;
}

function MotLocNoChanged() {
    GetMoveData();
}

function SakiLocNoChanged() {
    var motloc = document.getElementById("MotLocNo").value;
    var sakloc = document.getElementById("SakiLocNo").value;

    if (sakloc == "") {
        //        Ht.ShowErrorMessage("数量を入力してください。");
        //        document.getElementById("Qty").focus();
        //        document.getElementById("SubmitButton").disabled = true;
        return;
    }
    else {
        //        document.getElementById("SubmitButton").disabled = false;
        if (motloc == sakloc) {
            if (isChrome) {
                window.alert("移動元と移動先ロケーションが同じです。");
            } else {
                Ht.ShowErrorMessage("移動元と移動先ロケーションが同じです。");
            }
            document.getElementById("SakiLocNo").value = "";
            document.getElementById("SakiLocNo").focus();
        } else {
            document.getElementById("Qty").focus();
        }
    }
}

function QtyChanged() {

    try {
        var qty = parseInt(document.getElementById("Qty").value);
        var tanaZkQty = parseInt(document.getElementById("DispQty").value);

        if (qty <= 0) {
            if (isChrome) {
                window.alert("数量が不正です。");
            } else {
                Ht.ShowErrorMessage("数量が不正です。");
            }
            document.getElementById("Qty").value = "";
            return;
        }


        if (qty > tanaZkQty) {
            if (isChrome) {
                window.alert("移動数量が過剰です。");
            } else {
                Ht.ShowErrorMessage("移動数量が過剰です。");
            }
            document.getElementById("Qty").value = "";
            return;
        }
    }
    catch (e) {
        if (isChrome) {
            window.alert("数量が不正です。");
        } else {
            Ht.ShowErrorMessage("数量が不正です。");
        }
        document.getElementById("Qty").value = "";
        return;
    }

    document.getElementById("SubmitButton").focus();
}

function GetSuishoLoc(shnCd, tanto) {
    if (shnCd == "") {    
        return false;
    }
    
    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("KEY_CD", shnCd);
    pl.add("TANTO", tanto);    
    try {
        
        Ht.CallWebMethod(url, "GETSUISHOLOC", pl, false, GetSuishoLoc_callBack);
    }
    catch (e) {
        windows.alert(5);
    }
}

function GetSuishoLoc_callBack(r) {
    var result = "";    
    try {
        
        if (r.length > 0) {
        
            var childTemplate = "<dd class=\"w50 clear\">{0}</dd><dd class=\"w50 right\">{1}</dd>";
        
            for (var i = 0; i < r.length; i++) {
                result += Format(childTemplate, r[i].VAL_1, r[i].VAL_5);
            }

            result = "<dt class=\"w50\">ロケ</dt><dt class=\"w50\">在庫数</dt>" + result;
        }
        else {            
            result = "<p class=\"note\">在庫はありません。</p>";
        }
    }
    catch (e) {
        
        result = "<p class=\"note\">在庫はありません。</p>";
    }
    
    var element = document.createElement("div");
    
    element.id = "popup";
    
    element.innerHTML = "<h2>推奨ロケーション</h2><div class=\"block\"><dl id=\"LocationZaiko\" class=\"datagrid\"></dl></div>"
    
    var objBody = document.getElementsByTagName("body").item(0);
    
    objBody.appendChild(element);
    
    document.getElementById("LocationZaiko").innerHTML = result;
    
    SetFKeyAction('f1Action', 'close');    
    SetFKeyAction('f2Action', '');
    
}

function SelectLocation() {
    document.getElementById("CurrentIndex").value = currentIndex + 1;
    document.getElementById("RecordCount").value = recordCount;

    document.getElementById("HkatShijiNo").value = pickingDataCollection[currentIndex].HkatShijiNo;
    document.getElementById("ShnCd").value = pickingDataCollection[currentIndex].ShnCd;
    document.getElementById("DispLocNo").value = pickingDataCollection[currentIndex].LocNo;
    document.getElementById("LocNo").value = "";
    document.getElementById("JanCd").value = "";
    document.getElementById("Koza").value = pickingDataCollection[currentIndex].Koza;
    document.getElementById("DispQty").value = pickingDataCollection[currentIndex].YoteiQty;
    document.getElementById("Qty").value = 0;

    document.getElementById("SubmitButton").disabled = true;
    document.getElementById("LocNo").focus();
}