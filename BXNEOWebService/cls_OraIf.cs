using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace BXWebService
{
    public class cls_OraIf
    {

        private OracleConnection _con = null;
        private OracleTransaction _trn = null;
        private OracleCommand cmd = null;

        public Boolean Connect(int tot = -1)
        {
            Boolean f = true;
            try
            {
                if (_con == null)
                {
                    _con = new OracleConnection();
                }

                //string conStr = ConfigurationManager.AppSettings["conStringOracle"];

                if (cls_Func._OraCon == null)
                {

                    cls_Func._OraCon =  cls_Func._OraCon = cls_Func.DecryptString(ConfigurationManager.ConnectionStrings["SLPDB"].ConnectionString, "dfo85df41c8eofj$52wd");
                    
                }
                String cst = cls_Func._OraCon;

                if (tot > -1)
                {

                    cst = cst + ";Connect Timeout=" + tot.ToString();
                }

                if (_con.ConnectionString == "")
                {
                    _con.ConnectionString = cst;
                }
                
                    _con.Open();
                
            }
            catch (Exception ex)
            {

                ErrLog.logger.log(ex);
                f = false;
            }
            return f;

        }

        public void Disconnect()
        {

            try
            {
                    _con.Close();
                
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);

            }
        }

        public Boolean removeSqlParam()
        {
            Boolean f = true;
            try
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }

            return f;

        }
        public Boolean setSqlCmd(string sql, CommandType ct, Boolean ResetCmd = false)
        {
            Boolean flg = false;

            if (ResetCmd)
            {
                cmd = null;
            }


            if (cmd == null)
            {
                cmd = new OracleCommand(sql, _con);
                //cmd = new SqlCommand(sql, _con, _trn);
                cmd.CommandType = ct;
                //cmd.CommandType = CommandType.StoredProcedure;
                flg = true;
            }
            
            
            cmd.CommandText = sql;
            
            
            return flg;
        }
        public Boolean cmdExec()
        {
            Boolean flg = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                flg = false;
            }

            return flg;

        }

        public DataTable cmdExecDt()

        {
            DataTable dt = new DataTable();

            try
            {

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dt);
                adapter.Dispose();

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }

            return dt;
        }
        public Boolean addParam(ParameterDirection Direction,  string ParameterName,  OracleDbType ParameterType, string ParameterValue = "", int ParameterSize = 0)
        {
            
            Boolean flg = true;
            try
            {
                if (ParameterType == OracleDbType.Char || ParameterType == OracleDbType.Varchar2 || ParameterType == OracleDbType.NChar || ParameterType == OracleDbType.NVarchar2)
                {
                    cmd.Parameters.Add(new OracleParameter(ParameterName, ParameterType, ParameterSize));
                }
                else
                {
                    cmd.Parameters.Add(new OracleParameter(ParameterName, ParameterType));
                }
                cmd.Parameters[ParameterName].Direction = Direction;

                if (Direction == ParameterDirection.Input || Direction == ParameterDirection.InputOutput)
                {
                    cmd.Parameters[ParameterName].Value = ParameterValue;
                }

            }
            catch (Exception ex)
            {
                flg = false;
                ErrLog.logger.log(ex);
            }
            return flg;
        }

        public Boolean setParamValue(string ParameterName, string ParameterValue)
        {
            Boolean flg = true;
            try
            {

                cmd.Parameters[ParameterName].Value = ParameterValue;


            }
            catch (Exception ex)
            {
                flg = false;
                ErrLog.logger.log(ex);
            }

            return flg;

        }


        public void cmdGetReader(ref OracleDataReader reader)
        {

            string ret = string.Empty;
            try
            {

                
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {

                ErrLog.logger.log(ex);
            }

        }

        public string getDirectValue()
        {
            
            string ret = string.Empty;
            try
            {

                //OracleDataReader dr = cmd.ExecuteReader();

                ret = (string)cmd.ExecuteScalar();


            }
            catch (Exception ex)
            {
                
                ErrLog.logger.log(ex);
            }

            return ret;

        }


        public string getParameter(string ParameterName)
        {
            string ret = Convert.ToString(cmd.Parameters[ParameterName].Value);
            return ret;
        }

        public void ExecuteCmd(ref DataTable dt, int tot = -1)
        {


            try
            {
                if (tot > -1)
                {
                    cmd.CommandTimeout = tot;
                }

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dt);
                adapter.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
        }

        public Boolean eSQL(string ExecSQL, int tot = 30)
        {
            Boolean f = true;

            try
            {
                OracleCommand sqlCommand = new OracleCommand(ExecSQL, _con);
                sqlCommand.CommandTimeout = tot;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                f = false;
                ErrLog.logger.log(ex);
            }
            return f;
        }
        public DataTable ExecuteSql(String sql, int tot = -1)
        {
            DataTable dt = new DataTable();

            try
            {
                OracleCommand sqlCommand = new OracleCommand(sql, _con);

                if (tot > -1)
                {
                    sqlCommand.CommandTimeout = tot;
                }

                OracleDataAdapter adapter = new OracleDataAdapter(sqlCommand);

                adapter.Fill(dt);
                adapter.Dispose();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }

            return dt;
        }

        public Boolean ExportSQLToFile(string ExportFileName, string sql, Boolean HeaderNeed = false, string Separator = "\t"
                            , string sql2nd = "", int tot = -1, string enclosure = "", string encode = "shift_jis")
        {

            DataTable dt = new DataTable();
            Boolean f = false;
            int r, c;
            string stPrompt = string.Empty;

            try
            {
                if (cmd == null) { cmd = new OracleCommand(sql, _con); }
                if (tot > -1) { cmd.CommandTimeout = tot; }

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    System.IO.StreamWriter fs = new System.IO.StreamWriter(ExportFileName, false, System.Text.Encoding.GetEncoding(encode));
                    if (HeaderNeed)
                    {
                        for (c = 0; c < dt.Columns.Count; c++)
                        {
                            if (enclosure == "")
                            {
                                stPrompt += dt.Columns[c].ColumnName;
                            }
                            else
                            {
                                stPrompt += enclosure + dt.Columns[c].ColumnName + enclosure;
                            }

                            if (c < (dt.Columns.Count - 1))
                            {
                                stPrompt += Separator;
                            }
                        }
                        fs.WriteLine(stPrompt);
                        stPrompt = String.Empty;
                    }


                    for (r = 0; r < dt.Rows.Count; r++)
                    {
                        for (c = 0; c < dt.Columns.Count; c++)
                        {
                            if (enclosure == "")
                            {
                                stPrompt += dt.Rows[r][c];
                            }
                            else
                            {
                                stPrompt += enclosure + dt.Rows[r][c] + enclosure;
                            }

                            if (c < (dt.Columns.Count - 1))
                            {
                                stPrompt += Separator;
                            }
                        }
                        fs.WriteLine(stPrompt);
                        stPrompt = String.Empty;
                    }

                    adapter.Dispose();
                    cmd.Dispose();


                    if (sql2nd != "")
                    {
                        cmd = new OracleCommand(sql2nd, _con);
                        adapter = new OracleDataAdapter(cmd);
                        dt.Clear();
                        adapter.Fill(dt);
                        for (r = 0; r < dt.Rows.Count; r++)
                        {
                            for (c = 0; c < dt.Columns.Count; c++)
                            {
                                if (enclosure == "")
                                {
                                    stPrompt += dt.Rows[r][c];
                                }
                                else
                                {
                                    stPrompt += enclosure + dt.Rows[r][c] + enclosure;
                                }

                                if (c < (dt.Columns.Count - 1))
                                {
                                    stPrompt += Separator;
                                }
                            }
                            fs.WriteLine(stPrompt);
                            stPrompt = String.Empty;
                        }
                        adapter.Dispose();
                        cmd.Dispose();
                    }

                    fs.Close();
                    dt.Dispose();

                }





            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }


            return f;

        }
        public void BeginTransaction()
        {
            try
            {
                _trn = _con.BeginTransaction();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
        }
        public void CommitTransaction()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Commit();
                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                _trn = null;
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Rollback();
                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            finally
            {
                _trn = null;
            }
        }

        ~cls_OraIf()
        {
            Disconnect();
        }


    }
}