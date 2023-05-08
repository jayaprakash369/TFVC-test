using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class public_Confirmation : MyPage
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
        // To prevent crashing on an open thread, if the user needs redirection to the Login page, I save the flag to a hidden field and redirect outside of a try block
        if (hfRedirectToLoginPage_YN.Value == "Y")
            Response.Redirect("~/Login.aspx?m=c");
    }
    // -------------------------------------------------------------------------------------------------------
    protected void Check_Confirmation()
    {
        string sCode = "";
        string sEmail = "";
        hlLogin.Visible = false;
        hfRedirectToLoginPage_YN.Value = "N";

        if (Request.QueryString["code"] != null && Request.QueryString["code"].ToString() != "")
        {
            sCode = Request.QueryString["code"].ToString().Trim();
            sEmail = Parse_ConfirmationCode(sCode);
            if (!String.IsNullOrEmpty(sEmail))
            {
                // Ensure the email corresponds with a registered user
                DataTable dt = Select_UserRecord(sEmail);
                string sEmailConfirmed = "";
                string sConfirmationCode = "";
                DateTime datConfirmationDeadline = new DateTime();
                if (dt.Rows.Count > 0)
                {
                    sEmailConfirmed = dt.Rows[0]["EmailConfirmed"].ToString().Trim(); // bit -> True or False
                    sConfirmationCode = dt.Rows[0]["ConfirmationCode"].ToString().Trim();
                    if (DateTime.TryParse(dt.Rows[0]["ConfirmationDeadline"].ToString().Trim(), out datConfirmationDeadline) == false)
                    {
                        datConfirmationDeadline = new DateTime();
                    }

                }

                if (sEmailConfirmed == "True")
                {
                    lbSummary.Text = "Account Login Email Has Already Been Confirmed";
                    lbDetail.Text = "You are already authorized to login";
                }
                else if (String.IsNullOrEmpty(sCode))
                {
                    lbSummary.Text = "No confirmation code was found";
                    lbDetail.Text = "No code was found for account confirmation.  If you have already registered, please go to the confirmation email page to request a fresh confirmation email. ";
                }
                else if (String.IsNullOrEmpty(sEmail))
                {
                    lbSummary.Text = "Invalid Confirmation Code";
                    lbDetail.Text = "The confirmation code could not be successfully read.";
                }
                else if (dt.Rows.Count == 0)
                {
                    lbSummary.Text = "User Account Not Found";
                    lbDetail.Text = "No account was found matching the login email " + sEmail + ".  If you have registered an account for this email address, perhaps the account has been deleted.  Since this login email is currently available, it can be registered.";
                }
                else if (datConfirmationDeadline == new DateTime()) // The deadline only initialized (i.e. no record was found)
                {
                    lbSummary.Text = "Problem With Confirmation Deadline";
                    lbDetail.Text = "Please visit the confirmation email page to initialize a new confirmation email request.";
                }
                else if (datConfirmationDeadline.ToUniversalTime() < DateTime.Now.ToUniversalTime()) // And the deadline has not passed
                {
                    lbSummary.Text = "Confirmation Request Deadline Has Passed";
                    lbDetail.Text = "Please visit the confirmation email page to initialize a new confirmation email request.";
                }
                else if (sCode != sConfirmationCode) // And the deadline has not passed
                {
                    lbSummary.Text = "Code Submitted Does Not Match Account";
                    lbDetail.Text = "The confirmation code passed, does not match the current confirmation code for the account.  If multiple confirmation codes have been requested, please ensure the most recent is used to activate the account.  Otherwise, a fresh request can be initiated from the 'Email Confirmation' menu item.";
                }
                else if (
                    sCode == sConfirmationCode
                    && datConfirmationDeadline.ToUniversalTime() > DateTime.Now.ToUniversalTime()
                    )
                {
                    // Confirm the account
                    // Update the database
                    int iRowsAffected = Update_UserConfirmation(sEmail);

                    //lbSummary.Text = "Account Confirmation Successful";
                    //lbDetail.Text = "You are now authorized to...";
                    //hlLogin.Visible = true;
                    hfRedirectToLoginPage_YN.Value = "Y";
                }
                else
                {
                    lbSummary.Text = "Processing Error";
                    lbDetail.Text = "An exception has occurred.  We apologize for this inconvience.  ";
                    if (ShowNewCompanyName() == true)
                    lbDetail.Text = "Secur-Serv";
                    else
                        lbDetail.Text = "STS";
                    lbDetail.Text = " programming will contact you through this email address to help resolve this issue and get your account confirmed.";
                    // You need to send an email to yourself for this address to get this fixed. 
                }
            }
        }
        else 
        {
            lbSummary.Text = "Invalid Confirmation Code";
            lbDetail.Text = "Please visit the confirmation email page to initialize a new confirmation email request.";
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
    protected int Update_UserConfirmation(string userId)
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

            sqlCmd.Parameters.AddWithValue("@Code", "");
            sqlCmd.Parameters.AddWithValue("@Confirmed", 1);
            sqlCmd.Parameters.AddWithValue("@Deadline", DateTime.Now.AddMinutes(-15));
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

    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
