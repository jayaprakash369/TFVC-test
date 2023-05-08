using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;

public partial class public_sc_B2TicketDetail : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";

    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtService = new DataTable();
        DataTable dtProject = new DataTable();
        DataTable dt = new DataTable();

        lbMsg.Text = "";

        //int iCenter = 0;
        //int iTicket = 0;
        //int iCustomerNumber = 0;
        string sId = "";
        string sServiceOrProject = "";

        int[] iaCtrTck = new int[2];
        if (Request.QueryString["key"] != null && Request.QueryString["key"].ToString() != "")
            sId = Request.QueryString["key"];

        if (Request.QueryString["typ"] != null && Request.QueryString["typ"].ToString() != "")
            sServiceOrProject = Request.QueryString["typ"];


        if ((sId == null) || (sId == ""))
        {
            lbMsg.Text = "A key must be passed to access ticket detail... ";
        }
        else
        {
            lbTitleTicket.Text = "ID:" + sId;
            dt = ws_Get_B2TicketDetailForCall(sId, sServiceOrProject);

            if (dt.Rows.Count <= 0)
            {
                lbMsg.Text = "The key did not correspond to a valid ticket number... ";
                Clear_Screen();
            }
            else
            {
                Load_B2TicketData(dt);

                if (sServiceOrProject == "Service")
                    dtService = ws_Get_B2TicketNotes(sId, "ServiceAll");
                else
                    dtProject = ws_Get_B2TicketNotes(sId, "Project");

                //dt = ws_Get_B2TicketNotes(sId, sServiceOrProject); // old minimal list of notes
                dt = Merge_NoteTables(dtService, dtProject);

                if (dt.Rows.Count > 0)
                {
                    rp_B2TicketNotesSmall.DataSource = dt;
                    rp_B2TicketNotesSmall.DataBind();

                    gv_B2TicketNotesLarge.DataSource = dt;
                    gv_B2TicketNotesLarge.DataBind();

                    pnNotesLarge.Visible = true;
                    pnNotesSmall.Visible = true;
                }
                else 
                {
                    pnNotesLarge.Visible = false;
                    pnNotesSmall.Visible = false;

                }
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable Merge_NoteTables(DataTable service, DataTable project)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        DateTime datTemp = new DateTime();
        DataRow dr;
        int iRowIdx = 0;
        int iQty = 0;
        int iTemp = 0;

        //dt.Columns.Add(MakeColumn("bundledFlag"));
        //dt.Columns.Add(MakeColumn("contact-id"));
        //dt.Columns.Add(MakeColumn("contact-name"));
        dt.Columns.Add(MakeColumn("detailDescriptionFlag"));
        dt.Columns.Add(MakeColumn("id"));
        dt.Columns.Add(MakeColumn("internalAnalysisFlag"));
        //dt.Columns.Add(MakeColumn("isMarkdownFlag"));
        //dt.Columns.Add(MakeColumn("issueFlag"));
        //dt.Columns.Add(MakeColumn("member-id"));
        //dt.Columns.Add(MakeColumn("member-identifier"));
        //dt.Columns.Add(MakeColumn("member-name"));
        //dt.Columns.Add(MakeColumn("mergedFlag"));
        dt.Columns.Add(MakeColumn("noteType"));
        dt.Columns.Add(MakeColumn("originalAuthor"));
        dt.Columns.Add(MakeColumn("resolutionFlag"));
        dt.Columns.Add(MakeColumn("text"));
        dt.Columns.Add(MakeColumn("ticket-id"));
        dt.Columns.Add(MakeColumn("ticket-summary"));
        //dt.Columns.Add(MakeColumn("timeEnd"));
        dt.Columns.Add(MakeColumn("timeStart"));

        //dt.Columns.Add(MakeColumn("timeEndSort"));
        dt.Columns.Add(MakeColumn("timeStartSort"));

        dt.Columns.Add(MakeColumn("Source"));


        foreach (DataRow row in service.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["text"] = row["text"].ToString().Trim();
            dt.Rows[iRowIdx]["ticket-summary"] = row["ticket-summary"].ToString().Trim();


            if (DateTime.TryParse(row["timeStart"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["timeStart"] = datTemp.ToString("MMM d yyyy");
                dt.Rows[iRowIdx]["timeStartSort"] = datTemp.ToString("o");
            }

            //if (DateTime.TryParse(row["timeEnd"].ToString().Trim(), out datTemp) == true)
            //{
            //    dt.Rows[iRowIdx]["timeEnd"] = datTemp.ToString("MMM d yyyy");
            //    dt.Rows[iRowIdx]["timeEndSort"] = datTemp.ToString("o");
            //}

            dt.Rows[iRowIdx]["Source"] = "Service";

            iRowIdx++;
        }

        foreach (DataRow row in project.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["text"] = row["text"].ToString().Trim();
            dt.Rows[iRowIdx]["ticket-summary"] = row["ticket-summary"].ToString().Trim();


            if (DateTime.TryParse(row["_info-dateEntered"].ToString().Trim(), out datTemp) == true)
            {
                dt.Rows[iRowIdx]["timeStart"] = datTemp.ToString("MMM d yyyy");
                dt.Rows[iRowIdx]["timeStartSort"] = datTemp.ToString("o");
            }

            dt.Rows[iRowIdx]["Source"] = "Project";

            iRowIdx++;
        }

        dt.AcceptChanges();

        return dt;
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B2TicketDetailForCall(string id, string serviceOrProject)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(id))
        {
            string sJobName = "Get_B2TicketDetailForCall";
            string sFieldList = "id|typ|x";
            string sValueList = id + "|" + serviceOrProject + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ---------------------------------------------------------------------------------
    protected DataTable ws_Get_B2TicketNotes(string id, string serviceOrProject)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(id))
        {
            string sJobName = "Get_B2TicketNotes";
            string sFieldList = "id|typ|x";
            string sValueList = id + "|" + serviceOrProject + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // -------------------------------------------------------------------------------------------------------
    protected void Load_B2TicketData(DataTable dt) 
    {
        if (dt.Rows.Count > 0)
        {
            // ----------------------------------------------------------------------
            //lbMsg.Text = "Ticket Decrypted: " + iCenter + "-" + iTicket;
            lbCstAddress.Text = dt.Rows[0]["addressLine1"].ToString().Trim() + " " + dt.Rows[0]["addressLine2"].ToString().Trim();
            lbCstCityStateZip.Text = dt.Rows[0]["city"].ToString().Trim() + " " + dt.Rows[0]["stateIdentifier"].ToString().Trim() + " " + dt.Rows[0]["zip"].ToString().Trim();
            lbCstContact.Text = dt.Rows[0]["contactName"].ToString().Trim();
            lbCstName.Text = dt.Rows[0]["siteName"].ToString().Trim();
            lbCstPhone.Text = dt.Rows[0]["contactPhone"].ToString().Trim();
            lbTckSeverity.Text = dt.Rows[0]["severity"].ToString().Trim();
            lbTckStatus.Text = dt.Rows[0]["status-name"].ToString().Trim();
            lbTckSummary.Text = dt.Rows[0]["summary"].ToString().Trim();
            // ----------------------------------------------------------------------
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void Clear_Screen()
    {
        // ----------------------------------------------------------------------
        //lbMsg.Text = "Ticket Decrypted: " + iCenter + "-" + iTicket;
        lbCstAddress.Text = "";
        lbCstCityStateZip.Text = "";
        lbCstContact.Text = "";
        lbCstName.Text = "";
        lbCstPhone.Text = "";
        lbTckSeverity.Text = "";
        lbTckStatus.Text = "";
        lbTckSummary.Text = "";
        // ----------------------------------------------------------------------
    }
    // -------------------------------------------------------------------------------------------------------
    protected void Load_B2NoteData(DataTable dt)
    {
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================

    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
