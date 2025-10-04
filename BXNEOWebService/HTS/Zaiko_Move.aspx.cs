using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BXWebService.HTS
{
    public partial class Zaiko_Move : System.Web.UI.Page
    {
        public string tantoEncode;        
        public string Select_SOKO = string.Empty;
        public string KOZA_Selet = string.Empty;

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
                    if (!Ora.Connect()) throw new Exception("DB Connection Error");
                    SOKOSelect(ref Ora,ref tantoEncode);
                    setListBox(ref Ora);
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


        private void SOKOSelect(ref cls_OraIf Ora, ref string PlainTanto_CD)
        {
            string sql = "SELECT NNSSOK.SOKO_KANRI_CD, NNSSOK.NNS_SOKO_SHORT_NM, NVL(v.SOKO_KANRI_CD, 'XXXX') as LOGINSOKO " +
                            "FROM NNSSOK LEFT JOIN (  " +
                                "SELECT NVL(NNSSOK.SOKO_KANRI_CD, '0001') AS SOKO_KANRI_CD " +
                                "FROM NNSSOK INNER JOIN TANTO ON     TANTO.SOKO_CD = NNSSOK.NNS_SOKO_CD " +
                                "WHERE TANTO_CD = '" + PlainTanto_CD + "' " +
                            ") v ON NNSSOK.SOKO_KANRI_CD = v.SOKO_KANRI_CD " +
                            "ORDER BY NNSSOK.NNS_SOKO_CD ";

            //string sql = "SELECT SOKO_CD FROM TANTO WHERE TANTO_CD='"+ PlainTanto_CD+"'";

            DataTable li = new DataTable();
            Ora.setSqlCmd(sql, CommandType.Text);
            Ora.ExecuteCmd(ref li);

            for (int i = 0; i < li.Rows.Count; i++)
            {
                Select_SOKO += "<option value=\"" + li.Rows[i]["SOKO_KANRI_CD"].ToString() + "\"";
                if (li.Rows[i]["SOKO_KANRI_CD"].ToString() == li.Rows[i]["LOGINSOKO"].ToString())
                {
                    Select_SOKO += " selected";
                }
                Select_SOKO += ">" + li.Rows[i]["NNS_SOKO_SHORT_NM"].ToString() + "</option>" + Environment.NewLine;
            }

        }
        private void setListBox(ref cls_OraIf Ora, string Koza="0")
        {
            string sql = "SELECT KOZA, KOZA_NM FROM KOZA ORDER BY KOZA";
            DataTable li = new DataTable();
            Ora.setSqlCmd(sql, CommandType.Text, true);
            Ora.ExecuteCmd(ref li);

            for (int i = 0; i < li.Rows.Count; i++)
            {
                KOZA_Selet += "<option value=\"" + li.Rows[i]["KOZA"].ToString() + "\"";
                if (li.Rows[i]["KOZA"].ToString() == Koza)
                {
                    KOZA_Selet += " selected";
                }
                KOZA_Selet += ">" + li.Rows[i]["KOZA"] + ":" + li.Rows[i]["KOZA_NM"].ToString() + "</option>" + Environment.NewLine;
            }
        }
    }
}