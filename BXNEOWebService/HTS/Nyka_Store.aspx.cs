using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BXWebService.HTS
{
    public partial class Nyka_Store : System.Web.UI.Page
    {
        public string tantoEncode;
        public string HKAT_SHIJI_NO;
        public string QTY;
        public string SUISHO_LOC;
        public string KYK_SHN_CD;
        public string ZK_KANRI;
        public string KOZA;
        public string SOKO_KANRI_CD;
        public string KOZA_Selet = string.Empty;
        public string LOC_Select = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (Request.QueryString["TANTO_CD"] == null)
            {
                Response.Redirect("login.aspx"); ;
            }
            else
            {
                tantoEncode = Request.QueryString["TANTO_CD"];
            }

            HKAT_SHIJI_NO = Request.QueryString["hkat_shiji_no"];


            string sql = "SELECT HKATLOC.LOC_NO,LOC_NO_EDIT(HKATLOC.LOC_NO) as LOCEDIT, HKATSHIJI.QTY, HKATSHIJI.ZK_KANRI, SGYSHIJI.KYK_SHN_CD,SGYSHIJI.KOZA, SGYUKETUKE.IRI_SOKO_KANRI_CD " +
                            "FROM HKATSHIJI, HKATLOC, SGYSHIJI, SGYUKETUKE, KOTEIPROC " +
                            "WHERE HKATSHIJI.HKAT_SHIJI_NO = :HKAT_SHIJI_NO " +
                                "AND HKATSHIJI.HKAT_DTL_NO = HKATLOC.HKAT_DTL_NO " +
                                "AND HKATSHIJI.SGY_SHIJI_NO = SGYSHIJI.SGY_SHIJI_NO " +
                                "AND SGYSHIJI.SGY_UKETUKE_NO = SGYUKETUKE.SGY_UKETUKE_NO "+
                                "AND SGYSHIJI.SGY_KOTE_CD = KOTEIPROC.SGY_KOTE_CD " +
                                "AND KOTEIPROC.PROC IN('ENDSTORE') " +
                                "AND(HKATSHIJI.STS BETWEEN   KOTEIPROC.MIN_STS AND KOTEIPROC.MAX_STS) " +
                                "AND KOTEIPROC.PROC_KBN = 'H' ";
                               
            cls_OraIf Ora = new cls_OraIf();
            DataTable dt = new DataTable();

            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                Ora.removeSqlParam();
                Ora.setSqlCmd(sql, CommandType.Text);
                Ora.addParam(ParameterDirection.Input, "HKAT_SHIJI_NO", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, HKAT_SHIJI_NO, 15);
                
                dt = Ora.cmdExecDt();
                if (dt.Rows.Count > 0)
                {
                    QTY = dt.Rows[0]["QTY"].ToString();
                    SUISHO_LOC = dt.Rows[0]["LOCEDIT"].ToString();
                    KYK_SHN_CD = dt.Rows[0]["KYK_SHN_CD"].ToString();
                    ZK_KANRI = dt.Rows[0]["ZK_KANRI"].ToString();
                    KOZA = dt.Rows[0]["KOZA"].ToString();
                    setListBox(ref Ora, KOZA);
                    LocListBox(ref Ora, KOZA, dt.Rows[0]["IRI_SOKO_KANRI_CD"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Ora.Disconnect();
            }

        }
        private void setListBox(ref cls_OraIf Ora,  string Koza)
        {
            string sql = "SELECT KOZA, KOZA_NM FROM KOZA ORDER BY KOZA";
            DataTable li = new DataTable();
            Ora.setSqlCmd(sql, CommandType.Text);
            Ora.ExecuteCmd(ref li);
            
            for (int i= 0; i<li.Rows.Count; i++)
            {
                KOZA_Selet += "<option value=\"" + li.Rows[i]["KOZA"].ToString() +"\"";
                if (li.Rows[i]["KOZA"].ToString() == Koza)
                {
                    KOZA_Selet += " selected";
                }
                KOZA_Selet += ">" + li.Rows[i]["KOZA"] + ":" + li.Rows[i]["KOZA_NM"].ToString() + "</option>"+Environment.NewLine;
            }
        } 
        private void LocListBox(ref cls_OraIf Ora, string Koza, string SOKO_KANRI_CD)
        {
            string sql = "SELECT KOZA, LOC_NO, KOZA||':'||LOC_NO_EDIT(LOC_NO) as LOCEDIT, SOKO_KANRI_CD " +
                        "FROM( " +
                        "    SELECT distinct SOKO_KANRI_CD, KOZA, LOC_NO " +
                        "    FROM NEWSHNSTRLOC  " +
                        "    WHERE NYSHK_KBN IN('1', '3')  " +
                        ") v " +
                        "ORDER BY SOKO_KANRI_CD, KOZA";
            DataTable li = new DataTable();
            Ora.setSqlCmd(sql, CommandType.Text, true);
            Ora.ExecuteCmd(ref li);

            for (int i = 0; i < li.Rows.Count; i++)
            {
                LOC_Select += "<option value=\"" + li.Rows[i]["LOC_NO"].ToString() + "\"";
                if (li.Rows[i]["KOZA"].ToString() == Koza && li.Rows[i]["SOKO_KANRI_CD"].ToString() == SOKO_KANRI_CD)
                {
                    LOC_Select += " selected";
                }
                LOC_Select += ">" + li.Rows[i]["LOCEDIT"].ToString() +"</option>"+ Environment.NewLine;
            }




        }
    }
}