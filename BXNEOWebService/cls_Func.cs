using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BXWebService
{
    public class cls_Func
    {
        public static string baseUrl = "http://192.168.6.7/BxHt/HTS/";
        public static string _MyCon;
        public static string _OraCon;
        public static string AplName;
        public static string APlVerSion;
        public static string pcLoginUser;
        public static string pcMachine;
        public static string pcIpAddress;

        public static int MyempID = 0;

        public static string myName = string.Empty;
        public static string myEmail = string.Empty;


        public static int ProcessID = 0;



        public static Boolean ExecDos(string DosCmd, Boolean OpenDosWindow = false)
        {
            Boolean f = true;
            System.Diagnostics.Process p = new System.Diagnostics.Process();


            try
            {
                //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
                p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                //出力を読み取れるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = false;
                //ウィンドウを表示しないようにする
                p.StartInfo.CreateNoWindow = (!OpenDosWindow);      //true 非表示
                //コマンドラインを指定（"/c"は実行後閉じるために必要）
                p.StartInfo.Arguments = "/c " + DosCmd;

                //起動
                p.Start();



                //出力を読み取る
                string results = p.StandardOutput.ReadToEnd();

                //プロセス終了まで待機する
                //WaitForExitはReadToEndの後である必要がある
                //(親プロセス、子プロセスでブロック防止のため)
                p.WaitForExit();
                p.Close();

                //出力された結果を表示
                //MessageBox.Show(results, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }
            return f;
        }

        //public static string getSavedFileName(string defName)
        //{
        //    string savedFileName = "";

        //    System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
        //    sfd.FileName = defName;
        //    sfd.Filter = "カンマ区切り(*.csv)|*.csv|タブ区切り(.tcv)|*.tsv";
        //    sfd.FilterIndex = 1;
        //    sfd.Title = "保存先のファイルを選択してください";
        //    sfd.RestoreDirectory = true;
        //    sfd.OverwritePrompt = true;
        //    sfd.CheckPathExists = true;

        //    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        //OKボタンがクリックされたとき、選択されたファイル名を表示する
        //        savedFileName = sfd.FileName;
        //    }


        //    return savedFileName;
        //}


        //public static Boolean saveDatagridView(ref System.Windows.Forms.DataGridView dg, string ExportFileName, Boolean needHeader = true, Int16 startColomns = 0)
        //{
        //    Boolean f = true;
        //    string separator = string.Empty;
        //    string headerTxt = string.Empty;
        //    if (ExportFileName.Substring(ExportFileName.Length - 3, 3) == "csv")
        //    {
        //        separator = ",";
        //    }

        //    if (ExportFileName.Substring(ExportFileName.Length - 3, 3) == "tsv")
        //    {
        //        separator = "\t";
        //    }


        //    try
        //    {
        //        if (System.IO.File.Exists(ExportFileName))
        //        {
        //            System.IO.File.Delete(ExportFileName);
        //        }

        //        System.IO.StreamWriter sw = new System.IO.StreamWriter(
        //                ExportFileName
        //                , false
        //                , System.Text.Encoding.GetEncoding("shift_jis")

        //        );


        //        for (int r = 0; r < dg.Rows.Count; r++)
        //        {




        //            string line = string.Empty;

        //            if (needHeader && r == 0)
        //            {
        //                for (int c = startColomns; c < dg.Columns.Count; c++)
        //                {

        //                    if (line != "") { line += separator; }
        //                    line += dg.Columns[c].HeaderText;
        //                }

        //                sw.WriteLine(line);
        //                line = string.Empty;
        //            }



        //            for (int c = startColomns; c < dg.Columns.Count; c++)
        //            {

        //                if (line != "") { line += separator; }
        //                line += dg.Rows[r].Cells[c].Value.ToString();

        //            }

        //            sw.WriteLine(line);
        //        }

        //        sw.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        f = false;
        //        ErrLog.logger.log(ex);
        //    }
        //    return f;
        //}


        //public static string[] getOpenFiles()
        //{

        //    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();


        //    ofd.FileName = "";


        //    ofd.Filter = "カンマ区切り(*.csv)|*.csv|タブ区切り(.tcv)|*.tsv";

        //    ofd.FilterIndex = 1;

        //    ofd.Title = "開くファイルを選択してください";

        //    ofd.RestoreDirectory = true;
        //    ofd.CheckFileExists = true;

        //    ofd.CheckPathExists = true;
        //    ofd.Multiselect = true;

        //    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        return ofd.FileNames;
        //    }
        //    else
        //    {
        //        return null;
        //    }


        //}





        public static void setLocalIP()
        {
            string LocalIP = "";

            // ホスト名を取得する
            string hostname = System.Net.Dns.GetHostName();
            // ホスト名からIPアドレスを取得する
            System.Net.IPAddress[] adrList = System.Net.Dns.GetHostAddresses(hostname);
            foreach (System.Net.IPAddress address in adrList)
            {
                string ip = address.ToString();
                string sfx;

                if (ip.IndexOf(".") > 0)
                {

                    sfx = ip.Substring(0, ip.IndexOf("."));


                    double d;
                    if (double.TryParse(sfx, out d))
                    {
                        LocalIP = ip;
                        break;
                    }
                }

            }



            pcIpAddress = LocalIP;
        }

        public static string getSH1String(string src)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sh1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] bs = sh1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(src));

            sh1.Clear();

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }


            return result.ToString().ToUpper();

        }

        public static Boolean isDate(string s)
        {
            try
            {
                DateTime.Parse(s);
            }
            catch (StackOverflowException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch
            {
                return false;
            }

            return true;


        }


        public static Boolean chkFLD(string fld)
        {

            Boolean f = true;

            try
            {
                if (!System.IO.Directory.Exists(fld))
                {
                    System.IO.Directory.CreateDirectory(fld);

                }
            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
                f = false;
            }

            return f;


        }
        public static string getIniSetting(int setting_id)
        {
            string iniFile = @"C:\Users\Administrator\Documents\Visual Studio 2013\WebSites\WebFormDemo\MySetting.ini";
            string setting_value = string.Empty;



            try
            {

                if (!System.IO.File.Exists(iniFile))
                {
                    throw new Exception("設定ファイルが見つかりません。");
                }
                //int counter = 0;
                string line;
                char[] delimiterChars = { '\t' };

                System.IO.StreamReader file = new System.IO.StreamReader(iniFile);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length != 0)
                    {
                        if (line.Substring(0, 2) != "//")
                        {
                            string[] val = line.Split(delimiterChars);
                            if (setting_id.ToString() == val[0])
                            {
                                setting_value = val[2];
                                break;
                            }
                        }
                    }
                }

                file.Close();





            }
            catch (Exception ex)
            {
                ErrLog.logger.log(ex);
            }


            return setting_value;

        }



        public string escape(string param)
        {
            string str = param;
            str = str.Replace("'", "''");
            str = str.Replace("%", "[%]");


            return str;
        }


        public static string getCaption(string fn = "")
        {


            string v = fn + "[ " + AplName + " ]-Ver." + APlVerSion;

            return v;
        }


        public static string EncryptString(string sourceString, string password)
        {

            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);


            System.Security.Cryptography.ICryptoTransform encryptor =
                rijndael.CreateEncryptor();

            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            encryptor.Dispose();


            return System.Convert.ToBase64String(encBytes);
        }

        public static Boolean ChkLoginUser(string t)
        {
            Boolean f = true;

            if (DecryptString(t, System.DateTime.Now.ToString("yyyyMMdd"))=="")
            {
                f = false;
            }
            return f;
        }

        public static string DecryptString(string sourceString, string password)
        {
            string bf = string.Empty;
            try
            {
                System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();


                byte[] key, iv;
                GenerateKeyFromPassword(
                    password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                byte[] strBytes = System.Convert.FromBase64String(sourceString);
                System.Security.Cryptography.ICryptoTransform decryptor =
                    rijndael.CreateDecryptor();

                byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                decryptor.Dispose();
                bf =  System.Text.Encoding.UTF8.GetString(decBytes);
            } catch(Exception ex)
            {
                ErrLog.logger.log(ex);
                bf = "";
            }


            return bf;
        }


        public static Boolean isNumeric(string num)
        {
            Boolean f = false;


            double d;
            if (double.TryParse(num, out d))
            {
                f = true;
            }

            return f;
        }
        private static void GenerateKeyFromPassword(string password,
            int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            //パスワードから共有キーと初期化ベクタを作成する
            //saltを決める
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("38UDSF9KUW3MK4FPX9WCPFANHGBT");
            //Rfc2898DeriveBytesオブジェクトを作成する
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
                new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
            //.NET Framework 1.1以下の時は、PasswordDeriveBytesを使用する
            //System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
            //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
            //反復処理回数を指定する デフォルトで1000回
            deriveBytes.IterationCount = 1000;

            //共有キーと初期化ベクタを生成する
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }

        public static string NumberFormat(string v, int point = 0)
        {
            v = v.Replace(",", "");
            v = v.Replace("%", "");
            v = v.Replace("+", "");
            double dl;

            if (!double.TryParse(v, out dl))
            {
                v = "-";
            }
            else
            {
                if (point == 0)
                {
                    v = Convert.ToDouble(v).ToString("#,0");
                }
                else
                {
                    v = Convert.ToDouble(v).ToString("F");
                    //Console.WriteLine(v);
                }



            }
            return v;
        }

        public static string ConvStr(int v)
        {
            string ret = string.Empty;

            ret = Convert.ToInt32(v).ToString("#,##0");

            return ret;
        }

    }
    class KeyValuePair
    {
        int key;
        string val;

        /// <summary>
        /// キーと値を設定するコンストラクタ
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="val">値</param>
        public KeyValuePair(int key, string val)
        {
            this.key = key;
            this.val = val;
        }

        /// <summary>
        /// キー
        /// </summary>
        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 値
        /// </summary>
        public string Value
        {
            get { return val; }
            set { val = value; }
        }

        /// <summary>
        /// 表示される値
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return val;
        }
    }
}