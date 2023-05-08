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
        string sSite = "";
        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

        if (sUserURL.Contains("ws.harlandts.com"))
            sSite = "LIVE";
        //else if (sUserURL.Contains(":170"))
        //    sSite = "TEST";
        else if (sUserURL.Contains(":70"))
            sSite = "DEV";
        else 
            sSite = "DEV";

        return sSite;
    }
    // ========================================================================
    public string getLibrary()
    {
        string sLib = "";
        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

        if (sUserURL.Contains("ws.harlandts.com"))
            sLib = "OMDTALIB";
        else if (sUserURL.Contains("localhost") || sUserURL.Contains(":70"))
            sLib = "OMTDTALIB";
        else
            sLib = "OMTDTALIB";

        // sLib = "OMDTALIB"; // Override To Force EVERYTHING to Live (Error, Ajax, Database)

        // Stay in test
        // sLib = "OMTDTALIB";

        return sLib;
    }

    // ========================================================================
    // ========================================================================
}