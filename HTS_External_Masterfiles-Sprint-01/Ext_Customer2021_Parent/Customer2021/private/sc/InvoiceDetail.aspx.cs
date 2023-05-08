using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_sc_InvoiceDetail : MyPage
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
            if (Request.QueryString["inv"] != null && Request.QueryString["inv"].ToString() != "")
                hfPassedInvoice.Value = Request.QueryString["inv"].ToString().Trim();
            if (Request.QueryString["src"] != null && Request.QueryString["src"].ToString() != "")
                hfPassedSource.Value = Request.QueryString["src"].ToString().Trim();

            Get_UserPrimaryCustomerNumber();

            try
            {
                Load_InvoiceDetail();
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
    #region myWebServiceCalls
  
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ----------------------------------------------------------------------------
    protected void Load_InvoiceDetail()
    {
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");
        DataTable dt = new DataTable("");
        int iPassedInvoice = 0;
        int iRowCount = 0;
        string sPassedSource = "";
        string sPassedType = "";
        string sPassedParm2 = "";

        try
        {
            sPassedParm2 = hfPassedSource.Value;
            if (sPassedParm2 != "")
            {
                sPassedSource = sPassedParm2.Substring(0, 1);
                int ilen = sPassedSource.Length;
                if (ilen > 0)
                    sPassedType = sPassedParm2.Substring(1, 1);
            }
            if (int.TryParse(hfPassedInvoice.Value, out iPassedInvoice) == false)
                iPassedInvoice = -1;

            if (iPassedInvoice > 0 &&  !String.IsNullOrEmpty(sPassedSource))
            {

                if (sPassedSource == "1")
                {
                    string sJobName = "Get_B1Invoice_DetailRecord";
                    string sFieldList = "invoiceNumber|invoiceType|x";
                    string sValueList = iPassedInvoice.ToString() + "|" + sPassedType + "|x";
                    dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
                    iRowCount = dt.Rows.Count;
                    iRowCount = iRowCount - 1;
                    if (dt.Rows[iRowCount]["sDescription"].ToString() == "Invoice Total:")
                    {
                        lbInvoiceTotal.Text = "Invoice total w/o taxes: " + dt.Rows[iRowCount]["sTotalAmount"].ToString();
                        dt.Rows.Remove(dt.Rows[iRowCount]);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        lbInvoiceNoBl1.Text = "Invoice No: " + iPassedInvoice.ToString();
                        rp_InvoiceDetailBld1Small.DataSource = dt;
                        rp_InvoiceDetailBld1Small.DataBind();

                        ViewState["vsDataTable_Inv"] = null;
                        BindGrid_InvDtlBld1(dt);
                        pnBuilding1.Visible = true;
                    }
                }
                else if (sPassedSource == "2")
                {
                    string sJobName = "Get_B2Invoice_DetailRecord";
                    string sFieldList = "invoiceNumber|invoiceType|x";
                    string sValueList = iPassedInvoice.ToString() + "|" + sPassedType + "|x";
                    dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
                    iRowCount = dt.Rows.Count;
                    iRowCount = iRowCount - 1;
                    if (dt.Rows[iRowCount]["dspDescription"].ToString() == "Invoice Total:")
                    {
                        lbInvoiceTotalBl2.Text = "Invoice total w/o taxes: " + dt.Rows[iRowCount]["dspExtPrice"].ToString();
                        dt.Rows.Remove(dt.Rows[iRowCount]);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        lbInvoiceNoBl2.Text = "Invoice No: " + iPassedInvoice.ToString();
                        rp_InvoiceDetailBld2Small.DataSource = dt;
                        rp_InvoiceDetailBld2Small.DataBind();

                        ViewState["vsDataTable_Inv"] = null;
                        BindGrid_InvDtlBld2(dt);
                        pnBuilding2.Visible = true;
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
                hfPrimaryCs1Type.Value = ws_Get_B1CustomerType(iCustomerNumber);
        }

    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ----------------------------------------------------------------------------
    // ========================================================================
    protected void BindGrid_InvDtlBld1(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_InvDtlBld1"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_InvDtlBld1"] = dt;

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
            dt = (DataTable)ViewState["vsDataTable_InvDtlBld1"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_InvDtlBld1 == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_InvDtlBld1 + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_InvDtlBld1 + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_InvoiceDetailBld1Large.DataSource = dt.DefaultView;
        gv_InvoiceDetailBld1Large.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_InvoiceDetailBld1Small.DataSource = dt.DefaultView;
            rp_InvoiceDetailBld1Small.DataBind();
        }
    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_InvDtlBld1(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_InvoiceDetailBld1Large.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_InvDtlBld1(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_InvDtlBld1(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_InvDtlBld1 == SortDirection.Ascending)
                gridSortDirection_InvDtlBld1 = SortDirection.Descending;
            else
                gridSortDirection_InvDtlBld1 = SortDirection.Ascending;
        }
        else
        {
            if (gridSortDirection_InvDtlBld1 == SortDirection.Ascending)
                gridSortDirection_InvDtlBld1 = SortDirection.Descending;
            else
                gridSortDirection_InvDtlBld1 = SortDirection.Ascending;
        }
 
        // Save the new sort expression
        gridSortExpression_InvDtlBld1 = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_InvDtlBld1(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_InvDtlBld1
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_InvDtlBld1"] == null)
            {
                ViewState["GridSortDirection_InvDtlBld1"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_InvDtlBld1"];
        }
        set
        {
            ViewState["GridSortDirection_InvDtlBld1"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_InvDtlBld1
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_InvDtlBld1"] == null)
            {
                ViewState["GridSortExpression_InvDtlBld1"] = "sPart"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_InvDtlBld1"];
        }
        set
        {
            ViewState["GridSortExpression_InvDtlBld1"] = value;
        }
    }
    // ========================================================================
    protected void BindGrid_InvDtlBld2(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_InvDtlBld2"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_InvDtlBld2"] = dt;

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
            dt = (DataTable)ViewState["vsDataTable_InvDtlBld2"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_InvDtlBld2 == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_InvDtlBld2 + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_InvDtlBld2 + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_InvoiceDetailBld2Large.DataSource = dt.DefaultView;
        gv_InvoiceDetailBld2Large.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_InvoiceDetailBld2Small.DataSource = dt.DefaultView;
            rp_InvoiceDetailBld2Small.DataBind();
        }
    }

    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_InvDtlBld2(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_InvoiceDetailBld2Large.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_InvDtlBld2(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_InvDtlBld2(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_InvDtlBld2 == SortDirection.Ascending)
                gridSortDirection_InvDtlBld2 = SortDirection.Descending;
            else
                gridSortDirection_InvDtlBld2 = SortDirection.Ascending;
        }
        else
        {
            if (gridSortDirection_InvDtlBld2 == SortDirection.Ascending)
                gridSortDirection_InvDtlBld2 = SortDirection.Descending;
            else
                gridSortDirection_InvDtlBld2 = SortDirection.Ascending;
        }
        // Save the new sort expression
        gridSortExpression_InvDtlBld2 = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_InvDtlBld2(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_InvDtlBld2
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_InvDtlBld2"] == null)
            {
                ViewState["GridSortDirection_InvDtlBld2"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_InvDtlBld2"];
        }
        set
        {
            ViewState["GridSortDirection_InvDtlBld1"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_InvDtlBld2
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_InvDtlBld2"] == null)
            {
                ViewState["GridSortExpression_InvDtlBld2"] = "dspProduct"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_InvDtlBld2"];
        }
        set
        {
            ViewState["GridSortExpression_InvDtlBld2"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents

    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}