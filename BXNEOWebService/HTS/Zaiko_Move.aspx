<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zaiko_Move.aspx.cs" Inherits="BXWebService.HTS.Zaiko_Move" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT在庫管理</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";
</script>

</head>
<body>
    <script src="./scripts/zaiko_move.js" type="text/javascript"></script>
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
    <h2>ロケーション移動</h2>
   
    <div class="block">
        <dl class="dataitems">            
          <dt>倉庫</dt>
          <dd>

<select id="SOKO_KANRI_CD" name="SOKO_KANRI_CD">
<%=Select_SOKO%>
</select>
</dd>
          <dt>読取コード</dt>
          <dd>
              <input type="text" 
                    id="ShnCd" 
                    class="numericentry" 
                    maxlength="15" 
                    inputmode="keybcr_sftoff_f3sft" 
                    onchange = "javascript:ShnCdChanged();"/>
          </dd>
          <dt>商品コード</dt>
          <dd><input type="text" 
                    id="KykShnCd" 
                    class="numericentry" 
                    inputmode="key_sftoff_f3disable" 
                    readonly="readonly" /></dd>
          <dt>品質</dt>
          <dd>

<select id="KOZA" name="KOZA">
<%=KOZA_Selet%>
</select>
</dd>
          <dt>移動元</dt>
          <dd>
              <input type="text" 
                    id="MotLocNo" 
                    class="numericentry" 
                    maxlength="15" 
                    
                    inputmode="keybcr_sftoff_f3sft" 
                    onchange = "javascript:MotLocNoChanged();" />
          </dd>
          <dt>移動先</dt>
          <dd>
              <input type="text" 
                    id="SakiLocNo" 
                    class="numericentry" 
                    maxlength="15"                     
                    inputmode="keybcr_sftoff_f3sft"                     
                    onchange = "javascript:SakiLocNoChanged();" />
          </dd>
          <dt>数量</dt>
          <dd>
            <input type="text"  
                   id="Qty"  
                   class="qtyentry" 
                   maxlength="15" 
                   
                   inputmode="key_sftoff_f3disable" 
                   onchange = "javascript:QtyChanged();" /><span class="vinculum">/</span><input  type="text" 
                    id="DispQty" 
                    class="qtyentry" 
                    maxlength="15" 
                    inputmode="key_sftoff_f3disable" 
                    readonly="readonly" />
           </dd>
        </dl>
    </div>
    
    <input type="hidden" id="HkatFlg" value="1" />
    <input type="hidden" id="DictFlg" value="1" />
    <input type="hidden" id="GnpnShnCd" value="" />
    <input type="hidden" id="Unit" value="P" />
    <input type="hidden" id="ZkKanri" value="" />

    <div class="block">
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="javascript:MoveZaiko();" value="送信" />
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
