using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;


public partial class private_sc_req_Serial : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    int[] iaCs1Cs2 = new int[2];

    //string sCs1Family = "";
    //string sChosenCs1Type = "";
    //string sCs1Changed = "";
    char[] cSplitter = { ',' };
    char[] cSplitter2 = { '|' };
    char[] cSplitter3 = { '~' };
    string[] saCs1All = new string[1];
    string[] saCs1Nam = new string[1];

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMove.Text = "";
        lbMove.Visible = false;
        int iCs1ToUse = 0;

        if (!IsPostBack)
        {
            if (PreviousPage == null || PreviousPage.IsCrossPagePostBack == false)
            {
                Response.Redirect("~/private/sc/req/Location.aspx", false);
            }
            else
            {
                hfPri.Value = PreviousPage.pp_Pri().ToString();
                hfCs1.Value = PreviousPage.pp_Cs1().ToString();
            }
            // If ARCA (or Patty's Place)
            if (
                   hfCs1.Value == "99999"
                || hfCs1.Value == "127"
                // || hfCs1.Value == "114818" 
                // || hfCs1.Value == "114819" 
                // || hfCs1.Value == "114820" 
                // || hfCs1.Value == "114821"
                )
            {
                pnPM.Visible = true;
                if (hfCs1.Value == "127")
                    rblPmRequest.SelectedValue = "PM";
            }
            else
                pnPM.Visible = false;
        }

        int.TryParse(hfCs1.Value, out iCs1ToUse);
    }
    // =========================================================
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (lbMove.Text != "")
            lbMove.Visible = true;
    }
    // =========================================================
    // START CLICK METHODS
    // =========================================================
    protected void btMove_Click(object sender, EventArgs e)
    {
        string sValid = "";
        sValid = ServerSideVal_Move();

        if (sValid == "VALID")
        {
            sendMoveEmail();
        }
    }
    // =========================================================
    protected void btSerialSearch_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iCs1ToUse = 0;
        int.TryParse(hfCs1.Value, out iCs1ToUse);
        string sSerial = txSerialSearch.Text.ToUpper().Trim();

        if (sPageLib == "L")
        {
            if (rdSel.Text == "Serial")
            {
                dataTable = wsLive.GetUnitBySerial(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
            else if (rdSel.Text == "CrossRef")
            {
                dataTable = wsLive.GetUnitByXref(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
            else if (rdSel.Text == "Asset")
            {
                dataTable = wsLive.GetUnitByAsset(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
         
        }
        else
        {
            if (rdSel.Text == "Serial")
            {
                dataTable = wsTest.GetUnitBySerial(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
            else if (rdSel.Text == "CrossRef")
            {
                dataTable = wsTest.GetUnitByXref(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
            else if (rdSel.Text == "Asset")
            {
                dataTable = wsTest.GetUnitByAsset(sfd.GetWsKey(), iCs1ToUse, sSerial);
            }
        }

        gvSerial.DataSource = dataTable;
        gvSerial.DataBind();

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetLocList(sfd.GetWsKey(), iCs1ToUse);
        }
        else
        {
            dataTable = wsTest.GetLocList(sfd.GetWsKey(), iCs1ToUse);
        }

        pnMove.Visible = true;
        ddLocMove.DataSource = dataTable;
        ddLocMove.DataTextField = "TextField";
        ddLocMove.DataValueField = "ValueField";
        ddLocMove.DataBind();
        ddLocMove.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
    }
    // =========================================================
    protected void lkSerialPick_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[4];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iCs1 = 0;
        int iCs2 = 0;
        int iUnt = 0;
        string sAgr = "";
        if (int.TryParse(saArg[0], out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(saArg[1], out iCs2) == false)
            iCs2 = 0;
        if (int.TryParse(saArg[2], out iUnt) == false)
            iUnt = 0;
        sAgr = saArg[3].Trim();

        if (iUnt > 0)
        {
            hfCs1.Value = iCs1.ToString();
            hfCs2.Value = iCs2.ToString();
            hfUnitList.Value = iUnt.ToString() + "~" + sAgr;

            Session["reqPri"] = hfPri.Value.Trim();
            Session["reqCs1"] = hfCs1.Value.Trim();
            Session["reqCs2"] = hfCs2.Value.Trim();
            Session["reqUnitList"] = hfUnitList.Value.Trim();
            if (rblPmRequest.SelectedValue == "PM")
                Session["reqListPm"] = "PM";
            else
                Session["reqListPm"] = "";
            Session["reqSource"] = "Serial";

            Response.Redirect("~/private/sc/req/Problem.aspx", false);
        }
    }
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    protected void sendMoveEmail()
    {
        char[] cSplitter1 = { '|' };
        char[] cSplitter2 = { '~' };

        string[] saCs1Cs2 = new string[2];
        int iCs1Target = 0;
        int iCs2Target = 0;
        string sTarget = "";
        string sContact = "";
        string sPhone = "";
        string sSubject = "";
        string sComment = "";
        string sCs1 = "";
        string sCs2 = "";
        string sUnt = "";
        string sAgr = "";
        string sMod = "";
        string sSer = "";

        string sResult = "";

        try
        {
            sTarget = ddLocMove.SelectedValue;
            if (sTarget != "")
            {
                saCs1Cs2 = sTarget.Split(cSplitter2);
                if (int.TryParse(saCs1Cs2[0], out iCs1Target) == false)
                    iCs1Target = 0;
                if (int.TryParse(saCs1Cs2[1], out iCs2Target) == false)
                    iCs2Target = 0;
            }

            sContact = txNameMove.Text.Trim();

            if ((txPhone1Move.Text != "") || (txPhone2Move.Text != "") || (txPhone3Move.Text != ""))
            {
                sPhone = "(" + txPhone1Move.Text.Trim() + ") " + txPhone2Move.Text.Trim() + "-" + txPhone3Move.Text.Trim();
                if (txExtMove.Text != "")
                    sPhone += "  Ext: " + txExtMove.Text.Trim();
            }

            // Build HTML Email Content
            sSubject = "Equipment Move Email";
            if (sContact != "")
                sSubject += " from " + sContact;

            sComment = "<html><head><title>" +
                sSubject +
            "</title>" +
            "<style>" +
            "body { font-family: verdana; font-size: 13px; margin-left: 30px; }" +
            "</style>" +
            "</head><body>";

            sComment += "<p><b>Contact</b><br />";
            if (sContact != "")
                sComment += sContact;
            else
                sComment += "No name given...";
            sComment += "</p>";

            if (sPhone != "")
                sComment += "<p><b>Phone</b><br />" + sPhone + "</p>";

            string sTargetName = "";
            if (sPageLib == "L")
            {
                sTargetName = wsLive.GetCustName(sfd.GetWsKey(), iCs1Target, iCs2Target);
            }
            else
            {
                sTargetName = wsTest.GetCustName(sfd.GetWsKey(), iCs1Target, iCs2Target);
            }

            sComment += "<p><b><font size='+0' color='#AD0034'>Please move the following units to " + sTargetName + ": " + iCs1Target.ToString() + "-" + iCs2Target.ToString() + "</font></b></p>";

            string sMoves = GetSelectedMoves_gv("READ");
            string[] saMoves = new string[1];
            saMoves = sMoves.Split(cSplitter1);

            string[] saCs1Cs2UntAgrMod = new string[6];
            for (int i = 0; i < saMoves.Length; i++)
            {
                saCs1Cs2UntAgrMod = saMoves[i].Split(cSplitter2);
                if (saCs1Cs2UntAgrMod.Length == 6)
                {
                    sCs1 = saCs1Cs2UntAgrMod[0].ToString().Trim();
                    sCs2 = saCs1Cs2UntAgrMod[1].ToString().Trim();
                    sUnt = saCs1Cs2UntAgrMod[2].ToString().Trim();
                    sAgr = saCs1Cs2UntAgrMod[3].ToString().Trim();
                    sMod = saCs1Cs2UntAgrMod[4].ToString().Trim();
                    sSer = saCs1Cs2UntAgrMod[5].ToString().Trim();

                    sComment += "<p>";
                    sComment += "<b>Model</b> " + sMod + "<br />";
                    sComment += "<b>Serial</b> " + sSer + "<br />";
                    sComment += "<b>Unit</b> " + sUnt + "<br />";
                    sComment += "<b>Agreement</b> " + sAgr + "<br />";
                    sComment += "<b>Current Location</b> " + sCs1 + "-" + sCs2;
                    sComment += "</p>";
                }
            }

            if (sPageLib == "L")
            {
                sResult = wsLive.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C05");
            }
            else
            {
                sResult = wsTest.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C05");
            }
            if (sResult == "SUCCESS")
            {
                lbMove.Text = "Email Successfully Sent For Move to " + sTargetName + ": " + iCs1Target.ToString() + "-" + iCs2Target.ToString();
                GetSelectedMoves_gv("CLEAR");
                ddLocMove.SelectedValue = "";
            }
            else
            {
                lbMove.Text = "There was a problem with the email: " + sResult;
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        { 
        }
        // ----------------
    }
    // =========================================================
    protected string ServerSideVal_Move()
    {
        string sResult = "";
        int iNum = 0;

        try
        {
            if (vCus_Move.IsValid == true)
            {
                if (ddLocMove.DataValueField == "")
                {
                    vCus_Move.ErrorMessage = "Please select a customer location for the move";
                    vCus_Move.IsValid = false;
                    ddLocMove.Focus();
                }
            }
            if (vCus_Move.IsValid == true)
            {
                if (txNameMove.Text == "")
                {
                    vCus_Move.ErrorMessage = "Please enter a contact name";
                    vCus_Move.IsValid = false;
                    txNameMove.Focus();
                }
                else
                {
                    if (txNameMove.Text.Length > 50)
                    {
                        vCus_Move.ErrorMessage = "The contact name must be 50 characters or less";
                        vCus_Move.IsValid = false;
                        txNameMove.Focus();
                    }
                }
            }
            if (vCus_Move.IsValid == true)
            {
                if (txPhone1Move.Text != "")
                {
                    if (int.TryParse(txPhone1Move.Text, out iNum) == false)
                    {
                        vCus_Move.ErrorMessage = "The area code must be a number";
                        vCus_Move.IsValid = false;
                        txPhone1Move.Focus();
                    }
                    else
                    {
                        if (txPhone1Move.Text.Length != 3)
                        {
                            vCus_Move.ErrorMessage = "The area code must be 3 digits";
                            vCus_Move.IsValid = false;
                            txPhone1Move.Focus();
                        }
                    }
                }
            }
            if (vCus_Move.IsValid == true)
            {
                if (txPhone2Move.Text != "")
                {
                    if (int.TryParse(txPhone2Move.Text, out iNum) == false)
                    {
                        vCus_Move.ErrorMessage = "The phone prefix must be a number";
                        vCus_Move.IsValid = false;
                        txPhone2Move.Focus();
                    }
                    else
                    {
                        if (txPhone2Move.Text.Length != 3)
                        {
                            vCus_Move.ErrorMessage = "The phone prefix must be 3 digits";
                            vCus_Move.IsValid = false;
                            txPhone2Move.Focus();
                        }
                    }
                }
            }
            if (vCus_Move.IsValid == true)
            {
                if (txPhone3Move.Text != "")
                {
                    if (int.TryParse(txPhone3Move.Text, out iNum) == false)
                    {
                        vCus_Move.ErrorMessage = "The phone suffix must be a number";
                        vCus_Move.IsValid = false;
                        txPhone3Move.Focus();
                    }
                    else
                    {
                        if (txPhone3Move.Text.Length != 4)
                        {
                            vCus_Move.ErrorMessage = "The phone suffix must be four digits";
                            vCus_Move.IsValid = false;
                            txPhone3Move.Focus();
                        }
                    }
                }
            }
            if (vCus_Move.IsValid == true)
            {
                if (txExtMove.Text != "")
                {
                    if (int.TryParse(txExtMove.Text, out iNum) == false)
                    {
                        vCus_Move.ErrorMessage = "The extension must be a number";
                        vCus_Move.IsValid = false;
                        txExtMove.Focus();
                    }
                    else
                    {
                        if (iNum > 99999999)
                        {
                            vCus_Move.ErrorMessage = "The extension may be no more than eight digits";
                            vCus_Move.IsValid = false;
                            txExtMove.Focus();
                        }
                    }
                }
            }

            if (vCus_Move.IsValid == true)
            {
                string sMoves = GetSelectedMoves_gv("READ");
                if (sMoves == "")
                {
                    vCus_Move.ErrorMessage = "Please select at least one unit to be moved";
                    vCus_Move.IsValid = false;
                }
            }

            // -------------------
            if (vCus_Move.IsValid == true)
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            vCus_Move.ErrorMessage = "A unexpected system error has occurred";
            vCus_Move.IsValid = false;
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
    protected string GetSelectedMoves_gv(string task)
    {
        // task is READ or CLEAR
        CheckBox chkBox = new CheckBox();
        string sType = "";
        string sMoves = "";

        foreach (Control c1 in gvSerial.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.ChildTable"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.GridViewRow"))
                    {
                        foreach (Control c3 in c2.Controls)
                        {
                            sType = c3.GetType().ToString();
                            if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
                            {
                                foreach (Control c4 in c3.Controls)
                                {
                                    sType = c4.GetType().ToString();
                                    if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                    {
                                        chkBox = (CheckBox)c4;
                                        if (chkBox.Checked == true)
                                        {
                                            if (task == "READ") 
                                            {
                                                if (sMoves != "")
                                                    sMoves += "|";
                                                sMoves += chkBox.Text;
                                            }
                                            else if (task == "CLEAR") 
                                            { 
                                                chkBox.Checked = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return sMoves;
    }
    // =========================================================
    // =========================================================
}

