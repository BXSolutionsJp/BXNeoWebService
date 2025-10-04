using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BXWebService
{
    public class cls_MsSqlIf
    {
        private SqlConnection _con = null;
        private SqlTransaction _trn = null;
        private SqlCommand cmd = null;

        public Boolean Connect(int tot = -1)
        {
            Boolean f = true;
            try
            {
                if (_con == null)
                {
                    _con = new SqlConnection();
                }

                //string conStr = ConfigurationManager.AppSettings["conStringOracle"];

                if (cls_Func._MyCon == null)
                {
                   
                        cls_Func._MyCon = cls_Func.DecryptString(ConfigurationManager.ConnectionStrings["ASKGJP"].ConnectionString, "dfo85df41c8eofj$52wd");
                   
                }
                String cst = cls_Func._MyCon;

                if (tot > -1)
                {

                    cst = cst + ";Connect Timeout=" + tot.ToString();
                }

                if (_con.ConnectionString == "")
                {
                    _con.ConnectionString = cst;
                }
                if (_con.State == ConnectionState.Closed)
                {
                    _con.Open();
                }
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
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
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
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }

            return f;

        }
        public Boolean setSqlCmd(string sql)
        {
            Boolean flg = false;



            if (cmd == null)
            {

                cmd = new SqlCommand(sql, _con, _trn);
                cmd.CommandType = CommandType.StoredProcedure;
                flg = true;
            }
            else
            {
                cmd.CommandText = sql;
                flg = true;
            }
            return flg;
        }
        public Boolean cmdExec()
        {
            Boolean flg = true;
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                }
                dr.Close();
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

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
                adapter.Dispose();

            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }

            return dt;
        }

        public Boolean addParam(ParameterDirection Direction, string ParameterName, SqlDbType ParameterType, string ParameterValue = "", int ParameterSize = 0)
        {
            Boolean flg = true;
            try
            {
                if (ParameterType == SqlDbType.Char || ParameterType == SqlDbType.VarChar || ParameterType == SqlDbType.NChar || ParameterType == SqlDbType.NVarChar)
                {
                    cmd.Parameters.Add(new SqlParameter(ParameterName, ParameterType, ParameterSize));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter(ParameterName, ParameterType));
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

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

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
                SqlCommand sqlCommand = new SqlCommand(ExecSQL, _con, _trn);
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
                SqlCommand sqlCommand = new SqlCommand(sql, _con, _trn);

                if (tot > -1)
                {
                    sqlCommand.CommandTimeout = tot;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

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
                if (cmd == null) { cmd = new SqlCommand(sql, _con, _trn); }
                if (tot > -1) { cmd.CommandTimeout = tot; }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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
                        cmd = new SqlCommand(sql2nd, _con, _trn);
                        adapter = new SqlDataAdapter(cmd);
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

        ~cls_MsSqlIf()
        {
            Disconnect();
        }
    }
}