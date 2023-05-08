using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration; // for Connection String
using System.Data;          // for DataTable
using System.Web.UI;

/// <summary>
/// Summary description for TimestampHandler
/// </summary>

namespace CoreWs.Helpers
{
    public class TimestampHandler
    {
        // ---------------------------------------
        // GLOBAL VARIABLES 
        // ---------------------------------------

        // ========================================================================
        private string sLibrary = "OMDTALIB";
        private int iGlobalUser = 0;
        private int iOriginalKey = 0;
        private int iTryCount = 0;

        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        SqlDataReader sqlReader;

        OdbcConnection odbcConn;
        OdbcCommand odbcCmd;
        OdbcDataReader odbcReader;

        //private string sTemp = "";

        DateTime datNow;
        MyPage myPage;
        // -----------------------------------------------------
        public TimestampHandler(string library)
        {
            sLibrary = library;
            

            datNow = DateTime.Now;
            myPage = new MyPage();
        }

        // -----------------------------------------------------
        // ========================================================================
        // PUBLIC METHODS
        // ========================================================================
        public string ProcessTimestamp(int user, int ctr, int tck, string statusCode, string reasonCode, DateTime dateStamped)
        {
            iGlobalUser = user; // for error messaging...
            string sResult = "";
            int iRowsAffected = 0;
            //ErrorHandler erh = new ErrorHandler(sLibrary, _configuration);

            odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

            //KeyHandler kh;
            KeyRetriever kr;
            // EmailHandler emh;

            string sTemp = "";
            int iStampDate = 0;
            double dStampTime = 0.0;
            double[] daSchedDatTim;
            int iScheduleDate = 0;
            int iNewKey = 0;
            double dScheduleTime = 0.0;
            string sRemark = "";
            string sRemarkCurrent = "";

            try
            {
                odbcConn.Open();  // Open on your "gate" methods (normally public)


                if (int.TryParse(dateStamped.ToString("yyyyMMdd"), out iStampDate) == false)
                {
                    iStampDate = 0;
                }

                sTemp = dateStamped.ToString("HH.mm");
                if (double.TryParse(sTemp, out dStampTime) == false)
                {
                    dStampTime = 0.0;
                }

                // Check if stamp is already applied (if not)
                string sStampExists = DoesStampAlreadyExist(user, ctr, tck, statusCode, reasonCode, iStampDate, dStampTime);
                if (sStampExists == "Y")
                    sResult = "Error: Stamp already exists";
                else if (dStampTime == 0.0)
                    sResult = "Error: " + statusCode + " stamp failed with time that is not processing (" + sTemp + ").";
                else
                {
                    // Get "Schedule Date/Time" from TCKASSN
                    daSchedDatTim = GetScheduleDateTime(ctr, tck);
                    if (daSchedDatTim.Length > 0)
                        int.TryParse(daSchedDatTim[0].ToString(), out iScheduleDate);
                    if (daSchedDatTim.Length > 1)
                        dScheduleTime = daSchedDatTim[1];

                    // ````````````````````````````````````````````````
                    // DO SVRTICK REMARK FIRST TO ENSURE TEXT UPDATE
                    // ````````````````````````````````````````````````
                    if (statusCode == "C")
                    {
                        try
                        {
                            iRowsAffected = UpdateSvrtickRemark(ctr, tck, "WORK DONE-AWAITS CLOSURE");
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamp C: Svrtick Remark Update");
                        }
                    }
                    // Insert values into TIMESTMP
                    try
                    {
                        iRowsAffected = AddTimestamp(user, ctr, tck, statusCode, reasonCode, iStampDate, dStampTime, iScheduleDate, dScheduleTime);
                    }
                    catch (Exception ex2)
                    {
                        myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamp ANY: Add to TIMESTMP");
                    }

                    if (iRowsAffected > 0)
                        sResult = "SUCCESS: Added Timestamp to call " + ctr + "-" + tck;

                    string sReasonDesc = GetReasonDesc(reasonCode);
                    string[] saCs1Cs2DatTimPriCodTyp = GetSvrtickData(ctr, tck);

                    // -------------------------------------------------------
                    int iCustomerNumber = GetCs1(ctr, tck);
                    // -------------------------------------------------------

                    // Further processing for specific stamps
                    if (statusCode == "T" || statusCode == "S" || statusCode == "$")
                    {

                        if (statusCode == "T")
                        {
                            if (sRemarkCurrent == "WORK DONE-AWAITS CLOSURE")
                                sRemark = sRemarkCurrent;
                            else
                                sRemark = "TECHNICIAN EN ROUTE";
                            // You need to run program that checks for other tickets OTHRTCKS
                            if (CheckForcedCustomer(ctr, tck) != "F")
                            {
                                iRowsAffected = 0; // Initialize
                                try
                                {
                                    kr = new KeyRetriever(sLibrary);
                                    iNewKey = kr.GetKey("TRIGMAST");
                                    kr = null;
                                    iRowsAffected = AddParmsToTRIGMAST_Travel(iNewKey, user, ctr, tck);
                                }
                                catch (Exception ex2)
                                {
                                    // I don't think these are ever being used due to the catch inside the sql
                                    myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamp T: Trigmast Travel Email -- 1st Attempt Failure NewKey: " + iNewKey.ToString());
                                }
                            }
                        }
                        else if (statusCode == "$")
                        {
                            if (sRemarkCurrent == "WORK DONE-AWAITS CLOSURE")
                                sRemark = sRemarkCurrent;
                            else
                                sRemark = "REMOTE START";
                        }
                        else
                        {
                            if (sRemarkCurrent == "WORK DONE-AWAITS CLOSURE")
                                sRemark = sRemarkCurrent;
                            else
                                sRemark = "TECHNICIAN ONSITE";
                        }

                        // Close any non productive tickets 
                        // I don't think I should do this because the work market tech may have an STS center ticket
                        /*
                        try
                        {
                            int iUserCenter = GetEmpCenter(user);
                            CloseAnyNonProductiveCalls(user, iUserCenter, iStampDate, dStampTime);
                            // You need a secondary clean up for TCKASSN
                            CloseTCKASSNLeftovers(iStampDate, dStampTime);
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamps T S $: NonProductive Close" + " User: " + iGlobalUser.ToString());
                        }
                        */
                    }
                    //------------------------------------------------------------------------------
                    // COMPLETE STAMP
                    else if (statusCode == "C")
                    {
                        // ````````````````````````````````````````````````
                        // UPDATE SVRTICK
                        // ````````````````````````````````````````````````
                        try
                        {
                            sRemark = "WORK DONE-AWAITS CLOSURE";
                            iRowsAffected = UpdateSvrtickd(ctr, tck, iStampDate, dStampTime, "MARKET");
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Android Timestamp C: SvrtickD Update" + " User: " + iGlobalUser.ToString());
                        }

                        // ````````````````````````````````````````````````
                        // Check if Complete customer email is needed
                        // ````````````````````````````````````````````````
                        // Send Customer Email TSTWBB -> EMAILPCL -> (EMAILPGR) 
                        // FYI... TSTWBB Seems to update times on old similar stamps instead of creating new ones) 
                        iRowsAffected = 0;
                        try
                        {
                            kr = new KeyRetriever(sLibrary);
                            iNewKey = kr.GetKey("TRIGMAST");
                            kr = null;

                            iRowsAffected = AddParmsToTRIGMAST_Complete(iNewKey, ctr, tck);
                        }
                        catch (Exception ex2)
                        {
                            // I don't think these are ever being used due to the catch inside the sql
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "WorkMarket Timestamp C: Trigmast insert -> Complete -- 1st Attempt Failure NewKey: " + iNewKey.ToString());
                        }


                        // I don't think this applies to work market
                        /*
                        // ````````````````````````````````````````````````
                        // Check if Install Call: Change TCKASSN assignment 
                        // ````````````````````````````````````````````````
                        // Change Assignment for Install Calls (Rather than leave it on the device for a web close) 
                        try
                        {
                            if (ctr == 420 || ctr == 423)
                            {
                                iRowsAffected = UpdateTckassnForCompleteInstalls(ctr, tck);
                            }
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Android Timestamp C: Install TCKASSN Update" + " User: " + iGlobalUser.ToString());
                        }
                        */
                        // ````````````````````````````````````````````````
                        // END COMPLETE STAMP
                        // ````````````````````````````````````````````````
                    }
                    //------------------------------------------------------------------------------
                    else if (statusCode == "M")
                    {
                        sRemark = "REASSIGNED";
                    }
                    else if (statusCode == "N")
                    {
                        sRemark = "TECHNICIAN NOTIFIED";
                    }
                    else if (statusCode == "X")
                    {
                        sRemark = "TRANSMITTED";
                    }
                    else if (statusCode == "1")
                    {
                        sRemark = "REVIEWED";
                    }
                    else if (statusCode == "H")
                    {
                        // -------------------------------
                        switch (reasonCode)
                        {
                            case "CA": { sRemark = "WAITING CUSTOMER APPROVAL"; break; }
                            case "CB": { sRemark = "CUSTOMER CALLED BACK"; break; }
                            case "CC": { sRemark = "CUSTOMER CANCELLED"; break; }
                            case "CP": { sRemark = "CHANGE OF PLANS"; break; }
                            case "DC": { sRemark = "NEW TICKET/DATA CHANGE"; break; }
                            case "DU": { sRemark = "DUPLICATE CALL"; break; }
                            case "IN": { sRemark = "INCOMPLETE"; break; }
                            case "LF": { sRemark = "LEFT MESSAGE"; break; }
                            case "MI": { sRemark = "MADE A MISTAKE"; break; }
                            case "NA": { sRemark = "NEVER ARRIVED"; break; }
                            case "NH": { sRemark = "CUSTOMER NOT FOUND"; break; }
                            case "NM": { sRemark = "EQUIP NOT UNDER MAINT"; break; }
                            case "OS": { sRemark = "UNIT TO DEPOT FOR R&R"; break; }
                            case "RE": { sRemark = "REASSIGNED"; break; }
                            case "SH": { sRemark = "PULLED TO SHOP"; break; }
                            case "TE": { sRemark = "REPAIR BEING TESTED"; break; }
                            case "TS": { sRemark = "TONER HAS BEEN SHIPPED"; break; }
                            case "WP": { sRemark = "PART ORDERED"; break; }
                            case "1": { sRemark = "CUSTOMER CONTACTED"; break; }
                            case "2": { sRemark = "EQUIPMENT ORDERED"; break; }
                            case "3": { sRemark = "CABLE REV/ORG"; break; }
                            case "4": { sRemark = "EQUIPMENT RECEIVED"; break; }
                            case "5": { sRemark = "EQUIPMENT PREPARED"; break; }
                            case "6": { sRemark = "INSTALL CONFIRMED"; break; }
                            case "7": { sRemark = "EQUIPMENT SHIPPED"; break; }
                            case "8": { sRemark = "ONSITE INSTALL COMPLETED"; break; }
                            case "9": { sRemark = "BILLED/DOCUMENTED"; break; }

                                // default: { break; };
                        }
                        // -------------------------------

                        DateTime datNow = DateTime.Now;

                        int iCs1 = 0;
                        int iCs2 = 0;
                        int iPri = 0;
                        int iDatSched = 0;
                        double dTimSched = 0.0;
                        int iDatStamped = 0;
                        double dTimStamped = 0.0;
                        int iDatNow = 0;
                        double dTimNow = 0.0;
                        int.TryParse(saCs1Cs2DatTimPriCodTyp[0], out iCs1);
                        int.TryParse(saCs1Cs2DatTimPriCodTyp[1], out iCs2);
                        int.TryParse(saCs1Cs2DatTimPriCodTyp[2], out iDatSched);
                        double.TryParse(saCs1Cs2DatTimPriCodTyp[3], out dTimSched);
                        int.TryParse(saCs1Cs2DatTimPriCodTyp[4], out iPri);
                        int.TryParse(dateStamped.ToString("yyyyMMdd"), out iDatStamped);
                        double.TryParse(dateStamped.ToString("HH.mm"), out dTimStamped);
                        int.TryParse(datNow.ToString("yyyyMMdd"), out iDatNow);
                        double.TryParse(datNow.ToString("HH.mm"), out dTimNow);

                        try
                        {
                            iRowsAffected = AddReschedAudit(user, ctr, tck, iCs1, iCs2, sReasonDesc, iDatSched, dTimSched, iDatStamped, dTimStamped, iDatNow, dTimNow);
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Android Timestamp H: ReschedAudit" + " User: " + iGlobalUser.ToString());
                        }
                        try
                        {
                            string sCurrStatus = GetEscalationStamp(ctr, tck);
                            iRowsAffected = UpdateEscalation(ctr, tck, sCurrStatus, statusCode, iDatNow, dTimNow, iPri);
                        }
                        catch (Exception ex2)
                        {
                            myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Android Timestamp H: Update Escalation" + " User: " + iGlobalUser.ToString());
                        }
                        // -------------------------------
                    }
                    else
                    {
                    }

                    // ---------------------------------
                    // Do the following for ALL Stamps...
                    // ---------------------------------
                    string sCurrStamp = saCs1Cs2DatTimPriCodTyp[5];

                    try
                    {
                        // 
                        if (statusCode == "C" && sRemark != "WORK DONE-AWAITS CLOSURE")
                        {
                            myPage.SaveError("Work Marekt Work Done Awaits Closure CHANGED somehow", ctr + "-" + tck, "Bad remark" + sRemark + " User: " + iGlobalUser.ToString());
                            sRemark = "WORK DONE-AWAITS CLOSURE";
                        }

                        // Update SVRTICK/Remark
                        iRowsAffected = UpdateSvrtick(ctr, tck, statusCode, reasonCode, sCurrStamp);
                        iRowsAffected = UpdateSvrtickRemark(ctr, tck, sRemark);
                    }
                    catch (Exception ex2)
                    {
                        myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamp ALL: Svrtick Update");
                    }

                    // Update TCKASSN with date/time of this new stamp (don't update with B=Busy or M=Move)
                    try
                    {
                        if (statusCode != "M")
                            iRowsAffected = UpdateTckassnWithStamp(ctr, tck, statusCode, iStampDate, dStampTime);
                    }
                    catch (Exception ex2)
                    {
                        myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Work Market Timestamp All (but M): TckAssn Update" + " User: " + iGlobalUser.ToString());
                    }

                    // Delete any "Notifies" (NOTIFL8 or NOTIFPF) that are over 2 minutes old
                    try
                    {
                        iRowsAffected = DeleteOldNoteAlerts(user);
                    }
                    catch (Exception ex2)
                    {
                        myPage.SaveError(ex2.Message.ToString(), ex2.ToString(), "Android Timestamp C: SvrtickD Update" + " User: " + iGlobalUser.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Timestamp Final Catch Error " + " User: " + iGlobalUser.ToString());
                iRowsAffected = -1;
            }
            finally
            {
                odbcConn.Close();
                myPage = null;
            }

            return sResult;
        }
        // ========================================================================
        #region mySqls
        // ========================================================================
        private string DoesStampAlreadyExist(int user, int ctr, int tck, string statusCode, string reasonCode, int stampDate, double stampTime)
        {
            string sStampExists = "";
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " TIMSTS" +
                " from " + sLibrary + ".TIMESTMP" +
                " where TIMCTR = ?" + // Center
                " and TIMTCK = ?" +  // Ticket
                " and TIMTCH = ?" +  // Tech Stamping
                " and TIMSTS = ?" + // Status Code
                " and TIMRSC = ?" + // Reason Code
                " and TIMDST = ?" + // Date stamped
                " and TIMTST = ?";  // Time stamped

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);
                odbcCmd.Parameters.AddWithValue("@Emp", user);
                odbcCmd.Parameters.AddWithValue("@Sts", statusCode);
                odbcCmd.Parameters.AddWithValue("@Rsn", reasonCode);
                odbcCmd.Parameters.AddWithValue("@Dat", stampDate);
                odbcCmd.Parameters.AddWithValue("@Tim", stampTime);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                    sStampExists = "Y";
            }
            catch (Exception ex)
            {
                string sStampData = "Ctr: " + ctr +
        " -- Tck: " + tck +
        " -- User: " + user +
        " -- StsCod: " + statusCode +
        " -- RsnCod: " + reasonCode +
        " -- StampDate: " + stampDate +
        " -- StampTime: " + stampTime +
        "";
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), sStampData);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sStampExists;
        }
        // ========================================================================
        private double[] GetScheduleDateTime(int ctr, int tck)
        {
            double[] daDatTim = { 0.0, 0.0 };
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " ASCHDT" +
                ", TAATIM" +
                " from " + sLibrary + ".TCKASSN" +
                " where TACENT = ?" + // Center
                " and ATCK# = ?";  // Ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows) // there should only be one rec...
                {
                    if (double.TryParse(dt.Rows[iRowIdx]["ASCHDT"].ToString().Trim(), out daDatTim[0]) == false)
                        daDatTim[0] = -1;
                    if (double.TryParse(dt.Rows[iRowIdx]["TAATIM"].ToString().Trim(), out daDatTim[1]) == false)
                        daDatTim[1] = -1;
                    iRowIdx++;
                }
            }
            catch (Exception ex)
            {
                string sStampData = "Ctr: " + ctr +
                    " -- Tck: " + tck +
                    "";

                myPage.SaveError(ex.Message.ToString(), ex.ToString(), sStampData);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return daDatTim;
        }
        // ========================================================================
        private int AddTimestamp(int user, int ctr, int tck, string statusCode, string reasonCode, int stampDate, double stampTime, int scheduleDate, double scheduleTime)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "insert into " + sLibrary + ".TIMESTMP (" +
                      "TIMCTR" + // center
                    ", TIMTCK" + // ticket
                    ", TIMTCH" + // tech assigned
                    ", TIMEMP" + // emp entering stamp
                    ", TIMSTS" + // status code
                    ", TIMRSC" + // reason code
                    ", TIMDST" + // stamp date
                    ", TIMTST" + // stamp time
                    ", TIMSDT" + // schedule date
                    ", TIMSTM" + // schedule time
                    ") values(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);
                odbcCmd.Parameters.AddWithValue("@EmpAssigned", user);
                odbcCmd.Parameters.AddWithValue("@EmpStamping", user);
                odbcCmd.Parameters.AddWithValue("@StsCode", statusCode);
                odbcCmd.Parameters.AddWithValue("@RsnCode", reasonCode);
                odbcCmd.Parameters.AddWithValue("@StampDate", stampDate);
                odbcCmd.Parameters.AddWithValue("@StampTime", stampTime);
                odbcCmd.Parameters.AddWithValue("@SchedDate", scheduleDate);
                odbcCmd.Parameters.AddWithValue("@SchedTime", scheduleTime);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sStampData = "Ctr: " + ctr +
                    " -- Tck: " + tck +
                    " -- User: " + user +
                    " -- StsCod: " + statusCode +
                    " -- RsnCod: " + reasonCode +
                    " -- StampDate: " + stampDate +
                    " -- StampTime: " + stampTime +
                    " -- SchedDate: " + scheduleDate +
                    " -- SchedTime: " + scheduleTime +
                    "";
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), sStampData);
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int UpdateTckassnWithStamp(int ctr, int tck, string statusCode, int stampDate, double stampTime)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";
            //string sSecondaryStamp = "";

            try
            {
                sSql = "update " + sLibrary + ".TCKASSN set" +
                 " ALSTDT = ?" + // last date stamped
                ", ALSTTM = ?" + // last time stamped
                ", ATCKST = ?"; // regular stamp
                if (statusCode == "H")
                    sSql += ", ATKSTS = ?"; // end of day stamp stamp
                sSql += " where ATCKST not in ('C')" + // center
                    " and TACENT = ?" + // center
                    " and ATCK# = ?";  // ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.AddWithValue("@StampDate", stampDate);
                odbcCmd.Parameters.AddWithValue("@StampTime", stampTime);
                odbcCmd.Parameters.AddWithValue("@StatusCode", statusCode);
                if (statusCode == "H")
                    odbcCmd.Parameters.AddWithValue("@SecondCode", statusCode);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sStampData = "Ctr: " + ctr +
        " -- Tck: " + tck +
        " -- StsCod: " + statusCode +
        " -- StampDate: " + stampDate +
        " -- StampTime: " + stampTime +
        "";

                myPage.SaveError(ex.Message.ToString(), ex.ToString(), sStampData);
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int UpdateTckassnForCompleteInstalls(int ctr, int tck)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".TCKASSN set" +
                    " AEMP# = 421" +
                    " where TACENT = ?" + // center
                    " and ATCK# = ?";  // ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int GetEmpCenter(int user)
        {
            int iEmpCenter = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " ECENT" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Emp", user);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["ECENT"].ToString().Trim(), out iEmpCenter) == false)
                        iEmpCenter = 0;
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iEmpCenter;
        }
        // ========================================================================
        private int GetEmpDepartment(int user)
        {
            int iEmpDept = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " EMPDEP" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Emp", user);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["EMPDEP"].ToString().Trim(), out iEmpDept) == false)
                        iEmpDept = 0;
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iEmpDept;
        }
        // ========================================================================
        private string CloseAnyNonProductiveCalls(int user, int userCenter, int stampDate, double stampTime)
        {
            string sResult = "";
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select " +
                 " TCCENT" +
                ", TICKNR" +
                " from " + sLibrary + ".SVRTICK" +
                " where CALLCD = 'N'" +
                " and PROCCD = ''" +
                " and PBMCD = 9" +
                " and (STCUS1 >= 90 and STCUS1 <= 99)" +
                " and TCCENT = ?" +
                " and EMPR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", userCenter);
                odbcCmd.Parameters.AddWithValue("@Emp", user);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                int iCtr = 0;
                int iTck = 0;

                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (int.TryParse(dt.Rows[iRowIdx]["TCCENT"].ToString().Trim(), out iCtr) == false)
                        iCtr = 0;
                    if (int.TryParse(dt.Rows[iRowIdx]["TICKNR"].ToString().Trim(), out iTck) == false)
                        iTck = 0;
                    if (iCtr > 0 && iTck > 0)
                    {
                        iRowsAffected = CloseNonProductiveUserCall(user, iCtr, iTck, stampDate, stampTime);
                        iRowsAffected = CloseTCKASSN(iCtr, iTck, stampDate, stampTime);
                    }
                    iRowIdx++;
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sResult;
        }
        // ========================================================================
        private int CloseNonProductiveUserCall(int user, int ctr, int tck, int stampDate, double stampTime)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".SVRTICK set" +
                     " PROCCD = 'C'" +
                    ", POSTCD = 'A'" +
                    ", PRTCD = 'I'" +
                    ", TRDATE = ?" +
                    ", TCDATE = ?" +
                    ", ENDTIM = ?" +
                    ", EMPC = ?" +
                    " where TCCENT = ?" + // center
                    " and TICKNR = ?";  // ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@RunDate", stampDate);
                odbcCmd.Parameters.AddWithValue("@CloseDate", stampDate);
                odbcCmd.Parameters.AddWithValue("@CloseTime", stampTime);
                odbcCmd.Parameters.AddWithValue("@CloseEmp", user);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int CloseTCKASSN(int ctr, int tck, int stampDate, double stampTime)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".TCKASSN set" +
                     " ATKSTS = 'C'" + // Assignment Status
                    ", ATCKST = 'C'" + // Ticket Header Status
                    ", ALSTDT = ?" + // last date stamped
                    ", ALSTTM = ?" + // last time stamped
                    " where TACENT = ?" + // center
                    " and ATCK# = ?";  // ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@StampDate", stampDate);
                odbcCmd.Parameters.AddWithValue("@StampTime", stampTime);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private string CloseTCKASSNLeftovers(int stampDate, double stampTime)
        {
            string sResult = "";
            int iRowsAffected = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select " +
                 " TCCENT" +
                ", TICKNR" +
                ", EMPR" +
                " from " + sLibrary + ".SVRTICK s, " + sLibrary + ".TCKASSN t" +
                " where TCCENT = TACENT" +
                " and TICKNR = ATCK#" +
                " and (STCUS1 >= 90 and STCUS1 <= 99)" +
                " and s.PROCCD = 'C'" +
                " and ATCKST not in ('C')" +
                " and TEDATE > 20150101" +
                " and EMPR > 1000" +
                " and TCCENT < 1000";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                int iCtr = 0;
                int iTck = 0;

                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (int.TryParse(dt.Rows[iRowIdx]["TCCENT"].ToString().Trim(), out iCtr) == false)
                        iCtr = 0;
                    if (int.TryParse(dt.Rows[iRowIdx]["TICKNR"].ToString().Trim(), out iTck) == false)
                        iTck = 0;
                    if (iCtr > 0 && iTck > 0)
                    {
                        iRowsAffected = CloseTCKASSN(iCtr, iTck, stampDate, stampTime);
                    }
                    iRowIdx++;
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sResult;
        }

        // ========================================================================
        //private int UpdateSvrtick(int ctr, int tck, string remark, string statusCode, string reasonCode, string currStamp)
        private int UpdateSvrtick(int ctr, int tck, string statusCode, string reasonCode, string currStamp)
        {
            int iRowsAffected = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            // PROCCD can only be H, C or V
            string sChangeCodeTo = "";
            if (
                (currStamp == "" && statusCode == "H") // 1st hold
                ||
                (currStamp == "H" && statusCode == "H") // Updated to another Hold
                )
                sChangeCodeTo = "HOLD";
            else if (currStamp == "H" && (statusCode == "T" || statusCode == "S" || statusCode == "$" || statusCode == "C")) // or B or A which Android doesn't offer
                sChangeCodeTo = "BLANKS";

            if (sChangeCodeTo != "" && (statusCode != "" || reasonCode != ""))
            {
                try
                {
                    sSql = "update " + sLibrary + ".SVRTICK set" +
                         " PROCCD = ?" +
                        ", TKRSCD = ?" +
                        " where TCCENT = ?" +
                        " and TICKNR = ?";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    if (sChangeCodeTo == "HOLD")
                    {
                        odbcCmd.Parameters.AddWithValue("@Status", statusCode);
                        odbcCmd.Parameters.AddWithValue("@Reason", reasonCode);
                    }
                    else if (sChangeCodeTo == "BLANKS")
                    {
                        odbcCmd.Parameters.AddWithValue("@Status", "");
                        odbcCmd.Parameters.AddWithValue("@Reason", "");
                    }
                    //}
                    odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                    odbcCmd.Parameters.AddWithValue("@Tck", tck);

                    iRowsAffected = odbcCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Timestamp SVRTICK Update without Remark -- User: " + iGlobalUser.ToString());
                    iRowsAffected = -1;
                }
                finally
                {
                    odbcCmd.Dispose();
                }
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int UpdateSvrtickRemark(int ctr, int tck, string remark)
        {
            int iRowsAffected = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".SVRTICK set" +
                     " REMARK = ?" +
                     //", PROCCD = ?" +
                     //", TKRSCD = ?" +
                     " where TCCENT = ?" +
                     " and TICKNR = ?" +
                     " and IFNULL((select REMARK from " + sLibrary + ".SVRTICK where TCCENT = ? and TICKNR = ?),'') not in ('WORK DONE-AWAITS CLOSURE')";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Rmk", remark);
                //odbcCmd.Parameters.AddWithValue("@Cod", code);
                //odbcCmd.Parameters.AddWithValue("@Rsn", reason);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);
                odbcCmd.Parameters.AddWithValue("@Ctr2", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck2", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "CRASH: Timestamp SVRTICK Remark Update Inside Sql... " + " User: " + iGlobalUser.ToString());
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int UpdateSvrtickEmpr(int ctr, int tck, int empr)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".SVRTICK set" +
                     " empr = ?";
                sSql += " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Rempr", empr);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Empr-SVRTICK Update" + " User: " + iGlobalUser.ToString());
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int DeleteOldNoteAlerts(int user) // generally known as "notifies" 
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            DateTime datTemp = DateTime.Now.AddMinutes(-2);
            int iDate = 0;
            double dTime = 0.0;
            string sSql = "";

            try
            {
                int.TryParse(datTemp.ToString("yyyyMMdd"), out iDate);
                double.TryParse(datTemp.ToString("HH.mm"), out dTime);

                sSql = "delete from " + sLibrary + ".NOTIFL8" +
                " where NTFEMP = ?" +
                " and (NTFDAT < ?" +
                " or (NTFDAT = ? and NTFTIM < ?))";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Emp", user);
                odbcCmd.Parameters.AddWithValue("@DateLess", iDate);
                odbcCmd.Parameters.AddWithValue("@DateEqual", iDate);
                odbcCmd.Parameters.AddWithValue("@TimeLess", dTime);

                iRowsAffected = odbcCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private string GetReasonDesc(string reasonCode)
        {
            string sReasonDesc = "";
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " RSNDSC" +
                " from " + sLibrary + ".RSNCODPF" +
                " where RSNCD = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Code", reasonCode);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                    sReasonDesc = dt.Rows[0]["RSNDSC"].ToString().Trim();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sReasonDesc;
        }
        // ========================================================================
        private string GetProblemSubtypeNumber(int center, int ticket)
        {
            string sSecondNum = "";
            int iSubtype = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " PBMSCD" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", center);
                odbcCmd.Parameters.AddWithValue("@Tck", ticket);

                using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(odbcReader);
                }

                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["PBMSCD"].ToString().Trim(), out iSubtype) == false)
                        iSubtype = -1;
                    if (iSubtype > 0)
                    {
                        string sSubtype = iSubtype.ToString("00");
                        if (!String.IsNullOrEmpty(sSubtype) && sSubtype.Length > 1)
                            sSecondNum = sSubtype.Substring(1, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sSecondNum;
        }
        // ========================================================================
        private int GetCs1(int ctr, int tck)
        {
            int iCs1 = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " STCUS1" +
                //", STCUS2" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["STCUS1"].ToString().Trim(), out iCs1) == false)
                        iCs1 = -1;
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iCs1;
        }
        // ========================================================================
        private string[] GetSvrtickData(int ctr, int tck)
        {
            string[] saCs1Cs2DatTimPriCodTyp = { "", "", "", "", "", "", "" };
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " STCUS1" +
                ", STCUS2" +
                ", TSDATE" +
                ", TSCHTM" +
                ", TPRIO" +
                ", PROCCD" +
                ", CALLCD" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    saCs1Cs2DatTimPriCodTyp[0] = dt.Rows[0]["STCUS1"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[1] = dt.Rows[0]["STCUS2"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[2] = dt.Rows[0]["TSDATE"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[3] = dt.Rows[0]["TSCHTM"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[4] = dt.Rows[0]["TPRIO"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[5] = dt.Rows[0]["PROCCD"].ToString().Trim();
                    saCs1Cs2DatTimPriCodTyp[6] = dt.Rows[0]["CALLCD"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                string sStampData = "Ctr: " + ctr +
        " -- Tck: " + tck +
        "";

                myPage.SaveError(ex.Message.ToString(), ex.ToString(), sStampData);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return saCs1Cs2DatTimPriCodTyp;
        }
        // ========================================================================
        private string GetCurrentRemark(int ctr, int tck)
        {
            string sCurrentRemark = "";
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " REMARK" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    sCurrentRemark = dt.Rows[0]["REMARK"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sCurrentRemark;
        }
        // ========================================================================
        private string GetEscalationStamp(int ctr, int tck)
        {
            string sCurrentStamp = "";
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " NSTAT" +
                " from " + sLibrary + ".NEPESCTK" +
                " where NCENT = ?" +
                " and NTCKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    sCurrentStamp = dt.Rows[0]["NSTAT"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sCurrentStamp;
        }
        // ========================================================================
        private int AddReschedAudit(
            int user,
            int ctr,
            int tck,
            int cs1,
            int cs2,
            string reasonDesc,
            int datSched,
            double timSched,
            int datStamped,
            double timStamped,
            int datNow,
            double timNow)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "insert into " + sLibrary + ".RESAUDIT" +
                     " (ACENT, ATCKNR, ACUS1, ACUS2, AORGDT, AORGTM, ANEWDT, ANEWTM, ASYSDT, ASYSTM, AATEMP, ATECFR, ATECTO, ACOMMT, APGMNM) " +
                    " values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);
                odbcCmd.Parameters.AddWithValue("@Cs1", cs1);
                odbcCmd.Parameters.AddWithValue("@Cs2", cs2);
                odbcCmd.Parameters.AddWithValue("@DatSched", datSched);
                odbcCmd.Parameters.AddWithValue("@TimSched", timSched);
                odbcCmd.Parameters.AddWithValue("@DatStamped", datStamped);
                odbcCmd.Parameters.AddWithValue("@TimStamped", timStamped);
                odbcCmd.Parameters.AddWithValue("@DatNow", datNow);
                odbcCmd.Parameters.AddWithValue("@TimNow", timNow);
                odbcCmd.Parameters.AddWithValue("@EmpAuth", user);
                odbcCmd.Parameters.AddWithValue("@EmpFrom", user);
                odbcCmd.Parameters.AddWithValue("@EmpTo", user);
                odbcCmd.Parameters.AddWithValue("@Comment", reasonDesc);
                odbcCmd.Parameters.AddWithValue("@Pgm", "ANDROID");

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int UpdateEscalation(int ctr, int tck, string currStatus, string newStatus, int datNow, double timNow, int priority)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".NEPESCTK set" +
                     " NSTAT = ?" + // ticket status
                    ", NCSTAT = ?" + // status at day's end
                    ", NTKSDT = ?" +
                    ", NTKSTM = ?" +
                    ", NLSTDT = 0" +
                    ", NLSTTM = 0" +
                    ", NPRIOR = ?" +
                    ", NSATTM = 0" +
                    ", NESC# = 0" +
                    ", NALRT# = 0" +
                    " where NCENT = ?" + // center
                    " and NTCKNR = ?";  // ticket


                odbcCmd = new OdbcCommand(sSql, odbcConn);
                if (currStatus == "X")
                {
                    odbcCmd.Parameters.AddWithValue("@Sts", "X");
                    odbcCmd.Parameters.AddWithValue("@End", newStatus);
                }
                else
                {
                    odbcCmd.Parameters.AddWithValue("@Sts", newStatus);
                    odbcCmd.Parameters.AddWithValue("@End", "");
                }
                odbcCmd.Parameters.AddWithValue("@DatNow", datNow);
                odbcCmd.Parameters.AddWithValue("@TimNow", timNow);
                odbcCmd.Parameters.AddWithValue("@Priority", priority);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
                //string sShow = newStatus + "|" + datNow + "|" + timNow.ToString("0.00") + "|" + priority + "|" + ctr + "|" + tck + "|" + iRowsAffected + "|";
                //myPage.SaveError(sSql, "newStatus|datNow|timNow|pri|ctr|tck|rows|", sShow);
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private string CheckForcedCustomer(int ctr, int tck)
        {
            string sForcedCustomer = "";
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "Select" +
                 " FRCCUS" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    sForcedCustomer = dt.Rows[0]["FRCCUS"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sForcedCustomer;
        }
        // ========================================================================
        protected int UpdateSvrtickd(int ctr, int tck, int completeDate, double completeTime, string program)
        {
            int iRowsAffected = 0;
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".SVRTICKD set" +
                     " SDCLPD = ?" +
                    ", SDCLPT = ?" +
                    ", SDCDRP = ?" +
                    " where SDCENT = ?" +
                    " and SDTNUM = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Dat", completeDate);
                odbcCmd.Parameters.AddWithValue("@Tim", completeTime);
                odbcCmd.Parameters.AddWithValue("@Pgm", program);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }

            return iRowsAffected;
        }
        // ========================================================================
        private int AddParmsToTRIGMAST_Travel(int newKey, int user, int ctr, int tck)
        {
            int iRowsAffected = 0;
            string sTest = "";
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                if (sLibrary != "OMDTALIB")
                    sTest = "Y";

                sSql = "insert into " + sLibrary + ".TRIGMAST (" +
                      "TMKEY" + // newKey
                    ", TMPGM" + // TRIGFMT (always used to format parms you are loading)
                    ", TMRUN" + // OTHRTCKS (Program to execute job) 
                    ", TMTX1" + // user
                    ", TMTX2" + // ctr
                    ", TMTX3" + // tck
                    ", TMTST" + // Y = run in test
                    ") values(?, ?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Key", newKey);
                odbcCmd.Parameters.AddWithValue("@Fmt", "TRIGFMT");
                odbcCmd.Parameters.AddWithValue("@Run", "OTHRTCKS");
                odbcCmd.Parameters.AddWithValue("@Tx1", user.ToString("00000"));
                odbcCmd.Parameters.AddWithValue("@Tx2", ctr.ToString("000"));
                odbcCmd.Parameters.AddWithValue("@Tx3", tck.ToString("0000000"));
                odbcCmd.Parameters.AddWithValue("@Tst", sTest);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Timestamp: Trigmast Travel Crash (in sql) -- NewKey: " + newKey.ToString() + "   Orig Key: " + iOriginalKey.ToString() + " User: " + iGlobalUser.ToString() + "   Try: " + iTryCount.ToString());
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        // THIS SENDS COMPLETE EMAIL TO THE CUSTOMER
        private int AddParmsToTRIGMAST_Complete(int newKey, int ctr, int tck)
        {
            int iRowsAffected = 0;
            string sTest = "";
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                if (sLibrary != "OMDTALIB")
                    sTest = "Y";

                sSql = "insert into " + sLibrary + ".TRIGMAST (" +
                      "TMKEY" + // newKey
                    ", TMPGM" + // TRIGFMT (always used to format parms you are loading)
                    ", TMRUN" + // EMAILPCL (Program to execute job) 
                    ", TMTX1" + // ctr
                    ", TMTX2" + // tck
                    ", TMTST" + // Y = run in test
                    ") values(?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Key", newKey);
                odbcCmd.Parameters.AddWithValue("@Fmt", "TRIGFMT");
                odbcCmd.Parameters.AddWithValue("@Run", "EMAILPCL");
                odbcCmd.Parameters.AddWithValue("@Tx1", ctr.ToString("000"));
                odbcCmd.Parameters.AddWithValue("@Tx2", tck.ToString("0000000"));
                odbcCmd.Parameters.AddWithValue("@Tst", sTest);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Timestamp: Trigmast complete crash -- NewKey: " + newKey.ToString() + "   Orig Key: " + iOriginalKey.ToString() + " User: " + iGlobalUser.ToString() + "   Try: " + iTryCount.ToString());
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iRowsAffected;
        }
        // ========================================================================
        private int GetCallUnit(int ctr, int tck)
        {
            int iUnit = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " tesys#" +
                " from " + sLibrary + ".TICKEQP" +
                " where TECNT# = ?" + // Center
                " and TETCK# = ?";  // Ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["TESYS#"].ToString().Trim(), out iUnit);
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return iUnit;
        }

        // ========================================================================
        private string GetCallPart(int ctr, int tck)
        {
            string sPart = "";
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " TEPRTO" +
                " from " + sLibrary + ".TICKEQP" +
                " where TECNT# = ?" + // Center
                " and TETCK# = ?";  // Ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    sPart = dt.Rows[0]["TEPRTO"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return sPart;
        }
        // ========================================================================
        private string[] GetOemPrt(int unit, int stampDate)
        {
            string[] saOemPrt = { "N", "" };
            string sEqpSubtype = "";
            string sTemp = "";
            int iDate1 = 0;
            int iDate2 = 0;
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select distinct" +
                 " cesubt" +
                ", cewdts" +
                ", cewdte" +
                ", ceprt#" +
                " from " + sLibrary + ".CUSEQUIP" +
                " where Cesys# = ?";  // unit

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Unit", unit);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    // Get Equipment Subtype: If subtype is PXMC, PXMM, PXMW, PXSW...
                    sEqpSubtype = dt.Rows[0]["cesubt"].ToString().Trim();

                    if (sEqpSubtype == "PXMC" ||
                        sEqpSubtype == "PXMM" ||
                        sEqpSubtype == "PXMW" ||
                        sEqpSubtype == "PXSW")
                    {
                        sTemp = dt.Rows[0]["cewdts"].ToString().Trim();
                        int.TryParse(sTemp, out iDate1);
                        sTemp = dt.Rows[0]["cewdte"].ToString().Trim();
                        int.TryParse(sTemp, out iDate2);

                        if (stampDate >= iDate1 && stampDate <= iDate2)
                        {
                            saOemPrt[0] = "Y";
                            saOemPrt[1] = dt.Rows[0]["CEPRT#"].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return saOemPrt;
        }

        // ========================================================================
        private string[] GetTckCustomer(int ctr, int tck)
        {

            string[] aCstDtl = { "", "", "", "", "" };
            DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
            string sSql = "";

            try
            {
                sSql = "Select " +
                 " STCUS1, STCUS2, SDCONT, SDPHN#, SDPHNE" +
                " from " + sLibrary + ".SVRTICK, " + sLibrary + ".SVRTICKD" +
                " where TCCENT = ?" + // Center
                " and TICKNR = ?" + // Ticket
                " and TCCENT = SDCENT" +
                " and TICKNR = SDTNUM";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                if (dt.Rows.Count > 0)
                {
                    aCstDtl[0] = dt.Rows[0]["Stcus1"].ToString().Trim();
                    aCstDtl[1] = dt.Rows[0]["Stcus2"].ToString().Trim();
                    aCstDtl[2] = dt.Rows[0]["SDCONT"].ToString().Trim();
                    aCstDtl[3] = dt.Rows[0]["SDPHN#"].ToString().Trim();
                    aCstDtl[4] = dt.Rows[0]["SDPHNE"].ToString().Trim();

                }
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
            return aCstDtl;
        }

        // ========================================================================
        private int UpdateTckassnForCompleteReimbursement(int ctr, int tck)
        {
            int iRowsAffected = 0;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            string sSql = "";

            try
            {
                sSql = "update " + sLibrary + ".TCKASSN set" +
                    " AEMP# = 8003" +
                    " where TACENT = ?" + // center
                    " and ATCK# = ?";  // ticket

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
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
}
