<%@ Application Language="C#" %>

<script runat="server">
    // ==================================================================================
    /*
    void Application_BeginRequest(object sender, EventArgs e)
    {
        string sUrl = Request.Url.ToString().ToLower();
        if (sUrl.Contains(".pdf"))
            Response.Redirect("https://www.scantron.com/resources/accelerated-business-partnerships/");
    }
    */
    // ==================================================================================
    void Application_Start(object sender, EventArgs e)
    {
        //string sAppStart = "";
        // Code that runs on application startup

    }
    // ==================================================================================
    void Application_End(object sender, EventArgs e)
    {
        //string sAppEnd = "";
        //  Code that runs on application shutdown

    }
    // ==================================================================================
    void Application_Error(object sender, EventArgs e)
    {
        //string sAppError = "";
        // Code that runs when an unhandled error occurs

        SaveUnhandledException();
    }
    // ==================================================================================
    void Session_Start(object sender, EventArgs e)
    {
        //string sSessionStart = "";
        // Code that runs when a new session is started
    }
    // ==================================================================================
    void Session_End(object sender, EventArgs e)
    {
        //string sSessionEnd = "";
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
    // ==================================================================================
    protected void SaveUnhandledException()
    {
        //uncomment in order to bypass logging when running locally.
        //if (!Request.IsLocal)
        string sErrorSummary = "";
        string sErrorDescription = "";
        string sErrorValues = "Cust Unhandled Exception";

        //{
        Exception ex = Server.GetLastError();
        string sUserURL = HttpContext.Current.Request.Url.ToString();
        string sPageNotFound = "";
        int iPosStart = 0;
        int iPosEnd = 0;

        if (ex is HttpUnhandledException && ex.InnerException != null)
        {
            ex = ex.InnerException;
        }
        if (ex != null)
        {
            // Save Error to Web Error Log
            // Koi Oct 20th, 2011 The state information is invalid for this page and might be corrupted. 
            // Koi Dec 31st 2018 -- Encoding the values so they will be saved and I can see them
            sErrorSummary = ex.Message.ToString().Trim();
            sErrorSummary = HttpUtility.HtmlEncode(sErrorSummary);
            sErrorDescription = ex.ToString().Trim();
            sErrorDescription = HttpUtility.HtmlEncode(sErrorDescription);

            // Redirect to specific error pages if created
            if (sErrorSummary.Contains("does not exist"))
            {
                iPosStart = sErrorSummary.LastIndexOf("/");
                iPosEnd = sErrorSummary.LastIndexOf("'");
                sPageNotFound = sErrorSummary.Substring(iPosStart + 1, (iPosEnd - iPosStart) - 1);

                //                The file '/Ext_Dev_Cst/private/sc/ServiceRequest.aspx' does not exist.
                Server.ClearError();
                Response.Redirect("~/public/error/404_PageNotFound.aspx?pg=" + sPageNotFound, false);
            }
            else
            {
                Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
                Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();

                string sUserId = "";
                if (User.Identity.IsAuthenticated)
                {
                    MembershipUser mu = Membership.GetUser();
                    sUserId = mu.UserName;
                }
                else
                    sUserId = "Anonymous";

                string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                string sSave = "YES";
                // Bypass inconsequential errors to disregard
                if (
                           //  (sUserURL.ToString().StartsWith("https://www.Scantronts.com/WebResource.axd?") == true)
                           (sErrorSummary == "The client disconnected.")
                        || (sErrorSummary == "The state information is invalid for this page and might be corrupted.")
                    )
                {
                    sSave = "NO";
                }

                if (sSave == "YES")
                {
                    sErrorValues += ": " + sUserURL.ToString().Trim();
                    sErrorValues = HttpUtility.HtmlEncode(sErrorValues);

                    string sLib = "L";
                    if (
                        sUserURL.Contains("localhost") // on a local PC
                        || sUserURL.Contains(":180") || sUserURL.Contains(":4180") // Live Server TEST
                        || sUserURL.Contains(":280") || sUserURL.Contains(":4280") // Dev Server Development
                        || sUserURL.Contains(":380") || sUserURL.Contains(":4380") // Dev Server Isabel
                        || sUserURL.Contains(":480") || sUserURL.Contains(":4480") // Dev Server Steve
                        )
                        sLib = "T";

                    if (sLib == "L")
                    {
                        wsLive.SaveErrorText(sErrorSummary, sErrorDescription, sErrorValues, sUserId, sIpAddress, "CST LIVE Global");
                    }
                    else
                    {
                        wsTest.SaveErrorText(sErrorSummary, sErrorDescription, sErrorValues, sUserId, sIpAddress, "CST DEV Global");
                    }
                }
                Server.ClearError();
                Response.Redirect("~/public/error/Default.aspx", false);
            }
        }
        //}
    }
    // ==================================================================================
    // ==================================================================================

</script>
