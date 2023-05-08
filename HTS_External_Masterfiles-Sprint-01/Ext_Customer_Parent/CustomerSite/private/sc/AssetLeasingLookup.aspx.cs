using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_sc_AssetLeasingLookup : MyPage
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
    string sDownload = "";
    string sXref = "";
    string sAsset = "";
    string sSerial = "";
    string sLoc = "";
    string sDownloadTck = "N";
    int iLoc = 0;
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
        hfCs1.Value = iCs1ToUse.ToString();
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
        }

        lbError.Text = "";
        pnEqpSearch.Visible = true;
        pnLeasingDetails.Visible = false;
        pnAssetLog.Visible = false;
        pnTickets.Visible = false;
    }
    // =========================================================
    protected void lkTicket_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saParms = new string[1];
        char[] cSplitter = { '|' };
        saParms = sParms.Split(cSplitter);
        string sKey = sfd.GetWsKey();
        lbLeasing.Text = "";
        lbLog.Text = "";
        lbTckHist.Text = "";
        lbError.Text = "";

        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saParms[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saParms[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnLeasingDetails.Visible = false;
            pnAssetLog.Visible = false;
            pnTickets.Visible = false;

            pnDisplay.Visible = true;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTicketDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);
        }
    }

   // =========================================================
    protected void lkAssetPick_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable leasingdt = new DataTable(sMethodName);

        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[1];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        string sKey = sfd.GetWsKey();
        string sAsset = saArg[0].Trim();    
        string sAgr = saArg[1].Trim();
        int iRowIdx = 0;
        lbTckHist.Text = "";
        lbLeasing.Text = "";
        lbLog.Text = "";
        lbError.Text = "";

        if (sAsset != "")
        {
            leasingdt = wsTest.GetLeasingDetails(sKey, sAsset);
            if (leasingdt.Rows.Count == 0)
                lbError.Text = "No matching Leasing Details records were found...";
            else
            {
                txAcctName.Text = leasingdt.Rows[iRowIdx]["laDAccountName"].ToString();
                txAcctAddrs.Text = leasingdt.Rows[iRowIdx]["laDAccountAddress"].ToString();
                txAcctCity.Text = leasingdt.Rows[iRowIdx]["laDAccountCity"].ToString();
                txAcctSt.Text = leasingdt.Rows[iRowIdx]["laDAccountState"].ToString();
                txAcctZip.Text = leasingdt.Rows[iRowIdx]["laDAccountZipCode"].ToString();
             //   txAgrId.Text = leasingdt.Rows[iRowIdx]["laDID"].ToString();
                txAgrNo.Text = leasingdt.Rows[iRowIdx]["laDContractNumber"].ToString();
                txStrtDate.Text = leasingdt.Rows[iRowIdx]["laDCommencementDate"].ToString();
                txDays2Term.Text = leasingdt.Rows[iRowIdx]["laDDaysUntilTermDate"].ToString();
                txAgrBalance.Text = leasingdt.Rows[iRowIdx]["laDContractBalance"].ToString();
                txCstPayment.Text = leasingdt.Rows[iRowIdx]["laDTotalCustomerPayment"].ToString();
                txRentPayment.Text = leasingdt.Rows[iRowIdx]["laDRentPayment"].ToString();
                txPaid2Date.Text = leasingdt.Rows[iRowIdx]["laDPaidToDate"].ToString();
                txNxtInvDate.Text = leasingdt.Rows[iRowIdx]["laDNextInvoiceDate"].ToString();

                txLeaseType.Text = leasingdt.Rows[iRowIdx]["laDLeaseType"].ToString();
                txLeaseTerms.Text = leasingdt.Rows[iRowIdx]["laDLeaseTerms"].ToString();
                txBillingCycle.Text = leasingdt.Rows[iRowIdx]["laDBillingCycle"].ToString();
                txBillingType.Text = leasingdt.Rows[iRowIdx]["laDBillingType"].ToString();
                txPrgType.Text = leasingdt.Rows[iRowIdx]["laDProgramType"].ToString();

                pnEquipment.Visible = false;
                pnTickets.Visible = false;
                pnAssetLog.Visible = false;
             //   pnEqpSearch.Visible = false;
                pnLeasingDetails.Visible = true;
                lbLeasing.Text = "Asset: " + sAsset + " Details";
                lbError.Text = "";
                gvLeasingDtls.DataSource = leasingdt;
                gvLeasingDtls.DataBind();
            }
        }
    }
    // =========================================================
    // =========================================================
    protected void lkUnitPick_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        string sDat = "";
        string sTempAction = "";
        lbTckHist.Text = "";
        lbLeasing.Text = "";
        lbLog.Text = "";
        lbError.Text = "";

        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        string sCs2 = "";
        string sCs1 = hfCs1.Value;
        string sUnit = "";
        string sKey = sfd.GetWsKey();
        sUnit = saArg[0];
        sCs2 = saArg[1];

      // 

        // If you have data, 
        if (sUnit != "")
        {
           // dataTable.Columns.Add(MakeColumn("Entered"));
            dataTable = wsTest.GetAssetLog(sKey, sUnit);
            dataTable.Columns.Add("dispDate");
            if (dataTable.Rows.Count == 0)
                lbError.Text = "No matching Asset History records were found...";
            else
            {
                int iRowIdx = 0;
                foreach (DataRow row in dataTable.Rows)
                {

                    //  dataTable.Rows[0]["dispTicket"] = dataTable.Rows[0]["ADDCTR"].ToString().Trim() + "-" + dataTable.Rows[0]["ADDTCK"].ToString().Trim();

                    //   sTempAction = dataTable.Rows[0]["Action"].ToString().Trim();
                    //  dataTable.Rows[0]["Action"] = sTempAction.Trim();

                    sDat = dataTable.Rows[iRowIdx]["ADDDTE"].ToString().Trim();
                    if (sDat.Length == 8)
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        dataTable.Rows[iRowIdx]["DispDate"] = datTemp.Month + "/" + datTemp.Day.ToString() + "/" + datTemp.Year.ToString();
                    }
                    else
                        dataTable.Rows[iRowIdx]["DispDate"] = dataTable.Rows[iRowIdx]["ADDDTE"].ToString().Trim();
                    iRowIdx++;
                }
                dataTable.AcceptChanges();

                pnAssetLog.Visible = true;
                pnTickets.Visible = false;
                pnLeasingDetails.Visible = false;
                pnEquipment.Visible = false;
                lbLog.Text = "Asset History for unit: " + sUnit;
                lbError.Text = "";
                gvAssetLog.DataSource = dataTable;
                gvAssetLog.DataBind();
            }
        }
    }
    // =========================================================
    protected void lkSerialPick_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        string sCs2 = "";
        int iCs1 = 0;
        lbTckHist.Text = "";
        lbLeasing.Text = "";
        lbLog.Text = "";
        lbError.Text = "";

        string sKey = sfd.GetWsKey();

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = -1;
        sSerial = saArg[1];        
        sCs2 = saArg[0];

        // If you have data, 
        if (sSerial != "")
        {
            dataTable = wsTest.GetTicketHistory(sKey, iCs1, sCs2, sSerial);
            if (dataTable.Rows.Count == 0)
               lbError.Text = "No matching tickets were found...";
            else
            {
                pnEquipment.Visible = false;
                pnAssetLog.Visible = false;
                pnLeasingDetails.Visible = false;
                pnTickets.Visible = true;
                lbTckHist.Text = "Service Ticket History for serial: " + sSerial;
                lbError.Text = "";
               gvTickets.DataSource = dataTable;
               gvTickets.DataBind();
            }
        }
    }

  
    // ====end of copy code from service history ===============
    // =========================================================
    protected void btDownld_Click(object sender, EventArgs e) 
    {
        Button buttonControl = (Button)sender;
        string sButtonText = buttonControl.Text;
        sDownload = "";
        if (sButtonText == "Download")
            sDownload = "Y";

        txXref.Text = txXref.Text.ToUpper().Trim();
        txSer.Text = txSer.Text.ToUpper().Trim();
        txAsset.Text = txAsset.Text.ToUpper().Trim();

        int iCs1 = 0;
        int iCs2 = 0;

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(hfCs2.Value, out iCs2) == false)
            iCs2 = 0;

        string sXref = txXref.Text.Trim();
        string sSer = txSer.Text.Trim();
        string sAsset = txAsset.Text.Trim();

        // If you have data, 
        if (iCs1 > 0)
        {
            ReloadEqpPage(iCs1, iCs2, sXref, sSer, sAsset, sDownload);
        }
    }
    // =========================================================
    protected void btEqpSearch_Click(object sender, EventArgs e)
    {
        Button buttonControl = (Button)sender;
        string sButtonText = buttonControl.Text;
        sDownload = "";
        if (sButtonText == "Download")
            sDownload = "Y";

        txXref.Text = txXref.Text.ToUpper().Trim();
        txSer.Text = txSer.Text.ToUpper().Trim();
        txAsset.Text = txAsset.Text.ToUpper().Trim();

        int iCs1 = 0;
        int iCs2 = 0;

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;
        if (int.TryParse(txLoc.Text, out iCs2) == false)
            iCs2 = -1;
        string sSer = txSer.Text.Trim();
        string sXref = txXref.Text.Trim();
        string sAsset = txAsset.Text.Trim();
      
        // If you have data, 
        if (iCs1 > 0)
        {
            ReloadEqpPage(iCs1, iCs2, sXref, sSer, sAsset, sDownload);            
        }
    }

    // =========================================================
    // START LOCATION GRID
  
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
            string sXref = txXref.Text.Trim();
            string sSer = txSer.Text.Trim();
            string sAsset = txAsset.Text.Trim();
            string sCs2 = txLoc.Text;
            int cs1 = int.Parse(hfCs1.Value);

            GetCs1Cs2();

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetEqpAsset(sfd.GetWsKey(), cs1, sCs2, sXref, sSer, sAsset);
                
            }
            else
            {
                dataTable = wsTest.GetEqpAsset(sfd.GetWsKey(), cs1, sCs2, sXref, sSer, sAsset);
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
    protected void ReloadEqpPage(int cs1, int cs2, string xref, string ser, string asset, string sDownload)
    {
        DataTable dt = new DataTable();
        string sCs2 = "";
        if (cs2 != -1)
            sCs2 = cs2.ToString();

        pnEqpSearch.Visible = true;
        pnEquipment.Visible = true;
        pnTickets.Visible = false;

        pnCs1Header.Controls.Add(sfc.GetCustDataTable(cs1, cs2, "", "", ""));
        lbEquipment.Text = "Equipment Records";

        if (sPageLib == "L")
        {
           dt = wsLive.GetEqpAsset(sfd.GetWsKey(), cs1, sCs2, xref, ser, asset);
        }
        else
        {
           dt = wsTest.GetEqpAsset(sfd.GetWsKey(), cs1, sCs2, xref, ser, asset);
        }

        gvEquipment.DataSource = dt;
        gvEquipment.DataBind();

        if (sDownload == "Y")
        {
            try
            {
                if (sPageLib == "L")
                {
             //      dt = wsLive.GetEqpAsstDownload(sfd.GetWsKey(), cs1, cs2, xref, ser, asset);
                }
                else
                {
                   dt = wsTest.GetEqpAssetDownload(sfd.GetWsKey(), cs1, sCs2, xref, ser, asset);
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
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1();
        ReloadPage(iCs1ToUse);
      //  pnUpdate.Visible = false;
    }
    // =========================================================
    protected void ReloadPage(int cs1ToUse)
    {

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

         

           /// pnLocations.Visible = false;
        }
        else
        {
          //  lbCust.Text = "";
         //   ddCs1Family.Visible = false;
          //  lbAddress.Text = "Address";
         //   txAdr.Visible = true;

          //  BindGrid_Loc();
          //  pnLocations.Visible = true;
        }
        txLoc.Focus();
    }
    // =========================================================
    // ========================================================
    // =========================================================
    // =========================================================

    protected void gvLocations_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}