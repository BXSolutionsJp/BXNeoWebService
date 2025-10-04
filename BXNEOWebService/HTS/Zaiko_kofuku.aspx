<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zaiko_kofuku.aspx.cs" Inherits="BXWebService.HTS.Zaiko_kofuku" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!-- #include file ="contents/main.html" -->   
<title>BxHT入荷検品</title>
<script type="text/javascript">
    UseKeyRedirect = false;
    var tantocd = "<%=tantoEncode%>";
</script>
</head>
<body>
<script src="./scripts/zaiko_kofuku.js" type="text/javascript"></script>
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
    <h2>庫腹管理</h2>
   
    <div class="block">
        <dl class="dataitems">            
          <dt>読取コード</dt>
          <dd>
              <input type="text" 
                    id="KeyCode" 
                    class="numericentry" 
                    maxlength="15" 
                    inputmode="keybcr_sftoff_f3sft" 
                    onchange = "javascript:KeyCode_Changed();"/>
          </dd>
          <dt>JanCode</dt>
           <dd>
              <input type="text" 
                    id="JanCode" 
                    class="numericentry" 
                    maxlength="15" 
                    inputmode="keybcr_sftoff_f3sft" 
                    readonly="readonly"
                    disabled="true"/>
          </dd>             
            <hr />
          <dt>幅(mm)</dt>
          <dd><input type="text" 
                    id="konpo_w"                 
                    class="numericentry" 
                    onchange="javascript:konpo_w_change()"
                    maxlength="10" 
                    inputmode="key_sftoff_f3disable" 
                    onfocus="this.select()"
                    />
          </dd>
          <dt>奥行(mm)</dt>
          <dd>
              <input type="text" 
                    id="konpo_d" 
                    maxlength="10" 
                    class="numericentry" 
                    onchange="javascript:konpo_d_change()"                    
                    inputmode="keybcr_sftoff_f3sft" 
                    onfocus="this.select()"
                    />
          </dd>
          <dt>高さ(mm)</dt>
          <dd>
              <input type="text" 
                    id="konpo_h"
                    maxlength="10" 
                    class="numericentry" 
                    onchange="javascript:konpo_h_change()"                    
                    inputmode="keybcr_sftoff_f3sft"       
                    onfocus="this.select()"                                  
                    />
          </dd>
          <dt>重量(g)</dt>
          <dd>
            <input  type="text"  
                    id="konpo_wght"  
                    maxlength="10"    
                    class="numericentry"                                      
                   onchange = "javascript:konpo_wght_Changed();" 
                   inputmode="keybcr_sftoff_f3sft"                   
                    onfocus="this.select()"
                   />
           </dd>
        </dl>
    </div>
    
    <input type="hidden" id="HkatFlg" value="1" />
    <input type="hidden" id="DictFlg" value="1" />
    <input type="hidden" id="GnpnShnCd" value="" />
    <input type="hidden" id="Unit" value="P" />
    <input type="hidden" id="ZkKanri" value="" />

    <div class="block">
        <input id="SubmitButton" class="submitbutton" type="submit" onclick="javascript:Kofuku_Reg_Send();" value="送信" />
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