using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Xml;

public partial class private_tools_Geocode : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    Api_LIVE.ApiMenuSoapClient wsLiveApi = new Api_LIVE.ApiMenuSoapClient();
    Api_DEV.ApiMenuSoapClient wsTestApi = new Api_DEV.ApiMenuSoapClient();

    DateTime datTemp;
    int iDate = 0;
    int iRowsAffected = 0;
    string sMethodName = "";
    //string sLibrary = "OMDTALIB";
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ===========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["qty"] != null)
            {
                string sQty = Request.QueryString["qty"].ToString().Trim();
                int iQty = 0;
                int.TryParse(sQty, out iQty);
                if (iQty > 0 && iQty <= 20000)
                {
                    // Allow this to run daily from robot (then stop) 
                    RunCodes(iQty);
                    Response.End();
                }
            }
            else
            {
                try
                {
                    string sNextCode = "";
                    if (sLibrary == "OMDTALIB")
                    {
                        sNextCode = wsLiveApi.ZipGetNext("CANADA");
                    }
                    else
                    {
                        sNextCode = wsTestApi.ZipGetNext("CANADA");
                    }
                    lbNextCode.Text = "Next Postal Code To Process <b>" + sNextCode + "</b>";
                }
                catch (Exception ex)
                {
                    lbMsg.Text += " Main: " + ex.Message.ToString();
                }
                finally
                {
                }
            }
        }
    }
    // ===========================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            // set properties of the client
            return client.DownloadString(url);
        }
    }
    // ===========================================================
    protected void RunCodes(int qty)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        pnResults.Visible = true;

        datTemp = DateTime.Now;
        iDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        DateTime startTime = new DateTime();
        DateTime checkTime = new DateTime();
        DateTime endTime = new DateTime();
        TimeSpan ts = new TimeSpan();

        string sPageSource = "";
        string sPostalCode = "";
        string sUrl = "";
        string sRunTime = "";
        string sLastCodeProcessed = "";
        string sNextCode = "";
        string sNextChar = "";
        string sResult = "";

        int iCodesToProcess = 0;
        int iTrueCount = 0;
        int i = 0;

        try
        {
            startTime = DateTime.Now;
            lbMsg.Text = "";

            if (sLibrary == "OMDTALIB")
            {
                sNextCode = wsLiveApi.ZipGetNext("CANADA");
            }
            else
            {
                sNextCode = wsTestApi.ZipGetNext("CANADA");
            }

            if (sNextCode != "")
                sNextChar = sNextCode.Substring(0, 1);

            if (sLibrary == "OMDTALIB")
            {
                dataTable = wsLiveApi.ZipCaGetSet(sNextChar);
            }
            else
            {
                dataTable = wsTestApi.ZipCaGetSet(sNextChar);
            }

            startTime = DateTime.Now;
            checkTime = startTime;

            if (qty > 0)
                iCodesToProcess = qty;
            else
                int.TryParse(ddCodeQty.SelectedValue, out iCodesToProcess);

            if (dataTable.Rows.Count == 0)
                iCodesToProcess = 0;
            else
            {
                if (iCodesToProcess > dataTable.Rows.Count)
                    iCodesToProcess = dataTable.Rows.Count;
            }
            // iCodesToProcess = 1;

            for (i = 0; i < iCodesToProcess; i++)
            {
                iTrueCount++;

                sPostalCode = dataTable.Rows[i]["ZAZIP"].ToString().Trim();
//                sPostalCode = "E2B4T9"; // E2B4T9 gets Japan E1N1E1 A0A2M0

                if (ddSite.SelectedValue == "GEOCODER")
                {
                    sUrl = "http://geocoder.ca/?locate=" + sPostalCode;
                }
                else if (ddSite.SelectedValue == "GOOGLE") // use google
                {
                    sUrl = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + sPostalCode + "&sensor=false&region=ca";
                }
                else // use Yahoo
                {
                    sUrl = "http://where.yahooapis.com/geocode?q=" + sPostalCode + ",CANADA&locale=en_CA&appid=iUcnMEPV34HCMIwCi5UXBBkGBWLvqSXWFCBQDb14ScND_yG46zIXL_09oGoL_4xRGqA-";
                }
                // Steve Carlson iUcnMEPV34HCMIwCi5UXBBkGBWLvqSXWFCBQDb14ScND_yG46zIXL_09oGoL_4xRGqA -
                // TechToCustomer 10HPyx_V34GKdbHCo1YHuCsZcfYkg4Oj2kpxuyR97kyOPAQPiky79aQwcB6ujpgDEwU-
            // http://where.yahooapis.com/geocode?q=1600+Pennsylvania+Avenue,+Washington,+DC&appid=[yourappidhere]
                //endTime = DateTime.Now;
                //ts = endTime - checkTime;
                //lbTimeEachZip.Text += "HTS " + Convert.ToInt32(ts.TotalSeconds).ToString() + "| ";
                //checkTime = DateTime.Now;
                
                sPageSource = ScreenScrape(sUrl);

                //endTime = DateTime.Now;
                //ts = endTime - checkTime;
                //lbTimeEachZip.Text += "US " + Convert.ToInt32(ts.TotalSeconds).ToString() + "| ";
                //checkTime = endTime;
                
                if (ddSite.SelectedValue == "GEOCODER")
                {
                    System.Threading.Thread.Sleep(2100);
                    //sResult = ProcessGeoCoder(sPostalCode, sPageSource);
                }
                else if (ddSite.SelectedValue == "GOOGLE") // use google
                {
                    System.Threading.Thread.Sleep(2100);
                    //sResult = ProcessGoogle(sPostalCode, sPageSource);
                }
                else
                {
                    sResult = ProcessYahoo(sPostalCode, sPageSource);
                }

                if (sResult != "GOOD" &&
                    sResult != "ZERO_RESULTS" &&
                    sResult != "No error")
                {
                    lbMsg.Text = sResult;
                    i = iCodesToProcess;
                }
            }
            lbRecs.Text = "Records processed... " + iTrueCount.ToString();
            ltPageSource.Text = sPageSource;

            endTime = DateTime.Now;
            ts = endTime - startTime;
            sRunTime = ts.Minutes.ToString() + " Min... " + ts.Seconds.ToString() + " Sec...";
            lbTime.Text = "Run Time: " + sRunTime;

            if (sLibrary == "OMDTALIB")
            {
                sNextCode = wsLiveApi.ZipGetNext("CANADA");
            }
            else
            {
                sNextCode = wsTestApi.ZipGetNext("CANADA");
            }
            lbNextCode.Text = "Next Postal Code To Process <b>" + sNextCode + "</b>";
        }
        catch (Exception ex)
        {
            lbMsg.Text += "Last Code Processed: " + sLastCodeProcessed.ToString() + " -- " + ex.Message.ToString();
        }
        finally
        {
        }
    }
    // ===========================================================
    protected string ProcessYahoo(string postalCode, string src)
    {
        string sResult = "";
        string sError = "";
        string sErrorMessage = "";
        string sLatitude =  "";
        string sLongitude =  "";
        string sCity =  "";
        string sCounty =  "";
        string sState =  "";
        //string sCountry =  "";
        //string sCountryCode =  "";
        string sStateCode = "";

        double dLatitude = 0.0;
        double dLongitude = 0.0;

        try 
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(src);
            // doc.Load(Server.MapPath("~/public/tools/Test.xml"));

            XmlNode root = doc.DocumentElement;
            sError = root.SelectSingleNode("Error").ChildNodes[0].Value;
            sErrorMessage = root.SelectSingleNode("ErrorMessage").ChildNodes[0].Value;
            if (sError == "0")
            {
                if (root.SelectSingleNode("Result").SelectSingleNode("latitude").ChildNodes[0] != null)
                    sLatitude = root.SelectSingleNode("Result").SelectSingleNode("latitude").ChildNodes[0].Value;
                if (root.SelectSingleNode("Result").SelectSingleNode("longitude").ChildNodes[0] != null)
                    sLongitude = root.SelectSingleNode("Result").SelectSingleNode("longitude").ChildNodes[0].Value;

                double.TryParse(sLatitude, out dLatitude);
                double.TryParse(sLongitude, out dLongitude);

                //if (dLatitude != 0 && dLongitude != 0) 
                if (dLatitude > 11 && dLongitude < -11) 
                {
                    if (root.SelectSingleNode("Result").SelectSingleNode("city").ChildNodes[0] != null)
                        sCity = root.SelectSingleNode("Result").SelectSingleNode("city").ChildNodes[0].Value.ToUpper();
                    if (root.SelectSingleNode("Result").SelectSingleNode("county").ChildNodes[0] != null)
                        sCounty = root.SelectSingleNode("Result").SelectSingleNode("county").ChildNodes[0].Value.ToUpper();
                    if (root.SelectSingleNode("Result").SelectSingleNode("state").ChildNodes[0] != null)
                        sState = root.SelectSingleNode("Result").SelectSingleNode("state").ChildNodes[0].Value.ToUpper();
                    //sCountry = root.SelectSingleNode("Result").SelectSingleNode("country").ChildNodes[0].Value.ToUpper();
                    //sCountryCode = root.SelectSingleNode("Result").SelectSingleNode("countrycode").ChildNodes[0].Value.ToUpper();
                    if (root.SelectSingleNode("Result").SelectSingleNode("statecode").ChildNodes[0] != null)
                        sStateCode = root.SelectSingleNode("Result").SelectSingleNode("statecode").ChildNodes[0].Value.ToUpper();

                    sCity = encode(sCity);
                    sState = encode(sState);
                    sCounty = encode(sCounty);

                    if (sLibrary == "OMDTALIB")
                    {
                        iRowsAffected = wsLiveApi.ZipCaUpdGood(postalCode, sCity, sStateCode, sState, sCounty, dLatitude, dLongitude);
                    }
                    else
                    {
                        iRowsAffected = wsTestApi.ZipCaUpdGood(postalCode, sCity, sStateCode, sState, sCounty, dLatitude, dLongitude);

                    }
                    sResult = "GOOD";
                }
                /*
lbMsg.Text += "Xml Results:<br /> " +
sError + "<br />" +
sErrorMessage + "<br />" +
sLatitude + "<br />" +
sLongitude + "<br />" +
sCity + "<br />" +
sCounty + "<br />" +
sState + "<br />" +
sCountry + "<br />" +
sCountryCode + "<br />" +
sStateCode;
*/

            }

            if (sError != "0" || dLatitude < 11 || dLongitude > -11) // Bad entries...
            {
                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsLiveApi.ZipCaUpdBad(postalCode, "Z");
                }
                else
                {
                    iRowsAffected = wsTestApi.ZipCaUpdBad(postalCode, "Z");
                }
                sResult = sErrorMessage;
            }
        }
        catch (Exception ex)
        {
            //            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text += "Xml Error: " + ex.Message.ToString();
            sResult = ex.Message.ToString();
        }
        finally
        {
//            odbcCmd.Dispose();
//            odbcConn.Close();
//            odbcConn.Dispose();
        }


        return sResult;
    }
    // ===========================================================
    protected string encode(string textIn)
    {
        string sTxt = textIn;

        sTxt = sTxt.Replace("Ã‰", "E");
        sTxt = sTxt.Replace("Ã€", "A");
        sTxt = sTxt.Replace("ÃŽ", "I");
        sTxt = sTxt.Replace("Ã¨", "E");
        sTxt = sTxt.Replace("Ã®", "I");
        sTxt = sTxt.Replace("Ã©", "E");

        return sTxt;
    }
    // ===========================================================
    protected void btCodeRun_Click(object sender, EventArgs e)
    {
        RunCodes(0);
    }
    // ===========================================================
    // ===========================================================
}
