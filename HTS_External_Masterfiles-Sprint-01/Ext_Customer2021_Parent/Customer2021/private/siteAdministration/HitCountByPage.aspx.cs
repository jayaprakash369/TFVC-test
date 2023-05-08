using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_siteAdministration_HitCountByPage : MyPage
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
            try
            {
                Load_PagDataTables();
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
    protected void Load_PagDataTables()
    {
        DataTable dt = new DataTable("");
        string sSql = "";

        try
        {
            sqlConn.Open();

            sSql = "Select" +
                 " hcKey" +
                ", hcPath" +
                ", hcPage" +
                ", hcCount" +
                ", hcYear" +
                " from HitCount" +
                " where hcYear = @Year";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@Year", DateTime.Now.Year);
            sqlCmd.CommandText = sSql;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(sqlReader);

            dt.Columns.Add(MakeColumn("CountSort"));

            dt.Columns["hcKey"].ColumnName = "Key";
            dt.Columns["hcPath"].ColumnName = "Path";
            dt.Columns["hcPage"].ColumnName = "Page";
            dt.Columns["hcCount"].ColumnName = "Count";
            dt.Columns["hcYear"].ColumnName = "Year";

            dt.AcceptChanges();

            int iCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(row["Count"].ToString().Trim(), out iCount) == false)
                    iCount = -1;

                if (iCount > -1)
                    row["CountSort"] = iCount.ToString("0000000");
            }

            dt.AcceptChanges();

            rp_PagSmall.DataSource = dt;
            rp_PagSmall.DataBind();

            ViewState["vsDataTable_Pag"] = null;
            BindGrid_Pag(dt);

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
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Pag)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Pag(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null
        
        string sortExpression_Pag = "";

        if (ViewState["vsDataTable_Pag"] == null)
        {
            lbMsg.Text = "";

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Pag"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Pag"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        if (gridSortDirection_Pag == SortDirection.Ascending)
        {
            sortExpression_Pag = gridSortExpression_Pag + " ASC";
        }
        else
        {
            sortExpression_Pag = gridSortExpression_Pag + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Pag;

        gv_PagLarge.DataSource = dt.DefaultView;
        gv_PagLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Pag(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_PagLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Pag(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Pag(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Pag == SortDirection.Ascending)
                gridSortDirection_Pag = SortDirection.Descending;
            else
                gridSortDirection_Pag = SortDirection.Ascending;
        }
        else
            gridSortDirection_Pag = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Pag = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Pag(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Pag
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Pag"] == null)
            {
                //ViewState["GridSortDirection_Pag"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Pag"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Pag"];
        }
        set
        {
            ViewState["GridSortDirection_Pag"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Pag
    {
        get
        {
            if (ViewState["GridSortExpression_Pag"] == null)
            {
                ViewState["GridSortExpression_Pag"] = "CountSort"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Pag"];
        }
        set
        {
            ViewState["GridSortExpression_Pag"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Pag)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
