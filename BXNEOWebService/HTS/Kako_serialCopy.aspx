<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kako_serialCopy.aspx.cs" Inherits="BXWebService.HTS.Kako_serialCopy" %>


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
    <script src="./scripts/Kako_serialCopy.js" type="text/javascript"></script>
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
    <h2>シリアルコピー</h2>
   
    <div class="block">
        <dl class="dataitems">            
          <dt>印刷ロケ</dt>
          <dd><select id="PrtLocID" name="PrtLocID">
<%=Select_PrinterLoc%>
          </select></dd>
          <dt>枚数</dt>
          <dd>
            <input type="text"  
                   id="PrtCnt"  
                   class="qtyentry" 
                   maxlength="15"                    
                   inputmode="key_sftoff_f3disable" 
                    value="1"
                   />
           </dd>
          <dt>読取S/No</dt>
          <dd>
          </dd>          
          
        </dl>
    </div>
    <div class="block">
    <input style="width:100%;" id="SerialNo" inputmode="key_sftoff_f3sft" name="SerialNo" type="text" onchange="javascript:SerialCopyRegister()" />            
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
