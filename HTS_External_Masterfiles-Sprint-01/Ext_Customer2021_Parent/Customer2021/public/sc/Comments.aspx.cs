using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;

public partial class public_sc_Comments : MyPage
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
            // Public page so user is NOT authenticated
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
    // ------------------------------------------------------------------------
    protected void ResetPage()
    {

        chBxAccounting.Checked = false;
        chBxDelivery.Checked = false;
        chBxLogistics.Checked = false;
        chBxProducts.Checked = false;
        chBxUtilities.Checked = false;

        txDelivery.Text = "";
        txLogistics.Text = "";
        txUtilities.Text = "";
        txAccounting.Text = "";
        txProducts.Text = "";
        txGeneral.Text = "";

        pnDelivery.Visible = false;
        pnLogistics.Visible = false;
        pnUtilities.Visible = false;
        pnAccounting.Visible = false;
        pnProducts.Visible = false;

    }
    // ------------------------------------------------------------------------
    protected string Validate_Comment()
    {
        string sResult = "";
        string sEntryFound = "N";
        string sPhone = "";
        //string sEmail = "";
        bool bIsEmailFormatValid = false;
        lbMsg.Text = "";
        //int iNum = 0;

        txAccounting.Text = scrub(txAccounting.Text);
        txContact.Text = scrub(txContact.Text);
        txDelivery.Text = scrub(txDelivery.Text);
        txGeneral.Text = scrub(txGeneral.Text);
        txLogistics.Text = scrub(txLogistics.Text);
        txProducts.Text = scrub(txProducts.Text);
        txUtilities.Text = scrub(txUtilities.Text);

        try
        {
            if (!String.IsNullOrEmpty(txGeneral.Text.Trim()))
                sEntryFound = "Y";
            else if (!String.IsNullOrEmpty(txContact.Text.Trim()))
                sEntryFound = "Y";
            else if (chBxAccounting.Checked == true && !String.IsNullOrEmpty(txAccounting.Text))
                sEntryFound = "Y";
            else if (chBxDelivery.Checked == true && !String.IsNullOrEmpty(txDelivery.Text))
                sEntryFound = "Y";
            else if (chBxLogistics.Checked == true && !String.IsNullOrEmpty(txLogistics.Text))
                sEntryFound = "Y";
            else if (chBxProducts.Checked == true && !String.IsNullOrEmpty(txProducts.Text))
                sEntryFound = "Y";
            else if (chBxUtilities.Checked == true && !String.IsNullOrEmpty(txUtilities.Text))
                sEntryFound = "Y";

            if (sEntryFound != "Y")
            {
                lbMsg.Text = "Please include a general comment or select a specific area for your input";
                chBxDelivery.Focus();
            }

            sPhone = Clean_PhoneEntry(txPhone.Text.Trim());

            if (sPhone.Length != 10)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text = "Phone entry must contain 10 numbers";
                txPhone.Focus();
            }

            if (!String.IsNullOrEmpty(txEmail.Text.Trim()))
            {
                bIsEmailFormatValid = isEmailFormatValid(txEmail.Text.Trim());
                if (bIsEmailFormatValid == false)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text += "Email entry format appears to be invalid";
                    txEmail.Focus();
                }
            }


            if (!String.IsNullOrEmpty(txContact.Text.Trim()) && txContact.Text.Length > 100)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "Contact must be 100 characters or less";
                txContact.Focus();
            }

            if ((pnDelivery.Visible == true) && (txDelivery.Text != ""))
            {
                if (txDelivery.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Delivery comment must be 1000 characters or less";
                    txDelivery.Text = txDelivery.Text.Substring(0, 1000);
                    txDelivery.Focus();
                }
            }

            if ((pnLogistics.Visible == true) && (txLogistics.Text != ""))
            {
                if (txLogistics.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Logistics comment must be 1000 characters or less";
                    txLogistics.Text = txLogistics.Text.Substring(0, 1000);
                    txLogistics.Focus();
                }
            }

            if ((pnUtilities.Visible == true) && (txUtilities.Text != ""))
            {
                if (txUtilities.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Utility comment must be 1000 characters or less";
                    txUtilities.Text = txUtilities.Text.Substring(0, 1000);
                    txUtilities.Focus();
                }
            }

            if ((pnAccounting.Visible == true) && (txAccounting.Text != ""))
            {
                if (txAccounting.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Accounting comment must be 1000 characters or less";
                    txAccounting.Text = txAccounting.Text.Substring(0, 1000);
                    txAccounting.Focus();
                }
            }
            if ((pnProducts.Visible == true) && (txProducts.Text != ""))
            {

                if (txProducts.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Product comment must be 1000 characters or less";
                    txProducts.Text = txProducts.Text.Substring(0, 1000);
                    txProducts.Focus();
                }
            }

            if (txGeneral.Text != "")
            {
                if (txGeneral.Text.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "General comment must be 1000 characters or less";
                    txGeneral.Text = txGeneral.Text.Substring(0, 1000);
                    txGeneral.Focus();
                }
            }

            // -------------------
            if (lbMsg.Text == "")
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text = "A unexpected system error has occurred";
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // ----------------------------------------------------------------------------
    protected override object LoadPageStateFromPersistenceMedium()
    {
        return Session["_ViewState"];
    }
    // ----------------------------------------------------------------------------
    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
        Session["_ViewState"] = viewState;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ========================================================================
    // WS: STRINGS (start)
    // ========================================================================
    // ------------------------------------------------------------------------
    protected DataTable ws_Get_B1EmailAddressesByCodeForEmployees(
        string code,
        string employeeNumber // optional almost all the time
        )
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(code))
        {
            string sJobName = "Get_B1EmailAddressesByCodeForEmployees";
            string sFieldList = "code|employeeNumber|x";
            string sValueList =
                code + "|" +
                employeeNumber + "|" +
                "x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    // WS: STRINGS (end)
    // ========================================================================
    // ========================================================================
    // WS: DATA TABLES (start)
    // ========================================================================
    // WS: DATA TABLES (end)
    // ========================================================================
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void chBxGroup_CheckedChanged(object sender, EventArgs e)
    {
        string sText = "";
        CheckBox cbControl = (CheckBox)sender;
        sText = cbControl.Text;

        if (sText == "Service Delivery")
        {
            if (cbControl.Checked)
                pnDelivery.Visible = true;
            else
                pnDelivery.Visible = false;
        }
        if (sText == "Service Logistics")
        {
            if (cbControl.Checked)
                pnLogistics.Visible = true;
            else
                pnLogistics.Visible = false;
        }

        if (sText == "ServiceCOMMAND® Utilities")
        {
            if (cbControl.Checked)
                pnUtilities.Visible = true;
            else
                pnUtilities.Visible = false;
        }
        if (sText == "Accounting/Invoicing")
        {
            if (cbControl.Checked)
                pnAccounting.Visible = true;
            else
                pnAccounting.Visible = false;
        }
        if (sText == "Service Offerings")
        {
            if (cbControl.Checked)
                pnProducts.Visible = true;
            else
                pnProducts.Visible = false;
        }
    }
    // ------------------------------------------------------------------------
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string sValid = Validate_Comment();

        if (sValid == "VALID")
        {
            string sContact = "";
            string sEmail = "";
            string sPhone = "";
            string sComment = "";
            string sDelivery = "";
            string sLogistics = "";
            string sUtilities = "";
            string sAccounting = "";
            string sProducts = "";
            string sGeneral = "";
            string sSubject = "";
            //string sUserName = "";
            string sEmailFrom = "";
            string sEmailTo = "";
            string sResult = "";
            string sEmailResult = "";
            string sPrimaryCustomerName = "";
            int iPrimaryCustomer = 0;

            try
            {
                sContact = txContact.Text.Trim();
                sEmail = txEmail.Text.Trim();
                sPhone = Clean_PhoneEntry(txPhone.Text.Trim());
                if (sPhone.Length == 10) 
                {
                    sPhone = FormatPhone1(sPhone);
                    if (txExtension.Text != "")
                        sPhone += "  Ext: " + txExtension.Text.Trim();
                }

                sDelivery = txDelivery.Text.Trim();
                sLogistics = txLogistics.Text.Trim();
                sUtilities = txUtilities.Text.Trim();
                sAccounting = txAccounting.Text.Trim();
                sProducts = txProducts.Text.Trim();
                sGeneral = txGeneral.Text.Trim();

                // Build HTML Email Content
                sSubject = "Customer Comments";
                if (sContact != "")
                    sSubject += " from " + sContact;

                sComment = "<html><head><title>" +
                    HttpUtility.HtmlEncode(sSubject) +
                "</title>" +
                "<style>" +
                "body { font-family: verdana; font-size: 13px; margin-left: 30px; }" +
                "</style>" +
                "</head><body>";

                sComment += "<p><b>Contact</b><br />";
                if (sContact != "")
                    sComment += HttpUtility.HtmlEncode(sContact);
                else
                    sComment += "No contact name given...";
                sComment += "</p>";

                if (sGeneral != "")
                    sComment += "<p><b>General Comments</b><br /> " + HttpUtility.HtmlEncode(sGeneral) + "</p>";

                if ((pnDelivery.Visible == true) && (sDelivery != ""))
                    sComment += "<p><b>Service Delivery</b><br />" + HttpUtility.HtmlEncode(sDelivery) + "</p>";

                if ((pnLogistics.Visible == true) && (sLogistics != ""))
                    sComment += "<p><b>Service Logistics</b><br />" + HttpUtility.HtmlEncode(sLogistics) + "</p>";

                if ((pnUtilities.Visible == true) && (sUtilities != ""))
                    sComment += "<p><b>ServiceCOMMAND® Utilities</b><br /> " + HttpUtility.HtmlEncode(sUtilities) + "</p>";

                if ((pnAccounting.Visible == true) && (sAccounting != ""))
                    sComment += "<p><b>Accounting / Invoicing</b><br /> " + HttpUtility.HtmlEncode(sAccounting) + "</p>";

                if ((pnProducts.Visible == true) && (sProducts != ""))
                    sComment += "<p><b>Service Offerings</b><br /> " + HttpUtility.HtmlEncode(sProducts) + "</p>";

                if (txEmail.Text != "")
                    sComment += "<p><b>Customer</b><br />" + iPrimaryCustomer.ToString() + "  " + sPrimaryCustomerName + "</p>";

                if (txEmail.Text != "")
                    sComment += "<p><b>Email</b><br />" + HttpUtility.HtmlEncode(sEmail) + "</p>";

                if (sPhone != "")
                    sComment += "<p><b>Phone</b><br />" + HttpUtility.HtmlEncode(sPhone) + "</p>";

                sComment += "</body></html>";

                EmailHandler emailHandler = new EmailHandler();
                sEmailFrom = "adv320@scantron.com";

                DataTable dt = ws_Get_B1EmailAddressesByCodeForEmployees("C07", "");
                if (sLibrary == "OMDTALIB") 
                {
                    sResult = emailHandler.EmailGroup(sSubject, sComment, dt, sEmailFrom);
                }
                
//                foreach (DataRow row in dt.Rows)
//                {
//                    sEmailTo = row["Email"].ToString().Trim();
//                    sResult = emailHandler.EmailIndividual(sSubject, sComment, sEmailTo, sEmailFrom);
//                }

                sEmailTo = "steve.carlson@scantron.com";
                sResult = emailHandler.EmailIndividual(sSubject, sComment, sEmailTo, sEmailFrom);
                
                emailHandler = null;

                if (sResult.StartsWith("SUCCESS"))
                {
                    lbMsg.Text = "Email Successfully Sent";
                    ResetPage();
                }
                else
                {
                    lbMsg.Text = "There was a problem with the email";
                }
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMsg.Text = "Error: There was a problem processing the email";
            }
            finally
            {
            }
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
