using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for KeyRetriever 
/// /// HOW TO CALL (create and destroy instance like ErrorHandler, MailHandler on init and close)
/// -------------------------------------
/// KeyRetriever kr = new KeyRetriever();
/// int iNewKey = kr.GetKey("TESTKEY");
/// kr = null;
/// -------------------------------------
/// </summary>
public class KeyRetriever
{
    private string sLibrary = "";
    // ==========================================================
    public KeyRetriever(string library)
    {
        sLibrary = library;
    }
    // ==========================================================

    public int GetKey(string fileName)
    {
        int iNewKey = 0;

        try
        {
            string sResponse = "";
            string sUrl = "";
            // --------------------------------------------------------
            if (sLibrary == "OMDTALIB")
                sUrl = "http://www.oursts.com:90/public/GetKey.aspx?nam=" + fileName.Trim();
            else
                sUrl = "http://oma-dev-int:490/public/GetKey.aspx?nam=" + fileName.Trim();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);

            request.Timeout = 1000000; // To stop it from timing outwhile debugging
            request.Method = "GET";
            // request.ContentType = "application/json";
            // request.Accept = "application/json";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException we)
            {
                response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                sResponse = new StreamReader(responseStream).ReadToEnd();
            }
            else
            {
                Stream responseStream = response.GetResponseStream();
                sResponse = new StreamReader(responseStream).ReadToEnd();
            }
            // --------------------------------------------------------
            if (int.TryParse(sResponse, out iNewKey) == false)
                iNewKey = -1;

        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
        }
        return iNewKey;
    }
    // ========================================================================
    // ========================================================================
}