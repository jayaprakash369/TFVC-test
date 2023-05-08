using System;

public partial class public_marketing_win10Migration_Welcome : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sResult = "";

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
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
