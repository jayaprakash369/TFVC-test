using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_koi : MyPage
{
    // -----------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        Do_Administration();
        // Encrypt();
    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { }
    // ========================================================================
    // -----------------------------------------------------------
    protected void Do_Administration()
    {
        // Do site setup here (create roles, so when users register they will have roles to go into)
        string sDebug = "";
        // Do site setup here (create roles, so when users register they will have roles to go into)
        try
        {
            //Roles.CreateRole("SiteAdministrator");
            //Roles.CreateRole("CustomerAdministrator");

            //Membership.DeleteUser("steve.carlson@scantron.com");

            //Roles.RemoveUserFromRole("sarah.engels@scantron.com", "CustomerAdministrator");

            //Roles.AddUserToRole("steve.carlson@scantron.com", "CustomerAdministrator");

            //Roles.AddUserToRole("sarah.engels@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("isabel.labrador@scantron.com", "SiteAdministrator");

            //Roles.AddUserToRole("dana.freeman@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("april.wiggins@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("randy.lorenzen@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("herb.stebbins@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("amy.garner@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("vern.kathol@scantron.com", "SiteAdministrator");
            //Roles.AddUserToRole("pat.heller@scantron.com", "SiteAdministrator");

            //Roles.CreateRole("ServRightChildFst");
            //Roles.CreateRole("ServRightParentVendor");
            //Roles.CreateRole("ServRightGrandparentVendor");

            //Roles.CreateRole("ServrightChildFst");
            //Roles.CreateRole("ServrightParentProvidingFsts");
            //Roles.CreateRole("ServrightGrandparentToBePaid");

            //Roles.AddUserToRole("bcocran@servright.com", "CustomerAdministrator");
            //Roles.AddUserToRole("bcocran@servright.com", "SiteAdministrator");

            //Roles.AddUserToRole("dennis.hutson@scantron.com", "CustomerAdministrator");
            //Roles.AddUserToRole("dennis.hutson@scantron.com", "SiteAdministrator");

            //Roles.AddUserToRole("john.anderson@scantron.com", "CustomerAdministrator");
            //Roles.AddUserToRole("john.anderson@scantron.com", "SiteAdministrator");

            //Roles.AddUserToRole("todd.nunley@scantron.com", "CustomerAdministrator");
            //Roles.AddUserToRole("Todd.Nunley@Scantron.com", "SiteAdministrator");

        }
        catch (Exception ex)
        {
            sDebug = ex.Message.ToString();
        }
        finally
        {
        }


    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // -----------------------------------------------------------
    protected void Do_Starter()
    {
        string sDebug = "";

        try
        {

        }
        catch (Exception ex)
        {
            sDebug = ex.Message.ToString();
        }
        finally
        {
        }

    }

    // -----------------------------------------------------------
    protected void Encrypt()
    {
        string sDebug = "";
        //string sOrig = "2021-12-31T13:14:15.0000-05:00|steve.carlson@scantron.com";
        string sEmail = "steve.carlson@scantron.com";
        string sTemp = "";

        try
        {
            sTemp = Make_ConfirmationCode(sEmail);
            sTemp = Parse_ConfirmationCode(sTemp);
        }
        catch (Exception ex)
        {
            sDebug = ex.Message.ToString();
        }
        finally
        {
        }
    }
    // -----------------------------------------------------------
    protected void Do_EmailConfirmation()
    {
        string sDebug = "";

        try
        {
            // Successfully log in 
            // add database fields
            // generate token to pass in email (timestamp, useremail, some other hash?)
            // flag whether they have been confirmed
            // (and first name, last name in place of contact)
            // generate some random code or hash
            // save it to the user's database
            // Email with useremail, hash with a return url to a new page, base 64 encrypt
            // Write page to validate url link against database, approve/error message
            // Make it so the tool and page works with registration or re-send email confirmation works



            //var manager = new ApplicationUserManager();

            //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>(); 
            //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            //var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            //IdentityResult result = manager.Create(user, Password.Text);

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //string code = manager.GenerateEmailConfirmationToken("steve.carlson@scantron.com");
            //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
            // manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

            }
        catch (Exception ex)
        {
            sDebug = ex.Message.ToString();
        }
        finally
        {
        }

    }
    // -----------------------------------------------------------


    // -----------------------------------------------------------
    // -----------------------------------------------------------

}