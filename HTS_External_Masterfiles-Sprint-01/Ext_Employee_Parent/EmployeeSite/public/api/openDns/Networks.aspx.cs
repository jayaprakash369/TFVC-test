using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;

public partial class public_api_openDns_Networks : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    protected static ErrorHandler eh;
    string sOrgId = "";
    string sBuildingProductsId = "92877";

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); this.RequireSSL = false; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ===========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Sql_OpenDns sod = new Sql_OpenDns();
            ddDnsCustomers.DataSource = sod.GetOpenDnsCustomers();
            sod = null;
            ddDnsCustomers.DataTextField = "ocDnsNam";
            ddDnsCustomers.DataValueField = "ocDnsCs1";
            ddDnsCustomers.DataBind();
            //ddDnsCustomers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            //ddDnsCustomers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("HTS", "2679505"));
            
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
    public string FormatResponse(string response)
    {
        string sTemp = "";

        response = "{\"organizations\" :" + response + "}";  // I added this to the front and end of the response to make it easier for me to parse the result
        OrganizationList orgList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OrganizationList>(response);

        sTemp = "<table class=\"tableWithLines\">";
        sTemp += "<tr>";
        sTemp += "<th>Label</th>";
        sTemp += "<th>User Id</th>";
        sTemp += "<th>Network Id</th>";
        sTemp += "<th>Origin Id</th>";
        sTemp += "<th>Ip Address</th>";
        sTemp += "<th>Cidr Base</th>";
        sTemp += "<th>Dynamic</th>";
        sTemp += "<th>Parent Id</th>";
        sTemp += "<th>Prefix Length</th>";
        sTemp += "<th>Stamp Created</th>";
        sTemp += "<th>Stamp Verified</th>";
        sTemp += "<th>Email Address</th>";
        sTemp += "<th>Status</th>";
        sTemp += "<th>Verified</th>";
        sTemp += "<th>Verify Code</th>";
        sTemp += "</tr>";

        int iObjIdx = 0;
        foreach (Organization org in orgList.organizations)
        {
            sTemp += "<tr>";
            sTemp += "<td>" + org.label + "</td>";
            sTemp += "<td>" + org.userId + "</td>";
            sTemp += "<td>" + org.networkId + "</td>";
            sTemp += "<td>" + org.originId + "</td>";
            sTemp += "<td>" + org.ipAddress + "</td>";
            sTemp += "<td>" + org.cidrBase + "</td>";
            sTemp += "<td>" + org.dynamic + "</td>";
            sTemp += "<td>" + org.parentId + "</td>";
            sTemp += "<td>" + org.prefixLength + "</td>";
            sTemp += "<td>" + org.stampCreated + "</td>";
            sTemp += "<td>" + org.stampVerified + "</td>";
            sTemp += "<td>" + org.statsEmailAddress + "</td>";
            sTemp += "<td>" + org.status + "</td>";
            sTemp += "<td>" + org.verified + "</td>";
            sTemp += "<td>" + org.verifycode + "</td>";
            sTemp += "</tr>";

            iObjIdx++;
        }

        sTemp += "</table>";

        return sTemp;
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

        try
        {
            //string sOrgId = sBuildingProductsId;
            string sOrgId = ddDnsCustomers.SelectedValue;
            string sUrl = "https://api.opendns.com/v3/organizations/" + sOrgId + "/networks?" + 
                "api-key=" + sOpenDnsApiKey + 
                "&token=" + sOpenDnsToken;
            string sResponse = ScreenScrape(sUrl);
            lbMsg.Text = FormatResponse(sResponse);

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
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ===========================================================
    // ===========================================================
}

