using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_sc_map_EncryptTicket : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(Request.QueryString["ctr"], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(Request.QueryString["tck"], out iTck) == false)
            iTck = 0;

        string sEncrypted = sfd.GetTicketEncrypted(iCtr, iTck);
        Response.Write(sEncrypted);
        Response.End();
    }
    // =========================================================
    // =========================================================
}