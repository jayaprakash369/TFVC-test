using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_ss_Tracking : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();

    SelfService_LIVE.SelfServiceMenuSoapClient wsLiveSs = new SelfService_LIVE.SelfServiceMenuSoapClient();
    SelfService_DEV.SelfServiceMenuSoapClient wsTestSs = new SelfService_DEV.SelfServiceMenuSoapClient();

    SourceForDefaults sfd = new SourceForDefaults();
    SourceForCustomer sfc = new SourceForCustomer();

    DataTable dataTable;

    string sCs1Changed = "";
    string sMethodName = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            getSsTechs(iCs1ToUse);
            getSsStockingLocations(iCs1ToUse);
            inzSsTechNum();
            inzSsStockLoc();

            DateTime datTemp = new DateTime();
            datTemp = DateTime.Today;
            calEnd.SelectedDate = datTemp;
            calStart.SelectedDate = datTemp.AddMonths(-2);
            calStart.VisibleDate = datTemp.AddMonths(-2);

            if (ddStockLoc.Items.Count == 1)
                loadPanelShipments();

        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        if (ddStockLoc.Items.Count == 1)
            loadPanelShipments();
    }

    // =========================================================
    protected void getSsTechs(int cs1)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsTechs(sfd.GetWsKey(), cs1);
        }
        else
        {
            dataTable = wsTestSs.GetSsTechs(sfd.GetWsKey(), cs1);
        }
        if (dataTable.Rows.Count > 0) 
        {
            ddTechNum.DataSource = dataTable;
            ddTechNum.DataValueField = "TechNum";
            ddTechNum.DataTextField = "TechNam";
            ddTechNum.DataBind();
        }
    }
    // =========================================================
    protected void getSsStockingLocations(int cs1)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsStockLocs(sfd.GetWsKey(), cs1);
        }
        else
        {
            dataTable = wsTestSs.GetSsStockLocs(sfd.GetWsKey(), cs1);
        }
        if (dataTable.Rows.Count > 0)
        {
            ddStockLoc.DataSource = dataTable;
            ddStockLoc.DataValueField = "StockLocNum";
            ddStockLoc.DataTextField = "StockLocDisplay";
            ddStockLoc.DataBind();
        }
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        CheckCs1Changed();

        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iPrimaryCs1 = iCs1User;
        int iCs1Change = 0;

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (tbCs1Change.Visible == false)
                    tbCs1Change.Visible = true;

                if (txCs1Change.Text != "")
                {
                    if (int.TryParse(txCs1Change.Text, out iCs1Change) == false)
                        iCs1Change = 0;
                    else
                    {
                        if (iCs1Change > 0)
                        {
                            Session["adminCs1"] = txCs1Change.Text;
                            iPrimaryCs1 = iCs1Change;
                            if (sCs1Changed == "YES")
                            {
                                Session["ssTechNum"] = null;
                                Session["ssStockLoc"] = null;
                                getSsTechs(iCs1Change);
                                getSsStockingLocations(iCs1Change);
                                string sGoToMenu = sfd.checkGoToMenu("SspSsb", iPrimaryCs1);
                                if (sGoToMenu == "GO")
                                    Response.Redirect("~/private/shared/Menu.aspx", false);
                            }
                        }
                    }
                }
                else
                {
                    if (Session["adminCs1"] != null)
                    {
                        if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Change) == false)
                            iCs1Change = 0;
                        else
                        {
                            if (iCs1Change > 0)
                            {
                                txCs1Change.Text = iCs1Change.ToString();
                                iPrimaryCs1 = iCs1Change;
                            }
                        }
                    }
                    else
                    {
                        txCs1Change.Text = iCs1User.ToString();
                        Session["adminCs1"] = txCs1Change.Text;
                    }
                }
            }
        }

        return iPrimaryCs1;
    }
    // =========================================================
    protected string CheckCs1Changed()
    {
        sCs1Changed = "NO";

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (Session["adminCs1"] != null)
                {
                    // Admin changed cust but did not click change button
                    int iCs1Session = 0;
                    int iCs1Textbox = 0;
                    if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Session) == false)
                        iCs1Session = 0;
                    if (int.TryParse(txCs1Change.Text, out iCs1Textbox) == false)
                        iCs1Textbox = 0;

                    if ((iCs1Session != iCs1Textbox) && (txCs1Change.Text != ""))
                        sCs1Changed = "YES";
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    protected void btInput_Click(object sender, EventArgs e)
    {
        loadPanelShipments();
    }
    // =========================================================
    protected void inzSsTechNum()
    {
        int iSsTechNum = 0;
        if (Session["ssTechNum"] != null)
        {
            if (int.TryParse(Session["ssTechNum"].ToString(), out iSsTechNum) == false)
                iSsTechNum = 0;
            else
            {
                if (iSsTechNum > 0)
                {
                    ddTechNum.SelectedValue = iSsTechNum.ToString();
                }
            }
        }
        else
        {
            Session["ssTechNum"] = ddTechNum.SelectedValue.ToString();
        }
    }
    // =========================================================
    protected void ddTechNum_Changed(object sender, EventArgs e)
    {
        Session["ssTechNum"] = ddTechNum.SelectedValue.ToString();
    }
    // =========================================================
    protected void inzSsStockLoc()
    {
        int iSsStockLoc = 0;
        if (Session["ssStockLoc"] != null)
        {
            if (int.TryParse(Session["ssStockLoc"].ToString(), out iSsStockLoc) == false)
                iSsStockLoc = 0;
            else
            {
                if (iSsStockLoc > 0)
                {
                    ddStockLoc.SelectedValue = iSsStockLoc.ToString();
                }
            }
        }
        else
        {
            Session["ssStockLoc"] = ddStockLoc.SelectedValue.ToString();
        }
    }
    // =========================================================
    protected void ddStockLoc_Changed(object sender, EventArgs e)
    {
        Session["ssStockLoc"] = ddStockLoc.SelectedValue.ToString();
        loadPanelShipments();
    }
    // =========================================================
    protected void loadPanelShipments()
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        int iDateStart = 0;
        int iDateEnd = 0;
        
        if (int.TryParse(calStart.SelectedDate.ToString("yyyyMMdd"), out iDateStart) == false)
            iDateStart = 0;
        if (int.TryParse(calEnd.SelectedDate.ToString("yyyyMMdd"), out iDateEnd) == false)
            iDateEnd = 0;

//        int iTech = 0;
        int iStockLoc = 0;
//        if (int.TryParse(ddTechNum.SelectedValue.ToString(), out iTech) == false)
//            iTech = 0;
        if (int.TryParse(ddStockLoc.SelectedValue.ToString(), out iStockLoc) == false)
            iStockLoc = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsShipments(sfd.GetWsKey(), iStockLoc, iDateStart, iDateEnd);
        }
        else
        {
            dataTable = wsTestSs.GetSsShipments(sfd.GetWsKey(), iStockLoc, iDateStart, iDateEnd);
        }
        gvShipments.DataSource = dataTable;
        gvShipments.DataBind();
    }
    // =========================================================
    protected void lkTransfer_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sTransfer = linkControl.CommandArgument.ToString();
        int iTransfer = 0;
        if (int.TryParse(sTransfer, out iTransfer) == false)
            iTransfer = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsTransferParts(sfd.GetWsKey(), iTransfer);
        }
        else
        {
            dataTable = wsTestSs.GetSsTransferParts(sfd.GetWsKey(), iTransfer);
        }
        gvTransfer.DataSource = dataTable;
        gvTransfer.DataBind();

        lbTransfer.Text = "Parts Shipped on Transfer " + iTransfer.ToString();
        pnCs1Change.Visible = false;
        pnInput.Visible = false;
        pnShipments.Visible = false;
        pnTransfer.Visible = true;
    }
    // =========================================================
    protected void lkShipTo_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sTransfer = linkControl.CommandArgument.ToString();
        int iTransfer = 0;
        if (int.TryParse(sTransfer, out iTransfer) == false)
            iTransfer = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsTransferShipTo(sfd.GetWsKey(), iTransfer);
        }
        else
        {
            dataTable = wsTestSs.GetSsTransferShipTo(sfd.GetWsKey(), iTransfer);
        }
        rpShipTo.DataSource = dataTable;
        rpShipTo.DataBind();

        lbShipTo.Text = "Shipping Information For Transfer " + iTransfer.ToString();
        pnCs1Change.Visible = false;
        pnInput.Visible = false;
        pnShipments.Visible = false;
        pnShipTo.Visible = true;
    }

    // =========================================================
    // =========================================================
}
