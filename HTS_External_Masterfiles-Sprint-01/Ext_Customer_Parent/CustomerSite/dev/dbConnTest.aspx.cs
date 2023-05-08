using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dev_dbConnTest : System.Web.UI.Page
{
    OdbcConnection netSuiteConn;
    OdbcCommand netSuiteCmd;
    OdbcDataReader netSuiteReader;

    string sDebug = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        sDebug = "";

        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        netSuiteConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["NetSuiteConn64"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

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
            sDebug = ex.Message.ToString();
        }
        finally
        {
            netSuiteConn.Dispose();
            netSuiteConn.Close();
        }

    }
}