<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zaiko_Search.aspx.cs" Inherits="BXWebService.HTS.Nyka_Inspect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT在庫検索</title>
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
<script src="./scripts/zaiko_search.js" type="text/javascript"></script>
<div id="main">
    <h2>在庫検索</h2>

    <div class="block">
        <dl class="dataitems">
          <dt><label for="ShnCd">商品コード</label></dt>
          <dd class="clear">
            <input class="textentry" id="ShnCd" inputmode="keybcr_sftoff_f3sft" maxlength="15" name="ShnCd" onchange="javascript:GetZaikoList();" type="text" value="" />
              
          </dd>
        </dl> 
    </div>
    <div class="block">
		<dl class="dataitems">
          <dt>在庫リスト</dt>
		  <dd class="clear">
              	
			<dl id="ZaikoList" class="datagrid"><p class="note"></p></dl>
		  </dd>
        </dl>
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