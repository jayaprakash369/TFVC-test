using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;

public partial class private_sc_req_Location : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    string sCs1Family = "";
    string sChosenCs1Type = "";
    char[] cSplitter = { ',' };
    char[] cSplitter2 = { '|' };
    char[] cSplitter3 = { '~' };
    string[] saCs1All = new string[1];
    string[] saCs1Nam = new string[1];
    string sCs1Changed = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sJScript = "";

        if (!IsPostBack)
        {
            // sJScript = @"alert(document.forms[0]['txNam'].value);";
            //sJScript = "../../../private/js/ProgressBar.js";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScriptBar", sJScript, true);

            int iCs1 = GetPrimaryCs1();
            hfCs1.Value = iCs1.ToString();
            ViewState["vsDataTable_Loc"] = null;
            txNam.Text = ""; // Response.Redirect to restart after backing from Result to Problem loaded txRef value in name...
            txCit.Text = "";
            txSta.Text = "";
            txZip.Text = "";
            txPhn.Text = "";
            txXrf.Text = "";
            LoadPanelLocation(iCs1);

            sJScript = @"clearLocSearch();";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", sJScript, true);
        }
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
            int iCs1ToUse = GetChosenCs1();
            int iCs2 = 0;

            string sNam = "";
            string sCon = "";
            string sAdr = "";
            string sCit = "";
            string sSta = "";
            string sZip = "";
            string sPhn = "";
            string sXrf = "";
            string sCs2Used = "N";

            if (int.TryParse(txCs2.Text, out iCs2) == false)
                iCs2 = 0;
            else
                sCs2Used = "Y";
            
            sNam = txNam.Text.Trim();
            sAdr = txAdr.Text.Trim();
            sCit = txCit.Text.Trim();
            sSta = txSta.Text.Trim();
            sZip = txZip.Text.Trim();
            sPhn = txPhn.Text.Trim();
            sXrf = txXrf.Text.Trim();

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetLocDetailAll(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sNam, sCon, sAdr, sCit, sSta, sZip, sPhn, sXrf);
            }
            else
            {
                dataTable = wsTest.GetLocDetailAll(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sNam, sCon, sAdr, sCit, sSta, sZip, sPhn, sXrf);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dataTable;
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Loc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Loc;
        if (gridSortDirection_Loc == SortDirection.Ascending)
        {
            sortExpression_Loc = gridSortExpression_Loc + " ASC";
        }
        else
        {
            sortExpression_Loc = gridSortExpression_Loc + " DESC";
        }
        //    Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression_Loc;

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
            // Initial sort expression is...
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
    // Name field selects a specific location (so it could be called lbLocationOne_Click
    protected void lkName_Click(object sender, EventArgs e)  // new in progress
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iCs1 = 0;
        int iCs2 = 0;
        if (int.TryParse(saArg[0], out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(saArg[1], out iCs2) == false)
            iCs2 = 0;

        // If specific customer chosen from parent group, save Cs1, Cs2 in hidden fields for SQLs
        if (iCs1 > 0)
        {
            hfCs1.Value = iCs1.ToString();  
            hfCs2.Value = iCs2.ToString();
            // Jump to contact page, save as pp values instead of hidden
        }
    }
    // =========================================================
    // END LOCATION GRID 
    // =========================================================


    // =========================================================
    // START CLICK METHODS
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iCs1 = GetPrimaryCs1();
        ViewState["vsDataTable_Loc"] = null;
        LoadPanelLocation(iCs1);
    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        if (sCs1Changed == "YES")
        {
            int iCs1 = GetPrimaryCs1();
            LoadPanelLocation(iCs1);
        }
        else
        {

        // Check for hackers bypassing client validation
            if (Page.IsValid)
            {
                ServerSideVal_LocSearch();

                // If all server side validation is also passed...
                if (vCustom_LocSearch.IsValid == true)
                {
                    ViewState["vsDataTable_Loc"] = null;
                    BindGrid_Loc();
                    pnLocations.Visible = true;
                }
            }
        }
    }
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    protected string ServerSideVal_LocSearch()
    {
        string sResult = "";

        txNam.Text = txNam.Text.ToUpper();
        txAdr.Text = txAdr.Text.ToUpper();
        txCit.Text = txCit.Text.ToUpper();
        txSta.Text = txSta.Text.ToUpper();
        txZip.Text = txZip.Text.ToUpper();
        txXrf.Text = txXrf.Text.ToUpper();

        try
        {
            int iNum = 0;
            long lNum = 0;
            // Location
            if (txCs2.Text != "")
            {
                if (int.TryParse(txCs2.Text, out iNum) == false)
                {
                    if (vCustom_LocSearch.ErrorMessage == "")
                    {
                        vCustom_LocSearch.ErrorMessage = "The location must be a number";
                        vCustom_LocSearch.IsValid = false;
                        txCs2.Focus();
                    }
                }
                else
                {
                    if (iNum > 999)
                    {
                        if (vCustom_LocSearch.ErrorMessage == "")
                        {
                            vCustom_LocSearch.ErrorMessage = "Location entry must be 3 digits or less";
                            vCustom_LocSearch.IsValid = false;
                            txCs2.Focus();
                        }
                    }
                }
            }
            // Name
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txNam.Text != "" && txNam.Text.Length > 40)
                {
                    vCustom_LocSearch.ErrorMessage = "Name must be 40 characters or less";
                    vCustom_LocSearch.IsValid = false;
                    txNam.Text = txNam.Text.Substring(0, 40);
                    txNam.Focus();
                }
            }
            // Cust Cross Ref
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txXrf.Text != "" && txXrf.Text.Length > 15)
                {
                    vCustom_LocSearch.ErrorMessage = "Cross reference must be 15 characters or less";
                    vCustom_LocSearch.IsValid = false;
                    txXrf.Text = txXrf.Text.Substring(0, 15);
                    txXrf.Focus();
                }
            }
            // Address
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txAdr.Text != "" && txAdr.Text.Length > 30)
                {
                    vCustom_LocSearch.ErrorMessage = "The address entry must be 30 characters or less";
                    vCustom_LocSearch.IsValid = false;
                    txAdr.Text = txAdr.Text.Substring(0, 30);
                    txAdr.Focus();
                }
            }
            
            // City
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txCit.Text != "" && txCit.Text.Length > 30)
                {
                    vCustom_LocSearch.ErrorMessage = "City must be 30 characters or less";
                    vCustom_LocSearch.IsValid = false;
                    txCit.Text = txCit.Text.Substring(0, 30);
                    txCit.Focus();
                }
            }
            
            // State
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txSta.Text != "" && txSta.Text.Length > 2)
                {
                    vCustom_LocSearch.ErrorMessage = "The state abbreviation must be 2 characters or less";
                    vCustom_LocSearch.IsValid = false;
                    txSta.Text = txSta.Text.Substring(0, 2);
                    txSta.Focus();
                }
            }
            
            // Zip
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txZip.Text != "")
                {
                    if (int.TryParse(txZip.Text, out iNum) == false)
                    {

                        vCustom_LocSearch.ErrorMessage = "The zip code must be a number";
                        vCustom_LocSearch.IsValid = false;
                        txZip.Focus();
                    }
                    else 
                    {
                        if (txZip.Text.Length > 9)
                        {
                            vCustom_LocSearch.ErrorMessage = "The zip code must be 9 digits or less";
                            vCustom_LocSearch.IsValid = false;
                            txZip.Text = txZip.Text.Substring(0, 9);
                            txZip.Focus();
                        }
                    }
                }
            }
            
            // Phone
            if (vCustom_LocSearch.IsValid == true)
            {
                if (txPhn.Text != "")
                {
                    if (long.TryParse(txPhn.Text, out lNum) == false)
                    {

                        vCustom_LocSearch.ErrorMessage = "Phone entry must only consist of numbers";
                        vCustom_LocSearch.IsValid = false;
                        txPhn.Focus();
                    }
                    else
                    {
                        if (txPhn.Text.Length > 10)
                        {
                            vCustom_LocSearch.ErrorMessage = "The phone entry must be 10 digits or less";
                            vCustom_LocSearch.IsValid = false;
                            txPhn.Text = txPhn.Text.Substring(0, 10);
                            txPhn.Focus();
                        }
                    }
                }
            }
            
            // Large Customer Query Size Check
            if (vCustom_LocSearch.IsValid == true)
            {
                int iCs1ToUse = 0;
                int.TryParse(hfCs1.Value, out iCs1ToUse);
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
                    if ((ddCs1Family.SelectedValue == "") &&
                            (txNam.Text == "") &&
                            (txCs2.Text == "") &&
                            (txCit.Text == "") &&
                            (txSta.Text == "") &&
                            (txZip.Text == "") &&
                            (txPhn.Text == "") &&
                            (txXrf.Text == ""))
                    {
                        vCustom_LocSearch.ErrorMessage = "Please narrow your search by entering values in the search boxes";
                        vCustom_LocSearch.IsValid = false;
                        txNam.Focus();
                    }
                }
            }
            // ----------------------------------
            if (vCustom_LocSearch.IsValid == true)
                sResult = "VALID";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            vCustom_LocSearch.ErrorMessage = "A unexpected system error has occurred";
            vCustom_LocSearch.IsValid = false;
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
    protected void LoadPanelLocation(int cs1ToUse)
    {
        string sCs1 = "";
        string sNam = "";
        string sCs1Nam = "";

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
            lbCust.Text = "Customer Number";
            ddCs1Family.Visible = true;
            lbAddress.Text = "";
            txAdr.Visible = false;

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
            if (sCs1Family != "")
            {
                for (int i = 0; i < saCs1All.Length; i++)
                {
                    sCs1Nam = saCs1All[i];
                    saCs1Nam = sCs1Nam.Split(cSplitter2);
                    sCs1 = "";
                    sNam = "";

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
            }
            ddCs1Family.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
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
                if (pnCs1Change.Visible == false)
                    pnCs1Change.Visible = true;

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
    // Previous Page Parms to load for next page retrieval 
    // =========================================================
    public int pp_Pri()
    {
        return GetPrimaryCs1();
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
    // =========================================================
}

