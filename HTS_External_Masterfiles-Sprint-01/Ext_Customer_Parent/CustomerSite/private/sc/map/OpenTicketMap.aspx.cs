using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Web.Security;

public partial class private_sc_map_OpenTicketMap : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

 //   ErrorHandler eh;
    string sConnectionString = "";
    string sSql = "";

    string sStatus = "";

    char[] cSplitter = { '|' };
    string[] saMapData = new string[1];
    string[] saNames = new string[1];
    string[] saAddresses = new string[1];
    string[] saAddr = new string[1];
    string[] saCityStZip = new string[1];
    string[] saLat = new string[1];
    string[] saLng = new string[1];

    double dLatCtr = 0.0;
    double dLngCtr = 0.0;
    double dLatSpan = 0.0;
    double dLngSpan = 0.0;
    double dScreenHeight = 0.45;

    int iZoom = 0;

    XmlDocument doc = new XmlDocument();

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e)
    { }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
     //   sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
     //   odbcConn = new OdbcConnection(sConnectionString);

        string sMapType = "";

        if (Request.QueryString["custMap"] != null && Request.QueryString["custMap"].ToString() != "")
        {
            sMapType = "Cs1";
            saMapData = Request.QueryString["custMap"].ToString().Trim().Split(cSplitter);
        }

        if (saMapData.Length > 0)
        {
                    GetCustAddresses();
                    BuildMap();
        }
    }
    // ========================================================================
    protected void BuildMap()
    {
        HtmlMeta meta1 = new HtmlMeta { Name = "viewport", Content = "initial-scale=1.0, user-scalable=no" };
        Header.Controls.Add(meta1);

        // New Paid Key
        Page.Header.Controls.Add(
            new LiteralControl(
            @"<script src='http://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyCYaxJlAzZcpd3PPubmktLQBFjHh8ELuYQ&sensor=false'></script>"

            )
        );
        // --------------------------------------
        string sScript = @"<script>" +
            "var map, marker, myLatLng;";
        // Added the map.API.geocode to our google account so we would not have a query limit usage error message. 
        // This seems to have solved the problem of not showing all the marks on the map. (06/12/2018) 
        sScript += "geocoder = new google.maps.Geocoder();";

        // THIS CREATES A MAP BASED ON A CENTER LAT LNG
        sScript += " function initialize() {" +
            "var mapOptions = {" +
                "zoom: " + iZoom.ToString() + "," +
                "center: new google.maps.LatLng(" + dLatCtr.ToString() + ", " + dLngCtr.ToString() + ")" +
            "};" +
            "map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);";

        for (int i = 0; i < saNames.Length; i++)
        {
            if (saLat[i] != "" && saLng[i] != "")
            {
                sScript += @"myLatLng = new google.maps.LatLng(" + saLat[i] + "," + saLng[i] + ");" +
                    //" alert('" + saNamesAddresses[0] + "');" + 
                    "marker = new google.maps.Marker({" +
                        "position: myLatLng," +
                        "map: map," +
                        "title: '" + saNames[i] + "\\n" + saAddr[i] + "\\n" + saCityStZip[i] + "'" +
                    "});";
            }
        }

        sScript += "}" +
        "google.maps.event.addDomListener(window, 'load', initialize);" +
    "</script>";
        // --------------------------------------
        Page.Header.Controls.Add(
            new LiteralControl(sScript));
        // --------------------------------------

    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected void GetCustAddresses()
    {
        string sCs1List = "";

        for (int i = 0; i < saMapData.Length; i++)
        {
            if (sCs1List != "")
                sCs1List += ",";
            sCs1List += saMapData[i];
        }

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                 " CUSTNM" +
                ", SADDR1" +
                ", SADDR2" +
                ", CITY" +
                ", STATE" +
                ", ZIPCD" +
                ", CLLAT" +
                ", CLLNG" +
                " from OMDTALIB.Custmast, OMDTALIB.CSTLATLNG" +
                " where CSTRNR in (" + sCs1List + ")" +
                " and CSTRCD = 0" +
                " and cstrnr = clcs1" +
                " and cstrcd = clcs2" +
                " Order by Zipcd";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            //odbcCmd.Parameters.AddWithValue("@Key", headerKey);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            string sTemp = "";
            string sNam = "";
            string sAd1 = "";
            string sAd2 = "";
            string sCit = "";
            string sSta = "";
            string sZip = "";

            //string sApiKey = "AIzaSyDZqbWEmwhYFrjhZomKF4oABzqp1etU_uI";
            int iRowIdx = 0;
            double dTempLat = 0.0;
            double dTempLng = 0.0;
            // New way to determine screen size
            double dLatHigh = 0.0;
            double dLatLow = 0.0;
            double dLngHigh = 0.0;
            double dLngLow = 0.0;

            string[] saLatLng = new string[2];
            if (dt.Rows.Count > 0)
            {
                saAddresses = new string[dt.Rows.Count];
                saNames = new string[dt.Rows.Count];
                saAddr = new string[dt.Rows.Count];
                saCityStZip = new string[dt.Rows.Count];
                saLat = new string[dt.Rows.Count];
                saLng = new string[dt.Rows.Count];

                foreach (DataRow row in dt.Rows)
                {
                    sNam = dt.Rows[iRowIdx]["CUSTNM"].ToString().Trim();
                    sAd1 = dt.Rows[iRowIdx]["SADDR1"].ToString().Trim();
                    sAd2 = dt.Rows[iRowIdx]["SADDR2"].ToString().Trim();
                    sCit = dt.Rows[iRowIdx]["CITY"].ToString().Trim();
                    sSta = dt.Rows[iRowIdx]["STATE"].ToString().Trim();
                    sZip = dt.Rows[iRowIdx]["ZIPCD"].ToString().Trim();
                    if (sZip.Length > 8)
                        sZip = sZip.Substring(0, 5);

                    sNam = sNam.Replace("'", "\\'"); // 58506 Junkman's Daughter with single quote breaks script
                    sNam = sNam.Replace("&", "\\&"); // 52124 A & M

                    saNames[iRowIdx] = sNam;
                    saAddresses[iRowIdx] = sAd1 + " " + sCit + " " + sSta + " " + sZip;

                    sTemp = sAd1;
                    sTemp = sTemp.Replace("'", "\\'");
                    sTemp = sTemp.Replace("&", "\\&");
                    saAddr[iRowIdx] = sTemp;

                    sTemp = sCit + ", " + sSta + " " + sZip;
                    sTemp = sTemp.Replace("'", "\\'");
                    sTemp = sTemp.Replace("&", "\\&");
                    saCityStZip[iRowIdx] = sTemp;

                    sTemp = sAd1 + " " + sAd2 + " " + sCit + " " + sSta + " " + sZip;
                    sTemp = sTemp.Replace("'", "\\'");
                    sTemp = sTemp.Replace("&", "\\&");
                    sTemp = sTemp.Replace("#", "");
                    sTemp = sTemp.Replace("  ", " ");
                    sTemp = sTemp.Replace(" ", "+");

                    saLat[iRowIdx] = dt.Rows[iRowIdx]["CLLAT"].ToString().Trim();
                    saLng[iRowIdx] = dt.Rows[iRowIdx]["CLLNG"].ToString().Trim();
                    if (double.TryParse(saLat[iRowIdx], out dTempLat) == false)
                        dTempLat = 0;
                    if (double.TryParse(saLng[iRowIdx], out dTempLng) == false)
                        dTempLng = 0;

                    if (dTempLat != 0 && dTempLng != 0)
                    {
                        //  iSuccesses++;
                        if (dTempLat > dLatHigh || dLatHigh == 0)
                            dLatHigh = dTempLat;
                        if (dTempLat < dLatLow || dLatLow == 0)
                            dLatLow = dTempLat;
                        if (dTempLng > dLngHigh || dLngHigh == 0)
                            dLngHigh = dTempLng;
                        if (dTempLng < dLngLow || dLngLow == 0)
                            dLngLow = dTempLng;
                    }
                    //   else                      
                    //   iFailures++;

                    iRowIdx++;
                }
            }

            dLatCtr = (dLatHigh + dLatLow) / 2;
            dLngCtr = (dLngHigh + dLngLow) / 2;
            dLatSpan = dLatHigh - dLatLow;
            dLngSpan = dLngHigh - dLngLow;

            if (dLngSpan > 30 || dLatSpan > (30 * dScreenHeight))
                iZoom = 5;
            else if (dLngSpan > 15 || dLatSpan > (15 * dScreenHeight))
                iZoom = 6;
            else if (dLngSpan > 7.5 || dLatSpan > (7.5 * dScreenHeight))
                iZoom = 7;
            else if (dLngSpan > 4 || dLatSpan > (4 * dScreenHeight))
                iZoom = 8;
            else if (dLngSpan > 2 || dLatSpan > (2 * dScreenHeight))
                iZoom = 9;
            else if (dLngSpan > 1 || dLatSpan > (1 * dScreenHeight))
                iZoom = 10;
            else if (dLngSpan > .5 || dLatSpan > (.5 * dScreenHeight))
                iZoom = 11;
            else if (dLngSpan > .30 || dLatSpan > (.25 * dScreenHeight))
                iZoom = 12;
            else if (dLngSpan > .15 || dLatSpan > (.10 * dScreenHeight))
                iZoom = 13;
            else if (dLngSpan > .10 || dLatSpan > (.05 * dScreenHeight))
                iZoom = 14;
            else
                iZoom = 15;
        }
     //   catch (Exception ex)
     //   {
      //      eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
     //   }
        finally
        {
            odbcCmd.Dispose();
        }
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region myXml
    // ===========================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            // set properties of the client
            return client.DownloadString(url);
        }
    }
    // ========================================================================
    protected string[] xmlQuickParse(string src)
    {
        string[] saLatLng = { "", "" };

        //    string sStatus = "";
        string sResult = "";
        //     xx = xx + 1; 

        try
        {
            doc.LoadXml(src);

            XmlNode root = doc.DocumentElement;
            if (root.SelectSingleNode("status").ChildNodes[0] != null)
            {
                sStatus = root.SelectSingleNode("status").ChildNodes[0].Value;
                if (sStatus == "OK")
                {
                    if (root.SelectSingleNode("result").SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").ChildNodes[0] != null)
                        saLatLng[0] = root.SelectSingleNode("result").SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").ChildNodes[0].Value;
                    if (root.SelectSingleNode("result").SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").ChildNodes[0] != null)
                        saLatLng[1] = root.SelectSingleNode("result").SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").ChildNodes[0].Value;
                }
                else
                    sResult = sStatus;
            }
            else
                sResult = "No status node";
        }
        catch (Exception ex)
        {
        //    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return saLatLng;
    }
    // ========================================================================
    #endregion // end myXml
    // ========================================================================
    // ========================================================================
}

