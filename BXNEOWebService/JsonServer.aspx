<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsonServer.aspx.cs" Inherits="BXWebService.JsonServer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<%

    if (isJason)
    {
        Response.Write(JsonTxt);
    } 
    if(isDt)
    {
        string line = string.Empty;
        if(dt.Rows.Count>0)
        {
            for(int r = 0; r<dt.Rows.Count; r++)
            {
                for (int c = 0; c<dt.Columns.Count; c++)
                {
                    if (c > 0) line += ",";
                    line += dt.Rows[r][c].ToString();
                }
                Response.Write(line + "\n");
                line = string.Empty;
            }
        }
    }


%>
</body>
</html>
