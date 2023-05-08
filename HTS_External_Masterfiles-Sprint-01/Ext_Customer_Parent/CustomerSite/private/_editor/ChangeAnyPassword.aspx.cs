using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class private__editor_ChangeAnyPassword : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
        txUserID.Focus();
    }
    // ================================================================
    protected void btReset_Click(object sender, EventArgs e)
    {
        string sUserID = txUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sVerdict = sfd.ValidatePassword(sUserID, sPassword);

        if (sVerdict != "VALID")
        {
            vCusPassword.ErrorMessage = sVerdict;
            vCusPassword.IsValid = false;
        }
        else
        {

            try
            {
                MembershipUser mu = Membership.GetUser(sUserID);
                mu.ChangePassword(mu.ResetPassword(), sPassword);
                txUserID.Text = "";
                txPassword.Text = "";
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
    // ================================================================
}