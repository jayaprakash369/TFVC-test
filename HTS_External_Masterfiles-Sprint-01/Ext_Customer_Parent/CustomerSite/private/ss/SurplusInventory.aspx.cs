using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_ss_SurplusInventory : MyPage
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
    string sDownload = "";
    string sMethodName = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbError.Text = "";
        lbError.Visible = false;
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            getSsTechs(iCs1ToUse);
            getSsStockingLocations(iCs1ToUse);
            inzSsTechNum();
            inzSsStockLoc();

            if (ddStockLoc.Items.Count == 1)
                loadPanelSurplus();
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        if (ddStockLoc.Items.Count == 1)
            loadPanelSurplus();
    }
    // =========================================================
    protected void btSelections_Click(object sender, EventArgs e)
    {
        GetSelectedUnits_gv();
        if (hfUnitList.Value == "")
        {
            lbError.Text = "Please select at least one unit for your email.";
            lbError.Visible = true;
        }
        else 
        {
            sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            dataTable = new DataTable(sMethodName);

            pnCs1Change.Visible = false;
            pnInput.Visible = false;
            pnSurplus.Visible = false;
            pnEmail.Visible = true;
            char[] cSplitter1 = { '|' };
            char[] cSplitter2 = { '~' };
            string[] saParms = new string[1];
            string[] saPrtQty = new string[1];

            saParms = hfUnitList.Value.Split(cSplitter1);
            string sPrt = "";
            int iQty = 0;

            DataColumn dc;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Part";
            dataTable.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Qty";
            dataTable.Columns.Add(dc);
            
            dataTable.AcceptChanges();

            int iRowIdx = 0;
            for (int i = 0; i < saParms.Length; i++) 
            {
                saPrtQty = saParms[i].Split(cSplitter2);
                sPrt = saPrtQty[0].Trim();
                if (int.TryParse(saPrtQty[1].Trim(), out iQty) == false)
                    iQty = 0;

                // ----------------------
                // New Row
                // ----------------------
                //public DataRow dr = new DataRow();
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.Add(dr);

                dataTable.Rows[iRowIdx]["Part"] = sPrt;
                dataTable.Rows[iRowIdx]["Qty"] = iQty.ToString();
               
                iRowIdx++;
            }
            // -------------------------------------------------
            dataTable.AcceptChanges();
        }
        rpEmail.DataSource = dataTable;
        rpEmail.DataBind();
    }
    // =========================================================
    protected void GetSelectedUnits_gv()
    {
        CheckBox chkBox = new CheckBox();
        string sType = "";
        hfUnitList.Value = "";

        foreach (Control c1 in gvSurplus.Controls)
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
                                            if (hfUnitList.Value != "")
                                                hfUnitList.Value += "|";
                                            hfUnitList.Value += chkBox.Text;
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
    // =========================================================
    protected void btEmail_Click(object sender, EventArgs e)
    {
        string sEmailLines = GetSurplusComments_rp();
        sendEmail(sEmailLines);
    }
    // =========================================================
    protected string GetSurplusComments_rp()
    {
        string sEmailLines = "";
        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        string sType = "";
        string sPrt = "";
        int iQty = 0;
        string sComment = "";

        foreach (Control c1 in rpEmail.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPart")
                            sPrt = lbTemp.Text.Trim();
                        if (lbTemp.ID == "lbQty") 
                        {
                            if (int.TryParse(lbTemp.Text.Trim(), out iQty) == false)
                                iQty = 0;
                        }
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;
                        if (txTemp.ID == "txComment") 
                        {
                            sComment = txTemp.Text.Trim();
                            // Load values into email...
                            sEmailLines += "<p><b>Part: " + sPrt + "</b> Qty: " + iQty.ToString()  + "<br />Comment: " +
                                HttpUtility.HtmlEncode(sComment) + "</p>";
                            // Initialize in loop
                            sPrt = "";
                            iQty = 0;
                            sComment = "";
                        }
                    }
                }
            }
        }
        // ----------------------
        return sEmailLines;
    }
    // =========================================================
    protected void sendEmail(string emailLines)
    {
        int iCs1 = 0;
        int iStockLoc = 0;
        string sCs1Name = "";
        string sUserName = "";
        string sEmailReply = "";
        string sSubject = "";
        string sComment = "";
        string sResult = "";

        if (int.TryParse(ddStockLoc.SelectedValue.ToString(), out iStockLoc) == false)
            iStockLoc = 0;

        try
        {
            if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1) == false)
                iCs1 = 0;

            if (sPageLib == "L")
            {
                sCs1Name = wsLive.GetCustName(sfd.GetWsKey(), iCs1, 0);
            }
            else
            {
                sCs1Name = wsTest.GetCustName(sfd.GetWsKey(), iCs1, 0);
            }

            if (User.Identity.IsAuthenticated)
                sUserName = User.Identity.Name.ToString();

            sEmailReply = txEmailReply.Text.Trim();

            // Build HTML Email Content
            sSubject = "Self-Service Surplus Email";
            if (iStockLoc > 0)
                sSubject += ": Stocking Location " + iStockLoc.ToString();

            sComment = "<html><head><title>" +
                HttpUtility.HtmlEncode(sSubject) +
            "</title>" +
            "<style>" +
            "body { font-family: verdana; font-size: 13px; margin-left: 30px; }" +
            "</style>" +
            "</head><body>";

            if (sUserName != "")
            {
                sComment += "<p><b>Logged On User</b><br />" + sUserName + "</p>";
            }

            if (sUserName != "")
            {
                sComment += "<p><b>Parent Customer</b><br />" + iCs1.ToString() + "  " + sCs1Name + "</p>";
            }

            sComment += emailLines;

            if (sEmailReply != "")
            {
                sComment += "<p><b>Email Reply Requested</b><br />";
                sComment += HttpUtility.HtmlEncode(sEmailReply);
            }

            if (sPageLib == "L")
            {
                sResult = wsLive.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C04");
            }
            else
            {
                sResult = wsTest.EmailBasic(sfd.GetWsKey(), sSubject, sComment, "HTML", "C04");
            }
            if (sResult == "SUCCESS")
            {
                lbError.Text = "Email Successfully Sent";
                pnCs1Change.Visible = true;
                pnInput.Visible = true;
                pnSurplus.Visible = true;
                pnEmail.Visible = false;
                loadPanelSurplus();
            }
            else
            {
                lbError.Text = "There was a problem with the email: " + sResult;
            }
            lbError.Visible = true;
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        { 
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
    protected void btInput_Click(object sender, EventArgs e)
    {
        loadPanelSurplus();
    }
    // =========================================================
    protected void btDownload_Click(object sender, EventArgs e)
    {
        sDownload = "Y";
        loadPanelSurplus();
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
    protected void ddTechNum_Changed(object sender, EventArgs e)
    {
        Session["ssTechNum"] = ddTechNum.SelectedValue.ToString();
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
    protected void ddStockLoc_Changed(object sender, EventArgs e)
    {
        Session["ssStockLoc"] = ddStockLoc.SelectedValue.ToString();
        loadPanelSurplus();
    }
    // =========================================================
    protected void loadPanelSurplus()
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        int iStockLoc = 0;
        if (int.TryParse(ddStockLoc.SelectedValue.ToString(), out iStockLoc) == false)
            iStockLoc = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsSurplusInventory(sfd.GetWsKey(), iStockLoc);
        }
        else
        {
            dataTable = wsTestSs.GetSsSurplusInventory(sfd.GetWsKey(), iStockLoc);
        }
        gvSurplus.DataSource = dataTable;
        gvSurplus.DataBind();
        if (sDownload == "Y") 
        {
            DownloadHandler dh = new DownloadHandler();
            string sCsv = dh.DataTableToExcelCsv(dataTable);
            dh = null;

            if (dataTable.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=SurplInv_Loc_" + iStockLoc.ToString() + ".csv");
                Response.Write(sCsv);
                Response.End();
            }
        }
    }
    // =========================================================
    // =========================================================
}