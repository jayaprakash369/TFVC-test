using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_sc_EmailManagement : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForTicket sft = new SourceForTicket();
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
        // Wipe Screen Clean on each pass
        lbError.Text = "";
        lbMessage.Text = "";
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            int iPrimaryCs1 = iCs1ToUse;
            hfCs1.Value = iPrimaryCs1.ToString();
            hfCs2.Value = "";

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
                LoadLocationSearch(iCs1ToUse); 
            }
            else 
            {
                LoadParentLocation(iCs1ToUse);
            }
        }
    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";
        lbError.Text = "";

        if (sCs1Changed == "YES")
        {
            int iPrimaryCs1 = GetPrimaryCs1();
            ReloadPage(iPrimaryCs1);
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
            if (lbError.Text != "")
                lbError.Visible = true;
            else
                lbError.Visible = false;
        }
    }
    // =========================================================
    protected void AllLocations_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        int iCs1ToUse = iPrimaryCs1;
        string sFamilyMemberType = "PARENT";

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
            if (rbFamilyGlobal.Checked == true)
                sFamilyMemberType = "GRANDPARENT";

            if ((ddCs1Family.SelectedValue == "") && (sFamilyMemberType == "PARENT"))
            {
                vCusAllLoc.ErrorMessage = "A parent customer must be selected from the drop down list to change settings for that customer group";
                vCusAllLoc.IsValid = false;
            }
            else 
            {
                if (sFamilyMemberType == "PARENT")
                {
                    if (int.TryParse(ddCs1Family.SelectedValue, out iCs1ToUse) == false)
                        iCs1ToUse = 0;
                }
                else 
                {
                    iCs1ToUse = iPrimaryCs1;
                }
            }
        }

        if (Page.IsValid)
        {
            int iCs2 = 0;

            hfCs2.Value = "";

            if (int.TryParse(hfCs2.Value, out iCs2) == false)
                iCs2 = 0;

            if (iCs1ToUse > 0)
            {
                pnCs1Change.Visible = false;
                pnOneOrAllLocs.Visible = false;
                pnLocations.Visible = false;
                pnEmail.Visible = true;

                string sCustName = "";
                if (sPageLib == "L")
                {
                    sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
                }
                else
                {
                    sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
                }

                lbEmailCust.Text = sCustName + ": Customer " + iCs1ToUse.ToString();
                btToggleScreen.Text = "Switch To Manage INDIVIDUAL Locations";

                if (rbFamilyCs1.Checked == true)
                {
                    if (int.TryParse(ddCs1Family.SelectedValue, out iCs1ToUse) == false)
                        iCs1ToUse = 0;
                }
                else 
                {
                    iCs1ToUse = iPrimaryCs1;
                }
                hfCs1.Value = iCs1ToUse.ToString();
                hfCs2.Value = "";

                LoadEmailPanel(iCs1ToUse, iCs2, sFamilyMemberType);
            }
        }
    }
    // =========================================================
    protected void btToggleScreen_Click(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1();
        // --------------------------------------------------------------------------
        if (btToggleScreen.Text == "Switch To Manage ALL Locations")
        {
            LoadParentLocation(iCs1ToUse);
        }
        // --------------------------------------------------------------------------
        else if (btToggleScreen.Text == "Switch To Manage INDIVIDUAL Locations")
        {
            LoadLocationSearch(iCs1ToUse);
        }
        // --------------------------------------------------------------------------
    }
    // =========================================================
    // START LOCATION GRID
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

        int iCs1 = Int32.Parse(saArg[0]);
        int iCs2 = Int32.Parse(saArg[1]);

        // If you have data, 
        if (iCs1 > 0)
        {
            hfCs1.Value = iCs1.ToString();
            hfCs2.Value = iCs2.ToString();
            pnCs1Change.Visible = false;
            pnOneOrAllLocs.Visible = false;
            pnLocations.Visible = false;
            pnEmail.Visible = true;

            string sCustName = "";
            if (sPageLib == "L")
            {
                sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1, iCs2);
            }
            else
            {
                sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1, iCs2);
            }

            lbEmailCust.Text = sCustName + ": Customer " + iCs1.ToString();
            if (hfCs2.Value != "")
                lbEmailCust.Text += " at Location " + hfCs2.Value;

            LoadEmailPanel(iCs1, iCs2, "CHILD");
            btToggleScreen.Text = "Switch To Manage ALL Locations";
        }
    }

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
            // LRG/DLR section --------------- Plus iCs1 -> iCs1ToUse in GetLocDetail()
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

            sNam = txNam.Text.ToUpper();
            sAdr = txAdr.Text.ToUpper();
            sCit = txCit.Text.ToUpper();
            sSta = txSta.Text.ToUpper();
            sZip = txZip.Text.ToUpper();
            sPhn = txPhn.Text.ToUpper();
            sXrf = txXrf.Text.ToUpper();

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
        // Sort the data
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
    protected string ServerSideVal_LocSearch()
    {
        string sResult = "";
        lbError.Text = "";

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
                    if (iNum > 99)
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
    protected void LoadEmailPanel(int cs1, int cs2, string familyMemberType)
    {
        string sCs2 = "";
        if (familyMemberType == "CHILD")
            sCs2 = cs2.ToString();

        loadOpenCloseFlags(cs1, sCs2, familyMemberType);
        loadOpenRepeater(cs1, sCs2);
        loadCloseRepeater(cs1, sCs2);
    }
    // =========================================================
    protected void rblEmailSwitch_Click(object sender, EventArgs e)
    {
        RadioButtonList rblControl = (RadioButtonList)sender;
        //string sOpnOrCls = rblControl.CommandArgument.ToString();
        string sYESorNO = rblControl.SelectedValue.ToString();
        string sRadioID = rblControl.ID.ToString();
        string sOPNorCLS = "";
        string sOpenOrCloseText = "";
        string sFamilyMemberType = "";
        string sYorN = "";
        string sResult = "";

        if (sRadioID == "rblOpen")
        {
            sOPNorCLS = "OPN";
            sOpenOrCloseText = "Ticket OPEN email is now";
        }
        else if (sRadioID == "rblClose") 
        {
            sOPNorCLS = "CLS";
            sOpenOrCloseText = "Ticket CLOSE email is now";
        }

        if (sYESorNO == "YES")
        {
            sYorN = "Y";
            sOpenOrCloseText += " ACTIVE";
        }

        else
        {
            sYorN = "N";
            sOpenOrCloseText += " INACTIVE";
        }

        int iCs1 = 0;
        int iCs2 = 0;

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
            iCs2 = 0;

        sFamilyMemberType = GetFamilyMemberType(iCs1);

        if (sPageLib == "L")
        {
            sResult = wsLive.SetEmailOpenCloseFlag(sfd.GetWsKey(), iCs1, iCs2, sFamilyMemberType, sOPNorCLS, sYorN);
        }
        else
        {
            sResult = wsTest.SetEmailOpenCloseFlag(sfd.GetWsKey(), iCs1, iCs2, sFamilyMemberType, sOPNorCLS, sYorN);
        }
        if (sResult == "SUCCESS") 
            lbMessage.Text = sOpenOrCloseText;
        else
            lbError.Text = "Update failed: " + sResult;

        // --------------------   
    }
    // =========================================================
    protected void add_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sOpnOrCls = linkControl.CommandArgument.ToString();
        string sEmail = "";
        if (sOpnOrCls == "OPN")
            sEmail = txOpenAdd.Text;

        else if (sOpnOrCls == "CLS")
            sEmail = txCloseAdd.Text;

        if (sEmail != "") 
        {
            if (sEmail.Length > 50)
                sEmail = sEmail.Substring(0, 50);
        }

        int iCs1 = 0;
        int iCs2 = 0;
        string sCs2 = "";
        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
        {
            iCs2 = 0;
        }
        else
        {
            sCs2 = iCs2.ToString();
        }

        if (sEmail != "")
        {
            if (sPageLib == "L")
            {
                wsLive.AddDelOpenCloseEmail(sfd.GetWsKey(), iCs1, sCs2, sEmail, sOpnOrCls, "ADD");
            }
            else
            {
                wsTest.AddDelOpenCloseEmail(sfd.GetWsKey(), iCs1, sCs2, sEmail, sOpnOrCls, "ADD");
            }
        }
        // --------------------   
        // Update the Screen Tables
        if (sOpnOrCls == "OPN")
        {
            txOpenAdd.Text = "";
            loadOpenRepeater(iCs1, sCs2);
        }
        else if (sOpnOrCls == "CLS") 
        {
            txCloseAdd.Text = "";
            loadCloseRepeater(iCs1, sCs2);
        }
        // --------------------   
    }
    // =========================================================
    protected void delete_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        string sEmail = saArg[0].Trim();
        string sOpnOrCls = saArg[1].Trim();

        int iCs1 = 0;
        int iCs2 = 0;
        string sCs2 = "";
        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
        {
            iCs2 = 0;
        }
        else
        {
            sCs2 = iCs2.ToString();
        }

        if (sPageLib == "L")
        {
            wsLive.AddDelOpenCloseEmail(sfd.GetWsKey(), iCs1, sCs2, sEmail, sOpnOrCls, "DEL");
        }
        else
        {
            wsTest.AddDelOpenCloseEmail(sfd.GetWsKey(), iCs1, sCs2, sEmail, sOpnOrCls, "DEL");
        }
        
        // Update the Screen Tables
        if (sOpnOrCls == "OPN")
            loadOpenRepeater(iCs1, sCs2);
        else if (sOpnOrCls == "CLS")
            loadCloseRepeater(iCs1, sCs2);

    }
    // =========================================================
    protected void loadOpenCloseFlags(int iCs1, string sCs2, string familyMemberType)
    {
        string[] sAryOpnCls = new string[2];

        if (sPageLib == "L")
        {
            sAryOpnCls = wsLive.GetEmailOpenCloseFlags(sfd.GetWsKey(), iCs1, sCs2, familyMemberType);
        }
        else
        {
            sAryOpnCls = wsTest.GetEmailOpenCloseFlags(sfd.GetWsKey(), iCs1, sCs2, familyMemberType);
        }
        if (sAryOpnCls[0] == "Y")
            rblOpen.SelectedValue = "YES";
        else
            rblOpen.SelectedValue = "NO";
        if (sAryOpnCls[1] == "Y")
            rblClose.SelectedValue = "YES";
        else
            rblClose.SelectedValue = "NO";

    }
    // =========================================================
    protected void loadOpenRepeater(int iCs1, string sCs2)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetEmailAddresses(sfd.GetWsKey(), iCs1, sCs2, "OPN");
        }
        else
        {
            dataTable = wsTest.GetEmailAddresses(sfd.GetWsKey(), iCs1, sCs2, "OPN");
        }
        rpOpenDel.Visible = false;
        if (dataTable.Rows.Count > 0)
        {
            rpOpenDel.Visible = true;
            rpOpenDel.DataSource = dataTable;
            rpOpenDel.DataBind();
        }

    }
    // =========================================================
    protected void loadCloseRepeater(int iCs1, string sCs2)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetEmailAddresses(sfd.GetWsKey(), iCs1, sCs2, "CLS");
        }
        else
        {
            dataTable = wsTest.GetEmailAddresses(sfd.GetWsKey(), iCs1, sCs2, "CLS");
        }
        rpCloseDel.Visible = false;
        if (dataTable.Rows.Count > 0)
        {
            rpCloseDel.Visible = true;
            rpCloseDel.DataSource = dataTable;
            rpCloseDel.DataBind();
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1();
        hfCs1.Value = iCs1ToUse.ToString();
        ReloadPage(iCs1ToUse);
    }
    // =========================================================
    protected void ReloadPage(int cs1ToUse)
    {
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
            // lbSearchInstructions.Text = "Enter one or multiple values to search for a specific location, or switch to all locations for <b>a selected customer number</b>";
            lbSearchInstructions.Text = "Enter values to search for a specific location, or...";
            rbFamilyCs1.Visible = true;
            rbFamilyGlobal.Visible = true;

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
            lbSearchInstructions.Text = "Enter one or multiple values to search for a specific location";
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
    protected void LoadParentLocation(int cs1ToUse)
    {
        btToggleScreen.Text = "Switch To Manage INDIVIDUAL Locations";
        hfCs2.Value = "";

        int iCs2 = 0;
        string sCustName = "";

        if (sPageLib == "L")
        {
            sCustName = wsLive.GetCustName(sfd.GetWsKey(), cs1ToUse, iCs2);
        }
        else
        {
            sCustName = wsTest.GetCustName(sfd.GetWsKey(), cs1ToUse, iCs2);
        }

        lbEmailCust.Text = sCustName + ": Customer " + cs1ToUse.ToString();

        LoadEmailPanel(cs1ToUse, iCs2, "Parent");

        pnCs1Change.Visible = false;
        pnOneOrAllLocs.Visible = false;
        pnLocations.Visible = false;
        pnEmail.Visible = true;
    }
    // =========================================================
    protected void LoadLocationSearch(int cs1ToUse)
    {
        int iCs1ToUse = cs1ToUse;

        int iCs2 = 0;
        hfCs2.Value = "";

        if (iCs1ToUse > 0)
        {
            string sCustName = "";
            if (sPageLib == "L")
            {
                sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
            }
            else
            {
                sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
            }

            ReloadPage(iCs1ToUse);
            hfCs1.Value = iCs1ToUse.ToString();

            pnCs1Change.Visible = true;
            pnOneOrAllLocs.Visible = true;
            pnLocations.Visible = true;
            pnEmail.Visible = false;
        }
    }
    // =========================================================
    protected string GetFamilyMemberType(int cs1Passed)
    {
        string sFamilyMemberType = "";
        string sPassedCs1Type = "";

        if (hfCs2.Value == "")
        {
            if (sPageLib == "L")
                sPassedCs1Type = wsLive.GetCustType(sfd.GetWsKey(), cs1Passed);
            else
                sPassedCs1Type = wsTest.GetCustType(sfd.GetWsKey(), cs1Passed);
            if ((sPassedCs1Type == "LRG") || (sPassedCs1Type == "DLR"))
                sFamilyMemberType = "GRANDPARENT";
            else
                sFamilyMemberType = "PARENT";
        }
        else
        {
            sFamilyMemberType = "CHILD";
        }

        return sFamilyMemberType;
    }
    // =========================================================
    // =========================================================
}