using System;
using System.Collections.Generic;
using System.Web;

using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Odbc;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// MyPage overrides the standard Page class
/// </summary>
public class MyPage : System.Web.UI.Page
{
    public string sMyPublicVariable = "";
    // ==================================================
    private bool _RequireSSL;
    public string sDevTestLive = "";
    public string sLibrary = "";
    public string sSqlDbToUse_Customer = "";
    public string sSqlDbToUse_Error = "";

    string sEncryptOrig = "0123456789=abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string sEncryptSwap = "g8TOrDuGwXRIdMy1An3EbQ6l4VoY9jsZ0aKi7efWp2HcxP=NhBqJk5zSLvUCFtm";

    SqlConnection sqlConn_MyPage;
    SqlCommand sqlCmd_MyPage;
    SqlDataReader sqlReader_MyPage;

    Customer2021_LIVE.Customer2021MenuSoapClient wsLive = new Customer2021_LIVE.Customer2021MenuSoapClient();
    Customer2021_DEV.Customer2021MenuSoapClient wsDev = new Customer2021_DEV.Customer2021MenuSoapClient();

    // ==================================================
    public MyPage()
    {
        Set_Environment();

        // DEBUG: OVERRIDE TO SPECIFIC LIBRARY
        //sDevTestLive = "LIVE";
        //sSqlDbToUse_Error = "Workfiles_LIVE.dbo";
        //sSqlDbToUse_Customer = "CustomerLegacy_LIVE.dbo";
        //sLibrary = "OMDTALIB";

    }
    // ========================================================================
    #region WebServiceCalls: Core
    // ========================================================================
    protected string Call_WebService_ForString(string jobName, string fieldList, string valueList)
    {
        string sResponse = "";

        if (sLibrary == "OMDTALIB")
        {
            sResponse = wsLive.ProcessJob_Return_String(Get_WebServiceKey(jobName), jobName, fieldList, valueList);
        }
        else
        {
            sResponse = wsDev.ProcessJob_Return_String(Get_WebServiceKey(jobName), jobName, fieldList, valueList);
        }

        return sResponse;
    }
    // -------------------------------------------------------------------------
    protected DataTable Call_WebService_ForDataTable(string jobName, string fieldList, string valueList)
    {
        DataTable dt = new DataTable("");

        if (sLibrary == "OMDTALIB")
        {
            dt = wsLive.ProcessJob_Return_DataTable(Get_WebServiceKey(jobName), jobName, fieldList, valueList);
        }
        else
        {
            dt = wsDev.ProcessJob_Return_DataTable(Get_WebServiceKey(jobName), jobName, fieldList, valueList);
        }

        return dt;
    }
    // ========================================================================
    #endregion // end WebServiceCalls: Core
    // ========================================================================
    // ========================================================================
    #region WebServiceCalls: Global Use
    // ========================================================================
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CustomerName(string customerNumber, string customerLocation)
    {
        string sCustomerName = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustomerName";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber + "|" + customerLocation + "|x";

            sCustomerName = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCustomerName;
    }
    // ----------------------------------------------------------------------------------------------------
    protected string ws_Get_B1AccountName(string accountPrefix, string accountNumber, string accountLocation)
    {
        string sAccountName = "";

        if (!String.IsNullOrEmpty(accountNumber))
        {
            string sJobName = "Get_B1AccountName";
            string sFieldList = "accountPrefix|accountNumber|accountLocation|x";
            string sValueList = accountPrefix + "|" + accountNumber + "|" + accountLocation + "|x";

            sAccountName = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAccountName;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustomerType(int customerNumber) // REG, LRG, DLR, SS, SUB, BAK
    {
        string sCustomerType = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustomerType";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber.ToString() + "|" + "0" + "|x";

            sCustomerType = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCustomerType;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustomerTypeForSqlUserTable(string registrationPrefix, int registrationNumber) // REG, LRG, DLR, SS, SUB, BAK, SRG, SRP, SRC
    {
        string sCustomerType = "";

        if (registrationNumber > 0)
        {
            string sJobName = "Get_B1CustomerTypeForSqlUserTable";
            string sFieldList = "registrationPrefix|registrationNumber|x";
            string sValueList = registrationPrefix + "|" + registrationNumber + "|x";

            sCustomerType = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCustomerType;
    }
    // ------------------------------------------------------------------------
    protected DataTable ws_Get_B1CustomerLocationDetail(
        string customerNumber,
        string customerLocation,
        string customerName,
        string contact,
        string address,
        string city,
        string state,
        string zip,
        string phone,
        string xref
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustomerLocationDetail";
            string sFieldList = "number|location|name|contact|address|city|state|zip|phone|xref|x";
            string sValueList =
                customerNumber.ToString() + "|" +
                customerLocation.ToString() + "|" +
                customerName.ToString() + "|" +
                contact.ToString() + "|" +
                address.ToString() + "|" +
                city.ToString() + "|" +
                state.ToString() + "|" +
                zip.ToString() + "|" +
                phone.ToString() + "|" +
                xref.ToString() + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustomerFamilyMemberNameAndNumberList(int customerNumber)
    {
        string sFamilyMemberList = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustomerFamilyMemberNameAndNumberList";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sFamilyMemberList = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sFamilyMemberList;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_AllowEmailManagement_YN(string customerNumber)
    {
        string sAllowEmailManagement_YN = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustPref_AllowEmailManagement_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

            sAllowEmailManagement_YN = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAllowEmailManagement_YN;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_AllowTicketMapping_YN(string customerNumber)
    {
        string sAllowTicketMapping_YN = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustPref_AllowTicketMapping_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

            sAllowTicketMapping_YN = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sAllowTicketMapping_YN;
    }
    // ------------------------------------------------------------------------
    protected string ws_Get_B1CustPref_UserRegistrationForCustomer_OpenOrClosed(int customerNumber) // "Open" or "Closed"  returned
    {
        string sIsUserRegistrationForCustomerOpenOrClosed = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustPref_UserRegistrationForCustomer_OpenOrClosed";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sIsUserRegistrationForCustomerOpenOrClosed = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sIsUserRegistrationForCustomerOpenOrClosed;
    }
    // ------------------------------------------------------------------------
    protected string ws_Upd_B1CustPref_ToggleRegistrationToOpenOrClosed(string customerNumber, string openOrClosed) // "Open" or "Closed"  returned
    {
        string sSuccessOrFailure = "";

        if (
            !String.IsNullOrEmpty(customerNumber) 
            && (openOrClosed.ToLower() == "open" || openOrClosed.ToLower() == "closed")
            )
        {
            string sJobName = "Upd_B1CustPref_ToggleRegistrationToOpenOrClosed";
            string sFieldList = "customerNumber|openOrClosed|x";
            string sValueList = customerNumber.ToString() + "|" + openOrClosed + "|x";

            sSuccessOrFailure = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sSuccessOrFailure;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1DetailForSelectedUnits(
        string unitList,
        string agreementList
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(unitList))
        {
            string sJobName = "Get_B1DetailForSelectedUnits";
            string sFieldList = "unitList|agreementList|x";
            string sValueList =
                unitList + "|" +
                agreementList + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ----------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B2DetailForSelectedUnits(
        string unitList,
        string agreementList
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(unitList))
        {
            string sJobName = "Get_B2DetailForSelectedUnits";
            string sFieldList = "unitList|agreementList|x";
            string sValueList =
                unitList + "|" +
                agreementList + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    #endregion // WebServiceCalls: Global Use
    // ========================================================================
    #region mySqls
    // ========================================================================
    public string GetEmpName(int empNum)
    {
        OdbcConnection odbcConn;
        OdbcCommand odbcCmd;
        OdbcDataReader odbcReader;

        string sSql = "";

        string sEmpName = "";

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@EmpNum", empNum);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            odbcCmd.Dispose();

            if (dt.Rows.Count > 0)
                sEmpName = dt.Rows[0]["EMPNAM"].ToString().Trim();
        }
        catch (Exception ex)
        {
            string sDebug = ex.Message.ToString();
            //ErrorHandler erh = new ErrorHandler();
            //erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            //erh = null;
        }
        finally
        {

            odbcConn.Close();
        }
        return sEmpName;
    }
    // ========================================================================
    public int SaveError(string errorSummary, string errorDescription, string errorValues)
    {

        int iRowsAffected = 0;
        int iSummaryLength = 0;
        int iDescriptionLength = 0;
        int iValuesLength = 0;
        sqlConn_MyPage = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        string sSourceSite = "";
        string sSql = "";
        string sUser = "";
        string sIpAddress = "";

        if (!String.IsNullOrEmpty(errorSummary))
            iSummaryLength = errorSummary.Length;
        if (!String.IsNullOrEmpty(errorDescription))
            iDescriptionLength = errorDescription.Length;
        if (!String.IsNullOrEmpty(errorValues))
            iValuesLength = errorValues.Length;

        // The "x"s are my attempt to disable executable script (display on my html page) but still see what's they are doing
        string sErrorSummary = errorSummary.Trim();
        sErrorSummary = sErrorSummary.Replace("<", "<.");
        sErrorSummary = sErrorSummary.Replace(">", ".>");

        string sErrorDescription = errorDescription.Trim();
        sErrorDescription = sErrorDescription.Replace("<", "<.");
        sErrorDescription = sErrorDescription.Replace(">", ".>");

        string sErrorValues = errorValues.Trim();
        sErrorValues = sErrorValues.Replace("<", "<.");
        sErrorValues = sErrorValues.Replace(">", ".>");

        if (sDevTestLive == "LIVE")
        {
            sSqlDbToUse_Error = "Workfiles_LIVE.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_LIVE.dbo";
            sSourceSite = "CUSTOMER_2021_DMZ_LIVE";
        }
        else
        {
            sSqlDbToUse_Error = "Workfiles_TEST.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_TEST.dbo";
            if (sDevTestLive == "TEST")
                sSourceSite = "CUSTOMER_2021_DMZ_TEST";
            else
                sSourceSite = "CUSTOMER_2021_DMZ_DEV";
        }

        try
        {
            errorSummary = HttpUtility.HtmlEncode(errorSummary);
            errorDescription = HttpUtility.HtmlEncode(errorDescription);
            errorValues = HttpUtility.HtmlEncode(errorValues);

            sUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
            sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            DateTime datToday = new DateTime();
            datToday = DateTime.Now;

            sSql = "insert into " + sSqlDbToUse_Error + ".WebErrLog (" +
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

            sqlCmd_MyPage = new SqlCommand(sSql, sqlConn_MyPage);

            if (!String.IsNullOrEmpty(errorSummary) && errorSummary.Length > 450)
                errorSummary = errorSummary.Substring(0, 450);
            sqlCmd_MyPage.Parameters.AddWithValue("@Summary", sErrorSummary);
            
            if (!String.IsNullOrEmpty(sErrorDescription) && sErrorDescription.Length > 15000)
                sErrorDescription = sErrorDescription.Substring(0, 15000);
            sqlCmd_MyPage.Parameters.AddWithValue("@Description", sErrorDescription);
            
            if (!String.IsNullOrEmpty(sErrorValues) && sErrorValues.Length > 15000)
                sErrorValues = sErrorValues.Substring(0, 15000);
            sqlCmd_MyPage.Parameters.AddWithValue("@Values", sErrorValues);
            
            if (!String.IsNullOrEmpty(sSourceSite) && sSourceSite.Length > 99)
                sSourceSite = sSourceSite.Substring(0, 99);
            sqlCmd_MyPage.Parameters.AddWithValue("@Source", sSourceSite);

            sqlCmd_MyPage.Parameters.AddWithValue("@Created", DateTime.Now.ToString("o"));
            
            if (!String.IsNullOrEmpty(sUser) && sUser.Length > 99)
                sUser = sUser.Substring(0, 99);
            sqlCmd_MyPage.Parameters.AddWithValue("@User", sUser);
            
            if (!String.IsNullOrEmpty(sIpAddress) && sIpAddress.Length > 99)
                sIpAddress = sIpAddress.Substring(0, 99);
            sqlCmd_MyPage.Parameters.AddWithValue("@IpAddress", sIpAddress);

            sqlConn_MyPage.Open();

            iRowsAffected = sqlCmd_MyPage.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            if (DateTime.Now.Hour >= 3 && DateTime.Now.Hour <= 22)
                SendEmail(
                    "Cust 2021 MyPage/Save Error Crashed During Insert: ",
                    " *** Summary Passed In: " + iSummaryLength + "\r\n" + 
                    sErrorSummary +
                    "\r\n\r\n *** Description Passed In: " + iDescriptionLength + "\r\n " + 
                    sErrorDescription +
                    "\r\n\r\n*** Values Passed In: " + iValuesLength + "\r\n " + 
                    sErrorValues + 
                    "\r\n\r\n *** Crash ex.Message text:\r\n" + 
                    ex.Message.ToString() + 
                    "\r\n\r\n *** User: " + sUser,
                    "steve.carlson@scantron.com", 
                    "adv320@scantron.com");
        }
        finally
        {
            sqlCmd_MyPage.Dispose();
            sqlConn_MyPage.Close();
        }

        return iRowsAffected = 0;
    }
    // =========================================================
    //public string[] Get_UserCustomerNumber(string username) // better name change needed
    public string[] Get_UserAccountIds(string username) 
    {
        string[] saPreNumTyp = { "", "", "" }; // Prefix, Account Number, Account Type
        //int iCustomerNumber = 0;

        string sLoweredUsername = username.ToLower();

        string sSql = "";

        sqlConn_MyPage = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn_MyPage.Open();

            sSql = "Select" +
                 " PrimaryPrefix" +
                ", PrimaryCs1" +
                ", CompanyType" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u" +
                " where LoweredUsername = @LoweredUserName";

            sqlCmd_MyPage = new SqlCommand(sSql, sqlConn_MyPage);

            sqlCmd_MyPage.Parameters.AddWithValue("@LoweredUserName", sLoweredUsername);

            using (sqlReader_MyPage = sqlCmd_MyPage.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader_MyPage);
                if (dt.Rows.Count > 0)
                {
                    saPreNumTyp[0] = dt.Rows[0]["PrimaryPrefix"].ToString().Trim();
                    saPreNumTyp[1] = dt.Rows[0]["PrimaryCs1"].ToString().Trim();
                    saPreNumTyp[2] = dt.Rows[0]["CompanyType"].ToString().Trim();
                    //if (int.TryParse(dt.Rows[0]["PrimaryCs1"].ToString().Trim(), out iCustomerNumber) == false)
                    //    iCustomerNumber = -1;
                }
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd_MyPage.Dispose();
            sqlConn_MyPage.Close();
        }

        return saPreNumTyp;
    }
    // =========================================================
    public string Get_UserCompanyTypeById(string registrationPrefix, int registrationNumber)
    {
        string sCompanyType = "";

        string sSql = "";

        sqlConn_MyPage = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn_MyPage.Open();

            sSql = "Select" +
                 " CompanyType" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u" +
                " where PrimaryPrefix = @PrimaryPrefix" + 
                " and PrimaryCs1 = @PrimaryCs1";

            sqlCmd_MyPage = new SqlCommand(sSql, sqlConn_MyPage);

            sqlCmd_MyPage.Parameters.AddWithValue("@PrimaryPrefix", registrationPrefix);
            sqlCmd_MyPage.Parameters.AddWithValue("@PrimaryCs1", registrationNumber);

            using (sqlReader_MyPage = sqlCmd_MyPage.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader_MyPage);
                if (dt.Rows.Count > 0)
                {
                    sCompanyType = dt.Rows[0]["CompanyType"].ToString().Trim();
                }
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd_MyPage.Dispose();
            sqlConn_MyPage.Close();
        }

        return sCompanyType;
    }
    // =========================================================
    public string Get_UserCompanyType(string username)
    {
        string sCompanyType = "";
        string sLoweredUsername = username.ToLower();
        string sSql = "";

        sqlConn_MyPage = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn_MyPage.Open();

            sSql = "Select" +
                 " CompanyType" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u" +
                " where LoweredUsername = @LoweredUserName";

            sqlCmd_MyPage = new SqlCommand(sSql, sqlConn_MyPage);

            sqlCmd_MyPage.Parameters.AddWithValue("@LoweredUserName", sLoweredUsername);

            using (sqlReader_MyPage = sqlCmd_MyPage.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader_MyPage);
                if (dt.Rows.Count > 0)
                {
                    sCompanyType = dt.Rows[0]["CompanyType"].ToString().Trim();
                }
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd_MyPage.Dispose();
            sqlConn_MyPage.Close();
        }

        return sCompanyType;
    }
    // =========================================================================
    public static Boolean ShowNewCompanyName(string library)
    {
        Boolean bShowNewName = false;

        OdbcConnection odbcConn;
        OdbcCommand odbcCmd;
        OdbcDataReader odbcReader;

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        int iDateToday = 0;
        int iDateToSwitch = 0;

        if (int.TryParse(DateTime.Now.ToLocalTime().ToString(), out iDateToday) == false)
            iDateToday = 0;

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " NDDATE" +
                " from " + library + ".NEWNAMEDT" +
                " where NDKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", 1);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["NDDATE"].ToString().Trim(), out iDateToSwitch) == false)
                    iDateToSwitch = 0;
            }
            if (iDateToday > 0
                && iDateToSwitch > 0
                && iDateToday >= iDateToSwitch)
                bShowNewName = true;

            odbcCmd.Dispose();
        }
        catch (Exception ex)
        {
            string sDebug = ex.ToString();
        }
        finally
        {
            odbcConn.Close();
        }
        return bShowNewName;
    }
    // =========================================================================
    public Boolean ShowNewCompanyName()
    {
        Boolean bShowNewName = false;

        OdbcConnection odbcConn;
        OdbcCommand odbcCmd;
        OdbcDataReader odbcReader;

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        int iDateToday = 0;
        int iDateToSwitch = 0;

        if (int.TryParse(DateTime.Now.ToLocalTime().ToString("yyyyMMdd"), out iDateToday) == false)
            iDateToday = 0;

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " NDDATE" +
                " from " + sLibrary + ".NEWNAMEDT" +
                " where NDKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", 1);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["NDDATE"].ToString().Trim(), out iDateToSwitch) == false)
                    iDateToSwitch = 0;
            }
            if (iDateToday > 0
                && iDateToSwitch > 0
                && iDateToday >= iDateToSwitch)
                bShowNewName = true;
            
            odbcCmd.Dispose();
        }
        catch (Exception ex)
        {
            string sDebug = ex.ToString();
        }
        finally
        {
            
            odbcConn.Close();
        }
        return bShowNewName;
    }

    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    public string Get_WebServiceKey(string job)
    {
        string sWebServiceKey = "";
        // ---------------------------------------------
        // a) create a string of keyboard characters that may appear in the job string
        string sKeyboardCharacters = "cegouq_xmjilaztwhfnksrbypvd";
        // b) get an integer for the julian day of the year (this is to ensure the result will be different on each day of the year)

        int iJul = DateTime.Now.DayOfYear;

        // c) loop though each character in the inbound job string, 
        //      match to the position in the keyboard string
        //      retrieve the number for the integer array element 
        //      add for a numeric total.

        int i = 0;
        int iTotal = 0;
        int iPos = 0;
        string sChar = "";

        for (i = 0; i < job.Length; i++)
        {
            sChar = job.Substring(i, 1);
            iPos = sKeyboardCharacters.IndexOf(sChar.ToLower());
            if (iPos > -1)
            {
                iTotal += iPos + iJul;
            }
        }

        sWebServiceKey = iTotal.ToString();

        // ---------------------------------------------
        return sWebServiceKey;
    }
    // ========================================================================
    public string GetTicketEncrypted(int ctr, int tck)
    {
        string sEncrypted = "";

        string sCtr = ctr.ToString();
        string sTck = tck.ToString();
        string sCtrTck = "";
        string[] saCode = new string[10];

        for (int i = 0; i < 7; i++)
        {
            if (sCtr.Length < 3)
                sCtr = "0" + sCtr;
            if (sTck.Length < 7)
                sTck = "0" + sTck;
        }

        sCtrTck = sCtr + sTck;
        saCode[0] = "aBcDeFgHiJ";
        saCode[1] = "kLmNoPqRsT";
        saCode[2] = "uVwXyZAbCd";
        saCode[3] = "EfGhIjKlMn";
        saCode[4] = "OpQrStUvWx";
        saCode[5] = "YzaBcDeFgH";
        saCode[6] = "iJkLmNoPqR";
        saCode[7] = "sTuVwXyZAb";
        saCode[8] = "CdEfGhIjKl";
        saCode[9] = "MnOpQrStUv";

        string sNums = "5820416937";
        int iTckNum = 0;
        string sReplacementChar = "";
        sEncrypted = sNums;

        for (int i = 0; i < 10; i++)
        {
            // for each pos 1-10 get number in ticket
            if (int.TryParse(sCtrTck.Substring(i, 1), out iTckNum) == false)
                iTckNum = 0;
            // replacement character = a) array of codes[loop num] b) character at replacement position
            sReplacementChar = saCode[i].Substring(iTckNum, 1);
            // use .Replace to move the replacement character to the new position in encrypted value            
            sEncrypted = sEncrypted.Replace(i.ToString(), sReplacementChar);
        }

        return sEncrypted;
    }
    // ========================================================================
    public int[] GetTicketDecrypted(string encrypted)
    {
        int[] iaCtrTck = new int[2];
        string[] saCode = new string[10];

        saCode[0] = "aBcDeFgHiJ";
        saCode[1] = "kLmNoPqRsT";
        saCode[2] = "uVwXyZAbCd";
        saCode[3] = "EfGhIjKlMn";
        saCode[4] = "OpQrStUvWx";
        saCode[5] = "YzaBcDeFgH";
        saCode[6] = "iJkLmNoPqR";
        saCode[7] = "sTuVwXyZAb";
        saCode[8] = "CdEfGhIjKl";
        saCode[9] = "MnOpQrStUv";

        string sNums = "5820416937";
        int iPosNum = 0;
        int iPosChar = 0;
        string sChar = "";
        string sOrigOrder = "";

        for (int i = 0; i < 10; i++)
        {
            if (int.TryParse(sNums.IndexOf(i.ToString()).ToString(), out iPosNum) == false)
                iPosNum = 0;
            sChar = encrypted.Substring(iPosNum, 1);
            iPosChar = saCode[i].IndexOf(sChar);
            sOrigOrder += iPosChar.ToString();
        }

        if (int.TryParse(sOrigOrder.Substring(0, 3), out iaCtrTck[0]) == false)
            iaCtrTck[0] = 0;
        if (int.TryParse(sOrigOrder.Substring(3, 7), out iaCtrTck[1]) == false)
            iaCtrTck[1] = 0;

        return iaCtrTck;
    }
    // ========================================================================
    public void Set_Environment()
    {
        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

        sDevTestLive = "LIVE";
        sSqlDbToUse_Error = "Workfiles_LIVE.dbo";
        sSqlDbToUse_Customer = "CustomerLegacy_LIVE.dbo";
        sLibrary = "OMDTALIB";

        if (
                  sUserURL.Contains("c2.scantronts")
               || sUserURL.Contains(":180") || sUserURL.Contains(":1443") // my test site
               || sUserURL.Contains(":181") || sUserURL.Contains(":4181") 
            )
        {
            sDevTestLive = "TEST";
            sSqlDbToUse_Error = "Workfiles_TEST.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_TEST.dbo";
            sLibrary = "OMTDTALIB";
        }
        else if (sUserURL.Contains("localhost")
            || sUserURL.Contains(":280") || sUserURL.Contains(":4280")
            || sUserURL.Contains(":281") || sUserURL.Contains(":4281")
            )
        {
            sDevTestLive = "DEV";
            sSqlDbToUse_Error = "Workfiles_TEST.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_TEST.dbo";
            sLibrary = "OMTDTALIB";
        }
        else if (
                  sUserURL.Contains("oma-shrdd-w01")
               || sUserURL.Contains(":380") || sUserURL.Contains(":4380")
               || sUserURL.Contains(":381") || sUserURL.Contains(":4381")
            )
        {
            sDevTestLive = "DEV"; // DEV ISA?
            sSqlDbToUse_Error = "Workfiles_TEST.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_TEST.dbo";
            sLibrary = "OMTDTALIB";
        }
        else if (
              sUserURL.Contains(":480") || sUserURL.Contains(":4480")
              || sUserURL.Contains(":482") || sUserURL.Contains(":4482")
              || sUserURL.Contains(":481") || sUserURL.Contains(":4481")
            )
        {
            sDevTestLive = "DEV"; // DEV STEVE?
            sSqlDbToUse_Error = "Workfiles_TEST.dbo";
            sSqlDbToUse_Customer = "CustomerLegacy_TEST.dbo";
            sLibrary = "OMTDTALIB";
        }
        //EmailHandler emailHandler = new EmailHandler();
        //emailHandler.EmailIndividual("MyPage Environment", "URL: " + sUserURL + " <br /> DB: " + sSqlDbToUse_Customer, "steve.carlson@scantron.com", "adv320@scantron.com");
        //emailHandler = null;

    }
    // ==================================================
    //    [Description("Indicates whether or not this page should be forced into or out of SSL")]
    //    [Browsable(true)]
    public virtual bool RequireSSL
    {
        get
        {
            return _RequireSSL;
        }
        set
        {
            _RequireSSL = value;
        }
    }
    // ==================================================
    // 1st Option prevents debugger from walking through code (leave this on in non SSL development: off for live) 
    // [System.Diagnostics.DebuggerStepThrough()]
    // 2nd Option only allows it to run in secure environment
    // [System.Diagnostics.Conditional("SECURE")]
    private void PushSSL()
    {
        const string SECURE = "https://";
        const string UNSECURE = "http://";
        const string sLivUnSecure = ":81/"; // 80
        const string sLivSecure = ":4444/"; // 443
        const string sLi2UnSecure = ":81/"; // This was the scantrontechnologysolutions.com but I can use it for the live IP too
        const string sLi2Secure = ":4444/";
        const string sTstUnSecure = ":180/";
        const string sTstSecure = ":1443/";
        const string sDevUnSecure = ":281/";
        const string sDevSecure = ":4281/";
        const string sIsaUnSecure = ":482/";
        const string sIsaSecure = ":4482/";
        const string sSteUnSecure = ":481/";
        const string sSteSecure = ":4481/";

        string sUserURL_Original = Request.Url.ToString();
        string sUserURL = sUserURL_Original;

        //Force required into secure channel

        //string sWWWRedirect = "NO";
        if (!sUserURL.StartsWith("http://localhost"))
        {

            if (RequireSSL && Request.IsSecureConnection == false)
            {
                sUserURL = sUserURL.Replace(sLivUnSecure, sLivSecure);
                sUserURL = sUserURL.Replace(sLi2UnSecure, sLi2Secure);
                sUserURL = sUserURL.Replace(sTstUnSecure, sTstSecure);
                sUserURL = sUserURL.Replace(sDevUnSecure, sDevSecure);
                sUserURL = sUserURL.Replace(sIsaUnSecure, sIsaSecure);
                sUserURL = sUserURL.Replace(sSteUnSecure, sSteSecure);
                sUserURL = sUserURL.Replace(UNSECURE, SECURE);
                
                if (sUserURL_Original != sUserURL)
                    Response.Redirect(sUserURL);
            }
            //Force non-required out of secure channel
            if (!RequireSSL && Request.IsSecureConnection == true)
            {
                sUserURL = sUserURL.Replace(sLivSecure, sLivUnSecure);
                sUserURL = sUserURL.Replace(sLi2Secure, sLi2UnSecure);
                sUserURL = sUserURL.Replace(sTstSecure, sTstUnSecure);
                sUserURL = sUserURL.Replace(sDevSecure, sDevUnSecure);
                sUserURL = sUserURL.Replace(sIsaSecure, sIsaUnSecure);
                sUserURL = sUserURL.Replace(sSteSecure, sSteUnSecure);

                sUserURL = sUserURL.Replace(SECURE, UNSECURE);
                if (sUserURL_Original != sUserURL)
                    Response.Redirect(sUserURL);
            }
            //if (sWWWRedirect == "YES")
            //    Response.Redirect(sUserURL);
        }
    }
    // ==================================================
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    // ==================================================
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        PushSSL();

        //SiteHandler sh = new SiteHandler();
        //sDevTestLive = sh.getWebSite();
        //sDevTestLive = "DEV";
        /*
        if (sDevTestLive == "LIVE")
            sLibrary = "OMDTALIB";
        else
            sLibrary = "OMTDTALIB";
        */
        if (!IsPostBack)
        {
            SaveHitsToPages();
            SaveHitsFromSameIP();
        }
    }
    // ==================================================
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    // ==================================================
    public string ValidatePassword(string username, string password)
    {
        string sIsPasswordFormatValid = "";
        string sError = "";
        int iUpper = 0;
        int iLower = 0;
        int iNumber = 0;
        int iUserInPwd = 0;
        int iPwdInUser = 0;
        int iPasswordLength = password.Length;
        string sUppercaseFound = "NO";
        string sLowercaseFound = "NO";
        string sNumberFound = "NO";
        string sChar = "";
        string sUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string sLower = "abcdefghijklmnopqrstuvwxyz";
        string sNumber = "0123456789";

        if (password == "")
        {
            sError = "A password is required.";
        }
        else
        {
            for (int i = 0; i < iPasswordLength; i++)
            {
                sChar = password.Substring(i, 1);
                iUpper = sUpper.IndexOf(sChar);
                iLower = sLower.IndexOf(sChar);
                iNumber = sNumber.IndexOf(sChar);

                if (iUpper > -1)
                    sUppercaseFound = "YES";
                if (iLower > -1)
                    sLowercaseFound = "YES";
                if (iNumber > -1)
                    sNumberFound = "YES";
            }
            if (iPasswordLength < 7)
                sError += "Password is too short (7 character minimum).<br />";
            else if (iPasswordLength > 30)
                sError += "Password is too long (30 character maximum).<br />";

            if (sUppercaseFound == "NO")
                sError += "No uppercase character found.<br />";
            if (sLowercaseFound == "NO")
                sError += "No lowercase character found.<br />";
            if (sNumberFound == "NO")
                sError += "No number was found.<br />";

            iUserInPwd = password.IndexOf(username);
            if (iUserInPwd > -1)
                sError += "The password may not contain the username.<br />";
            iPwdInUser = username.IndexOf(password);
            if (iPwdInUser > -1)
                sError += "The username may not contain the password.<br />";

        }
        if (sError == "")
            sIsPasswordFormatValid = "Y";
        else
        {
            sIsPasswordFormatValid = "INVALID PASSWORD FORMAT<br />" + sError;
        }

        return sIsPasswordFormatValid;
    }
    // =========================================================
    protected string GetAdminAccessCodes()
    {
        string sAccessCode = "";

        int iThisMonth = DateTime.Now.Month - 1;  // Backup one because arrays are zero based (i.e. January = Month 1, - but the array index starts at zero)
        int iNextMonth = DateTime.Now.AddMonths(1).Month - 1;
        string[] saCode = { "orange8", "tiger3", "houston9", "island4", "mexico7", "tango1", "baseball6", "delta3", "panda5", "canada2", "toyota4", "beach7" };

        sAccessCode = saCode[iThisMonth] + "|" + saCode[iNextMonth];

        return sAccessCode;
    }
    // ========================================================================

    public string SendEmail(string subject, string body, string emailTo, string emailFrom)
    {
        string sResult = "";
        string sSubject = subject;
        string sBody = body;
        string sMailServerIp = "SMTP6.Scantron.com"; // Updating Nov 18th, 2020

        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        if (!String.IsNullOrEmpty(emailTo))
        {
            try
            {
                sSubject = scrub(subject);

                sBody = scrub(sBody);
                sBody = sBody.Replace(Environment.NewLine, "<br />");

                MailAddress fromAddress = new MailAddress(emailFrom);
                message.From = fromAddress;
                message.Subject = subject;
                MailAddress toAddress = new MailAddress(emailTo);
                message.To.Add(toAddress);
                message.IsBodyHtml = true;
                message.Body = sBody;
                smtpClient.Host = sMailServerIp;
                smtpClient.Send(message);
                sResult = "SUCCESS";
            }
            catch (Exception ex)
            {
                sResult = ex.ToString();
                SaveError("Customer Email Failure", "Subject: " + sSubject + "<br /> Body: " + sBody, "Error: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        // -----------------------------------
        return sResult;
    }
    // ========================================================================
    public string scrub(string txt)
    {
        // Remember to also update any class version of scrub (noteHandler)
        string sTxt = txt;
        // “Quote” and then a ‘single’ quote from word
        sTxt = sTxt.Replace("“", "\"");
        sTxt = sTxt.Replace("”", "\"");
        sTxt = sTxt.Replace("‘", "'");
        sTxt = sTxt.Replace("’", "'");
        sTxt = sTxt.Replace("•", "");
        sTxt = sTxt.Replace("·\t", "&#8226;&nbsp;&nbsp;");
        sTxt = sTxt.Replace("o\t", "&#8226;&nbsp;&nbsp;");

        //sTxt = sTxt.Replace("<", "&lt;"); // this is pointless because it will Asp.Net security will make it crash before reaching this validation
        //sTxt = sTxt.Replace(">", "&gt;");
        sTxt = sTxt.Trim();

        return sTxt;
    }
    // ========================================================================
    public string scrubFileName(string fileName, string fileExtension)
    {
        // Remember to also update any class version of scrub (noteHandler)
        string sFileName = "";
        string sTemp = fileName;
        sTemp = fileName.Replace(fileExtension, "");

        // Clean up file name

        sTemp = scrub(sTemp);
        sTemp = sTemp.Replace(" ", "-");
        sTemp = sTemp.Replace("'", "");
        sTemp = sTemp.Replace("\"", "");
        sTemp = sTemp.Replace("%", "");
        sTemp = sTemp.Replace("@", "");
        sTemp = sTemp.Replace("!", "");
        sTemp = sTemp.Replace("~", "");
        sTemp = sTemp.Replace("`", "");
        sTemp = sTemp.Replace("#", "");
        sTemp = sTemp.Replace("^", "");
        sTemp = sTemp.Replace("&", "");
        sTemp = sTemp.Replace("*", "");
        sTemp = sTemp.Replace("(", "");
        sTemp = sTemp.Replace(")", "");
        sTemp = sTemp.Replace("+", "");
        sTemp = sTemp.Replace("=", "");
        sTemp = sTemp.Replace("{", "");
        sTemp = sTemp.Replace("}", "");
        sTemp = sTemp.Replace("[", "");
        sTemp = sTemp.Replace("]", "");
        sTemp = sTemp.Replace("/", "");
        sTemp = sTemp.Replace("<", "");
        sTemp = sTemp.Replace(">", "");
        sTemp = sTemp.Replace("?", "");
        sTemp = sTemp.Replace(",", "");
        sTemp = sTemp.Replace(".", "");

        if (String.IsNullOrEmpty(sTemp))
            sTemp = DateTime.Now.ToString("yyyyMMdd-hhmmss");

        sFileName = (sTemp + fileExtension).Trim();

        return sFileName;
    }
    // ========================================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            // set properties of the client
            return client.DownloadString(url);
        }
    }
    // ========================================================================
    public static string FormatPhone1(string phone10)
    {
        phone10 = phone10.Trim();
        string sPhoneFormatted = "";
        long lPhone = 0;
        if (long.TryParse(phone10, out lPhone) == false)
            lPhone = -1;
        if (lPhone > -1 && phone10.Length == 10)
        {

            if (!String.IsNullOrEmpty(phone10)
                && phone10.Length == 10
                && phone10 != "0000000000"
                && phone10 != "1111111111"
                && phone10 != "2222222222"
                && phone10 != "3333333333"
                && phone10 != "4444444444"
                && phone10 != "5555555555"
                && phone10 != "6666666666"
                && phone10 != "7777777777"
                && phone10 != "8888888888"
                && phone10 != "9999999999"
                )
            {
                sPhoneFormatted = "(" + phone10.Substring(0, 3) + ") " + phone10.Substring(3, 3) + "-" + phone10.Substring(6, 4);
            }
        }
        return sPhoneFormatted;
    }
    // ========================================================================
    protected string Fix_Case(string textIn)
    {
        string sTextOut = "";

        string sTemp = textIn;
        string sChar1 = "";
        string sChar2 = "";
        string sChar3 = "";
        string sChar4 = "";
        string sCharBefore = "";

        try
        {
            // Consider looping through a database of fixes
            if (!String.IsNullOrEmpty(sTemp))
            {
                // Convert to lowercase
                sTemp = sTemp.ToLower();

                if (sTemp.StartsWith("nyc ") && sTemp.Length > 4)
                    sTemp = "NYC " + sTemp.Substring(4);
                if (sTemp.StartsWith("dwi ") && sTemp.Length > 4)
                    sTemp = "DWI " + sTemp.Substring(4);
                if (sTemp.StartsWith("aaa ") && sTemp.Length > 4)
                    sTemp = "AAA " + sTemp.Substring(4);
                if (sTemp.StartsWith("abs ") && sTemp.Length > 4)
                    sTemp = "ABS " + sTemp.Substring(4);

                // Convert standardized items
                sTemp = sTemp.Replace(" cc ", " CC ");
                sTemp = sTemp.Replace(" ci ", " CI ");
                sTemp = sTemp.Replace(" es ", " ES ");
                sTemp = sTemp.Replace(" hs ", " HS ");
                sTemp = sTemp.Replace(" is ", " IS ");
                //sTemp = sTemp.Replace(" ms ", " MS "); handled by Mississippi below
                //sTemp = sTemp.Replace(" ne ", " NE "); handled by Nebraska below
                sTemp = sTemp.Replace(" nw ", " NW ");
                sTemp = sTemp.Replace(" po ", " PO ");
                sTemp = sTemp.Replace(" ps ", " PS ");
                sTemp = sTemp.Replace(" pt ", " PT ");
                sTemp = sTemp.Replace(" rr ", " RR ");
                //sTemp = sTemp.Replace(" sd ", " SD "); handled by South Dakota below
                sTemp = sTemp.Replace(" se ", " SE ");
                sTemp = sTemp.Replace(" ss ", " SS ");
                sTemp = sTemp.Replace(" sw ", " SW ");

                sTemp = sTemp.Replace(" ak ", " AK ");
                sTemp = sTemp.Replace(" al ", " AL ");
                sTemp = sTemp.Replace(" ar ", " AR ");
                sTemp = sTemp.Replace(" az ", " AZ ");
                sTemp = sTemp.Replace(" ca ", " CA ");
                // sTemp = sTemp.Replace(" co ", " CO ");  // too many companies... skip Colorado
                //sTemp = sTemp.Replace(" ct ", " CT "); // too many courts, skip connecticut
                sTemp = sTemp.Replace(" dc ", " DC ");
                sTemp = sTemp.Replace(" de ", " DE ");
                sTemp = sTemp.Replace(" fl ", " FL ");
                sTemp = sTemp.Replace(" ga ", " GA ");
                sTemp = sTemp.Replace(" hi ", " HI ");
                sTemp = sTemp.Replace(" ia ", " IA ");
                sTemp = sTemp.Replace(" id ", " ID ");
                sTemp = sTemp.Replace(" il ", " IL ");
                sTemp = sTemp.Replace(" in ", " IN ");
                sTemp = sTemp.Replace(" ks ", " KS ");
                sTemp = sTemp.Replace(" ky ", " KY ");
                sTemp = sTemp.Replace(" la ", " LA ");
                sTemp = sTemp.Replace(" ma ", " MA ");
                sTemp = sTemp.Replace(" md ", " MD ");
                sTemp = sTemp.Replace(" me ", " ME ");
                sTemp = sTemp.Replace(" mi ", " MI ");
                sTemp = sTemp.Replace(" mn ", " MN ");
                sTemp = sTemp.Replace(" mo ", " MO ");
                sTemp = sTemp.Replace(" mp ", " MP ");
                sTemp = sTemp.Replace(" ms ", " MS ");
                sTemp = sTemp.Replace(" mt ", " MT ");
                sTemp = sTemp.Replace(" nc ", " NC ");
                sTemp = sTemp.Replace(" nd ", " ND ");
                sTemp = sTemp.Replace(" ne ", " NE ");
                sTemp = sTemp.Replace(" nh ", " NH ");
                sTemp = sTemp.Replace(" nj ", " NJ ");
                sTemp = sTemp.Replace(" nm ", " NM ");
                sTemp = sTemp.Replace(" nv ", " NV ");
                sTemp = sTemp.Replace(" ny ", " NY ");
                sTemp = sTemp.Replace(" oh ", " OH ");
                sTemp = sTemp.Replace(" ok ", " OK ");
                sTemp = sTemp.Replace(" or ", " OR ");
                sTemp = sTemp.Replace(" pa ", " PA ");
                sTemp = sTemp.Replace(" ri ", " RI ");
                sTemp = sTemp.Replace(" sc ", " SC ");
                sTemp = sTemp.Replace(" sd ", " SD ");
                sTemp = sTemp.Replace(" tn ", " TN ");
                sTemp = sTemp.Replace(" tx ", " TX ");
                sTemp = sTemp.Replace(" ut ", " UT ");
                sTemp = sTemp.Replace(" va ", " VA ");
                sTemp = sTemp.Replace(" vt ", " VT ");
                sTemp = sTemp.Replace(" wa ", " WA ");
                sTemp = sTemp.Replace(" wi ", " WI ");
                sTemp = sTemp.Replace(" wv ", " WV ");
                sTemp = sTemp.Replace(" wy ", " WY ");

                sTemp = sTemp.Replace(" boe ", " BOE ");
                sTemp = sTemp.Replace(" dds ", " DDS ");
                sTemp = sTemp.Replace(" dwi ", " DWI ");
                sTemp = sTemp.Replace(" isd ", " ISD ");
                sTemp = sTemp.Replace(" jhs ", " JHS ");
                sTemp = sTemp.Replace(" nyc ", " NYC ");
                sTemp = sTemp.Replace(" nys ", " NYS ");
                sTemp = sTemp.Replace(" llc ", " LLC ");
                sTemp = sTemp.Replace(" usa ", " USA ");
                sTemp = sTemp.Replace(" usd ", " USD ");


                sTemp = sTemp.Replace("po box", "PO Box");

                //  state abbreviations Mc & San St ps ss hs sd boe po, Sandy vs SanDiego


                for (int i = 0; i < sTemp.Length; i++)
                {
                    // Save blocks for review 
                    sChar1 = sTemp.Substring(i, 1);
                    if (sTemp.Length > i + 1)
                        sChar2 = sTemp.Substring(i, 2);
                    else
                        sChar2 = "  ";
                    if (sTemp.Length > i + 2)
                        sChar3 = sTemp.Substring(i, 3);
                    else
                        sChar3 = "   ";
                    if (sTemp.Length > i + 3)
                        sChar4 = sTemp.Substring(i, 4);
                    else
                        sChar4 = "    ";

                    // Raise First character of every word    
                    if (i == 0
                        || sCharBefore == " "
                        )
                        sChar1 = sChar1.ToUpper();

                    if (
                           (sChar1 == "&" && sChar2.Substring(1, 1) != " ")
                        || (sChar1 == "'" && sChar2.Substring(1, 1) != " " && sChar3.Substring(2, 1) != " ") // skip apostrophe's O'Malley ok, Fred's skip
                        || (sChar1 == "-" && sChar2.Substring(1, 1) != " ")
                        || (sChar1 == "/" && sChar2.Substring(1, 1) != " ")
                    )
                    {
                        // handling pos 1
                        sTextOut += sChar1;

                        // handling pos 2
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                    }

                    // Handle 2 char patterns at the end of the string
                    else if (
                       (sChar3 == " cc" && sTemp.Length == (i + 3))
                    || (sChar3 == " ci" && sTemp.Length == (i + 3))
                    || (sChar3 == " es" && sTemp.Length == (i + 3))
                    || (sChar3 == " hs" && sTemp.Length == (i + 3))
                    || (sChar3 == " is" && sTemp.Length == (i + 3))
                    //|| (sChar3 == " ms" && sTemp.Length == (i + 3))
                    //|| (sChar3 == " ne" && sTemp.Length == (i + 3))
                    || (sChar3 == " nw" && sTemp.Length == (i + 3))
                    || (sChar3 == " po" && sTemp.Length == (i + 3))
                    || (sChar3 == " ps" && sTemp.Length == (i + 3))
                    || (sChar3 == " rr" && sTemp.Length == (i + 3))
                    || (sChar3 == " se" && sTemp.Length == (i + 3))
                    //|| (sChar3 == " sd" && sTemp.Length == (i + 3))
                    || (sChar3 == " ss" && sTemp.Length == (i + 3))
                    || (sChar3 == " sw" && sTemp.Length == (i + 3))
                    || (sChar3 == " ak" && sTemp.Length == (i + 3))
                    || (sChar3 == " al" && sTemp.Length == (i + 3))
                    || (sChar3 == " ar" && sTemp.Length == (i + 3))
                    || (sChar3 == " az" && sTemp.Length == (i + 3))
                    || (sChar3 == " ca" && sTemp.Length == (i + 3))
                    //|| (sChar3 == " co" && sTemp.Length == (i + 3))
                    //|| (sChar3 == " ct" && sTemp.Length == (i + 3))
                    || (sChar3 == " dc" && sTemp.Length == (i + 3))
                    || (sChar3 == " de" && sTemp.Length == (i + 3))
                    || (sChar3 == " fl" && sTemp.Length == (i + 3))
                    || (sChar3 == " ga" && sTemp.Length == (i + 3))
                    || (sChar3 == " hi" && sTemp.Length == (i + 3))
                    || (sChar3 == " ia" && sTemp.Length == (i + 3))
                    || (sChar3 == " id" && sTemp.Length == (i + 3))
                    || (sChar3 == " il" && sTemp.Length == (i + 3))
                    || (sChar3 == " in" && sTemp.Length == (i + 3))
                    || (sChar3 == " ks" && sTemp.Length == (i + 3))
                    || (sChar3 == " ky" && sTemp.Length == (i + 3))
                    || (sChar3 == " la" && sTemp.Length == (i + 3))
                    || (sChar3 == " ma" && sTemp.Length == (i + 3))
                    || (sChar3 == " md" && sTemp.Length == (i + 3))
                    || (sChar3 == " me" && sTemp.Length == (i + 3))
                    || (sChar3 == " mi" && sTemp.Length == (i + 3))
                    || (sChar3 == " mn" && sTemp.Length == (i + 3))
                    || (sChar3 == " mo" && sTemp.Length == (i + 3))
                    || (sChar3 == " mp" && sTemp.Length == (i + 3))
                    || (sChar3 == " ms" && sTemp.Length == (i + 3))
                    || (sChar3 == " mt" && sTemp.Length == (i + 3))
                    || (sChar3 == " nc" && sTemp.Length == (i + 3))
                    || (sChar3 == " nd" && sTemp.Length == (i + 3))
                    || (sChar3 == " ne" && sTemp.Length == (i + 3))
                    || (sChar3 == " nh" && sTemp.Length == (i + 3))
                    || (sChar3 == " nj" && sTemp.Length == (i + 3))
                    || (sChar3 == " nm" && sTemp.Length == (i + 3))
                    || (sChar3 == " nv" && sTemp.Length == (i + 3))
                    || (sChar3 == " ny" && sTemp.Length == (i + 3))
                    || (sChar3 == " oh" && sTemp.Length == (i + 3))
                    || (sChar3 == " ok" && sTemp.Length == (i + 3))
                    || (sChar3 == " or" && sTemp.Length == (i + 3))
                    || (sChar3 == " pa" && sTemp.Length == (i + 3))
                    || (sChar3 == " ri" && sTemp.Length == (i + 3))
                    || (sChar3 == " sc" && sTemp.Length == (i + 3))
                    || (sChar3 == " sd" && sTemp.Length == (i + 3))
                    || (sChar3 == " tn" && sTemp.Length == (i + 3))
                    || (sChar3 == " tx" && sTemp.Length == (i + 3))
                    || (sChar3 == " ut" && sTemp.Length == (i + 3))
                    || (sChar3 == " va" && sTemp.Length == (i + 3))
                    || (sChar3 == " vt" && sTemp.Length == (i + 3))
                    || (sChar3 == " wa" && sTemp.Length == (i + 3))
                    || (sChar3 == " wi" && sTemp.Length == (i + 3))
                    || (sChar3 == " wv" && sTemp.Length == (i + 3))
                    || (sChar3 == " wy" && sTemp.Length == (i + 3))
                    )
                    {
                        // handling pos 1
                        sTextOut += sChar1;

                        // handling pos 2
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                        sTextOut += sChar1;

                        // handling pos 3
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                    }
                    // Handle 3 char patterns at the end of the string
                    else if (
                       (sChar4 == " boe" && sTemp.Length == (i + 4))
                    || (sChar4 == " dds" && sTemp.Length == (i + 4))
                    || (sChar4 == " dwi" && sTemp.Length == (i + 4))
                    || (sChar4 == " isd" && sTemp.Length == (i + 4))
                    || (sChar4 == " jhs" && sTemp.Length == (i + 4))
                    || (sChar4 == " llc" && sTemp.Length == (i + 4))
                    || (sChar4 == " nyc" && sTemp.Length == (i + 4))
                    || (sChar4 == " nys" && sTemp.Length == (i + 4))
                    || (sChar4 == " usa" && sTemp.Length == (i + 4))
                    || (sChar4 == " usd" && sTemp.Length == (i + 4))
                    )
                    {
                        // handling pos 1
                        sTextOut += sChar1;

                        // handling pos 2
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                        sTextOut += sChar1;

                        // handling pos 3
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                        sTextOut += sChar1;

                        // handling pos 4
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();

                    }

                    // Handle McDonalds types of patterns
                    else if (
                       (sChar2 == "mc" && sChar3.Substring(2, 1) != " " && sChar4.Substring(3, 1) != " ")
                    )
                    {
                        // handling pos 1
                        sTextOut += sChar1.ToUpper();

                        // handling pos 2
                        i++;
                        sChar1 = sTemp.Substring(i, 1);
                        sTextOut += sChar1;

                        // handling pos 3
                        i++;
                        sChar1 = sTemp.Substring(i, 1).ToUpper();
                    }


                    sCharBefore = sChar1;
                    sTextOut += sChar1;
                }
            }
        }
        catch (Exception ex)
        {
            string sDebug = ex.Message.ToString();
            //ErrorHandler erh = new ErrorHandler();
            //erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            //erh = null;
        }
        finally
        {
        }

        return sTextOut;
    }
    // ========================================================================
    public string Make_ConfirmationCode(string userId)
    {
        string sConfirmationCode = "";

        // 2021-12-31T13:14:15.0000-05:00|steve.carlson@scantron.com
        // Code: Generate a code to pass in the user email, and receive when link is clicked to compare with what's stored in the database
        // Deadine Date: Store the deadline date on the user record
        // Success: a) Code received from user matches database copy b) returned prior to deadline date. 
        // The code must contain the user email to find who it belongs to, but you must have something else unique to store to prevent hacker from reproducing a valid match
        // A timestamp would be unique and may be adequate
        // Challenge: Hacker creates plain account, receives confirmation email, parses it to figure out what it contains
        // I think, even if he knows the format the odds that he could reproduce the exact timestamp that was generated for an email address that he can't see would be remote. 
        // They are going to know it will contain the email address, the challenge is to make it difficult to know where it is in the encrypted string. 
        // On the first confirmation attempt, success or failure, wipe out the code in the database, to force another request

        try 
        {
            string sTemp = "";
            string sChar = "";
            int iPos = 0;
            // 1) Make unique string
            sConfirmationCode = DateTime.Now.ToUniversalTime().ToString("o") + "|" + userId;
            // 2) Encode
            sConfirmationCode = Convert.ToBase64String(Encoding.Default.GetBytes(sConfirmationCode)); // 1x

            // 3) Scramble
            for (int i = 0; i < sConfirmationCode.Length; i++)
            {
                sChar = sConfirmationCode.Substring(i, 1);
                iPos = sEncryptOrig.IndexOf(sChar);
                if (iPos > -1)
                    sTemp += sEncryptSwap.Substring(iPos, 1);
                else
                    sTemp += sChar;
            }
            sConfirmationCode = sTemp;
        }
        catch (Exception ex)
        {
            string sDebug = ex.Message.ToString();
        }
        finally
        {
        }

        return sConfirmationCode;
    }
    // ========================================================================
    public string Parse_ConfirmationCode(string confirmationCode)
    {
        string sUserEmail = "";

        try 
        {
            string sTemp = "";
            string sChar = "";
            int iPos = 0;
            byte[] baCode = { 0 };

            // 3) Unscramble back into Base64 format
            for (int i = 0; i < confirmationCode.Length; i++)
            {
                sChar = confirmationCode.Substring(i, 1);
                iPos = sEncryptSwap.IndexOf(sChar);
                if (iPos > -1)
                    sTemp += sEncryptOrig.Substring(iPos, 1);
                else
                    sTemp += sChar;
            }
            confirmationCode = sTemp;

            // 2) Base64 to string version (readable again)
            baCode = Convert.FromBase64String(confirmationCode);
            confirmationCode = System.Text.ASCIIEncoding.ASCII.GetString(baCode);

            // 1) separate email from string
            string[] saStpEml = confirmationCode.Split('|');
            if (saStpEml.Length > 1)
            {
                sUserEmail = saStpEml[1];
            }
        }
        catch (Exception ex)
        {
            string sDebug = ex.Message.ToString();
        }
        finally
        {
        }

        return sUserEmail;
    }
    // ========================================================================
    public Boolean isAPositiveInteger(string number)
    {
        Boolean bIsAPositiveInteger = false;
        string sAllCharAreNumbers = "";
        string sNums = "0123456789";
        string sChar = "";

        number = number.Trim();

        for (int i = 0; i < number.Length; i++) 
        {
            if (sAllCharAreNumbers != "N") 
            {
                sChar = number.Substring(0, 1);
                if (sNums.Contains(sChar) == true)
                    sAllCharAreNumbers = "Y";
                else
                    sAllCharAreNumbers = "N";
            }
        }

        if (sAllCharAreNumbers == "Y") 
        {
            int iTemp = 0;
            if (int.TryParse(number, out iTemp) == false)
                iTemp = -1;
            if (iTemp > 0)
                bIsAPositiveInteger = true;
        }
            

        return bIsAPositiveInteger;
    }
    // ========================================================================
    public string GetIp()
    {
        string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ip))
        {
            ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        return ip;
    }
    // ------------------------------------------------------------------------
    public string Clean_PhoneEntry(string sOriginal)
    {
        string sNums = "0123456789";
        string sChar = "";
        string sCleaned = "";
        string sTemp = "";

        try
        {
            if (!String.IsNullOrEmpty(sOriginal))
            {
                sChar = "";
                for (int i = 0; i < sOriginal.Length; i++)
                {
                    sChar = sOriginal.Substring(i, 1);
                    if (sNums.Contains(sChar))
                        sTemp += sChar;
                }
                sCleaned = sTemp;
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {

        }
        return sCleaned;
    }
    // ----------------------------------------------------------------------------
    public bool isEmailFormatValid(string email)
    {
        var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");

        return !string.IsNullOrEmpty(email) && r.IsMatch(email);
    }
    // ==================================================
    public void SaveHitsToPages()
    {
        string sParentPagePath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sParentPagePath);
        string sParentPageName = oInfo.Name;

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

            sSql = "Select hcKey, hcCount from HitCount where hcPath = @PagePath and hcYear = @ThisYear";
            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            sqlCmd.Parameters.Add("@PagePath", System.Data.SqlDbType.VarChar, 100);
            sqlCmd.Parameters["@PagePath"].Value = sParentPagePath;

            int iYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            sqlCmd.Parameters.Add("@ThisYear", System.Data.SqlDbType.Int);
            sqlCmd.Parameters["@ThisYear"].Value = iYear;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

            dataTable = new DataTable();
            dataTable.Load(sqlReader);

            int iKey = 0;
            int iCount = 0;

            if (dataTable.Rows.Count > 0)
            {
                // update hit count rec
                if (int.TryParse(dataTable.Rows[0]["hcKey"].ToString(), out iKey) == false)
                    iKey = 0;
                if (int.TryParse(dataTable.Rows[0]["hcCount"].ToString(), out iCount) == false)
                    iCount = 0;

                sSql = "Update HitCount" +
                    " set hcCount = @HitCount" +
                    " where hcKey = @HitKey";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.Add("@HitCount", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@HitCount"].Value = iCount + 1;

                sqlCmd.Parameters.Add("@HitKey", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@HitKey"].Value = iKey;
            }
            else
            {
                // Add first rec for this page
                sSql = "Insert into HitCount" +
                    " (hcPath, hcPage, hcCount, hcYear)" +
                    " VALUES(@Path, @Name, @Count, @Year)";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar, 200);
                sqlCmd.Parameters["@Path"].Value = sParentPagePath;

                sqlCmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100);
                sqlCmd.Parameters["@Name"].Value = sParentPageName;

                sqlCmd.Parameters.Add("@Count", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@Count"].Value = 1;

                sqlCmd.Parameters.Add("@Year", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@Year"].Value = iYear;
            }
            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        // ----------------------------------
    }
    // ==================================================
    public void SaveHitsFromSameIP()
    {
        string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        string sParentPage = oInfo.Name;

        if ((sParentPage != "Timeout.aspx") && (sParentPage != "Lockout.aspx"))
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConn;
            SqlDataReader sqlReader;
            DataTable dataTable;

            DateTime datTemp = new DateTime();
            TimeSpan ts = new TimeSpan();
            string sSql = "";
            string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            sqlConn = new SqlConnection(sConnectionString);

            double dTimeoutHours = 4.0;
            int iMaxPeriodHits = 500; // 500
            int iMaxTimeouts = 3; // 10

            int iIpKey = 0;
            int iPeriodHits = 0;
            int iLifeHits = 0;
            int iTimeoutCount = 0;
            int iBlacklisted = 0;
            int iRowsAffected = 0;

            string sRedirect = "";
            string sUserIdLast = "";
            string sUserIdCurrent = "";
            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserIdCurrent = mu.UserName;
            }

            string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Trim();

            try
            {
                sqlConn.Open();

                sSql = "Select siKey, siPeriodStart, siPeriodHits, siLifeHits, siTimeoutCount, siBlacklisted, siUserID from HitsFromSameIP where siIpa = @UserIP";
                sqlCmd = new SqlCommand(sSql, sqlConn);
                sqlCmd.CommandText = sSql;
                sqlCmd.Parameters.Add("@UserIP", System.Data.SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@UserIP"].Value = sIpAddress;

                sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

                dataTable = new DataTable();
                dataTable.Load(sqlReader);

                if (dataTable.Rows.Count > 0)
                {
                    // Update IP rec for this count
                    datTemp = Convert.ToDateTime(dataTable.Rows[0]["siPeriodStart"].ToString());

                    if (int.TryParse(dataTable.Rows[0]["siKey"].ToString(), out iIpKey) == false)
                        iIpKey = 0;
                    if (int.TryParse(dataTable.Rows[0]["siPeriodHits"].ToString(), out iPeriodHits) == false)
                        iPeriodHits = 0;
                    if (int.TryParse(dataTable.Rows[0]["siLifeHits"].ToString(), out iLifeHits) == false)
                        iLifeHits = 0;
                    if (int.TryParse(dataTable.Rows[0]["siTimeoutCount"].ToString(), out iTimeoutCount) == false)
                        iTimeoutCount = 0;
                    if (int.TryParse(dataTable.Rows[0]["siBlacklisted"].ToString(), out iBlacklisted) == false)
                        iBlacklisted = 0;
                    sUserIdLast = dataTable.Rows[0]["siUserID"].ToString();

                    if (!String.IsNullOrEmpty(sUserIdLast))
                    {
                        if (sUserIdLast.ToLower().Contains(sUserIdCurrent.ToLower()) == true)
                        {
                            sUserIdCurrent = sUserIdLast;
                        }
                        else 
                        {
                            sUserIdCurrent = sUserIdLast + " " + sUserIdCurrent;
                            if (!String.IsNullOrEmpty(sUserIdCurrent) && sUserIdCurrent.Length > 2000)
                                sUserIdCurrent = sUserIdCurrent.Substring(0, 2000);
                        }
                    }

                    ts = DateTime.Now - datTemp;
                    //if (iBlacklisted == 1)
                    //{
                    //    sRedirect = "LOCKOUT";
                    //}
                    //else if (iIpKey > 0)
                    if (iIpKey > 0)
                    {
                        if (Convert.ToDouble(ts.TotalHours.ToString()) < dTimeoutHours) // Inside 2 hour window
                        {
                            if (iPeriodHits < iMaxPeriodHits)
                            {
                                // Just Increment Hits during this period
                                sSql = "Update HitsFromSameIP" +
                                    " set siPeriodHits = @PeriodHits" +
                                    ", siLifeHits = @LifeHits" +
                                    ", siLastAccess = @LastAccess" +
                                    ", siUserID = @UserIdCurrent" +
                                    " where siKey = @IpKey";
                                sqlCmd = new SqlCommand(sSql, sqlConn);

                                sqlCmd.Parameters.Add("@PeriodHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@PeriodHits"].Value = iPeriodHits + 1;

                                sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                                sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                                sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                                sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                                sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                            }
                            else
                            {
                                // Increment Hits, Reset Temp Lock and Start time for 2 hour user timeout window
                                sSql = "Update HitsFromSameIP" +
                                    " set siPeriodHits = @PeriodHits" +
                                    ", siLifeHits = @LifeHits" +
                                    ", siLastAccess = @LastAccess" +
                                    ", siTimeoutCount = @TimeoutCount" +
                                    ", siBlacklisted = @Blacklisted" +
                                    ", siPeriodStart = @TimeStart" +
                                    ", siUserID = @UserIdCurrent" +
                                    " where siKey = @IpKey";
                                sqlCmd = new SqlCommand(sSql, sqlConn);

                                sqlCmd.Parameters.Add("@PeriodHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@PeriodHits"].Value = iPeriodHits + 1;

                                sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                                sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                                if (iPeriodHits == iMaxPeriodHits)
                                {
                                    iTimeoutCount++;
                                }

                                sqlCmd.Parameters.Add("@TimeoutCount", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@TimeoutCount"].Value = iTimeoutCount;

                                // 10 temp timeouts? Lock out this IP!
                                if (iTimeoutCount >= iMaxTimeouts)
                                {
                                    iBlacklisted = 1;
                                    sRedirect = "LOCKOUT";
                                }
                                else
                                {
                                    sRedirect = "TIMEOUT";
                                }

                                sqlCmd.Parameters.Add("@Blacklisted", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@Blacklisted"].Value = iBlacklisted;

                                sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                                sqlCmd.Parameters["@TimeStart"].Value = DateTime.Now;

                                sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                                sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                                sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                            }
                        }
                        else // Been more than 2 hours since last visit
                        {
                            // Reset start time for IP
                            sSql = "Update HitsFromSameIP" +
                                " set siPeriodHits = 1" +
                                ", siLifeHits = @LifeHits" +
                                ", siLastAccess = @LastAccess" +
                                ", siPeriodStart = @TimeStart" +
                                ", siUserID = @UserIdCurrent" +
                                " where siKey = @IpKey";
                            sqlCmd = new SqlCommand(sSql, sqlConn);

                            sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                            sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                            sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                            datTemp = DateTime.Now;
                            sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                            sqlCmd.Parameters["@TimeStart"].Value = datTemp;

                            sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                            sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                            sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                            sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                        }

                        iRowsAffected = sqlCmd.ExecuteNonQuery();

                        // April 17th change, now logging activity but still locking out blacklisted users
                        if (iBlacklisted == 1)
                        {
                            sRedirect = "LOCKOUT";
                        }
                    }
                }
                else
                {
                    // Add first rec for this IP
                    sSql = "Insert into HitsFromSameIP" +
                        " (siIpa, siPeriodHits, siLifeHits, siPeriodStart, siFirstUse, siUserID)" +
                        " VALUES(@UserIP, 1, 1, @TimeStart, @FirstUse, @UserIdCurrent)";

                    sqlCmd = new SqlCommand(sSql, sqlConn);

                    sqlCmd.Parameters.Add("@UserIP", System.Data.SqlDbType.VarChar, 50);
                    sqlCmd.Parameters["@UserIP"].Value = sIpAddress;

                    datTemp = DateTime.Now;
                    sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                    sqlCmd.Parameters["@TimeStart"].Value = datTemp;

                    sqlCmd.Parameters.Add("@FirstUse", System.Data.SqlDbType.DateTime);
                    sqlCmd.Parameters["@FirstUse"].Value = datTemp;

                    sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                    sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                    iRowsAffected = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string sResult = ex.ToString();
            }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            // ----------------------------------

            if (sRedirect != "")
            {
                if (sRedirect == "LOCKOUT")
                    Response.Redirect("~/public/error/Lockout.aspx", false);
                else
                    Response.Redirect("~/public/error/Timeout.aspx", false);

                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

        }
    }
    // ========================================================================
    // dt.Columns.Add(MakeColumn("Strings"));
    public DataColumn MakeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
    // ========================================================================
    // dt.Columns.Add(MakeColumn("Numbers"));
    public DataColumn MakeColumnInt(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.Int32");
        dc.ColumnName = name;

        return dc;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    // ========================================================================
}

