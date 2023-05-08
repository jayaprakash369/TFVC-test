using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web;

public partial class private_customerAdministration_UserMaintenance : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    string sTemp = "";
    
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMessage.Text = "";

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        if (!IsPostBack)
        {
            try
            {
                sqlConn.Open();

                //GetUser();
                Get_UserPrimaryCustomerNumber();

                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }

    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected string ws_Get_B1CustomerFamilyNumberList(int customerNumber) 
    {
        string sCustomerNumberList = "";

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1CustomerFamilyNumberList";
            string sFieldList = "customerNumber";
            string sValueList = customerNumber.ToString();

            sCustomerNumberList = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCustomerNumberList;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetUsers(int cs1)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        int iPrimaryCs1 = 0;
        int iSearchCs1 = 0;
        //int iLoginCount = 0;

        string sUserEmail = "";
        string sLockedOut = "";
        string sConfirmed = "";
        string sSearchEmail = "";
        string sSearchFirstName = "";
        string sSearchLastName = "";
        string sCompanyType = "";
        string sSql = "";

        DateTime datTemp = new DateTime();

        //string sCs1Type = "";

        if (pnSearchUser.Visible == true)
        {
            sSearchEmail = txSearchEmail.Text.Trim();
            if (int.TryParse(txSearchCs1.Text.ToString().Trim(), out iSearchCs1) == false)
                iSearchCs1 = 0;
            sSearchFirstName = txSearchFirstName.Text.Trim();
            sSearchLastName = txSearchLastName.Text.Trim();
        }

        string sCs1Family = "";

        try
        {
            sSql = "Select" + 
                 " UserName" +  // is an email
                ", PrimaryPrefix" +
                ", PrimaryCs1" +
                ", CompanyType" +
                ", FirstName" + 
                ", LastName" + 
                ", LoginCount" + 
                ", LastActivityDate" + 
                ", IsLockedOut" +
                ", EmailConfirmed" +
                ", " +
                    " ISNULL((Select 'True '" +
                    " from " + 
                    sSqlDbToUse_Customer + ".aspnet_Users uu, " +
                    sSqlDbToUse_Customer + ".aspnet_Roles rr, " +
                    sSqlDbToUse_Customer + ".aspnet_UsersInRoles ur" +
                    " where uu.UserId = ur.UserId" +
                    " and ur.RoleId = rr.RoleId" +
                    " and uu.UserName = u.UserName" +
                    " and rr.RoleName = 'CustomerAdministrator'), '') as CustomerAdministrator" +
                " from " +
                sSqlDbToUse_Customer + ".aspnet_Users u, " +
                sSqlDbToUse_Customer + ".aspnet_Membership m" +
                " where u.UserId = m.UserId";
            if (chBxAll.Checked == false)
            {
                sCs1Family = ws_Get_B1CustomerFamilyNumberList(cs1);
                if (sCs1Family.Contains("ERROR|"))
                    sCs1Family = hfPrimaryCs1.Value;
                else 
                {
                    if (!String.IsNullOrEmpty(sCs1Family))
                        sCs1Family += ", ";
                    sCs1Family += cs1.ToString();
                }

                sSql += " and u.PrimaryCs1 in (" + sCs1Family + ")";
            }
            if (sSearchEmail != "")
                sSql += " and u.UserName like @SearchEmail";
            if (iSearchCs1 > 0)
                sSql += " and u.PrimaryCs1 = @SearchCs1";
            if (sSearchFirstName != "")
                sSql += " and u.FirstName like @SearchFirstName";
            if (sSearchLastName != "")
                sSql += " and u.LastName like @SearchLastName";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            if (sSearchEmail != "")
                sqlCmd.Parameters.AddWithValue("@SearchEmail", sSearchEmail + "%");

            if (iSearchCs1 > 0)
                sqlCmd.Parameters.AddWithValue("@SearchCs1", iSearchCs1);

            if (sSearchFirstName != "")
                sqlCmd.Parameters.AddWithValue("@SearchFirstName", sSearchFirstName + "%");

            if (sSearchLastName != "")
                sqlCmd.Parameters.AddWithValue("@SearchLastName", sSearchLastName + "%");


            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                //if (sqlReader.HasRows) {}
                dt.Load(sqlReader);
            }

            dt.Columns.Add(MakeColumn("Locked"));
            dt.Columns.Add(MakeColumn("AccountId"));
            dt.Columns.Add(MakeColumn("Cs1Name"));
            dt.Columns.Add(MakeColumn("LastLogin"));
            dt.Columns.Add(MakeColumn("LastLoginSort"));
            dt.Columns.Add(MakeColumn("RegistrationNotConfirmed"));

            dt.AcceptChanges();

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(dt.Rows[iRowIdx]["PrimaryCs1"].ToString().Trim(), out iPrimaryCs1) == false)
                    iPrimaryCs1 = 0;
                
                
                sUserEmail = dt.Rows[iRowIdx]["UserName"].ToString().Trim();
                sCompanyType = dt.Rows[iRowIdx]["CompanyType"].ToString().Trim();

                if (sCompanyType == "SRG" || sCompanyType == "SRP" || sCompanyType == "SRC") 
                {
                    dt.Rows[iRowIdx]["AccountId"] = dt.Rows[iRowIdx]["PrimaryPrefix"].ToString().Trim() + iPrimaryCs1.ToString();
                }
                else 
                {
                    dt.Rows[iRowIdx]["AccountId"] = iPrimaryCs1.ToString();
                    //if (iPrimaryCs1 > 0)
                    //    dt.Rows[iRowIdx]["Cs1Name"] = ws_Get_B1CustomerName(iPrimaryCs1.ToString(), "");
                }
                
                dt.Rows[iRowIdx]["Cs1Name"] = ws_Get_B1AccountName(dt.Rows[iRowIdx]["PrimaryPrefix"].ToString().Trim(), dt.Rows[iRowIdx]["PrimaryCs1"].ToString().Trim(), "");


                if (DateTime.TryParse(dt.Rows[iRowIdx]["LastActivityDate"].ToString(), out datTemp) == true) 
                {
                    dt.Rows[iRowIdx]["LastLogin"] = datTemp.ToString("MMM d yyyy");
                    dt.Rows[iRowIdx]["LastLoginSort"] = datTemp.ToString("o");
                }

                sLockedOut = dt.Rows[iRowIdx]["IsLockedOut"].ToString().Trim();
                if (sLockedOut == "True")
                    dt.Rows[iRowIdx]["Locked"] = "Locked";

                //sCs1Admin = dt.Rows[iRowIdx]["CustomerAdmin"].ToString().Trim();
                //if (int.TryParse(dt.Rows[iRowIdx]["LoginCount"].ToString().Trim(), out iLoginCount) == false)
                 //   iLoginCount = 0;


                //sCs1Type = "";


                sConfirmed = dt.Rows[iRowIdx]["EmailConfirmed"].ToString().Trim();
                if (sConfirmed == "True") { }
                else 
                    dt.Rows[iRowIdx]["RegistrationNotConfirmed"] = "Pending";

                dt.AcceptChanges();

                iRowIdx++;
            }
            //pnUpdateUser.Visible = false;
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            //EmailHandler emailHandler = new EmailHandler();
            //string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();
            //emailHandler.EmailIndividual("SQL library check from " + sUserURL, "SQL: " + sSql, "steve.carlson@scantron.com", "adv320@scantron.com");
            //emailHandler = null;
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------
    protected int getUserIdCs1(string userName)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        int iCustomerNumber = 0;

        try
        {
            string sSql = "";

            sSql = "Select PrimaryCs1" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserName", userName);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["PrimaryCs1"].ToString().Trim(), out iCustomerNumber) == false)
                        iCustomerNumber = 0;
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
        return iCustomerNumber;
    }
    // -------------------------------------------------------------------------------------------------
    protected void GetLockStatus()
    {
        string sLoginEmail = lbLoginEmail.Text;
        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sUserId = getUserKey(sLoginEmail);

            sSql = "select IsLockedOut" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Membership" +
                " where UserId = @UserId";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@UserId", sUserId);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                //if (sqlReader.HasRows) {}
                dt.Load(sqlReader);
            }

            if (dt.Rows.Count > 0)
            {
                string sLockedOut = dt.Rows[0]["IsLockedOut"].ToString().Trim();

                if (sLockedOut == "False")
                    btToggleLock.Text = "Lock Account";
                else
                    btToggleLock.Text = "Unlock Account";
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void GetCustomerAdministratorStatus()
    {
        string sUserID = lbLoginEmail.Text;

        try
        {
            if (Roles.IsUserInRole(sUserID, "CustomerAdministrator") == true)
                btToggleAdmin.Text = "Revoke Admin";
            else
                btToggleAdmin.Text = "Grant Admin";
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
    protected string getUserKey(string loginEmail)
    {
        string sUserKey = "";
        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            // Currently only being called when connection is already open...
            sSql = "select UserId from " + sSqlDbToUse_Customer + ".aspnet_Users where LoweredUserName = @LoginEmail";
            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@LoginEmail", loginEmail.ToLower());

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                //if (sqlReader.HasRows) {}
                dt.Load(sqlReader);
            }

            if (dt.Rows.Count > 0)
                sUserKey = dt.Rows[0]["UserId"].ToString().Trim();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return sUserKey;
    }
    // -------------------------------------------------------------------------------------------------
    protected int lockUser(string loginEmail)
    {
        int iRowsAffected = 0;

        string sSql = "";

        try
        {
            string sUserId = getUserKey(loginEmail);

            sSql = "update " + sSqlDbToUse_Customer + ".aspnet_Membership set" +
                " IsLockedOut = @Locked" +
            //" where LoweredEmail = @UserId";
            " where UserId = @UserId";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Locked", 1);
            //sqlCmd.Parameters.AddWithValue("@UserId", loginEmail.ToLower());
            sqlCmd.Parameters.AddWithValue("@UserId", sUserId);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // -------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
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

            if (User.Identity.IsAuthenticated)
                hfUserEmailName.Value = User.Identity.Name;
        }
    }
    // -------------------------------------------------------------------------------------------------
    //protected void GetUser()
    //{
    //    if (User.Identity.IsAuthenticated) 
    //    {
    //        hfUserEmailName.Value = User.Identity.Name;
    //        hfPrimaryCs1.Value = getUserIdCs1(hfUserEmailName.Value).ToString();
    //        hfCurrentCs1.Value = hfPrimaryCs1.Value;
    //        //if (User.IsInRole("SiteAdministrator")) 
    //    }
    //}
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region Grid Controls
    // =========================================================
    // START: USER GRID
    // =========================================================
    protected void BindGrid_Usr()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Usr"] == null)
        {
            int iCurrentCustomerNumber = 0;
            if (int.TryParse(hfPrimaryCs1.Value, out iCurrentCustomerNumber) == false)
                iCurrentCustomerNumber = -1;
            dt = GetUsers(iCurrentCustomerNumber);
            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Usr"] = dt;
        }
        else
        {
            dt = (DataTable)ViewState["vsDataTable_Usr"];
        }

        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Usr;
        if (gridSortDirection_Usr == SortDirection.Ascending)
        {
            sortExpression_Usr = gridSortExpression_Usr + " ASC";
        }
        else
        {
            sortExpression_Usr = gridSortExpression_Usr + " DESC";
        }
        // Sort the data
        if (dt.Rows.Count > 0)
            dt.DefaultView.Sort = sortExpression_Usr;

        gv_UsrLarge.DataSource = dt.DefaultView;
        gv_UsrLarge.DataBind();

        rp_UserSmall.DataSource = dt.DefaultView;
        rp_UserSmall.DataBind();

    }
    // -----------------------------------------------------------------------
    protected void gvSorting_Usr(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Usr = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Usr == e.SortExpression)
        {
            if (gridSortDirection_Usr == SortDirection.Ascending)
                gridSortDirection_Usr = SortDirection.Descending;
            else
                gridSortDirection_Usr = SortDirection.Ascending;
        }
        else
            gridSortDirection_Usr = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Usr = sortExpression_Usr;
        // Rebind the grid to its data source
        BindGrid_Usr();
    }
    // -----------------------------------------------------------------------
    private SortDirection gridSortDirection_Usr
    {
        get
        {
            if (ViewState["GridSortDirection_Usr"] == null)
            {
                // Initial state in this program will be "Ascending"
                ViewState["GridSortDirection_Usr"] = SortDirection.Ascending;
                //ViewState["GridSortDirection_Usr"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Usr"];
        }
        set
        {
            ViewState["GridSortDirection_Usr"] = value;
        }
    }
    // -----------------------------------------------------------------------
    private string gridSortExpression_Usr
    {
        get
        {
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Usr"] == null)
            {
                ViewState["GridSortExpression_Usr"] = "UserName"; // *** INITIAL SORT BY FIELD *** xxx
            }
            return (string)ViewState["GridSortExpression_Usr"];
        }
        set
        {
            ViewState["GridSortExpression_Usr"] = value;
        }
    }
    // -----------------------------------------------------------------------
    protected void gvPageIndexChanging_Usr(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gv_UsrLarge.PageIndex = newPageIndex;
        BindGrid_Usr();
    }
    // =========================================================
    // END: USER GRID 
    // =========================================================
    #endregion // end Grid Controls
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // =========================================================
    protected void lkLoginEmail_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sLoginEmail = linkControl.CommandArgument.ToString();
        lbLoginEmail.Text = sLoginEmail;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn.Open();

            string sSql = "";
            string sUserId = "";
            //string sEmail = "";
            string sFirstName = "";
            string sLastName = "";

            sSql = "Select UserId, UserName, FirstName, LastName" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u" +
                " where LoweredUserName = @LoginEmail";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@LoginEmail", sLoginEmail.ToLower());

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                //if (sqlReader.HasRows) {}
                dt.Load(sqlReader);
            }

            if (dt.Rows.Count > 0)
            {
                sUserId = dt.Rows[0]["UserId"].ToString().Trim();
                sLoginEmail = dt.Rows[0]["UserName"].ToString().Trim().ToLower();
                sFirstName = dt.Rows[0]["FirstName"].ToString().Trim();
                sLastName = dt.Rows[0]["LastName"].ToString().Trim();

                txUpdatePassword.Text = "";
                txUpdatePassword2.Text = "";
                txUpdateFirstName.Text = "";
                txUpdateLastName.Text = "";
                txUpdateCustNum.Text = "";
                txUpdateEmail.Text = "";
                txUpdateEmail2.Text = "";
                txUpdateCustNum.Text = "";
                GetLockStatus();
                GetCustomerAdministratorStatus();

            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            pnUpdateUser.Visible = true;
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void chBxAllChange_Click(object sender, EventArgs e)
    {
        try
        {
            sqlConn.Open();
            ViewState["vsDataTable_Usr"] = null;
            BindGrid_Usr();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btSearchUser_Click(object sender, EventArgs e)
    {
        try
        {
            sqlConn.Open();
            ViewState["vsDataTable_Usr"] = null;
            BindGrid_Usr();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateEmail_Click(object sender, EventArgs e)
    {
        string sOriginalEmail = lbLoginEmail.Text.Trim();
        string sNewEmail = txUpdateEmail.Text.Trim();
        string sNewEmail2 = txUpdateEmail2.Text.Trim();
        string sSql = "";
        int iRowsAffected = 0;

        if (String.IsNullOrEmpty(sNewEmail))
        {
            lbMessage.Text = "Email entry required.";
            txUpdateEmail.Focus();
        }
        else if (String.IsNullOrEmpty(sNewEmail2))
        {
            lbMessage.Text = "Email confirmation entry required.";
            txUpdateEmail2.Focus();
        }
        else if (isEmailFormatValid(sNewEmail) != true)
        {
            lbMessage.Text = "Email entry format is invalid.";
            txUpdateEmail.Focus();
        }
        else if (isEmailFormatValid(sNewEmail2) != true)
        {
            lbMessage.Text = "Email confirmation entry format is invalid.";
            txUpdateEmail2.Focus();
        }
        else if (sNewEmail != sNewEmail2)
        {
            lbMessage.Text = "Email entry and confirmation do not match.";
            txUpdateEmail.Focus();
        }
        else
        {

            try
            {
                sqlConn.Open();

                sSql = "update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                     " UserName = @NewEmail_1" +
                    ", LoweredUserName = @NewEmail_2" +
                    ", Email = @NewEmail_3" +
                    ", EmailConfirmed = @EmailConfirmed" +
                    " where LoweredUserName = @OriginalEmail";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@NewEmail_1", sNewEmail);
                sqlCmd.Parameters.AddWithValue("@NewEmail_2", sNewEmail.ToLower());
                sqlCmd.Parameters.AddWithValue("@NewEmail_3", sNewEmail.ToLower());
                sqlCmd.Parameters.AddWithValue("@EmailConfirmed", 0);
                sqlCmd.Parameters.AddWithValue("@OriginalEmail", sOriginalEmail.ToLower());

                iRowsAffected = sqlCmd.ExecuteNonQuery();

                if (iRowsAffected > 0)
                {
                    lbMessage.Text = "Login email successfully updated from " + sOriginalEmail + " to " + sNewEmail;
                    txUpdateEmail.Text = "";
                    txUpdateEmail2.Text = "";
                }
                else
                    lbMessage.Text = "Error: login email update attempt failed.";

            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "Error: login email update attempt failed: " + ex.Message.ToString();
            }
            finally
            {
                sqlCmd.Dispose();
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                sqlConn.Close();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateFirstName_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim().ToLower();
        string sFirstName = txUpdateFirstName.Text.Trim();
        string sSql = "";
        int iRowsAffected = 0;

        if (String.IsNullOrEmpty(sFirstName))
        {
            lbMessage.Text = "First name entry required.";
        }
        else
        {
            try
            {
                sqlConn.Open();

                sSql = "update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                     " FirstName = @FirstName" +
                    " where LoweredUserName = @LoginEmail";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@FirstName", sFirstName);
                sqlCmd.Parameters.AddWithValue("@LoginEmail", sLoginEmail);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

                if (iRowsAffected > 0)
                {
                    lbMessage.Text = "First Name successfully updated to " + sFirstName;
                    txUpdateFirstName.Text = "";
                }
                else
                    lbMessage.Text = "Error: first name update attempt failed.";

            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "Error: first name update attempt failed: " + ex.Message.ToString();
            }
            finally
            {
                sqlCmd.Dispose();
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                sqlConn.Close();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateLastName_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim().ToLower();
        string sLastName = txUpdateLastName.Text.Trim();
        string sSql = "";
        int iRowsAffected = 0;

        if (String.IsNullOrEmpty(sLastName))
        {
            lbMessage.Text = "Last name entry required.";
        }
        else
        {
            try
            {
                sqlConn.Open();

                sSql = "update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                     " LastName = @LastName" +
                    " where LoweredUserName = @LoginEmail";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@LastName", sLastName);
                sqlCmd.Parameters.AddWithValue("@LoginEmail", sLoginEmail);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

                if (iRowsAffected > 0)
                {
                    lbMessage.Text = "Last Name successfully updated to " + sLastName;
                    txUpdateLastName.Text = "";
                }
                else
                    lbMessage.Text = "Error: last name update attempt failed.";

            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "Error: last name update attempt failed: " + ex.Message.ToString();
            }
            finally
            {
                sqlCmd.Dispose();
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                sqlConn.Close();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdateCustNum_Click(object sender, EventArgs e)
    {
        string sSql = "";
        string sCompanyType = "";
        int iRowsAffected = 0;

        string sLoginEmail = lbLoginEmail.Text.Trim().ToLower();
        int iPrimaryCs1 = 0;
        if (int.TryParse(txUpdateCustNum.Text.Trim(), out iPrimaryCs1) == false)
            iPrimaryCs1 = -1;

        if (isAPositiveInteger(txUpdateCustNum.Text.Trim()) == false)
        {
            lbMessage.Text = "Customer entry must be a positive integer";
        }
        else
        {
            // KOI: I may need to add a prefix on the screen...This may be inadequate if they're switching to a servright customer with a prefix of S or T
            sCompanyType = hfPrimaryCs1Type.Value; // it was...  ws_Get_B1CustomerType(iPrimaryCs1);

            try
            {
                sqlConn.Open();

                sSql = "update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                     " PrimaryCs1 = @PrimaryCs1" +
                    ", CompanyType = @CompanyType" +
                    " where LoweredUserName = @LoginEmail";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@PrimaryCs1", iPrimaryCs1);
                sqlCmd.Parameters.AddWithValue("@CompanyType", sCompanyType);
                sqlCmd.Parameters.AddWithValue("@LoginEmail", sLoginEmail);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

                if (iRowsAffected > 0) 
                {
                    lbMessage.Text = "Customer number successfully updated to " + iPrimaryCs1;
                    txUpdateCustNum.Text = "";
                }
                else
                    lbMessage.Text = "Error: customer number update attempt failed.";

            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "Error: customer number update attempt failed: " + ex.Message.ToString();
            }
            finally
            {
                sqlCmd.Dispose();
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                sqlConn.Close();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btUpdatePassword_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim();
        string sNewPassword = txUpdatePassword.Text.Trim();
        string sVerdict = ValidatePassword(sLoginEmail, sNewPassword);

        string sIsPasswordFormatValid = ValidatePassword(sLoginEmail, sNewPassword);
        if (sIsPasswordFormatValid != "Y")
        {
            lbMessage.Text += sIsPasswordFormatValid;
            txUpdatePassword.Focus();
        }
        else 
        {
            try
            {
                MembershipUser mu = Membership.GetUser(sLoginEmail);
                mu.ChangePassword(mu.ResetPassword(), sNewPassword);
                lbMessage.Text = "Password for account " + sLoginEmail + " successfully updated.";
                txUpdatePassword.Text = "";
                txUpdatePassword2.Text = "";
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "Error: password update failed (" + ex.Message.ToString() + ")";
            }
            finally
            {
            }

        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btToggleLock_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim();
        int iRowsAffected = 0;

        try
        {
            sqlConn.Open();

            if (btToggleLock.Text == "Lock Account")
            {
                iRowsAffected = lockUser(sLoginEmail);
                if (iRowsAffected > 0)
                    lbMessage.Text = "Account for " + sLoginEmail + " is now LOCKED.";
                else
                    lbMessage.Text = "Error: Lock attempt failed for account " + sLoginEmail + ".";
            }
            else
            {

                MembershipUser mu = Membership.GetUser(sLoginEmail);
                mu.UnlockUser();
                lbMessage.Text = "Account for " + sLoginEmail + " is now UNLOCKED.";
                mu = null;
            }

            GetLockStatus();

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred changing the lock on account " + sLoginEmail;
        }
        finally
        {
            ViewState["vsDataTable_Usr"] = null;
            BindGrid_Usr();
            sqlConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btDelete_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim();
        MembershipUser mu = Membership.GetUser();
        string sUserIDSignedOn = mu.UserName;
        if (sLoginEmail == sUserIDSignedOn)
        {
            lbMessage.Text = "You may not delete your own account";
        }
        else
        {
            try
            {
                sqlConn.Open();
                Membership.DeleteUser(sLoginEmail);
                lbMessage.Text = "Account " + sLoginEmail + " has been deleted.";
                pnUpdateUser.Visible = false;
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "An error occurred deleting account " + sLoginEmail;
            }
            finally
            {
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                sqlConn.Close();
            }
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btToggleAdmin_Click(object sender, EventArgs e)
    {
        string sLoginEmail = lbLoginEmail.Text.Trim();

        try
        {
            sqlConn.Open();
            if (btToggleAdmin.Text == "Revoke Admin")
            {
                Roles.RemoveUserFromRole(sLoginEmail, "CustomerAdministrator");
                lbMessage.Text = "Customer admin privileges REMOVED from account " + sLoginEmail;
            }
            else
            {
                Roles.AddUserToRole(sLoginEmail, "CustomerAdministrator");
                lbMessage.Text = "Customer admin privileges GRANTED to account " + sLoginEmail;
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred changing admin privileges for account " + sLoginEmail;
        }
        finally
        {
            ViewState["vsDataTable_Usr"] = null;
            BindGrid_Usr();
            GetCustomerAdministratorStatus();
            sqlConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------
    protected void btCloseUpdateUserPanel_Click(object sender, EventArgs e)
    {
        try
        {
            pnUpdateUser.Visible = false;
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
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}

