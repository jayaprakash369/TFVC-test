using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for ErrorHandler
/// </summary>
public class ErrorHandler
{
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    KeyHandler kh;
    string sSql = "";
    string sErrMessage = "";
    //string sErrValues = "";
    string sLibrary = "";

    // ========================================================================
    public ErrorHandler()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
    }
    // ==================================================
    public void SaveErrorText(string errSummary, string errMessage, string errValues)
    {
        // Set here once for all page SQLs
        string sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        try
        {
            string sTargetFilename = "WEBERRLOG";
            string sUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
            string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            kh = new KeyHandler();
            int iNewKey = kh.MakeNewKey(sTargetFilename);

            DateTime datToday = new DateTime();
            datToday = DateTime.Now;
            int iDateToday = Int32.Parse(datToday.ToString("yyyyMMdd"));
            int iTimeToday = Int32.Parse(datToday.ToString("HHmmss"));
            string sWebsite = "";

            if (sLibrary == "OMDTALIB")
                sWebsite = "GENERIC DMZ LIVE";
            else
                sWebsite = "GENERIC DMZ DEV";

            sSql = "insert into " + sLibrary + ".WEBERRLOG" +
                " (WEKEY, WEDAT, WETIM, WEUSR, WEIPA, WEERR, WEDSC, WEVAL, WEWEB)" +
                " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@NewKey", iNewKey);
            odbcCmd.Parameters.AddWithValue("@Date", iDateToday);
            odbcCmd.Parameters.AddWithValue("@Time", iTimeToday);
            odbcCmd.Parameters.AddWithValue("@User", "");
            odbcCmd.Parameters.AddWithValue("@IpAddress", "");
            odbcCmd.Parameters.AddWithValue("@ErrSummary", errSummary);
            odbcCmd.Parameters.AddWithValue("@ErrMessage", errMessage);
            odbcCmd.Parameters.AddWithValue("@ErrValues", errValues);
            odbcCmd.Parameters.AddWithValue("@WebSource", sWebsite);

            odbcConn.Open();
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = "ERROR SAVING ERROR TEXT --> " + ex.ToString();
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        // ---------------------------------------
    }
    // ========================================================================
    public string KeyboardCharactersOnly(string stringIn, int maxLength)
    {
        string sRtn = "";
        string sKeyboardChararacters = "0123456789 abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";

        if ((stringIn != null) && (stringIn != ""))
        {
            sRtn = stringIn.ToString().Trim();

            // 1) Trim to max length
            int iLength = sRtn.Length;
            if (iLength > maxLength)
            {
                sRtn = sRtn.Substring(0, maxLength);
                iLength = maxLength;
            }
            // 2) Ensure each character is a keyboard character
            string sChar = "";
            int iPos = 0;
            for (int i = 0; i < iLength; i++)
            {
                sChar = sRtn.Substring(i, 1);
                try
                {
                    iPos = sKeyboardChararacters.IndexOf(sChar);
                    if ((iPos < 0) || ((iPos == 0) && (sChar != "0")))
                        sRtn = sRtn.Replace(sChar, " ");
                }
                catch (Exception ex)
                {
                    string sEx = ex.ToString();
                    sRtn = sRtn.Replace(sChar, " ");
                }
            }
        }

        return sRtn;
    }
    // ========================================================================
    // ========================================================================
}