using System;
using System.Web.Security;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
public partial class private_Menu : MyPage
{
    string sTemp = "";

    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ShowNewCompanyName() == true) 
        {
            lbCompanyName1.Text = "Secur-Serv's";
            lbCompanyName2.Text = "Secur-Serv";
            lbCompanyName3.Text = "Secur-Serv";
        }
            
        else 
        {
            lbCompanyName1.Text = "Scantron Technology Solutions'";
            lbCompanyName2.Text = "Scantron Technology Solutions";
            lbCompanyName3.Text = "Scantron Technology Solutions";
        }

        if (!IsPostBack == true) 
        {
            GetUser();
            Get_UserPrimaryCustomerNumber();
        }
    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // -------------------------------------------------------------------------------------------------------
    protected void GetUser()
    {
        pnRegular.Visible = false;
        pnAdmin.Visible = false;
        pnServright.Visible = false;

        txAdminCustomerNumber.Text = "";

        if (User.Identity.IsAuthenticated)
        {
            // -------------------------------------------------
            if (User.IsInRole("SiteAdministrator"))
                {
                    MembershipUser mu = Membership.GetUser();
                    //if (mu.UserName.ToString().ToLower() == "steve.carlson@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "isabel.labrador@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "sarah.engels@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "vern.kathol@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "dana.freeman@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "april.wiggins@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "randy.lorenzen@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "herb.stebbins@scantron.com"
                    //    || mu.UserName.ToString().ToLower() == "amy.garner@scantron.com"
                    //    )
                    //{
                    pnAdmin.Visible = true;
                    lbAdminCust.Text = "Account in use: ";
                    if (Session["AdminCustomerNumber"] != null)
                    {
                        txAdminCustomerNumber.Text = Session["AdminCustomerNumber"].ToString().Trim();
                        int iCustomerNumber = 0;
                        if (int.TryParse(txAdminCustomerNumber.Text, out iCustomerNumber) == false)
                            iCustomerNumber = -1;
                        if (iCustomerNumber > 0)
                            lbCustomerInUse.Text = ws_Get_B1CustomerName(iCustomerNumber.ToString(), "");
                        else
                            lbCustomerInUse.Text = "";
                    }
                    else
                    {
                        txAdminCustomerNumber.Text = "99999";
                        lbCustomerInUse.Text = "PATTY'S PLACE";
                    }
                    //} // Restriction to specific admin names now commented Fri May 13th, 2022
                }
            // -------------------------------------------------
        }
    }
    // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 1)
                hfPrimaryCs1.Value = saPreNumTyp[1];

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0)
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
            }

            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
            if (iCustomerNumber > 0) 
            {
                sTemp = ws_Get_B1CustomerType(iCustomerNumber);
                if (sTemp.StartsWith("ERROR"))
                    sTemp = "REG"; // sTemp = "";
                hfPrimaryCs1Type.Value = sTemp;
                if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "") 
                {
                    Session["AdminCustomerType"] = hfPrimaryCs1Type.Value;
                }

                string sTyp = "";
                if (hfPrimaryCs1Type.Value == "REG") sTyp = "Regular";
                else if (hfPrimaryCs1Type.Value == "LRG") sTyp = "Large";
                else if (hfPrimaryCs1Type.Value == "DLR") sTyp = "Dealer";
                else if (hfPrimaryCs1Type.Value == "SRC") sTyp = "Servright Child";
                else if (hfPrimaryCs1Type.Value == "SRP") sTyp = "Servright Parent";
                else if (hfPrimaryCs1Type.Value == "SRG") sTyp = "Servright Grandparent";
                else if (hfPrimaryCs1Type.Value == "SSB") sTyp = "Self-Service Both";
                else if (hfPrimaryCs1Type.Value == "SSP") sTyp = "Self-Service Parts";

                lbAccountType.Text = "Type: " + sTyp;
                string sDisplayEmailManagement = ws_Get_B1CustPref_AllowEmailManagement_YN(iCustomerNumber.ToString());
                if (sDisplayEmailManagement == "Y")
                    pnEmailManagementLink.Visible = true;
                else
                    pnEmailManagementLink.Visible = false;


                // -------------------------------------------------
                // Show/Hide links based on user's role
                // -------------------------------------------------
                string sDebug = "";
                if (hfPrimaryCs1Type.Value == "SRG" || User.IsInRole("ServrightGrandparentToBePaid"))
                {
                    sDebug = "Grand";
                    pnServright.Visible = true;
                }
                else if (hfPrimaryCs1Type.Value == "SRP" || User.IsInRole("ServrightParentProvidingFsts"))
                {
                    sDebug = "Parent";
                    pnServright.Visible = true;
                }
                else if (hfPrimaryCs1Type.Value == "SRC" || User.IsInRole("ServrightChildFst"))
                {
                    sDebug = "Child";
                    pnServright.Visible = true;
                }
                else
                {
                    pnRegular.Visible = true;
                }

                // -------------------------------------------------
                if (User.IsInRole("CustomerAdministrator") || User.IsInRole("SiteAdministrator"))
                {
                    pnCustomerAdministrationLink.Visible = true;
                    //pnInvoiceLink.Visible = true;
                }
                else
                {
                    pnCustomerAdministrationLink.Visible = false;
                    //pnInvoiceLink.Visible = false;
                }

            }

        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btAdminCustomerSubmit_Click(object sender, EventArgs e)
    {
        int iCustomerNumberAlternate = 0;
        //lbAdminCust.Text = "Customer: ";
        if (!String.IsNullOrEmpty(txAdminCustomerNumber.Text))
        {
            if (int.TryParse(txAdminCustomerNumber.Text, out iCustomerNumberAlternate) == false)
                iCustomerNumberAlternate = -1;
            if (iCustomerNumberAlternate > 0)
            {
                Session["AdminCustomerNumber"] = iCustomerNumberAlternate.ToString();
                if (iCustomerNumberAlternate > 0)
                {
                    sTemp = ws_Get_B1CustomerName(iCustomerNumberAlternate.ToString(), "");
                    if (sTemp.StartsWith("ERROR"))
                        sTemp = "";

                    lbCustomerInUse.Text = sTemp;
                    Session["DisplayEmailManagement"] = ws_Get_B1CustPref_AllowEmailManagement_YN(iCustomerNumberAlternate.ToString());
                    Response.Redirect("~/private/Menu.aspx");
                }
                else
                    lbCustomerInUse.Text = "";

            }
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}