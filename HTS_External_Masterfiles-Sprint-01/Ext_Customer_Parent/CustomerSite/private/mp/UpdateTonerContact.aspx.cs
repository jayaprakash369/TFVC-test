using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_mp_UpdateTonerContact : MyPage
{
    ManagedPrint_LIVE.ManagedPrintMenuSoapClient wsLiveMps = new ManagedPrint_LIVE.ManagedPrintMenuSoapClient();
    ManagedPrint_DEV.ManagedPrintMenuSoapClient wsTestMps = new ManagedPrint_DEV.ManagedPrintMenuSoapClient();

//    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
//    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
//    SourceForDefaults sfd = new SourceForDefaults();

    int iPrimaryCs1 = 0;
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int.TryParse(Profile.LoginCs1.ToString(), out iPrimaryCs1);
        if (Session["adminCs1"] != null)
            int.TryParse(Session["adminCs1"].ToString(), out iPrimaryCs1);
        BindGrid_Con();
    }
    // =========================================================
    // START LOCATION GRID
    // =========================================================
    protected void gvPageIndexChanging_Con(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvContacts.PageIndex = newPageIndex;
        BindGrid_Con();
    }
    // =========================================================
    protected void BindGrid_Con()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        string sSearchLoc = txSearchLoc.Text.Trim();
        string sSearchFxa = txSearchFxa.Text.Trim();
        string sSearchSer = txSearchSer.Text.Trim();
        string sSearchCon = txSearchCon.Text.Trim();
        string sSearchEml = txSearchEml.Text.Trim();

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Con"] == null)
        {
            if (sDevTestLive == "LIVE")
            {
                dataTable = wsLiveMps.getTonerContacts(iPrimaryCs1, sSearchLoc, sSearchFxa, sSearchSer, sSearchCon, sSearchEml);
            }
            else
            {
                dataTable = wsTestMps.getTonerContacts(iPrimaryCs1, sSearchLoc, sSearchFxa, sSearchSer, sSearchCon, sSearchEml);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Con"] = dataTable;

        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Con"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Con == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Con + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Con + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression;

        gvContacts.DataSource = dataTable.DefaultView;
        gvContacts.DataBind();
    }
    // =========================================================
    protected void gvSorting_Con(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Con = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Con == e.SortExpression)
        {
            if (gridSortDirection_Con == SortDirection.Ascending)
                gridSortDirection_Con = SortDirection.Descending;
            else
                gridSortDirection_Con = SortDirection.Ascending;
        }
        else
            gridSortDirection_Con = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Con = sortExpression_Con;
        // Rebind the grid to its data source
        BindGrid_Con();
    }
    private SortDirection gridSortDirection_Con
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Con"] == null)
            {
                ViewState["GridSortDirection_Con"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Con"];
        }
        set
        {
            ViewState["GridSortDirection_Con"] = value;
        }
    }
    private string gridSortExpression_Con
    {
        get
        {
            // Initial sort expression is location
            if (ViewState["GridSortExpression_Con"] == null)
            {
                ViewState["GridSortExpression_Con"] = "Cs2";
            }
            return (string)ViewState["GridSortExpression_Con"];
        }
        set
        {
            ViewState["GridSortExpression_Con"] = value;
        }
    }
    // =========================================================
    // END CONTACT GRID
    // =========================================================
    protected void lkContact_Click(object sender, EventArgs e)
    {
        LinkButton lkControl = (LinkButton)sender;
        string sParms = lkControl.CommandArgument.ToString();
        string[] saArg = new string[1]; // it will break into 5
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        hfCs1.Value = saArg[0].Trim();
        hfCs2.Value = saArg[1].Trim();
        hfTyp.Value = saArg[2].Trim();
        hfCon.Value = saArg[3].Trim();
        hfUnt.Value = saArg[4].Trim();
        
/*
        int iCs1 = Int32.Parse(saArg[0]);
        int iCs2 = Int32.Parse(saArg[1]);
        string sTyp = saArg[2].Trim();
        string sCon = saArg[3].Trim();
        int iUnt = Int32.Parse(saArg[4]);
*/
       Label lbTitle = lkControl.NamingContainer.FindControl("lbTitle") as Label;
       HiddenField hfPhone = lkControl.NamingContainer.FindControl("hfPhone") as HiddenField;
       Label lbExt = lkControl.NamingContainer.FindControl("lbExt") as Label;
       Label lbEmail = lkControl.NamingContainer.FindControl("lbEmail") as Label;
       Label lbAsset = lkControl.NamingContainer.FindControl("lbAsset") as Label;
       Label lbSerial = lkControl.NamingContainer.FindControl("lbSerial") as Label;

       lbUpdKeys.Text = "Updating contact for customer " + hfCs1.Value + " at location " + hfCs2.Value;
       if (hfUnt.Value != "" && hfUnt.Value != "0") 
       {
           lbUpdKeys.Text += " for asset " + lbAsset.Text + " with serial " + lbSerial.Text;
       }

       txContact.Text = lkControl.Text.Trim();
       txTitle.Text = lbTitle.Text.Trim();
       hfPhone.Value = hfPhone.Value.Trim();
       if (hfPhone.Value != "" && hfPhone.Value.Length == 10) 
       {
           txPh1.Text = hfPhone.Value.Substring(0, 3);
           txPh2.Text = hfPhone.Value.Substring(3, 3);
           txPh3.Text = hfPhone.Value.Substring(6, 4);
       }
       txExt.Text = lbExt.Text.Trim();
       txEmail.Text = lbEmail.Text.Trim();
       pnUpdate.Visible = true;
    }
    // =========================================================
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        string sCon = txContact.Text.ToUpper().Trim();
        
        string sTit = txTitle.Text.ToUpper().Trim();
        string sExt = txExt.Text.ToUpper().Trim();
        string sEml = txEmail.Text.Trim();
        string sPhn = "";
        int iPh1 = 0;
        int iPh2 = 0;
        int iPh3 = 0;
        int iCs1 = 0;
        int iCs2 = 0;
        int iUnt = 0;

        int.TryParse(txPh1.Text.Trim(), out iPh1);
        int.TryParse(txPh2.Text.Trim(), out iPh2);
        int.TryParse(txPh3.Text.Trim(), out iPh3);
        int.TryParse(hfCs1.Value.Trim(), out iCs1);
        int.TryParse(hfCs2.Value.Trim(), out iCs2);
        int.TryParse(hfUnt.Value.Trim(), out iUnt);

        string sConOrig = hfCon.Value.Trim();
        string sTyp = hfTyp.Value.Trim();

        sPhn = iPh1.ToString("000") + iPh2.ToString("000") + iPh3.ToString("0000");
        if (sPhn == "0000000000")
            sPhn = "";

        int iExt = 0;
        if (int.TryParse(sExt, out iExt) == false)
            sExt = "";
        else
        {
            if (iExt == 0)
                sExt = "";
        }

        int iRowsAffected = 0;

        if (sDevTestLive == "LIVE")
        {
            iRowsAffected = wsLiveMps.updTonerContact(sCon, sTit, sPhn, sExt, sEml, iCs1, iCs2, sTyp, sConOrig, iUnt);
        }
        else 
        {
            iRowsAffected = wsTestMps.updTonerContact(sCon, sTit, sPhn, sExt, sEml, iCs1, iCs2, sTyp, sConOrig, iUnt);
        }

        if (iRowsAffected == -1) 
        {
            vCus_Contact.ErrorMessage = "Entry would have been a duplicate and was not processed.";
            vCus_Contact.IsValid = false;
        }
        else if (iRowsAffected == 0)
        {
            vCus_Contact.ErrorMessage = "An unknown error occurred and this update has failed.";
            vCus_Contact.IsValid = false;
        }
        else 
        {
            txContact.Text = "";
            txTitle.Text = "";
            txPh1.Text = "";
            txPh2.Text = "";
            txPh3.Text = "";
            txExt.Text = "";
            txEmail.Text = "";
            
            hfCs1.Value = "";
            hfCs2.Value = "";
            hfTyp.Value = "";
            hfCon.Value = "";
            hfUnt.Value = "";
            pnUpdate.Visible = false;

            ViewState["vsDataTable_Con"] = null;
            BindGrid_Con();
        }
    }
    // =========================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        pnUpdate.Visible = false;
        ViewState["vsDataTable_Con"] = null;
        BindGrid_Con();
    }
    // =========================================================
    // =========================================================
}