using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Xml;

public partial class private_tools_ZipRoadUpdater : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    Api_LIVE.ApiMenuSoapClient wsLiveApi = new Api_LIVE.ApiMenuSoapClient();
    Api_DEV.ApiMenuSoapClient wsTestApi = new Api_DEV.ApiMenuSoapClient();

    /* As400 SQL to check for how many US Zips need updating...
    select count(*) from omdtalib/zip2fst where z2fupd = 'Y'                                              
    and substring(z2fzp1,1,1) 
    in ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')                                                   
     * This was used to clear military records: check to see if you are disregarding them now in zipld2 pgms
     * delete from omdtalib/zip2mkt           
where z2mzp1 in                        
(select distinct z2mzp1                
from omdtalib/zip2mkt, omdtalib/zipall 
where z2mzp1 = zazip                   
and zasts = 'MILITARY')                
     */
    DateTime datTemp;
    int iDate = 0;
    int iRowsAffected = 0;
    string sMethodName = "";
    string sFilename = ""; // ZIP2MKT, ZIP2CTR, ZIP2FST
    string sCountry = ""; // USA, CANADA
    string sRegion = ""; // us or ca
    string[] saNextRec = new string[6];

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
                if (iQty > 0 && iQty < 2500)
                {
                    // Allow this to run daily from robot (then stop) 
                    ProcessRecs(iQty);
                    Response.End();
                }
            }
            else
            {
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
    protected void ProcessRecs(int qty)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        pnResults.Visible = true;

        datTemp = DateTime.Now;
        iDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();
        TimeSpan ts = new TimeSpan();

        string sPageSource = "";
        string sUrl = "";
        string sRunTime = "";
        string sLastCodeProcessed = "";
//        string sNextChar = "";
        string sResult = "";

        int iCodesToProcess = 0;
        int iTrueCount = 0;
//        int iRowsAffected = 0;
        int iRecsToUpdate = 0;
        int i = 0;

        try
        {
            startTime = DateTime.Now;
            lbMsg.Text = "";

            sFilename = ddFilename.SelectedValue;
            sCountry = ddCountry.SelectedValue;
            if (sCountry == "CANADA")
                sRegion = "ca";
            else
                sRegion = "us";

            if (qty > 0)
                iCodesToProcess = qty;
            else
                int.TryParse(ddCodeQty.SelectedValue, out iCodesToProcess);

//            iCodesToProcess = 1;

            if (sLibrary == "OMDTALIB")
            {
                saNextRec = wsLiveApi.DistanceGetNextRec(sFilename, sCountry);
                iRecsToUpdate = wsLiveApi.DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
            }
            else
            {
                saNextRec = wsTestApi.DistanceGetNextRec(sFilename, sCountry);
                iRecsToUpdate = wsTestApi.DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
            }

            for (i = 0; i < iCodesToProcess; i++)
            {
                if (saNextRec[0] == null || saNextRec[1] == null)
                {
                    i = iCodesToProcess;
                }
                else
                {
                    iTrueCount++;

                    // Try Lat/Lng
                    //                sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=68130&destination=68132&region=us&units=imperial&sensor=false";
                    sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + saNextRec[2] + "," + saNextRec[3] + "&destination=" + saNextRec[4] + "," + saNextRec[5] + "&region=" + sRegion + "&units=imperial&sensor=false";
                    sPageSource = ScreenScrape(sUrl);
                    //System.Threading.Thread.Sleep(1000);
                    System.Threading.Thread.Sleep(800);
                    sResult = GetResult(sPageSource, saNextRec[0], saNextRec[1]);

                    if (sResult == "ZERO_RESULTS")
                    {
                        i++;
                        // Try Zip or Postal Code instead of lat/lng
                        sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + saNextRec[0] + "&destination=" + saNextRec[1] + "&region=" + sRegion + "&units=imperial&sensor=false";
                        sPageSource = ScreenScrape(sUrl);
                        System.Threading.Thread.Sleep(1000);
                        sResult = GetResult(sPageSource, saNextRec[0], saNextRec[1]);
                    }
                    //sResult = ProcessGoogle(saNextRec[0], sPageSource);

                    if (sResult != "OK" &&
                        sResult != "ZERO_RESULTS" &&
                        sResult != "NOT_FOUND")
                    {
                        lbMsg.Text = sResult;
                        i = iCodesToProcess;
                    }
                    if (sLibrary == "OMDTALIB")
                    {
                        saNextRec = wsLiveApi.DistanceGetNextRec(sFilename, sCountry);
                        iRecsToUpdate = wsLiveApi.DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
                    }
                    else
                    {
                        saNextRec = wsTestApi.DistanceGetNextRec(sFilename, sCountry);
                        iRecsToUpdate = wsTestApi.DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
                    }
                    // Valid Values to process?  
                }
            }
            sLastCodeProcessed = saNextRec[0];
            lbRecs.Text = "Records processed... " + iTrueCount.ToString() + "   ---> Last zip from: " + saNextRec[1] + "   ---> Last zip to: " + saNextRec[0];
            ltPageSource.Text = sPageSource;

            endTime = DateTime.Now;
            ts = endTime - startTime;
            sRunTime = ts.Minutes.ToString() + " Min... " + ts.Seconds.ToString() + " Sec...";
            lbTime.Text = "Run Time: " + sRunTime;

            lbNextCode.Text = "Next Code To Process <b>" + saNextRec[0] + "</b> (Remaining Recs " + iRecsToUpdate + ")";
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
    protected string GetResult(string src, string code1, string code2)
    {
        string sResult = "";
        string sSeconds = "";
        string sMeters = "";
        double dSeconds = 0.0;
        double dMeters = 0.0;
        double dHoursMinutes = 0.0;
        double dMiles = 0.0;

        try 
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(src);

            XmlNode root = doc.DocumentElement;

            if (root.SelectSingleNode("status").ChildNodes[0] != null) 
            {
                sResult = root.SelectSingleNode("status").ChildNodes[0].Value;
            }
            if (sResult == "ZERO_RESULTS" || sResult == "NOT_FOUND") 
            {
                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsLiveApi.DistanceSetZeroResults(sFilename, code1, code2);
                }
                else
                {
                    iRowsAffected = wsTestApi.DistanceSetZeroResults(sFilename, code1, code2);
                }
            }
            if (sResult == "OK") 
            {
                if (root.SelectSingleNode("route").SelectSingleNode("leg").SelectSingleNode("duration").SelectSingleNode("value").ChildNodes[0] != null) 
                {
                    sSeconds = root.SelectSingleNode("route").SelectSingleNode("leg").SelectSingleNode("duration").SelectSingleNode("value").ChildNodes[0].Value;
                    double.TryParse(sSeconds, out dSeconds);
                    TimeSpan ts = TimeSpan.FromSeconds(dSeconds);
                    dHoursMinutes = (ts.Days * 24) + ts.Hours + (ts.Minutes * .01);
                }
                if (root.SelectSingleNode("route").SelectSingleNode("leg").SelectSingleNode("distance").SelectSingleNode("value").ChildNodes[0] != null) 
                {
                    sMeters = root.SelectSingleNode("route").SelectSingleNode("leg").SelectSingleNode("distance").SelectSingleNode("value").ChildNodes[0].Value;
                    double.TryParse(sMeters, out dMeters);
                    dMiles = dMeters * 0.0006214;
                    dMiles = Convert.ToDouble(string.Format("{0:0.00}", dMiles));
                }

                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsLiveApi.DistanceSetMilesTime(sFilename, code1, code2, dMiles, dHoursMinutes);
                }
                else
                {
                    iRowsAffected = wsTestApi.DistanceSetMilesTime(sFilename, code1, code2, dMiles, dHoursMinutes);
                }
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
    protected string ProcessGoogle(string code, string src)
    {
        string sResult = "OK";
        XmlDocument xmlBak = new XmlDocument();
        XmlNode nodeBak;
        XmlNodeReader nr;
        XmlReaderSettings settings = new XmlReaderSettings();

        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;

        xmlBak.LoadXml(src);
        nodeBak = xmlBak.DocumentElement;
        nr = new XmlNodeReader(nodeBak);
        // ====================================

        string[] saCitProNat = new string[3];
        char[] cSplitter = { ',' };

        string sCurrNodeType = "";
        string sCurrNodeName = "";
        string sCurrNodeValue = "";

        string sLevel1 = "";
        string sLevel2 = "";
        string sLevel3 = "";
        string sLevel4 = "";
        string sLevel5 = "";
        string sLevel6 = "";

        string sDocStatus = "";
        // -------------------------
        string sRouteSummary = "";
        string sStepHtmlInstructions = "";
        string sStepPolyline = "";
        string sStepDurationSeconds = "";
        string sStepDurationText = "";
        string sStepDistanceMeters = "";
        string sStepDistanceText = "";
        
        string sStartAddress = "";
        string sEndAddress = "";
        string sCopyrights = "";

        string sPolyline = "";
        string sDurationSeconds = "";
        string sDurationText = "";
        string sDistanceMeters = "";
        string sDistanceText = "";
        // -------------------------
//        int iCurrentLevel = 1;
//        int iRowsAffected = 0;

//        double dLatitude = 0.0;
//        double dLongitude = 0.0;

        //        string sTest = "N";

        try
        {
            //            if (sTest == "N")
            //            {
            while (nr.Read())
            {
                if (sDocStatus == "" || sDocStatus == "OK")
                {
                    sCurrNodeType = nr.NodeType.ToString();
                    sCurrNodeName = nr.LocalName;
                    sCurrNodeValue = nr.Value;
                    // ---------------------------------------------
                    // START ELEMENT
                    // -------------------------------------
                    if (nr.NodeType == XmlNodeType.Element)
                    {
                        if (sLevel1 == "")
                        {
                            //                        if ((nr.LocalName == "status") ||
                            //                            (nr.LocalName == "route"))
                            //                            sLevel1 = nr.LocalName;
                            sLevel1 = nr.LocalName;
                        }
                        else // inside a level 1 element...
                        {
                            if (sLevel2 == "")
                            {
                                sLevel2 = nr.LocalName;
                            }
                            else  // inside a level 2 element...
                            {
                                if (sLevel3 == "")
                                {
                                    sLevel3 = nr.LocalName;
                                }
                                else  // inside a level 3 element...
                                {
                                    if (sLevel4 == "")
                                    {
                                        sLevel4 = nr.LocalName;
                                    }
                                    else   // inside a level 4 element...
                                    {
                                        if (sLevel5 == "")
                                        {
                                            sLevel5 = nr.LocalName;
                                        }
                                        else  // inside a level 4 element...
                                        {
                                            if (sLevel6 == "")
                                            {
                                                sLevel6 = nr.LocalName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // -------------------------------------
                    // TEXT
                    // -------------------------------------
                    else if (nr.NodeType == XmlNodeType.Text)
                    {
                        if (sLevel2 == "status")
                        {
                            sDocStatus = nr.Value;
                            if (sDocStatus != "OK")
                            {
                                if (sDocStatus == "ZERO_RESULTS")
                                {
                                }
                            }
                        }
                        if (sLevel2 == "route")
                        {
                            if (sLevel3 == "summary") 
                            {
                                sRouteSummary = nr.Value;
                            }
                            else if (sLevel3 == "leg")
                            {
                                if (sLevel4 == "step") 
                                {
                                    if (sLevel5 == "travel_mode") 
                                    { 
                                    }
                                    else if (sLevel5 == "start_location")
                                    {
                                        if (sLevel6 == "lat")
                                        {
                                        }
                                        else if (sLevel6 == "lng")
                                        {
                                        }
                                    }
                                    else if (sLevel5 == "end_location")
                                    {
                                        if (sLevel6 == "lat")
                                        {
                                        }
                                        else if (sLevel6 == "lng")
                                        {
                                        }
                                    }
                                    else if (sLevel5 == "polyline")
                                    {
                                        if (sLevel6 == "points")
                                        {
                                            sStepPolyline = nr.Value;
                                        }
                                    }
                                    else if (sLevel5 == "duration")
                                    {
                                        if (sLevel6 == "value")
                                        {
                                            sStepDurationSeconds = nr.Value;
                                        }
                                        else if (sLevel6 == "text")
                                        {
                                            sStepDurationText = nr.Value;
                                        }
                                    }
                                    else if (sLevel5 == "html_instructions")
                                    {
                                        sStepHtmlInstructions = HttpUtility.HtmlDecode(nr.Value).Trim();
                                    }
                                    else if (sLevel5 == "distance")
                                    {
                                        if (sLevel6 == "value")
                                        {
                                            sStepDistanceMeters = nr.Value;
                                        }
                                        else if (sLevel6 == "text")
                                        {
                                            sStepDistanceText = nr.Value;
                                        }
                                    }
                                }
                                else if (sLevel4 == "duration")
                                {
                                    if (sLevel5 == "value")
                                    {
                                        sDurationSeconds = nr.Value;
                                    }
                                    else if (sLevel5 == "text")
                                    {
                                        sDurationText = nr.Value;
                                    }
                                }
                                else if (sLevel4 == "distance")
                                {
                                    if (sLevel5 == "value")
                                    {
                                        sDistanceMeters = nr.Value;
                                    }
                                    else if (sLevel5 == "text")
                                    {
                                        sDistanceText = nr.Value;
                                    }
                                }
                                else if (sLevel4 == "start_location")
                                {
                                    if (sLevel5 == "lat")
                                    {
                                    }
                                    else if (sLevel5 == "lng")
                                    {
                                    }
                                }
                                else if (sLevel4 == "end_location")
                                {
                                    if (sLevel5 == "lat")
                                    {
                                    }
                                    else if (sLevel5 == "lng")
                                    {
                                    }
                                }
                                else if (sLevel4 == "start_address")
                                {
                                    sStartAddress = nr.Value;
                                }
                                else if (sLevel4 == "end_address")
                                {
                                    sEndAddress = nr.Value;
                                }
                            }
                            else if (sLevel3 == "copyrights")
                            {
                                sCopyrights = HttpUtility.HtmlDecode(nr.Value).Trim();
                            }
                            else if (sLevel3 == "overview_polyline")
                            {
                                if (sLevel4 == "points")
                                {
                                    sPolyline = nr.Value;
                                }
                            }
                            else if (sLevel3 == "bounds")
                            {
                                if (sLevel4 == "southwest")
                                {
                                    if (sLevel5 == "lat")
                                    {
                                    }
                                    else if (sLevel5 == "lng")
                                    {
                                    }
                                }
                                else if (sLevel4 == "northeast")
                                {
                                    if (sLevel5 == "lat")
                                    {
                                    }
                                    else if (sLevel5 == "lng")
                                    {
                                    }
                                }
                            }
                        }
                    }
                    // -------------------------------------
                    // END ELEMENT
                    // -------------------------------------
                    else if (nr.NodeType == XmlNodeType.EndElement)
                    {
                        if (sLevel6 != "")
                            sLevel6 = "";
                        else
                        {
                            if (sLevel5 != "")
                                sLevel5 = "";
                            else
                            {
                                if (sLevel4 != "")
                                    sLevel4 = "";
                                else
                                {
                                    if (sLevel3 != "")
                                        sLevel3 = "";
                                    else
                                    {
                                        if (sLevel2 != "")
                                            sLevel2 = "";
                                        else
                                        {
                                            if (sLevel1 != "")
                                                sLevel1 = "";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // ---------------------------------------------
                }
            }
        }
        //        } // Test bypass
        catch (Exception ex)
        {
            string sError = ex.ToString();
        }
        // ====================================
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
        ProcessRecs(0);
    }
    // ===========================================================
    // ===========================================================
}

