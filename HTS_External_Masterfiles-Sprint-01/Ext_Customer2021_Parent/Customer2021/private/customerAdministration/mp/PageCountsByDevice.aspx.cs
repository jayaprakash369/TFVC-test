using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_customerAdministration_mp_PageCountsByDevice : MyPage
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
                    Load_DeviceDataTables();
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
    protected DataTable ws_Get_B1CustomerManagedPrintDevices(
        string customerNumber,
        string customerLocation,
        string locationName,
        string locationCrossRef,
        string city,
        string state,
        string model,
        string serial,
        string equipmentCrossRef
    )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustomerManagedPrintDevices";
            string sFieldList = "customerNumber|customerLocation|locationName|locationCrossRef|city|state|model|serial|equipmentCrossRef|x";
            string sValueList = 
                customerNumber + "|" + 
                customerLocation + "|" + 
                locationName + "|" + 
                locationCrossRef +  "|" + 
                city + "|" + 
                state + "|" + 
                model + "|" + 
                serial +  "|" + 
                equipmentCrossRef + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }
        return dt;
    }
    // ========================================================================
    protected DataTable ws_Get_B1PageCountsForOneDevice(
        string unit,
        string startDate,
        string endDate
    )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(unit)
            && !String.IsNullOrEmpty(startDate)
            && !String.IsNullOrEmpty(endDate)
            )
        {
            string sJobName = "Get_B1PageCountsForOneDevice";
            string sFieldList = "unit|startDate|endDate|x";
            string sValueList =
                unit + "|" +
                startDate + "|" +
                endDate + 
                "|x";

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
                }
                ddSearchCustomerFamily.Items.Insert(i, new System.Web.UI.WebControls.ListItem(saNamNum[1] + "  " + saNamNum[0], saNamNum[1]));
            }

            ddSearchCustomerFamily.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        }

    }
    // -------------------------------------------------------------------------------------------------
    protected void Load_DeviceDataTables()
    {
        //DataTable dtB1 = new DataTable("B1");
        //DataTable dtB2 = new DataTable("B2");
        DataTable dt = new DataTable("Default");

        string sSearchCustomerNumber = "";
        int iSelectedCustomerNumber = 0;
        string sSearchCustomerLocation = "";
        string sSearchLocationName = "";
        string sSearchLocationCrossRef = "";
        string sSearchCity = "";
        string sSearchState = "";
        string sSearchModel = "";
        string sSearchSerial = "";
        string sSearchEquipmentCrossRef = "";

        if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
        {
            if (!String.IsNullOrEmpty(ddSearchCustomerFamily.SelectedValue))
            {
                if (int.TryParse(ddSearchCustomerFamily.SelectedValue, out iSelectedCustomerNumber) == false)
                    iSelectedCustomerNumber = -1;
            }
        }

        if (iSelectedCustomerNumber > 0) // i.e. LRG cust picked one of their own sub cust numbers
            sSearchCustomerNumber = iSelectedCustomerNumber.ToString();
        else
            sSearchCustomerNumber = hfPrimaryCs1.Value;

        sSearchCustomerLocation = txSearchLocation.Text.Trim().ToUpper();
        sSearchLocationName = txSearchLocationName.Text.Trim();
        sSearchLocationCrossRef = txSearchLocationCrossRef.Text.Trim().ToUpper();
        sSearchCity = txSearchCity.Text.Trim().ToUpper();
        sSearchState = txSearchState.Text.Trim().ToUpper(); 
        sSearchModel = txSearchModel.Text.Trim().ToUpper();
        sSearchSerial = txSearchSerial.Text.Trim().ToUpper();
        sSearchEquipmentCrossRef = txSearchEquipmentCrossRef.Text.Trim().ToUpper();

        try
        {
            if (
                    (hfPrimaryCs1Type.Value != "LRG" && hfPrimaryCs1Type.Value != "DLR") // Always load all locations if a regular customer
                    ||
                    ( // LRG/DLR must pick something!
                        (iSelectedCustomerNumber > 0)
                        || !String.IsNullOrEmpty(sSearchCustomerLocation)
                        || !String.IsNullOrEmpty(sSearchLocationName)
                        || !String.IsNullOrEmpty(sSearchLocationCrossRef)
                        || !String.IsNullOrEmpty(sSearchCity)
                        || !String.IsNullOrEmpty(sSearchState)
                        || !String.IsNullOrEmpty(sSearchModel)
                        || !String.IsNullOrEmpty(sSearchSerial)
                        || !String.IsNullOrEmpty(sSearchEquipmentCrossRef)
                    )
                )
            {
                dt = ws_Get_B1CustomerManagedPrintDevices(
                    sSearchCustomerNumber, 
                    sSearchCustomerLocation, 
                    sSearchLocationName, 
                    sSearchLocationCrossRef, 
                    sSearchCity, 
                    sSearchState, 
                    sSearchModel, 
                    sSearchSerial, 
                    sSearchEquipmentCrossRef
                    );

                // Merge
                //dt = Merge_DeviceTables(dtB1, dtB2);
            }

            rp_DeviceSmall.DataSource = dt;
            rp_DeviceSmall.DataBind();

            ViewState["vsDataTable_Dev"] = null;
            BindGrid_Dev(dt);
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
    protected DataTable Merge_DeviceTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("ContactType"));
        dt.Columns.Add(MakeColumn("ContactName"));
        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("Title"));
        dt.Columns.Add(MakeColumn("Phone"));
        dt.Columns.Add(MakeColumn("Extension"));
        dt.Columns.Add(MakeColumn("Email"));
        dt.Columns.Add(MakeColumn("Asset"));
        dt.Columns.Add(MakeColumn("Serial"));

        dt.Columns.Add(MakeColumn("PhoneDisplay"));

        DataRow dr;
        int iRowIdx = 0;
        string sPhone = "";
        //int iTemp = 0;


        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["CustomerNumber"] = row["CNCS1"].ToString().Trim();
            dt.Rows[iRowIdx]["CustomerLocation"] = row["CNCS2"].ToString().Trim();
            dt.Rows[iRowIdx]["ContactType"] = row["CNCOD"].ToString().Trim();
            dt.Rows[iRowIdx]["ContactName"] = Fix_Case(row["CNNAM"].ToString()).Trim();
            dt.Rows[iRowIdx]["Unit"] = row["CUNIT"].ToString().Trim();
            dt.Rows[iRowIdx]["Title"] = Fix_Case(row["CNTIT"].ToString()).Trim();
            sPhone = row["CNPHN"].ToString().Trim();
            sPhone = Clean_PhoneEntry(sPhone);
            if (sPhone.Length == 10) 
            {
                dt.Rows[iRowIdx]["Phone"] = sPhone;
                dt.Rows[iRowIdx]["PhoneDisplay"] = FormatPhone1(sPhone);
            }
            
            dt.Rows[iRowIdx]["Extension"] = row["CNEXT"].ToString().Trim();
            dt.Rows[iRowIdx]["Email"] = Fix_Case(row["CNEM1"].ToString()).Trim();
            dt.Rows[iRowIdx]["Asset"] = row["CXREF"].ToString().Trim();
            dt.Rows[iRowIdx]["Serial"] = row["CSERL"].ToString().Trim();

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
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
            }

            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Dev)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Dev(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Dev = "";

        if (ViewState["vsDataTable_Dev"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Dev"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching managed print devices were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Dev"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        if (gridSortDirection_Dev == SortDirection.Ascending)
        {
            sortExpression_Dev = gridSortExpression_Dev + " ASC";
        }
        else
        {
            sortExpression_Dev = gridSortExpression_Dev + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Dev;

        gv_DeviceLarge.DataSource = dt.DefaultView;
        gv_DeviceLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Dev(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_DeviceLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Dev(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Dev(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Dev == SortDirection.Ascending)
                gridSortDirection_Dev = SortDirection.Descending;
            else
                gridSortDirection_Dev = SortDirection.Ascending;
        }
        else
            gridSortDirection_Dev = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Dev = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Dev(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Dev
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Dev"] == null)
            {
                ViewState["GridSortDirection_Dev"] = SortDirection.Ascending;
                //ViewState["GridSortDirection"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Dev"];
        }
        set
        {
            ViewState["GridSortDirection_Dev"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Dev
    {
        get
        {
            if (ViewState["GridSortExpression_Dev"] == null)
            {
                ViewState["GridSortExpression_Dev"] = "LocationName"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Dev"];
        }
        set
        {
            ViewState["GridSortExpression_Dev"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Dev)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btDeviceSearchSubmit_Click(object sender, EventArgs e)
    {
        Load_DeviceDataTables();
    }
    // -------------------------------------------------------------------------------------------------
    protected void btDeviceSearchClear_Click(object sender, EventArgs e)
    {
        ddSearchCustomerFamily.SelectedValue = "";
        
        txSearchLocation.Text = "";
        txSearchLocationName.Text = "";
        txSearchLocationCrossRef.Text = "";
        txSearchCity.Text = "";
        txSearchState.Text = "";
        txSearchModel.Text = "";
        txSearchSerial.Text = "";

    }
    // -------------------------------------------------------------------------------------------------
    protected void lkDeviceSerial_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[1];
        saArg = sParms.Split('|');

        string sUnit = "";
        string sStartDate = DateTime.Now.AddMonths(-7).ToString("yyyyMMdd");
        string sEndDate = DateTime.Now.ToString("yyyyMMdd");
        string sDat = "";

        int iMaxMono = 0;
        int iMaxColor = 0;
        int iTemp = 0;

        DateTime datTemp;

        pnMono.Visible = false;
        pnColor.Visible = false;

        DataTable dt = new DataTable();

        if (saArg.Length > 0)
        {
            sUnit = saArg[0];
            dt = ws_Get_B1PageCountsForOneDevice(sUnit, sStartDate, sEndDate);

            if (dt.Rows.Count > 0) 
            {
                foreach (DataRow row in dt.Rows)
                {
                    sDat = row["FLUPDD"].ToString().Trim();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        sDat = datTemp.ToString("MMM yyyy");

                    if (hfChartMonoLabel.Value != "")
                        hfChartMonoLabel.Value += ",";
                    hfChartMonoLabel.Value += sDat;

                    if (!String.IsNullOrEmpty(hfChartMonoData.Value))
                        hfChartMonoData.Value += ",";
                    hfChartMonoData.Value += row["FLMONO"].ToString();

                    if (int.TryParse(row["FLMONO"].ToString(), out iTemp) == false)
                        iTemp = -1;
                    if (iTemp > iMaxMono)
                        iMaxMono = iTemp;

                    sDat = row["FLUPDD"].ToString().Trim();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        sDat = datTemp.ToString("MMM yyyy");

                    if (hfChartColorLabel.Value != "")
                        hfChartColorLabel.Value += ",";
                    hfChartColorLabel.Value += sDat;

                    if (!String.IsNullOrEmpty(hfChartColorData.Value))
                        hfChartColorData.Value += ",";
                    hfChartColorData.Value += row["FLCOLR"].ToString();

                    if (int.TryParse(row["FLCOLR"].ToString(), out iTemp) == false)
                        iTemp = -1;
                    if (iTemp > iMaxColor)
                        iMaxColor = iTemp;
                }

                if (iMaxMono > 15)
                    iTemp = iMaxMono / 4;
                else
                    iTemp = 5;
                hfChartMonoIncrement.Value = iTemp.ToString();

                if (iMaxColor > 15)
                    iTemp = iMaxColor / 4;
                else
                    iTemp = 5;
                hfChartColorIncrement.Value = iTemp.ToString();

                if (iMaxMono > 0)
                    pnMono.Visible = true;
                if (iMaxColor > 0)
                    pnColor.Visible = true;

            }

            pnDevice.Visible = false;
            pnPages.Visible = true;
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
