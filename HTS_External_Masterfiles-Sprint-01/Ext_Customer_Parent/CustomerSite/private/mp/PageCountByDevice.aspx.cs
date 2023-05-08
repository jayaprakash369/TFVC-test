using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_mp_PageCountByDevice : MyPage
{
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

        string sAgr = "";
        string[] saMonoColorMicr = new string[3];

        DateTime datTemp = DateTime.Now;
        int iEnd = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        int iStart = Int32.Parse(datTemp.AddMonths(-12).ToString("yyyyMMdd"));

        int iUnt = 0;
        if (Session["pageCountUnit"] != null)
            int.TryParse(Session["pageCountUnit"].ToString(), out iUnt);

        // Page Sums For Month For Customer
        if (sDevTestLive == "LIVE")
        {
            dataTable = wsLiveMps.getPageCountsForOneUnit(iUnt, iStart, iEnd);
        }
        else 
        {
            dataTable = wsTestMps.getPageCountsForOneUnit(iUnt, iStart, iEnd);
        }
        saMonoColorMicr = PagesByTypeSearch(dataTable);

        if (saMonoColorMicr[0] == "Y")
            PagesByTypeMono(dataTable);
        else
            chartByPagesMono.Visible = false;

        if (saMonoColorMicr[1] == "Y")
            PagesByTypeColor(dataTable);
        else
            chartByPagesColor.Visible = false;

        if (saMonoColorMicr[2] == "Y")
            PagesByTypeMicr(dataTable);
        else
            chartByPagesMicr.Visible = false;

    }
    // =========================================================================
    protected string[] PagesByTypeSearch(DataTable dtTable)
    {
        string[] saMonoColorMicr = { "N", "N", "N" };

        int iRowIdx = 0;
        int iMono = 0;
        int iColor = 0;
        int iMicr = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            iMono = Int32.Parse(dtTable.Rows[iRowIdx]["MONOMONTH"].ToString());
            iColor = Int32.Parse(dtTable.Rows[iRowIdx]["COLORMONTH"].ToString());
            iMicr = Int32.Parse(dtTable.Rows[iRowIdx]["MICRMONTH"].ToString());
            if (iMono > 0)
                saMonoColorMicr[0] = "Y";
            if (iColor > 0)
                saMonoColorMicr[1] = "Y";
            if (iMicr > 0)
                saMonoColorMicr[2] = "Y";
            iRowIdx++;
        }
        return saMonoColorMicr;
    }
    // =========================================================================
    protected void PagesByTypeMono(DataTable dtTable)
    {
        int iDateHold = 0;
        int iMonoMonth = 0;
        int iRowIdx = 0;
        int iPageTotal = 0;

        string[] xValues = new string[dtTable.Rows.Count]; // X Values can be text (for labels)

        int[] yValuesMono = new int[dtTable.Rows.Count];       // Y Values must be numeric (for values)

        int iMonth = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            iDateHold = Int32.Parse(dtTable.Rows[iRowIdx]["INVDATE"].ToString());
            iMonoMonth = Int32.Parse(dtTable.Rows[iRowIdx]["MONOMONTH"].ToString());
            iPageTotal += iMonoMonth;
            iMonth = Int32.Parse((iDateHold.ToString()).Substring(4, 2));
            xValues[iRowIdx] = sAryMonthAbbrev[iMonth];
            yValuesMono[iRowIdx] = iMonoMonth;
            iRowIdx++;
        }

        chartByPagesMono.Series["SeriesMono"].Points.DataBindXY(xValues, yValuesMono);
        chartByPagesMono.Series["SeriesMono"]["PointWidth"] = "0.5";
        chartByPagesMono.Titles[1].Text = "Period Total: " + String.Format("{0:#,###}", iPageTotal);

        // chartByPagesMono.Series["SeriesMono"]["ValueLabel"] = "Outside";
        //chartByPagesMono.ChartAreas["ChartArea1"].AxisX.Interval = 1; 

    }
    // =========================================================================
    protected void PagesByTypeColor(DataTable dtTable)
    {
        int iDateHold = 0;
        int iColorMonth = 0;
        int iRowIdx = 0;
        int iPageTotal = 0;

        string[] xValues = new string[dtTable.Rows.Count]; // X Values can be text (for labels)
        int[] yValuesColor = new int[dtTable.Rows.Count];       // Y Values must be numeric (for values)
        int iMonth = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            iDateHold = Int32.Parse(dtTable.Rows[iRowIdx]["INVDATE"].ToString());
            iColorMonth = Int32.Parse(dtTable.Rows[iRowIdx]["COLORMONTH"].ToString());
            iPageTotal += iColorMonth;
            iMonth = Int32.Parse((iDateHold.ToString()).Substring(4, 2));
            xValues[iRowIdx] = sAryMonthAbbrev[iMonth];
            yValuesColor[iRowIdx] = iColorMonth;

            iRowIdx++;
        }

        chartByPagesColor.Series["SeriesColor"].Points.DataBindXY(xValues, yValuesColor);
        chartByPagesColor.Series["SeriesColor"]["PointWidth"] = "0.5";
        chartByPagesColor.Titles[1].Text = "Period Total: " + String.Format("{0:#,###}", iPageTotal);
    }
    // =========================================================================
    protected void PagesByTypeMicr(DataTable dtTable)
    {
        int iDateHold = 0;
        int iMicrMonth = 0;
        int iRowIdx = 0;
        int iPageTotal = 0;

        string[] xValues = new string[dtTable.Rows.Count]; // X Values can be text (for labels)

        int[] yValuesMicr = new int[dtTable.Rows.Count];       // Y Values must be numeric (for values)

        int iMonth = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            iDateHold = Int32.Parse(dtTable.Rows[iRowIdx]["INVDATE"].ToString());
            iMicrMonth = Int32.Parse(dtTable.Rows[iRowIdx]["MICRMONTH"].ToString());
            iPageTotal += iMicrMonth;
            iMonth = Int32.Parse((iDateHold.ToString()).Substring(4, 2));
            xValues[iRowIdx] = sAryMonthAbbrev[iMonth];
            yValuesMicr[iRowIdx] = iMicrMonth;

            iRowIdx++;
        }

        chartByPagesMicr.Series["SeriesMicr"].Points.DataBindXY(xValues, yValuesMicr);
        chartByPagesMicr.Series["SeriesMicr"]["PointWidth"] = "0.5";
        chartByPagesMicr.Titles[1].Text = "Period Total: " + String.Format("{0:#,###}", iPageTotal);

    }

    // =========================================================
    // =========================================================
}