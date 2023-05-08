using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterParent : System.Web.UI.MasterPage
{
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUserURL = Request.Url.ToString();
        lbSiteID.Text = ""; // Live shows blank
        if (sUserURL.Contains("localhost"))
            lbSiteID.Text = "Localhost";
        if (sUserURL.Contains(":92") || sUserURL.Contains(":9092"))
            lbSiteID.Text = "Test";
        if (sUserURL.Contains(":93") || sUserURL.Contains(":9093"))
            lbSiteID.Text = "Dev";
    }
    // =========================================================
    // =========================================================
}
