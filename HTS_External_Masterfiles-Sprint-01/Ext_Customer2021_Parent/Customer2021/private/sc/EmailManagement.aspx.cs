using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_sc_EmailManagement : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    // sg prefix means s:string g:global variable
    string sgCustomerNumber = "";  
    string sgCustomerLocation = "";

    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMessage.Text = "";
        lbError.Text = "";

        if (!IsPostBack) 
        {
            Get_UserPrimaryCustomerNumber();

            Load_CurrentCustomerNumberAndLocation();
                string sCustomerName = ws_Get_B1CustomerName(sgCustomerNumber, sgCustomerLocation);
                lbTogglePanels.Text = "All locations under " + sCustomerName + ": Customer " + sgCustomerNumber;

                if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
                {
                    
                    pnChildLocationSelection.Visible = true;
                    rblUmberellaSize.Visible = true;
                    Load_FamilyMemberDropDownList();
                    pnSearchCustomerFamily.Visible = true;
                    btTogglePanels.Text = "ALL Locations";
                    // --------------------------------------------------------------------
                    // --------------------------------------------------------------------
                }
                else 
                {
                    pnParentOrChildLocation.Visible = true;
                    pnSearchCustomerFamily.Visible = false;
                    // --------------------------------------------------------------------
                    if (!String.IsNullOrEmpty(sgCustomerNumber))
                    {
                        btTogglePanels.Text = "INDIVIDUAL Locations";
                        Load_CallOpenAndCloseEmailFlags(sgCustomerNumber, sgCustomerLocation);

                        Load_CallOpenOrCloseEmailAddresses(sgCustomerNumber, sgCustomerLocation, "AtOpen");
                        Load_CallOpenOrCloseEmailAddresses(sgCustomerNumber, sgCustomerLocation, "AtClose");
                    }
                    // --------------------------------------------------------------------
                }
            // -------------------------------------------------------------------------------
            

            // -------------------------------------------------------------------------------

            
        }

    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ------------------------------------------------------------------------
    protected void Load_LocationDataTables()
    {
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");
        DataTable dt = new DataTable("");

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
        else if (hfPrimaryCs1Type.Value != "LRG" && hfPrimaryCs1Type.Value != "DLR") // Then the primary is ok to use
            sCustomerNumber = hfPrimaryCs1.Value;

        string sCustomerName = txSearchName.Text.Trim().ToUpper().Trim();
        string sCustomerLocation = txSearchLocation.Text.Trim();
        string sContact = txSearchContact.Text.Trim().ToUpper().Trim();
        string sAddress = txSearchAddress.Text.Trim().ToUpper().Trim();
        string sCity = txSearchCity.Text.Trim().ToUpper().Trim();
        string sState = txSearchState.Text.Trim().ToUpper().Trim();
        string sZip = txSearchZip.Text.Trim().ToUpper().Trim();
        string sPhone = txSearchPhone.Text.Trim().ToUpper().Trim();
        string sXref = txSearchXref.Text.Trim().ToUpper().Trim();

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
                    sContact,
                    sAddress,
                    sCity,
                    sState,
                    sZip,
                    sPhone,
                    sXref);

                // Merge
                dt = Merge_LocationTables(dtB1, dtB2);
            }
            if (dt.Rows.Count > 0)
            {
                
                rp_LocationSmall.DataSource = dt;
                rp_LocationSmall.DataBind();

                ViewState["vsDataTable_Loc"] = null;
                BindGrid_Loc(dt);

                rp_LocationSmall.Visible = true;
                gv_LocationLarge.Visible = true;
            }
            else 
            {
                rp_LocationSmall.Visible = false;
                gv_LocationLarge.Visible = false;
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
    // ------------------------------------------------------------------------
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
        dt.Columns.Add(MakeColumn("CombinedEqpCount"));
        dt.Columns.Add(MakeColumn("CombinedEqpCountSort"));
        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        int iRowIdx = 0;
        int iB1EqpCount = 0;
        //int iB2EqpCount = 0;
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
            dt.Rows[iRowIdx]["FLAG8"] = row["FLAG8"].ToString().Trim();
            dt.Rows[iRowIdx]["ORCACC"] = row["ORCACC"].ToString().Trim();
            dt.Rows[iRowIdx]["B1EqpCount"] = row["B1EqpCount"].ToString().Trim();

            if (int.TryParse(row["b1EqpCount"].ToString().Trim(), out iB1EqpCount) == false)
                iB1EqpCount = -1;

            if (int.TryParse(row["CombinedEqpCount"].ToString().Trim(), out iCombinedEqpCount) == false)
                iCombinedEqpCount = -1;

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
    protected string GetFamilyMemberType(string customerNumber)
    {
        int iCustomerNumber = 0;
        string sFamilyMemberType = "";

        if (int.TryParse(customerNumber, out iCustomerNumber) == false)
            iCustomerNumber = -1;
        if (iCustomerNumber > 0)
        {
            if (!String.IsNullOrEmpty(hfChosenCs2.Value))
            {
                sFamilyMemberType = "CHILD";
            }
            else
            {
                string sPassedCustomerType = ws_Get_B1CustomerType(iCustomerNumber);

                if (sPassedCustomerType == "LRG" || sPassedCustomerType == "DLR")
                    sFamilyMemberType = "GRANDPARENT";
                else
                    sFamilyMemberType = "PARENT";
            }
        }
        return sFamilyMemberType;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_CallOpenOrCloseEmailAddresses(string customerNumber, string customerLocation, string atOpenOrAtClose)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt = ws_Get_B1CallOpenOrCloseEmails(customerNumber, customerLocation, atOpenOrAtClose);
        if (atOpenOrAtClose == "AtOpen")
        {
            if (dt.Rows.Count > 0)
            {
                gv_EmailsAtOpenToDelete.Visible = true;
                gv_EmailsAtOpenToDelete.DataSource = dt;
                gv_EmailsAtOpenToDelete.DataBind();
            }
            else
                gv_EmailsAtOpenToDelete.Visible = false;
        }
        else // AtClose
        {
            if (dt.Rows.Count > 0)
            {
                gv_EmailsAtCloseToDelete.Visible = true;
                gv_EmailsAtCloseToDelete.DataSource = dt;
                gv_EmailsAtCloseToDelete.DataBind();
            }
            else
                gv_EmailsAtCloseToDelete.Visible = false;
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_CurrentCustomerNumberAndLocation()
    {
        sgCustomerNumber = "";
        sgCustomerLocation = "";

        int iCustomerNumber = 0;
        int iCustomerLocation = 0;

        if (!String.IsNullOrEmpty(hfChosenCs1.Value))
            sgCustomerNumber = hfChosenCs1.Value;
        else
            sgCustomerNumber = hfPrimaryCs1.Value;

        if (!String.IsNullOrEmpty(hfChosenCs2.Value))
            sgCustomerLocation = hfChosenCs2.Value;

        if (!String.IsNullOrEmpty(sgCustomerNumber))
        {
            if (int.TryParse(sgCustomerNumber, out iCustomerNumber) == false)
            {
                iCustomerNumber = -1;
                sgCustomerNumber = "";
            }
            else if (iCustomerNumber > 0)
            {
                sgCustomerNumber = iCustomerNumber.ToString();
            }
        }
        if (!String.IsNullOrEmpty(sgCustomerLocation))
        {
            if (int.TryParse(sgCustomerLocation, out iCustomerLocation) == false)
            {
                iCustomerLocation = -1;
                sgCustomerLocation = "";
            }

            else if (iCustomerLocation > -1)
            {
                sgCustomerLocation = iCustomerLocation.ToString();
            }
        }

    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_FamilyMemberDropDownList()
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
                }
                ddSearchCustomerFamily.Items.Insert(i, new System.Web.UI.WebControls.ListItem(saNamNum[1] + "  " + saNamNum[0], saNamNum[1]));
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
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Loc)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Loc(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But because you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Loc = "";

        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbMessage.Text = "";
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dt;

            //if (dt.Rows.Count == 0)
            //{
            //    lbMessage.Text = "No matching locations were found...";
            //}
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Loc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
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
        {
            dt.DefaultView.Sort = sortExpression_Loc;
        }
        gv_LocationLarge.DataSource = dt.DefaultView;
        gv_LocationLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Loc(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_LocationLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Loc(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Loc(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Loc == SortDirection.Ascending)
                gridSortDirection_Loc = SortDirection.Descending;
            else
                gridSortDirection_Loc = SortDirection.Ascending;
        }
        else
            gridSortDirection_Loc = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Loc = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Loc(dt);
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
                //ViewState["GridSortDirection"] = SortDirection.Descending;
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
            if (ViewState["GridSortExpression_Loc"] == null)
            {
                ViewState["GridSortExpression_Loc"] = "CUSTNM"; // xxx *** INITIAL SORT ***
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
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // WS: STRINGS (start)
    // ========================================================================
    protected string ws_Upd_B1CustPref_OpenCloseEmailsAsActive_YesOrNo(
        string customerNumber,
        string customerLocation,
        string childParentOrGrandparent,
        string atOpenOrAtClose,
        string yesOrNo
        )
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Upd_B1CustPref_OpenCloseEmailsAsActive_YesOrNo";
            string sFieldList = "customerNumber|customerLocation|childParentOrGrandparent|atOpenOrAtClose|yesOrNo|x ";
            string sValueList = customerNumber + "|" + customerLocation + "|" + childParentOrGrandparent + "|" + atOpenOrAtClose + "|" + yesOrNo + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Add_B1CallOpenOrCloseEmails(
        string customerNumber,
        string customerLocation,
        string email,
        string atOpenOrAtClose
        )
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Add_B1CallOpenOrCloseEmails";
            string sFieldList = "customerNumber|customerLocation|email|atOpenOrAtClose|x ";
            string sValueList = customerNumber + "|" + customerLocation + "|" + email + "|" + atOpenOrAtClose + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Del_B1CallOpenOrCloseEmails(
        string customerNumber,
        string customerLocation,
        string email,
        string atOpenOrAtClose
        )
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Del_B1CallOpenOrCloseEmails";
            string sFieldList = "customerNumber|customerLocation|email|atOpenOrAtClose|x ";
            string sValueList = customerNumber + "|" + customerLocation + "|" + email + "|" + atOpenOrAtClose + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CallEmailFlagsForOpenAndClose_YN(
        string customerNumber,
        string customerLocation
        )
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CallEmailFlagsForOpenAndClose_YN";
            string sFieldList = "customerNumber|customerLocation|x ";
            string sValueList = customerNumber + "|" + customerLocation + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ========================================================================
    // WS: STRINGS (end)
    // ========================================================================
    // ========================================================================
    // WS: DATA TABLES (start)
    // ========================================================================
    protected DataTable ws_Get_B1CallOpenOrCloseEmails(
        string customerNumber,
        string customerLocation,
        string atOpenOrAtClose
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CallOpenOrCloseEmails";
            string sFieldList = "customerNumber|customerLocation|atOpenOrAtClose|x";
            string sValueList =
                customerNumber + "|" +
                customerLocation + "|" +
                atOpenOrAtClose + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    // WS: DATA TABLES (end)
    // ========================================================================

    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btLocationSearchSubmit_Click(object sender, EventArgs e)
    {
        Load_LocationDataTables();
    }
    // -------------------------------------------------------------------------------------------------
    protected void btTogglePanels_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;
        //string sParms = myControl.CommandArgument.ToString();
        //string[] saArg = new string[2];
        //saArg = sParms.Split('|');
        string sButtonTitle = myControl.Text.Trim();
        string sCustomerName = "";


        if (sButtonTitle == "ALL Locations" 
            && rblUmberellaSize.Visible == true 
            && rblUmberellaSize.SelectedValue == "OneGroup" 
            && ddSearchCustomerFamily.SelectedValue == ""
            ) 
        {
            lbError.Text = "A parent customer must be selected from the drop down list to change settings for that customer group";
        }
        else
        {
            try
            {
                pnChildLocationSelection.Visible = false;
                pnParentOrChildLocation.Visible = false;
                //pnOneLocation.Visible = false;

                //if (sButtonTitle == "Switch To Manage INDIVIDUAL Locations")
                if (sButtonTitle == "INDIVIDUAL Locations")
                {
                    Load_CurrentCustomerNumberAndLocation();

                    pnChildLocationSelection.Visible = true;
                    if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
                        rblUmberellaSize.Visible = true;

                    btTogglePanels.Text = "ALL Locations";
                    Load_LocationDataTables();

                    sCustomerName = ws_Get_B1CustomerName(sgCustomerNumber, sgCustomerLocation);
                    lbTogglePanels.Text = "Locations under " + sCustomerName + ": Customer " + sgCustomerNumber;

                }
                else if (sButtonTitle == "ALL Locations")
                {
                    hfChosenCs1.Value = "";
                    hfChosenCs2.Value = "";

                    if (rblUmberellaSize.Visible == true
                        && rblUmberellaSize.SelectedValue == "OneGroup"
                        )
                        hfChosenCs1.Value = ddSearchCustomerFamily.SelectedValue;

                    Load_CurrentCustomerNumberAndLocation();

                    pnParentOrChildLocation.Visible = true;
                    rblUmberellaSize.Visible = false;
                    btTogglePanels.Text = "INDIVIDUAL Locations";
                    // Initialize hidden fields before they pick a new one

                    Load_CallOpenAndCloseEmailFlags(sgCustomerNumber, sgCustomerLocation);

                    Load_CallOpenOrCloseEmailAddresses(sgCustomerNumber, sgCustomerLocation, "AtOpen");
                    Load_CallOpenOrCloseEmailAddresses(sgCustomerNumber, sgCustomerLocation, "AtClose");

                    sCustomerName = ws_Get_B1CustomerName(sgCustomerNumber, "");
                    lbTogglePanels.Text = "All locations under " + sCustomerName + ": Customer " + sgCustomerNumber;
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
    // ----------------------------------------------------------------------------------------------------
    protected void lkName_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');

        if (saArg.Length > 1)
        {
            // Set the chosen customer to use going forward
            int iCustomerNumber = 0;
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = 0;
            int iCustomerLocation = 0;
            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = 0;

            if (iCustomerNumber > 0) 
            {
                hfChosenCs1.Value = iCustomerNumber.ToString();
                hfChosenCs2.Value = iCustomerLocation.ToString();
            }

            pnChildLocationSelection.Visible = false;
            pnParentOrChildLocation.Visible = true;
            //pnOneLocation.Visible = false;

            //pnOneLocation.Visible = true;

            string sCustomerChildName = ws_Get_B1CustomerName(iCustomerNumber.ToString(), iCustomerLocation.ToString());
            lbTogglePanels.Text = "Emails Only For " + sCustomerChildName + ": Customer " + iCustomerNumber.ToString() + "-" + iCustomerLocation.ToString();

            Load_CallOpenAndCloseEmailFlags(iCustomerNumber.ToString(), iCustomerLocation.ToString());

            Load_CallOpenOrCloseEmailAddresses(iCustomerNumber.ToString(), iCustomerLocation.ToString(), "AtOpen");
            Load_CallOpenOrCloseEmailAddresses(iCustomerNumber.ToString(), iCustomerLocation.ToString(), "AtClose");
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void rblEmailSwitch_Click(object sender, EventArgs e)
    {
        RadioButtonList rblControl = (RadioButtonList)sender;
        //string sOpnOrCls = rblControl.CommandArgument.ToString();
        string sYesorNo = rblControl.SelectedValue.ToString();
        string sRadioID = rblControl.ID.ToString();
        string sAtOpenOrAtClose = "";
        string sOpenOrCloseText = "";
        string sChildParentOrGrandparent = "";
        string sYesOrNo = "";
        string sResult = "";

        if (sRadioID == "rblOpen")
        {
            sAtOpenOrAtClose = "AtOpen";
            sOpenOrCloseText = "Ticket OPEN email is now";
            sYesOrNo = rblOpen.SelectedValue;
        }
        else if (sRadioID == "rblClose")
        {
            sAtOpenOrAtClose = "AtClose";
            sOpenOrCloseText = "Ticket CLOSE email is now";
            sYesOrNo = rblClose.SelectedValue;
        }

        if (sYesOrNo.ToLower() == "yes")
        {
            sYesOrNo = "Yes";
            sOpenOrCloseText += " ACTIVE";
        }

        else
        {
            sYesOrNo = "No";
            sOpenOrCloseText += " INACTIVE";
        }

        Load_CurrentCustomerNumberAndLocation();

        sChildParentOrGrandparent = GetFamilyMemberType(sgCustomerNumber);

        if (!String.IsNullOrEmpty(sgCustomerNumber))
            sResult = ws_Upd_B1CustPref_OpenCloseEmailsAsActive_YesOrNo(
                sgCustomerNumber, 
                sgCustomerLocation, 
                sChildParentOrGrandparent, 
                sAtOpenOrAtClose, 
                sYesOrNo
                );

        if (sResult == "SUCCESS")
        {
            lbError.Text = sOpenOrCloseText;
            Load_CallOpenAndCloseEmailFlags(sgCustomerNumber, sgCustomerLocation);
        }
        else
        {
            lbError.Text = "Update failed: " + sResult;
        }
            

        // --------------------   
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkEmailToAdd_Click(object sender, EventArgs e)
    {
        lbMessage.Text = "";
        lbError.Text = "";

        LinkButton linkControl = (LinkButton)sender;
        string sAtOpenOrAtClose = linkControl.CommandArgument.ToString();
        string sEmail = "";
        string sResult = "";

        if (sAtOpenOrAtClose.ToLower().Contains("open"))
            sEmail = txOpenAdd.Text;
        else // if (sOpnOrCls == "CLS")
            sEmail = txCloseAdd.Text;

        bool bValid = isEmailFormatValid(sEmail);
        if (bValid == false)
        {
            if (sAtOpenOrAtClose.ToLower().Contains("open"))
                lbError.Text = "Your open email entry format appears to be invalid";
            else
                lbError.Text = "Your closed email entry format appears to be invalid";
        }
        else if (!String.IsNullOrEmpty(sEmail) && sEmail.Length > 50)
        {
            sEmail = sEmail.Substring(0, 50);
            if (sAtOpenOrAtClose.ToLower().Contains("open"))
                lbError.Text = "Your open email entry is too long (50 max)";
            else
                lbError.Text = "Your closed email entry is too long (50 max)";
        }
        else
        {
            Load_CurrentCustomerNumberAndLocation();

            if (!String.IsNullOrEmpty(sgCustomerNumber) && !String.IsNullOrEmpty(sEmail))
            {
                sResult = ws_Add_B1CallOpenOrCloseEmails(sgCustomerNumber, sgCustomerLocation, sEmail, sAtOpenOrAtClose);
            }
            // --------------------   
            if (sResult.StartsWith("SUCCESS"))
            { 
                // Update the Screen Tables
                if (sAtOpenOrAtClose.ToLower().Contains("open"))
                {
                    txOpenAdd.Text = "";
                    Load_CallOpenEmails(sgCustomerNumber, sgCustomerLocation);
                    lbMessage.Text = "Email at open added for " + sEmail;
                }
                else // if (sOpnOrCls.ToLower().Contains("close"))
                {
                    txCloseAdd.Text = "";
                    Load_CallCloseEmails(sgCustomerNumber, sgCustomerLocation);
                    lbMessage.Text = "Email at close added for " + sEmail;
                }
            }
            else 
            {
                lbError.Text = "Error: Email entry attempt did not succeed";
            }
            // --------------------   
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkEmailToDelete_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');

        string sEmail = saArg[0].Trim();
        string sAtOpenOrAtClose = saArg[1].Trim();
        string sResult = "";

        Load_CurrentCustomerNumberAndLocation();

        sResult = ws_Del_B1CallOpenOrCloseEmails(sgCustomerNumber, sgCustomerLocation, sEmail, sAtOpenOrAtClose);
        // --------------------   
        if (sResult.StartsWith("SUCCESS"))
        {
            // Update the Screen Tables
            if (sAtOpenOrAtClose.ToLower().Contains("open"))
            {
                txOpenAdd.Text = "";
                Load_CallOpenEmails(sgCustomerNumber, sgCustomerLocation);
                lbMessage.Text = "Email at open deleted for " + sEmail;
            }
            else // if (sOpnOrCls.ToLower().Contains("close"))
            {
                txCloseAdd.Text = "";
                Load_CallCloseEmails(sgCustomerNumber, sgCustomerLocation);
                lbMessage.Text = "Email at close deleted for " + sEmail;
            }
        }
        else
        {
            lbError.Text = "Error: Email delete attempt did not succeed";
        }
        // --------------------   
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_CallOpenAndCloseEmailFlags(string customerNumber, string customerLocation)
    {
        string sOpenCloseList = "";
        string[] saOpenClose = new string[2];

        sOpenCloseList = ws_Get_B1CallEmailFlagsForOpenAndClose_YN(customerNumber, customerLocation);
        saOpenClose = sOpenCloseList.Split('|');

        if (saOpenClose.Length > 1) 
        {
            if (saOpenClose[0] == "Y")
                rblOpen.SelectedValue = "YES";
            else
                rblOpen.SelectedValue = "NO";
            if (saOpenClose[1] == "Y")
                rblClose.SelectedValue = "YES";
            else
                rblClose.SelectedValue = "NO";
        }

    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_CallOpenEmails(string customerNumber, string customerLocation)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        
        dt = ws_Get_B1CallOpenOrCloseEmails(customerNumber, customerLocation, "AtOpen");

        if (dt.Rows.Count > 0)
        {
            gv_EmailsAtOpenToDelete.DataSource = dt;
            gv_EmailsAtOpenToDelete.DataBind();
            gv_EmailsAtOpenToDelete.Visible = true;
        }
        else 
        {
            gv_EmailsAtOpenToDelete.Visible = false;
        }

    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_CallCloseEmails(string customerNumber, string customerLocation)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt = ws_Get_B1CallOpenOrCloseEmails(customerNumber, customerLocation, "AtClose");

        if (dt.Rows.Count > 0)
        {
            gv_EmailsAtCloseToDelete.DataSource = dt;
            gv_EmailsAtCloseToDelete.DataBind();
            gv_EmailsAtCloseToDelete.Visible = true;
        }
        else 
        {
            gv_EmailsAtCloseToDelete.Visible = false;
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ========================================================================
    // ========================================================================

}
