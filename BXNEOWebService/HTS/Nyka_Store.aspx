<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nyka_Store.aspx.cs" Inherits="BXWebService.HTS.Nyka_Store" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
<!-- #include file ="contents/main.html" -->   
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>BxHT入荷格納</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";   

</script>
</head>
<body runat="server">    
  <div class="page">
    <div id="header">
      <div id="title">
        <h1>SLP</h1>
      </div>
      <div id="logindisplay">
        <a href="login.aspx">ログオフ</a> 
      </div>
    </div>
        
<!--<script type="text/javascript">
    //画面初期化
//    window.addEventListener("load", function () {		
//		document.getElementById("Koza").disabled = true;
//		GetStoreData();		
//    }, false);
</script>-->
    <script src="./scripts/nyka_store.js" type="text/javascript"></script>
    <div id="main">
      <h2>入庫完了</h2>
        <a href="../packages.config">../packages.config</a>
        <div class="block">
          <dl class="dataitems">
          <dt>作業指示番号</dt>
          <dd class="clear">
              <%=HKAT_SHIJI_NO%>
          </dd>
          </dl>
        </div>
        <div class="block">
          <dl class="dataitems">
          <dt><label for="ShnCd">商品コード</label></dt>
          <dd id="ShnCd"><%=KYK_SHN_CD%></dd>
                        
          <dt><label for="KozaList">品質</label></dt>
          <dd><select id="KOZA" name="KOZA" disabled="true">
              <%=KOZA_Selet%>
             </select>
          </dd>
          
          
          <dt><label for="LOC_NO">格納ロケ</label></dt>
          <dd><select id="LOC_NO" name="LOC_NO">
              <%=LOC_Select%>
             </select>
          </dd>

          <dt>予定数量</dt>
		  <dd id="QtyDisp"><%=QTY%></dd>
          <dt><label for="Qty">数量</label></dt>
          <dd><input class="numericentry" id="Qty" inputmode="key_sftoff_f3disable" maxlength="15" name="Qty" onchange="javascript:QtyChanged();" type="text" value="" />
           </dd>
          </dl>
        </div>

    <input type="hidden" id="HkatShijiNo" value="<%=HKAT_SHIJI_NO%>"/>    
    
    <input type="hidden" id="YoteiQty" value="<%=QTY%>" />

    <div class="block">
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="return EndLipStore();" value="送信" />
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
