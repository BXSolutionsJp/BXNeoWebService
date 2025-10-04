using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BXWebService.HTS
{
    public partial class Kako_serialCopy : System.Web.UI.Page
    {
        public string tantoEncode;        
        public string Select_PrinterLoc = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Request.QueryString["TANTO_CD"] == null)
            {
                Response.Redirect("login.aspx"); ;
            }
            else
            {
                tantoEncode = Request.QueryString["TANTO_CD"];
               

                cls_MsSqlIf cmd = new cls_MsSqlIf();
                try
                {
                    if (!cmd.Connect()) throw new Exception("DB Connection Error");

                    DataTable dt = new DataTable();
                    cmd.setSqlCmd("ht_sp_PrtLocation");
                    cmd.addParam(ParameterDirection.Input, "@TANTO_CD", SqlDbType.VarChar, tantoEncode, 15);                    
                    cmd.ExecuteCmd(ref dt);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i= 0; i< dt.Rows.Count; i++){
                            Select_PrinterLoc += "<option value=\"" + dt.Rows[i][0].ToString() + "\">" + dt.Rows[i][1].ToString() + "</option>";
                        }
                        
                    }


                }
                catch (Exception ex)
                {
                    ErrLog.logger.log(ex);                    
                }

                finally
                {
                    cmd.Disconnect();
                }


            }
        }

        
    }
}