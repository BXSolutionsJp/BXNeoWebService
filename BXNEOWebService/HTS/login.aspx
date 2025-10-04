<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="BXWebService.HTS.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
<!-- #include file ="contents/main.html" -->   
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<script type="text/javascript">

function   ChkSubmit(f){
    if (f.getElementById("UserPass").value == "") {
        return false;
    } else {
        return true;
    }
}


       
    </script>
</head>
<body>
<div class="page">
        <div id="header">
            <div id="title">
                <h1>SLP</h1>
            </div>
            <div id="logindisplay">
                     <a href="/BxHt/login.aspx">ログオン</a> 

            </div>
        </div>
        

<script src="./Scripts/slp_funcs.js" type="text/javascript"></script>
<div id="main">
    <h2>ログオン</h2>

<form id="fm1" method="post" action="chkLogin.aspx" onsubmit="return ChkSubmit(this)">     <div class="block">
        <div class="editor-label">
            <label for="UserName">ユーザー名</label>
        </div>
        <div class="editor-field">
            <input class="textentry" data-val="true" data-val-required="ユーザー名 フィールドが必要です。" id="TANTO_CD" inputmode="key_sftoff_f3sft" maxlength="8" name="UserName" onchange="javascript:GetTantoNm();" tabindex="10" type="text" value="" />
            <div id="LoginName">
                &nbsp;
            </div>
        </div>
    </div>
    <div class="block">
        <div class="editor-label">
            <label for="Password">パスワード</label>
        </div>
        <div class="editor-field">
            <input class="passwordentry" data-val="true" data-val-required="パスワード フィールドが必要です。" id="UserPass" inputmode="key_sftoff_f3sft" maxlength="8" name="Password" onchange="javascript:PasswordChanged();" tabindex="5" type="password" />            
        </div>
    </div>
    <div class="block">
        <input id="LoginButton" class="submitbutton" type="submit" value="ログオン"  tabindex="30" disabled="true"/><!--onclick="return ValidateTanto();"-->
    </div>
</form></div>
<input type="hidden" id="f1Action" value="" />
<input type="hidden" id="f2Action" value="" />
<input type="hidden" id="f3Action" value="" />    
        <div id="footer">
            <ul class="fkeyguide">
                <li class="keyNum"><span>F1</span></li>
                <li id="KeyF1" class="keydesc">&nbsp;</li>
                <li class="keyNum"><span>F2</span></li>
                <li id="KeyF2" class="keydesc">&nbsp;</li>
                <li class="keyNum"><span>F3</span></li>
                <li id="KeyF3" class="keydesc">&nbsp;</li>
            </ul>
<form action="/HTTEST/Account/LogOff" id="LogOffForm" method="post"></form>        </div>
    </div>   
</body>
</html>

