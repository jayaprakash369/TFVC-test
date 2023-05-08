using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class private__admin_PrintFiles : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    Employee_LIVE.EmployeeMenuSoapClient wsLive = new Employee_LIVE.EmployeeMenuSoapClient();
    Employee_DEV.EmployeeMenuSoapClient wsTest = new Employee_DEV.EmployeeMenuSoapClient();

    string sConnectionString = "";
    string sSql = "";
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            loadDdSql();
        }
    }
    // ========================================================================
    protected void loadDdSql()
    {
        ddSql.DataSource = getOldQueries();
        ddSql.DataValueField = "msQry";
        ddSql.DataTextField = "msQryDisplay";
        ddSql.DataBind();
        ddSql.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
    }
    // ========================================================================
    protected void runSql(string sql)
    {
        SourceForDefaults sfd = new SourceForDefaults();
        if (sDevTestLive == "LIVE")
        {
            gvSql.DataSource = wsLive.PrintFiles(sfd.GetWsKey(), sql);
        }
        else 
        {
            gvSql.DataSource = wsTest.PrintFiles(sfd.GetWsKey(), sql);
        }
        gvSql.DataBind();
    }
    // =========================================================
    protected int saveQuery(string query)
    {
        int iRowsAffected = 0;

        sConnectionString = ConfigurationManager.ConnectionStrings["Employee"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;

        string sSql = "";

        try
        {
            sSql = "Insert into mySqls" +
                " (msDat, msQry)" +
                " values(@Dat, @Qry)";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@Dat", System.Data.SqlDbType.DateTime);
            sqlCmd.Parameters["@Dat"].Value = datTemp;

            sqlCmd.Parameters.Add("@Qry", System.Data.SqlDbType.VarChar);
            sqlCmd.Parameters["@Qry"].Value = query;

            sqlConn.Open();
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
    // =========================================================
    protected DataTable getOldQueries()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        sConnectionString = ConfigurationManager.ConnectionStrings["Employee"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        string sSql = "";

        try
        {
            sSql = "Select top 50 msKey, msDat, msQry, substring(msQry, 0, 300) as msQryDisplay" +
                " from mySqls" +
                " order by msKey desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
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
        return dataTable;
    }
    // ========================================================================
    protected void btSql_Click(object sender, EventArgs e)
    {
        runSql(txSql.Text);
        int iRowsAffected = saveQuery(txSql.Text);
        loadDdSql();
    }
    // ========================================================================
    protected void ddSql_SelectedIndexChanged(object sender, EventArgs e)
    {
        txSql.Text = ddSql.SelectedValue;
    }
    // ========================================================================
    // ========================================================================
}