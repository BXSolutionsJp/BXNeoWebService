using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BXWebService.HTS
{
    public partial class login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TANTO_CD"] != null)
            {
                string url = "menu.aspx?TANTO_CD=" + Request.QueryString["TANTO_CD"];
                Response.Redirect(url);
            }
        }
    }
}