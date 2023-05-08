using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_sc_SaveRequest : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    string sErrValues = "";

    int iReqKey = 0;
    string sAutoOrForced = "";
    string sPM = "";

    string sModel = "";
    string sSerial = "";
    string sServiceType = "";
    string sProblem = "";
    string sModelXref = "";
    string sPrinterInterface = "";
    int iCs1 = 0;
    int iCs2 = 0;
    int iDealer = 0;
    int iTech = 0;
    int iUnit = 0;
    int iVia = 0;
    int iSeq = 0;
    int iReqCount = 0;

    string sKeyAvailability = "";

    string sEmail = "";
    string sComment = "";
    string sAddress1 = "";
    string sAddress2 = "";
    string sAddress3 = "";
    string sCity = "";
    string sState = "";
    string sZip = "";
    string sProgram = "";
    string sForced = "";
    string sMethodI = "";
    string sMethodT = "";

    string sCustName = "";
    string sPaymentMethod = "";
    int iPhone1 = 0;
    int iPhone2 = 0;
    int iPhone3 = 0;
    string sPhone = "";
    string sExt = "";
    string sContact = "";
    string sResult = "";
    string sUserId = "";
   

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;

        if (!IsPostBack)
        {
        }
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            iReqKey = 0;
            try
            {
                if (Request.Form["key"] != null) 
                {
                    if (int.TryParse(Request.Form["key"].ToString().Trim(), out iReqKey) == false)
                        iReqKey = 0;
                }

                if (Request.Form["pgm"] != null)
                    sProgram = Request.Form["pgm"].ToString().Trim();

                // Make sure you have a key
                if (iReqKey > 0)
                {

                    // Make sure they key is not yet used (Check if they hit the refresh or back button) 
                    if (sPageLib == "L")
                        sKeyAvailability = wsLive.GetRequestKeyAvailability(sfd.GetWsKey(), iReqKey);
                    else
                        sKeyAvailability = wsTest.GetRequestKeyAvailability(sfd.GetWsKey(), iReqKey);

                    // Get New key from cleared bottom section of file from KEYMAST (150,000 available) 
                    if (sKeyAvailability != "AVAILABLE")
                    {
                        iReqKey = 0;
                        if (sPageLib == "L")
                            iReqKey = wsLive.GetNextKey("SRQ3A");
                        else
                            iReqKey = wsTest.GetNextKey("SRQ3A");
                        if (iReqKey > 0)
                            sKeyAvailability = "AVAILABLE";
                    }

                    if (sKeyAvailability == "AVAILABLE")
                    {
                        // ----------------
                        // Save Header
                        // ----------------
                        iReqCount = 0;
                        if (Request.Form["num_lines"] != null) 
                        {
                            if (int.TryParse(Request.Form["num_lines"].ToString().Trim(), out iReqCount) == false)
                                iReqCount = 0;
                        }
                        iDealer = 0;
                        if (Request.Form["dlr_id"] != null) 
                        {
                            if (int.TryParse(Request.Form["dlr_id"].ToString().Trim(), out iDealer) == false)
                                iDealer = 0;
                        }
                        iCs1 = 0;
                        if (Request.Form["cs1"] != null) 
                        {
                            if (int.TryParse(Request.Form["cs1"].ToString().Trim(), out iCs1) == false)
                                iCs1 = 0;
                        }
                        iCs2 = 0;
                        if (Request.Form["cs2"] != null) 
                        {
                            if (int.TryParse(Request.Form["cs2"].ToString().Trim(), out iCs2) == false)
                                iCs2 = 0;
                        }
                        if (Request.Form["name"] != null)
                            sCustName = Request.Form["name"].ToString().Trim();
                        if (Request.Form["pay"] != null)
                            sPaymentMethod = Request.Form["pay"].ToString().Trim();
                        iPhone1 = 0;
                        if (Request.Form["phone1"] != null)
                        {
                            if (int.TryParse(Request.Form["phone1"].ToString().Trim(), out iPhone1) == false)
                                iPhone1 = 0;
                        }
                        iPhone2 = 0;
                        if (Request.Form["phone2"] != null) 
                        {
                            if (int.TryParse(Request.Form["phone2"].ToString().Trim(), out iPhone2) == false)
                                iPhone2 = 0;
                        }
                        iPhone3 = 0;
                        if (Request.Form["phone2"] != null) 
                        {
                            if (int.TryParse(Request.Form["phone3"].ToString().Trim(), out iPhone3) == false)
                                iPhone3 = 0;
                        }
                        sPhone = string.Format("{0:000}", iPhone1) + 
                            string.Format("{0:000}", iPhone2) + 
                            string.Format("{0:0000}", iPhone3);
                        if (Request.Form["ext"] != null)
                            sExt = Request.Form["ext"].ToString().Trim();
                        if (Request.Form["contact"] != null)
                            sContact = Request.Form["contact"].ToString().Trim();
                        if (Request.Form["email"] != null)
                            sEmail = Request.Form["email"].ToString().Trim();
                        if (Request.Form["comment"] != null)
                            sComment = Request.Form["comment"].ToString().Trim();
                        if (Request.Form["add1"] != null)
                            sAddress1 = Request.Form["add1"].ToString().Trim();
                        if (Request.Form["add2"] != null)
                            sAddress2 = Request.Form["add2"].ToString().Trim();
                        if (Request.Form["add3"] != null)
                            sAddress3 = Request.Form["add3"].ToString().Trim();
                        if (Request.Form["city"] != null)
                            sCity = Request.Form["city"].ToString().Trim();
                        if (Request.Form["state"] != null)
                            sState = Request.Form["state"].ToString().Trim();
                        if (Request.Form["zip"] != null)
                            sZip = Request.Form["zip"].ToString().Trim();
                        if (Request.Form["forced"] != null)
                            sForced = Request.Form["forced"].ToString().Trim();
                        if (Request.Form["pm"] != null)
                            sPM = Request.Form["pm"].ToString().Trim();

                        sAutoOrForced = "FORCED";
                        if ((sForced == "N") || (sPaymentMethod == "TM"))
                            sAutoOrForced = "AUTO";

                        // Isabel 07/17/2020
                        if (Request.Form["MthdT"] != null)
                            sMethodT = Request.Form["MthdT"].ToString().Trim();

                        if (Request.Form["MthdI"] != null)
                            sMethodI = Request.Form["MthdI"].ToString().Trim();

                        // Detail Parms

                        if (sPageLib == "L")
                        {
                            sResult = wsTest.AddRequestHeaderMthd(sfd.GetWsKey(), iReqKey, iReqCount, iCs1, iCs2, sContact, iPhone1, iPhone2, iPhone3, sExt, sEmail, sPaymentMethod, sProgram, sUserId, iDealer, sComment, sMethodT, sMethodI);
                            //  sResult = wsLive.AddRequestHeader(sfd.GetWsKey(), iReqKey, iReqCount, iCs1, iCs2, sContact, iPhone1, iPhone2, iPhone3, sExt, sEmail, sPaymentMethod, sProgram, sUserId, iDealer, sComment);
                        }
                        else
                        {
                            sResult = wsTest.AddRequestHeaderMthd(sfd.GetWsKey(), iReqKey, iReqCount, iCs1, iCs2, sContact, iPhone1, iPhone2, iPhone3, sExt, sEmail, sPaymentMethod, sProgram, sUserId, iDealer, sComment, sMethodT, sMethodI);
                        }

                        // ----------------
                        // Save Detail 
                        // ----------------
                        // Request 1 -----------------------------------------------------------------
                        if (iReqCount >= 1)
                        {
                            iSeq = 1;
                            InitializeDetail();
                            if (Request.Form["part_1"] != null)
                                sModel = Request.Form["part_1"].ToString().Trim();
                            if (Request.Form["serial_1"] != null)
                                sSerial = Request.Form["serial_1"].ToString().Trim();
                            if (Request.Form["type_1"] != null)
                                sServiceType = Request.Form["type_1"].ToString().Trim();
                            if (Request.Form["problem_1"] != null)
                                sProblem = Request.Form["problem_1"].ToString().Trim();
                            if (Request.Form["xref_1"] != null)
                                sModelXref = Request.Form["xref_1"].ToString().Trim();
                            if (Request.Form["face_1"] != null)
                                sPrinterInterface = Request.Form["face_1"].ToString().Trim();
                            if (Request.Form["tech1"] != null)
                            {
                                if (int.TryParse(Request.Form["tech1"].ToString().Trim(), out iTech) == false)
                                    iTech = 0;
                            }
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_1"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_1"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_1"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_1"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }
                        // Request 2 -----------------------------------------------------------------
                        if (iReqCount >= 2)
                        {
                            iSeq = 2;
                            InitializeDetail();
                            if (Request.Form["part_2"] != null)
                                sModel = Request.Form["part_2"].ToString().Trim();
                            if (Request.Form["serial_2"] != null)
                                sSerial = Request.Form["serial_2"].ToString().Trim();
                            if (Request.Form["type_2"] != null)
                                sServiceType = Request.Form["type_2"].ToString().Trim();
                            if (Request.Form["problem_2"] != null)
                                sProblem = Request.Form["problem_2"].ToString().Trim();
                            if (Request.Form["xref_2"] != null)
                                sModelXref = Request.Form["xref_2"].ToString().Trim();
                            if (Request.Form["face_2"] != null)
                                sPrinterInterface = Request.Form["face_2"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_2"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_2"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_2"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_2"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }
                        // Request 3 -----------------------------------------------------------------
                        if (iReqCount >= 3)
                        {
                            iSeq = 3;
                            InitializeDetail();
                            if (Request.Form["part_3"] != null)
                                sModel = Request.Form["part_3"].ToString().Trim();
                            if (Request.Form["serial_3"] != null)
                                sSerial = Request.Form["serial_3"].ToString().Trim();
                            if (Request.Form["type_3"] != null)
                                sServiceType = Request.Form["type_3"].ToString().Trim();
                            if (Request.Form["problem_3"] != null)
                                sProblem = Request.Form["problem_3"].ToString().Trim();
                            if (Request.Form["xref_3"] != null)
                                sModelXref = Request.Form["xref_3"].ToString().Trim();
                            if (Request.Form["face_3"] != null)
                                sPrinterInterface = Request.Form["face_3"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_3"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_3"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_3"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_3"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 4 -----------------------------------------------------------------
                        if (iReqCount >= 4)
                        {
                            iSeq = 4;
                            InitializeDetail();
                            if (Request.Form["part_4"] != null)
                                sModel = Request.Form["part_4"].ToString().Trim();
                            if (Request.Form["serial_4"] != null)
                                sSerial = Request.Form["serial_4"].ToString().Trim();
                            if (Request.Form["type_4"] != null)
                                sServiceType = Request.Form["type_4"].ToString().Trim();
                            if (Request.Form["problem_4"] != null)
                                sProblem = Request.Form["problem_4"].ToString().Trim();
                            if (Request.Form["xref_4"] != null)
                                sModelXref = Request.Form["xref_4"].ToString().Trim();
                            if (Request.Form["face_4"] != null)
                                sPrinterInterface = Request.Form["face_4"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_4"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_4"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_4"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_4"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 5 -----------------------------------------------------------------
                        if (iReqCount >= 5)
                        {
                            iSeq = 5;
                            InitializeDetail();
                            if (Request.Form["part_5"] != null)
                                sModel = Request.Form["part_5"].ToString().Trim();
                            if (Request.Form["serial_5"] != null)
                                sSerial = Request.Form["serial_5"].ToString().Trim();
                            if (Request.Form["type_5"] != null)
                                sServiceType = Request.Form["type_5"].ToString().Trim();
                            if (Request.Form["problem_5"] != null)
                                sProblem = Request.Form["problem_5"].ToString().Trim();
                            if (Request.Form["xref_5"] != null)
                                sModelXref = Request.Form["xref_5"].ToString().Trim();
                            if (Request.Form["face_5"] != null)
                                sPrinterInterface = Request.Form["face_5"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_5"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_5"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_5"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_5"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 6 -----------------------------------------------------------------
                        if (iReqCount >= 6)
                        {
                            iSeq = 6;
                            InitializeDetail();
                            if (Request.Form["part_6"] != null)
                                sModel = Request.Form["part_6"].ToString().Trim();
                            if (Request.Form["serial_6"] != null)
                                sSerial = Request.Form["serial_6"].ToString().Trim();
                            if (Request.Form["type_6"] != null)
                                sServiceType = Request.Form["type_6"].ToString().Trim();
                            if (Request.Form["problem_6"] != null)
                                sProblem = Request.Form["problem_6"].ToString().Trim();
                            if (Request.Form["xref_6"] != null)
                                sModelXref = Request.Form["xref_6"].ToString().Trim();
                            if (Request.Form["face_6"] != null)
                                sPrinterInterface = Request.Form["face_6"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_6"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_6"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_6"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_6"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 7 -----------------------------------------------------------------
                        if (iReqCount >= 7)
                        {
                            iSeq = 7;
                            InitializeDetail();
                            if (Request.Form["part_7"] != null)
                                sModel = Request.Form["part_7"].ToString().Trim();
                            if (Request.Form["serial_7"] != null)
                                sSerial = Request.Form["serial_7"].ToString().Trim();
                            if (Request.Form["type_7"] != null)
                                sServiceType = Request.Form["type_7"].ToString().Trim();
                            if (Request.Form["problem_7"] != null)
                                sProblem = Request.Form["problem_7"].ToString().Trim();
                            if (Request.Form["xref_7"] != null)
                                sModelXref = Request.Form["xref_7"].ToString().Trim();
                            if (Request.Form["face_7"] != null)
                                sPrinterInterface = Request.Form["face_7"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_7"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_7"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_7"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_7"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 8 -----------------------------------------------------------------
                        if (iReqCount >= 8)
                        {
                            iSeq = 8;
                            InitializeDetail();
                            if (Request.Form["part_8"] != null)
                                sModel = Request.Form["part_8"].ToString().Trim();
                            if (Request.Form["serial_8"] != null)
                                sSerial = Request.Form["serial_8"].ToString().Trim();
                            if (Request.Form["type_8"] != null)
                                sServiceType = Request.Form["type_8"].ToString().Trim();
                            if (Request.Form["problem_8"] != null)
                                sProblem = Request.Form["problem_8"].ToString().Trim();
                            if (Request.Form["xref_8"] != null)
                                sModelXref = Request.Form["xref_8"].ToString().Trim();
                            if (Request.Form["face_8"] != null)
                                sPrinterInterface = Request.Form["face_8"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_8"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_8"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_8"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_8"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 9 -----------------------------------------------------------------
                        if (iReqCount >= 9)
                        {
                            iSeq = 9;
                            InitializeDetail();
                            if (Request.Form["part_1"] != null)
                                sModel = Request.Form["part_1"].ToString().Trim();
                            if (Request.Form["serial_1"] != null)
                                sSerial = Request.Form["serial_1"].ToString().Trim();
                            if (Request.Form["type_1"] != null)
                                sServiceType = Request.Form["type_1"].ToString().Trim();
                            if (Request.Form["problem_1"] != null)
                                sProblem = Request.Form["problem_1"].ToString().Trim();
                            if (Request.Form["xref_1"] != null)
                                sModelXref = Request.Form["xref_1"].ToString().Trim();
                            if (Request.Form["face_1"] != null)
                                sPrinterInterface = Request.Form["face_1"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_1"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_1"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_1"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_1"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }

                        // Request 10 -----------------------------------------------------------------
                        if (iReqCount >= 10)
                        {
                            iSeq = 10;
                            InitializeDetail();
                            if (Request.Form["part_10"] != null)
                                sModel = Request.Form["part_10"].ToString().Trim();
                            if (Request.Form["serial_10"] != null)
                                sSerial = Request.Form["serial_10"].ToString().Trim();
                            if (Request.Form["type_10"] != null)
                                sServiceType = Request.Form["type_10"].ToString().Trim();
                            if (Request.Form["problem_10"] != null)
                                sProblem = Request.Form["problem_10"].ToString().Trim();
                            if (Request.Form["xref_10"] != null)
                                sModelXref = Request.Form["xref_10"].ToString().Trim();
                            if (Request.Form["face_10"] != null)
                                sPrinterInterface = Request.Form["face_10"].ToString().Trim();
                            if (sForced != "Y")
                            {
                                if (Request.Form["unit_10"] != null)
                                {
                                    if (int.TryParse(Request.Form["unit_10"].ToString().Trim(), out iUnit) == false)
                                        iUnit = 0;
                                }
                                if (Request.Form["via_10"] != null)
                                {
                                    if (int.TryParse(Request.Form["via_10"].ToString().Trim(), out iVia) == false)
                                        iVia = 0;
                                }
                            }
                            sResult = SaveDetail();
                        }
                        // ---------------------------------------------------------------------------
                    }
                    else 
                    { 
                        // Key Passed, but was no longer available...
                        sErrValues = "Key no longer available ! From Program... " + sProgram;
                        if (Request.Form["key"] != null)
                            sErrValues += " Key passed... " + Request.Form["key"].ToString().Trim();
                        else
                            sErrValues += " Key was null";
                        sErrValues += " Req Count: " + iReqCount.ToString() +
                            " Pgm: " + sProgram +
                            " Cs1: " + iCs1.ToString() +
                            " Cs2: " + iCs2.ToString() +
                            " Mod: " + sModel +
                            " Ser: " + sSerial +
                            " SvcTyp: " + sServiceType +
                            " Prb: " + sProblem +
                            " ModXrf: " + sModelXref +
                            " Face: " + sPrinterInterface +
                            " Tech: " + iTech.ToString() +
                            " Unit: " + iUnit.ToString() +
                            " Via: " + iVia.ToString() +
                            " Contact: " + sContact +
                            " Ph1: " + iPhone1.ToString() +
                            " Ph2: " + iPhone2.ToString() +
                            " Ph3: " + iPhone3.ToString() +
                            " Ext: " + sExt +
                            " Email: " + sEmail +
                            " Pay: " + sPaymentMethod +
                            " UserId: " + sUserId +
                            " Dlr: " + iDealer.ToString() +
                            " Comment: " + sComment;
                        SaveError("Key no longer available", "", sErrValues);
                    }
                }
                else 
                { 
                    // No Key was passed from the old program!
                    sErrValues = "NO SRQ KEY PASSED! From Program... " + sProgram;
                    if (Request.Form["key"] != null)
                        sErrValues += " Key passed... " + Request.Form["key"].ToString().Trim();
                    else
                        sErrValues += " Key was null";
                    SaveError("No Srq key passed", "", sErrValues);
                }
            }
            catch (Exception ex)
            {
                // Any error in processing ends up here
                string sResult = ex.ToString();
                sErrValues = "Gen Failure -- Key: " + iReqKey.ToString() +
                    " Req Count: " + iReqCount.ToString() +
                    " Pgm: " + sProgram +
                    " Cs1: " + iCs1.ToString() +
                    " Cs2: " + iCs2.ToString() +
                    " Mod: " + sModel +
                    " Ser: " + sSerial +
                    " SvcTyp: " + sServiceType +
                    " Prb: " + sProblem +
                    " ModXrf: " + sModelXref +
                    " Face: " + sPrinterInterface +
                    " Tech: " + iTech.ToString() +
                    " Unit: " + iUnit.ToString() +
                    " Via: " + iVia.ToString() +
                    " Contact: " + sContact +
                    " Ph1: " + iPhone1.ToString() +
                    " Ph2: " + iPhone2.ToString() +
                    " Ph3: " + iPhone3.ToString() +
                    " Ext: " + sExt +
                    " Email: " + sEmail +
                    " Pay: " + sPaymentMethod +
                    " UserId: " + sUserId +
                    " Dlr: " + iDealer.ToString() +
                    " Comment: " + sComment;

                SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
        }
        try
        {
            string sLink = "";
            if (sProgram == "ReqCourtesy" || sProgram == "ReqPm")
            {
                sLink = "http://www.scantronssg.com/cgi-bin/emp/ReqEnd.d2w/result";
            }
            else 
            {
                sLink = "http://www.scantronssg.com/cgi-bin/srqA8_process_20.d2w/result";
            }
            sLink += "?key=" + iReqKey.ToString() +
                "&emp=" + iTech.ToString() +
                "&cs1=" + iCs1.ToString() +
                "&cs2=" + iCs2.ToString() +
                "&dlr_id=" + iDealer.ToString() +
                "&pm=" + sPM;
            Response.Redirect(sLink, false);
        }
        catch (Exception ex)
        {
            // Redirect to Result page Failed
            string sResult = ex.ToString();
            sErrValues = "Redir Fail-- Key: " + iReqKey.ToString() +
                " Req Count: " + iReqCount.ToString() +
                " Pgm: " + sProgram +
                " Cs1: " + iCs1.ToString() +
                " Cs2: " + iCs2.ToString() +
                " Mod: " + sModel +
                " Ser: " + sSerial +
                " SvcTyp: " + sServiceType +
                " Prb: " + sProblem +
                " ModXrf: " + sModelXref +
                " Face: " + sPrinterInterface +
                " Tech: " + iTech.ToString() +
                " Unit: " + iUnit.ToString() +
                " Via: " + iVia.ToString() +
                " Contact: " + sContact +
                " Ph1: " + iPhone1.ToString() +
                " Ph2: " + iPhone2.ToString() +
                " Ph3: " + iPhone3.ToString() +
                " Ext: " + sExt +
                " Email: " + sEmail +
                " Pay: " + sPaymentMethod +
                " UserId: " + sUserId +
                " Dlr: " + iDealer.ToString() +
                " Comment: " + sComment;
                
            SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
    }
    // =========================================================
    protected string SaveDetail()
    {
        string sResult = "";
        sServiceType = FormatServiceType(sServiceType);

        if (sPageLib == "L")
            sResult = wsLive.AddRequestDetail(sfd.GetWsKey(), iReqKey, iSeq, sModel, sSerial, iUnit, sProblem, sModelXref, "", "", sServiceType, sPrinterInterface, iVia, sPM, iTech, sAutoOrForced, "", "", "");
        else
            sResult = wsTest.AddRequestDetail(sfd.GetWsKey(), iReqKey, iSeq, sModel, sSerial, iUnit, sProblem, sModelXref, "", "", sServiceType, sPrinterInterface, iVia, sPM, iTech, sAutoOrForced, "", "", "");

        return sResult;
    }
    // =========================================================
    protected string FormatServiceType(string serviceType)
    {
        string sType = serviceType.ToUpper();
        if (sType == "MANAGED SE")
            sType = "MANAGED SERVICE";
        else if (sType == "MANAGED PR")
            sType = "MANAGED PRINT";
        else if (sType == "SELF SERVE")
            sType = "SELF SERVICE";
        else if (sType == "PER INCIDE")
            sType = "PER INCIDENT";
        else if (sType == "WARR-EXCHA")
            sType = "WARR-EXCHANGE";

        return sType;
    }
    // =========================================================
    protected void InitializeDetail()
    {
        sModel = "";
        sSerial = "";
        sServiceType = "";
        sProblem = "";
        sModelXref = "";
        sPrinterInterface = "";
        sModel = "";
        iTech = 0;
        iUnit = 0;
        iVia = 0;
    }
    // =========================================================
    // =========================================================
}