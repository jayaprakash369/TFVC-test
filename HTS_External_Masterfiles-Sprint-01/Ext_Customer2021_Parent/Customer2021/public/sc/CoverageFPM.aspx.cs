using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_sc_CoverageFPM : MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B1MaintenanceCoverage(string city, string zip)
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(city) || !String.IsNullOrEmpty(zip))
        {
            string sJobName = "Get_B1MaintenanceCoverage";
            string sFieldList = "city|zip|x";
            string sValueList = city + "|" + zip + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchSubmit_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string sCity = txSearchCity.Text.Trim().ToUpper();
        string sZip = txSearchZip.Text.Trim().ToUpper();

        dt = ws_Get_B1MaintenanceCoverage(sCity, sZip);

        gv_Coverage.DataSource = dt;
        gv_Coverage.DataBind();
    }
    // ========================================================================

    protected void btSearchClear_Click(object sender, EventArgs e)
    {
        txSearchCity.Text = "";
        txSearchZip.Text = "";
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

}