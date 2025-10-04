<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nyka.aspx.cs" Inherits="BXWebService.HTS.Nyka" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT入荷業務</title>
<script type="text/javascript">
    UseKeyRedirect = true;
    var tantocd = "<%=tantoEncode%>";
    url[1] = "NyKa_Inspect.aspx?TANTO_CD=" + tantocd;
    url[2] = "";
    url[3] = "";
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
                <a href="login.aspx">ログオフ</a> 

            </div>
        </div>
        
<div id="main">
    <h2>1.入荷業務</h2>
    <ul class="menu">
        <li>
            <a href="JavaScript:Redirect(1)">1.入荷検品</a>
        </li>
        <li>
        <a href="JavaScript:history.back();">戻る</a>
        </li>
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