using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class private_customerAdministration_CustomerPreferences : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsgUpdateRegistrationStatus.Text = "";

        if (!IsPostBack) 
        {
            int iCustomerNumber = Get_UserPrimaryCustomerNumber();

            if (iCustomerNumber > 0) 
            {
                string sRegistrationOpenOrClosed = ws_Get_B1CustPref_UserRegistrationForCustomer_OpenOrClosed(iCustomerNumber);
                if (sRegistrationOpenOrClosed.ToLower() == "open") 
                {
                    lbCurrentRegistrationStatus_OpenOrClosed.Text = "Current Status: OPEN";
                    btUpdateRegistrationStatus_ToOpenOrClosed.Text = "Change Status to CLOSED";
                }
                else 
                {
                    lbCurrentRegistrationStatus_OpenOrClosed.Text = "Current Status: CLOSED";
                    btUpdateRegistrationStatus_ToOpenOrClosed.Text = "Change Status to OPEN";
                }
            }
        }
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ----------------------------------------------------------------------------
    protected int Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 2) 
            {
                hfPrimaryCs1.Value = saPreNumTyp[1];
                hfPrimaryCs1Type.Value = saPreNumTyp[2];
            }

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
        }
        return iCustomerNumber;
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btUpdateRegistrationStatus_ToOpenOrClosed_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        Button myControl = (Button)sender;
        string sButtonText = myControl.Text.ToString();
        string sSuccessOrFailure = "";

        lbMsgUpdateRegistrationStatus.Text = "";

        int iCustomerNumber = 0;
        if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
            iCustomerNumber = -1;
        if (iCustomerNumber > 0)
        {
            if (sButtonText == "Change Status to OPEN")
            {
                sSuccessOrFailure = ws_Upd_B1CustPref_ToggleRegistrationToOpenOrClosed(iCustomerNumber.ToString(), "Open");
                if (sSuccessOrFailure == "SUCCESS")
                {
                    lbCurrentRegistrationStatus_OpenOrClosed.Text = "Current Status: OPEN";
                    btUpdateRegistrationStatus_ToOpenOrClosed.Text = "Change Status to CLOSED";
                    lbMsgUpdateRegistrationStatus.Text = "Status successfully updated to open";
                }
                else 
                {
                    lbMsgUpdateRegistrationStatus.Text = "Error: Status update was NOT successful (status is still closed)";
                }
            }
            else  // buttonText = "Change Status to CLOSED"
            {
                sSuccessOrFailure = ws_Upd_B1CustPref_ToggleRegistrationToOpenOrClosed(iCustomerNumber.ToString(), "Closed");
                if (sSuccessOrFailure == "SUCCESS")
                {
                    lbCurrentRegistrationStatus_OpenOrClosed.Text = "Current Status: CLOSED";
                    btUpdateRegistrationStatus_ToOpenOrClosed.Text = "Change Status to OPEN";
                    lbMsgUpdateRegistrationStatus.Text = "Status successfully updated to closed";
                }
                else
                {
                    lbMsgUpdateRegistrationStatus.Text = "Error: Status update was NOT successful (status is still open)";
                }
            }
        }
    }
    // ------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================


}
