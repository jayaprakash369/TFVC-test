using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_custAdmin_ContactInformation : MyPage
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
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            ReloadPage(iCs1ToUse);
        }
    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";
        lbError.Text = "";

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
    // =========================================================
    protected void lbName_Click(object sender, EventArgs e)  // new in progress
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split(cSplitter2);

        int iCs1 = Int32.Parse(saArg[0]);
        int iCs2 = Int32.Parse(saArg[1]);

        // If you have data, 
        if (iCs1 > 0)
        {
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dataTable = new DataTable(sMethodName);

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetCustBasics(sfd.GetWsKey(), iCs1, iCs2);
            }
            else
            {
                dataTable = wsTest.GetCustBasics(sfd.GetWsKey(), iCs1, iCs2);
            }
            if (dataTable.Rows.Count > 0) 
            { 
                lbCustomerName.Text = dataTable.Rows[0]["Name"].ToString().Trim();
                lbCs1.Text = dataTable.Rows[0]["Cs1"].ToString().Trim();
                lbCs2.Text = dataTable.Rows[0]["Cs2"].ToString().Trim();
                txContact.Text = dataTable.Rows[0]["Contact"].ToString().Trim();
                txPhone.Text = dataTable.Rows[0]["Phone"].ToString().Trim();
            }
            pnUpdate.Visible = true;
        }
    }
    // =========================================================
    // START: LOCATIONS GRID
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
            sCon = txCon.Text;
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
        string sortExpression_Loc = "";
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
    // END: LOCATIONS GRID
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
                if (txCon.Text != "")
                {
                    if (txNam.Text.Length > 30)
                    {
                        lbError.Text = "Contact name must be 30 characters or less";
                        txNam.Text = txNam.Text.Substring(0, 30);
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
                    if ((ddCs1Family.SelectedValue == "")  &&
                            (txNam.Text == "") &&
                            (txCon.Text == "") &&
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
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        int iCs1 = 0;
        int iCs2 = 0;
        if (int.TryParse(lbCs1.Text, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(lbCs2.Text, out iCs2) == false)
            iCs2 = 0;

        string sContact = txContact.Text.Trim().ToUpper();
        string sPhone = txPhone.Text.Trim();

        if (sContact.Length > 30) sContact = sContact.Substring(0, 30);
        if (sPhone.Length > 10) sPhone = sPhone.Substring(0, 10);
        
        if (sPageLib == "L")
        {
            wsLive.UpdateContactPhone(sfd.GetWsKey(), iCs1, iCs2, sContact, sPhone);
        }
        else
        {
            wsTest.UpdateContactPhone(sfd.GetWsKey(), iCs1, iCs2, sContact, sPhone);
        }
        
        lbCustomerName.Text = "";
        lbCs1.Text = "";
        lbCs2.Text = "";
        txContact.Text = "";
        txPhone.Text = "";
        pnUpdate.Visible = false;

        ViewState["vsDataTable_Loc"] = null;
        BindGrid_Loc();
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1(); 
        ReloadPage(iCs1ToUse);
        pnUpdate.Visible = false;
    }
    // =========================================================
    protected void ReloadPage(int cs1ToUse)
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
            for (int i = 0; i < saCs1All.Length; i++)
            {
                sNam = "";
                sCs1Nam = saCs1All[i];
                saCs1Nam = sCs1Nam.Split(cSplitter2);
                if (saCs1Nam.Length > 1)
                {
                    sCs1 = saCs1Nam[0];
                    sNam = saCs1Nam[1];
                    if (sNam.Length > 30)
                        sNam = sNam.Substring(0, 30);
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

            BindGrid_Loc();
            pnLocations.Visible = true;
        }
        txNam.Focus();
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
        int iChosenCs1 = GetPrimaryCs1();

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
    // =========================================================
}
