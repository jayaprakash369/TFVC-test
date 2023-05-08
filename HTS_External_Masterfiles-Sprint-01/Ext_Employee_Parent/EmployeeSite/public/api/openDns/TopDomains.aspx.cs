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

public partial class public_api_openDns_TopDomains : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    protected static ErrorHandler eh;
    //string sOrgId = "";
    //string sBuildingProductsId = "92877";

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
            Api_OpenDns aod = new Api_OpenDns();
            //DataTable dt = aod.GetOpenDnsCustomers();
            //ddDnsCustomers.DataSource = sod.GetOpenDnsCustomers();
            ddDnsCustomers.DataSource = aod.GetOpenDnsCustomers();
            //ddDnsCustomers.DataTextField = "ocDnsNam";
            //ddDnsCustomers.DataValueField = "ocDnsCs1";
            ddDnsCustomers.DataTextField = "organizationName";
            ddDnsCustomers.DataValueField = "organizationId";
            ddDnsCustomers.DataBind();
            ddDnsCustomers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));

            DataTable dt = sod.GetOpenDnsCategories();

            cbxlCategories.DataSource = dt;
            cbxlCategories.DataTextField = "ocTitle";
            cbxlCategories.DataValueField = "ocCode";
            cbxlCategories.DataBind();

            gvCategories.DataSource = dt;
            gvCategories.DataBind();

            sod = null;
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

        try
        {
            response = "{\"websites\" :" + response + "}";  // I added this to the front and end of the response to make it easier for me to parse the result
            WebsiteList websiteList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<WebsiteList>(response);
            if (websiteList != null)
            {
                sTemp = "<table class=\"tableWithLines\">";
                sTemp += "<tr>";
                sTemp += "<th>Rank</th>";
                sTemp += "<th>Domain</th>";
                sTemp += "<th>Queries</th>";
                sTemp += "<th>Categories</th>";
                sTemp += "</tr>";

                int i = 0;
                foreach (Website website in websiteList.websites) // websites is name of list object from parent class
                {
                    sTemp += "<tr>";
                    sTemp += "<td>" + website.rank + "</td>";
                    sTemp += "<td>" + website.domain + "</td>";
                    sTemp += "<td>" + website.queries + "</td>";
//                    sTemp += "<td>" + website.categories + "</td>";
                    sTemp += "<td>";
                    for (i = 0; i < website.categories.Length; i++)
                    {
                        if (i > 0)
                            sTemp += ", ";
                        sTemp += website.categories[i];
                    }
                    sTemp += "</td>";
                    sTemp += "</tr>";
                }

                sTemp += "</table>";
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }


        return sTemp;
    }
    // ===========================================================
    public void LoadCustList(string response)
    {
        response = "{\"organizations\" :" + response + "}";  // I added this to the front and end of the response to make it easier for me to parse the result
        OrganizationList orgList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OrganizationList>(response);

        int iObjIdx = 0;
        foreach (Organization org in orgList.organizations)
        {
            ddDnsCustLocs.Items.Insert(0, new System.Web.UI.WebControls.ListItem(org.label, org.originId));
            iObjIdx++;
        }
        ddDnsCustLocs.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
    }
    // =========================================================
    protected long getUnixTimestamp(DateTime dat)
    {
        //DateTime datTemp = new DateTime(2013, 09, 09);
        long unixTimestamp = dat.Ticks - new DateTime(1970, 1, 1).Ticks;
        unixTimestamp /= TimeSpan.TicksPerSecond;

        return unixTimestamp;
    }
    // ========================================================================
    #region myClasses
    // ========================================================================
    public class WebsiteList
    {
        public List<Website> websites { get; set; }
    }
    // ========================================================================
    public class Website
    {
        public string rank { get; set; }
        public string domain { get; set; }
        public string queries { get; set; }
        public string[] categories { get; set; }
    }
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
            string sOriginId = ""; // 14196917 (Council bluffs for building products)
            string sLimit = ddRecs.SelectedValue;
            sOriginId = ddDnsCustLocs.SelectedValue; // "14196917"; 
            string sHandlingCode = ddFilter.SelectedValue; // "-1";
            string sFilterNoisyDomains = "true"; // true
            string sCategoryList = "";
            //sCategoryList = "7,8"; // 94,96
            //if (sHandlingCode == "2048" || sHandlingCode == "-1")
            //{
                foreach (ListItem li in cbxlCategories.Items)
                {
                    if (li.Selected == true)
                    {
                        if (!String.IsNullOrEmpty(sCategoryList))
                            sCategoryList += ",";
                        sCategoryList += li.Value;
                    }
                }
            //}


            //DateTime datTemp = new DateTime(2013, 09, 09);
            DateTime datTemp = DateTime.Now;
            long lEnd = getUnixTimestamp(datTemp);
            int iDays = 0;
            if (int.TryParse(ddDays.SelectedValue, out iDays) == false)
                iDays = 14;
            long lStart = getUnixTimestamp(datTemp.AddDays(-iDays));

            string sFilters = "";

            // https://api.opendns.com/v3/organizations/<ORGANIZATIONID>/reports/topdomains?offset=0&limit=500&filters=\{"originId":"<ORIGIN_ID>","start":1378684800,"end":1379466000,"handlingCode":\[-1\],"filterNoisyDomains":"true"\}&api-key=<API_KEY>&token=<TOKEN>'
            // How to get all our HTS customers https://api.opendns.com/v3/msps/1278219/customers?api-key=C72A49CD82889B273BC844053158270E&token=D24E19E4E589F9C7D9AE19241258F7C5
            //string sOrgId = sBuildingProductsId;
            string sOrgId = ddDnsCustomers.SelectedValue;
            string sUrl = "https://api.opendns.com/v3/organizations/" + sOrgId + "/reports/topdomains?" +
                "offset=0" +
                "&limit=" + sLimit + // 500
                "&filters={";
            if (!String.IsNullOrEmpty(sOriginId)) {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"originId\":\"" + sOriginId + "\"";
            }
            if (lStart > 0)
            {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"start\":" + lStart.ToString();
            }
            if (lEnd > 0)
            {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"end\":" + lEnd.ToString();
            }
            if (!String.IsNullOrEmpty(sHandlingCode))
            {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"handlingCode\":[" + sHandlingCode + "]";
            }
            if (!String.IsNullOrEmpty(sFilterNoisyDomains))
            {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"filterNoisyDomains\":\"" + sFilterNoisyDomains + "\"";
            }
            //if ((sHandlingCode == "2048" || sHandlingCode == "-1")  && !String.IsNullOrEmpty(sCategoryList))
            if (!String.IsNullOrEmpty(sCategoryList))
            {
                if (!String.IsNullOrEmpty(sFilters))
                    sFilters += ",";
                sFilters += "\"categories\":[" + sCategoryList + "]";
            }
                sUrl += sFilters + "}" +
                "&api-key=" + sOpenDnsApiKey + 
                "&token=" + sOpenDnsToken;

//            Response.Write("<br /><br /><br />" + sUrl);
            string sResponse = ScreenScrape(sUrl);
            //lbMsg.Text = sResponse;
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
    protected void ddDnsCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sOrgId = ddDnsCustomers.SelectedValue;

        try
        {
            DataTable dt = new DataTable();
            ddDnsCustLocs.DataSource = dt;
            ddDnsCustLocs.DataBind();

            if (!String.IsNullOrEmpty(sOrgId))
            {
                //ddDnsCustLocs.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));

                //string sOrgId = sBuildingProductsId;
                string sUrl = "https://api.opendns.com/v3/organizations/" + sOrgId + "/networks?" +
                    "api-key=" + sOpenDnsApiKey +
                    "&token=" + sOpenDnsToken;
                string sResponse = ScreenScrape(sUrl);
                LoadCustList(sResponse);
            }
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
    protected void ddFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddFilter.SelectedValue == "2048" || ddFilter.SelectedValue == "-1")
        {
            pnCategories.Visible = true;
            pnCategoryDescriptions.Visible = true;
        }
            
        else {
            pnCategories.Visible = false;
            pnCategoryDescriptions.Visible = false;
        }
            

    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ========================================================================
    // ========================================================================
}

