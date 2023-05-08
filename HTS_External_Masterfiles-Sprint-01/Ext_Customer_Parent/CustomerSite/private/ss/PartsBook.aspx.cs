using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_ss_PartsBook : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SelfService_LIVE.SelfServiceMenuSoapClient wsLiveSs = new SelfService_LIVE.SelfServiceMenuSoapClient();
    SelfService_DEV.SelfServiceMenuSoapClient wsTestSs = new SelfService_DEV.SelfServiceMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

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
        lbError.Text = "";
        lbError.Visible = false;
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        if (!IsPostBack)
        {
            setProductCodes();
            setDrives();
            getPcSubtypes();
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
    }
    // =========================================================
    protected void setProductCodes()
    {
        ddPrd.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        ddPrd.Items.Insert(1, new System.Web.UI.WebControls.ListItem("BTOS", "BTOS"));
        ddPrd.Items.Insert(2, new System.Web.UI.WebControls.ListItem("COMP", "COMP"));
        ddPrd.Items.Insert(3, new System.Web.UI.WebControls.ListItem("DRIVE", "DRIVE"));
        ddPrd.Items.Insert(4, new System.Web.UI.WebControls.ListItem("IBM", "IBM"));
        ddPrd.Items.Insert(5, new System.Web.UI.WebControls.ListItem("MISC", "MISC"));
        ddPrd.Items.Insert(6, new System.Web.UI.WebControls.ListItem("MODEMS", "MODEMS"));
        ddPrd.Items.Insert(7, new System.Web.UI.WebControls.ListItem("OMR", "OMR"));
        ddPrd.Items.Insert(8, new System.Web.UI.WebControls.ListItem("PC", "PC"));
        ddPrd.Items.Insert(9, new System.Web.UI.WebControls.ListItem("PTR", "PTR"));
        ddPrd.Items.Insert(10, new System.Web.UI.WebControls.ListItem("TERM", "TERM"));
        ddPrd.Items.Insert(11, new System.Web.UI.WebControls.ListItem("UNIX", "UNIX"));
        ddPrd.Items.Insert(12, new System.Web.UI.WebControls.ListItem("UPS", "UPS"));
        ddPrd.Items.Insert(13, new System.Web.UI.WebControls.ListItem("XE", "XE"));
    }
    // =========================================================
    protected void setDrives()
    {
        ddDrv.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        ddDrv.Items.Insert(1, new System.Web.UI.WebControls.ListItem("CDROM Drives", "DCDR"));
        ddDrv.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Drive Power Supplies", "DP/S"));
        ddDrv.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Floppy Drives", "DFD"));
        ddDrv.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Hard Drives", "DDRV"));
        ddDrv.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Misc Drive Parts", "DMSC"));
        ddDrv.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Misc PC Drive Parts", "XMAC"));
        ddDrv.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Optical Drives", "DOPL"));
        ddDrv.Items.Insert(8, new System.Web.UI.WebControls.ListItem("PC CDROM Drives", "XCDR"));
        ddDrv.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Tape Drives", "DTAP"));
    }

    // =========================================================
    protected void getPcSubtypes()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        if (sPageLib == "L")
        {
            dataTable = wsLiveSs.GetSsPcSubtypes(sfd.GetWsKey());
        }
        else
        {
            dataTable = wsTestSs.GetSsPcSubtypes(sfd.GetWsKey());
        }
        if (dataTable.Rows.Count > 0)
        {
            ddPc.DataSource = dataTable;
            ddPc.DataTextField = "SubTypeDesc";
            ddPc.DataValueField = "SubTypeCode";
            ddPc.DataBind();
            ddPc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
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
        string sResult = ServerSideVal_Parts();
        if (sResult == "VALID") 
        {
            ViewState["vsDataTable_Eqp"] = null;
            BindGrid_Eqp();
        }
    }
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
            string sPrt = txPrt.Text.Trim().ToUpper();
            string sDsc = txDsc.Text.Trim().ToUpper();
            string sSty = txSty.Text.Trim().ToUpper();
            string sMfr = txMfr.Text.Trim().ToUpper();
            string sPrd = ddPrd.SelectedValue;
            string sDrv = ddDrv.SelectedValue;
            string sPc = ddPc.SelectedValue;

            if (sPageLib == "L")
            {
                dataTable = wsLiveSs.GetSsParts(sfd.GetWsKey(), sPrt, sDsc, sSty, sMfr, sPrd, sDrv, sPc);
            }
            else
            {
                dataTable = wsTestSs.GetSsParts(sfd.GetWsKey(), sPrt, sDsc, sSty, sMfr, sPrd, sDrv, sPc);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dataTable;

            if (dataTable.Rows.Count == 0) 
            {
                lbError.Text = "No matching parts were found...";
                lbError.Visible = true;
            }
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
        //gvEquipment.PageSize = 500;
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
    protected string ServerSideVal_Parts()
    {
        string sResult = "";

        try
        {
            if (vCus_Parts.IsValid == true)
            {
                if ((txPrt.Text != "") && (txPrt.Text.Length < 2))
                {
                    vCus_Parts.ErrorMessage = "The part entry must be at least 2 characters";
                    vCus_Parts.IsValid = false;
                    txPrt.Focus();
                }
            }
            if (vCus_Parts.IsValid == true)
            {
                if ((txDsc.Text != "") && (txDsc.Text.Length < 2))
                {
                    vCus_Parts.ErrorMessage = "The part description entry must be at least 2 characters";
                    vCus_Parts.IsValid = false;
                    txDsc.Focus();
                }
            }
            if (vCus_Parts.IsValid == true)
            {
                if ((txMfr.Text != "") && (txMfr.Text.Length < 2))
                {
                    vCus_Parts.ErrorMessage = "The manufacturer part name entry must be at least 2 characters";
                    vCus_Parts.IsValid = false;
                    txSty.Focus();
                }
            }
            if (vCus_Parts.IsValid == true)
            {
                if ((txSty.Text != "") && (txSty.Text.Length < 2))
                {
                    vCus_Parts.ErrorMessage = "The style name entry must be at least 2 characters";
                    vCus_Parts.IsValid = false;
                    txSty.Focus();
                }
            }
            if (vCus_Parts.IsValid == true)
            {
                if ((txPrt.Text == "") && 
                    (txDsc.Text == "") && 
                    (txMfr.Text == "") && 
                    (txSty.Text == "") && 
                    (ddPrd.SelectedValue == "") && 
                    (ddDrv.SelectedValue == "") && 
                    (ddPc.SelectedValue == ""))
                {
                    vCus_Parts.ErrorMessage = "Please enter search values to continue.";
                    vCus_Parts.IsValid = false;
                    txPrt.Focus();
                }
            }
            // -------------------
            if (vCus_Parts.IsValid == true)
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            vCus_Parts.ErrorMessage = "A unexpected system error has occurred";
            vCus_Parts.IsValid = false;
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
    // =========================================================
}