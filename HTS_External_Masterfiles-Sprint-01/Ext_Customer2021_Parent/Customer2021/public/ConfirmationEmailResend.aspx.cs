using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web;

public partial class public_ConfirmationEmailResend : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    //string sTemp = "";
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        try 
        {
            sqlConn.Open();

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void Check_Confirmation()
    {
        string sEmail = txAccountEmail.Text.Trim().ToLower();
        DataTable dt = new DataTable(""); 
        string sConfirmationCode = "";
        string sEmailConfirmed = "";
        DateTime datConfirmationDeadline = new DateTime();

        if (!String.IsNullOrEmpty(sEmail))
        {
            // Ensure the email corresponds with a registered user
            dt = Select_UserRecord(sEmail);
            
            if (dt.Rows.Count > 0)
                sEmailConfirmed = dt.Rows[0]["EmailConfirmed"].ToString().Trim(); // True or False
        }


        if (String.IsNullOrEmpty(sEmail))
        {
            lbSummary.Text = "Email Required";
            lbDetail.Text = "Please enter the login email address for this account.";
        }
        else if (dt.Rows.Count == 0)
        {
            lbSummary.Text = "User Account Not Found";
            lbDetail.Text = "No account was found matching the login email " + sEmail + ".  If you have registered an account for this email address, perhaps the account has been deleted.  Since this login email is currently available, it can be registered.";
        }
        else if (sEmailConfirmed == "True")
        {
            lbSummary.Text = "Account Login Email Has Already Been Confirmed";
            lbDetail.Text = "This account is already authorized for login";
        }
        else
        {
            // Update the database
            datConfirmationDeadline = DateTime.Now.ToUniversalTime().AddMinutes(15);
            sConfirmationCode = Make_ConfirmationCode(sEmail);
            int iRowsAffected = Update_UserConfirmation(sEmail, sConfirmationCode);

            // Submit the email
            string sUserUrl = HttpContext.Current.Request.Url.ToString().ToLower();
            int iPos = sUserUrl.IndexOf("/public/confirmationemailresend.aspx");
            if (iPos > -1)
            {
                sUserUrl = sUserUrl.Substring(0, iPos) + "/public/confirmation.aspx?code=" + sConfirmationCode;
            }
            // Email confirmation
            EmailHandler emailHandler = new EmailHandler();

            string sSubject = "";
            if (ShowNewCompanyName() == true)
                sSubject = "Account Email Confirmation (Secur-Serv)";
            else
                sSubject = "Account Email Confirmation (Scantron Technology Solutions)";
            
            string sMessage = "To confirm your ServiceCOMMAND® account email address" +
                "<br />" +
                "please <a href='" + sUserUrl + "'>Click Here</a>" +
                //"<br /><br /> <i>(The link will be valid only for the next 15 minutes)</i>";
                "<br /><br /><ul>" +
                "<li>Each requested confirmation link is unique</li>" +
                "<li>Only the most recent will be valid</li>" +
                "<li>Access will expire in 15 minutes</li>" +
                "</ul>";

            string sEmailTo = sEmail;
            string sEmailFrom = "adv320@scantron.com";
            string sEmailResult = emailHandler.EmailIndividual(sSubject, sMessage, sEmailTo, sEmailFrom);
            emailHandler = null;

            lbSummary.Text = "Confirmation Email Sent";
            lbDetail.Text = "Please check for a new email in this account and click the confirmation link. The link will only be valid for the next 15 minutes";
        }

    }
    // ========================================================================
    #region mySqls
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
    // -------------------------------------------------------------------------------------------------------
    protected int Update_UserConfirmation(string userId, string confirmationCode)
    {
        int iRowsAffected = 0;
        string sSql = "";

        try
        {
            sSql = "Update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                 " ConfirmationCode = @Code" +
                ", EmailConfirmed = @Confirmed" +
                ", ConfirmationDeadline = @Deadline" +
                " where UserName = @Email";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Code", confirmationCode);
            sqlCmd.Parameters.AddWithValue("@Confirmed", 0);
            sqlCmd.Parameters.AddWithValue("@Deadline", DateTime.Now.AddMinutes(15).ToUniversalTime());
            sqlCmd.Parameters.AddWithValue("@Email", userId);

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
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        //Button myControl = (Button)sender;
        //string sParms = myControl.CommandArgument.ToString().Trim();
        //string[] saParms = new string[1];
        //saParms = sParms.Split('|');

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn.Open();
            Check_Confirmation();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
