using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_sc_Comments : MyPage
{
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            string sEncrypted = Request.QueryString["key"];
            int[] iaCtrTck = new int[2];
            iaCtrTck[0] = 0;
            iaCtrTck[1] = 0;
            if ((sEncrypted != null) && (sEncrypted != ""))
                iaCtrTck = sfd.GetTicketDecrypted(sEncrypted);
            hfCtr.Value = iaCtrTck[0].ToString();
            hfTck.Value = iaCtrTck[1].ToString();
        }
    }
    // =========================================================
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
    // =========================================================
        protected void btSubmit_Click(object sender, EventArgs e)
    {
        string sValid = ServerSideVal_Comment();
        
        if (sValid == "VALID")
        {
            Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
            Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();

            string sResult = "";
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
            string sCs1Name = "";
            string sUserName = "";
            int iCs1 = 0;
            int iCtr = 0;
            int iTck = 0;

            try
            {
                if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1) == false)
                    iCs1 = 0;

                if ((hfCtr.Value != "0") && (hfTck.Value != "0")) 
                {
                    int.TryParse(hfCtr.Value, out iCtr);
                    int.TryParse(hfTck.Value, out iTck);

                    if (sPageLib == "L")
                    {
                        iCs1 = wsLive.GetTicketCs1(sfd.GetWsKey(), iCtr, iTck);
                    }
                    else
                    {
                        iCs1 = wsTest.GetTicketCs1(sfd.GetWsKey(), iCtr, iTck);
                    }
                }

                if (iCs1 > 0) 
                {
                    if (sPageLib == "L")
                    {
                        sCs1Name = wsLive.GetCustName(sfd.GetWsKey(), iCs1, 0);
                    }
                    else
                    {
                        sCs1Name = wsTest.GetCustName(sfd.GetWsKey(), iCs1, 0);
                    }
                }

                if (User.Identity.IsAuthenticated) 
                    sUserName = User.Identity.Name.ToString();
                else
                    sUserName = "Anonymous (Not logged in)";

                sContact = txContact.Text.Trim();
                sEmail = txEmail.Text.Trim();

                if ((txPhone1.Text != "") || (txPhone2.Text != "") || (txPhone3.Text != ""))
                {
                    sPhone = "(" + txPhone1.Text.Trim() + ") " + txPhone2.Text.Trim() + "-" + txPhone3.Text.Trim();
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

                if (iCtr > 0 && iTck > 0) 
                {
                    sComment += "<p><b>Ticket</b><br />" + 
                        iCtr.ToString() + "-" + iTck.ToString() + "</p>";
                }

                sComment += "<p><b>Contact</b><br />";
                if (sContact != "")
                    sComment += HttpUtility.HtmlEncode(sContact);
                else
                    sComment += "No name given...";
                sComment += "</p>";

                if (sCs1Name != "")
                    sComment += "<p><b>Customer</b><br />" + iCs1.ToString() + "  " + sCs1Name + "</p>";

                if (sUserName != "")
                    sComment += "<p><b>Web Username</b><br />" + sUserName + "</p>";

                if (txEmail.Text != "")
                    sComment += "<p><b>Email</b><br />" + HttpUtility.HtmlEncode(sEmail) + "</p>";

                if (sPhone != "")
                    sComment += "<p><b>Phone</b><br />" + HttpUtility.HtmlEncode(sPhone) + "</p>";
                
                sComment += "</body></html>";

                if (sPageLib == "L")
                {
                    sResult = wsLive.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C07");
                }
                else
                {
                    sResult = wsTest.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C07");
                }
                if (sResult == "SUCCESS")
                {
                    lbError.Text = "Email Successfully Sent";
                    ResetPage();
                }
                else
                {
                    lbError.Text = "There was a problem with the email";
                }
            }
            catch (Exception ex)
            {
                sResult = ex.ToString();
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbError.Text = "A problem occurred sending the email";
            }
            finally
            {
            }
        }
    }
    // =========================================================
    protected string ServerSideVal_Comment()
    {
        string sResult = "";
        lbError.Text = "";
        int iNum = 0;

        try
        {
            if (lbError.Text == "")
            {
                if ((txAccounting.Text == "") && 
                    (txContact.Text == "") && 
                    (txDelivery.Text == "") && 
                    (txGeneral.Text == "") && 
                    (txLogistics.Text == "") && 
                    (txProducts.Text == "") && 
                    (txUtilities.Text == ""))
                {
                    lbError.Text = "Please include a general comment or select a specific area for your remark";
                    chBxDelivery.Focus();
                }
            }
            if (lbError.Text == "")
            {
                if (txPhone2.Text != "")
                {

                    if (int.TryParse(txPhone1.Text, out iNum) == false)
                    {
                        lbError.Text = "The area code must be a number";
                        txPhone1.Focus();
                    }
                    else
                    {
                        if (txPhone1.Text.Length != 3)
                        {
                            lbError.Text = "The area code must be 3 digits";
                            txPhone1.Focus();
                        }
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txPhone2.Text != "")
                {
                    if (int.TryParse(txPhone2.Text, out iNum) == false)
                    {
                        lbError.Text = "The phone prefix must be a number";
                        txPhone2.Focus();
                    }
                    else
                    {
                        if (txPhone2.Text.Length != 3)
                        {
                            lbError.Text = "The phone prefix must be 3 digits";
                            txPhone2.Focus();
                        }
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txPhone3.Text != "")
                {
                    if (int.TryParse(txPhone3.Text, out iNum) == false)
                    {
                        lbError.Text = "The phone suffix must be a number";
                        txPhone3.Focus();
                    }
                    else
                    {
                        if (txPhone3.Text.Length != 4)
                        {
                            lbError.Text = "The phone suffix must be four digits";
                            txPhone3.Focus();
                        }
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txExtension.Text != "")
                {
                    if (int.TryParse(txExtension.Text, out iNum) == false)
                    {
                        lbError.Text = "The extension must be a number";
                        txExtension.Focus();
                    }
                    else
                    {
                        if (iNum > 99999999)
                        {
                            lbError.Text = "The extension may be no more than eight digits";
                            txExtension.Focus();
                        }
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txContact.Text != "")
                {
                    if (txContact.Text.Length > 100)
                    {
                        lbError.Text = "Contact must be 100 characters or less";
                        txContact.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txGeneral.Text != "")
                {
                    if (txGeneral.Text.Length > 1000)
                    {
                        lbError.Text = "General comment must be 1000 characters or less";
                        txGeneral.Text = txGeneral.Text.Substring(0, 1000);
                        txGeneral.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if ((pnDelivery.Visible == true) && (txDelivery.Text != ""))
                {
                    if (txDelivery.Text.Length > 1000)
                    {
                        lbError.Text = "Delivery comment must be 1000 characters or less";
                        txDelivery.Text = txDelivery.Text.Substring(0, 1000);
                        txDelivery.Focus();
                    }
                }
            }

            if (lbError.Text == "")
            {
                if ((pnLogistics.Visible == true) && (txLogistics.Text != ""))
                {
                    if (txLogistics.Text.Length > 1000)
                    {
                        lbError.Text = "Logistics comment must be 1000 characters or less";
                        txLogistics.Text = txLogistics.Text.Substring(0, 1000);
                        txLogistics.Focus();
                    }
                }
            }

            if (lbError.Text == "")
            {
                if ((pnUtilities.Visible == true) && (txUtilities.Text != ""))
                {
                    if (txUtilities.Text.Length > 1000)
                    {
                        lbError.Text = "Utility comment must be 1000 characters or less";
                        txUtilities.Text = txUtilities.Text.Substring(0, 1000);
                        txUtilities.Focus();
                    }
                }
            }

            if (lbError.Text == "")
            {
                if ((pnAccounting.Visible == true) && (txAccounting.Text != ""))
                {
                    if (txAccounting.Text.Length > 1000)
                    {
                        lbError.Text = "Accounting comment must be 1000 characters or less";
                        txAccounting.Text = txAccounting.Text.Substring(0, 1000);
                        txAccounting.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if ((pnProducts.Visible == true) && (txProducts.Text != ""))
                {
                    if (txProducts.Text.Length > 1000)
                    {
                        lbError.Text = "Product comment must be 1000 characters or less";
                        txProducts.Text = txProducts.Text.Substring(0, 1000);
                        txProducts.Focus();
                    }
                }
            }
            // -------------------
            if (lbError.Text == "")
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbError.Text = "A unexpected system error has occurred";
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
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
    // =========================================================
    // =========================================================
}