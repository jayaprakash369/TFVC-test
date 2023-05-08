using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

public partial class dev_Test : MyPage
{
    // ===================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // ===================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // Google explanation of how to use this...
        // http://code.google.com/apis/maps/documentation/distancematrix/
        // old link with key... string sURL = "http://maps.google.com/maps/geo?q=" + sAdr + "&output=csv&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ";
        
        // http and https examples
        // http://maps.googleapis.com/maps/api/distancematrix/output?parameters
        // https://maps.googleapis.com/maps/api/distancematrix/output?parameters
        
        // Required Parms...
        // origins (required) — One or more addresses and/or textual latitude/longitude values, separated with the pipe (|) character, from which to calculate distance and time. *
        // origins=Bobcaygeon+ON|41.43206,-81.38992
        // destinations (required) — One or more addresses and/or textual latitude/longitude values, separated with the pipe (|) character, to which to calculate distance and time.*
        // destinations=Darling+Harbour+NSW+Australia|24+Sussex+Drive+Ottawa+ON|Capitola+CA
        // Note: You may pass either an address or a latitude/longitude coordinate with the origins and destinations parameters. If you pass an address as a string, the service will geocode the string and convert it to a latitude-longitude coordinate to calculate directions. If you pass coordinates, ensure that no space exists between the latitude and longitude values.

        //int i = 0;
        //string sZip = i.ToString("00000");

        XmlDocument xmlBak = new XmlDocument();
        XmlNode nodeBak;
        XmlNodeReader nr;

        string sUrl = "http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Bellingham+WA+98229&destinations=Portland+OR+97220&sensor=false&units=imperial";

        string sXmlBak = ScreenScrape(sUrl);

        try
        {
            xmlBak.LoadXml(sXmlBak);
        }
        catch (Exception ex)
        {
            string sError = ex.ToString();
        }

        nodeBak = xmlBak.DocumentElement;

        nr = new XmlNodeReader(nodeBak);
        while (nr.Read())
        {
            // add code from OrderXmlUnload() from supnet/ord/default.aspx
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
    // ===================================================
    // ===================================================
}