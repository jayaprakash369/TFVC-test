using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class public_utils_ZipMgr : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    DataTable dataTable;
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            txZip.Focus();
        }
    }
    // =========================================================
    protected void btZip_Click(object sender, EventArgs e)
    {
        loadZipDetail();
    }
    // =========================================================
    protected void loadZipDetail()
    {
        string sZip = txZip.Text.Trim();
        if ((sZip != "") && (sZip.Length > 7)) 
            sZip = sZip.Substring(0, 7);
        if ((sZip != "") && (sZip.Length == 7) && (sZip.Substring(3, 1) == " "))
            sZip = sZip.Substring(0, 3) + sZip.Substring(4, 3);


        string sZoneLetter = "";

        if (sPageLib == "L")
        {
            sZoneLetter = wsLive.GetZipZone(sfd.GetWsKey(), sZip);
            dataTable = wsLive.GetZipDetail(sfd.GetWsKey(), sZip);
        }
        else
        {
            sZoneLetter = wsTest.GetZipZone(sfd.GetWsKey(), sZip);
            dataTable = wsTest.GetZipDetail(sfd.GetWsKey(), sZip);
        }

        if (sZoneLetter == "MILITARY" || sZoneLetter == "INACTIVE")
        {
            if (sZoneLetter == "MILITARY")
                sZoneLetter = "US Military";
            if (sZoneLetter == "INACTIVE")
                sZoneLetter = "Inactivated";
            lbZoneLetter.Font.Size = 24;
        }

        lbZoneLetter.Text = sZoneLetter;

        rpZipDetail.DataSource = dataTable;
        rpZipDetail.DataBind();

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetZipMarket(sfd.GetWsKey(), sZip);
        }
        else
        {
            dataTable = wsTest.GetZipMarket(sfd.GetWsKey(), sZip);
        }

        rpZipMarket.DataSource = dataTable;
        rpZipMarket.DataBind();

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetZipCenter(sfd.GetWsKey(), sZip);
        }
        else
        {
            dataTable = wsTest.GetZipCenter(sfd.GetWsKey(), sZip);
        }

        rpZipCenter.DataSource = dataTable;
        rpZipCenter.DataBind();

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetZipFst(sfd.GetWsKey(), sZip);
        }
        else
        {
            dataTable = wsTest.GetZipFst(sfd.GetWsKey(), sZip);
        }

        rpZipFst.DataSource = dataTable;
        rpZipFst.DataBind();

        pnZipData.Visible = true;
    }
    // =========================================================
    // =========================================================
}