<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kako_serialCompose.aspx.cs" Inherits="BXWebService.HTS.Kako_serialCompose" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT加工業務</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";
</script>
</head>
<body>
    <script src="./scripts/Kako_serialCompose.js" type="text/javascript"></script>
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
    <h2>組込シリアル</h2>
   
    <div class="block">
        <dl class="dataitems">            
          <dt>印刷ロケ</dt>
          <dd><select id="PrtLocID" name="PrtLocID">
<%=Select_PrinterLoc%>
          </select></dd>
          
            <dt>商品コード</dt><dd><input type="text" id="ItemCode" style="width:6.5em;"  maxlength="8" inputmode="keybcr_sftoff_f3sft" onchange="CheckItemInfo(this.value)" /></dd>
            <dt>シリアルNo</dt><dd><input type="text" id="SerialNo" style="width:6.5em;"  maxlength="8" inputmode="keybcr_sftoff_f3sft" onChage="RegItemSerial()"/></dd>         
          
        </dl>
    </div>
    <div class="block">
		<dl class="dataitems">
          <dt>組込商品</dt>
		  <dd class="clear">              	
			<dl id="KumikomiInfo" class="datagrid"><p class="note"></p></dl>
		  </dd>
        </dl>
    </div>
    <div class="block">
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="javascript:SerialCopyRegister();" value="送信" />
    </div>
</div>
<input type="hidden" id="f1Action" value="back" />
<input type="hidden" id="f2Action" value="search" />
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
