<%@ Application Language="C#" %>

<script runat="server">
    // ==================================================================================
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    // ==================================================================================
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
    // ==================================================================================        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        SaveUnhandledException();
    }
    // ==================================================================================
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }
    // ==================================================================================
    void Session_End(object sender, EventArgs e) 
    {
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
        string sErrorValues = "Emp Unhandled Exception";

        //{
        Exception ex = Server.GetLastError();
        string sUserURL = HttpContext.Current.Request.Url.ToString();

        if (ex is HttpUnhandledException && ex.InnerException != null)
        {
            ex = ex.InnerException;
        }
        if (ex != null)
        {
            // Save Error to Web Error Log
            sErrorSummary = ex.Message.ToString().Trim();
            sErrorDescription = ex.ToString().Trim();

            Emp_LIVE.EmployeeMenuSoapClient wsLive = new Emp_LIVE.EmployeeMenuSoapClient();
            Emp_DEV.EmployeeMenuSoapClient wsTest = new Emp_DEV.EmployeeMenuSoapClient();

            string sUserId = "";
            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserId = mu.UserName;
            }
            else
                sUserId = "Anonymous";

            //sUserId = HttpContext.Current.Request.ServerVariables["LOGON_USER"]; ;
            string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //sErrorValues += ": " + HttpUtility.HtmlEncode(sUserURL.ToString().Trim());
            sErrorValues += ": " + sUserURL.ToString().Trim();

            string sLib = "L";
            if (sUserURL.Contains("localhost"))
                sLib = "T";
            else if ((sUserURL.Contains(":93")) || (sUserURL.Contains(":9093")))
                sLib = "T";
            else if ((sUserURL.Contains(":92")) || (sUserURL.Contains(":9092")))
                sLib = "T";
// Error messages I can do nothing about at this time...
            if (sErrorSummary.StartsWith("The file") && sErrorSummary.EndsWith("does not exist."))
            { 
                // do nothing now...
            }
            else 
            {
                if (sLib == "T")
                {
                    wsLive.SaveErrorText(sErrorSummary, sErrorDescription, sErrorValues, sUserId, sIpAddress, "DMZ EMP TEST Global");
                }
                else
                {
                    //string sProblem = "PROBLEM";
                    wsLive.SaveErrorText(sErrorSummary, sErrorDescription, sErrorValues, sUserId, sIpAddress, "DMZ EMP LIVE Global");
                    //wsTest.SaveErrorText(sErrorSummary, sErrorDescription, sErrorValues, sUserId, sIpAddress, "EMP DEV Global");
                }
            }
            
            // If error was deliberate...
            // Save Android IP data
           
            int iUser = 0;
            if (sErrorSummary.Contains("<html>Send IP Info"))
            { 
                // IpMsg:1862
                string sUser = "";
                int iIdx = 0;
                iIdx = sErrorSummary.IndexOf("IpMsg:");
                if (iIdx > -1) 
                {
                    iIdx += 6; // move to employee number
                    sUser = sErrorSummary.Substring(iIdx, 4);
                    if (int.TryParse(sUser, out iUser) == false)
                        iUser = 0;
                }
                if (iUser > 0) 
                {
                    SharedSqlHandler ssh = new SharedSqlHandler();
                    ssh.SaveUserIp(iUser, sIpAddress);
                    ssh = null;
                }
            }
            
            Server.ClearError();
            if (iUser == 0) // not a deliberate crash
                Response.Redirect("~/public/error/Default.aspx");
        }
        //}
    }
    // ==================================================================================
    // ==================================================================================       
</script>
