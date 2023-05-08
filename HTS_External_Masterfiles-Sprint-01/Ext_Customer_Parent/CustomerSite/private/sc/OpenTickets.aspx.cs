using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_sc_OpenTickets : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForTicket sft = new SourceForTicket();
    SourceForDefaults sfd = new SourceForDefaults();
    string sCs1Changed = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbError.Visible = false;
        // 02/26/2020     int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        int iCs1ToUse = 89866;
        if (!IsPostBack)
        {
            ViewState["vsDataTable_Tck"] = null;
            BindGrid_Tck();
        }
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        string sMapTickets = "";

        if (sPageLib == "L")
        {
            sMapTickets = wsLive.GetPrefMapTickets(sfd.GetWsKey(), iCs1ToUse);
        }
        else
        {
            sMapTickets = wsTest.GetPrefMapTickets(sfd.GetWsKey(), iCs1ToUse);
        }
        if (sMapTickets == "YES")
        {
            pnMap.Visible = true;

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetTicketSummaryByModel(sfd.GetWsKey(), iCs1ToUse);
            }
            else
            {
                dataTable = wsTest.GetTicketSummaryByModel(sfd.GetWsKey(), iCs1ToUse);
            }
            gvModels.DataSource = dataTable;
            gvModels.DataBind();
        }
        else
        {
            pnMap.Visible = false;
        }
    }
    // =========================================================
    protected void lbTicket_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        string sTckEncrypt = sfd.GetTicketEncrypted(iCtr, iTck);
        Response.Redirect("~/public/sc/TicketDetail.aspx?key=" + sTckEncrypt, false);
    }
    // =========================================================
    protected void lbStatus_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnCs1Change.Visible = false;
            pnTickets.Visible = false;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTimestampDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected void lbPartUse_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnCs1Change.Visible = false;
            pnTickets.Visible = false;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetPartUseDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected void gvPageIndexChanging_Tck(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvTickets.PageIndex = newPageIndex;
        BindGrid_Tck();
    }
    // =========================================================
    protected void BindGrid_Tck()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        if (ViewState["vsDataTable_Tck"] == null)
        {
            lbError.Text = "";
            int iCs1 = GetPrimaryCs1();

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetTicketSummaryOpen(sfd.GetWsKey(), iCs1);
            }
            else
            {
                dataTable = wsTest.GetTicketSummaryOpen(sfd.GetWsKey(), iCs1);
            }
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Tck"] = dataTable;

            if (dataTable.Rows.Count == 0)
            {
                lbError.Text = "No open tickets were found...";
                lbError.Visible = true;
            }
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Tck"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Tck == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Tck + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Tck + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression;

        gvTickets.DataSource = dataTable.DefaultView;
        gvTickets.DataBind();

    }
    // =========================================================
    protected void gvSorting_Tck(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Tck == SortDirection.Ascending)
                gridSortDirection_Tck = SortDirection.Descending;
            else
                gridSortDirection_Tck = SortDirection.Ascending;
        }
        else
            gridSortDirection_Tck = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Tck = sortExpression;
        // Rebind the grid to its data source
        BindGrid_Tck();
    }
    private SortDirection gridSortDirection_Tck
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection"] == null)
            {
                //ViewState["GridSortDirection"] = SortDirection.Ascending;
                ViewState["GridSortDirection"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection"];
        }
        set
        {
            ViewState["GridSortDirection"] = value;
        }
    }
    private string gridSortExpression_Tck
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression"] == null)
            {
                //ViewState["GridSortExpression"] = "alpCtrTck"; // was ModelXref
                ViewState["GridSortExpression"] = "DateEntered";
            }
            return (string)ViewState["GridSortExpression"];
        }
        set
        {
            ViewState["GridSortExpression"] = value;
        }
    }

    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (txCs1Change.Text != "")
            {
                pnTickets.Visible = true;
                if (txCs1Change.Text.Length > 7)
                    txCs1Change.Text = txCs1Change.Text.Substring(0, 7);
                int iCs1Admin = 0;
                if (int.TryParse(txCs1Change.Text, out iCs1Admin) == false)
                {
                    iCs1Admin = 0;
                }
                else
                {
                    ViewState["vsDataTable_Tck"] = null;
                    BindGrid_Tck();
                }
            }
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
                                string sGoToMenu = sfd.checkGoToMenu("RegLrgDlrSsb", iPrimaryCs1);
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
    protected void btMapAll_Click(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1();
        Session["mapCs1"] = iCs1ToUse.ToString();
        Session["mapMod"] = null;
        //string sUrl = "http://www.scantronts.com/SOU_opn.asp?custNum=" + iCs1ToUse.ToString();
        //string sUrl = "~/private/sc/map/Cs1Tickets.aspx?cs1=" + iCs1ToUse.ToString();
        //string sUrl = "http://www.scantronts.com/private/sc/map/Cs1Tickets.aspx?cs1=" + iCs1ToUse.ToString();
        // 02/26/2020  string sUrl = "http://www.scantronts.com/private/sc/map/Cs1Tickets.aspx";

        string sUrl = "http://www.scantronts.com/private/sc/map/OpenTicketMap.aspx?cs1=" + iCs1ToUse.ToString();
        Response.Redirect(sUrl, false);
    }
    // =========================================================
    protected void lkMapModel_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sMod = linkControl.CommandArgument.ToString();

        int iCs1ToUse = GetPrimaryCs1();
        Session["mapCs1"] = iCs1ToUse.ToString();
        Session["mapMod"] = sMod;
        // 02/26/2020   string sUrl = "http://www.scantronts.com/private/sc/map/Cs1Tickets.aspx";
        string sUrl = "http://www.scantronts.com/private/sc/map/OpenTicketMap.aspx";
        Response.Redirect(sUrl, false);
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

                    if (iCs1Session != iCs1Textbox)
                        sCs1Changed = "YES";
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    // =========================================================
}