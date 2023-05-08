using System;
using System.Data;

public partial class private_sc_Timestamps : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();

            int iCenter = 0;
            int iTicket = 0;
            if (Request.QueryString["ctr"] != null && Request.QueryString["ctr"].ToString() != "") 
            {
                if (int.TryParse(Request.QueryString["ctr"].ToString(), out iCenter) == false)
                    iCenter = -1;
            }
            if (Request.QueryString["tck"] != null && Request.QueryString["tck"].ToString() != "")
            {
                if (int.TryParse(Request.QueryString["tck"].ToString(), out iTicket) == false)
                    iTicket = -1;
            }
            if (iCenter > 0 && iTicket > 0) 
            {
                lbTitleTicket.Text = iCenter.ToString("") + "-" + iTicket.ToString("");

                dt = ws_Get_B1TicketTimestamps(iCenter, iTicket);
                //if (dt.Rows.Count > 0) // Show them if there are no records (but there should always be something...)
                //{
                    rp_TimestampSmall.DataSource = dt;
                    rp_TimestampSmall.DataBind();
                    gv_TimestampLarge.DataSource = dt;
                    gv_TimestampLarge.DataBind();

                    string sScheduleDateFound = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!String.IsNullOrEmpty(row["ScheduleFormatted"].ToString().Trim()))
                            sScheduleDateFound = "Y";
                    }
                    if (sScheduleDateFound == "Y")
                        gv_TimestampLarge.Columns[4].Visible = true;
                    else
                        gv_TimestampLarge.Columns[4].Visible = false;
                //}
            }
        }
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
    protected DataTable ws_Get_B1TicketTimestamps(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_TicketTimestamps";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }

    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
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
