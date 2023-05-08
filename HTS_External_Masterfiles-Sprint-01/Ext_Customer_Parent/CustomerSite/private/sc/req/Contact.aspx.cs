using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;

public partial class private_sc_req_Contact : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    SourceForCustomer sfc = new SourceForCustomer();

    char[] cSplitter = { ',' };
    char[] cSplitter2 = { '|' };
    char[] cSplitter3 = { '~' };

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                hfPri.Value = PreviousPage.pp_Pri().ToString();
                hfCs1.Value = PreviousPage.pp_Cs1().ToString();
                hfCs2.Value = PreviousPage.pp_Cs2().ToString();
                LoadPanelContact();
            }
            else // This will stop bookmarking this page (but allow backing in from later page) 
                Response.Redirect("~/private/sc/req/Location.aspx", false);
        }
    }
    // =========================================================
    protected void LoadPanelContact()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        NumberFormatter nf = new NumberFormatter();

        int iCs1 = 0;
        int iCs2 = 0;
        int.TryParse(hfCs1.Value, out iCs1);
        int.TryParse(hfCs2.Value, out iCs2);
        
        // Load Header with customer values
        pnCs1Header.Controls.Add(sfc.GetCustDataTable(iCs1, iCs2, "", "", ""));

        string sLoadContactPhone = "";
        string sPmRequest = "";

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetCustBasics(sfd.GetWsKey(), iCs1, iCs2);
            sLoadContactPhone = wsLive.GetPrefRequestContactPhone(sfd.GetWsKey(), iCs1);
            sPmRequest = wsLive.GetPrefPmRequest(sfd.GetWsKey(), iCs1);
        }
        else
        {
            dataTable = wsTest.GetCustBasics(sfd.GetWsKey(), iCs1, iCs2);
            sLoadContactPhone = wsTest.GetPrefRequestContactPhone(sfd.GetWsKey(), iCs1);
            sPmRequest = wsTest.GetPrefPmRequest(sfd.GetWsKey(), iCs1);
        }

        if (dataTable.Rows.Count > 0)
        {
            if (sLoadContactPhone == "AUTOLOAD")
            {
                // Call Center Requests Contact and Phone Not Auto filled (make user fill it out) 
                // Users complained: some get a cust pref exception...
                txContact.Text = dataTable.Rows[0]["Contact"].ToString().Trim();
                string sPhone = dataTable.Rows[0]["Phone"].ToString().Trim();
                string[] saPhone = nf.phoneToThreeParts(sPhone);
                txPhone1.Text = saPhone[0];
                txPhone2.Text = saPhone[1];
                txPhone3.Text = saPhone[2];
                lbPhoneEntry.Text = "Verify company phone number";
                lbContactEntry.Text = "Verify name of company contact";
            }
            else 
            {
                lbPhoneEntry.Text = "Enter company phone number";
                lbContactEntry.Text = "Enter name of company contact";
            }
        }
        // Change Entry Options if CPXA46 = "Y" (Currently to allow ARCA to make PMs off Web) 
        if (sPmRequest == "Y") 
        {
            lbListTitle.Text = "Select the type of service needed (PM or Service) from your location list";
            rbList.Text = "Service Request from your list";
            rbPm.Visible = true;
        }

        int iValue = 0;
        for (int i = 0; i < 25; i++) 
        { 
            iValue = i + 1;
            ddForcedQty.Items.Insert(i, new System.Web.UI.WebControls.ListItem(iValue.ToString(), iValue.ToString()));
        }
        ddMethod.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Preference (phone above will be called)", "NON"));
        ddMethod.Items.Insert(1, new System.Web.UI.WebControls.ListItem("By Email (provide email address below)", "EML"));
        ddMethod.Items.Insert(2, new System.Web.UI.WebControls.ListItem("By Text Message (provide mobile phone number)", "TXT"));
        ddMethod.Items.Insert(3, new System.Web.UI.WebControls.ListItem("By Phone Call (provide preferred phone number)", "PHN"));
    }
    // =========================================================
    // SERVER SIDE VALIDATION
    // =========================================================
    protected string ServerSideVal_Contact()
    {
        string sResult = "";
        int iNum = 0;

        txContact.Text = txContact.Text.ToUpper();

        try
        {
            // Phone 1
            if (vCustom_Contact.IsValid == true)
            {
                if (txPhone1.Text == "")
                {
                    vCustom_Contact.ErrorMessage = "An area code is required";
                    vCustom_Contact.IsValid = false;
                    txPhone1.Focus();
                }
                else
                {
                    if (int.TryParse(txPhone1.Text, out iNum) == false)
                    {
                        vCustom_Contact.ErrorMessage = "The area code must be a number";
                        vCustom_Contact.IsValid = false;
                        txPhone1.Focus();
                    }
                    else
                    {
                        if (txPhone1.Text.Length != 3)
                        {
                            vCustom_Contact.ErrorMessage = "The area code must be 3 digits";
                            if (txPhone1.Text.Length > 3)
                                txPhone1.Text = txPhone1.Text.Substring(0, 3);
                            vCustom_Contact.IsValid = false;
                            txPhone1.Focus();
                        }
                        if (iNum < 1)
                        {
                            vCustom_Contact.ErrorMessage = "The area code must not be zeros";
                            txPhone1.Text = txPhone1.Text.Substring(0, 3);
                            txPhone1.Focus();
                        }
                    }
                }
            }
            // Phone 2
            if (vCustom_Contact.IsValid == true)
            {
                if (txPhone2.Text == "")
                {
                    vCustom_Contact.ErrorMessage = "An phone prefix is required";
                    vCustom_Contact.IsValid = false;
                    txPhone2.Focus();
                }
                else
                {
                    if (int.TryParse(txPhone2.Text, out iNum) == false)
                    {
                        vCustom_Contact.ErrorMessage = "The phone prefix must be a number";
                        vCustom_Contact.IsValid = false;
                        txPhone2.Focus();
                    }
                    else
                    {
                        if (txPhone2.Text.Length != 3)
                        {
                            vCustom_Contact.ErrorMessage = "The phone prefix must be 3 digits";
                            vCustom_Contact.IsValid = false;
                            if (txPhone2.Text.Length > 3)
                                txPhone2.Text = txPhone2.Text.Substring(0, 3);
                            txPhone2.Focus();
                        }
                        if (iNum < 1)
                        {
                            vCustom_Contact.ErrorMessage = "The phone prefix must not be zeros";
                            vCustom_Contact.IsValid = false;
                            txPhone2.Focus();
                        }
                    }
                }
            }

            // Phone 3
            if (vCustom_Contact.IsValid == true)
            {
                if (txPhone3.Text == "")
                {
                    vCustom_Contact.ErrorMessage = "An phone suffix is required";
                    vCustom_Contact.IsValid = false;
                    txPhone3.Focus();
                }
                else
                {
                    if (int.TryParse(txPhone3.Text, out iNum) == false)
                    {
                        vCustom_Contact.ErrorMessage = "The phone suffix must be a number";
                        vCustom_Contact.IsValid = false;
                        txPhone3.Focus();
                    }
                    else
                    {
                        if (txPhone3.Text.Length != 4)
                        {
                            vCustom_Contact.ErrorMessage = "The phone suffix must be 4 digits";
                            vCustom_Contact.IsValid = false;
                            if (txPhone3.Text.Length > 4)
                                txPhone3.Text = txPhone3.Text.Substring(0, 4);
                            txPhone3.Focus();
                        }
                    }
                }
            }

            // Extension
            if (vCustom_Contact.IsValid == true)
            {
                if (txExtension.Text != "")
                {
                    if (int.TryParse(txExtension.Text, out iNum) == false)
                    {
                        vCustom_Contact.ErrorMessage = "The extension must be a number";
                        vCustom_Contact.IsValid = false;
                        txExtension.Focus();
                    }
                    else
                    {
                        if (iNum > 99999999)
                        {
                            vCustom_Contact.ErrorMessage = "The extension may be no more than eight digits";
                            vCustom_Contact.IsValid = false;
                            if (txExtension.Text.Length > 8)
                                txExtension.Text = txExtension.Text.Substring(0, 8);
                            txExtension.Focus();
                        }
                    }
                }
            }
            // Contact
            if (vCustom_Contact.IsValid == true)
            {
                if (txContact.Text != "")
                {
                    if (txContact.Text.Length > 30)
                    {
                        vCustom_Contact.ErrorMessage = "Contact must be 30 characters or less";
                        vCustom_Contact.IsValid = false;
                        txContact.Text = txContact.Text.Substring(0, 6);
                        txContact.Focus();
                    }
                }
            }
            string sTemp = "";
            if (vCustom_Contact.IsValid == true)
            {
                if (ddMethod.SelectedValue == "EML")
                {
                    if (txMethodInfo.Text.Trim() != "")
                    {
                        if (txMethodInfo.Text.Trim().Length > 50)
                        {
                            vCustom_Contact.ErrorMessage = "Email must be 50 characters or less";
                            vCustom_Contact.IsValid = false;
                            txMethodInfo.Focus();
                        }
                        if (!txMethodInfo.Text.Trim().Contains("@"))
                        {
                            vCustom_Contact.ErrorMessage = "Please verify the format of the email.";
                            vCustom_Contact.IsValid = false;
                            txMethodInfo.Focus();
                        }
                        //if (!txMethodInfo.Text.Contains(" ")) // This was not working! It was finding a space when it was not there!
                        if (txMethodInfo.Text.Trim().IndexOf(" ") > -1)
                        {
                            vCustom_Contact.ErrorMessage = "Spaces are not permitted within email addresses.";
                            vCustom_Contact.IsValid = false;
                            txMethodInfo.Focus();
                        }
                        sTemp = txMethodInfo.Text.Trim();
                        if (sTemp.IndexOf("@") > -1 && sTemp.LastIndexOf("@") > -1 && sTemp.IndexOf("@") != sTemp.LastIndexOf("@"))
                        {
                            vCustom_Contact.ErrorMessage = "Only one email address is permitted.";
                            vCustom_Contact.IsValid = false;
                            txMethodInfo.Focus();
                        }
                    }
                    else
                    {
                        vCustom_Contact.ErrorMessage = "Please enter an email for the technician to contact you";
                        vCustom_Contact.IsValid = false;
                        txMethodInfo.Focus();
                    }
                }
                else if (ddMethod.SelectedValue == "PHN")
                {
                    if (txMethodInfo.Text.Length != 10)
                    {
                        vCustom_Contact.ErrorMessage = "Please enter a 10-digit phone number with area code, prefix and suffix (numbers only).";
                        vCustom_Contact.IsValid = false;
                        txMethodInfo.Focus();
                    }
                    else if (txMethodInfo.Text.Length == 0)
                    {
                        vCustom_Contact.ErrorMessage = "Please enter a phone number for the technician to contact you";
                        vCustom_Contact.IsValid = false;
                        txMethodInfo.Focus();
                    }
                    if (txMethodPhoneExt.Text != "") 
                    {
                        int iExt = 0;
                        if (int.TryParse(txMethodPhoneExt.Text, out iExt) == false)
                            iExt = -1;
                        if (iExt <= 0) 
                        {
                            vCustom_Contact.ErrorMessage = "Please enter only numbers for the phone extension";
                            vCustom_Contact.IsValid = false;
                            txMethodPhoneExt.Focus();
                        }
                    }
                }
                else if (ddMethod.SelectedValue == "TXT")
                {
                    if (txMethodInfo.Text.Length != 10)
                    {
                        vCustom_Contact.ErrorMessage = "Please enter a 10-digit mobile phone number with area code, prefix and suffix (numbers only).";
                        vCustom_Contact.IsValid = false;
                        txMethodInfo.Focus();
                    }
                    else if (txMethodInfo.Text.Length == 0)
                    {
                        vCustom_Contact.ErrorMessage = "Please enter a mobile phone number for text messaging";
                        vCustom_Contact.IsValid = false;
                        txMethodInfo.Focus();
                    }
                }
                else 
                {
                    txMethodInfo.Text = "";
                    txMethodPhoneExt.Text = "";
                }
            }
            // ---------------------------------------
            if (vCustom_Contact.IsValid == true)
                sResult = "VALID";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            vCustom_Contact.ErrorMessage = "A unexpected system error has occurred";
            vCustom_Contact.IsValid = false;
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
    // START CLICK METHODS
    // =========================================================
    protected void btContact_Click(object sender, EventArgs e)
    {
        string sValid = "";

        if (Page.IsValid)
        {
            sValid = ServerSideVal_Contact();

            // If all server side validation is also passed...
            if (sValid == "VALID")
            {
                pnContact.Visible = false;
                Session["reqListPm"] = "";
                if (rbList.Checked == true || rbPm.Checked == true)
                {
                    if (rbPm.Checked == true)
                        Session["reqListPm"] = "PM";
                    else
                        Session["reqListPm"] = "LIST";

                    Server.Transfer("~/private/sc/req/Equipment.aspx", false);
                }
                else
                {
                    // Load Session Variables to send to problem page 
                    Session["reqPri"] = hfPri.Value.Trim();
                    Session["reqCs1"] = hfCs1.Value.Trim();
                    Session["reqCs2"] = hfCs2.Value.Trim();

                    Session["reqPhone"] = pp_Phone();
                    Session["reqExtension"] = pp_Extension();
                    Session["reqContact"] = pp_Contact();
                    Session["reqEmail"] = pp_Email();
                    Session["reqReqType"] = pp_ReqType();
                    Session["reqForcedQty"] = pp_ForcedQty().ToString();
                    
                    Session["reqSource"] = "Contact";
                    Session["methdInfo"] = pp_MethdInfo(); // I think she mismatched here variables here
                    Session["methdType"] = pp_MethdType();
                    Session["reqMthdI"] = pp_MethdInfo(); // So I am added what I think she was passing
                    Session["reqMthdT"] = pp_MethdType();
                    Session["reqMethodPhoneExt"] = pp_MethodPhoneExt();
                    Response.Redirect("~/private/sc/req/Problem.aspx", false);
                }
            }
        }
    }
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    // =========================================================
    // Previous Page Parms to load for next page retrieval 
    // =========================================================
    public int pp_Pri()
    {
        int iPri = 0;
        int.TryParse(hfPri.Value, out iPri);
        return iPri;
    }
    // =========================================================
    public int pp_Cs1()
    {
        int iCs1 = 0;
        int.TryParse(hfCs1.Value, out iCs1);
        return iCs1;
    }
    // =========================================================
    public int pp_Cs2()
    {
        int iCs2 = 0;
        int.TryParse(hfCs2.Value, out iCs2);
        return iCs2;
    }
    // =========================================================
    public string pp_Phone()
    {
        return txPhone1.Text.Trim() + txPhone2.Text.Trim() + txPhone3.Text.Trim();
    }
    // =========================================================
    public string pp_Extension()
    {
        return txExtension.Text.Trim();
    }
    // =========================================================
    public string pp_Contact()
    {
        return txContact.Text.Trim();
    }
    // =========================================================
    public string pp_Email()
    {
        return txEmail.Text.Trim();
    }
    // =========================================================
    public string pp_Creator()
    {
        return txCreator.Text.Trim().ToUpper();
    }
    // =========================================================
    public string pp_ReqType()
    {
        string sReqType = "";
        if (rbList.Checked == true)
            sReqType = "";
        else if (rbContract.Checked == true)
            sReqType = "AGR";
        else if (rbTM.Checked == true)
            sReqType = "TM";
        
        return sReqType;
    }
    // =========================================================
    public string pp_MethdType()
    {
        string sMethodType = ddMethod.SelectedValue;
        return sMethodType;
    }
    // =========================================================
    public string pp_MethdInfo()
    {
        return txMethodInfo.Text.Trim();
    }
    // =========================================================
    public string pp_MethodPhoneExt()
    {
        return txMethodPhoneExt.Text.Trim();
    }
    // =========================================================
    // =========================================================
    public int pp_ForcedQty()
    {
        int iForcedQty = 0;
        int.TryParse(ddForcedQty.SelectedValue, out iForcedQty);

        return iForcedQty;
    }
    // =========================================================
    // =========================================================
}

