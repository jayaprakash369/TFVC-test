using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class private__editor_HitCount : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadHitCount();
        }
    }
    // ================================================================
    protected void LoadHitCount()
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlConnection sqlConn;
        SqlDataReader sqlReader;
        DataTable dataTable;

        string sSql = "";

        string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        try
        {
            sqlConn.Open();

            sSql = "Select" + 
                " hcPath as hitPath" + 
                ", hcPage as hitPage" + 
                ", hcCount as hitCount" + 
                ", hcYear as hitYear" + 
                " from HitCount" + 
                " where hcYear = @ThisYear" +
                " and substring(hcPath, 1, 12) not in ('/Ext_Dev_Cst', '/Ext_Tst_Cst', '/Ext_Liv_Cst')" +
                " order by hitCount desc, hitPath";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            int iYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            sqlCmd.Parameters.Add("@ThisYear", System.Data.SqlDbType.Int);
            sqlCmd.Parameters["@ThisYear"].Value = iYear;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

            dataTable = new DataTable();
            dataTable.Load(sqlReader);
            sqlCmd.ExecuteReader(CommandBehavior.Default);

            if (dataTable.Rows.Count > 0)
            {
                rpHitCount.DataSource = dataTable;
                rpHitCount.DataBind();
            }
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
    // ================================================================
}