using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace BXWebService
{
    /// <summary>
    /// BXSoap の概要の説明です
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    // [System.Web.Script.Services.ScriptService]
    public class BXSoap : System.Web.Services.WebService
    {
        public struct GENERAL{
            public string VAL_1;
            public string VAL_2;
            public string VAL_3;
            public string VAL_4;
            public string VAL_5;
            public string VAL_6;
        }
        public struct HKATInfo
        {
            public string HKAT_SHIJI_NO;
            public string OKURIJO_NO;
            public string NYSHK_DATE;
            public string QTY;
        }
        public struct MoveItem
        {
            public string SOKO_KANRI_CD;
            public string KYK_SHN_CD;
            public string GNPN_SHN_CD;
            public string SHN_NM;
            public string KOZA;
            public string ZK_KANRI;
            public string TANA_ZK_QTY;
        }
        public struct KofukuInfo
        {
            public string KYK_SHN_CD;
            public string JAN_CD;
            public string KONPO_W;
            public string KONPO_D;
            public string KONPO_H;
            public string KONPO_WGHT;
        }
        public struct MoveFIFO
        {
            public string RTN;
            public string RTN_QTY;
            public string KYK_SHN_CD;
            public string MOT_LOC;
            public string SAKI_LOC;
        }        
        [WebMethod]
        public string RegSeraialCopy(string PrtLocID, string PrtCnt, string SerialNo)
        {

            cls_MsSqlIf cmd = new cls_MsSqlIf();
            Boolean f = false;
            cmd.Connect();

            f = cmd.setSqlCmd("HT_sp_RegSerialCopy");
            f = cmd.addParam(ParameterDirection.Input, "@PrtLocID", SqlDbType.Int, PrtLocID);
            f = cmd.addParam(ParameterDirection.Input, "@PrtCnt", SqlDbType.Int, PrtCnt);
            f = cmd.addParam(ParameterDirection.Input, "@SerialNo", SqlDbType.VarChar, SerialNo, 255);
            f = cmd.addParam(ParameterDirection.Output, "@RET", SqlDbType.Int);

            f = cmd.cmdExec();

            return cmd.getParameter("@RET");
            
        }

        [WebMethod]
        public string RegASKLabel(string ItemCode,  string PrtLocID, string PrtCnt, string LabelType)
        {
            

            cls_MsSqlIf cmd = new cls_MsSqlIf();
            Boolean f = false;
            cmd.Connect();

            f = cmd.setSqlCmd("ht_sp_RegASKLabel");
            
            f = cmd.addParam(ParameterDirection.Input, "@ItemCode", SqlDbType.VarChar, ItemCode, 22);
            f = cmd.addParam(ParameterDirection.Input, "@PrtLocID", SqlDbType.Int, PrtLocID);
            f = cmd.addParam(ParameterDirection.Input, "@PrtCnt", SqlDbType.Int, PrtCnt);
            f = cmd.addParam(ParameterDirection.Input, "@LabelType", SqlDbType.Int, LabelType);
            f = cmd.addParam(ParameterDirection.Output, "@RET", SqlDbType.Int);

            f = cmd.cmdExec();

            return cmd.getParameter("@RET");

        }

        [WebMethod]
        public GENERAL[] REG_HAIKI(string SOKO, string ItemCode, string JAN_CD, string QTY)
        {
            GENERAL[] GN = new GENERAL[0];

           
           

            cls_MsSqlIf clsCmd = new cls_MsSqlIf();

            try
            {
                if (!clsCmd.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                clsCmd.removeSqlParam();
                clsCmd.setSqlCmd("sp_Reg_HaikiShip");
                clsCmd.addParam(ParameterDirection.Input, "@SOKO", SqlDbType.NVarChar, SOKO, 3);
                clsCmd.addParam(ParameterDirection.Input, "@ItemCode", SqlDbType.NVarChar, ItemCode, 20);
                clsCmd.addParam(ParameterDirection.Input, "@JanCode", SqlDbType.NVarChar, JAN_CD, 13);
                clsCmd.addParam(ParameterDirection.Input, "@QTY", SqlDbType.Int, QTY);


                DataTable dt = new DataTable();
                dt = clsCmd.cmdExecDt();

                if (dt.Rows.Count > 0)
                {
                    Array.Resize(ref GN, dt.Rows.Count);

                    for (int i = 0; i<dt.Rows.Count; i++)
                    {
                        GN[i].VAL_1 = dt.Rows[i]["ItemCode"].ToString();
                        GN[i].VAL_2 = dt.Rows[i]["JanCode"].ToString();
                        GN[i].VAL_3 = dt.Rows[i]["QTY"].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                clsCmd.Disconnect(); 
            }

            return GN;


        }
        [WebMethod]
        public string isLocationExists(string SOKO_KANRI_CD, string LOC_EDIT)
        {
            string  ret=string.Empty;
            string sql = @"SELECT CAST(COUNT(*) as  varchar2(10)) as CNT
                            FROM BLKAREA, AREA, NNSSOK, LOCATION
                            WHERE BLKAREA.AREA_NO = AREA.AREA_NO
                                AND AREA.SOKO_CD = NNSSOK.NNS_SOKO_CD 
                                AND SOKO_KANRI_CD = :SOKO_KANRI_CD
                                AND LOCATION.LOC_NO	BETWEEN FROM_LOC_NO AND TO_LOC_NO
                                AND LOCATION.LOC_NO   = LOC_NO_EDIT_ADD(:LOC_EDIT)";

            cls_OraIf Ora = new cls_OraIf();
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                Ora.removeSqlParam();
                Ora.setSqlCmd(sql, CommandType.Text);
                Ora.addParam(ParameterDirection.Input, "SOKO_KANRI_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, SOKO_KANRI_CD, 8);
                Ora.addParam(ParameterDirection.Input, "LOC_EDIT", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, LOC_EDIT, 15);

                ret = Ora.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Ora.Disconnect();
            }

            return ret;
        }
        [WebMethod]
        public KofukuInfo getKofukuInfo(string KEY)
        {

            KofukuInfo ki = new KofukuInfo();
            

            
            cls_OraIf Ora = new cls_OraIf();
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                Ora.removeSqlParam();
                Ora.setSqlCmd("SP_GET_KOFUKU", CommandType.StoredProcedure);
                Ora.addParam(ParameterDirection.Input, "KEY", OracleDbType.Varchar2, KEY, 13);
                Ora.addParam(ParameterDirection.Output, "po_kyk_shn_cd", OracleDbType.Varchar2, "", 20);
                Ora.addParam(ParameterDirection.Output, "po_jan_cd", OracleDbType.Varchar2, "", 13);
                Ora.addParam(ParameterDirection.Output, "po_konpo_d", OracleDbType.Int16);
                Ora.addParam(ParameterDirection.Output, "po_konpo_w", OracleDbType.Int16);
                Ora.addParam(ParameterDirection.Output, "po_konpo_h", OracleDbType.Int16);
                Ora.addParam(ParameterDirection.Output, "po_konpo_wght", OracleDbType.Int16);

                Ora.cmdExec();

                ki.KYK_SHN_CD = Ora.getParameter("po_kyk_shn_cd");
                ki.JAN_CD = Ora.getParameter("po_jan_cd");
                ki.KONPO_W = Ora.getParameter("po_konpo_w");
                ki.KONPO_D = Ora.getParameter("po_konpo_d");
                ki.KONPO_H = Ora.getParameter("po_konpo_h");
                ki.KONPO_WGHT = Ora.getParameter("po_konpo_wght");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Ora.Disconnect();
            }
            return ki;
        }
        [WebMethod]
        public MoveItem GetMoveData(string SOKO_KANRI_CD, string SHN_CD, string LOC_NO, string KOZA)
       {
            MoveItem list = new MoveItem();
            
            string sql = @"SELECT NNSSOK.SOKO_KANRI_CD AS SOKO_KANRI_CD,
                            SHN.KYK_SHN_CD AS KYK_SHN_CD,                            
                            GNPNZAIKO.GNPN_SHN_CD AS GNPN_SHN_CD,
                            SHN.SHN_NM AS SHN_NM,
                            GNPNZAIKO.KOZA AS KOZA,
                            SUM(TANA_ZK_QTY) as TANA_ZK_QTY
                        
                        FROM NNSSOK,SHN,GNPNZAIKO   
                        WHERE (SHN.KYK_SHN_CD = :kyk_shn_cd) AND (GNPNZAIKO.SHN_CD =SHN.SHN_CD) AND (NNSSOK.NNS_SOKO_SEQ = SUBSTR(GNPNZAIKO.GNPN_SHN_CD,4,3)) AND NNSSOK.SOKO_KANRI_CD = :soko_kanri_cd
                            AND LOC_NO = LOC_NO_EDIT_ADD(:loc_no) AND KOZA LIKE :koza
                        GROUP BY NNSSOK.SOKO_KANRI_CD,SHN.KYK_SHN_CD,GNPNZAIKO.GNPN_SHN_CD,SHN.SHN_NM,GNPNZAIKO.KOZA  ";

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
                Ora.addParam(ParameterDirection.Input, "kyk_shn_cd", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, SHN_CD, 13);
                Ora.addParam(ParameterDirection.Input, "soko_kanri_cd", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, SOKO_KANRI_CD, 13);
                Ora.addParam(ParameterDirection.Input, "loc_no", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, LOC_NO, 13);
                Ora.addParam(ParameterDirection.Input, "koza", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, KOZA, 13);

                dt = Ora.cmdExecDt();
                if (dt.Rows.Count > 0)
                {
                    
                    list.SOKO_KANRI_CD = dt.Rows[0]["SOKO_KANRI_CD"].ToString();
                    list.KYK_SHN_CD = dt.Rows[0]["KYK_SHN_CD"].ToString();
                    list.GNPN_SHN_CD = dt.Rows[0]["GNPN_SHN_CD"].ToString();
                    list.SHN_NM = dt.Rows[0]["SHN_NM"].ToString();
                    list.KOZA = dt.Rows[0]["KOZA"].ToString();
                    list.ZK_KANRI = "";
                    list.TANA_ZK_QTY = dt.Rows[0]["TANA_ZK_QTY"].ToString();
                    
                }
            }
            catch (OracleException ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Ora.Disconnect();
            }
            return list;
        }
        [WebMethod]
        public GENERAL GetMoveData_Tana(string SOKO_KANRI_CD, string LOC_NO)
        {

            GENERAL gn = new GENERAL();

            cls_OraIf Ora = new cls_OraIf();
            DataTable dt = new DataTable();

            try
            {
                string stad = "SP_GET_TANA_INFO";                
               
                    if (!Ora.Connect())
                    {
                        throw new Exception("Oracle Connection Error");
                    }

                    Ora.removeSqlParam();
                    Ora.setSqlCmd(stad, CommandType.StoredProcedure);
                    Ora.addParam(ParameterDirection.Input, "p_soko_kanri_cd", OracleDbType.Varchar2, SOKO_KANRI_CD, 4);
                    Ora.addParam(ParameterDirection.Input, "p_loc_no", OracleDbType.Varchar2, LOC_NO, 15);
                    Ora.addParam(ParameterDirection.Output, "p_sku_cnt", OracleDbType.Int32);
                    Ora.addParam(ParameterDirection.Output, "p_koza_cnt", OracleDbType.Int32);
                    Ora.addParam(ParameterDirection.Output, "p_total_cnt", OracleDbType.Int32);
                    Ora.addParam(ParameterDirection.Output, "p_hkat_cnt", OracleDbType.Int32);
                if (Ora.cmdExec())
                {
                    gn.VAL_1 = Ora.getParameter("p_sku_cnt").ToString();
                    gn.VAL_2 = Ora.getParameter("p_koza_cnt").ToString();
                    gn.VAL_3 = Ora.getParameter("p_total_cnt").ToString();
                    gn.VAL_4 = Ora.getParameter("p_hkat_cnt").ToString();
                }
               
                
            }
            catch (OracleException ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Ora.Disconnect();
            }

            return gn;
        }
        [WebMethod]
        public HKATInfo[] GetNyukoList(string shnCd)
        {

            HKATInfo[] list = new HKATInfo[0];
            string sql = "SELECT HKATSHIJI.HKAT_SHIJI_NO,SGYUKETUKE.OKURIJO_NO,TO_CHAR(SGYUKETUKE.NYSHK_DATE, 'MM/DD') as NYSHK_DATE,HKATSHIJI.QTY " +
                          "FROM SGYUKETUKE,HKATSHIJI,SGYSHIJI,KOTEIPROC,HKATLOC,SHN " +
                           "WHERE SGYUKETUKE.SGY_UKETUKE_NO = SGYSHIJI.SGY_UKETUKE_NO " +
                                "AND SGYSHIJI.SHN_CD = SHN.SHN_CD " +
                                "AND SGYSHIJI.SGY_KOTE_CD = KOTEIPROC.SGY_KOTE_CD " +
                                "AND HKATSHIJI.HKAT_DTL_NO = HKATLOC.HKAT_DTL_NO " +
                                "AND KOTEIPROC.PROC IN('ENDSTORE') " +
                                "AND KOTEIPROC.PROC_KBN = 'H' " +
                                "AND ( HKATSHIJI.STS BETWEEN   KOTEIPROC.MIN_STS AND KOTEIPROC.MAX_STS ) " +
                                "AND HKATSHIJI.SGY_SHIJI_NO = SGYSHIJI.SGY_SHIJI_NO " +
                                "AND (SHN.KYK_SHN_CD LIKE '" + shnCd + "%' OR  SHN.JAN_CD = '" + shnCd + "')";
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
                //Ora.addParam(ParameterDirection.Input, "kyk_shn_cd", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, shnCd, 13);


                dt = Ora.cmdExecDt();
                if (dt.Rows.Count > 0)
                {
                    Array.Resize(ref list, dt.Rows.Count);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list[i].HKAT_SHIJI_NO = dt.Rows[i]["HKAT_SHIJI_NO"].ToString();
                        list[i].OKURIJO_NO = dt.Rows[i]["OKURIJO_NO"].ToString();
                        list[i].NYSHK_DATE = dt.Rows[i]["NYSHK_DATE"].ToString();
                        list[i].QTY = dt.Rows[i]["QTY"].ToString();
                    }
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

            return list;
        }
        [WebMethod]
        public string getSLPTanToName(string TANTO_CD)
        {

            string sql = "SELECT TANTO_NM FROM TANTO WHERE TANTO_CD=:TANTO_CD";
            cls_OraIf Ora = new cls_OraIf();
            DataTable dt = new DataTable();
            string TANTO_NM = string.Empty;
            try
            {


                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                Ora.removeSqlParam();
                Ora.setSqlCmd(sql, CommandType.Text);
                Ora.addParam(ParameterDirection.Input, "TANTO_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, TANTO_CD, 13);


                TANTO_NM = Ora.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Ora.Disconnect();
            }

            return TANTO_NM;
        }
        [WebMethod]
        public string getTantoNmStored(string TANTO_CD)
        {
            string tanto_nm = string.Empty;
            string stad = "SP_GET_TANTO_NM";
            cls_OraIf Ora = new cls_OraIf();
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                Ora.removeSqlParam();
                Ora.setSqlCmd(stad, CommandType.StoredProcedure);
                Ora.addParam(ParameterDirection.Input, "pi_tanto_cd", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, TANTO_CD, 15);
                Ora.addParam(ParameterDirection.Output, "po_tanto_nm", OracleDbType.Varchar2, "", 15);
                if (Ora.cmdExec())
                {
                    tanto_nm = Ora.getParameter("po_tanto_nm");
                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Ora.Disconnect();
            }
            return tanto_nm;
        }
        [WebMethod]
        public string LipStoreEnd(string HKAT_SHIJI_NO, string QTY, string LOC_NO, string TANTO_CD, string LIP_NO = "", string LOT_NO = "", string RSN_CD = "00")
        {
            string TANTO = string.Empty;
            if (TANTO_CD.Length <= 8)
            {
                TANTO = TANTO_CD;
            }
            else
            {
                TANTO = cls_Func.DecryptString(TANTO_CD, System.DateTime.Now.ToString("yyyyMMdd"));
            }

            if (LOT_NO == "")
            {
                LOT_NO = System.DateTime.Now.ToString("yyyyMMdd");
            }
            cls_OraIf OraDb = new cls_OraIf();
            Boolean f = false;
            string ret = string.Empty;

            try
            {
                if (!OraDb.Connect()) throw new System.Exception("Oracle Connection Error");
                f = OraDb.setSqlCmd("pack_lip.proc_LipStore_End", CommandType.StoredProcedure);
                f = OraDb.addParam(ParameterDirection.Input, "p_hkat_shiji_no", OracleDbType.Varchar2, HKAT_SHIJI_NO, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_qty", OracleDbType.Decimal, QTY);
                f = OraDb.addParam(ParameterDirection.Input, "p_loc_no", OracleDbType.Varchar2, LOC_NO, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_lip_no", OracleDbType.Varchar2, LIP_NO, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_lot_no", OracleDbType.Varchar2, LOT_NO, 64);
                f = OraDb.addParam(ParameterDirection.Input, "p_rsn_cd", OracleDbType.Varchar2, RSN_CD, 6);
                f = OraDb.addParam(ParameterDirection.Input, "p_upd_tanto_cd", OracleDbType.Varchar2, TANTO, 8);
                f = OraDb.addParam(ParameterDirection.Output, "p_rc", OracleDbType.Varchar2, "", 2);
                f = OraDb.cmdExec();
                ret = OraDb.getParameter("p_rc").ToString().Trim();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                OraDb.Disconnect();
            }
            return ret;
        }        
        [WebMethod]
        public MoveFIFO MoveZaikoFIFO(string GNPN_SHN_CD,string KYK_SHN_CD,  string KOZA, string MOTO_LOC, string SAKI_LOC, string QTY, string TANTO_CD)
        {
            MoveFIFO Ret = new MoveFIFO();
            string TANTO = string.Empty;
            string MOT_LOC_ADD = string.Empty;
            string SAKI_LOC_ADD = string.Empty;
            

            cls_OraIf OraDb = new cls_OraIf();
            Boolean f = false;
            string ret = string.Empty;

            

            try
            {
                if (!OraDb.Connect()) throw new System.Exception("Oracle Connection Error");
                MOT_LOC_ADD = LOC_NO_EDIT_ADD(ref OraDb, MOTO_LOC);
                SAKI_LOC_ADD = LOC_NO_EDIT_ADD(ref OraDb, SAKI_LOC);
                f = OraDb.setSqlCmd("SP_FIFO_IDO", CommandType.StoredProcedure, true);
                f = OraDb.addParam(ParameterDirection.Input, "p_gnpn_shn_cd", OracleDbType.Varchar2, GNPN_SHN_CD, 36);
                f = OraDb.addParam(ParameterDirection.Input, "p_koza", OracleDbType.Varchar2, KOZA, 4);
                f = OraDb.addParam(ParameterDirection.Input, "p_unit", OracleDbType.Char, "P", 1);
                f = OraDb.addParam(ParameterDirection.Input, "p_motoloc_no", OracleDbType.Varchar2, MOT_LOC_ADD, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_sakiloc_no", OracleDbType.Varchar2, SAKI_LOC_ADD, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_qty", OracleDbType.Decimal, QTY);
                f = OraDb.addParam(ParameterDirection.Input, "p_hkat_flg", OracleDbType.Varchar2, "0",3);
                f = OraDb.addParam(ParameterDirection.Input, "p_dict_flg", OracleDbType.Varchar2, "0", 3);
                f = OraDb.addParam(ParameterDirection.Input, "p_tanto_cd", OracleDbType.Varchar2, TANTO_CD, 8);
                f = OraDb.addParam(ParameterDirection.Output, "p_rtn", OracleDbType.Varchar2, "", 15);
                f = OraDb.addParam(ParameterDirection.Output, "p_rtn_qty", OracleDbType.Decimal);
                f = OraDb.cmdExec();

                Ret.RTN = OraDb.getParameter("p_rtn").ToString().Trim();
                Ret.RTN_QTY = OraDb.getParameter("p_rtn_qty").ToString().Trim();
                Ret.KYK_SHN_CD = KYK_SHN_CD;
                Ret.MOT_LOC = MOTO_LOC;
                Ret.SAKI_LOC = SAKI_LOC;
                
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                OraDb.Disconnect();
            }


            return Ret;

        }
        [WebMethod]
        public MoveFIFO MoveZaikoTanaAll(string SOKO_KANRI_CD, string KOZA, string MOTO_LOC, string SAKI_LOC, string TANTO_CD)
        {
            MoveFIFO Ret = new MoveFIFO();
            string TANTO = string.Empty;
            string MOT_LOC_ADD = string.Empty;
            string SAKI_LOC_ADD = string.Empty;
            

            cls_OraIf OraDb = new cls_OraIf();
            Boolean f = false;
            string ret = string.Empty;



            try
            {
                if (!OraDb.Connect()) throw new System.Exception("Oracle Connection Error");
                MOT_LOC_ADD = LOC_NO_EDIT_ADD(ref OraDb, MOTO_LOC);
                SAKI_LOC_ADD = LOC_NO_EDIT_ADD(ref OraDb, SAKI_LOC);
                f = OraDb.setSqlCmd("SP_IDO_TANA_ALL", CommandType.StoredProcedure, true);
                f = OraDb.addParam(ParameterDirection.Input, "p_soko_kanri_cd", OracleDbType.Varchar2, SOKO_KANRI_CD, 8);
                f = OraDb.addParam(ParameterDirection.Input, "p_koza", OracleDbType.Varchar2, KOZA, 4);                
                f = OraDb.addParam(ParameterDirection.Input, "p_motoloc_no", OracleDbType.Varchar2, MOT_LOC_ADD, 15);
                f = OraDb.addParam(ParameterDirection.Input, "p_sakiloc_no", OracleDbType.Varchar2, SAKI_LOC_ADD, 15);                
                f = OraDb.addParam(ParameterDirection.Output, "p_cnt", OracleDbType.Decimal);
                f = OraDb.cmdExec();

                Ret.RTN = "";
                Ret.RTN_QTY = OraDb.getParameter("p_cnt").ToString().Trim();
                Ret.KYK_SHN_CD = "";
                Ret.MOT_LOC = MOTO_LOC;
                Ret.SAKI_LOC = SAKI_LOC;

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                OraDb.Disconnect();
            }


            return Ret;

        }
        [WebMethod]
        public string RegKofukuInfo(string KYK_SHN_CD, string KONPO_W, string KONPO_D, string KONPO_H, string KONPO_WGHT)
        {
            string ret = string.Empty;
            //string sql = "BEGIN  SP_REG_KOFUKU(:pi_jan_cd,:pi_konpo_d,:pi_konpo_w,:pi_konpo_h,:pi_konpo_wght,:po_ret);  END;";
            string stad = "SP_REG_KOFUKU";
            cls_OraIf Ora = new cls_OraIf();

            cls_MsSqlIf SqlDb = new cls_MsSqlIf();
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                Ora.removeSqlParam();
                Ora.setSqlCmd(stad, CommandType.StoredProcedure);
                Ora.addParam(ParameterDirection.Input, "pi_kyk_shn_cd", OracleDbType.Varchar2, KYK_SHN_CD, 13);
                Ora.addParam(ParameterDirection.Input, "pi_konpo_d", OracleDbType.Int32, KONPO_D);
                Ora.addParam(ParameterDirection.Input, "pi_konpo_w", OracleDbType.Int32, KONPO_W);                
                Ora.addParam(ParameterDirection.Input, "pi_konpo_h", OracleDbType.Int32, KONPO_H);
                Ora.addParam(ParameterDirection.Input, "pi_konpo_wght", OracleDbType.Int32, KONPO_WGHT);
                Ora.addParam(ParameterDirection.Output, "po_ret", OracleDbType.Int32);
                if (Ora.cmdExec())
                {
                    ret = Ora.getParameter("po_ret");
                }

                if (!SqlDb.Connect())
                {
                    throw new Exception("MSSQL Connection Error");
                }

                SqlDb.removeSqlParam();
                SqlDb.setSqlCmd("sp_SOAP_UpdateItemKofuku");
                SqlDb.addParam(ParameterDirection.Input, "@ItemCode", SqlDbType.VarChar, KYK_SHN_CD, 20);
                SqlDb.addParam(ParameterDirection.Input, "@KONPO_W", SqlDbType.Int, KONPO_W);
                SqlDb.addParam(ParameterDirection.Input, "@KONPO_D", SqlDbType.Int, KONPO_D);
                SqlDb.addParam(ParameterDirection.Input, "@KONPO_H", SqlDbType.Int, KONPO_H);
                SqlDb.addParam(ParameterDirection.Input, "@KONPO_WGHT", SqlDbType.Int, KONPO_WGHT);
                SqlDb.cmdExec();

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);                
            }
            finally
            {
                Ora.Disconnect();
                SqlDb.Disconnect();
            }
            return ret;
        }
        [WebMethod]
        public string strJanToKykShnCd(string JanCode)
        {
            string sql = "SELECT KYK_SHN_CD FROM SHN WHERE JAN_CD=:JAN_CD OR KYK_SHN_CD=:JAN_CD";
            cls_OraIf Ora = new cls_OraIf();
            DataTable dt = new DataTable();
            string kykShnCd = string.Empty;
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                Ora.removeSqlParam();
                Ora.setSqlCmd(sql, CommandType.Text);
                Ora.addParam(ParameterDirection.Input, "JAN_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, JanCode, 13);
                kykShnCd = Ora.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Ora.Disconnect();
            }

            return kykShnCd;
        }
        [WebMethod]
        public GENERAL getShnCdFromKEY(string KEY_CD)
        {
            GENERAL gn = new GENERAL();

            
            cls_OraIf Ora = new cls_OraIf();
            
            
            try
            {
                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                Boolean f =  Ora.removeSqlParam();
                f = Ora.setSqlCmd("SP_GETSHN_CD_FROMKEY", CommandType.StoredProcedure);
                f = Ora.addParam(ParameterDirection.Input, "KEY_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, KEY_CD, 13);
                f = Ora.addParam(ParameterDirection.Output, "po_kyk_shn_cd", OracleDbType.Varchar2, "", 15);
                f = Ora.addParam(ParameterDirection.Output, "po_jan_cd", OracleDbType.Varchar2, "", 15);

                f = Ora.cmdExec();

                if (f)
                {
                    gn.VAL_1 = Ora.getParameter("po_kyk_shn_cd").ToString();
                    gn.VAL_2 = Ora.getParameter("po_jan_cd").ToString();
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

            




            return gn;
        }
        [WebMethod]
        public GENERAL[] GETSUISHOLOC(string KEY_CD, string TANTO)
        {
            GENERAL[] GN = new GENERAL[0];


            cls_OraIf Ora = new cls_OraIf();
            DataTable dt = new DataTable();
            Boolean f = true;

            string sql = "SELECT LOC_NO_EDIT(G.LOC_NO) as LOCEDIT, G.LOC_NO,S.KYK_SHN_CD,G.GNPN_SHN_CD,SUM(G.TANA_ZK_QTY) as TANA_QTY,SUM(G.SHKA_HKTE_QTY) as SHKA_QTY "+
                        "FROM GNPNZAIKO G, SHN S, NNSSOK N, TANTO T " +
                        "WHERE G.SHN_CD = S.SHN_CD " +
                        "AND SUBSTR(G.GNPN_SHN_CD, 4, 3) = N.NNS_SOKO_SEQ " +
                        "AND N.NNS_SOKO_CD = T.SOKO_CD " +
                        "AND G.TANA_ZK_QTY > 0 " +
                        "AND T.TANTO_CD = :TANTO_CD " +
                        "AND (S.KYK_SHN_CD = :CODE " +
                        "        OR S.JAN_CD = :CODE ) " +
                        "GROUP BY G.LOC_NO,S.KYK_SHN_CD,G.GNPN_SHN_CD " +
                        "HAVING SUM(G.TANA_ZK_QTY) > 0 " +
                        "ORDER BY SUM(G.TANA_ZK_QTY)  DESC";
            
            
            try
            {


                if (!Ora.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }
                f = Ora.removeSqlParam();
                f = Ora.setSqlCmd(sql, CommandType.Text);
                f = Ora.addParam(ParameterDirection.Input, "TANTO_CD", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, TANTO, 15);
                f = Ora.addParam(ParameterDirection.Input, "CODE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, KEY_CD, 15);
                
                


                dt = Ora.cmdExecDt();
                if (dt.Rows.Count > 0)
                {
                    Array.Resize(ref GN, dt.Rows.Count);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GN[i].VAL_1 = dt.Rows[i]["LOCEDIT"].ToString();
                        GN[i].VAL_2 = dt.Rows[i]["LOC_NO"].ToString();
                        GN[i].VAL_3 = dt.Rows[i]["KYK_SHN_CD"].ToString();
                        GN[i].VAL_4 = dt.Rows[i]["GNPN_SHN_CD"].ToString();
                        GN[i].VAL_5 = dt.Rows[i]["TANA_QTY"].ToString();
                        GN[i].VAL_6 = dt.Rows[i]["SHKA_QTY"].ToString();
                    }
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





            return GN;
        }        
        [WebMethod]
        public GENERAL GETITEMINFO(string ItemCode) {

            GENERAL GN = new GENERAL();
            cls_MsSqlIf SqlDb = new cls_MsSqlIf();
            try
            {
                
                if (!SqlDb.Connect())
                {
                    throw new Exception("MSSQL Connection Error");
                }

                SqlDb.removeSqlParam();
                SqlDb.setSqlCmd("ht_sp_getItemInfo");
                SqlDb.addParam(ParameterDirection.Input, "@ItemCode", SqlDbType.VarChar, ItemCode, 20);                
                SqlDb.addParam(ParameterDirection.Output, "@ItemName", SqlDbType.NVarChar, "",100);
                SqlDb.addParam(ParameterDirection.Output, "@JANCode", SqlDbType.VarChar, "",13);
                SqlDb.addParam(ParameterDirection.Output, "@MakerName", SqlDbType.NVarChar, "",100);
                SqlDb.cmdExec();

                GN.VAL_1 = ItemCode;
                GN.VAL_2 = SqlDb.getParameter("@ItemName");
                GN.VAL_3 = SqlDb.getParameter("@JANCode");
                GN.VAL_4 = SqlDb.getParameter("@MakerName");
                GN.VAL_5 = "";
                GN.VAL_6 = "";
                

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                
                SqlDb.Disconnect();
            }
            return GN;




        }
        private string LOC_NO_EDIT_ADD(ref cls_OraIf OraDb, string LOC_NO)
        {
            string LOC_EDIT = string.Empty;
            string sql = "SELECT LOC_NO_EDIT_ADD(:LOC_NO) as LOC_EDIT FROM DUAL";
            OraDb.removeSqlParam();
            OraDb.setSqlCmd(sql, CommandType.Text);
            OraDb.addParam(ParameterDirection.Input, "LOC_NO", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, LOC_NO, 13);
            LOC_EDIT = OraDb.getDirectValue();
            return LOC_EDIT;
        }
    }
}
 