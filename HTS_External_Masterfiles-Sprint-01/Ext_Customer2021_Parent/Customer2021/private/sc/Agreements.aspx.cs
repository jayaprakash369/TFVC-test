using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

public partial class private_sc_Agreements : MyPage
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
        lbMsg.Text = "";

        if (!IsPostBack)
        {
            Get_UserPrimaryCustomerNumber();


            try
                {
                    Load_AgreementDataTables();

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
    #region tableSortHandler
    // ========================================================================
    protected void BindGrid_Agr(DataTable dt)
    {

        if (ViewState["vsDataTable_Agr"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Agr"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No agreements were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Agr"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Agr == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Agr + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Agr + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_AgreementLarge.DataSource = dt.DefaultView;
        gv_AgreementLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Agr(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_AgreementLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Agr(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Agr(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Agr == SortDirection.Ascending)
                gridSortDirection_Agr = SortDirection.Descending;
            else
                gridSortDirection_Agr = SortDirection.Ascending;
        }
        else
            gridSortDirection_Agr = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Agr = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Agr(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Agr
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Agr"] == null)
            {
                //ViewState["GridSortDirection_Agr"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Agr"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Agr"];
        }
        set
        {
            ViewState["GridSortDirection_Agr"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Agr
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Agr"] == null)
            {
                ViewState["GridSortExpression_Agr"] = "DateStartingSort"; // was AgreementId
            }
            return (string)ViewState["GridSortExpression_Agr"];
        }
        set
        {
            ViewState["GridSortExpression_Agr"] = value;
        }
    }
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
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
    // -------------------------------------------------------------------------------------------------
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
    // -------------------------------------------------------------------------------------------------
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
    // -------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2AgreementEquipment(
        int agreementNumber,
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

        if (agreementNumber > 0)
        {
            string sJobName = "Get_B2AgreementEquipment";
            string sFieldList = "agreementNumber|productCode|model|serial|modelDescription|asset|agentId|downloadExcelY|x";
            string sValueList =
                agreementNumber + "|" +
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
    // -------------------------------------------------------------------------------------------------
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
    protected void Load_AgreementDataTables()
    {
        DataTable dt = new DataTable("");
        DataTable dt1 = new DataTable("");
        DataTable dt2 = new DataTable("");

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;
                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;
                if (iCustomerNumber > 0)
                {
                    int iSearchAgreement = 0;
                    if (!String.IsNullOrEmpty(txSearchAgreement.Text)) 
                    {
                        if (int.TryParse(txSearchAgreement.Text, out iSearchAgreement) == false)
                            iSearchAgreement = 0;
                    }

                    dt1 = ws_Get_B1Agreements(iCustomerNumber, iSearchAgreement);
                    dt2 = ws_Get_B2Agreements(iCustomerNumber, iSearchAgreement);

                    dt = Merge_AgreementTables(dt1, dt2);

                    rp_AgreementSmall.DataSource = dt;
                    rp_AgreementSmall.DataBind();

                    ViewState["vsDataTable_Agr"] = null;
                    BindGrid_Agr(dt);
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
    // -------------------------------------------------------------------------------------------------
    protected DataTable Merge_AgreementTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("AgreementId"));
        dt.Columns.Add(MakeColumn("DateStarting"));
        dt.Columns.Add(MakeColumn("DateStartingSort"));
        dt.Columns.Add(MakeColumn("DateEnding"));
        dt.Columns.Add(MakeColumn("DateEndingSort"));
        dt.Columns.Add(MakeColumn("AgreementType"));
        dt.Columns.Add(MakeColumn("Source"));
        dt.Columns.Add(MakeColumn("EquipmentCount"));
        dt.Columns.Add(MakeColumn("EquipmentCountSort"));
        dt.Columns.Add(MakeColumn("LocationId1"));
        dt.Columns.Add(MakeColumn("LocationId2"));
        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("B1Name"));
        dt.Columns.Add(MakeColumn("B2Address1"));
        dt.Columns.Add(MakeColumn("B2City"));
        dt.Columns.Add(MakeColumn("B2State"));

        DateTime datTemp = new DateTime();
        DataRow dr;
        int iRowIdx = 0;
        int iQty = 0;
        int iTemp = 0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            if (int.TryParse(row["CONTNR"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0)
                dt.Rows[iRowIdx]["AgreementId"] = iTemp.ToString("");

            if (DateTime.TryParse(row["DateStartingStamp"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateStarting"] = datTemp.ToString("MMM d yyyy");
                dt.Rows[iRowIdx]["DateStartingSort"] = datTemp.ToString("o");
            }

            if (DateTime.TryParse(row["DateEndingStamp"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["DateEnding"] = datTemp.ToString("MMM d yyyy");
                dt.Rows[iRowIdx]["DateEndingSort"] = datTemp.ToString("o");
            }

            dt.Rows[iRowIdx]["AgreementType"] = Fix_Case(row["Description"].ToString().Trim());


            if (int.TryParse(row["AgreementEquipmentCount"].ToString().Trim(), out iQty) == false)
                iQty = 0;
            if (iQty > 0) 
            {
                dt.Rows[iRowIdx]["EquipmentCount"] = iQty.ToString();
                dt.Rows[iRowIdx]["EquipmentCountSort"] = iQty.ToString("0000");
            }
            dt.Rows[iRowIdx]["LocationId1"] = row["CNTRNR"].ToString().Trim();
            dt.Rows[iRowIdx]["LocationId2"] = row["CNTRCD"].ToString().Trim();

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {

            if (row["agreementStatus"].ToString().Trim() == "Active")
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);



                if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
                    iTemp = -1;
                if (iTemp > 0)
                    dt.Rows[iRowIdx]["AgreementId"] = iTemp.ToString("");

                if (DateTime.TryParse(row["startDate"].ToString().Trim(), out datTemp) == true)
                {
                    dt.Rows[iRowIdx]["DateStarting"] = datTemp.ToString("MMM d yyyy");
                    dt.Rows[iRowIdx]["DateStartingSort"] = datTemp.ToString("o");
                }

                if (DateTime.TryParse(row["endDate"].ToString().Trim(), out datTemp) == true)
                {
                    dt.Rows[iRowIdx]["DateEnding"] = datTemp.ToString("MMM d yyyy");
                    dt.Rows[iRowIdx]["DateEndingSort"] = datTemp.ToString("o");
                }

                dt.Rows[iRowIdx]["AgreementType"] = row["type-name"].ToString().Trim();

                if (int.TryParse(row["AgreementEquipmentCount"].ToString().Trim(), out iQty) == false)
                    iQty = 0;
                if (iQty > 0)
                {
                    dt.Rows[iRowIdx]["EquipmentCount"] = iQty.ToString();
                    dt.Rows[iRowIdx]["EquipmentCountSort"] = iQty.ToString("0000");
                }

                dt.Rows[iRowIdx]["LocationId1"] = row["OracleParentId"].ToString().Trim();
                dt.Rows[iRowIdx]["LocationId2"] = row["OracleChildId"].ToString().Trim();

                dt.Rows[iRowIdx]["CustomerNumber"] = row["CustomerNumber"].ToString().Trim();
                dt.Rows[iRowIdx]["CustomerLocation"] = row["CustomerLocation"].ToString().Trim();
                dt.Rows[iRowIdx]["B1Name"] = row["B1Name"].ToString().Trim();
                dt.Rows[iRowIdx]["B2Address1"] = row["LocationAddress1"].ToString().Trim();
                dt.Rows[iRowIdx]["B2City"] = row["LocationCity"].ToString().Trim();
                dt.Rows[iRowIdx]["B2State"] = row["LocationState"].ToString().Trim();

                dt.Rows[iRowIdx]["Source"] = "2";

                iRowIdx++;
            }
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
    protected void btSearchAgreementSubmit_Click(object sender, EventArgs e)
    {
        Load_AgreementDataTables();
    }
    // ========================================================================
    protected void btSearchAgreementClear_Click(object sender, EventArgs e)
    {
        txSearchAgreement.Text = "";
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkQty_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');

        string sUrl = "";

        if (saArg.Length > 3)
        {
            // Set the chosen customer to use going forward
            string sAgreementSource = saArg[0];
            int iAgreementNumber = 0;
            int iCustomerNumber = 0;
            int iCustomerLocation = 0;
            if (int.TryParse(saArg[1], out iAgreementNumber) == false)
                iAgreementNumber = 0;
            if (int.TryParse(saArg[2], out iCustomerNumber) == false)
                iCustomerNumber = 0;
            if (int.TryParse(saArg[3], out iCustomerLocation) == false)
                iCustomerLocation = 0;

            sUrl = "~/private/sc/AgreementEquipment.aspx" +
                "?src=" + sAgreementSource +
                "&agr=" + iAgreementNumber.ToString("00000000");
            if (iCustomerNumber > 0)
                sUrl += "&cs1=" + iCustomerNumber;
            if (iCustomerLocation > 0)
                sUrl += "&cs2=" + iCustomerLocation;

            Response.Redirect(sUrl, false);
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}