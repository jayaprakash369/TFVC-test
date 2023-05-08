using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Security;
using System.Text.RegularExpressions;

public partial class private_sc_ServiceRequest : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd; 
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    Obj_ServiceRequest serviceRequest;

    string sTemp = "";
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";
        lbProblemContactError.Text = "";

        if (!IsPostBack)
        {
            Hide_Panels();
            Get_UserPrimaryCustomerNumber();

            if (User.Identity.IsAuthenticated)
            {
                hfEmailUserName.Value = User.Identity.Name.ToString();
            }
                string sPassedUnt = "";
                string sPassedAgr = "";
                string sPassedSrc = "";
                int iCustomerNumber = 0;
                int iCustomerLocation = 0;

                if (Request.QueryString["cs1"] != null && Request.QueryString["cs1"].ToString() != "") 
                {
                    if (int.TryParse(Request.QueryString["cs1"].ToString().Trim(), out iCustomerNumber) == false)
                        iCustomerNumber = -1;
                    if (iCustomerNumber > 0) 
                    {
                        hfChosenCs1.Value = iCustomerNumber.ToString();
                    }
                }

                if (Request.QueryString["cs2"] != null && Request.QueryString["cs2"].ToString() != "")
                {
                    if (int.TryParse(Request.QueryString["cs2"].ToString().Trim(), out iCustomerLocation) == false)
                        iCustomerLocation = -1;
                    if (iCustomerLocation > -1)
                        hfChosenCs2.Value = iCustomerLocation.ToString();
                }
                
                if (Request.QueryString["unt"] != null && Request.QueryString["unt"].ToString() != "")
                    sPassedUnt = Request.QueryString["unt"].ToString().Trim();
                if (Request.QueryString["agr"] != null && Request.QueryString["agr"].ToString() != "")
                    sPassedAgr = Request.QueryString["agr"].ToString().Trim();
                if (Request.QueryString["src"] != null && Request.QueryString["src"].ToString() != "")
                    sPassedSrc = Request.QueryString["src"].ToString().Trim();
                if (Request.QueryString["pag"] != null && Request.QueryString["pag"].ToString() != "")
                    hfRequestSourcePage.Value = Request.QueryString["pag"].ToString().Trim();

                if (!String.IsNullOrEmpty(sPassedUnt)
                    && !String.IsNullOrEmpty(sPassedAgr)
                    && !String.IsNullOrEmpty(sPassedSrc)
                    )
                {
                    // Request unit came in from AgreementEquipment so jump straight to problem
                    hfUnitList.Value = sPassedUnt + "~" + sPassedAgr + "~" + sPassedSrc;

                    if (serviceRequest == null)
                        serviceRequest = new Obj_ServiceRequest();

                    if (iCustomerNumber > 0 && iCustomerLocation > -1)
                        Load_SelectedLocationLabel(iCustomerNumber, iCustomerLocation);

                    pnSelected.Visible = true;
                    pnSelectedLocation.Visible = true;  // Loaded right here
                    pnSelectedContact.Visible = false; // need to load this on the next problem page
                    pnSelectedContactEntry.Visible = true;
                    pnSelectedEquipment.Visible = true; // This will be loaded on the problem page as you parse the hfUnitList.Value

                    pnProblem.Visible = true;
                    Load_ProblemPanel();

                }
                else 
                {
                    pnLocation.Visible = true;

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
                    }

                    Load_LocationDataTables();
                }
               

        }

        Move_RequestHiddenFieldsToObject();

    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ==========================================================
    protected int Insert_BL2ServiceRequestLog(
        string companyId, 
        string siteId, 
        string companyName, 
        string model, 
        string modelCrossRef, 
        string problemSummary, 
        string problemDetail, 
        string loginEmail,
        string contactName,
        string contactPhone,
        string contactExtension,
        string acknowledgementEmail
        )
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        int iCompanyId = 0;
        int iSiteId = 0;

        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        if (int.TryParse(companyId, out iCompanyId) == false)
            iCompanyId = 0;
        if (int.TryParse(siteId, out iSiteId) == false)
            iSiteId = 0;

        try
        {
            sqlConn.Open();

            sSql = "Insert into " + sSqlDbToUse_Customer + ".bl2_ServiceRequestLog" +
                 " (sr_OracleCompanyId" +
                 ", sr_OracleSiteId" +
                 ", sr_CompanyName" +
                 ", sr_Model" +
                 ", sr_ModelCrossRef" +
                 ", sr_ProblemSummary" +
                 ", sr_ProblemDetail" +
                 ", sr_LoginEmail" +
                 ", sr_ContactName" +
                 ", sr_ContactPhone" +
                 ", sr_ContactExtension" +
                 ", sr_AcknowledgementEmail" +
                 ", sr_CreationTimestamp)" +
                " values(@CompanyId" + 
                ", @SiteId" + 
                ", @CompanyName" + 
                ", @Model" + 
                ", @ModelCrossRef" + 
                ", @ProblemSummary" + 
                ", @ProblemDetail" + 
                ", @LoginEmail" + 
                ", @ContactName" + 
                ", @ContactPhone" + 
                ", @ContactExtension" + 
                ", @AcknowledgementEmail" + 
                ", @CreationTimestamp)";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@CompanyId", iCompanyId);
            sqlCmd.Parameters.AddWithValue("@SiteId", iSiteId);
            sqlCmd.Parameters.AddWithValue("@CompanyName", companyName);
            sqlCmd.Parameters.AddWithValue("@Model", model);
            sqlCmd.Parameters.AddWithValue("@ModelCrossRef", modelCrossRef);
            sqlCmd.Parameters.AddWithValue("@ProblemSummary", problemSummary);
            sqlCmd.Parameters.AddWithValue("@ProblemDetail", problemDetail);
            sqlCmd.Parameters.AddWithValue("@LoginEmail", loginEmail);
            sqlCmd.Parameters.AddWithValue("@ContactName", contactName);
            sqlCmd.Parameters.AddWithValue("@ContactPhone", contactPhone);
            sqlCmd.Parameters.AddWithValue("@ContactExtension", contactExtension);
            sqlCmd.Parameters.AddWithValue("@AcknowledgementEmail", acknowledgementEmail);
            sqlCmd.Parameters.AddWithValue("@CreationTimestamp", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ========================================================================
    // WS: STRINGS (start)
    // ========================================================================
    protected string ws_Get_B1UnitCustomerToSubcontractCallIfXerox_YN(
        string unit
        )
    {
        string sSubcontractThisCall = "";

        if (!String.IsNullOrEmpty(unit))
        {
            int iUnit = 0;
            if (int.TryParse(unit, out iUnit) == false)
                iUnit = -1;

            if (iUnit > 0)
            {
                string sJobName = "Get_B1UnitCustomerToSubcontractCallIfXerox_YN";
                string sFieldList = "unit|x";
                string sValueList =
                    unit + "|" +
                    "x";

                sSubcontractThisCall = Call_WebService_ForString(sJobName, sFieldList, sValueList);
            }
        } 
        return sSubcontractThisCall;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1PartPriority(
        string part
        )
    {
        string sPartPriority = "";

        if (!String.IsNullOrEmpty(part))
        {
            if (part.Length > 15)
                part = part.Substring(0, 15);

            string sJobName = "Get_B1PartPriority";
            string sFieldList = "part|x";
            string sValueList =
                part + "|" +
                "x";

            sPartPriority = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sPartPriority;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1PartProductCode(
        string part
        )
    {
        string sB1PartProductCode = "";

        if (!String.IsNullOrEmpty(part))
        {
            if (part.Length > 15)
                part = part.Substring(0, 15);

            string sJobName = "Get_B1PartProductCode";
            string sFieldList = "part|x";
            string sValueList =
                part + "|" +
                "x";

            sB1PartProductCode = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sB1PartProductCode;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CustomerOracleIds(int customerNumber, int customerLocation)
    {
        string sOracleIds = "";

        if (customerNumber > 0 && customerLocation > -1)
        {
            string sJobName = "Get_B1CustomerOracleIds";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber.ToString() + "|" + customerLocation.ToString() + "|x";

            sOracleIds = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sOracleIds;
    
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_AllowRequestEntryAsPM_YN(int customerNumber)
    {
        string sAllowRequestEntryAsPM_YN = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustPref_AllowRequestEntryAsPM_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sAllowRequestEntryAsPM_YN = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAllowRequestEntryAsPM_YN;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_AutoLoadRequestContactAndPhone_YN(int customerNumber)
    {
        string sAutoLoadRequestContactAndPhone_YN = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustPref_AutoLoadRequestContactAndPhone_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sAutoLoadRequestContactAndPhone_YN = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAutoLoadRequestContactAndPhone_YN;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1ServrightCustomer_ServiceType(int customerNumber, int customerLocation) // OA or SIG (or blank)
    {
        string sB1ServrightCustomer_ServiceType = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1ServrightCustomer_ServiceType";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber.ToString() + "|" + customerLocation.ToString() + "|x";

            sB1ServrightCustomer_ServiceType = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sB1ServrightCustomer_ServiceType;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Upd_B1RequestDuration(int requestWorkfileKey, double duration)
    {
        string sResponse = "";

        if (requestWorkfileKey > 0)
        {
            string sJobName = "Upd_B1RequestDuration";
            string sFieldList = "requestWorkfileKey|duration|x";
            string sValueList = requestWorkfileKey.ToString() + "|" + duration.ToString("00000.00"); // BNUM2 = 9/2

            sResponse = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResponse;
    }
    // ========================================================================
    // WS: STRINGS (end)
    // ========================================================================
    // ========================================================================
    // WS: DATA TABLES (start)
    // ========================================================================
    protected DataTable ws_Get_B1LocationEquipment(
        string customerNumber,
        string customerLocation,
        string productCode,
        string model,
        string serial,
        string modelDescription,
        string asset,
        string agentId,
        string excelVersion
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1LocationEquipment";
            string sFieldList = "customerNumber|customerLocation|productCode|model|serial|modelDescription|asset|agentId|excelVersion|x";
            string sValueList =
                customerNumber + "|" +
                customerLocation + "|" +
                productCode + "|" +
                model + "|" +
                serial + "|" +
                modelDescription + "|" +
                asset + "|" +
                agentId + "|" +
                excelVersion + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1PartProductCodes(
        string customerNumber,
        string customerLocation
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1PartProductCodes";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList =
                customerNumber + "|" +
                customerLocation + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1PrimaryCustomerLocations(
        int primaryCustomer
        )
    {
        DataTable dt = new DataTable("");

        if (primaryCustomer > 0)
        {
            string sJobName = "Get_B1PrimaryCustomerLocations";
            string sFieldList = "primaryCustomer|x";
            string sValueList =
                primaryCustomer + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }

    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2LocationEquipment(
        string oracleParentId,
        string oracleChildId,
        //string productCode,
        string model,
        string serial,
        string modelDescription
        //string asset,
        //string agentId,
        //string downloadExcelY
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(oracleParentId) && !String.IsNullOrEmpty(oracleChildId))
        {
            string sJobName = "Get_B2LocationEquipment";
            //string sFieldList = "agreementNumber|productCode|model|serial|modelDescription|asset|agentId|downloadExcelY|x";
            string sFieldList = "oracleParentId|oracleChildId|model|serial|modelDescription|x";
            string sValueList =
                oracleParentId + "|" +
                oracleChildId + "|" +
                //productCode + "|" +
                model + "|" +
                serial + "|" +
                modelDescription + "|" +
                //asset + "|" +
                //agentId + "|" +
                //downloadExcelY + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1UnitBySerialOrAsset(
        int primaryCustomer,
        string serialOrAsset
        )
    {
        DataTable dt = new DataTable("");

        if (primaryCustomer > 0 && !String.IsNullOrEmpty(serialOrAsset))
        {
            string sJobName = "Get_B1UnitBySerialOrAsset";
            string sFieldList = "primaryCustomer|serialOrAsset|x";
            string sValueList =
                primaryCustomer.ToString() + "|" +
                serialOrAsset + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1EmailAddressesByCodeForCustomers(
        string code,
        string customerNumber,
        string customerLocation
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1EmailAddressesByCodeForCustomers";
            string sFieldList = "code|customerNumber|customerLocation|x";
            string sValueList =
                code + "|" +
                customerNumber + "|" +
                customerLocation + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1EmailAddressesByCodeForEmployees(
        string code,
        string employeeNumber // optional almost all the time
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(code))
        {
            string sJobName = "Get_B1EmailAddressesByCodeForEmployees";
            string sFieldList = "code|employeeNumber|x";
            string sValueList =
                code + "|" +
                employeeNumber + "|" +
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

    // =========================================================
    // START EQUIPMENT GRID
    // =========================================================
    protected void BindGrid_Eqp()
    {
        DataTable dtB1 = new DataTable("B1");
        DataTable dtB2 = new DataTable("B2");
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Eqp"] == null)
        {
            string customerNumber = "";
            string customerLocation = "";
            string productCode = "";
            string model = "";
            string modelDescription = "";
            string serial = "";
            string asset = "";
            string agentId = "";
            string excelVersion = "";

            int iCs1 = 0;
            int iCs2 = 0;
            int.TryParse(hfChosenCs1.Value, out iCs1);
            int.TryParse(hfChosenCs2.Value, out iCs2);

            if (iCs1 > 0 && iCs2 > -1)
            {
                string sOr1Or2 = ws_Get_B1CustomerOracleIds(iCs1, iCs2);
                string[] saOr1Or2 = sOr1Or2.Split('|');
                if (saOr1Or2.Length > 1)
                {
                    hfOracleParentId.Value = saOr1Or2[0];
                    hfOracleChildId.Value = saOr1Or2[1];
                }
            }


            if (iCs1 > 0)
                customerNumber = iCs1.ToString();

            if (iCs2 > -1)
                customerLocation = iCs2.ToString();

            productCode = ddSearchEquipmentCategory.SelectedValue.ToString();
            model = txSearchEquipmentModel.Text.Trim();
            modelDescription = txSearchEquipmentModelDescription.Text.Trim();
            serial = txSearchEquipmentSerial.Text.Trim();
            asset = txSearchEquipmentEquipmentXref.Text.Trim();
            agentId = txSearchEquipmentAgentId.Text.Trim();
            excelVersion = "N";

            //dt = ws_Get_B1Equipment(customerNumber, customerLocation, productCode, model, modelDescription, serial, asset, agentId, excelVersion);
            dtB1 = ws_Get_B1LocationEquipment(customerNumber, customerLocation, productCode, model, serial, modelDescription, asset, agentId, excelVersion);

            if (!String.IsNullOrEmpty(hfOracleParentId.Value) && !String.IsNullOrEmpty(hfOracleChildId.Value))
            {
                dtB2 = ws_Get_B2LocationEquipment(
                    hfOracleParentId.Value,
                    hfOracleChildId.Value,
                    //sProductCode,
                    model,
                    serial,
                    modelDescription
                    //sAsset,
                    //sAgentId,
                    //isExcelVersionNeeded
                    );
            }

            // Do you want to exclude BL2 locations from BL1? (But you have combo...)
            dt = Merge_EquipmentTablesForLocations(dtB1, dtB2);

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dt;
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Eqp"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Eqp;
        if (gridSortDirection_Eqp == SortDirection.Ascending)
        {
            sortExpression_Eqp = gridSortExpression_Eqp + " ASC";
        }
        else
        {
            sortExpression_Eqp = gridSortExpression_Eqp + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Eqp;

        gv_EquipmentLarge.DataSource = dt.DefaultView;
        //gv_EquipmentLarge.PageSize = 500;
        gv_EquipmentLarge.DataBind();

        rpEquipment_Small.DataSource = dt.DefaultView;
        rpEquipment_Small.DataBind();

    }
    // -----------------------------------------------------------------
    protected void gvPageIndexChanging_Eqp(object sender, GridViewPageEventArgs e)
    {
        GetSelectedEquipmentRecords_gv();
        int newPageIndex = e.NewPageIndex;
        gv_EquipmentLarge.PageIndex = newPageIndex;
        BindGrid_Eqp();
        SetSelectedEquipmentRecords_gv();
    }
    // -----------------------------------------------------------------
    protected void gvSorting_Eqp(object sender, GridViewSortEventArgs e)
    {
        GetSelectedEquipmentRecords_gv();

        // Retrieve the name of the clicked column
        string sortExpression_Eqp = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Eqp == e.SortExpression)
        {
            if (gridSortDirection_Eqp == SortDirection.Ascending)
                gridSortDirection_Eqp = SortDirection.Descending;
            else
                gridSortDirection_Eqp = SortDirection.Ascending;
        }
        else
            gridSortDirection_Eqp = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Eqp = sortExpression_Eqp;
        // Rebind the grid to its data source
        BindGrid_Eqp();
        SetSelectedEquipmentRecords_gv();
    }
    // -----------------------------------------------------------------
    private SortDirection gridSortDirection_Eqp
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Eqp"] == null)
            {
                ViewState["GridSortDirection_Eqp"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Eqp"];
        }
        set
        {
            ViewState["GridSortDirection_Eqp"] = value;
        }
    }
    // -----------------------------------------------------------------
    private string gridSortExpression_Eqp
    {
        get
        {
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Eqp"] == null)
            {
                ViewState["GridSortExpression_Eqp"] = "Model"; // CUSTNM? Initial field to sort by
            }
            return (string)ViewState["GridSortExpression_Eqp"];
        }
        set
        {
            ViewState["GridSortExpression_Eqp"] = value;
        }
    }
    // =========================================================
    // END EQUIPMENT GRID 
    // =========================================================

    // =========================================================
    // START SERIAL GRID
    // =========================================================
    protected void BindGrid_Ser(DataTable dt)
    {
        //DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Ser"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Ser"] = dt;
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Ser"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Ser;
        if (gridSortDirection_Ser == SortDirection.Ascending)
        {
            sortExpression_Ser = gridSortExpression_Ser + " ASC";
        }
        else
        {
            sortExpression_Ser = gridSortExpression_Ser + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Ser;

        gv_SerialLarge.DataSource = dt.DefaultView;
        //gv_SerialLarge.PageSize = 500;
        gv_SerialLarge.DataBind();

        rp_SerialSmall.DataSource = dt.DefaultView;
        rp_SerialSmall.DataBind();

    }
    // -----------------------------------------------------------------
    protected void gvPageIndexChanging_Ser(object sender, GridViewPageEventArgs e)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        // Have to save and reset the checkmarks
        GetSelectedSerialRecords_gv();
        int newPageIndex = e.NewPageIndex;
        gv_SerialLarge.PageIndex = newPageIndex;

        BindGrid_Ser(dt);
        SetSelectedSerialRecords_gv();
    }
    // -----------------------------------------------------------------
    protected void gvSorting_Ser(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        GetSelectedSerialRecords_gv();

        // Retrieve the name of the clicked column
        string sortExpression_Ser = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Ser == e.SortExpression)
        {
            if (gridSortDirection_Ser == SortDirection.Ascending)
                gridSortDirection_Ser = SortDirection.Descending;
            else
                gridSortDirection_Ser = SortDirection.Ascending;
        }
        else
            gridSortDirection_Ser = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Ser = sortExpression_Ser;
        // Rebind the grid to its data source
        BindGrid_Ser(dt);
        SetSelectedSerialRecords_gv();
    }
    // -----------------------------------------------------------------
    private SortDirection gridSortDirection_Ser
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Eqp"] == null)
            {
                ViewState["GridSortDirection_Eqp"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Eqp"];
        }
        set
        {
            ViewState["GridSortDirection_Eqp"] = value;
        }
    }
    // --------------------------------------------------------------
    private string gridSortExpression_Ser
    {
        get
        {
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Ser"] == null)
            {
                ViewState["GridSortExpression_Ser"] = "CustName"; // CUSTNM? Initial field to sort by
            }
            return (string)ViewState["GridSortExpression_Ser"];
        }
        set
        {
            ViewState["GridSortExpression_Ser"] = value;
        }
    }
    // =========================================================
    // END EQUIPMENT GRID 
    // =========================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {   }
    // ----------------------------------------------------------------------------------------------------
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

        if (iSelectedCustomerNumber > 0)
            sCustomerNumber = iSelectedCustomerNumber.ToString();
        else 
            sCustomerNumber = hfPrimaryCs1.Value;

        string sCustomerName = txSearchLocationName.Text.Trim().ToUpper().Trim();
        //string sCustomerNumber =  txSearchNumber.Text.Trim();
        string sCustomerLocation = txSearchLocationNum.Text.Trim();
        string sAddress = txSearchLocationAddress.Text.Trim().ToUpper().Trim();
        string sCity = txSearchLocationCity.Text.Trim().ToUpper().Trim();
        string sState = txSearchLocationState.Text.Trim().ToUpper().Trim();
        string sZip = txSearchLocationZip.Text.Trim().ToUpper().Trim();
        string sPhone = txSearchLocationPhone.Text.Trim().ToUpper().Trim();
        string sXref = txSearchLocationXref.Text.Trim().ToUpper().Trim();

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
                    "",
                    sAddress,
                    sCity,
                    sState,
                    sZip,
                    sPhone,
                    sXref);


                // Merge
                dt = Merge_LocationTables(dtB1, dtB2);

                rp_LocationSmall.DataSource = dt;
                rp_LocationSmall.DataBind();

                ViewState["vsDataTable_Loc"] = null;
                BindGrid_Loc(dt);

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
        dt.Columns.Add(MakeColumn("PhoneFormatted"));
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
            dt.Rows[iRowIdx]["PhoneFormatted"] = row["PhoneFormatted"].ToString().Trim();
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
    protected DataTable Merge_RequestTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("Agreement"));
        dt.Columns.Add(MakeColumn("AgreementDescription"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("AgreementCode"));
        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("ProductCode"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        int iRowIdx = 0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["Unit"] = row["Unit"].ToString().Trim();
            dt.Rows[iRowIdx]["Agreement"] = row["Agreement"].ToString().Trim();
            dt.Rows[iRowIdx]["AgreementDescription"] = row["AgreementDescription"].ToString().Trim().ToUpper();
            dt.Rows[iRowIdx]["Model"] = row["Model"].ToString().Trim().ToUpper();
            dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim().ToUpper();
            dt.Rows[iRowIdx]["AgreementCode"] = row["AgreementCode"].ToString().Trim();
            dt.Rows[iRowIdx]["CustomerNumber"] = row["CustomerNumber"].ToString().Trim();
            dt.Rows[iRowIdx]["CustomerLocation"] = row["CustomerLocation"].ToString().Trim();
            dt.Rows[iRowIdx]["ProductCode"] = row["ProductCode"].ToString().Trim();
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString()).Trim();

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["Unit"] = row["Unit"].ToString().Trim();
            dt.Rows[iRowIdx]["Agreement"] = row["Agreement"].ToString().Trim();
            dt.Rows[iRowIdx]["AgreementDescription"] = row["AgreementDescription"].ToString().Trim().ToUpper();
            dt.Rows[iRowIdx]["Model"] = row["Model"].ToString().Trim();
            dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim();
            dt.Rows[iRowIdx]["AgreementCode"] = Fix_Case(row["AgreementCode"].ToString()).Trim();
            dt.Rows[iRowIdx]["CustomerNumber"] = row["CustomerNumber"].ToString().Trim();
            dt.Rows[iRowIdx]["CustomerLocation"] = row["CustomerLocation"].ToString().Trim();
            dt.Rows[iRowIdx]["ProductCode"] = Fix_Case(row["ProductCode"].ToString()).Trim();
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString()).Trim();

            dt.Rows[iRowIdx]["Source"] = "2";

            iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Merge_SerialTables(DataTable dt) // Just formatting at this point...
    {
        foreach (DataRow row in dt.Rows)
        {
            row["Model"] = row["Model"].ToString().Trim().ToUpper();
            row["Serial"] = row["Serial"].ToString().Trim().ToUpper();
            row["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString()).Trim();
            row["AgreementDescription"] = row["AgreementDescription"].ToString().Trim().ToUpper();
            row["Asset"] = Fix_Case(row["Asset"].ToString()).Trim();
            row["CustName"] = Fix_Case(row["CustName"].ToString()).Trim();
            row["City"] = Fix_Case(row["City"].ToString()).Trim();
            // Leave the state abbreviation alone...
            //row["State"] = Fix_Case(row["State"].ToString()).Trim();
        }

        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void GetSelectedEquipmentRecords_gv()
    {
        CheckBox chBxTemp = new CheckBox();
        string sType = "";
        hfUnitList.Value = "";

        foreach (Control c1 in gv_EquipmentLarge.Controls)
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
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chBxTemp = (CheckBox)c4;
                                        if (chBxTemp.Checked == true)
                                        {
                                            if (hfUnitList.Value != "")
                                                hfUnitList.Value += "|";
                                            hfUnitList.Value += chBxTemp.Text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void SetSelectedEquipmentRecords_gv()
    {
        CheckBox chBxTemp = new CheckBox();
        string sType = "";

        foreach (Control c1 in gv_EquipmentLarge.Controls)
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
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chBxTemp = (CheckBox)c4;
                                        if (hfMoveList.Value.Contains(chBxTemp.Text.Trim()))
                                            chBxTemp.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void GetSelectedSerialRecords_gv()
    {
        CheckBox chBxTemp = new CheckBox();
        string sType = "";
        hfMoveList.Value = "";

        foreach (Control c1 in gv_SerialLarge.Controls)
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
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chBxTemp = (CheckBox)c4;
                                        if (chBxTemp.Checked == true)
                                        {
                                            if (hfMoveList.Value != "")
                                                hfMoveList.Value += "|";
                                            hfMoveList.Value += chBxTemp.Text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void SetSelectedSerialRecords_gv()
    {
        CheckBox chBxTemp = new CheckBox();
        string sType = "";

        foreach (Control c1 in gv_SerialLarge.Controls)
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
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chBxTemp = (CheckBox)c4;
                                        if (hfMoveList.Value.Contains(chBxTemp.Text.Trim()))
                                            chBxTemp.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Move_RequestObjectToHiddenFields() 
    {

        // FieldDelimeter = |
        // Equipment item delimiter = ~

        string sHeader = "";
        string sEquipmentList = "";

        if (serviceRequest != null) 
        {
            if (serviceRequest.address1 != null) 
            {
                serviceRequest.address1 = scrubDelim(serviceRequest.address1);
                sHeader += serviceRequest.address1 + "|";
            }
            else
                sHeader += "|";
            
            if (serviceRequest.address2 != null) {
                serviceRequest.address2 = scrubDelim(serviceRequest.address2);
                sHeader += serviceRequest.address2 + "|";
            }
            else
                sHeader += "|";
            
            if (serviceRequest.address3 != null) 
            {
                serviceRequest.address3 = scrubDelim(serviceRequest.address3);
                sHeader += serviceRequest.address3 + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.city != null) 
            {
                serviceRequest.city = scrubDelim(serviceRequest.city);
                sHeader += serviceRequest.city + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.commentForAllRequestItems != null) 
            {
                serviceRequest.commentForAllRequestItems = scrubDelim(serviceRequest.commentForAllRequestItems);
                sHeader += serviceRequest.commentForAllRequestItems + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.companyPhone != null) {
                serviceRequest.companyPhone = scrubDelim(serviceRequest.companyPhone);
                sHeader += serviceRequest.companyPhone + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.companyPhoneExtension != null) {
                serviceRequest.companyPhoneExtension = scrubDelim(serviceRequest.companyPhoneExtension);
                sHeader += serviceRequest.companyPhoneExtension + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.contactName != null) {
                serviceRequest.contactName = scrubDelim(serviceRequest.contactName);
                sHeader += serviceRequest.contactName + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.creatorUsername != null) {
                serviceRequest.creatorUsername = scrubDelim(serviceRequest.creatorUsername);
                sHeader += serviceRequest.creatorUsername + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.customerLocation != null) {
                serviceRequest.customerLocation = scrubDelim(serviceRequest.customerLocation);
                sHeader += serviceRequest.customerLocation + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.customerName != null) {
                serviceRequest.customerName = scrubDelim(serviceRequest.customerName);
                sHeader += serviceRequest.customerName + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.customerNumber != null) {
                serviceRequest.customerNumber = scrubDelim(serviceRequest.customerNumber);
                sHeader += serviceRequest.customerNumber + "|";
            }
            else
                sHeader += "|";


            if (serviceRequest.dealerNumber != null) {
                serviceRequest.dealerNumber = scrubDelim(serviceRequest.dealerNumber);
                sHeader += serviceRequest.dealerNumber + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.defaultContactName != null)
            {
                serviceRequest.defaultContactName = scrubDelim(serviceRequest.defaultContactName);
                sHeader += serviceRequest.defaultContactName + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.emailForAcknowledgement != null)
            {
                serviceRequest.emailForAcknowledgement = scrubDelim(serviceRequest.emailForAcknowledgement);
                sHeader += serviceRequest.emailForAcknowledgement + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.keyForWorkfiles != null) {
                serviceRequest.keyForWorkfiles = scrubDelim(serviceRequest.keyForWorkfiles);
                sHeader += serviceRequest.keyForWorkfiles + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.equipmentQtyOnRequest != null) {
                serviceRequest.equipmentQtyOnRequest = scrubDelim(serviceRequest.equipmentQtyOnRequest);
                sHeader += serviceRequest.equipmentQtyOnRequest + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.payingByAgrOrTM != null) {
                serviceRequest.payingByAgrOrTM = scrubDelim(serviceRequest.payingByAgrOrTM);
                sHeader += serviceRequest.payingByAgrOrTM + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.preferredTechContactDetail != null) {
                serviceRequest.preferredTechContactDetail = scrubDelim(serviceRequest.preferredTechContactDetail);
                sHeader += serviceRequest.preferredTechContactDetail + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.preferredTechContactMethod != null) {
                serviceRequest.preferredTechContactMethod = scrubDelim(serviceRequest.preferredTechContactMethod);
                sHeader += serviceRequest.preferredTechContactMethod + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.state != null) {
                serviceRequest.state = scrubDelim(serviceRequest.state);
                sHeader += serviceRequest.state + "|";
            }
            else
                sHeader += "|";

            if (serviceRequest.zip != null) {
                serviceRequest.zip = scrubDelim(serviceRequest.zip);
                sHeader += serviceRequest.zip + "|";
            }
            else
                sHeader += "|";

            // ---------------------------------------------------
            if (serviceRequest.equipmentList != null) 
            {
                foreach (Obj_ServiceRequest.Equipment equipment in serviceRequest.equipmentList) 
                {
                    if (equipment.agreementCode != null) {
                        equipment.agreementCode = scrubDelim(equipment.agreementCode);
                        sEquipmentList += equipment.agreementCode + "|";
                    }
                    
                    else
                        sEquipmentList += "|";

                    if (equipment.agreementDescription != null) {
                        equipment.agreementDescription = scrubDelim(equipment.agreementDescription);
                        sEquipmentList += equipment.agreementDescription + "|";
                    }
                    
                    else
                        sEquipmentList += "|";

                    if (equipment.agreementNumber != null) {
                        equipment.agreementNumber = scrubDelim(equipment.agreementNumber);
                        sEquipmentList += equipment.agreementNumber + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.alternateRequestType != null) {
                        equipment.alternateRequestType = scrubDelim(equipment.alternateRequestType);
                        sEquipmentList += equipment.alternateRequestType + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.assignToSpecificTech != null)
                    {
                        equipment.assignToSpecificTech = scrubDelim(equipment.assignToSpecificTech);
                        sEquipmentList += equipment.assignToSpecificTech + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.callPaymentCode != null) {
                        equipment.callPaymentCode = scrubDelim(equipment.callPaymentCode);
                        sEquipmentList += equipment.callPaymentCode + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.center != null)
                    {
                        equipment.center = scrubDelim(equipment.center);
                        sEquipmentList += equipment.center + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.customerCallAlias != null) {
                        equipment.customerCallAlias = scrubDelim(equipment.customerCallAlias);
                        sEquipmentList += equipment.customerCallAlias + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.flexField != null)
                    {
                        equipment.flexField = scrubDelim(equipment.flexField);
                        sEquipmentList += equipment.flexField + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.itemAutoSubmitted != null) {
                        equipment.itemAutoSubmitted = scrubDelim(equipment.itemAutoSubmitted);
                        sEquipmentList += equipment.itemAutoSubmitted + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.itemOnContract != null) {
                        equipment.itemOnContract = scrubDelim(equipment.itemOnContract);
                        sEquipmentList += equipment.itemOnContract + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.itemSequence != null)
                    {
                        equipment.itemSequence = scrubDelim(equipment.itemSequence);
                        sEquipmentList += equipment.itemSequence + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.model != null) {
                        equipment.model = scrubDelim(equipment.model);
                        sEquipmentList += equipment.model + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.printerInterfaceType != null) {
                        equipment.printerInterfaceType = scrubDelim(equipment.printerInterfaceType);
                        sEquipmentList += equipment.printerInterfaceType + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.priority != null)
                    {
                        equipment.priority = scrubDelim(equipment.priority);
                        sEquipmentList += equipment.priority + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.problemSubtype != null)
                    {
                        equipment.problemSubtype = scrubDelim(equipment.problemSubtype);
                        sEquipmentList += equipment.problemSubtype + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.problemSummary != null) {
                        equipment.problemSummary = scrubDelim(equipment.problemSummary);
                        sEquipmentList += equipment.problemSummary + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.purchaseOrder != null)
                    {
                        equipment.purchaseOrder = scrubDelim(equipment.purchaseOrder);
                        sEquipmentList += equipment.purchaseOrder + "|";
                    }
                    else
                        sEquipmentList += "|";

                    if (equipment.serial != null) {
                        equipment.serial = scrubDelim(equipment.serial);
                        sEquipmentList += equipment.serial + "~";
                    }
                    else
                        sEquipmentList += "~";

                    if (equipment.shipVia != null)
                    {
                        equipment.shipVia = scrubDelim(equipment.shipVia);
                        sEquipmentList += equipment.shipVia + "~";
                    }
                    else
                        sEquipmentList += "~";

                    if (equipment.source != null)
                    {
                        equipment.source = scrubDelim(equipment.source);
                        sEquipmentList += equipment.source + "~";
                    }
                    else
                        sEquipmentList += "~";

                    if (equipment.ticket != null)
                    {
                        equipment.ticket = scrubDelim(equipment.ticket);
                        sEquipmentList += equipment.ticket + "~";
                    }
                    else
                        sEquipmentList += "~";

                    if (equipment.unit != null)
                    {
                        equipment.unit = scrubDelim(equipment.unit);
                        sEquipmentList += equipment.unit + "~";
                    }
                    else
                        sEquipmentList += "~";

                }
            }
            // ----------------------------------------------
        }

        // Save the object values to the hidden field that will survive a postback
        hfRequestHeader.Value = sHeader;
        hfRequestEquipmentList.Value = sEquipmentList;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Move_RequestHiddenFieldsToObject()
    {
        string[] saHead = hfRequestHeader.Value.Split('|');
        string[] saMachList = hfRequestEquipmentList.Value.Split('~');
        string[] saMach = { "" };
        int i = 0;

        if (serviceRequest == null)
            serviceRequest = new Obj_ServiceRequest();

        Obj_ServiceRequest.Equipment equipment;

        if (saHead.Length > 21)
        {
            serviceRequest.address1 = saHead[0];
            serviceRequest.address2 = saHead[1];
            serviceRequest.address3 = saHead[2];
            serviceRequest.city = saHead[3];
            serviceRequest.commentForAllRequestItems = saHead[4];
            serviceRequest.companyPhone = saHead[5];
            serviceRequest.companyPhoneExtension = saHead[6];
            serviceRequest.contactName = saHead[7];
            serviceRequest.creatorUsername = saHead[8];
            serviceRequest.customerLocation = saHead[9];
            serviceRequest.customerName = saHead[10];
            serviceRequest.customerNumber = saHead[11];
            serviceRequest.dealerNumber = saHead[12];
            serviceRequest.defaultContactName = saHead[13];
            serviceRequest.emailForAcknowledgement = saHead[14];
            serviceRequest.keyForWorkfiles = saHead[15];
            serviceRequest.equipmentQtyOnRequest = saHead[16];
            serviceRequest.payingByAgrOrTM = saHead[17];
            serviceRequest.preferredTechContactDetail = saHead[18];
            serviceRequest.preferredTechContactMethod = saHead[19];
            serviceRequest.state = saHead[20];
            serviceRequest.zip = saHead[21];

            for (i = 0; i < saMachList.Length; i++)
            {
                equipment = new Obj_ServiceRequest.Equipment();

                saMach = saMachList[i].Split('|');
                if (saMach.Length > 22)
                {
                    equipment.agreementCode = saMach[0];
                    equipment.agreementDescription = saMach[1];
                    equipment.agreementNumber = saMach[2];
                    equipment.alternateRequestType = saMach[3];
                    equipment.assignToSpecificTech = saMach[4];
                    equipment.callPaymentCode = saMach[5];
                    equipment.center = saMach[6];
                    equipment.customerCallAlias = saMach[7];
                    equipment.flexField = saMach[8];
                    equipment.itemAutoSubmitted = saMach[9];
                    equipment.itemOnContract = saMach[10];
                    equipment.itemSequence = saMach[11];
                    equipment.model = saMach[12];
                    equipment.printerInterfaceType = saMach[13];
                    equipment.priority = saMach[14];
                    equipment.problemSubtype = saMach[15];
                    equipment.problemSummary = saMach[16];
                    equipment.purchaseOrder = saMach[17];
                    equipment.serial = saMach[18];
                    equipment.shipVia = saMach[19];
                    equipment.source = saMach[20];
                    equipment.ticket = saMach[21];
                    equipment.unit = saMach[22];

                    serviceRequest.equipmentList.Add(equipment);
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected string scrubDelim(string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            value = value.Replace("|", "");
            value = value.Replace("~", "");
        }

        return value;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_ContactPanel()
    {
        DataTable dt = new DataTable("");
        string sAllowRequestEntryAsPM_YN = "";
        string sAutoLoadRequestContactAndPhone_YN = "";
        int iCustomerNumber = 0;

        if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
            iCustomerNumber = -1;
        
        rbContactListForPm.Visible = false;

        try
        {
            if (iCustomerNumber > 0) 
            {
                sAllowRequestEntryAsPM_YN = ws_Get_B1CustPref_AllowRequestEntryAsPM_YN(iCustomerNumber);
                if (sAllowRequestEntryAsPM_YN == "Y")
                {
                    rbContactListForPm.Visible = true;
                    lbContactListPickTitle.Text = "Select type of service needed <br />(Standard or PM) from your location list";
                }
                else 
                {
                    lbContactListPickTitle.Text = "Select units from this location's <br />equipment list (recommended)";
                }

                sAutoLoadRequestContactAndPhone_YN = ws_Get_B1CustPref_AutoLoadRequestContactAndPhone_YN(iCustomerNumber);
                if (sAutoLoadRequestContactAndPhone_YN == "Y")
                {
                    txContactName.Text = serviceRequest.contactName;
                    txCompanyPhone.Text = serviceRequest.companyPhone;
                    lbContactNameTitle.Text = "Verify name of company contact";
                    lbContactPhoneTitle.Text = "Verify company phone number";
                }
                else 
                {
                    txContactName.Text = "";
                    txCompanyPhone.Text = "";
                    lbContactNameTitle.Text = "Enter name of company contact";
                    lbContactPhoneTitle.Text = "Enter company phone number";
                }
            }


            int iValue = 0;
            for (int i = 0; i < 25; i++)
            {
                iValue = i + 1;
                ddContactManualEntryQty.Items.Insert(i, new System.Web.UI.WebControls.ListItem(iValue.ToString(), iValue.ToString()));
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
    protected void Load_EquipmentPanel()
    {
        DataTable dt = new DataTable("");

        try
        {
            dt = ws_Get_B1PartProductCodes(hfChosenCs1.Value, hfChosenCs2.Value);

            // Load it whether query was successful or not to clear or reload the drop down list
            ddSearchEquipmentCategory.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                ddSearchEquipmentCategory.DataValueField = "ProductCode";
                ddSearchEquipmentCategory.DataTextField = "ProductCode";
                ddSearchEquipmentCategory.DataBind();
                pnSearchProductCodes.Visible = true;
            }
            else 
            {
                pnSearchProductCodes.Visible = false;
            }

            ddSearchEquipmentCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value

            ViewState["vsDataTable_Eqp"] = null;
            BindGrid_Eqp();
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
    protected void Load_ProblemPanel()
    {
        DataTable dt = new DataTable("CombinedUnits");
        DataTable dtB1 = new DataTable("B1Units");
        DataTable dtB2 = new DataTable("B2Units");
        string sTemp = "";

        try
        {
            if (hfRequestSourcePage.Value == "EquipmentList") 
            {
                GetSelectedEquipmentRecords_gv();
                serviceRequest.contactName = txContactName.Text.Trim();
            }
            else if (hfRequestSourcePage.Value == "SerialList" || hfRequestSourcePage.Value == "AgrEqp")
            {
                // hfUnitList.Value is already loaded on the serial page...

                string sAutoLoadRequestContactAndPhone_YN = "N";
                int iCustomerNumber = 0;
                if (int.TryParse(hfChosenCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = 0;

                //
                if (iCustomerNumber > 0)
                {
                    sAutoLoadRequestContactAndPhone_YN = ws_Get_B1CustPref_AutoLoadRequestContactAndPhone_YN(iCustomerNumber); // 
                    if (sAutoLoadRequestContactAndPhone_YN == "Y")
                    {
                        txProblemContactName.Text = serviceRequest.contactName;
                        txProblemContactPhone.Text = serviceRequest.companyPhone;
                    }
                }
            }

            if (!String.IsNullOrEmpty(hfUnitList.Value))
            {
                string[] saUntAgrSrcList = hfUnitList.Value.Split('|');
                string[] saUntAgrSrc = { "" };
                string sB1UnitList = "";
                string sB1AgreementList = "";
                string sB2UnitList = "";
                string sB2AgreementList = "";

                // Separate the equipment checked into separate lists for a B1 query and a B2 api search
                for (int i = 0; i < saUntAgrSrcList.Length; i++)
                {
                    saUntAgrSrc = saUntAgrSrcList[i].Split('~');
                    if (saUntAgrSrc.Length > 2)
                    {
                        if (saUntAgrSrc[2] == "1")
                        {
                            if (!String.IsNullOrEmpty(sB1UnitList))
                                sB1UnitList += "*";
                            sB1UnitList += saUntAgrSrc[0];

                            if (!String.IsNullOrEmpty(sB1AgreementList))
                                sB1AgreementList += "*";
                            sB1AgreementList += saUntAgrSrc[1];
                        }
                        else if (saUntAgrSrc[2] == "2")
                        {
                            if (!String.IsNullOrEmpty(sB2UnitList))
                                sB2UnitList += "*";
                            sB2UnitList += saUntAgrSrc[0];

                            if (!String.IsNullOrEmpty(sB2AgreementList))
                                sB2AgreementList += "*";
                            sB2AgreementList += saUntAgrSrc[1];
                        }

                    }
                }
                if (!String.IsNullOrEmpty(sB1UnitList) && !String.IsNullOrEmpty(sB1AgreementList))
                    dtB1 = ws_Get_B1DetailForSelectedUnits(sB1UnitList, sB1AgreementList);

                if (!String.IsNullOrEmpty(sB2UnitList) && !String.IsNullOrEmpty(sB2AgreementList))
                    dtB2 = ws_Get_B2DetailForSelectedUnits(sB2UnitList, sB2AgreementList);

                // merge
                dt = Merge_RequestTables(dtB1, dtB2);

                if (dt.Rows.Count > 0)
                {
                    sTemp = "<table>";
                    foreach (DataRow row in dt.Rows)
                    {
                        sTemp += "<tr>" +
                            "<td style='padding-right:15px;'>" + row["Model"].ToString().Trim().ToUpper() + "</td>" +
                            "<td style='padding-right:15px;'>" + Fix_Case(row["ModelDescription"].ToString().Trim()) + "</td>" +
                            "<td style='padding-right:5px;'>" + row["Serial"].ToString().Trim().ToUpper() + "</td>" +
                            "</tr>";
                    }
                    sTemp += "</table>";
                }
                lbSelectedEquipment.Text = sTemp;
            }
            else if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry") 
            {
                // make a forced data table
                dt = Build_ForcedItemTable();
            }  
            

            rp_Problem.DataSource = dt;
            rp_Problem.DataBind();

            CustomizeLoadedProblemRecords_rp(); 

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
    protected void Load_ResultPanel()
    {
        DataTable dt = new DataTable("");
        lbResultMsg.Text = "";

        try
        {
            dt = Build_ResultTable();
            if (dt.Rows.Count > 0)
            {
                gv_Result.DataSource = dt;
                gv_Result.DataBind();
                gv_Result.Visible = true;

                // Save returned result to workfiles (i.e. duration)
            }
            else 
            {
                lbResultMsg.Text = "Error: No ticket was created";
                gv_Result.Visible = false;
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
    protected void Hide_Panels()
    {
        pnLocation.Visible = false;
        pnContact.Visible = false;
        pnEquipment.Visible = false;
        pnProblem.Visible = false;
        pnResult.Visible = false;
        pnSerial.Visible = false;
        pnSelected.Visible = false;
        pnSelectedLocation.Visible = false;
        pnSelectedContact.Visible = false;
        pnSelectedContactEntry.Visible = false;
        pnSelectedEquipment.Visible = false;

    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Build_ResultTable()
    {
        string[] saTck = { "" };
        string[] saCtrTckModPrb = { "" };
        DataRow dr;
        int iRowIdx = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        int iCenter = 0;
        int iTicket = 0;
        try
        {
            dt.Columns.Add(MakeColumn("Call"));
            dt.Columns.Add(MakeColumn("Model"));
            dt.Columns.Add(MakeColumn("Problem"));

            saTck = hfRequestResultList.Value.Split('|');
            for (int i = 0; i < saTck.Length; i++)
            {
                saCtrTckModPrb = saTck[i].Split('~');
                if (saCtrTckModPrb.Length > 3)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);

                    if (int.TryParse(saCtrTckModPrb[0], out iCenter) == false)
                        iCenter = -1;
                    if (int.TryParse(saCtrTckModPrb[1], out iTicket) == false)
                        iTicket = -1;

                    if (iCenter > 0 && iTicket > 0)
                    {
                        dt.Rows[iRowIdx]["Call"] = iCenter + "-" + iTicket;
                    }
                    else 
                    {
                        dt.Rows[iRowIdx]["Call"] = saCtrTckModPrb[0].Trim() + " " + saCtrTckModPrb[1].Trim();
                    }
                    
                    dt.Rows[iRowIdx]["Model"] = saCtrTckModPrb[2];
                    dt.Rows[iRowIdx]["Problem"] = saCtrTckModPrb[3];
                    
                    iRowIdx++;
                }
            }
            dt.AcceptChanges();
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
    protected DataTable Build_ForcedItemTable()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("Agreement"));
        dt.Columns.Add(MakeColumn("AgreementDescription"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("AgreementCode"));
        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("ProductCode"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        int iRowIdx = 0;
        int iForcedRowCount = 0;
        if (int.TryParse(ddContactManualEntryQty.SelectedValue, out iForcedRowCount) == false)
            iForcedRowCount = -1;
        if (iForcedRowCount > 0)
        {

            for (int i=0; i< iForcedRowCount; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                dt.Rows[iRowIdx]["Unit"] = "";
                dt.Rows[iRowIdx]["Agreement"] = ""; 
                dt.Rows[iRowIdx]["AgreementDescription"] = ""; 
                dt.Rows[iRowIdx]["Model"] = ""; 
                dt.Rows[iRowIdx]["Serial"] = ""; 
                dt.Rows[iRowIdx]["AgreementCode"] = ""; 
                dt.Rows[iRowIdx]["CustomerNumber"] = "";
                dt.Rows[iRowIdx]["CustomerLocation"] = "";
                dt.Rows[iRowIdx]["ProductCode"] = "";
                dt.Rows[iRowIdx]["ModelDescription"] = "";

                dt.Rows[iRowIdx]["Source"] = "0";

                iRowIdx++;
            }
        }
        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable Merge_EquipmentTablesForLocations(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("AgentId"));
        dt.Columns.Add(MakeColumn("Agreement"));
        dt.Columns.Add(MakeColumn("AgreementDescription"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("ModelXref"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("Source"));
        dt.Columns.Add(MakeColumn("Unit"));

        DataRow dr;
        int iRowIdx = 0;
        int iTemp = 0;

        foreach (DataRow row in dt1.Rows)
        {
            if (row["AgrType"].ToString().Trim() == "CW")
            {
                // for now, query the connectwise records, but exclude them from the merged table
            }
            else
            {


                dr = dt.NewRow();
                dt.Rows.Add(dr);

                dt.Rows[iRowIdx]["AgentId"] = Fix_Case(row["AgentId"].ToString()).Trim();
                dt.Rows[iRowIdx]["Agreement"] = row["Agreement"].ToString().Trim();
                dt.Rows[iRowIdx]["AgreementDescription"] = row["AgreementDescription"].ToString().Trim().ToUpper();
                dt.Rows[iRowIdx]["Model"] = row["Model"].ToString().Trim().ToUpper();
                dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString().Trim());
                dt.Rows[iRowIdx]["ModelXref"] = Fix_Case(row["ModelXref"].ToString().Trim());
                dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim().ToUpper();

                if (int.TryParse(row["Unit"].ToString().Trim(), out iTemp) == false)
                    iTemp = -1;
                if (iTemp > 0)
                    dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

                if (row["AgrType"].ToString().Trim() == "CW")
                    dt.Rows[iRowIdx]["Source"] = "2";
                else
                    dt.Rows[iRowIdx]["Source"] = "1";

                iRowIdx++;
            }
        }

        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["AgentId"] = "";
            dt.Rows[iRowIdx]["Agreement"] = row["agreementId"].ToString().Trim();
            dt.Rows[iRowIdx]["AgreementDescription"] = ""; // row["description"].ToString().Trim();
            dt.Rows[iRowIdx]["Model"] = row["product-identifier"].ToString().Trim();
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["description"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelXref"] = "";
            dt.Rows[iRowIdx]["Serial"] = row["serialNumber"].ToString().Trim();

            if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0)
                dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

            dt.Rows[iRowIdx]["Source"] = "2";

            iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string Send_MoveEmail(
        int targetCustomerNumber, 
        int targetCustomerLocation, 
        string contact, 
        string phone, 
        string extension, 
        string moveList
        )
    {
        EmailHandler emailHandler = new EmailHandler();
        DataTable dt = new DataTable("");

        string[] saMoveList = new string[1];
        string[] saCs1Cs2UntAgrModSer = new string[1];
        string sPhoneFormatted = "";
        string sSubject = "";
        string sHtmlBody = "";
        string sSourceCustomerNumber = "";
        string sSourceCustomerLocation = "";
        string sSourceCustomerName = "";
        string sTargetCustomerName = "";
        string sItemId = "";
        string sAgreement = "";
        string sModel = "";
        string sSerial = "";
        string sEmailTo = "";
        string sEmailFrom = "";
        string sResult = "";
        //string sEmailAddress = "";

        int iSourceCustomerNumber = 0;
        int iSourceCustomerLocation = 0;

        try
        {
            if (!String.IsNullOrEmpty(phone) && phone.Length == 10) 
            {
                sPhoneFormatted = FormatPhone1(phone);
                if (!String.IsNullOrEmpty(extension))
                    sPhoneFormatted += " Ext: " + extension;
            }

            // Build HTML Email Content
            sSubject = "Equipment Move Email";
            if (contact != "")
                sSubject += " from " + contact;

            sHtmlBody = "<html><head><title>" +
                sSubject +
            "</title>" +
            "<style>" +
            "body { font-family: verdana; font-size: 13px; margin-left: 30px; }" +
            "</style>" +
            "</head><body>";

            sHtmlBody += "<p><b>Contact</b><br />";
            if (contact != "")
                sHtmlBody += contact;
            else
                sHtmlBody += "No name given...";
            sHtmlBody += "</p>";

            if (sPhoneFormatted != "")
                sHtmlBody += "<p><b>Phone</b><br />" + sPhoneFormatted + "</p>";

            sTargetCustomerName = ws_Get_B1CustomerName(targetCustomerNumber.ToString(), targetCustomerLocation.ToString());

            sHtmlBody += "<p><b><font size='+0' color='#AD0034'>" + 
                "Please move the following units to " + sTargetCustomerName + ": " + 
                targetCustomerNumber + "-" + targetCustomerLocation + 
                "</font></b></p>";
            sHtmlBody += "<table>";
            saMoveList = moveList.Split('|');

            for (int i = 0; i < saMoveList.Length; i++)
            {
                saCs1Cs2UntAgrModSer = saMoveList[i].Split('~');
                if (saCs1Cs2UntAgrModSer.Length > 5)
                {
                    sSourceCustomerNumber = saCs1Cs2UntAgrModSer[0].ToString().Trim();
                    sSourceCustomerLocation = saCs1Cs2UntAgrModSer[1].ToString().Trim();
                    sItemId = saCs1Cs2UntAgrModSer[2].ToString().Trim();
                    sAgreement = saCs1Cs2UntAgrModSer[3].ToString().Trim();
                    sModel = saCs1Cs2UntAgrModSer[4].ToString().Trim();
                    sSerial = saCs1Cs2UntAgrModSer[5].ToString().Trim();

                    if (int.TryParse(sSourceCustomerNumber, out iSourceCustomerNumber) == false)
                        iSourceCustomerNumber = -1;
                    if (int.TryParse(sSourceCustomerLocation, out iSourceCustomerLocation) == false)
                        iSourceCustomerLocation = -1;

                    sSourceCustomerName = ws_Get_B1CustomerName(iSourceCustomerNumber.ToString(), iSourceCustomerLocation.ToString());

                    sHtmlBody +=
                    "<tr><td><b>Model</b></td><td>" + sModel + "</td></tr>" +
                    "<tr><td><b>Serial</b></td><td>" + sSerial + "</td></tr>" +
                    "<tr><td><b>ItemId</b></td><td>" + sItemId + "</td></tr>" +
                    "<tr><td><b>Agreement</b></td><td>" + sAgreement + "</td></tr>" +
                    "<tr><td style='padding-right: 20px;'><b>Current Location</b></td><td>" + sSourceCustomerName + " (" + sSourceCustomerNumber + "-" + sSourceCustomerLocation + ")</td></tr>" +
                    "<tr><td>&nbsp;</td><td>&nbsp;</td></tr>";
                }
            }
            sHtmlBody += "</table>";

            if (!String.IsNullOrEmpty(hfEmailUserName.Value))
                sEmailFrom = hfEmailUserName.Value;
            else
                sEmailFrom = "adv320@scantron.com";

            dt = ws_Get_B1EmailAddressesByCodeForEmployees("C05", "");

            sResult = emailHandler.EmailGroup(sSubject, sHtmlBody, dt, sEmailFrom);
            //foreach (DataRow row in dt.Rows)
            //{
            //    sEmailTo = row["Email"].ToString().Trim();
            //    sResult = emailHandler.EmailIndividual(sSubject, sHtmlBody, sEmailTo, sEmailFrom);
            //}

            sEmailTo = "steve.carlson@scantron.com";
            sResult = emailHandler.EmailIndividual(sSubject, sHtmlBody, sEmailTo, sEmailFrom);

            if (sResult == "SUCCESS")
            {
                lbSearchSerialOrAssetMsg.Text = "Email successfully sent for equipment move to " + sTargetCustomerName + " (" + targetCustomerNumber.ToString() + "-" + targetCustomerLocation.ToString() + ")";
                
                // Clear all checked boxes
                ClearSelectedSerialRecords_gv();
                ClearSelectedSerialRecords_rp();
                
                // Clear Move Contact Fields
                ddSerialMoveLocation.SelectedValue = "";
                
                // Commenting clearing the name and phone because it may be the same during user's repeated cleanup
                //txSerialMoveName.Text = "";
                //txSerialMovePhone.Text = "";
                //txSerialMoveExtension.Text = "";
            }
            else
            {
                lbSearchSerialOrAssetMsg.Text = "Error: There was a problem sending the email: " + sResult;
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            emailHandler = null;
        }
        // ----------------
        return sResult;
    }
    // ----------------------------------------------------------------------------------------------------
    protected void ClearSelectedSerialRecords_gv()
    {
        CheckBox chBxTemp = new CheckBox();
        string sType = "";

        foreach (Control c1 in gv_SerialLarge.Controls)
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
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chBxTemp = (CheckBox)c4;
                                        if (chBxTemp.Checked == true)
                                        {
                                            chBxTemp.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void Load_SelectedLocationLabel(int customerNumber, int customerLocation)
    {
        lbSelectedLocation.Text = "";
        
        try
        {
            string sZip = "";

            DataTable dt = ws_Get_B1CustomerLocationDetail(customerNumber.ToString(), customerLocation.ToString(), "", "", "", "", "", "", "", "");
            if (dt.Rows.Count > 0)
            {
                serviceRequest.customerNumber = dt.Rows[0]["CSTRNR"].ToString().Trim();
                serviceRequest.customerLocation = dt.Rows[0]["CSTRCD"].ToString().Trim();
                serviceRequest.customerName = dt.Rows[0]["CUSTNM"].ToString().Trim();
                serviceRequest.address1 = dt.Rows[0]["SADDR1"].ToString().Trim();
                serviceRequest.address2 = dt.Rows[0]["SADDR2"].ToString().Trim();
                serviceRequest.city = dt.Rows[0]["CITY"].ToString().Trim();
                serviceRequest.state = dt.Rows[0]["STATE"].ToString().Trim();
                serviceRequest.companyPhone = dt.Rows[0]["HPHONE"].ToString().Trim();
                serviceRequest.contactName = dt.Rows[0]["CONTNM"].ToString().Trim();
                sZip = dt.Rows[0]["ZIPCD"].ToString().Trim();
                if (!String.IsNullOrEmpty(sZip))
                {
                    if (sZip.Substring(0, 1) == "0"
                        || sZip.Substring(0, 1) == "1"
                        || sZip.Substring(0, 1) == "2"
                        || sZip.Substring(0, 1) == "3"
                        || sZip.Substring(0, 1) == "4"
                        || sZip.Substring(0, 1) == "5"
                        || sZip.Substring(0, 1) == "6"
                        || sZip.Substring(0, 1) == "7"
                        || sZip.Substring(0, 1) == "8"
                        || sZip.Substring(0, 1) == "9"
                    )
                    {
                        // US Zips
                        if (sZip.Length > 5)
                            sZip = sZip.Substring(0, 5);
                    }
                    else
                    {
                        // Canada and other Foreign Zips
                        sZip = sZip.Replace("    ", " ");
                        sZip = sZip.Replace("   ", " ");
                        sZip = sZip.Replace("  ", " ");
                    }
                }
                serviceRequest.zip = sZip;
                serviceRequest.companyPhone = dt.Rows[0]["HPHONE"].ToString().Trim();
                txCompanyPhone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();
                txCompanyPhoneExtension.Text = "";

                serviceRequest.defaultContactName = dt.Rows[0]["CONTNM"].ToString().Trim();
                txContactName.Text = dt.Rows[0]["CONTNM"].ToString().Trim();


                sTemp = Fix_Case(serviceRequest.customerName) +
                    "  (<i>Customer:</i> " + serviceRequest.customerNumber + "-" + serviceRequest.customerLocation + ")";

                if (!String.IsNullOrEmpty(serviceRequest.address1))
                    sTemp += " " + Fix_Case(serviceRequest.address1);
                if (!String.IsNullOrEmpty(serviceRequest.address2))
                    sTemp += " " + Fix_Case(serviceRequest.address2);
                if (!String.IsNullOrEmpty(serviceRequest.city))
                    sTemp += " " + Fix_Case(serviceRequest.city);
                if (!String.IsNullOrEmpty(serviceRequest.state))
                    sTemp += " " + serviceRequest.state;
                if (!String.IsNullOrEmpty(serviceRequest.zip))
                    sTemp += " " + serviceRequest.zip;

                // Don't load this in the location label, wait to see what is selected for contact
                //if (!String.IsNullOrEmpty(serviceRequest.defaultContactName))
                //    sTemp += "  <i>Contact:<i> " + Fix_Case(serviceRequest.defaultContactName);

                //if (!String.IsNullOrEmpty(serviceRequest.companyPhone))
                //{
                //    if (serviceRequest.companyPhone.Length == 10)
                //    {
                //        sTemp += " (" + serviceRequest.companyPhone.Substring(0, 3) + ")" +
                //            " " + serviceRequest.companyPhone.Substring(3, 3) +
                //            "-" + serviceRequest.companyPhone.Substring(6, 4);
                //    }
                //}

                lbSelectedLocation.Text = sTemp;
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
                    
                    // Jan 20th, 2023 - DEBUG: have to move this inside to ensure the array has elements
                    ddSearchCustomerFamily.Items.Insert(i, new System.Web.UI.WebControls.ListItem(saNamNum[1] + "  " + saNamNum[0], saNamNum[1]));
                }
                
            }

            ddSearchCustomerFamily.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected string Validate_ContactEntries()
    {
        string sVerdict = "";

        string sPhone = Clean_PhoneEntry(txCompanyPhone.Text);
        txCompanyPhone.Text = sPhone;
        string sExtension = Clean_PhoneEntry(txCompanyPhoneExtension.Text);
        txCompanyPhoneExtension.Text = sExtension;

        try 
        {
            // -----------------------------------------------------------
            if (String.IsNullOrEmpty(txContactName.Text))
                sVerdict = "Contact name is required";
            else if (txContactName.Text.Length > 30) 
            {
                sVerdict = "Contact name too long (max length 30)";
                txContactName.Text = txContactName.Text.Substring(0, 30);
            }
            // -----------------------------------------------------------
            if (String.IsNullOrEmpty(txCompanyPhone.Text))
            {
                if (sVerdict != "") sVerdict += "<br />";
                sVerdict += "Company phone required";
            }
            else if (sPhone.Length != 10) 
            {
                if (sVerdict != "") sVerdict += "<br />";
                sVerdict += "Company phone entry must contain 10 numbers";
            }
            // -----------------------------------------------------------
            if (!String.IsNullOrEmpty(txCompanyPhoneExtension.Text) && sExtension.Length > 7)
            {
                if (sVerdict != "") sVerdict += "<br />";
                sVerdict += "Extension too long (max length 8)";
            }

            // -----------------------------------------------------------

            if (ddContactMethodPreference.SelectedValue == "EML")
            {
                if (!String.IsNullOrEmpty(txContactMethodDetail.Text))
                {
                    if (isEmailFormatValid(txContactMethodDetail.Text.Trim()) != true)
                    {
                        if (sVerdict != "") sVerdict += "<br />";
                        sVerdict += "Contact email format appears to be invalid";
                    }
                    if (txContactMethodDetail.Text.Trim().Length > 50)
                    {
                        if (sVerdict != "") sVerdict += "<br />";
                        sVerdict += "Email too long (max length 50)";
                    }
                    if (txContactMethodDetail.Text.IndexOf("@") > -1 && txContactMethodDetail.Text.LastIndexOf("@") > -1 && txContactMethodDetail.Text.IndexOf("@") != txContactMethodDetail.Text.LastIndexOf("@"))
                    {
                        if (sVerdict != "") sVerdict += "<br />";
                        sVerdict += "Please entry only one email address";
                    }
                }
                else
                {
                    if (sVerdict != "") sVerdict += "<br />";
                    sVerdict += "Please enter an email for the technician to contact you";
                }
            }
            else if (ddContactMethodPreference.SelectedValue == "PHN")
            {
                sPhone = Clean_PhoneEntry(txContactMethodDetail.Text);
                txContactMethodDetail.Text = sPhone;
                if (String.IsNullOrEmpty(txContactMethodDetail.Text))
                {
                    if (sVerdict != "") sVerdict += "<br />";
                    sVerdict += "Please enter a contact phone number so the technician can reach you";
                }
                else if (sPhone.Length != 10)
                {
                    if (sVerdict != "") sVerdict += "<br />";
                    sVerdict += "Please enter the 10 digits of a phone number (numbers only).";
                }

                if (!String.IsNullOrEmpty(txContactMethodPhoneExt.Text))
                {
                    sExtension = Clean_PhoneEntry(txContactMethodPhoneExt.Text);
                    txContactMethodPhoneExt.Text = sExtension;
                    int iExt = 0;
                    if (int.TryParse(sExtension, out iExt) == false)
                        iExt = -1;
                    if (sExtension.Length >= 8)
                    {
                        if (sVerdict != "") sVerdict += "<br />";
                        sVerdict += "Extension too long: max length 7";
                    }
                    if (iExt <= 0)
                    {
                        if (sVerdict != "") sVerdict += "<br />";
                        sVerdict += "Please enter only numbers for the phone extension";
                    }
                }
            }
            else if (ddContactMethodPreference.SelectedValue == "TXT")
            {
                sPhone = Clean_PhoneEntry(txContactMethodDetail.Text);
                txContactMethodDetail.Text = sPhone;
                if (String.IsNullOrEmpty(txContactMethodDetail.Text))
                {
                    if (sVerdict != "") sVerdict += "<br />";
                    sVerdict += "Please enter a mobile phone number for text messaging";
                }
                else if (sPhone.Length != 10)
                {
                    if (sVerdict != "") sVerdict += "<br />";
                    sVerdict += "Please enter the 10 digits of a phone number for text messages (numbers only).";
                }

            }
            // -----------------------------------------------------------
            if (!String.IsNullOrEmpty(txContactEmailAcknowledgement.Text) && isEmailFormatValid(txContactEmailAcknowledgement.Text) == false) 
            {
                if (sVerdict != "") sVerdict += "<br />";
                sVerdict += "Acknowledgement email format appears to be invalid.";
            }
            
            // -----------------------------------------------------------
            if (String.IsNullOrEmpty(sVerdict))
                sVerdict = "VALID";

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {

        }

        return sVerdict;
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
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Loc = "";

        if (ViewState["vsDataTable_Loc"] == null)
        {
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dt;
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
    #region repeaterRowProcessing
    // ========================================================================
    protected void GetSelectedProblemRecords_rp()
    {
        // This loops through all problems items and places each as request one at a time.
        EmailHandler emailHandler = new EmailHandler();

        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        HiddenField hfTemp = new HiddenField();
        DropDownList ddTemp = new DropDownList();

        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();
        TimeSpan timespan = new TimeSpan();

        string sDuration = "";
        double dDuration = 0.0;
        int iMinutes = 0;
        int iSeconds = 0;
        int iMilliseconds = 0;

        int iSeq = 0;
        int iUnit = 0;
        int iVia = 0;
        int iPrimaryCustomerNumber = 0;
        int iRequestWorkfileKey = 0;

        string sAgreement = "";
        string sModel = "";
        string sSerial = "";
        string sProblemSummary = "";
        string sTicketCrossRef = "";
        string sAgreementCode = "";
        string sAgreementDescription = "";
        string sSource = "";
        string sSpecialRequestType = "";
        string sPrinterInterface = "";
        string sPrimaryCustomerType = "";
        string sCreatorName = "";
        string sResponseFromWebService = "";
        string sUserLoginEmail = hfEmailUserName.Value.Trim();
        string sEmailSubject = "";
        string sEmailMessage = "";
        string sEmailTo = "";
        string sEmailFrom = "";
        string sEnteredContactName = "";
        string sEnteredContactPhone = "";
        string sEnteredContactExtension = "";
        string sEnteredEmailAcknowledgment = "";
        string sEnteredTicketCreator = "";

        string sControlType = "";
        //string sResult = "";
        string sAutoOrForced = "";
        string sB1ServrightCustomer_ServiceType = "";

        if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
            sAutoOrForced = "FORCED";
        else
            sAutoOrForced = "AUTO";

        string sEmailForCallAcknowledgement = "";
        if (!String.IsNullOrEmpty(txContactEmailAcknowledgement.Text))
            sEmailForCallAcknowledgement = txContactEmailAcknowledgement.Text.Trim();
        string sEmailResult = "";

        string sCommunicationMethodType = ddContactMethodPreference.SelectedValue;
        string sCommunicationMethodInfo = "";
        if (sCommunicationMethodType == "PHN" && txContactMethodPhoneExt.Text.Trim() != "")
            sCommunicationMethodInfo =  txContactMethodDetail.Text.Trim() + "|" + txContactMethodPhoneExt.Text.Trim();
        else if (sCommunicationMethodType == "NON")
            sCommunicationMethodInfo = "";
        else
            sCommunicationMethodInfo = txContactMethodDetail.Text.Trim();

        if (hfSerialPickRequestType.Value == "PM"
            || rbContactListForPm.Checked == true)
                sSpecialRequestType = "PM";

        if (!String.IsNullOrEmpty(txContactRequestorName.Text))
            sCreatorName = txContactRequestorName.Text.Trim();

        if (int.TryParse(hfPrimaryCs1.Value.Trim(), out iPrimaryCustomerNumber) == false)
            iPrimaryCustomerNumber = 0;

        sPrimaryCustomerType = hfPrimaryCs1Type.Value.Trim();

        TicketHandler ticketHandler = new TicketHandler(sLibrary);

        int[] iaCtrTckReq = { 0, 0, 0 };

        string sRequestSourceName = "";
        if (sLibrary == "OMDTALIB")
            sRequestSourceName = "CustSiteRequest_LIVE";
        else
            sRequestSourceName = "CustSiteRequest_TEST";

        // Load fields based on users chosen path into the request
        if (
               hfRequestSourcePage.Value == "SerialList"
            || hfRequestSourcePage.Value == "AgrEqp")
        {
            sEnteredContactName = txProblemContactName.Text.Trim();
            sEnteredContactPhone = txProblemContactPhone.Text.Trim();
            sEnteredContactExtension = txProblemContactExtension.Text.Trim();
            sEnteredEmailAcknowledgment = txProblemEmailAcknowledgement.Text.Trim();
            sEnteredTicketCreator = txProblemTicketCreator.Text.Trim();
        }
        else
        {
            sEnteredContactName = txContactName.Text.Trim();
            sEnteredContactPhone = txCompanyPhone.Text.Trim();
            sEnteredContactExtension = txCompanyPhoneExtension.Text.Trim();
            sEnteredEmailAcknowledgment = sEmailForCallAcknowledgement.Trim();
            sEnteredTicketCreator = sCreatorName.Trim();
        }

        try
        {

            foreach (Control c1 in rp_Problem.Controls)
            {
                sControlType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sControlType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                        {
                            lbTemp = (Label)c2;
                            if (lbTemp.ID.Contains("lbProblemModel") && sSource != "0")
                                sModel = lbTemp.Text.Trim().ToUpper();
                            else if (lbTemp.ID.Contains("lbProblemSerial") && sSource != "0")
                                sSerial = lbTemp.Text.Trim().ToUpper();
                            else if (lbTemp.ID.Contains("lbProblemAgreementDescription"))
                                sAgreementDescription = lbTemp.Text.Trim().ToUpper();
                        }
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                        {
                            txTemp = (TextBox)c2;
                            if (txTemp.ID.StartsWith("txProblemDescription"))
                                sProblemSummary = txTemp.Text.Trim().ToUpper();
                            else if (txTemp.ID.StartsWith("txProblemTicketXref"))
                                sTicketCrossRef = txTemp.Text.Trim().ToUpper();
                            else if (txTemp.ID.StartsWith("txProblemModel") && sSource == "0")
                                sModel = txTemp.Text.Trim().ToUpper();
                            else if (txTemp.ID.StartsWith("txProblemSerial") && sSource == "0")
                                sSerial = txTemp.Text.Trim().ToUpper();

                        }
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                        {
                            ddTemp = (DropDownList)c2;
                            if (ddTemp.ID.Contains("ddProblemInterface"))
                            {
                                sPrinterInterface = ddTemp.SelectedValue;
                            }
                            if (ddTemp.ID == "ddProblemVia")
                            {
                                if (int.TryParse(ddTemp.SelectedValue.ToString(), out iVia) == false)
                                    iVia = 0;
                            }
                        }
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                        {
                            hfTemp = (HiddenField)c2;
                            if (hfTemp.ID.StartsWith("hfProblemSource"))
                                sSource = hfTemp.Value.Trim();
                            if (hfTemp.ID.StartsWith("hfProblemAgreementNumber"))
                                sAgreement = hfTemp.Value.Trim();
                            if (hfTemp.ID.StartsWith("hfProblemAgreementCode"))
                                sAgreementCode = hfTemp.Value.Trim();
                            if (hfTemp.ID.StartsWith("hfProblemUnit"))
                                if (int.TryParse(hfTemp.Value, out iUnit) == false)
                                    iUnit = 0;
                            if (hfTemp.ID.StartsWith("hfProblemProcessor"))
                            {
                                if (iUnit > 0)
                                {
                                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                                    if (sSource == "1") // Building One Service Ticket
                                    {
                                        // Now Create Service Request
                                        startTime = DateTime.Now;
                                        iSeq++;
                                        // xxxx

                                        iaCtrTckReq = ticketHandler.AddTicket(
                                            iUnit,
                                            sTicketCrossRef,
                                            sProblemSummary,
                                            txProblemComment.Text.Trim(),
                                            sEnteredContactName,  // txContactName.Text.Trim(),
                                            sEnteredContactPhone, // txCompanyPhone.Text.Trim(),
                                            sEnteredContactExtension, // txCompanyPhoneExtension.Text.Trim(),
                                            sEnteredEmailAcknowledgment, // sEmailForCallAcknowledgement,
                                            sCommunicationMethodType,
                                            sCommunicationMethodInfo,
                                            sPrinterInterface,
                                            iVia,
                                            sSpecialRequestType,
                                            sAutoOrForced,
                                            sRequestSourceName,
                                            sEnteredTicketCreator, // sCreatorName,
                                            iPrimaryCustomerNumber,
                                            sPrimaryCustomerType,
                                            0,
                                            sUserLoginEmail
                                            );
                                        if (iaCtrTckReq.Length > 2 && iaCtrTckReq[0] > 0 && iaCtrTckReq[1] > 0)
                                        {
                                            // Save duration
                                            endTime = DateTime.Now;
                                            timespan = endTime - startTime;
                                            sDuration = "";
                                            dDuration = 0.0;
                                            iMinutes = 0;
                                            iSeconds = 0;
                                            iMilliseconds = 0;
                                            if (int.TryParse(timespan.Minutes.ToString(), out iMinutes) == false)
                                                iMinutes = 0;
                                            if (int.TryParse(timespan.Seconds.ToString(), out iSeconds) == false)
                                                iSeconds = 0;
                                            if (int.TryParse(timespan.Milliseconds.ToString(), out iMilliseconds) == false)
                                                iMilliseconds = 0;
                                            iSeconds = (60 * iMinutes) + iSeconds;
                                            sDuration = iSeconds.ToString() + "." + iMilliseconds.ToString();
                                            if (double.TryParse(sDuration, out dDuration) == false)
                                                dDuration = -0.1;
                                            iRequestWorkfileKey = iaCtrTckReq[2];

                                            sResponseFromWebService = ws_Upd_B1RequestDuration(iRequestWorkfileKey, dDuration);

                                            // Save results from each request to load in table on final result panel to show customer
                                            if (!String.IsNullOrEmpty(hfRequestResultList.Value))
                                                hfRequestResultList.Value += "|";
                                            //hfRequestResultList.Value += iaCtrTckReq[0] + "-" + iaCtrTckReq[1];
                                            hfRequestResultList.Value += iaCtrTckReq[0] + "~" + iaCtrTckReq[1] + "~" + sModel + "~" + sProblemSummary + "~x";
                                        }

                                    }
                                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                                    else if (sSource == "2") // Building Two Ticket (Service or Project? -- maybe generic email) 
                                    {
                                        int iCs1 = 0;
                                        int iCs2 = 0;
                                        string sCustomerName = "";

                                        if (int.TryParse(hfChosenCs1.Value, out iCs1) == false)
                                            iCs1 = -1;
                                        if (int.TryParse(hfChosenCs2.Value, out iCs2) == false)
                                            iCs2 = -1;

                                        if (iCs1 > 0 && iCs2 > -1)
                                        {
                                            sCustomerName = ws_Get_B1CustomerName(iCs1.ToString(), iCs2.ToString());
                                        }
                                        //string sPhone = txCompanyPhone.Text.Trim();
                                        string sPhone = sEnteredContactPhone;
                                        if (!String.IsNullOrEmpty(sPhone) && sPhone.Length == 10)
                                        {
                                            sPhone = FormatPhone1(sPhone);
                                        }

                                        string sContactPref = sCommunicationMethodType;
                                        if (sContactPref == "NON")
                                            sContactPref = "No preference (just use primary phone)";
                                        else if (sContactPref == "TXT")
                                            sContactPref = "By text message";
                                        else if (sContactPref == "PHN")
                                            sContactPref = "By phone";
                                        else if (sContactPref == "EML")
                                            sContactPref = "By email";

                                        /*
                                        sEmailSubject = "Connectwise Ticket Request";
                                        if (!String.IsNullOrEmpty(sCustomerName))
                                            sEmailSubject += " From " + Fix_Case(sCustomerName);

                                        sEmailMessage = "<html>";
                                        sEmailMessage += "<style type='text/css'>";
                                        sEmailMessage += "table { border: 1px solid #cccccc; }";
                                        sEmailMessage += "tr { vertical-align: top;}";
                                        sEmailMessage += "td { padding: 3px; }";
                                        sEmailMessage += "</style>";
                                        sEmailMessage += "Please create a ticket for the following request";
                                        sEmailMessage += "<table>";
                                        if (!String.IsNullOrEmpty(sCustomerName))
                                            sEmailMessage += "<tr><td>Customer</td><td>" + Fix_Case(sCustomerName) + "</td></tr>";
                                        sEmailMessage += "<tr><td>Oracle Id</td><td>" + hfOracleParentId.Value + "</td></tr>" +
                                            "<tr><td>Site Id</td><td>" + hfOracleChildId.Value + "</td></tr>" +
                                            "<tr><td>Item Id</td><td>" + iUnit.ToString() + "</td></tr>" +
                                            "<tr><td>Model</td><td>" + sModel + "</td></tr>" +
                                            "<tr><td>Problem Summary</td><td>" + sProblemSummary + "</td></tr>" +
                                            "<tr><td>Comment (Optional)</td><td>" + txProblemComment.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Agreement Desc</td><td>" + sAgreementDescription + "</td></tr>" +
                                            "<tr><td>Contact</td><td>" + txContactName.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Company Phone</td><td>" + sPhone + "</td></tr>" +
                                            "<tr><td>Extension</td><td>" + txCompanyPhoneExtension.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Email For Acknowledgement</td><td>" + sEmailForCallAcknowledgement + "</td></tr>" +
                                            "<tr><td>Preferred Communication Type</td><td>" + sContactPref + "</td></tr>" +
                                            "<tr><td>Additional Communication Info</td><td>" + sCommunicationMethodInfo + "</td></tr>" +
                                            "</table></html>";
                                        */

                                        sEmailSubject = sProblemSummary;

                                        sEmailMessage = "<html>";
                                        //sEmailMessage += "<style type='text/css'>";
                                        //sEmailMessage += "table { }";
                                        //sEmailMessage += "tr { vertical-align: top;}";
                                        //sEmailMessage += "td { padding: 3px; }";
                                        //sEmailMessage += "</style>";
                                        sEmailMessage += "<table>";
                                        sEmailMessage += "<tr><td>";

                                        if (!String.IsNullOrEmpty(txProblemComment.Text.Trim())) 
                                        {
                                            sEmailMessage += "Comments:";
                                            sEmailMessage += "<br />" + txProblemComment.Text.Trim();
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                        }

                                        if (!String.IsNullOrEmpty(sCustomerName)) 
                                        {
                                            sEmailMessage += "Company Name:";
                                            sEmailMessage += "<br />" + Fix_Case(sCustomerName);
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                        }

                                        sEmailMessage += "Parent Site Id:";
                                        sEmailMessage += "<br />" + hfOracleParentId.Value;
                                        sEmailMessage += "<br />----------------------------------------<br />";

                                        sEmailMessage += "Child Site Id:";
                                        sEmailMessage += "<br />" + hfOracleChildId.Value;
                                        sEmailMessage += "<br />----------------------------------------<br />";

                                        sEmailMessage += "Model:";
                                        sEmailMessage += "<br />" + sModel;
                                        sEmailMessage += "<br />----------------------------------------<br />";


                                        if (!String.IsNullOrEmpty(sTicketCrossRef)) 
                                        {
                                            sEmailMessage += "Ticket Cross Reference:";
                                            sEmailMessage += "<br />" + sTicketCrossRef;
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                        }

                                        sEmailMessage += "Contact Name";
                                        //sEmailMessage += "<br />" + txContactName.Text.Trim();
                                        sEmailMessage += "<br />" + sEnteredContactName;
                                        sEmailMessage += "<br />----------------------------------------<br />";


                                        sEmailMessage += "Phone:";
                                        sEmailMessage += "<br />" + sPhone;
                                        sEmailMessage += "<br />----------------------------------------<br />";

                                        //if (!String.IsNullOrEmpty(txCompanyPhoneExtension.Text.Trim()))
                                        if (!String.IsNullOrEmpty(sEnteredContactExtension))
                                        {
                                            sEmailMessage += "Extension:";
                                            //sEmailMessage += "<br />" + txCompanyPhoneExtension.Text.Trim();
                                            sEmailMessage += "<br />" + sEnteredContactExtension;
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                        }

                                        if (!String.IsNullOrEmpty(hfEmailUserName.Value)) 
                                        {
                                            sEmailMessage += "Ticket Creator Email Address:";
                                            sEmailMessage += "<br />" + hfEmailUserName.Value;
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                        }
                                        string sAcknowledgementEmail = "";
                                        //if (!String.IsNullOrEmpty(txContactEmailAcknowledgement.Text) && hfEmailUserName.Value.Trim() != txContactEmailAcknowledgement.Text.Trim())
                                        if (!String.IsNullOrEmpty(sEnteredEmailAcknowledgment) 
                                            && hfEmailUserName.Value.Trim() != sEnteredEmailAcknowledgment)
                                        {
                                            sEmailMessage += "*** Request for Ticket Acknowledgement to Alternate Email Address ***";
                                            //sEmailMessage += "<br />" + txContactEmailAcknowledgement.Text;
                                            sEmailMessage += "<br />" + sEnteredEmailAcknowledgment;
                                            sEmailMessage += "<br />----------------------------------------<br />";
                                            //sAcknowledgementEmail = txContactEmailAcknowledgement.Text;
                                            sAcknowledgementEmail = sEnteredEmailAcknowledgment;
                                        }

                                        sEmailMessage += "</td></tr>";
                                        sEmailMessage += "</table></html>";

                                        if (isEmailFormatValid(hfEmailUserName.Value) == true)
                                            sEmailFrom = hfEmailUserName.Value; 
                                        else
                                            sEmailFrom = "adv320@scantron.com";

                                        if (sLibrary == "OMDTALIB")
                                        {
                                            // B2 target email
                                            sEmailTo = "help@scantron.com";
                                            //sEmailTo = "mitsitops@scantron.com";
                                            sEmailResult = emailHandler.EmailIndividual(sEmailSubject, sEmailMessage, sEmailTo, sEmailFrom);
                                        }
                                        else 
                                        {
                                        }

                                        //sEmailTo = "steve.carlson@scantron.com";
                                        //sEmailResult = emailHandler.EmailIndividual(sEmailSubject, sEmailMessage, sEmailTo, sEmailFrom);

                                        if (!String.IsNullOrEmpty(hfRequestResultList.Value))
                                            hfRequestResultList.Value += "|";

                                        if (sEmailResult.StartsWith("SUCCESS"))
                                        {
                                            hfRequestResultList.Value += "Managed IT Ticket Created" + "~" + "" + "~" + sModel + "~" + sProblemSummary + "~x";
                                        }
                                        else
                                        {
                                            hfRequestResultList.Value += "Error: Request Saved But Placement Pending" + "~" + "" + "~" + sModel + "~" + sProblemSummary + "~x";
                                        }

                                        // Save BL2 Request to an SQL log file
                                        int iRowsAffected = Insert_BL2ServiceRequestLog(
                                            hfOracleParentId.Value,
                                            hfOracleChildId.Value,
                                            Fix_Case(sCustomerName),
                                            sModel,
                                            sTicketCrossRef,
                                            sProblemSummary,
                                            txProblemComment.Text.Trim(),
                                            hfEmailUserName.Value,
                                            txContactName.Text.Trim(),
                                            sPhone,
                                            txCompanyPhoneExtension.Text.Trim(),
                                            sAcknowledgementEmail);
                                    }

                                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                                }
                                else
                                {
                                    // Initialize
                                    sB1ServrightCustomer_ServiceType = "";
                                    // ---------------------------------------
                                    // Building One Forced Entry Ticket Email
                                    // ---------------------------------------
                                    if (sSource == "0") // FORCED Ticket (Unit == 0 also)
                                    {
                                        int iCs1 = 0;
                                        int iCs2 = 0;
                                        string sCustomerName = "";

                                        if (int.TryParse(hfChosenCs1.Value, out iCs1) == false)
                                            iCs1 = -1;
                                        if (int.TryParse(hfChosenCs2.Value, out iCs2) == false)
                                            iCs2 = -1;

                                        if (iCs1 > 0 && iCs2 > -1)
                                        {
                                            sCustomerName = ws_Get_B1CustomerName(iCs1.ToString(), iCs2.ToString());
                                            string sOr1Or2 = ws_Get_B1CustomerOracleIds(iCs1, iCs2);
                                            sB1ServrightCustomer_ServiceType = ws_Get_B1ServrightCustomer_ServiceType(iCs1, iCs2);
                                            string[] saOr1Or2 = sOr1Or2.Split('|');
                                            if (saOr1Or2.Length > 1)
                                            {
                                                hfOracleParentId.Value = saOr1Or2[0];
                                                hfOracleChildId.Value = saOr1Or2[1];
                                            }
                                        }
                                        string sPhone = txCompanyPhone.Text.Trim();
                                        if (!String.IsNullOrEmpty(sPhone) && sPhone.Length == 10)
                                        {
                                            sPhone = FormatPhone1(sPhone);
                                        }

                                        string sContactPref = sCommunicationMethodType;
                                        if (sContactPref == "NON")
                                            sContactPref = "No preference (just use primary phone)";
                                        else if (sContactPref == "TXT")
                                            sContactPref = "By text message";
                                        else if (sContactPref == "PHN")
                                            sContactPref = "By phone";
                                        else if (sContactPref == "EML")
                                            sContactPref = "By email";

                                        sEmailMessage = "<html>";
                                        sEmailMessage += "<style type='text/css'>";
                                        sEmailMessage += "table { border: 1px solid #cccccc; }";
                                        sEmailMessage += "tr { vertical-align: top;}";
                                        sEmailMessage += "td { padding: 5px;}";
                                        sEmailMessage += "</style>";
                                        sEmailMessage += "Please create a ServiceCOMMAND<span style='font-size: 12px; vertical-align: top; position: relative; top: 1px;'>®</span> or Connectwise ticket for the following request";
                                        sEmailMessage += "<table>";
                                        string sCs1Cs2 = "";
                                        if (iCs1 > 0)
                                            sCs1Cs2 = iCs1.ToString() + "-" + iCs2.ToString();
                                        if (!String.IsNullOrEmpty(sCustomerName))
                                            sEmailMessage += "<tr><td>Customer</td><td>" + Fix_Case(sCustomerName) + "</td></tr>" +
                                            "<tr><td>SC Customer Number</td><td>" + sCs1Cs2 + "</td></tr>";
                                        
                                        long lTemp = 0;
                                        if (long.TryParse(hfOracleParentId.Value, out lTemp) == false)
                                            lTemp = -1;
                                        if (lTemp > 0) 
                                            sEmailMessage += "<tr><td>Oracle Id</td><td>" + lTemp.ToString("") + "</td></tr>";
                                        else
                                            sEmailMessage += "<tr><td>Oracle Id</td><td></td></tr>";

                                        if (long.TryParse(hfOracleChildId.Value, out lTemp) == false)
                                            lTemp = -1;

                                        if (lTemp > 0)
                                            sEmailMessage += "<tr><td>Site Id</td><td>" + lTemp.ToString("") + "</td></tr>";
                                        else
                                            sEmailMessage += "<tr><td>Site Id</td><td></td></tr>";

                                        sEmailMessage += "<tr><td>Cust Ticket Cross Ref</td><td>" + sTicketCrossRef + "</td></tr>" +
                                            "<tr><td>Model</td><td>" + sModel + "</td></tr>" +
                                            "<tr><td>Serial</td><td>" + sSerial + "</td></tr>" +
                                            "<tr><td>Problem Summary</td><td>" + sProblemSummary + "</td></tr>" +
                                            "<tr><td>Comment (Optional)</td><td>" + txProblemComment.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Contact</td><td>" + txContactName.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Company Phone</td><td>" + sPhone + "</td></tr>" +
                                            "<tr><td>Extension</td><td>" + txCompanyPhoneExtension.Text.Trim() + "</td></tr>" +
                                            "<tr><td>Email For Acknowlegement</td><td>" + sEmailForCallAcknowledgement + "</td></tr>" +
                                            "<tr><td>Preferred Communication Type</td><td>" + sContactPref + "</td></tr>" +
                                            "<tr><td>Additional Communication Info</td><td>" + sCommunicationMethodInfo + "</td></tr>";
                                        if (sB1ServrightCustomer_ServiceType == "OA") 
                                        {
                                            sEmailMessage += "<tr><td>ServRight Service Type</td><td><b>Outsource Advantage (OA)</b></td></tr>";
                                            sEmailSubject = "Forced Service Request: (Outsource Advantage: OA) Manually Typed In - ";
                                        }
                                        else if (sB1ServrightCustomer_ServiceType == "SIG") 
                                        {
                                            sEmailMessage += "<tr><td>ServRight Service Type</td><td><b>Signature (SIG)</b></td></tr>";
                                            sEmailSubject = "Forced Service Request (Signature: SIG) Manually Typed In - ";
                                        }
                                        else 
                                        {
                                            sEmailSubject = "Forced Service Request (Manually Typed Entry)";
                                        }
                                        
                                        sEmailMessage += "</table></html>";
                                        
                                        if (!String.IsNullOrEmpty(sCustomerName))
                                            sEmailSubject += " From " + Fix_Case(sCustomerName);


                                        sEmailFrom = "adv320@scantron.com";

                                        if (sLibrary == "OMDTALIB")
                                        {
                                            sEmailTo = "ScantronHelp@Scantron.com";
                                            sEmailResult = emailHandler.EmailIndividual(sEmailSubject, sEmailMessage, sEmailTo, sEmailFrom);
                                        }
                                        else 
                                        {
                                            sEmailTo = "steve.carlson@scantron.com";
                                            sEmailResult = emailHandler.EmailIndividual(sEmailSubject, sEmailMessage, sEmailTo, sEmailFrom);
                                        }

                                        //sEmailTo = "steve.carlson@scantron.com";
                                        //sEmailResult = emailHandler.EmailIndividual(sEmailSubject, sEmailMessage, sEmailTo, sEmailFrom);

                                        if (!String.IsNullOrEmpty(hfRequestResultList.Value))
                                            hfRequestResultList.Value += "|";

                                        if (sEmailResult.StartsWith("SUCCESS"))
                                        {
                                            hfRequestResultList.Value += "Manual Placement Initiated" + "~" + "" + "~" + sModel + "~" + sProblemSummary + "~x";
                                        }
                                        else
                                        {
                                            hfRequestResultList.Value += "Error: Request Saved But Placement Pending" + "~" + "" + "~" + sModel + "~" + sProblemSummary + "~x";
                                        }
                                    }
                                    else // ??? No unit, not a forced ticket? If we someone got here, no ticket is being made?  Email somebody!
                                    { 

                                    }

                                }
                                // Clear workfields
                                iUnit = 0;
                                sAgreement = "";
                                sModel = "";
                                sSerial = "";
                                sProblemSummary = "";
                                sTicketCrossRef = "";
                                sAgreementCode = "";
                                sAgreementDescription = "";
                                sSource = "";
                            } // End the hidden field that means you have all fields gathered for the service request
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
            emailHandler = null;
            ticketHandler = null;
        }

    }
    // ----------------------------------------------------------------------------------------------------
    protected void CustomizeLoadedProblemRecords_rp()
    {
        Label lbTemp = new Label();
        DropDownList ddTemp = new DropDownList();
        HiddenField hfTemp = new HiddenField();
        TextBox txTemp = new TextBox();
        Panel pnTemp = new Panel();

        int iUnit = 0;

        string sPrt = "";
        string sPrtHold = "Your selected part";
        string sDsc = "";
        string sProductCode = "";
        string sInterfaceNeeded = "";
        //string sInterfaceFound = "";
        string sViaNeeded = "";
        //string sViaFound = "";
        string sControlType = "";
        string sSubcontractThisCallForXerox = "";
        string sPriority = "";
        string sSource = "";

        lbPriorityInfo.Text = "";

        foreach (Control c1 in rp_Problem.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    // ----------------------------------------------------------------------------------
                    // Label
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID.Contains("lbProblemModel"))
                        {
                            sPrt = lbTemp.Text.Trim().ToUpper();
                            sPrtHold = sPrt;
                            if (sSource == "1") 
                            {
                                // Check if Unit is a priority 1 (i.e. Express Printer)
                                sPriority = ws_Get_B1PartPriority(sPrt);
                                if (sPriority == "1")
                                {
                                    lbPriorityInfo.Text += sPrt + " is generally considered a CRITICAL piece of equipment...<br />";
                                }
                            }
                        }

                        else if (lbTemp.ID.Contains("lbProblemAgreementDescription"))
                        {
                            sDsc = lbTemp.Text.Trim().ToUpper();
                            if ((sDsc == "EXPRESS") || (sDsc == "DEPOT"))
                                sViaNeeded = "Y";
                            if (sSource == "1")
                            {
                                sProductCode = ws_Get_B1PartProductCode(sPrt); // sPrt for record was loaded above when model field was read (initialized at Via below)
                                if ((sProductCode == "PTR") && ((sDsc == "EXPRESS") || (sDsc == "PER-INCIDENT")))
                                    sInterfaceNeeded = "Y";
                            }
                        }
                        else if (lbTemp.ID.Contains("lbProblemModel") || lbTemp.ID.Contains("lbProblemSerial") || lbTemp.ID.Contains("lbProblemAgreementDescription"))
                        {
                            if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                                lbTemp.Visible = false;
                            else
                                lbTemp.Visible = true;
                        }
                    }
                    //-----------------------------------------------------------------------------
                    // Panels
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Panel"))
                    {
                        pnTemp = (Panel)c2;

                        if (pnTemp.ID.Contains("pnProblemInterface"))
                        {
                            if (sInterfaceNeeded == "Y")
                                pnTemp.Visible = true;
                            else
                                pnTemp.Visible = false;
                        }
                        else if (pnTemp.ID.Contains("pnProblemVia"))
                        {
                            if (sViaNeeded == "Y")
                                pnTemp.Visible = true;
                            else
                                pnTemp.Visible = false;
                        }
                        else if (pnTemp.ID.Contains("pnProblemError"))
                        {
                            pnTemp.Visible = false; 
                        }
                        else if (pnTemp.ID.Contains("pnProblemModel"))
                        {
                            if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                                pnTemp.Visible = true; 
                            else
                                pnTemp.Visible = false;
                        }
                        else if (pnTemp.ID.Contains("pnProblemServiceType"))
                        {
                            if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                                pnTemp.Visible = true;
                            else
                                pnTemp.Visible = false;
                        }
                    }
                    //-----------------------------------------------------------------------------
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;

                        if (hfTemp.ID.Contains("hfProblemUnit") && hfChosenCs1.Value == "102360") // condition for UNFI only
                        {
                            iUnit = 0;
                            if (int.TryParse(hfTemp.Value, out iUnit) == false)
                                iUnit = 0;
                            if (iUnit > 0)
                            {
                                // Check if machine is a Xerox which will be subcontracted
                                sSubcontractThisCallForXerox = ws_Get_B1UnitCustomerToSubcontractCallIfXerox_YN(iUnit.ToString());
                                if (sSubcontractThisCallForXerox == "Y")
                                {
                                    lbPriorityInfo.Text = "XEROX";
                                }
                            }
                        }
                        else if (hfTemp.ID.Contains("hfProblemSource"))
                        {
                            sSource = hfTemp.Value;
                        }
                        else if (hfTemp.ID.Contains("hfProblemProcessor"))
                        {
                            // Clear workfields
                            sPrt = "";
                            sDsc = "";
                            sInterfaceNeeded = "";
                            sViaNeeded = "";
                            sSource = "";
                        }
                    }
                    //-----------------------------------------------------------------------------
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;

                        if (txTemp.ID.Contains("txProblemModel") || txTemp.ID.Contains("txProblemSerial"))
                        {
                            if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                                txTemp.Visible = true;
                            else
                                txTemp.Visible = false;
                        }
                    }
                    //-----------------------------------------------------------------------------
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                      ddTemp = (DropDownList)c2;

                        if (ddTemp.ID.Contains("ddProblemServiceType"))
                        {
                            if (hfRequestSourcePage.Value == "ContactPage-ForcedEntry") 
                            {
                                ddTemp.Visible = true;
                                if (ddTemp.SelectedValue == "DEPOT" || ddTemp.SelectedValue == "EXPRESS")
                                    sViaNeeded = "Y";
                                else
                                    sViaNeeded = "N";
                            }
                            else
                                ddTemp.Visible = false;
                        }
                    }
                    // -----------------------------
                }
            } // end "foreach"
        } // end if a repeater item


        if (lbPriorityInfo.Text == "XEROX")
        {
            lbPriorityInfo.Text = "<b>URGENT:</b>  " + sPrtHold + " will require triage prior to technician dispatch.  " +
                "<br />To expedite the dispatch process, please call ";

            if (ShowNewCompanyName())
                lbPriorityInfo.Text += "Secur-Serv";
            else
                lbPriorityInfo.Text += "STS";

            lbPriorityInfo.Text += " at 1-800-892-8332 ext. 3276 " +
                "<br />with enduser on the phone" +
                "<br />before clicking the \"Submit Automated Request\" button " +
                "<br />to initiate a triage conversation with the contact listed on this service request." +
                "<br /><br />(Hours of operation Monday to Friday- 7 am to 7 pm CST)" +
                "<br /><br />";
            lbPriorityInfo.Visible = true;
        }
        else if (lbPriorityInfo.Text != "")
        {
            lbPriorityInfo.Text += "(You may proceed, but a live representative at 1-800-228-3628" +
                " can access all available support services.)" +
                "<br /><br />";
            lbPriorityInfo.Visible = true;
        }

        if (sInterfaceNeeded == "Y")
        {
            lbInterfaceInfo.Text = 
                "<table style='margin-bottom: 20px;'>" +
                "<tr><td colspan='2'>To help determine your printer interface type... </td></tr>" +
                "<tr><td colspan='2' style='padding-bottom: 5px; font-weight: bold;'>HOW DO YOU REMOVE YOUR CABLE?</td></tr>" +
                "<tr><td style='padding-left: 15px; padding-right: 10px;'>1) If it looks like a large phone jack... </td><td style='color:#AD0034'>It's NETWORK</td></tr>" +
                "<tr><td style='padding-left: 15px; padding-right: 10px;'>2) If it has a square connector... </td><td style='color:#AD0034'>It's USB</td></tr>" +
                "</table>";
            lbInterfaceInfo.Visible = true;
        }
        // --------------------------------------
    }
    // ----------------------------------------------------------------------------------------------------
    protected void GetSelectedSerialRecords_rp()
    {
        CheckBox chBxTemp = new CheckBox();
        string sControlType = "";

        try
        {

            foreach (Control c1 in rp_SerialSmall.Controls)
            {
                sControlType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sControlType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                        {
                            chBxTemp = (CheckBox)c2;
                            if (chBxTemp.ID.Contains("chBxMove")) 
                            {
                                if (chBxTemp.Checked == true)
                                {
                                    if (!hfMoveList.Value.Contains(chBxTemp.Text)) 
                                    {
                                        if (hfMoveList.Value != "")
                                            hfMoveList.Value += "|";
                                        hfMoveList.Value += chBxTemp.Text;
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
    // ----------------------------------------------------------------------------------------------------
    protected void ClearSelectedSerialRecords_rp()
    {
        CheckBox chBxTemp = new CheckBox();
        string sControlType = "";

        try
        {

            foreach (Control c1 in rp_SerialSmall.Controls)
            {
                sControlType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sControlType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                        {
                            chBxTemp = (CheckBox)c2;
                            if (chBxTemp.ID.Contains("chBxMove"))
                            {
                                if (chBxTemp.Checked == true)
                                {
                                    chBxTemp.Checked = false;
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
    // ----------------------------------------------------------------------------------------------------
    protected string Validate_Problem()
    {
        string sValid = "";

        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        DropDownList ddTemp = new DropDownList();
        HiddenField hfTemp = new HiddenField();
        Panel pnTemp = new Panel();
        Panel pnProblemError = new Panel();
        Label lbProblemError = new Label();


        string sPrt = "";
        string sDsc = "";
        string sProductCode = "";
        string sInterfaceNeeded = "";
        string sViaNeeded = "";
        string sControlType = "";
        string sProblemError = "";
        string sSource = "";

        // BACK IN lbPriorityInfo.Text = "";

        foreach (Control c1 in rp_Problem.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    // ----------------------------------------------------------
                    // Labels
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID.Contains("lbProblemModel") && hfRequestSourcePage.Value != "ContactPage-ForcedEntry")
                        {
                            sPrt = lbTemp.Text.Trim().ToUpper();
                        }
                        else if (lbTemp.ID.Contains("lbProblemAgreementDescription") && hfRequestSourcePage.Value != "ContactPage-ForcedEntry")
                        {
                            sDsc = lbTemp.Text.Trim().ToUpper();
                            if ((sDsc == "EXPRESS") || (sDsc == "DEPOT"))
                                sViaNeeded = "Y";
                            if (sSource == "1") 
                            {
                                sProductCode = ws_Get_B1PartProductCode(sPrt);

                                if ((sProductCode == "PTR") && ((sDsc == "EXPRESS") || (sDsc == "PER-INCIDENT")))
                                    sInterfaceNeeded = "Y";
                            }
                        }
                    }
                    // ----------------------------------------------------------
                    // Textboxes
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;
                        if (txTemp.ID.Contains("txProblemDescription"))
                        {
                            if (txTemp.Text == "")
                            {
                                if (!String.IsNullOrEmpty(sProblemError))
                                    sProblemError += "<br />";
                                sProblemError += "A problem description is required";

                                if (sValid == "")
                                {
                                    sValid = "INVALID";
                                    txTemp.Focus();
                                }
                            }
                        }
                        else if (txTemp.ID.Contains("txCrossRef"))
                        {
                            if (txTemp.Text == ""
                                && hfChosenCs1.Value == "79206" // Cinemark
                                                          //&& hfCs1.Value == "99999" // Test
                                )
                            {
                                if (sProblemError != "")
                                    sProblemError += "<br />";
                                sProblemError += "A ticket cross reference is required";

                                if (sValid == "")
                                {
                                    sValid = "INVALID";
                                    txTemp.Focus();
                                }
                            }
                        }
                        else if (txTemp.ID.Contains("txProblemModel") &&  hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                        {
                            if (String.IsNullOrEmpty(txTemp.Text.Trim()))
                            {
                                if (sProblemError != "")
                                    sProblemError += "<br />";
                                sProblemError += "A model name is required";

                                if (sValid == "")
                                {
                                    sValid = "INVALID";
                                    txTemp.Focus();
                                }
                            }
                        }
                        // ----------------------------------
                    }
                    // --------------------------------------------------
                    // Drop Down Lists
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        ddTemp = (DropDownList)c2;
                        if (ddTemp.ID.Contains("ddProblemServiceType") && hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                        {
                            sDsc = ddTemp.SelectedValue.ToUpper();
                            if ((sDsc == "EXPRESS") || (sDsc == "DEPOT"))
                                sViaNeeded = "Y";
                        }
                    }
                    // --------------------------------------------------
                    // Panels
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Panel"))
                    {
                        pnTemp = (Panel)c2;

                        if (pnTemp.ID.Contains("pnProblemError"))
                        {
                            pnProblemError = pnTemp;

                            //if (String.IsNullOrEmpty(sProblemError))
                            //    pnTemp.Visible = false;
                            //else
                            //    pnTemp.Visible = true;

                            foreach (Control c3 in pnTemp.Controls)
                            {
                                sControlType = c3.GetType().ToString();
                                // ---------------------------------------
                                if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                {
                                    lbTemp = (Label)c3;
                                    if (lbTemp.ID.Contains("lbProblemError"))
                                    {
                                        lbProblemError = lbTemp;
                                        //if (!String.IsNullOrEmpty(sProblemError))
                                        //    lbTemp.Text = sProblemError; // already visible...

                                        //// Clear workfields now that you've read the last important field on that record
                                        //sPrt = "";
                                        //sDsc = "";

                                        //sInterfaceNeeded = "";
                                        //sViaNeeded = "";
                                        //sProblemError = "";
                                    }
                                }
                                // ---------------------------------------
                            }
                        }
                        else if (pnTemp.ID.Contains("pnProblemInterface"))
                        {
                            if (sInterfaceNeeded == "Y")
                            {
                                foreach (Control c3 in pnTemp.Controls)
                                {
                                    sControlType = c3.GetType().ToString();
                                    // ---------------------------------------
                                    if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                                    {
                                        ddTemp = (DropDownList)c3;
                                        if (ddTemp.ID.Contains("ddProblemInterface"))
                                        {
                                            if (sInterfaceNeeded == "Y")
                                            {
                                                if (ddTemp.SelectedValue == "")
                                                {
                                                    if (sProblemError != "")
                                                        sProblemError += "<br />";
                                                    sProblemError += "The printer interface type is required";

                                                    if (sValid == "")
                                                    {
                                                        sValid = "INVALID";
                                                        ddTemp.Focus();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // ---------------------------------------
                                }
                            }
                        }
                        else if (pnTemp.ID.Contains("pnProblemVia"))
                        {
                            if (sViaNeeded == "Y")
                            {
                                // Trick is for forced entry if they change to EXPRESS or DEPOT, immediately show VIA
                                pnTemp.Visible = true;
                                foreach (Control c3 in pnTemp.Controls)
                                {
                                    sControlType = c3.GetType().ToString();
                                    // ---------------------------------------
                                    if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                                    {
                                        ddTemp = (DropDownList)c3;
                                        if (ddTemp.ID.Contains("ddProblemVia"))
                                        {
                                            if (sViaNeeded == "Y")
                                            {
                                                if (ddTemp.SelectedValue == "")
                                                {
                                                    if (sProblemError != "")
                                                        sProblemError += "<br />";
                                                    sProblemError += "A shipping method is required";

                                                    if (sValid == "")
                                                    {
                                                        sValid = "INVALID";
                                                        ddTemp.Focus();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // ---------------------------------------
                                }
                            }
                        }
                    } // End checking panel controls
                      // --------------------------------------------------------
                    // Hidden Fields
                    // If Face or Via missing, load validators
                    else if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID.Contains("hfProblemSource"))
                        {
                            sSource = hfTemp.Value;
                        }
                        else if (hfTemp.ID.Contains("hfProblemProcessor"))
                        {
                            if (String.IsNullOrEmpty(sProblemError))
                                pnProblemError.Visible = false;
                            else
                            {
                                pnProblemError.Visible = true;
                                lbProblemError.Text = sProblemError; 
                            }

                            // Clear workfields now that you've read the last important field on that record
                            sPrt = "";
                            sDsc = "";
                            sProductCode = "";
                            sSource = "";

                            sInterfaceNeeded = "";
                            sViaNeeded = "";
                            sProblemError = "";

                        }

                    }
                    // -----------------------------
                }
            }
        }
        // --------------------------------------
        if (sValid == "")
        {
            sValid = "VALID";
        }
        return sValid;
    }
    // ========================================================================
    #endregion // end repeaterRowProcessing
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchLocationSubmit_Click(object sender, EventArgs e)
    {
        Load_LocationDataTables();
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkLocationName_Click(object sender, EventArgs e)
    {
        Hide_Panels();

        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
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

            // Load Request Object
            // -------------------------------------------------
            serviceRequest.customerNumber = iCustomerNumber.ToString();
            serviceRequest.customerLocation = iCustomerLocation.ToString();

            Load_SelectedLocationLabel(iCustomerNumber, iCustomerLocation);

            Move_RequestObjectToHiddenFields();
            // -------------------------------------------------

            hfChosenCs1.Value = iCustomerNumber.ToString();
            hfChosenCs2.Value = iCustomerLocation.ToString();


            // If you have data, 
            if (iCustomerNumber > 0)
            {
                Load_ContactPanel();

                pnContact.Visible = true;
                pnSelected.Visible = true;
                pnSelectedLocation.Visible = true;
                pnSelectedContact.Visible = false;
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btContactSubmit_Click(object sender, EventArgs e)
    {
        string sTemp = "";
        string sPhn = "";
        lbMsgContact.Text = "";

        string sContactVerdict = Validate_ContactEntries();

        if (sContactVerdict != "VALID")
        {
            lbMsgContact.Text = sContactVerdict;
        }
        else
        {
            Hide_Panels();

            // Move entries to request object
            serviceRequest.contactName = txContactName.Text.Trim();
            serviceRequest.companyPhone = txCompanyPhone.Text.Trim();
            serviceRequest.companyPhoneExtension = txCompanyPhoneExtension.Text.Trim();
            serviceRequest.preferredTechContactMethod = ddContactMethodPreference.SelectedValue;
            serviceRequest.preferredTechContactDetail = txContactMethodDetail.Text.Trim();
            serviceRequest.creatorUsername = txContactRequestorName.Text;
            serviceRequest.emailForAcknowledgement = txContactEmailAcknowledgement.Text.Trim();

            sPhn = "";
            if (!String.IsNullOrEmpty(serviceRequest.companyPhone))
            {
                if (serviceRequest.companyPhone.Length == 10)
                    sPhn += FormatPhone1(serviceRequest.companyPhone);
            }

            sTemp = Fix_Case(txContactName.Text) + " - Company Phone: " + sPhn;

            if (ddContactMethodPreference.SelectedValue == "NON")
                sTemp += " - Will be contacted by company phone";
            else if (ddContactMethodPreference.SelectedValue == "EML")
                sTemp += " - Prefers contact by email: " + txContactMethodDetail.Text;
            else if (ddContactMethodPreference.SelectedValue == "TXT")
            {
                sPhn = Clean_PhoneEntry(txContactMethodDetail.Text.Trim());
                txContactMethodDetail.Text = sPhn;
                if (sPhn.Length == 10)
                {
                    sPhn = FormatPhone1(sPhn);
                    sTemp += " - Prefers contact by text: " + sPhn;
                }
            }
            else if (ddContactMethodPreference.SelectedValue == "PHN")
            {
                sPhn = Clean_PhoneEntry(txContactMethodDetail.Text.Trim());
                txContactMethodDetail.Text = sPhn;
                if (sPhn.Length == 10)
                {
                    sPhn = " - Prefers contact by alternate phone: " + FormatPhone1(sPhn);
                    if (!String.IsNullOrEmpty(txContactMethodPhoneExt.Text))
                        sPhn += " Ext: " + txContactMethodPhoneExt.Text;
                    sTemp += sPhn;
                }
            }

            lbSelectedContact.Text = sTemp;

            // Determine which screen to present next

            if (rbContactListForReg.Checked == true
                || rbContactListForPm.Checked == true
                )
            {
                serviceRequest.payingByAgrOrTM = "AGR";

                pnSelected.Visible = true;
                pnSelectedLocation.Visible = true;
                pnSelectedContact.Visible = true;

                pnEquipment.Visible = true;

                Load_EquipmentPanel();
            }
            else if (rbContactManualPayingByContract.Checked == true
                || rbContactManualPayingByTm.Checked == true
                )
            {
                if (rbContactManualPayingByTm.Checked == true)
                {
                    serviceRequest.payingByAgrOrTM = "TM";
                }
                else
                {
                    serviceRequest.payingByAgrOrTM = "AGR";
                }

                pnSelected.Visible = true;
                pnSelectedLocation.Visible = true;
                pnSelectedContact.Visible = true;

                pnProblem.Visible = true;

                // Need to load the problem panel with no parts but a quantity
                hfRequestSourcePage.Value = "ContactPage-ForcedEntry";
                Load_ProblemPanel();
            }
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchLocationClear_Click(object sender, EventArgs e)
    {
        txSearchLocationAddress.Text = "";
        txSearchLocationCity.Text = "";
        txSearchLocationName.Text = "";
        txSearchLocationNum.Text = "";
        txSearchLocationPhone.Text = "";
        txSearchLocationState.Text = "";
        txSearchLocationXref.Text = "";
        txSearchLocationZip.Text = "";
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchEquipmentSubmit_Click(object sender, EventArgs e)
    {
        ViewState["vsDataTable_Eqp"] = null;
        BindGrid_Eqp();
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchEquipmentClear_Click(object sender, EventArgs e)
    {
        txSearchEquipmentAgentId.Text = "";
        txSearchEquipmentEquipmentXref.Text = "";
        txSearchEquipmentModel.Text = "";
        txSearchEquipmentModelDescription.Text = "";
        txSearchEquipmentSerial.Text = "";
        
        ddSearchEquipmentCategory.SelectedValue = "";
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btEquipmentSubmit_Click(object sender, EventArgs e)
    {
        Hide_Panels();

        // Determine which screen to present next

        pnSelected.Visible = true;
        pnSelectedLocation.Visible = true;
        pnSelectedContact.Visible = true;
        pnSelectedEquipment.Visible = true;

        pnProblem.Visible = true;
        hfRequestSourcePage.Value = "EquipmentList";
        Load_ProblemPanel();

    }
    // ----------------------------------------------------------------------------------------------------
    protected void btProblemSubmit_Click(object sender, EventArgs e)
    {
        Hide_Panels();
        lbProblemContactError.Text = "";
        // Validate input
        
        string sProblemVerdict = Validate_Problem();
        string sPhone = "";

        // Validate Later Contact Entry
        if (
               hfRequestSourcePage.Value == "SerialList"
            || hfRequestSourcePage.Value == "AgrEqp"
            // hfRequestSourcePage.Value == "ContactPage-ForcedEntry"
            )
        {
            if (String.IsNullOrEmpty(txProblemContactName.Text))
            {
                lbProblemContactError.Text = "Contact Name is Required";
                txProblemContactName.Focus();
            }
            if (String.IsNullOrEmpty(txProblemContactPhone.Text))
            {
                if (!String.IsNullOrEmpty(lbProblemContactError.Text))
                    lbProblemContactError.Text += "<br />";
                lbProblemContactError.Text += "Contact Phone is Required";
                txProblemContactPhone.Focus();
            }
            
            sPhone = Clean_PhoneEntry(txProblemContactPhone.Text.Trim());
            txProblemContactPhone.Text = sPhone;

            if (sPhone.Length != 10)
            {
                if (!String.IsNullOrEmpty(lbProblemContactError.Text))
                    lbProblemContactError.Text += "<br />";
                lbProblemContactError.Text += "Phone entry must contain 10 numbers";
                txProblemContactPhone.Focus();
            }
        }

        if (sProblemVerdict != "VALID" || !String.IsNullOrEmpty(lbProblemContactError.Text))
        {
            pnProblem.Visible = true;
            pnSelected.Visible = true;
            pnSelectedLocation.Visible = true;

            // Already have loaded contact: just display
            if (hfRequestSourcePage.Value == "EquipmentList" 
                || hfRequestSourcePage.Value == "ContactPage-ForcedEntry")
                pnSelectedContact.Visible = true;
            
            // Bypassed standard contact entry: display secondary entry boxes
            if (hfRequestSourcePage.Value == "SerialList"
                || hfRequestSourcePage.Value == "AgrEqp") 
            {
                pnSelectedContactEntry.Visible = true;
                //pnSelectedContact.Visible = false;
            }
            
            if (hfRequestSourcePage.Value == "EquipmentList" 
                || hfRequestSourcePage.Value == "SerialList" 
                || hfRequestSourcePage.Value == "AgrEqp") 
                pnSelectedEquipment.Visible = true;
            

        }
        else 
        {
            GetSelectedProblemRecords_rp();  // Loops through the repeater and places requests one at a time
            Load_ResultPanel();  // Now show the ticket numbers that were created (or an error message)
            pnResult.Visible = true;
        }

        // Process Service Ticket
        // Where are all your values? Hidden Fields, on screen?

        // Determine which screen to present next


    }
    // ----------------------------------------------------------------------------------------------------
    protected void btSearchSerialOrAssetSubmit_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("");

        string sSerialOrAsset = txSearchSerialOrAsset.Text.Trim();
        lbSearchSerialOrAssetMsg.Text = "";
        int iPrimaryCustomer = 0;

        if (String.IsNullOrEmpty(sSerialOrAsset))
        {
            lbSearchSerialOrAssetMsg.Text = "An entry is required.";
        }
        else 
        {
            if (int.TryParse(hfPrimaryCs1.Value, out iPrimaryCustomer) == false)
                iPrimaryCustomer = -1;
            if (iPrimaryCustomer > 0)
            {
                try
                {
                    dt = ws_Get_B1UnitBySerialOrAsset(iPrimaryCustomer, sSerialOrAsset);
                    dt = Merge_SerialTables(dt);

                    ViewState["vsDataTable_Ser"] = null;
                    BindGrid_Ser(dt);

                    pnSerialRequestType.Visible = true;
                    pnSerialEquipmentMove.Visible = true;
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
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkSerialRequestPick_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[4];
        saArg = sParms.Split('|');

        int iCs1 = 0;
        int iCs2 = 0;
        int iUnt = 0;
        string sAgr = "";
        string sSrc = "";
        if (saArg.Length > 4) 
        {
            if (int.TryParse(saArg[0], out iCs1) == false)
                iCs1 = 0;
            if (int.TryParse(saArg[1], out iCs2) == false)
                iCs2 = 0;
            if (int.TryParse(saArg[2], out iUnt) == false)
                iUnt = 0;
            sAgr = saArg[3].Trim();
            sSrc = saArg[4].Trim();
        }

        if (iUnt > 0)
        {
            Hide_Panels();

            hfChosenCs1.Value = iCs1.ToString();
            hfChosenCs2.Value = iCs2.ToString();
            hfUnitList.Value = iUnt.ToString() + "~" + sAgr + "~" + sSrc;

            hfSerialPickRequestType.Value = "";
            if (rblRequestServiceType.SelectedValue == "PM")
                hfSerialPickRequestType.Value = "PM";

            Load_SelectedLocationLabel(iCs1, iCs2);

            pnSelected.Visible = true;
            pnSelectedLocation.Visible = true;  // Loaded right here
            pnSelectedContact.Visible = false; // need to load this on the next problem page
            pnSelectedContactEntry.Visible = true; 
            pnSelectedEquipment.Visible = true; // This will be loaded on the problem page as you parse the hfUnitList.Value

            pnProblem.Visible = true;
            hfRequestSourcePage.Value = "SerialList";
            Load_ProblemPanel();
        }
        else 
        {
            lbSearchSerialOrAssetMsg.Text = "Sorry!  The record selected did not carry some required values for service request. ";
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void lkSearchBySerial_Click(object sender, EventArgs e)
    {
        int iPrimaryCustomer = 0;
        DataTable dt = new DataTable("");

        try 
        {
            Hide_Panels();
            pnSerial.Visible = true;

            if (int.TryParse(hfPrimaryCs1.Value, out iPrimaryCustomer) == false)
                iPrimaryCustomer = -1;
            if (iPrimaryCustomer > 0) 
            {
                dt = ws_Get_B1PrimaryCustomerLocations(iPrimaryCustomer);
                if (dt.Rows.Count > 0) 
                {
                    ddSerialMoveLocation.DataSource = dt;
                    ddSerialMoveLocation.DataTextField = "TextField";
                    ddSerialMoveLocation.DataValueField = "ValueField";
                    ddSerialMoveLocation.DataBind();
                    ddSerialMoveLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
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
    protected void btSerialUpdateItemLocationSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lbSearchSerialOrAssetMsg.Text = "";

            int iCustomerNumber = 0;
            int iCustomerLocation = 0;
            string sContact = "";
            string sPhone = "";
            string sExtension = "";
            string[] saCs1Cs2 = { "" };

            if (!String.IsNullOrEmpty(ddSerialMoveLocation.SelectedValue)) 
            {
                saCs1Cs2 = ddSerialMoveLocation.SelectedValue.Split('~');
                if (saCs1Cs2.Length > 1)
                {
                    if (int.TryParse(saCs1Cs2[0], out iCustomerNumber) == false)
                        iCustomerNumber = -1;
                    if (int.TryParse(saCs1Cs2[1], out iCustomerLocation) == false)
                        iCustomerLocation = -1;
                }
            }

            sContact = txSerialMoveName.Text.Trim();
                sPhone = txSerialMovePhone.Text.Trim();
                sExtension = txSerialMoveExtension.Text.Trim();
                
                sPhone = Clean_PhoneEntry(sPhone);
                sExtension = Clean_PhoneEntry(sExtension);
            txSerialMovePhone.Text = sPhone;
            txSerialMoveExtension.Text = sExtension;
                
                // Pull items to move from repeater and grid view
                GetSelectedSerialRecords_gv(); // loading hfMoveList
                GetSelectedSerialRecords_rp();

            if (String.IsNullOrEmpty(ddSerialMoveLocation.SelectedValue))
            {
                lbSearchSerialOrAssetMsg.Text = "Please select a target location for the move";
                ddSerialMoveLocation.Focus();
            }
            else if (iCustomerNumber <= 0 || iCustomerLocation < 0)
            {
                lbSearchSerialOrAssetMsg.Text = "Please select a target location for the move";
                ddSerialMoveLocation.Focus();
            }
            else if (String.IsNullOrEmpty(sContact))
            {
                lbSearchSerialOrAssetMsg.Text = "A contact name is required";
                txSerialMoveName.Focus();
            }
            else if (String.IsNullOrEmpty(sPhone))
            {
                lbSearchSerialOrAssetMsg.Text = "A contact phone number is required";
                txSerialMovePhone.Focus();
            }
            else if (sPhone.Length != 10)
            {
                lbSearchSerialOrAssetMsg.Text = "Phone entry requires 10 numbers";
                txSerialMovePhone.Focus();
            }
            else if (String.IsNullOrEmpty(hfMoveList.Value))
            {
                lbSearchSerialOrAssetMsg.Text = "To update the item location, at least one item must be selected ";
                txSerialMovePhone.Focus();
            }
            else // All is well, send email 
            {
                Send_MoveEmail(iCustomerNumber, iCustomerLocation, sContact, sPhone, sExtension, hfMoveList.Value);
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
    protected void btSubmitAnotherRequest_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        Response.Redirect("~/private/sc/ServiceRequest.aspx");
    }
    // ----------------------------------------------------------------------------------------------------
    protected void btViewMyOpenTickets_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        Response.Redirect("~/private/sc/OpenTickets.aspx");
    }
    // ----------------------------------------------------------------------------------------------------
    protected void ddContactMethodPreference_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbContactMethodDetailTitle.Visible = false;
        lbContactMethodDetailExtentionTitle.Visible = false;
        txContactMethodDetail.Visible = false;
        txContactMethodPhoneExt.Visible = false;
        txContactMethodDetail.Width = 300;

        txContactMethodDetail.Text = "";
        txContactMethodPhoneExt.Text = "";

        if (ddContactMethodPreference.SelectedValue == "NON")
        {
        }
        else if (ddContactMethodPreference.SelectedValue == "EML")
        {
            lbContactMethodDetailTitle.Visible = true;
            txContactMethodDetail.Visible = true;
            lbContactMethodDetailTitle.Text = "Address ";
        }
        else if (ddContactMethodPreference.SelectedValue == "TXT")
        {
            lbContactMethodDetailTitle.Visible = true;
            txContactMethodDetail.Visible = true;
            lbContactMethodDetailTitle.Text = "Mobile ";
        }
        else if (ddContactMethodPreference.SelectedValue == "PHN")
        {
            lbContactMethodDetailTitle.Visible = true;
            txContactMethodDetail.Visible = true;
            lbContactMethodDetailTitle.Text = "Phone ";
            txContactMethodDetail.Width = 200;

            lbContactMethodDetailExtentionTitle.Visible = true;
            txContactMethodPhoneExt.Visible = true;
            lbContactMethodDetailExtentionTitle.Text = "Ext? ";
        }

    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
