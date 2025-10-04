
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kako.aspx.cs" Inherits="BXWebService.HTS.Kako" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT在庫管理</title>
<script type="text/javascript">
    UseKeyRedirect = true;
    var tantocd = "<%=tantoEncode%>";
    url[1] = "Kako_askLabel.aspx?TANTO_CD=" + tantocd;
    url[2] = "Kako_serialCopy.aspx?TANTO_CD="+tantocd;
    url[3] = "Kako_serialCompose.aspx?TANTO_CD=" + tantocd;
    url[4] = "";
    
    
   
</script>

</head>
<body>
    <div class="page">
        <div id="header">
            <div id="title">
                <h1>SLP</h1>
            </div>
            <div id="logindisplay">
                <input id="TantoCd" name="TantoCd" type="hidden" value="0010" /><input id="TantoGrp" name="TantoGrp" type="hidden" value="00001" />     <a href="/T1/Account/LogOff">ログオフ</a> 

            </div>
        </div>
        
<div id="main">
    <h2>4.加工業務</h2>
    <ul class="menu">
        <li>
        <a href="JavaScript:Redirect(1)">1.ASKラベル印刷</a>
        </li>

        <li>
        <a href="JavaScript:Redirect(2)">2.シリアルコピー</a>
        </li>

        <li>
        <a href="JavaScript:Redirect(3)">3.組込シリアル</a>
        </li>

        <!--<li>
        <a href="JavaScript:Redirect(4)">4.次へ</a>
        </li>-->

        

        
    </ul>
</div>
<input type="hidden" id="f1Action" value="back" />
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
<form action="login.aspx" id="LogOffForm" method="post"></form>        </div>
    </div>
</body>

</html>
