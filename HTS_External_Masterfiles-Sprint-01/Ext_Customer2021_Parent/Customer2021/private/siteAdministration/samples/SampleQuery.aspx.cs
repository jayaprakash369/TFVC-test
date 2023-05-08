using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;

public partial class private_siteAdministration_samples_SampleQuery : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    //SqlConnection sqlConn;
    //SqlCommand sqlCmd;
    //SqlDataReader sqlReader;

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sTemp = "";
    
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        // Overriding the "MyPage" sLibrary to ensure I'm looking at live in this program
        sLibrary = "OMDTALIB";
    }
    // -------------------------------------------------
    protected void LoadDataTables()
    {
        DataTable dt = new DataTable("Query");
        string sName = txSearchName.Text.Trim().ToUpper();

        if (!String.IsNullOrEmpty(sName))
        {
            try
            {
                odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
                //sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                //sqlConn.Open();
                odbcConn.Open();
                dt = query400Database(sName);

                rp_Small.DataSource = dt;
                rp_Small.DataBind();

                rp_Large.DataSource = dt;
                rp_Large.DataBind();
            }
            catch (Exception ex)
            {
                sTemp = ex.Message.ToString();
            }
            finally
            {
                //sqlConn.Close();
                odbcConn.Close();
            }
        }

    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable query400Database(string employeeName)
    {
        DataTable dt = new DataTable("Query");
        string sSql = "";

        if (!String.IsNullOrEmpty(employeeName))
        {
            try
            {
                sSql = "Select" +
                 " EMPNAM" +
                ", EMPNUM" +
                ", ECITY" +
                ", ESTATE" +
                //", IFNULL((select PEMLIF from " + sLibrary + ".PRODEQP where PEPART = c.cPartName and PEPART <> ''),0) as cartridgePages" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNAM like ?" +
                " order by EMPNAM";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Nam", employeeName + "%");
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                // dt.Columns.Add(MakeColumn("New"));

                sTemp = "";
                foreach (DataRow row in dt.Rows)
                {
                    sTemp = row["EMPNAM"].ToString().Trim();
                }

                //dt.Columns["CKEY"].ColumnName = "Key";
                // Remove unnecessary columns
                //dt.Columns.Remove("WEERR");
                // dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                sTemp = ex.Message.ToString();
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }

        return dt;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ========================================================================
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchSubmit_Click(object sender, EventArgs e)
    {
        //Button myControl = (Button)sender;
        //string sParms = myControl.CommandArgument.ToString().Trim();
        //string[] saParms = new string[1];
        //saParms = sParms.Split('|');
            
        LoadDataTables();
    }

    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
