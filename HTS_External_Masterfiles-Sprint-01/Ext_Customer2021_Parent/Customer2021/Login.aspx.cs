using System;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.Profile;

public partial class Login : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbError.Text = "";
        lbNotice.Text = "";

        if (ShowNewCompanyName() == true) 
        {
            lbCompanyName1.Text = "Secur-Serv's";
            lbCompanyName1.Text = "Secur-Serv";
        }
            
        else 
        {
            lbCompanyName1.Text = "Scantron Technology Solutions'";
            lbCompanyName1.Text = "STS";
        }

        if (!IsPostBack)
        {
            // string sDbName = GetMembershipDatabase();

            txUserID.Focus();
            pnNotice.Visible = false;

            if (Request.QueryString["m"] != null && Request.QueryString["m"].ToString() != "") 
            { 
                string sPassedCode = Request.QueryString["m"].ToString().Trim();
                if (sPassedCode == "c") 
                {
                    lbError.Text = "Account Confirmation Successful <br />(You are now authorized to login)";
                }
            }


            /* 
             */
            Set_TimeToShow_NotificationMessage(
                20220916,   // Date To Show Message
                12,          // Hour To Show Message
                20220918,   // Date To Hide Message
                15,         // Hour To Hide Message 
                20220918,   // Date To Say It Begins
                12          // Hour To Say It Begins
                ); 
            Set_TimeToHide_LoginBox(
                20220918,   // Date To Hide Login Box
                12,         // Hour To Hide Login Box
                20220918,   // Date To Show Login Box
                15          // Hour To Show Login Box
                );  


            if (Page.User.Identity.IsAuthenticated)
            {
                //lbMsg.Text = "The page you are trying to access requires greater privileges.<br />  Please log in using a different user account with higher credentials.";
            }
            txUserID.Focus();
        }
        else
        {
            txPassword.Focus();
        }
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    protected int Update_LoginCount(string userID)
    {
        int iRowsAffected = 0;
        string sSql = "";

        try
        {
            sSql = "Update " + sSqlDbToUse_Customer + ".aspnet_Users" +
                " set LoginCount = (1 + (Select LoginCount from " + sSqlDbToUse_Customer + ".aspnet_Users where UserName = @UserName1))" +
                " where UserName = @UserName2";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName1", userID);
            sqlCmd.Parameters.AddWithValue("@UserName2", userID);

            iRowsAffected = sqlCmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return iRowsAffected; 
    }
    // =========================================================
    protected int Get_BadPasswordCount(string userID)
    {
        int iBadPasswords = 0;
        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select FailedPasswordAttemptCount" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u, " + sSqlDbToUse_Customer + ".aspnet_Membership m" +
                " where u.UserId = m.UserId" +
                " and UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName", userID);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["FailedPasswordAttemptCount"].ToString().Trim(), out iBadPasswords) == false)
                        iBadPasswords = -1;
                }
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return iBadPasswords;
    }
    // =========================================================
    protected int Check_LockoutThreat(string userID, int currFailures)
    {
        int iMaxAttempts = Membership.MaxInvalidPasswordAttempts;
        int iRemainingAttempts = iMaxAttempts; // Start assuming all are left;
        double dTimeoutWindow = Convert.ToDouble(Membership.PasswordAttemptWindow); // Span in minutes to accumulate bad pwd count

        DateTime datStart = new DateTime();
        DateTime datEnd = new DateTime();
        DateTime datNow = new DateTime();

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "";

            sSql = "Select FailedPasswordAttemptWindowStart" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u, " + sSqlDbToUse_Customer + ".aspnet_Membership m" +
                " where u.UserId = m.UserId" +
                " and UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName", userID);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                if (dt.Rows.Count > 0)
                {
                    if (DateTime.TryParse(dt.Rows[0]["FailedPasswordAttemptWindowStart"].ToString(), out datStart) == true)
                    {
                        datStart = datStart.ToLocalTime();
                        datEnd = datStart.AddMinutes(dTimeoutWindow);
                        datNow = DateTime.Now;
                        if (datNow < datEnd)
                            iRemainingAttempts = iMaxAttempts - (currFailures - 1);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return iRemainingAttempts;
    }
    // =========================================================
    protected int Get_UserIdCs1(string userID)
    {
        int iCustomerNumber = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "";

            sSql = "Select PrimaryCs1" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName", userID);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["PrimaryCs1"].ToString().Trim(), out iCustomerNumber) == false)
                        iCustomerNumber = 0;
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return iCustomerNumber;
    }
    // ========================================================================
    protected DataTable Select_UserRecord(string userId)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";
        string sUserID = userId.Trim().ToLower();

        try
        {
            sSql = "Select" +
               " UserId" +
              ", UserName" +
              ", LoweredUserName" +
              ", MobileAlias" +
              ", IsAnonymous" +
              ", LastActivityDate" +
              ", AdminRequestor" +
              ", CompanyType" +
              ", Phone" +
              ", PhoneAreaCode" +
              ", PhonePrefix" +
              ", PhoneSuffix" +
              ", PhoneExtension" +
              ", Email" +
              ", LoginCount" +
              ", PrimaryCs1" +
              ", RegistrationDate" +
              ", FirstName" +
              ", LastName" +
              ", EmailConfirmed" +
              ", ConfirmationCode" +
              ", ConfirmationDeadline" +

                " from " + sSqlDbToUse_Customer + ".aspnet_Users" +
                " where lower(UserName) = @UserID";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserID", sUserID);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected string ws_Get_B1LoginAuthorizationForUserCustomerNumber_YN(string customerNumber) 
    {
        string sUserCustomerNumberIsAuthorizedToLogin = "";
        int iCustomerNumber = 0;
        if (int.TryParse(customerNumber, out iCustomerNumber) == false)
            iCustomerNumber = -1;
        if (iCustomerNumber > 0 && iCustomerNumber <= 9999999)
        {
            string sJobName = "Get_B1LoginAuthorizationForUserCustomerNumber_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

            sUserCustomerNumberIsAuthorizedToLogin = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sUserCustomerNumberIsAuthorizedToLogin;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
        string userURL = Request.Url.ToString();
        string last11 = userURL.Substring(userURL.Length - 11);

        if (userURL.EndsWith("ReturnUrl=/"))
        {
            if (userURL.StartsWith("http://localhost"))
            {
                Response.Redirect("http://localhost/private/menu.aspx", false);
            }
            else
            {
                Response.Redirect("~/private/menu.aspx", false);
                //Response.Redirect("~/default.aspx", false);
            }
        }
        // ------------------------------------
        this.RequireSSL = true;

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
    }
    // =========================================================
    protected string Set_TimeToShow_NotificationMessage(
        int dateToShowMessage, 
        int militaryHourToShowMessage, 
        int dateToHideMessage, 
        int militaryHourToHideMessage, 
        int dateToSayItBegins, 
        int militaryHourToSayItBegins)
    {
        string sResponse = "";
        lbNotice.Text = "";
        pnNotice.Visible = false;

        DateTime datNow = DateTime.Now;
        DateTime datToShowMessage; 
        DateTime datToHideMessage;
        DateTime datToSayItBegins;
        // Parse "Show Message" timestamp first
        string sDate = dateToShowMessage.ToString("00000000");
        string sHour = militaryHourToShowMessage.ToString("00");
        string sTimestamp = sDate.Substring(0, 4) + " - " + sDate.Substring(4, 2) + " - " + sDate.Substring(6, 2) + " " + sHour + ":00:00.000";

        if (DateTime.TryParse(sTimestamp, out datToShowMessage) == false)
        {
            sResponse = "Start date values could not convert to a valid date.";
        }
        else 
        {
            // Parse "Hide Message" timestamp next
            sDate = dateToHideMessage.ToString("00000000");
            sHour = militaryHourToHideMessage.ToString("00");
            sTimestamp = sDate.Substring(0, 4) + " - " + sDate.Substring(4, 2) + " - " + sDate.Substring(6, 2) + " " + sHour + ":00:00.000";
            if (DateTime.TryParse(sTimestamp, out datToHideMessage) == false) 
            {
                sResponse = "Stop date values could not convert to a valid date.";
            }
            else
            {
                // Finally parse "Say It Begins" timestamp 
                sDate = dateToSayItBegins.ToString("00000000");
                sHour = militaryHourToSayItBegins.ToString("00");
                sTimestamp = sDate.Substring(0, 4) + " - " + sDate.Substring(4, 2) + " - " + sDate.Substring(6, 2) + " " + sHour + ":00:00.000";
                if (DateTime.TryParse(sTimestamp, out datToSayItBegins) == false)
                {
                    sResponse = "Time to say it beings could not convert to a valid date.";
                }

                if (datNow > datToShowMessage && datNow < datToHideMessage)
                {
                    if (ShowNewCompanyName()) 
                    {
                        lbNotice.Text = "Secur-Serv";
                    }
                    else 
                    {
                        lbNotice.Text = "STS";
                    }
                    lbNotice.Text += 
                        " will be performing routine system maintenance" +
                        "<br />from " + datToSayItBegins.ToString("dddd MMMM d") + " at " + datToSayItBegins.ToString("h tt") +
                        "<br />to " + datToHideMessage.ToString("dddd MMMM d") + " at " + datToHideMessage.ToString("h tt") + " (CST)" +
                        "<br /><i> ServiceCOMMAND will be unavailable during this period.</i>";
                    pnNotice.Visible = true;
                    sResponse = "Notification ON";
                }
                else 
                {
                    sResponse = "Notification OFF";
                }
            }
        }

        // --------------------------------------------------------------------------------------
        return sResponse;
    }
    // =========================================================
    protected string Set_TimeToHide_LoginBox(int dateToHideBox, int militaryHourToHideBox, int dateToShowBox, int militaryHourToShowBox)
    {
        string sResponse = "";
        pnLoginBox.Visible = true;

        DateTime datNow = DateTime.Now;
        DateTime datToHideBox;
        DateTime datToShowBox;
        
        string sDate = dateToHideBox.ToString("00000000");
        string sHour = militaryHourToHideBox.ToString("00");
        string sTimestamp = sDate.Substring(0, 4) + " - " + sDate.Substring(4, 2) + " - " + sDate.Substring(6, 2) + " " + sHour + ":00:00.000";

        if (DateTime.TryParse(sTimestamp, out datToHideBox) == false)
        {
            sResponse = "Date values to hide login box could not convert to a valid date.";
        }
        else
        {
            sDate = dateToShowBox.ToString("00000000");
            sHour = militaryHourToShowBox.ToString("00");
            sTimestamp = sDate.Substring(0, 4) + " - " + sDate.Substring(4, 2) + " - " + sDate.Substring(6, 2) + " " + sHour + ":00:00.000";
            if (DateTime.TryParse(sTimestamp, out datToShowBox) == false)
            {
                sResponse = "Date values to show login box again could not convert to a valid date.";
            }
            else
            {
                if (datNow > datToHideBox && datNow < datToShowBox)
                {
                    pnLoginBox.Visible = false;
                    sResponse = "Login Box Hidden";
                }
                else
                {
                    sResponse = "Login Box Visible";
                }
            }
        }

        // --------------------------------------------------------------------------------------
        return sResponse;
    }
    // ----------------------------------------------------------------------------
    protected string GetMembershipDatabase()
    {
        string sMembershipDatabaseName = "";
        FieldInfo connectionStringField = Membership.Provider.GetType().GetField("_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);
        if (connectionStringField != null)
        {
            sMembershipDatabaseName = connectionStringField.GetValue(Membership.Provider).ToString();
        }
        
        return sMembershipDatabaseName;
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btLogin_Click(object sender, EventArgs e)
    {
        // -----------------------------------------------------------
        // You can't use a try/catch if you're redirecting (Thread exception...)
        // close close your connection THEN, do the redirect if needed
        string sUserID = txUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sUserCustomerNumberIsAuthorizedToLogin = "";
        string sLoginRedirectNeeded = "";
        string[] saPreNumTyp = { "", "", "" };
        string sCustomerType = "";

        int iRowsAffected = 0;
        int iAttemptsBeforeLockout = 0;
        try
        {
            sqlConn.Open();

            saPreNumTyp = Get_UserAccountIds(sUserID);
            if (saPreNumTyp.Length > 2)
                sCustomerType = saPreNumTyp[2];

            MembershipUser mu = Membership.GetUser(sUserID);
            // CHECK IF LOCKED OUT
            if (mu == null)
            {
                lbError.Text = "Login failed.  Please check your entries and try again.";
            }
            else if (mu.IsLockedOut == true)
            {
                lbError.Text = "UserID " + sUserID + " is currently locked after repeated login failures." +
                    "<br />We're sorry you're having difficulty with your account credentials." +
                    "<br />Those at your company with admin privileges" +
                    "<br /> can unlock this account and reset your password if needed," +
                    " or you can contact ";

                if (ShowNewCompanyName())
                    lbError.Text += "Secure-Serv for help.";
                else
                    lbError.Text += "STS for help.";
            }
            else
            {
                // Don't allow long login entries to be passed.
                if (sPassword.Length > 30)
                    sPassword = sPassword.Substring(0, 30);

                if (sUserID.Length > 50)
                    sUserID = sUserID.Substring(0, 50);

                // check if username's customer num is still active...
                int iCustomerNumber = Get_UserIdCs1(sUserID);
                if (iCustomerNumber > 0)
                {
                    if (sCustomerType == "SRG" || sCustomerType == "SRP" || sCustomerType == "SRC")
                        sUserCustomerNumberIsAuthorizedToLogin = "Y";
                    else 
                        sUserCustomerNumberIsAuthorizedToLogin = ws_Get_B1LoginAuthorizationForUserCustomerNumber_YN(iCustomerNumber.ToString());

                    if (sUserCustomerNumberIsAuthorizedToLogin != "Y")
                    {
                        lbError.Text = "The primary customer account (" + iCustomerNumber.ToString() + ") associated with your UserID is no longer active. <br />Please contact your Sales Rep for help.";
                    }
                    else
                    {
                        // Check if user has a confirmed email yet

                        string sEmailConfirmed = "";
                        DataTable dt = new DataTable("");
                        if (!String.IsNullOrEmpty(sUserID))
                            dt = Select_UserRecord(sUserID);
                        if (dt.Rows.Count > 0)
                            sEmailConfirmed = dt.Rows[0]["EmailConfirmed"].ToString().Trim();

                        if (sEmailConfirmed != "True")
                        {
                            lbError.Text = "<i>Email confirmation is required before login. An email for account confirmation is sent at registration (and when requested through the menu item 'Resend Confirmation Email').  Please locate this email and click the link to authorize your account.</i>";
                        }
                        else if (Membership.ValidateUser(sUserID, sPassword) == true)
                        {
                            iRowsAffected = Update_LoginCount(sUserID);
                            string sDisplayEmailManagement_YN = ws_Get_B1CustPref_AllowEmailManagement_YN(iCustomerNumber.ToString());
                            Session["DisplayEmailManagement"] = sDisplayEmailManagement_YN;

                            if (Session["AdminCustomerNumber"] != null)
                                Session["AdminCustomerNumber"] = null;
                            sLoginRedirectNeeded = "Y";
                        }
                        else // Login NOT Successful
                        {
                            // CHECK IF ABOUT TO BE LOCKED OUT
                            int iCurrentFailures = Get_BadPasswordCount(sUserID);
                            iAttemptsBeforeLockout = Check_LockoutThreat(sUserID, iCurrentFailures);

                            if (iAttemptsBeforeLockout <= 1)
                                lbError.Text = "THE NEXT LOGIN FAILURE WILL LOCK THIS ACCOUNT";
                            else if (iAttemptsBeforeLockout <= 3)
                                lbError.Text = "ONLY " + iAttemptsBeforeLockout + " MORE ATTEMPTS BEFORE LOCKOUT." +
                                    "<br />We're sorry you're having difficulty with these credentials." +
                                    "<br />To prevent locking the account" +
                                    "<br />please wait " + Membership.PasswordAttemptWindow.ToString() + " minutes for the account to reset before trying again.";
                            else
                                lbError.Text = "Login failed.  Please check your entries and try again.";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "User: " + txUserID.Text.Trim());
        }
        finally
        {
            sqlConn.Close();
        }
        // -----------------------------------------------------------
        // You have to keep the redirect OUTSIDE of the try catch block
        // So INSTEAD of redirecting at the point where it is authorized, wait and do it here!
        if (sLoginRedirectNeeded == "Y")
            FormsAuthentication.RedirectFromLoginPage(sUserID, false);
        // -----------------------------------------------------------
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================
}
