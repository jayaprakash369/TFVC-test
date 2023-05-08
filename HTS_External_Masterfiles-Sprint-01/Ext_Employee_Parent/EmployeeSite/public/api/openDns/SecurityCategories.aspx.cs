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

public partial class public_api_openDns_SecurityCategories : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    protected static ErrorHandler eh;

    //string sApiKey = "C72A49CD82889B273BC844053158270E";
    //string sToken = "D24E19E4E589F9C7D9AE19241258F7C5";
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

            ddDnsCustomers.DataSource = sod.GetOpenDnsCustomers();
            ddDnsCustomers.DataTextField = "ocDnsNam";
            ddDnsCustomers.DataValueField = "ocDnsCs1";
            ddDnsCustomers.DataBind();
            //ddTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));

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
            response = "{\"securityItems\" :" + response + "}";  // I added this to the front and end of the response to make it easier for me to parse the result
            SecurityList securityList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<SecurityList>(response);
            if (securityList != null)
            {
                sTemp = "<table class=\"tableWithLines\">";
                sTemp += "<tr>";
                sTemp += "<th>Timestamp</th>";
                sTemp += "<th>Origin Id</th>";
                sTemp += "<th>Query Type</th>";
                sTemp += "<th>Client Ip</th>";
                sTemp += "<th>Handling</th>";
                sTemp += "<th>Remote Ip</th>";
                sTemp += "<th>Block Categories</th>";
                sTemp += "<th>Categories</th>";
                sTemp += "<th>Domain</th>";
                sTemp += "<th>Origin</th>";
                sTemp += "<th>Origin Type</th>";
                sTemp += "</tr>";

                string sTitle = "";
                string sCategories = "";

                long lUnixTimestamp = 0;
                DateTime unixYear0 = new DateTime(1970, 1, 1);
                long unixTimeStampInTicks = 0;
                DateTime dtUnix;

                Sql_OpenDns sod = new Sql_OpenDns();
                int iCode = 0;
                int i = 0;
                // SecurityItem is name of the child class
                // securityList is name of the instance of the parent class just created above
                // securityItems is name of list object created in the parent class
                foreach (SecurityItem si in securityList.securityItems) // securityItems is name of list object created in the parent class
                {
                    sTemp += "<tr>";
                    if (long.TryParse(si.timestamp, out lUnixTimestamp) == false)
                    {
                        lUnixTimestamp = 0;
                        //sTemp += "<td>&nbsp;</td>";
                    }
                    else 
                    {
                        lUnixTimestamp /= 1000;  // convert from milliseconds to seconds
                        unixTimeStampInTicks = lUnixTimestamp * TimeSpan.TicksPerSecond;
                        dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
                        sTemp += "<td>" + dtUnix.ToString("MMM dd, yyyy HH:mm") + "</td>";
                    }
                    
                    sTemp += "<td>" + si.originId + "</td>";
                    sTemp += "<td>" + si.queryType + "</td>";
                    sTemp += "<td>" + si.clientIp + "</td>";
                    sTemp += "<td>" + si.handling + "</td>";
                    sTemp += "<td>" + si.remoteIp + "</td>";
                    // sTemp += "<td>" + si.blockCategories + "</td>";
                    sTemp += "<td>";
                    sCategories = "";
                    for (i = 0; i < si.blockCategories.Length; i++)
                    {
                        if (int.TryParse(si.blockCategories[i], out iCode) == false)
                            iCode = 0;
                        if (iCode > 0) {
                            if (!String.IsNullOrEmpty(sCategories))
                                sCategories += ", ";
                            sTitle = sod.GetOpenDnsCategoryTitle(iCode);
                            if (!String.IsNullOrEmpty(sTitle))
                                sCategories += sTitle;
                            else 
                                sCategories += "(" + si.blockCategories[i] + ")";
                        }
                            
                    }
                    sTemp += sCategories + "</td>";
                    //sTemp += "<td>" + si.categories + "</td>";
                    sTemp += "<td>";
                    sCategories = "";
                    for (i = 0; i < si.categories.Length; i++)
                    {
                        if (int.TryParse(si.categories[i], out iCode) == false)
                            iCode = 0;
                        if (iCode > 0) 
                        {
                            if (!String.IsNullOrEmpty(sCategories))
                                sCategories += ", ";
                            sTitle = sod.GetOpenDnsCategoryTitle(iCode);
                            if (!String.IsNullOrEmpty(sTitle))
                                sCategories += sTitle;
                            else
                                sCategories += "(" + si.categories[i] + ")";
                        }
                    }
                    sTemp += sCategories + "</td>";
                    sTemp += "<td>" + si.domain + "</td>";
                    sTemp += "<td>" + si.origin + "</td>";
                    sTemp += "<td>" + si.originType + "</td>";
                    sTemp += "</tr>";
                }

                sTemp += "</table>";
                
                sod = null;
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
    public class SecurityList
    {
        public List<SecurityItem> securityItems { get; set; }
    }
    // ========================================================================
    public class SecurityItem
    {
        public string timestamp { get; set; }
        public string originId { get; set; }
        public string queryType { get; set; }
        public string clientIp { get; set; }
        public string handling { get; set; }
        public string remoteIp { get; set; }
        public string[] blockCategories { get; set; }
        public string[] categories { get; set; }
        public string domain { get; set; }
        public string origin { get; set; }
        public string originType { get; set; }
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
            string sHandlingCode = ddFilter.SelectedValue; // "-1";
            string sLimit = ddRecs.SelectedValue;
            string sCategoryList = "";
            //sCategoryList = ""; // 94,96
            //if (sHandlingCode == "2048")
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

            // https://api.opendns.com/v3/organizations/<ORGANIZATIONID>/reports/topdomains?offset=0&limit=500&filters=\{"originId":"<ORIGIN_ID>","start":1378684800,"end":1379466000,"handlingCode":\[-1\],"filterNoisyDomains":"true"\}&api-key=<API_KEY>&token=<TOKEN>'
            //string sOrgId = sBuildingProductsId;
            string sOrgId = ddDnsCustomers.SelectedValue;
            string sUrl = "https://api.opendns.com/v3/organizations/" + sOrgId + "/reports/securitycategoryqueries?" +
                "offset=0" + 
                "&limit=" + sLimit + // 500
                "&filters={" +
                "\"start\":" + lStart.ToString() + 
                ",\"end\":" + lEnd.ToString() + 
                ",\"categories\":[" + sCategoryList + "]" +
                ",\"handlingCode\":[" + sHandlingCode + "]" +
                "}" +
                "&api-key=" + sOpenDnsApiKey + 
                "&token=" + sOpenDnsToken;
            // This worked...
            // https://api.opendns.com/v3/organizations/92877/reports/securitycategoryqueries?offset=0&limit=50&filters={"start":1430579018,"end":1433171018,"categories":[],"handlingCode":[-1]}&api-key=C72A49CD82889B273BC844053158270E&token=D24E19E4E589F9C7D9AE19241258F7C5 
//            Response.Write("<br /><br /><br />" + sUrl);
            string sResponse = ScreenScrape(sUrl);
            // lbMsg.Text = sResponse;
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
            string sTest = "";
        }
    }
    // ========================================================================
    protected void ddFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddFilter.SelectedValue == "2048")
        {
            pnCategories.Visible = true;
            pnCategoryDescriptions.Visible = true;
        }

        else
        {
            pnCategories.Visible = false;
            pnCategoryDescriptions.Visible = false;
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ===========================================================
    // ===========================================================
}

