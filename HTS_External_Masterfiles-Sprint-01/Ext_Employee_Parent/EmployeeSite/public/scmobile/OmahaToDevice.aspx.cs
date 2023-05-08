using System;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using Org.BouncyCastle.Bcpg.OpenPgp;

public partial class public_scmobile_OmahaToDevice : MyPage
{
    // string sLibrary = "OMTDTALIB"; 
    OdbcCommand odbcCmd;
    OdbcConnection odbcConn;
    OdbcDataReader odbcReader;

    ErrorHandler erh;
    MailHandler emh;

    string sResponseToOmaha = "DMZ DEFAULT|";
    int iUserPassed = -1;
    int iJobIdPassed = 0;

    int iMaxRecsToPushforEmp = 0;
    int iMaxRecsToPushforAll = 0;
    int iMinutesToWaitBetweenPushes = 0;
    int iPushAging = 0;
    int iPushHold = 0;

    ScMobile_LIVE.ScMobileMenuSoapClient wsLive = new ScMobile_LIVE.ScMobileMenuSoapClient();
    ScMobile_DEV.ScMobileMenuSoapClient wsDev = new ScMobile_DEV.ScMobileMenuSoapClient();
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { 
        this.RequireSSL = true;
        erh = new ErrorHandler();
        emh = new MailHandler();
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { erh = null; emh = null; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        try
        {
            odbcConn.Open();

            if (Request.QueryString["user"] != null && Request.QueryString["user"].ToString() != "")
            {
                if (int.TryParse(Request.QueryString["user"].ToString().Trim(), out iUserPassed) == false)
                    iUserPassed = -1;
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                if (int.TryParse(Request.QueryString["id"].ToString().Trim(), out iJobIdPassed) == false)
                    iJobIdPassed = -1;
            }
            if (iJobIdPassed > -1)
            {
                Select_ConfigValues();
                if (iJobIdPassed > 0)
                {
                    GetValuesToPush(iUserPassed, iJobIdPassed, "SELECT");
                    GetValuesToPush(0, iJobIdPassed, "EXCLUDE");
                }
                else
                {
                    GetValuesToPush(0, 0, "");
                }
            }
        }
        catch (Exception ex)
        {
            //erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "ScMobile DMZ OmahaToDevice Crash: Job Id Passed: " + iJobIdPassed + " User: " + iUserPassed);
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "ScMobile DMZ OmahaToDevice Crash");
        }
        finally
        {
            // sResponseToDevice += ": Response From Omaha: " + DateTime.Now.ToString("t");
            // Response to Omaha would show up on the screen, (only display when debugging or on error condition)
            //Response.Write(sResponseToOmaha);
            odbcConn.Close();
        }
        Response.End();
    }
    // ========================================================================    

    // ========================================================================
    protected string Get_Ws_Key(int user, string job)
    {
        string sKey = "";
        string sChar = "";

        int i = 0;
        int j = 0;
        int k = 0;

        int iJul = 0;

        //int iUsrJul = 0;
        String sUsrJul = "";
        int iResetIdx = 0;
        int iUsrJulIdx = 0;

        String sRnd = "kJ6enM2DbQo7shlBxcRK0FiLVAjzI4vWqZu1THfXt9yOrUYG5gaEwN8Sp3dmCP";

        // remove underscores
        job = job.Replace("_", "");
        // ensure the string is long enough
        if (job == "")
            job = "ABCDEFG"; // just to stop an infinite loop...
        while (job.Length < 25)
            job += job;

        // Get Julian day of year
        DateTime datTemp = DateTime.Now;
        iJul = datTemp.DayOfYear;

        sKey = "";
        sUsrJul = (user * iJul).ToString();

        // Use each number from number array as indexes to pull assorted values from job array
        for (i = 0; i < sUsrJul.Length; i++)
        {
            int.TryParse(sUsrJul.Substring(i, 1), out iUsrJulIdx);
            iResetIdx += iUsrJulIdx;
            if (iResetIdx >= job.Length)
                iResetIdx = iUsrJulIdx;
            sChar = job.Substring(iResetIdx, 1);
            j = sRnd.IndexOf(sChar);
            if (j + i + 1 < sRnd.Length)
                k = j + i + 1;
            else
                k = j - i - 1;

            sKey += sRnd.Substring(k, 1);
        }

        return sKey;
    }

    // ========================================================================
    #region mySqls
    // ========================================================================

    // ========================================================================
    protected void Select_ConfigValues()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        try
        {
            // string sLibrary = "STEVEC";

            sSql = "Select" +
                 " ACDMXEMP" +
                ", ACDMXALL" +
                ", ACDWAITM" +
                ", ACDAGING" +
                ", ACDHOLD" +
                " from " + sLibrary + ".ANCONFIG" +
                " where ACKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@key", 1);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["ACDMXEMP"].ToString().Trim(), out iMaxRecsToPushforEmp) == false)
                    iMaxRecsToPushforEmp = 10;
                if (int.TryParse(dt.Rows[0]["ACDMXALL"].ToString().Trim(), out iMaxRecsToPushforAll) == false)
                    iMaxRecsToPushforAll = 100;
                if (int.TryParse(dt.Rows[0]["ACDWAITM"].ToString().Trim(), out iMinutesToWaitBetweenPushes) == false)
                    iMinutesToWaitBetweenPushes = 5;
                if (int.TryParse(dt.Rows[0]["ACDAGING"].ToString().Trim(), out iPushAging) == false)
                    iPushAging = 5;
                if (int.TryParse(dt.Rows[0]["ACDHOLD"].ToString().Trim(), out iPushHold) == false)
                    iPushHold = 30;
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sSql);
        }
        finally
        {
            odbcCmd.Dispose();
        }
    }
    // ========================================================================
    protected void GetValuesToPush(int userPassed, int jobIdPassed, string selectExcludeOrDisregard)
    {

        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datNow = DateTime.Now;
        DateTime datLastPushed = new DateTime();
        TimeSpan ts = new TimeSpan();

        try
        {
            sSql = "Select" +
                 " AP2DEVID" +
                ", APUSER" +
                ", APWHAT" +
                ", APWHY" +
                ", APSTATUS" +
                ", APFIELDS" +
                ", APVALUES" +
                ", APTRYCNT" +
                ", APSTSCNT" +
                ", APCREATE" +
                ", APLSTTRY" +
                " from " + sLibrary + ".ANPUSHQ" +
                " where APSTATUS <> 'HOLD'";

            if (jobIdPassed > 0)
            {
                if (selectExcludeOrDisregard == "SELECT")
                    sSql += " and AP2DEVID = ?" + 
                        " and APUSER = ?";
                else if (selectExcludeOrDisregard == "EXCLUDE")
                    sSql += " and AP2DEVID <> ?";
            }

            sSql += " ORDER BY APUSER, APWHAT desc, AP2DEVID desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            if (jobIdPassed > 0)
            {
                odbcCmd.Parameters.AddWithValue("@JobToDeviceID", jobIdPassed);
                if (selectExcludeOrDisregard == "SELECT")
                    odbcCmd.Parameters.AddWithValue("@User", userPassed);
            }

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            int iJobToDeviceId = 0;
            int iUser = 0;
            int iPushCount = 0;
            int iStatusCount = 0;
            string sWhatToDo = "";
            string sWhyDoIt = "";
            string sStatus = "";
            string sFieldList = "";
            string sValueList = "";
            string sEmployeeEmail = "";
            string[] saCenterTicket = { "", "" };
            string sCallSubject = "";
            string sCallSummary = "";
            string sCtrTck = "";
            string sTicketsProcessed = "";

            int iLatestEmp = 0;
            int iPushedEmpRecs = 0;
            int iPushedTotRecs = 0;
            string sSqlOrig = sSql;
            string sCreateStamp = "";
            string skipThisPush = "";
            string[] saTotqCurq = { "", "" };
            int iTotalAttempts = 0;
            int iCurrentAttempts = 0;
            int iCenter = 0;
            int iTicket = 0;
            int iRowsAffected = 0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                iPushCount = 0;
                iStatusCount = 0;

                if (int.TryParse(dt.Rows[iRowIdx]["APUSER"].ToString().Trim(), out iUser) == false)
                    iUser = -1;

                sCreateStamp = dt.Rows[iRowIdx]["APCREATE"].ToString().Trim();

                sWhatToDo = dt.Rows[iRowIdx]["APWHAT"].ToString().Trim();

                sWhyDoIt = dt.Rows[iRowIdx]["APWHY"].ToString().Trim();

                // THROTTLE: Limit number of PUSHQ recs to push at one time...
                if (iLatestEmp != iUser) // Reset emp count for each new user
                    iPushedEmpRecs = 0;

                if (iPushedEmpRecs < iMaxRecsToPushforEmp &&
                    iPushedTotRecs < iMaxRecsToPushforAll)
                {
                    skipThisPush = "";
                    if (!String.IsNullOrEmpty(dt.Rows[iRowIdx]["APLSTTRY"].ToString().Trim()))  // Have some sort of entry..
                    {
                        if (DateTime.TryParse(dt.Rows[iRowIdx]["APLSTTRY"].ToString().Trim(), out datLastPushed) == true) // It's a date
                        {
                            ts = datNow - datLastPushed;
                            if (sWhatToDo == "SENDCALL" && (sWhyDoIt == "CREATED" || sWhyDoIt == "MOVEDFROMANOTHER"))
                            {
                                if (ts.TotalSeconds > 0 && ts.TotalMinutes < 5)
                                    skipThisPush = "Y";
                            }
                            else 
                            {
                                if (ts.TotalSeconds > 0 && ts.TotalMinutes < iMinutesToWaitBetweenPushes)
                                    skipThisPush = "Y";
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(skipThisPush))
                    {

                        iPushedEmpRecs++;
                        iPushedTotRecs++;

                        if (int.TryParse(dt.Rows[iRowIdx]["AP2DEVID"].ToString().Trim(), out iJobToDeviceId) == false)
                            iJobToDeviceId = -1;
                        if (int.TryParse(dt.Rows[iRowIdx]["APTRYCNT"].ToString().Trim(), out iPushCount) == false)
                            iPushCount = -1;
                        if (int.TryParse(dt.Rows[iRowIdx]["APSTSCNT"].ToString().Trim(), out iStatusCount) == false)
                            iStatusCount = -1;

                        if (iJobToDeviceId > 0 && String.IsNullOrEmpty(sCreateStamp))
                        {
                            UpdateCreationStamp(iUser, iJobToDeviceId);
                        }


                        sStatus = dt.Rows[iRowIdx]["APSTATUS"].ToString().Trim();

                        sFieldList = dt.Rows[iRowIdx]["APFIELDS"].ToString().Trim();

                        sValueList = dt.Rows[iRowIdx]["APVALUES"].ToString().Trim();

                        if (iUser > 0) 
                        {
                            sWsKey = Get_Ws_Key(iUser, sWhatToDo);
                            // ----------------------------------------
                            if (sLibrary == "OMDTALIB")
                            {
                                sResponseToOmaha = wsLive.Process_OmahaToDeviceJob(sWsKey, iUser.ToString("0"), sWhatToDo, iJobToDeviceId.ToString(), sWhyDoIt, sFieldList, sValueList);
                            }
                            else
                            {
                                sResponseToOmaha = wsDev.Process_OmahaToDeviceJob(sWsKey, iUser.ToString("0"), sWhatToDo, iJobToDeviceId.ToString(), sWhyDoIt, sFieldList, sValueList);
                            }
                            if (String.IsNullOrEmpty(sResponseToOmaha))
                                sResponseToOmaha = "EMPTY RESPONSE: Lib --" + sLibrary + " Job: " + sWhatToDo;

                            // ----------------------------------------
                            // Update ANPUSHQ "try count" with this attempt (job deletion will occur after device confirmation)
                            // ----------------------------------------
                            saTotqCurq = Select_JobToDeviceCounts(iUser, iJobToDeviceId);
                            if (saTotqCurq.Length > 1)
                            {
                                if (int.TryParse(saTotqCurq[0], out iTotalAttempts) == false)
                                    iTotalAttempts = 0;
                                if (int.TryParse(saTotqCurq[1], out iCurrentAttempts) == false)
                                    iCurrentAttempts = 0;

                                // ================================================================================
                                // DELETE JOBS THAT HAVE HAD ENOUGH CHANCES TO BE DELIVERED
                                // ================================================================================
                                if (
                                       (sWhatToDo == "STAMP_HOME" && iCurrentAttempts > 8)
                                    || (sWhatToDo == "DELETECALL" && iCurrentAttempts > 12)
                                    || (sWhatToDo == "SYNC_ALL" && sWhyDoIt == "WEEKLY_UPDATE" && iCurrentAttempts > 4)
                                    || (sWhatToDo == "ADMIN_UPDATE" && iCurrentAttempts > 3)
                                    || (sWhatToDo == "SEND_NEWS" && iCurrentAttempts > 3)
                                    )
                                {
                                    iRowsAffected = Delete_Job(iJobToDeviceId);
                                }
                                // --------------------------------------------------------------------------------

                                // ================================================================================
                                // EMAIL TO BREAK ANDROID DOZE MODE
                                // ================================================================================

                                if (   iUser != 1798 // Clay Cardwell 
                                && iUser != 2092 // Rich Moore 401
                                && iUser != 1715 // Paul Gajus 402
                                && iUser != 1796 // John Martinez 403
                                && iUser != 1199 // Bill Garrod 404
                                && iUser != 1124 // Steve Baehr 405
                                && iUser != 1137 // Terry Sandrock 406
                                && iUser != 1626 // Steve Greczmiel 407
                                && iUser != 2070 // Isabel Labrador 
                                && iUser != 2214 // Sarah Engels
                                && iUser != 2414 // Chris Reinhardt
                                ) 
                                {
                                    if (sWhatToDo == "SENDCALL" || sWhatToDo == "SEND_CALL") 
                                    {
                                        if (sWhyDoIt == "CREATED" || sWhatToDo == "MOVEDTOANOTHER")
                                        {

                                            saCenterTicket = sValueList.Split('|');

                                            if (saCenterTicket.Length > 1)
                                            {
                                                if (int.TryParse(saCenterTicket[0], out iCenter) == false)
                                                    iCenter = -1;
                                                if (int.TryParse(saCenterTicket[1], out iTicket) == false)
                                                    iTicket = -1;
                                                if (iCenter > 0 && iTicket > 0)
                                                {
                                                    string sCallNotifiedByTech = Seek_NotificationByTech(iCenter, iTicket, iUser);

                                                    if (sCallNotifiedByTech == "Y" && iCurrentAttempts >= 2) // Call has been notified
                                                    {
                                                        iRowsAffected = Delete_Job(iJobToDeviceId);
                                                        /* March 26, 2021 commented after seeing emails long enough to be confident it is working
                                                        // Send to me (debug visibility) 
                                                        emh.EmailIndividual2(
                                                          "SENDCALL job deleted " + iCenter + "-" + iTicket
                                                        , "Previous Attempts to deliver: " + iCurrentAttempts + " User: " + iUser + " " + Select_EmpName(iUser)
                                                        , "steve.carlson@scantron.com"
                                                        , "adv320@scantron.com"
                                                        , "HTML"); 
                                                        */
                                                    }
                                                    else
                                                    {
                                                        if (iCurrentAttempts == 3 || iCurrentAttempts == 6 || iCurrentAttempts == 11)
                                                        {
                                                            DayOfWeek today = DateTime.Today.DayOfWeek;

                                                            if (
                                                                today != DayOfWeek.Saturday &&
                                                                today != DayOfWeek.Sunday &&
                                                                DateTime.Now.Hour >= 7 &&
                                                                DateTime.Now.Hour <= 18
                                                                ) // 8am Eastern to 4pm west coast
                                                            {
                                                                // -----------------------------------------------------
                                                                sEmployeeEmail = GetEmpEmail(iUser);

                                                                // Make your CtrTck 111-2222 to compare with previous emails
                                                                sCtrTck = iCenter + "-" + iTicket;
                                                                //erh.SaveErrorText("(" + iCurrentAttempts + "x) " + iCenter + "-" + iTicket + " Doze Mode Ticket Notification", "For " + sEmployeeEmail, "Tickets Processed this pass: " + sTicketsProcessed);
                                                                if (sTicketsProcessed.Contains(sCtrTck) == false)
                                                                {
                                                                    sCallSummary = Select_CallSummary(iCenter, iTicket, iCurrentAttempts);
                                                                    sCallSubject = "UNDELIVERED CALL " + iCenter + "-" + iTicket + " (" + iCurrentAttempts + " Prior";
                                                                    if (iCurrentAttempts == 1)
                                                                        sCallSubject += " Attempt)";
                                                                    else
                                                                        sCallSubject += " Attempts)";

                                                                    // Send to tech
                                                                    emh.EmailIndividual2(
                                                                          sCallSubject
                                                                        , sCallSummary
                                                                        , sEmployeeEmail
                                                                        , "steve.carlson@scantron.com"
                                                                        , "HTML");
                                                                    /*

                                                                    // Send to me (debug visibility) 
                                                                    emh.EmailIndividual2(
                                                                      sCallSubject
                                                                    , sCallSummary
                                                                    , "steve.carlson@scantron.com"
                                                                    , sEmployeeEmail
                                                                    , "HTML");
                                                                    */
                                                                }
                                                                sTicketsProcessed += sCtrTck + "X";

                                                                // -----------------------------------------------------
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                } // Exluding Managers and IT
                                // ================================================================================
                                iRowsAffected = Update_JobToDeviceCounts(iUser, iJobToDeviceId, iTotalAttempts, iCurrentAttempts);
                            }

                            // ----------------------------------------
                        }
                    }
                }
                iLatestEmp = iUser;
                iRowIdx++;
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
    }
    // ========================================================================
    private int UpdateCreationStamp(int user, int jobToDeviceId)
    {
        int iRowsAffected = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);
        string sSql = "";

        try
        {
            sSql = " update " + sLibrary + ".ANPUSHQ set" +
                 " APCREATE = ?" +
                " where AP2DEVID = ?" +
                " and APUSER = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Created", DateTime.Now.ToString("o"));
            odbcCmd.Parameters.AddWithValue("@JobToDeviceID", jobToDeviceId);
            odbcCmd.Parameters.AddWithValue("@User", user);

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
    private int Delete_Job(int jobToDeviceId)
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        try
        {
            sSql = " delete from " + sLibrary + ".ANPUSHQ" +
                " where AP2DEVID = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@JobToDeviceID", jobToDeviceId);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Failure while deleting notified call - ID: " + jobToDeviceId);
            iRowsAffected = -1;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected string[] Select_JobToDeviceCounts(int user, int jobToDeviceID)
    {
        string[] saTotqCurq = { "", "" };

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        try
        {
            sSql = " select " +
                 " APTRYCNT" +
                ", APSTSCNT" +
                " from " + sLibrary + ".ANPUSHQ" +
                " where AP2DEVID = ?" +
                " and APUSER = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@JobToDeviceID", jobToDeviceID);
            odbcCmd.Parameters.AddWithValue("@User", user);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                saTotqCurq[0] = dt.Rows[0]["APTRYCNT"].ToString().Trim();
                saTotqCurq[1] = dt.Rows[0]["APSTSCNT"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return saTotqCurq;
    }
    // ========================================================================
    protected int Update_JobToDeviceCounts(int user, int jobToDeviceID, int pushCount, int statusCount)
    {
        int iRowsAffected = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";
        string sStatus = "";

        try
        {
            sSql = " update " + sLibrary + ".ANPUSHQ set" +
                 " APTRYCNT = ?" +
                ", APSTSCNT = ?" +
                ", APLSTTRY = ?" +
                ", APSTATUS = ?" +
                " where AP2DEVID = ?" +
                " and APUSER = ?";

            pushCount++;
            statusCount++;

            if (statusCount < iPushAging) sStatus = "NEW";
            else if (statusCount < iPushHold) sStatus = "AGING";
            else sStatus = "HOLD";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@TryCount", pushCount);
            odbcCmd.Parameters.AddWithValue("@StsCount", statusCount);
            odbcCmd.Parameters.AddWithValue("@LastTry", DateTime.Now.ToString("o"));
            odbcCmd.Parameters.AddWithValue("@Status", sStatus);
            odbcCmd.Parameters.AddWithValue("@JobToDeviceID", jobToDeviceID);
            odbcCmd.Parameters.AddWithValue("@User", user);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected string Select_CallSummary(int center, int ticket, int currentAttempts)
    {
        string sSummary = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";
        int iExt = 0;

        try
        {
            sSql = " select " +
                 " STCUS1" +
                ", STCUS2" +
                ", SDCSTN" +
                ", TCOMM1" +
                ", TCOMM2" +
                ", SDCONT" +
                ", SDPHN#" +
                ", SDPHNE" +
                ", SDADR1" +
                ", SDADR2" +
                ", SDCITY" +
                ", SDSTAT" +
                ", SDZIPC" +
                " from " + sLibrary + ".SVRTICK, " + sLibrary + ".SVRTICKD" +
                " where TCCENT = SDCENT" +
                " and TICKNR = SDTNUM" +
                " and TCCENT = ?" +
                " and TICKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            NumberFormatter numberFormatter = new NumberFormatter();
            String sPhone = numberFormatter.phoneFormat1(dt.Rows[0]["SDPHN#"].ToString().Trim());

            if (dt.Rows.Count > 0)
            {
                sSummary = "<table>" +
                "<tr style=\"vertical-align:top;\"><td>Ticket</td><td>" + center + "-" + ticket + "</td></tr>" +
                "<tr style=\"vertical-align:top;\"><td>Customer</td><td>" + dt.Rows[0]["SDCSTN"].ToString().Trim() + " (" + dt.Rows[0]["STCUS1"].ToString().Trim() + "-" + dt.Rows[0]["STCUS2"].ToString().Trim() + ")" + "</td></tr>" +
                "<tr style=\"vertical-align:top;\"><td>Problem</td><td>" + dt.Rows[0]["TCOMM1"].ToString().Trim() + " " + dt.Rows[0]["TCOMM2"].ToString().Trim() + "</td></tr>" +
                "<tr style=\"vertical-align:top;\"><td>Address</td><td>" + dt.Rows[0]["SDADR1"].ToString().Trim() + " " + dt.Rows[0]["SDADR2"].ToString().Trim() + " " + dt.Rows[0]["SDCITY"].ToString().Trim() + " " + dt.Rows[0]["SDSTAT"].ToString().Trim() + "</td></tr>" +
                "<tr style=\"vertical-align:top;\"><td>Contact</td><td>" + dt.Rows[0]["SDCONT"].ToString().Trim() + " " + sPhone;
                if (int.TryParse(dt.Rows[0]["SDPHNE"].ToString().Trim(), out iExt) == false)
                    iExt = -1;
                if (iExt > 0)
                    sSummary += " Ext: " + iExt.ToString();
                sSummary += "</td></tr>";
                sSummary += "</table>";

                if (currentAttempts == 12) // 12 will never fire...
                {
                    sSummary += "<br /><br /><i>Have you noticed that often your calls aren't delivered until you pick up your phone and swipe open the app?  " +
                        "It's called \"Doze Mode\".  " +
                        "To save battery, after a period of inactivity (starting at 30 minutes) Android puts the device into deeper and deeper levels of sleep.  " +
                        "This postpones the background activity of all apps so the device only has to wake up once, and then all processes are run at the same time. " +
                        "Then the device goes back to sleep.  " +
                        "The longer the device is inactive, the longer it sleeps during the next period.  " +
                        "<br /><br />While working on recent updates for the app, I have tried repeatedly (and failed) to break through this doze mode and deliver your calls.  " +
                        "So at this point, I'm experimenting with another route.  The most popular apps on Google Play (and email) are granted an exception to interrupt doze mode.  " +
                        "<br /><br />So here's what I'm trying now.  " +
                        "When a new call has been sent but not delivered, every 5 minutes I try again but also send this email -- after 3 (@15 min), 6 (@30 min) and 11 (@55 min) failed delivery attempts (on weekdays during working hours). " +
                        "If you've got sound turned on for arriving email (in settings) it can break through doze mode at any time. (Consider choosing a unique sound...)  " +
                        "Now I know you already get hammered with emails, but it's my best option at this point. As always, I'm open to discussion about how this experiment is actually working out in the field. " +
                        "<br /><br />Thanks, Steve";
                }
                else 
                {
                    //sSummary += "<br /><br /> These emails serve as \"notification backup\" for a device that may have dropped into doze mode.";
                    //sSummary += "<br /><br /> Call missing? Sync. <br /><br />(These reminder emails are sent when Omaha isn't receiving confirmation back of a successfully delivery.)";
                    sSummary += "<br /><br /> Call missing? Sync... <br /><br />(These reminder emails are sent when Omaha hasn't received normal delivery confirmation from your phone.)";
                }

            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sSummary;
    }
    // ========================================================================
    protected string Seek_NotificationByTech(int center, int ticket, int user)
    {
        string sTechNotifyStampFound = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        try
        {
            sSql = "Select" +
                 " TIMSTS" +
                " from " + sLibrary + ".TIMESTMP" +
                " where TIMCTR = ?" + 
                " and TIMTCK = ?" +
                " and TIMEMP = ?" +
                " and TIMSTS = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            
            odbcCmd.Parameters.AddWithValue("@Ctr", center);
            odbcCmd.Parameters.AddWithValue("@Tck", ticket);
            odbcCmd.Parameters.AddWithValue("@Emp", user);
            odbcCmd.Parameters.AddWithValue("@Sts", "N");

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                sTechNotifyStampFound = "Y";
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Failure while seeking notification by tech on call " + center + "-" + ticket + " User: " + user);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sTechNotifyStampFound;
    }
    // ========================================================================
    protected string Select_EmpName(int user)
    {
        string sEmpName = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        try
        {
            sSql = "Select" +
                 " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Usr", user);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                sEmpName = dt.Rows[0]["EMPNAM"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Failure getting emp name");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sEmpName;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    // ========================================================================
}
