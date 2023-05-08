using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for KeyHandler
/// </summary>
public class KeyHandler
{
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sSql = "";
    string sErrMessage = "";
    string sLibrary = "";

    // ========================================================================
    public KeyHandler()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
    }
    // ========================================================================
    public int MakeNewKey(string targetFilename)
    {
        // Set here once for all page SQLs
        string sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        int iNewKey = 0;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            // ---------------------------------------------------
            // Get Old Key from KEYMAST
            // ---------------------------------------------------
            sSql = "select kmkey" +
                " from " + sLibrary + ".KEYMAST" +
                " where kmfil = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Filename", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@Filename"].Value = targetFilename;

            odbcConn.Open();
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            iNewKey = ReadAndIncrementKey(dt, targetFilename);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }

        //--------------------------------
        return iNewKey;
    }
    // ========================================================================
    public int ReadAndIncrementKey(DataTable dt, string targetFilename)
    {
        string sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        int iOldKey = 0;
        int iNewKey = 0;

        if (dt.Rows.Count > 0)
        {
            if (int.TryParse(dt.Rows[0]["kmkey"].ToString(), out iOldKey) == false)
                iOldKey = 0;
            if (iOldKey > 0)
            {
                iNewKey = iOldKey + 1;

                try
                {
                    // -----------------------------------
                    // Save new key back to file as last used 
                    // -----------------------------------
                    sSql = "update " + sLibrary + ".KEYMAST" +
                        " set KMKEY = ?" +
                        " where kmfil = ?";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.AddWithValue("@IncrementedKey", iNewKey);
                    odbcCmd.Parameters.AddWithValue("@Filename", targetFilename);

                    odbcConn.Open();
                    int iRowsAffected = odbcCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                }
                finally
                {
                    odbcCmd.Dispose();
                    odbcConn.Close();
                }
            }
        }
        return iNewKey;
    }
    // ========================================================================
    // ========================================================================
}