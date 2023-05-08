using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;

public partial class private_sc_req_Equipment : MyPage
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
            //if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            if (PreviousPage != null)
            {
                hfPri.Value = PreviousPage.pp_Pri().ToString();
                hfCs1.Value = PreviousPage.pp_Cs1().ToString();
                hfCs2.Value = PreviousPage.pp_Cs2().ToString();
                hfPhone.Value = PreviousPage.pp_Phone().ToString();
                hfExtension.Value = PreviousPage.pp_Extension().ToString();
                hfContact.Value = PreviousPage.pp_Contact().ToString();
                hfEmail.Value = PreviousPage.pp_Email().ToString();
                hfCreator.Value = PreviousPage.pp_Creator().ToString();
                hfReqType.Value = PreviousPage.pp_ReqType().ToString();
                hfForcedQty.Value = PreviousPage.pp_ForcedQty().ToString();
                hfCommMethodType.Value = PreviousPage.pp_MethdType().ToString();
                hfCommMethodInfo.Value = PreviousPage.pp_MethdInfo().ToString();
                hfCommMethodPhoneExt.Value = PreviousPage.pp_MethodPhoneExt().ToString();

                pnCs1Header.Controls.Add(sfc.GetCustDataTable(PreviousPage.pp_Cs1(), PreviousPage.pp_Cs2(), hfContact.Value, hfPhone.Value, hfExtension.Value));
                LoadPanelEquipment();
            }
            else
            {
                Response.Redirect("~/private/sc/req/Location.aspx", false);
            }
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
            string sPrd = ddPrd.SelectedValue.ToString();
            string sMod = txMod.Text.Trim();
            string sDsc = txDsc.Text.Trim();
            string sSer = txSer.Text.Trim();
            string sFxa = txFxa.Text.Trim();
            string sAgn = txAgn.Text.Trim();

            int iCs1 = 0;
            int iCs2 = 0;
            int.TryParse(hfCs1.Value, out iCs1);
            int.TryParse(hfCs2.Value, out iCs2);

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetLocEqp(sfd.GetWsKey(), iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn);
            }
            else
            {
                dataTable = wsTest.GetLocEqp(sfd.GetWsKey(), iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dataTable;
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
        GetSelectedUnits_gv();
        int newPageIndex = e.NewPageIndex;
        gvEquipment.PageIndex = newPageIndex;
        BindGrid_Eqp();
        SetSelectedUnits_gv();
    }
    // =========================================================
    protected void gvSorting_Eqp(object sender, GridViewSortEventArgs e)
    {
        GetSelectedUnits_gv();

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
        SetSelectedUnits_gv();
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
    // START CLICK METHODS
    // =========================================================
    protected void btEquipment_Click(object sender, EventArgs e)
    {
        GetSelectedUnits_gv();

        if (hfUnitList.Value == "")
        {
            vCustom_Equipment.ErrorMessage = "Please choose a unit for the service request";
            vCustom_Equipment.IsValid = false;
        }
        else
        {
            // Load Session Variables to send to problem page 
            Session["reqPri"] = hfPri.Value.Trim();
            Session["reqCs1"] = hfCs1.Value.Trim();
            Session["reqCs2"] = hfCs2.Value.Trim();
            Session["reqPhone"] = hfPhone.Value.Trim();
            Session["reqExtension"] = hfExtension.Value.Trim();
            Session["reqContact"] = hfContact.Value.Trim();
            Session["reqEmail"] = hfEmail.Value.Trim();
            Session["reqCreator"] = hfCreator.Value.Trim();
            Session["reqReqType"] = hfReqType.Value.Trim();
            Session["reqForcedQty"] = hfForcedQty.Value.Trim();
            Session["reqUnitList"] = hfUnitList.Value.Trim();
            Session["reqMthdT"] = hfCommMethodType.Value.Trim();
            Session["reqMthdI"] = hfCommMethodInfo.Value.Trim();
            Session["reqMethodPhoneExt"] = hfCommMethodPhoneExt.Value.Trim();
            Session["reqSource"] = "Equipment";

            Response.Redirect("~/private/sc/req/Problem.aspx", false);
            //Server.Transfer("~/private/sc/req/Problem.aspx", false);
        }
    }
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    protected void GetSelectedUnits_gv()
    {
        CheckBox chkBox = new CheckBox();
        string sType = "";
        hfUnitList.Value = "";

        foreach (Control c1 in gvEquipment.Controls)
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
    protected void SetSelectedUnits_gv()
    {
        CheckBox chkBox = new CheckBox();
        string sType = "";

        foreach (Control c1 in gvEquipment.Controls)
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
                                        if (hfUnitList.Value.Contains(chkBox.Text.Trim()))
                                            chkBox.Checked = true;
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
    protected void btEqpSearch_Click(object sender, EventArgs e)
    {
        LoadPanelEquipment();
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
    protected void LoadPanelEquipment()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        txMod.Text = txMod.Text.ToUpper().Trim();
        txDsc.Text = txDsc.Text.ToUpper().Trim();
        txSer.Text = txSer.Text.ToUpper().Trim();
        txFxa.Text = txFxa.Text.ToUpper().Trim();
        txAgn.Text = txAgn.Text.ToUpper().Trim();

        string sPrd = ddPrd.SelectedValue.ToString();
        string sMod = txMod.Text.Trim();
        string sDsc = txDsc.Text.Trim();
        string sSer = txSer.Text.Trim();
        string sFxa = txFxa.Text.Trim();
        string sAgn = txAgn.Text.Trim();

        int iCs1 = 0;
        int iCs2 = 0;
        int.TryParse(hfCs1.Value, out iCs1);
        int.TryParse(hfCs2.Value, out iCs2);

        LoadProductCodes(iCs1, iCs2); // loading a page early, but consistent with equip contract

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetLocEqp(sfd.GetWsKey(), iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn);
        }
        else
        {
            dataTable = wsTest.GetLocEqp(sfd.GetWsKey(), iCs1, iCs2, sPrd, sMod, sSer, sDsc, sFxa, sAgn);
        }

        gvEquipment.DataSource = dataTable;
        gvEquipment.PageSize = 500;
        gvEquipment.DataBind();
    }
    // =========================================================
    // =========================================================
}

