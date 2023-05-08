using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_customerAdministration_mp_UpdateTonerContact : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    string sTemp = "";

    // -------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);


        if (!IsPostBack)
        {
            Get_UserPrimaryCustomerNumber();

            try
            {
                if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
                {
                    pnSearchCustomerFamily.Visible = true;
                    LoadFamilyMemberDropDownList();
                    // First pass, only large customers and dealers only get a list of customers to choose from (too big...)
                }
                else
                {
                    pnSearchCustomerFamily.Visible = false;
                    // First pass, only regular customers get the full list
                    Load_ContactDataTables();
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
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected string ws_Upd_B1TonerContact(
            string asset,
            string contactNameOld,
            string contactNameNew,
            string customerLocation,
            string customerNumber,
            string email,
            string extension,
            string phone,
            string title,
            string unit
        )
    {
        string sSuccessOrFailure = "";

        if (!String.IsNullOrEmpty(customerNumber) 
            && !String.IsNullOrEmpty(customerLocation) 
            && !String.IsNullOrEmpty(unit)
            )
        {
            string sJobName = "Upd_B1TonerContact";
            string sFieldList = "asset|contactNameOld|contactnameNew|customerLocation|customerNumber|email|extension|phone|title|unit|x";
            string sValueList = 
                asset.Trim() + "|" + 
                contactNameOld + "|" + 
                contactNameNew + "|" + 
                customerLocation + "|" + 
                customerNumber + "|" + 
                email + "|" + 
                extension + "|" + 
                phone + "|" + 
                title + "|" + 
                unit + "|x";

            sSuccessOrFailure = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }
        return sSuccessOrFailure;
    }
    // ========================================================================
    protected DataTable ws_Get_B1TonerContact(
        string customerNumber,
        string customerLocation,
        string contact,
        string unit
    )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1TonerContact";
            string sFieldList = "customerNumber|customerLocation|contact|unit|x";
            string sValueList = customerNumber + "|" + customerLocation + "|" + contact + "|" + unit + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }
        return dt;
    }
    // ========================================================================
    protected DataTable ws_Get_B1TonerContacts(
            string customerNumber, 
            string customerLocation,
            string asset, 
            string serial,
            string contact,
            string title,
            string email
        )
    {
        DataTable dt = new DataTable();

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1TonerContacts";
            string sFieldList = "customerNumber|customerLocation|asset|serial|contact|title|email|x";
            string sValueList = customerNumber + "|" + customerLocation + "|" + asset + "|" + serial + "|" + contact + "|" + title + "|" + email + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }
        return dt;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // -------------------------------------------------------------------------------------------------
    protected void LoadFamilyMemberDropDownList()
    {
        int iPrimaryCustomerNumber = 0;
        string sFamilyMemberList = "";

        if (int.TryParse(hfPrimaryCs1.Value, out iPrimaryCustomerNumber) == false)
            iPrimaryCustomerNumber = -1;
        if (iPrimaryCustomerNumber > 0)
        {


            // 1) clear any items from the drop down list (shouldn't be any) 
            for (int i = 0; i < ddSearchCustomerFamily.Items.Count; i++)
            {
                ddSearchCustomerFamily.Items.RemoveAt(0);
            }

            // 2) Get Family member name and number list
            sFamilyMemberList = ws_Get_B1CustomerFamilyMemberNameAndNumberList(iPrimaryCustomerNumber);
            string[] saFamilyMembers = sFamilyMemberList.Split('|');
            string[] saNamNum = { "", "" };

            // 3) Load list values into drop down list
            for (int i = 0; i < saFamilyMembers.Length; i++)
            {
                saNamNum = saFamilyMembers[i].Split('~');
                if (saNamNum.Length > 1)
                {
                    if (saNamNum[0].Length > 40)
                        saNamNum[0] = saNamNum[0].Substring(0, 40);
                }
                ddSearchCustomerFamily.Items.Insert(i, new System.Web.UI.WebControls.ListItem(saNamNum[1] + "  " + saNamNum[0], saNamNum[1]));
            }

            ddSearchCustomerFamily.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
        }

    }
    // -------------------------------------------------------------------------------------------------
    protected void Load_ContactDataTables()
    {
        DataTable dtB1 = new DataTable("B1");
        DataTable dtB2 = new DataTable("B2");
        DataTable dt = new DataTable("Default");

        string sSearchCustomerNumber = "";
        int iSelectedCustomerNumber = 0;

        if (hfPrimaryCs1Type.Value == "LRG" || hfPrimaryCs1Type.Value == "DLR")
        {
            if (!String.IsNullOrEmpty(ddSearchCustomerFamily.SelectedValue))
            {
                if (int.TryParse(ddSearchCustomerFamily.SelectedValue, out iSelectedCustomerNumber) == false)
                    iSelectedCustomerNumber = -1;
            }
        }

        if (iSelectedCustomerNumber > 0) // i.e. LRG cust picked one of their own sub cust numbers
            sSearchCustomerNumber = iSelectedCustomerNumber.ToString();
        else
            sSearchCustomerNumber = hfPrimaryCs1.Value;

        string sSearchContact = txSearchContact.Text.Trim().ToUpper().Trim();
        string sSearchLocation = txSearchLocation.Text.Trim();
        string sSearchSerial = txSearchSerial.Text.Trim().ToUpper().Trim();
        string sSearchEmail = txSearchEmail.Text.Trim().ToUpper().Trim();
        string sSearchTitle = ""; // txSearchTitle.Text.Trim().ToUpper().Trim();
        string sSearchAsset = txSearchAsset.Text.Trim().ToUpper().Trim();

        try
        {
            if (
                    (hfPrimaryCs1Type.Value != "LRG" && hfPrimaryCs1Type.Value != "DLR")
                    ||
                    ( // LRG/DLR must pick something!
                        (iSelectedCustomerNumber > 0)
                        || !String.IsNullOrEmpty(sSearchContact)
                        || !String.IsNullOrEmpty(sSearchLocation)
                        || !String.IsNullOrEmpty(sSearchSerial)
                        || !String.IsNullOrEmpty(sSearchEmail)
                    )
                )
            {
                dtB1 = ws_Get_B1TonerContacts(
                    sSearchCustomerNumber, 
                    sSearchLocation, 
                    sSearchAsset, 
                    sSearchSerial, 
                    sSearchContact, 
                    sSearchTitle, 
                    sSearchEmail
                    );

                // Merge
                dt = Merge_ContactTables(dtB1, dtB2);
            }
            rp_ContactSmall.DataSource = dt;
            rp_ContactSmall.DataBind();

            ViewState["vsDataTable_Con"] = null;
            BindGrid_Con(dt);

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected DataTable Merge_ContactTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("CustomerNumber"));
        dt.Columns.Add(MakeColumn("CustomerLocation"));
        dt.Columns.Add(MakeColumn("ContactType"));
        dt.Columns.Add(MakeColumn("ContactName"));
        dt.Columns.Add(MakeColumn("Unit"));
        dt.Columns.Add(MakeColumn("Title"));
        dt.Columns.Add(MakeColumn("Phone"));
        dt.Columns.Add(MakeColumn("Extension"));
        dt.Columns.Add(MakeColumn("Email"));
        dt.Columns.Add(MakeColumn("Asset"));
        dt.Columns.Add(MakeColumn("Serial"));

        dt.Columns.Add(MakeColumn("PhoneDisplay"));

        DataRow dr;
        int iRowIdx = 0;
        string sPhone = "";
        //int iTemp = 0;


        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["CustomerNumber"] = row["CNCS1"].ToString().Trim();
            dt.Rows[iRowIdx]["CustomerLocation"] = row["CNCS2"].ToString().Trim();
            dt.Rows[iRowIdx]["ContactType"] = row["CNCOD"].ToString().Trim();
            dt.Rows[iRowIdx]["ContactName"] = Fix_Case(row["CNNAM"].ToString()).Trim();
            dt.Rows[iRowIdx]["Unit"] = row["CUNIT"].ToString().Trim();
            dt.Rows[iRowIdx]["Title"] = Fix_Case(row["CNTIT"].ToString()).Trim();
            sPhone = row["CNPHN"].ToString().Trim();
            sPhone = Clean_PhoneEntry(sPhone);
            if (sPhone.Length == 10) 
            {
                dt.Rows[iRowIdx]["Phone"] = sPhone;
                dt.Rows[iRowIdx]["PhoneDisplay"] = FormatPhone1(sPhone);
            }
            
            dt.Rows[iRowIdx]["Extension"] = row["CNEXT"].ToString().Trim();
            dt.Rows[iRowIdx]["Email"] = Fix_Case(row["CNEM1"].ToString()).Trim();
            dt.Rows[iRowIdx]["Asset"] = row["CXREF"].ToString().Trim();
            dt.Rows[iRowIdx]["Serial"] = row["CSERL"].ToString().Trim();

            iRowIdx++;
        }

        foreach (DataRow row in dt2.Rows)
        {
            //dr = dt.NewRow();
            //dt.Rows.Add(dr);

            //dt.Rows[iRowIdx]["AgentId"] = "";
            //dt.Rows[iRowIdx]["Agreement"] = row["agreementId"].ToString().Trim();
            //dt.Rows[iRowIdx]["Model"] = Fix_Case(row["product-identifier"].ToString().Trim());
            //dt.Rows[iRowIdx]["ModelDescription"] = Fix_Case(row["description"].ToString().Trim());
            //dt.Rows[iRowIdx]["ModelXref"] = "";
            //dt.Rows[iRowIdx]["Serial"] = Fix_Case(row["serialNumber"].ToString().Trim());

            //if (int.TryParse(row["id"].ToString().Trim(), out iTemp) == false)
            //    iTemp = -1;
            //if (iTemp > 0)
            //    dt.Rows[iRowIdx]["Unit"] = iTemp.ToString("");

            //dt.Rows[iRowIdx]["Source"] = "2";

            //iRowIdx++;
        }


        dt.AcceptChanges();

        return dt;
    }
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
            if (saPreNumTyp.Length > 2) 
            {
                hfPrimaryCs1.Value = saPreNumTyp[1];
                hfPrimaryCs1Type.Value = saPreNumTyp[2];
            }
                

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
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Con)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Con(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null

        string sortExpression_Con = "";

        if (ViewState["vsDataTable_Con"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Con"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching locations were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Con"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
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
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Con;

        gv_ContactLarge.DataSource = dt.DefaultView;
        gv_ContactLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Con(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_ContactLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Con(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Con(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Con == SortDirection.Ascending)
                gridSortDirection_Con = SortDirection.Descending;
            else
                gridSortDirection_Con = SortDirection.Ascending;
        }
        else
            gridSortDirection_Con = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Con = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Con(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Con
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Con"] == null)
            {
                ViewState["GridSortDirection_Con"] = SortDirection.Ascending;
                //ViewState["GridSortDirection"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Con"];
        }
        set
        {
            ViewState["GridSortDirection_Con"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Con
    {
        get
        {
            if (ViewState["GridSortExpression_Con"] == null)
            {
                ViewState["GridSortExpression_Con"] = "ContactName"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Con"];
        }
        set
        {
            ViewState["GridSortExpression_Con"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Con)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btContactSearchSubmit_Click(object sender, EventArgs e)
    {
        Load_ContactDataTables();
    }
    // ========================================================================
    protected void btContactSearchClear_Click(object sender, EventArgs e)
    {
        txSearchAsset.Text = "";
        txSearchContact.Text = "";
        txSearchEmail.Text = "";
        txSearchLocation.Text = "";
        txSearchSerial.Text = "";
        ddSearchCustomerFamily.SelectedValue = "";
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkLoadContactForEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('|');
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
        int iUnit = 0;
        string sSerial = "";
        string sContactName = "";
        string sTitle = "";
        string sPhone = "";
        string sExtension = "";
        string sEmail = "";
        string sAsset = "";

        if (saArg.Length > 9)
        {
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = -1;

            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = -1;

            sSerial = saArg[2].Trim();

            if (int.TryParse(saArg[3], out iUnit) == false)
                iUnit = -1;

            sContactName = saArg[4].Trim();

            sTitle = saArg[5].Trim();

            sPhone = saArg[6].Trim();

            sExtension = saArg[7].Trim();

            sEmail = saArg[8].Trim();

            sAsset = saArg[9].Trim();

            sTemp = "";
            // If you have data, 
            if (iCustomerNumber > 0 && iUnit > 0)
            {
                sTemp = 
                    "Updating: " + ws_Get_B1CustomerName(iCustomerNumber.ToString(), iCustomerLocation.ToString()) + " (" + 
                    iCustomerNumber.ToString() + "-" + iCustomerLocation.ToString() + ")";
                if (!String.IsNullOrEmpty(sSerial))
                    sTemp += " Serial: " + sSerial;
                if (!String.IsNullOrEmpty(sAsset))
                    sTemp += " Asset: " + sAsset;

                lbUpdateMachineContactHeader.Text = sTemp;

                txUpdateMachineContactEmail.Text = sEmail;
                txUpdateMachineContactExtension.Text = sExtension;
                txUpdateMachineContactName.Text = sContactName;
                txUpdateMachineContactPhone.Text = sPhone;
                txUpdateMachineContactTitle.Text = sTitle;

                hfUpdateMachineContact_Cs1Cs2SerUntConFxa.Value = iCustomerNumber + "|" + iCustomerLocation + "|" + sSerial + "|" + iUnit + "|" + sContactName + "|" + sAsset + "|x";

                pnUpdateMachineContact.Visible = true;
                txUpdateMachineContactName.Focus();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateMachineContact_Click(object sender, EventArgs e)
    {
        string[] saCs1Cs2SerUntConFxa = { "", "", "", "", "", "" };
        string sMachineContactOld = "";
        string sMachineSerialOld = "";
        string sMachineAssetOld = "";
        string sResult = "";
        int iCustomerNumberOld = 0;
        int iCustomerLocationOld = 0;
        int iMachineUnitOld = 0;

        if (!String.IsNullOrEmpty(hfUpdateMachineContact_Cs1Cs2SerUntConFxa.Value))
            saCs1Cs2SerUntConFxa = hfUpdateMachineContact_Cs1Cs2SerUntConFxa.Value.Split('|');
        if (saCs1Cs2SerUntConFxa.Length > 5)
        {
            if (int.TryParse(saCs1Cs2SerUntConFxa[0], out iCustomerNumberOld) == false)
                iCustomerNumberOld = -1;
            if (int.TryParse(saCs1Cs2SerUntConFxa[1], out iCustomerLocationOld) == false)
                iCustomerLocationOld = -1;
            sMachineSerialOld = saCs1Cs2SerUntConFxa[2].Trim();
            if (int.TryParse(saCs1Cs2SerUntConFxa[3], out iMachineUnitOld) == false)
                iMachineUnitOld = -1;
            sMachineContactOld = saCs1Cs2SerUntConFxa[4].Trim();
            sMachineAssetOld = saCs1Cs2SerUntConFxa[5].Trim();

            //if (iCustomerNumberOld > 0 && iCustomerLocationOld > -1 && sMachineSerialOld != "" && iMachineUnitOld > 0)
            if (iCustomerNumberOld > 0 && iCustomerLocationOld > -1 && iMachineUnitOld > 0)
            {
                string sMachineContactNew = txUpdateMachineContactName.Text;
                string sMachinePhoneNew = txUpdateMachineContactPhone.Text;
                string sMachineExtensionNew = txUpdateMachineContactExtension.Text;
                string sMachineEmailNew = txUpdateMachineContactEmail.Text;
                string sMachineTitleNew = txUpdateMachineContactTitle.Text;

                Session.Timeout = 100000;

                sResult = ws_Upd_B1TonerContact(
                    sMachineAssetOld, 
                    sMachineContactOld, 
                    sMachineContactNew, 
                    iCustomerLocationOld.ToString(), 
                    iCustomerNumberOld.ToString(), 
                    sMachineEmailNew, 
                    sMachineExtensionNew, 
                    sMachinePhoneNew, 
                    sMachineTitleNew, 
                    iMachineUnitOld.ToString());

                if (sResult.StartsWith("SUCCESS"))
                {
                    // Clear input fields
                    hfUpdateMachineContact_Cs1Cs2SerUntConFxa.Value = "";
                    txUpdateMachineContactEmail.Text = "";
                    txUpdateMachineContactExtension.Text = "";
                    txUpdateMachineContactName.Text = "";
                    txUpdateMachineContactPhone.Text = "";
                    txUpdateMachineContactTitle.Text = "";
                    pnUpdateMachineContact.Visible = false;

                    Load_ContactDataTables();
                    pnUpdateMachineContact.Visible = false;
                    lbMsg.Text = "Contact update was successful (" + sMachineContactNew + ")";
                }
                else 
                {
                    lbMsg.Text = "Error: The update was NOT successful.";
                }
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateMachineContactClose_Click(object sender, EventArgs e)
    {
        pnUpdateMachineContact.Visible = false;
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
