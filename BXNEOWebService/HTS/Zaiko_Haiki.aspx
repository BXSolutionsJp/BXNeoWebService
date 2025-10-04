<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zaiko_Haiki.aspx.cs" Inherits="BXWebService.HTS.Zaiko_Haiki" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT廃棄出荷</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";
    var soko = "<%=SOKO%>"
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
<script src="./scripts/zaiko_haiki.js" type="text/javascript"></script>
<div id="main">
    <h2>廃棄出荷登録</h2>

    <div class="block">
        <dl class="dataitems">
          <dt><label for="ShnCd">商品コード</label></dt>
          <dd class="clear">
            <input class="textentry" id="ShnCd" inputmode="keybcr_sftoff_f3sft" maxlength="15" name="ShnCd" onchange="javascript:ShnCdChange();" type="text" value="" />              
          </dd>
          <dt><label for="ShnCd">数量</label></dt>
          <dd class="clear">
            <input type="text"  
                   id="qty"  
                   class="qtyentry" 
                   maxlength="15"                    
                   inputmode="key_sftoff_f3disable" 
                   onchange="javascript:qtychanged();"
                />&nbsp;&nbsp;<input id="sendButton" type="button" value="送信" onclick="javascript: sendRegHaiki()"/>
          </dd>
        </dl> 
    </div>
    <div class="block">
		<dl class="dataitems">
          <dt>登録リスト</dt>
		  <dd class="clear">
              	
			<dl id="HikiList" class="datagrid"><p class="note"></p></dl>
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
