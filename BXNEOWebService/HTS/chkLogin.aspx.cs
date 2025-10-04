using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BXWebService.HTS
{
    public partial class chkLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tc = Request.Form["UserName"];
            string ps = Request.Form["Password"];

            if (Login(ref tc, ref ps))
            {
                string url = "menu.aspx?TANTO_CD="+tc;
                Response.Redirect(url);
            } else
            {
                Response.Redirect("login.aspx");
            }

        }

        private Boolean Login(ref string tc, ref string ps)
        {
            cls_OraIf Ora = new cls_OraIf();
            Boolean f = false;

           


            string ret = string.Empty;
            //string sql = "BEGIN  SP_REG_KOFUKU(:pi_jan_cd,:pi_konpo_d,:pi_konpo_w,:pi_konpo_h,:pi_konpo_wght,:po_ret);  END;";
            string stad = "SP_CHKLOGIN";
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                Ora.removeSqlParam();
                Ora.setSqlCmd(stad, System.Data.CommandType.StoredProcedure);
                Ora.addParam(System.Data.ParameterDirection.Input, "pi_jan_cd", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, tc, 40);
                Ora.addParam(System.Data.ParameterDirection.Input, "pi_konpo_d", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ps, 20);
                Ora.addParam(System.Data.ParameterDirection.Output, "w_exst", Oracle.ManagedDataAccess.Client.OracleDbType.Int32);



                if (Ora.cmdExec())
                {
                    if (Ora.getParameter("w_exst") == "1")
                    {
                        f = true;
                    }
                    else
                    {
                        f = false;
                    }
                }


            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;

            }
            finally
            {
                Ora.Disconnect();
            }
            return f;
        }        

    }
}