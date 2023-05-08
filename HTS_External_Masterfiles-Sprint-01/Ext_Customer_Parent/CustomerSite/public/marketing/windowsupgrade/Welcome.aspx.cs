using System;

public partial class public_marketing_windowsupgrade_Welcome : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sResult = "";

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("https://www.scantron.com/assessment-solutions/educational-assessment/corporate/ready-your-network-for-the-future/");

        if (!IsPostBack)
        { 
        }
    }
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

        }
        // ------------------------------------
        this.RequireSSL = false;
    }
    // =========================================================
    // =========================================================
}
