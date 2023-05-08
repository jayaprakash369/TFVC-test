using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class private__admin_IpCount : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // ================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbIp.Text = "IP: " + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Trim();
            LoadIpHitCount();
        }
    }
    // ================================================================
    protected void LoadIpHitCount()
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlConnection sqlConn;
        SqlDataReader sqlReader;
        DataTable dataTable;

        string sSql = "";
        string sIpa = txIpSearch.Text;

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

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
                        " or siPeriodStart > @DateCutoff) and siPeriodHits > 4";
                        
            }
            if (chbxBlacklisted.Checked == true)
                sSql += " and siBlacklisted = 1";

            if (ddSort.SelectedValue == "PeriodHits")
                sSql += " order by siPeriodHits desc, siPeriodStart desc";
            else if (ddSort.SelectedValue == "LifetimeHits")
                sSql += " order by siLifeHits desc";
            else if (ddSort.SelectedValue == "PeriodStart")
                sSql += " order by siPeriodStart desc, siPeriodHits desc";
            else if (ddSort.SelectedValue == "LastAccess")
                sSql += " order by siLastAccess desc";
            else if (ddSort.SelectedValue == "Blacklisted")
                sSql += " order by siBlacklisted";
            else 
                sSql += " order by siPeriodHits desc, siLifeHits desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

//            int iYear = Convert.ToInt32(DateTime.Now.Year.ToString());

//            sqlCmd.Parameters.Add("@ThisYear", System.Data.SqlDbType.Int);
//            sqlCmd.Parameters["@ThisYear"].Value = iYear;

            if ((sIpa != null) && (sIpa.Length > 0))
            {
                sqlCmd.Parameters.Add("@SearchIpa", System.Data.SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@SearchIpa"].Value = sIpa + "%";
            }
            else
            {
                //+ DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd") + ")";
                sqlCmd.Parameters.AddWithValue("@DateCutoff", DateTime.Now.AddDays(-14));
            }


            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

            dataTable = new DataTable();
            dataTable.Load(sqlReader);
            sqlCmd.ExecuteReader(CommandBehavior.Default);

            dataTable.Columns.Add(MakeColumn("ToggleText"));
            dataTable.Columns.Add(MakeColumn("Idx"));
            int iIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                iIdx++;
                if (row["siBlacklisted"].ToString().Trim() == "1")
                    row["ToggleText"] = "Clear";
                else
                    row["ToggleText"] = "Blacklist";
                row["Idx"] = iIdx.ToString();
            }

            dataTable.Columns["siKey"].ColumnName = "Key";
            dataTable.Columns["siIpa"].ColumnName = "Ip";
            dataTable.Columns["siPeriodStart"].ColumnName = "PeriodStart";
            dataTable.Columns["siLastAccess"].ColumnName = "LastAccess";
            dataTable.Columns["siPeriodHits"].ColumnName = "PeriodHits";
            dataTable.Columns["siLifeHits"].ColumnName = "LifeHits";
            dataTable.Columns["siTimeoutCount"].ColumnName = "TimeoutCount";
            dataTable.Columns["siBlacklisted"].ColumnName = "Blacklisted";
            dataTable.Columns["siFirstUse"].ColumnName = "FirstUse";
            dataTable.Columns["siUserID"].ColumnName = "UserID";
            dataTable.Columns["siComment"].ColumnName = "Comment";

            dataTable.AcceptChanges();

            rpIpCount.DataSource = dataTable;
            rpIpCount.DataBind();

        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "SQL: " + sSql);
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        // ----------------------------------
    }
    // ================================================================
    protected void lkCommentUpd_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iKey = 0;
        string sIpa = "";
        string sComment = "";

        int.TryParse(saArg[0], out iKey);
        sIpa = saArg[1].ToString().Trim();
        sComment = saArg[2].ToString().Trim();

        // --------------------------------
        lbUpdIp.Text = sIpa;
        txUpdComment.Text = sComment;
        hfUpdKey.Value = iKey.ToString();
        pnCommentUpd.Visible = true;
        txUpdComment.Focus();
    }
    // ================================================================
    protected void btUpdComment_Click(object sender, EventArgs e)
    {
        int iKey = 0;
        int.TryParse(hfUpdKey.Value, out iKey);
        string sComment = txUpdComment.Text.Trim();

        UpdateIpComment(iKey, sComment);

        pnCommentUpd.Visible = false;
        LoadIpHitCount();
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

            pnCommentUpd.Visible = false;
            LoadIpHitCount();
        }
    }
    // ================================================================
    protected void UpdateIpComment(int iKey, string sComment)
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlConnection sqlConn;

        string sSql = "";

        string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

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
        LoadIpHitCount();
    }
    // ================================================================
    // ================================================================
}