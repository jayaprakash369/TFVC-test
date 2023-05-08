using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
public partial class public_Demo : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbMenuTitle.Text = "Demo";
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
    // =========================================================
}
