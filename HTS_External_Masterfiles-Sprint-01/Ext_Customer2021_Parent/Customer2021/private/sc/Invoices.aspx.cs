using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_sc_Invoices : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMsg.Text = "";
        lbMsg.Text = "";

        if (!IsPostBack)
        {
            int iCustomerNumber = 0;
            int iCustomerLocation = 0;
                    
            Get_UserPrimaryCustomerNumber();

            try
            {
    //        hfPrimaryCs1.Value = "125";                   
                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                            iCustomerNumber = -1;
                        if  (iCustomerNumber > 0 && iCustomerLocation > -1)
                        {
                            string sOr1Or2 = ws_Get_B1CustomerOracleIds(iCustomerNumber, iCustomerLocation);
                            string[] saOr1Or2 = sOr1Or2.Split('|');
                            if (saOr1Or2.Length > 1)
                            {
                                hfOracleParentId.Value = saOr1Or2[0];
                                hfOracleChildId.Value = saOr1Or2[1];
                            }
                    LoadDateRangeDropDownList();
                    Load_InvoiceDataTables();
                        }
          //      string sIsExcelVersionNeeded = "";
           //     LoadEquipmentDataTables(sIsExcelVersionNeeded);

            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {

            }
        }        
    }
    // ========================================================================
    #region myWebServiceCalls
  
   // ========================================================================
    protected DataTable ws_Get_B1Invoices_AgreementHeaderRecords(
        string customerNumber, 
        string invoiceNumber,
        string invoiceDate)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1Invoices_AgreementHeaderRecords";
            string sFieldList = "customerNumber|invoiceNumber|invoiceDate|x";
            string sValueList = customerNumber.ToString() + "|" + invoiceNumber.ToString() + "|" + invoiceDate.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    protected DataTable ws_Get_B1Invoices_TicketHeaderRecords(
        string customerNumber, 
        string invoiceNumber,
        string invoiceDate)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1Invoices_TicketHeaderRecords";
            string sFieldList = "customerNumber|invoiceNumber|invoiceDate|x";
            string sValueList = customerNumber.ToString() + "|" + invoiceNumber.ToString() + "|" + invoiceDate.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }
        return dt;
    }
    // ========================================================================
    protected DataTable ws_Get_B1Invoices_MiscellaneousHeaderRecords(
        string customerNumber,
        string invoiceNumber,
        string invoiceDate)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1Invoices_MiscellaneousHeaderRecords";
            string sFieldList = "customerNumber|invoiceNumber|invoiceDate|x";
            string sValueList = customerNumber.ToString() + "|" + invoiceNumber.ToString() + "|" + invoiceDate.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    protected DataTable ws_Get_B2InvoicesHeaderRecords(
        string customerNumber,
        string invoiceType,
        string invoiceNumber,
        string invoiceDate)
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B2InvoicesHeaderRecords";
            string sFieldList = "customerNumber|invoiceType|invoiceDate|x";
            string sValueList = customerNumber.ToString() + "|" + invoiceType + "|" + invoiceNumber + "|" + invoiceDate + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
     // ---------------------------------------------------------------------------------------
    protected string ws_Get_B1CustomerOracleIds(int customerNumber, int customerLocation)
    {
        string sOracleIds = "";

        if (customerNumber > 0 && customerLocation > -1)
        {
            string sJobName = "Get_B1CustomerOracleIds";
            string sFieldList = "customerNumber|customerLocation|x";
            string sValueList = customerNumber.ToString() + "|" + customerLocation.ToString() + "|x";

            sOracleIds = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sOracleIds;
    }
    // ========================================================================
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ----------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ----------------------------------------------------------------------------
    protected void Load_InvoiceDataTables()
    {
        DataTable dtB1Misc = new DataTable("");
        DataTable dtB1Tck = new DataTable("");
        DataTable dtB1Agr = new DataTable("");
        DataTable dtB2Tck = new DataTable("");
        DataTable dtB2Agr = new DataTable("");
        DataTable dtMergeAgr = new DataTable("Merge Agreements");
        DataTable dtMergeTck = new DataTable("Merge Tickets");

        pnAgreement.Visible = false;
        pnMiscellaneous.Visible = false;
        pnTicket.Visible = false;

        try
        {
            if (!String.IsNullOrEmpty(hfPrimaryCs1.Value))
            {
                int iCustomerNumber = 0;
                int iSearchInvoice = 0;
                string sSearchInvoice = "";
                string sSelectedDateRange = "";
                string sB2InvoiceTypeAgreement = "Agreement";
                string sB2InvoiceTypeTicket = "Ticket";
                string invoiceFound = "N";
                if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                    iCustomerNumber = -1;

                if (iCustomerNumber > 0)
                {
                    if (!String.IsNullOrEmpty(ddSearchDateRange.SelectedValue))
                    {
                        sSelectedDateRange = ddSearchDateRange.Text;
                    }
                    else
                        sSelectedDateRange = "60 Days";

                    if (!String.IsNullOrEmpty(txSearchInvoice.Text.Trim()))
                    {
                        if (int.TryParse(txSearchInvoice.Text.Trim(), out iSearchInvoice) == false)
                            iSearchInvoice = -1;
                        if (iSearchInvoice > 0)
                        {
                            sSearchInvoice = iSearchInvoice.ToString();
                            sSelectedDateRange = "none";
                        }
                    }
                    
                    Session.Timeout = 60000;
                    // Agreement invoices
                    dtB1Agr = ws_Get_B1Invoices_AgreementHeaderRecords(iCustomerNumber.ToString(), iSearchInvoice.ToString(), sSelectedDateRange);
                    dtB2Agr = ws_Get_B2InvoicesHeaderRecords(iCustomerNumber.ToString(), sB2InvoiceTypeAgreement, sSearchInvoice, sSelectedDateRange);
                    
                //    int iCount1 = dtB1Agr.Rows.Count;
     //               int iCount2 = dtB2Agr.Rows.Count;

                    dtMergeAgr = Merge_AgrInvoiceTables(dtB1Agr, dtB2Agr);
                    if (dtMergeAgr.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(sSearchInvoice))
                        {
                            invoiceFound = "Y";
                        }
                       rp_AgreementInvoiceSmall.DataSource = dtMergeAgr;
                       rp_AgreementInvoiceSmall.DataBind();

                        ViewState["vsDataTable_InvAgr"] = null;
                        BindGrid_InvAgr(dtMergeAgr);
                        pnAgreement.Visible = true;
                    }

                    if (invoiceFound == "N")
                    {
                        //    // Ticket invoices
                        dtB1Tck = ws_Get_B1Invoices_TicketHeaderRecords(iCustomerNumber.ToString(), iSearchInvoice.ToString(), sSelectedDateRange);
                        dtB2Tck = ws_Get_B2InvoicesHeaderRecords(iCustomerNumber.ToString(), sB2InvoiceTypeTicket, sSearchInvoice, sSelectedDateRange);
                        dtMergeTck = Merge_TckInvoiceTables(dtB1Tck, dtB2Tck);
                        if (dtMergeTck.Rows.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(txSearchInvoice.Text.Trim()))
                            {
                                invoiceFound = "Y";
                            }

                            rp_TicketInvoiceSmall.DataSource = dtMergeTck;
                            rp_TicketInvoiceSmall.DataBind();

                            ViewState["vsDataTable_InvTck"] = null;
                            BindGrid_InvTck(dtMergeTck);
                            pnTicket.Visible = true;
                        }
                    }
                    //    // Miscellaneous invoices
                    if (invoiceFound == "N")
                    {
                        dtB1Misc = ws_Get_B1Invoices_MiscellaneousHeaderRecords(iCustomerNumber.ToString(), sSearchInvoice, sSelectedDateRange);
                        if (dtB1Misc.Rows.Count > 0)
                        {
                            rp_MiscellaneousInvoicesSmall.DataSource = dtB1Misc;
                            rp_MiscellaneousInvoicesSmall.DataBind();

                            ViewState["vsDataTable_InvMisc"] = null;
                            BindGrid_InvMisc(dtB1Misc);
                            pnMiscellaneous.Visible = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }

    // ========================================================================
    protected DataTable Merge_TckInvoiceTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        int iTemp = 0;
        double dTemp = 0.0;
        double dTempAmount = 0.0;
        double dTempTax = 0.0;
        string oneChar = "";
        string parmType = "";
        
        dt.Columns.Add(MakeColumn("tckBillingDateDisplay"));
        dt.Columns.Add(MakeColumn("Invoice"));
        dt.Columns.Add(MakeColumn("tckAmountDisplay"));
        dt.Columns.Add(MakeColumn("tckTaxDisplay"));
        dt.Columns.Add(MakeColumn("tckReferenceDisplay"));
        dt.Columns.Add(MakeColumn("tckTypeDisplay"));
    //    dt.Columns.Add(MakeColumn("tckCallTypeDisplay"));
        dt.Columns.Add(MakeColumn("tckDescriptionDisplay"));
        dt.Columns.Add(MakeColumn("Source"));

        dt.Columns.Add(MakeColumn("tckBillingDateSort"));
        dt.Columns.Add(MakeColumn("tckTypeSort"));
        dt.Columns.Add(MakeColumn("tckInvoiceSort"));
        dt.Columns.Add(MakeColumn("tckAmountSort"));
        dt.Columns.Add(MakeColumn("tckTaxSort"));
        //    dt.Columns.Add(MakeColumn("tckCallTypeSort"));
        dt.Columns.Add(MakeColumn("tckReferenceSort"));
        dt.Columns.Add(MakeColumn("tckDescriptionSort"));
    //    dt.Columns.Add(MakeColumn("tckSource"));

        DataRow dr;
        int iRowIdx = 0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            
                   
            dt.Rows[iRowIdx]["tckBillingDateDisplay"] = row["invDat"].ToString();
            dt.Rows[iRowIdx]["tckBillingDateSort"] = row["tckDateSort"].ToString();

            dt.Rows[iRowIdx]["Invoice"] = row["invnum"].ToString();
            if (int.TryParse(row["INVNUM"].ToString().Trim(), out iTemp) == false)
                iTemp = 0;
            dt.Rows[iRowIdx]["tckInvoiceSort"] = iTemp.ToString("00000000");

            if (double.TryParse(row["invtx1"].ToString().Trim(), out dTempTax) == false)
                dTempTax = 0;
            if (double.TryParse(row["INVOAM"].ToString().Trim(), out dTempAmount) == false)
                dTempAmount = 0;
            dTemp = dTempAmount - dTempTax;
            dt.Rows[iRowIdx]["tckAmountSort"] = dTemp.ToString("000000000000.00");
            dt.Rows[iRowIdx]["tckAmountDisplay"] = dTemp.ToString("0.00");
            dt.Rows[iRowIdx]["tckTaxSort"] = dTempTax.ToString("000000000.00");
            dt.Rows[iRowIdx]["tckTaxDisplay"] = dTempTax.ToString("0.00");
            dt.Rows[iRowIdx]["tckDescriptionDisplay"] = row["INVDSR"].ToString();
            dt.Rows[iRowIdx]["tckDescriptionSort"] = row["INVDSR"].ToString();
            dt.Rows[iRowIdx]["tckTypeDisplay"] = row["SCONTYP"].ToString();
            dt.Rows[iRowIdx]["tckTypeSort"] = row["SCONTYP"].ToString();
        //    dt.Rows[iRowIdx]["tckCallTypeDisplay"] = row["SCLASS"].ToString();
       //     dt.Rows[iRowIdx]["tckCallTypeSort"] = row["SCLASS"].ToString();
            dt.Rows[iRowIdx]["tckReferenceDisplay"] = row["INVTCK"].ToString();
            dt.Rows[iRowIdx]["tckReferenceSort"] = row["INVTCK"].ToString();
            //       dt.Rows[iRowIdx]["tckTaxesSort"] = dTemp.ToString("0000.00");

              dt.Rows[iRowIdx]["Source"] = "1T";
      //      dt.Rows[iRowIdx]["tckSource"] = "1";
            iRowIdx++;
        }


        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            oneChar = row["AgreementType"].ToString();
            parmType = oneChar.Substring(0, 1);
           
            dt.Rows[iRowIdx]["tckBillingDateDisplay"] = row["InvoiceDate"].ToString();
            dt.Rows[iRowIdx]["tckBillingDateSort"] = row["InvDateSort"].ToString();

            if (int.TryParse(row["InvoiceNo"].ToString().Trim(), out iTemp) == false)
                iTemp = 0;
            dt.Rows[iRowIdx]["tckInvoiceSort"] = iTemp.ToString("000000000");
            dt.Rows[iRowIdx]["Invoice"] = row["InvoiceNo"].ToString();

            if (double.TryParse(row["InvoiceTax"].ToString().Trim(), out dTempTax) == false)
                dTempTax = 0;
            if (double.TryParse(row["InvoiceAmount"].ToString().Trim(), out dTempAmount) == false)
                dTempAmount = 0;
            dTemp = dTempAmount + dTempTax;
            dt.Rows[iRowIdx]["tckAmountSort"] = dTempAmount.ToString("000000000000.00");
            dt.Rows[iRowIdx]["tckAmountDisplay"] = dTempAmount.ToString("0.00");
            dt.Rows[iRowIdx]["tckTaxSort"] = dTempTax.ToString("00000000.00");
            dt.Rows[iRowIdx]["tckTaxDisplay"] = dTempTax.ToString("0.00");

            //        dt.Rows[iRowIdx]["tckTaxesSort"] = dTemp.ToString("0000.00");

            dt.Rows[iRowIdx]["tckDescriptionDisplay"] = row["InvoiceSource"].ToString();
            dt.Rows[iRowIdx]["tckDescriptionSort"] = row["InvoiceSource"].ToString();
            dt.Rows[iRowIdx]["tckTypeDisplay"] = row["AgreementType"].ToString();
            dt.Rows[iRowIdx]["tckTypeSort"] = row["AgreementType"].ToString(); 
       //    dt.Rows[iRowIdx]["tckCallTypeDisplay"] = "";
       //    dt.Rows[iRowIdx]["tckCallTypeSort"] = "";
            dt.Rows[iRowIdx]["tckReferenceDisplay"] = row["InvoiceJob"].ToString();
            dt.Rows[iRowIdx]["tckReferenceSort"] = row["InvoiceJob"].ToString();
            dt.Rows[iRowIdx]["Source"] = "2" + parmType;
        //    dt.Rows[iRowIdx]["tckSource"] = "2";
            iRowIdx++;
        }
        dt.AcceptChanges();
        return dt;  // merge Ticket tables
    }
    // ========================================================================
    // ========================================================================
    protected DataTable Merge_AgrInvoiceTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("agrInvoiceDateDisplay"));
        dt.Columns.Add(MakeColumn("Invoice"));
        dt.Columns.Add(MakeColumn("agrAmountDisplay"));
        dt.Columns.Add(MakeColumn("agrReferenceDisplay"));
        dt.Columns.Add(MakeColumn("agrTypeDisplay"));
        dt.Columns.Add(MakeColumn("agrDescriptionDisplay"));
        dt.Columns.Add(MakeColumn("Source"));

        dt.Columns.Add(MakeColumn("agrInvoiceDateSort"));
        dt.Columns.Add(MakeColumn("agrInvoiceSort"));
        dt.Columns.Add(MakeColumn("agrAmountSort"));
  //      dt.Columns.Add(MakeColumn("agrSource"));

        DataRow dr;
        int iRowIdx = 0;
        int iTemp = 0;
        double dTemp = 0.0;
        double dTempAmount = 0.0;
        double dTempTax = 0.0;

        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[iRowIdx]["agrInvoiceDateDisplay"] = row["invDate"].ToString();
            dt.Rows[iRowIdx]["agrInvoiceDateSort"] = row["agrDateSort"].ToString();

            dt.Rows[iRowIdx]["Invoice"] = row["invnum"].ToString();
            if (int.TryParse(row["INVNUM"].ToString().Trim(), out iTemp) == false)
                iTemp = 0;
            dt.Rows[iRowIdx]["agrInvoiceSort"] = iTemp.ToString("00000000");

            if (double.TryParse(row["INVOAM"].ToString().Trim(), out dTempAmount) == false)
                dTempAmount = 0;
            if (double.TryParse(row["invtx1"].ToString().Trim(), out dTempTax) == false)
                dTempTax = 0;
            dTemp = dTempAmount - dTempTax;
            dt.Rows[iRowIdx]["agrAmountSort"] = dTempAmount.ToString("000000000000.00");
            dt.Rows[iRowIdx]["agrAmountDisplay"] = dTempAmount.ToString("0.00");

            dt.Rows[iRowIdx]["agrDescriptionDisplay"] = row["INVDSR"].ToString();
            dt.Rows[iRowIdx]["agrTypeDisplay"] = row["SCONTYP"].ToString();
            dt.Rows[iRowIdx]["agrReferenceDisplay"] = row["INVJOB"].ToString();
          
            dt.Rows[iRowIdx]["Source"] = "1A";
   //         dt.Rows[iRowIdx]["agrSource"] = "1";
            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["agrInvoiceDateDisplay"] = row["InvoiceDate"].ToString();
            dt.Rows[iRowIdx]["agrInvoiceDateSort"] = row["InvoiceDate"].ToString();

            dt.Rows[iRowIdx]["Invoice"] = row["InvoiceNo"].ToString();
            if (int.TryParse(row["InvoiceNo"].ToString().Trim(), out iTemp) == false)
                iTemp = 0;
            dt.Rows[iRowIdx]["agrInvoiceSort"] = iTemp.ToString("00000000");

            if (double.TryParse(row["InvoiceAmount"].ToString().Trim(), out dTempAmount) == false)
                dTempAmount = 0;
            if (double.TryParse(row["InvoiceTax"].ToString().Trim(), out dTempTax) == false)
                dTempTax = 0;
            dTemp = dTempAmount + dTempTax;
            dt.Rows[iRowIdx]["agrAmountSort"] = dTemp.ToString("000000000000.00");
            dt.Rows[iRowIdx]["agrAmountDisplay"] = dTemp.ToString("0.00");

            
     //       dt.Rows[iRowIdx]["agrTaxesSort"] = dTemp.ToString("0000000.00");

            dt.Rows[iRowIdx]["agrDescriptionDisplay"] = row["InvoiceSource"].ToString();
            dt.Rows[iRowIdx]["agrTypeDisplay"] = row["AgreementType"].ToString();
            dt.Rows[iRowIdx]["agrReferenceDisplay"] = row["InvoiceJob"].ToString();
            dt.Rows[iRowIdx]["Source"] = "2A";
       //     dt.Rows[iRowIdx]["agrSource"] = "2";
            iRowIdx++;
        }
        dt.AcceptChanges();
        return dt;  // Merger Agreement tables
    }
    // ========================================================================

  // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 1)
                hfPrimaryCs1.Value = saPreNumTyp[1];

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0)
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
            }

            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;
            if (iCustomerNumber > 0)
                hfPrimaryCs1Type.Value = ws_Get_B1CustomerType(iCustomerNumber);
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    protected void BindGrid_InvAgr(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_InvAgr"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_InvAgr"] = dt;

            lbMsg.Text = "";
            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_InvAgr"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_InvAgr == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_InvAgr + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_InvAgr + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;   

        gv_AgreementInvoiceLarge.DataSource = dt.DefaultView;
        gv_AgreementInvoiceLarge.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_AgreementInvoiceSmall.DataSource = dt.DefaultView;
            rp_AgreementInvoiceSmall.DataBind();
        }
    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_InvAgr(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_AgreementInvoiceLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_InvAgr(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_InvAgr(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_InvAgr == SortDirection.Ascending)
                gridSortDirection_InvAgr = SortDirection.Descending;
            else
                gridSortDirection_InvAgr = SortDirection.Ascending;
        }
        else
            gridSortDirection_InvAgr = SortDirection.Ascending;
      
        // Save the new sort expression
        gridSortExpression_InvAgr = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_InvAgr(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_InvAgr
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_InvAgr"] == null)
            {
                ViewState["GridSortDirection_InvAgr"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_InvAgr"];
        }
        set
        {
            ViewState["GridSortDirection_InvAgr"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_InvAgr
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_InvAgr"] == null)
            {
                ViewState["GridSortExpression_InvAgr"] = "Invoice"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_InvAgr"];
        }
        set
        {
            ViewState["GridSortExpression_InvAgr"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    protected void BindGrid_InvTck(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_InvTck"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_InvTck"] = dt;

            lbMsg.Text = "";
            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_InvTck"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_InvTck == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_InvTck + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_InvTck + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_TicketInvoiceLarge.DataSource = dt.DefaultView;
        gv_TicketInvoiceLarge.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_TicketInvoiceSmall.DataSource = dt.DefaultView;
            rp_TicketInvoiceSmall.DataBind();
        }

    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_InvTck(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_TicketInvoiceLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_InvTck(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_InvTck(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_InvTck == SortDirection.Ascending)
                gridSortDirection_InvTck = SortDirection.Descending;
            else
                gridSortDirection_InvTck = SortDirection.Ascending;
        }
        else
            gridSortDirection_InvTck = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_InvTck = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_InvTck(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_InvTck
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_InvTck"] == null)
            {
                ViewState["GridSortDirection_InvTck"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_InvTck"];
        }
        set
        {
            ViewState["GridSortDirection_InvTck"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_InvTck
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_InvTck"] == null)
            {
                ViewState["GridSortExpression_InvTck"] = "Invoice"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_InvTck"];
        }
        set
        {
            ViewState["GridSortExpression_InvTck"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    protected void BindGrid_InvMisc(DataTable dt)
    {
        string sReloadRepeater = "";
        if (ViewState["vsDataTable_InvMisc"] == null)
        {
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_InvMisc"] = dt;

            lbMsg.Text = "";
            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching records were found...";
                lbMsg.Visible = true;
            }
            sReloadRepeater = "Y";
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_InvMisc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_InvMisc == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_InvMisc + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_InvMisc + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression;

        gv_MiscellaneousInvoiceLarge.DataSource = dt.DefaultView;
        gv_MiscellaneousInvoiceLarge.DataBind();
        if (sReloadRepeater == "Y")
        {
            rp_MiscellaneousInvoicesSmall.DataSource = dt.DefaultView;
            rp_MiscellaneousInvoicesSmall.DataBind();
        }
    }
    // ----------------------------------------------------------------------------
    protected void gvPageIndexChanging_InvMisc(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_MiscellaneousInvoiceLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_InvMisc(dt);
    }
    // ----------------------------------------------------------------------------
    protected void gvSorting_InvMisc(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_InvMisc == SortDirection.Ascending)
                gridSortDirection_InvMisc = SortDirection.Descending;
            else
                gridSortDirection_InvMisc = SortDirection.Ascending;
        }
        else
            gridSortDirection_InvMisc = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_InvMisc = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_InvMisc(dt);
    }
    // ----------------------------------------------------------------------------
    private SortDirection gridSortDirection_InvMisc
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_InvMisc"] == null)
            {
                ViewState["GridSortDirection_InvMisc"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Inv"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_InvMisc"];
        }
        set
        {
            ViewState["GridSortDirection_InvMisc"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    private string gridSortExpression_InvMisc
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            if (ViewState["GridSortExpression_InvMisc"] == null)
            {
                ViewState["GridSortExpression_InvMisc"] = "INVNUM"; // *** INITIAL SORT FIELD ***
            }
            return (string)ViewState["GridSortExpression_InvMisc"];
        }
        set
        {
            ViewState["GridSortExpression_InvMisc"] = value;
        }
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btSearchInvoiceSubmit_Click(object sender, EventArgs e)
    {
        int iInvoiceNumber = 0;

        if (!String.IsNullOrEmpty(txSearchInvoice.Text)) 
        {
            if (int.TryParse(txSearchInvoice.Text.Trim(), out iInvoiceNumber) == false)
                iInvoiceNumber = -1;
            if (iInvoiceNumber <= 0)
                lbMsg.Text = "Invoice entry must be an integer";
        }

        if (String.IsNullOrEmpty(lbMsg.Text)) // i.e. no validation errors...
        {
            Load_InvoiceDataTables();
        }
    }
    // ----------------------------------------------------------------------------
    protected void btSearchInvoiceClear_Click(object sender, EventArgs e)
    {
        ddSearchDateRange.SelectedIndex = 2;
        txSearchInvoice.Text = "";
        pnAgreement.Visible = false;
        pnMiscellaneous.Visible = false;
        pnTicket.Visible = false;
    }
    // ----------------------------------------------------------------------------
    protected void lkInvoice_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        saArg = sParms.Split('|');
        
        int iInvoiceNumber = 0;
        string sInvoiceSource = "";
        string sUrl = "";
        string sTemp = "";

        if (saArg.Length > 1)
        {
            if (int.TryParse(saArg[0], out iInvoiceNumber) == false)
                iInvoiceNumber = 0;
            sTemp = saArg[1];
            sInvoiceSource = sTemp.Substring(0,1);

            if ((sInvoiceSource == "1") || (sInvoiceSource == "4"))
            {
                if (sInvoiceSource == "4")
                    sTemp = "1M";
                 sUrl = "~/private/sc/InvoiceDetail.aspx" +
                 "?inv=" + iInvoiceNumber +
                 "&src=" + sTemp;
            }
            else if (sInvoiceSource == "2")
            {
                sUrl = "~/private/sc/InvoiceDetail.aspx" +
                "?inv=" + iInvoiceNumber +
                "&src=" + sTemp;               
            }
            Response.Redirect(sUrl, false);
        }
    }
    // ----------------------------------------------------------------------------------------------------
    protected void LoadDateRangeDropDownList()
    {
        string sDateName = "";
        // 1) clear any items from the drop down list (shouldn't be any) 
        for (int i = 0; i < ddSearchDateRange.Items.Count; i++)
        {
            ddSearchDateRange.Items.RemoveAt(0);
        }

        // 2) Load list values into drop down list
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0: { sDateName = "30 Days"; break; }
                case 1: { sDateName = "60 Days"; break; }
                case 2: { sDateName = "90 Days"; break; }
                case 3: { sDateName = "120 Days"; break; }
                case 4: { sDateName = "180 Days"; break; }
                case 5: { sDateName = "360 Days"; break; }
            }    
                             
            ddSearchDateRange.Items.Insert(i, new System.Web.UI.WebControls.ListItem(sDateName));
        }

        ddSearchDateRange.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        ddSearchDateRange.SelectedIndex = 2;
    }
    // ----------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}