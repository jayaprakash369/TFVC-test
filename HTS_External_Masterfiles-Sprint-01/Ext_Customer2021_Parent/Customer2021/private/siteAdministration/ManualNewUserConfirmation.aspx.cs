using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web;

public partial class private_siteAdministration_ManualNewUserConfirmation : MyPage
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
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
        lbMessage.Text = "";

        if (!IsPostBack)
        {
            try
            {
                sqlConn.Open();

                GetUser();

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
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetUsers()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        DateTime datTemp;

        int iPrimaryCs1 = 0;
        int iSearchCs1 = 0;

        string sSearchEmail = "";
        string sSearchFirstName = "";
        string sSearchLastName = "";
        string sSql = "";
        string sDat = "";


        if (pnSearchUser.Visible == true)
        {
            sSearchEmail = txSearchEmail.Text.Trim();
            if (int.TryParse(txSearchCs1.Text.ToString().Trim(), out iSearchCs1) == false)
                iSearchCs1 = 0;
            sSearchFirstName = txSearchFirstName.Text.Trim();
            sSearchLastName = txSearchLastName.Text.Trim();
        }

        try
        {
            sSql = "Select distinct" + 
                 " UserName" +  // is an email
                ", PrimaryCs1" +
                ", CompanyType" +
                ", FirstName" + 
                ", LastName" +
                ", RegistrationDate" +
                //", LoginCount" + 
                //", LastActivityDate" + 
                //", IsLockedOut" +
                //", EmailConfirmed" +
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
            sSql += " and u.EmailConfirmed IS NULL or u.EmailConfirmed not in (1)";

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

            dt.Columns.Add(MakeColumn("Cs1Name"));
            dt.Columns.Add(MakeColumn("Registered"));

            dt.AcceptChanges();

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(row["PrimaryCs1"].ToString().Trim(), out iPrimaryCs1) == false)
                    iPrimaryCs1 = 0;

                if (iPrimaryCs1 > 0)
                {
                    row["Cs1Name"] = ws_Get_B1CustomerName(iPrimaryCs1.ToString(), "");
                }

                if (DateTime.TryParse(row["RegistrationDate"].ToString().Trim(), out datTemp) == true)
                {
                    row["Registered"] = datTemp.ToString("MMM d yyyy");
                }

                dt.AcceptChanges();
            }
            
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
    protected int Update_UserConfirmation(string userId)
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sSql = "";

        try
        {
            sSql = "Update " + sSqlDbToUse_Customer + ".aspnet_Users set" +
                 " EmailConfirmed = @Confirmed" +
                ", ConfirmationCode = @Code" +
                " where LoweredUserName = @UserId";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Confirmed", 1);
            sqlCmd.Parameters.AddWithValue("@Code", "");
            sqlCmd.Parameters.AddWithValue("@UserId", userId);

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
    // -------------------------------------------------------------------------------------------------
    protected void GetUser()
    {
        if (User.Identity.IsAuthenticated) 
        {
            hfUserEmailName.Value = User.Identity.Name;
            hfPrimaryCs1.Value = getUserIdCs1(hfUserEmailName.Value).ToString();
            hfCurrentCs1.Value = hfPrimaryCs1.Value;
            //if (User.IsInRole("SiteAdministrator")) 
           
        }
            
    }
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
            dt = GetUsers();
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
        {
            dt.DefaultView.Sort = sortExpression_Usr;
        }

        gv_UsrLarge.DataSource = dt.DefaultView;
        gv_UsrLarge.DataBind();

        rp_UserSmall.DataSource = dt.DefaultView;
        rp_UserSmall.DataBind();

        if (gv_UsrLarge.Rows.Count > 0)
        {
            //lbClickToConfirm.Visible = true;
            lbClickToConfirm.Text = "Click user id below to confirm account on that user's behalf";
        }
        else
        {
            lbClickToConfirm.Text = "<i>No accounts currently exist that are still unconfirmed.</i>";
        }

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
        LinkButton myControl = (LinkButton)sender;
        string sUserId = myControl.CommandArgument.ToString();
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn.Open();

            iRowsAffected = Update_UserConfirmation(sUserId);
            if (iRowsAffected > 0)
            {
                ViewState["vsDataTable_Usr"] = null;
                BindGrid_Usr();
                lbMessage.Text = "Success: account " + sUserId + " now confirmed for login";
            }
            else 
            {
                lbMessage.Text = "Error: The confirmation update failed.";
            }
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
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}

