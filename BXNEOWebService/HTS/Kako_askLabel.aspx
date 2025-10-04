<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kako_askLabel.aspx.cs" Inherits="BXWebService.HTS.Kako_askLabel" %>

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
    <div class="page">
        <div id="header">
            <div id="title">
                <h1>SLP</h1>
            </div>
            <div id="logindisplay">
            <a href="login.aspx">ログオフ</a> 
            </div>
        </div>
<script src="./scripts/Kako_askLabel.js" type="text/javascript"></script>
<div id="main">
    <h2>ASKラベル印刷</h2>

    <div class="block">
        <dl class="dataitems">
            <dt>印刷ロケ</dt>
          <dd><select id="PrtLocID" name="PrtLocID">
<%=Select_PrinterLoc%>
          </select></dd>
            <dt>種別</dt>
            <dd><select id="LabelType" name="LabelType">
                <option value="1">65面ラベル</option>
                <option value="2">30面ラベル</option>
                <option value="3">10面ラベル</option>
                <option value="4">10面大ラベル</option>
          </select></dd>
          <dt>枚数</dt><dd><input type="text" id="PrtCnt" class="qtyentry" maxlength="15" inputmode="key_sftoff_f3disable" value="100"/></dd>
          <dt><label for="ShnCd">商品コード</label></dt>
          <dd class="clear">
            <input class="textentry" id="ItemCode" inputmode="keybcr_sftoff_f3sft" maxlength="15" name="ItemCode" onchange="javascript:getItemInfo();" type="text" value="" />
          </dd>
        </dl> 
    </div>
    <div class="block">
		<dl class="dataitems">
          <dt>商品情報</dt>
		  <dd class="clear">              	
			<dl id="ItemInfo" class="datagrid"><p class="note"></p></dl>
		  </dd>
        </dl>
    </div>
    <div class="block">
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="javascript:AskLabelRegister();" value="送信" />
    </div>
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