using System;
using System.Collections.Generic;
using System.Web;

using System.Configuration;
using System.Data;
//using System.Web.Security;
using System.Data.Odbc;

/// <summary>
/// Summary description for TicketHandler
/// </summary>
public class TicketHandler
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    KeyRetriever kr;

    string sLibrary = "";
    string sDevTestLive = "DEV"; // Initialize just to be safe...

    // ========================================================================
    public TicketHandler(string library)
    {
        sLibrary = library;
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        if (sLibrary == "OMDTALIB")
        {
            sDevTestLive = "LIVE";
        }
        else
        {
            sDevTestLive = "TEST";
        }

        // Debug? Hard code your preference here 
        //sDevTestLive = "DEV";
       
    }
    // ========================================================================
    public int[] AddTicket(
        int unit,
        //int cs1,
        //int cs2,
        string custCallXref,
        string problem,
        string note,
        string contact,
        string phone,
        string extension,
        string email,
        string communicationMethodType,
        string communicationMethodInfo,
        //string street,
        //string city,
        //string state,
        //string zip,
        string printerInterface,
        int shipVia,
        string requestType,
        string autoOrForced,
        string program,
        string creatorName,
        int primaryCustomerNumber, // Needed to determine if a dealer request
        string primaryCustomerType, // Needed to determine if a dealer request
        //string priority,
        int techToAssignTo,
        string userLoginEmail)
    {
        int[] iaCtrTckSrq = { 0, 0, 0 };
        int[] iaCtrTck = { 0, 0 };
        string sResult = "";

        kr = new KeyRetriever(sLibrary);
        DataTable dt;

        int iNextRequestKey = 0;
        int iNextTriggerKey = 0;
        //int iHtsNum = 0;
        int iCs1 = 0; // cs1 on EQPCONTR rec (as opposed to customer derrived from the key cross reference)
        int iCs2 = 0; // cs2 on EQPCONTR rec
        int iDealerNumber = 0;

        string sPrt = "";
        string sSer = "";
        string sAgrNum = "";
        string sAgrTyp = "";
        //string sAgrDsc = "";
        string sPM = "";
        //string sFirst = "";
        //string sLast = "";
        //string sEmail = "";
        string sCreatorName = creatorName;
        

        // Unused fields

        //string sExt = "";  // always blank for key
        string sPaymentMethod = "";

        try
        {
            odbcConn.Open();

            iNextRequestKey = kr.GetKey("SRQ3A");

            dt = GetUnitDetail(unit);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ECUSNR"].ToString().Trim(), out iCs1);
                int.TryParse(dt.Rows[0]["ECUSCD"].ToString().Trim(), out iCs2);
                sAgrNum = dt.Rows[0]["ECCNTR"].ToString().Trim();
                sPrt = dt.Rows[0]["EPART"].ToString().Trim();
                sSer = dt.Rows[0]["ESERL"].ToString().Trim();
                sAgrTyp = dt.Rows[0]["ECNTYP"].ToString().Trim();
            }

            string sAgrDsc = GetAgrDsc(sAgrNum);

            if (primaryCustomerType == "DLR")
                iDealerNumber = primaryCustomerNumber;
            else
                iDealerNumber = 0;

            //sResult = AddRequestHeader(iNextRequestKey, iCs1, iCs2, note, program, sRequestType, contact, phone, extension, email, sPaymentMethod);
            sResult = AddRequestHeader(
                iNextRequestKey, 
                iCs1, 
                iCs2, 
                note, 
                program, 
                requestType, 
                contact, 
                phone, 
                extension, 
                email, 
                sPaymentMethod, 
                iDealerNumber,
                communicationMethodType, 
                communicationMethodInfo,
                userLoginEmail);

            if (requestType == "PM")
                sPM = requestType;

            //sResult = AddRequestDetail(iNextRequestKey, 1, sPrt, sSer, unit, problem, custCallXref, sAgrNum, sAgrTyp, sAgrDsc, sPM, tech, sCreator);
            sResult = AddRequestDetail(
                iNextRequestKey, 
                1, 
                sPrt, 
                sSer, 
                unit, 
                problem, 
                custCallXref, 
                sAgrNum, 
                sAgrTyp, 
                sAgrDsc, 
                printerInterface, 
                shipVia, 
                sPM, 
                autoOrForced, 
                sCreatorName, 
                techToAssignTo);

            // ------------------------------------------------------
            // START: Trigger Section
            // ------------------------------------------------------
            iNextTriggerKey = kr.GetKey("T2TRIGGER");

            TriggerHandler triggerHandler = new TriggerHandler(sDevTestLive);

            TriggerHandler.TriggerObject triggerObject = triggerHandler.Get_EmptyTriggerObjectToLoad();

            triggerObject.key = iNextTriggerKey;
            triggerObject.programToRun = "T2SERVREQ";
            triggerObject.DEV_TEST_LIVE = sDevTestLive;
            //triggerObject.doubleToSend_01 = iNextTriggerKey;
            triggerObject.doubleToSend_01 = iNextRequestKey;
            triggerObject.doubleToSend_02 = 1; // Sequence of the request (this process only passes one at a time) 

            triggerObject = triggerHandler.Save_ObjectToTriggerFile(triggerObject);

            string sRequestSuccessOrFailure = triggerObject.stringReturned;
            int iReturnedCenter = (int)triggerObject.doubleReturned_01;
            int iReturnedTicket = (int)triggerObject.doubleReturned_02;

            triggerObject = null;
            triggerHandler = null;

            // ------------------------------------------------------
            // END: Trigger Section
            // ------------------------------------------------------

            //sResult = TriggerServiceRequest(iNextKey);
            //iaCtrTck = GetCtrTck(iNextKey);

            if (iReturnedCenter > 0 && iReturnedTicket > 0)
            {
                if (!String.IsNullOrEmpty(contact)
                    || !String.IsNullOrEmpty(phone)
                )
                {
                    int iRowsAffected = UpdateRequestContactInfo(iReturnedCenter, iReturnedTicket, contact, phone);
                }
                // Load Call and SRQA key for return
                iaCtrTckSrq[0] = iReturnedCenter;
                iaCtrTckSrq[1] = iReturnedTicket;
                iaCtrTckSrq[2] = iNextRequestKey;
            }

        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Request failure " + iNextRequestKey.ToString());
            myPage = null;
        }
        finally
        {
            odbcConn.Close();
            kr = null;
        }

        return iaCtrTckSrq;
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    // 400 Queries
    // ========================================================================
    protected string AddRequestHeader(
        int key,
        int cs1,
        int cs2,
        string comment,
        string programSubmittingRequest,
        string requestType,
        string contact,
        string phone,
        string ext,
        string email,
        string paymentMethod,
        int dealerNumber,
        string communicationMethodType,
        string communicationMethodInfo,
        string userLoginEmail
        )
    {
        string sSql = "";

        string sResult = "";
        string sPhone = "";
        string sName = "";
        string sAddress1 = "";
        string sAddress2 = "";
        string sAddress3 = "";
        string sCity = "";
        string sState = "";
        string sZip = "";
        string sPhone1 = "";
        string sPhone2 = "";
        string sPhone3 = "";
        string sDealerSubmission = "";

        DateTime datTemp = DateTime.Now;
        int iDateNow = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        int iTimeNow = Int32.Parse(datTemp.ToString("HHmmss"));

        int iPhone1 = 0;
        int iPhone2 = 0;
        int iPhone3 = 0;

        try
        {
            // GREENSCREEN SRQ3 DISREGARDS passed address, will use cs1 cs2 address regardless, so don't use on add or update!
            DataTable dt = GetCustDetail(cs1, cs2);
            if (dt.Rows.Count > 0)
            {
                sName = dt.Rows[0]["Name"].ToString().Trim();
                sAddress1 = dt.Rows[0]["Address1"].ToString().Trim();
                sAddress2 = dt.Rows[0]["Address2"].ToString().Trim();
                sAddress3 = dt.Rows[0]["Address3"].ToString().Trim();
                sCity = dt.Rows[0]["City"].ToString().Trim();
                sState = dt.Rows[0]["State"].ToString().Trim();
                sZip = dt.Rows[0]["Zip"].ToString().Trim();

                if (contact == "")
                    contact = dt.Rows[0]["Contact"].ToString().Trim();
                if (phone == "")
                    phone = dt.Rows[0]["Phone"].ToString().Trim();
            }

            if (!String.IsNullOrEmpty(userLoginEmail) && userLoginEmail.Length > 70)
            {
                    userLoginEmail = userLoginEmail.Substring(0, 70);
            }

            if (contact.Length > 30)
                contact = contact.Substring(0, 30);
            contact = scrub(contact);

            if (comment.Length > 5000) // was 1000, bumped it up to 5000 (but the web service ane message to key says 10,000)
                comment = comment.Substring(0, 5000);
            comment = scrub(comment);

            if (phone == "9999999999")
                phone = "";

            if (phone.Length == 10)
            {
                sPhone1 = phone.Substring(0, 3);
                sPhone2 = phone.Substring(3, 3);
                sPhone3 = phone.Substring(6, 4);
            }

            int.TryParse(sPhone1, out iPhone1);
            int.TryParse(sPhone2, out iPhone2);
            int.TryParse(sPhone3, out iPhone3);
            string[] saExt = { "" };
            int iTemp = 0;
            int iExtLength = 0;
            string sTemp = "";
            string sNum = "";

            if (!String.IsNullOrEmpty(ext) && ext.Length > 8) 
            {
                saExt = ext.Split(' '); // Customer Entered Ext As: '504 or 509'
                for (int i = 0; i< saExt.Length; i++) 
                {
                    if (int.TryParse(saExt[i], out iTemp) == true) // Is the value a number? Rather than 'Ext:' or 'or' or '-'?
                    {
                        sNum = iTemp.ToString("0");  // Move it to a num so you can calculate length
                        if (!String.IsNullOrEmpty(sTemp)) // not first pass, something already loaded
                        {
                            iExtLength = sTemp.Length + 1 + sNum.Length;  // What would be the length if I added the next value (> 8)?
                            if (iExtLength <= 8) 
                            {
                                sTemp += "/" + sNum;
                            }
                        }
                        else 
                        {
                            sTemp = sNum;
                        }
                    }
                    if (sTemp.Length > 8) 
                    {
                        sTemp = sTemp.Substring(0, 8);
                    }
                }
                // first try to pull valid entry
                ext = sTemp;
                if (!String.IsNullOrEmpty(ext) && ext.Length > 8)
                    ext = ext.Substring(0, 8);
            }

            if (paymentMethod == "")
            {
                if (requestType == "PM" || requestType == "COURTESY" || requestType == "QC")
                    paymentMethod = "AGR";
            }

            if (dealerNumber > 0)
                sDealerSubmission = "Y";
            else
                sDealerSubmission = "N";

            sSql = "insert into " + sLibrary + ".SRQ3A " +
                    "(AKEY" +
                ", ADATE" +
                ", ATIME" +
                ", ALINES" +
                ", ACS1" +
                ", ACS2" +
                ", ANAME" +
                ", AADD1" +
                ", AADD2" +
                ", AADD3" +
                ", ACITY" +
                ", ASTATE" +
                ", AZIP" +
                ", ACONTACT" +
                ", APHONE1" +
                ", APHONE2" +
                ", APHONE3" +
                ", APHONE" +
                ", AEXT" +
                ", AEMAIL" +
                ", APAY" +
                ", APGM" +
                ", AUSERID" +
                ", ADLR" +
                ", ADLRID" +
                ", AMTHDTYP" +
                ", AMTHDINF" +
                ", ACOMMENT)" +
                " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Key", key);
            odbcCmd.Parameters.AddWithValue("@Date", iDateNow);
            odbcCmd.Parameters.AddWithValue("@Time", iTimeNow);
            odbcCmd.Parameters.AddWithValue("@ReqCount", 1);
            odbcCmd.Parameters.AddWithValue("@Cs1", cs1);
            odbcCmd.Parameters.AddWithValue("@Cs2", cs2);
            odbcCmd.Parameters.AddWithValue("@Name", sName);
            odbcCmd.Parameters.AddWithValue("@Address1", sAddress1);
            odbcCmd.Parameters.AddWithValue("@Address2", sAddress2);
            odbcCmd.Parameters.AddWithValue("@Address3", sAddress3);
            odbcCmd.Parameters.AddWithValue("@City", sCity);
            odbcCmd.Parameters.AddWithValue("@State", sState);
            odbcCmd.Parameters.AddWithValue("@Zip", sZip);
            odbcCmd.Parameters.AddWithValue("@Contact", contact);
            odbcCmd.Parameters.AddWithValue("@Phone1", iPhone1);
            odbcCmd.Parameters.AddWithValue("@Phone2", iPhone2);
            odbcCmd.Parameters.AddWithValue("@Phone3", iPhone3);
            odbcCmd.Parameters.AddWithValue("@Phone", sPhone);
            odbcCmd.Parameters.AddWithValue("@Ext", ext);
            odbcCmd.Parameters.AddWithValue("@Email", email);
            odbcCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            odbcCmd.Parameters.AddWithValue("@CallingProgram", programSubmittingRequest);
            odbcCmd.Parameters.AddWithValue("@UserId", userLoginEmail);
            odbcCmd.Parameters.AddWithValue("@DealerSubmission", sDealerSubmission);
            odbcCmd.Parameters.AddWithValue("@DealerNum", dealerNumber);
            odbcCmd.Parameters.AddWithValue("@CommunicationMethodType", communicationMethodType);
            odbcCmd.Parameters.AddWithValue("@CommunicationMethodInfo", communicationMethodInfo);
            odbcCmd.Parameters.AddWithValue("@Comment", comment);

            int iRowsAffected = odbcCmd.ExecuteNonQuery();
            sResult = "Rows Affected: " + iRowsAffected.ToString();
        }
        catch (Exception ex)
        {
            string sErrValues = "Library: " + sLibrary +
                " --- Key: " + key.ToString() +
                " Comment: " + comment +
                " Dt: " + iDateNow.ToString() +
                " Tm: " + iTimeNow.ToString() +
                " Cs1: " + cs1.ToString() +
                " Cs2: " + cs2.ToString() +
                " Nam: " + sName +
                " Ad1: " + sAddress1 +
                " Ad2: " + sAddress2 +
                " Ad3: " + sAddress3 +
                " Cit: " + sCity +
                " St: " + sState +
                " Zip: " + sZip +
                " Con: " + contact +
                " Phn: " + sPhone +
                " Ext: " + ext +
                " Eml: " + email +
                " Pay: " + paymentMethod +
                " Pgm: " + programSubmittingRequest +
                " Usr: " + userLoginEmail;

            sResult = ex.ToString();
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -------------------------------
        return sResult;
    }
    // ===================================================================
    //protected string AddRequestDetail(int key, int seq, string model, string serial, int unit, string prob, string tckXrf, string agr, string agrCd, string agrDsc, string pm, int tech, string creator)
    protected string AddRequestDetail(int key, int seq, string model, string serial, int unit, string prob, string tckXrf, string agr, string agrCd, string agrDsc, string printerInterface, int shipVia, string pm, string autoOrForced, string creator, int tech)
    {
        string sResult = "";
        string sSql = "";

        int iDateToday = 0;
        string sDateToday = DateTime.Now.ToString("yyyyMMdd");
        if (int.TryParse(sDateToday, out iDateToday) == false)
            iDateToday = -1;

        if (!String.IsNullOrEmpty(creator) && creator.Length > 30)
            creator = creator.Substring(0, 30);

        if (!String.IsNullOrEmpty(tckXrf) && tckXrf.Length > 24)
            tckXrf = tckXrf.Substring(0, 24);

        if (!String.IsNullOrEmpty(prob) && prob.Length > 50)
            prob = prob.Substring(0, 50);

        prob = scrub(prob);

        // If in test, always assign to Steve
        //if (sLibrary == "OMTDTALIB" || iDateToday <= iWs1LastDay)
        if (sLibrary == "OMTDTALIB")
            tech = 1862;
        //tech = 0;
        // otherwise, just leave tech alone, allow it to be passed in from calling program

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sAuto = "";
        string sForced = "";
        string sAutoModel = "";
        string sForcedModel = "";
        //string sPrinterInterface = "";
        //int iVia = 0;

        if (autoOrForced.ToLower() == "forced") 
        {
            sAuto = "N";
            sForced = "Y";
            sAutoModel = "";
            sForcedModel = model;
        }
        else 
        {
            sAuto = "Y";
            sForced = "N";
            sAutoModel = model;
            sForcedModel = "";
        }

        try
        {
            sSql = "insert into " + sLibrary + ".SRQ3B " +
                    "(BKEY" +
                ", BSEQ" +
                ", BFORCED" +
                ", BAUTO" +
                ", BPART" +
                ", BPARTF" +
                ", BSER" +
                ", BUNIT" +
                ", BAGR" +
                ", BAGRCODE" +
                ", BSERVTYPE" +
                ", BPROB" +
                ", BXREF" +
                ", BXRFREQ" +
                ", BFACE" +
                ", BVIA" +
                ", BPM" +
                ", BNUM1" +
                ", BCREATOR)" +
                " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Key", key);
            odbcCmd.Parameters.AddWithValue("@Seq", seq);
            odbcCmd.Parameters.AddWithValue("@Forced", sForced);
            odbcCmd.Parameters.AddWithValue("@Auto", sAuto);
            odbcCmd.Parameters.AddWithValue("@AutoModel", sAutoModel);
            odbcCmd.Parameters.AddWithValue("@ForcedModel", sForcedModel);
            odbcCmd.Parameters.AddWithValue("@Serial", serial);
            odbcCmd.Parameters.AddWithValue("@Unit", unit);
            odbcCmd.Parameters.AddWithValue("@AgrNum", agr);
            odbcCmd.Parameters.AddWithValue("@AgrCode", agrCd);
            odbcCmd.Parameters.AddWithValue("@AgrDesc", agrDsc);
            odbcCmd.Parameters.AddWithValue("@Problem", prob);
            odbcCmd.Parameters.AddWithValue("@OldTckXrf", tckXrf);
            odbcCmd.Parameters.AddWithValue("@NewTckXrf", tckXrf);
            odbcCmd.Parameters.AddWithValue("@PrinterInterface", printerInterface);
            odbcCmd.Parameters.AddWithValue("@ShipVia", shipVia);
            odbcCmd.Parameters.AddWithValue("@Pm", pm);
            odbcCmd.Parameters.AddWithValue("@Tech", tech);
            odbcCmd.Parameters.AddWithValue("@Creator", creator);

            int iRowsAffected = odbcCmd.ExecuteNonQuery();
            sResult = "Rows Affected: " + iRowsAffected.ToString();
        }
        catch (Exception ex)
        {
            string sErrValues = "Library: " + sLibrary +
                    " ----- Key: " + key.ToString() +
                    " Seq: " + seq.ToString() +
                    " aMod: " + sAutoModel +
                    " fMod: " + sForcedModel +
                    " Ser: " + serial +
                    " Prob: " + prob +
                    " Frc: " + sForced +
                    " Aut: " + sAuto +
                    " Unt: " + unit.ToString() +
                    " Agr: " + agr +
                    " AgrCd: " + agrCd +
                    " AgrDsc: " + agrDsc +
                    " TckXrf: " + tckXrf +
                    //" Face: " + ptrFace +
                    //" Via: " + via.ToString() +
                    " PM: " + pm +
                    " Tech: " + tech.ToString() +
                    " Creator: " + creator;

            sResult = ex.ToString();
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -------------------------------
        return sResult;
    }
    // =========================================================================
    protected int UpdateRequestContactInfo(
          int ctr
        , int tck
        , string contact
        , string phone
        )
    {
        int iRowsAffected = 0;

        string sSql = "";

        // If you have a value to update
        if (!String.IsNullOrEmpty(contact)
            || !String.IsNullOrEmpty(phone)
            )
        {
            //contact = contact.ToUpper();
            contact = contact.ToUpper();
            phone = phone.Replace("-", "");

            // Ensure inbound values will fit into 400 field sizes
            if (!String.IsNullOrEmpty(contact) && contact.Length > 30)
                contact = contact.Substring(0, 30);
            if (!String.IsNullOrEmpty(phone) && phone.Length > 10)
                phone = phone.Substring(0, 10);

            try
            {
                string sUpdates = "";

                sSql = "update " + sLibrary + ".SVRTICKD set";

                if (!String.IsNullOrEmpty(contact))
                    sUpdates = " SDCONT = ?";
                if (!String.IsNullOrEmpty(phone))
                {
                    if (!String.IsNullOrEmpty(sUpdates))
                        sUpdates += ",";
                    sUpdates += " SDPHN# = ?";
                }

                sSql += sUpdates +
                " where SDCENT = ?" +
                " and SDTNUM = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                if (!String.IsNullOrEmpty(contact))
                    odbcCmd.Parameters.AddWithValue("@Contact", contact);
                if (!String.IsNullOrEmpty(phone))
                {
                    odbcCmd.Parameters.AddWithValue("@Phone", phone);
                }

                odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
                odbcCmd.Parameters.AddWithValue("@Tck", tck);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MyPage myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return iRowsAffected;
    }
    // ===================================================================
    protected DataTable GetCustDetail(int cs1, int cs2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        try
        {
            sSql = "Select" +
                     " CSTRNR as Cs1" +
                    ", CSTRCD as Cs2" +
                    ", CUSTNM as Name" +
                    ", SADDR1 as Address1" +
                    ", SADDR2 as Address2" +
                    ", SADDR3 as Address3" +
                    ", CITY as City" +
                    ", STATE as State" +
                    ", ZIPCD as Zip" +
                    ", CONTNM as Contact" +
                    ", HPHONE as Phone" +
                    ", XREFCS as CrossRef" +
                " from " + sLibrary + ".CUSTMAST" +
                " where CSTRNR = ?" +
                " and CSTRCD = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Cs1", cs1);
            odbcCmd.Parameters.AddWithValue("@Cs2", cs2);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }

        return dt;
    }
    // ========================================================================
    protected DataTable GetUnitDetail(int unit)
    {

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        try
        {
            sSql = "Select" +
                 " e.ECUSNR" +
                ", e.ECUSCD" +
                ", e.ECCNTR" +
                ", e.EPART" +
                ", e.ESERL" +
                ", e.ECNTYP" +
                ", c.CECNAM" +
                " from " + sLibrary + ".EQPCONTR e, " + sLibrary + ".CUSEQUIP c" +
                " where e.ECSYS# = c.CESYS#" +
                " and e.ECSYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unit", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    protected string GetAgrDsc(string agr)
    {
        string sAgrDsc = "";
        string sSql = "";

        DataTable dataTable = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sAgrCode = "";
        int iGeneralLedgerNumber = 0;

        try
        {
            sSql = "Select distinct" +
                 " ECNTYP" +
                ", GLNUM" +
                " from " +
                sLibrary + ".EQPCONTR e, " +
                sLibrary + ".SCHEADER s, " +
                sLibrary + ".CONTRTYP c, " +
                sLibrary + ".GLNUMBER g" +
                " where e.ECCNTR = s.CONTNR" +
                " and s.CONTYP = c.CONTYP" +
                " and c.CTDGL# = g.GLNUM" +
                " and e.ECCNTR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Agr", agr);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                sAgrCode = dataTable.Rows[0]["ECNTYP"].ToString().Trim();
                if (int.TryParse(dataTable.Rows[0]["GLNUM"].ToString().Trim(), out iGeneralLedgerNumber) == false)
                    iGeneralLedgerNumber = 0;

                if ((sAgrCode == "MS") || (sAgrCode == "MB") || (sAgrCode == "ME"))
                    sAgrDsc = "MANAGED SERVICE";
                else if (sAgrCode == "MP")
                    sAgrDsc = "MANAGED PRINT";
                else if (sAgrCode == "5D")
                    sAgrDsc = "5D-ONSITE";
                else if (sAgrCode == "WR")
                    sAgrDsc = "WARR-DEPOT";
                else if (sAgrCode == "WX")
                    sAgrDsc = "WARR-EXCHANGE";
                else if (sAgrCode == "SA")
                    sAgrDsc = "SOFTWARE";
                else if (sAgrCode == "PM")
                    sAgrDsc = "ONSITE-PM";
                else if (sAgrCode == "PI")
                    sAgrDsc = "PER-INCIDENT";
                else if (sAgrCode == "FR")
                    sAgrDsc = "FLAT-RATE";
                else if (iGeneralLedgerNumber == 2225211)
                    sAgrDsc = "ONSITE";
                else if (iGeneralLedgerNumber == 2225213)
                    sAgrDsc = "DEPOT";
                else if (iGeneralLedgerNumber == 2225215)
                    sAgrDsc = "EXPRESS";
                else if (iGeneralLedgerNumber == 2225218)
                    sAgrDsc = "SELF-SERVICE";
                else
                    sAgrDsc = "ONSITE";
            }
        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sAgrDsc;
    }
    // ===================================================================
    protected string TriggerServiceRequest(int reqKey)
    {
        string sResult = "";
        string sSql = "";
        int iRowsAffected = 0;

        string sTestFlag = "";

        if (sLibrary != "OMDTALIB")
            sTestFlag = "Y";

        string sTriggerProgram = "TRIGFMT";
        string sTargetProgram = "SRQ3CL";

        kr = new KeyRetriever(sLibrary);
        int iNextKey = kr.GetKey("TRIGMAST");
        kr = null;

        try
        {
            sSql = "insert into " + sLibrary + ".TRIGMAST " +
                 "(TMKEY" +
                ", TMPGM" +
                ", TMNM1" +
                ", TMNM2" +
                ", TMTX1" +
                ", TMTST)" +
                " VALUES (?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@TriggerKey", iNextKey);
            odbcCmd.Parameters.AddWithValue("@TriggerProgram", sTriggerProgram);
            odbcCmd.Parameters.AddWithValue("@ReqKey", reqKey);
            odbcCmd.Parameters.AddWithValue("@ReqCount", 1);
            odbcCmd.Parameters.AddWithValue("@TargetProgram", sTargetProgram);
            odbcCmd.Parameters.AddWithValue("@Test", sTestFlag);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
            sResult = "Rows Affected: " + iRowsAffected.ToString();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -------------------------------
        return sResult;
    }
    // ========================================================================
    protected int[] GetCtrTck(int iKey)
    {
        int[] iaCtrTck = { 0, 0 };
        string sSql = "";

        DataTable dataTable = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " BCTR" +
                ", BTCK" +
                " from " + sLibrary + ".SRQ3B" +
                " where BKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", iKey);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0]["BCTR"].ToString().Trim(), out iaCtrTck[0]);
                int.TryParse(dataTable.Rows[0]["BTCK"].ToString().Trim(), out iaCtrTck[1]);
            }
        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iaCtrTck;
    }
    // ========================================================================
    #endregion // mySqls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    // --------------------------------------------------------------------------
    public string scrub(string txt)
    {
        // Remember to also update any class version of scrub (noteHandler)
        string sTxt = txt;
        // “Quote” and then a ‘single’ quote from word
        sTxt = sTxt.Replace("“", "\"");
        sTxt = sTxt.Replace("”", "\"");
        sTxt = sTxt.Replace("‘", "'");
        sTxt = sTxt.Replace("’", "'");
        sTxt = sTxt.Replace("’", "'"); // La’Tisha 	D’Abreau -> LAâ€™TISHA	Dâ€™ABREAU
        sTxt = sTxt.Replace("•", "");
        sTxt = sTxt.Replace("‐", "-");
        sTxt = sTxt.Replace("–", "-");
        sTxt = sTxt.Replace("·\t", "&#8226;&nbsp;&nbsp;");
        sTxt = sTxt.Replace("o\t", "&#8226;&nbsp;&nbsp;");

        //sTxt = sTxt.Replace("<", "&lt;"); // this is pointless because it will Asp.Net security will make it crash before reaching this validation
        //sTxt = sTxt.Replace(">", "&gt;");

        sTxt = KeyboardCharactersOnly(sTxt);

        sTxt = sTxt.Trim();

        return sTxt;
    }
    // --------------------------------------------------------------------------
    public string KeyboardCharactersOnly(string stringIn)
    {
        string sRtn = "";
        string sKeyboardChararacters = "0123456789 abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";

        int iLength = 0;
        if (!String.IsNullOrEmpty(stringIn))
        {
            sRtn = stringIn.ToString().Trim();
            iLength = sRtn.Length;
            /*
            // 1) Trim to max length
            if (iLength > maxLength)
            {
                sRtn = sRtn.Substring(0, maxLength);
                iLength = maxLength;
            }
            */
            // 2) Ensure each character is a keyboard character
            string sChar = "";
            //string sDebug = "";
            int iPos = 0;
            for (int i = 0; i < iLength; i++)
            {
                if (i < sRtn.Length)
                {
                    sChar = sRtn.Substring(i, 1);
                    try
                    {
                        //if (i == 241 || i == 242 || i == 243 || i == 244)
                        //    sDebug = sChar;

                        iPos = sKeyboardChararacters.IndexOf(sChar);
                        if ((iPos < 0) || ((iPos == 0) && (sChar != "0")))
                        {
                            // Permit some characters the 400 sees, but are not visible keyboard characters
                            if (
                                sChar == "\r"
                                || sChar == "\n"
                                )
                                sChar = sRtn.Substring(i, 1);
                            else
                                sRtn = sRtn.Replace(sChar, " ");
                        }

                    }
                    catch (Exception ex)
                    {
                        string sEx = ex.ToString();
                        sRtn = sRtn.Replace(sChar, " ");
                    }
                }
            }
        }

        return sRtn;
    }
    // ========================================================================
    #endregion // misc
    // ========================================================================


    // ========================================================================
    // ========================================================================
}