
function GetTantoNm() {
    var tantoCd = document.getElementById("TANTO_CD").value;



    if (tantoCd == "") {
        Ht.ShowErrorMessage("担当コードを入力してください。");
        return false;
    }

    var url = BXWebServiceUrl;
    var pl = new SOAPClientParameters();


    pl.add("TANTO_CD", tantoCd);

    Ht.CallWebMethod(url, "getSLPTanToName", pl, false, GetTantoNm_callBack);
}
function GetTantoNm_callBack(r) {


    if (r == null) {
        Ht.ShowErrorMessage("無効な担当コードです。");
        document.getElementById("TANTO_CD").value = "";

        return;
    }

    document.getElementById("LoginName").innerHTML = r;
    document.getElementById("UserPass").value = "";
    document.getElementById("UserPass").focus();
}

function PasswordChanged() {
    var password = document.getElementById("UserPass").value;

    if (password == "") {
        document.getElementById("LoginButton").disabled = true;
        return;
    }



    document.getElementById("LoginButton").disabled = false;
    document.getElementById("LoginButton").focus();
}