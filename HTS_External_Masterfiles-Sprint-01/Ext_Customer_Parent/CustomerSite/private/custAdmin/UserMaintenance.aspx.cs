using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

public partial class private_admCust_UserMaintenance : MyPage
{    
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    DataTable dataTable;
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    string sResult = "";
    string sSql = "";
    string sCs1Changed = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ====================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        lbMessage.Text = "";

        if (!IsPostBack) 
        {
            txPassword.Attributes.Add("autocomplete", "off");
            txPassword2.Attributes.Add("autocomplete", "off");
            txEmail2.Attributes.Add("autocomplete", "off");

            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }
    
    // =========================================================
    protected void userPick_Click(object sender, EventArgs e)
    {
        LinkButton linkControl = (LinkButton)sender;
        string sUserID = linkControl.CommandArgument.ToString();
        lbUserID.Text = sUserID;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        try
        {
            string sSql = "";
            string sContact = "";
            string sPhone1 = "";
            string sPhone2 = "";
            string sPhone3 = "";
            string sExtension = "";
            string sEmail = "";

            sSql = "Select ContactName, ContactEmail, PhoneAreaCode, PhonePrefix, PhoneSuffix, PhoneExtension" +
                " from aspnet_Users u" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = sUserID;

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
            {
                sContact = dataTable.Rows[0]["ContactName"].ToString().Trim();
                sPhone1 = dataTable.Rows[0]["PhoneAreaCode"].ToString().Trim();
                sPhone2 = dataTable.Rows[0]["PhonePrefix"].ToString().Trim();
                sPhone3 = dataTable.Rows[0]["PhoneSuffix"].ToString().Trim();
                sExtension = dataTable.Rows[0]["PhoneExtension"].ToString().Trim();
                sEmail = dataTable.Rows[0]["ContactEmail"].ToString().Trim();

                txPassword.Text = "";
                txPassword2.Text = "";
                txContact.Text = sContact;
                txPhone1.Text = sPhone1;
                txPhone2.Text = sPhone2;
                txPhone3.Text = sPhone3;
                txExtension.Text = sExtension;
                txEmail.Text = sEmail;
                txEmail2.Text = "";
                GetLockStatus();
                GetAdminStatus();
                
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            pnUpdateUser.Visible = true;
        }
    }
    // =========================================================
    protected void GetLockStatus()
    {
        string sUserID = lbUserID.Text;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        string sSql = "";

        try
        {
            string sUserKey = getUserKey(sUserID);

            sSql = "select IsLockedOut" +
                " from aspnet_Membership" +
                " where UserId = @UserId";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserId", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserId"].Value = sUserKey;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
            {
                string sLockedOut = dataTable.Rows[0]["IsLockedOut"].ToString().Trim();

                if (sLockedOut == "False")
                    btToggleLock.Text = "Lock Account";
                else
                    btToggleLock.Text = "Unlock Account";
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
    }
    // =========================================================
    protected void GetAdminStatus()
    {
        string sUserID = lbUserID.Text;

        try
        {
            if (Roles.IsUserInRole(sUserID, "EditorCustomer") == true)
                btToggleAdmin.Text = "Revoke Admin";
            else
                btToggleAdmin.Text = "Grant Admin";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ====================================================
    protected void btUpdateContact_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        string sContact = txContact.Text.Trim();
        try
        {
            string sSql = "";

            sSql = "update aspnet_Users set" +
                " ContactName = @Contact" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@Contact", System.Data.SqlDbType.VarChar, 30);
            sqlCmd.Parameters["@Contact"].Value = sContact;

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = sUserID;

            sqlConn.Open();

            sqlCmd.ExecuteNonQuery();

            lbMessage.Text = "Contact for account " + sUserID + " successfully updated.";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred updating contact for account " + sUserID;
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }
    // ====================================================
    protected void btUpdatePhone_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        string sPhone1 = txPhone1.Text.Trim();
        string sPhone2 = txPhone2.Text.Trim();
        string sPhone3 = txPhone3.Text.Trim();
        string sExtension = txExtension.Text.Trim();
        string sPhone = "(" + sPhone1 + ") " + sPhone2 + "-" + sPhone3;

        try
        {
            string sSql = "";

            sSql = "update aspnet_Users set" +
                 " ContactPhone = @Phone" +
                ", PhoneAreaCode = @PhoneAreaCode" +
                ", PhonePrefix = @PhonePrefix" +
                ", PhoneSuffix = @PhoneSuffix" +
                ", PhoneExtension = @PhoneExtension" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar, 15);
            sqlCmd.Parameters["@Phone"].Value = sPhone;

            sqlCmd.Parameters.Add("@PhoneAreaCode", System.Data.SqlDbType.VarChar, 3);
            sqlCmd.Parameters["@PhoneAreaCode"].Value = sPhone1;

            sqlCmd.Parameters.Add("@PhonePrefix", System.Data.SqlDbType.VarChar, 3);
            sqlCmd.Parameters["@PhonePrefix"].Value = sPhone2;

            sqlCmd.Parameters.Add("@PhoneSuffix", System.Data.SqlDbType.VarChar, 4);
            sqlCmd.Parameters["@PhoneSuffix"].Value = sPhone3;

            sqlCmd.Parameters.Add("@PhoneExtension", System.Data.SqlDbType.VarChar, 8);
            sqlCmd.Parameters["@PhoneExtension"].Value = sExtension;

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = sUserID;

            sqlConn.Open();

            sqlCmd.ExecuteNonQuery();

            lbMessage.Text = "Phone for account " + sUserID + " successfully updated.";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");

            lbMessage.Text = "An error occurred updating phone for account " + sUserID;
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }
    // ====================================================
    protected void btUpdateEmail_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        string sEmail = txEmail.Text.Trim();
        try
        {
            string sSql = "";

            sSql = "update aspnet_Users set" +
                " ContactEmail = @Email" +
                " where UserName = @UserName";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 50);
            sqlCmd.Parameters["@Email"].Value = sEmail;

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = sUserID;

            sqlConn.Open();

            sqlCmd.ExecuteNonQuery();

            lbMessage.Text = "Email for account " + sUserID + " successfully updated.";
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred updating the email address for account " + sUserID;
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }

    // ====================================================
    protected void btUpdatePassword_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sVerdict = sfd.ValidatePassword(sUserID, sPassword);

        if (sVerdict != "VALID")
        {
            vCusPassword.ErrorMessage = sVerdict;
            vCusPassword.IsValid = false;
        }
        else
        {
            try
            {
                MembershipUser mu = Membership.GetUser(sUserID);
                mu.ChangePassword(mu.ResetPassword(), sPassword);
                lbMessage.Text = "Password for account " + sUserID + " successfully updated.";
            }
            catch (Exception ex)
            {
                sResult = ex.ToString();
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "An error occurred updating the password for account " + sUserID;
            }
            finally
            {
            }
        }
    }
    // ====================================================
    protected void btToggleLock_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();

        try
        {
            if (btToggleLock.Text == "Lock Account")
            {
                lockUser(sUserID);
                lbMessage.Text = "Account " + sUserID + " is now LOCKED.";
            }
            else
            {

                MembershipUser mu = Membership.GetUser(sUserID);
                mu.UnlockUser();
                lbMessage.Text = "Account " + sUserID + " is now UNLOCKED.";
            }
            sqlConn.Open();
            GetLockStatus();
            sqlConn.Close();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred changing the lock on account " + sUserID;
        }
        finally
        {
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }
    // ====================================================
    protected void btDelete_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();
        MembershipUser mu = Membership.GetUser();
        string sUserIDSignedOn = mu.UserName;
        if (sUserID == sUserIDSignedOn)
        {
            lbMessage.Text = "You may not delete your own account";
        }
        else 
        {
            try
            {
                Membership.DeleteUser(sUserID);
                lbMessage.Text = "Account " + sUserID + " has been deleted.";
                pnUpdateUser.Visible = false;
            }
            catch (Exception ex)
            {
                sResult = ex.ToString();
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMessage.Text = "An error occurred deleting account " + sUserID;
            }
            finally
            {
                ViewState["vsDataTable_gvUsr"] = null;
                BindGrid("gvUsr");
            }
        }
    }
    // ====================================================
    protected void btToggleAdmin_Click(object sender, EventArgs e)
    {
        string sUserID = lbUserID.Text.Trim();

        try
        {
            if (btToggleAdmin.Text == "Revoke Admin")
            {
                Roles.RemoveUserFromRole(sUserID, "EditorCustomer");
                lbMessage.Text = "Customer admin privileges REMOVED from account " + sUserID;
            }
            else 
            {
                Roles.AddUserToRole(sUserID, "EditorCustomer");
                lbMessage.Text = "Customer admin privileges GRANTED to account " + sUserID;
            }
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMessage.Text = "An error occurred changing admin privileges for account " + sUserID;
        }
        finally
        {
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
            GetAdminStatus();
        }
    }
    // ====================================================
    protected void lockUser(string userID)
    {
        string sSql = "";

        try
        {
            sqlConn.Open(); 
            string sUserKey = getUserKey(userID);

            sSql = "update aspnet_Membership" + 
                " set IsLockedOut = '1'" +
                " where UserId = @UserId";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserId", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserId"].Value = sUserKey;

            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            ViewState["vsDataTable_gvUsr"] = null;
            BindGrid("gvUsr");
        }
    }
    // ====================================================
    protected string getUserKey(string userID)
    {
        string sUserKey = "";
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        string sSql = "";

        try
        {
            // Currently only being called when connection is already open...
            sSql = "select UserId from aspnet_Users where UserName = @UserName";
            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserName"].Value = userID;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            if (dataTable.Rows.Count > 0)
                sUserKey = dataTable.Rows[0]["UserId"].ToString().Trim();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return sUserKey;
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        CheckCs1Changed();
        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iCs1ToUse = iCs1User;
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
                            iCs1ToUse = iCs1Change;
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
                                iCs1ToUse = iCs1Change;
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

        return iCs1ToUse;
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
    // ================================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        ViewState["vsDataTable_gvUsr"] = null;
        BindGrid("gvUsr");
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        ViewState["vsDataTable_gvUsr"] = null;
        BindGrid("gvUsr");
    }
    // ================================================================
    protected void chBxAllChange_Click(object sender, EventArgs e)
    {
        ViewState["vsDataTable_gvUsr"] = null;
        BindGrid("gvUsr");
    }
    // =========================================================
    // START GENERIC GRID LOAD SECTION (Part 1 of 3) (Nothing should ever need to be changed here...)
    // =========================================================
    protected void gvPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gv = (GridView)sender;
        int newPageIndex = e.NewPageIndex;
        gv.PageIndex = newPageIndex;
        BindGrid(gv.ID);
    }
    // =========================================================
    protected void gvSorting(object sender, GridViewSortEventArgs e)
    {
        GridView gv = (GridView)sender;
        Session["thisSortExpressionName"] = "thisSortExpression_" + gv.ID;
        Session["thisSortDirectionName"] = "thisSortDirection_" + gv.ID;
        Session["thisDefaultSortField"] = SetDefaultSortField(gv.ID);
        // Retrieve the name of the clicked column
        string sortExpression_Here = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Here == e.SortExpression)
        {
            if (gridSortDirection_Here == SortDirection.Ascending)
                gridSortDirection_Here = SortDirection.Descending;
            else
                gridSortDirection_Here = SortDirection.Ascending;
        }
        else
            gridSortDirection_Here = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Here = sortExpression_Here;
        // Rebind the grid to its data source
        BindGrid(gv.ID);
    }
    private SortDirection gridSortDirection_Here
    {
        get
        {
            // Initial state is Ascending
            string sSortDirectionName = GetSortDirectionName();
            if (ViewState[sSortDirectionName] == null)
            {
                ViewState[sSortDirectionName] = SortDirection.Ascending;
            }
            // return the state
            return (SortDirection)ViewState[sSortDirectionName];
        }
        set
        {
            string sSortDirectionName = GetSortDirectionName();
            ViewState[sSortDirectionName] = value;
        }
    }
    private string gridSortExpression_Here
    {
        get
        {
            // Initial sort expression is Model Cross Ref
            string sSortExpressionName = GetSortExpressionName();
            if (ViewState[sSortExpressionName] == null)
            {
                if (Session["thisDefaultSortField"] != null)
                    ViewState[sSortExpressionName] = Session["thisDefaultSortField"].ToString();  // Initial Sort...
            }
            return (string)ViewState[sSortExpressionName];
        }
        set
        {
            string sSortExpressionName = GetSortExpressionName();
            ViewState[sSortExpressionName] = value;
        }
    }
    // =========================================================
    protected string GetSortExpressionName()
    {
        string sSortExpressionName = "";
        if (Session["thisSortExpressionName"] != null)
            sSortExpressionName = Session["thisSortExpressionName"].ToString().Trim();

        return sSortExpressionName;
    }
    // =========================================================
    protected string GetSortDirectionName()
    {
        string sSortDirectionName = "";
        if (Session["thisSortDirectionName"] != null)
            sSortDirectionName = Session["thisSortDirectionName"].ToString().Trim();

        return sSortDirectionName;
    }
    // =========================================================
    protected void BindGrid(string gvID)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        string sVsDataTableTitle = "vsDataTable_" + gvID;
        Session["thisSortExpressionName"] = "thisSortExpression_" + gvID;
        Session["thisSortDirectionName"] = "thisSortDirection_" + gvID;
        Session["thisDefaultSortField"] = SetDefaultSortField(gvID);

        // Load only when new or when parms have changed
        if (ViewState[sVsDataTableTitle] == null)
        {
            int iCs1ToUse = GetPrimaryCs1();
            dataTable = LoadDataTable(iCs1ToUse, gvID);

            // Store the data in memory (so you don't have to keep getting it) 
            //ViewState["vsDataTable_Loc"] = dataTable;
            ViewState[sVsDataTableTitle] = dataTable;
        }
        else
        {
            dataTable = (DataTable)ViewState[sVsDataTableTitle];
        }

        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression;
        if (gridSortDirection_Here == SortDirection.Ascending)
        {
            sortExpression = gridSortExpression_Here + " ASC";
        }
        else
        {
            sortExpression = gridSortExpression_Here + " DESC";
        }
        // Sort the data
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression;

        BindDataTable(dataTable, gvID);
    }
    // =========================================================
    // GENERIC GRID: Customized Methods  (Part 2 of 3)
    // =========================================================
    protected DataTable LoadDataTable(int cs1, string gvID)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        
        lbMessage.Text = "";

        if (gvID == "gvUsr") 
        {
            dataTable = GetUsers(cs1);
            if (dataTable.Rows.Count == 0)
                lbMessage.Text = "No matching users were found...";
        }

        return dataTable;
    }
    // =========================================================
    protected void BindDataTable(DataTable dTable, string gvID)
    {
        if (gvID == "gvUsr")
        {
            gvUsr.DataSource = dTable.DefaultView;
            gvUsr.DataBind();
        }
    }
    // =========================================================
    protected string SetDefaultSortField(string gvID)
    {
        string sDefaultSortField = "";
        if (gvID == "gvUsr")
            sDefaultSortField = "UserName";

        return sDefaultSortField;
    }
    // =========================================================
    // GENERIC GRID: SQLs (Part 3 of 3: SQLS)
    // =========================================================
    protected DataTable GetUsers(int cs1)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        dataTable = new DataTable(sMethodName);

        int iPrimaryCs1 = 0;
        string sUserName = "";
        string sContactName = "";
        string sContactPhone = "";
        string sContactEmail = "";
        string sLastActivityDate = "";
        string sLockedOut = "";
        string sCs1Admin = "";
        int iLoginCount = 0;
        string sSearchUser = "";
        int iSearchCs1 = 0;
        int iLastLogin = 0;
        string sSearchEmail = "";
        //string sCs1Type = "";

        if (pnSearch.Visible == true)
        {
            sSearchUser = txSearchUser.Text.Trim();
            sSearchEmail = txSearchEmail.Text.Trim();
            if (int.TryParse(txSearchCs1.Text.ToString().Trim(), out iSearchCs1) == false)
                iSearchCs1 = 0;
        }

        string sCs1Family = "";

        try
        {
            sSql = "Select UserName, PrimaryCs1, ContactName, ContactPhone, PhoneExtension, ContactEmail, LoginCount, LastActivityDate, IsLockedOut, " +
                    " ISNULL((Select 'True '" +
                    " from aspnet_Users uu, aspnet_Roles rr, aspnet_UsersInRoles ur" +
                    " where uu.UserId = ur.UserId" +
                    " and ur.RoleId = rr.RoleId" +
                    " and uu.UserName = u.UserName" +
                    " and rr.RoleName = 'EditorCustomer'), '')" +
                " from aspnet_Users u, aspnet_Membership m" +
                " where u.UserId = m.UserId";
            if (chBxAll.Checked == false)
            {
                if (sPageLib == "L")
                {
                    sCs1Family = wsLive.GetCs1AllNums(sfd.GetWsKey(), cs1);
                }
                else
                {
                    sCs1Family = wsTest.GetCs1AllNums(sfd.GetWsKey(), cs1);
                }
                sSql += " and u.PrimaryCs1 in (" + sCs1Family + ")";
            }
            if (sSearchUser != "")
                sSql += " and u.UserName like @SearchUser";
            if (sSearchEmail != "")
                sSql += " and u.ContactEmail like @SearchEmail";
            if (iSearchCs1 > 0)
                sSql += " and u.PrimaryCs1 = @SearchCs1";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            if (sSearchUser != "")
            {
                sqlCmd.Parameters.Add("@SearchUser", System.Data.SqlDbType.VarChar, 25);
                sqlCmd.Parameters["@SearchUser"].Value = sSearchUser + "%";
            }
            if (sSearchEmail != "")
            {
                sqlCmd.Parameters.Add("@SearchEmail", System.Data.SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@SearchEmail"].Value = sSearchEmail + "%";
            }
            if (iSearchCs1 > 0)
            {
                sqlCmd.Parameters.Add("@SearchCs1", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@SearchCs1"].Value = iSearchCs1;
            }

            sqlConn.Open();
            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            dataTable.Load(sqlReader);
            dataTable.Columns["Column1"].ColumnName = "CustomerAdmin";

            DataColumn dc = new DataColumn();

            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Locked";
            dataTable.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Cs1Name";
            dataTable.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "LastLogin";
            dataTable.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "LastLoginNum";
            dataTable.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Cs1Type";
            dataTable.Columns.Add(dc);

            dataTable.AcceptChanges();

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                if (int.TryParse(dataTable.Rows[iRowIdx]["PrimaryCs1"].ToString().Trim(), out iPrimaryCs1) == false)
                    iPrimaryCs1 = 0;
                sUserName = dataTable.Rows[iRowIdx]["UserName"].ToString().Trim();
                sContactName = dataTable.Rows[iRowIdx]["ContactName"].ToString().Trim();
                sContactPhone = dataTable.Rows[iRowIdx]["ContactPhone"].ToString().Trim();
                sContactEmail = dataTable.Rows[iRowIdx]["ContactEmail"].ToString().Trim();
                DateTime datTemp = new DateTime();
                datTemp = Convert.ToDateTime(dataTable.Rows[iRowIdx]["LastActivityDate"].ToString());
                datTemp = datTemp.ToLocalTime();
                sLastActivityDate = datTemp.ToString();
                iLastLogin = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                sLockedOut = dataTable.Rows[iRowIdx]["IsLockedOut"].ToString().Trim();

                sCs1Admin = dataTable.Rows[iRowIdx]["CustomerAdmin"].ToString().Trim();
                if (int.TryParse(dataTable.Rows[iRowIdx]["LoginCount"].ToString().Trim(), out iLoginCount) == false)
                    iLoginCount = 0;

                if (sLockedOut == "True")
                    dataTable.Rows[iRowIdx]["Locked"] = "Locked";

                //sCs1Type = "";

                if (iPrimaryCs1 > 0)
                {
                    if (sPageLib == "L")
                    {
                        dataTable.Rows[iRowIdx]["Cs1Name"] = wsLive.GetCustName(sfd.GetWsKey(), iPrimaryCs1, 0);
                        dataTable.Rows[iRowIdx]["Cs1Type"] = wsLive.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
                    }
                    else
                    {
                        dataTable.Rows[iRowIdx]["Cs1Name"] = wsTest.GetCustName(sfd.GetWsKey(), iPrimaryCs1, 0);
                        dataTable.Rows[iRowIdx]["Cs1Type"] = wsTest.GetCustType(sfd.GetWsKey(), iPrimaryCs1);
                    }
                }
                //dataTable.Rows[iRowIdx]["Cs1Name"] = dataTable.Rows[iRowIdx]["Cs1Name"].ToString.Trim().ToLower();
                if ((sLastActivityDate != "") && (sLastActivityDate.Length >= 10))
                    sLastActivityDate = sLastActivityDate.Substring(0, 10);
                dataTable.Rows[iRowIdx]["LastLogin"] = sLastActivityDate;
                dataTable.Rows[iRowIdx]["LastLoginNum"] = iLastLogin.ToString();

                dataTable.AcceptChanges();

                iRowIdx++;
            }
            pnUpdateUser.Visible = false;
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }

        return dataTable;
    }
    // =========================================================
    // END GENERIC GRID (Part 3: SQLS)
    // =========================================================
    // ====================================================
    // ====================================================
}