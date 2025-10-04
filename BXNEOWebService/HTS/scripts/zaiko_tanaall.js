

function MotoLocMoveAll() {
    var sokoKanriCd = document.getElementById("SOKO_KANRI_CD").value;    
    var koza = document.getElementById("KOZA").value;    
    var locNo = document.getElementById("MotLocNo").value;
       
    //var msg = "sokoKanriCd=" + sokoKanriCd + "\n";
    //msg += "koza=" + koza + "\n";
    //msg += "locNo=" + locNo + "\n";
    

    //window.alert(msg);

    
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
    pl.add("LOC_NO", locNo);
    

    try {
        Ht.CallWebMethod(url, "GetMoveData_Tana", pl, false, MotoLocMoveAll_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }
}

function MotoLocMoveAll_callBack(r) {   

    
    if (r !=null) {
        document.getElementById("SkyCnt").value = r["VAL_1"];
        document.getElementById("KozaCnt").value = r["VAL_2"];
        document.getElementById("TotalCnt").value = r["VAL_3"];
        document.getElementById("HkatCnt").value = r["VAL_4"];

        

        if (r["VAL_3"] == "0") {        
            if (isChrome) {        
                window.alert("移動可能な商品はありません。");
            } else {                
                Ht.ShowErrorMessage("移動可能な商品はありません。");
            }
            InitView();
            return;
        }

        if (r["VAL_4"] != "0") {
            if (isChrome) {
                window.alert("出荷引当中在庫が含まれているので一括移動はできません。");
            } else {
                Ht.ShowErrorMessage("出荷引当中在庫が含まれているので一括移動はできません。");
            }
            document.getElementById("MotLocNo").focus();
            document.getElementById("SakiLocNo").disabled = true;
            document.getElementById("SubmitButton").disabled = true;
        } else {
            document.getElementById("SakiLocNo").disabled = false;
            document.getElementById("SakiLocNo").focus();
            document.getElementById("SubmitButton").disabled = false;
        }
        
    }
    else {
        if (isChrome) {
            Window.alert("移動可能な商品はありません。");
        } else {
            Ht.ShowErrorMessage("移動可能な商品はありません。");        
        }
        InitView();
    }

    return;
}

function InitView() {
    //document.getElementById("SOKO_KANRI_CD").selectedIndex = 0;
    
    document.getElementById("MotLocNo").value = "";
    document.getElementById("SakiLocNo").value = "";

    
    
    document.getElementById("SkyCnt").value = "";
    document.getElementById("KozaCnt").value = "";
    document.getElementById("TotalCnt").value = "";
    document.getElementById("HkatCnt").value = "";
    document.getElementById("MotLocNo").focus();
}


function SakiLocNoChanged() {
    var value = document.getElementById("SakiLocNo").value;
    var soko_kanri_cd = document.getElementById("SOKO_KANRI_CD").value;

    if (value == "") {       
        //        document.getElementById("SubmitButton").disabled = true;
        return;
    }

    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("LOC_EDIT", value);
    pl.add("SOKO_KANRI_CD", soko_kanri_cd);

    try {
        Ht.CallWebMethod(url, "isLocationExists", pl, false, SakiLocChanged_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }
        
    
}

function SakiLocChanged_callBack(r) {
    
    sakloc = document.getElementById("SakiLocNo").value;
    motloc = document.getElementById("MotLocNo").value;

    if (motloc == sakloc) {
        if (isChrome) {
            window.alert("移動元と移動先ロケーションが同じです。");
        } else {
            Ht.ShowErrorMessage("移動元と移動先ロケーションが同じです。");
        }
        document.getElementById("SakiLocNo").value = "";
        document.getElementById("SakiLocNo").focus();
        return;
    } 



    if (r == "1") {
        document.getElementById("SubmitButton").disabled = false;
        document.getElementById("SubmitButton").focus();
    } else {
        document.getElementById("SubmitButton").disabled = true;
        if (isChrome) {
            window.alert("存在しないロケーションです");
        } else {
            Ht.ShowErrorMessage("存在しないロケーションです");
        }
        document.getElementById("SakiLocNo").value = "";
        document.getElementById("SakiLocNo").focus();
    }
}

function MoveZaiko_TanaAll() {
    var soko_kanri_cd = document.getElementById("SOKO_KANRI_CD").value;
    var koza = document.getElementById("KOZA").value;
    var motolocno = document.getElementById("MotLocNo").value;
    var sakilocno = document.getElementById("SakiLocNo").value;

     var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();
    pl.add("SOKO_KANRI_CD", soko_kanri_cd);
    pl.add("KOZA", koza);
    pl.add("MOTO_LOC", motolocno);
    pl.add("SAKI_LOC", sakilocno);
    pl.add("TANTO_CD", tantocd);

    try {
        Ht.CallWebMethod(url, "MoveZaikoTanaAll", pl, false, MoveZaiko_TanaAll_callBack);
    }
    catch (e) {
        //Ht.ShowErrorMessage("在庫移動エラー");
        //InitView();
    }

}

function MoveZaiko_TanaAll_callBack(r) {
    var ret = r["RTN_QTY"];

    if (ret > 0) {
        if (isChrome) {
            window.alert("総PCS数[ "+ret+" ]を移動しました。");
        } else {
            Ht.ShowMessage("総PCS数[ " + ret + " ]を移動しました。");
        }
        InitView();
        

        
    }
    else {
        if (isChrome) {
            Window.alert("移動可能な商品はありません。");
        } else {
            Ht.ShowErrorMessage("移動可能な商品はありません。");
        }
        InitView();
    }

    return;
}
