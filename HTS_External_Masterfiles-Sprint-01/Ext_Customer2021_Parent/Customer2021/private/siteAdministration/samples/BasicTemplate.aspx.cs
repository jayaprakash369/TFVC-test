using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_siteAdministration_samples_BasicTemplate : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    //string sTemp = "";

    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";

        if (!IsPostBack)
        {

            Get_UserPrimaryCustomerNumber();

            try
            {
                Load_BasicDataTables();
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
    // ========================================================================
    protected DataTable ws_Get_B1CustomerLocationsFromTemplate(
        string customerNumber, 
        string city)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustomerLocationsFromTemplate";
            string sFieldList = "customerNumber|city|x";
            string sValueList = customerNumber.ToString() + "|" + city + "|x";

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
    // ----------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ----------------------------------------------------------------------------
    protected void Load_BasicDataTables()
    {
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");
        DataTable dt = new DataTable("");

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;
                string sSearchCity = "";

                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;

                if (!String.IsNullOrEmpty(txSearchCity.Text.Trim())) 
                    sSearchCity = txSearchCity.Text.Trim();

                if (iCustomerNumber > 0)
                {
                    dtB1 = ws_Get_B1CustomerLocationsFromTemplate(iCustomerNumber.ToString(), sSearchCity);
                    //dtB2 = ...

                    dt = Merge_BasicTables(dtB1, dtB2);

                    rp_BasicSmall.DataSource = dt;
                    rp_BasicSmall.DataBind();

                    ViewState["vsDataTable_Bas"] = null;
                    BindGrid_Bas(dt);
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
    protected DataTable Merge_BasicTables(DataTable dtB1, DataTable dtB2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        // Original Columns (using generic names that will fit B1 and B2 data)
        dt.Columns.Add(MakeColumn("CustomerName"));
        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("Address1"));
        dt.Columns.Add(MakeColumn("City"));
        dt.Columns.Add(MakeColumn("State"));

        // Formatted Versions
        dt.Columns.Add(MakeColumn("CustomerNumberSort"));
        dt.Columns.Add(MakeColumn("CustomerLocationSort"));
        dt.Columns.Add(MakeColumn("Source"));

        DataRow dr;
        int iRowIdx = 0;
        int iTemp = 0;

        foreach (DataRow row in dtB1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            if (int.TryParse(row["CSTRNR"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0) 
            {
                dt.Rows[iRowIdx]["CustomerNumber"] = iTemp.ToString("");
                dt.Rows[iRowIdx]["CustomerNumberSort"] = iTemp.ToString("0000000"); // Format to the largest possible size (or 1 more)
            }

            if (int.TryParse(row["CSTRCD"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > -1)
            {
                dt.Rows[iRowIdx]["CustomerLocation"] = iTemp.ToString("");
                dt.Rows[iRowIdx]["CustomerLocationSort"] = iTemp.ToString("000");
            }
            
            dt.Rows[iRowIdx]["CustomerName"] = Fix_Case(row["CUSTNM"].ToString().Trim());
            dt.Rows[iRowIdx]["Address1"] = Fix_Case(row["SADDR1"].ToString().Trim());
            dt.Rows[iRowIdx]["City"] = Fix_Case(row["CITY"].ToString().Trim());
            dt.Rows[iRowIdx]["State"] = row["STATE"].ToString().Trim();

            dt.Rows[iRowIdx]["Source"] = "1";

            iRowIdx++;
        }

        //foreach (DataRow row in dtB2.Rows)
        //{
        //    dr = dt.NewRow();
        //    dt.Rows.Add(dr);

        //    if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
        //        iTemp = -1;
        //    if (iTemp > 0)
        //        dt.Rows[iRowIdx]["Basic"] = iTemp.ToString("");

        //    dt.Rows[iRowIdx]["Source"] = "2";

        //    iRowIdx++;
        //}


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
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    protected void BindGrid_Bas(DataTable dt)
    {

        if (ViewState["vsDataTable_Bas"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Bas"] = dt;

            lbMsg.Text = "";
            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Bas"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Bas == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Bas + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Bas + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_BasicLarge.DataSource = dt.DefaultView;
        gv_BasicLarge.DataBind();

    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_Bas(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_BasicLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Bas(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_Bas(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Bas == SortDirection.Ascending)
                gridSortDirection_Bas = SortDirection.Descending;
            else
                gridSortDirection_Bas = SortDirection.Ascending;
        }
        else
            gridSortDirection_Bas = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Bas = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Bas(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_Bas
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Bas"] == null)
            {
                ViewState["GridSortDirection_Bas"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Bas"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Bas"];
        }
        set
        {
            ViewState["GridSortDirection_Bas"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_Bas
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Bas"] == null)
            {
                ViewState["GridSortExpression_Bas"] = "CustomerName"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_Bas"];
        }
        set
        {
            ViewState["GridSortExpression_Bas"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btBasicSearchSubmit_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(txSearchCity.Text)) 
        {
            if (txSearchCity.Text.Length > 30)
                lbMsg.Text = "City entry longer than 30 char max";
        }

        if (String.IsNullOrEmpty(lbMsg.Text)) // i.e. no validation errors...
        {
            Load_BasicDataTables();
        }
    }
    // ----------------------------------------------------------------------------
    protected void btBasicSearchClear_Click(object sender, EventArgs e)
    {
        txSearchCity.Text = "";
    }
    // ----------------------------------------------------------------------------
    protected void lkBasic_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');
        
        int iBasicNumber = 0;
        string sBasicSource = "";
        string sUrl = "";

        if (saArg.Length > 1)
        {
            if (int.TryParse(saArg[0], out iBasicNumber) == false)
                iBasicNumber = 0;

            sBasicSource = saArg[1];

            sUrl = "~/private/sc/BasicDetail.aspx" +
                "?inv=" + iBasicNumber +
                "&src=" + sBasicSource;

            Response.Redirect(sUrl, false);
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}