using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class private_sc_TicketStatus : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";
        if (!IsPostBack)
        {
            Get_UserPrimaryCustomerNumber();

        }
    }

    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    protected void BindGrid_Tck(DataTable dt)
    {

        if (ViewState["vsDataTable_Tck"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Tck"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching ticket was found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Tck"];
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
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_TicketLarge.DataSource = dt.DefaultView;
        gv_TicketLarge.DataBind();

        if (dt.Rows.Count > 0)
            gv_TicketLarge.Visible = true;
        else
            gv_TicketLarge.Visible = false;


    }
    // ----------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Tck(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_TicketLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Tck(dt);
    }
    // ----------------------------------------------------------------------------------------------------
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
        DataTable dt = null;
        BindGrid_Tck(dt);
    }
    // ----------------------------------------------------------------------------------------------------
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
    // ----------------------------------------------------------------------------------------------------
    private string gridSortExpression_Tck
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression"] == null)
            {
                //ViewState["GridSortExpression"] = "alpCtrTck"; // was ModelXref
                ViewState["GridSortExpression"] = "DateEnteredSort";
            }
            return (string)ViewState["GridSortExpression"];
        }
        set
        {
            ViewState["GridSortExpression"] = value;
        }
    }
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B2TicketSummary(
        string customerNumber,
        string customerLocation,
        string ticketNumber,
        string openClosedOrAll,
        string startTimestamp,
        string endTimestamp,
        string serviceOrProject)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B2TicketSummary";
            string sFieldList = "customerNumber|customerLocation|ticketNumber|openClosedOrAll|startTimestamp|endTimestamp|serviceOrProject|x";
            string sValueList = customerNumber + "|" + customerLocation + "|" + ticketNumber + "|" + openClosedOrAll.ToString() + "|" + startTimestamp + "|" + endTimestamp + "|" + serviceOrProject + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketSummaryForAnyCall(int customerNumber, string ticketOrCrossRef)
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1TicketSummaryForAnyCall";
            string sFieldList = "customerNumber|ticketOrCrossRef|x";
            string sValueList = customerNumber.ToString() + "|" + ticketOrCrossRef + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketSummaryForOpenCalls(int customerNumber) 
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1TicketSummaryForOpenCalls";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected string ws_Get_B2TicketNoteCount(int ticketId, string serviceOrProject) 
    {
        string sNoteCount = "";

        if (ticketId > 0)
        {
            string sJobName = "Get_B2TicketNoteCount";
            string sFieldList = "ticketId|serviceOrProject|x";
            string sValueList = ticketId.ToString() + "|" + serviceOrProject + "|x";

            sNoteCount = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sNoteCount;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ----------------------------------------------------------------------------------------------------
    protected void LoadTicketDataTables()
    {
        DataTable dt = new DataTable("");
        DataTable dt1 = new DataTable("");
        DataTable dt2 = new DataTable("");
        DataTable dt3 = new DataTable("");

        try
        {
            string sTicketOrCrossRef = txSearchTicket.Text.Trim();

            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value) && !String.IsNullOrEmpty(sTicketOrCrossRef))
            {
                int iCustomerNumber = 0;

                int iPassedCenter = 0;
                int iPassedTicket = 0;
                int iPassedPotentialB2Ticket = 0;

                string[] saCtrTck = sTicketOrCrossRef.Split('-');
                if (saCtrTck.Length > 1)
                {
                    if (int.TryParse(saCtrTck[0], out iPassedCenter) == false)
                        iPassedCenter = -1;
                    if (int.TryParse(saCtrTck[1], out iPassedTicket) == false)
                        iPassedTicket = -1;
                }
                else if (saCtrTck.Length > 0) 
                {
                    if (int.TryParse(saCtrTck[0], out iPassedPotentialB2Ticket) == false)
                        iPassedPotentialB2Ticket = -1;
                }

                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;
                if (iCustomerNumber > 0 && !String.IsNullOrEmpty(sTicketOrCrossRef))
                {
                    dt1 = ws_Get_B1TicketSummaryForAnyCall(iCustomerNumber, sTicketOrCrossRef);
                    if (iPassedCenter <= 0 && iPassedTicket <= 0 && iPassedPotentialB2Ticket > 0)
                    {
                        dt2 = ws_Get_B2TicketSummary(iCustomerNumber.ToString(), "", iPassedPotentialB2Ticket.ToString(), "All", "", "", "Service");
                        dt3 = ws_Get_B2TicketSummary(iCustomerNumber.ToString(), "", iPassedPotentialB2Ticket.ToString(), "All", "", "", "Project");
                    }

                    dt = Merge_Tables(dt1, dt2, dt3);

                    rp_TicketSmall.DataSource = dt;
                    rp_TicketSmall.DataBind();

                    if (dt.Rows.Count > 0)
                        rp_TicketSmall.Visible = true;
                    else
                        rp_TicketSmall.Visible = false;

                    ViewState["vsDataTable_Tck"] = null;
                    BindGrid_Tck(dt);


                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Merge_Tables(DataTable dt1, DataTable dt2, DataTable dt3)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("TicketId"));
        dt.Columns.Add(MakeColumn("TicketIdSort"));
        dt.Columns.Add(MakeColumn("DateEnteredSort"));
        dt.Columns.Add(MakeColumn("DateEntered"));
        dt.Columns.Add(MakeColumn("CustName"));
        dt.Columns.Add(MakeColumn("City"));
        dt.Columns.Add(MakeColumn("State"));
        dt.Columns.Add(MakeColumn("Status"));
        dt.Columns.Add(MakeColumn("Summary"));
        dt.Columns.Add(MakeColumn("Source"));

        DateTime datTemp = new DateTime();
        DataRow dr;
        int iRowIdx = 0;
        int iCenter = 0;
        int iTicket = 0;
        int iId = 0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["TicketId"] = row["Center"].ToString().Trim() + "-" + row["Ticket"].ToString().Trim();
            if (int.TryParse(row["Center"].ToString().Trim(), out iCenter) == false)
                iCenter = -1;
            if (int.TryParse(row["Ticket"].ToString().Trim(), out iTicket) == false)
                iTicket = -1;
            dt.Rows[iRowIdx]["TicketIdSort"] = iCenter.ToString("000") + iTicket.ToString("0000000");
            dt.Rows[iRowIdx]["CustName"] = Fix_Case(row["CustName"].ToString().Trim());
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["City"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["State"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = Fix_Case(row["Remark"].ToString().Trim());
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Summary"].ToString().Trim());
            dt.Rows[iRowIdx]["Source"] = "1";

            if (DateTime.TryParse(row["DateEntered"].ToString().Trim(), out datTemp) == true) 
            {
                if (datTemp != new DateTime()) 
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }
               

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["TicketId"] = row["id"].ToString().Trim();
            if (int.TryParse(row["id"].ToString().Trim(), out iId) == false)
                iId = -1;
            dt.Rows[iRowIdx]["TicketIdSort"] = iId.ToString("000000000");
            dt.Rows[iRowIdx]["CustName"] = row["siteName"].ToString().Trim();
            dt.Rows[iRowIdx]["City"] = row["city"].ToString().Trim();
            dt.Rows[iRowIdx]["State"] = row["stateIdentifier"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = row["status-name"].ToString().Trim();
            dt.Rows[iRowIdx]["Summary"] = row["summary"].ToString().Trim();
            dt.Rows[iRowIdx]["Source"] = "2";

            //if (DateTime.TryParse(row["dateEntered"].ToString().Trim(), out datTemp) == true)
            if (DateTime.TryParse(row["dateResponded"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp != new DateTime())
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }

            iRowIdx++;
        }

        foreach (DataRow row in dt3.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["TicketId"] = row["id"].ToString().Trim();
            if (int.TryParse(row["id"].ToString().Trim(), out iId) == false)
                iId = -1;
            dt.Rows[iRowIdx]["TicketIdSort"] = iId.ToString("000000000");
            dt.Rows[iRowIdx]["CustName"] = row["siteName"].ToString().Trim();
            dt.Rows[iRowIdx]["City"] = row["city"].ToString().Trim();
            dt.Rows[iRowIdx]["State"] = row["stateIdentifier"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = row["status-name"].ToString().Trim();
            dt.Rows[iRowIdx]["Summary"] = row["summary"].ToString().Trim();
            dt.Rows[iRowIdx]["Source"] = "3";

            if (DateTime.TryParse(row["dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp != new DateTime())
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }

            iRowIdx++;
        }

        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 1)
                hfPrimaryCs1.Value = saPreNumTyp[1];

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0)
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
            }

            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
            if (iCustomerNumber > 0)
                hfPrimaryCs1Type.Value = ws_Get_B1CustomerType(iCustomerNumber);
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchTicketSubmit_Click(object sender, EventArgs e)
    {
        try 
        {
            LoadTicketDataTables();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkTicket_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('-');
        string sDataSource = "";  // 1=Building 1 Tickets, 2= Building 2 Service Tickets, 3= Building 2 Project Tickets
        int iCtr = 0;
        int iTck = 0;
        int iId = 0;
        if (saArg.Length > 2)
        {
            sDataSource = saArg[0];
            if (int.TryParse(saArg[1], out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(saArg[2], out iTck) == false)
                iTck = 0;
            if (iCtr > 0 && iTck > 0) 
            {
                string sTckEncrypt = GetTicketEncrypted(iCtr, iTck);
                Response.Redirect("~/public/sc/B1TicketDetail.aspx?key=" + sTckEncrypt, false);
            }
        }
        else if (saArg.Length > 1)
        {
            sDataSource = saArg[0];
            if (int.TryParse(saArg[1], out iId) == false)
                iId = 0;
            if (iId > 0) 
            {
                if (sDataSource == "2")
                    Response.Redirect("~/public/sc/B2TicketDetail.aspx?key=" + iId + "&typ=Service", false);
                else if (sDataSource == "3")
                    Response.Redirect("~/public/sc/B2TicketDetail.aspx?key=" + iId + "&typ=Project", false);
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkStatus_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            Response.Redirect("~/private/sc/Timestamps.aspx?ctr=" + iCtr.ToString() + "&tck=" + iTck.ToString(), false);
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkPartUse_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            Response.Redirect("~/private/sc/TicketPartUse.aspx?ctr=" + iCtr.ToString() + "&tck=" + iTck.ToString(), false);
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
