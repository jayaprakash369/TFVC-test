using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Web.UI.Design.WebControls;

public partial class private_sc_DealerCustomers : MyPage
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

    //    iCs1ToUse = 2222;
        string sDlrNam = "";
        string sContract = "";
        string sEdate = "";
        string sDownloadEqp = "N";

        if (!IsPostBack)
        {
            ReloadLocPage(iCs1ToUse, sDlrNam, sContract, sEdate, sDownloadEqp);
        }
    }
    // =========================================================
    protected void ReloadLocPage(int cs1ToUse, string sDlrNam, string sContract, string sEdate, string sDownloadEqp)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt2 = new DataTable(sMethodName);
      //  DataTable dt3 = new DataTable(sMethodName);
        DataTable dt = new DataTable(sMethodName);

        int iCs2 = 0;
        string sCs2Used = "";     
        int eqpCs1 = 0;
        int eqpCs2 = 0;
        string agrNo = "";

        //      dt.Columns.Add(MakeColumn("CustNam"));
        dt.Columns.Add(MakeColumn("CustNum"));
        dt.Columns.Add(MakeColumn("CustLoc"));
        dt.Columns.Add(MakeColumn("DealerName"));
        dt.Columns.Add(MakeColumn("Contract"));
        dt.Columns.Add(MakeColumn("Type"));
        dt.Columns.Add(MakeColumn("Start Date"));
        dt.Columns.Add(MakeColumn("End Date"));
        dt.Columns.Add(MakeColumn("CustName"));
        dt.Columns.Add(MakeColumn("Part"));
        dt.Columns.Add(MakeColumn("PartDesc"));
        dt.Columns.Add(MakeColumn("Amount"));

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
            int iCs1ToUse = cs1ToUse;

     //      iCs1ToUse = 2222;
            if (sPageLib == "L")
            {
                  dt2 = wsLive.GetDlrAgrmnts(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sDlrNam, sContract, sEdate);
            }
            else
            {
                dt2 = wsTest.GetDlrAgrmnts(sfd.GetWsKey(), iCs1ToUse, iCs2, sCs2Used, sDlrNam, sContract, sEdate);
            }

            int iRow3 = 0;
            int iRowAdd = 0;
            foreach (DataRow row in dt2.Rows)
            {
                DataTable dt3 = new DataTable(sMethodName);
                iRow3 = 0;

                eqpCs1 = int.Parse(dt2.Rows[iRowAdd]["CustNum"].ToString());
                eqpCs2 = int.Parse(dt2.Rows[iRowAdd]["CustLoc"].ToString());
                agrNo = dt2.Rows[iRowAdd]["Contract"].ToString();
                if (sPageLib == "L")
                {
                     dt3 = wsLive.GetDlrAgrEqp(sfd.GetWsKey(), eqpCs1, agrNo);
                }
                else
                {
                    dt3 = wsTest.GetDlrAgrEqp(sfd.GetWsKey(), eqpCs1, agrNo);
                }
                foreach (DataRow row3 in dt3.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Contract"] = dt2.Rows[iRowAdd]["Contract"].ToString();
                    dr["Start Date"] = dt2.Rows[iRowAdd]["Start Date"].ToString();
                    dr["End Date"] = dt2.Rows[iRowAdd]["End Date"].ToString();
                    dr["Type"] = dt2.Rows[iRowAdd]["Type"].ToString();
                    dr["DealerName"] = dt2.Rows[iRowAdd]["DealerName"].ToString();
                    dr["Amount"] = dt3.Rows[iRow3]["Amount"].ToString();
                    dr["Part"] = dt3.Rows[iRow3]["Part"].ToString();
                    dr["PartDesc"] = dt3.Rows[iRow3]["PartDesc"].ToString();
                    dr["CustName"] = dt3.Rows[iRow3]["CustName"].ToString();
                    dr["CustNum"] = dt3.Rows[iRow3]["ECust"].ToString();
                    dr["CustLoc"] = dt3.Rows[iRow3]["ELoc"].ToString();

                    dt.Rows.Add(dr);
                    iRow3++;
                }
                iRowAdd++;              
            }
            // for download option

            if (sDownloadEqp == "Y")
            {
                try
                {
                    dt.TableName = "AgrEqp" + "_" + cs1ToUse.ToString() + "-" + sDlrNam.ToString();

                    if (dt.Rows.Count > 0)
                    {
                        DownloadHandler dh = new DownloadHandler();
                        string sCsv = dh.DataTableToExcelCsv(dt);
                        dh = null;

                        Response.ClearContent();
                        Response.ContentType = "application/ms-excel";
                        Response.AddHeader("content-disposition", "attachment; filename=AgrEqp_" + sDlrNam.ToString() + ".csv");
                        Response.Write(sCsv);
                    }
                }
                catch (Exception ex)
                {
                    string sReturn = ex.ToString();
                }
                // Show/hide edit columns based on user type
                //  if (User.IsInRole("Administrator") || User.IsInRole("Editor") || (User.IsInRole("EditorCustomer") && hfXrefEqpEditor.Value == "Y"))
                //   {
                //      gvEquipment.Columns[4].Visible = true;
                //   }
                //   else
                //        gvEquipment.Columns[4].Visible = false;

                Response.End();
            }
            else
            {

                // end of download option
                if (ViewState["vsDataTable_Loc"] == null)
                {
                    // Store the data in memory (so you don't have to keep getting it) 
                    ViewState["vsDataTable_Loc"] = dt;

                    if (dt.Rows.Count == 0)
                        lbError.Text = "No matching locations were found...";
                    else
                        lbError.Text = "";
                }
                else
                {
                    dt = (DataTable)ViewState["vsDataTable_Loc"];
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
                if (dt.Rows.Count > 0)
                    dt.DefaultView.Sort = sortExpression;

                gvLocations.DataSource = dt.DefaultView;
                gvLocations.DataBind();
                pnLocations.Visible = true;
            }
        }
    }

    // =========================================================
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        Button buttonControl = (Button)sender;
        string sButtonText = buttonControl.Text;
        sDownloadEqp = "";
        if (sButtonText == "Download")
            sDownloadEqp = "Y";

       int iCs1ToUse = GetPrimaryCs1();
       //   iCs1ToUse = 2222;

        txDlrNam.Text = txDlrNam.Text.ToUpper().Trim();
        string sDlrNam = txDlrNam.Text;
        string   sContract = txContract.Text;
        string  sEdate = txExpDate.Text;

       ReloadLocPage(iCs1ToUse, sDlrNam, sContract, sEdate, sDownloadEqp);

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
        DataTable dt = new DataTable(sMethodName);
        DataTable dt2 = new DataTable(sMethodName);
        DataTable dt3 = new DataTable(sMethodName);

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbError.Text = "";

           // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dt;

            if (dt.Rows.Count == 0)
                lbError.Text = "No matches were found...";
            else
                lbError.Text = "";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Loc"];
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
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gvLocations.DataSource = dt.DefaultView;
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
    protected string ServerSideVal_LocSearch()
    {
        string sResult = "";
        lbError.Text = "";

        txDlrNam.Text = txDlrNam.Text.ToUpper();
        txContract.Text = txContract.Text;
        txExpDate.Text = txExpDate.Text;

        try
        {
            if (lbError.Text == "")
            {
                if (txDlrNam.Text != "")
                {
                    if (txDlrNam.Text.Length > 40)
                    {
                        lbError.Text = "Dealer Name must be 40 characters or less";
                        txDlrNam.Text = txDlrNam.Text.Substring(0, 40);
                        txDlrNam.Focus();
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txContract.Text != "")
                {
                    if (txContract.Text.Length > 8)
                    {
                        lbError.Text = "Contract number must be 8 characters";
                        txContract.Text = txContract.Text.Substring(0, 15);
                        txContract.Focus();
                    }
                }
            }
          //  if (lbError.Text == "")
       //     {
       //         if (txCstNam.Text != "")
      //          {
       //             if (txCstNam.Text.Length > 40)
        //            {
        //                lbError.Text = "Customer Name must be 40 characters or less";
        //                txCstNam.Text = txCstNam.Text.Substring(0, 30);
        //                txCstNam.Focus();
         //           }
        //        }
       //     }
            if (lbError.Text == "")
            {
                if (txExpDate.Text != "")
                {
                    if (txExpDate.Text.Length > 8)
                    {
                        lbError.Text = "The Expired Date must be 8 characters and format: (YYYYMMDD)";
                        txExpDate.Text = txExpDate.Text.Substring(0, 2);
                        txExpDate.Focus();
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
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        ReloadLocPage(iPrimaryCs1, "", "", "", "N");
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
       
        int iChosenCs1 = iPrimaryCs1;

        if (sPageLib == "L")
        {
            sChosenCs1Type = wsLive.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
        }
        else
        {
            sChosenCs1Type = wsTest.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
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