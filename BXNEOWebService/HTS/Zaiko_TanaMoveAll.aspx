<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zaiko_TanaMoveAll.aspx.cs" Inherits="BXWebService.HTS.Zaiko_TanaMoveAll" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT棚移動</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";
</script>
</head>
<body>
<script src="./scripts/zaiko_tanaall.js" type="text/javascript"></script>
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
                    onchange = "javascript:MotoLocMoveAll();" />
          </dd>
          <dt>移動先</dt>
          <dd>
              <input type="text" 
                    id="SakiLocNo" 
                    class="numericentry" 
                    maxlength="15" 
                    
                    inputmode="keybcr_sftoff_f3sft" 
                    onblur = "javascript:SetFKeyAction('f2Action', '');" 
                    onfocus = "javascript:SetFKeyAction('f2Action', 'search');" 
                    onchange = "javascript:SakiLocNoChanged();" />
          </dd>
          <dt>SKU数/品質</dt>
          <dd>
            <input type="text"  
                   id="SkyCnt"  
                   class="qtyentry" 
                   maxlength="15" 
                   readonly="readonly"
                   inputmode="key_sftoff_f3disable"/><span class="vinculum">/</span><input  type="text" 
                    id="KozaCnt" 
                    class="qtyentry" 
                    maxlength="15" 
                    inputmode="key_sftoff_f3disable" 
                    readonly="readonly" />
           </dd>         
           <dt>総数/引当</dt>
          <dd>
            <input type="text"  
                   id="TotalCnt"  
                   class="qtyentry" 
                   maxlength="15" 
                   readonly="readonly"
                   inputmode="key_sftoff_f3disable"/><span class="vinculum">/</span><input  type="text" 
                    id="HkatCnt" 
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
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="javascript:MoveZaiko_TanaAll();" value="送信" />
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