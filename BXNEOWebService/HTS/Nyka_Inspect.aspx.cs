using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Web.Services;
using System.Data;



namespace BXWebService.HTS
{
    public partial class Nyka_Inspect : System.Web.UI.Page
    {
        public string tantoEncode;
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
        }       
    }
}