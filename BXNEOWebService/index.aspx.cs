using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BXWebService
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SLPUser"] == null)
            {
                Response.Redirect("./HTS/login.aspx");
            } else
            {
                Response.Redirect("./HTS/menu.aspx");
            }
        }
    }
}