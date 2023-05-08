using System;

public partial class public_marketing_MpsSmb_Welcome : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sResult = "";

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Redirect("https://www.scantron.com/technology-solutions/managed-print-services/mps-smb-html/");
        Response.Redirect("https://www.scantron.com/technology-solutions/managed-print-services/mps-smb/");

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
