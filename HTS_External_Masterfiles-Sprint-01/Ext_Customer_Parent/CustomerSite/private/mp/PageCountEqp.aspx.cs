using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_mp_PageCountEqp : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    //SourceForTicket sft = new SourceForTicket();
    SourceForCustomer sfc = new SourceForCustomer();

    string sCs1Family = "";
    string sChosenCs1Type = "";
    char[] cSplitterPipe = { '|' };
    char[] cSplitterTilda = { '~' };
    string[] saCs1All = new string[1];
    string[] saCs1Nam = new string[1];
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
        int iCs1ToUse = GetPrimaryCs1();

        if (!IsPostBack)
        {
            ReloadEqpPage(iCs1ToUse);
        }
    }
    // =========================================================
    protected void btEqpSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";

        // Check for hackers bypassing client validation
        if (Page.IsValid)
        {
            sValid = ServerSideVal_EqpSearch();

            // If all server side validation is also passed...
            if (sValid == "VALID")
            {
                ViewState["vsDataTable_Eqp"] = null;
                BindGrid_Eqp();
            }
        }
    }
    // =========================================================
    protected void lkSerial_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        int iUnt = 0;
        iUnt = Int32.Parse(sParms);

        Session["pageCountUnit"] = iUnt.ToString();
        Response.Redirect("~/private/mp/PageCountByDevice.aspx", false);
        Response.End();
    }
    // =========================================================
    protected string ServerSideVal_EqpSearch()
    {
        string sResult = "";

        txNam.Text = txNam.Text.ToUpper();
        txCit.Text = txCit.Text.ToUpper();
        txSta.Text = txSta.Text.ToUpper();
        txCrf.Text = txCrf.Text.ToUpper();
        txMod.Text = txMod.Text.ToUpper();
        txSer.Text = txSer.Text.ToUpper();
        txMrf.Text = txMrf.Text.ToUpper();

        try
        {
            int iNum = 0;
            if (txCs2.Text != "")
            {
                if (int.TryParse(txCs2.Text, out iNum) == false)
                {

                    if (vCus_EqpSearch.IsValid == true)
                    {
                        vCus_EqpSearch.ErrorMessage = "The location must be a number";
                        vCus_EqpSearch.IsValid = false;
                        txCs2.Focus();
                    }
                }
                else
                {
                    if (iNum > 999)
                    {
                        if (vCus_EqpSearch.IsValid == true)
                        {
                            vCus_EqpSearch.ErrorMessage = "Location entry must be 3 digits or less";
                            vCus_EqpSearch.IsValid = false;
                            txCs2.Text = txCs2.Text.Substring(0, 3);
                            txCs2.Focus();
                        }
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txNam.Text != "")
                {
                    if (txNam.Text.Length > 40)
                    {
                        vCus_EqpSearch.ErrorMessage = "Name must be 40 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txNam.Text = txNam.Text.Substring(0, 40);
                        txNam.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txCrf.Text != "")
                {
                    if (txCrf.Text.Length > 15)
                    {
                        vCus_EqpSearch.ErrorMessage = "Customer cross reference must be 15 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txCrf.Text = txCrf.Text.Substring(0, 15);
                        txCrf.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txCit.Text != "")
                {
                    if (txCit.Text.Length > 30)
                    {
                        vCus_EqpSearch.ErrorMessage = "City must be 30 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txCit.Text = txCit.Text.Substring(0, 30);
                        txCit.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txSta.Text != "")
                {
                    if (txSta.Text.Length > 2)
                    {
                        vCus_EqpSearch.ErrorMessage = "The state abbreviation must be 2 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txSta.Text = txSta.Text.Substring(0, 2);
                        txSta.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txMod.Text != "")
                {
                    if (txMod.Text.Length > 15)
                    {
                        vCus_EqpSearch.ErrorMessage = "The model must be 15 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txMod.Text = txMod.Text.Substring(0, 15);
                        txMod.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txSer.Text != "")
                {
                    if (txSer.Text.Length > 25)
                    {
                        vCus_EqpSearch.ErrorMessage = "The serial number must be 25 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txSer.Text = txSer.Text.Substring(0, 25);
                        txSer.Focus();
                    }
                }
            }
            if (vCus_EqpSearch.IsValid == true)
            {
                if (txMrf.Text != "")
                {
                    if (txMrf.Text.Length > 15)
                    {
                        vCus_EqpSearch.ErrorMessage = "Model cross reference must be 15 characters or less";
                        vCus_EqpSearch.IsValid = false;
                        txMrf.Text = txCrf.Text.Substring(0, 15);
                        txMrf.Focus();
                    }
                }
            }
            // ---------------------------------------
            if (vCus_EqpSearch.IsValid == true)
            {
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
                    if ((ddCs1Family.SelectedValue == "") &&
                            (txNam.Text == "") &&
                            (txCs2.Text == "") &&
                            (txCit.Text == "") &&
                            (txSta.Text == "") &&
                            (txCrf.Text == "") &&
                            (txMod.Text == "") &&
                            (txSer.Text == "") &&
                            (txMrf.Text == ""))
                    {
                        vCus_EqpSearch.ErrorMessage = "Please narrow your search by entering values in the search boxes";
                        vCus_EqpSearch.IsValid = false;
                        txNam.Focus();
                    }
                }
            }
            // -------------------
            if (vCus_EqpSearch.IsValid == true)
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "Unexpected error caught inside page count eqp search validation");
            vCus_EqpSearch.ErrorMessage = "A unexpected system error has occurred";
            vCus_EqpSearch.IsValid = false;
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
    // START EQUIPMENT GRID
    // =========================================================
    protected void gvPageIndexChanging_Eqp(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvEquipment.PageIndex = newPageIndex;
        BindGrid_Eqp();
    }
    // =========================================================
    protected void BindGrid_Eqp()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Eqp"] == null)
        {
            int iCs1ToUse = GetChosenCs1();
            int iCs2 = 0;

            string sNam = "";
            string sCit = "";
            string sSta = "";
            string sCrf = "";
            string sMod = "";
            string sSer = "";
            string sMrf = "";
            string sCs2 = "";

            if (txCs2.Text != "")
            {
                if (int.TryParse(txCs2.Text, out iCs2) == false)
                {
                    iCs2 = -1;
                }
                else 
                {
                    sCs2 = iCs2.ToString();
                }
            }

            sNam = txNam.Text;
            sCit = txCit.Text;
            sSta = txSta.Text;
            sCrf = txCrf.Text;
            sMod = txMod.Text;
            sSer = txSer.Text;
            sMrf = txMrf.Text;

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetAllEquipMP(sfd.GetWsKey(), iCs1ToUse.ToString(), sCs2, sNam, sCit, sSta, sCrf, sMod, sSer, sMrf);
            }
            else
            {
                dataTable = wsTest.GetAllEquipMP(sfd.GetWsKey(), iCs1ToUse.ToString(), sCs2, sNam, sCit, sSta, sCrf, sMod, sSer, sMrf);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Eqp"] = dataTable;
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Eqp"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Eqp == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Eqp + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Eqp + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression;

        gvEquipment.DataSource = dataTable.DefaultView;
        gvEquipment.DataBind();
        pnEquipment.Visible = true;
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
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Eqp"] == null)
            {
                ViewState["GridSortExpression_Eqp"] = "Cs2";
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
    protected void ReloadEqpPage(int cs1ToUse)
    {
        string sCs1 = "";
        string sNam = "";
        string sCs1Nam = "";

        lbCust.Text = "Customer Number";
        ddCs1Family.Visible = true;

        if (sPageLib == "L")
        {
            sChosenCs1Type = wsLive.GetCustType(sfd.GetWsKey(), cs1ToUse);
        }
        else
        {
            sChosenCs1Type = wsTest.GetCustType(sfd.GetWsKey(), cs1ToUse);
        }

        ViewState["vsDataTable_Eqp"] = null;
        // Don't show default screen for huge customers, make them select search values
        if ((sChosenCs1Type == "LRG") || (sChosenCs1Type == "DLR"))
        {
            if (sPageLib == "L")
            {
                sCs1Family = wsLive.GetCs1KidNames(sfd.GetWsKey(), cs1ToUse);
                saCs1All = sCs1Family.Split(cSplitterTilda);
            }
            else
            {
                sCs1Family = wsTest.GetCs1KidNames(sfd.GetWsKey(), cs1ToUse);
                saCs1All = sCs1Family.Split(cSplitterTilda);
            }
            int iItems = ddCs1Family.Items.Count;
            for (int i = 0; i < iItems; i++)
            {
                ddCs1Family.Items.RemoveAt(0);
            }
            for (int i = 0; i < saCs1All.Length; i++)
            {
                sCs1Nam = saCs1All[i];
                saCs1Nam = sCs1Nam.Split(cSplitterPipe);
                sCs1 = saCs1Nam[0];
                sNam = saCs1Nam[1];
                if (sNam.Length > 40)
                    sNam = sNam.Substring(0, 40);
                ddCs1Family.Items.Insert(i, new System.Web.UI.WebControls.ListItem(sCs1 + "  " + sNam, sCs1));
            }

            ddCs1Family.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value

            ddCs1Family.Enabled = true;
            pnEquipment.Visible = false;

        }
        else 
        {
            lbCust.Text = "";
            ddCs1Family.Visible = false;
            ViewState["vsDataTable_Eqp"] = null;
            BindGrid_Eqp();
            pnEquipment.Visible = true;
            txNam.Focus();
        }
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iPrimaryCs1 = iCs1User;

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (Session["adminCs1"] != null)
                {
                    if (int.TryParse(Session["adminCs1"].ToString(), out iPrimaryCs1) == false)
                        iPrimaryCs1 = 0;
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

}