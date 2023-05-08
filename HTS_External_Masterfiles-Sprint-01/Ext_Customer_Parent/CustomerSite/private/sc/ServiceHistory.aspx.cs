using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_sc_ServiceHistory : MyPage
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
    string sDownloadTck = "";

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

        if (!IsPostBack)
        {
            hfCs1.Value = iCs1ToUse.ToString();
            ReloadPage(iCs1ToUse);
        }
    }
    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        ViewState["vsDataTable_Loc"] = null;
        ReloadPage(iPrimaryCs1);
    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";
        lbError.Text = "";
        lbError.Visible = false;

        txNam.Text = txNam.Text.ToUpper();
        txAdr.Text = txAdr.Text.ToUpper();
        txCit.Text = txCit.Text.ToUpper();
        txSta.Text = txSta.Text.ToUpper();
        txZip.Text = txZip.Text.ToUpper();
        txXrf.Text = txXrf.Text.ToUpper();

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
                sValid = ServerSideVal_LocSearch();

                // If all server side validation is also passed...
                if (sValid == "VALID")
                {
                    ViewState["vsDataTable_Loc"] = null;
                    BindGrid_Loc();
                    pnLocations.Visible = true;
                }
            }
        }
    }
    // =========================================================
    protected void AllLocations_Click(object sender, EventArgs e)
    {
        lbError.Text = "";
        lbError.Visible = false;
        int iPrimaryCs1 = GetPrimaryCs1();
        int iCs1ToUse = iPrimaryCs1;

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
            if (ddCs1Family.SelectedValue == "")
            {
                // Vern Says Let them search with all customers...
                //                vCusAllLoc.ErrorMessage = "A parent customer must be selected from the drop down list to report on that customer group";
                //                vCusAllLoc.IsValid = false;
            }
            else
            {
                if (int.TryParse(ddCs1Family.SelectedValue, out iCs1ToUse) == false)
                    iCs1ToUse = 0;
            }
        }

        hfCs1.Value = iCs1ToUse.ToString();

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
                pnConditions.Visible = true;
                pnConditions.DefaultButton = "btReport";

                int iIdx = 0;
                foreach (ListItem li in cbServiceType.Items)
                {
                    if (iIdx == 0)
                        li.Selected = true;
//                    else if (iIdx == 3)
//                        li.Selected = false;
                    else
                        li.Selected = true;
                    iIdx++;
                }

                string sCustName = "";
                if (sPageLib == "L")
                {
                    sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
                }
                else
                {
                    sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1ToUse, iCs2);
                }

                if (hfCs2.Value != "")
                {
                    lbConditions.Text = "Reporting on Customer " + iCs1ToUse.ToString() + " at Location " + hfCs2.Value + " (" + sCustName + ")";
                }
                else 
                {
                    lbConditions.Text = "Reporting on All Locations Under Customer " + iCs1ToUse.ToString() + " (" + sCustName + ")";
                }

                SetConditionDefaults();
            }
        }
    }
    // =========================================================
    protected void lsBxReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbError.Text = "";
        lbError.Visible = false;
        pnLocations.Visible = false;
        pnCalendars.Visible = false;
        pnCtrTck.Visible = false;
        pnCrossRef.Visible = false;
        pnTickets.Visible = false;
        // Clear to free memory
        ViewState["vsDataTable_Loc"] = null;
        ViewState["vsDataTable_Tck"] = null;

        if ((lsBxReportType.SelectedValue == "OpenRange") || (lsBxReportType.SelectedValue == "ClosedRange") || (lsBxReportType.SelectedValue == "ClosedModel"))
        {
            pnCalendars.Visible = true;
        }
        if (lsBxReportType.SelectedValue == "ByTicket")
        {
            pnCtrTck.Visible = true;
            txCtr.Focus();
        }
        if (lsBxReportType.SelectedValue == "ByXref")
        {
            pnCrossRef.Visible = true;
            txRef.Focus();
        }

    }
    // =========================================================
    protected string ServerSideVal_LocSearch()
    {
        string sResult = "";
        lbError.Text = "";
        lbError.Visible = false;

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
                    if (iNum >= 1000)
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
            if (lbError.Text != "")
                lbError.Visible = true;
        }
        // --------------------------------
        return sResult;
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
            hfCs1.Value = iCs1.ToString(); // my add for Admin/Lrg/Dlr
            hfCs2.Value = iCs2.ToString();
            pnCs1Change.Visible = false;
            pnOneOrAllLocs.Visible = false;
            pnLocations.Visible = false;
            pnConditions.Visible = true;

            int iIdx = 0;
            foreach (ListItem li in cbServiceType.Items)
            {
                if (iIdx == 0)
                    li.Selected = true;
//                else if (iIdx == 3)
//                    li.Selected = false;
                else
                    li.Selected = true; 
                iIdx++;
            }

            string sCustName = "";
            if (sPageLib == "L")
            {
                sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1, iCs2);
            }
            else
            {
                sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1, iCs2);
            }

            if (hfCs2.Value != "")
            {
                lbConditions.Text = "Reporting on Customer " + hfCs1.Value + " at Location " + hfCs2.Value + " (" + sCustName + ")"; ;
            }
            else
            {
                lbConditions.Text = "Reporting on All Locations Under Customer " + hfCs1.Value + " (" + sCustName + ")"; ;
            }

            SetConditionDefaults();
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
            lbError.Text = "";
            lbError.Visible = false;

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

        if (lbError.Text != "")
            lbError.Visible = true;
        
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
    // END LOCATION GRID / START TICKET GRID
    // =========================================================
    protected void lkTicket_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnConditions.Visible = false;
            pnLocations.Visible = false;
            pnTickets.Visible = false;

            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTicketDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);
        }
    }
    // =========================================================
    protected void gvPageIndexChanging_Tck(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvTickets.PageIndex = newPageIndex;
        sDownloadTck = "Y";
        BindGrid_Tck();
    }
   
    // =========================================================
    protected void BindGrid_Tck()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        DataTable dt = new DataTable(sMethodName);

        if (ViewState["vsDataTable_Tck"] == null)
        {
            lbError.Text = "";
            lbError.Visible = false;
            int iCtr = 0;
            int iTck = 0;
            string sRef = "";
            int iDateStart = 0;
            int iDateEnd = 0;
            string sOpn = "";
            string sOnsite = "";
            string sDepot = "";
            string sInstall = "";
            string sPM = "";
            string sMITS = "";
            string sMP = "";
            string sToner = "";
            string sSupplies = "";
            int iCs1 = 0;

            if (int.TryParse(hfCs1.Value, out iCs1) == false)
                iCs1 = 0;

            int iIdx = 0;
            foreach (ListItem li in cbServiceType.Items)
            {
                if (li.Selected == true)
                {
                    if (iIdx == 0)
                        sOnsite = "Y";                   
                    if (iIdx == 1)
                        sDepot = "Y";                   
                    if (iIdx == 2)
                        sMITS = "Y";
                    if (iIdx == 3)
                        sMP = "Y";
                    if (iIdx == 4)
                        sToner = "Y";
                    if (iIdx == 5)
                        sPM = "Y";
                    if (iIdx == 6)
                        sInstall = "Y";
                    if (iIdx == 7)
                        sSupplies = "Y";
                }
                iIdx++;
            }

            if ((lsBxReportType.SelectedValue == "OpenRange") || (lsBxReportType.SelectedValue == "ClosedRange"))
            {
                DateTime datTemp = new DateTime();
                if ((calStart.SelectedDate.Year > 1) && (calStart.SelectedDate.Year > 1))
                {

                    datTemp = calStart.SelectedDate;
                    if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateStart) == false)
                        iDateStart = 0;
                    datTemp = calEnd.SelectedDate;
                    if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateEnd) == false)
                        iDateEnd = 0;

                    if ((iDateStart == 0) || (iDateEnd == 0) || (iDateStart > iDateEnd))
                    {
                        iDateStart = 0;
                        iDateEnd = 0;
                    }
                }
            }

            if (lsBxReportType.SelectedValue == "Open") 
            {
                sOpn = "OPEN ONLY";
            }
            else if (lsBxReportType.SelectedValue == "OpenRange") 
            {
                sOpn = "OPENED";
            }
            else if (lsBxReportType.SelectedValue == "ClosedRange")
            {
                sOpn = "CLOSED";
            }
            else if (lsBxReportType.SelectedValue == "ByTicket")
            {
                if (int.TryParse(txCtr.Text, out iCtr) == false)
                    iCtr = 0;
                if (int.TryParse(txTck.Text, out iTck) == false)
                    iTck = 0;
            }
            else if (lsBxReportType.SelectedValue == "ByXref")
            {
                sRef = txRef.Text.Trim();
            }

            if (sPageLib == "L")
            {                
                     dataTable = wsLive.GetTicketHistory2(sfd.GetWsKey(), iCs1, hfCs2.Value, iCtr, iTck, sRef, iDateStart, iDateEnd, sOpn, sOnsite, sDepot, sInstall, sPM, sMITS, sMP, sToner, sSupplies);
                    if (sDownloadTck == "Y")
                    {
                        dt = wsLive.GetTicketDownload2(sfd.GetWsKey(), iCs1, hfCs2.Value, iCtr, iTck, sRef, iDateStart, iDateEnd, sOpn, sOnsite, sDepot, sInstall, sPM, sMITS, sMP, sToner, sSupplies);
                    }
            }
            else
            {
                    dataTable = wsTest.GetTicketHistory2(sfd.GetWsKey(), iCs1, hfCs2.Value, iCtr, iTck, sRef, iDateStart, iDateEnd, sOpn, sOnsite, sDepot, sInstall, sPM, sMITS, sMP, sToner, sSupplies);

                    if (sDownloadTck == "Y")
                    {
                        dt = wsTest.GetTicketDownload2(sfd.GetWsKey(), iCs1, hfCs2.Value, iCtr, iTck, sRef, iDateStart, iDateEnd, sOpn, sOnsite, sDepot, sInstall, sPM, sMITS, sMP, sToner, sSupplies);
                    }
            }
            if (sDownloadTck == "Y" && dt.Rows.Count > 0) 
            {
                try
                {
                    DownloadHandler dh = new DownloadHandler();
                    string sCsv = dh.DataTableToExcelCsv(dt);
                    dh = null;

                    Response.ClearContent();
                    Response.ContentType = "application/ms-excel";
                    Response.AddHeader("content-disposition", "attachment; filename=ServHist_" + iCs1.ToString() + "-" + hfCs2.Value + ".csv");
                    Response.Write(sCsv);
                }
                catch (Exception ex)
                {
                    string sReturn = ex.ToString();
                }
            }
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Tck"] = dataTable;

            if (dataTable.Rows.Count == 0)
                lbError.Text = "No matching tickets were found...";
            else
                lbError.Text = "";
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Tck"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Tck;
        if (gridSortDirection_Tck == SortDirection.Ascending)
        {
            sortExpression_Tck = gridSortExpression_Tck + " ASC";
        }
        else
        {
            sortExpression_Tck = gridSortExpression_Tck + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression_Tck;

        gvTickets.DataSource = dataTable.DefaultView;
        gvTickets.DataBind();
        if (hfCs1.Value != "89866")
        {
            gvTickets.Columns[10].Visible = false;
        }
        else
        {
            gvTickets.Columns[10].Visible = true;
        }
        pnTickets.Visible = true;
        pnModel.Visible = false;

        if (lbError.Text != "")
            lbError.Visible = true;

        if (sDownloadTck == "Y" && dt.Rows.Count > 0)
            Response.End();
    }

    // =========================================================
    protected void gvSorting_Tck(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Tck == SortDirection.Ascending)
                gridSortDirection_Tck = SortDirection.Descending;
            else
                gridSortDirection_Tck = SortDirection.Ascending;
        }
        else
            gridSortDirection_Tck = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Tck = sortExpression;
        // Rebind the grid to its data source
        sDownloadTck = "Y";
        BindGrid_Tck();
    }
    private SortDirection gridSortDirection_Tck
    {
        get
        {
            // Initial state is Ascending
            if (ViewState["GridSortDirection_Tck"] == null)
            {
                ViewState["GridSortDirection_Tck"] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Tck"];
        }
        set
        {
            ViewState["GridSortDirection_Tck"] = value;
        }
    }
    private string gridSortExpression_Tck
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_Tck"] == null)
            {
                ViewState["GridSortExpression_Tck"] = "alpCtrTck"; // was ModelXref
            }
            return (string)ViewState["GridSortExpression_Tck"];
        }
        set
        {
            ViewState["GridSortExpression_Tck"] = value;
        }
    }
    // =========================================================
    // END TICKET GRID
    // =========================================================
    // =========================================================
    // START CLOSED TICKET By MODEL SUMMARY GRID
    // =========================================================
    // =========================================================
    protected void BindGrid_TckModel()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        DataTable dt = new DataTable(sMethodName);

        lbError.Text = "";
        lbError.Visible = false;
        int iDateStart = 0;
        int iDateEnd = 0;
        int iCs1 = 0;

        if (int.TryParse(hfCs1.Value, out iCs1) == false)
            iCs1 = 0;

        DateTime datTemp = new DateTime();
        if ((calStart.SelectedDate.Year > 1) && (calStart.SelectedDate.Year > 1))
        {
            datTemp = calStart.SelectedDate;
            if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateStart) == false)
                iDateStart = 0;
            datTemp = calEnd.SelectedDate;
            if (int.TryParse(datTemp.ToString("yyyyMMdd"), out iDateEnd) == false)
                iDateEnd = 0;

            if ((iDateStart == 0) || (iDateEnd == 0) || (iDateStart > iDateEnd))
            {
                iDateStart = 0;
                iDateEnd = 0;
            }
        }

        //    sOpn = "CLOSED BY MODEL";

        if (sPageLib == "L")
        {
            dataTable = wsTest.GetClosedTicketSummaryByModel(sfd.GetWsKey(), iCs1, hfCs2.Value, iDateStart, iDateEnd);
            if (sDownloadTck == "Y")
            {
               dt = wsLive.GetClosedTicketByModelDownload(sfd.GetWsKey(), iCs1, hfCs2.Value, iDateStart, iDateEnd);
            }
        }
        else
        {
            dataTable = wsTest.GetClosedTicketSummaryByModel(sfd.GetWsKey(), iCs1, hfCs2.Value, iDateStart, iDateEnd);
            if (sDownloadTck == "Y")
            {
               dt = wsTest.GetClosedTicketByModelDownload(sfd.GetWsKey(), iCs1, hfCs2.Value, iDateStart, iDateEnd);
            }
        }
        if (sDownloadTck == "Y" && dt.Rows.Count > 0)
        {
            try
            {
                DownloadHandler dh = new DownloadHandler();
                string sCsv = dh.DataTableToExcelCsv(dt);
                dh = null;

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ServHist_" + iCs1.ToString() + "-" + hfCs2.Value + ".csv");
                Response.Write(sCsv);
            }
            catch (Exception ex)
            {
                string sReturn = ex.ToString();
            }
        }

        gvModels.DataSource = dataTable;
        gvModels.DataBind();

        pnModel.Visible = true;
        pnTickets.Visible = false;

        if (lbError.Text != "")
            lbError.Visible = true;

        if (sDownloadTck == "Y" && dt.Rows.Count > 0)
            Response.End();
    }

    // =========================================================
    // END CLOSED TICKET By MODEL SUMMARY GRID
    // =========================================================
    protected void btClearConditions_Click(object sender, EventArgs e)
    {
        SetConditionDefaults();
    }
    // =========================================================
    protected void SetConditionDefaults()
    {
        int iIdx = 0;
        pnCalendars.Visible = false;
        pnCtrTck.Visible = false;
        pnCrossRef.Visible = false;

        iIdx = 0;
        foreach (ListItem li in lsBxReportType.Items)
        {
            if (iIdx == 0)
                li.Selected = true;
            else
                li.Selected = false;
            iIdx++;
        }
        iIdx = 0;
        foreach (ListItem li in cbServiceType.Items)
        {
            if (iIdx == 0)
                li.Selected = true;
            else
                li.Selected = true; // was false
            iIdx++;
        }

        txCtr.Text = "";
        txTck.Text = "";
        txRef.Text = "";
        
        int iPrimaryCs1 = GetPrimaryCs1();
        if (iPrimaryCs1 == 13465)
        {
            calStart.SelectedDate = DateTime.Now.AddMonths(-12);
            calStart.VisibleDate = calStart.SelectedDate;
        }
        else 
        {
            calStart.SelectedDate = DateTime.Now;
        }
        calEnd.SelectedDate = DateTime.Now;

    }
    // =========================================================
    protected void lkStatus_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTimestampDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected void lkPartUse_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetPartUseDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }

    // =========================================================
    protected string ValDateRange(int iMonths)
    {
        string sResult = "";
        DateTime datStart = new DateTime();
        DateTime datEnd = new DateTime();
        DateTime datTemp = new DateTime();
//        TimeSpan ts = new TimeSpan();

        if ((calStart.SelectedDate.Year > 1) && (calStart.SelectedDate.Year > 1))
        {
            datStart = calStart.SelectedDate;
            datEnd = calEnd.SelectedDate;
            datTemp = DateTime.Now;
            datTemp = datTemp.AddMonths(-iMonths);
            datTemp = datTemp.AddDays(-1);
            if (datStart > datEnd)
                sResult = "Your dates are reversed";
            else if (datStart < datTemp) 
            {
                sResult = "Your start date was beyond the " + iMonths.ToString() + " month limit (start date moved to limit).";
                datTemp = datTemp.AddDays(1);
                calStart.SelectedDate = datTemp.Date;
            }
//            ts = datEnd - datStart;
        }
        return sResult;
    }
    // =========================================================
    protected void ValReportParms()
    {
        lbError.Text = "";
        lbError.Visible = false;

        int iCtr = 0;
        int iTck = 0;
        String sRange = "";
        
        // Date Range
        int iPrimaryCs1 = GetPrimaryCs1();
        // Expand History date range for certain customers
        if (iPrimaryCs1 == 89866 || iPrimaryCs1 == 108765)
            sRange = ValDateRange(18);
        else if (iPrimaryCs1 == 13465)
            sRange = ValDateRange(24);
        else
            sRange = ValDateRange(4);

        if (sRange != "")
            lbError.Text = sRange;

        if (lsBxReportType.SelectedValue == "ByTicket")
        {
            if (txCtr.Text == "") 
            {
                lbError.Text = "A center number is required...";
                txCtr.Focus();
            }
            else if (txTck.Text == "")
            {
                lbError.Text = "A ticket number is required...";
                txTck.Focus();
            }

            else if (int.TryParse(txCtr.Text, out iCtr) == false) 
            {
                lbError.Text = "The center must be a number...";
                txCtr.Focus();
            }
            else if (int.TryParse(txTck.Text, out iTck) == false)
            {
                lbError.Text = "The ticket must be a number...";
                txTck.Focus();
            }
        }
        if (lsBxReportType.SelectedValue == "ByXref")
        {
            if (txRef.Text == "") 
            {
                lbError.Text = "The cross reference is required";
                txRef.Focus();
            }
        }
        if (lbError.Text != "")
            lbError.Visible = true;
    }
    // =========================================================
    protected void btReport_Click(object sender, EventArgs e)
    {   // Do Server site validation here
        
        ValReportParms();
        // If OK, go ahead
        if (lbError.Text == "")
        {
            if ((lsBxReportType.SelectedValue == "ClosedModel"))
            {
                BindGrid_TckModel();
            }
            else
            {
                sDownloadTck = "N";
                ViewState["vsDataTable_Tck"] = null;
                BindGrid_Tck();
            }
        }
    }
    // =========================================================
    protected void btDownload_Click(object sender, EventArgs e)
    {   
        ValReportParms();
        // If OK, go ahead
        if (lbError.Text == "")
        {
           if ((lsBxReportType.SelectedValue == "ClosedModel"))
           {
               sDownloadTck = "Y";
               BindGrid_TckModel();
           }
           else
           {
               sDownloadTck = "Y";
               ViewState["vsDataTable_Tck"] = null;
               BindGrid_Tck();
           }
        }
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
            lbSearchInstructions.Text = "Enter one or multiple values to search for a specific location, or select \"All Locations\" for a selected customer number";

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
                sCs1Nam = saCs1All[i];
                saCs1Nam = sCs1Nam.Split(cSplitter2);

                sCs1 = "";
                sNam = "";

                if(saCs1Nam.Length > 0)
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

            pnLocations.Visible = false;

        }
        else
        {
            lbSearchInstructions.Text = "Enter one or multiple values to search for a specific location, or select \"All Locations\"";
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
    // =========================================================
}