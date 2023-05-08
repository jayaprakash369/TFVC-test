using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
//using System.Data.Odbc;
using System.Configuration;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;

public partial class public_api_openDns_Test0 : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    protected static ErrorHandler eh;
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); this.RequireSSL = false; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ===========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Api_OpenDns aod = new Api_OpenDns();
            DataTable dt = aod.GetOpenDnsCustomers();
            if (dt.Rows.Count > 0)
                lbMsg.Text = dt.Rows[0][1].ToString().Trim();
            else
                lbMsg.Text = "No data found";
        }
    }
    // ===========================================================
    // ===========================================================
}

