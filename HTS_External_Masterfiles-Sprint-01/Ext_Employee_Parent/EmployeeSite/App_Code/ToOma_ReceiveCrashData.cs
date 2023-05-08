using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for ToOma_ReceiveCrashData
/// </summary>
public class ToOma_ReceiveCrashData
{
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler erh;
    EmailHandler emh;

    string sConnectionString = "";
    string sSql = "";
    string sLibrary = "";

    // ========================================================================
    // Constructor
    // ========================================================================
    public ToOma_ReceiveCrashData()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
    }
    // ========================================================================
    // PUBLIC METHODS
    // ========================================================================
    public string ProcessCrash(int user, string crashErr, string crashLog, DateTime crashDate)
    {
        string sResponseToDevice = "TRY_AGAIN|DEFAULT_CRASH_PROCESSING|";
        int iRowsAffected = 0;

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);
        
        erh = new ErrorHandler();
        // emh = new EmailHandler();
        try
        {
            odbcConn.Open();

            iRowsAffected = AddCrash(user, crashErr, crashLog, crashDate);

            if (iRowsAffected > 0) {
                sResponseToDevice = "FINISHED|DEBUG|Crash Data Saved|";
            }
            else
                sResponseToDevice = "FINISHED|DEBUG|ERROR: Crash Save Failed|"; 

        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            sResponseToDevice = "FINISHED|DEBUG|" + ex.Message.ToString() + "|"; 
        }
        finally
        {
            odbcConn.Close();
            erh = null;
        }
        
        return sResponseToDevice;
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    private int AddCrash(int user, string crashErr, string crashLog, DateTime crashDate)
    {
        int iRowsAffected = 0;
        if (!String.IsNullOrEmpty(crashErr) && crashErr.Length > 900)
            crashErr = crashErr.Substring(0, 890);
        if (!String.IsNullOrEmpty(crashLog) && crashLog.Length > 9000)
            crashLog = crashLog.Substring(0, 8990);

        try
        {

            crashErr = crashErr.Replace("&gt;", ">");
            crashErr = crashErr.Replace("&lt;", "<");
            crashErr = crashErr.Replace("~`", "|");
            crashErr = crashErr.Replace("%2B", "+");
            crashErr = crashErr.Replace("%26", "&");

            crashLog = crashLog.Replace("&gt;", ">");
            crashLog = crashLog.Replace("&lt;", "<");
            crashLog = crashLog.Replace("~`", "|");
            crashLog = crashLog.Replace("%2B", "+");
            crashLog = crashLog.Replace("%26", "&");

            sSql = "insert into " + sLibrary + ".ANCRASH (" +
                  "CUSER" + 
                ", CERR" +
                ", CLOG" + 
                ", CDATE" + 
                ") values(?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@User", user);
            odbcCmd.Parameters.AddWithValue("@Err", crashErr);
            odbcCmd.Parameters.AddWithValue("@Log", crashLog);
            odbcCmd.Parameters.AddWithValue("@Date", crashDate.ToString("o"));

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            iRowsAffected = -1;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    // ========================================================================
}