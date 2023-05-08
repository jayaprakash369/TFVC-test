using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------

    // =========================================================
    protected void Pre_Init(object sender, EventArgs e)
    {
    }
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
        //this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("https://www.scantron.com");

        //Page.ClientScript.RegisterClientScriptInclude("jsBanner", "/public/js/HomeBanner.js");
        //Page.ClientScript.RegisterClientScriptInclude("jsFade", "/public/js/gray/fade/doFade.js");
    }
    // =========================================================
    // =========================================================
}
