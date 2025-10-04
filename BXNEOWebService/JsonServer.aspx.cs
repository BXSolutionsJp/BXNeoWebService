using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;

namespace BXWebService
{
    public partial class JsonServer : System.Web.UI.Page
    {
        public string JsonTxt;
        public DataTable dt;
        public Boolean isJason;
        public Boolean isDt;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cmd = Request.QueryString["command"];
            string key = Request.QueryString["key"];

            switch(cmd)
            {
                case "koza":
                    getKoza();
                    isJason = true;
                    isDt = false;
                    break;
                case "picking":
                    pickdata(key);
                    isJason = false;
                    isDt = true;
                    break;
            }
        }

        private void pickdata(string okurijo_no)
        {
            cls_OraIf Ora = new cls_OraIf();
            try
            {
                if (!Ora.Connect()) throw new Exception("Oracle Connection Error");
            
                string sql = @"
                            SELECT SGYUKETUKE.OKURIJO_NO,SGYUKETUKE.CHUMON_NO,
                                    RGSTRHIST.HSO_UKETUKE_NO, SGYSHIJI.KYK_SHN_CD, 
                                    SGYSHIJI.SHN_QTY as SHIJIQTY
                                    ,HKATSHIJI.QTY as HKATQTY
                                    ,LOC_NO_EDIT(HKATLOC.LOC_NO) as LOC_EDIT, HKATLOC.QTY as LOCQTY
                            FROM SGYUKETUKE
	                            , SGYSHIJI
	                            ,RGSTRHIST
	                            , HKATSHIJI
                            , HKATLOC

                            WHERE SGYUKETUKE.SGY_UKETUKE_NO	 = SGYSHIJI.SGY_UKETUKE_NO
	                            AND SGYUKETUKE.OKURIJO_NO	 = RGSTRHIST.BIKO
	                            AND RGSTRHIST.CHHYO_ID	 	 = 'PICKING'	
	                            AND SGYSHIJI.SGY_SHIJI_NO	 = HKATSHIJI.SGY_SHIJI_NO	
	                            AND HKATSHIJI.HKAT_DTL_NO	 = HKATLOC.HKAT_DTL_NO
	                            AND RGSTRHIST.HSO_UKETUKE_NO = (
			                            SELECT HSO_UKETUKE_NO FROM RGSTRHIST
			                            WHERE CHHYO_ID = 'PICKING' AND FUNC = 'MKPICKING'  AND BIKO =:OKURIJO_NO
	                            )	";

                Ora.setSqlCmd(sql, CommandType.Text);
                Ora.addParam(ParameterDirection.Input, "OKURIJO_NO", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, okurijo_no, 15);

                dt = new DataTable();
                dt = Ora.cmdExecDt();

                //if (dt.Rows.Count > 0)
                //{
                //    JsonTxt = JsonConvert.SerializeObject(dt);
                //}


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

        private void getKoza()
        {
            cls_OraIf Ora = new cls_OraIf();

            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                Ora.setSqlCmd("SELECT * FROM KOZA", System.Data.CommandType.Text);
                DataTable dt = Ora.cmdExecDt();
                JsonTxt = JsonConvert.SerializeObject(dt);



            }
            catch (Exception ex)
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