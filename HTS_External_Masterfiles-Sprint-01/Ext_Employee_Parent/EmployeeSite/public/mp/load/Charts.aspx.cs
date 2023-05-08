using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.Odbc;

using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class public_mp_load_Charts : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------

    OdbcCommand odbcCmd;
    OdbcConnection odbcConn;
    OdbcDataReader odbcReader;

    string sConnectionString = "";
    string sErrMessage = "";
    string sErrValues = "";
    string sSql = "";
    ErrorHandler eh;
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        eh = new ErrorHandler();
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e)
    {
        eh = null;
    }
    //==================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();
        TimeSpan ts = new TimeSpan();
        string sRunTime = "";

        try
        {
            odbcConn.Open();
            startTime = DateTime.Now;
            dataTable = GetHeaderKey();
            CreateChartsForAllUnits(dataTable);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();

            endTime = DateTime.Now;
            ts = endTime - startTime;
            sRunTime = ts.Minutes.ToString() + " Min... " + ts.Seconds.ToString() + " Sec...";
            Response.Write("<br /> Run Time: " + sRunTime);
        }

    }
    // ========================================================================
    protected DataTable GetHeaderKey()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select hKey" +
                " from " + sLibrary + ".MPUHD" +
                " where hNotMan = 0" +
                " and hHidden = 0" +
                " order by hKey desc";
            //                " where hNotMan = 0" +
            //                " and hKey < 100";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected void CreateChartsForAllUnits(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        int iKey = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString().Trim());

            try
            {
                DataTable chTable = new DataTable(sMethodName);
                Chart chLevels = new Chart();

                chTable = GetPastTonerLevels(iKey);
                chLevels = ChartTonerLevelsForImage(chTable);

                SaveChartAsImage(chLevels, iKey);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            iRowIdx++;
        }
        // --------------------------------------------------

    }
    // =========================================================================
    protected Chart ChartTonerLevelsForImage(DataTable dTable)
    {
        // Create a Chart   
        Chart chart1 = new Chart();
        // Create Chart Area   
        ChartArea chartArea1 = new ChartArea();

        // Add Chart Area to the Chart   
        chart1.ChartAreas.Add(chartArea1);

        // Create a data series   
        Series series0 = new Series();
        Series series1 = new Series();
        Series series2 = new Series();
        Series series3 = new Series();
        Series series4 = new Series();
        Series series5 = new Series();
        Series series6 = new Series();

        System.Drawing.Color myBorderColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
        System.Drawing.Color myChartColor = System.Drawing.ColorTranslator.FromHtml("#EBEBE0");
        System.Drawing.Color myWhite = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

        int iXAxisInterval = 7;

        // chart1.ID = "ch" + iRowIdx.ToString();
        chart1.Palette = ChartColorPalette.BrightPastel;
        chart1.Width = 600;
        chart1.Height = 125; // 175
        //chart1.BackColor = myCharColor;
        chart1.BackGradientStyle = GradientStyle.TopBottom;
        chart1.BorderlineWidth = 2;
        chart1.BorderColor = myBorderColor;

        series0.ChartType = SeriesChartType.Spline;
        series1.ChartType = SeriesChartType.Spline;
        series2.ChartType = SeriesChartType.Spline;
        series3.ChartType = SeriesChartType.Spline;
        series4.ChartType = SeriesChartType.Spline;
        series5.ChartType = SeriesChartType.Spline;
        series6.ChartType = SeriesChartType.Spline;

        chart1.Series.Add(series0);
        chart1.Series.Add(series1);
        chart1.Series.Add(series2);
        chart1.Series.Add(series3);
        chart1.Series.Add(series4);
        chart1.Series.Add(series5);
        chart1.Series.Add(series6);

        string sDate = "";
        int iDate = 0;
        double dLevelBlack = 0.0;
        double dLevelCyan = 0.0;
        double dLevelMagenta = 0.0;
        double dLevelYellow = 0.0;
        int iRowIdx = 0;
        int iLineWidth = 3;

        string sMonthName = "";
        DateTime datScanned = new DateTime();

        string[] xValues = new string[dTable.Rows.Count]; // X Values can be text (for labels)

        double[] yValuesBlack = new double[dTable.Rows.Count];  // Y Values must be numeric (for values)
        double[] yValuesCyan = new double[dTable.Rows.Count];
        double[] yValuesMagenta = new double[dTable.Rows.Count];
        double[] yValuesYellow = new double[dTable.Rows.Count];

        iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iDate = Int32.Parse(dTable.Rows[iRowIdx]["tDat"].ToString());
            sDate = iDate.ToString();
            sDate = sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2) + " 01:01:01.000";
            datScanned = Convert.ToDateTime(sDate);

            if (datScanned.Month == 1)
                sMonthName = "Jan";
            else if (datScanned.Month == 2)
                sMonthName = "Feb";
            else if (datScanned.Month == 3)
                sMonthName = "Mar";
            else if (datScanned.Month == 4)
                sMonthName = "Apr";
            else if (datScanned.Month == 5)
                sMonthName = "May";
            else if (datScanned.Month == 6)
                sMonthName = "Jun";
            else if (datScanned.Month == 7)
                sMonthName = "Jul";
            else if (datScanned.Month == 8)
                sMonthName = "Aug";
            else if (datScanned.Month == 9)
                sMonthName = "Sep";
            else if (datScanned.Month == 10)
                sMonthName = "Oct";
            else if (datScanned.Month == 11)
                sMonthName = "Nov";
            else if (datScanned.Month == 12)
                sMonthName = "Dec";

            string sDateFormat = sMonthName + " " + datScanned.Day;
            dLevelBlack = double.Parse(dTable.Rows[iRowIdx]["tBlack"].ToString());
            dLevelCyan = double.Parse(dTable.Rows[iRowIdx]["tCyan"].ToString());
            dLevelMagenta = double.Parse(dTable.Rows[iRowIdx]["tMagenta"].ToString());
            dLevelYellow = double.Parse(dTable.Rows[iRowIdx]["tYellow"].ToString());

            xValues[iRowIdx] = sDateFormat;
            yValuesBlack[iRowIdx] = dLevelBlack;
            yValuesCyan[iRowIdx] = dLevelCyan;
            yValuesMagenta[iRowIdx] = dLevelMagenta;
            yValuesYellow[iRowIdx] = dLevelYellow;
            iRowIdx++;
        }

        chart1.Series[5].Points.DataBindXY(xValues, yValuesBlack);
        chart1.Series[5].IsValueShownAsLabel = false;
        chart1.Series[5].BorderWidth = iLineWidth;

        chart1.Series[0].Points.DataBindXY(xValues, yValuesCyan);
        chart1.Series[0].IsValueShownAsLabel = false;
        chart1.Series[0].BorderWidth = iLineWidth;

        chart1.Series[2].Points.DataBindXY(xValues, yValuesMagenta);
        chart1.Series[2].IsValueShownAsLabel = false;
        chart1.Series[2].BorderWidth = iLineWidth;

        chart1.Series[6].Points.DataBindXY(xValues, yValuesYellow);
        chart1.Series[6].IsValueShownAsLabel = false;
        chart1.Series[6].BorderWidth = iLineWidth;

        chart1.ChartAreas["ChartArea1"].AxisX.Interval = iXAxisInterval;  // Recs between labels.

        return chart1;
    }

    // ========================================================================
    protected DataTable GetPastTonerLevels(int key)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddMonths(-6);
        int iStartDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        if (key > 0)
        {
            try
            {
                // ---------------------------------------------------
                // Get Toner Levels for unit passed from metric_tonerlevel
                // ---------------------------------------------------
                sSql = "select tDat, tBlack, tCyan, tMagenta, tYellow, tScn" +
                " from " + sLibrary + ".MPUTONLG" +
                " where tKey = ?" +
                " and tDat > ?" +
                " order by tDat ";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = key;

                odbcCmd.Parameters.Add("@StartDate", OdbcType.Int);
                odbcCmd.Parameters["@StartDate"].Value = iStartDate;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dataTable.Load(odbcReader);

                DataColumn dc;

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "CalDate";
                dataTable.Columns.Add(dc);

                double dBlack = 0.0;
                double dCyan = 0.0;
                double dMagenta = 0.0;
                double dYellow = 0.0;

                double dBlackH = 0.0;
                double dCyanH = 0.0;
                double dMagentaH = 0.0;
                double dYellowH = 0.0;

                int iRowIdx = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    if (double.TryParse(dataTable.Rows[iRowIdx]["tBlack"].ToString(), out dBlack) == false)
                        dBlack = 0;
                    if (double.TryParse(dataTable.Rows[iRowIdx]["tCyan"].ToString(), out dCyan) == false)
                        dCyan = 0;
                    if (double.TryParse(dataTable.Rows[iRowIdx]["tMagenta"].ToString(), out dMagenta) == false)
                        dMagenta = 0;
                    if (double.TryParse(dataTable.Rows[iRowIdx]["tYellow"].ToString(), out dYellow) == false)
                        dYellow = 0;

                    if (dBlack == 0 && dBlackH > 0)
                    {
                        dBlack = dBlackH;
                        dataTable.Rows[iRowIdx]["tBlack"] = dBlack.ToString();
                    }

                    if (dCyan == 0 && dCyanH > 0)
                    {
                        dCyan = dCyanH;
                        dataTable.Rows[iRowIdx]["tCyan"] = dCyan.ToString();
                    }
                    if (dMagenta == 0 && dMagentaH > 0)
                    {
                        dMagenta = dMagentaH;
                        dataTable.Rows[iRowIdx]["tMagenta"] = dMagenta.ToString();
                    }
                    if (dYellow == 0 && dYellowH > 0)
                    {
                        dYellow = dYellowH;
                        dataTable.Rows[iRowIdx]["tYellow"] = dYellow.ToString();
                    }

                    dBlackH = dBlack;
                    dCyanH = dCyan;
                    dMagentaH = dMagenta;
                    dYellowH = dYellow;

                    iRowIdx++;
                }
                dataTable.AcceptChanges();

            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return dataTable;
    }

    // =========================================================================
    protected void SaveChartAsImage(Chart tonerLevels, int key)
    {
        try
        {
            string sFilePath = CreateImageFilePath(key);
            //tonerLevels.SaveImage(sFilePath, ChartImageFormat.Bmp);
            tonerLevels.SaveImage(sFilePath, ChartImageFormat.Gif);
        }
        catch (Exception ex)
        {
            sErrValues = "Error With Charts...";
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
    }


    // =========================================================================
    protected string CreateImageFilePath(int key)
    {
        string sFilePath = "";
        string sRootPath = Server.MapPath("~") + "\\media\\charts\\mp\\tonerHist\\";

        string sFilename = "chart_00000";
        //string sExtension = ".bmp";
        string sExtension = ".gif";
        string sKey = "";

        sKey = key.ToString().Trim();
        if (key >= 10000)
            sFilename = sFilename.Substring(0, 6) + sKey + sExtension;
        else if (key >= 1000)
            sFilename = sFilename.Substring(0, 7) + sKey + sExtension;
        else if (key >= 100)
            sFilename = sFilename.Substring(0, 8) + sKey + sExtension;
        else if (key >= 10)
            sFilename = sFilename.Substring(0, 9) + sKey + sExtension;
        else
            sFilename = sFilename.Substring(0, 10) + sKey + sExtension;

        sFilePath = sRootPath + sFilename;

        return sFilePath;
    }
    // =========================================================================
    // =========================================================================
}