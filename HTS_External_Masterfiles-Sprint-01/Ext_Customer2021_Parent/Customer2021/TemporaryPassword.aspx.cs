using System;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.Profile;

public partial class TemporaryPassword : MyPage
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
        if (!IsPostBack)
        {
            txUserIdEmail.Focus();
        }
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    protected string UserIdEmailExists(string userIdEmail)
    {
        string sUserIdEmailExists = "";
        string sUserIdEmail = userIdEmail.ToLower();
        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " UserId" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users" +
                " where LoweredUserName = @UserIdEmail";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserIdEmail", sUserIdEmail);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
            }

            if (dt.Rows.Count > 0)
                sUserIdEmailExists = "Y";
            else
                sUserIdEmailExists = "N";

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return sUserIdEmailExists;
    }

    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
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
        // ------------------------------------
        this.RequireSSL = true;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btReset_Click(object sender, EventArgs e)
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        EmailHandler emh = new EmailHandler();

        string sUserIdEmail = txUserIdEmail.Text.ToLower();
        string sUserIdEmailExists = "";
        string sRandomPassword = "";


        try
        {
            sqlConn.Open();
            MembershipUser mu = Membership.GetUser(sUserIdEmail);
            sUserIdEmailExists = UserIdEmailExists(sUserIdEmail);
            
            if (String.IsNullOrEmpty(sUserIdEmail))
            {
                lbResult.Text = "Entry of your user account email is required";
                txUserIdEmail.Focus();
            }
            else if (sUserIdEmail.Length < 10 || sUserIdEmail.Length > 50)
            {
                lbResult.Text = "Your entry must be between 10 and 50 characters";
                txUserIdEmail.Focus();
            }
            else if (isEmailFormatValid(sUserIdEmail) != true)
            {
                lbResult.Text = "Your entry does not appear to be in the format of a valid email address";
                txUserIdEmail.Focus();
            }
            else if (sUserIdEmailExists != "Y")
            {
                lbResult.Text = "No account was found matching the User ID Email " + txUserIdEmail.Text + ".  Password reset cancelled. (Are you using a user id from our old site?)";
                txUserIdEmail.Focus();
            }
            else if (mu == null)
            {
                lbResult.Text = "There was a problem accessing the account for User ID Email " + txUserIdEmail.Text + ".  Password reset cancelled. (Are you using a user id from our old site?)";
                txUserIdEmail.Focus();
            }
            else
            {
                int iRandom = 0;
                Random rNum = new Random();
                iRandom = rNum.Next(0, 99);

                sRandomPassword = "Pwd" + iRandom.ToString() + Membership.GeneratePassword(4, 0);
                mu.ChangePassword(mu.ResetPassword(), sRandomPassword);

                string sSubject = "Your ServiceCOMMAND account password has been reset";
                string sBody =
                    "<span style='font-size: 18px; font-weight: bold; color:crimson;'>A password reset request was issued for this account.</span>" +
                    "<br /><br />The temporary password is displayed below. " + 
                    "<br /><br /><b>User ID Email:</b> <br /><br />" + txUserIdEmail.Text + 
                    "<br /><br /><b>Temporary Password:</b> <br /><br /><span style='font-size: 18px;'>" + sRandomPassword + "</span>" +
                    "<br /><br />Please use these credentials next time you access your account. " +
                    "<br /><br />Once logged in, you may then update your password using the menu item \"Change Your Password\"" +
                    "<br /><br /><br /><br /><i>If you did NOT request this change to your account please contact <a style=\"font-size: 14px; font-style: italic; color: #406080\" href='mailto:servicecommandsupport@scantron.com'>ServiceCOMMAND Support</a> to resolve this issue.</i>";

                string sEmailResult = emh.EmailIndividual(sSubject, sBody, sUserIdEmail, "adv320@scantron.com");

                if (sEmailResult == "SUCCESS")
                {
                    lbResult.Text = "Password successfully reset - email sent" +
                        "<br />" +
                        "<br />1) Locate the email just sent and copy the auto generated password" +
                        "<br />2) Return to this site's login page and enter the auto generated password" +
                        "<br />3) Once logged in, use the \"Change Your Password\" link on the left blue navigation bar to reset to a preferred password.";
                    txUserIdEmail.Text = "";
                }
                else
                    lbResult.Text = "Email Failure: " + sEmailResult;
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
            emh = null;
        }

    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================
}
