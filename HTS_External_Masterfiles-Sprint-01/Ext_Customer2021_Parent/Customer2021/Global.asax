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
        SetCurrentDatabase();
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
        //string sErrorSummary = "";
        //string sErrorDescription = "";
        //string sErrorValues = "Emp Unhandled Exception";

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
            string sErrorSummary = ex.Message.ToString().Trim();
            string sErrorDescription = ex.ToString().Trim();

            string sUserId = "";
            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserId = mu.UserName;
            }
            else
                sUserId = "Anonymous";

            // Error messages I can do nothing about at this time...
            if (sErrorSummary.StartsWith("The file") && sErrorSummary.EndsWith("does not exist."))
            { 
                // do nothing now...
            }
            else 
            {
                MyPage myPage = new MyPage();
                myPage.SaveError("Summary: \r\n" + sErrorSummary, "Description: \r\n" + sErrorDescription, "Previous Page: " + sUserURL + "\r\n\r\nUser: " + sUserId + "\r\n\r\nSending Page: Cust2021 Global.asax Error Handler");
                myPage = null;
            }

            Server.ClearError();
            //if (iUser == 0) // not a deliberate crash
                Response.Redirect("~/public/error/Default.aspx");
        }
    }
        // ----------------------------------------------------------------------------
    protected void SetCurrentDatabase()
    {
        // Microsoft defaults it's user and membership login system to a "default database"
        // if you only use that one Sql server database, all is well, 
        // But if you need to have a Production AND a Development database, you have to specify which database to use
        // This is my attempt to point membership to either the production or development database on startup.

        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();
        string sLibrary = "OMTDTALIB";
        if (
               sUserURL.Contains("servicecommand.com")
            || (sUserURL.Contains("scantronts.com") && !sUserURL.Contains("c2.scantronts.com"))
            || sUserURL.Contains("192.168.100.23:81")
            || sUserURL.Contains("192.168.100.23:4444")
            )
            sLibrary = "OMDTALIB";

        //sLibrary = "OMDTALIB"; // DEBUG: override while running in test to the live membership library

        System.Reflection.FieldInfo connectionStringField = Membership.Provider.GetType().GetField("_sqlConnectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (connectionStringField != null) 
        {
            if (sLibrary == "OMDTALIB")
                connectionStringField.SetValue(Membership.Provider, ConfigurationManager.ConnectionStrings["CustomerLiveSqlConnection"].ToString());
            else
                connectionStringField.SetValue(Membership.Provider, ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString());
        }

        System.Reflection.FieldInfo roleField = Roles.Provider.GetType().GetField("_sqlConnectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (roleField != null)
        {
            if (sLibrary == "OMDTALIB")
                roleField.SetValue(Roles.Provider, ConfigurationManager.ConnectionStrings["CustomerLiveSqlConnection"].ToString());
            else
                roleField.SetValue(Roles.Provider, ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString());
        }

        System.Reflection.FieldInfo profileField = ProfileManager.Provider.GetType().GetField("_sqlConnectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (profileField != null)
        {
            if (sLibrary == "OMDTALIB")
                profileField.SetValue(ProfileManager.Provider, ConfigurationManager.ConnectionStrings["CustomerLiveSqlConnection"].ToString());
            else
                profileField.SetValue(ProfileManager.Provider, ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString());
        }
        //EmailHandler emailHandler = new EmailHandler();
        //emailHandler.EmailIndividual("Global.asax Environment", "URL: " + sUserURL + " <br /> Lib: " + sLibrary, "steve.carlson@scantron.com", "adv320@scantron.com");
        //emailHandler = null;

    }
    // ----------------------------------------------------------------------------

    // ==================================================================================
    // ==================================================================================       
</script>
