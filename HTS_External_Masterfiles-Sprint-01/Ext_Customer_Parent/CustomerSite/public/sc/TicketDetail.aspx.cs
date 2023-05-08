using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_sc_TicketDetail : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForTicket sft = new SourceForTicket();
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
        int iCs1 = 0;
        string sEncrypted = "";
        
        //sEncrypted = sfd.GetTicketEncrypted(iCtr, iTck);

        int[] iaCtrTck = new int[2];

        sEncrypted = Request.QueryString["key"];

        if ((sEncrypted == null) || (sEncrypted == ""))
        {
            lbMessage.Text = "A key must be passed to access ticket detail... ";
        }
        else
        {
            iaCtrTck = sfd.GetTicketDecrypted(sEncrypted);
            iCtr = iaCtrTck[0];
            iTck = iaCtrTck[1];

            // Testing? hard code ticket and library here (you also need to add library in SourceFor Ticket...)
            //iCtr = 450;
            //iTck = 154466;
            //sPageLib = "L";

            if ((iCtr == 0) || (iTck == 0))
            {
                lbMessage.Text = "The key did not correspond to a valid ticket number... ";
            }
            else
            {
                if (sPageLib == "L")
                {
                    iCs1 = wsLive.GetTicketCs1(sfd.GetWsKey(), iCtr, iTck);
                }
                else
                {
                    iCs1 = wsTest.GetTicketCs1(sfd.GetWsKey(), iCtr, iTck);
                }

                pnDisplay.Controls.Clear();
                Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

                pnTemp = sft.GetTicketDisplayPanel(iCtr, iTck, iCs1);
                pnDisplay.Controls.Add(pnTemp);
            }
        }
    }
    // =========================================================
    // =========================================================
}