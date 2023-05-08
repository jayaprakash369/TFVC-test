using System;
using System.Activities.Expressions;
using System.Data;
using System.Web.UI.WebControls;

public partial class private_pre_ServiceHistory : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    // string sLibrary = "OMDTALIB";
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
                lbMsg.Text = "No matching tickets were found...";
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
    protected DataTable ws_Get_B1TicketHistoryForPrefixedAccounts(
        string accountType,
        string accountNumber,
        string openClosedAny,
        string center,
        string ticket,
        string ticketXref,
        string customerName,
        string customerNumber,
        string customerLocation,
        string city,
        string stateAbbreviation,
        string assigned,
        string remark,
        string problem,
        string enteredGT,
        string enteredLT,
        string closedGT,
        string closedLT
    )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(accountType) && !String.IsNullOrEmpty(accountNumber))
        {
            string sJobName = "Get_B1TicketHistoryForPrefixedAccounts";
            string sFieldList = "accountType|accountNumber|openClosedAny|center|ticket|ticketXref|customerName|customerNumber|customerLocation|city|stateAbbreviation|assigned|remark|problem|enteredGT|enteredLT|closedGT|closedLT|x";
            // accountType|accountNumber|openClosedAny|center|ticket|ticketXref|customerName|customerNumber|customerLocation|city|stateAbbreviation|assigned|remark|problem|enteredGT|enteredLT|closedGT|closedLT|x
            string sValueList = 
                accountType + "|" + 
                accountNumber + "|" + 
                openClosedAny + "|" +
                center + "|" +
                ticket + "|" +
                ticketXref + "|" +
                customerName + "|" +
                customerNumber + "|" +
                customerLocation + "|" +
                city + "|" +
                stateAbbreviation + "|" +
                assigned + "|" +
                remark + "|" +
                problem + "|" +
                enteredGT + "|" +
                enteredLT + "|" +
                closedGT + "|" +
                closedLT + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
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

        int iTemp = 0;
        string sTemp = "";

        string sOpenClosedAny = "";
        string sCenter = "";
        string sTicket = "";
        string sTicketXref = "";
        string sCustomerName = "";
        string sCustomerNumber = "";
        string sCustomerLocation = "";
        string sCity = "";
        string sStateAbbreviation = "";
        string sAssigned = "";
        string sRemark = "";
        string sProblem = "";
        string sEnteredGT = "";
        string sEnteredLT = "";
        string sClosedGT = "";
        string sClosedLT = "";
        
        string sDat = "";
        DateTime datTemp;

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;
                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;
                if (txSearchCenter.Text != "")
                {
                    if (int.TryParse(txSearchCenter.Text, out iTemp) == false || iTemp > 999 || iTemp < 1)
                    {
                        iTemp = -1;
                        lbMsg.Text = "The center entry must be a positive integer 3 digits or less";
                        txSearchCenter.Focus();
                    }
                    else
                        sCenter = iTemp.ToString("0");
                        
                }
                if (txSearchTicket.Text != "")
                {
                    if (int.TryParse(txSearchTicket.Text, out iTemp) == false || iTemp > 9999999 || iTemp < 1)
                    {
                        iTemp = -1;
                        if (lbMsg.Text != "") lbMsg.Text += "<br />";
                        lbMsg.Text += "The ticket entry must be a positive integer 7 digits or less";
                        txSearchTicket.Focus();
                    }
                    else
                        sTicket = iTemp.ToString("0");
                }
                if (txSearchTicketXref.Text != "")
                {
                    sTicketXref = scrub(txSearchTicketXref.Text.Trim());
                    if (sTicketXref.Length > 30)
                        sTicketXref = sTicketXref.Substring(0, 30);

                }
                if (txSearchCustomerName.Text != "")
                {
                    sCustomerName = scrub(txSearchCustomerName.Text.Trim());
                    if (sCustomerName.Length > 40)
                        sCustomerName = sCustomerName.Substring(0, 40);
                           
                }
                if (txSearchCustomerNum.Text != "")
                {
                    if (int.TryParse(txSearchCustomerNum.Text, out iTemp) == false || iTemp > 9999999 || iTemp < 1)
                    {
                        iTemp = -1;
                        if (lbMsg.Text != "") lbMsg.Text += "<br />";
                        lbMsg.Text += "The customer number entry must be a positive integer 7 digits or less";
                        txSearchCustomerNum.Focus();
                    }
                    else
                        sCustomerNumber = iTemp.ToString("0");
                }
                if (txSearchCustomerLoc.Text != "")
                {
                    if (int.TryParse(txSearchCustomerLoc.Text, out iTemp) == false || iTemp > 999 || iTemp < 1)
                    {
                        iTemp = -1;
                        if (lbMsg.Text != "") lbMsg.Text += "<br />";
                        lbMsg.Text += "The customer location entry must be a positive integer 3 digits or less";
                        txSearchCustomerNum.Focus();
                    }
                    else
                        sCustomerLocation = iTemp.ToString("0");
                }
                
                if (txSearchCity.Text != "")
                {
                    sCity = scrub(txSearchCity.Text.Trim());
                    if (sCity.Length > 25)
                        sCity = sCity.Substring(0, 25);

                }
                if (txSearchState.Text != "")
                {
                    sStateAbbreviation = scrub(txSearchState.Text.Trim());
                    if (sStateAbbreviation.Length > 2) 
                    {
                        sStateAbbreviation = sStateAbbreviation.Substring(0, 2);
                        lbMsg.Text += "The state/province entry must be a valid 2 char abbreviation";
                        txSearchState.Focus();
                    }
                }
                
                if (txSearchTech.Text != "")
                {
                    if (int.TryParse(txSearchTech.Text, out iTemp) == false || iTemp > 99999 || iTemp < 1)
                    {
                        iTemp = -1;
                        if (lbMsg.Text != "") lbMsg.Text += "<br />";
                        lbMsg.Text += "The assigned tech entry must be a positive integer 5 digits or less";
                        txSearchTech.Focus();
                    }
                    else
                        sAssigned = iTemp.ToString("0");
                }

                if (txSearchStatus.Text != "")
                {
                    sRemark = scrub(txSearchStatus.Text.Trim());
                    if (sRemark.Length > 25)
                        sRemark = sRemark.Substring(0, 25);
                }

                if (txSearchSummary.Text != "")
                {
                    sProblem = scrub(txSearchSummary.Text.Trim());
                    if (sProblem.Length > 50)
                        sProblem = sProblem.Substring(0, 50);
                }

                if (txSearchEnteredGT.Text != "")
                {
                    if (int.TryParse(txSearchEnteredGT.Text, out iTemp) == true)
                    {
                        sDat = iTemp.ToString();
                        if (sDat.Length == 8)
                        {
                            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                                sEnteredGT = datTemp.ToString("yyyyMMdd");
                        }
                        else 
                        {
                            if (lbMsg.Text != "") lbMsg.Text += "<br />";
                            lbMsg.Text += "The date entered (greater than) must be a valid date (yyyymmdd)";
                            txSearchEnteredGT.Focus();
                        }
                    }
                }
                if (txSearchEnteredLT.Text != "")
                {
                    if (int.TryParse(txSearchEnteredLT.Text, out iTemp) == true)
                    {
                        sDat = iTemp.ToString();
                        if (sDat.Length == 8)
                        {
                            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                                sEnteredLT = datTemp.ToString("yyyyMMdd");
                        }
                        else
                        {
                            if (lbMsg.Text != "") lbMsg.Text += "<br />";
                            lbMsg.Text += "The date entered (less than) must be a valid date (yyyymmdd)";
                            txSearchEnteredLT.Focus();
                        }
                    }
                }
                if (txSearchClosedGT.Text != "")
                {
                    if (int.TryParse(txSearchClosedGT.Text, out iTemp) == true)
                    {
                        sDat = iTemp.ToString();
                        if (sDat.Length == 8)
                        {
                            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                                sClosedGT = datTemp.ToString("yyyyMMdd");
                        }
                        else
                        {
                            if (lbMsg.Text != "") lbMsg.Text += "<br />";
                            lbMsg.Text += "The date closed (greater than) must be a valid date (yyyymmdd)";
                            txSearchClosedGT.Focus();
                        }
                    }
                }
                if (txSearchClosedLT.Text != "")
                {
                    if (int.TryParse(txSearchClosedLT.Text, out iTemp) == true)
                    {
                        sDat = iTemp.ToString();
                        if (sDat.Length == 8)
                        {
                            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                                sClosedLT = datTemp.ToString("yyyyMMdd");
                        }
                        else
                        {
                            if (lbMsg.Text != "") lbMsg.Text += "<br />";
                            lbMsg.Text += "The date closed (less than) must be a valid date (yyyymmdd)";
                            txSearchClosedLT.Focus();
                        }
                    }
                }

                sOpenClosedAny = ddSearchOpenClosed.SelectedValue;

                if (lbMsg.Text == "" && iCustomerNumber > 0)
                {
                    dt1 = ws_Get_B1TicketHistoryForPrefixedAccounts(
                        hfPrimaryCs1Type.Value, 
                        hfPrimaryCs1.Value, 
                        sOpenClosedAny,
                        sCenter,
                        sTicket, 
                        sTicketXref,
                        sCustomerName,
                        sCustomerNumber,
                        sCustomerLocation,
                        sCity, 
                        sStateAbbreviation,
                        sAssigned,
                        sRemark,
                        sProblem, 
                        sEnteredGT,
                        sEnteredLT,
                        sClosedGT,
                        sClosedLT
                        );

                    dt = Merge_Tables(dt1, dt2, dt3);

                    rp_TicketSmall.DataSource = dt;
                    rp_TicketSmall.DataBind();

                    ViewState["vsDataTable_Tck"] = null;
                    BindGrid_Tck(dt);
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
        dt.Columns.Add(MakeColumn("TckXrefCust"));
        dt.Columns.Add(MakeColumn("TckXrefWorkMarket"));
        dt.Columns.Add(MakeColumn("DateEntered"));
        dt.Columns.Add(MakeColumn("DateEnteredSort"));
        dt.Columns.Add(MakeColumn("DateCompleted"));
        dt.Columns.Add(MakeColumn("DateCompletedSort"));
        dt.Columns.Add(MakeColumn("CustName"));
        dt.Columns.Add(MakeColumn("CustNum"));
        dt.Columns.Add(MakeColumn("CustSort"));
        dt.Columns.Add(MakeColumn("City"));
        dt.Columns.Add(MakeColumn("State"));
        dt.Columns.Add(MakeColumn("Status"));
        dt.Columns.Add(MakeColumn("Summary"));
        dt.Columns.Add(MakeColumn("Assigned"));
        dt.Columns.Add(MakeColumn("AssignedSort"));
        dt.Columns.Add(MakeColumn("Source"));

        dt.Columns.Add(MakeColumn("TicketIdSort"));

        DateTime datTemp = new DateTime();
        DataRow dr;
        int iRowIdx = 0;
        int iCenter = 0;
        int iTicket = 0;
        int iCwId = 0;

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
            dt.Rows[iRowIdx]["CustNum"] = row["CustNumDisplay"].ToString().Trim();
            dt.Rows[iRowIdx]["CustSort"] = row["CustNumSort"].ToString().Trim();
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["City"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["State"].ToString().Trim();
            dt.Rows[iRowIdx]["Status"] = Fix_Case(row["Remark"].ToString().Trim());
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Summary"].ToString().Trim());
            dt.Rows[iRowIdx]["Assigned"] = row["Assigned"].ToString().Trim();
            dt.Rows[iRowIdx]["AssignedSort"] = row["AssignedSort"].ToString().Trim();
            dt.Rows[iRowIdx]["TckXrefCust"] = row["TckXrefCust"].ToString().Trim();
            dt.Rows[iRowIdx]["TckXrefWorkMarket"] = row["TckXrefWorkMarket"].ToString().Trim();
            dt.Rows[iRowIdx]["Source"] = "1";

            if (DateTime.TryParse(row["DateEnteredStamp"].ToString().Trim(), out datTemp) == true) 
            {
                if (datTemp != new DateTime()) 
                {
                    dt.Rows[iRowIdx]["DateEnteredSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d yyyy");
                }
            }

            if (DateTime.TryParse(row["DateCompletedStamp"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp != new DateTime())
                {
                    dt.Rows[iRowIdx]["DateCompletedSort"] = datTemp.ToString("o");
                    dt.Rows[iRowIdx]["DateCompleted"] = datTemp.ToString("MMM d yyyy");
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
            if (saPreNumTyp.Length > 2) 
            {
                hfPrimaryCs1.Value = saPreNumTyp[1];
                hfPrimaryCs1Type.Value = saPreNumTyp[2];
            }

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0) 
                {
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
                    if (Session["AdminCustomerType"] != null && Session["AdminCustomerType"].ToString().Trim() != "")
                        hfPrimaryCs1Type.Value = Session["AdminCustomerType"].ToString().Trim();
                    else
                        hfPrimaryCs1Type.Value = "REG";
                }
            }
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
    // -------------------------------------------------------------------------------------------------
    protected void btSearchTicketClear_Click(object sender, EventArgs e)
    {
        txSearchCenter.Text = "";
        txSearchTicket.Text = "";
        txSearchTicketXref.Text = "";
        txSearchCustomerName.Text = "";
        txSearchCustomerNum.Text = "";
        txSearchCustomerLoc.Text = "";
        txSearchCity.Text = "";
        txSearchState.Text = "";
        txSearchTech.Text = "";
        txSearchStatus.Text = "";
        txSearchSummary.Text = "";
        txSearchEnteredGT.Text = "";
        txSearchEnteredLT.Text = "";
        txSearchClosedGT.Text = "";
        txSearchClosedLT.Text = "";
        
        ddSearchOpenClosed.SelectedIndex = 2;

    }
    // =========================================================
    protected void btSearchTicketSubmit_Click(object sender, EventArgs e)
    {
        try 
        {
            // ViewState["vsDataTable_Tck"] = null;  // make null before bind (sorting will not use the full load SR) 
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
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
