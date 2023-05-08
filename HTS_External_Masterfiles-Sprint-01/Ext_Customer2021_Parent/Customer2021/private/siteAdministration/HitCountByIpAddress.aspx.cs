using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_siteAdministration_HitCountByIpAddress : MyPage
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
                Load_IpaDataTables();
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
    protected void Load_IpaDataTables()
    {
        DataTable dt = new DataTable("");
        string sSql = "";
        string sIpa = txSearchIpa.Text.Trim();

        try
        {
            sqlConn.Open();

            sSql = "Select" +
                 " siKey" +
                ", siIpa" +
                ", siPeriodStart" +
                ", siLastAccess" +
                ", siPeriodHits" +
                ", siLifeHits" +
                ", siTimeoutCount" +
                ", siBlacklisted" +
                ", siFirstUse" +
                ", siUserID" +
                ", siComment" +
                " from HitsFromSameIP";
            if ((sIpa != null) && (sIpa.Length > 0))
            {
                sSql += " where siIpa like @SearchIpa";
            }
            else
            {
                sSql += " where (siComment != ''" +
                        //" or siPeriodStart > @DateCutoff) and siPeriodHits > 4";
                        " or siPeriodStart > @DateCutoff)";
            }
            if (chbxIpa_Blacklisted.Checked == true)
                sSql += " and siBlacklisted = 1";

            // sSql += " order by siPeriodHits desc, siLifeHits desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            if (!String.IsNullOrEmpty(sIpa))
            {
                sqlCmd.Parameters.Add("@SearchIpa", System.Data.SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@SearchIpa"].Value = sIpa + "%";
            }
            else
            {
                sqlCmd.Parameters.AddWithValue("@DateCutoff", DateTime.Now.AddDays(-14));
            }

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(sqlReader);

            dt.Columns.Add(MakeColumn("ToggleText"));
            dt.Columns.Add(MakeColumn("Idx"));
            int iIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                iIdx++;
                if (row["siBlacklisted"].ToString().Trim() == "1")
                    row["ToggleText"] = "Clear";
                else
                    row["ToggleText"] = "Blacklist";
                row["Idx"] = iIdx.ToString();
            }

            dt.Columns["siKey"].ColumnName = "Key";
            dt.Columns["siIpa"].ColumnName = "Ip";
            dt.Columns["siPeriodStart"].ColumnName = "PeriodStart";
            dt.Columns["siLastAccess"].ColumnName = "LastAccess";
            dt.Columns["siPeriodHits"].ColumnName = "PeriodHits";
            dt.Columns["siLifeHits"].ColumnName = "LifeHits";
            dt.Columns["siTimeoutCount"].ColumnName = "TimeoutCount";
            dt.Columns["siBlacklisted"].ColumnName = "Blacklisted";
            dt.Columns["siFirstUse"].ColumnName = "FirstUse";
            dt.Columns["siUserID"].ColumnName = "UserID";
            dt.Columns["siComment"].ColumnName = "Comment";

            dt.AcceptChanges();

            rp_IpaSmall.DataSource = dt;
            rp_IpaSmall.DataBind();

            ViewState["vsDataTable_Ipa"] = null;
            BindGrid_Ipa(dt);

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
    // BEGIN: Location Table (_Ipa)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Ipa(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null
        
        string sortExpression_Ipa = "";

        if (ViewState["vsDataTable_Ipa"] == null)
        {
            lbMsg.Text = "";

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Ipa"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Ipa"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        if (gridSortDirection_Ipa == SortDirection.Ascending)
        {
            sortExpression_Ipa = gridSortExpression_Ipa + " ASC";
        }
        else
        {
            sortExpression_Ipa = gridSortExpression_Ipa + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Ipa;

        gv_IpaLarge.DataSource = dt.DefaultView;
        gv_IpaLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Ipa(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_IpaLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Ipa(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Ipa(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Ipa == SortDirection.Ascending)
                gridSortDirection_Ipa = SortDirection.Descending;
            else
                gridSortDirection_Ipa = SortDirection.Ascending;
        }
        else
            gridSortDirection_Ipa = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Ipa = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Ipa(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Ipa
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Ipa"] == null)
            {
                //ViewState["GridSortDirection_Ipa"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Ipa"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Ipa"];
        }
        set
        {
            ViewState["GridSortDirection_Ipa"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Ipa
    {
        get
        {
            if (ViewState["GridSortExpression_Ipa"] == null)
            {
                ViewState["GridSortExpression_Ipa"] = "PeriodHits"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Ipa"];
        }
        set
        {
            ViewState["GridSortExpression_Ipa"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Ipa)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchIpaSubmit_Click(object sender, EventArgs e)
    {
        Load_IpaDataTables();
    }
    // ========================================================================
    protected void btSearchIpaClear_Click(object sender, EventArgs e)
    {
        txSearchIpa.Text = "";
    }
    // ================================================================
    protected void lkCommentUpd_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton myControl = (LinkButton)sender;
        string sParms = myControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('|');

        int iKey = 0;
        string sIpa = "";
        string sComment = "";

        int.TryParse(saArg[0], out iKey);
        sIpa = saArg[1].ToString().Trim();
        sComment = saArg[2].ToString().Trim();

        // --------------------------------
        lbUpdateIpa_Address.Text = sIpa;
        txUpdateIpa_Comment.Text = sComment;
        hfUpdateIpa_RecId.Value = iKey.ToString();
        pnUpdateIpaComment.Visible = true;
        txUpdateIpa_Comment.Focus();
    }
    // ================================================================
    protected void btUpdateIpa_Comment_Click(object sender, EventArgs e)
    {
        int iKey = 0;
        int.TryParse(hfUpdateIpa_RecId.Value, out iKey);
        string sComment = txUpdateIpa_Comment.Text.Trim();

        UpdateIpComment(iKey, sComment);

        pnUpdateIpaComment.Visible = false;
        Load_IpaDataTables();
    }
    // ================================================================
    protected void lkBlacklistToggle_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        int iKey = 0;
        int iToggle = 0;
        if (myControl.Text == "Blacklist")
            iToggle = 1;
        else
            iToggle = 0;

        if (int.TryParse(myControl.CommandArgument.ToString().Trim(), out iKey) == false)
            iKey = -1;
        if (iKey > 0)
        {
            UpdateBlacklistStatus(iKey, iToggle);

            pnUpdateIpaComment.Visible = false;
            Load_IpaDataTables();
        }
    }
    // ================================================================
    protected void UpdateIpComment(int iKey, string sComment)
    {
        string sSql = "";

        try
        {
            sqlConn.Open();

            sSql = "Update" +
                " HitsFromSameIP" +
                " set siComment = @Comment" +
                " where siKey = @Key";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            sqlCmd.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar, 200);
            sqlCmd.Parameters["@Comment"].Value = sComment.Trim();

            sqlCmd.Parameters.Add("@Key", System.Data.SqlDbType.Int);
            sqlCmd.Parameters["@Key"].Value = iKey;

            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        // ----------------------------------
    }
    // ================================================================
    protected int UpdateBlacklistStatus(int key, int toggleValue)
    {
        int iRowsAffected = 0;

        SqlCommand sqlCmd = new SqlCommand();
        SqlConnection sqlConn;

        string sSql = "";
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        try
        {
            sqlConn.Open();

            sSql = "Update" +
                " HitsFromSameIP" +
                " set siBlacklisted = @Toggle" +
                " where siKey = @Key";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            sqlCmd.Parameters.AddWithValue("@Toggle", toggleValue);
            sqlCmd.Parameters.AddWithValue("@Key", key);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        // ----------------------------------
        return iRowsAffected;
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        Load_IpaDataTables();
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateIpa_CommentClose_Click(object sender, EventArgs e)
    {
        pnUpdateIpaComment.Visible = false;
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
