using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_Test : MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("Test Site");
        Response.End();
    }
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
}