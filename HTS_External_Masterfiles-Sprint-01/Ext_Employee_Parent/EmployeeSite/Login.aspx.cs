using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Login : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;
    string sConnectionString = "";
    string sResult = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        string userURL = Request.Url.ToString();
        string last11 = userURL.Substring(userURL.Length - 11);

        if (userURL.EndsWith("ReturnUrl=/"))
        {
            if (userURL.StartsWith("http://localhost"))
            {
                Response.Redirect("http://localhost/default.aspx");
            }
            else
            {
                Response.Redirect("~/Default.aspx");

            }
            Response.End();
        }
        // ------------------------------------
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        if (!IsPostBack)
        {
            txPassword.Attributes.Add("onkeypress", "javascript:checkCapsLock(event)");
            txPassword.Attributes.Add("autocomplete", "off");
            txUserID.Attributes.Add("autocomplete", "off");

            lbMessage.Text = "Sessions time out after a period of inactivity.";

            if (Page.User.Identity.IsAuthenticated)
            {
                lbMessage.Text = "The page you are trying to access requires greater privileges.<br />  Please log in with proper credentials.";
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
            //string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            //            sqlConn = new SqlConnection(sConnectionString);

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

        int iAttemptsBeforeLockout = 0;

        MembershipUser mu = Membership.GetUser(sUserID);
        // CHECK IF LOCKED OUT
        if ((mu != null) && (mu.IsLockedOut == true))
        {
            vCusLogin.ErrorMessage = "UserID " + sUserID + " is currently locked after repeated login failures." +
                "<br />We're sorry you're having difficulty with your account credentials." +
                "<br />Those at your company with admin privileges" +
                "<br /> can unlock this account and reset your password if needed," +
                " or you can contact HTS for help.";
            vCusLogin.IsValid = false;
        }
        else
        {

            // Don't allow long login entries to be passed.
            if (sPassword.Length > 15)
                sPassword = sPassword.Substring(0, 15);

            if (sUserID.Length > 20)
                sUserID = sUserID.Substring(0, 20);

            if (Membership.ValidateUser(sUserID, sPassword))
            {
                updateLoginCount(sUserID);

                /*
                string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                // If from an internal address: login to both sites
                if (sIpAddress.StartsWith("10.") == true || 
                    sIpAddress.StartsWith("127.") == true || 
                    sIpAddress.StartsWith("172.16.") == true ||
                    sIpAddress.StartsWith("172.17.") == true ||
                    sIpAddress.StartsWith("172.18.") == true ||
                    sIpAddress.StartsWith("172.19.") == true ||
                    sIpAddress.StartsWith("172.20.") == true ||
                    sIpAddress.StartsWith("172.21.") == true ||
                    sIpAddress.StartsWith("172.22.") == true ||
                    sIpAddress.StartsWith("172.23.") == true ||
                    sIpAddress.StartsWith("172.24.") == true ||
                    sIpAddress.StartsWith("172.25.") == true ||
                    sIpAddress.StartsWith("172.26.") == true ||
                    sIpAddress.StartsWith("172.27.") == true ||
                    sIpAddress.StartsWith("172.28.") == true ||
                    sIpAddress.StartsWith("172.29.") == true ||
                    sIpAddress.StartsWith("172.30.") == true ||
                    sIpAddress.StartsWith("172.31.") == true || 
                    sIpAddress.StartsWith("192.168.") == true)
                {
                    FormsAuthentication.SetAuthCookie(sUserID, false);

                    string sUserUrl = Request.Url.ToString();
                    string sReturnUrl = "";
                    int iReturnUrlIndex = sUserUrl.IndexOf("ReturnUrl=");
                    if (iReturnUrlIndex > -1)
                    {
                        sReturnUrl = sUserUrl.Substring(iReturnUrlIndex + 10);
                    }
                    string sPort = "";
                    if (sDevTestLive == "LIVE")
                        sPort = "90";
                    else if (sDevTestLive == "TEST")
                        sPort = "92";
                    else
                        sPort = "93";
                    string sLoginLink = "http://10.41.30.9:" +
                        sPort +
                        "/public/tools/singleSignOn.aspx?uid=" + sUserID + "&pwd=" + sPassword + "&url=" + sReturnUrl;
                    Response.Redirect(sLoginLink);
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(sUserID, false);
                }
                */

                FormsAuthentication.RedirectFromLoginPage(sUserID, false);
                Response.End();
            }
            else
            {
                // CHECK IF ABOUT TO BE LOCKED OUT
                int iCurrentFailures = getBadPasswordCount(sUserID);
                iAttemptsBeforeLockout = checkLockoutThreat(sUserID, iCurrentFailures);
                //lbMessage.Text = "Login Failure Count... " + iCurrentFailures.ToString();

                if (iAttemptsBeforeLockout <= 1)
                    vCusLogin.ErrorMessage = "THE NEXT LOGIN FAILURE WILL LOCK THIS ACCOUNT";
                else if (iAttemptsBeforeLockout <= 3)
                    vCusLogin.ErrorMessage = "ONLY " + iAttemptsBeforeLockout + " MORE ATTEMPTS BEFORE LOCKOUT." +
                        "<br />We're sorry you're having difficulty with these credentials." +
                        "<br />To prevent locking the account" +
                        "<br />please wait " + Membership.PasswordAttemptWindow.ToString() + " minutes for the account to reset before trying again.";
                else
                    vCusLogin.ErrorMessage = "Login failed.  Please try again.";
                vCusLogin.IsValid = false;
            }
        }
    }
    // =========================================================
    // =========================================================
}