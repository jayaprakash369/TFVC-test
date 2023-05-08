using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;

public partial class private_shared_ChangeYourPassword : MyPage
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
    // =====================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //vRegPassword.ValidationExpression = sfd.sPasswordRequirementCode;
            //vRegPassword.ErrorMessage = sfd.sPasswordRequirementText;
        }
        MembershipUser mu = Membership.GetUser();
        lbUserID.Text = mu.UserName.ToString();
    }
    // =====================================================
    protected void btChangePassword_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        string sPwdOld = txPwdOld.Text.Trim();
        string sPwdNew = txPwdNew.Text.Trim();
        string sVerdict = "";

        if (Membership.ValidateUser(sUserID, sPwdOld))
        {
            sVerdict = sfd.ValidatePassword(sUserID, sPwdNew);
            if (sVerdict != "VALID")
            {
                vCusPassword.ErrorMessage = sVerdict;
                vCusPassword.IsValid = false;
            }
            else
            {

                try
                {
                    string password = txPwdNew.Text.Trim();
                    //MembershipUser mu = Membership.GetUser(userID);
                    MembershipUser mu = Membership.GetUser();
                    mu.ChangePassword(mu.ResetPassword(), password);
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
    // =====================================================
}