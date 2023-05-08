using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Web.Security;

public partial class private_ss_AutoReplenish : MyPage
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
        lbError.Visible = false;
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            getSsTechs(iCs1ToUse);
            inzSsTechNum();
            getSsStockingLocations(iCs1ToUse);
            inzSsStockLoc();
            loadNewLineCounter();

            if (ddStockLoc.Items.Count == 1)
                loadPanelAutoReplenish();
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        if (ddStockLoc.Items.Count == 1)
            loadPanelAutoReplenish();
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
    protected void loadNewLineCounter()
    {
        int iQty = 0;
        for (int i = 0; i < 20; i++)
        {
            iQty++;
            ddNewLines.Items.Insert(i, new System.Web.UI.WebControls.ListItem(iQty.ToString(), iQty.ToString())); 
        }
        ddNewLines.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // "text", "value"
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
        loadPanelAutoReplenish();
    }
    // =========================================================
    protected void loadPanelAutoReplenish()
    {
        int iPrimaryCs1 = GetPrimaryCs1();
//        int iTech = 0;
        int iStockLoc = 0;
//        if (int.TryParse(ddTechNum.SelectedValue.ToString(), out iTech) == false)
//            iTech = 0;
        if (int.TryParse(ddStockLoc.SelectedValue.ToString(), out iStockLoc) == false)
            iStockLoc = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsAutoReplenish(sfd.GetWsKey(), iStockLoc);
        }
        else
        {
            dataTable = wsTestSs.GetSsAutoReplenish(sfd.GetWsKey(), iStockLoc);
        }
        gvAutoReplenish.DataSource = dataTable;
        gvAutoReplenish.DataBind();
        if (sDownload == "Y") 
        {
            DownloadHandler dh = new DownloadHandler();
            string sCsv = dh.DataTableToExcelCsv(dataTable);
            dh = null;

            Response.ClearContent();
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=AutoReplen_Loc_" + iStockLoc.ToString() + ".csv");
            Response.Write(sCsv);
            Response.End();
        }
        
    }
    // =========================================================
    protected DataTable GetSelectedLines_gv()
    {
        dataTable = new DataTable();
        DataRow dr;
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        CheckBox chkBox = new CheckBox();
        string sType = "";
        string sParms = "";
        string[] saArg = new string[5];
        char[] cSplitter = { '|' };
        string sPrd = "";
        string sMod = "";
        string sDsc = "";
        int iReorderPoint = 0;
        int iReorderQty = 0;

        dataTable.Columns.Add(makeColumn("Product Code"));
        dataTable.Columns.Add(makeColumn("Model"));
        dataTable.Columns.Add(makeColumn("Description"));
        dataTable.Columns.Add(makeColumn("Reorder Point"));
        dataTable.Columns.Add(makeColumn("Reorder Qty"));

        dataTable.AcceptChanges();

        foreach (Control c1 in gvAutoReplenish.Controls)
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
                                            // ---------------------------
                                            sParms = chkBox.Text;
                                            saArg = sParms.Split(cSplitter);

                                            sPrd = saArg[0].Trim();
                                            sMod = saArg[1].Trim();
                                            sDsc = saArg[2].Trim();
                                            if (int.TryParse(saArg[3], out iReorderPoint) == false)
                                                iReorderPoint = 0;
                                            if (int.TryParse(saArg[4], out iReorderQty) == false)
                                                iReorderQty = 0;

                                            dr = dataTable.NewRow();
                                            dr["Product Code"] = sPrd;
                                            dr["Model"] = sMod;
                                            dr["Description"] = sDsc;
                                            dr["Reorder Point"] = iReorderPoint.ToString();
                                            dr["Reorder Qty"] = iReorderQty.ToString();
                                            dataTable.Rows.Add(dr); 
                                            // ---------------------------
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        dataTable.AcceptChanges();
        return dataTable;
    }
    // =========================================================
    protected void SetOldLines_gv()
    {
        DropDownList dd = new DropDownList();
        HiddenField hf = new HiddenField();
        string sType = "";
        int iReorderPoint = 0;
        int iReorderQty = 0;
        string sResult = "";

        try
        {
            foreach (Control c1 in gvPartsOld.Controls)
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
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                                        {
                                            hf = (HiddenField)c4;
                                            if (hf.ID == "hfReorderPoint")
                                            {
                                                if (int.TryParse(hf.Value, out iReorderPoint) == false)
                                                    iReorderPoint = 0;
                                            }
                                            if (hf.ID == "hfReorderQty")
                                            {
                                                if (int.TryParse(hf.Value, out iReorderQty) == false)
                                                    iReorderQty = 0;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                                        {
                                            dd = (DropDownList)c4;
                                            if (dd.ID == "ddReorderPointOld")
                                                dd.SelectedValue = iReorderPoint.ToString();
                                            if (dd.ID == "ddReorderQtyOld")
                                                dd.SelectedValue = iReorderQty.ToString();
                                        }
                                        // -------------------------------------
                                    }
                                }
                            }
                        }
                    }
                }
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
    }
    // =========================================================
    protected string GetNewLines_gv()
    {
        string sRtn = "";

        TextBox tx = new TextBox();
        DropDownList dd = new DropDownList();
        string sType = "";
        
        string sPrd = "";
        string sMod = "";
        string sDsc = "";
        int iReorderPoint = 0;
        int iReorderQty = 0;
        string sHeaderDone = "";

        try
        {
            foreach (Control c1 in gvPartsNew.Controls)
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
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                                        {
                                            tx = (TextBox)c4;
                                            if (tx.ID == "txModNew")
                                                sMod = tx.Text;
                                            if (tx.ID == "txDscNew")
                                                sDsc = tx.Text;
                                        }
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                                        {
                                            dd = (DropDownList)c4;
                                            if (dd.ID == "ddPrdNew")
                                            {
                                                sPrd = dd.SelectedValue;
                                            }
                                            if (dd.ID == "ddReorderPointNew")
                                            {
                                                if (int.TryParse(dd.SelectedValue.Trim(), out iReorderPoint) == false)
                                                    iReorderPoint = 0;
                                            }
                                            if (dd.ID == "ddReorderQtyNew")
                                            {
                                                if (int.TryParse(dd.SelectedValue.Trim(), out iReorderQty) == false)
                                                    iReorderQty = 0;

                                                // ---------------------------
                                                // Build Table Header (if first pass) 
                                                // ---------------------------
                                                if (sHeaderDone == "")
                                                {
                                                    sHeaderDone = "YES";
                                                    sRtn = "<p><b><font color='#AD0034'>Additions</font> to Auto Replenish List</b><br />";
                                                    sRtn += "<table><tr>" +
                                                        "<th>Product Code</th>" +
                                                        "<th>Model</th>" +
                                                        "<th>Description</th>" +
                                                        "<th>Reorder Point</th>" +
                                                        "<th>Reorder Qty</th>" +
                                                        "</tr>";
                                                }

                                                // ---------------------------
                                                // Add line to email
                                                // ---------------------------
                                                sRtn += "<tr>" +
                                                    "<td>" + sPrd + "&nbsp;</td>" +
                                                    "<td>" + sMod + "&nbsp;</td>" +
                                                    "<td>" + sDsc + "&nbsp;</td>" +
                                                    "<td>" + iReorderPoint.ToString() + "&nbsp;</td>" +
                                                    "<td>" + iReorderQty.ToString() + "&nbsp;</td>" +
                                                    "</tr>";

                                                // ---------------------------
                                                // Initialize Hold Fields
                                                // ---------------------------
                                                sPrd = "";
                                                sMod = "";
                                                sDsc = "";
                                                iReorderPoint = 0;
                                                iReorderQty = 0;
                                                // ---------------------------
                                            }
                                        }
                                        // ==============================
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (sHeaderDone == "YES")
                sRtn += "</table><br /><br />";
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            sRtn = "An error occurred retrieving additions to the auto replenish list";
        }
        finally
        { 
        }

        return sRtn;
    }
    // =========================================================
    protected string GetOldLines_gv()
    {
        string sRtn = "";

        Label lb = new Label();
        HiddenField hf = new HiddenField();
        DropDownList dd = new DropDownList();
        CheckBox chBx = new CheckBox();
        string sType = "";

        string sPrd = "";
        string sMod = "";
        string sDsc = "";
        int iReorderPoint = 0;
        int iReorderQty = 0;
        int iReorderPointOrig = 0;
        int iReorderQtyOrig = 0;
        string sChk = "";
        string sHeaderDone = "";

        try
        {
            foreach (Control c1 in gvPartsOld.Controls)
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
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                        {
                                            lb = (Label)c4;
                                            if (lb.ID == "lbPrdOld")
                                                sPrd = lb.Text;
                                            if (lb.ID == "lbModOld")
                                                sMod = lb.Text;
                                            if (lb.ID == "lbDscOld")
                                                sDsc = lb.Text;
                                        }
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                                        {
                                            hf = (HiddenField)c4;
                                            if (hf.ID == "hfReorderPoint")
                                            {
                                                if (int.TryParse(hf.Value.Trim(), out iReorderPointOrig) == false)
                                                    iReorderPointOrig = 0;
                                            }
                                            if (hf.ID == "hfReorderQty")
                                            {
                                                if (int.TryParse(hf.Value.Trim(), out iReorderQtyOrig) == false)
                                                    iReorderQtyOrig = 0;
                                            }
                                        }
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                                        {
                                            dd = (DropDownList)c4;
                                            if (dd.ID == "ddReorderPointOld")
                                            {
                                                if (int.TryParse(dd.SelectedValue.Trim(), out iReorderPoint) == false)
                                                    iReorderPoint = 0;
                                            }
                                            if (dd.ID == "ddReorderQtyOld")
                                            {
                                                if (int.TryParse(dd.SelectedValue.Trim(), out iReorderQty) == false)
                                                    iReorderQty = 0;
                                            }
                                        }
                                        // ==============================
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                                        {
                                            chBx = (CheckBox)c4;
                                            if (chBx.Checked == true)
                                                sChk = "DELETE";

                                            // Regardless of checked or not, you reached the last record on line. Process now.
                                            // ---------------------------
                                            // Build Table Header (if first pass) 
                                            // ---------------------------
                                            if (sHeaderDone == "")
                                            {
                                                sHeaderDone = "YES";
                                                sRtn = "<p><b><font color='#AD0034'>Updates</font> to Auto Replenish List</b><br />";
                                                sRtn += "<table><tr>" +
                                                    "<th>Product Code</th>" +
                                                    "<th>Model</th>" +
                                                    "<th>Description</th>" +
                                                    "<th>Reorder Point</th>" +
                                                    "<th>Reorder Qty</th>" +
                                                    "<th>Delete Line?</th>" +
                                                    "</tr>";
                                            }

                                            // ---------------------------
                                            // Add line to email
                                            // ---------------------------
                                            sRtn += "<tr>" +
                                                "<td>" + sPrd + "</td>" +
                                                "<td>" + sMod + "</td>" +
                                                "<td>" + sDsc + "</td>" +
                                                "<td>" + iReorderPointOrig.ToString() + " -> " + iReorderPoint.ToString() + "</td>" +
                                                "<td>" + iReorderQtyOrig.ToString() + " -> " + iReorderQty.ToString() + "</td>" +
                                                "<td>" + sChk + "&nbsp;</td>" +
                                                "</tr>";

                                            // ---------------------------
                                            // Initialize Hold Fields
                                            // ---------------------------
                                            sPrd = "";
                                            sMod = "";
                                            sDsc = "";
                                            iReorderPoint = 0;
                                            iReorderPointOrig = 0;
                                            iReorderQty = 0;
                                            iReorderQtyOrig = 0;
                                            sChk = "";
                                            // ---------------------------
                                        }
                                        // ==============================
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (sHeaderDone == "YES")
                sRtn += "</table><br /><br />";

        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            sRtn = "An error occurred retrieving updates to the auto replenish list";
        }
        finally
        {
        }

        return sRtn;
    }
    // =========================================================
    protected void btInput_Click(object sender, EventArgs e)
    {
        loadPanelAutoReplenish();
    }
    // =========================================================
    protected void btDownload_Click(object sender, EventArgs e)
    {
        sDownload = "Y";
        loadPanelAutoReplenish();
    }
    // =========================================================
    protected void btUpdateList_Click(object sender, EventArgs e)
    {
        pnInput.Visible = false;
        pnCs1Change.Visible = false;
        pnAutoReplenish.Visible = false;
        pnPartsNew.Visible = false;
        pnPartsOld.Visible = false;
        pnUpdateList.Visible = true;

        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        // ----------------------
        // Check for New Parts to Add
        // ----------------------
        int iNewLines = 0;
        if (ddNewLines.SelectedValue != "")
        {
            if (int.TryParse(ddNewLines.SelectedValue.ToString().Trim(), out iNewLines) == false)
            {
                iNewLines = 0;
            }
            else
            {
                lbPartsNew.Text = "New Parts To Add To List";
                dataTable = new DataTable(sMethodName);
                dataTable.Columns.Add(makeColumn("Test"));

                for (int i = 0; i < iNewLines; i++) 
                    dataTable.Rows.Add();
                dataTable.AcceptChanges();
                gvPartsNew.DataSource = dataTable;
                gvPartsNew.DataBind();
                pnPartsNew.Visible = true;
            }
        }
        // ----------------------
        // Check for Old Parts to Update
        // ----------------------
        dataTable = GetSelectedLines_gv();
        if (dataTable.Rows.Count > 0) 
        {
            lbPartsOld.Text = "Updates To Current List";
            gvPartsOld.DataSource = dataTable;
            gvPartsOld.DataBind();
            SetOldLines_gv();
            pnPartsOld.Visible = true;
        }
        // ----------------------
    }
    // =========================================================
    protected void btEmail_Click(object sender, EventArgs e)
    {
        sendEmail();
    }
    // =========================================================
    protected void sendEmail()
    {
        string sSbj = "";
        string sMsg = "";
        string sRtn = "";

        string sEmail = "";
        string sComment = "";
        string sCustName = "";
        string sStockLoc = "";
        string sUserId = "";

        try
        {
            int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

            if (sPageLib == "L")
            {
                sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1ToUse, 0);
            }
            else
            {
                sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1ToUse, 0);
            }

            sStockLoc = ddStockLoc.SelectedValue;

            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserId = mu.UserName;
            }

            sEmail = txEmail.Text.Trim();
            sComment = txComment.Text.Trim();

            // Build HTML Email Content
            sSbj = "Self Service Auto Replenish Update";

            sMsg = "<html><head><title>" +
                HttpUtility.HtmlEncode(sSbj) +
            "</title>" +
            "<style>" +
            "body { font-family: verdana; font-size: 13px; margin-left: 30px; border-collapse: collapse; }" +
            "table { font-family: verdana; }" +
            "th { font-size: 13px; background-color: #406080; color: #FFFFFF; font-weight: normal; padding: 4px; }" + // light steel blue #B0C4DE 
            "td { font-size: 12px; border: 1px solid #555555; padding: 2px; padding-left: 5px; padding-right: 5px; }" +
            "</style>" +
            "</head><body>";

            if (txComment.Text != "")
                sMsg += "<p><b>Comment</b><br />" + HttpUtility.HtmlEncode(sComment) + "</p>";

            sMsg += "<p><b>Customer</b><br />" + sCustName + " (" + iCs1ToUse.ToString() + ")" + "</p>";

            sMsg += "<p><b>Stocking Location</b><br />" + sStockLoc + "</p>";

            sMsg += "<p><b>User Id</b><br />" + sUserId + "</p>";

            sMsg += GetNewLines_gv();
            sMsg += GetOldLines_gv();

            if (sEmail != "")
                sMsg += "<p><b>Fulfillment Response Requested</b><br />" + HttpUtility.HtmlEncode(sEmail) + "</p>";

            if (sPageLib == "L")
            {
                sRtn = wsLive.EmailBasic(sfd.GetWsKey(), sSbj, sMsg, "HTML", "C06");
            }
            else
            {
                sRtn = wsTest.EmailBasic(sfd.GetWsKey(), sSbj, sMsg, "HTML", "C06");
            }
            if (sRtn == "SUCCESS")
            {
                lbError.Text = "Email Successfully Sent";
            }
            else
            {
                lbError.Text = "There was a problem with the email: " + sRtn;
            }
            lbError.Visible = true;
        }
        catch (Exception ex)
        {
            sRtn = ex.ToString();
            lbError.Text = "There was a problem with the email: " + ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            pnInput.Visible = true;
            pnCs1Change.Visible = true;
            pnAutoReplenish.Visible = true;
            pnUpdateList.Visible = false;
            pnPartsNew.Visible = false;
            pnPartsOld.Visible = false;
            ddNewLines.SelectedValue = "";
            txComment.Text = "";
            loadPanelAutoReplenish();
        }
    }
    // ========================================================================
    protected DataColumn makeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
    // =========================================================
    // =========================================================
}