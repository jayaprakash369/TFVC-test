using System;
using System.Collections.Generic;
using System.Web;
//using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for Api_OpenDns
/// Database Retrieval for Open DNS
/// </summary>
public class Api_OpenDns
{
    // ========================================================================
    // Global Variables
    // ========================================================================
    string sLibrary = "";

    //OdbcConnection odbcConn;
    //OdbcCommand odbcCmd;
    //OdbcDataReader odbcReader;

    ErrorHandler erh;

//    string sConnectionString = "";
//    string sSql = "";

    public string sApiKey = "C72A49CD82889B273BC844053158270E";
    public string sToken = "D24E19E4E589F9C7D9AE19241258F7C5";
    public string sHtsId = "1278219";

    string sMethodName = "";

    // ========================================================================
    // Constructor
    // ========================================================================
    public Api_OpenDns()
    {
//        SiteHandler sh = new SiteHandler();
//        sLibrary = sh.getLibrary();
//        sh = null;

    }
    // ========================================================================
    // PUBLIC METHODS
    // ========================================================================
    // ===========================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            return client.DownloadString(url);
        }
    }
    // ========================================================================
    public DataColumn MakeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
    // ===========================================================
    public DataTable FormatResponse(string response)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);
        DataRow dr;

        
        DateTime unixYear0 = new DateTime(1970, 1, 1);
        DateTime dtUnix;
        long unixTimeStampInTicks = 0;
        long lUnixTimestamp = 0;

        //DateTime datTemp;

        //string sTemp = "";

        dt.Columns.Add(MakeColumn("organizationId"));
        dt.Columns.Add(MakeColumn("organizationName"));
        dt.Columns.Add(MakeColumn("users"));
        dt.Columns.Add(MakeColumn("subscriptionId"));
        dt.Columns.Add(MakeColumn("packageId"));
        dt.Columns.Add(MakeColumn("packageName"));
        dt.Columns.Add(MakeColumn("packageInternalName"));
        dt.Columns.Add(MakeColumn("createdAt"));
        dt.Columns.Add(MakeColumn("modifiedAt"));

        try
        {
            response = "{\"customers\" :" + response + "}";  // I added this to the front and end of the response to make it easier for me to parse the result
            CustomerList customerList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<CustomerList>(response);
            if (customerList != null)
            {
                //int i = 0;
                foreach (Customer customer in customerList.customers) // websites is name of list object from parent class
                {
                    dr = dt.Rows.Add();
                    dr["organizationId"] = customer.organizationId;
                    dr["organizationName"] = customer.organizationName;
                    dr["users"] = customer.users;
                    dr["subscriptionId"] = customer.subscriptionId;
                    dr["packageId"] = customer.packageId;
                    dr["packageName"] = customer.packageName;
                    dr["packageInternalName"] = customer.packageInternalName;
                    if (long.TryParse(customer.createdAt, out lUnixTimestamp) == false)
                        lUnixTimestamp = 0;
                    else
                    {
                        unixTimeStampInTicks = lUnixTimestamp * TimeSpan.TicksPerSecond;
                        dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
                        dr["createdAt"] = dtUnix.ToString("o");
                    }
                    if (long.TryParse(customer.modifiedAt, out lUnixTimestamp) == false)
                        lUnixTimestamp = 0;
                    else
                    {
                        unixTimeStampInTicks = lUnixTimestamp * TimeSpan.TicksPerSecond;
                        dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
                        dr["modifiedAt"] = dtUnix.ToString("o");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

        return dt;
    }
    // ========================================================================
    #region myApis
    // ========================================================================
    public DataTable GetOpenDnsCustomers()
    {
        DataTable dt = new DataTable("");

        try
        {
            // How to get all our HTS customers https://api.opendns.com/v3/msps/1278219/customers?api-key=C72A49CD82889B273BC844053158270E&token=D24E19E4E589F9C7D9AE19241258F7C5
            string sUrl = "https://api.opendns.com/v3/msps/" + sHtsId + "/customers?" + "&api-key=" + sApiKey + "&token=" + sToken;
            string sResponse = ScreenScrape(sUrl);
            dt = FormatResponse(sResponse);

        }
        catch (Exception ex)
        {
            erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
        }
        return dt;
    }
     // ========================================================================
    #endregion // end myApis
    // ========================================================================
    // ========================================================================
    #region myClasses
    // ========================================================================
    public class CustomerList
    {
        public List<Customer> customers { get; set; }
    }
    // ========================================================================
    public class Customer
    {
        public string organizationId { get; set; }
        public string organizationName { get; set; }
        public string users { get; set; }
        public string subscriptionId { get; set; }
        public string packageId { get; set; }
        public string packageName { get; set; }
        public string packageInternalName { get; set; }
        public string createdAt { get; set; }
        public string modifiedAt { get; set; }
    }
    // ========================================================================
    #endregion // end myClasses
    // ========================================================================
    // ========================================================================
}