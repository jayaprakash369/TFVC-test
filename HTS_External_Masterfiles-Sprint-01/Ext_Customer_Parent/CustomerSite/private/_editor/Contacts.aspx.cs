using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class private__editor_Contacts : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    //DataTable dataTable;

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadTable();
        }
    }
    // ================================================================
    protected void LoadTable()
    {
        try
        {
            ViewState["vsDataTable_Con"] = null;
            BindGrid_Con();
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        // ----------------------------------
    }
    // ================================================================
    protected void lkUpdate_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[6];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);

        int iKey = 0;
        int iOptOutPhone = 0;
        int iOptOutEmail = 0;

        int.TryParse(saArg[0].ToString(), out iKey);
        int.TryParse(saArg[1].ToString(), out iOptOutPhone);
        int.TryParse(saArg[2].ToString(), out iOptOutEmail);

        string sFirst = saArg[3].ToString().Trim();
        string sLast = saArg[4].ToString().Trim();
        string sUpdComment = saArg[5].ToString().Trim();

        // --------------------------------
        hfUpdKey.Value = iKey.ToString();
        lbUpdate.Text = sFirst + " " + sLast;
        txUpdComment.Text = sUpdComment;
        if (iOptOutPhone == 1)
            chBxPhone.Checked = true;
        else
            chBxPhone.Checked = false;
        if (iOptOutEmail == 1)
            chBxEmail.Checked = true;
        else
            chBxEmail.Checked = false;
        
        pnUpdate.Visible = true;
        txUpdComment.Focus();
    }
    // ================================================================
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        int iKey = 0;
        int iOptOutPhone = 0;
        int iOptOutEmail = 0;
        int.TryParse(hfUpdKey.Value, out iKey);
        if (chBxPhone.Checked == true)
            iOptOutPhone = 1;
        if (chBxEmail.Checked == true)
            iOptOutEmail = 1;

        string sComment = txUpdComment.Text.ToUpper().Trim();
        string sResult = "";

        if (sPageLib == "L")
        {
            sResult = wsLive.UpdPublicCustContact(sfd.GetWsKey(), iKey, sComment, iOptOutPhone, iOptOutEmail);
        }
        else
        {
            sResult = wsTest.UpdPublicCustContact(sfd.GetWsKey(), iKey, sComment, iOptOutPhone, iOptOutEmail);
        }

        pnUpdate.Visible = false;
        LoadTable();
    }

    // =========================================================
    protected void updateContacts_rp()
    {

        Label lbTemp = new Label();
        HiddenField hfTemp = new HiddenField();

        int iOptOutPhone = 0;
        int iOptOutEmail = 0;
        string sOptOutPhone = "";
        string sOptOutEmail = "";
        string sControlType = "";

        foreach (Control c1 in rpContacts.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfOptOutPhone")
                        {
                            int.TryParse(hfTemp.Value.ToString(), out iOptOutPhone);
                            if (iOptOutPhone == 1)
                                sOptOutPhone = "Y";
                        }
                        if (hfTemp.ID == "hfOptOutEmail")
                        {
                            int.TryParse(hfTemp.Value.ToString(), out iOptOutEmail);
                            if (iOptOutEmail == 1)
                                sOptOutEmail = "Y";
                        }
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPhone")
                        {
                            if (sOptOutPhone == "Y")
                                lbTemp.ForeColor = System.Drawing.Color.Crimson;
                        }
                        if (lbTemp.ID == "lbEmail")
                        {
                            if (sOptOutEmail == "Y")
                                lbTemp.ForeColor = System.Drawing.Color.Crimson;
                                // Clear workfields now that you're on the last important field
                                sOptOutPhone = "";
                                sOptOutEmail = "";

                        }
                    }
                // -------------------------------
                }
            }
        }
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        LoadTable();
    }

    #region User Grid Controls
    // =========================================================
    // START CONTACT GRID
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

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Con"] == null)
        {
            string sFirst = txFirst.Text.ToUpper();
            string sLast = txLast.Text.ToUpper();
            string sPhone = txPhone.Text.ToUpper();
            string sEmail = txEmail.Text.ToUpper();
            string sCity = txCity.Text.ToUpper();
            string sST = txST.Text.ToUpper();
            string sZip = txZip.Text.ToUpper();
            string sSource = txSource.Text.ToUpper();
            int iLastDate = 0;
            int.TryParse(txDate.Text, out iLastDate);

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetContacts(sfd.GetWsKey(), sPhone, sEmail, sFirst, sLast, sCity, sST, sZip, sSource, iLastDate);
            }
            else
            {
                dataTable = wsTest.GetContacts(sfd.GetWsKey(), sPhone, sEmail, sFirst, sLast, sCity, sST, sZip, sSource, iLastDate);
            }

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Con"] = dataTable;
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Con"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Con;
        if (gridSortDirection_Con == SortDirection.Ascending)
        {
            sortExpression_Con = gridSortExpression_Con + " ASC";
        }
        else
        {
            sortExpression_Con = gridSortExpression_Con + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression_Con;
        /*
        if (dataTable.Rows.Count > 0)
        {
            rpContacts.DataSource = dataTable;
            rpContacts.DataBind();
            updateContacts_rp();
            pnTable.Visible = true;
            lbMessage.Visible = false;
        }
        else
        {
            pnTable.Visible = false;
        }
        */
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
            // Initial state is Ascending in this program here... xxx
            if (ViewState["GridSortDirection_Con"] == null)
            {
                //ViewState["GridSortDirection_Con"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Con"] = SortDirection.Descending;
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
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Con"] == null)
            {
                ViewState["GridSortExpression_Con"] = "DateEntered"; // INITIAL SORT BY FIELD xxx
            }
            return (string)ViewState["GridSortExpression_Con"];
        }
        set
        {
            ViewState["GridSortExpression_Con"] = value;
        }
    }
    // =========================================================
    // END USER GRID 
    // =========================================================
    #endregion

    // ================================================================
    // ================================================================
}