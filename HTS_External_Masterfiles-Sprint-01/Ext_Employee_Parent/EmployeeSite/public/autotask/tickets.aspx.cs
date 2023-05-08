using System;
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

public partial class public_autotask_Tickets : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler erh;
    DateFormatter df;
    char[] cSplitter = { '|' };
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        if (!IsPostBack)
        {

            try
            {
                odbcConn.Open();

                if (Request.QueryString["tid"] != null && Request.QueryString["tid"].ToString() != "")
                {
                    string sGuid = Request.QueryString["tid"].ToString().Trim();
                    hfTicketDateId.Value = GetDateIdFromGuid(sGuid);
                }

                gvTickets.DataSource = SelectAtTickets(hfTicketDateId.Value.Trim());
                gvTickets.DataBind();
                
                if (!String.IsNullOrEmpty(hfTicketDateId.Value))
                    LoadDetail(hfTicketDateId.Value);
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
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected string GetDateIdFromGuid(string guid)
    {
        string sTicketDateId = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                         " ATTICKET" +
                        " from " + sLibrary + ".ATTCKXRF" +
                        " where ID = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@AtId", guid);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sTicketDateId = dt.Rows[0]["ATTICKET"].ToString().Trim();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sTicketDateId;
    }
    // ========================================================================
    protected DataTable SelectAtTickets(string ticketID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                         " ID" +
                        ", ATTICKET" +
                        ", ATCUSXRF" +
                        ", QUEUEID" +
                        ", ISSTYPNM" +
                        ", ISSTYPTX" +
                        ", ISSSUBNM" +
                        ", ISSSUBTX" +
                        ", PRIORITY" +
                        ", STATUS" +
                        ", HOURS" +
                        ", ORACLEID" +
                        ", ACCTID" +
                        ", ACCTNAME" +
                        ", ACCTCON" +
                        ", ACCTPHN" +
                        ", ACCTAD1" +
                        ", ACCTAD2" +
                        ", ACCTCIT" +
                        ", ACCTSTA" +
                        ", ACCTZIP" +
                        ", DTCREATE" +
                        ", DTDUE" +
                        ", DTLASTAC" +
                        ", TITLE" +
                        ", DESC" +
                        ", ATMODEL" +
                        ", ATSERIAL" +
                        ", CONTRACT" +
                        ", ALLOCDID" +
                        ", ALLOCDDS" +
                        ", ACLOSED" +
                        ", HCLOSED" +
                        ", CS1" +
                        ", CS2" +
                        ", CS1CS2" +
                        ", CTR" +
                        ", TCK" +
                        ", FSTNUM" +
                        ", FSTNAM" +
                        ", REMARK" +
                        ", ERACLOSE" +
                        " from " + sLibrary + ".ATTCKXRF" +
                        " where STATUS > 0";
            if (!String.IsNullOrEmpty(ticketID))
                sSql += " and ATTICKET like ?";
            else 
            {
                if (rblSearchStatus.SelectedValue == "Closed")
                    sSql += " and STATUS = 5";
                else if (rblSearchStatus.SelectedValue == "Both") { }
                else
                    sSql += " and STATUS <> 5";
            }
            sSql += " order by ATTICKET desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            if (!String.IsNullOrEmpty(ticketID))
                odbcCmd.Parameters.AddWithValue("@AtId", ticketID + "%");

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayCreated"));
            dt.Columns.Add(MakeColumn("DisplayStatus"));
            dt.Columns.Add(MakeColumn("DisplayCall"));

            string sTemp = "";
            string sStatus = "";
            DateTime datTemp;
            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                sTemp = dt.Rows[iRowIdx]["DTCREATE"].ToString().Trim();
                if (!String.IsNullOrEmpty(sTemp))
                {
                    if (DateTime.TryParse(sTemp, out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayCreated"] = datTemp.ToString("MMM d, yyyy  h:mm tt");
                }
                if (!String.IsNullOrEmpty(dt.Rows[iRowIdx]["CTR"].ToString().Trim()))
                    dt.Rows[iRowIdx]["DisplayCall"] = dt.Rows[iRowIdx]["CTR"].ToString().Trim() + "-" + dt.Rows[iRowIdx]["TCK"].ToString().Trim();

                sStatus = dt.Rows[iRowIdx]["STATUS"].ToString().Trim();

                // --------------------------------------
                switch (sStatus)
                {
                    case "1": { dt.Rows[iRowIdx]["DisplayStatus"] = "New"; break; }
                    case "5": { dt.Rows[iRowIdx]["DisplayStatus"] = "Complete"; break; }
                    case "7": { dt.Rows[iRowIdx]["DisplayStatus"] = "Waiting Customer"; break; }
                    case "8": { dt.Rows[iRowIdx]["DisplayStatus"] = "In Progress"; break; }
                    case "9": { dt.Rows[iRowIdx]["DisplayStatus"] = "Waiting Materials"; break; }
                    case "10": { dt.Rows[iRowIdx]["DisplayStatus"] = "Dispatched"; break; }
                    case "11": { dt.Rows[iRowIdx]["DisplayStatus"] = "Escalated"; break; }
                    case "12": { dt.Rows[iRowIdx]["DisplayStatus"] = "Waiting Vendor"; break; }
                    case "13": { dt.Rows[iRowIdx]["DisplayStatus"] = "Scheduled"; break; }
                    case "14": { dt.Rows[iRowIdx]["DisplayStatus"] = "Client Update Required"; break; }
                    case "15": { dt.Rows[iRowIdx]["DisplayStatus"] = "Opportunity Pending"; break; }
                    case "16": { dt.Rows[iRowIdx]["DisplayStatus"] = "Pending Complete"; break; }
                    case "17": { dt.Rows[iRowIdx]["DisplayStatus"] = "Waiting Approval"; break; }
                    case "18": { dt.Rows[iRowIdx]["DisplayStatus"] = "Waiting Dispatch"; break; }
                    case "19": { dt.Rows[iRowIdx]["DisplayStatus"] = "Test"; break; }
                    case "20": { dt.Rows[iRowIdx]["DisplayStatus"] = "Contacted Client"; break; }
                    case "21": { dt.Rows[iRowIdx]["DisplayStatus"] = "Re-Opened"; break; }
                    case "22": { dt.Rows[iRowIdx]["DisplayStatus"] = "Esc-In Progress"; break; }
                    case "23": { dt.Rows[iRowIdx]["DisplayStatus"] = "Esc-Waiting Vendor"; break; }
                    case "24": { dt.Rows[iRowIdx]["DisplayStatus"] = "Esc-Waiting Resource"; break; }
                    case "25": { dt.Rows[iRowIdx]["DisplayStatus"] = "Esc-Scheduled"; break; }
                    case "26": { dt.Rows[iRowIdx]["DisplayStatus"] = "Investigating"; break; }
                    case "27": { dt.Rows[iRowIdx]["DisplayStatus"] = "Action Plan Ready"; break; }
                    case "28": { dt.Rows[iRowIdx]["DisplayStatus"] = "Validating"; break; }
                    case "29": { dt.Rows[iRowIdx]["DisplayStatus"] = "Escalated to Vendor"; break; }
                    case "30": { dt.Rows[iRowIdx]["DisplayStatus"] = "Technician Notified"; break; }
                    case "31": { dt.Rows[iRowIdx]["DisplayStatus"] = "Technician En Route"; break; }
                    case "32": { dt.Rows[iRowIdx]["DisplayStatus"] = "Technician Onsite"; break; }
                    case "33": { dt.Rows[iRowIdx]["DisplayStatus"] = "On Hold"; break; }
                    // default: { break; };
                }
                // --------------------------------------

                iRowIdx++;
            }

            dt.AcceptChanges();

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
    protected DataTable SelectErrors(string ticketDateID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        try
        {
            string sSql = "Select" +
                // ATERRLOG
                         " ELTCKID" +
                        ", ELERRID" +
                        ", ELCODE" +
                        ", ELEVENT" +
                        ", ELSTAMP" +
                // ATERRCOD
                        ", ECTYPE" +
                        ", ECDESC" +
                        " from " + sLibrary + ".ATERRLOG l, " + sLibrary + ".ATERRCOD c, " + sLibrary + ".ATTCKXRF x" +
                        " where " +
                        " l.ELTCKID = x.ID " + // error log to ticket
                        " and l.ELCODE = c.ECNUM " + // error log to error code
                        " and x.ATTICKET = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@AtDateId", ticketDateID);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayStamp"));

            string sTemp = "";
            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                sTemp = dt.Rows[iRowIdx]["ELSTAMP"].ToString().Trim();
                if (!String.IsNullOrEmpty(sTemp))
                {
                    if (DateTime.TryParse(sTemp, out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayStamp"] = datTemp.ToString("MMM d, yyyy  h:mm tt");
                }


                iRowIdx++;
            }

            dt.AcceptChanges();

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
    protected DataTable SelectAtNotes(string ticketID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                         " TNTITLE" +
                        ", TNTEXT" +
                        " from " + sLibrary + ".ATTCKNOTA n, " + sLibrary + ".ATTCKXRF x" +
                        " where " +
                        " n.TNTCKID = x.ID " + 
                        " and x.ATTICKET = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@AtId", ticketID);
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
    protected DataTable SelectHtsNotes(string ticketID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                         " HNTITLE" +
                        ", HNTEXT" +
                        " from " + sLibrary + ".ATTCKNOTH n, " + sLibrary + ".ATTCKXRF x" +
                        " where " +
                        " n.HNTCKID = x.ID " +
                        " and x.ATTICKET = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@AtId", ticketID);
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
    protected DataTable GetPartsUsed(int ctr, int tck)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        try
        {
            string sSql = "Select" +
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

            odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
            odbcCmd.Parameters.AddWithValue("@Tck", tck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));

            int iDat = 0;
            string sDat = "";

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["DateUsed"].ToString(), out iDat);
                if (iDat > 20000101 && iDat < 20990101) 
                {
                    sDat = iDat.ToString();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayDate"] = datTemp.ToString("MMM d, yyyy");
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
        return dt;
    }
    // ========================================================================
    protected DataTable GetOnsiteLabor(int ctr, int tck)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        try
        {
            string sSql = "Select" +
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

            odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
            odbcCmd.Parameters.AddWithValue("@Tck", tck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));
            
            string sDat = "";
            int iDat = 0;
            double dTemp = 0.0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iDat);
                if (iDat > 20000101 && iDat < 20990101) 
                {
                    sDat = iDat.ToString();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayDate"] = datTemp.ToString("MMM d, yyyy");
                }
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
    protected DataTable GetTravelLaborOnsite(int ctr, int tck)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        try
        {
            string sSql = "Select" +
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

            odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
            odbcCmd.Parameters.AddWithValue("@Tck", tck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));

            int iDat = 0;
            double dTemp = 0.0;
            string sDat = "";

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iDat);
                if (iDat> 20000101 && iDat < 20990101) 
                {
                    sDat = iDat.ToString();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayDate"] = datTemp.ToString("MMM d, yyyy");
                }
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
    protected DataTable GetTravelLabor(int ctr, int tck)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        try
        {
            string sSql = "Select" +
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

            odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
            odbcCmd.Parameters.AddWithValue("@Tck", tck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayStart"));
            dt.Columns.Add(MakeColumn("DisplayEnd"));

            int iDat = 0;
            double dTemp = 0.0;
            string sDat = "";

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["StartDate"].ToString(), out iDat);
                if (iDat > 20000101 && iDat < 20990101) 
                {
                    sDat = iDat.ToString();
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayDate"] = datTemp.ToString("MMM d, yyyy");
                }
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
    protected void DoSqls()
    {
        try
        {
            odbcConn.Open();
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
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { erh = new ErrorHandler(); df = new DateFormatter(); }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { erh = null; df = null; }
    // ========================================================================
    protected DataTable LoadTicket(string ticketDateID) 
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            dt = SelectAtTickets(ticketDateID);
            string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                pnTicket.Visible = true;

                lbTckAid.Text = dt.Rows[0]["ATTICKET"].ToString().Trim();
                lbTckHid.Text = dt.Rows[0]["CTR"].ToString().Trim() + "-" + dt.Rows[0]["TCK"].ToString().Trim();
                lbTckNam.Text = dt.Rows[0]["ACCTNAME"].ToString().Trim();
                lbTckTit.Text = dt.Rows[0]["TITLE"].ToString().Trim();
                lbTckDsc.Text = dt.Rows[0]["DESC"].ToString().Trim();
                lbTckTyp.Text = dt.Rows[0]["ISSTYPTX"].ToString().Trim();
                lbTckSub.Text = dt.Rows[0]["ISSSUBTX"].ToString().Trim();
                lbTckCon.Text = dt.Rows[0]["ACCTCON"].ToString().Trim();
                sTemp = dt.Rows[0]["ACCTPHN"].ToString().Trim();
                if (!String.IsNullOrEmpty(sTemp) && sTemp.Length == 10)
                {
                    lbTckPhn.Text = "(" + sTemp.Substring(0, 3) + ") " + sTemp.Substring(3, 3) + "-" + sTemp.Substring(6, 4);
                }
                else
                    lbTckPhn.Text = sTemp;
                lbTckAdr.Text = dt.Rows[0]["ACCTAD1"].ToString().Trim() + " " +
                    dt.Rows[0]["ACCTAD2"].ToString().Trim() + " " +
                    dt.Rows[0]["ACCTCIT"].ToString().Trim() + " " +
                    dt.Rows[0]["ACCTSTA"].ToString().Trim() + " " +
                    dt.Rows[0]["ACCTZIP"].ToString().Trim();

                lbTckXrf.Text = dt.Rows[0]["ATCUSXRF"].ToString().Trim();
                lbTckAgr.Text = dt.Rows[0]["CONTRACT"].ToString().Trim();
                lbTckAlc.Text = dt.Rows[0]["ALLOCDDS"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    protected DataTable LoadErrors(string ticketDateID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            dt = SelectErrors(ticketDateID);
            //string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                pnErrors.Visible = true;
                rpErrors.DataSource = dt;
                rpErrors.DataBind();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    protected DataTable LoadAtNotes(string ticketID) 
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            dt = SelectAtNotes(ticketID);
            //string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                pnAtNotes.Visible = true;
                gvAtNotes.DataSource = dt;
                gvAtNotes.DataBind();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    protected DataTable LoadHtsNotes(string ticketID)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            dt = SelectHtsNotes(ticketID);
            //string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                pnHtsNotes.Visible = true;
                gvHtsNotes.DataSource = dt;
                gvHtsNotes.DataBind();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    protected void LoadDetail(string ticketDateID)
    {

        DataTable dt;
        DataTable dt2;

        int iCtr = 0;
        int iTck = 0;

        pnDetail.Visible = true;
        pnTicket.Visible = false;
        pnErrors.Visible = false;
        pnAtNotes.Visible = false;
        pnHtsNotes.Visible = false;

        try
        {
            dt = LoadTicket(ticketDateID);
            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["CTR"].ToString().Trim(), out iCtr) == false)
                    iCtr = 0;
                if (int.TryParse(dt.Rows[0]["TCK"].ToString().Trim(), out iTck) == false)
                    iTck = 0;
            }

            dt = LoadErrors(ticketDateID);
            LoadAtNotes(ticketDateID);

            if (iCtr > 0 && iTck > 0)
            {
                LoadHtsNotes(ticketDateID);

                // ---------------------------
                dt = GetPartsUsed(iCtr, iTck);
                if (dt.Rows.Count > 0)
                {
                    pnPartsUsed.Visible = true;
                    gvPartsUsed.DataSource = dt;
                    gvPartsUsed.DataBind();
                }
                // ---------------------------
                dt = GetOnsiteLabor(iCtr, iTck);
                if (dt.Rows.Count > 0)
                {
                    pnOnsiteLabor.Visible = true;
                    gvOnsiteLabor.DataSource = dt;
                    gvOnsiteLabor.DataBind();
                }
                // ---------------------------
                // If both have data, merge them together
                dt = GetTravelLaborOnsite(iCtr, iTck);
                if (dt.Rows.Count > 0)
                {
                    dt2 = GetTravelLabor(iCtr, iTck);
                    if (dt2.Rows.Count > 0)
                        dt.Merge(dt2);
                }
                else
                {
                    dt = GetTravelLabor(iCtr, iTck);
                }

                if (dt.Rows.Count > 0)
                {
                    pnTravelLabor.Visible = true;
                    gvTravelLabor.DataSource = dt;
                    gvTravelLabor.DataBind();
                }
                // ---------------------------
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void lkDetail_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        string sTicketID = myControl.CommandArgument.ToString().Trim();

        try
        {
            odbcConn.Open();
            LoadDetail(sTicketID);
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
    // ========================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        try
        {
            pnDetail.Visible = false;
            odbcConn.Open();
            gvTickets.DataSource = SelectAtTickets(txSearchTid.Text.Trim());
            gvTickets.DataBind();
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
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
}