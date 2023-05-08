using System;

public partial class public_marketing_southernhills_dg_Welcome : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    //string sResult = "";

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("https://www.scantron.com/resources/southern-hills-community-bank-technology-services-case-study/");
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
