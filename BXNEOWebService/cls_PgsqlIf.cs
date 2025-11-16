using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;

namespace BXWebService
{
    public class cls_PgsqlIf
    {
        private string connString = "host=localhost;port=5434;database=postgres;username=postgres;password=postgres";
        private NpgsqlConnection _con;
        private NpgsqlTransaction _trn;
        private NpgsqlCommand _cmd;

        public cls_PgsqlIf()
        {
            _con = new NpgsqlConnection(connString);
        }

        public Boolean Connect()
        {
            Boolean f = true;

            if (_con == null)
            {
                _con = new NpgsqlConnection(connString);
            }

            if (_con.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    _con.Open();
                }
                catch (Exception ex)
                {
                    ErrLog.logger.log(ex);
                    f = false;
                }
            }
            return f;
        }

        public void Disconnect()
        {
            try
            {
                _con.Close();
            }
            catch(Exception ex)
            {
                ErrLog.logger.log(ex);
            }
        }

        public Boolean removeSqlParam()
        {
            Boolean f = true;
            try
            {
                if (_cmd != null)
                {
                    _cmd.Parameters.Clear();
                }
            }
            catch(Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }
            return f;
        }

        public Boolean setSqlCmd(string sql, CommandType ct, Boolean ResetCmd = false)
        {
            Boolean flg = false;

            if(ResetCmd)
            {
                _cmd = null;
            }

            if (_cmd == null)
            {
                _cmd = new NpgsqlCommand();
                _cmd.Connection = _con;
                _cmd.CommandType = ct;
                _cmd.CommandText = sql;
                flg = true;
            }

            return flg;
        }

        public Boolean cmdExec()
        {
            Boolean flg = true;
            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
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
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(_cmd);

                adapter.Fill(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                dt = null;
            }
            return dt;
        }

        public Boolean addParam(ParameterDirection Direction, string ParameterName, NpgsqlDbType ParameterType, string ParameterValue = "", int ParameterSize = 0)
        {
            Boolean f = true;
            NpgsqlParameter param;
            try
            {
                if (ParameterType == NpgsqlDbType.Char || ParameterType == NpgsqlDbType.Varchar || ParameterType == NpgsqlDbType.Numeric || ParameterType == NpgsqlDbType.Varbit || ParameterType == NpgsqlDbType.Bit)
                {
                    if (ParameterSize > 0)
                    {
                        param = _cmd.Parameters.Add(ParameterName, ParameterType, ParameterSize);
                    }
                    else
                    {
                        param = _cmd.Parameters.Add(ParameterName, ParameterType);
                    }
                }
                else
                {
                    param = _cmd.Parameters.Add(ParameterName, ParameterType);
                }

                param.Direction = Direction;

                if (Direction != ParameterDirection.Output)
                {
                    param.Value = ParameterValue;
                }
                
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }
            return f;

        }

        public Boolean setParamValue(string ParameterName, string ParameterValue)
        {
            Boolean f = true;

            try
            {
                _cmd.Parameters[ParameterName].Value = ParameterValue;
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }
            return f;
        }

        // 呼び出し元に実行結果を引数経由で返す
        public void cmdGetReader(ref NpgsqlDataReader reader)
        {
            try
            {
                reader = _cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                reader = null;
            }
        }

        public string getDirectValue()
        {
            string ret = string.Empty;
            try
            {
                ret = (string)_cmd.ExecuteScalar(); // クエリを実行し、クエリによって返された結果セットの最初の行の最初の列を返します。余分な列や行は無視されます。
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }
            return ret;
        }

        public string getParameter(string ParameterName)
        {
            string ret = Convert.ToString(_cmd.Parameters[ParameterName].Value);
            return ret;
        }

        public void ExecuteCmd(ref DataTable dt, int tot = -1)
        {
            try
            {
                if (tot > -1)
                {
                    _cmd.CommandTimeout = tot;
                }

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(_cmd);

                adapter.Fill(dt);
                adapter.Dispose();
                _cmd.Dispose();
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
                NpgsqlCommand sqlCommand = new NpgsqlCommand (ExecSQL, _con);
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
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sql, _con);

                if (tot > -1)
                {
                    sqlCommand.CommandTimeout = tot;
                }

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlCommand);

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
                if (_cmd == null) { _cmd = new NpgsqlCommand(sql, _con); }
                if (tot > -1) { _cmd.CommandTimeout = tot; }

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(_cmd);
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
                    _cmd.Dispose();


                    if (sql2nd != "")
                    {
                        _cmd = new NpgsqlCommand(sql2nd, _con);
                        adapter = new NpgsqlDataAdapter(_cmd);
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
                        _cmd.Dispose();
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



    }
}