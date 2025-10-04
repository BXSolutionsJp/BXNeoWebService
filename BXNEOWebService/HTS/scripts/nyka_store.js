
//function GetStoreData() {
//    var hkatShijiNo = document.getElementById("HkatShijiNo").value;

//    if (hkatShijiNo == "") {
//        Ht.ShowErrorMessage("作業指示番号を入力してください。");
//        return false;
//    }

//    var url = htWebServiceUrl;
//    var pl = new SOAPClientParameters();
//    pl.add("hkat_shiji_no", hkatShijiNo);

//    try {
//        Ht.CallWebMethod(url, "GetStoreData", pl, false, GetStoreData_callBack);
//    }
//    catch (e) {
//        Ht.ShowErrorMessage("無効な作業指示番号です。");
//        document.getElementById("HkatShijiNo").value = "";
//    }
//}

//function GetStoreData_callBack(r) {
//    try {
//        document.getElementById("KanriNo").value = r.KanriNo;
//        document.getElementById("ShnCd").innerHTML = r.ShnCd;
//        //document.getElementById("ShnNm").innerHTML = r.ShnNm;
//        document.getElementById("LocNoDisp").innerHTML = '41-10-1-5';
//        document.getElementById("LocNo").value = 'A17411015';
//        document.getElementById("Koza").value = r.Koza;
//        document.getElementById("QtyDisp").innerHTML = r.Qty;
//        document.getElementById("YoteiQty").value = r.Qty;
//        document.getElementById("Qty").value = "";
//    }
//    catch(e)
//    {
//        Ht.ShowErrorMessage("無効な作業指示番号です。");
//        document.getElementById("HkatShijiNo").value = "";
//    }

//    return;
//}

////EndStore(string kbn, string kanri_no, string tanto_cd, string shiji_no, string shn_cd, string lot_no, string loc_no, decimal qty)
//function EndStore() {    
//    var kbn = document.getElementById("Kbn").value;
//    var kanriNo = document.getElementById("KanriNo").value;
//    var tantoCd = document.getElementById("TantoCd").value;
//    var shijiNo = document.getElementById("HkatShijiNo").value;
//    var shnCd = document.getElementById("ShnCd").innerHTML;
//    var lotNo = document.getElementById("LotNo").value;
//    var locNo = document.getElementById("LocNo").value;
//    var qty = parseInt(document.getElementById("Qty").value);
//    var yoteiQty = parseInt(document.getElementById("YoteiQty").value); 
    
       
//    if (shijiNo == ""){
//        Ht.ShowErrorMessage("作業指示番号を入力してください。");
//        return false;
//    }

//    if (qty == "") {
//        Ht.ShowErrorMessage("数量を入力してください。");
//        return false;
//    }

//    if (!Ht.ShowYesNoDialog("登録しますか？", "yes")) {
//        return;
//    }

//    if (qty > yoteiQty) {
//        var message = Format("+{0}\r\n数量過剰です。\r\n処理を中断します。", qty - qtyDisp);

//        if (!Ht.ShowYesNoDialog(message, "yes")) {
//            return;
//        }
//    }


//    var url = htWebServiceUrl;
//    var pl = new SOAPClientParameters();
//    pl.add("kbn", kbn);
//    pl.add("kanri_no", kanriNo);
//    pl.add("tanto_cd", tantoCd);
//    pl.add("shiji_no", shijiNo);
//    pl.add("shn_cd", shnCd);
//    pl.add("lot_no", lotNo);
//    pl.add("loc_no", locNo);
//    pl.add("qty", qty);

//    //alert(kbn + "_" + kanriNo + "_" + tantoCd + "_" + shijiNo + "_" + shnCd + "_" + lotNo + "_" + locNo + "_" + qty);

//    Ht.CallWebMethod(url, "EndStore", pl, false, EndStore_callBack);
//}

//function EndStore_callBack(r) {
//    if (!r) {
//        Ht.ShowErrorMessage("入庫エラー");
//    } else {
//        if (r.RC == 3) {
//            Ht.ShowErrorMessage("数量過剰...");
//            ReleaseJob();
//        } else if (r.RC == 0 || r.RC == 1 || r.RC == 9) {
//            Ht.ShowErrorMessage("入庫ＯＫ...");
//        } else {
//            Ht.ShowErrorMessage("入庫異常...rc:" + r.RC);
//        }
//    }

//    InitView();

//    return;
//}
//public ProcedureResult EndLipStore(string hkat_shiji_no, decimal qty, string loc_no, string lip_no, string lot_no, string rsn_cd, string upd_tanto_cd)
function EndLipStore() {

    
    

    var hkatShijiNo = document.getElementById("HkatShijiNo").value;    
    var qty = parseInt(document.getElementById("Qty").value);    
    var yoteiQty = parseInt(document.getElementById("YoteiQty").value);    
    var LocSec = document.getElementById("LOC_NO");
    
    var loc_no = LocSec.options[LocSec.selectedIndex].value;
    
    
    var url = baseUrl + "BXSoap.asmx";
    

    if (hkatShijiNo == "") {
        Ht.ShowErrorMessage("作業指示番号を入力してください。");
        return false;
    }
    
    if (isChrome) {
        if (!window.confirm('登録しますか？')) {
            return;
        }
    } else {
        if (!Ht.ShowYesNoDialog("登録しますか？", "yes")) {
            return;
        }
    }
    
    if (qty > yoteiQty) {
        var message = Format("+{0}\r\n数量過剰です。\r\n続行しますか?", qty - yoteiQty);

        if (!Ht.ShowYesNoDialog(message, "yes")) {
            return;
        } 
    }
    
    var pl = new SOAPClientParameters();
    pl.add("HKAT_SHIJI_NO", hkatShijiNo);
    pl.add("QTY", qty);
    pl.add("LOC_NO", loc_no);
    pl.add("TANTO_CD", tantocd);
    pl.add("LIP_NO", "");
    pl.add("LOT_NO", "");
    pl.add("RSN_CD", "00");
    


    
    Ht.CallWebMethod(url, "LipStoreEnd", pl, false, EndLipStore_callBack);
    
    
}

function EndLipStore_callBack(r) {
    
    if (isChrome) {        
        if (r == 3) {
            window.alert("過剰入庫OK...");
        } else if (r == 2) {
            window.alert("分割入庫OK...");
        } else if (r == 1) {
            window.alert("ロケーション変更OK...");
        } else if (r == 0) {
            window.alert("入庫OK...");
        } else {
            window.alert("入庫異常...rc:" + r);
        }
    } else {
        if (!r) {
            Ht.ShowErrorMessage("入庫エラー");
            Bt_1000.Back();
            return;
        } else {
            if (r == 3) {
                Ht.ShowErrorMessage("過剰入庫OK...");
            } else if (r == 2) {
                Ht.ShowErrorMessage("分割入庫OK...");
            } else if (r == 1) {
                Ht.ShowErrorMessage("ロケーション変更OK...");
            } else if (r == 0) {
                Ht.ShowErrorMessage("入庫OK...");
            } else {
                Ht.ShowErrorMessage("入庫異常...rc:" + r);
            }
        }      
    }

    Bt_1000.Back();
    return;
    
}

//function InitView() {
//    document.getElementById("HkatShijiNo").value = "";
//    document.getElementById("KanriNo").value = "";
//    document.getElementById("ShnCd").innerHTML = "&nbsp;";
//    //document.getElementById("ShnNm").innerHTML = "&nbsp;";
//    document.getElementById("Koza").selectedIndex = 0;
//    document.getElementById("LocNoDisp").innerHTML = "&nbsp;";
//    document.getElementById("LocNo").value = "";
//    document.getElementById("LotNo").value = "";
//    document.getElementById("Qty").value = "";
//    document.getElementById("YoteiQty").value = ""; 

//    document.getElementById("HkatShijiNo").focus();
//}

//function LocNoChanged() {
//    var locNo = document.getElementById("LocNo").value;

//    if (qty == "") {
//        //        Ht.ShowErrorMessage("数量を入力してください。");
//        //        document.getElementById("Qty").focus();
//        //document.getElementById("SubmitButton").disabled = true;
//        return;
//    }
//    else {
//        document.getElementById("Qty").focus();
//    }
//}

function QtyChanged() {
    var qty = document.getElementById("Qty").value;
    

    if (isNaN(qty)) {        
        document.getElementById("Qty").value = "";
        
        window.alert("数値を入力してください。");
        document.getElementById("Qty").focus();
        
    }

    if (qty == "") {
//        Ht.ShowErrorMessage("数量を入力してください。");
//        document.getElementById("Qty").focus();
        //document.getElementById("SubmitButton").disabled = true;
        return;
    }
    else {
        //document.getElementById("SubmitButton").disabled = false;
        document.getElementById("SubmitButton").focus();
    }
}