using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dev_Email1 : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    string sReturn = "";
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    // =========================================================
    protected void btEmail_Click(object sender, EventArgs e)
    {
        string sTo = txTo.Text.Trim();
        string sSbj = txSbj.Text.Trim();
        string sMsg= txMsg.Text.Trim();

        if (sPageLib == "L")
        {
            
        }
        else
        {
            sReturn = wsTest.EmailTool(sfd.GetWsKey(), sTo, "adv320@scantron.com", sSbj, sMsg);
        }
        lbMessage.Text = sReturn;
    }
    // =========================================================
    // =========================================================
}