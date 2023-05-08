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
		//
		// TODO: Add constructor logic here
		//
	}
    // ========================================================================
    public string getWebSite()
    {
        string sSite = "LIVE";
        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

        if (
                  sUserURL.Contains("c2.scantronts")
               || sUserURL.Contains(":180") || sUserURL.Contains(":4180")
            )
            sSite = "TEST";
        else if (sUserURL.Contains("localhost")
            || sUserURL.Contains(":280") || sUserURL.Contains(":4280")
            )
            sSite = "DEV";
        else if (
                  sUserURL.Contains("oma-shrdd-w01")
               || sUserURL.Contains(":380") || sUserURL.Contains(":4380")
            )
            sSite = "DEV ISA";
        else if (
              sUserURL.Contains(":480") || sUserURL.Contains(":4480")
            )
            sSite = "DEV STEVE";

        return sSite;
    }
    // ========================================================================
    public string getLibrary()
    {
        string sLib = "L";
        string sUserURL = HttpContext.Current.Request.Url.ToString();

        if (
               sUserURL.Contains("localhost")
            || sUserURL.Contains("c2.scantronts")
            || sUserURL.Contains("oma-shrdd-w01")
            || sUserURL.Contains(":180") || sUserURL.Contains(":4180")
            || sUserURL.Contains(":280") || sUserURL.Contains(":4280")
            || sUserURL.Contains(":380") || sUserURL.Contains(":4380")
            || sUserURL.Contains(":480") || sUserURL.Contains(":4480")
            )
            sLib = "T";

        // sLib = "OMDTALIB"; // Override To Force EVERYTHING to Live (Error, Ajax, Database)

        return sLib;
    }
    // ========================================================================
    // ========================================================================
}