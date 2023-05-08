using System;
using System.Data;

public partial class public_sc_MaintenanceCoverage : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {


    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
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
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================

    protected void btSearchSubmit_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string sCity = txSearchCity.Text.Trim().ToUpper();
        string sZip = txSearchZip.Text.Trim().ToUpper();

        dt = ws_Get_B1MaintenanceCoverage(sCity, sZip);
        rp_CoverageSmall.DataSource = dt;
        rp_CoverageSmall.DataBind();

        gv_CoverageLarge.DataSource = dt;
        gv_CoverageLarge.DataBind();
    }
    // -------------------------------------------------------------------------------------------------------
    protected void btSearchClear_Click(object sender, EventArgs e)
    {
        txSearchCity.Text = "";
        txSearchZip.Text = "";
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
