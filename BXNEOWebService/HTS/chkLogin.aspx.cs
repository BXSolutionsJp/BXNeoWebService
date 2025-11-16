using NpgsqlTypes;
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
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            Boolean f = false;


            string ret = string.Empty;
            //string sql = "BEGIN  SP_REG_KOFUKU(:pi_jan_cd,:pi_konpo_d,:pi_konpo_w,:pi_konpo_h,:pi_konpo_wght,:po_ret);  END;";
            string stad = "SP_CHKLOGIN";
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                    
                }

                Pg.removeSqlParam();
                Pg.setSqlCmd(stad, System.Data.CommandType.StoredProcedure);
                Pg.addParam(System.Data.ParameterDirection.Input, "pi_jan_cd", NpgsqlDbType.Varchar, tc, 40);
                Pg.addParam(System.Data.ParameterDirection.Input, "pi_konpo_d", NpgsqlDbType.Varchar, ps, 20);
                Pg.addParam(System.Data.ParameterDirection.Output, "w_exst", NpgsqlDbType.Integer);



                if (Pg.cmdExec())
                {
                    if (Pg.getParameter("w_exst") == "1")
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
                Pg.Disconnect();
            }
            return f;
        }        

    }
}