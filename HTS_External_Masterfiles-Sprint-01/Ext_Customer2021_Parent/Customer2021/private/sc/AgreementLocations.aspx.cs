using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_sc_AgreementLocations : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    string sTemp = "";

    // -------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
        

        if (!IsPostBack)
        {
            Get_UserPrimaryCustomerNumber();

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
                        Load_LocationDataTables();
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
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected string ws_Upd_B1LocationCrossRef(string locationCrossRef, string customerNumber, string customerLocation)
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(locationCrossRef) && !String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Upd_B1LocationCrossRef";
            string sFieldList = "locationCrossRef|customerNumber|customerLocation|x";
            string sValueList = locationCrossRef.Trim() + "|" + customerNumber + "|" + customerLocation + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }
        return sResult;
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1LocationProductCodes(
        string customerNumber,
        string customerLocation
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1LocationProductCodes";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList =
                customerNumber.ToString() + "|" +
                customerLocation.ToString() + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1LocationEquipment(
        int customerNumber,
        int customerLocation,
        string productCode,
        string model,
        string serial,
        string modelDescription,
        string asset,
        string agentId,
        string downloadExcelY
        )
    {
        DataTable dt = new DataTable("");

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1LocationEquipment";
            string sFieldList = "customerNumber|customerLocation|productCode|model|serial|modelDescription|asset|agentId|downloadExcelY|x";
            string sValueList =
                customerNumber.ToString() + "|" +
                customerLocation.ToString() + "|" +
                productCode + "|" +
                model + "|" +
                serial + "|" +
                modelDescription + "|" +
                asset + "|" +
                agentId + "|" +
                downloadExcelY + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_AllowEquipmentCrossRefUpdate_YN(int customerNumber)
    {
        string sAllowUpdate = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustPref_AllowEquipmentCrossRefUpdate_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sAllowUpdate = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAllowUpdate;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // -------------------------------------------------------------------------------------------------
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
    // -------------------------------------------------------------------------------------------------
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
        else
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
            rp_LocationSmall.DataSource = dt;
                rp_LocationSmall.DataBind();

                ViewState["vsDataTable_Loc"] = null;
                BindGrid_Loc(dt);

                FormatRowsIn_Repeater();
                FormatRowsIn_GridView();

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // -------------------------------------------------------------------------------------------------
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
            }
            dt.Rows[iRowIdx]["CombinedEqpCountSort"] = iCombinedEqpCount.ToString("00000");

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
    // -------------------------------------------------------------------------------------------------
    protected void FormatRowsIn_Repeater()
    {
        HiddenField hfTemp = new HiddenField();
        Panel pnTemp = new Panel();

        string sType = "";
        string sSource = "";
        int iBL1EquipmentCount = -1; // Negative 1 to start here becasue zero would be valid a lot...

        foreach (Control c1 in rp_LocationSmall.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfCompanySegment")
                            sSource = hfTemp.Value;
                        else if (hfTemp.ID == "hfB1EqpCount") 
                        {
                            if (int.TryParse(hfTemp.Value, out iBL1EquipmentCount) == false)
                                iBL1EquipmentCount = -1;
                        }
                            
                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Panel"))
                    {
                        pnTemp = (Panel)c2;
                        if (pnTemp.ID == "pnEditXref")
                        {
                            if (
                                hfPreferenceToAllowLocationCrossRefUpdate.Value == "Y" && // they MUST have the preference
                                (sSource != "BL2" // and be a REG BL1 customer
                                || 
                                (sSource == "BL2" && iBL1EquipmentCount > 0)  // or they are Both a BL2 customer and have BL1 Equipment on contract
                                )
                                )
                                pnTemp.Visible = true;
                            else
                                pnTemp.Visible = false;

                            // Now that you're done here, initialize your flag field for next loop
                            sSource = "";
                            iBL1EquipmentCount = -1;

                        }
                    }
                    //-------------------------------------------------------------------------
                    //-------------------------------------------------------------------------
                }
            }
        }

    }
    // -------------------------------------------------------------------------------------------------
    protected void FormatRowsIn_GridView()
    {

        HiddenField hfTemp = new HiddenField();
        LinkButton lkTemp = new LinkButton();
        //Panel pnTemp = new Panel();

        string sType = "";
        string sSource = "";
        int iBL1EquipmentCount = -1; // Negative 1 to start here becasue zero would be valid a lot...

        try
        {
            foreach (Control c1 in gv_LocationLarge.Controls)
            {
                sType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.ChildTable"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.GridViewRow"))
                        {
                            foreach (Control c3 in c2.Controls)
                            {
                                sType = c3.GetType().ToString();
                                if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
                                {
                                    foreach (Control c4 in c3.Controls)
                                    {
                                        // ==========================================================================
                                        sType = c4.GetType().ToString();
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                                        {
                                            hfTemp = (HiddenField)c4;
                                            if (hfTemp.ID == "hfCompanySegment")
                                            {
                                                sSource = hfTemp.Value;
                                            }
                                            else if (hfTemp.ID == "hfB1EqpCount")
                                            {
                                                if (int.TryParse(hfTemp.Value, out iBL1EquipmentCount) == false)
                                                    iBL1EquipmentCount = -1;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                                        {
                                            lkTemp = (LinkButton)c4;
                                            if (lkTemp.ID == "lkLoadLocationXrefForEdit")
                                            {
                                                if (
                                                    hfPreferenceToAllowLocationCrossRefUpdate.Value == "Y" && // they MUST have the preference
                                                    (sSource != "BL2" // and be a REG BL1 customer
                                                    ||
                                                    (sSource == "BL2" && iBL1EquipmentCount > 0)  // or they are Both a BL2 customer and have BL1 Equipment on contract
                                                    )
                                                    )
                                                        lkTemp.Visible = true;
                                                else
                                                    lkTemp.Visible = false;

                                                // Now that you're done here, initialize your flag field for next loop
                                                sSource = "";
                                                iBL1EquipmentCount = 0;
                                            }
                                        }
                                        // ==========================================================================
                                    }

                                }
                            }
                        }
                    }
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
            {
                hfPrimaryCs1Type.Value = ws_Get_B1CustomerType(iCustomerNumber);
                hfPreferenceToAllowLocationCrossRefUpdate.Value = ws_Get_B1CustPref_AllowEquipmentCrossRefUpdate_YN(iCustomerNumber);
            }
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
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null
        
        string sortExpression_Loc = "";

        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching locations were found...";
                lbMsg.Visible = true;
            }
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
            dt.DefaultView.Sort = sortExpression_Loc;

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
    #region actionEvents
    // ========================================================================
    protected void btSearchLocationSubmit_Click(object sender, EventArgs e)
    {
        Load_LocationDataTables();
    }
    // ========================================================================
    protected void btSearchLocationClear_Click(object sender, EventArgs e)
    {
        txSearchAddress.Text = "";
        txSearchCity.Text = "";
        txSearchContact.Text = "";
        txSearchLocation.Text = "";
        txSearchName.Text = "";
        txSearchPhone.Text = "";
        txSearchState.Text = "";
        txSearchXref.Text = "";
        txSearchZip.Text = "";
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkEquipCount_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');

        string sUrl = "";

        if (saArg.Length > 1)
        {
            // Set the chosen customer to use going forward
            int iCustomerNumber = 0;
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = 0;
            int iCustomerLocation = 0;
            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = 0;


            sUrl = "~/private/sc/AgreementEquipment.aspx" +
                "?src=" + "1" + 
                "&cs1=" + iCustomerNumber.ToString() +
                "&cs2=" + iCustomerLocation.ToString();

            Response.Redirect(sUrl, false);
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkLoadLocationXrefForEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('|');
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
        string sCustomerXref = "";

        if (saArg.Length > 2)
        {
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = -1;

            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = -1;

            sCustomerXref = saArg[2].Trim();

            // If you have data, 
            if (iCustomerNumber > 0 && !String.IsNullOrEmpty(sCustomerXref))
            {
                lbUpdateLocationXref_Customer.Text = ws_Get_B1CustomerName(iCustomerNumber.ToString(), iCustomerLocation.ToString());
                hfUpdateLocationXref_Cs1Cs2.Value = iCustomerNumber.ToString() + "-" + iCustomerLocation.ToString();
                txUpdateLocationXref_Xref.Text = sCustomerXref;

                pnUpdateLocationXref.Visible = true;
                txUpdateLocationXref_Xref.Focus();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateLocationXref_Click(object sender, EventArgs e)
    {
        string[] saCs1Cs2 = { "", "" };
        string sNewLocationXref = "";
        string sResult = "";
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;

        if (!String.IsNullOrEmpty(hfUpdateLocationXref_Cs1Cs2.Value))
            saCs1Cs2 = hfUpdateLocationXref_Cs1Cs2.Value.Split('-');
        if (saCs1Cs2.Length > 1)
        {
            if (int.TryParse(saCs1Cs2[0], out iCustomerNumber) == false)
                iCustomerNumber = -1;
            if (int.TryParse(saCs1Cs2[1], out iCustomerLocation) == false)
                iCustomerLocation = -1;

            if (iCustomerNumber > 0 && iCustomerLocation > -1) 
            {
                sNewLocationXref = txUpdateLocationXref_Xref.Text.Trim();

                sResult = ws_Upd_B1LocationCrossRef(sNewLocationXref, iCustomerNumber.ToString(), iCustomerLocation.ToString());

                if (sResult.StartsWith("SUCCESS")) 
                {
                    Load_LocationDataTables();
                    pnUpdateLocationXref.Visible = false;
                }
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateLocationXrefClose_Click(object sender, EventArgs e)
    {
        pnUpdateLocationXref.Visible = false;
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
