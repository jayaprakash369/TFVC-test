using System;
using System.Activities.Expressions;
using System.Data;
using System.Web.UI.WebControls;

public partial class private_sc_OpenTickets : MyPage
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

            LoadTicketDataTables();
        }
    }

    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Ticket Table (_Tck)
    // -------------------------------------------------------------------------------------------------
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
                lbMsg.Text = "No open tickets were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Tck"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Tck;
        if (gridSortDirection_Tck == SortDirection.Ascending)
        {
            sortExpression_Tck = gridSortExpression_Tck + " ASC";
        }
        else
        {
            sortExpression_Tck = gridSortExpression_Tck + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Tck;

        gv_TicketLarge.DataSource = dt.DefaultView;
        gv_TicketLarge.DataBind();

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
        string sortExpression_Tck = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Tck == e.SortExpression)
        {
            if (gridSortDirection_Tck == SortDirection.Ascending)
                gridSortDirection_Tck = SortDirection.Descending;
            else
                gridSortDirection_Tck = SortDirection.Ascending;
        }
        else
            gridSortDirection_Tck = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Tck = sortExpression_Tck;
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
    // -------------------------------------------------------------------------------------------------
    // END: Ticket Table (_Tck)
    // -------------------------------------------------------------------------------------------------

    // -------------------------------------------------------------------------------------------------
    // BEGIN: Model Table (_Mod)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Mod(DataTable dt)
    {
        string sReloadRepeater = "";

        if (ViewState["vsDataTable_Mod"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Mod"] = dt;

            if (dt.Rows.Count == 0)
            {
                //lbMsg.Text = "No matching locations found...";
                //lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Mod"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Mod;
        if (gridSortDirection_Mod == SortDirection.Ascending)
        {
            sortExpression_Mod = gridSortExpression_Mod + " ASC";
        }
        else
        {
            sortExpression_Mod = gridSortExpression_Mod + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Mod;

        gv_ModelLarge.DataSource = dt.DefaultView;
        gv_ModelLarge.DataBind();

        if (sReloadRepeater == "Y")
        {
            rp_ModelSmall.DataSource = dt.DefaultView;
            rp_ModelSmall.DataBind();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Mod(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_ModelLarge.PageIndex = newPageIndex;
        DataTable dt = new DataTable("");
        BindGrid_Mod(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Mod(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Mod = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Mod == e.SortExpression)
        {
            if (gridSortDirection_Mod == SortDirection.Ascending)
                gridSortDirection_Mod = SortDirection.Descending;
            else
                gridSortDirection_Mod = SortDirection.Ascending;
        }
        else
            gridSortDirection_Mod = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Mod = sortExpression_Mod;
        // Rebind the grid to its data source
        DataTable dt = new DataTable("");
        BindGrid_Mod(dt);

    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Mod
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Mod"] == null)
            {
                ViewState["GridSortDirection_Mod"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Mod"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Mod"];
        }
        set
        {
            ViewState["GridSortDirection_Mod"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Mod
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Mod"] == null)
            {
                ViewState["GridSortExpression_Mod"] = "Model";
            }
            return (string)ViewState["GridSortExpression_Mod"];
        }
        set
        {
            ViewState["GridSortExpression_Mod"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Model Table (_Mod)
    // -------------------------------------------------------------------------------------------------

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
    protected DataTable ws_Get_B1TicketSummaryByModel(
        string customerNumber,
        string customerLocation,
        string startDate8,
        string endDate8
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1TicketSummaryByModel";
            string sFieldList = "customerNumber|customerLocation|startDate8|endDate8|x";
            string sValueList = customerNumber + "|" + customerLocation + "|" + startDate8 + "|" + endDate8 + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketSummaryByModelForOpenCalls(
        string customerNumber
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1TicketSummaryByModelForOpenCalls";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

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
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;
                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;
                if (iCustomerNumber > 0)
                {
                    dt1 = ws_Get_B1TicketSummaryForOpenCalls(iCustomerNumber);
                    dt2 = ws_Get_B2TicketSummary(iCustomerNumber.ToString(), "", "", "Open", "", "", "Service");
                    dt3 = ws_Get_B2TicketSummary(iCustomerNumber.ToString(), "", "", "Open", "", "", "Project");

                    dt = Merge_Tables(dt1, dt2, dt3);

                    rp_TicketSmall.DataSource = dt;
                    rp_TicketSmall.DataBind();

                    ViewState["vsDataTable_Tck"] = null;
                    BindGrid_Tck(dt);
                    // -------------------------------------------
                    string sAllowTicketMapping_YN = ws_Get_B1CustPref_AllowTicketMapping_YN(iCustomerNumber.ToString());
                    if (sAllowTicketMapping_YN == "Y") // i.e. Container Store 89866
                    {
                        pnModel.Visible = true;
                        dt = ws_Get_B1TicketSummaryByModelForOpenCalls(
                            iCustomerNumber.ToString()
                            );
                        ViewState["vsDataTable_Mod"] = null;
                        BindGrid_Mod(dt);
                    }

                    // -------------------------------------------
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
        dt.Columns.Add(MakeColumn("DateEnteredSort"));
        dt.Columns.Add(MakeColumn("DateEntered"));
        dt.Columns.Add(MakeColumn("CustName"));
        dt.Columns.Add(MakeColumn("City"));
        dt.Columns.Add(MakeColumn("State"));
        dt.Columns.Add(MakeColumn("Status"));
        dt.Columns.Add(MakeColumn("Summary"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("PrimaryServiceCodeForCustomer"));
        dt.Columns.Add(MakeColumn("TicketXref1"));
        dt.Columns.Add(MakeColumn("TicketXref2"));
        dt.Columns.Add(MakeColumn("TicketXref3"));
        dt.Columns.Add(MakeColumn("TicketXrefsCombined"));
        dt.Columns.Add(MakeColumn("Source"));
        
        dt.Columns.Add(MakeColumn("TicketIdSort"));

        DateTime datTemp = new DateTime();
        DataRow dr;
        int iRowIdx = 0;
        int iCenter = 0;
        int iTicket = 0;
        int iCwId = 0;
        string sTemp = "";

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            if (int.TryParse(row["Center"].ToString().Trim(), out iCenter) == false)
                iCenter = -1;
            if (int.TryParse(row["Ticket"].ToString().Trim(), out iTicket) == false)
                iTicket = -1;

            dt.Rows[iRowIdx]["TicketId"] = row["Center"].ToString().Trim() + "-" + row["Ticket"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketIdSort"] = iCenter.ToString("000") + iTicket.ToString("0000000");
            dt.Rows[iRowIdx]["CustName"] = Fix_Case(row["CustName"].ToString().Trim());
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["City"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["State"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = Fix_Case(row["Remark"].ToString().Trim());
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Summary"].ToString().Trim());
            dt.Rows[iRowIdx]["Model"] = row["Model"].ToString().Trim();
            dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim();
            dt.Rows[iRowIdx]["PrimaryServiceCodeForCustomer"] = row["PrimaryServiceCodeForCustomer"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketXref1"] = row["TicketXref1"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketXref2"] = row["TicketXref2"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketXref3"] = row["TicketXref3"].ToString().Trim();

            if (!String.IsNullOrEmpty(row["TicketXref1"].ToString().Trim()))
                sTemp = row["TicketXref1"].ToString().Trim();

            if (!String.IsNullOrEmpty(row["TicketXref2"].ToString().Trim())
                && (row["TicketXref2"].ToString().Trim() != row["TicketXref1"].ToString().Trim())
                ) 
            {
                if (!String.IsNullOrEmpty(sTemp))
                    sTemp += " ";
                sTemp += row["TicketXref2"].ToString().Trim();
            }

            if (!String.IsNullOrEmpty(row["TicketXref3"].ToString().Trim())
                && (
                   row["TicketXref3"].ToString().Trim() != row["TicketXref1"].ToString().Trim()
                && row["TicketXref3"].ToString().Trim() != row["TicketXref2"].ToString().Trim()
                )
                )
            {
                if (!String.IsNullOrEmpty(sTemp))
                    sTemp += " ";
                sTemp += row["TicketXref3"].ToString().Trim();
            }

            dt.Rows[iRowIdx]["TicketXrefsCombined"] = sTemp;

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

            if (int.TryParse(row["id"].ToString().Trim(), out iCwId) == false)
                iCwId = -1;

            dt.Rows[iRowIdx]["TicketId"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketIdSort"] = iCwId.ToString("000000");

            dt.Rows[iRowIdx]["CustName"] = row["siteName"].ToString().Trim();
            dt.Rows[iRowIdx]["City"] = row["city"].ToString().Trim();
            dt.Rows[iRowIdx]["State"] = row["stateIdentifier"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = row["status-name"].ToString().Trim();
            dt.Rows[iRowIdx]["Summary"] = row["summary"].ToString().Trim();
            dt.Rows[iRowIdx]["Source"] = "2";

            // Trying the various date objects
            if (DateTime.TryParse(row["dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp != new DateTime())
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }
            else if (DateTime.TryParse(row["info-dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp != new DateTime())
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }
            else if (DateTime.TryParse(row["dateResponded"].ToString().Trim(), out datTemp) == true)
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

            if (int.TryParse(row["id"].ToString().Trim(), out iCwId) == false)
                iCwId = -1;

            dt.Rows[iRowIdx]["TicketId"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketIdSort"] = iCwId.ToString("000000");

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
