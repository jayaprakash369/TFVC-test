using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
//using System.Data;
//using System.Data.SqlClient;
using System.Configuration;

public partial class Scantron_HeaderFooter : System.Web.UI.MasterPage
{
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("jsGetXY", "/public/js/scantron/GetXY.js");
        Page.ClientScript.RegisterClientScriptInclude("jsParentNav", "/public/js/scantron/ParentNav.js");
        lbFooterYear.Text = DateTime.Now.Year.ToString("");
        /*
        string sUserURL = Request.Url.ToString();
        lbSiteID.Text = "";
        if (sUserURL.Contains("localhost"))
            lbSiteID.Text = "Localhost";
        else if (sUserURL.Contains(":380") || sUserURL.Contains(":1380"))
            lbSiteID.Text = "Isabel Dev";
        else if (sUserURL.Contains(":480") || sUserURL.Contains(":1480"))
            lbSiteID.Text = "Steve Dev";
        else if (sUserURL.Contains(":82") || sUserURL.Contains(":9082"))
            lbSiteID.Text = "Test";
        else if (sUserURL.Contains(":83") || sUserURL.Contains(":9083"))
            lbSiteID.Text = "Dev";

        if (Page.User.Identity.IsAuthenticated)
        {
            if (Page.User.IsInRole("Administrator"))
            {
                int iUsersOnline = Membership.GetNumberOfUsersOnline();
                lbUsersOnline.Text = "Logins: " + iUsersOnline.ToString();
                lbUsersOnline.Visible = true;
            }
        }
        */
    }
    // =========================================================
}
