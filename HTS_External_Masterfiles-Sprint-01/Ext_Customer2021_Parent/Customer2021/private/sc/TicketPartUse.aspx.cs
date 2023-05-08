using System;
using System.Data;

public partial class private_sc_TicketPartUse : MyPage
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

                dt = ws_Get_B1TicketPartsUsed(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_PartUseSmall.DataSource = dt;
                    rp_PartUseSmall.DataBind();
                    gv_PartUseLarge.DataSource = dt;
                    gv_PartUseLarge.DataBind();
                }
            }
        }
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B1TicketPartsUsed(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_TicketPartsUsed";
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
