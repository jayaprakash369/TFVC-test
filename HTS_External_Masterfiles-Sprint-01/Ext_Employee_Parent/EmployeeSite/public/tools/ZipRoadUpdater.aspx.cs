using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Xml;

public partial class public_tools_ZipRoadUpdater : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler eh;
    char[] cSplitter = { '|' };
    string sConnectionString = "";
    string sSql = "";
    int iZipsToDoStart = 0;
    int iZipsToDoEnd = 0;
    int iZipsUpdated = 0;
    string sErrorValues = "";
    
//    Api_LIVE.ApiMenuSoapClient wsLiveApi = new Api_LIVE.ApiMenuSoapClient();
//    Api_DEV.ApiMenuSoapClient wsTestApi = new Api_DEV.ApiMenuSoapClient();

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

string sLibrary = "OMDTALIB";
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); this.RequireSSL = false; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ===========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack)
        {
            string sFile = "";
            if (Request.QueryString["fil"] != null)
                sFile = Request.QueryString["fil"].ToString().Trim();
            if (!String.IsNullOrEmpty(sFile)) 
            {
                if (Request.QueryString["qty"] != null)
                {
                    string sQty = Request.QueryString["qty"].ToString().Trim();
                    int iQty = 0;
                    int.TryParse(sQty, out iQty);
                    if (iQty > 0 && iQty < 2500)
                    {
                        // Allow this to run daily from robot (then stop) 
                        ProcessRecs(sFile, iQty);
                        Response.End();
                    }
                }
                else
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
    protected void ProcessRecs(string filename, int qty)
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
        int iGoogleCallCount = 0;
//        int iRowsAffected = 0;
        int iRecsToUpdate = 0;
        int i = 0;

        try
        {
            odbcConn.Open();

            startTime = DateTime.Now;
            lbMsg.Text = "";

            if (!String.IsNullOrEmpty(filename)) 
            {
                sFilename = filename;
                sCountry = "USA";
                sRegion = "us";
            }
            else 
            {
                sFilename = ddFilename.SelectedValue;
                sCountry = ddCountry.SelectedValue;
                if (sCountry == "CANADA")
                    sRegion = "ca";
                else
                    sRegion = "us";
            }

            // Get initial count of zips to be updated to subtract from at the end to get "count processed"
            iZipsUpdated = 0;
            iZipsToDoStart = DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");

            if (qty > 0)
                iCodesToProcess = qty;
            else
                int.TryParse(ddCodeQty.SelectedValue, out iCodesToProcess);

//            iCodesToProcess = 1;

            saNextRec = DistanceGetNextRec(sFilename, sCountry);
            iRecsToUpdate = DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
/*
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
*/
            for (i = 0; i < iCodesToProcess; i++)
            {
                if (saNextRec[0] == null || saNextRec[1] == null)
                {
                    i = iCodesToProcess;
                }
                else
                {
                    iGoogleCallCount++;

                    // Try Lat/Lng
                    //                sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=68130&destination=68132&region=us&units=imperial&sensor=false";
                    sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + saNextRec[2] + "," + saNextRec[3] + "&destination=" + saNextRec[4] + "," + saNextRec[5] + "&region=" + sRegion + "&units=imperial&sensor=false";
                    sPageSource = ScreenScrape(sUrl);
                    //System.Threading.Thread.Sleep(1000);
                    // System.Threading.Thread.Sleep(800);
                    System.Threading.Thread.Sleep(600);
                    sResult = GetResult(sPageSource, saNextRec[0], saNextRec[1]);

                    if (sResult == "ZERO_RESULTS")
                    {
                        i++;
                        // Try Zip or Postal Code instead of lat/lng
                        sUrl = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + saNextRec[0] + "&destination=" + saNextRec[1] + "&region=" + sRegion + "&units=imperial&sensor=false";
                        sPageSource = ScreenScrape(sUrl);
                        // System.Threading.Thread.Sleep(800);
                        System.Threading.Thread.Sleep(600);
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

                    saNextRec = DistanceGetNextRec(sFilename, sCountry);
                    // Commented April 6th, 2015 because it needs to be run AFTER the loop
//                    iRecsToUpdate = DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
//                    iZipsToDoEnd = iRecsToUpdate;
/*
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
*/ 
                    // Valid Values to process?  
                }
            }

            iRecsToUpdate = DistanceGetCountRecsToUpdate(sFilename, sCountry, "Y");
            iZipsToDoEnd = iRecsToUpdate;

            iZipsUpdated = iZipsToDoStart - iZipsToDoEnd;

            sLastCodeProcessed = saNextRec[0];
            lbRecs.Text = "Records processed... " + iGoogleCallCount.ToString() + "   ---> Last zip from: " + saNextRec[0] + "   ---> Last zip to: " + saNextRec[1];
            ltPageSource.Text = sPageSource;

            endTime = DateTime.Now;
            ts = endTime - startTime;
            sRunTime = ts.Minutes.ToString() + " Min... " + ts.Seconds.ToString() + " Sec...";
            lbTime.Text = "Run Time: " + sRunTime;

            lbNextCode.Text = "Next Code To Process <b>" + saNextRec[0] + "</b> (Remaining Recs " + iRecsToUpdate + ")" + " GoogleCallCount: " + iGoogleCallCount + "  UpdateCount: " + iZipsUpdated;

            MailHandler mh = new MailHandler();
            mh.EmailIndividual("Public Zip Email: Next is " + saNextRec[0] + "  Updated: " + iZipsUpdated, lbNextCode.Text, "htslog@yahoo.com", "HTML");
            mh = null;

        }
        catch (Exception ex)
        {
            lbMsg.Text += "Last Code Processed: " + sLastCodeProcessed.ToString() + " -- " + ex.Message.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), lbMsg.Text);
        }
        finally
        {
            odbcConn.Close();
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
                iRowsAffected = DistanceSetZeroResults(sFilename, code1, code2);
/*
                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsLiveApi.DistanceSetZeroResults(sFilename, code1, code2);
                }
                else
                {
                    iRowsAffected = wsTestApi.DistanceSetZeroResults(sFilename, code1, code2);
                }
*/ 
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
                
                iRowsAffected = DistanceSetMilesTime(sFilename, code1, code2, dMiles, dHoursMinutes);
                /*
                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsLiveApi.DistanceSetMilesTime(sFilename, code1, code2, dMiles, dHoursMinutes);
                }
                else
                {
                    iRowsAffected = wsTestApi.DistanceSetMilesTime(sFilename, code1, code2, dMiles, dHoursMinutes);
                }
                */
            }
        }
        catch (Exception ex)
        {
            //            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text += "Xml Error: " + ex.Message.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
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
        ProcessRecs("", 0);
    }




    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    public string[] DistanceGetNextRec(string filename, string country)
    {
        string[] saLatLng = new string[6];
        try
        {
            // need to add country to 2ndary SQL!!!
            if (filename == "ZIP2MKT" || filename == "ZIP2CTR" || filename == "ZIP2FST")
            {
                if (filename == "ZIP2MKT")
                {
                    sSql = "select distinct Z2MZP1 as cod1, Z2MZP2 as cod2, ZALAT as lat1, ZALON as lng1, Z2MLAT as lat2, Z2MLON as lng2" +
                    " from " + sLibrary + ".ZIP2MKT, " + sLibrary + ".ZIPALL" +
                    " where ZAZIP = Z2MZP1" +
                    " AND Z2MUPD = 'Y'" +
                    " AND Z2MZP1 in (select min(z2mzp1) from " + sLibrary + ".ZIP2MKT, " + sLibrary + ".ZIPALL where Z2MZP1 = ZAZIP and Z2MUPD = 'Y'";
                    if (country != "")
                    {
                        sSql += " and ZANAT = ?)";
                        sSql += " and ZANAT = ?";
                    }
                    else
                        sSql += ")";
                }
                else if (filename == "ZIP2CTR")
                {
                    sSql = "select distinct Z2CZP1 as cod1, Z2CZP2 as cod2, ZALAT as lat1, ZALON as lng1, Z2CLAT as lat2, Z2CLON as lng2, Z2CTYP" +
                        " from " + sLibrary + ".ZIP2CTR, " + sLibrary + ".ZIPALL" +
                        " where ZAZIP = Z2CZP1" +
                        " AND Z2CUPD = 'Y'" +
                        " AND Z2CZP1 in (select min(z2czp1) from " + sLibrary + ".ZIP2CTR, " + sLibrary + ".ZIPALL where Z2CZP1 = ZAZIP and Z2CUPD = 'Y'";
                    if (country != "")
                    {
                        sSql += " and ZANAT = ?)";
                        sSql += " and ZANAT = ?";
                    }
                    else
                        sSql += ")";
                    sSql += " order by Z2CTYP";
                }
                else if (filename == "ZIP2FST")
                {
                    sSql = "select distinct Z2FZP1 as cod1, Z2FZP2 as cod2, ZALAT as lat1, ZALON as lng1, Z2FLAT as lat2, Z2FLON as lng2, Z2FRNK" +
                        " from " + sLibrary + ".ZIP2FST, " + sLibrary + ".ZIPALL" +
                        " where ZAZIP = Z2FZP1" +
                        " AND Z2FUPD = 'Y'" +
                        " AND Z2FZP1 in (select min(z2fzp1) from " + sLibrary + ".ZIP2FST, " + sLibrary + ".ZIPALL where Z2FZP1 = ZAZIP and Z2FUPD = 'Y'";
                    if (country != "")
                    {
                        sSql += " and ZANAT = ?)";
                        sSql += " and ZANAT = ?";
                    }
                    else
                        sSql += ")";
                    sSql += " order by Z2FRNK";
                }

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                if (country != "")
                {
                    odbcCmd.Parameters.Add("@Country1", OdbcType.VarChar, 10);
                    odbcCmd.Parameters["@Country1"].Value = country;

                    odbcCmd.Parameters.Add("@Country2", OdbcType.VarChar, 10);
                    odbcCmd.Parameters["@Country2"].Value = country;
                }

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

                string sLoaded = "N";
                if (odbcReader.HasRows)
                {
                    while (odbcReader.Read())
                    {
                        if (sLoaded == "N") // only take 1st match found (if mult)
                        {
                            saLatLng[0] = odbcReader["cod1"].ToString().Trim();
                            saLatLng[1] = odbcReader["cod2"].ToString().Trim();
                            saLatLng[2] = odbcReader["lat1"].ToString().Trim();
                            saLatLng[3] = odbcReader["lng1"].ToString().Trim();
                            saLatLng[4] = odbcReader["lat2"].ToString().Trim();
                            saLatLng[5] = odbcReader["lng2"].ToString().Trim();
                        }
                        sLoaded = "Y";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return saLatLng;
    }
    // ========================================================================
    public int DistanceGetCountRecsToUpdate(string filename, string country, string flag)
    {
        int iRecsToUpdate = 0;
        try
        {
            // need to add country to 2ndary SQL!!!
            if (filename == "ZIP2MKT" || filename == "ZIP2CTR" || filename == "ZIP2FST")
            {
                if (flag == "")
                    flag = "Y";

                if (filename == "ZIP2MKT")
                {
                    sSql = "select count(z2mzp1) as recCount" +
                    " from " + sLibrary + ".ZIP2MKT" +
                    " where z2mupd = '" + flag + "'" +
                    " AND substring(z2mzp1,1,1)";
                    if (country == "CANADA") { sSql += " not"; }
                    sSql += " in ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')";
                }
                else if (filename == "ZIP2CTR")
                {
                    sSql = "select count(z2czp1) as recCount" +
                    " from " + sLibrary + ".ZIP2CTR" +
                    " where z2cupd = '" + flag + "'" +
                    " AND substring(z2czp1,1,1)";
                    if (country == "CANADA") { sSql += " not"; }
                    sSql += " in ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')";
                }
                else if (filename == "ZIP2FST")
                {
                    sSql = "select count(z2fzp1) as recCount" +
                    " from " + sLibrary + ".ZIP2FST" +
                    " where z2fupd = '" + flag + "'" +
                    " AND substring(z2fzp1,1,1)";
                    if (country == "CANADA") { sSql += " not"; }
                    sSql += " in ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')";
                }

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

                string sLoaded = "N";
                if (odbcReader.HasRows)
                {
                    while (odbcReader.Read())
                    {
                        if (sLoaded == "N") // only take 1st match found (if mult)
                        {
                            int.TryParse(odbcReader["recCount"].ToString().Trim(), out iRecsToUpdate);
                        }
                        sLoaded = "Y";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRecsToUpdate;
    }
    // ========================================================================
    public int DistanceSetMilesTime(string filename, string code1, string code2, double miles, double hoursMinutes)
    {
        int iRowsAffected = 0;

        try
        {
            if (filename == "ZIP2MKT" || filename == "ZIP2CTR" || filename == "ZIP2FST")
            {
                if (filename == "ZIP2MKT")
                {
                    sSql = "Update " + sLibrary + ".ZIP2MKT set" +
                        " Z2MMIL = ?" +
                        ", Z2MTIM = ?" +
                        ", Z2MUPD = 'N'" +
                        " where Z2MZP1 = ?" +
                        " and Z2MZP2 = ?";
                }
                else if (filename == "ZIP2CTR")
                {
                    sSql = "Update " + sLibrary + ".ZIP2CTR set" +
                        " Z2CMIL = ?" +
                        ", Z2CTIM = ?" +
                        ", Z2CUPD = 'N'" +
                        " where Z2CZP1 = ?" +
                        " and Z2CZP2 = ?";

                }
                else if (filename == "ZIP2FST")
                {
                    sSql = "Update " + sLibrary + ".ZIP2FST set" +
                        " Z2FMIL = ?" +
                        ", Z2FTIM = ?" +
                        ", Z2FUPD = 'N'" +
                        " where Z2FZP1 = ?" +
                        " and Z2FZP2 = ?";
                }

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Miles", OdbcType.Double);
                odbcCmd.Parameters["@Miles"].Value = miles;

                odbcCmd.Parameters.Add("@HoursMinutes", OdbcType.Double);
                odbcCmd.Parameters["@HoursMinutes"].Value = hoursMinutes;

                odbcCmd.Parameters.Add("@Code1", OdbcType.VarChar, 10);
                odbcCmd.Parameters["@Code1"].Value = code1;

                odbcCmd.Parameters.Add("@Code2", OdbcType.VarChar, 10);
                odbcCmd.Parameters["@Code2"].Value = code2;

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int DistanceSetZeroResults(string filename, string code1, string code2)
    {
        int iRowsAffected = 0;

        if (code1 != "" && code2 != "")
        {
            try
            {
                if (filename == "ZIP2MKT" || filename == "ZIP2CTR" || filename == "ZIP2FST")
                {
                    if (filename == "ZIP2MKT")
                    {
                        sSql = "Update " + sLibrary + ".ZIP2MKT set" +
                            " Z2MUPD = 'Z'" +
                            " where Z2MZP1 = ?" +
                            " and Z2MZP2 = ?";
                    }
                    else if (filename == "ZIP2CTR")
                    {
                        sSql = "Update " + sLibrary + ".ZIP2CTR set" +
                            " Z2CUPD = 'Z'" +
                            " where Z2CZP1 = ?" +
                            " and Z2CZP2 = ?";

                    }
                    else if (filename == "ZIP2FST")
                    {
                        sSql = "Update " + sLibrary + ".ZIP2FST set" +
                            " Z2FMIL = 99999.99" +
                            ", Z2FTIM = 999.99" +
                            ", Z2FUPD = 'Z'" +
                            " where Z2FZP1 = ?" +
                            " and Z2FZP2 = ?";
                    }

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@Code1", OdbcType.VarChar, 10);
                    odbcCmd.Parameters["@Code1"].Value = code1;

                    odbcCmd.Parameters.Add("@Code2", OdbcType.VarChar, 10);
                    odbcCmd.Parameters["@Code2"].Value = code2;

                    iRowsAffected = odbcCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string sErrValues = "Filename: " + filename + " ----- Code1: " + code1 + " ----- Code2: " + code2;
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update " + sErrorValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public string ZipGetNext(string country)
    {
        string sNextCode = "";

        try
        {

            sSql = "SELECT MIN(ZAZIP) as NextCode" +
            " from " + sLibrary + ".ZIPALL" +
            " where ZAUPD = 'Y'";
            if (country != "")
                sSql += " and ZANAT = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            if (country != "")
            {
                odbcCmd.Parameters.Add("@Country", OdbcType.VarChar, 10);
                odbcCmd.Parameters["@Country"].Value = country;
            }

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            if (odbcReader.HasRows)
            {
                while (odbcReader.Read())
                {
                    sNextCode = odbcReader["NextCode"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sNextCode;
    }
    // ========================================================================
    public DataTable ZipCaGetSet(string nextChar)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "SELECT" +
                " ZAZIP" +
                " from " + sLibrary + ".ZIPALL" +
                " where ZANAT = 'CANADA'" +
                " and ZAUPD = 'Y'" +
                " and SUBSTRING(ZAZIP, 1, 1) = ?" +
                " order by ZAZIP";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@NextChar", OdbcType.VarChar, 10);
            odbcCmd.Parameters["@NextChar"].Value = nextChar;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }

    // ========================================================================
    public int ZipCaUpdGood(string postalCode, string city, string provinceCode, string provinceName, string county, double latitude, double longitude)
    {
        int iRowsAffected = 0;

        try
        {

            sSql = "update " + sLibrary + ".ZIPALL set" +
             " ZALAT = ?" +
            ", ZALON = ?" +
            ", ZAUPD = ?";
            if (city != "")
            {
                sSql += ", ZACIT = ?";
            }
            if (provinceCode != "")
            {
                sSql += ", ZASTA = ?";
            }
            if (provinceName != "")
            {
                sSql += ", ZASTN = ?";
            }
            if (county != "")
            {
                sSql += ", ZACOU = ?";
            }
            sSql += ", ZARGN = 90" +
            ", ZACTR = 917" +
            ", ZASVC = 'SUBCONTACT'" +
            ", ZAZON = 'D'" +
            " where ZAZIP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Latitude", OdbcType.Double);
            odbcCmd.Parameters["@Latitude"].Value = latitude;

            odbcCmd.Parameters.Add("@Longitude", OdbcType.Double);
            odbcCmd.Parameters["@Longitude"].Value = longitude;

            odbcCmd.Parameters.Add("@Update", OdbcType.VarChar, 1);
            odbcCmd.Parameters["@Update"].Value = "N";

            if (city != "")
            {
                odbcCmd.Parameters.Add("@City", OdbcType.VarChar, 40);
                odbcCmd.Parameters["@City"].Value = city;
            }

            if (provinceCode != "")
            {
                odbcCmd.Parameters.Add("@ProvinceCode", OdbcType.VarChar, 2);
                odbcCmd.Parameters["@ProvinceCode"].Value = provinceCode;
            }

            if (provinceName != "")
            {
                odbcCmd.Parameters.Add("@ProvinceName", OdbcType.VarChar, 30);
                odbcCmd.Parameters["@ProvinceName"].Value = provinceName;
            }

            if (county != "")
            {
                odbcCmd.Parameters.Add("@County", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@County"].Value = county;

            }

            odbcCmd.Parameters.Add("@PostalCode", OdbcType.VarChar, 10);
            odbcCmd.Parameters["@PostalCode"].Value = postalCode;

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string sErrorValues = "Lat: " + latitude.ToString() +
                " Lon: " + longitude.ToString() +
                " Cit: " + city +
                " Cou: " + county +
                " Cod: " + postalCode;
            //sErrorValues = HttpUtility.HtmlEncode(sErrorValues);
            //sErrorValues = HttpUtility.HtmlEncode(sErrorValues);
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update " + sErrorValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int ZipCaUpdBad(string postalCode, string statusCode)
    {
        int iRowsAffected = 0;

        try
        {
            sSql = "update " + sLibrary + ".ZIPALL set" +
            " ZAUPD = ?";
            if (statusCode == "Z")
                sSql += ", ZASTS = 'INACTIVE'";
            sSql += " where ZAZIP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Update", OdbcType.VarChar, 1);
            odbcCmd.Parameters["@Update"].Value = statusCode;

            odbcCmd.Parameters.Add("@PostalCode", OdbcType.VarChar, 10);
            odbcCmd.Parameters["@PostalCode"].Value = postalCode;

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Public Zip Update");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }

    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ===========================================================
    // ===========================================================
}

