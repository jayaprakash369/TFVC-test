using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;

public partial class private_sc_req_Result : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    SourceForCustomer sfc = new SourceForCustomer();

    char[] cSplitter = { ',' };
    char[] cSplitter2 = { '|' };
    char[] cSplitter3 = { '~' };

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
            // I don't think this one is being used...
            Response.Cookies["clientPage"].Value = "Result";
            if (Session["reqCs1"] != null) 
            {
                hfCs1.Value = Session["reqCs1"].ToString();
                Session["reqCs1"] = null;
            }

            if (Session["reqCs2"] != null) 
            {
                hfCs2.Value = Session["reqCs2"].ToString();
                Session["reqCs2"] = null;
            }

            if (Session["reqPhone"] != null) 
            {
                hfPhone.Value = Session["reqPhone"].ToString();
                Session["reqPhone"] = null;
            }

            if (Session["reqExtension"] != null) 
            {
                hfExtension.Value = Session["reqExtension"].ToString();
                Session["reqExtension"] = null;
            }

            if (Session["reqContact"] != null) 
            {
                hfContact.Value = Session["reqContact"].ToString();
                Session["reqContact"] = null;
            }

            if (Session["reqReqKey"] != null) 
            {
                hfReqKey.Value = Session["reqReqKey"].ToString().Trim();
                Session["reqReqKey"] = null;
            }

//            if (Session["reqParmList"] != null)
//            {
//                hfReqParmList.Value = Session["reqParmList"].ToString().Trim();
//                Session["reqParmList"] = null;
//            }

            Session["reqSource"] = "Result"; 

            LoadPanelResult();
        }
    }
    // =========================================================
    // START CLICK METHODS
    // =========================================================
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    protected void LoadPanelResult()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iKey = 0;
        int.TryParse(hfReqKey.Value, out iKey);

        if (iKey > 0)
        {
            if (sPageLib == "L")
            {
                dataTable = wsLive.GetRequestResult(sfd.GetWsKey(), iKey);
            }
            else
            {
                dataTable = wsTest.GetRequestResult(sfd.GetWsKey(), iKey);
            }
        }
        gvResult.DataSource = dataTable;
        gvResult.DataBind();

        int iCtr = 0;
        int iTck = 0;
        string sManualEntry = "";
        string sTicketFailure = "";
        lbNoTicket.Text = "";

        int iRowIdx = 0;
        foreach (DataRow row in dataTable.Rows)
        {
            if (int.TryParse(dataTable.Rows[iRowIdx]["Center"].ToString(), out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(dataTable.Rows[iRowIdx]["Ticket"].ToString(), out iTck) == false)
                iTck = 0;
            sManualEntry = dataTable.Rows[iRowIdx]["Delay"].ToString();
            if ((iCtr == 0 || iTck == 0) && sManualEntry != "Manual Entry")
                sTicketFailure = "Y";

            iRowIdx++;
        }
        if (sTicketFailure == "Y") 
        {
            lbNoTicket.Text = "<br /><br /><br /><br /><span style=\"font-size: 16px; line-height: 25px;\"><b><span style=\"color: #94002c; font-size: 18px;\">Ticket Creation Failure:</span></b><br />Ticket 0-0 indicates an error has occurred and this ticket was NOT created.  <br />Please phone our contact center at <b>1.800.228.3628</b> to place this request.<br /> (We apologize for the inconvenience.)</span>";
            lbNoTicket.Visible = true;
        }


    }
    // =========================================================
    protected void btAnotherRequest_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/private/sc/req/Location.aspx", false);
    }
    // =========================================================
    protected void btOpenTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/private/sc/OpenTickets.aspx", false);
    }
    // =========================================================
    // =========================================================
}

