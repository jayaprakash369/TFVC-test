﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Web.Security;

public partial class public_scmobile_web_CallDetail : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
 //string sLibrary = "OMDTALIB";
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;
    DateFormatter df;
    ErrorHandler erh;
    string sConnectionString = "";
    string sSql = "";

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { erh = new ErrorHandler(); df = new DateFormatter(); this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { erh = null; df = null; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack)
        {
            int iCtr = 0;
            if (Request.QueryString["ctr"] != null && Request.QueryString["ctr"].ToString() != "") { if (int.TryParse(Request.QueryString["ctr"].ToString().Trim(), out iCtr) == false) iCtr = 0; else hfCtr.Value = iCtr.ToString(); }
            int iTck = 0;
            if (Request.QueryString["tck"] != null && Request.QueryString["tck"].ToString() != "") { if (int.TryParse(Request.QueryString["tck"].ToString().Trim(), out iTck) == false) iTck = 0; else hfTck.Value = iTck.ToString(); }

            //hfCtr.Value = "101";
            //hfTck.Value = "33617";

            if (iCtr.ToString().Length > 3)
                iCtr = 0;
            if (iTck.ToString().Length > 7)
                iTck = 0;

            if (iCtr == 0 || iTck == 0) 
            {
                lbPageTitle.Text = "Call Detail: Invalid Call Number";
            }
            else 
            {
                lbPageTitle.Text = "Call Detail: " + iCtr + "-" + iTck;

                string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
                DataTable dt = new DataTable(sMethodName);
                DataTable dt2 = new DataTable(sMethodName);

                try
                {
                    odbcConn.Open();

                    GetCustDetail();
                    int iUnit = GetEquipment();
                    if (iUnit == 0)
                        GetForcedEquipment();
                    else
                        GetDealerName(iUnit);
                    GetEscalationTimes();
                    // ---------------------------
                    dt = GetTimestamps();
                    if (dt.Rows.Count > 0)
                    {
                        pnTimestamps.Visible = true;
                        rpTimestamps.DataSource = dt;
                        rpTimestamps.DataBind();
                    }
                    // ---------------------------
                    GetTicketLabor();
                    // ---------------------------
                    dt = GetMiscRevenue();
                    if (dt.Rows.Count > 0)
                    {
                        pnMiscRevenue.Visible = true;
                        rpMiscRevenue.DataSource = dt;
                        rpMiscRevenue.DataBind();
                    }
                    // ---------------------------
                    dt = GetPartsUsed();
                    if (dt.Rows.Count > 0)
                    {
                        pnPartsUsed.Visible = true;
                        rpPartsUsed.DataSource = dt;
                        rpPartsUsed.DataBind();
                    }
                    // ---------------------------
                    dt = GetOnsiteLabor();
                    if (dt.Rows.Count > 0)
                    {
                        pnOnsiteLabor.Visible = true;
                        rpOnsiteLabor.DataSource = dt;
                        rpOnsiteLabor.DataBind();
                    }
                    // ---------------------------
                    // If both have data, merge them together
                    dt = GetTravelLaborOnsite();
                    if (dt.Rows.Count > 0)
                    {

                        dt2 = GetTravelLabor();
                        if (dt2.Rows.Count > 0)
                            dt.Merge(dt2);
                    }
                    else
                    {
                        dt = GetTravelLabor();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        pnTravelLabor.Visible = true;
                        rpTravelLabor.DataSource = dt;
                        rpTravelLabor.DataBind();
                    }
                    // ---------------------------
                    // ---------------------------
                    dt = GetNotes();
                    if (dt.Rows.Count > 0)
                    {
                        pnNotes.Visible = true;
                        rpNotes.DataSource = dt;
                        rpNotes.DataBind();
                    }
                    // ---------------------------
                    loadPhotos();
                    // ---------------------------

                }
                catch (Exception ex)
                {
                    erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
                }
                finally
                {
                    odbcConn.Close();
                }
            }
        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected void GetCustDetail()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            string sDat = "";
            DateTime datTemp = new DateTime();

            sSql = "Select" +
                " TCCENT" +
                ", TICKNR" +
                ", STCUS1" +
                ", STCUS2" +
                ", TEDATE" +
                ", TCKTIM" +
                ", SDCLPD" +
                ", SDCLPT" +
                ", TCDATE" +
                ", ENDTIM" +
                ", STPRCH" +
                ", TCOMM1" +
                ", TCOMM2" +
                ", CALLCD" +
                ", CONTNR" +
                ", TRESP" +
                ", SDADR1" +
                ", SDADR2" +
                ", SDADR3" +
                ", SDCITY" +
                ", SDSTAT" +
                ", SDZIPC" +
                ", SDCSTN" +
                ", SDCONT" +
                ", SDPHN#" +
                ", SDPHNE" +
                ", TSDATE" +
                ", TSCHTM" +
                " from " + sLibrary + ".SVRTICK s, " + sLibrary + ".SVRTICKD d " +
                " where TCCENT = SDCENT" +
                " and TICKNR = SDTNUM" +
                " and TCCENT = ?" +
                " and TICKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                int iCs1 = 0;
                int iCs2 = 0;
                int.TryParse(dt.Rows[0]["STCUS1"].ToString().Trim(), out iCs1);
                int.TryParse(dt.Rows[0]["STCUS2"].ToString().Trim(), out iCs2);
                lbCs1Cs2.Text = iCs1 + "-" + iCs2;
                GetResponseTime(iCs1, iCs2);
                lbCustName.Text = dt.Rows[0]["SDCSTN"].ToString().Trim();
                lbAddress1.Text = dt.Rows[0]["SDADR1"].ToString().Trim();
                lbAddress2.Text = dt.Rows[0]["SDADR2"].ToString().Trim();
                lbCityStateZip.Text = dt.Rows[0]["SDCITY"].ToString().Trim() + ", " + dt.Rows[0]["SDSTAT"].ToString().Trim() + " " + dt.Rows[0]["SDZIPC"].ToString().Trim();
                lbContact.Text = dt.Rows[0]["SDCONT"].ToString().Trim();
                sTemp = dt.Rows[0]["SDPHN#"].ToString().Trim();
                if (sTemp.Length == 10 && sTemp != "9999999999")
                {
                    try
                    {
                        sTemp = "(" + sTemp.Substring(0, 3) + ") " + sTemp.Substring(3, 3) + "-" + sTemp.Substring(6, 4);
                    }
                    catch (Exception ex) { string sError = ex.ToString(); }
                    lbPhoneExt.Text = sTemp;
                }
                sTemp = dt.Rows[0]["SDPHNE"].ToString().Trim();
                if (sTemp != "")
                    lbPhoneExt.Text += " Ext: " + sTemp;
                lbTckXrf.Text = dt.Rows[0]["STPRCH"].ToString().Trim();
                lbRequiredResponseTime.Text = dt.Rows[0]["TRESP"].ToString().Trim();
                int iAgr = 0;
                int.TryParse(dt.Rows[0]["CONTNR"].ToString().Trim(), out iAgr);
                lbAgr.Text = iAgr.ToString();
                lbAgrType.Text = GetAgrType(dt.Rows[0]["CONTNR"].ToString().Trim());

                sDat = dt.Rows[0]["TEDATE"].ToString().Trim();
                if (sDat.Length == 8)
                {
                    try
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        lbEntryDate.Text = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                    }
                    catch (Exception ex) { string sError = ex.ToString(); }
                }

                sDat = dt.Rows[0]["SDCLPD"].ToString().Trim();
                if (sDat.Length == 8)
                {
                    try
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        lbCompleteDate.Text = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                    }
                    catch (Exception ex) { string sError = ex.ToString(); }
                }

                sDat = dt.Rows[0]["TCDATE"].ToString().Trim();
                if (sDat.Length == 8)
                {
                    try
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        lbCloseDate.Text = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                    }
                    catch (Exception ex) { string sError = ex.ToString(); }
                }

                lbEntryTime.Text = dt.Rows[0]["TCKTIM"].ToString().Trim().Replace(".", ":");
                lbCompleteTime.Text = dt.Rows[0]["SDCLPT"].ToString().Trim().Replace(".", ":");
                lbCloseTime.Text = dt.Rows[0]["ENDTIM"].ToString().Trim().Replace(".", ":");

                if (lbEntryTime.Text.Trim() == "0")
                    lbEntryTime.Text = "";
                if (lbCompleteTime.Text.Trim() == "0")
                    lbCompleteTime.Text = "";
                if (lbCloseTime.Text.Trim() == "0")
                    lbCloseTime.Text = "";

                sTemp = dt.Rows[0]["CALLCD"].ToString().Trim();
                if (sTemp == "R")
                    lbCallType.Text = "Agreement";
                else if (sTemp == "$")
                    lbCallType.Text = "T&M";
                else if (sTemp == "S")
                    lbCallType.Text = "SALE";
                else
                    lbCallType.Text = sTemp;

                lbComment.Text = dt.Rows[0]["TCOMM1"].ToString().Trim() + " " + dt.Rows[0]["TCOMM2"].ToString().Trim();

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
    protected string GetAgrType(string agr)
    {
        string sAgrType = "";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " CONDSC" +
                " from " + sLibrary + ".ISERVREQL1" +
                " where CONTNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Agr", agr);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sAgrType = dt.Rows[0]["CONDSC"].ToString().Trim();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sAgrType;
    }
    // ========================================================================
    protected int GetEquipment()
    {
        int iUnt = 0;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                " TESYS#" +
                ", TEPRTO" +
                ", TEDSER" +
                ", TERSER" +
                ", IMFDSC" +
                " from " + sLibrary + ".TICKEQP, " + sLibrary + ".PRODMST" +
                " where TEPRTO = PARTNR" +
                " and TECNT# = ?" +
                " and TETCK# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TESYS#"].ToString().Trim(), out iUnt);
                lbUnit.Text = iUnt.ToString();
                lbAgentId.Text = GetAgentId(iUnt);
                lbPart.Text = dt.Rows[0]["TEPRTO"].ToString().Trim();
                lbDescription.Text = dt.Rows[0]["IMFDSC"].ToString().Trim();
                sTemp = dt.Rows[0]["TERSER"].ToString().Trim();
                if (sTemp != "")
                    lbSerial.Text = sTemp;
                else
                    lbSerial.Text = dt.Rows[0]["TEDSER"].ToString().Trim();
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
        return iUnt;
    }
    // ========================================================================
    protected string GetAgentId(int unit)
    {
        string sAgentId = "";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " CELOCD" +
                " from " + sLibrary + ".CUSEQUIP" +
                " where CESYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unt", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sAgentId = dt.Rows[0]["CELOCD"].ToString().Trim();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sAgentId;
    }
    // ========================================================================
    protected void GetForcedEquipment()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " FEMOD#" +
                ", FESER#" +
                ", FELOCD" +
                " from " + sLibrary + ".FORCEEQP" +
                " where FECENT = ?" +
                " and FETCK# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                lbPart.Text = dt.Rows[0]["FEMOD#"].ToString().Trim();
                lbSerial.Text = dt.Rows[0]["FESER#"].ToString().Trim();
                lbEquipLoc.Text = dt.Rows[0]["FELOCD"].ToString().Trim();
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
    protected void GetEscalationTimes()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " NOTTME" +
                ", ONSTME" +
                ", DURTME" +
                " from " + sLibrary + ".ESCLEVEL" +
                " where ESCLC# = ?" +
                " and ESCLT# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                lbNotifyTime.Text = dt.Rows[0]["NOTTME"].ToString().Trim().Replace(".", ":");
                if (lbNotifyTime.Text == "0")
                    lbNotifyTime.Text = "";
                lbOnsiteTime.Text = dt.Rows[0]["ONSTME"].ToString().Trim().Replace(".", ":");
                if (lbOnsiteTime.Text == "0")
                    lbOnsiteTime.Text = "";
                lbDurationTime.Text = dt.Rows[0]["DURTME"].ToString().Trim().Replace(".", ":");
                if (lbDurationTime.Text == "0")
                    lbDurationTime.Text = "";

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
    protected void GetResponseTime(int cs1, int cs2)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " CRMONS" +
                " from " + sLibrary + ".CUSTCRM" +
                " where CRMCST = ?" +
                " and CRMLOC = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Cs1", cs1);
            odbcCmd.Parameters.AddWithValue("@Cs2", cs2);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                lbRequiredResponseTime.Text = dt.Rows[0]["CRMONS"].ToString().Trim().Replace(".", ":");
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
    protected void GetDealerName(int unit)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                 " DMNAME" +
                " from " + sLibrary + ".EQPCONTR, " + sLibrary + ".DLRCL1, " + sLibrary + ".DLRML1" +
                " where DCCCONT = ECCNTR" +
                " and DCDLR# = DMDLR#" +
                " and ECSYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unit", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                lbDealer.Text = dt.Rows[0]["DMNAME"].ToString().Trim();
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
    protected DataTable GetTimestamps()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                " TIMTCH" +
                ", TIMSTS" +
                ", TIMDST" +
                ", TIMTST" +
                ", TIMRSC" +
                ", STSCDL" +
                ", STSCDD" +
                ", RSNINT" +
                ", EMPDEP" +
                " from " + sLibrary + ".TIMESTMP t, " + sLibrary + ".STSCDEPF s, " + sLibrary + ".RSNCODPF r, " + sLibrary + ".EMPMST e" +
                " where TIMSTS = STSCDE" +
                " and TIMRSC = RSNCD" +
                " and TIMTCH = EMPNUM" +
                " and TIMDST <> 0" +
                " and TIMCTR = ?" +
                " and TIMTCK = ?" +
                " order by TIMDST, TIMTST";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayStamp"));
            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayTime"));

            string sStamp = "";
            string sDat = "";
            string sTim = "";
            double dTim = 0.0;
            int iDep = 0;
            DateTime datTemp = new DateTime();

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                //if (int.TryParse(dt.Rows[iRowIdx]["TIMTCH"].ToString().Trim(), out iDep) == false)
                //    iDep = 0;

                sStamp = dt.Rows[iRowIdx]["TIMSTS"].ToString().Trim();
                sDat = dt.Rows[iRowIdx]["TIMDST"].ToString().Trim();
                sTim = dt.Rows[iRowIdx]["TIMTST"].ToString().Trim();

                iDep = 0;
                if (int.TryParse(dt.Rows[iRowIdx]["EMPDEP"].ToString().Trim(), out iDep) == false)
                    iDep = 0;

                if (iCtr <= 400 || iCtr == 420 || iCtr > 900 ||
                    ((iDep >= 401 && iDep <= 408) || iDep == 851)
                )
                {
                    if (sStamp != "H")
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["STSCDL"].ToString().Trim();
                    else
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["RSNINT"].ToString().Trim();
                }
                else
                {
                    if (sStamp != "H")
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["STSCDD"].ToString().Trim();
                    else
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["RSNINT"].ToString().Trim();
                }
                if (sDat.Length == 8)
                {
                    try
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        dt.Rows[iRowIdx]["DisplayDate"] = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                    }
                    catch (Exception ex)
                    {
                        string sError = ex.ToString(); // usually a bad date i.e February 31st.
                    }
                }
                if (double.TryParse(dt.Rows[iRowIdx]["TIMTST"].ToString().Trim(), out dTim) == true)
                    dt.Rows[iRowIdx]["DisplayTime"] = df.FormatTime(dTim, ": pm");
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
        return dt;
    }
    // ========================================================================
    protected DataTable GetMiscRevenue()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                " MRDESC" +
                ", MRREVS" +
                ", MRREVC" +
                ", EMPNR" +
                ", TMRCOV" +
                ", MRCOMM" +
                " from " + sLibrary + ".TCKMSREV" +
                " where TMCENT = ?" +
                " and TICKNO = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    protected DataTable GetPartsUsed()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                " TPRT# as Part" +
                ", TPDESC as Description" +
                ", TPQTY as Qty" +
                ", TPSER# as Serial" +
                ", TPLOC as Location" +
                ", TPDATE as DateUsed" +
                " from " + sLibrary + ".TICKPART" +
                " where TPCENT = ?" +
                " and TCKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));

            int iDat = 0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["DateUsed"].ToString(), out iDat);
                if (iDat > 20000101 && iDat < 20990101)
                    dt.Rows[iRowIdx]["DisplayDate"] = df.FormatDate(iDat, "Mon dd, YYYY");

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
        return dt;
    }
    // ========================================================================
    protected DataTable GetOnsiteLabor()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " TOLDTS as StartDate" +
                ", TOLLBS as StartTime" +
                ", TOLLBE as EndTime" +
                ", TOLHRS as Duration" +
                ", EMPNR as Tech" +
                ", EMPNAM as Name" +
                " from " + sLibrary + ".TCKOSLBR, " + sLibrary + ".EMPMST" +
                " where EMPNR = EMPNUM" +
                " and TOWRKC <> 'TR'" +
                " and TOCENT = ?" +
                " and TICKNO = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));

            int iTemp = 0;
            double dTemp = 0.0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iTemp);
                if (iTemp > 20000101 && iTemp < 20990101)
                    dt.Rows[iRowIdx]["DisplayDate"] = df.FormatDate(iTemp, "Mon dd, YYYY");
                if (double.TryParse(dt.Rows[iRowIdx]["StartTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayStart"] = df.FormatTime(dTemp, ": pm");
                if (double.TryParse(dt.Rows[iRowIdx]["EndTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayEnd"] = df.FormatTime(dTemp, ": pm");

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
        return dt;
    }
    // ========================================================================
    protected DataTable GetTravelLaborOnsite()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " TOLDTS as StartDate" +
                ", TOLLBS as StartTime" +
                ", TOLLBE as EndTime" +
                ", TOLHRS as Duration" +
                ", EMPNR as Tech" +
                ", EMPNAM as Name" +
                " from " + sLibrary + ".TCKOSLBR, " + sLibrary + ".EMPMST" +
                " where EMPNR = EMPNUM" +
                " and TOWRKC = 'TR'" +
                " and TOCENT = ?" +
                " and TICKNO = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));

            int iTemp = 0;
            double dTemp = 0.0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iTemp);
                if (iTemp > 20000101 && iTemp < 20990101)
                    dt.Rows[iRowIdx]["DisplayDate"] = df.FormatDate(iTemp, "Mon dd, YYYY");
                if (double.TryParse(dt.Rows[iRowIdx]["StartTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayStart"] = df.FormatTime(dTemp, ": pm");
                if (double.TryParse(dt.Rows[iRowIdx]["EndTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayEnd"] = df.FormatTime(dTemp, ": pm");

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
        return dt;
    }
    // ========================================================================
    protected DataTable GetTravelLabor()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                 " TRVDTS as StartDate" +
                ", TRLABS as StartTime" +
                ", TRLABE as EndTime" +
                ", TRLABR as Duration" +
                ", EMPNR as Tech" +
                ", EMPNAM as Name" +
                " from " + sLibrary + ".TCKTVLBR, " + sLibrary + ".EMPMST" +
                " where EMPNR = EMPNUM" +
                " and TRCENT = ?" +
                " and TICKNO = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));

            int iTemp = 0;
            double dTemp = 0.0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iTemp);
                if (iTemp > 20000101 && iTemp < 20990101)
                    dt.Rows[iRowIdx]["DisplayDate"] = df.FormatDate(iTemp, "Mon dd, YYYY");
                if (double.TryParse(dt.Rows[iRowIdx]["StartTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayStart"] = df.FormatTime(dTemp, ": pm");
                if (double.TryParse(dt.Rows[iRowIdx]["EndTime"].ToString(), out dTemp) == true)
                    dt.Rows[iRowIdx]["DisplayEnd"] = df.FormatTime(dTemp, ": pm");

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
        return dt;
    }
    // ========================================================================
    protected string Select_EmpName(int emp)
    {
        string sName = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Emp", emp);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            if (dt.Rows.Count > 0)
                sName = dt.Rows[0]["EMPNAM"].ToString().Trim();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sName;
    }
    // ========================================================================
    protected DataTable GetNotes()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);
        DataTable dt2 = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);
            string sCtrTck = iCtr.ToString("000") + iTck.ToString("0000000");

            sSql = "Select" +
                 " NOTDTA" +
                ", NOTDAT" +
                ", NOTEMP" +
                ", NOTNBR" +
                ", NOTSEQ" +
                " from " + sLibrary + ".NOTESPF" +
                " where NOTTYP = 'T'" +
                " and NOTFMT <> 'TNT1'" +
                " and NOTTID = ?" +
                " order by NOTNBR, NOTSEQ desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@CtrTck", sCtrTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt2.Columns.Add(MakeColumn("DisplaySubject"));
            dt2.Columns.Add(MakeColumn("DisplayMessage"));
            dt2.Columns.Add(MakeColumn("DisplayEmp"));
            dt2.Columns.Add(MakeColumn("DisplayName"));
            dt2.Columns.Add(MakeColumn("DisplayDate"));

            int iTemp = 0;
            int iSeq = 0;
            string sMessage = "";
            DataRow dr;

            int iRowIdx = 0;
            int iRowIdx2 = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(dt.Rows[iRowIdx]["NOTSEQ"].ToString(), out iSeq) == false)
                    iSeq = -1;
                if (iSeq > -1)
                {
                    if (iSeq == 0)
                    {

                        dr = dt2.NewRow();
                        dt2.Rows.Add(dr);

                        dt2.Rows[iRowIdx2]["DisplaySubject"] = dt.Rows[iRowIdx]["NOTDTA"].ToString().Trim();
                        dt2.Rows[iRowIdx2]["DisplayMessage"] = sMessage;

                        int.TryParse(dt.Rows[iRowIdx]["NOTDAT"].ToString(), out iTemp);
                        if (iTemp > 20000101 && iTemp < 20990101)
                            dt2.Rows[iRowIdx2]["DisplayDate"] = df.FormatDate(iTemp, "Mon dd, YYYY");

                        dt2.Rows[iRowIdx2]["DisplayEmp"] = dt.Rows[iRowIdx]["NOTEMP"].ToString();
                        int.TryParse(dt.Rows[iRowIdx]["NOTEMP"].ToString(), out iTemp);
                        if (iTemp > 0)
                            dt2.Rows[iRowIdx2]["DisplayName"] = GetEmpName(iTemp);

                        sMessage = "";
                        iRowIdx2++;
                    }
                    else
                    {
                        sMessage = dt.Rows[iRowIdx]["NOTDTA"].ToString().Trim() + " " + sMessage;
                    }
                }
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
        return dt2;
    }
    // ========================================================================
    protected void GetTicketLabor()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        int iCtr = 0;
        int iTck = 0;
        int.TryParse(hfCtr.Value, out iCtr);
        int.TryParse(hfTck.Value, out iTck);

        try
        {
            sSql = "Select" +
                 " TRPCSN" +
                ", TRPCHG" +
                ", MAT$C" +
                ", MATSAL" +
                ", SCMATS" +
                ", LAB$C" +
                ", LABSAL" +
                ", SCLABS" +
                ", TRVCST" +
                ", TRVLBN" +
                ", TRVLBR" +
                ", TRVMCS" +
                ", TRVMIN" +
                ", TRVMIL" +
                ", MISCST" +
                ", MISREN" +
                ", MISREV" +
                ", TCKTAX" +
                ", TCKTOT" +
                ", TCKBAL" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            double dTemp = 0.0;
            //double dBil = 0.0;

            if (dt.Rows.Count > 0) 
            {
                // Trip
                if (double.TryParse(dt.Rows[0]["TRPCSN"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilTrip.Text = dTemp.ToString("$###,##0.00");

                // Material
                if (double.TryParse(dt.Rows[0]["MATSAL"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilMaterial.Text = dTemp.ToString("$###,##0.00");
                        
                // Labor
                if (double.TryParse(dt.Rows[0]["LABSAL"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilOnsLabor.Text = dTemp.ToString("$###,##0.00");

                // Travel Labor
                if (double.TryParse(dt.Rows[0]["TRVLBN"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilTrvLabor.Text = dTemp.ToString("$###,##0.00");

                // Travel Mileage
                if (double.TryParse(dt.Rows[0]["TRVMIN"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilTrvMilage.Text = dTemp.ToString("$###,##0.00");
                        
                // Misc Revenue
                if (double.TryParse(dt.Rows[0]["MISREN"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilMisc.Text = dTemp.ToString("$###,##0.00");

                // Sales Tax
                if (double.TryParse(dt.Rows[0]["TCKTAX"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0) 
                        lbBilTax.Text = dTemp.ToString("$###,##0.00");

                // Ticket Total
                if (double.TryParse(dt.Rows[0]["TCKTOT"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0)
                        lbTotDue.Text = dTemp.ToString("$###,##0.00");

                // Balance Due
                if (double.TryParse(dt.Rows[0]["TCKBAL"].ToString().Trim(), out dTemp) == true)
                    if (dTemp > 0)
                        lbBalDue.Text = dTemp.ToString("$###,##0.00");

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
    protected string[] Select_ImageText(int center, int ticket)
    {
        string[] saImgTxt = { "" };

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                 " CILD1" +
                ", CITX1" +
                ", CILD2" +
                ", CITX2" +
                ", CILD3" +
                ", CITX3" +
                " from " + sLibrary + ".CALIMGTXT" +
                " where CICTR = ?" +
                " and CITCK = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ctr", center);
            odbcCmd.Parameters.AddWithValue("@Tck", ticket);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            string sTextList = "";
            if (dt.Rows.Count > 0)
            {
                sTextList += dt.Rows[0]["CILD1"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX1"].ToString() + "|";
                sTextList += dt.Rows[0]["CILD2"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX2"].ToString() + "|";
                sTextList += dt.Rows[0]["CILD3"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX3"].ToString() + "|X";  // final end value to protect array size if blanks exist
            }
            saImgTxt = sTextList.Split('|');
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return saImgTxt;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void loadPhotos()
    {
        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            string sRootPhotoPath = "";
            string sUrl = "";

            if (sLibrary == "OMDTALIB")
                sRootPhotoPath = "http://e1.scantronts.com/";
            else
                sRootPhotoPath = "http://e2.scantronts.com/";
            sRootPhotoPath += "media/images/call/" + iCtr + "-" + iTck + "/IMG_" + iCtr + "-" + iTck + "_";
            sUrl = sRootPhotoPath + "1.jpg";
            imbt_Image1.ImageUrl = sUrl;
            imbt_Image1.CommandArgument = sUrl;

            sUrl = sRootPhotoPath + "2.jpg";
            imbt_Image2.ImageUrl = sUrl;
            imbt_Image2.CommandArgument = sUrl;

            sUrl = sRootPhotoPath + "3.jpg";
            imbt_Image3.ImageUrl = sUrl;
            imbt_Image3.CommandArgument = sUrl;

            string[] saImgTxt = Select_ImageText(iCtr, iTck);

            if (saImgTxt.Length > 5)
            {
                pnPhotos.Visible = true;
                if (saImgTxt[0] == "Y")
                {
                    imbt_Image1.Visible = true;
                    lb_Image1.Text = saImgTxt[1];
                }
                else
                {
                    imbt_Image1.Visible = false;
                    lb_Image1.Visible = false;
                }
                if (saImgTxt[2] == "Y")
                {
                    imbt_Image2.Visible = true;
                    lb_Image2.Text = saImgTxt[3];
                }
                else
                {
                    imbt_Image2.Visible = false;
                    lb_Image2.Visible = false;
                }
                if (saImgTxt[4] == "Y")
                {
                    imbt_Image3.Visible = true;
                    lb_Image3.Text = saImgTxt[5];
                }
                else
                {
                    imbt_Image3.Visible = false;
                    lb_Image3.Visible = false;
                }
            }
            else
            {
                pnPhotos.Visible = false;
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
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void lkImage_Click(object sender, EventArgs e)
    {
        ImageButton myControl = (ImageButton)sender;
        string sUrl = myControl.CommandArgument.ToString().Trim();
        Response.Redirect(sUrl);
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
}