using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Configuration;
using System.Data;
using System.IO;

public partial class public_marketing_formax_Welcome : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("https://www.scantron.com/formax/");
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
