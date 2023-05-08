using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

public partial class private_sc_AgreementEquipment : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";

    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";

        if (!IsPostBack)
        {
            int iCustomerNumber = 0;
            int iCustomerLocation = 0;
            int iAgreementNumber = 0;

            // Retrieve parms to know which program (Agreement Locs, or Agreements) called this page and the equipment to load
            hfPassedSrc.Value = "";
            hfPassedCs1.Value = "";
            hfPassedCs2.Value = "";
            hfPassedAgr.Value = "";

            if (Request.QueryString["src"] != null && Request.QueryString["src"].ToString() != "")
                hfPassedSrc.Value = Request.QueryString["src"].ToString().Trim();
            if (Request.QueryString["cs1"] != null && Request.QueryString["cs1"].ToString() != "")
                hfPassedCs1.Value = Request.QueryString["cs1"].ToString().Trim();
            if (Request.QueryString["cs2"] != null && Request.QueryString["cs2"].ToString() != "")
                hfPassedCs2.Value = Request.QueryString["cs2"].ToString().Trim();
            if (Request.QueryString["agr"] != null && Request.QueryString["agr"].ToString() != "") 
            {
                
                if (int.TryParse(Request.QueryString["agr"].ToString().Trim(), out iAgreementNumber) == false)
                    iAgreementNumber = 0;
                if (iAgreementNumber > 0)
                    hfPassedAgr.Value = iAgreementNumber.ToString("00000000");
            }

            Get_UserPrimaryCustomerNumber();

            try
            {
                // Load Product Code DropDown in Equipment Search Table?
                pnSearchProductCodes.Visible = false; // Initialize to hidden, then check below if passed parms require it
                if (hfPassedSrc.Value == "1")
                {
                    if (!String.IsNullOrEmpty(hfPassedAgr.Value)) 
                    {
                        LoadB1AgreementProductCodes(hfPassedAgr.Value);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(hfPassedCs1.Value))
                        {
                            if (int.TryParse(hfPassedCs1.Value, out iCustomerNumber) == false)
                                iCustomerNumber = -1;
                        }
                        else if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
                        {
                            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                            iCustomerNumber = -1;
                        }
                        
                        if (int.TryParse(hfPassedCs2.Value, out iCustomerLocation) == false)
                            iCustomerLocation = -1;
                        
                        if (iCustomerNumber > 0 && iCustomerLocation > -1) 
                        {
                            string sOr1Or2 = ws_Get_B1CustomerOracleIds(iCustomerNumber, iCustomerLocation);
                            string[] saOr1Or2 = sOr1Or2.Split('|');
                            if (saOr1Or2.Length > 1) 
                            {
                                hfOracleParentId.Value = saOr1Or2[0];
                                hfOracleChildId.Value = saOr1Or2[1];
                            }
                                
                            LoadB1LocationProductCodes(iCustomerNumber.ToString(), iCustomerLocation.ToString());
                        }
                    }
                }
                string sIsExcelVersionNeeded = "";
                LoadEquipmentDataTables(sIsExcelVersionNeeded);

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


    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================

    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1Agreements(int customerNumber, int agreementNumber)
    {
        DataTable dt = new DataTable();
        
        if (customerNumber > 0)
        {
            string sJobName = "Get_B1Agreements";
            string sFieldList = "customerNumber|agreementNumber|x";
            string sValueList = customerNumber.ToString() + "|" + agreementNumber.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected string ws_Get_B1CustomerOracleId(int customerNumber)
    {
        string sOracleId = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustomerOracleId";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sOracleId = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sOracleId;
    }
    // ---------------------------------------------------------------------------------------
    protected string ws_Get_B1CustomerOracleIds(int customerNumber, int customerLocation)
    {
        string sOracleIds = "";

        if (customerNumber > 0 && customerLocation > -1)
        {
            string sJobName = "Get_B1CustomerOracleIds";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber.ToString() + "|" + customerLocation.ToString() +  "|x";

            sOracleIds = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sOracleIds;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2Agreements(int customerNumber, int agreementNumber)
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0)
        {
            string sJobName = "Get_B2Agreements";
            string sFieldList = "customerNumber|agreementNumber|x";
            string sValueList = customerNumber.ToString() + "|" + agreementNumber.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected string ws_Upd_B1EquipmentCrossRef(string equipmentCrossRef, int unit) 
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(equipmentCrossRef) && unit > 0)
        {
            string sJobName = "Upd_B1EquipmentCrossRef";
            string sFieldList = "equipmentCrossRef|unit|x";
            string sValueList = equipmentCrossRef.Trim() + "|" + unit + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1AgreementEquipment(
        string agreementNumber,
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

        if (!String.IsNullOrEmpty(agreementNumber))
        {
            string sJobName = "Get_B1AgreementEquipment";
            string sFieldList = "agreementNumber|productCode|model|serial|modelDescription|asset|agentId|downloadExcelY|x";
            string sValueList =
                agreementNumber + "|" +
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
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2AgreementEquipment(
        int agreementNumber,
        string model,
        string serial,
        string modelDescription
        )
    {
        DataTable dt = new DataTable("");

        if (agreementNumber > 0)
        {
            string sJobName = "Get_B2AgreementEquipment";
            string sFieldList = "agreementNumber|model|serial|modelDescription|x";
            string sValueList =
                agreementNumber + "|" +
                model + "|" +
                serial + "|" +
                modelDescription + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2LocationEquipment(
        string oracleParentId,
        string oracleChildId,
        string model,
        string serial,
        string modelDescription
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(oracleParentId) && !String.IsNullOrEmpty(oracleChildId))
        {
            string sJobName = "Get_B2LocationEquipment";
            string sFieldList = "oracleParentId|oracleChildId|model|serial|modelDescription|x";
            string sValueList =
                oracleParentId + "|" +
                oracleChildId + "|" +
                model + "|" +
                serial + "|" +
                modelDescription + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1AgreementProductCodes(
        string agreement
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(agreement))
        {
            string sJobName = "Get_B1AgreementProductCodes";
            string sFieldList = "agreement|x";
            string sValueList =
                agreement + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1LocationProductCodes(
        string customerNumber,
        string customerLocation
        )
    {
        DataTable dt = new DataTable("");

        if (String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1LocationProductCodes";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList =
                customerNumber + "|" +
                customerLocation + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------------
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
    // ---------------------------------------------------------------------------------------
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
    #region tableSortHandler
    // ========================================================================
    // ========================================================================
    // BEGIN: Location Table (_Eqp)
    // ========================================================================
    protected void BindGrid_Eqp(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_, but...
        // a) because you HAVE to load both LARGE screen and a SMALL screen tables
        // b) becasue you are loading the same grid view with data from different queries (based on calling program)
        // You have to retrieve the datatable anyway (for any change requiring a new reload)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Eqp = "";

        if (ViewState["vsDataTable_Eqp"] == null)
        {
            lbMsg.Text = "";
            //int iCustomerNumber = 0;
            //if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
            //    iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching equipment was found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Eqp"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties

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
        gv_EquipmentLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Eqp(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_EquipmentLarge.PageIndex = newPageIndex;
        DataTable dt = null; // KOI you're going to have problems paging wiping out the data table here...
        BindGrid_Eqp(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Eqp(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Eqp == SortDirection.Ascending)
                gridSortDirection_Eqp = SortDirection.Descending;
            else
                gridSortDirection_Eqp = SortDirection.Ascending;
        }
        else
            gridSortDirection_Eqp = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Eqp = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Eqp(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Eqp
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Eqp"] == null)
            {
                ViewState["GridSortDirection_Eqp"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Eqp"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Eqp"];
        }
        set
        {
            ViewState["GridSortDirection_Eqp"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Eqp
    {
        get
        {
            if (ViewState["GridSortExpression_Eqp"] == null)
            {
                ViewState["GridSortExpression_Eqp"] = "Model"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Eqp"];
        }
        set
        {
            ViewState["GridSortExpression_Eqp"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Equipment Table (_Eqp)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // -------------------------------------------------------------------------------------------------
    protected void LoadEquipmentDataTables(string isExcelVersionNeeded) 
    {
        string sSource = "";
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
        int iAgreementNumber = 0;

        sSource = hfPassedSrc.Value.Trim();

        if (!String.IsNullOrEmpty(hfPassedCs1.Value))
        {
            if (int.TryParse(hfPassedCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }
        else if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
        {
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }


        if (!String.IsNullOrEmpty(hfPassedCs2.Value))
        {
            if (int.TryParse(hfPassedCs2.Value, out iCustomerLocation) == false)
                iCustomerLocation = -1;
        }

        if (!String.IsNullOrEmpty(hfPassedAgr.Value))
        {
            if (int.TryParse(hfPassedAgr.Value, out iAgreementNumber) == false)
                iAgreementNumber = -1;
        }
        // --------------------------------
        if (iAgreementNumber > 0)
        {
            LoadEquipmentDataTablesForAgreement(hfPassedAgr.Value, hfPassedSrc.Value, isExcelVersionNeeded);
        }
        else if (iCustomerNumber > 0 && iCustomerLocation > -1) 
        {
            LoadEquipmentDataTablesForLocation(iCustomerNumber, iCustomerLocation, isExcelVersionNeeded);
        }

    }
    // -------------------------------------------------------------------------------------------------
    protected void LoadEquipmentDataTablesForAgreement(string agreement, string source, string isExcelVersionNeeded)
    {
        DataTable dt = new DataTable();
        DataTable dtB1 = new DataTable();
        DataTable dtB2 = new DataTable();

        string sExcelVersionCreated = "";

        string sProductCode = ddSearchProductCodes.SelectedValue.ToString().Trim();
        string sModel = txSearchModel.Text.Trim().ToUpper();
        string sSerial = txSearchSerial.Text.Trim().ToUpper();
        string sModelDescription = txSearchModelDescription.Text.Trim().ToUpper();
        string sAsset = txSearchAsset.Text.Trim().ToUpper();
        string sAgentId = txSearchAgentId.Text.Trim().ToUpper();
        int iAgreement = 0;
        if (int.TryParse(agreement, out iAgreement) == false)
            iAgreement = -1;

        try 
        {
            if (!String.IsNullOrEmpty(hfPassedAgr.Value))
            {
                if (iAgreement > 0)
                    lbEquipmentSelectionTitle.Text = "Equipment (from agreement " + iAgreement.ToString() + ")";
                else
                    lbEquipmentSelectionTitle.Text = "Equipment";

                if (source == "1")
                {
                    dtB1 = ws_Get_B1AgreementEquipment(
                        iAgreement.ToString("00000000"),
                        sProductCode,
                        sModel,
                        sSerial,
                        sModelDescription,
                        sAsset,
                        sAgentId,
                        isExcelVersionNeeded
                        );
                }
                else
                {
                    dtB2 = ws_Get_B2AgreementEquipment(
                        iAgreement,
                        //sProductCode,
                        sModel,
                        sSerial,
                        sModelDescription
                        //sAsset,
                        //sAgentId,
                        //isExcelVersionNeeded
                        );
                }

                dt = Merge_EquipmentTablesForAgreements(dtB1, dtB2);

                if (dt.Rows.Count > 0)
                {
                    if (isExcelVersionNeeded == "Y")
                    {
                        // Remove unnecessary columns from Excel
                        dt.Columns.Remove("Source");
                        dt.Columns.Remove("UnitSort");

                        dt.AcceptChanges();

                        // Rename columns
                        dt.Columns["AgentId"].ColumnName = "AgentIdX";
                        dt.Columns["Agreement"].ColumnName = "AgreementX";
                        dt.Columns["Model"].ColumnName = "ModelX";
                        dt.Columns["ModelDescription"].ColumnName = "ModelDescriptionX";
                        dt.Columns["ModelXref"].ColumnName = "ModelXrefX";
                        dt.Columns["Serial"].ColumnName = "SerialX";
                        dt.Columns["Unit"].ColumnName = "UnitX";

                        dt.AcceptChanges();

                        // Add Columns for the Excel Order
                        
                        dt.Columns.Add(MakeColumn("Agreement"));
                        dt.Columns.Add(MakeColumn("Model"));
                        dt.Columns.Add(MakeColumn("ModelDescription"));
                        dt.Columns.Add(MakeColumn("ModelCrossRef"));
                        dt.Columns.Add(MakeColumn("Serial"));
                        dt.Columns.Add(MakeColumn("Unit"));
                        dt.Columns.Add(MakeColumn("AgentId"));

                        dt.AcceptChanges();

                        foreach (DataRow row in dt.Rows)
                        {
                            row["Agreement"] = row["AgreementX"].ToString().Trim();
                            row["Model"] = row["ModelX"].ToString().Trim();
                            row["ModelDescription"] = row["ModelDescriptionX"].ToString().Trim();
                            row["ModelCrossRef"] = row["ModelXrefX"].ToString().Trim();
                            row["Serial"] = row["SerialX"].ToString().Trim();
                            row["Unit"] = row["UnitX"].ToString().Trim();
                            row["AgentId"] = row["AgentIdX"].ToString().Trim();
                        }

                        dt.AcceptChanges();

                        // Remove All Original Columns (Leaving properly sorted columns)
                        dt.Columns.Remove("AgreementX");
                        dt.Columns.Remove("ModelX");
                        dt.Columns.Remove("ModelDescriptionX");
                        dt.Columns.Remove("ModelXrefX");
                        dt.Columns.Remove("SerialX");
                        dt.Columns.Remove("UnitX");
                        dt.Columns.Remove("AgentIdX");

                        dt.AcceptChanges();

                        // Rename columns so uppercase converts to mixed case as typed
                        dt.Columns["Agreement"].ColumnName = "Agreement";
                        dt.Columns["Model"].ColumnName = "Model";
                        dt.Columns["ModelDescription"].ColumnName = "Model Description";
                        dt.Columns["ModelCrossRef"].ColumnName = "Model Cross Ref";
                        dt.Columns["Serial"].ColumnName = "Serial";
                        dt.Columns["Unit"].ColumnName = "Unit";
                        dt.Columns["AgentId"].ColumnName = "Agent Id";

                        dt.TableName = "AgrEqp_" + hfPassedAgr.Value + "_" + DateTime.Now.ToString("yyyyMMdd_hh-mm");
                        DownloadHandler dh = new DownloadHandler();
                        string sCsv = dh.DataTableToExcelCsv(dt);
                        dh = null;

                        Response.ClearContent();
                        Response.ContentType = "application/ms-excel";
                        Response.AddHeader("content-disposition", "attachment; filename=AgrEqp_" + hfPassedAgr.Value + "_" + DateTime.Now.ToString("yyyyMMdd_hh-mm") + ".csv");
                        Response.Write(sCsv);

                        sExcelVersionCreated = "Y";
                    }
                    else
                    {
                        rp_EquipmentSmall.DataSource = dt;
                        rp_EquipmentSmall.DataBind();

                        ViewState["vsDataTable_Eqp"] = null;
                        BindGrid_Eqp(dt);

                        FormatRowsIn_Repeater();
                        FormatRowsIn_GridView();

                    }
                }

                // not sure how to show/hide with repeaters yet...
                /*
                // Show/hide edit columns based on user type
                if (User.IsInRole("Administrator") || User.IsInRole("Editor") || (User.IsInRole("EditorCustomer") && hfXrefEqpEditor.Value == "Y"))
                {
                    gvEquipment.Columns[4].Visible = true;
                }
                else
                    gvEquipment.Columns[4].Visible = false;
                */
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

        if (sExcelVersionCreated == "Y")
            Response.End();

    }
    // -------------------------------------------------------------------------------------------------
    protected void LoadEquipmentDataTablesForLocation(int customerNumber, int customerLocation, string isExcelVersionNeeded)
    {
        DataTable dtB1 = new DataTable("B1");
        DataTable dtB2 = new DataTable("B2");
        DataTable dt = new DataTable("Combined");

        string sExcelVersionCreated = "";

        string sProductCode = ddSearchProductCodes.SelectedValue.ToString().Trim();
        string sModel = txSearchModel.Text.Trim().ToUpper();
        string sSerial = txSearchSerial.Text.Trim().ToUpper();
        string sModelDescription = txSearchModelDescription.Text.Trim().ToUpper();
        string sAsset = txSearchAsset.Text.Trim().ToUpper();
        string sAgentId = txSearchAgentId.Text.Trim().ToUpper();

        try
        {
            if (customerNumber > 0)
            {
                LoadLocationHeader(customerNumber, customerLocation);

                dtB1 = ws_Get_B1LocationEquipment(
                    customerNumber,
                    customerLocation,
                    sProductCode,
                    sModel,
                    sSerial,
                    sModelDescription,
                    sAsset,
                    sAgentId,
                    isExcelVersionNeeded
                    );

                if (!String.IsNullOrEmpty(hfOracleParentId.Value) && !String.IsNullOrEmpty(hfOracleChildId.Value))
                {
                    dtB2 = ws_Get_B2LocationEquipment(
                        hfOracleParentId.Value,
                        hfOracleChildId.Value,
                        //sProductCode,
                        sModel,
                        sSerial,
                        sModelDescription
                        //sAsset,
                        //sAgentId,
                        //isExcelVersionNeeded
                        );
                }

                // Do you want to exclude BL2 locations from BL1? (But you have combo...)
                dt = Merge_EquipmentTablesForLocations(dtB1, dtB2);

                if (dt.Rows.Count > 0)
                {
                    if (isExcelVersionNeeded == "Y")
                    {
                        // Remove unnecessary columns from Excel
                        dt.Columns.Remove("Source");

                        dt.AcceptChanges();

                        // Rename columns
                        dt.Columns["AgentId"].ColumnName = "AgentIdX";
                        dt.Columns["Agreement"].ColumnName = "AgreementX";
                        dt.Columns["Model"].ColumnName = "ModelX";
                        dt.Columns["ModelDescription"].ColumnName = "ModelDescriptionX";
                        dt.Columns["ModelXref"].ColumnName = "ModelXrefX";
                        dt.Columns["Serial"].ColumnName = "SerialX";
                        dt.Columns["Unit"].ColumnName = "UnitX";

                        dt.AcceptChanges();

                        // Add Columns for the Excel Order

                        dt.Columns.Add(MakeColumn("Agreement"));
                        dt.Columns.Add(MakeColumn("Model"));
                        dt.Columns.Add(MakeColumn("ModelDescription"));
                        dt.Columns.Add(MakeColumn("ModelCrossRef"));
                        dt.Columns.Add(MakeColumn("Serial"));
                        dt.Columns.Add(MakeColumn("Unit"));
                        dt.Columns.Add(MakeColumn("AgentId"));

                        dt.AcceptChanges();

                        foreach (DataRow row in dt.Rows)
                        {
                            row["Agreement"] = row["AgreementX"].ToString().Trim();
                            row["Model"] = row["ModelX"].ToString().Trim();
                            row["ModelDescription"] = row["ModelDescriptionX"].ToString().Trim();
                            row["ModelCrossRef"] = row["ModelXrefX"].ToString().Trim();
                            row["Serial"] = row["SerialX"].ToString().Trim();
                            row["Unit"] = row["UnitX"].ToString().Trim();
                        }

                        dt.AcceptChanges();

                        // Remove All Original Columns (Leaving properly sorted columns)
                        dt.Columns.Remove("AgreementX");
                        dt.Columns.Remove("ModelX");
                        dt.Columns.Remove("ModelDescriptionX");
                        dt.Columns.Remove("ModelXrefX");
                        dt.Columns.Remove("SerialX");
                        dt.Columns.Remove("UnitX");
                        dt.Columns.Remove("AgentIdX");

                        dt.AcceptChanges();

                        // Rename columns so uppercase converts to mixed case as typed
                        dt.Columns["Agreement"].ColumnName = "Agreement";
                        dt.Columns["Model"].ColumnName = "Model";
                        dt.Columns["ModelDescription"].ColumnName = "Model Description";
                        dt.Columns["ModelCrossRef"].ColumnName = "Model Cross Ref";
                        dt.Columns["Serial"].ColumnName = "Serial";
                        dt.Columns["Unit"].ColumnName = "Unit";
                        dt.Columns["AgentId"].ColumnName = "AgentId";

                        dt.TableName = "AgrEqp" + "_" + customerNumber.ToString() + "-" + customerLocation.ToString();
                        DownloadHandler dh = new DownloadHandler();
                        string sCsv = dh.DataTableToExcelCsv(dt);
                        dh = null;

                        Response.ClearContent();
                        Response.ContentType = "application/ms-excel";
                        Response.AddHeader("content-disposition", "attachment; filename=AgrEqp_" + customerNumber.ToString() + "-" + customerLocation.ToString() + ".csv");
                        Response.Write(sCsv);

                        sExcelVersionCreated = "Y";
                    }
                    else
                    {
                        rp_EquipmentSmall.DataSource = dt;
                        rp_EquipmentSmall.DataBind();

                        ViewState["vsDataTable_Eqp"] = null;
                        BindGrid_Eqp(dt);

                        FormatRowsIn_Repeater();
                        FormatRowsIn_GridView();

                    }
                }

                // not sure how to show/hide with repeaters yet...
                /*
                // Show/hide edit columns based on user type
                if (User.IsInRole("Administrator") || User.IsInRole("Editor") || (User.IsInRole("EditorCustomer") && hfXrefEqpEditor.Value == "Y"))
                {
                    gvEquipment.Columns[4].Visible = true;
                }
                else
                    gvEquipment.Columns[4].Visible = false;
                */
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

        if (sExcelVersionCreated == "Y")
            Response.End();

    }
    // -------------------------------------------------------------------------------------------------
    protected void LoadLocationHeader(
        int customerNumber,
        int customerLocation)
    {
        DataTable dt = new DataTable();

        pnLocationHeader.Visible = true;
        lbLocationName.Text = "";
        lbLocationAddress.Text = "";
        lbLocationId.Text = "";
        lbLocationContact.Text = "";

        if (customerNumber > 0)
        {
            dt = ws_Get_B1CustomerLocationDetail(
                customerNumber.ToString(),
                customerLocation.ToString(),
                "", // sCustomerName
                "", // sContact
                "", // Address
                "", // City
                "", // State
                "", // Zip
                "", // Phone
                ""); // Xref

            if (dt.Rows.Count > 0)
            {
                lbLocationName.Text = dt.Rows[0]["CUSTNM"].ToString().Trim();
                sTemp = dt.Rows[0]["SADDR1"].ToString().Trim();
                if (!String.IsNullOrEmpty(dt.Rows[0]["SADDR1"].ToString().Trim()))
                    sTemp += " - " + dt.Rows[0]["SADDR2"].ToString().Trim();
                sTemp += " " + dt.Rows[0]["CITY"].ToString().Trim();
                if (!String.IsNullOrEmpty(sTemp))
                    sTemp += ", " + dt.Rows[0]["STATE"].ToString().Trim() + " " + dt.Rows[0]["ZIPCD"].ToString().Trim();
                lbLocationAddress.Text = sTemp;
                lbLocationId.Text = dt.Rows[0]["CSTRNR"].ToString().Trim() + "-" + dt.Rows[0]["CSTRCD"].ToString().Trim();
                lbLocationContact.Text = dt.Rows[0]["CONTNM"].ToString().Trim() + " " + FormatPhone1(dt.Rows[0]["HPHONE"].ToString().Trim());
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable Merge_EquipmentTablesForAgreements(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("AgentId"));
        dt.Columns.Add(MakeColumn("Agreement"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("ModelXref"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("Source"));
        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("UnitSort"));

        DataRow dr;
        int iRowIdx = 0;
        int iTemp = 0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["AgentId"] = Fix_Case(row["AgentId"].ToString()).Trim();
            dt.Rows[iRowIdx]["Agreement"] = row["Agreement"].ToString().Trim();
            dt.Rows[iRowIdx]["Model"] = Fix_Case(row["Model"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelXref"] = Fix_Case(row["ModelXref"].ToString().Trim());
            dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim().ToUpper();

            if (int.TryParse(row["Unit"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0) 
            {
                dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");
            }
            dt.Rows[iRowIdx]["UnitSort"] = iTemp.ToString("000000000");

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["AgentId"] = "";
            dt.Rows[iRowIdx]["Agreement"] = row["agreementId"].ToString().Trim();
            dt.Rows[iRowIdx]["Model"] = Fix_Case(row["product-identifier"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["description"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelXref"] = "";
            dt.Rows[iRowIdx]["Serial"] = row["serialNumber"].ToString().Trim().ToUpper();

            if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0) 
            {
                dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");
                
            }
            dt.Rows[iRowIdx]["UnitSort"] = iTemp.ToString("000000000");


            dt.Rows[iRowIdx]["Source"] = "2";

            iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable Merge_EquipmentTablesForLocations(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("AgentId"));
        dt.Columns.Add(MakeColumn("Agreement"));
        dt.Columns.Add(MakeColumn("Model"));
        dt.Columns.Add(MakeColumn("ModelDescription"));
        dt.Columns.Add(MakeColumn("ModelXref"));
        dt.Columns.Add(MakeColumn("Serial"));
        dt.Columns.Add(MakeColumn("Source"));
        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("UnitSort"));

        DataRow dr;
        int iRowIdx = 0;
        int iTemp = 0;

        foreach (DataRow row in dt1.Rows)
        {
            if (row["AgrType"].ToString().Trim() == "CW") 
            { 
                // for now, query the As400 connectwise records, but exclude them from the merged table display
            }
            else
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                dt.Rows[iRowIdx]["AgentId"] = Fix_Case(row["AgentId"].ToString()).Trim();
                dt.Rows[iRowIdx]["Agreement"] = row["Agreement"].ToString().Trim();
                dt.Rows[iRowIdx]["Model"] = Fix_Case(row["Model"].ToString().Trim());
                dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["ModelDescription"].ToString().Trim());
                dt.Rows[iRowIdx]["ModelXref"] = Fix_Case(row["ModelXref"].ToString().Trim());
                dt.Rows[iRowIdx]["Serial"] = row["Serial"].ToString().Trim().ToUpper();

                if (int.TryParse(row["Unit"].ToString().Trim(), out iTemp) == false)
                    iTemp = -1;
                if (iTemp > 0)
                    dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

                dt.Rows[iRowIdx]["UnitSort"] = iTemp.ToString("000000000");

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
            dt.Rows[iRowIdx]["Model"] = Fix_Case(row["product-identifier"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["description"].ToString().Trim());
            dt.Rows[iRowIdx]["ModelXref"] = "";
            dt.Rows[iRowIdx]["Serial"] = row["serialNumber"].ToString().Trim().ToUpper();

            if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0)
                dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

            dt.Rows[iRowIdx]["UnitSort"] = iTemp.ToString("000000000");

            dt.Rows[iRowIdx]["Source"] = "2";

            iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
    // -------------------------------------------------------------------------------------------------
    protected void LoadB1AgreementProductCodes(string agreement)
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
        {
            dt = ws_Get_B1AgreementProductCodes(
                agreement);
        }

        if (dt.Rows.Count > 0)
        {
            ddSearchProductCodes.DataSource = dt;

            ddSearchProductCodes.DataValueField = "ProductCode";
            ddSearchProductCodes.DataTextField = "ProductCode";
            ddSearchProductCodes.DataBind();
            ddSearchProductCodes.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
            pnSearchProductCodes.Visible = true;
        }
        else
        {
            pnSearchProductCodes.Visible = false;
        }
    }

    // -------------------------------------------------------------------------------------------------
    protected void LoadB1LocationProductCodes(string customerNumber, string customerLocation)
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
        {
            dt = ws_Get_B1LocationProductCodes(
                customerNumber,
                customerLocation);
        }

        if (dt.Rows.Count > 0) 
        {
            ddSearchProductCodes.DataSource = dt;

            ddSearchProductCodes.DataValueField = "ProductCode";
            ddSearchProductCodes.DataTextField = "ProductCode";
            ddSearchProductCodes.DataBind();
            ddSearchProductCodes.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
            pnSearchProductCodes.Visible = true;
        }
        else 
        {
            pnSearchProductCodes.Visible = false;
        }

    }
    // -------------------------------------------------------------------------------------------------
    protected void FormatRowsIn_Repeater()
    {
        HiddenField hfTemp = new HiddenField();
        Panel pnTemp = new Panel();

        string sType = "";
        string sSource = "";

        foreach (Control c1 in rp_EquipmentSmall.Controls)
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
                        if (hfTemp.ID == "hfSource")
                            sSource = hfTemp.Value;
                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Panel"))
                    {
                        pnTemp = (Panel)c2;
                        if (pnTemp.ID == "pnEditXref")
                        {
                            if (hfPreferenceToAllowEquipmentCrossRefUpdate.Value == "Y" && sSource == "1")
                                pnTemp.Visible = true;
                            else
                                pnTemp.Visible = false;

                            // Now that you're done here, initialize your flag field for next loop
                            sSource = "";

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

        try
        {
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
                                        // ==========================================================================
                                        sType = c4.GetType().ToString();
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                                        {
                                            hfTemp = (HiddenField)c4;
                                            if (hfTemp.ID == "hfSource")
                                            {
                                                sSource = hfTemp.Value;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                                        {
                                            lkTemp = (LinkButton)c4;
                                            if (lkTemp.ID == "lkLoadEquipXrefForEdit")
                                            {
                                                if (hfPreferenceToAllowEquipmentCrossRefUpdate.Value == "Y" && sSource == "1")
                                                    lkTemp.Visible = true;
                                                else
                                                    lkTemp.Visible = false;

                                                // Now that you're done here, initialize your flag field for next loop
                                                sSource = "";
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
                hfPreferenceToAllowEquipmentCrossRefUpdate.Value = ws_Get_B1CustPref_AllowEquipmentCrossRefUpdate_YN(iCustomerNumber);
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // =========================================================
    protected void btEquipmentSearchSubmit_Click(object sender, EventArgs e)
    {
        string sIsExcelNeeded = "";
        LoadEquipmentDataTables(sIsExcelNeeded);
    }
    // -------------------------------------------------------------------------------------------------
    protected void btEquipmentExcelSubmit_Click(object sender, EventArgs e)
    {
        string sIsExcelNeeded = "Y";

        LoadEquipmentDataTables(sIsExcelNeeded);
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkModelForTicket_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[1];
        saArg = sParms.Split('|');
        int iUnt = 0;
        int iCs1 = 0;
        int iCs2 = 0;
        string sAgr = "";
        string sSrc = "";
        string sUrl = "";


        DataTable dt;

        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
        string sCustomerNumber = "";
        string sCustomerLocation = "";

        if (!String.IsNullOrEmpty(hfPassedCs1.Value))
        {
            if (int.TryParse(hfPassedCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }
        else if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
        {
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }

        if (!String.IsNullOrEmpty(hfPassedCs2.Value))
        {
            if (int.TryParse(hfPassedCs2.Value, out iCustomerLocation) == false)
                iCustomerLocation = -1;
        }

        if (saArg.Length > 2) 
        {
            if (int.TryParse(saArg[0], out iUnt) == false)
                iUnt = 0;
            sAgr = saArg[1].Trim();
            sSrc = saArg[2].Trim();
        }

        if (iUnt > 0 && !String.IsNullOrEmpty(sAgr))
        {
            dt = ws_Get_B1DetailForSelectedUnits(iUnt.ToString(), sAgr);
            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["CustomerNumber"].ToString().Trim(), out iCs1) == false)
                    iCs1 = -1;
                if (int.TryParse(dt.Rows[0]["CustomerLocation"].ToString().Trim(), out iCs2) == false)
                    iCs2 = -1;
                if (iCs1 > 0 && iCs2 > -1)
                {
                    sCustomerNumber = iCs1.ToString();
                    sCustomerLocation = iCs2.ToString();
                }
            }
            else 
            {
                sCustomerNumber = iCustomerNumber.ToString();
                sCustomerLocation = hfPassedCs2.Value;
            }

            sUrl = "~/private/sc/ServiceRequest.aspx" +
                "?cs1=" + sCustomerNumber +
                "&cs2=" + sCustomerLocation +
                "&unt=" + iUnt.ToString() +
                "&agr=" + sAgr +
                "&pag=" + "AgrEqp" +
                "&src=" + sSrc;

            Response.Redirect(sUrl, false);
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkLoadEquipXrefForEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[4];
        saArg = sParms.Split('|');

        string sModel = saArg[0].Trim();
        string sSerial = saArg[1].Trim();
        int iUnit = 0;
        if (int.TryParse(saArg[2], out iUnit) == false)
            iUnit = 0;
        string sXref = saArg[3].Trim();

        // If you have data, 
        if (!String.IsNullOrEmpty(sModel))
        {
            lbEquipXrefUpdate_Model.Text = sModel;
            lbEquipXrefUpdate_Serial.Text = sSerial;
            hfEquipXrefUpdate_Unit.Value = iUnit.ToString();
            txEquipXrefUpdate_Xref.Text = sXref;
            
            pnEquipXrefUpdate.Visible = true;
            txEquipXrefUpdate_Xref.Focus();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btEquipXrefUpdate_Click(object sender, EventArgs e)
    {
        int iUnit = 0;
        if (int.TryParse(hfEquipXrefUpdate_Unit.Value, out iUnit) == false)
            iUnit = 0;

        string sNewXref = txEquipXrefUpdate_Xref.Text.Trim();

        string sResult = ws_Upd_B1EquipmentCrossRef(sNewXref, iUnit);

        string sIsExcelVersionNeeded = "";
        LoadEquipmentDataTables(sIsExcelVersionNeeded);

        pnEquipXrefUpdate.Visible = false;
    }
    // -------------------------------------------------------------------------------------------------
    protected void btEquipXrefClose_Click(object sender, EventArgs e)
    {
        pnEquipXrefUpdate.Visible = false;
    }
    // -------------------------------------------------------------------------------------------------
    protected void btEquipmentSearchClear_Click(object sender, EventArgs e)
    {
        ddSearchProductCodes.SelectedValue = "";
        txSearchModel.Text = "";
        txSearchSerial.Text = "";
        txSearchModelDescription.Text = "";
        txSearchAsset.Text = "";
        txSearchAgentId.Text = "";
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}