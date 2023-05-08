using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Data.Odbc;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ServiceRequestHandler
/// The trigger is only active running in live, the classes use the ports to determine library and can't be easily fooled. 
/// You could add and remove the trigger over the live file, perhaps add the commands here...
/// </summary>
public class ServiceRequestHandler
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    SqlConnection sqlConn;
    SqlCommand sqlCmd;
    SqlDataReader sqlReader;

    ErrorHandler eh;
    // MailHandler mh;
    string sConnectionString = "";
    string sSql = "";
    string sMethodName = "";
    string sLibrary = "";
    string sUserName = "";

    // ========================================================================
    public ServiceRequestHandler(string library, string userName)
    {
        sLibrary = library;
        sUserName = userName.ToLower();

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        // mh = new MailHandler();

    }
    // ========================================================================
    public int[] AddRequest(int unit, string requestType, string program, int tech)
    {
        int[] iaCtrTck = { 0, 0 };
        string sResult = "";

        KeyHandler kh = new KeyHandler();
        DataTable dt;

        int iNextKey = 0;
        int iCs1 = 0;
        int iCs2 = 0;
        int iHtsNum = 0;

        string sPrt = "";
        string sSer = "";
        string sAgrNum = "";
        string sAgrTyp = "";
        string sAgrDsc = "";
        string sPrb = "";
        string sCom = "";
        string sXrf = "";
        string sPM = "";
        string sFirst = "";
        string sLast = "";
        string sEmail = "";
        string sCreator = "";

        try
        {
            odbcConn.Open();
            sqlConn.Open();

            iNextKey = kh.MakeNewKey("SRQ3A");

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

            sAgrDsc = GetAgrDsc(sAgrNum);
            if (sUserName != "")
            {
                dt = GetUserDetail(sUserName);
                if (dt.Rows.Count > 0)
                {
                    sFirst = dt.Rows[0]["FirstName"].ToString().Trim();
                    sLast = dt.Rows[0]["LastName"].ToString().Trim();
                    sEmail = dt.Rows[0]["Email"].ToString().Trim();
                    int.TryParse(dt.Rows[0]["Email"].ToString().Trim(), out iHtsNum);
                    sCreator = sFirst + " " + sLast;
                }
            }

            switch (requestType)
            {
                case "PM":
                    {
                        sPrb = "TECH PM";
                        sPM = "PM";
                        break;
                    }
                case "COURTESY":
                    {
                        sPrb = "COURTESY CALL";
                        sPM = "CC";
                        break;
                    }
                case "ARCA":
                    {
                        sPrb = "ARCA NOTE COUNT MA";
                        sPM = "ARCA";
                        break;
                    }
            }

            sResult = AddRequestHeader(iNextKey, iCs1, iCs2, sCom, program, requestType, "", "", "", "", "");
            sResult = AddRequestDetail(iNextKey, 1, sPrt, sSer, unit, sPrb, sXrf, sAgrNum, sAgrTyp, sAgrDsc, sPM, tech, sCreator);
            sResult = TriggerServiceRequest(iNextKey);
            iaCtrTck = GetCtrTck(iNextKey);
        }
        catch (Exception ex)
        {
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
        }
        finally
        {
            odbcConn.Close();
            sqlConn.Close();
        }

        return iaCtrTck;
    }
    // ========================================================================
    public int[] AddQuickRequest(int unit, string requestType, string program, int user, string problem)
    {
        int[] iaCtrTck = { 0, 0 };
        string sResult = "";

        KeyHandler kh = new KeyHandler();
        DataTable dt;

        int iNextKey = 0;
        int iCs1 = 0;
        int iCs2 = 0;
        int iHtsNum = 0;

        string sPrt = "";
        string sSer = "";
        string sAgrNum = "";
        string sAgrTyp = "";
        string sAgrDsc = "";
        string sCom = "";
        string sXrf = "";
        string sPM = requestType; // PM now used for PM CC and QC
        string sFirst = "";
        string sLast = "";
        string sEmail = "";
        string sCreator = "";

        try
        {
            odbcConn.Open();
            sqlConn.Open();

            iNextKey = kh.MakeNewKey("SRQ3A");

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

            sAgrDsc = GetAgrDsc(sAgrNum);
            if (sUserName != "")
            {
                dt = GetUserDetail(sUserName);
                if (dt.Rows.Count > 0)
                {
                    sFirst = dt.Rows[0]["FirstName"].ToString().Trim();
                    sLast = dt.Rows[0]["LastName"].ToString().Trim();
                    sEmail = dt.Rows[0]["Email"].ToString().Trim();
                    int.TryParse(dt.Rows[0]["Email"].ToString().Trim(), out iHtsNum);
                    sCreator = sFirst + " " + sLast;
                }
            }

            sResult = AddRequestHeader(iNextKey, iCs1, iCs2, sCom, program, requestType, "", "", "", "", "");
            sResult = AddRequestDetail(iNextKey, 1, sPrt, sSer, unit, problem, sXrf, sAgrNum, sAgrTyp, sAgrDsc, sPM, user, sCreator);
            sResult = TriggerServiceRequest(iNextKey);
            iaCtrTck = GetCtrTck(iNextKey);
        }
        catch (Exception ex)
        {
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
        }
        finally
        {
            odbcConn.Close();
            sqlConn.Close();
        }

        return iaCtrTck;
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    // 400 Queries
    // ========================================================================
    protected string AddRequestHeader(int key, int cs1, int cs2, string comment, string program, string requestType, string contact, string phone, string ext, string email, string paymentMethod)
    {
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

        DateTime datTemp = DateTime.Now;
        int iDateNow = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        int iTimeNow = Int32.Parse(datTemp.ToString("HHmmss"));

        int iPhone1 = 0;
        int iPhone2 = 0;
        int iPhone3 = 0;

        try
        {
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

            if ((sUserName != null) && (sUserName != ""))
            {
                if (sUserName.Length > 25)
                    sUserName = sUserName.Substring(0, 25);
            }

            if (comment.Length > 1000)
                comment = comment.Substring(0, 1000);

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

            if (paymentMethod == "")
            {
                if (requestType == "PM" || requestType == "COURTESY" || requestType == "QC")
                    paymentMethod = "AGR";
            }

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
                ", ACOMMENT)" +
                " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

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
            odbcCmd.Parameters.AddWithValue("@CallingProgram", program);
            odbcCmd.Parameters.AddWithValue("@UserId", sUserName);
            odbcCmd.Parameters.AddWithValue("@DealerSubmission", "N");
            odbcCmd.Parameters.AddWithValue("@DealerNum", 0);
            odbcCmd.Parameters.AddWithValue("@Comment", comment);

            int iRowsAffected = odbcCmd.ExecuteNonQuery();
            sResult = "Rows Affected: " + iRowsAffected.ToString();
        }
        catch (Exception ex)
        {
            string sErrValues = "Key: " + key.ToString() +
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
                " Pgm: " + program +
                " Usr: " + sUserName;

            sResult = ex.ToString();
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            eh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -------------------------------
        return sResult;
    }
    // ===================================================================
    protected string AddRequestDetail(int key, int seq, string model, string serial, int unit, string prob, string tckXrf, string agr, string agrCd, string agrDsc, string pm, int tech, string creator)
    {
        string sResult = "";

        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        string sAuto = "Y";
        string sForced = "N";
        string sAutoModel = model;
        string sForcedModel = "";
        string sPrinterInterface = "";
        int iVia = 0;

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
                ", BFACE" +
                ", BVIA" +
                ", BPM" +
                ", BNUM1" +
                ", BCREATOR)" +
                " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

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
            odbcCmd.Parameters.AddWithValue("@TicketXrf", tckXrf);
            odbcCmd.Parameters.AddWithValue("@PrinterInterface", sPrinterInterface);
            odbcCmd.Parameters.AddWithValue("@ShipVia", iVia);
            odbcCmd.Parameters.AddWithValue("@Pm", pm);
            odbcCmd.Parameters.AddWithValue("@Tech", tech);
            odbcCmd.Parameters.AddWithValue("@Creator", creator);

            int iRowsAffected = odbcCmd.ExecuteNonQuery();
            sResult = "Rows Affected: " + iRowsAffected.ToString();
        }
        catch (Exception ex)
        {
            string sErrValues = "Key: " + key.ToString() +
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
                    " ModXrf: " + tckXrf +
                //" Face: " + ptrFace +
                //" Via: " + via.ToString() +
                    " PM: " + pm +
                    " Tech: " + tech.ToString() +
                    " Creator: " + creator;

            sResult = ex.ToString();
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            eh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -------------------------------
        return sResult;
    }
    // ===================================================================
    protected DataTable GetCustDetail(int cs1, int cs2)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

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
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
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

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                 " ECUSNR" +
                ", ECUSCD" +
                ", ECCNTR" +
                ", EPART" +
                ", ESERL" +
                ", ECNTYP" +
                " from " + sLibrary + ".EQPCONTR" +
                " where ECSYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unit", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
        }
        catch (Exception ex)
        {
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
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

        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

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
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
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

        int iRowsAffected = 0;

        string sTestFlag = "";

        if (sLibrary != "OMDTALIB")
            sTestFlag = "Y";

        string sTriggerProgram = "TRIGFMT";
        string sTargetProgram = "SRQ3CL";

        KeyHandler kh = new KeyHandler();
        int iNextKey = kh.MakeNewKey("TRIGMAST");

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
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
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

        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

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
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iaCtrTck;
    }
    // ========================================================================
    // MS SQL Queries
    // ========================================================================
    protected DataTable GetUserDetail(string userName)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " FirstName" +
                ", LastName" +
                ", HtsNum" +
                ", Email" +
                ", StockLoc" +
                ", Department" +
                ", Center" +
                " from aspnet_Users" +
                " where LoweredUserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName", userName);

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(sqlReader);
        }
        catch (Exception ex)
        {
            eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    #endregion
    // ========================================================================
    // ========================================================================
    // ========================================================================
}