using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
//using System.Web.Security;

public partial class private_mp_DeviceUtilization : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    ManagedPrint_LIVE.ManagedPrintMenuSoapClient wsLiveMps = new ManagedPrint_LIVE.ManagedPrintMenuSoapClient();
    ManagedPrint_DEV.ManagedPrintMenuSoapClient wsTestMps = new ManagedPrint_DEV.ManagedPrintMenuSoapClient();

//  Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
//  Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
//  SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        double dUseHigh = 6;
        double dUseLow = 1;
        string sAgr = "";
        string sHighLow = "";
        DateTime datTemp = DateTime.Now;
        int iEnd = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        int iStart = Int32.Parse(datTemp.AddMonths(-6).ToString("yyyyMMdd"));

        int iPrimaryCs1 = 0;
        int.TryParse(Profile.LoginCs1.ToString(), out iPrimaryCs1);
        if (Session["adminCs1"] != null)
            int.TryParse(Session["adminCs1"].ToString(), out iPrimaryCs1);

        // Devices By Utilization: High
        sHighLow = "HIGH";
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getUtilizationForAllDevices(iPrimaryCs1, sAgr, iStart, iEnd, dUseHigh, sHighLow);
        else
            dataTable = wsTestMps.getUtilizationForAllDevices(iPrimaryCs1, sAgr, iStart, iEnd, dUseHigh, sHighLow);
        DevicesByUse(dataTable, sHighLow, dUseHigh);

        // Devices By Utilization: Low
        sHighLow = "LOW";
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getUtilizationForAllDevices(iPrimaryCs1, sAgr, iStart, iEnd, dUseLow, sHighLow);
        else
            dataTable = wsTestMps.getUtilizationForAllDevices(iPrimaryCs1, sAgr, iStart, iEnd, dUseLow, sHighLow);
        DevicesByUse(dataTable, sHighLow, dUseLow);
    }
    // =========================================================================
    protected void DevicesByUse(DataTable dtTable, string highLow, double cutoff)
    {
        string sColName = "";
        string sFieldValue = "";
        string sPart = "";
        string sFxa = "";
        int iLoc = 0;
        int iRowIdx = 0;
        int iMaxRows = 30;

        if (dtTable.Rows.Count < iMaxRows)
            iMaxRows = dtTable.Rows.Count;

        string[] xValues = new string[iMaxRows];
        double[] yValues = new double[iMaxRows];

        double utilization = 0;
        iRowIdx = iMaxRows;

        foreach (DataRow row in dtTable.Rows)
        {

            if (iRowIdx > 0)
            {
                iRowIdx--;
                foreach (DataColumn col in dtTable.Columns)
                {
                    sColName = col.ToString();
                    sFieldValue = row[col].ToString();
                    if (sColName.Equals("LOCATION"))
                    {
                        iLoc = Int32.Parse(sFieldValue.Trim());
                    }
                    if (sColName.Equals("PART"))
                    {
                        sPart = sFieldValue.Trim();
                    }
                    if (sColName.Equals("FIXEDASSET"))
                    {
                        sFxa = sFieldValue.Trim();
                    }
                    if (sColName.Equals("UTILIZATION"))
                    {
                        utilization = Math.Round(100 * double.Parse(sFieldValue.Trim()), 2);
                        //xValues[iRowIdx] = utilization.ToString() + "% " + sPart + " (" + sSerial + ") Loc:" + iLoc.ToString();
                        xValues[iRowIdx] = sPart + " (" + sFxa + ") Loc: " + iLoc.ToString();
                        yValues[iRowIdx] = utilization;
                    }
                }
            }
        }

        if (highLow == "HIGH")
        {
            chartByUseHigh.Titles["Title1"].Text = "Devices By Utilization (Above " + cutoff.ToString() + "%)";
            chartByUseHigh.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chartByUseHigh.Series["Series1"].IsValueShownAsLabel = true;
            chartByUseHigh.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false; // makes labels line up with bars
            chartByUseHigh.Height = 65 + (iMaxRows * 2) + (iMaxRows * 35);
            chartByUseHigh.Legends[0].Enabled = false;
            // How to ensure AxisLabels always print!
            chartByUseHigh.ChartAreas[0].AxisX.Interval = 1;
        }
        else
        {
            chartByUseLow.Titles["Title1"].Text = "Devices By Utilization (Below " + cutoff.ToString() + "%)";
            chartByUseLow.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chartByUseLow.Series["Series1"].IsValueShownAsLabel = true;
            chartByUseLow.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;  // makes labels line up with bars
            chartByUseLow.Height = 65 + (iMaxRows * 35);
            chartByUseLow.Legends[0].Enabled = false;
            // How to ensure AxisLabels always print!
            chartByUseLow.ChartAreas[0].AxisX.Interval = 1;
        }
        // -------------------------        
    }
    // =========================================================
    // =========================================================
}