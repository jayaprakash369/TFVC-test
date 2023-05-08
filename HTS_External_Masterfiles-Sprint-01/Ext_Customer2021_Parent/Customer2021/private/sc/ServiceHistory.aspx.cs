using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_sc_ServiceHistory : MyPage
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

            pnHistory.Visible = false;
                pnLocation.Visible = true;

            try
            {
                if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
                {
                    pnSearchCustomerFamily.Visible = true;
                    LoadFamilyMemberDropDownList();
                    // First pass, only large customers and dealers only get a list of customers to choose from (too big...)
                }
                else
                {
                    pnSearchCustomerFamily.Visible = false;
                    // First pass, only regular customers get the full list

                    ViewState["vsDataTable_Loc"] = null;
                    BindGrid_Loc();
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
    }
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Loc)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Loc()
    {
        DataTable dt = new DataTable("");

        string sReloadRepeater = "";

        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            dt = LoadLocationDataTables();
            ViewState["vsDataTable_Loc"] = dt;

            if (dt.Rows.Count == 0)
            {
                //lbMsg.Text = "No matching locations found...";
                //lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Loc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Loc;
        if (gridSortDirection_Loc == SortDirection.Ascending)
        {
            sortExpression_Loc = gridSortExpression_Loc + " ASC";
        }
        else
        {
            sortExpression_Loc = gridSortExpression_Loc + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Loc;

        gv_LocationLarge.DataSource = dt.DefaultView;
        gv_LocationLarge.DataBind();

        if (sReloadRepeater == "Y") 
        {
            rp_LocationSmall.DataSource = dt.DefaultView;
            rp_LocationSmall.DataBind();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Loc(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_LocationLarge.PageIndex = newPageIndex;
        BindGrid_Loc();
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Loc(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Loc = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Loc == e.SortExpression)
        {
            if (gridSortDirection_Loc == SortDirection.Ascending)
                gridSortDirection_Loc = SortDirection.Descending;
            else
                gridSortDirection_Loc = SortDirection.Ascending;
        }
        else
            gridSortDirection_Loc = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Loc = sortExpression_Loc;
        // Rebind the grid to its data source
        BindGrid_Loc();
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Loc
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Loc"] == null)
            {
                ViewState["GridSortDirection_Loc"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Loc"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Loc"];
        }
        set
        {
            ViewState["GridSortDirection_Loc"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Loc
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Loc"] == null)
            {
                ViewState["GridSortExpression_Loc"] = "CUSTNM, CSTRNR, CSTRCD"; // CUSTNM
            }
            return (string)ViewState["GridSortExpression_Loc"];
        }
        set
        {
            ViewState["GridSortExpression_Loc"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Loc)
    // -------------------------------------------------------------------------------------------------

    // -------------------------------------------------------------------------------------------------
    // BEGIN: History Table (_Hst)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Hst(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But because you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Hst = "";
        string sReloadRepeater = "";

        if (ViewState["vsDataTable_Hst"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Hst"] = dt;

            if (dt.Rows.Count == 0)
            {
                //lbMsg.Text = "No matching tickets were found...";
                //lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Hst"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        if (gridSortDirection_Hst == SortDirection.Ascending)
        {
            sortExpression_Hst = gridSortExpression_Hst + " ASC";
        }
        else
        {
            sortExpression_Hst = gridSortExpression_Hst + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Hst;

        gv_HistoryLarge.DataSource = dt.DefaultView;
        gv_HistoryLarge.DataBind();

        if (hfPrimaryCs1.Value == "89866") // Container Store
            gv_HistoryLarge.Columns[14].Visible = true;
        else
            gv_HistoryLarge.Columns[14].Visible = false;


        if (sReloadRepeater == "Y") 
        {
            //https://10.41.30.53:4481/private/sc/ServiceHistory.aspx            
            rp_HistorySmall.DataSource = dt.DefaultView;
            rp_HistorySmall.DataBind();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Hst(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_HistoryLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Hst(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Hst(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Hst == SortDirection.Ascending)
                gridSortDirection_Hst = SortDirection.Descending;
            else
                gridSortDirection_Hst = SortDirection.Ascending;
        }
        else
            gridSortDirection_Hst = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Hst = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Hst(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Hst
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Hst"] == null)
            {
                //ViewState["GridSortDirection_Hst"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Hst"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Hst"];
        }
        set
        {
            ViewState["GridSortDirection_Hst"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Hst
    {
        get
        {
            if (ViewState["GridSortExpression_Hst"] == null)
            {
                ViewState["GridSortExpression_Hst"] = "SortEntered"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Hst"];
        }
        set
        {
            ViewState["GridSortExpression_Hst"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: History Table (_Hst)
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
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B1TicketHistory(
        string customerNumber,
        string customerLocation,
        string center,
        string ticket,
        string custCallAlias,
        string dateStart,
        string dateEnd,
        string open,
        string onsite,
        string depot,
        string install,
        string pm,
        string MITS,
        string MP,
        string toner,
        string supplies,
        string excelVersion
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1TicketHistory";
            string sFieldList = "customerNumber|customerLocation|center|ticket|custCallAlias|dateStart|dateEnd|open|onsite|depot|install|pm|MITS|MP|toner|supplies|excelVersion|x";
            string sValueList = 
                customerNumber.ToString() + "|" +
                customerLocation + "|" +
                center.ToString() + "|" +
                ticket.ToString() + "|" +
                custCallAlias + "|" +
                dateStart.ToString() + "|" +
                dateEnd.ToString() + "|" +
                open + "|" +
                onsite + "|" +
                depot + "|" +
                install + "|" +
                pm + "|" +
                MITS + "|" +
                MP + "|" +
                toner + "|" +
                supplies + "|" +
                excelVersion + "|" +
                "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
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
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable LoadLocationDataTables()
    {
        Session.Timeout = 600;

        DataTable dt = new DataTable("");
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");

        string sCustomerNumber = "";
        int iSelectedCustomerNumber = 0;

        if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
        {
            if (!String.IsNullOrEmpty(ddSearchCustomerFamily.SelectedValue))
            {
                if (int.TryParse(ddSearchCustomerFamily.SelectedValue, out iSelectedCustomerNumber) == false)
                    iSelectedCustomerNumber = -1;
            }
        }

        if (iSelectedCustomerNumber > 0) // i.e. LRG cust picked one of their own sub cust numbers
            sCustomerNumber = iSelectedCustomerNumber.ToString();
        else
            sCustomerNumber = hfPrimaryCs1.Value;

        string sCustomerName = txSearchLocationName.Text.Trim().ToUpper().Trim();
        string sCustomerLocation = txSearchLocationNum.Text.Trim();
        string sXref = txSearchLocationCustXref.Text.Trim().ToUpper().Trim();
        string sAddress = txSearchLocationAddress.Text.Trim().ToUpper().Trim();
        string sCity = txSearchLocationCity.Text.Trim().ToUpper().Trim();
        string sState = txSearchLocationState.Text.Trim().ToUpper().Trim();
        string sZip = txSearchLocationZip.Text.Trim().ToUpper().Trim();
        string sPhone = txSearchLocationPhone.Text.Trim().ToUpper().Trim();


        try
        {
            if (
                (hfPrimaryCs1Type.Value != "LRG" && hfPrimaryCs1Type.Value != "DLR")
                ||
                ( // LRG/DLR must pick something!
                    (iSelectedCustomerNumber > 0)
                    || !String.IsNullOrEmpty(sCustomerName)
                    || !String.IsNullOrEmpty(sCustomerLocation)
                    || !String.IsNullOrEmpty(sAddress)
                    || !String.IsNullOrEmpty(sCity)
                    || !String.IsNullOrEmpty(sState)
                    || !String.IsNullOrEmpty(sZip)
                    || !String.IsNullOrEmpty(sPhone)
                    || !String.IsNullOrEmpty(sXref)
                )
            )
            {
                dtB1 = ws_Get_B1CustomerLocationDetail(
                    sCustomerNumber,
                    sCustomerLocation,
                    sCustomerName,
                    "", // No contact used here
                    sAddress,
                    sCity,
                    sState,
                    sZip,
                    sPhone,
                    sXref);
            }

            // Merge
            dt = Merge_LocationTables(dtB1, dtB2);
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Merge_LocationTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("CSTRNR"));
        dt.Columns.Add(MakeColumn("CSTRCD"));
        dt.Columns.Add(MakeColumn("CUSTNM"));
        dt.Columns.Add(MakeColumn("CONTNM"));
        dt.Columns.Add(MakeColumn("XREFCS"));
        dt.Columns.Add(MakeColumn("SADDR1"));
        dt.Columns.Add(MakeColumn("SADDR2"));
        dt.Columns.Add(MakeColumn("CITY"));
        dt.Columns.Add(MakeColumn("STATE"));
        dt.Columns.Add(MakeColumn("ZIPCD"));
        dt.Columns.Add(MakeColumn("HPHONE"));
        dt.Columns.Add(MakeColumn("FLAG8"));
        dt.Columns.Add(MakeColumn("ORCACC"));
        dt.Columns.Add(MakeColumn("B1EqpCount"));
        dt.Columns.Add(MakeColumn("B2EqpCount"));
        dt.Columns.Add(MakeColumn("CombinedEqpCount"));
        dt.Columns.Add(MakeColumn("CombinedEqpCountSort"));
        dt.Columns.Add(MakeColumn("PhoneFormatted"));
        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        int iRowIdx = 0;
        int iB1EqpCount = 0;
        int iB2EqpCount = 0;
        int iCombinedEqpCount = 0;
        //int iTemp = 0;


        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["CSTRNR"] = row["CSTRNR"].ToString().Trim();
            dt.Rows[iRowIdx]["CSTRCD"] = row["CSTRCD"].ToString().Trim();
            dt.Rows[iRowIdx]["CUSTNM"] = Fix_Case(row["CUSTNM"].ToString()).Trim();
            dt.Rows[iRowIdx]["CONTNM"] = Fix_Case(row["CONTNM"].ToString()).Trim();

            dt.Rows[iRowIdx]["XREFCS"] = Fix_Case(row["XREFCS"].ToString().Trim());
            dt.Rows[iRowIdx]["SADDR1"] = Fix_Case(row["SADDR1"].ToString().Trim());
            dt.Rows[iRowIdx]["SADDR2"] = Fix_Case(row["SADDR2"].ToString().Trim());
            dt.Rows[iRowIdx]["CITY"] = Fix_Case(row["CITY"].ToString().Trim());
            dt.Rows[iRowIdx]["STATE"] = row["STATE"].ToString().Trim();
            dt.Rows[iRowIdx]["ZIPCD"] = row["ZIPCD"].ToString().Trim();
            dt.Rows[iRowIdx]["HPHONE"] = row["HPHONE"].ToString().Trim();
            dt.Rows[iRowIdx]["PhoneFormatted"] = row["PhoneFormatted"].ToString().Trim();
            dt.Rows[iRowIdx]["FLAG8"] = row["FLAG8"].ToString().Trim();
            dt.Rows[iRowIdx]["ORCACC"] = row["ORCACC"].ToString().Trim();
            
            

            if (int.TryParse(row["b1EqpCount"].ToString().Trim(), out iB1EqpCount) == false)
                iB1EqpCount = -1;

            if (int.TryParse(row["b2EqpCount"].ToString().Trim(), out iB2EqpCount) == false)
                iB2EqpCount = -1;

            if (int.TryParse(row["CombinedEqpCount"].ToString().Trim(), out iCombinedEqpCount) == false)
                iCombinedEqpCount = -1;


            if (iB1EqpCount > 0)
                dt.Rows[iRowIdx]["B1EqpCount"] = iB1EqpCount.ToString();

            if (iB2EqpCount > 0)
                dt.Rows[iRowIdx]["B2EqpCount"] = iB2EqpCount.ToString();

            if (iCombinedEqpCount > 0)
            {
                dt.Rows[iRowIdx]["CombinedEqpCount"] = iCombinedEqpCount;
                dt.Rows[iRowIdx]["CombinedEqpCountSort"] = iCombinedEqpCount.ToString("00000");
            }

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            //dr = dt.NewRow();
            //dt.Rows.Add(dr);

            //dt.Rows[iRowIdx]["AgentId"] = "";
            //dt.Rows[iRowIdx]["Agreement"] = row["agreementId"].ToString().Trim();
            //dt.Rows[iRowIdx]["Model"] = Fix_Case(row["product-identifier"].ToString().Trim());
            //dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["description"].ToString().Trim());
            //dt.Rows[iRowIdx]["ModelXref"] = "";
            //dt.Rows[iRowIdx]["Serial"] = Fix_Case(row["serialNumber"].ToString().Trim());

            //if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
            //    iTemp = -1;
            //if (iTemp > 0)
            //    dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

            //dt.Rows[iRowIdx]["Source"] = "2";

            //iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Merge_HistoryTables(DataTable dtB1, DataTable dtB2, DataTable dtB3)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp = new DateTime();

        dt.Columns.Add(MakeColumn("Call"));
        dt.Columns.Add(MakeColumn("Center"));
        dt.Columns.Add(MakeColumn("Ticket"));
        dt.Columns.Add(MakeColumn("TicketXRef"));
        dt.Columns.Add(MakeColumn("Status"));
        dt.Columns.Add(MakeColumn("Comment"));
        dt.Columns.Add(MakeColumn("Customer"));
        dt.Columns.Add(MakeColumn("Location"));
        dt.Columns.Add(MakeColumn("CustXref"));
        dt.Columns.Add(MakeColumn("CustomerName"));
        dt.Columns.Add(MakeColumn("Address1"));
        dt.Columns.Add(MakeColumn("Address2"));
        dt.Columns.Add(MakeColumn("Address3"));
        dt.Columns.Add(MakeColumn("City"));
        dt.Columns.Add(MakeColumn("State"));
        dt.Columns.Add(MakeColumn("Zip"));
        dt.Columns.Add(MakeColumn("Contact"));
        dt.Columns.Add(MakeColumn("Phone"));
        dt.Columns.Add(MakeColumn("Ext"));
        dt.Columns.Add(MakeColumn("PartsUsed"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("ModelSource"));
        dt.Columns.Add(MakeColumn("ModelXRef"));
        dt.Columns.Add(MakeColumn("Trips"));
        dt.Columns.Add(MakeColumn("Summary"));

        dt.Columns.Add(MakeColumn("DateEntered"));
        dt.Columns.Add(MakeColumn("TimeEntered"));
        dt.Columns.Add(MakeColumn("DateCompleted"));
        dt.Columns.Add(MakeColumn("TimeCompleted"));
        dt.Columns.Add(MakeColumn("StsTicket"));
        dt.Columns.Add(MakeColumn("CallType"));
        dt.Columns.Add(MakeColumn("SortEntered"));
        dt.Columns.Add(MakeColumn("SortCompleted"));

        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        string sStatus = "";
        int iRowIdx = 0;
        int iTrips = 0;

        foreach (DataRow row in dtB1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["Call"] = row["Center"].ToString().Trim() + "-" + row["Ticket"].ToString().Trim();
            dt.Rows[iRowIdx]["Center"] = row["Center"].ToString().Trim();
            dt.Rows[iRowIdx]["Ticket"] = row["Ticket"].ToString().Trim();
            dt.Rows[iRowIdx]["TicketXRef"] = row["TicketXRef"].ToString().Trim().ToUpper(); // ???
            dt.Rows[iRowIdx]["Status"] = Fix_Case(row["Status"].ToString()).Trim();
            dt.Rows[iRowIdx]["Comment"] = Fix_Case(row["Comment"].ToString()).Trim();
            dt.Rows[iRowIdx]["Customer"] = row["Customer"].ToString().Trim();
            dt.Rows[iRowIdx]["Location"] = row["Location"].ToString().Trim();
            dt.Rows[iRowIdx]["CustXref"] = Fix_Case(row["CustXref"].ToString().Trim());
            dt.Rows[iRowIdx]["CustomerName"] = row["CustomerName"].ToString().Trim().ToUpper();
            dt.Rows[iRowIdx]["Address1"] = Fix_Case(row["Address1"].ToString().Trim());
            dt.Rows[iRowIdx]["Address2"] = Fix_Case(row["Address2"].ToString().Trim());
            dt.Rows[iRowIdx]["Address3"] = Fix_Case(row["Address3"].ToString().Trim());
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["City"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["State"].ToString().Trim();
            dt.Rows[iRowIdx]["Zip"] = row["zip"].ToString().Trim();
            dt.Rows[iRowIdx]["Contact"] = Fix_Case(row["Contact"].ToString().Trim());
            dt.Rows[iRowIdx]["Phone"] = row["Phone"].ToString().Trim();
            dt.Rows[iRowIdx]["Ext"] = row["Ext"].ToString().Trim();
            if(row["PartsUsed"].ToString().Trim() == "Y")
                dt.Rows[iRowIdx]["PartsUsed"] = row["PartsUsed"].ToString().Trim();
            dt.Rows[iRowIdx]["Model"] = row["Model"].ToString().Trim();
            dt.Rows[iRowIdx]["ModelDescription"] = row["ModelDescription"].ToString().Trim();
            dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim();
            dt.Rows[iRowIdx]["ModelXRef"] = row["ModelXRef"].ToString().Trim();

            if (int.TryParse(row["Trips"].ToString().Trim(), out iTrips) == false)
                iTrips = -1;
            if (iTrips > 0)
                dt.Rows[iRowIdx]["Trips"] = iTrips.ToString();

            dt.Rows[iRowIdx]["DateEntered"] = row["DateEntered"].ToString().Trim();
            dt.Rows[iRowIdx]["TimeEntered"] = row["TimeEntered"].ToString().Trim();
            dt.Rows[iRowIdx]["DateCompleted"] = row["DateCompleted"].ToString().Trim();
            dt.Rows[iRowIdx]["TimeCompleted"] = row["TimeCompleted"].ToString().Trim();
            dt.Rows[iRowIdx]["StsTicket"] = row["StsTicket"].ToString().Trim();
            dt.Rows[iRowIdx]["CallType"] = row["CallType"].ToString().Trim();
            dt.Rows[iRowIdx]["SortEntered"] = row["SortEntered"].ToString().Trim();
            dt.Rows[iRowIdx]["SortCompleted"] = row["SortCompleted"].ToString().Trim();
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Comment"].ToString()).Trim();

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        foreach (DataRow row in dtB2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["Call"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["Center"] = "";
            dt.Rows[iRowIdx]["Ticket"] = "";
            dt.Rows[iRowIdx]["TicketXRef"] = "";
            sStatus = row["status-name"].ToString().Trim().Replace(">", "");
            dt.Rows[iRowIdx]["Status"] = Fix_Case(sStatus);
            dt.Rows[iRowIdx]["Comment"] = Fix_Case(row["summary"].ToString()).Trim();
            dt.Rows[iRowIdx]["Customer"] = hfChosenCs1.Value.Trim();
            dt.Rows[iRowIdx]["Location"] = hfChosenCs2.Value.Trim();
            dt.Rows[iRowIdx]["CustXref"] = "";
            dt.Rows[iRowIdx]["CustomerName"] = row["companyName"].ToString().Trim();  // Service ticket has no company-name, just identifier...
            dt.Rows[iRowIdx]["Address1"] = Fix_Case(row["addressLine1"].ToString().Trim());
            dt.Rows[iRowIdx]["Address2"] = Fix_Case(row["addressLine2"].ToString().Trim());
            dt.Rows[iRowIdx]["Address3"] = "";
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["city"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["stateIdentifier"].ToString().Trim();
            dt.Rows[iRowIdx]["Zip"] = row["zip"].ToString().Trim();
            dt.Rows[iRowIdx]["Contact"] = Fix_Case(row["contactName"].ToString().Trim());
            dt.Rows[iRowIdx]["Phone"] = row["contactPhone"].ToString().Trim();
            dt.Rows[iRowIdx]["Ext"] = "";
            dt.Rows[iRowIdx]["PartsUsed"] = "";
            dt.Rows[iRowIdx]["Model"] = "";
            dt.Rows[iRowIdx]["ModelDescription"] = "";
            dt.Rows[iRowIdx]["Serial"] = "";
            dt.Rows[iRowIdx]["ModelXRef"] = "";
            dt.Rows[iRowIdx]["Trips"] = "";
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Summary"].ToString()).Trim();

            if (DateTime.TryParse(row["dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d, yyyy"); 
                if (datTemp.Hour > 0)
                    dt.Rows[iRowIdx]["TimeEntered"] = datTemp.ToString("h:mm tt");
                dt.Rows[iRowIdx]["SortEntered"] = datTemp.ToString("o");
            }

            if (DateTime.TryParse(row["closedDate"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateCompleted"] = datTemp.ToString("MMM d, yyyy");
                if (datTemp.Hour > 0)
                    dt.Rows[iRowIdx]["TimeCompleted"] = datTemp.ToString("h:mm tt");
                dt.Rows[iRowIdx]["SortCompleted"] = datTemp.ToString("o");
            }

            dt.Rows[iRowIdx]["StsTicket"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["CallType"] = "Managed IT";

            dt.Rows[iRowIdx]["Source"] = "2";

            iRowIdx++;
        }

        foreach (DataRow row in dtB3.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["Call"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["Center"] = "";
            dt.Rows[iRowIdx]["Ticket"] = "";
            dt.Rows[iRowIdx]["TicketXRef"] = "";
            sStatus = row["status-name"].ToString().Trim().Replace(">", "");
            dt.Rows[iRowIdx]["Status"] = Fix_Case(sStatus);
            dt.Rows[iRowIdx]["Comment"] = Fix_Case(row["summary"].ToString()).Trim();
            dt.Rows[iRowIdx]["Customer"] = hfChosenCs1.Value.Trim();
            dt.Rows[iRowIdx]["Location"] = hfChosenCs2.Value.Trim();
            dt.Rows[iRowIdx]["CustXref"] = "";
            dt.Rows[iRowIdx]["CustomerName"] = row["companyName"].ToString().Trim();
            dt.Rows[iRowIdx]["Address1"] = Fix_Case(row["addressLine1"].ToString().Trim());
            dt.Rows[iRowIdx]["Address2"] = Fix_Case(row["addressLine2"].ToString().Trim());
            dt.Rows[iRowIdx]["Address3"] = "";
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["city"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["stateIdentifier"].ToString().Trim();
            dt.Rows[iRowIdx]["Zip"] = row["zip"].ToString().Trim();
            dt.Rows[iRowIdx]["Contact"] = Fix_Case(row["contactName"].ToString().Trim());
            dt.Rows[iRowIdx]["Phone"] = row["contactPhone"].ToString().Trim();
            dt.Rows[iRowIdx]["Ext"] = "";
            dt.Rows[iRowIdx]["PartsUsed"] = "";
            dt.Rows[iRowIdx]["Model"] = "";
            dt.Rows[iRowIdx]["ModelDescription"] = "";
            dt.Rows[iRowIdx]["Serial"] = "";
            dt.Rows[iRowIdx]["ModelXRef"] = "";
            dt.Rows[iRowIdx]["Trips"] = "";
            dt.Rows[iRowIdx]["Summary"] = Fix_Case(row["Summary"].ToString()).Trim();

            if (DateTime.TryParse(row["dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateEntered"] = datTemp.ToString("MMM d, yyyy");
                if (datTemp.Hour > 0)
                    dt.Rows[iRowIdx]["TimeEntered"] = datTemp.ToString("h:mm tt");
                dt.Rows[iRowIdx]["SortEntered"] = datTemp.ToString("o");
            }

            if (DateTime.TryParse(row["closedDate"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateCompleted"] = datTemp.ToString("MMM d, yyyy");
                if (datTemp.Hour > 0)
                    dt.Rows[iRowIdx]["TimeCompleted"] = datTemp.ToString("h:mm tt");
                dt.Rows[iRowIdx]["SortCompleted"] = datTemp.ToString("o");
            }


            dt.Rows[iRowIdx]["StsTicket"] = row["id"].ToString().Trim();
            dt.Rows[iRowIdx]["CallType"] = "Managed IT";

            dt.Rows[iRowIdx]["Source"] = "3";

            iRowIdx++;
        }

        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void LoadHistoryPanel()
    {
        DataTable dt = new DataTable("");

        try
        {
            string sTemp = "";

            if (String.IsNullOrEmpty(hfChosenCs2.Value))
            {
                sTemp = "Searching all locations under customer " + hfPrimaryCs1.Value;
                
                //+ "  (" + Fix_Case(hfChosenNam.Value) + ")";
            }
            else 
            {
                sTemp = "Searching customer " + hfChosenCs1.Value + " at location " + hfChosenCs2.Value;
                //+ "  (" + Fix_Case(hfChosenNam.Value) + ")";
            }
            lbHistoryCustomer.Text = sTemp;

            if (sLibrary == "OMDTALIB")
                txHistoryStart.Text = DateTime.Now.AddMonths(-4).ToString("yyyyMMdd");
            else 
                txHistoryStart.Text = DateTime.Now.AddMonths(-4).ToString("yyyyMMdd"); 

            txHistoryEnd.Text = DateTime.Now.ToString("yyyyMMdd");

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
    protected void LoadFamilyMemberDropDownList()
    {
        int iPrimaryCustomerNumber = 0;
        string sFamilyMemberList = "";

        if (int.TryParse(hfPrimaryCs1.Value, out iPrimaryCustomerNumber) == false)
            iPrimaryCustomerNumber = -1;
        if (iPrimaryCustomerNumber > 0)
        {


            // 1) clear any items from the drop down list (shouldn't be any) 
            for (int i = 0; i < ddSearchCustomerFamily.Items.Count; i++)
            {
                ddSearchCustomerFamily.Items.RemoveAt(0);
            }

            // 2) Get Family member name and number list
            sFamilyMemberList = ws_Get_B1CustomerFamilyMemberNameAndNumberList(iPrimaryCustomerNumber);
            string[] saFamilyMembers = sFamilyMemberList.Split('|');
            string[] saNamNum = { "", "" };

            // 3) Load list values into drop down list
            for (int i = 0; i < saFamilyMembers.Length; i++)
            {
                saNamNum = saFamilyMembers[i].Split('~');
                if (saNamNum.Length > 1)
                {
                    if (saNamNum[0].Length > 40)
                        saNamNum[0] = saNamNum[0].Substring(0, 40);
                    ddSearchCustomerFamily.Items.Insert(i, new System.Web.UI.WebControls.ListItem(saNamNum[1] + "  " + saNamNum[0], saNamNum[1]));
                }
                
            }

            ddSearchCustomerFamily.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        }

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
    protected void btSearchLocationSubmit_Click(object sender, EventArgs e)
    {
        ViewState["vsDataTable_Loc"] = null;
        BindGrid_Loc();
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchLocationClear_Click(object sender, EventArgs e)
    {
        txSearchLocationAddress.Text = "";
        txSearchLocationCity.Text = "";
        txSearchLocationCustXref.Text = "";
        txSearchLocationName.Text = "";
        txSearchLocationNum.Text = "";
        txSearchLocationPhone.Text = "";
        txSearchLocationState.Text = "";
        txSearchLocationZip.Text = "";
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchUseAllLocations_Click(object sender, EventArgs e)
    {
        pnLocation.Visible = false;
        pnHistory.Visible = true;

        hfChosenCs1.Value = hfPrimaryCs1.Value;
        hfChosenCs2.Value = "";

        LoadHistoryPanel();
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkLocationName_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('|');

        if (saArg.Length > 2)
        {
            ViewState["vsDataTable_Loc"] = null;

            // Set the chosen customer to use going forward
            int iCustomerNumber = 0;
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = 0;
            int iCustomerLocation = 0;
            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = 0;

            string sCustomerName = saArg[2];

            hfChosenCs1.Value = iCustomerNumber.ToString();
            hfChosenCs2.Value = iCustomerLocation.ToString();
            hfChosenNam.Value = sCustomerName;

            // If you have data...
            if (iCustomerNumber > 0)
            {
                ViewState["vsDataTable_Loc"] = null;
                pnLocation.Visible = false;
                pnHistory.Visible = true;
                
                LoadHistoryPanel();
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lsBxReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lbError.Text = "";
        //lbError.Visible = false;
        //pnLocations.Visible = false;
        pnHistoryCalendars.Visible = false;
        pnHistoryCenterTicket.Visible = false;
        pnHistoryCustCallAlias.Visible = false;
        //pnTickets.Visible = false;

        // Clear to free memory
        ViewState["vsDataTable_Loc"] = null;
        ViewState["vsDataTable_Tck"] = null;

        if (lsBxReportType.SelectedValue == "OpenRange"
            || lsBxReportType.SelectedValue == "ClosedRange"
            )
        {
            pnHistoryCalendars.Visible = true;
            //if (!String.IsNullOrEmpty(txHistoryStart.Text) && !String.IsNullOrEmpty(txHistoryEnd.Text))
            //{
            //    Load_History("");
            //}
        }
        else if (lsBxReportType.SelectedValue == "ClosedModel")
        {
            pnHistoryCalendars.Visible = true;
        }
        else if (lsBxReportType.SelectedValue == "ByTicket")
        {
            pnHistoryCenterTicket.Visible = true;
            txHistoryCenterTicket.Focus();
        }
        else if (lsBxReportType.SelectedValue == "ByXref")
        {
            pnHistoryCustCallAlias.Visible = true;
            txHistoryCustCallAlias.Focus();
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btHistoryReset_Click(object sender, EventArgs e)
    {

        lsBxReportType.SelectedValue = "Open";

        txHistoryCenterTicket.Text = "";
        txHistoryCustCallAlias.Text = "";
        //calHistoryStart.SelectedDate = DateTime.Now.AddMonths(-4);
        //calHistoryEnd.SelectedDate = DateTime.Now;
        txHistoryStart.Text = DateTime.Now.AddMonths(-4).ToString("yyyyMMdd");
        txHistoryEnd.Text = DateTime.Now.ToString("yyyyMMdd");

        cbServiceType.Items[0].Selected = true;
        cbServiceType.Items[1].Selected = true;
        cbServiceType.Items[2].Selected = true;
        cbServiceType.Items[3].Selected = false;
        cbServiceType.Items[4].Selected = false;
        cbServiceType.Items[5].Selected = true;
        cbServiceType.Items[6].Selected = false;
        cbServiceType.Items[7].Selected = false;

        pnHistoryCalendars.Visible = false;
        pnHistoryCenterTicket.Visible = false;
        pnHistoryCustCallAlias.Visible = false;

    }
    // ----------------------------------------------------------------------------------------------------
    protected void btHistorySubmit_Click(object sender, EventArgs e)
    {
        Load_History("");
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btExcelSubmit_Click(object sender, EventArgs e)
    {
        Load_History("Y");
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_History(string excelVersionY)
    {
        string sCustomerNumber = "";
        string sCustomerLocation = "";
        string sB1CustomerNumberForB2 = "";
        string sB1CustomerLocationForB2 = "";
        string sCenter = "";
        string sTicket = "";

        string sCustCallAlias = "";
        string sDateStart = "";
        string sDateEnd = "";
        string sOpen = "";
        string sOnsite = "";
        string sDepot = "";
        string sInstall = "";
        string sPm = "";
        string sMITS = "";
        string sMP = "";
        string sToner = "";
        string sSupplies = "";
        string sExcelVersion = "";
        string sB2OpenClosedOrAll = "";
        string sB2RunQueries = "Y";
        string sRunModelQuery = "";
        string sB2StartTimestamp = "";
        string sB2EndTimestamp = "";
        string sDat = "";

        int iCs1 = 0;
        int iCs2 = 0;
        int iCtr = 0;
        int iTck = 0;
        int iDateStart = 0;
        int iDateEnd = 0;

        if (int.TryParse(hfChosenCs1.Value, out iCs1) == false)
            iCs1 = -1;
        if (iCs1 > -1) 
        {
            sCustomerNumber = iCs1.ToString();
            sB1CustomerNumberForB2 = iCs1.ToString();
        }
            

        if (int.TryParse(hfChosenCs2.Value, out iCs2) == false)
            iCs2 = -1;
        if (iCs2 > -1) 
        {
            sCustomerLocation = iCs2.ToString();
            sB1CustomerLocationForB2 = iCs2.ToString();
        }
            

        int iIdx = 0;
        foreach (ListItem li in cbServiceType.Items)
        {
            if (li.Selected == true)
            {
                if (iIdx == 0)
                    sOnsite = "Y";
                if (iIdx == 1)
                    sDepot = "Y";
                if (iIdx == 2)
                    sMITS = "Y";
                if (iIdx == 3)
                    sMP = "Y";
                if (iIdx == 4)
                    sToner = "Y";
                if (iIdx == 5)
                    sPm = "Y";
                if (iIdx == 6)
                    sInstall = "Y";
                if (iIdx == 7)
                    sSupplies = "Y";
            }
            iIdx++;
        }

        if (lsBxReportType.SelectedValue == "OpenRange" || lsBxReportType.SelectedValue == "ClosedRange" || lsBxReportType.SelectedValue == "ClosedModel")
        {
            DateTime datBeg = new DateTime();
            DateTime datEnd = new DateTime();

            if (!String.IsNullOrEmpty(txHistoryStart.Text))
                sDat = txHistoryStart.Text.Trim();
            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datBeg) == false)
                datBeg = new DateTime();

            if (!String.IsNullOrEmpty(txHistoryEnd.Text))
                sDat = txHistoryEnd.Text.Trim();
            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datEnd) == false)
                datEnd = new DateTime();

            if (datBeg != new DateTime())
                sB2StartTimestamp = datBeg.ToString("o");
            if (datEnd != new DateTime())
                sB2EndTimestamp = datEnd.ToString("o");

            if (datBeg != new DateTime() && datEnd != new DateTime())
            {
                if (int.TryParse(datBeg.ToString("yyyyMMdd"), out iDateStart) == false)
                    iDateStart = 0;
                if (int.TryParse(datEnd.ToString("yyyyMMdd"), out iDateEnd) == false)
                    iDateEnd = 0;

                if ((iDateStart == 0) || (iDateEnd == 0) || (iDateStart > iDateEnd))
                {
                    iDateStart = 0;
                    iDateEnd = 0;
                }
                else
                {
                    sDateStart = iDateStart.ToString();
                    sDateEnd = iDateEnd.ToString();
                }
            }

            //if ((calHistoryStart.SelectedDate.Year > 1) && (calHistoryEnd.SelectedDate.Year > 1))
            //{

            //    datTemp = calHistoryStart.SelectedDate;
            //    if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateStart) == false)
            //        iDateStart = 0;
            //    datTemp = calHistoryEnd.SelectedDate;
            //    if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateEnd) == false)
            //        iDateEnd = 0;

            //    if ((iDateStart == 0) || (iDateEnd == 0) || (iDateStart > iDateEnd))
            //    {
            //        iDateStart = 0;
            //        iDateEnd = 0;
            //    }
            //}
        }
        
        sRunModelQuery = "N";

        if (lsBxReportType.SelectedValue == "ClosedModel")
        {
            sRunModelQuery = "Y";
        }
        else if (lsBxReportType.SelectedValue == "Open")
        {
            sOpen = "OPEN ONLY";
            sB2OpenClosedOrAll = "Open";
        }
        else if (lsBxReportType.SelectedValue == "OpenRange")
        {
            //sOpen = "OPENED";
            sOpen = "OPEN";
            sB2OpenClosedOrAll = "Open";
        }
        else if (lsBxReportType.SelectedValue == "ClosedRange")
        {
            sOpen = "CLOSED";
            sB2OpenClosedOrAll = "Closed";
        }
        else if (lsBxReportType.SelectedValue == "ByTicket")
        {
            sB2OpenClosedOrAll = "All";
            if (pnHistoryCenterTicket.Visible = true && !String.IsNullOrEmpty(txHistoryCenterTicket.Text))
            {
                string[] saCtrTck = txHistoryCenterTicket.Text.Trim().Split('-');
                if (saCtrTck.Length > 1)
                {
                    if (int.TryParse(saCtrTck[0], out iCtr) == false)
                        iCtr = 0;
                    if (iCtr > 0)
                        sCenter = iCtr.ToString();
                    if (int.TryParse(saCtrTck[1], out iTck) == false)
                        iTck = 0;
                    if (iTck > 0)
                        sTicket = iTck.ToString();
                }
                else if (saCtrTck.Length > 0) // B2 Ticket number
                {
                    if (int.TryParse(saCtrTck[0], out iTck) == false)
                        iTck = 0;
                    if (iTck > 0)
                        sTicket = iTck.ToString();
                }

            }
        }
        else if (lsBxReportType.SelectedValue == "ByXref")
        {
            sB2RunQueries = "N";
            if (pnHistoryCustCallAlias.Visible == true && !String.IsNullOrEmpty(txHistoryCustCallAlias.Text))
            {
                sCustCallAlias = txHistoryCustCallAlias.Text.Trim();
            }
        }

        //if (pnHistoryCalendars.Visible == true)
        //{
        //    sDateStart = calHistoryStart.SelectedDate.ToString("yyyyMMdd");
        //    sDateEnd = calHistoryEnd.SelectedDate.ToString("yyyyMMdd");
        //}

        DataTable dt = new DataTable("");
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");
        DataTable dtB3 = new DataTable("");
        
        Session.Timeout = 40000;
        pnHistory_Alt.Visible = false;
        pnHistory_Reg.Visible = false;

        if (sRunModelQuery == "Y")
        {
            pnHistory_Alt.Visible = true;
            dt = ws_Get_B1TicketSummaryByModel(
                sCustomerNumber,
                sCustomerLocation,
                sDateStart,
                sDateEnd
                );
        }
        else
        {
            pnHistory_Reg.Visible = true;


            dtB1 = ws_Get_B1TicketHistory(
                sCustomerNumber,
                sCustomerLocation,
                sCenter,
                sTicket,
                sCustCallAlias,
                sDateStart,
                sDateEnd,
                sOpen,
                sOnsite,
                sDepot,
                sInstall,
                sPm,
                sMITS,
                sMP,
                sToner,
                sSupplies,
                excelVersionY
            );

            if (sB2RunQueries == "Y" && sMITS == "Y" && sCenter == "")  // if you specified a center it's only an B1 Ticket
            {
                dtB2 = ws_Get_B2TicketSummary(
                    sB1CustomerNumberForB2,
                    sB1CustomerLocationForB2,
                    sTicket,
                    sB2OpenClosedOrAll,
                    sB2StartTimestamp,
                    sB2EndTimestamp,
                    "Service");

                dtB3 = ws_Get_B2TicketSummary(
                    sB1CustomerNumberForB2,
                    sB1CustomerLocationForB2,
                    sTicket,
                    sB2OpenClosedOrAll,
                    sB2StartTimestamp,
                    sB2EndTimestamp,
                    "Project");
            }

            dt = Merge_HistoryTables(dtB1, dtB2, dtB3);
        }

        if (excelVersionY == "Y")
        {
            if (dt.Rows.Count > 0)
            {
                if (sRunModelQuery == "Y")
                {
                    try 
                    {
                        // Rename Columns
                        dt.Columns["MODEL"].ColumnName = "Model";
                        dt.Columns["MODELCOUNT"].ColumnName = "Model Count";

                        DownloadHandler dh = new DownloadHandler();
                        string sCsv = dh.DataTableToExcelCsv(dt);
                        dh = null;

                        Response.ClearContent();
                        Response.ContentType = "application/ms-excel";
                        sTemp = "attachment; filename=ServHist_" + sCustomerNumber;
                        if (!String.IsNullOrEmpty(sCustomerLocation))
                            sTemp += "-" + sCustomerLocation.ToString();
                        sTemp += ".csv";
                        Response.AddHeader("content-disposition", sTemp);
                        Response.Write(sCsv);
                    }
                    catch (Exception ex)
                    {
                        string sReturn = ex.ToString();
                    }

                }
                else
                {
                    try
                    {

                        // Remove unnecessary columns from Excel
                        dt.Columns.Remove("Center");
                        dt.Columns.Remove("Ticket");
                        if (hfPrimaryCs1.Value != "89866")
                            dt.Columns.Remove("Trips");
                        dt.Columns.Remove("ModelSource");
                        dt.Columns.Remove("StsTicket"); // ??? confirm!
                        dt.Columns.Remove("SortEntered");
                        dt.Columns.Remove("SortCompleted");
                        dt.Columns.Remove("Summary");
                        dt.Columns.Remove("Source");
                        dt.AcceptChanges();

                        // Rename columns
                        dt.Columns["Call"].ColumnName = "TicketX";
                        dt.Columns["TicketXRef"].ColumnName = "TicketCrossRefX";
                        dt.Columns["Status"].ColumnName = "StatusX";
                        dt.Columns["Comment"].ColumnName = "CommentX";
                        dt.Columns["Customer"].ColumnName = "CustomerX";
                        dt.Columns["Location"].ColumnName = "LocationX";
                        dt.Columns["CustXref"].ColumnName = "CustCrossRefX";
                        dt.Columns["CustomerName"].ColumnName = "CustomerNameX";
                        dt.Columns["Address1"].ColumnName = "Address1X";
                        dt.Columns["Address2"].ColumnName = "Address2X";
                        dt.Columns["Address3"].ColumnName = "Address3X";
                        dt.Columns["City"].ColumnName = "CityX";
                        dt.Columns["State"].ColumnName = "StateX";
                        dt.Columns["Zip"].ColumnName = "ZipX";
                        dt.Columns["Contact"].ColumnName = "ContactX";
                        dt.Columns["Phone"].ColumnName = "PhoneX";
                        dt.Columns["Ext"].ColumnName = "ExtX";
                        dt.Columns["PartsUsed"].ColumnName = "PartsUsedX";
                        dt.Columns["Model"].ColumnName = "ModelX";
                        dt.Columns["ModelDescription"].ColumnName = "ModelDescriptionX";
                        dt.Columns["Serial"].ColumnName = "SerialX";
                        dt.Columns["ModelXRef"].ColumnName = "ModelCrossRefX";
                        dt.Columns["DateEntered"].ColumnName = "DateEnteredX";
                        dt.Columns["TimeEntered"].ColumnName = "TimeEnteredX";
                        dt.Columns["DateCompleted"].ColumnName = "DateCompletedX";
                        dt.Columns["TimeCompleted"].ColumnName = "TimeCompletedX";
                        dt.Columns["CallType"].ColumnName = "TicketTypeX";
                        if (hfPrimaryCs1.Value == "89866")
                            dt.Columns["Trips"].ColumnName = "TripsX";

                        dt.AcceptChanges();

                        // Add Columns for the Excel Order
                        dt.Columns.Add(MakeColumn("Ticket"));
                        dt.Columns.Add(MakeColumn("Ticket Cross Ref"));
                        dt.Columns.Add(MakeColumn("Ticket Type"));
                        dt.Columns.Add(MakeColumn("Ticket Status"));
                        dt.Columns.Add(MakeColumn("Ticket Comment"));
                        if (hfPrimaryCs1.Value == "89866")
                            dt.Columns.Add(MakeColumn("Trips"));

                        dt.Columns.Add(MakeColumn("Date Entered"));
                        dt.Columns.Add(MakeColumn("Time Entered"));
                        dt.Columns.Add(MakeColumn("Date Completed"));
                        dt.Columns.Add(MakeColumn("Time Completed"));

                        dt.Columns.Add(MakeColumn("Model"));
                        dt.Columns.Add(MakeColumn("Serial"));
                        dt.Columns.Add(MakeColumn("Model Description"));
                        dt.Columns.Add(MakeColumn("Model Cross Ref"));
                        dt.Columns.Add(MakeColumn("Parts Used"));

                        dt.Columns.Add(MakeColumn("Customer Name"));
                        dt.Columns.Add(MakeColumn("Customer"));
                        dt.Columns.Add(MakeColumn("Location"));
                        dt.Columns.Add(MakeColumn("Cust Cross Ref"));
                        dt.Columns.Add(MakeColumn("Address 1"));
                        dt.Columns.Add(MakeColumn("Address 2"));
                        dt.Columns.Add(MakeColumn("Address 3"));
                        dt.Columns.Add(MakeColumn("City"));
                        dt.Columns.Add(MakeColumn("State"));
                        dt.Columns.Add(MakeColumn("Zip"));
                        dt.Columns.Add(MakeColumn("Contact"));
                        dt.Columns.Add(MakeColumn("Phone"));
                        dt.Columns.Add(MakeColumn("Ext"));
                        dt.AcceptChanges();

                        foreach (DataRow row in dt.Rows)
                        {
                            row["Ticket"] = row["TicketX"].ToString().Trim();
                            row["Ticket Cross Ref"] = row["TicketCrossRefX"].ToString().Trim();
                            row["Ticket Type"] = row["TicketTypeX"].ToString().Trim();
                            row["Ticket Status"] = row["StatusX"].ToString().Trim();
                            row["Ticket Comment"] = row["CommentX"].ToString().Trim();
                            if (hfPrimaryCs1.Value == "89866")
                                row["Trips"] = row["TripsX"].ToString().Trim();
                            row["Date Entered"] = row["DateEnteredX"].ToString().Trim();
                            row["Time Entered"] = row["TimeEnteredX"].ToString().Trim();
                            row["Date Completed"] = row["DateCompletedX"].ToString().Trim();
                            row["Time Completed"] = row["TimeCompletedX"].ToString().Trim();

                            row["Model"] = row["ModelX"].ToString().Trim();
                            row["Serial"] = row["SerialX"].ToString().Trim();
                            row["Model Description"] = row["ModelDescriptionX"].ToString().Trim();
                            row["Model Cross Ref"] = row["ModelCrossRefX"].ToString().Trim();
                            row["Parts Used"] = row["PartsUsedX"].ToString().Trim();

                            row["Customer Name"] = row["CustomerNameX"].ToString().Trim();
                            row["Customer"] = row["CustomerX"].ToString().Trim();
                            row["Location"] = row["LocationX"].ToString().Trim();
                            row["Cust Cross Ref"] = row["CustCrossRefX"].ToString().Trim();

                            row["Address 1"] = row["Address1X"].ToString().Trim();
                            row["Address 2"] = row["Address2X"].ToString().Trim();
                            row["Address 3"] = row["Address3X"].ToString().Trim();
                            row["City"] = row["CityX"].ToString().Trim();
                            row["State"] = row["StateX"].ToString().Trim();
                            row["Zip"] = row["ZipX"].ToString().Trim();
                            row["Contact"] = row["ContactX"].ToString().Trim();
                            row["Phone"] = row["PhoneX"].ToString().Trim();
                            row["Ext"] = row["ExtX"].ToString().Trim();

                        }

                        dt.AcceptChanges();

                        // Remove All Original Columns (Leaving properly sorted columns)
                        dt.Columns.Remove("TicketX");
                        dt.Columns.Remove("TicketCrossRefX");
                        dt.Columns.Remove("CommentX");
                        dt.Columns.Remove("CustomerX");
                        dt.Columns.Remove("LocationX");
                        dt.Columns.Remove("CustCrossRefX");
                        dt.Columns.Remove("CustomerNameX");
                        dt.Columns.Remove("Address1X");
                        dt.Columns.Remove("Address2X");
                        dt.Columns.Remove("Address3X");
                        dt.Columns.Remove("CityX");
                        dt.Columns.Remove("StateX");
                        dt.Columns.Remove("ZipX");
                        dt.Columns.Remove("ContactX");
                        dt.Columns.Remove("PhoneX");
                        dt.Columns.Remove("ExtX");
                        dt.Columns.Remove("PartsUsedX");
                        dt.Columns.Remove("ModelX");
                        dt.Columns.Remove("ModelDescriptionX");
                        dt.Columns.Remove("SerialX");
                        dt.Columns.Remove("ModelCrossRefX");
                        dt.Columns.Remove("DateEnteredX");
                        dt.Columns.Remove("TimeEnteredX");
                        dt.Columns.Remove("DateCompletedX");
                        dt.Columns.Remove("TimeCompletedX");
                        dt.Columns.Remove("TicketTypeX");
                        dt.Columns.Remove("StatusX");
                        if (hfPrimaryCs1.Value == "89866")
                            dt.Columns.Remove("TripsX");

                        dt.AcceptChanges();

                        DownloadHandler dh = new DownloadHandler();
                        string sCsv = dh.DataTableToExcelCsv(dt);
                        dh = null;

                        Response.ClearContent();
                        Response.ContentType = "application/ms-excel";
                        sTemp = "attachment; filename=ServHist_" + sCustomerNumber;
                        if (!String.IsNullOrEmpty(sCustomerLocation))
                            sTemp += "-" + sCustomerLocation.ToString();
                        sTemp += ".csv";
                        Response.AddHeader("content-disposition", sTemp);
                        Response.Write(sCsv);
                    }
                    catch (Exception ex)
                    {
                        string sReturn = ex.ToString();
                    }
                }
            }
            if (dt.Rows.Count == 0)
                lbMsg.Text = "Search matched no records for the Excel download";
            else
                Response.End();
        }
        else
        {

            if (sRunModelQuery == "Y")
            {
                ViewState["vsDataTable_Mod"] = null;
                BindGrid_Mod(dt);
            }
            else
            {
                ViewState["vsDataTable_Hst"] = null;
                BindGrid_Hst(dt);
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkHistoryCall_Click(object sender, EventArgs e)
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
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
