using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace BXWebService
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string kyk_shn_cd = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "SELECT KYK_SHN_CD FROM SHN WHERE SHN_CD = 'S130442977'";
            cls_PgsqlIf Pg = new cls_PgsqlIf();
            try
            {

                
                if (!Pg.Connect())
                {
                    throw new Exception("Oracle Connection Error");
                }

                DataTable dt = new DataTable();

                Pg.setSqlCmd(sql, CommandType.Text);
                dt = Pg.cmdExecDt();

                if (dt.Rows.Count>=1)
                {
                    kyk_shn_cd = dt.Rows[0][0].ToString();
                }

                //using (OracleConnection conn = new OracleConnection())
                //{
                //    conn.ConnectionString = cls_Func.DecryptString( ConfigurationManager.ConnectionStrings["SLPDB"].ConnectionString, "dfo85df41c8eofj$52wd");



                //    conn.Open();
                //    using (OracleCommand cmd = new OracleCommand(sql))
                //    {
                //        cmd.Connection = conn;
                //        cmd.CommandType = System.Data.CommandType.Text;
                //        using(OracleDataReader reader = cmd.ExecuteReader())
                //        {
                //            while (reader.Read())
                //            {
                //                kyk_shn_cd = reader["kyk_shn_cd"].ToString();

                //            }
                //        }
                //    }

                //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                Pg.Disconnect();
            }
          }
    }
}