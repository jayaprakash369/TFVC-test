using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SiteHandler
/// </summary>
public class SiteHandler
{
    // ========================================================================
    public SiteHandler()
    {
         // TODO: Add constructor logic here
     }
    // ========================================================================
    public string getWebSite()
    {
        string sSite = "LIVE";
        string sUserURL = HttpContext.Current.Request.Url.ToString();

        if (sUserURL.Contains("localhost"))
            sSite = "DEV";
        else if (sUserURL.Contains(":81") || sUserURL.Contains(":444") || sUserURL.Contains(":490"))
            sSite = "DEV";
        else if (sUserURL.Contains("e2.scantronts.com") || sUserURL.Contains("htsweb2"))
            sSite = "TEST";

        return sSite;
    }
    // ========================================================================
    public string getLibrary()
    {
        string sLib = "OMDTALIB";
        string sUserURL = HttpContext.Current.Request.Url.ToString();

        if (sUserURL.Contains("localhost"))
            sLib = "OMTDTALIB";
        else if (
                sUserURL.Contains(":81")
            || sUserURL.Contains(":444")
            || sUserURL.Contains(":381")
            || sUserURL.Contains(":1381")
            || sUserURL.Contains(":481")
            || sUserURL.Contains(":1481")
            || sUserURL.Contains(":490")
            )
            sLib = "OMTDTALIB";
        else if (sUserURL.Contains("htsweb2") 
            || sUserURL.Contains("e2.scantronts.com"))
            sLib = "OMTDTALIB";

        // sLib = "OMDTALIB"; // Override To Force EVERYTHING to Live (Error, Ajax, Database)

        return sLib;
    }

    // ========================================================================
    // ========================================================================
}