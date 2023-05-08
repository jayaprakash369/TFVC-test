using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
//using System.Data.Odbc;
using System.Configuration;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;

public partial class public_api_openDns_Test1 : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    //string sLibrary = "OMDTALIB";

//    OdbcConnection odbcConn;
//    OdbcCommand odbcCmd;
//    OdbcDataReader odbcReader;

    protected static ErrorHandler eh;
    //char[] cSplitter = { '|' };
    //string sConnectionString = "";
    //string sSql = "";
    
    //DateTime datTemp;
    //int iRowsAffected = 0;
    //string sMethodName = "";
    string sOrgId = "";

    string sBuildingProductsId = "92877";

    /*
 * Umbrella Api
 * a) Top Domains Report 
 * b) Security Activity Report
 * The Umbrella API is RESTful and returns reporting data in a JSON format
 * 
 * cURL -X POST 'https://api.opendns.com/v3/users/me/tokens?api-key=C72A49CD82889B273BC844053158270E' -d '{"email":"htslog@yahoo.com","passphrase":"StJohn.2"}'
 * {"token":"D24E19E4E589F9C7D9AE19241258F7C5","user":6985111}
 * 
 * Keys
 * a) Email Address: Your OpenDNS account login for the Dashboard 
 *      htslog@yahoo.com
 * b) Account ID: The internal ID number associated with your account 
 *      user: 6985111
 *      token: D24E19E4E589F9C7D9AE19241258F7C5
 *      
 *      
 * c) API Key: A key to allow you access to the OpenDNS API 
 *      Orig Api Key:   C72A49CD82889B273BC844053158270E
 *      2nd Api Key:    351C5D30302B6D0CAC7F139F2595985C
 * d) Organization ID: An OpenDNS identifier for each customer organization.
 *      Building Products: 92877
 *      
 *  Use cURL to get token if needed
 *  $ cURL -X POST 'https://api.opendns.com/v3/users/me/tokens?api-key=C72A49CD82889B273BC844053158270E' -d '{"email":"htslog@yahoo.com","passphrase":"Rainier.1"}'
     *   
 */

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); this.RequireSSL = false; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ===========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
//        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
//        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack)
        {
            // RunApi();
            // Response.End();
        }
    }
    // ===========================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            return client.DownloadString(url);
        }
    }
    // ===========================================================
    protected void RunApi()
    {
        //sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        //DataTable dt = new DataTable(sMethodName);

        //datTemp = DateTime.Now;
        //int iDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        string sResponse = "";
        //string sResult = "";

        try
        {
            // The tutorial to break the json into an object came from this web site
            //http://www.aspforums.net/Threads/161080/How-to-parse-JSON-string-in-C-Net/
            //odbcConn.Open();
            sResponse = GetOriginIds();
            ltMsg.Text = sResponse;
            //JsonResult result = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<JsonResult>(json);
            // I added this to the front and end of the response to make it easier for me to parse the result
            sResponse = "{\"organizations\" :" + sResponse + "}";
            //JsonResult jsonObj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<JsonResult>(sResponse);
            OrganizationList orgList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OrganizationList>(sResponse);

            string sTemp = "";
            sTemp = "<br /><br /><table class=\"tableWithLines\">";
            sTemp += "<tr>";
            sTemp += "<th>NetworkId</th>";
            sTemp += "<th>UserId</th>";
            sTemp += "<th>Label</th>";
            sTemp += "<th>OriginId</th>";
            sTemp += "<th>IpAddress</th>";
            sTemp += "</tr>";

            int iObjIdx = 0;
            foreach (Organization org in orgList.organizations)
            {
                sTemp += "<tr>";
                sTemp += "<td>" + org.networkId + "</td>";
                sTemp += "<td>" + org.userId + "</td>";
                sTemp += "<td>" + org.label + "</td>";
                sTemp += "<td>" + org.originId + "</td>";
                sTemp += "<td>" + org.ipAddress + "</td>";
                sTemp += "</tr>";

                iObjIdx++;
            }

            sTemp += "</table>";
            lbMsg.Text = sTemp;

//            MailHandler mh = new MailHandler();
//            mh.EmailIndividual("Api Subject", "Api Content", "htslog@yahoo.com", "HTML");
//            mh = null;

        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
        }
    }
    // ===========================================================
    protected string GetOriginIds() // 
    /*
     * Networks are assigned an ‘originId’ by OpenDNS and a human-readable ‘label’, which is assigned by the
     * Dashboard administrator, under the “Network Name” field in the Dashboard.
     * An originId is the identifier given to any identity in the Umbrella Dashboard, including AD Computers, AD
     * Groups, AD Users, Mobile Devices, Networks, Roaming Computers and Sites. In this document we
     * primarily discuss originIds that are tied to Networks, but it’s important to know they exist for all identities
     * in the Dashboard.
     * Queries to get reporting data from the API are done with the ‘OriginID’.
     * 
     * cURL -X GET 'https://api.opendns.com/v3/organizations/<ORGANIZATION_ID>/networks?api-key=<API_KEY>&token=<TOKEN>'
     * cURL -X GET 'https://api.opendns.com/v3/organizations/92877/networks?api-key=C72A49CD82889B273BC844053158270E&token=D24E19E4E589F9C7D9AE19241258F7C5'
     * https://api.opendns.com/v3/organizations/92877/networks?api-key=C72A49CD82889B273BC844053158270E&token=D24E19E4E589F9C7D9AE19241258F7C5
     * 
     * */
    {
        string sUrl = "";
        string sResponse = "";
        //string sResult = "";

        sOrgId = sBuildingProductsId;

        sUrl = "https://api.opendns.com/v3/organizations/" + sOrgId + "/networks?api-key=" + sOpenDnsApiKey + "&token=" + sOpenDnsToken;

        try
        {
            sResponse = ScreenScrape(sUrl);


            // Use a literal so you see the source 
            // If you want it to generate only HTML display version then load into the label
            // lbMsg.Text = sResponse;
            // System.Threading.Thread.Sleep(600);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {

        }

        return sResponse;
    }
    // ========================================================================
    #region myClasses
    // ========================================================================
    public class OrganizationList
    {
        //public string version { get; set; }
        //public string id { get; set; }
        public List<Organization> organizations { get; set; }
    }
    // ========================================================================
    public class Organization
    {
        public string networkId { get; set; }
        public string parentId { get; set; }
        public string userId { get; set; }
        public string cidrBase { get; set; }
        public string prefixLength { get; set; }
        public string dynamic { get; set; }
        public string verified { get; set; }
        public string status { get; set; }
        public string verifycode { get; set; }
        public string stampCreated { get; set; }
        public string stampVerified { get; set; }
        public string label { get; set; }
        public string originId { get; set; }
        public string ipAddress { get; set; }
        public string statsEmailAddress { get; set; }
    }
    // ========================================================================
    #endregion // end myClasses
    // ========================================================================
    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    // ===========================================================
    protected void btRun_Click(object sender, EventArgs e)
    {
        RunApi();
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ===========================================================
    // ===========================================================
}

