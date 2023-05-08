using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class public_sc_AddNote : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForTicket sft = new SourceForTicket();
    SourceForDefaults sfd = new SourceForDefaults();
    //string sCs1Changed = "";
    // Also a 'MyPage' field so just using it here for simplicity
    //iCs1User = 0;
    int iCtr = 0;
    int iTck = 0;

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
                iCs1User = 0;

            string sTckEncrypt = Request.QueryString["key"].ToString();

            int[] iaCtrTck = sfd.GetTicketDecrypted(sTckEncrypt);
            hfCtr.Value = iaCtrTck[0].ToString();
            hfTck.Value = iaCtrTck[1].ToString();

            txSubject.Focus();
        }
        LoadTckDetail();
    }
    // =========================================================
    protected void LoadTckDetail()
    {
        if ((hfCtr.Value != "") && (hfTck.Value != "")) 
        {
            if (int.TryParse(hfCtr.Value, out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(hfTck.Value, out iTck) == false)
                iTck = 0;

            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTicketDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);
        }
    }
    // =========================================================
    protected void btNoteEntry_Click(object sender, EventArgs e)
    {
        if (txMessage.Text != "" && txMessage.Text.Length > 1000)
        {
            txMessage.Text = txMessage.Text.Substring(0, 1000);
            vCus_Note.ErrorMessage = "Your message is limited to 1000 characters. (Your entry has been truncated to the maximum length.)";
            vCus_Note.IsValid = false;
            txMessage.Focus();
        }
        else
        {
            int iRowsAffected = 0;

            if (int.TryParse(hfCtr.Value, out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(hfTck.Value, out iTck) == false)
                iTck = 0;

            if (sPageLib == "L")
            {
                iRowsAffected = wsLive.AddNoteToTicket(sfd.GetWsKey(), iCtr, iTck, "CUST", txSubject.Text, txMessage.Text);
            }
            else
            {
                iRowsAffected = wsTest.AddNoteToTicket(sfd.GetWsKey(), iCtr, iTck, "CUST", txSubject.Text, txMessage.Text);
            }

            string sTckEncrypt = sfd.GetTicketEncrypted(iCtr, iTck);
            Response.Redirect("~/public/sc/TicketDetail.aspx?key=" + sTckEncrypt, false);
        }
    }
    // =========================================================
    // =========================================================
}