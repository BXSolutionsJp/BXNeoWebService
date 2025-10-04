using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace BXWebService.HTS
{
    public partial class Zaiko_Haiki : System.Web.UI.Page
    {
        public string tantoEncode;
        public string SOKO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TANTO_CD"] == null)
            {
                Response.Redirect("login.aspx"); ;
            }
            else
            {
                tantoEncode = Request.QueryString["TANTO_CD"];

                cls_OraIf Ora = new cls_OraIf();
                try
                {
                    if (!Ora.Connect()) throw new Exception("Oracle Connection Error");
                    string　sql = "SELECT SOKO_CD FROM TANTO WHERE TANTO_CD = :TANTO_CD";

                    Ora.setSqlCmd(sql, CommandType.Text);
                    Ora.addParam(ParameterDirection.Input, "TANTO_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, tantoEncode, 8);

                    SOKO = Ora.getDirectValue();
                }
                catch(Exception ex)
                {
                    ErrLog.logger.log(ex);
                }
                finally
                {
                    Ora.Disconnect();
                }
            }
        }
    }
}