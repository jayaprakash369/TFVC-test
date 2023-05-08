using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_sc_ContractEquipment : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    SourceForTicket sft = new SourceForTicket();
    SourceForCustomer sfc = new SourceForCustomer();

    string sCs1Family = "";
    string sChosenCs1Type = "";
    char[] cSplitter = { ',' };
    char[] cSplitter2 = { '|' };
    char[] cSplitter3 = { '~' };
    string[] saCs1All = new string[1];
    string[] saCs1Nam = new string[1];
    string sCs1Changed = "";
    string sDownloadEqp = "";
    //string sXrefEdit = "";
    int[] iaCs1Cs2 = new int[2];

//    DataTable dataTable;
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            if (sPageLib == "L")
            {
                hfXrefLocEditor.Value = wsLive.GetPrefRequestUpdLocXref(sfd.GetWsKey(), iCs1ToUse);
                hfXrefEqpEditor.Value = wsLive.GetPrefRequestUpdEqpXref(sfd.GetWsKey(), iCs1ToUse);
            }
            else
            {
                hfXrefLocEditor.Value = wsTest.GetPrefRequestUpdLocXref(sfd.GetWsKey(), iCs1ToUse);
                hfXrefEqpEditor.Value = wsTest.GetPrefRequestUpdEqpXref(sfd.GetWsKey(), iCs1ToUse);
            }
            ReloadLocPage(iCs1ToUse);
        }
        pnCstXrfUpd.Visible = false;
        pnEqpXrfUpd.Visible = false;
    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";
        lbError.Text = "";

        if (sCs1Changed == "YES")
        {
            int iPrimaryCs1 = GetPrimaryCs1();
            ReloadLocPage(iPrimaryCs1);
        }
        else
        {
            // Check for hackers bypassing client validation
            if (Page.IsValid)
            {
                vSummary_LocSearch.Visible = false;
                sValid = ServerSideVal_LocSearch();

                // If all server side validation is also passed...
                if (sValid == "VALID")
                {
                    ViewState["vsDataTable_Loc"] = null;
                    BindGrid_Loc();
                    pnLocations.Visible = true;
                }
            }
            else
            {
                vSummary_LocSearch.Visible = true;
            }
        }
        if (lbError.Text != "")
            lbError.Visible = true;
        else
            lbError.Visible = false;
    }
    // =========================================================
    protected void lkCstXrfEdit_Click(object sender, EventArgs e) 
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iCs1 = Int32.Parse(saArg[0]);
        int iCs2 = Int32.Parse(saArg[1]);
        string sXrf = saArg[2].Trim();

        // If you have data, 
        if (iCs1 > 0)
        {
            lbCstXrfUpdCs1.Text = iCs1.ToString();
            lbCstXrfUpdCs2.Text = iCs2.ToString();
            txCstXrfUpd.Text = sXrf;
            pnCstXrfUpd.Visible = true;
            txCstXrfUpd.Focus();
        }
    }
    // =========================================================
    protected void btCstXrfUpd_Click(object sender, EventArgs e)
    {
        int iCs1 = Int32.Parse(lbCstXrfUpdCs1.Text);
        int iCs2 = Int32.Parse(lbCstXrfUpdCs2.Text);
        string sXrf = txCstXrfUpd.Text.Trim();

        if (sPageLib == "L")
        {
            wsLive.SetCustCrossRef(sfd.GetWsKey(), iCs1, iCs2, sXrf);
        }
        else
        {
            wsTest.SetCustCrossRef(sfd.GetWsKey(), iCs1, iCs2, sXrf);
        }
        int iPrimaryCs1 = GetPrimaryCs1();
        ReloadLocPage(iPrimaryCs1);
    }
    // =========================================================
    protected void lkEqpXrfEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[4];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        string sMod = saArg[0].Trim();
        string sSer = saArg[1].Trim();
        int iUnt = Int32.Parse(saArg[2]);
        string sXrf = saArg[3].Trim();

        // If you have data, 
        if ((sMod != null) && (sMod != ""))
        {
            lbEqpXrfUpdMod.Text = sMod;
            lbEqpXrfUpdSer.Text = sSer;
            hfEqpXrfUpdUnt.Value = iUnt.ToString();
            txEqpXrfUpd.Text = sXrf;
            pnEqpXrfUpd.Visible = true;
            txEqpXrfUpd.Focus();
        }
    }
    // =========================================================
    protected void btEqpXrfUpd_Click(object sender, EventArgs e)
    {
        int iUnt = Int32.Parse(hfEqpXrfUpdUnt.Value);
        string sXrf = txEqpXrfUpd.Text.Trim();

        if (sPageLib == "L")
        {
            wsLive.SetEqpCrossRef(sfd.GetWsKey(), iUnt, sXrf);
        }
        else
        {
            wsTest.SetEqpCrossRef(sfd.GetWsKey(), iUnt, sXrf);
        }

        int iCs1 = 0;
        int iCs2 = 0;
        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
            iCs2 = 0;

        ReloadEqpPage(iCs1, iCs2, "", "", "", "", "", "", "");
    }

//==========================================================
// =========================================================
    protected void lkXrefPick_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[1];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
       
        int iUnt = 0;
        if (int.TryParse(saArg[0], out iUnt) == false)
            iUnt = 0;
        string sAgr = saArg[1].Trim();

  if (iUnt != 0)
        {
            hfUnitList.Value = iUnt.ToString() + "~" + sAgr;

            Session["reqCs1"] = hfCs1.Value.Trim();
            Session["reqCs2"] = hfCs2.Value.Trim();
            Session["reqXref"] = hfXref.Value.Trim();
            Session["reqUnitList"] = hfUnitList.Value.Trim();
            Session["reqSource"] = "Xref";

            Response.Redirect("~/private/sc/req/Problem.aspx", false);
        }
    }
// =========================================================
// =========================================================
    protected void lkName_Click(object sender, EventArgs e) 
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iCs1 = Int32.Parse(saArg[0]);
        int iCs2 = Int32.Parse(saArg[1]);
        hfCs1.Value = iCs1.ToString();
        hfCs2.Value = iCs2.ToString();

        string sPrd = "";
        string sMod = "";
        string sSer = "";
        string sDsc = "";
        string sFxa = "";
        string sDownload = "";
        string sAgn = "";

        LoadProductCodes(iCs1, iCs2);

        // If you have data, 
        if (iCs1 > 0)
        {
            ReloadEqpPage(iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn, sDownload);
        }
    }
    // =========================================================
    protected void btEqpSearch_Click(object sender, EventArgs e) 
    {
        Button buttonControl = (Button)sender;
        string sButtonText = buttonControl.Text;
        sDownloadEqp = "";
        if (sButtonText == "Download")
            sDownloadEqp = "Y";

        txMod.Text = txMod.Text.ToUpper().Trim();
        txDsc.Text = txDsc.Text.ToUpper().Trim();
        txSer.Text = txSer.Text.ToUpper().Trim();
        txFxa.Text = txFxa.Text.ToUpper().Trim();
        txAgn.Text = txAgn.Text.ToUpper().Trim();

        int iCs1 = 0;
        int iCs2 = 0;

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
            iCs2 = 0;

        string sPrd = ddPrd.SelectedValue.ToString().Trim();
        string sMod = txMod.Text.Trim();
        string sSer = txSer.Text.Trim();
        string sDsc = txDsc.Text.Trim();
        string sFxa = txFxa.Text.Trim();
        string sAgn = txAgn.Text.Trim();

        // If you have data, 
        if (iCs1 > 0)
        {
            ReloadEqpPage(iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn, sDownloadEqp);
        }
    }
    // =========================================================
    protected string ServerSideVal_LocSearch()
    {
        string sResult = "";
        lbError.Text = "";

        txNam.Text = txNam.Text.ToUpper();
        txAdr.Text = txAdr.Text.ToUpper();
        txCit.Text = txCit.Text.ToUpper();
        txSta.Text = txSta.Text.ToUpper();
        txZip.Text = txZip.Text.ToUpper();
        txXrf.Text = txXrf.Text.ToUpper();

        try
        {
            int iNum = 0;
            if (txCs2.Text != "")
            {
                if (int.TryParse(txCs2.Text, out iNum) == false)
                {
                    if (lbError.Text == "")
                    {
                        lbError.Text = "The location must be a number";
                        txCs2.Focus();
                    }
                }
                else
                {
                    if (iNum > 999)
                    {
                        if (lbError.Text == "")
                        {
                            lbError.Text = "Location entry must be 3 digits or less";
                            txCs2.Text = txCs2.Text.Substring(0, 3);
                            txCs2.Focus();
                        }
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txNam.Text != "")
                {
                    if (txNam.Text.Length > 40)
                    {
                        lbError.Text = "Name must be 40 characters or less";
                        txNam.Text = txNam.Text.Substring(0, 40);
                        txNam.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txXrf.Text != "")
                {
                    if (txXrf.Text.Length > 15)
                    {
                        lbError.Text = "Cross reference must be 15 characters or less";
                        txXrf.Text = txXrf.Text.Substring(0, 15);
                        txXrf.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txAdr.Text != "")
                {
                    if (txAdr.Text.Length > 30)
                    {
                        lbError.Text = "The address entry must be 30 characters or less";
                        txAdr.Text = txAdr.Text.Substring(0, 30);
                        txAdr.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txCit.Text != "")
                {
                    if (txCit.Text.Length > 30)
                    {
                        lbError.Text = "City must be 30 characters or less";
                        txCit.Text = txCit.Text.Substring(0, 30);
                        txCit.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txSta.Text != "")
                {
                    if (txSta.Text.Length > 2)
                    {
                        lbError.Text = "The state abbreviation must be 2 characters or less";
                        txSta.Text = txSta.Text.Substring(0, 2);
                        txSta.Focus();
                    }
                }
            }

            if (lbError.Text == "")
            {
                if (txZip.Text != "")
                {
                    if (int.TryParse(txZip.Text, out iNum) == false)
                    {
                        if (lbError.Text == "")
                        {
                            lbError.Text = "The zip code must be a number";
                            txZip.Focus();
                        }
                    }
                    else
                    {
                        if (txZip.Text.Length > 9)
                        {
                            lbError.Text = "The zip code must be 9 digits or less";
                            txZip.Text = txZip.Text.Substring(0, 9);
                            txZip.Focus();
                        }
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txPhn.Text != "")
                {
                    if (int.TryParse(txPhn.Text, out iNum) == false)
                    {
                        if (lbError.Text == "")
                        {
                            lbError.Text = "The phone entry must be a number";
                            txPhn.Focus();
                        }
                    }
                    else
                    {
                        if (txPhn.Text.Length > 10)
                        {
                            lbError.Text = "The phone entry must be 10 digits or less";
                            txPhn.Text = txPhn.Text.Substring(0, 10);
                            txPhn.Focus();
                        }
                    }
                }
            }
            // ---------------------------------------
            int iCs1ToUse = GetPrimaryCs1();
            sChosenCs1Type = "";
            if (sPageLib == "L")
            {
                sChosenCs1Type = wsLive.GetCustType(sfd.GetWsKey(), iCs1ToUse);
            }
            else
            {
                sChosenCs1Type = wsTest.GetCustType(sfd.GetWsKey(), iCs1ToUse);
            }

            if ((sChosenCs1Type == "LRG") || (sChosenCs1Type == "DLR"))
            {
                if (lbError.Text == "")
                {
                    if ((ddCs1Family.SelectedValue == "") &&
                            (txNam.Text == "") &&
                            (txCs2.Text == "") &&
                            (txCit.Text == "") &&
                            (txSta.Text == "") &&
                            (txZip.Text == "") &&
                            (txPhn.Text == "") &&
                            (txXrf.Text == ""))
                    {
                        lbError.Text = "Please narrow your search by entering values in the search boxes";
                        txNam.Focus();
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
    // START LOCATION GRID
    // =========================================================
    protected void gvPageIndexChanging_Loc(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvLocations.PageIndex = newPageIndex;
        BindGrid_Loc();
    }
    // =========================================================
    protected void BindGrid_Loc()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbError.Text = "";

            int iCs1ToUse = GetChosenCs1();
            int iCs2 = 0;

            string sCs2Used = "";
            string sNam = "";
            string sCon = "";
            string sAdr = "";
            string sCit = "";
            string sSta = "";
            string sZip = "";
            string sPhn = "";
            string sXrf = "";

            if (int.TryParse(txCs2.Text, out iCs2) == false)
                iCs2 = 0;
            else
                sCs2Used = "Y";

            sNam = txNam.Text;
            sAdr = txAdr.Text;
            sCit = txCit.Text;
            sSta = txSta.Text;
            sZip = txZip.Text;
            sPhn = txPhn.Text;
            sXrf = txXrf.Text;

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetLocDetail(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sNam, sCon, sAdr, sCit, sSta, sZip, sPhn, sXrf);
            }
            else
            {
                dataTable = wsTest.GetLocDetail(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sNam, sCon, sAdr, sCit, sSta, sZip, sPhn, sXrf);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dataTable;

            if (dataTable.Rows.Count == 0)
                lbError.Text = "No matching locations were found...";
            else
                lbError.Text = "";
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Loc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Loc == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Loc + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Loc + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression;

        gvLocations.DataSource = dataTable.DefaultView;
        gvLocations.DataBind();
    }
    // =========================================================
    protected void gvSorting_Loc(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Loc = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Loc == e.SortExpression)
        {
            if (gridSortDirection_Loc == SortDirection.Ascending)
                gridSortDirection_Loc = SortDirection.Descending;
            else
                gridSortDirection_Loc = SortDirection.Ascending;
        }
        else
            gridSortDirection_Loc = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Loc = sortExpression_Loc;
        // Rebind the grid to its data source
        BindGrid_Loc();
    }
    private SortDirection gridSortDirection_Loc
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Loc"] == null)
            {
                ViewState["GridSortDirection_Loc"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Loc"];
        }
        set
        {
            ViewState["GridSortDirection_Loc"] = value;
        }
    }
    private string gridSortExpression_Loc
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Loc"] == null)
            {
                ViewState["GridSortExpression_Loc"] = "CustLoc";
            }
            return (string)ViewState["GridSortExpression_Loc"];
        }
        set
        {
            ViewState["GridSortExpression_Loc"] = value;
        }
    }
    // =========================================================
    // END LOCATION GRID
    // =========================================================
    // =========================================================
    // START EQUIPMENT GRID
    // =========================================================
    protected void BindGrid_Eqp()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Eqp"] == null)
        {
            string sPrd = ddPrd.SelectedValue.ToString();
            string sMod = txMod.Text.Trim();
            string sDsc = txDsc.Text.Trim();
            string sSer = txSer.Text.Trim();
            string sFxa = txFxa.Text.Trim();
            string sAgn = txAgn.Text.Trim();

            GetCs1Cs2();

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetLocEqp(sfd.GetWsKey(), iaCs1Cs2[0], iaCs1Cs2[1], sPrd, sMod, sSer, sDsc, sFxa, sAgn);
            }
            else
            {
                dataTable = wsTest.GetLocEqp(sfd.GetWsKey(), iaCs1Cs2[0], iaCs1Cs2[1], sPrd, sMod, sSer, sDsc, sFxa, sAgn);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dataTable;

            if (dataTable.Rows.Count == 0)
                lbError.Text = "No matching equipment was found...";
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Eqp"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Eqp;
        if (gridSortDirection_Eqp == SortDirection.Ascending)
        {
            sortExpression_Eqp = gridSortExpression_Eqp + " ASC";
        }
        else
        {
            sortExpression_Eqp = gridSortExpression_Eqp + " DESC";
        }

        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression_Eqp;

        gvEquipment.DataSource = dataTable.DefaultView;
        gvEquipment.PageSize = 500;
        gvEquipment.DataBind();
    }
    // =========================================================
    protected void gvPageIndexChanging_Eqp(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvEquipment.PageIndex = newPageIndex;
        BindGrid_Eqp();
    }
    // =========================================================
    protected void gvSorting_Eqp(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Eqp = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Eqp == e.SortExpression)
        {
            if (gridSortDirection_Eqp == SortDirection.Ascending)
                gridSortDirection_Eqp = SortDirection.Descending;
            else
                gridSortDirection_Eqp = SortDirection.Ascending;
        }
        else
            gridSortDirection_Eqp = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Eqp = sortExpression_Eqp;
        // Rebind the grid to its data source
        BindGrid_Eqp();
    }
    private SortDirection gridSortDirection_Eqp
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Eqp"] == null)
            {
                ViewState["GridSortDirection_Eqp"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Eqp"];
        }
        set
        {
            ViewState["GridSortDirection_Eqp"] = value;
        }
    }
    private string gridSortExpression_Eqp
    {
        get
        {
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Eqp"] == null)
            {
                ViewState["GridSortExpression_Eqp"] = "Part";
            }
            return (string)ViewState["GridSortExpression_Eqp"];
        }
        set
        {
            ViewState["GridSortExpression_Eqp"] = value;
        }
    }
    // =========================================================
    // END EQUIPMENT GRID 
    // =========================================================

    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        ReloadLocPage(iPrimaryCs1);
    }
    // =========================================================
    protected void ReloadLocPage(int cs1ToUse)
    {
        pnEqpSearch.Visible = false;
        pnEquipment.Visible = false;

        string sCs1 = "";
        string sNam = "";
        string sCs1Nam = "";

        lbCust.Text = "Customer Number";
        ddCs1Family.Visible = true;
        lbAddress.Text = "";
        txAdr.Visible = false;

        if (sPageLib == "L")
        {
            sChosenCs1Type = wsLive.GetCustType(sfd.GetWsKey(), cs1ToUse);
        }
        else
        {
            sChosenCs1Type = wsTest.GetCustType(sfd.GetWsKey(), cs1ToUse);
        }

        ViewState["vsDataTable_Loc"] = null;
        // Don't show default screen for huge customers, make them select search values
        if ((sChosenCs1Type == "LRG") || (sChosenCs1Type == "DLR"))
        {
            if (sPageLib == "L")
            {
                sCs1Family = wsLive.GetCs1KidNames(sfd.GetWsKey(), cs1ToUse);
                saCs1All = sCs1Family.Split(cSplitter3);
            }
            else
            {
                sCs1Family = wsTest.GetCs1KidNames(sfd.GetWsKey(), cs1ToUse);
                saCs1All = sCs1Family.Split(cSplitter3);
            }
            int iItems = ddCs1Family.Items.Count;
            for (int i = 0; i < iItems; i++)
            {
                ddCs1Family.Items.RemoveAt(0);
            }
            for (int i = 0; i < saCs1All.Length; i++)
            {
                sCs1 = "";
                sNam = "";
                sCs1Nam = saCs1All[i];
                saCs1Nam = sCs1Nam.Split(cSplitter2);
                if (saCs1Nam.Length > 0)
                    sCs1 = saCs1Nam[0];
                if (saCs1Nam.Length > 1)
                {
                    sNam = saCs1Nam[1];
                    if (sNam.Length > 40)
                        sNam = sNam.Substring(0, 40);
                }
                ddCs1Family.Items.Insert(i, new System.Web.UI.WebControls.ListItem(sCs1 + "  " + sNam, sCs1));
            }

            ddCs1Family.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value

            ddCs1Family.Enabled = true;
            pnLocations.Visible = false;

        }
        else 
        {
            lbCust.Text = "";
            ddCs1Family.Visible = false;
            lbAddress.Text = "Address";
            txAdr.Visible = true;

            ViewState["vsDataTable_Loc"] = null;
            BindGrid_Loc();
            pnLocations.Visible = true;
            txNam.Focus();
        }
        // Show/hide edit columns based on user type
        if (User.IsInRole("Administrator") || User.IsInRole("Editor") || (User.IsInRole("EditorCustomer") && hfXrefLocEditor.Value == "Y"))
        {
            gvLocations.Columns[4].Visible = true;
        }
        else
            gvLocations.Columns[4].Visible = false;
    }
    // =========================================================
    protected void ReloadEqpPage(int cs1, int cs2, string prd, string mod, string ser, string dsc, string fxa, string agn, string sDownload)
    {
        DataTable dt = new DataTable();

        pnLocSearch.Visible = false;
        pnLocations.Visible = false;
        pnEqpSearch.Visible = true;
        pnEquipment.Visible = true;

        pnCs1Header.Controls.Add(sfc.GetCustDataTable(cs1, cs2, "", "", ""));

        lbEquipment.Text = "Equipment at Location " + cs1.ToString() + "-" + cs2.ToString();

        if (sPageLib == "L")
        {
            dt = wsLive.GetLocEqp(sfd.GetWsKey(), cs1, cs2, prd, mod, ser, dsc, fxa, agn);
        }
        else
        {
            dt = wsTest.GetLocEqp(sfd.GetWsKey(), cs1, cs2, prd, mod, ser, dsc, fxa, agn);
        }

        gvEquipment.DataSource = dt;
        gvEquipment.DataBind();

        if (sDownload == "Y")
        {
            try
            {
                if (sPageLib == "L")
                {
                    dt = wsLive.GetLocEqpDownload(sfd.GetWsKey(), cs1, cs2, prd, mod, ser, dsc, fxa);
                }
                else
                {
                    dt = wsTest.GetLocEqpDownload(sfd.GetWsKey(), cs1, cs2, prd, mod, ser, dsc, fxa);
                }
                dt.TableName = "AgrEqp" + "_" + cs1.ToString() + "-" + cs2.ToString();

                if (dt.Rows.Count > 0)
                {
                    DownloadHandler dh = new DownloadHandler();
                    string sCsv = dh.DataTableToExcelCsv(dt);
                    dh = null;

                    Response.ClearContent();
                    Response.ContentType = "application/ms-excel";
                    Response.AddHeader("content-disposition", "attachment; filename=AgrEqp_" + cs1.ToString() + "-" + cs2.ToString() + ".csv");
                    Response.Write(sCsv);
                }
            }
            catch (Exception ex)
            {
                string sReturn = ex.ToString();
            }
        }
        // Show/hide edit columns based on user type
        if (User.IsInRole("Administrator") || User.IsInRole("Editor") || (User.IsInRole("EditorCustomer") && hfXrefEqpEditor.Value == "Y"))
        {
            gvEquipment.Columns[4].Visible = true;
        }
        else
            gvEquipment.Columns[4].Visible = false;

        if (sDownload == "Y")
            Response.End();

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
                                string sGoToMenu = sfd.checkGoToMenu("RegLrgDlrSsb", iPrimaryCs1);
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
    protected int GetChosenCs1()
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        int iFamilyCs1 = 0;
        int iChosenCs1 = iPrimaryCs1;

        if (sPageLib == "L")
        {
            sChosenCs1Type = wsLive.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
        }
        else
        {
            sChosenCs1Type = wsTest.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
        }

        if ((sChosenCs1Type == "LRG") || (sChosenCs1Type == "DLR"))
        {
            if (ddCs1Family.SelectedValue != "")
            {
                if (int.TryParse(ddCs1Family.SelectedValue, out iFamilyCs1) == false)
                    iFamilyCs1 = 0;
                else
                {
                    iChosenCs1 = iFamilyCs1;
                }
            }
        }

        return iChosenCs1;
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

                    if (iCs1Session != iCs1Textbox) 
                        sCs1Changed = "YES";
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    protected void LoadProductCodes(int cs1, int cs2)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetPartProductCodes(sfd.GetWsKey(), cs1, cs2);
        }
        else
        {
            dataTable = wsTest.GetPartProductCodes(sfd.GetWsKey(), cs1, cs2);
        }
        if (dataTable.Rows.Count > 0) 
        {
            ddPrd.DataSource = dataTable;

            ddPrd.DataSource = dataTable;
            ddPrd.DataValueField = "ProductCode";
            ddPrd.DataTextField = "ProductCode";
            ddPrd.DataBind();
            ddPrd.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        }
    }
    // =========================================================
    protected void GetCs1Cs2()
    {
        iaCs1Cs2[0] = 0;
        iaCs1Cs2[1] = 0;

        if (int.TryParse(hfCs1.Value, out iaCs1Cs2[0]) == false)
            iaCs1Cs2[0] = 0;
        if (int.TryParse(hfCs2.Value, out iaCs1Cs2[1]) == false)
            iaCs1Cs2[1] = 0;
    }
    // =========================================================
    // =========================================================

    protected void gvLocations_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}