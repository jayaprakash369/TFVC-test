using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_customerAdministration_mp_DeviceUtilization : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    //string sTemp = "";

    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";

        if (!IsPostBack)
        {

            Get_UserPrimaryCustomerNumber();

            try
            {
                Load_DeviceUtilizationData();
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {

            }
        }
    }
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B1UtilizationForAllDevices(
        string customerNumber,
        string startDate,
        string endDate,
        string cutoffLevel,
        string highOrLow
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1UtilizationForAllDevices";
            string sFieldList = "customerNumber|startDate|endDate|cutoffLevel|highOrLow|x";
            string sValueList = customerNumber + "|" + startDate + "|" + endDate + "|" + cutoffLevel + "|" + highOrLow + "|x";

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
    // ----------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ----------------------------------------------------------------------------
    protected void Load_DeviceUtilizationData()
    {
        DataTable dt = new DataTable("Primary");
        DateTime datTemp = DateTime.Now;
        string sDataFoundToShow = "";

        pnHigh.Visible = false;
        pnLow.Visible = false;

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                //hfChartDataA.Value = "Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec";
                //hfChartDataB.Value = "54, 67, 41, 55, 62, 45, 55, 73, 60, 76, 48, 79";

                int iCustomerNumber = 0;

                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;

                if (iCustomerNumber > 0)
                {
                    dt = ws_Get_B1UtilizationForAllDevices(
                        iCustomerNumber.ToString(),
                        datTemp.AddMonths(-7).ToString("yyyyMMdd"),
                        datTemp.ToString("yyyyMMdd"),
                        "6",
                        "high"
                        );

                    if (dt.Rows.Count > 0)
                    {
                        sDataFoundToShow = Load_ChartFields_High(dt);
                        gv_High.DataSource = dt;
                        gv_High.DataBind();
                        if (sDataFoundToShow == "Y")
                            pnHigh.Visible = true;

                    }

                    dt = ws_Get_B1UtilizationForAllDevices(
                        iCustomerNumber.ToString(),
                        datTemp.AddMonths(-7).ToString("yyyyMMdd"),
                        datTemp.ToString("yyyyMMdd"),
                        "1",
                        "low"
                        );
                    if (dt.Rows.Count > 0)
                    {
                        sDataFoundToShow = Load_ChartFields_Low(dt);
                        gv_Low.DataSource = dt;
                        gv_Low.DataBind();
                        if (sDataFoundToShow == "Y")
                            pnLow.Visible = true;
                    }
                }
            }
            if (pnHigh.Visible == false && pnLow.Visible == false)
                lbMsg.Text = "No recent Managed Print utilization data found to display.";
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ----------------------------------------------------------------------------
    protected string Load_ChartFields_High(DataTable dt)
    {
        string sDataFoundToShow = "";

        double dUtilization = 0.0;
        string sUtilization = "";

        hfChartHighLabel.Value = "";
        hfChartHighData.Value = "";

        int iRowIdx = 0;
        int iRowsLoaded = 0;

        foreach (DataRow row in dt.Rows)
        {
            if (iRowIdx < 15)
            {
                if (hfChartHighLabel.Value != "")
                    hfChartHighLabel.Value += ",";
                hfChartHighLabel.Value += row["PART"].ToString().Trim() + " (" + row["SERIAL"].ToString().Trim() + ")";

                if (double.TryParse(row["UTILIZATION"].ToString().Trim(), out dUtilization) == true)
                {
                    sUtilization = (dUtilization * 100).ToString("0.00");

                    if (hfChartHighData.Value != "")
                        hfChartHighData.Value += ",";
                    hfChartHighData.Value += sUtilization;
                    
                    iRowsLoaded++;
                }
            }
            iRowIdx++;
        }

        if (iRowsLoaded > 0)
            sDataFoundToShow = "Y";

        return sDataFoundToShow;
    }
    // ----------------------------------------------------------------------------
    protected string Load_ChartFields_Low(DataTable dt)
    {
        string sDataFoundToShow = "";

        double dUtilization = 0.0;
        string sUtilization = "";

        hfChartLowLabel.Value = "";
        hfChartLowData.Value = "";
        
        int iRowIdx = 0;
        int iRowsLoaded = 0;

        foreach (DataRow row in dt.Rows)
        {
            if (iRowIdx < 15)
            {

                if (hfChartLowLabel.Value != "")
                    hfChartLowLabel.Value += ",";
                hfChartLowLabel.Value += row["PART"].ToString().Trim() + " (" + row["SERIAL"].ToString().Trim() + ")";

                if (double.TryParse(row["UTILIZATION"].ToString().Trim(), out dUtilization) == true)
                {
                    sUtilization = (dUtilization * 100).ToString("0.0000");
                    if ((dUtilization * 100) < 0.05)
                        sUtilization = "0.0500";

                    if (hfChartLowData.Value != "")
                        hfChartLowData.Value += ",";
                    hfChartLowData.Value += sUtilization;
                    
                    iRowsLoaded++;
                }
            }
            iRowIdx++;
        }

        if (iRowsLoaded > 0)
            sDataFoundToShow = "Y";

        return sDataFoundToShow;
    }
    // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 2) 
            {
                hfPrimaryCs1.Value = saPreNumTyp[1];
                hfPrimaryCs1Type.Value = saPreNumTyp[2];
            }
                

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0)
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
            }

            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}