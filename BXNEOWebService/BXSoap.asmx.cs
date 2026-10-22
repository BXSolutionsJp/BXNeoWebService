using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BXWebService
{
    /// <summary>
    /// BXSoap の概要の説明です
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    [System.Web.Script.Services.ScriptService]
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
        public struct HKATInfo // 入出荷情報
        {
            public string HKAT_SHIJI_NO; // 出荷指示番号
            public string OKURIJO_NO; // 送り状番号
            public string NYSHK_DATE; // 入荷&出荷日
            public string QTY; // quantity 数量
        }
        public struct MoveItem // 商品移動
        {
            public string SOKO_KANRI_CD; // 倉庫管理コード
            public string KYK_SHN_CD; // 商品コード
            public string GNPN_SHN_CD; // 商品コード?
            public string SHN_NM; // 商品名
            public string KOZA; // 口座番号
            public string ZK_KANRI; // 在庫の管理区分
            public string TANA_ZK_QTY; // 在庫の数量
        }
        public struct KofukuInfo // 梱包用
        {
            public string KYK_SHN_CD; // 商品コード
            public string JAN_CD; // JANコード
            public string KONPO_W; // 梱包幅
            public string KONPO_D; // 梱包奥行き?
            public string KONPO_H; // 梱包高さ
            public string KONPO_WGHT; // 梱包重量
        }
        public struct MoveFIFO // 商品移動
        {
            public string RTN; // id
            public string RTN_QTY; // 移動数量
            public string KYK_SHN_CD; // 移動する商品コード
            public string MOT_LOC; // 移動元ロケーション
            public string SAKI_LOC; // 移動先ロケーション
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
                    throw new Exception("MSSQL Connection Error");
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
        /// <summary>
        /// 倉庫管理コードとロケーションコードからロケーションが存在するかを確認する
        /// BLKAREA: 倉庫内の各エリアのブロックを定義する
        /// AREA: 倉庫内のエリアを定義する
        /// NNSSOK: 荷主毎の倉庫を定義する
        /// LOCATION: ロケーションの属性を定義する
        /// </summary>
        /// <param name="SOKO_KANRI_CD">倉庫管理コード</param>
        /// <param name="LOC_EDIT">ロケーションコード</param>
        /// <returns>ロケーションの数を返す</returns>
        [WebMethod]
        public string isLocationExists(string SOKO_KANRI_CD, string LOC_EDIT)
        {
            string  ret=string.Empty;
            string sql = @"SELECT CAST(COUNT(*) as  VARCHAR(10)) as CNT
                            FROM BLKAREA, AREA, NNSSOK, LOCATION
                            WHERE BLKAREA.AREA_NO = AREA.AREA_NO
                                AND AREA.SOKO_CD::INTEGER = NNSSOK.NNS_SOKO_CD::INTEGER
                                AND SOKO_KANRI_CD = :SOKO_KANRI_CD
                                AND LOCATION.LOC_NO	BETWEEN FROM_LOC_NO AND TO_LOC_NO
                                AND LOCATION.LOC_NO   = LOC_NO_EDIT_ADD(:LOC_EDIT)";

            cls_PgsqlIf Pg = new cls_PgsqlIf();
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Pg.removeSqlParam();
                Pg.setSqlCmd(sql, CommandType.Text);
                Pg.addParam(ParameterDirection.Input, "SOKO_KANRI_CD", NpgsqlDbType.Varchar, SOKO_KANRI_CD, 8);
                Pg.addParam(ParameterDirection.Input, "LOC_EDIT", NpgsqlDbType.Varchar, LOC_EDIT, 15);

                ret = Pg.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Pg.Disconnect();
            }

            return ret;
        }
        /// <summary>
        /// JANCODE(13桁。12桁なら先頭に0を補う)もしくは客先商品コード(12桁・13桁以外)を入力し、商品テーブルから商品に関する情報を取得する
        /// SHN: 受託商品の属性を定義する。主に容積、重量など
        /// </summary>
        /// <param name="KEY">JANCODEもしくは客先商品コード</param>
        /// <returns>客先商品コード、JANCODE、梱包長さ、幅、高さ、重さの6種類の商品情報</returns>
        [WebMethod]
        public KofukuInfo getKofukuInfo(string KEY)
        {

            KofukuInfo ki = new KofukuInfo();

            
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }

                Pg.removeSqlParam();
                Pg.setSqlCmd("SP_GET_KOFUKU", CommandType.StoredProcedure);
                Pg.addParam(ParameterDirection.Input, "key_cd", NpgsqlDbType.Varchar, KEY, 13);
                Pg.addParam(ParameterDirection.Output, "po_kyk_shn_cd", NpgsqlDbType.Varchar, "", 20);
                Pg.addParam(ParameterDirection.Output, "po_jan_cd", NpgsqlDbType.Varchar, "", 13);
                Pg.addParam(ParameterDirection.Output, "po_konpo_d", NpgsqlDbType.Integer);
                Pg.addParam(ParameterDirection.Output, "po_konpo_w", NpgsqlDbType.Integer);
                Pg.addParam(ParameterDirection.Output, "po_konpo_h", NpgsqlDbType.Integer);
                Pg.addParam(ParameterDirection.Output, "po_konpo_wght", NpgsqlDbType.Integer);

                Pg.cmdExec();

                ki.KYK_SHN_CD = Pg.getParameter("po_kyk_shn_cd");
                ki.JAN_CD = Pg.getParameter("po_jan_cd");
                ki.KONPO_W = Pg.getParameter("po_konpo_w");
                ki.KONPO_D = Pg.getParameter("po_konpo_d");
                ki.KONPO_H = Pg.getParameter("po_konpo_h");
                ki.KONPO_WGHT = Pg.getParameter("po_konpo_wght");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Pg.Disconnect();
            }
            return ki;
        }
        /// <summary>
        /// 指定した倉庫・商品・ロケーション・口座に該当する在庫情報を1件取得する
        /// </summary>
        /// <param name="SOKO_KANRI_CD"></param>
        /// <param name="SHN_CD"></param>
        /// <param name="LOC_NO"></param>
        /// <param name="KOZA"></param>
        /// <returns>移動する商品のデータ</returns>
        /// <exception cref="Exception"></exception>
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
                        WHERE (SHN.KYK_SHN_CD = :kyk_shn_cd) AND (GNPNZAIKO.SHN_CD =SHN.SHN_CD) 
                            AND (NNSSOK.NNS_SOKO_SEQ = SUBSTR(GNPNZAIKO.GNPN_SHN_CD,4,3)) 
                            AND NNSSOK.SOKO_KANRI_CD = :soko_kanri_cd
                            AND LOC_NO = LOC_NO_EDIT_ADD(:loc_no) AND KOZA LIKE :koza
                        GROUP BY NNSSOK.SOKO_KANRI_CD,SHN.KYK_SHN_CD,GNPNZAIKO.GNPN_SHN_CD,SHN.SHN_NM,GNPNZAIKO.KOZA  ";

                cls_PgsqlIf Pg = new cls_PgsqlIf();
                DataTable dt = new DataTable();

            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Pg.removeSqlParam();
                Pg.setSqlCmd(sql, CommandType.Text);
                Pg.addParam(ParameterDirection.Input, "kyk_shn_cd", NpgsqlDbType.Varchar, SHN_CD, 13);
                Pg.addParam(ParameterDirection.Input, "soko_kanri_cd", NpgsqlDbType.Varchar, SOKO_KANRI_CD, 13);
                Pg.addParam(ParameterDirection.Input, "loc_no", NpgsqlDbType.Varchar, LOC_NO, 13);
                Pg.addParam(ParameterDirection.Input, "koza", NpgsqlDbType.Varchar, KOZA, 13);

                dt = Pg.cmdExecDt();
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
            catch (NpgsqlException ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Pg.Disconnect();
            }
            return list;
        }
        [WebMethod]
        public GENERAL GetMoveData_Tana(string SOKO_KANRI_CD, string LOC_NO)
        {

            GENERAL gn = new GENERAL();

            cls_PgsqlIf Pg = new cls_PgsqlIf();
            DataTable dt = new DataTable();

            try
            {
                string stad = "SP_GET_TANA_INFO";                
               
                    if (!Pg.Connect())
                    {
                        throw new Exception("Postgres Connection Error");
                    }

                    Pg.removeSqlParam();
                    Pg.setSqlCmd(stad, CommandType.StoredProcedure);
                    Pg.addParam(ParameterDirection.Input, "p_soko_kanri_cd", NpgsqlDbType.Varchar, SOKO_KANRI_CD, 4);
                    Pg.addParam(ParameterDirection.Input, "p_loc_no", NpgsqlDbType.Varchar, LOC_NO, 15);
                    Pg.addParam(ParameterDirection.Output, "p_sku_cnt", NpgsqlDbType.Integer);
                    Pg.addParam(ParameterDirection.Output, "p_koza_cnt", NpgsqlDbType.Integer);
                    Pg.addParam(ParameterDirection.Output, "p_total_cnt", NpgsqlDbType.Integer);
                    Pg.addParam(ParameterDirection.Output, "p_hkat_cnt", NpgsqlDbType.Integer);
                if (Pg.cmdExec())
                {
                    gn.VAL_1 = Pg.getParameter("p_sku_cnt").ToString();
                    gn.VAL_2 = Pg.getParameter("p_koza_cnt").ToString();
                    gn.VAL_3 = Pg.getParameter("p_total_cnt").ToString();
                    gn.VAL_4 = Pg.getParameter("p_hkat_cnt").ToString();
                }
               
                
            }
            catch (NpgsqlException ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Pg.Disconnect();
            }

            return gn;
        }
        /// <summary>
        /// 指定した商品コードに該当する入庫情報を配列で取得する
        /// </summary>
        /// <param name="shnCd"></param>
        /// <returns></returns>
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
                                "AND (SHN.KYK_SHN_CD LIKE (:shn_cd || '%') OR SHN.JAN_CD = :shn_cd)";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            DataTable dt = new DataTable();

            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }

                Pg.removeSqlParam();
                Pg.setSqlCmd(sql, CommandType.Text);
                Pg.addParam(ParameterDirection.Input, "shn_cd", NpgsqlDbType.Varchar, shnCd ?? "", 0);


                dt = Pg.cmdExecDt();
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
                Pg.Disconnect();
            }

            return list;
        }
        [WebMethod]
        public string getSLPTanToName(string TANTO_CD)
        {

            string sql = "SELECT TANTO_NM FROM TANTO WHERE TANTO_CD=:TANTO_CD";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            DataTable dt = new DataTable();
            string TANTO_NM = string.Empty;
            try
            {


                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }

                Pg.removeSqlParam();
                Pg.setSqlCmd(sql, CommandType.Text);
                Pg.addParam(ParameterDirection.Input, "TANTO_CD", NpgsqlDbType.Varchar, TANTO_CD, 13);


                TANTO_NM = Pg.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Pg.Disconnect();
            }

            return TANTO_NM;
        }
        /// <summary>
        /// 担当者マスター(TANTO)をSP_GET_TANTO_NMで検索し、担当者名を返す。該当がなければ空文字を返す
        /// </summary>
        /// <param name="TANTO_CD"></param>
        /// <returns>商品マスターにデータがあれば担当コードが、なければ空文字列が返る</returns>
        [WebMethod]
        public string getTantoNmStored(string TANTO_CD)
        {
            string tanto_nm = string.Empty;
            string stad = "SP_GET_TANTO_NM";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Pg.removeSqlParam();
                Pg.setSqlCmd(stad, CommandType.StoredProcedure);
                Pg.addParam(ParameterDirection.Input, "pi_tanto_cd", NpgsqlDbType.Varchar, TANTO_CD, 15);
                Pg.addParam(ParameterDirection.Output, "po_tanto_nm", NpgsqlDbType.Varchar, "", 15);
                if (Pg.cmdExec())
                {
                    tanto_nm = Pg.getParameter("po_tanto_nm");
                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                Pg.Disconnect();
            }
            return tanto_nm;
        }

        [WebMethod]
        public string LipStoreEnd(string HKAT_SHIJI_NO, string QTY, string LOC_NO, string TANTO_CD, string LIP_NO = "", string LOT_NO = "", string RSN_CD = "00")
        {
            // npgsqlに渡す際に引数省略するとなぜかnull扱いになるのでここで改めて代入
            LIP_NO = LIP_NO ?? "";
            RSN_CD = RSN_CD ?? "00";

            // TANTO_CD バリデーション
            if (string.IsNullOrWhiteSpace(TANTO_CD))
            {
                throw new Exception("TANTO_CDは必須です。");
            }

            // QTY バリデーション
            decimal qty;
            if (!decimal.TryParse(
                QTY,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out qty))
            {
                throw new Exception("QTYには数値を指定してください。");
            }

            string TANTO = string.Empty;
            if (TANTO_CD.Length <= 8)
            {
                TANTO = TANTO_CD;
            }
            else
            {
                TANTO = cls_Func.DecryptString(TANTO_CD, System.DateTime.Now.ToString("yyyyMMdd"));
            }

            if (string.IsNullOrEmpty(LOT_NO))
            {
                LOT_NO = System.DateTime.Now.ToString("yyyyMMdd");
            }


            cls_PgsqlIf Pg = new cls_PgsqlIf();
            Boolean f = false;
            string ret = string.Empty;

            try
            {
                if (!Pg.Connect()) throw new System.Exception("Postgres Connection Error");
                f = Pg.setSqlCmd("pack_lip__proc_LipStore_End", CommandType.StoredProcedure);
                f = Pg.addParam(ParameterDirection.Input, "p_hkat_shiji_no", NpgsqlDbType.Varchar, HKAT_SHIJI_NO, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_qty", NpgsqlDbType.Numeric, qty);
                f = Pg.addParam(ParameterDirection.Input, "p_loc_no", NpgsqlDbType.Varchar, LOC_NO, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_lip_no", NpgsqlDbType.Varchar, LIP_NO, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_lot_no", NpgsqlDbType.Varchar, LOT_NO, 64);
                f = Pg.addParam(ParameterDirection.Input, "p_rsn_cd", NpgsqlDbType.Varchar, RSN_CD, 6);
                f = Pg.addParam(ParameterDirection.Input, "p_upd_tanto_cd", NpgsqlDbType.Varchar, TANTO, 8);
                f = Pg.addParam(ParameterDirection.Output, "p_rc", NpgsqlDbType.Varchar, "", 2);

                if (!Pg.cmdExec())
                {
                    throw new Exception("DB処理に失敗しました。");
                }

                ret = Pg.getParameter("p_rc").Trim();

                if (string.IsNullOrEmpty(ret))
                {
                    throw new Exception("DB処理の結果が返されませんでした。");
                }

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                throw;
            }
            finally
            {
                Pg.Disconnect();
            }
            return ret;
        }        
        [WebMethod]
        public MoveFIFO MoveZaikoFIFO(string GNPN_SHN_CD,string KYK_SHN_CD,  string KOZA, string MOTO_LOC, string SAKI_LOC, int QTY, string TANTO_CD)
        {
            MoveFIFO Ret = new MoveFIFO();
            string TANTO = string.Empty;
            string MOT_LOC_ADD = string.Empty;
            string SAKI_LOC_ADD = string.Empty;

            cls_PgsqlIf Pg = new cls_PgsqlIf();
            Boolean f = false;
            string ret = string.Empty;

            try
            {
                if (!Pg.Connect()) throw new System.Exception("Postgres Connection Error");
                MOT_LOC_ADD = LOC_NO_EDIT_ADD(ref Pg, MOTO_LOC);
                SAKI_LOC_ADD = LOC_NO_EDIT_ADD(ref Pg, SAKI_LOC);
                f = Pg.setSqlCmd("SP_FIFO_IDO", CommandType.StoredProcedure, true);
                f = Pg.addParam(ParameterDirection.Input, "p_gnpn_shn_cd", NpgsqlDbType.Varchar, GNPN_SHN_CD, 36);
                f = Pg.addParam(ParameterDirection.Input, "p_koza", NpgsqlDbType.Varchar, KOZA, 4);
                f = Pg.addParam(ParameterDirection.Input, "p_unit", NpgsqlDbType.Char, "P", 1);
                f = Pg.addParam(ParameterDirection.Input, "p_motoloc_no", NpgsqlDbType.Varchar, MOT_LOC_ADD, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_sakiloc_no", NpgsqlDbType.Varchar, SAKI_LOC_ADD, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_qty", NpgsqlDbType.Numeric, QTY);
                f = Pg.addParam(ParameterDirection.Input, "p_hkat_flg", NpgsqlDbType.Varchar, "0",3);
                f = Pg.addParam(ParameterDirection.Input, "p_dict_flg", NpgsqlDbType.Varchar, "0", 3);
                f = Pg.addParam(ParameterDirection.Input, "p_tanto_cd", NpgsqlDbType.Varchar, TANTO_CD, 8);
                f = Pg.addParam(ParameterDirection.Output, "p_rtn", NpgsqlDbType.Varchar, "", 15);
                f = Pg.addParam(ParameterDirection.Output, "p_rtn_qty", NpgsqlDbType.Numeric);
                f = Pg.cmdExec();

                Ret.RTN = Pg.getParameter("p_rtn").ToString().Trim();
                Ret.RTN_QTY = Pg.getParameter("p_rtn_qty").ToString().Trim();
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
                Pg.Disconnect();
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

            cls_PgsqlIf Pg = new cls_PgsqlIf();
            Boolean f = false;
            string ret = string.Empty;

            try
            {
                if (!Pg.Connect()) throw new System.Exception("Postgres Connection Error");
                // LOC_NO_EDIT_ADD の Pg 版が必要
                MOT_LOC_ADD = LOC_NO_EDIT_ADD(ref Pg, MOTO_LOC);
                SAKI_LOC_ADD = LOC_NO_EDIT_ADD(ref Pg, SAKI_LOC);
                f = Pg.setSqlCmd("SP_IDO_TANA_ALL", CommandType.StoredProcedure, true);
                f = Pg.addParam(ParameterDirection.Input, "p_soko_kanri_cd", NpgsqlDbType.Varchar, SOKO_KANRI_CD, 8);
                f = Pg.addParam(ParameterDirection.Input, "p_koza", NpgsqlDbType.Varchar, KOZA, 4);                
                f = Pg.addParam(ParameterDirection.Input, "p_motoloc_no", NpgsqlDbType.Varchar, MOT_LOC_ADD, 15);
                f = Pg.addParam(ParameterDirection.Input, "p_sakiloc_no", NpgsqlDbType.Varchar, SAKI_LOC_ADD, 15);                
                f = Pg.addParam(ParameterDirection.Output, "p_cnt", NpgsqlDbType.Numeric);
                f = Pg.cmdExec();

                Ret.RTN = "";
                Ret.RTN_QTY = Pg.getParameter("p_cnt").ToString().Trim();
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
                Pg.Disconnect();
            }
            return Ret;
        }
        [WebMethod]
        public string RegKofukuInfo(string KYK_SHN_CD, string KONPO_W, string KONPO_D, string KONPO_H, string KONPO_WGHT)
        {
            string ret = string.Empty;
            string stad = "SP_REG_KOFUKU";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            cls_MsSqlIf SqlDb = new cls_MsSqlIf();
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Pg.removeSqlParam();
                Pg.setSqlCmd(stad, CommandType.StoredProcedure);
                Pg.addParam(ParameterDirection.Input, "pi_kyk_shn_cd", NpgsqlDbType.Varchar, KYK_SHN_CD, 13);
                Pg.addParam(ParameterDirection.Input, "pi_konpo_d", NpgsqlDbType.Integer, KONPO_D);
                Pg.addParam(ParameterDirection.Input, "pi_konpo_w", NpgsqlDbType.Integer, KONPO_W);                
                Pg.addParam(ParameterDirection.Input, "pi_konpo_h", NpgsqlDbType.Integer, KONPO_H);
                Pg.addParam(ParameterDirection.Input, "pi_konpo_wght", NpgsqlDbType.Integer, KONPO_WGHT);
                Pg.addParam(ParameterDirection.Output, "po_ret", NpgsqlDbType.Integer);
                if (Pg.cmdExec())
                {
                    ret = Pg.getParameter("po_ret");
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
                Pg.Disconnect();
                SqlDb.Disconnect();
            }
            return ret;
        }
        /// <summary>
        /// JANコードから客先商品コードへの変換を行う。入力が客先商品コードならそのまま返す
        /// </summary>
        /// <param name="JanCode"></param>
        /// <returns></returns>
        [WebMethod]
        public string strJanToKykShnCd(string JanCode)
        {
            string sql = "SELECT KYK_SHN_CD FROM SHN WHERE JAN_CD=:JAN_CD OR KYK_SHN_CD=:JAN_CD";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            DataTable dt = new DataTable();
            string kykShnCd = string.Empty;
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Pg.removeSqlParam();
                Pg.setSqlCmd(sql, CommandType.Text);
                Pg.addParam(ParameterDirection.Input, "JAN_CD", NpgsqlDbType.Varchar, JanCode, 13);
                kykShnCd = Pg.getDirectValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Pg.Disconnect();
            }
            return kykShnCd;
        }
        /// <summary>
        /// 客先商品コードまたはJANコードのうちどちらかをキーにして両方取得するAPI
        /// </summary>
        /// <param name="KEY_CD"></param>
        /// <returns></returns>
        [WebMethod]
        public GENERAL getShnCdFromKEY(string KEY_CD)
        {
            GENERAL gn = new GENERAL();
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            try
            {
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                Boolean f =  Pg.removeSqlParam();
                f = Pg.setSqlCmd("SP_GETSHN_CD_FROMKEY", CommandType.StoredProcedure);
                f = Pg.addParam(ParameterDirection.Input, "key_cd", NpgsqlDbType.Varchar, KEY_CD, 13);
                f = Pg.addParam(ParameterDirection.Output, "po_kyk_shn_cd", NpgsqlDbType.Varchar, "", 15);
                f = Pg.addParam(ParameterDirection.Output, "po_jan_cd", NpgsqlDbType.Varchar, "", 15);
                f = Pg.cmdExec();
                if (f)
                {
                    gn.VAL_1 = Pg.getParameter("po_kyk_shn_cd").ToString();
                    gn.VAL_2 = Pg.getParameter("po_jan_cd").ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Pg.Disconnect();
            }
            return gn;
        }
        /// <summary>
        /// 指定した商品（KEY_CD）と担当者（TANTO）に紐づく在庫が存在する棚の一覧を取得する
        /// </summary>
        /// <param name="KEY_CD">商品コード</param>
        /// <param name="TANTO">担当者コード。実質ユーザーの識別子</param>
        /// <returns></returns>
        [WebMethod]
        public GENERAL[] GETSUISHOLOC(string KEY_CD, string TANTO)
        {
            GENERAL[] GN = new GENERAL[0];
            cls_PgsqlIf Pg = new cls_PgsqlIf();
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
                if (!Pg.Connect())
                {
                    throw new Exception("Postgres Connection Error");
                }
                f = Pg.removeSqlParam();
                f = Pg.setSqlCmd(sql, CommandType.Text);
                f = Pg.addParam(ParameterDirection.Input, "TANTO_CD", NpgsqlDbType.Varchar, TANTO, 15);
                f = Pg.addParam(ParameterDirection.Input, "CODE", NpgsqlDbType.Varchar, KEY_CD, 15);
                dt = Pg.cmdExecDt();
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
                Pg.Disconnect();
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
        private string LOC_NO_EDIT_ADD(ref cls_PgsqlIf Pg, string LOC_NO)
        {
            string LOC_EDIT = string.Empty;
            string sql = "SELECT LOC_NO_EDIT_ADD(:LOC_NO) as LOC_EDIT FROM DUAL";
            Pg.removeSqlParam();
            Pg.setSqlCmd(sql, CommandType.Text);
            Pg.addParam(ParameterDirection.Input, "LOC_NO", NpgsqlDbType.Varchar, LOC_NO, 13);
            LOC_EDIT = Pg.getDirectValue();
            return LOC_EDIT;
        }
    }
}
 