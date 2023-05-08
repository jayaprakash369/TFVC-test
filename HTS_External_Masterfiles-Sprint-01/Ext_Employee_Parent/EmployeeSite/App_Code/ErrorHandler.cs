using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for ErrorHandler
/// </summary>
public class ErrorHandler
{
    SqlConnection sqlConn;
    SqlCommand sqlCmd;
    SqlDataReader sqlReader;

    DateTime datToday;

    string sSql = "";
    string sErrMessage = "";
    string sLibrary = "";
    string sWebsite = "";
    string sSqlDbToUse = "";

    int iRowsAffected = 0;

    // ========================================================================
    public ErrorHandler()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        if (sLibrary == "OMDTALIB")
        {
            sSqlDbToUse = "Workfiles_LIVE.dbo";
            sWebsite = "EMP EXT LIVE";
        }
        else
        {
            sSqlDbToUse = "Workfiles_TEST.dbo";
            sWebsite = "EMP EXT DEV";
        }
        datToday = DateTime.Now;

    }
    // ==================================================
    public void SaveErrorText(string errSummary, string errDescription, string errValues)
    {
        string sErrSummary = errSummary;
        sErrSummary = sErrSummary.Replace("<", "<`");
        sErrSummary = sErrSummary.Replace(">", "`>");
        string sErrDescription = errDescription;
        sErrDescription = sErrDescription.Replace("<", "<`");
        sErrDescription = sErrDescription.Replace(">", "`>");
        string sErrValues = errValues;
        sErrValues = sErrValues.Replace("<", "<`");
        sErrValues = sErrValues.Replace(">", "`>");

        string sUser = "";
        string sIpAddress = "";

        try
        {
            sUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
            sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            DateTime datToday = DateTime.Now;

            sSql = "insert into " + sSqlDbToUse + ".WebErrLog (" +
                  "weSummary" +
                ", weDescription" +
                ", weValues" +
                ", weSource" +
                ", weCreated" +
                ", weUser" +
                ", weIpAddress" +
                ") VALUES (" +
                  "@Summary" +
                ", @Description" +
                ", @Values" +
                ", @Source" +
                ", @Created" +
                ", @User" +
                ", @IpAddress" +
                ")";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            if (!String.IsNullOrEmpty(sErrSummary) && sErrSummary.Length > 450)
                sErrSummary = sErrSummary.Substring(0, 450);

            if (!String.IsNullOrEmpty(sWebsite) && sWebsite.Length > 100)
                sWebsite = sWebsite.Substring(0, 100);

            if (!String.IsNullOrEmpty(sUser) && sUser.Length > 100)
                sUser = sUser.Substring(0, 100);

            if (!String.IsNullOrEmpty(sIpAddress) && sIpAddress.Length > 100)
                sIpAddress = sIpAddress.Substring(0, 100);

            sqlCmd.Parameters.AddWithValue("@Summary", sErrSummary);
            sqlCmd.Parameters.AddWithValue("@Description", sErrDescription);
            sqlCmd.Parameters.AddWithValue("@Values", sErrValues);
            sqlCmd.Parameters.AddWithValue("@Source", sWebsite);
            sqlCmd.Parameters.AddWithValue("@Created", DateTime.Now.ToString("o"));
            sqlCmd.Parameters.AddWithValue("@User", sUser);
            sqlCmd.Parameters.AddWithValue("@IpAddress", sIpAddress);

            sqlConn.Open();
            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            
            if (DateTime.Now.Hour >= 3 && DateTime.Now.Hour <= 22) 
            {
                EmailHandler emh = new EmailHandler();
                emh.EmailIndividual(
                    "EXT Emp ErrorHandler crash while saving error to sql db"
                    , " *** Crash: " + ex.Message.ToString() + "\r\n\r\n*** Summary: " + sErrSummary + "\r\n\r\n *** Description: " + sErrDescription + "\r\n\r\n*** Values: " + sErrValues
                    , "steve.carlson@scantron.com"
                    , "adv320@scantron.com"
                    , "HTML");
                emh = null;
            }
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
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