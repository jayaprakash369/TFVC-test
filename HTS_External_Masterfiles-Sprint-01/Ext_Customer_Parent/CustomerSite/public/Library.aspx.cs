using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_Library : MyPage
{
    // =========================================================
    protected void Page_Init(object sender, EventArgs e) 
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e) 
    {
        Response.Redirect("https://www.scantron.com/resources/");
    }
    // =========================================================
}