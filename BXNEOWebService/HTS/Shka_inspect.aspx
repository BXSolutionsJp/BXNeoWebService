<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shka_inspect.aspx.cs" Inherits="BXWebService.HTS.Shka_inspect" %>
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
    <h2>出荷検品</h2>
   
    <div class="block">
        <dl class="dataitems">            
          <dt>伝票番号</dt>
          <dd>
              <input type="text" 
                    id="BillNo" 
                    class="textentry" 
                    maxlength="15" 
                    inputmode="keybcr_sftoff_f3sft" 
                    onchange = "javascript:BillNo_Changed();"/>
          </dd>
          <dt>出荷先</dt>
           <dd>
              <input type="text" 
                    id="ShipToCode" 
                    class="textentry" 
                    maxlength="15" 
                    inputmode="keybcr_sftoff_f3sft" 
                    readonly="readonly"
                    disabled="true"/>
          </dd>             
            <hr />
          <dt>商品コード</dt>
          <dd><input type="text" 
                    id="InspectCode"                 
                    class="textentry" 
                    onchange="javascript:konpo_w_change()"
                    maxlength="10" 
                    inputmode="key_sftoff_f3disable" 
                    onfocus="this.select()"
                    />
          </dd>
          <dt>シリアル</dt>
          <dd>
              <input type="text" 
                    id="konpo_d" 
                    maxlength="10" 
                    class="textentry" 
                    onchange="javascript:konpo_d_change()"                    
                    inputmode="keybcr_sftoff_f3sft" 
                    onfocus="this.select()"
                    />
          </dd>          
        </dl>
    </div>
    <div class="block">
		<dl class="dataitems">
          <dt>検品状況</dt>
		  <dd class="clear">              	
			<dl id="NyukoList" class="datagrid"><p class="note"></p></dl>
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