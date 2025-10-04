
//ハンディ機種設定
var Ht = Bt_1000;
var MyPage = "";
var url = ["", "", "", "", "", "", "", "", "", ""];
var UseKeyRedirect = false;

//HT URL

var baseUrl;
var BXWebServiceUrl;

if (window.location.hostname == "192.168.6.7") {
    baseUrl = "http://192.168.6.7/BxHt/";
    BXWebServiceUrl = "http://192.168.6.7/BxHt/BXSoap.asmx";
} else {
    baseUrl = "http://localhost:63571/";
    BXWebServiceUrl = "http://localhost:63571/BXSoap.asmx";

}



var logOffUrl = baseUrl;
var pickingListUrl = baseUrl + "Shka/PickingList/";
var sequentialPickingUrlFormat = "Shka/SequentialPicking/{0}/";



var agent = window.navigator.userAgent.toLowerCase();
var isChrome = true;


function Format(format /*,obj1,obj2...*/) {
    var args = arguments;
    return format.replace(/\{(\d)\}/g, function (m, c) { return args[parseInt(c) + 1] });
}

var ActiveElement;

function input_Focus(e) {
    ActiveElement = e.target;

    SetInputMode(ActiveElement.getAttribute("inputmode"));
}

function a_Focus(e) {
    ActiveElement = e.target;

    SetInputMode(null);
}

function document_KeyPress(e) {

    flg = false;
    if (flg) {
        window.alert(e.keyCode + "&" + this.para);
    }
    switch (e.keyCode) {
        case 66:
            ReadedEvent();
            break;
        case 70:    //F1
            FKeyPressEvent("F1Action");
            break;
        case 71:    //F2
            Ht.F2Click();
            FKeyPressEvent("F2Action");
            break;
        case 72:    //F3 KeyイベントはHTが吸収?ここに来ない
            FKeyPressEvent("F3Action");
            break;
        case 48:    //0           
        case 49:    //1            
        case 50:    //2            
        case 51:    //3            
        case 52:    //4            
        case 53:    //5            
        case 54:    //6            
        case 55:    //7            
        case 56:    //8            
        case 57:    //9
            if (UseKeyRedirect) { LocationByKey(e.keyCode, this.para); }
            break;

            /*
                L=76
                R=82
                SCAN⇔ともに=84
                上下左右= 1,2,3,4
                C=99
                C長押し~=67
                END=13
                *=46
                ==45

                0=48
                1=49～9=57


            */
    }

}

function ReadedEvent() {
    ActiveElement.onchange();
}

function LocationByKey(KeyID, Page) {




    if (KeyID == 48 && url[0] != "") {
        location.href = url[0];
    }

    if (KeyID == 49 && url[1] != "") {
        location.href = url[1];
    }
    if (KeyID == 50 && url[2] != "") {
        location.href = url[2];
    }
    if (KeyID == 51 && url[3] != "") {
        location.href = url[3];
    }
    if (KeyID == 52 && url[4] != "") {
        location.href = url[4];
    }
    if (KeyID == 53 && url[5] != "") {
        location.href = url[5];
    }
    if (KeyID == 54 && url[6] != "") {
        location.href = url[6];
    }
    if (KeyID == 55 && url[7] != "") {
        location.href = url[7];
    }
    if (KeyID == 56 && url[8] != "") {
        location.href = url[8];
    }
    if (KeyID == 57 && url[9] != "") {
        location.href = url[9];
    }



}


function FKeyPressEvent(id) {
    var obj = document.getElementById(id);

    if (obj) {
        if (obj.tagName == "INPUT") {
            switch (obj.value) {
                case "back":
                    Ht.Back();
                    break;
                case "next":
                    Ht.Next();
                    break;
                case "end":
                    Ht.LogOff();
                    break;
                case "close":
                    Ht.Close();
                    break;
                case "search":
                    Ht.F2Click(); 
                    break;
            }
        }
        else if (obj.tagName == "FORM") {
            obj.submit();
        }
    }
}

function SetInputMode(value) {
    var actionValue = "";

    if (value != null) {
        var valueArray = value.split('_');

        actionValue = valueArray[2];
    }

    SetFKeyAction("f3Action", actionValue);
}

function SetFKeyAction(id, value) {
    var keyAct = document.getElementById(id);

    keyAct.value = value;

    InitFKeyGuide();
}

function GetFKeyAction(id) {
    var value = document.getElementById(id).value;

    return value;
}

function InitFKeyGuide() {
    var keyActF1 = document.getElementById("f1Action");
    var keyActF2 = document.getElementById("f2Action");
    var keyActF3 = document.getElementById("f3Action");

    document.getElementById("KeyF1").innerHTML = GetFKeyDesc(keyActF1.value);
    document.getElementById("KeyF2").innerHTML = GetFKeyDesc(keyActF2.value);
    document.getElementById("KeyF3").innerHTML = GetFKeyDesc(keyActF3.value);
}

function GetFKeyDesc(action) {
    if (action == null)
        action = "";

    switch (action) {
        case "back":
            return "戻る";
        case "next":
            return "次頁";
        case "end":
            return "終了";
        case "info":
            return "情報";
        case "search":            
            return "検索";
        case "close":
            return "戻る";
        case "f3Edit":
            return "編集";
        case "f3sft":
            return "文字";
        case "f3calc":
            return "情報";
        default:
            return "　";
            break;
    }
}