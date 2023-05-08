using System;
using System.Data;
using System.Configuration;
using System.Web.Security;

public partial class private_ChangeYourPassword : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MembershipUser mu = Membership.GetUser();
            lbUserIdEmail.Text = mu.UserName.ToString();
        }
    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btChangePassword_Click(object sender, EventArgs e)
    {
        lbResult.Text = "";

        string sUserIdEmail = lbUserIdEmail.Text.Trim();
        string sPasswordCurrent = txPasswordCurrent.Text.Trim();
        string sPasswordNew = txPasswordNew.Text.Trim();
        string sPasswordNew2 = txPasswordNew2.Text.Trim();

        if (Membership.ValidateUser(sUserIdEmail, sPasswordCurrent))
        {
            if (sPasswordNew == "")
            {
                lbResult.Text += "An updated password is required<br />";
                txPasswordNew.Focus();
            }
            else
            {
                if (sPasswordNew.Length < 7 || sPasswordNew.Length > 30)
                {
                    lbResult.Text += "Your updated password entry must be between 7 and 30 characters<br />";
                    txPasswordNew.Focus();
                }
            }

            if (sPasswordNew2 == "")
            {
                lbResult.Text += "Password confirmation is required<br />";
                txPasswordNew2.Focus();
            }
            else
            {
                if (sPasswordNew2.Length < 7 || sPasswordNew2.Length > 30)
                {
                    lbResult.Text += "Your password confirmation entry must be between 7 and 30 characters<br />";
                    txPasswordNew2.Focus();
                }
                if (sPasswordNew != sPasswordNew2)
                {
                    lbResult.Text += "Your new password and confirmation entry do not match<br />";
                    txPasswordNew2.Focus();
                }
            }

            string sIsPasswordFormatValid = ValidatePassword(sUserIdEmail, sPasswordCurrent);
            if (sIsPasswordFormatValid != "Y")
            {
                lbResult.Text += sIsPasswordFormatValid;
                txPasswordNew.Focus();
            }

            if (lbResult.Text == "")
            {
                try
                {
                    MembershipUser mu = Membership.GetUser();
                    mu.ChangePassword(mu.ResetPassword(), sPasswordNew);
                    lbResult.Text = "Password successfully changed.";
                }
                catch (Exception ex)
                {
                    string sResult = ex.ToString();
                    SaveError(ex.Message.ToString(), ex.ToString(), "");
                    lbResult.Text = "An error occurred during the update: " + ex.ToString();
                }
                finally
                {
                }
            }
        }
        else
        {
            lbResult.Text = "Your entry for your current password is not valid.";
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
