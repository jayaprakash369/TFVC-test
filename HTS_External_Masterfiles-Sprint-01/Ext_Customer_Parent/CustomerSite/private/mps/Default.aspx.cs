using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class private_mps_Default : MyPage
{
    // ========================================================================
    // Connect to Web Services  ------------------------------------
    ManagedPrint_LIVE.ManagedPrintMenuSoapClient wsLiveMps = new ManagedPrint_LIVE.ManagedPrintMenuSoapClient();
    ManagedPrint_DEV.ManagedPrintMenuSoapClient wsTestMps = new ManagedPrint_DEV.ManagedPrintMenuSoapClient();
    // X is NAME
    // Y is NUMBER
    protected System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#406080");
    protected System.Drawing.Color myRed = System.Drawing.ColorTranslator.FromHtml("#AD0034");
    protected Document doc;
   // -----------------------------
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set Javascript client side actions here
        ddCustomer.Attributes.Add("onChange", "return clearOtherDropDown('ddCustomer')");
        ddAgreement.Attributes.Add("onChange", "return clearOtherDropDown('ddAgreement')");
        buttonLoad.Attributes.Add("onClick", "return doClientVal()");

        if (!IsPostBack)
        {
            panelHeader.Visible = false;
            panelFleet.Visible = false;
            panelService.Visible = false;
            panelHidden.Visible = false;
            buttonPDF.Visible = false;
                
            // 2) Call Web Service
            DataTable dataTable = new DataTable();

            // Load customer name Drop Down
            if (sDevTestLive == "LIVE")
                dataTable = wsLiveMps.getNameNumForAllCustomers();
            else
                dataTable = wsTestMps.getNameNumForAllCustomers();

            ddCustomer.DataSource = dataTable;
            ddCustomer.DataValueField = "CSTRNR";
            ddCustomer.DataTextField = "NAME_CS1";
            ddCustomer.DataBind();
            ddCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));

            // Load agreement Drop Down
            if (sDevTestLive == "LIVE")
                dataTable = wsLiveMps.getNameNumAgrForAllCustomers();
            else
                dataTable = wsTestMps.getNameNumAgrForAllCustomers();
            ddAgreement.DataSource = dataTable;
            ddAgreement.DataValueField = "CONTNR";
            ddAgreement.DataTextField = "NAME_AGR";
            ddAgreement.DataBind();
            ddAgreement.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            
            // Load Start Date Drop Down
            DateTime hDate = new DateTime();
            DateTime monthStart = new DateTime();
            DateTime monthEnd = new DateTime();
            int iNumDays = 0;
            int iDaysInMonth = 0;
            string sDateFmt = "";
            string sDateStart8 = "";
            string sDateEnd8 = "";
            double j = 0.0;

            int i = 0;
            //ddDateStart.Items.Insert(0, new ListItem("", "0"));
            //ddDateEnd.Items.Insert(0, new ListItem("", "0"));

            hDate = DateTime.Now;
            for (i = 0; i < 24; i++)  // Do 24 months (start date = 1st of month, end date = last of month)
            {
                iNumDays = hDate.Day - 1; 
                monthStart = hDate.AddDays(-iNumDays);  // Get date of 1st of month
                iDaysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
                iNumDays = iDaysInMonth - 1;
                monthEnd = monthStart.AddDays(iNumDays);

                sDateFmt = monthStart.ToString("MMM yyyy");
                sDateStart8 = monthStart.ToString("yyyyMMdd");
                sDateEnd8 = monthEnd.ToString("yyyyMMdd");
                
                ddDateStart.Items.Insert(i, new System.Web.UI.WebControls.ListItem(sDateFmt, sDateStart8));
                ddDateEnd.Items.Insert(i, new System.Web.UI.WebControls.ListItem(sDateFmt, sDateEnd8));

                hDate = monthStart.AddDays(-1);
            }
            ddDateStart.Items[6].Selected = true;  // select date 6 months back
            ddDateEnd.Items[1].Selected = true;  
            
            // Load Service Requests
            for (i = 0; i < 10; i++) {
                ddRequests.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            ddRequests.Items[2].Selected = true;

            // Load Paper Jams
            for (i = 0; i < 30; i++) {
                ddPaperJams.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            ddPaperJams.Items[10].Selected = true;

            j = 0;
            // Load Utilization: Low
            for (i = 0; i < 31; i++) {
                ddUseLow.Items.Insert(i, new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
                j = Math.Round((j + .1), 2);
            }
            ddUseLow.Items[10].Selected = true;

            // Load Utilization: High
            j = 4;
            for (i = 0; i < 41; i++) {
                ddUseHigh.Items.Insert(i, new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
                j = Math.Round((j + .1), 2);
            }
            ddUseHigh.Items[20].Selected = true;

        }
    }

    // =========================================================================
    protected void buttonLoad_Click(object sender, EventArgs e)
    {

        DataTable dataTable = new DataTable();
        string sHighLow = "";
        string sAgr = ddAgreement.SelectedItem.Value.ToString();
        int iCs1 = Int32.Parse(ddCustomer.SelectedItem.Value.ToString());
        int iStart = Int32.Parse(ddDateStart.SelectedItem.Value.ToString());
        int iEnd = Int32.Parse(ddDateEnd.SelectedItem.Value.ToString());
        double dUseLow = double.Parse(ddUseLow.SelectedItem.Value.ToString());
        double dUseHigh = double.Parse(ddUseHigh.SelectedItem.Value.ToString());
        //int iRequestThreshold = Int32.Parse(ddRequests.SelectedItem.Value.ToString());
        //int iPaperJamThreshold = Int32.Parse(ddPaperJams.SelectedItem.Value.ToString());
        //ddUseHigh.Items[6].Selected = true;

        if (sAgr.Length > 8) { sAgr = sAgr.Substring(1, 8); }

        // Customer Detail
        string[] sAryDetail = new string[10];
        if (sDevTestLive == "LIVE")
            sAryDetail = wsLiveMps.getDetailForOneCustomer(iCs1, sAgr);
        else
            sAryDetail = wsTestMps.getDetailForOneCustomer(iCs1, sAgr);
        HeaderDetail(sAryDetail);

        // Page Sums For Month For Customer
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getPageCountsForOneCustomer(iCs1, sAgr, iStart, iEnd);
        else
            dataTable = wsTestMps.getPageCountsForOneCustomer(iCs1, sAgr, iStart, iEnd);
        PagesByTypeSearch(dataTable);
//        PagesByTypeAll(dataTable);
        if (hfMonoFound.Value == "Y")
            PagesByTypeMono(dataTable);
        else
            chartByPagesMono.Visible = false;
        if (hfColorFound.Value == "Y")
            PagesByTypeColor(dataTable);
        else
            chartByPagesColor.Visible = false;
        if (hfMicrFound.Value == "Y")
            PagesByTypeMicr(dataTable);
        else
            chartByPagesMicr.Visible = false;


        // Devices By Type
        int[] iAryDeviceTotals = new int[3];
        if (sDevTestLive == "LIVE")
            iAryDeviceTotals = wsLiveMps.getTypeForAllDevices(iCs1, sAgr);
        else
            iAryDeviceTotals = wsTestMps.getTypeForAllDevices(iCs1, sAgr);
        DevicesByType(iAryDeviceTotals);

        // Devices By Manufacturer
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getManufacturerForAllDevices(iCs1, sAgr);
        else
            dataTable = wsTestMps.getManufacturerForAllDevices(iCs1, sAgr);
        DevicesByManufacturer(dataTable);

        // Devices By Model

        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getModelForAllDevices(iCs1, sAgr);
        else
            dataTable = wsTestMps.getModelForAllDevices(iCs1, sAgr);
        DevicesByModel(dataTable);

        // Devices By Manageability: Count
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getCountReportingForAllDevices(iCs1, sAgr);
        else
            dataTable = wsTestMps.getCountReportingForAllDevices(iCs1, sAgr);

        DevicesByManageabilityCount(dataTable);
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getTonerReportingForAllDevices(iCs1, sAgr);
        else
            dataTable = wsTestMps.getTonerReportingForAllDevices(iCs1, sAgr);
        DevicesByManageabilityToner(dataTable);

        // Devices By Manageability: (DOUBLE PIE CHART EXAMPLE) 
        //dataTable = mpsWs.DevicesByManageabilityCount_Read(iCs1, sAgr);
        //DataTable dataTable2 = new DataTable();
        //dataTable2 = mpsWs.DevicesByManageabilityToner_Read(iCs1, sAgr);
        //DevicesByManageability(dataTable, dataTable2);

        // Devices By Utilization: High
        sHighLow = "HIGH";
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getUtilizationForAllDevices(iCs1, sAgr, iStart, iEnd, dUseHigh, sHighLow);
        else
            dataTable = wsTestMps.getUtilizationForAllDevices(iCs1, sAgr, iStart, iEnd, dUseHigh, sHighLow);
        DevicesByUse(dataTable, sHighLow, dUseHigh);

        // Devices By Utilization: Low
        sHighLow = "LOW";
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getUtilizationForAllDevices(iCs1, sAgr, iStart, iEnd, dUseLow, sHighLow);
        else
            dataTable = wsTestMps.getUtilizationForAllDevices(iCs1, sAgr, iStart, iEnd, dUseLow, sHighLow);
        DevicesByUse(dataTable, sHighLow, dUseLow);
        
        string sSortBy = ddChildSort.SelectedValue;

        // Devices: All Detail
        if (sDevTestLive == "LIVE")
        {
            dataTable = wsLiveMps.getDetailForAllDevices(iCs1, sAgr, iStart, iEnd, sSortBy);
        }
        else 
        {
            dataTable = wsTestMps.getDetailForAllDevices(iCs1, sAgr, iStart, iEnd, sSortBy);
        }
        DeviceDetail(dataTable);

        // Requests By Category
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getCloseCategoryForAllRequests(iCs1, sAgr, iStart, iEnd);
        else
            dataTable = wsTestMps.getCloseCategoryForAllRequests(iCs1, sAgr, iStart, iEnd);
        RequestsByCategoryClose(dataTable);

        // ----------------
        panelHeader.Visible = true;
        panelFleet.Visible = true;
        panelService.Visible = true;
        buttonPDF.Visible = true;
        SaveImages();        

    }

    // =========================================================================
    protected void buttonPDF_Click(object sender, EventArgs e)
    {
        BuildPDF();
    }

    // =========================================================================
    protected void DevicesByType(int[] iAryQty)
    {
        int i = 0;
        int iNonZeroUnits = 0;
        int iTotalUnits = 0;

        for (i = 0; i < iAryQty.Length; i++)
        {
            if (iAryQty[i] > 0)
            {
                iNonZeroUnits += 1;
                iTotalUnits += iAryQty[i];
            }
        }

        string[] xValues = new string[iNonZeroUnits];
        int[] yValues = new int[iNonZeroUnits];
        int iRowIdx = 0;
        string[] sAryLabels = new string[3];

        if (iTotalUnits > 0)
        {
            for (i = 0; i < iAryQty.Length; i++)
            {
                if (iAryQty[i] > 0)
                {

                    //yValues[iAryIdx] = Convert.ToInt32((100 * (Convert.ToDouble(iAryQty[i]) / Convert.ToDouble(iTotalUnits)))); // example of calculating the percent
                    yValues[iRowIdx] = iAryQty[i];
                    xValues[iRowIdx] = ""; // If you load labels here, and use the auto percent calc, your text will turn back into percent based on the number
                    iRowIdx++;
                }
            }
        }
        chartByType.Series[0].Points.DataBindXY(xValues, yValues);       

        chartByType.Series[0]["PieLabelStyle"] = "Outside";
        chartByType.Series[0]["PieDrawingStyle"] = "SoftEdge";
        //chartByType.Series[0]["PieDrawingStyle"] = "Concave";
        //chartByType.Series[0].ChartType = SeriesChartType.Doughnut;
        //chartByType.Series[0]["DoughnutRadius"] = "80";
        //chartByType.Titles[1].Text = "Total Devices: " + iTotalUnits.ToString();
        chartByType.Series[0].Label = "#PERCENT{P0}";

        // Now load customized legend Text...
        chartByType.Legends[0].Enabled = true;
        sAryLabels[0] = "Mono: ";
        sAryLabels[1] = "Color: ";
        sAryLabels[2] = "Micr: ";
        iRowIdx = 0;
        for (i = 0; i < iAryQty.Length; i++) 
        { 
            if (iAryQty[i] > 0) 
            {
                chartByType.Series[0].Points[iRowIdx].LegendText = sAryLabels[i] + iAryQty[i];        
                iRowIdx++;
            }
        }

    }
    // =========================================================================
    protected void DevicesByModel(DataTable dtTable)
    {

        int iRowIdx = 0;
        int iPartQty = 0;
        int iExtraRows = 0;
        string sPartName = "";
        string[] xValues = new string[dtTable.Rows.Count];
        int[] yValues = new int[dtTable.Rows.Count];
      
        if (dtTable.Rows.Count > 18) 
        {
            iExtraRows = dtTable.Rows.Count - 18;
            chartByModel.Height = Convert.ToInt32(chartByModel.Height.Value + (iExtraRows * 23));
        }

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            xValues[iRowIdx] = ""; // If you load labels here, and use the auto percent calc, your text will turn back into percent based on the number
            yValues[iRowIdx] = Int32.Parse(dtTable.Rows[iRowIdx]["PARTQTY"].ToString().Trim());
            iRowIdx++;
        }

        chartByModel.Series["Series1"].Points.DataBindXY(xValues, yValues);

        chartByModel.Series["Series1"]["PieLabelStyle"] = "Outside";
        chartByModel.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
        chartByModel.Series["Series1"]["LabelsRadialLineSize"] = "1";
        chartByModel.Series[0].Label = "#PERCENT{P0}"; // P0

        chartByModel.Legends[0].Enabled = true;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            sPartName = dtTable.Rows[iRowIdx]["PARTNAME"].ToString().Trim();
            iPartQty = Int32.Parse(dtTable.Rows[iRowIdx]["PARTQTY"].ToString());
            chartByModel.Series[0].Points[iRowIdx].LegendText = sPartName + ": " + iPartQty;
            iRowIdx++;
        }
    }

    // =========================================================================
    protected void DevicesByManufacturer(DataTable dtTable)
    {

        string sColName = "";
        string sFieldValue = "";
        int iRowIdx = 0;
        string[] xValues = new string[dtTable.Rows.Count];
        int[] yValues = new int[dtTable.Rows.Count];

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("VENDORNAME"))
                {
                    xValues[iRowIdx] = sFieldValue.Trim();
                }
                if (sColName.Equals("VENDORQTY"))
                {
                    yValues[iRowIdx] = Int32.Parse(sFieldValue.Trim());
                }
            }
            iRowIdx++;
        }

        chartByManufacturer.Series["Series1"].Points.DataBindXY(xValues, yValues);
        //chartByManufacturer.Series["Series1"].ChartType = SeriesChartType.Pie;  // Doughnut
        //chartByManufacturer.Series["Series1"]["DoughnutRadius"] = "80";
        chartByManufacturer.Series["Series1"]["PieLabelStyle"] = "Outside";
        chartByManufacturer.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
        chartByManufacturer.Series["Series1"]["LabelsRadialLineSize"] = "1";
        //chartByManufacturer.Series["Series1"]["Label"] = "#PERCENT{P1}";
        chartByManufacturer.Series[0].Label = "#PERCENT{P0}"; // P0
        //chartByManufacturer.Series[0].Label = "#PERCENT"; // P0
        //chartByManufacturer.Series[0].LabelFormat = "{0:0}"; 
        chartByManufacturer.Legends[0].Enabled = true;
        //chartByManufacturer.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true; // Draw chart as 3D   
        // chartByManufacturer.Series["Series1"]["DrawingStyle"] = "Cylinder";  // don't see this working...


        string sVendorName = "";
        int iVendorQty = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("VENDORNAME"))
                {
                    sVendorName = sFieldValue.Trim();
                }
                if (sColName.Equals("VENDORQTY"))
                {
                    iVendorQty = Int32.Parse(sFieldValue.Trim());
                    chartByManufacturer.Series[0].Points[iRowIdx].LegendText = sVendorName + ": " + sFieldValue.Trim();
                }
            }
            iRowIdx++;
        }
    }
    // =========================================================================
    protected void DevicesByManageabilityCount(DataTable dtTable)
    {

        string sColName = "";
        string sFieldValue = "";
        int iRowIdx = 0;
        string[] xValues = new string[dtTable.Rows.Count];
        int[] yValues = new int[dtTable.Rows.Count];

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("COUNTINGSTS"))
                {
                    xValues[iRowIdx] = sFieldValue.Trim();
                }
                if (sColName.Equals("COUNTINGQTY"))
                {
                    yValues[iRowIdx] = Int32.Parse(sFieldValue.Trim());
                }
            }
            iRowIdx++;
        }

        chartByManageabilityCount.Series["SeriesCount"].Points.DataBindXY(xValues, yValues);
        chartByManageabilityCount.Series["SeriesCount"]["PieLabelStyle"] = "Outside";
        chartByManageabilityCount.Series["SeriesCount"]["PieDrawingStyle"] = "SoftEdge";
        chartByManageabilityCount.Series["SeriesCount"]["LabelsRadialLineSize"] = "1";
        chartByManageabilityCount.Series["SeriesCount"].Label = "#PERCENT{P0}"; // P0

        chartByManageabilityCount.Legends["LegendCount"].Enabled = true;

        string sCountingSts = "";
        int iCountingQty = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("COUNTINGSTS"))
                {
                    sCountingSts = sFieldValue.Trim();
                    if (sCountingSts.Length < 1)
                    {
                        sCountingSts = "";
                    }
                    else if (sCountingSts == "Y")
                    {
                        sCountingSts = "Yes";
                    }
                    else if (sCountingSts == "N")
                    {
                        sCountingSts = "No";
                    }
                    else if (sCountingSts == "U")
                    {
                        sCountingSts = "Unknown";
                    }
                    else if (sCountingSts == "L")
                    {
                        sCountingSts = "Life, No Color";
                    }
                    else if (sCountingSts == "C")
                    {
                        sCountingSts = "Color, No Life";
                    }
                    else if (sCountingSts == "F")
                    {
                        sCountingSts = "Firmware";
                    }

                }
                if (sColName.Equals("COUNTINGQTY"))
                {
                    iCountingQty = Int32.Parse(sFieldValue.Trim());
                    chartByManageabilityCount.Series["SeriesCount"].Points[iRowIdx].LegendText = sCountingSts + ": " + iCountingQty.ToString();
                }
            }
            iRowIdx++;
        }
        // ----------------------------------
    }
    // =========================================================================
    protected void DevicesByManageabilityToner(DataTable dtTable)
    {
        string sColName = "";
        string sFieldValue = "";
        int iRowIdx = 0;
        string[] xValues = new string[dtTable.Rows.Count];
        int[] yValues = new int[dtTable.Rows.Count];

        xValues = new string[dtTable.Rows.Count];
        yValues = new int[dtTable.Rows.Count];

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("TONERSTS"))
                {
                    xValues[iRowIdx] = sFieldValue.Trim();
                }
                if (sColName.Equals("TONERQTY"))
                {
                    yValues[iRowIdx] = Int32.Parse(sFieldValue.Trim());
                }
            }
            iRowIdx++;
        }

        chartByManageabilityToner.Series["SeriesToner"].Points.DataBindXY(xValues, yValues);
        chartByManageabilityToner.Series["SeriesToner"]["PieLabelStyle"] = "Outside";
        chartByManageabilityToner.Series["SeriesToner"]["PieDrawingStyle"] = "SoftEdge";
        chartByManageabilityToner.Series["SeriesToner"]["LabelsRadialLineSize"] = "1";
        chartByManageabilityToner.Series["SeriesToner"].Label = "#PERCENT{P0}"; // P0

        chartByManageabilityToner.Legends["LegendToner"].Enabled = true;


        string sTonerSts = "";
        int iTonerQty = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("TONERSTS"))
                {
                    sTonerSts = sFieldValue.Trim();
                    if (sTonerSts.Length < 1)
                    {
                        sTonerSts = "";
                    }
                    else if (sTonerSts == "Y")
                    {
                        sTonerSts = "Yes";
                    }
                    else if (sTonerSts == "N")
                    {
                        sTonerSts = "No";
                    }
                    else if (sTonerSts == "U")
                    {
                        sTonerSts = "Unknown";
                    }
                    else if (sTonerSts == "L")
                    {
                        sTonerSts = "Life, No Color";
                    }
                    else if (sTonerSts == "C")
                    {
                        sTonerSts = "Color, No Life";
                    }
                    else if (sTonerSts == "F")
                    {
                        sTonerSts = "Firmware";
                    }
                }
                if (sColName.Equals("TONERQTY"))
                {
                    iTonerQty = Int32.Parse(sFieldValue.Trim());
                    chartByManageabilityToner.Series["SeriesToner"].Points[iRowIdx].LegendText = sTonerSts + ": " + iTonerQty.ToString();
                }
            }
            iRowIdx++;
        }
        // ----------------------------------
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
        int iMaxRows = 10;

        if (dtTable.Rows.Count < 10)
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
                        utilization = Math.Round(100*double.Parse(sFieldValue.Trim()),2);
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
            chartByUseHigh.Height = 65 + (iMaxRows*2) + (iMaxRows * 35);
            chartByUseHigh.Legends[0].Enabled = false;
        }
        else
        {
            chartByUseLow.Titles["Title1"].Text = "Devices By Utilization (Below " + cutoff.ToString() + "%)";
            chartByUseLow.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chartByUseLow.Series["Series1"].IsValueShownAsLabel = true;
            chartByUseLow.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;  // makes labels line up with bars
            chartByUseLow.Height = 65 + (iMaxRows * 35);
            chartByUseLow.Legends[0].Enabled = false;
        }
    // -------------------------        
    }

    // =========================================================================
    protected void HeaderDetail(string[] sAryCust)
    {
//        System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#406080");
//        System.Drawing.Color myRed = System.Drawing.ColorTranslator.FromHtml("#AD0034");

        lbCustNum.Text = sAryCust[1];
        lbCustName.Text = sAryCust[0];
        lbCustName.ForeColor = myRed;
        lbCustName.Font.Bold = true;
        lbAddress.Text = sAryCust[2];
        String sAddress2 = sAryCust[3];
        if (sAddress2.Length > 0)
        {
            lbAddress.Text += ", " + sAryCust[3];
        }
        lbCityStateZip.Text = sAryCust[4] + ", " + sAryCust[5] + " " + sAryCust[6];
        lbAgrLocations.Text = sAryCust[7];
        lbAgrDevices.Text = sAryCust[8];
        string sAgr = sAryCust[9];
        char[] separator = new char[] { '|' };
        string[] sAryAgr = sAgr.Split(separator);
        int i = 0;
        for (i = 0; i < sAryAgr.Length; i++)
        {
            if (i > 0)
            {
                lbAgreements.Text += ", " + sAryAgr[i];
            }
            else
            {
                lbAgreements.Text = sAryAgr[0];
            }
        }
    }
    // =========================================================================
    protected void RequestsByCategoryClose(DataTable dtTable)
    {
        string sColName = "";
        string sFieldValue = "";
        int iRowIdx = 0;
        string[] xValues = new string[dtTable.Rows.Count];
        int[] yValues = new int[dtTable.Rows.Count];

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("CATEGORYCODE"))
                {
                    xValues[iRowIdx] = sFieldValue.Trim();
                }
                if (sColName.Equals("CATEGORYQTY"))
                {
                    yValues[iRowIdx] = Int32.Parse(sFieldValue.Trim());
                }
            }
            iRowIdx++;
        }


        chartByCategory.Series["Series1"].Points.DataBindXY(xValues, yValues);
        //chartByCategory.Series["Series1"].ChartType = SeriesChartType.Pie;  // Doughnut
        //chartByCategory.Series["Series1"]["DoughnutRadius"] = "80";
        chartByCategory.Series["Series1"]["PieLabelStyle"] = "Outside";
        chartByCategory.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
        chartByCategory.Series["Series1"]["LabelsRadialLineSize"] = "1";
        //chartByManufacturer.Series["Series1"]["Label"] = "#PERCENT{P1}";
        chartByCategory.Series[0].Label = "#PERCENT{P0}"; // P0
        //chartByManufacturer.Series[0].Label = "#PERCENT"; // P0
        //chartByManufacturer.Series[0].LabelFormat = "{0:0}"; 
        chartByCategory.Legends[0].Enabled = true;

        string sCategoryCode = "";
        int iCategoryQty = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sColName = col.ToString();
                sFieldValue = row[col].ToString();
                if (sColName.Equals("CATEGORYCODE"))
                {
                    if (sFieldValue == "Toner Replenish")
                        sFieldValue = "Toner Replenishment";
                    sCategoryCode = sFieldValue.Trim();
                }
                if (sColName.Equals("CATEGORYQTY"))
                {
                    iCategoryQty = Int32.Parse(sFieldValue.Trim());
                    chartByCategory.Series[0].Points[iRowIdx].LegendText = sCategoryCode + ": " + sFieldValue.Trim();
                }
            }
            iRowIdx++;
        }
    }
    // =========================================================================
    protected void DeviceDetail(DataTable dtTable)
    {
        int iRowIdx = 0;
        int iRowColor = 0;
        int iLocHold = 9999; // different that any possible location
        TableHeaderRow thRow;
        TableRow tRow;
        TableHeaderCell thCell;
        TableCell tCell;

        System.Drawing.Color myGray = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
        System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#406080");
        System.Drawing.Color myLightBlue = System.Drawing.ColorTranslator.FromHtml("#C5D4E2"); // 7194B7
        System.Drawing.Color myOldLace = System.Drawing.ColorTranslator.FromHtml("#FDF5E6");
        System.Drawing.Color myYellow = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
        System.Drawing.Color myLightGreen = System.Drawing.ColorTranslator.FromHtml("#B4D9C7");


        foreach (DataRow row in dtTable.Rows)
        {
            int iUnit = Int32.Parse(dtTable.Rows[iRowIdx]["CESYS#"].ToString());
            int iCustNum = Int32.Parse(dtTable.Rows[iRowIdx]["CERNR"].ToString());
            int iLocNum = Int32.Parse(dtTable.Rows[iRowIdx]["CERCD"].ToString());
            string sLocName = dtTable.Rows[iRowIdx]["CUSTNM"].ToString().Trim();
            string sLocAdr1 = dtTable.Rows[iRowIdx]["SADDR1"].ToString().Trim();
            string sLocAdr2 = dtTable.Rows[iRowIdx]["SADDR2"].ToString().Trim();
            string sLocCity = dtTable.Rows[iRowIdx]["CITY"].ToString().Trim();
            string sLocState = dtTable.Rows[iRowIdx]["STATE"].ToString().Trim();
            string sLocZip = dtTable.Rows[iRowIdx]["ZIPCD"].ToString().Trim();
            string sAgr = dtTable.Rows[iRowIdx]["ECCNTR"].ToString().Trim();
            string sFxa = dtTable.Rows[iRowIdx]["CEFAA"].ToString().Trim();
            string sPrt = dtTable.Rows[iRowIdx]["CEPRT#"].ToString().Trim();
            string sSer = dtTable.Rows[iRowIdx]["CESER#"].ToString().Trim();
            string sMod = dtTable.Rows[iRowIdx]["PEMOD"].ToString().Trim();
            string sSalesName = dtTable.Rows[iRowIdx]["P#PRTS"].ToString().Trim();
            string sVenName = dtTable.Rows[iRowIdx]["VENDR1"].ToString().Trim();
            string sSub = dtTable.Rows[iRowIdx]["PESUBT"].ToString().Trim();
            int iMfrMax = Int32.Parse(dtTable.Rows[iRowIdx]["PEDUTY"].ToString());
            string sSendsToner = dtTable.Rows[iRowIdx]["PETNRL"].ToString().Trim();
            string sSendsCount = dtTable.Rows[iRowIdx]["PEPGEC"].ToString().Trim();
            string sCustCrossRef = dtTable.Rows[iRowIdx]["XREFCS"].ToString().Trim();
            int iMonoPgs = Int32.Parse(dtTable.Rows[iRowIdx]["MONOPAGES"].ToString());
            int iColorPgs = Int32.Parse(dtTable.Rows[iRowIdx]["COLORPAGES"].ToString());
            int iTotalPgs = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALPAGES"].ToString());
            double dUtilization = double.Parse(dtTable.Rows[iRowIdx]["UTILIZATION"].ToString());
            //int iRequests = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALREQUESTS"].ToString());
            int iServiceTickets = Int32.Parse(dtTable.Rows[iRowIdx]["SERVICETICKETS"].ToString());
            int iTonerTickets = Int32.Parse(dtTable.Rows[iRowIdx]["TONERTICKETS"].ToString());
            int iOtherTickets = Int32.Parse(dtTable.Rows[iRowIdx]["OTHERTICKETS"].ToString());
            //int iSurveyTickets = Int32.Parse(dtTable.Rows[iRowIdx]["SURVEYTICKETS"].ToString());
            //int iCountTickets = Int32.Parse(dtTable.Rows[iRowIdx]["COUNTTICKETS"].ToString());
            int iJams = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALJAMS"].ToString());
            int iLifeCount = Int32.Parse(dtTable.Rows[iRowIdx]["LIFECOUNT"].ToString());

            // -------------------------
            if (iLocNum != iLocHold)
            {
                iLocHold = iLocNum;
                iRowColor = 0;

                if (iLocNum > 0)
                {
                    // New Divider Row
                    tRow = new TableHeaderRow();
                    tbDevices.Rows.Add(tRow);

                    tCell = new TableHeaderCell();
                    tCell.ColumnSpan = 15;
                    tCell.Height = 20;
                    tCell.BackColor = System.Drawing.Color.White;
                    tRow.Cells.Add(tCell);
                }
                // Header Location Detail
                thRow = new TableHeaderRow();
                tbDevices.Rows.Add(thRow);

                thCell = new TableHeaderCell();
                thCell.ColumnSpan = 1;
                thCell.Text = "Location " + iLocNum.ToString();
                thCell.BackColor = System.Drawing.Color.White;
                thCell.ForeColor = myBlue;
                thCell.Font.Size = 13;
                thCell.Font.Bold = false;
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.ColumnSpan = 1;
                thCell.Text = "Cross Ref<br />" + sCustCrossRef.Trim();
                thCell.ForeColor = myBlue;
                thCell.Font.Size = 10;
                thCell.Font.Bold = false;
                thCell.BackColor = System.Drawing.Color.White;
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.ColumnSpan = 14;
                thCell.Text = sLocName + "<br />" + sLocAdr1 + " " + sLocAdr2 + " " + sLocCity + ", " + sLocState + " " + sLocZip.Substring(0, 5);
                thCell.HorizontalAlign = HorizontalAlign.Left;
                thCell.BackColor = System.Drawing.Color.White;
                thCell.ForeColor = myBlue;
                thRow.Cells.Add(thCell);

                // Header Categories
                thRow = new TableHeaderRow();
                tbDevices.Rows.Add(thRow);

                thCell = new TableHeaderCell();
                thCell.Text = "Part";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Serial";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Fixed Asset";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Unit";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Subt";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Pg";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Tn";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Duty";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "AvM";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "AvC";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Util";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Life";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "SvT";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "TnT";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "OtT";
                thRow.Cells.Add(thCell);

                thCell = new TableHeaderCell();
                thCell.Text = "Jam";
                thRow.Cells.Add(thCell);
            }

            tRow = new TableRow();
            if (iRowColor == 0)
            {
                iRowColor = 1;
                tRow.BackColor = myGray;
            }
            else
            {
                iRowColor = 0;
                tRow.BackColor = System.Drawing.Color.White;
            }

            tbDevices.Rows.Add(tRow);

            tCell = new TableCell();
            tCell.Text = sPrt;
            if ((sSub == "PLMC") || (sSub == "PLSC"))
            {
                tCell.BackColor = System.Drawing.Color.Thistle;

            }
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSer;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sFxa;
            if (sFxa.Length > 0) 
            {
                if (sFxa.Substring(0, 1) == "%")
                {
                    tCell.BackColor = System.Drawing.Color.MistyRose;
                }
            }
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iUnit.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSub;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSendsCount;
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSendsToner;
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iMfrMax.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iMonoPgs.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iColorPgs.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = dUtilization.ToString();
            double dHighUse = double.Parse(ddUseHigh.SelectedItem.Value.ToString());
            if (dUtilization >= dHighUse)
            {
                tCell.BackColor = System.Drawing.Color.Khaki;
            }
            double dLowUse = double.Parse(ddUseLow.SelectedItem.Value.ToString());
            if (dUtilization <= dLowUse)
            {
                tCell.BackColor = System.Drawing.Color.LightBlue;
            }

            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iLifeCount.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iServiceTickets.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            int iRequestThreshold = Int32.Parse(ddRequests.SelectedItem.Value.ToString());
            if (iServiceTickets >= iRequestThreshold)
            {
                tCell.BackColor = System.Drawing.Color.BurlyWood;
            }
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iTonerTickets.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iOtherTickets.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iJams.ToString();
            tCell.HorizontalAlign = HorizontalAlign.Center;
            int iJamThreshold = Int32.Parse(ddPaperJams.SelectedItem.Value.ToString());
            if (iJams >= iJamThreshold)
            {
                tCell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }
            tRow.Cells.Add(tCell);

            // -------------------------
            iRowIdx++;
        }
    }

    // =========================================================================
    protected void DeviceDetail2(DataTable dtTable)
    {
        int iRowIdx = 0;
        int iLocHold = 9999; // different that any possible location

        //iTextSharp.text.pdf.PdfPTable table2 = new iTextSharp.text.pdf.PdfPTable(1);
        iTextSharp.text.pdf.PdfPTable table;
        int [] intArray;
        intArray = new int[5]; 

        PdfPCell cell;
        Phrase phrase;
        iTextSharp.text.Color cRed = new iTextSharp.text.Color(173, 0, 52);
        iTextSharp.text.Color cBlue = new iTextSharp.text.Color(64, 96, 128);
        iTextSharp.text.Color cKhaki = new iTextSharp.text.Color(240, 230, 140);
        iTextSharp.text.Color cLemonChiffon = new iTextSharp.text.Color(255, 250, 205);
        iTextSharp.text.Color cDarkSeaGreen = new iTextSharp.text.Color(143, 188, 132);
        iTextSharp.text.Color cPowderBlue = new iTextSharp.text.Color(176, 224, 230);
        iTextSharp.text.Color cThistle = new iTextSharp.text.Color(216, 191, 216);
        iTextSharp.text.Color cMistyRose = new iTextSharp.text.Color(255, 228, 255);
        
        iTextSharp.text.Color cLightPink = new iTextSharp.text.Color(255, 182, 193);
        iTextSharp.text.Color cBurlywood = new iTextSharp.text.Color(222, 184, 135);
        iTextSharp.text.Color cLightBlue = new iTextSharp.text.Color(173, 216, 230);

        // Color Legend Table -----------------------------------------------------------------

        table = new iTextSharp.text.pdf.PdfPTable(6);
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        table.SpacingAfter = 20f;
        float[] widths = new float[] { 10f, 10f, 10f, 10f, 10f, 10f };
        table.SetWidths(widths);

        // Color Table Header
        phrase = new Phrase("Highlighted Results");
        phrase.Font.Size = 8;
        phrase.Font.Color = iTextSharp.text.Color.WHITE;
        cell = new PdfPCell(phrase);
        cell.Colspan = 6;
        cell.BackgroundColor = cBlue;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);
        
        // Color Table Legend
        phrase = new Phrase("Color Device");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cThistle;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        phrase = new Phrase("Micr Device");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cMistyRose;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        phrase = new Phrase("Low Utilization");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cLightBlue;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        phrase = new Phrase("High Utilization");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cKhaki;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        phrase = new Phrase("High Service");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cBurlywood;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        phrase = new Phrase("High Jams");
        phrase.Font.Size = 7;
        cell = new PdfPCell(phrase);
        cell.BackgroundColor = cDarkSeaGreen;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        doc.Add(table);

        // -----------------------------------------------------------------

        foreach (DataRow row in dtTable.Rows)
        {
            int iUnit = Int32.Parse(dtTable.Rows[iRowIdx]["CESYS#"].ToString());
            int iCustNum = Int32.Parse(dtTable.Rows[iRowIdx]["CERNR"].ToString());
            int iLocNum = Int32.Parse(dtTable.Rows[iRowIdx]["CERCD"].ToString());
            string sLocName = dtTable.Rows[iRowIdx]["CUSTNM"].ToString().Trim();
            string sLocAdr1 = dtTable.Rows[iRowIdx]["SADDR1"].ToString().Trim();
            string sLocAdr2 = dtTable.Rows[iRowIdx]["SADDR2"].ToString().Trim();
            string sLocCity = dtTable.Rows[iRowIdx]["CITY"].ToString().Trim();
            string sLocState = dtTable.Rows[iRowIdx]["STATE"].ToString().Trim();
            string sLocZip = dtTable.Rows[iRowIdx]["ZIPCD"].ToString().Trim();
            string sAgr = dtTable.Rows[iRowIdx]["ECCNTR"].ToString().Trim();
            string sFxa = dtTable.Rows[iRowIdx]["CEFAA"].ToString().Trim();
            string sPrt = dtTable.Rows[iRowIdx]["CEPRT#"].ToString().Trim();
            string sSer = dtTable.Rows[iRowIdx]["CESER#"].ToString().Trim();
            string sMod = dtTable.Rows[iRowIdx]["PEMOD"].ToString().Trim();
            string sSalesName = dtTable.Rows[iRowIdx]["P#PRTS"].ToString().Trim();
            string sVenName = dtTable.Rows[iRowIdx]["VENDR1"].ToString().Trim();
            string sSub = dtTable.Rows[iRowIdx]["PESUBT"].ToString().Trim();
            int iMfrMax = Int32.Parse(dtTable.Rows[iRowIdx]["PEDUTY"].ToString());
            string sSendsToner = dtTable.Rows[iRowIdx]["PETNRL"].ToString().Trim();
            string sSendsCount = dtTable.Rows[iRowIdx]["PEPGEC"].ToString().Trim();
            int iMonoPgs = Int32.Parse(dtTable.Rows[iRowIdx]["MONOPAGES"].ToString());
            int iColorPgs = Int32.Parse(dtTable.Rows[iRowIdx]["COLORPAGES"].ToString());
            int iTotalPgs = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALPAGES"].ToString());
            double dUtilization = double.Parse(dtTable.Rows[iRowIdx]["UTILIZATION"].ToString());
            //int iRequests = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALREQUESTS"].ToString());
            int iServiceTickets = Int32.Parse(dtTable.Rows[iRowIdx]["SERVICETICKETS"].ToString());
            int iTonerTickets = Int32.Parse(dtTable.Rows[iRowIdx]["TONERTICKETS"].ToString());
            int iOtherTickets = Int32.Parse(dtTable.Rows[iRowIdx]["OTHERTICKETS"].ToString());
            int iJams = Int32.Parse(dtTable.Rows[iRowIdx]["TOTALJAMS"].ToString());
            int iLifeCount = Int32.Parse(dtTable.Rows[iRowIdx]["LIFECOUNT"].ToString());
            string sCustCrossRef = dtTable.Rows[iRowIdx]["XREFCS"].ToString().Trim();

            // -------------------------
            if (iLocNum != iLocHold) 
            {
                iLocHold = iLocNum;

                if (iLocNum > 0) 
                {
                    doc.Add(table);
                }
             // Header Location Detail

                // Start new table
                table = new iTextSharp.text.pdf.PdfPTable(16);  // 16 Columns 
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SpacingAfter = 30f;
                //relative col widths in proportions - 1/3 and 2/3
                widths = new float[] { 20f, 20f, 20f, 9f, 7f, 4f, 4f, 8f, 7f, 5f, 9f, 8f, 5f, 5f, 5f, 5f };
                table.SetWidths(widths);

                phrase = new Phrase("Location " + iLocNum.ToString());
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("XRef " + sCustCrossRef);
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase(sLocName + "   " + sLocAdr1 + " " + sLocAdr2 + " " + sLocCity + ", " + sLocState + " " + sLocZip.Substring(0, 5));
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.Colspan = 13;
                table.AddCell(cell);


             // Header Categories
                phrase = new Phrase("Part");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Serial");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Fixed Asset");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Unit");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Subt");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Pg");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Tn");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Duty");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("AvM");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("AvC");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Util");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Life");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("SvT");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("TnT");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("OtT");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                phrase = new Phrase("Jam");
                phrase.Font.Color = iTextSharp.text.Color.WHITE;
                phrase.Font.Size = 7;
                cell = new PdfPCell(phrase);
                cell.BackgroundColor = cBlue;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }


            phrase = new Phrase(sPrt);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            if ((sSub == "PLMC") || (sSub == "PLSC"))
            {
                cell.BackgroundColor = cThistle;
            }
            table.AddCell(cell);

            phrase = new Phrase(sSer);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            table.AddCell(cell);

            phrase = new Phrase(sFxa);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            if ((sFxa != "") && (sFxa.Substring(0,1) == "%"))
            {
                cell.BackgroundColor = cMistyRose;
            }
            table.AddCell(cell);

            phrase = new Phrase(iUnit.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(sSub);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(sSendsCount);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(sSendsToner);
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iMfrMax.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iMonoPgs.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iColorPgs.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(dUtilization.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;

            double dHighUse = double.Parse(ddUseHigh.SelectedItem.Value.ToString());
            if (dUtilization >= dHighUse)
            {
                cell.BackgroundColor = cKhaki;
            }
            double dLowUse = double.Parse(ddUseLow.SelectedItem.Value.ToString());
            if (dUtilization <= dLowUse)
            {
                cell.BackgroundColor = cLightBlue;
            }
            table.AddCell(cell);

            phrase = new Phrase(iLifeCount.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iServiceTickets.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            int iRequestThreshold = Int32.Parse(ddRequests.SelectedItem.Value.ToString());
            if (iServiceTickets >= iRequestThreshold)
            {
                cell.BackgroundColor = cBurlywood;
            }
            table.AddCell(cell);

            phrase = new Phrase(iTonerTickets.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iOtherTickets.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            phrase = new Phrase(iJams.ToString());
            phrase.Font.Size = 7;
            cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = 1;
            int iJamThreshold = Int32.Parse(ddPaperJams.SelectedItem.Value.ToString());
            if (iJams >= iJamThreshold) 
            {
                cell.BackgroundColor = cDarkSeaGreen;
            }
            table.AddCell(cell);

            // -------------------------
            iRowIdx++;
        }
        doc.Add(table);
    // Return array of tables and loop through to present...
    }
    // =========================================================================
    protected void SaveImages()
    {
        string sPath = "";
        string sRootPathImg = Server.MapPath("~") + "\\media\\scantron\\workfiles\\mp\\";

        try
        {
            //sPath = sRootPathImg + "ByPagesAll.bmp";
            //chartByPagesAll.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByPagesMono.bmp";
            chartByPagesMono.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByPagesColor.bmp";
            chartByPagesColor.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByPagesMicr.bmp";
            chartByPagesMicr.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByType.bmp";
            chartByType.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByModel.bmp";
            chartByModel.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByManufacturer.bmp";
            chartByManufacturer.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByCount.bmp";
            chartByManageabilityCount.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByToner.bmp";
            chartByManageabilityToner.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByHighUse.bmp";
            chartByUseHigh.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByLowUse.bmp";
            chartByUseLow.SaveImage(sPath, ChartImageFormat.Bmp);

            sPath = sRootPathImg + "ByCategory.bmp";
            chartByCategory.SaveImage(sPath, ChartImageFormat.Bmp);
        }
        catch 
        {
            //string myError = "Error With Images";
        }
    }
    // =========================================================================
    protected void BuildPDF()
    {

        //iTextSharp.text.pdf.PdfPTable table2 = new iTextSharp.text.pdf.PdfPTable(1);
        iTextSharp.text.pdf.PdfPTable table, tableWrap;
        PdfPCell cell, cellWrap;
        Phrase phrase, phrase2;

        string fullPath = "";
        string rootPathPdf = Server.MapPath("~") + "\\media\\scantron\\workfiles\\mp\\";
        //string rootPathImg = Server.MapPath("~") + "\\media\\scantron\\images\\logos\\company\\";
        string rootPathImg = Server.MapPath("~") + "\\media\\scantron\\workfiles\\mp\\";
        string sCurrentFile = GetAvailableFile();

        //Document doc = new Document();
        doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(rootPathPdf + sCurrentFile, FileMode.CreateNew));

        fullPath = rootPathImg + "MPowerPrint.png"; // 422
        iTextSharp.text.Image logoMPowerPrint = iTextSharp.text.Image.GetInstance(fullPath);

        doc.Open();
        // -----------------------------------------------------------------

        table = new iTextSharp.text.pdf.PdfPTable(8);
        table.TotalWidth = 450f;
        table.LockedWidth = true;
        table.SpacingAfter = 12f;

        //iTextSharp.text.Font fontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
        //iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 3f, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
        //iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 3f, iTextSharp.text.Font.NORMAL,  ;
        //new iTextSharp.text.Color(125, 88, 15);

        iTextSharp.text.Color cBlue = new iTextSharp.text.Color(64, 96, 128);


        // Row 1: Title and Logo
        phrase = new Phrase("Managed Print Review");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        cell = new PdfPCell(phrase);
        //cell.AddElement(phrase);
        cell.Colspan = 4;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        //cell.Padding = 5f;
        cell.PaddingTop = 23f;
        table.AddCell(cell);

        cell = new PdfPCell();
        cell.AddElement(logoMPowerPrint);
        cell.Colspan = 4;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        //cell.Padding = 5f;
        table.AddCell(cell);

        // Row 2: Divider Line
        cell = new PdfPCell();
        cell.Colspan = 8;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.BackgroundColor = iTextSharp.text.Color.GRAY;
        cell.Padding = 1;
        table.AddCell(cell);

        // Row 3: Spacer
        cell = new PdfPCell();
        cell.Colspan = 8;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.Padding = 5;
        table.AddCell(cell);

        // Row 4: Customer Name
        phrase = new Phrase(lbCustName.Text);
        //phrase.Font.Color = iTextSharp.text.Color.BLUE;
        phrase.Font.Color = new iTextSharp.text.Color(173, 0, 52);
        cell = new PdfPCell(phrase);
        cell.Colspan = 8;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.Padding = 5f;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        // Row 5: Customer Address
        phrase = new Phrase(lbAddress.Text + "   " + lbCityStateZip.Text);
        phrase.Font.Size = 8;
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        cell = new PdfPCell(phrase);
        cell.Colspan = 8;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.Padding = 5f;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        // ------------------------------------------------------
        // ------------------------------------------------------
        // Row 6: Agreement Detail
        phrase = new Phrase("Customer");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 8;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 6;
        table.AddCell(cell);

        phrase = new Phrase(lbCustNum.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 8;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Agreements");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 8;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 6;
        table.AddCell(cell);

        phrase = new Phrase(lbAgreements.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 8;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Locations");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 8;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 6;
        table.AddCell(cell);

        phrase = new Phrase(lbAgrLocations.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 8;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Devices");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 8;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 6;
        table.AddCell(cell);

        phrase = new Phrase(lbAgrDevices.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 8;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);
       
        // ------------------------------------------------------
        // ------------------------------------------------------

        doc.Add(table);

        // ------------------------------------------------------

        tableWrap = new iTextSharp.text.pdf.PdfPTable(1);
        tableWrap.TotalWidth = 452f;
        tableWrap.LockedWidth = false;
        tableWrap.SpacingAfter = 15f;
        cellWrap = new PdfPCell();
        cellWrap.BorderColor = iTextSharp.text.Color.GRAY;
        cellWrap.PaddingBottom = 7f;

        table = new iTextSharp.text.pdf.PdfPTable(8);
        table.TotalWidth = 450f;
        table.LockedWidth = true;
        //table.SpacingAfter = 20f;

        // Row 1: Report Conditions
        phrase = new Phrase("Date Range");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 6;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 5;
        table.AddCell(cell);

        phrase = new Phrase(ddDateStart.SelectedItem.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 6;
        phrase2 = new Phrase(ddDateEnd.SelectedItem.Text);
        phrase2.Font.Color = iTextSharp.text.Color.BLACK;
        phrase2.Font.Size = 6;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.AddElement(phrase2);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Utilization Threshold");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 6;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 5;
        table.AddCell(cell);

        phrase = new Phrase("Low:  " + ddUseLow.SelectedItem.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 6;
        phrase2 = new Phrase("High:  " + ddUseHigh.SelectedItem.Text);
        phrase2.Font.Color = iTextSharp.text.Color.BLACK;
        phrase2.Font.Size = 6;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.AddElement(phrase2);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Service Ticket Threshold");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 6;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 5;
        table.AddCell(cell);

        phrase = new Phrase(ddRequests.SelectedItem.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 6;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        phrase = new Phrase("Paper Jam Threshold");
        phrase.Font.Color = iTextSharp.text.Color.DARK_GRAY;
        phrase.Font.SetStyle(1);
        phrase.Font.Size = 6;
        cell = new PdfPCell(phrase); // Alignment is lost if you add element to cell after creation
        cell.HorizontalAlignment = 2;
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.PaddingTop = 5;
        table.AddCell(cell);

        phrase = new Phrase(ddPaperJams.SelectedItem.Text);
        phrase.Font.Color = iTextSharp.text.Color.BLACK;
        phrase.Font.Size = 6;
        cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Colspan = 1;
        cell.BorderColor = iTextSharp.text.Color.WHITE;
        cell.HorizontalAlignment = 0;
        table.AddCell(cell);

        cellWrap.AddElement(table);
        tableWrap.AddCell(cellWrap);
        doc.Add(tableWrap);

        //doc.Add(table);
        // ------------------------------------------------------


        // ------------------------------------------------
        //fullPath = rootPathImg + "ByPagesAll.bmp";
        //iTextSharp.text.Image byPagesAll = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByPagesMono.bmp";
        iTextSharp.text.Image byPagesMono = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByPagesColor.bmp";
        iTextSharp.text.Image byPagesColor = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByPagesMicr.bmp";
        iTextSharp.text.Image byPagesMicr = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByType.bmp";
        iTextSharp.text.Image byType = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByManufacturer.bmp";
        iTextSharp.text.Image byManufacturer = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByModel.bmp";
        iTextSharp.text.Image byModel = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByCount.bmp";
        iTextSharp.text.Image byCount = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByToner.bmp";
        iTextSharp.text.Image byToner = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByHighUse.bmp";
        iTextSharp.text.Image byHighUse = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByLowUse.bmp";
        iTextSharp.text.Image byLowUse = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPathImg + "ByCategory.bmp";
        iTextSharp.text.Image byCategory = iTextSharp.text.Image.GetInstance(fullPath);

        // -----------------------------------------------------------------
        // Detail Table
        // -----------------------------------------------------------------

        string sAgr = ddAgreement.SelectedItem.Value.ToString();
        int iCs1 = Int32.Parse(ddCustomer.SelectedItem.Value.ToString());
        int iStart = Int32.Parse(ddDateStart.SelectedItem.Value.ToString());
        int iEnd = Int32.Parse(ddDateEnd.SelectedItem.Value.ToString());
        
        string sSortBy = ddChildSort.SelectedValue;

        DataTable dataTable = new DataTable();
        if (sDevTestLive == "LIVE")
            dataTable = wsLiveMps.getDetailForAllDevices(iCs1, sAgr, iStart, iEnd, sSortBy);
        else
            dataTable = wsTestMps.getDetailForAllDevices(iCs1, sAgr, iStart, iEnd, sSortBy);

        // -----------------------------------------------------------------
        PdfPCell cell2;
        iTextSharp.text.pdf.PdfPTable table2 = new iTextSharp.text.pdf.PdfPTable(2);
        table2.HorizontalAlignment = 1;
        table2.TotalWidth = 450f;
        table2.LockedWidth = true;
/*
        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byPagesAll);
        table2.AddCell(cell2);
*/
        if (hfMonoFound.Value == "Y") 
        {
            cell2 = new PdfPCell();
            cell2.Colspan = 2;
            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell2.PaddingBottom = 20f;
            cell2.BorderColor = iTextSharp.text.Color.WHITE;
            cell2.AddElement(byPagesMono);
            table2.AddCell(cell2);
        }
        if (hfColorFound.Value == "Y") 
        {
            cell2 = new PdfPCell();
            cell2.Colspan = 2;
            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell2.PaddingBottom = 20f;
            cell2.BorderColor = iTextSharp.text.Color.WHITE;
            cell2.AddElement(byPagesColor);
            table2.AddCell(cell2);
        }
        if (hfMicrFound.Value == "Y")
        {
            cell2 = new PdfPCell();
            cell2.Colspan = 2;
            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell2.PaddingBottom = 20f;
            cell2.BorderColor = iTextSharp.text.Color.WHITE;
            cell2.AddElement(byPagesMicr);
            table2.AddCell(cell2);
        }

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byType);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byManufacturer);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byModel);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.HorizontalAlignment = 1;
        cell2.Colspan = 2;
        cell2.AddElement(byToner);
        cell2.PaddingRight = 5f;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.HorizontalAlignment = 1;
        cell2.Colspan = 2;
        cell2.AddElement(byCount);
        cell2.PaddingLeft = 5f;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byHighUse);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byLowUse);
        table2.AddCell(cell2);

        doc.Add(table2);

        DeviceDetail2(dataTable);
        //doc.Add(tablePdf);

        // Final Pie Chart below Location Tables
        table2 = new iTextSharp.text.pdf.PdfPTable(2);
        table2.HorizontalAlignment = 1;
        table2.TotalWidth = 450f;
        table2.LockedWidth = true;

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = iTextSharp.text.Color.WHITE;
        cell2.AddElement(byCategory);
        table2.AddCell(cell2);
        
        doc.Add(table2);

        // -----------------------------------------------------------------
        doc.Close();
        //Response.Redirect("~/media/scantron/pdf/arc/MpsReview.pdf" + sCurrentFile, false);
        Response.Redirect("~/media/scantron/pdf/arc/" + sCurrentFile, false);
    }

    // =========================================================================
    protected void PagesByTypeSearch(DataTable dtTable)
    {
        hfMonoFound.Value = "N";
        hfColorFound.Value = "N";
        hfMicrFound.Value = "N";

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
                hfMonoFound.Value = "Y";
            if (iColor > 0)
                hfColorFound.Value = "Y";
            if (iMicr > 0)
                hfMicrFound.Value = "Y";
            iRowIdx++;
        }
    }
    // =========================================================================
    /*
    protected void PagesByTypeAll(DataTable dtTable)
    {
        int iDateHold = 0;
        int iMonoMonth = 0;
        int iColorMonth = 0;
        int iMicrMonth = 0;
        int iRowIdx = 0;

        string[] xValues = new string[dtTable.Rows.Count]; // X Values can be text (for labels)

        int[] yValuesMono = new int[dtTable.Rows.Count];       // Y Values must be numeric (for values)
        int[] yValuesColor = new int[dtTable.Rows.Count];
        int[] yValuesMicr = new int[dtTable.Rows.Count];

        int iMonth = 0;

        iRowIdx = 0;
        foreach (DataRow row in dtTable.Rows)
        {
            iDateHold = Int32.Parse(dtTable.Rows[iRowIdx]["INVDATE"].ToString());
            iMonoMonth = Int32.Parse(dtTable.Rows[iRowIdx]["MONOMONTH"].ToString());
            iColorMonth = Int32.Parse(dtTable.Rows[iRowIdx]["COLORMONTH"].ToString());
            iMicrMonth = Convert.ToInt32(dtTable.Rows[iRowIdx]["MICRMONTH"]);
            iMonth = Int32.Parse((iDateHold.ToString()).Substring(4, 2));
            xValues[iRowIdx] = GetMonthAbbrev(iMonth);
            yValuesMono[iRowIdx] = iMonoMonth;
            yValuesColor[iRowIdx] = iColorMonth;
            yValuesMicr[iRowIdx] = iMicrMonth;
            iRowIdx++;
        }

        if (hfMonoFound.Value == "Y")
        {
            chartByPagesAll.Series["SeriesMono"].Points.DataBindXY(xValues, yValuesMono);
            //chartByPageCounts.Series["SeriesMono"]["PointWidth"] = "0.5";
            chartByPagesAll.Series["SeriesMono"]["DrawingStyle"] = "Cylinder";
        }
        if (hfColorFound.Value == "Y")
        {
            chartByPagesAll.Series["SeriesColor"].Points.DataBindXY(xValues, yValuesColor);
            //chartByPageCounts.Series["SeriesColor"]["PointWidth"] = "0.5";
            chartByPagesAll.Series["SeriesColor"]["DrawingStyle"] = "Cylinder";
        }
        if (hfMicrFound.Value == "Y")
        {
            chartByPagesAll.Series["SeriesMicr"].Points.DataBindXY(xValues, yValuesMicr);
            //chartByPageCounts.Series["SeriesMicr"]["PointWidth"] = "0.5";
            chartByPagesAll.Series["SeriesMicr"]["DrawingStyle"] = "Cylinder";
        }

        chartByPagesAll.Series["SeriesMono"].LegendText = "Monochrome";
        chartByPagesAll.Series["SeriesColor"].LegendText = "Color";
        chartByPagesAll.Series["SeriesMicr"].LegendText = "Micr";

        chartByPagesAll.Legends["Legend1"].Enabled = true;
        chartByPagesAll.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

        // ----------------------------------
    }
    */
    // =========================================================================
    protected string GetMonthAbbrev(int iMonth)
    {
        string sMonth = "";

        if (iMonth == 1)
            sMonth = "Jan";
        else if (iMonth == 2)
            sMonth = "Feb";
        else if (iMonth == 3)
            sMonth = "Mar";
        else if (iMonth == 4)
            sMonth = "Apr";
        else if (iMonth == 5)
            sMonth = "May";
        else if (iMonth == 6)
            sMonth = "Jun";
        else if (iMonth == 7)
            sMonth = "Jul";
        else if (iMonth == 8)
            sMonth = "Aug";
        else if (iMonth == 9)
            sMonth = "Sep";
        else if (iMonth == 10)
            sMonth = "Oct";
        else if (iMonth == 11)
            sMonth = "Nov";
        else if (iMonth == 12)
            sMonth = "Dec";

        return sMonth;

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
            xValues[iRowIdx] = GetMonthAbbrev(iMonth);
            yValuesMono[iRowIdx] = iMonoMonth;
            iRowIdx++;
        }

            chartByPagesMono.Series["SeriesMono"].Points.DataBindXY(xValues, yValuesMono);
            chartByPagesMono.Series["SeriesMono"]["PointWidth"] = "0.5";
            chartByPagesMono.Titles[1].Text = "Period Total: " + String.Format("{0:#,###}", iPageTotal); 

            // chartByPagesMono.Series["SeriesMono"]["ValueLabel"] = "Outside";
            //chartByPagesAmono.ChartAreas["ChartArea1"].AxisX.Interval = 1; 

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
            xValues[iRowIdx] = GetMonthAbbrev(iMonth);
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
            xValues[iRowIdx] = GetMonthAbbrev(iMonth);
            yValuesMicr[iRowIdx] = iMicrMonth;

            iRowIdx++;
        }

        chartByPagesMicr.Series["SeriesMicr"].Points.DataBindXY(xValues, yValuesMicr);
        chartByPagesMicr.Series["SeriesMicr"]["PointWidth"] = "0.5";
        chartByPagesMicr.Titles[1].Text = "Period Total: " + String.Format("{0:#,###}", iPageTotal); 

    }
    // =========================================================================
    protected string GetAvailableFile()
    {
        string sFilename = "";
        string sFullName = "";
        string sRootPathPdf = Server.MapPath("~") + "\\media\\scantron\\workfiles\\mp\\";
        int i = 0;
        string sRootName = "MpsReview";
        

        for (i = 1; i < 10; i++)
        {
            try
            {
                sFilename = sRootName + i.ToString() + ".pdf";
                sFullName = sRootPathPdf + sFilename;
                FileInfo TheFile = new FileInfo(sFullName);
                if (TheFile.Exists)
                {
                    File.Delete(sFullName);
                }
                else
                {
                    // File.Create(sFullName);
                }
                i = 10;
            }
            catch (Exception ex) 
            {
                string sError = ex.Message.ToString();
            }
        }
        return sFilename;
    }
    // =========================================================================
}