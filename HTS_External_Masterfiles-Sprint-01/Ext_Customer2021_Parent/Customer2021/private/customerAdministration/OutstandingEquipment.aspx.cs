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

public partial class private_sc_OutstandingEquipment : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------

    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";

        if (!IsPostBack)
        {
            DataTable dtEqp = new DataTable("");
            Get_UserPrimaryCustomerNumber();

            try
            {
               dtEqp = Load_PageData();
               if (dtEqp.Rows.Count > 0)
               {
                    rp_EquipmentSmall.DataSource = dtEqp;
                    rp_EquipmentSmall.DataBind();

                    ViewState["vsDataTable_Eqp"] = null;
                    BindGrid_Eqp(dtEqp);
                    pnOutstanding.Visible = true;
                    pnEquipmentSearch.Visible = false;
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
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // ========================================================================
    protected void BindGrid_Eqp(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_Eqp"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dt;

            lbMsg.Text = "";
            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Eqp"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Eqp == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Eqp + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Eqp + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_EquipmentLarge.DataSource = dt.DefaultView;
        gv_EquipmentLarge.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_EquipmentSmall.DataSource = dt.DefaultView;
            rp_EquipmentSmall.DataBind();
        }
    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_Eqp(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_EquipmentLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Eqp(dt);
    }
    // ----------------------------------------------------------------------------
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
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_Eqp
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Eqp"] == null)
            {
                ViewState["GridSortDirection_Eqp"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Eqp"];
        }
        set
        {
            ViewState["GridSortDirection_Eqp"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_Eqp
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Eqp"] == null)
            {
                ViewState["GridSortExpression_Eqp"] = "LocationDsp"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_Eqp"];
        }
        set
        {
            ViewState["GridSortExpression_Eqp"] = value;
        }
    }
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================

    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ========================================================================
    // ---------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1OutstandingEquipmentForCustomer(
        string customerNumber
        )
    {
        DataTable dt = new DataTable("");
        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1OutstandingEquipmentForCustomer";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

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
    // BEGIN: Location Table (_Eqp)
  
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
    protected void Load_OutStandingEquipment(string isExcelVersionNeeded) 
    {
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
      
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

    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable Load_PageData()
    {
        DataTable dt = new DataTable("");
        DataTable dt1 = new DataTable("");
        DateTime datTemp = DateTime.Now;

        dt.Columns.Add(MakeColumn("XrefDsp"));
        dt.Columns.Add(MakeColumn("LocationDsp"));
        dt.Columns.Add(MakeColumn("CityDsp"));
        dt.Columns.Add(MakeColumn("StateDsp"));
        dt.Columns.Add(MakeColumn("PartDsp"));
        dt.Columns.Add(MakeColumn("DescDsp"));
        dt.Columns.Add(MakeColumn("TicketDsp"));
        dt.Columns.Add(MakeColumn("ShipDsp"));
        dt.Columns.Add(MakeColumn("SupTrkDsp"));
        dt.Columns.Add(MakeColumn("TicketSort"));
        dt.Columns.Add(MakeColumn("ShipSort"));
        DataRow dr;
        int iRowIdx = 0;
        int iCenter = 0;
        int iTicket = 0;

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;

                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;

                if (iCustomerNumber > 0)
                {
                    // You're getting all three values in different columns in this query (3 charts) 
                    dt1 = ws_Get_B1OutstandingEquipmentForCustomer(
                        iCustomerNumber.ToString());

                    foreach (DataRow row in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        if (int.TryParse(row["S1CTR"].ToString().Trim(), out iCenter) == false)
                            iCenter = 0;
                        if (int.TryParse(row["S1TCK"].ToString().Trim(), out iTicket) == false)
                            iTicket = 0;
                        string Ticket = row["S1CTR"].ToString().Trim() + "-" + row["S1Tck"].ToString().Trim(); 
                        dt.Rows[iRowIdx]["XrefDsp"] = row["XREF"].ToString().Trim();
                        dt.Rows[iRowIdx]["LocationDsp"] = row["CUSTNAME"].ToString().Trim();
                        dt.Rows[iRowIdx]["CityDsp"] = Fix_Case(row["CITY"].ToString().Trim());
                        dt.Rows[iRowIdx]["StateDsp"] = row["STATE"].ToString().Trim();
                        dt.Rows[iRowIdx]["PartDsp"] = row["InstalledPart"].ToString().Trim();
                        dt.Rows[iRowIdx]["DescDsp"] = Fix_Case(row["Description"].ToString().Trim());
                        dt.Rows[iRowIdx]["TicketDsp"] = Ticket;
                        dt.Rows[iRowIdx]["TicketSort"] = iCenter.ToString("000").Trim() + iTicket.ToString("0000000");
                        string sDate = row["ShipDate"].ToString().Trim();
                        if (sDate.Length == 8)
                        {
                            string syyyy = sDate.Substring(0, 4);
                            string smm = sDate.Substring(4, 2);
                            string sdd = sDate.Substring(6, 2);
                            dt.Rows[iRowIdx]["ShipDsp"] = smm + "/" + sdd + "/" + syyyy;
                        }
                        dt.Rows[iRowIdx]["ShipSort"] = row["ShipDate"].ToString().Trim();
                        dt.Rows[iRowIdx]["SupTrkDsp"] = row["Tracking"].ToString().Trim();

                        iRowIdx++;
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
        Load_OutStandingEquipment(sIsExcelNeeded);
    }
    // -------------------------------------------------------------------------------------------------
    protected void btEquipmentExcelSubmit_Click(object sender, EventArgs e)
    {
        string sIsExcelNeeded = "Y";

        Load_OutStandingEquipment(sIsExcelNeeded);
    }
   // -------------------------------------------------------------------------------------------------
    protected void btEquipmentSearchClear_Click(object sender, EventArgs e)
    {
        txSearchModel.Text = "";
        txSearchSerial.Text = "";
        txSearchModelDescription.Text = "";
        txSearchXref.Text = "";
        txSearchTracking.Text = "";
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}