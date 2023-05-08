using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_ss_PartsUsed : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SelfService_LIVE.SelfServiceMenuSoapClient wsLiveSs = new SelfService_LIVE.SelfServiceMenuSoapClient();
    SelfService_DEV.SelfServiceMenuSoapClient wsTestSs = new SelfService_DEV.SelfServiceMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    SourceForCustomer sfc = new SourceForCustomer();

    DataTable dataTable;

    string sCs1Changed = "";
    string sMethodName = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields
        vSum_StockSearch.Visible = false;

        if (!IsPostBack)
        {
            getLatestTicket(iCs1ToUse);
            getSsTechs(iCs1ToUse);
            inzSsTechNum();

            // Load Qty Used
            for (int i = 0; i <= 25; i++)
            {
                ddUsedQty.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            ddUsedQty.Items[1].Selected = true;
            getPartsUsedToday();
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        if (ddStockLoc.Items.Count == 1)
            loadPanelContractUnits();
    }
    // =========================================================
    protected void getLatestTicket(int cs1)
    {
        int[] iaCtrTck = { 0, 0 };

        if (sPageLib == "L")
        {
            iaCtrTck = wsLiveSs.GetSsTicket(sfd.GetWsKey(), cs1);
        }
        else
        {
            iaCtrTck = wsTestSs.GetSsTicket(sfd.GetWsKey(), cs1);
        }
        if (iaCtrTck.Length == 2)
        {
            hfCtr.Value = iaCtrTck[0].ToString();
            hfTck.Value = iaCtrTck[1].ToString();
            lbCtrTck.Text = hfCtr.Value + "-" + hfTck.Value;
        }
    }
    // =========================================================
    protected void getPartsUsedToday()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        int iCtr = 0;
        int iTck = 0;

        if (int.TryParse(hfCtr.Value, out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(hfTck.Value, out iTck) == false)
            iTck = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetPartsUsedToday(sfd.GetWsKey(), iCtr, iTck);
        }
        else
        {
            dataTable = wsTestSs.GetPartsUsedToday(sfd.GetWsKey(), iCtr, iTck);
        }
        if (dataTable.Rows.Count > 0)
        {
            gvPartsProcessed.DataSource = dataTable;
            gvPartsProcessed.DataBind();
            gvPartsProcessed.Visible = true;
        }
    }
    // =========================================================
    protected void getSsTechs(int cs1)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsTechs(sfd.GetWsKey(), cs1);
        }
        else
        {
            dataTable = wsTestSs.GetSsTechs(sfd.GetWsKey(), cs1);
        }
        if (dataTable.Rows.Count > 0) 
        {
            ddTechNum.DataSource = dataTable;
            ddTechNum.DataValueField = "TechNum";
            ddTechNum.DataTextField = "TechNam";
            ddTechNum.DataBind();
        }
    }
    // =========================================================
    protected void inzSsTechNum()
    {
        int iSsTechNum = 0;
        if (Session["ssTechNum"] != null)
        {
            if (int.TryParse(Session["ssTechNum"].ToString(), out iSsTechNum) == false)
                iSsTechNum = 0;
            else
            {
                if (iSsTechNum > 0)
                {
                    ddTechNum.SelectedValue = iSsTechNum.ToString();
                }
            }
        }
        else
        {
            Session["ssTechNum"] = ddTechNum.SelectedValue.ToString();
        }
    }
    // =========================================================
    protected void getSsStockingLocations(int cs1)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsStockLocs(sfd.GetWsKey(), cs1);
        }
        else
        {
            dataTable = wsTestSs.GetSsStockLocs(sfd.GetWsKey(), cs1);
        }
        if (dataTable.Rows.Count > 0)
        {
            ddStockLoc.DataSource = dataTable;
            ddStockLoc.DataValueField = "StockLocNum";
            ddStockLoc.DataTextField = "StockLocDisplay";
            ddStockLoc.DataBind();
        }
    }
    // =========================================================
    protected void inzSsStockLoc()
    {
        int iSsStockLoc = 0;
        if (Session["ssStockLoc"] != null)
        {
            if (int.TryParse(Session["ssStockLoc"].ToString(), out iSsStockLoc) == false)
                iSsStockLoc = 0;
            else
            {
                if (iSsStockLoc > 0)
                {
                    ddStockLoc.SelectedValue = iSsStockLoc.ToString();
                }
            }
        }
        else
        {
            Session["ssStockLoc"] = ddStockLoc.SelectedValue.ToString();
        }
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        CheckCs1Changed();

        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iPrimaryCs1 = iCs1User;
        int iCs1Change = 0;

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (tbCs1Change.Visible == false)
                    tbCs1Change.Visible = true;

                if (txCs1Change.Text != "")
                {
                    if (int.TryParse(txCs1Change.Text, out iCs1Change) == false)
                        iCs1Change = 0;
                    else
                    {
                        if (iCs1Change > 0)
                        {
                            Session["adminCs1"] = txCs1Change.Text;
                            iPrimaryCs1 = iCs1Change;
                            if (sCs1Changed == "YES")
                            {
                                Session["ssTechNum"] = null;
                                Session["ssStockLoc"] = null;
                                getSsTechs(iCs1Change);
                                getSsStockingLocations(iCs1Change);
                                string sGoToMenu = sfd.checkGoToMenu("SspSsb", iPrimaryCs1);
                                if (sGoToMenu == "GO")
                                    Response.Redirect("~/private/shared/Menu.aspx", false);
                            }
                        }
                    }
                }
                else
                {
                    if (Session["adminCs1"] != null)
                    {
                        if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Change) == false)
                            iCs1Change = 0;
                        else
                        {
                            if (iCs1Change > 0)
                            {
                                txCs1Change.Text = iCs1Change.ToString();
                                iPrimaryCs1 = iCs1Change;
                            }
                        }
                    }
                    else
                    {
                        txCs1Change.Text = iCs1User.ToString();
                        Session["adminCs1"] = txCs1Change.Text;
                    }
                }
            }
        }

        return iPrimaryCs1;
    }
    // =========================================================
    protected string CheckCs1Changed()
    {
        sCs1Changed = "NO";

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (Session["adminCs1"] != null)
                {
                    // Admin changed cust but did not click change button
                    int iCs1Session = 0;
                    int iCs1Textbox = 0;
                    if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Session) == false)
                        iCs1Session = 0;
                    if (int.TryParse(txCs1Change.Text, out iCs1Textbox) == false)
                        iCs1Textbox = 0;

                    if ((iCs1Session != iCs1Textbox) && (txCs1Change.Text != ""))
                        sCs1Changed = "YES";
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    protected void btContractSearch_Click(object sender, EventArgs e)
    {
        loadPanelContractUnits();
    }
    // =========================================================
    protected void ddTechNum_Changed(object sender, EventArgs e)
    {
        Session["ssTechNum"] = ddTechNum.SelectedValue.ToString();
        // loadPanelContract();
    }
    // =========================================================
    protected void ddStockLoc_Changed(object sender, EventArgs e)
    {
        Session["ssStockLoc"] = ddStockLoc.SelectedValue.ToString();
        loadPanelContractUnits();
    }
    // =========================================================
    protected void loadPanelContractUnits()
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        string sMod = txMod.Text.Trim();
        string sDsc = txDsc.Text.Trim();
        string sSer = txSer.Text.Trim();

        int iStockLoc = 0;
        if (int.TryParse(ddStockLoc.SelectedValue.ToString(), out iStockLoc) == false)
            iStockLoc = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsContractUnits(sfd.GetWsKey(), iPrimaryCs1, sMod, sDsc, sSer);
        }
        else
        {
            dataTable = wsTestSs.GetSsContractUnits(sfd.GetWsKey(), iPrimaryCs1, sMod, sDsc, sSer);
        }
        gvContractUnits.DataSource = dataTable;
        gvContractUnits.DataBind();
    }
    // =========================================================
    protected void btEmp_Click(object sender, EventArgs e)
    {
        loadPanelContract();
    }
    // =========================================================
    protected void lkSerial_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        lbPickMod.Text = saArg[0];
        lbPickSer.Text = saArg[1];

        int iUnit = 0;
        if (int.TryParse(saArg[2].ToString().Trim(), out iUnit) == false)
            iUnit = 0;
        hfPickUnit.Value = iUnit.ToString();

        loadGvPartsList();

        lbPickMod.Visible = true;
        lbPickModHead.Visible = true;
        lbPickSer.Visible = true;
        lbPickSerHead.Visible = true;

        pnCs1Change.Visible = false;
        pnContractSearch.Visible = false;
        pnContractUnits.Visible = false;
        pnReplacementParts.Visible = true;
        
        gvPartsList.Visible = true;
        pnPartUseEntry.Visible = false;
    }
    // =========================================================
    protected void btStockSearch_Click(object sender, EventArgs e)
    {
        if (rbLetMeType.Checked == true) 
        {
            txUsedPart.Text = "";
            lbUsedPart.Text = "";
            lbUsedPart.Visible = false;
            txUsedPart.Visible = true;
            txUsedLoc.Text = hfEmpLoc.Value;
            lbUsedLoc.Text = "";
            lbUsedLoc.Visible = false;
            txUsedLoc.Visible = true;
            ddUsedQty.SelectedValue = "1";
            lbUsedReturnable.Text = "NO";

            foreach (TableRow tbRow in tbPartUseEntry.Rows)
            {
                tbRow.Cells[4].Visible = false;
                tbRow.Cells[5].Visible = false;
            }
            txUsedPart.Focus();

            gvPartsList.Visible = false;
            pnPartUseEntry.Visible = true;
        }
        else 
        {
            loadGvPartsList();

            gvPartsList.Visible = true;
            pnPartUseEntry.Visible = false;
        }
    }
    // =========================================================
    protected void loadGvPartsList()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        int iStockLoc = 0;
        if (rbLocEmp.Checked == true)
        {
            if (int.TryParse(hfEmpLoc.Value, out iStockLoc) == false)
                iStockLoc = 0;
        }
        else if (rbLocCs1.Checked == true)
        {
            if (int.TryParse(ddStockLoc.SelectedValue, out iStockLoc) == false)
                iStockLoc = 0;
        }
        else if (rbLocPrt.Checked == true)
        {
        }
        if (iStockLoc > 0)
        {
            if (sPageLib == "L")
            {
                dataTable = wsLiveSs.GetSsStockLocParts(sfd.GetWsKey(), iStockLoc);
            }
            else
            {
                dataTable = wsTestSs.GetSsStockLocParts(sfd.GetWsKey(), iStockLoc);
            }
        }
        else
        {
            string sPrt = txStockPart.Text;

            if ((sPrt == "") || (sPrt.Length < 2))
            {
                vCus_Part.ErrorMessage = "The part entry must be at least two characters.";
                vCus_Part.IsValid = false;
                vSum_StockSearch.Visible = true;
                txStockPart.Focus();
            }
            else
            {
                if (sPageLib == "L")
                {
                    dataTable = wsLiveSs.GetSsProdmstParts(sfd.GetWsKey(), sPrt);
                }
                else
                {
                    dataTable = wsTestSs.GetSsProdmstParts(sfd.GetWsKey(), sPrt);
                }
            }
        }
        gvPartsList.DataSource = dataTable;
        gvPartsList.DataBind();
        gvPartsList.Visible = true;
    }
    // =========================================================
    protected void loadPanelContract()
    {
        int iEmpNum = 0;
        if (int.TryParse(ddTechNum.SelectedValue.ToString(), out iEmpNum) == false)
            iEmpNum = 0;

        if (sPageLib == "L")
        {
            hfEmpNam.Value = wsLiveSs.GetEmpName(sfd.GetWsKey(), iEmpNum);
        }
        else
        {
            hfEmpNam.Value = wsTestSs.GetEmpName(sfd.GetWsKey(), iEmpNum);
        }

        if (sPageLib == "L")
        {
            hfEmpLoc.Value = wsLiveSs.GetEmpStockLoc(sfd.GetWsKey(), iEmpNum).ToString().Trim();
        }
        else
        {
            hfEmpLoc.Value = wsTestSs.GetEmpStockLoc(sfd.GetWsKey(), iEmpNum).ToString().Trim();
        }

        rbLocEmp.Text = "Show my stock loc " + hfEmpLoc.Value;
        //lbPickEmp.Text =  iEmpNum.ToString() + " " + hfEmpNam.Value;
        lbPickEmp.Text = hfEmpNam.Value;

        lbPickEmp.Visible = true;
        lbPickEmpHead.Visible = true;

        int iCs1ToUse = GetPrimaryCs1();
        getSsStockingLocations(iCs1ToUse);
        inzSsStockLoc();
        loadPanelContractUnits();

        pnCs1Change.Visible = false;
        pnEmp.Visible = false;
        pnContractSearch.Visible = true;
        pnContractUnits.Visible = true;
    }
    // =========================================================
    protected void btAddPart_Click(object sender, EventArgs e)
    {
        Button buttonControl = (Button)sender;
        string sParms = buttonControl.CommandArgument.ToString();
        string[] saArg = new string[4];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iStk = 0;
        int iQoh = 0;

        string sPrt = saArg[0].Trim();
        if (int.TryParse(saArg[1].ToString().Trim(), out iStk) == false)
            iStk = 0;
        if (int.TryParse(saArg[2].ToString().Trim(), out iQoh) == false)
            iQoh = 0;
        hfLocQty.Value = iQoh.ToString();
        string sRtn = saArg[3];

        txUsedPart.Text = sPrt;
        lbUsedPart.Text = sPrt;
        txUsedPart.Visible = false;
        lbUsedPart.Visible = true;
        txUsedLoc.Text = iStk.ToString();
        lbUsedLoc.Text = iStk.ToString();
        if (rbLocPrt.Checked == true)
        {
            txUsedLoc.Visible = true;
            lbUsedLoc.Visible = false;
        }
        else 
        {
            txUsedLoc.Visible = false;
            lbUsedLoc.Visible = true;
        }
        ddUsedQty.SelectedValue = "1";

        if (sRtn == "Y")
        {
            lbUsedReturnable.Text = "YES";
            foreach (TableRow tbRow in tbPartUseEntry.Rows)
            {
                tbRow.Cells[4].Visible = true;
                tbRow.Cells[5].Visible = true;
            }
        }
        else
        {
            lbUsedReturnable.Text = "NO";
            foreach (TableRow tbRow in tbPartUseEntry.Rows)
            {
                tbRow.Cells[4].Visible = false;
                tbRow.Cells[5].Visible = false;
            }
        }
        btUsedSubmission.CommandArgument = sPrt + "|" + iStk.ToString();

        pnPartUseEntry.Visible = true;
        gvPartsList.Visible = false;
    }
    // =========================================================
    protected void btUsedSubmission_Click(object sender, EventArgs e)
    {
        string sReturn = "";

        validatePartUse();

        if (vCus_PartUseEntry.ErrorMessage != "")
        {
            vCus_PartUseEntry.IsValid = false;
        }
        else
        // all is well -- process part
        {
            int iCtr = 0;
            int iTck = 0;
            int iEmp = 0;
            int iUnt = 0;
            int iQty = 0;
            int iLoc = 0;
            string sPrt = txUsedPart.Text.Trim();
            string sPgm = "private/ss/PartsUsed.aspx";
            string sRtn = lbUsedReturnable.Text;
            if (sRtn == "YES")
                sRtn = "Y";
            else
                sRtn = "N";
            string sSnd = ddUsedReturning.Text;
            if (sSnd == "")
                sSnd = "N";
            string sRsn = ddUsedReasonNotReturning.SelectedValue;
            if (sRsn == "Other")
                sRsn = txUsedTextNotReturning.Text;

            if (int.TryParse(hfCtr.Value, out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(hfTck.Value, out iTck) == false)
                iTck = 0;
            if (int.TryParse(ddTechNum.SelectedValue, out iEmp) == false)
                iEmp = 0;
            if (int.TryParse(hfPickUnit.Value, out iUnt) == false)
                iUnt = 0;
            if (int.TryParse(ddUsedQty.SelectedValue, out iQty) == false)
                iQty = 0;
            if (int.TryParse(txUsedLoc.Text, out iLoc) == false)
                iLoc = 0;

            if (sPageLib == "L")
            {
                sReturn = wsLiveSs.TriggerPartUse(sfd.GetWsKey(), iCtr, iTck, iEmp, iUnt, iQty, iLoc, sPgm, sPrt, sRtn, sSnd, sRsn, "");
            }
            else
            {
                sReturn = wsTestSs.TriggerPartUse(sfd.GetWsKey(), iCtr, iTck, iEmp, iUnt, iQty, iLoc, sPgm, sPrt, sRtn, sSnd, sRsn, "Y");
            }

            getPartsUsedToday();

            // initialize entry fields
            txUsedPart.Text = "";
            txUsedLoc.Text = "";
            lbUsedReturnable.Text = "";
            ddUsedQty.SelectedIndex = 0;
            ddUsedReturning.SelectedValue = "";
            ddUsedReasonNotReturning.SelectedValue = "";
            txUsedTextNotReturning.Text = "";
            rbLocCs1.Checked = true;

            loadGvPartsList();
            gvPartsList.Visible = true;
            pnPartUseEntry.Visible = false;

        }
    }
    // =========================================================
    protected void validatePartUse()
    {
        vCus_PartUseEntry.ErrorMessage = "";
        string[] sAryUnkStySncBilIns = new string[5]; // unknown, style, serial num controlled, billable, install
        txUsedPart.Text = txUsedPart.Text.Trim().ToUpper();
        string sPrt = txUsedPart.Text.Trim();
        string sValidLoc = "";

        int iLoc = 0;
        if (int.TryParse(txUsedLoc.Text, out iLoc) == false)
            iLoc = 0;

        if (sPageLib == "L")
        {
            sAryUnkStySncBilIns = wsLiveSs.ValidatePartUsed(sfd.GetWsKey(), sPrt);
            sValidLoc = wsLiveSs.ValidateStockLoc(sfd.GetWsKey(), iLoc);
        }
        else
        {
            sAryUnkStySncBilIns = wsTestSs.ValidatePartUsed(sfd.GetWsKey(), sPrt);
            sValidLoc = wsTestSs.ValidateStockLoc(sfd.GetWsKey(), iLoc);
        }

        if ((lbUsedReturnable.Text == "YES") && (ddUsedReturning.SelectedValue != "YES"))
        {
            if (ddUsedReturning.SelectedValue == "")
            {
                vCus_PartUseEntry.ErrorMessage = "Please indicate whether the part is being returned";
                ddUsedReturning.Focus();
            }
            else if (ddUsedReasonNotReturning.SelectedValue == "")
            {
                vCus_PartUseEntry.ErrorMessage = "A reason must be selected for not returning a returnable part";
                ddUsedReasonNotReturning.Focus();
            }
            else if ((ddUsedReasonNotReturning.SelectedValue == "Other") && (txUsedTextNotReturning.Text == ""))
            {
                vCus_PartUseEntry.ErrorMessage = "Please enter the other reason...";
                txUsedTextNotReturning.Focus();
            }
        }
        if (sAryUnkStySncBilIns[0] == "Unknown Part")
        {
            vCus_PartUseEntry.ErrorMessage = "This part number is invalid or unknown";
            txUsedPart.Focus();
        }
        else if (sAryUnkStySncBilIns[1] == "Equipment Style")
        {
            vCus_PartUseEntry.ErrorMessage = "This is an equipment style rather than a specific part number";
            txUsedPart.Focus();
        }
        else if (sAryUnkStySncBilIns[2] == "Serial Controlled")
        {
            vCus_PartUseEntry.ErrorMessage = "This is a serial number controlled part. Please process this part use through our call center";
            txUsedPart.Focus();
        }
        else if (sAryUnkStySncBilIns[3] == "Install Part")
        {
            vCus_PartUseEntry.ErrorMessage = "This is an install part. Please process this part use through our call center";
            txUsedPart.Focus();
        }
//        else if (sAryUnkStySncBilIns[4] == "Billable Part")
//        {
//            vCus_PartUseEntry.ErrorMessage = "This part is often billable. Please process this part use through our call center";
//            txUsedPart.Focus();
//        }
        if (txUsedLoc.Visible == true) 
        {
            if (txUsedLoc.Text == "")
            {
                vCus_PartUseEntry.ErrorMessage = "A source stocking location is required.";
                txUsedLoc.Focus();
            }
            else if (sValidLoc != "Y")
            {
                vCus_PartUseEntry.ErrorMessage = "This is not a valid stocking location";
                txUsedLoc.Focus();
            }
        }
        if (txUsedPart.Visible == true)
        {
            string sReturnable = "";
            if (sPageLib == "L")
            {
                sReturnable = wsLiveSs.GetPartReturnStatus(sfd.GetWsKey(), sPrt);
            }
            else
            {
                sReturnable = wsTestSs.GetPartReturnStatus(sfd.GetWsKey(), sPrt);
            }
            if ((lbUsedReturnable.Text == "NO") && (sReturnable == "Y"))
            {
                lbUsedReturnable.Text = "YES";
                vCus_PartUseEntry.ErrorMessage = "Please indicate whether the part will be returned.";
                ddUsedReturning.Focus();
                foreach (TableRow tbRow in tbPartUseEntry.Rows)
                {
                    tbRow.Cells[4].Visible = true;
                    tbRow.Cells[5].Visible = true;
                }
            }
            if ((lbUsedReturnable.Text == "YES") && (sReturnable == "N"))
            {
                lbUsedReturnable.Text = "NO";
                ddUsedReturning.SelectedValue = "NO";
                foreach (TableRow tbRow in tbPartUseEntry.Rows)
                {
                    tbRow.Cells[4].Visible = false;
                    tbRow.Cells[5].Visible = false;
                }
            }
        }
    }
    // =========================================================
    // =========================================================
}