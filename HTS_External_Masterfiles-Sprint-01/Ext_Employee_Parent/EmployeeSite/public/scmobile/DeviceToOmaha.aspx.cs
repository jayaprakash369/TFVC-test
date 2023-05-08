using System;
using System.Collections.Specialized;

public partial class public_scmobile_DeviceToOmaha : MyPage
{
    // No connection
    string sResponseToDevice = "DMZ DEFAULT|";
    string sUser = "";
    string sJob = "";
    string sWsKey = "";
    string sFieldList = "";
    string sValueList = "";
    string sErrorText = "";
    string sDebug = "";

    int iUser = -1;

    ScMobile_LIVE.ScMobileMenuSoapClient wsLive = new ScMobile_LIVE.ScMobileMenuSoapClient();
    ScMobile_DEV.ScMobileMenuSoapClient wsDev = new ScMobile_DEV.ScMobileMenuSoapClient();

    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // -------------------------------------------------------------------------
        try 
        {
            NameValueCollection nvc = Request.Form;

            // --------------------------------
            sErrorText = "";
            string sUser = "";
            int iUser = -1;
            if (!string.IsNullOrEmpty(nvc["usr"])) {
                sUser = nvc["usr"];
                if (int.TryParse(sUser, out iUser) == false)
                    iUser = -1;
            }

            string sJob = "";
            if (!string.IsNullOrEmpty(nvc["job"]))
                sJob = nvc["job"];

            string sWsKey = "";
            if (!string.IsNullOrEmpty(nvc["wsk"]))
                sWsKey = nvc["wsk"];

            string sFieldList = "";
            if (!string.IsNullOrEmpty(nvc["fld"]))
                sFieldList = nvc["fld"];

            string sValueList = "";
            if (!string.IsNullOrEmpty(nvc["val"]))
            {
                sValueList = nvc["val"];
                sErrorText = sValueList;
                if (sErrorText.Length > 3000)
                    sErrorText = sErrorText.Substring(0, 3000).Trim();
            }
                

            // --------------------------------
            //sUser = "1862"; // 1803
            //iUser = 1862;
            //sJob = "Retrieve_Customers_All";
            //sLibrary = "OMTDTALIB";
            //sWsKey = "abc";

            // --------------------------------
            // FieldList and ValueList cannot be checked for "" (because sometimes they are)
            if (!String.IsNullOrEmpty(sWsKey) 
                && !String.IsNullOrEmpty(sJob)
                && iUser > -1)
            {
                sDebug = "ScMobile DMZ DeviceToOmaha Receiver Crash: Job " + sJob + " -- User: " + sUser + " -- Values: " + sErrorText;

                if (sLibrary == "OMDTALIB")
                {
                    sResponseToDevice = wsLive.Process_DeviceToOmahaJob(sWsKey, sUser, sJob, sFieldList, sValueList);
                }
                else
                {
                    sResponseToDevice = wsDev.Process_DeviceToOmahaJob(sWsKey, sUser, sJob, sFieldList, sValueList);
                }

            }
            else 
            {
                sResponseToDevice = "Invalid Request Values|";
            }
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            //erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "ScMobile DMZ DeviceToOmaha Receiver Crash: Job " + sJob + " -- User: " + sUser + " -- Values: " + sErrorText);
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sDebug);
            erh = null;
        }
        finally
        {
            // sResponseToDevice += ": Response From Omaha: " + DateTime.Now.ToString("t");

            Response.Write(sResponseToDevice);
        }
        Response.End();
    }
    // ========================================================================    
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }


    // ========================================================================
    // ========================================================================
}
