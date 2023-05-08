using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dev_Test : MyPage
{
    // ===================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // ===================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
        Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
        sPageLib = "T";
        DataTable dt = new DataTable();

        if (sPageLib == "L")
        {
            dt = wsLive.GetLeasingDetails("", "abc");
        }
        else
        {
            dt = wsLive.GetLeasingDetails("", "abc");
        }

    }
    // ===================================================
    // ===================================================
}