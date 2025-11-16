using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NpgsqlTypes;

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

                cls_PgsqlIf Pg = new cls_PgsqlIf();
                try
                {
                    if (!Pg.Connect()) throw new Exception("Oracle Connection Error");
                    string　sql = "SELECT SOKO_CD FROM TANTO WHERE TANTO_CD = :TANTO_CD";

                    Pg.setSqlCmd(sql, CommandType.Text);
                    Pg.addParam(ParameterDirection.Input, "TANTO_CD", NpgsqlDbType.Varchar, tantoEncode, 8);

                    SOKO = Pg.getDirectValue();
                }
                catch(Exception ex)
                {
                    ErrLog.logger.log(ex);
                }
                finally
                {
                    Pg.Disconnect();
                }
            }
        }
    }
}