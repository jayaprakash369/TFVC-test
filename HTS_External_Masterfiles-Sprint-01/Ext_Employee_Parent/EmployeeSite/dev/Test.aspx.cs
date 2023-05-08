using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Web.Security;

public partial class dev_Test : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    //    SqlConnection sqlConn;
    //    SqlCommand sqlCmd;
    //    SqlDataReader sqlReader;

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler eh;
    char[] cSplitter = { '|' };
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        // sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        if (!IsPostBack)
        {
            try
            {
                odbcConn.Open();
                //sqlConn.Open();
                string sTemp = selectData(1862);
            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
                //sqlConn.Close();
            }
        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected string selectData(int key)
    {
        string sTemp = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", key);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            //sqlCmd = new SqlCommand(sSql, sqlConn);
            //sqlCmd.Parameters.AddWithValue("@Key", headerKey);
            //sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            //dt.Load(sqlReader);

            sTemp = "";
            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                sTemp = dt.Rows[iRowIdx]["EMPNAM"].ToString().Trim();
                iRowIdx++;
            }

        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
            //sqlCmd.Dispose();
        }
        return sTemp;
    }
    // ========================================================================
    protected void DoSqls()
    {
        try
        {
            //odbcConn.Open();
            //sqlConn.Open();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
            //sqlConn.Close();
        }
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btAction_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;
        string sParms = myControl.CommandArgument.ToString().Trim();
        char[] cSplitter = { '|' };
        string[] saParms = new string[1];
        saParms = sParms.Split(cSplitter);

        //HyperLink hlUnit = myControl.NamingContainer.FindControl("hlUnit") as HyperLink;
        //TextBox txEndMeter = myControl.NamingContainer.FindControl("txEndMeter") as TextBox;
        //TextBox txPages = myControl.NamingContainer.FindControl("txPages") as TextBox;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            //odbcConn.Open();
            //sqlConn.Open();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
            //sqlConn.Close();
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ========================================================================
}