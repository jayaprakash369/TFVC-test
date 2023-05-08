using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_customerAdministration_mp_PageCountsForCustomer : MyPage
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
                Load_PageCountData();
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
    protected DataTable ws_Get_B1PageCountsForCustomerOrAgreement(
        string customerNumber, 
        string agreement,
        string startDate,
        string endDate
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1PageCountsForCustomerOrAgreement";
            string sFieldList = "customerNumber|agreement|startDate|endDate|x";
            string sValueList = customerNumber + "|" + agreement + "|" + startDate + "|" + endDate + "|x";

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
    protected void Load_PageCountData()
    {
        DataTable dt = new DataTable("");
        DateTime datTemp = DateTime.Now;
        string sDataFoundToShow = "";

        pnMono.Visible = false;
        pnColor.Visible = false;
        pnMicr.Visible = false;

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
                    // You're getting all three values in different columns in this query (3 charts) 
                    dt = ws_Get_B1PageCountsForCustomerOrAgreement(
                        iCustomerNumber.ToString(), 
                        "", 
                        datTemp.AddMonths(-7).ToString("yyyyMMdd"), 
                        datTemp.ToString("yyyyMMdd"));

                    if (dt.Rows.Count > 0)
                    {
                        sDataFoundToShow = Load_ChartFields_Mono(dt);
                        if (sDataFoundToShow == "Y")
                            pnMono.Visible = true;

                        sDataFoundToShow = Load_ChartFields_Color(dt);
                        if (sDataFoundToShow == "Y")
                            pnColor.Visible = true;

                        sDataFoundToShow = Load_ChartFields_Micr(dt);
                        if (sDataFoundToShow == "Y")
                            pnMicr.Visible = true;
                    }
                }
            }
            if (pnMono.Visible == false && pnColor.Visible == false && pnMicr.Visible == false)
                lbMsg.Text = "No recent Managed Print page count history found.";
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
    protected string Load_ChartFields_Mono(DataTable dt)
    {
        string sDataFoundToShow = "";

        DateTime datTemp = DateTime.Now;
        string sDat = "";
        int iTemp = 0;
        int iMax = 0;
        string sTemp = "";

        hfChartMonoLabel.Value = "";
        hfChartMonoData.Value = "";
        hfChartMonoIncrement.Value = "";

        foreach (DataRow row in dt.Rows)
        {
            sDat = row["INVDATE"].ToString().Trim();
            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                sDat = datTemp.ToString("MMM yyyy");

            if (hfChartMonoLabel.Value != "")
                hfChartMonoLabel.Value += ",";
            hfChartMonoLabel.Value += sDat;

            if (int.TryParse(row["MONOMONTH"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > -1)
            {
                if (hfChartMonoData.Value != "")
                    hfChartMonoData.Value += ",";
                hfChartMonoData.Value += iTemp.ToString();
                if (iTemp > iMax)
                    iMax = iTemp;
                if (iTemp > 0)
                    sDataFoundToShow = "Y";
            }
        }

        if (iMax > 100)
        {
            iTemp = iMax / 5;
            sTemp = iTemp.ToString("0");
            if (sTemp.Length == 1) { }
            else if (sTemp.Length == 2) sTemp = sTemp.Substring(0, 1) + "0";
            else if (sTemp.Length == 3) sTemp = sTemp.Substring(0, 1) + "00";
            else if (sTemp.Length == 4) sTemp = sTemp.Substring(0, 2) + "00";
            else if (sTemp.Length == 5) sTemp = sTemp.Substring(0, 2) + "000";
            else if (sTemp.Length == 6) sTemp = sTemp.Substring(0, 2) + "0000";
            else if (sTemp.Length == 7) sTemp = sTemp.Substring(0, 2) + "00000";
            else if (sTemp.Length == 8) sTemp = sTemp.Substring(0, 2) + "000000";
            else if (sTemp.Length == 9) sTemp = sTemp.Substring(0, 2) + "0000000";
        }
        else
        {
            //iTemp = 10;
            sTemp = "10";
        }


        hfChartMonoIncrement.Value = sTemp;

        //hfChartMonoLabel.Value = "Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec";
        //hfChartMonoData.Value = "54, 67, 41, 55, 62, 45, 55, 73, 60, 76, 48, 79";

        return sDataFoundToShow;
    }
    // ----------------------------------------------------------------------------
    protected string Load_ChartFields_Color(DataTable dt)
    {
        string sDataFoundToShow = "";

        DateTime datTemp = DateTime.Now;
        string sDat = "";
        int iTemp = 0;
        int iMax = 0;
        string sTemp = "";

        hfChartColorLabel.Value = "";
        hfChartColorData.Value = "";
        hfChartColorIncrement.Value = "";

        foreach (DataRow row in dt.Rows)
        {
            sDat = row["INVDATE"].ToString().Trim();
            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                sDat = datTemp.ToString("MMM yyyy");

            if (hfChartColorLabel.Value != "")
                hfChartColorLabel.Value += ",";
            hfChartColorLabel.Value += sDat;

            if (int.TryParse(row["COLORMONTH"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp > -1)
            {
                if (hfChartColorData.Value != "")
                    hfChartColorData.Value += ",";
                hfChartColorData.Value += iTemp.ToString();
                if (iTemp > iMax)
                    iMax = iTemp;
                if (iTemp > 0)
                    sDataFoundToShow = "Y";
            }
        }

        if (iMax > 100)
        {
            iTemp = iMax / 5;
            sTemp = iTemp.ToString("0");
            if (sTemp.Length == 1) { }
            else if (sTemp.Length == 2) sTemp = sTemp.Substring(0, 1) + "0";
            else if (sTemp.Length == 3) sTemp = sTemp.Substring(0, 1) + "00";
            else if (sTemp.Length == 4) sTemp = sTemp.Substring(0, 2) + "00";
            else if (sTemp.Length == 5) sTemp = sTemp.Substring(0, 2) + "000";
            else if (sTemp.Length == 6) sTemp = sTemp.Substring(0, 2) + "0000";
            else if (sTemp.Length == 7) sTemp = sTemp.Substring(0, 2) + "00000";
            else if (sTemp.Length == 8) sTemp = sTemp.Substring(0, 2) + "000000";
            else if (sTemp.Length == 9) sTemp = sTemp.Substring(0, 2) + "0000000";
        }
        else
        {
            //iTemp = 10;
            sTemp = "10";
        }


        hfChartColorIncrement.Value = sTemp;
        
        return sDataFoundToShow;
    }
    // ----------------------------------------------------------------------------
    protected string Load_ChartFields_Micr(DataTable dt)
    {
        string sDataFoundToShow = "";
        DateTime datTemp = DateTime.Now;
        string sDat = "";
        int iTemp = 0;
        int iMax = 0;
        string sTemp = "";

        hfChartMicrLabel.Value = "";
        hfChartMicrData.Value = "";
        hfChartMicrIncrement.Value = "";

        foreach (DataRow row in dt.Rows)
        {
            sDat = row["INVDATE"].ToString().Trim();
            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                sDat = datTemp.ToString("MMM yyyy");

            if (hfChartMicrLabel.Value != "")
                hfChartMicrLabel.Value += ",";
            hfChartMicrLabel.Value += sDat;

            if (int.TryParse(row["MICRMONTH"].ToString().Trim(), out iTemp) == false)
                iTemp = -1;
            if (iTemp != -1)
            {
                if (hfChartMicrData.Value != "")
                    hfChartMicrData.Value += ",";
                hfChartMicrData.Value += iTemp.ToString();
                if (iTemp > iMax)
                    iMax = iTemp;
                if (iTemp > 0)
                    sDataFoundToShow = "Y";
            }
        }

        if (iMax > 100)
        {
            iTemp = iMax / 5;
            sTemp = iTemp.ToString("0");
            if (sTemp.Length == 1) { }
            else if (sTemp.Length == 2) sTemp = sTemp.Substring(0, 1) + "0";
            else if (sTemp.Length == 3) sTemp = sTemp.Substring(0, 1) + "00";
            else if (sTemp.Length == 4) sTemp = sTemp.Substring(0, 2) + "00";
            else if (sTemp.Length == 5) sTemp = sTemp.Substring(0, 2) + "000";
            else if (sTemp.Length == 6) sTemp = sTemp.Substring(0, 2) + "0000";
            else if (sTemp.Length == 7) sTemp = sTemp.Substring(0, 2) + "00000";
            else if (sTemp.Length == 8) sTemp = sTemp.Substring(0, 2) + "000000";
            else if (sTemp.Length == 9) sTemp = sTemp.Substring(0, 2) + "0000000";
        }
        else
        {
            //iTemp = 10;
            sTemp = "10";
        }


        hfChartMicrIncrement.Value = sTemp;

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