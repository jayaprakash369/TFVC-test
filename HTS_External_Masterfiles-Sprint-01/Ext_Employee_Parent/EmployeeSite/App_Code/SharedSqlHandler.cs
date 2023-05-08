using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for SharedSqlHandler
/// </summary>
public class SharedSqlHandler
{
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sSql = "";
    string sLibrary = "";

    // ========================================================================
    public SharedSqlHandler()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
    }
    // ==================================================
    public void SaveUserIp(int user, string ip)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);  

        try
        {
            odbcConn.Open();

            sSql = "Update " + sLibrary + ".ANUSERS set" +
                " AUIPADDR = ?" +
                " where AUEMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ip", ip);
            odbcCmd.Parameters.AddWithValue("@User", user);
            odbcCmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "DMZ Shared Sql Handler");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        // ---------------------------------------
    }
    // ========================================================================
    // ========================================================================
}