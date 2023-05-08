using System;
using System.Activities.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Responsive : System.Web.UI.MasterPage
{
    // ============================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUserName = "";
        string sUserAccountType = "";
        string sStsNum = "";
        string sUserRoleList = "";

        string[] saPreNumTyp = { "", "", "" };

        if (!IsPostBack)
        {
            MultiViewLeftNav.ActiveViewIndex = 0; // Anonymous Nav Items
            // Set this as the default help message (refine once you know roles later)
            hlHelp.Text = "Questions? Issues? Help?";
            hlHelp.NavigateUrl = "mailto:servicecommandsupport@scantron.com";

            lbScantronYear.Text = DateTime.Now.ToString("yyyy");

            int iDateToday = 0;
            lbCompanyName.Text = "Scantron Technology Solutions"; 
            imBannerName.Width = 450;
            int iDateToSwitchName = 0;
            string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

            if (int.TryParse(DateTime.Now.ToLocalTime().ToString("yyyyMMdd"), out iDateToday) == true) 
            {

                if (sUserURL.ToLower().Contains("servicecommand.com"))
                    iDateToSwitchName = 20230510;
                else 
                    iDateToSwitchName = 20230313;

                if (iDateToday >= iDateToSwitchName)  // 20230403
                {
                    lbCompanyName.Text = "Secur-Serv";
                    imBannerName.ImageUrl = "~/media/images/secur-serv/Secur-Serv_Name_White_Transparent_375_64.png";
                    imBannerName.Width = 175; // was 275
                    imBannerName.Height = 28; 

                    imMenuLeft.ImageUrl = "~/media/images/secur-serv/Secur-Serv_Logo_blue_text2.png";
                    imMenuLeft.Height = 55; // was 55
                }
            }

            hfMenuOptionEmail.Value = "N";

            if (Session["DisplayEmailManagement"] != null && Session["DisplayEmailManagement"].ToString().Trim() != "")
            {
                if (Session["DisplayEmailManagement"].ToString().Trim() == "Y")
                    hfMenuOptionEmail.Value = "Y";
            }

            hfMenuOptionInvoices.Value = "N";

            if (Session["DisplayInvoices"] != null && Session["DisplayInvoices"].ToString().Trim() != "")
            {
                if (Session["DisplayInvoices"].ToString().Trim() == "Y")
                    hfMenuOptionInvoices.Value = "Y";
            }

            if (Page.User.Identity.IsAuthenticated)
            {
                MyPage myPage = new MyPage();
                //hfUsername.Value = Page.User.Identity.Name;
                sUserName = Page.User.Identity.Name;
                sUserAccountType = "";
                sStsNum = "";

                saPreNumTyp = myPage.Get_UserAccountIds(sUserName);
                myPage = null;

                if (saPreNumTyp.Length > 2)
                {
                    int iRegistrationCs1OrEmp = 0;
                    if (int.TryParse(saPreNumTyp[1], out iRegistrationCs1OrEmp) == false)
                        iRegistrationCs1OrEmp = 0;
                    if (iRegistrationCs1OrEmp > 0)
                    {
                        if (saPreNumTyp[2] != "" && saPreNumTyp[2] != "SRG" && saPreNumTyp[2] != "SRP")
                            sStsNum = iRegistrationCs1OrEmp.ToString("0");
                    }

                    sUserAccountType = saPreNumTyp[2]; // Type (REG, LRG, SRC/P/G servright child parent grandparent determines file visibility)
                }

                // Admin: viewing as an Alias? 
                if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
                {
                    if (Session["AdminCustomerType"] != null && Session["AdminCustomerType"].ToString().Trim() != "")
                    {
                        //string sDebug = "It can see alternate type: " + Session["AdminCustomerType"].ToString().Trim();
                        // Switch to alias account type
                        sUserAccountType = Session["AdminCustomerType"].ToString().Trim();

                        //if (sUserAccountType == "REG") MultiViewLeftNav.ActiveViewIndex = 0;
                        //else if (sUserAccountType == "SRC") MultiViewLeftNav.ActiveViewIndex = 1;
                    }
                }
                // ServrightGrandparentToBePaid, ServrightParentProvidingFsts, ServrightChildFst
                sUserRoleList = "";
                if (Page.User.IsInRole("CustomerAdministrator")) sUserRoleList += "CustomerAdministrator|";
                if (Page.User.IsInRole("SiteAdministrator")) sUserRoleList += "SiteAdministrator|";
                if (Page.User.IsInRole("ServrightGrandparentToBePaid")) sUserRoleList += "ServrightGrandparentToBePaid|";
                if (Page.User.IsInRole("ServrightParentProvidingFsts")) sUserRoleList += "ServrightParentProvidingFsts|";
                if (Page.User.IsInRole("ServrightChildFst")) sUserRoleList += "ServrightChildFst|";


                // -------------------------------------------------------------------------------------
                // Determine menu to show A) As user logged in, or B) As an admin impersonating another
                // --------------------------------------------------------------------------------------
                if (
                          sUserRoleList.Contains("ServrightChildFst")
                       || sUserRoleList.Contains("ServrightParentProvidingFsts")
                       || sUserRoleList.Contains("ServrightGrandparentToBePaid")
                       || sUserAccountType == "SRG"  // Default or Admin Impersonating this role
                       || sUserAccountType == "SRP"
                       || sUserAccountType == "SRC"
                   )
                {
                    MultiViewLeftNav.ActiveViewIndex = 4;
                    hlHelp.Text = "Contact Member Services?"; // Questions? Issues? Help?
                    hlHelp.NavigateUrl = "mailto:memberservices@scantron.com";
                }
                else if (sUserRoleList.Contains("SiteAdministrator"))
                {
                    MultiViewLeftNav.ActiveViewIndex = 2;
                }
                else if (sUserRoleList.Contains("CustomerAdministrator"))
                {
                    MultiViewLeftNav.ActiveViewIndex = 3;
                }
                else // Just a logged in user with no special roles
                {
                    MultiViewLeftNav.ActiveViewIndex = 1;
                }
            } // If they are logged in...

        }
    }
    // ============================================================================


    // ============================================================================
    // ============================================================================
}
