using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class private_customerAdministration_ChangeContactInformation : MyPage
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
                        Load_LocationDataTables();
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
    protected string ws_Upd_B1LocationContactAndPhone(string contactName, string locationPhone, string customerNumber, string customerLocation)
    {
        string sResult = "";

        if (
            (!String.IsNullOrEmpty(contactName) || !String.IsNullOrEmpty(locationPhone)) 
            && !String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Upd_B1LocationContactAndPhone";
            string sFieldList = "contactName|locationPhone|customerNumber|customerLocation|x";
            string sValueList = contactName.Trim() + "|" + locationPhone.Trim() + "|" + customerNumber + "|" + customerLocation + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }
        return sResult;
    }
    // ========================================================================
    protected string ws_Upd_B1LocationCrossRef(string locationCrossRef, string customerNumber, string customerLocation)
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(locationCrossRef) && !String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Upd_B1LocationCrossRef";
            string sFieldList = "locationCrossRef|customerNumber|customerLocation|x";
            string sValueList = locationCrossRef.Trim() + "|" + customerNumber + "|" + customerLocation + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }
        return sResult;
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
    protected void Load_LocationDataTables()
    {
        DataTable dtB1 = new DataTable("");
        DataTable dtB2 = new DataTable("");
        DataTable dt = new DataTable("");

        string sCustomerNumber = "";
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
            sCustomerNumber = iSelectedCustomerNumber.ToString();
        else
            sCustomerNumber = hfPrimaryCs1.Value;

        string sCustomerName = txSearchName.Text.Trim().ToUpper().Trim();
        string sCustomerLocation = txSearchLocation.Text.Trim();
        string sContact = txSearchContact.Text.Trim().ToUpper().Trim();
        string sAddress = txSearchAddress.Text.Trim().ToUpper().Trim();
        string sCity = txSearchCity.Text.Trim().ToUpper().Trim();
        string sState = txSearchState.Text.Trim().ToUpper().Trim();
        string sZip = txSearchZip.Text.Trim().ToUpper().Trim();
        string sPhone = txSearchPhone.Text.Trim().ToUpper().Trim();
        sPhone = Clean_PhoneEntry(sPhone);
        string sXref = txSearchXref.Text.Trim().ToUpper().Trim();

        try
        {
            if (
                    (hfPrimaryCs1Type.Value != "LRG" && hfPrimaryCs1Type.Value != "DLR")
                    ||
                    ( // LRG/DLR must pick something!
                        (iSelectedCustomerNumber > 0)
                        || !String.IsNullOrEmpty(sCustomerName)
                        || !String.IsNullOrEmpty(sCustomerLocation)
                        || !String.IsNullOrEmpty(sAddress)
                        || !String.IsNullOrEmpty(sCity)
                        || !String.IsNullOrEmpty(sState)
                        || !String.IsNullOrEmpty(sZip)
                        || !String.IsNullOrEmpty(sPhone)
                        || !String.IsNullOrEmpty(sXref)
                    )
                )
            {
                dtB1 = ws_Get_B1CustomerLocationDetail(
                    sCustomerNumber,
                    sCustomerLocation,
                    sCustomerName,
                    sContact,
                    sAddress,
                    sCity,
                    sState,
                    sZip,
                    sPhone,
                    sXref);

                // Merge
                dt = Merge_LocationTables(dtB1, dtB2);
            }
            rp_LocationSmall.DataSource = dt;
                rp_LocationSmall.DataBind();

                ViewState["vsDataTable_Loc"] = null;
                BindGrid_Loc(dt);
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
    protected DataTable Merge_LocationTables(DataTable dt1, DataTable dt2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        dt.Columns.Add(MakeColumn("CSTRNR"));
        dt.Columns.Add(MakeColumn("CSTRCD"));
        dt.Columns.Add(MakeColumn("CUSTNM"));
        dt.Columns.Add(MakeColumn("CONTNM"));
        dt.Columns.Add(MakeColumn("XREFCS"));
        dt.Columns.Add(MakeColumn("SADDR1"));
        dt.Columns.Add(MakeColumn("SADDR2"));
        dt.Columns.Add(MakeColumn("CITY"));
        dt.Columns.Add(MakeColumn("STATE"));
        dt.Columns.Add(MakeColumn("ZIPCD"));
        dt.Columns.Add(MakeColumn("HPHONE"));
        dt.Columns.Add(MakeColumn("FLAG8"));
        dt.Columns.Add(MakeColumn("ORCACC"));
        dt.Columns.Add(MakeColumn("Source"));

        dt.Columns.Add(MakeColumn("PhoneDisplay"));

        DataRow dr;
        int iRowIdx = 0;
        string sPhone = "";
        //int iTemp = 0;


        foreach (DataRow row in dt1.Rows)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iRowIdx]["CSTRNR"] = row["CSTRNR"].ToString().Trim();
            dt.Rows[iRowIdx]["CSTRCD"] = row["CSTRCD"].ToString().Trim();
            dt.Rows[iRowIdx]["CUSTNM"] = Fix_Case(row["CUSTNM"].ToString()).Trim();
            dt.Rows[iRowIdx]["CONTNM"] = Fix_Case(row["CONTNM"].ToString()).Trim();
            
            dt.Rows[iRowIdx]["XREFCS"] = Fix_Case(row["XREFCS"].ToString().Trim());
            dt.Rows[iRowIdx]["SADDR1"] = Fix_Case(row["SADDR1"].ToString().Trim());
            dt.Rows[iRowIdx]["SADDR2"] = Fix_Case(row["SADDR2"].ToString().Trim());
            dt.Rows[iRowIdx]["CITY"] = Fix_Case(row["CITY"].ToString().Trim());
            dt.Rows[iRowIdx]["STATE"] = row["STATE"].ToString().Trim();
            dt.Rows[iRowIdx]["ZIPCD"] = row["ZIPCD"].ToString().Trim();
            sPhone = row["HPHONE"].ToString().Trim();
            sPhone = Clean_PhoneEntry(sPhone);
            if (sPhone.Length == 10 && sPhone != "9999999999" && sPhone != "8888888888" && sPhone != "0000000000")
            {
                dt.Rows[iRowIdx]["HPHONE"] = sPhone;
                dt.Rows[iRowIdx]["PhoneDisplay"] = FormatPhone1(sPhone);
            }
            
            dt.Rows[iRowIdx]["FLAG8"] = row["FLAG8"].ToString().Trim();
            dt.Rows[iRowIdx]["ORCACC"] = row["ORCACC"].ToString().Trim();
                
            dt.Rows[iRowIdx]["Source"] = "1";

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

            // Aug 22nd, 2022 no longer needed I'm getting account type from username above
            // Get current primary customer number so you get determine their customer type to know what to show on the screens here

            //if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
            //    iCustomerNumber = -1;
            //if (iCustomerNumber > 0) 
            //{
            //    hfPrimaryCs1Type.Value = Get_UserCompanyTypeById("", iCustomerNumber);
            //}
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region tableSortHandler
    // ========================================================================
    // -------------------------------------------------------------------------------------------------
    // BEGIN: Location Table (_Loc)
    // -------------------------------------------------------------------------------------------------
    protected void BindGrid_Loc(DataTable dt)
    {
        // Normally you don't pass the DataTable into the BindGrid_
        // But becasue you HAVE to load both LARGE screen and a SMALL screen tables
        // You have to retrieve the datatable anyway (or a change)
        // But the sorts, still function from the "ViewState" copy saving the reload
        // So make sure every time you directly call BindGrid_ you make the view state null
        
        string sortExpression_Loc = "";

        if (ViewState["vsDataTable_Loc"] == null)
        {
            lbMsg.Text = "";
            int iCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCustomerNumber) == false)
                iCustomerNumber = -1;

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Loc"] = dt;

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "No matching locations were found...";
                lbMsg.Visible = true;
            }
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Loc"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
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
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Loc;

        gv_LocationLarge.DataSource = dt.DefaultView;
        gv_LocationLarge.DataBind();

    }
    // -------------------------------------------------------------------------------------------------
    protected void gvPageIndexChanging_Loc(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_LocationLarge.PageIndex = newPageIndex;
        DataTable dt = null;
        BindGrid_Loc(dt);
    }
    // -------------------------------------------------------------------------------------------------
    protected void gvSorting_Loc(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression == e.SortExpression)
        {
            if (gridSortDirection_Loc == SortDirection.Ascending)
                gridSortDirection_Loc = SortDirection.Descending;
            else
                gridSortDirection_Loc = SortDirection.Ascending;
        }
        else
            gridSortDirection_Loc = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Loc = sortExpression;
        // Rebind the grid to its data source
        DataTable dt = null;
        BindGrid_Loc(dt);
    }
    // -------------------------------------------------------------------------------------------------
    private SortDirection gridSortDirection_Loc
    {
        get
        {
            // Initial state is descending (was Ascending)
            if (ViewState["GridSortDirection_Loc"] == null)
            {
                ViewState["GridSortDirection_Loc"] = SortDirection.Ascending;
                //ViewState["GridSortDirection"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Loc"];
        }
        set
        {
            ViewState["GridSortDirection_Loc"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    private string gridSortExpression_Loc
    {
        get
        {
            if (ViewState["GridSortExpression_Loc"] == null)
            {
                ViewState["GridSortExpression_Loc"] = "CUSTNM"; // xxx *** INITIAL SORT ***
            }
            return (string)ViewState["GridSortExpression_Loc"];
        }
        set
        {
            ViewState["GridSortExpression_Loc"] = value;
        }
    }
    // -------------------------------------------------------------------------------------------------
    // END: Location Table (_Loc)
    // -------------------------------------------------------------------------------------------------
    // ========================================================================
    #endregion // end tableSortHandler
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btLocationSearchSubmit_Click(object sender, EventArgs e)
    {
        Load_LocationDataTables();
    }
    // -------------------------------------------------------------------------------------------------
    protected void lkLoadLocationContactForEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[3];
        saArg = sParms.Split('|');
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;
        string sContactName = "";
        string sContactPhone = "";

        if (saArg.Length > 3)
        {
            if (int.TryParse(saArg[0], out iCustomerNumber) == false)
                iCustomerNumber = -1;

            if (int.TryParse(saArg[1], out iCustomerLocation) == false)
                iCustomerLocation = -1;

            sContactName = saArg[2].Trim();
            sContactPhone = saArg[3].Trim();

            // If you have data, 
            if (iCustomerNumber > 0)
            {
                lbUpdateLocationContact_Customer.Text = ws_Get_B1CustomerName(iCustomerNumber.ToString(), iCustomerLocation.ToString());
                hfUpdateLocationContact_Cs1Cs2.Value = iCustomerNumber.ToString() + "-" + iCustomerLocation.ToString();
                txUpdateLocationContact_Name.Text = sContactName;
                txUpdateLocationContact_Phone.Text = sContactPhone;

                pnUpdateLocationContact.Visible = true;
                txUpdateLocationContact_Name.Focus();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateLocationContact_Click(object sender, EventArgs e)
    {
        lbMsg.Text = "";

        string[] saCs1Cs2 = { "", "" };
        string sNewContactName = "";
        string sNewLocationPhone = "";
        string sResult = "";
        int iCustomerNumber = 0;
        int iCustomerLocation = 0;

        if (!String.IsNullOrEmpty(hfUpdateLocationContact_Cs1Cs2.Value))
            saCs1Cs2 = hfUpdateLocationContact_Cs1Cs2.Value.Split('-');
        if (saCs1Cs2.Length > 1)
        {
            if (int.TryParse(saCs1Cs2[0], out iCustomerNumber) == false)
                iCustomerNumber = -1;
            if (int.TryParse(saCs1Cs2[1], out iCustomerLocation) == false)
                iCustomerLocation = -1;

            if (iCustomerNumber > 0 && iCustomerLocation > -1) 
            {
                sNewContactName = txUpdateLocationContact_Name.Text.Trim();
                sNewLocationPhone = txUpdateLocationContact_Phone.Text.Trim();

                // Validation
                // ---------------------------------------------------------------
                if (String.IsNullOrEmpty(sNewContactName))
                    lbMsg.Text = "A contact name is required";
                else if (sNewContactName.Length > 30) 
                {
                    lbMsg.Text = "Contact name entry exceeds the 30 character maximum.";
                }

                if (String.IsNullOrEmpty(sNewLocationPhone))
                {
                    if (String.IsNullOrEmpty(lbMsg.Text)) lbMsg.Text += "<br />";
                    lbMsg.Text += "A location primary phone is required";
                }
                else 
                {
                    sNewLocationPhone = Clean_PhoneEntry(sNewLocationPhone);
                    if (sNewLocationPhone.Length != 10)
                    {
                        if (!String.IsNullOrEmpty(lbMsg.Text)) lbMsg.Text += "<br />";
                        lbMsg.Text += "Phone entry must be 10 integers";
                    }
                }

                if (String.IsNullOrEmpty(lbMsg.Text)) 
                {
                    sResult = ws_Upd_B1LocationContactAndPhone(sNewContactName, sNewLocationPhone, iCustomerNumber.ToString(), iCustomerLocation.ToString());

                    if (sResult.StartsWith("SUCCESS"))
                    {
                        Load_LocationDataTables();
                        pnUpdateLocationContact.Visible = false;
                    }
                }
                // ---------------------------------------------------------------
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateLocationContactClose_Click(object sender, EventArgs e)
    {
        pnUpdateLocationContact.Visible = false;
    }
    // ========================================================================
    protected void btLocationSearchClear_Click(object sender, EventArgs e)
    {
        txSearchAddress.Text = "";
        txSearchCity.Text = "";
        txSearchContact.Text = "";
        txSearchLocation.Text = "";
        txSearchName.Text = "";
        txSearchPhone.Text = "";
        txSearchState.Text = "";
        txSearchXref.Text = "";
        txSearchZip.Text = "";
        ddSearchCustomerFamily.SelectedValue = "";
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
