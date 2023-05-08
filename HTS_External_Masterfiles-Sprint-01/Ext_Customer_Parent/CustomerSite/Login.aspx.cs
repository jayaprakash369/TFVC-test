using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;


public partial class Login : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sResult = "";

    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);  

        // Login1.Attributes.Add("autocomplete", "off");
        //form1.Attributes.Add("autocomplete", "off");

        if (!IsPostBack)
        {
            lbMenuTitle.Text = "ServiceCOMMAND<span style='font-size: 11px; vertical-align: top; position: relative; top: -4px;'>®</span>";
            // --------------------------------------------------------------------------------------
            // LONGER PERIOD TO SHOW THE MESSAGE
            // Maintenance Message
            // Date was wrong on server: it was behind, look to see what current time is 
            DateTime datTemp = DateTime.Now;
            //datTemp = datTemp.AddDays(2);
            if (
                       (datTemp.ToString("yyyyMMdd") == "20220916" && datTemp.Hour >= 12)  // MESSAGE START: 12:00 noon Friday
                    //|| (datTemp.ToString("yyyyMMdd") == "20220827")
                    //|| (datTemp.ToString("yyyyMMdd") == "20201018")
                    || (datTemp.ToString("yyyyMMdd") == "20220918" && datTemp.Hour <= 14) // MESSAGE END: 6 pm Sunday (remember you want it to turn BACK ON on the restart hour)
                )
            {
                lbMaintenance.Text = "STS will be performing routine system maintenance" +
                    //"<br />Starting Saturday December 15th, at 6 PM (CST)" +
                    //"<br />thru Sunday December 15th 8 PM (CST)." +
                    "<br />this weekend on Sunday September 18th" +
                    "<br />from 1 pm to 6 pm (CST)" +
                    //"<br />through Saturday August 27th at 11 am (CST)" +
                    //"<br />until Sunday August 11th at 12 noon (CST)" +
                    "<br /><i> ServiceCOMMAND will be unavailable during this period.</i>";
                pnNotice.Visible = true;
            }
            else
            {
                pnNotice.Visible = false;
            }
            // --------------------------------------------------------------------------------------
            // SHORTER PERIOD TO HIDE THE LOGIN BOX
            if (
                (datTemp.ToString("yyyyMMdd") == "20220918" && datTemp.Hour >= 12)  // START: LOGIN BOX REMOVAL 7 pm Monday
                // && (datTemp.ToString("yyyyMMdd") == "20190518" && datTemp.Hour < 21)  // 21 = 9pm (if you make it <= it will stay hidden all hour)
                //|| (datTemp.ToString("yyyyMMdd") == "20190810")
                && (datTemp.ToString("yyyyMMdd") == "20220918" && datTemp.Hour <= 14)  // 22 = 10pm (if you make it <= it will stay hidden all hour)
                )
            {
                pnLoginBox.Visible = false;
            }
            else
            {
                pnLoginBox.Visible = true;
            }
            // --------------------------------------------------------------------------------------

            if (sDevTestLive == "LIVE" || sDevTestLive == "TEST")
            {
                txPassword.Attributes.Add("onkeypress", "javascript:checkCapsLock(event)");
                txPassword.Attributes.Add("autocomplete", "off");
                txUserID.Attributes.Add("autocomplete", "off");
            }

            lbMsg.Text = "User sessions will time out after a period of inactivity.";
            //lbTemp.Text = "LOGIN REMINDERS: <br />1) Your old credentials WILL NOT WORK.  If a new company account has not been created, please click the Registration button at the upper right corner to create new credentials.<br /><br />2) FORGOTTEN PASSWORDS ARE LOST FOREVER.  Though they can be overwritten, it's easier to learn and remember the initial password (especially with accounts shared by a group).";

            if (Page.User.Identity.IsAuthenticated)
            {
                lbMsg.Text = "The page you are trying to access requires greater privileges.<br />  Please log in using a different user account with higher credentials.";
            }
            txUserID.Focus();
        }
        else
        {
            // processLogin();
            txPassword.Focus();
        }
    }
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("jsCapsLock", "/public/js/CapsLock.js");
        Page.ClientScript.RegisterClientScriptInclude("jsLogin", "/public/js/Login.js");

        if (!IsPostBack)
        {

        }
        string userURL = Request.Url.ToString();
        string last11 = userURL.Substring(userURL.Length - 11);

        if (userURL.EndsWith("ReturnUrl=/"))
        {
            if (userURL.StartsWith("http://localhost"))
            {
                Response.Redirect("http://localhost/private/shared/menu.aspx", false);
            }
            else
            {
                Response.Redirect("~/private/shared/menu.aspx", false);
            }
        }
        // ------------------------------------
        this.RequireSSL = true;

        //      InitializeComponent();
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
    }
    // =========================================================
    protected void btLogin_Click(object sender, EventArgs e)
    {
        processLogin();
    }
    // =========================================================
    protected void updateLoginCount(string userID)
    {
        string sSql = "";

        try
        {
            //            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            sSql = "Update aspnet_Users" +
                " set LoginCount = (1 + (Select LoginCount from aspnet_Users where UserName = @UserName1))" +
                " where UserName = @UserName2";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName1", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName1"].Value = userID;

            sqlCmd.Parameters.Add("@UserName2", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName2"].Value = userID;

            sqlConn.Open();

            sqlCmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
    }
    // =========================================================
    protected int getBadPasswordCount(string userID)
    {
        int iBadPasswords = 0;
        string sSql = "";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        try
        {
            sSql = "Select FailedPasswordAttemptCount" +
                " from aspnet_Users u, aspnet_Membership m" +
                " where u.UserId = m.UserId" +
                " and UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = userID;

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
            {
                iBadPasswords = Convert.ToInt32(dataTable.Rows[0]["FailedPasswordAttemptCount"].ToString());
            }

        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        return iBadPasswords;
    }
    // =========================================================
    protected int checkLockoutThreat(string userID, int currFailures)
    {
        int iMaxAttempts = Membership.MaxInvalidPasswordAttempts;
        int iRemainingAttempts = iMaxAttempts; // Start assuming all are left;
        double dTimeoutWindow = Convert.ToDouble(Membership.PasswordAttemptWindow); // Span in minutes to accumulate bad pwd count

        DateTime datStart = new DateTime();
        DateTime datEnd = new DateTime();
        DateTime datNow = new DateTime();

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        try
        {
            string sSql = "";

            sSql = "Select FailedPasswordAttemptWindowStart" +
                " from aspnet_Users u, aspnet_Membership m" +
                " where u.UserId = m.UserId" +
                " and UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = userID;

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
            {
                datStart = Convert.ToDateTime(dataTable.Rows[0]["FailedPasswordAttemptWindowStart"].ToString());
                datStart = datStart.ToLocalTime();
                datEnd = datStart.AddMinutes(dTimeoutWindow);
                datNow = DateTime.Now;
                if (datNow < datEnd)
                    iRemainingAttempts = iMaxAttempts - (currFailures - 1);
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return iRemainingAttempts;
    }
    // =========================================================
    protected void processLogin()
    {
        vCusLogin.ErrorMessage = "";

        string sUserID = txUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sCustomerStatus = "";
        string jumpForVern = "N";

        int iAttemptsBeforeLockout = 0;
        try
        {
            MembershipUser mu = Membership.GetUser(sUserID);
            // CHECK IF LOCKED OUT
            if (mu == null)
            {
                vCusLogin.ErrorMessage = "Login failed.  Please check your entries and try again.";
                vCusLogin.IsValid = false;
            }
            else if (mu.IsLockedOut == true)
            {
                vCusLogin.ErrorMessage = "UserID " + sUserID + " is currently locked after repeated login failures." +
                    "<br />We're sorry you're having difficulty with your account credentials." +
                    "<br />Those at your company with admin privileges" +
                    "<br /> can unlock this account and reset your password if needed," +
                    " or you can contact STS for help.";
                vCusLogin.IsValid = false;
            }
            else
            {

                // Don't allow long login entries to be passed.
                if (sPassword.Length > 15)
                    sPassword = sPassword.Substring(0, 15);

                if (sUserID.Length > 20)
                    sUserID = sUserID.Substring(0, 20);

                // check if username's customer num is still active...
                int iCs1 = getUserIdCs1(sUserID);
                if (iCs1 > 0)
                {
                    if (sPageLib == "L")
                    {
                        sCustomerStatus = wsLive.GetStatusAsCustomer(sfd.GetWsKey(), iCs1);
                    }
                    else
                    {
                        sCustomerStatus = wsTest.GetStatusAsCustomer(sfd.GetWsKey(), iCs1);
                    }
                    if (sCustomerStatus != "ACTIVE")
                    {
                        vCusLogin.ErrorMessage = "The primary customer account (" + iCs1.ToString() + ") associated with your UserID is no longer active. <br />Please contact your Sales Rep for help.";
                        vCusLogin.IsValid = false;
                    }
                    else
                    {
                        if (Membership.ValidateUser(sUserID, sPassword))
                        {
                            updateLoginCount(sUserID);
// Code for the asset history utility marked with IJL
                         //IJL   if (sUserID.ToLower() == "vjkathol" || sUserID.ToLower() == "isabel" || sUserID.ToLower() == "sarahengels")
                         //IJL   {
                        //IJL        FormsAuthentication.SetAuthCookie(sUserID, true);
                         //IJL       jumpForVern= "Y";
                         //IJL   }
                         //IJL   else
                         //IJL   {
                                FormsAuthentication.RedirectFromLoginPage(sUserID, false);
                                //                        if (iCs1 == 99999)
                                //                            HttpContext.Current.Session.Timeout = 1;
                         //IJL   }
                        }
                        else
                        {
                            // CHECK IF ABOUT TO BE LOCKED OUT
                            int iCurrentFailures = getBadPasswordCount(sUserID);
                            iAttemptsBeforeLockout = checkLockoutThreat(sUserID, iCurrentFailures);
                            //lbMsg.Text = "Login Failure Count... " + iCurrentFailures.ToString();

                            if (iAttemptsBeforeLockout <= 1)
                                vCusLogin.ErrorMessage = "THE NEXT LOGIN FAILURE WILL LOCK THIS ACCOUNT";
                            else if (iAttemptsBeforeLockout <= 3)
                                vCusLogin.ErrorMessage = "ONLY " + iAttemptsBeforeLockout + " MORE ATTEMPTS BEFORE LOCKOUT." +
                                    "<br />We're sorry you're having difficulty with these credentials." +
                                    "<br />To prevent locking the account" +
                                    "<br />please wait " + Membership.PasswordAttemptWindow.ToString() + " minutes for the account to reset before trying again.";
                            else
                                vCusLogin.ErrorMessage = "Login failed.  Please check your entries and try again.";
                            vCusLogin.IsValid = false;
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
        }
     //IJL   if(jumpForVern == "Y")
     //IJL   Response.Redirect("~/private/sc/AssetLeasingLookup.aspx");
    }
    // =========================================================
    protected int getUserIdCs1(string userID)
    {
        int iCs1 = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        try
        {
            string sSql = "";

            sSql = "Select PrimaryCs1" +
                " from aspnet_Users" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = userID;

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
            {
                if (int.TryParse(dataTable.Rows[0]["PrimaryCs1"].ToString().Trim(), out iCs1) == false)
                    iCs1 = 0;
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return iCs1;
    }
    // =========================================================
    // =========================================================
}
