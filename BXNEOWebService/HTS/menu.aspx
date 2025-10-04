<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="BXWebService.HTS.menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
<!-- #include file ="contents/main.html" -->   
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>BxHTメニュー</title>
<script type="text/javascript">
    UseKeyRedirect = true;
    var tantocd = "<%=tantoEncode%>";
    url[1] = "Nyka.aspx?TANTO_CD=" + tantocd;
    url[2] = "Shka.aspx?TANTO_CD=" + tantocd;
    url[3] = "Zaiko.aspx?TANTO_CD=" + tantocd;
    url[4] = "Kako.aspx?TANTO_CD=" + tantocd;    

</script>
</head>
<body>
    <div class="page">
        <div id="header">
            <div id="title">
                <h1>SLP</h1>
            </div>
            <div id="logindisplay">
                  <a href="login.aspx">ログオフ</a> 
            </div>
        </div>
        
<div id="main">
    <h2>メインメニュー</h2>
    <ul class="menu">
        <li>
        <a href="JavaScript:Redirect(1)">1.入荷業務</a>
        </li>

        <li>
        <a href="JavaScript:Redirect(2)">2.出荷業務</a>
        </li>

        <li>
        <a href="JavaScript:Redirect(3)">3.在庫管理</a>
        </li>

        <li>
        <a href="JavaScript:Redirect(4)">4.加工業務</a>
        </li>
    </ul>    
</div>
<input type="hidden" id="f1Action" value="end" />
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
<form action="login.aspxf" id="LogOffForm" method="post"></form>        </div>
    </div>
</body>
</html>
