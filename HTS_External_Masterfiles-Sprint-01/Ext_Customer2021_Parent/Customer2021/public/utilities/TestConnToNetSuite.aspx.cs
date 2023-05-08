using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_utilities_TestConnToNetSuite : MyPage
{
    OdbcConnection netSuiteConn;
    OdbcCommand netSuiteCmd;
    OdbcDataReader netSuiteReader;

    // -----------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Connect();
    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // -------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region mySqls
    // ========================================================================
    protected void Connect()
    {
        string sDebug = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        netSuiteConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["NetSuiteConn64"].ConnectionString);
       

        try
        {
            netSuiteConn.Open();

            sDebug = "Just looking after the connection";


            string sSql = "SELECT * from customer";

            netSuiteCmd = new OdbcCommand(sSql, netSuiteConn);
            //netSuiteCmd.Parameters.AddWithValue("@Key", key);
            using (netSuiteReader = netSuiteCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(netSuiteReader);
            }

            foreach (DataRow row in dt.Rows)
            {
                //sTemp = row["Temp"].ToString().Trim();
                sDebug = "Trying see values in the rows";
            }


        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Steve Testing");
            myPage = null;
        }
        finally
        {
        }

    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // -----------------------------------------------------------


    // -----------------------------------------------------------
    // -----------------------------------------------------------

}