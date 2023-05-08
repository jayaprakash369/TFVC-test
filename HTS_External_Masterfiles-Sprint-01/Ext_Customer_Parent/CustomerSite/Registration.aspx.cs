using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Registration : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES // Changed for TFS Lock Testing Aug 10th, 2019 at 9:04 pm
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    SqlCommand sqlCmd; 
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    string sConnectionString = "";
    string sResult = "";
    string sSql = "";

    DataTable dataTable;
    // ==========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ==========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        if (User.Identity.IsAuthenticated)
        {
            lbAccessInfo.Visible = false;
            lbAccess.Visible = false;
            txCod.Visible = false;
            if (User.IsInRole("Administrator") || User.IsInRole("Editor") || User.IsInRole("EditorCustomer"))
            {

            }
        }
        else
        {
            lbAccessInfo.Visible = true;
            lbAccess.Visible = true;
            txCod.Visible = true;
        }

        txCs1.Focus();
    }
    // ==========================================================
    protected void btRegId_Click(object sender, EventArgs e)
    {
        string sCustType = "";
        string sCustName = "";
        string sAccessCode = "";
        string sAccessCodeToday = "";
        string sOpenOrClosed = "";
        string sAdminBypass = "";
        string sAdminCodeCorrect = "";
        string sCustomerStatus = "";
        // Validate Numeric

        int iCs1 = 0;
        int iAccountCount = 0;
        if (int.TryParse(txCs1.Text, out iCs1) == false)
            iCs1 = 0;
        hfCod.Value = txCod.Text.Trim();
        sAccessCode = hfCod.Value;

        if (iCs1 > 0)
        {
            if (iCs1 > 9999999)
            {
                vCusRegId.ErrorMessage = "Our customer numbers are a maximum of 7 digits.";
                vCusRegId.IsValid = false;
            }
            else
            {
                if (sPageLib == "L")
                {
                    sCustomerStatus = wsLive.GetStatusAsCustomer(sfd.GetWsKey(), iCs1);
                }
                else
                {
                    sCustomerStatus = wsTest.GetStatusAsCustomer(sfd.GetWsKey(), iCs1);
                }
                if (sCustomerStatus != "ACTIVE")
                {
                    vCusRegId.ErrorMessage = "Your entry does not match a currently active customer number.";
                    vCusRegId.IsValid = false;
                }
            }
            if (vCusRegId.IsValid == true)
            {
                // check for prior accounts
                iAccountCount = GetCs1AccountCount();

                // Check if registration is open to multiple basic accounts
                sOpenOrClosed = RegistrationOpenOrClosed(iCs1);

                // Check if they have entered a valid admin access code
                if (sAccessCode != "")
                {
                    sAccessCodeToday = GetAccessCodeToday();
                    if (sAccessCode == sAccessCodeToday)
                        sAdminCodeCorrect = "YES";
                    else
                        sAdminCodeCorrect = "NO";
                }
                // Check if they are logged in as a privileged user
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Administrator") || User.IsInRole("Editor") || User.IsInRole("EditorCustomer"))
                    {
                        sAdminBypass = "YES";
                    }
                }
                // Get Cust Name and Type: REG, LRG, DLR, SSP, SSB, SUB, BAK
                if (iCs1 > 0)
                {
                    if (sPageLib == "L")
                    {
                        sCustType = wsLive.GetCustType(sfd.GetWsKey(), iCs1);
                        sCustName = wsLive.GetCustName(sfd.GetWsKey(), iCs1, 0);
                    }
                    else
                    {
                        sCustType = wsTest.GetCustType(sfd.GetWsKey(), iCs1);
                        sCustName = wsTest.GetCustName(sfd.GetWsKey(), iCs1, 0);
                    }
                }

                // DO VALIDATION ==========================
                // No customer for number entered 
                if (sCustType == "")
                {
                    //vCusRegId.ErrorMessage = "Access was not granted.<br>Please recheck your entries.";
                    vCusRegId.ErrorMessage = "Your customer number entry is invalid.";
                    vCusRegId.IsValid = false;
                }
                // Admin code entered, but BAD
                else if (sAdminCodeCorrect == "NO")
                {
                    vCusRegId.ErrorMessage = "Your access code entry is not valid." +
                    "<br />Please confirm your code with an STS representative at 800.228.3628 to allow access.";
                    vCusRegId.IsValid = false;
                }
                // Account already exists, it's closed and no bypass authority
                else if ((sAdminBypass != "YES") && (sAdminCodeCorrect != "YES") && (iAccountCount >= 1) && (sOpenOrClosed == "CLOSED"))
                {
                    vCusRegId.ErrorMessage = "New account registration for this customer is currently closed." +
                    "<br />Those at your company with admin account privileges can open registration for additional accounts if needed.";
                    vCusRegId.IsValid = false;
                }
                else
                {
                    // ACCESS GRANTED -- Continue...
                    lbStep2Cs1.Text = iCs1.ToString() + "&nbsp;&nbsp;" + sCustName;
                    /*
                    if (sCustType == "REG") lbStep2Cs1Type.Text = "REGULAR customer";
                    else if (sCustType == "LRG") lbStep2Cs1Type.Text = "LARGE customer";
                    else if (sCustType == "DLR") lbStep2Cs1Type.Text = "BUSINESS PARTNER";
                    else if (sCustType == "SSP") lbStep2Cs1Type.Text = "SELF SERVICE customer (Parts Support)";
                    else if (sCustType == "SSB") lbStep2Cs1Type.Text = "SELF SERVICE customer (Parts and Maintenance)";
                    else if (sCustType == "SUB") lbStep2Cs1Type.Text = "SUB-CONTRACT customer";
                    else if (sCustType == "BAK") lbStep2Cs1Type.Text = "NON-CONTRACT customer";
                    */

                    txUserID.Focus();
                    pnOne.Visible = false;
                    pnTwo.Visible = true;
                }
            } // status checked...
        }
    }
    // ==========================================================
    protected void btUserID_Click(object sender, EventArgs e)
    {
        serverSideVal_UserId();

        if (vCusUserID.ErrorMessage != "")
            vCusUserID.IsValid = false;
        else
            checkUserIdAvailability();
    }
    // ==========================================================
    protected void serverSideVal_UserId()
    {
        string sUserID = txUserID.Text.Trim();
        vCusUserID.ErrorMessage = "";
        if (sUserID == "")
        {
            vCusUserID.ErrorMessage = "A proposed User ID is required";
            txUserID.Focus();
        }
        else
        {
            int iUserIDLength = sUserID.Length;
            if ((iUserIDLength < 5) || (iUserIDLength > 20))
            {
                vCusUserID.ErrorMessage = "The User ID must be 5 to 20 characters";
                txUserID.Focus();
            }
            else
            {
                if (sUserID.IndexOf(" ") != -1)
                {
                    vCusUserID.ErrorMessage = "The User ID may not contain blanks";
                    txUserID.Focus();
                }
            }
        }
    }
    // ==========================================================
    protected void checkUserIdAvailability()
    {
        try
        {
            string sUserID = txUserID.Text.Trim();
            sSql = "Select UserName from aspnet_Users where lower(UserName) = @UserID";
            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.Add("@UserID", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@UserID"].Value = sUserID.ToLower();
            sqlConn.Open();
            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataTable = new DataTable();
            dataTable.Load(sqlReader);
            if (dataTable.Rows.Count == 0)  // name has not yet been taken
            {
                // Check if they are logged in...
                if (User.Identity.IsAuthenticated)
                {
                    chBxGrantAdmin.Checked = false;
                    // Logged in as a privileged user?
                    if (User.IsInRole("Administrator") || User.IsInRole("Editor") || User.IsInRole("EditorCustomer"))
                    {
                        lbGrantAdmin.Visible = true;
                        chBxGrantAdmin.Visible = true;
                    }
                }
                // Anonymous User
                else
                {
                    if (hfCod.Value == GetAccessCodeToday())
                        chBxGrantAdmin.Checked = true;
                    else
                        chBxGrantAdmin.Visible = false;
                }

                lbUserIDDisplay.Text = sUserID;
                txContact.Focus();
                pnTwo.Visible = false;
                pnThree.Visible = true;
            }
            else
            {
                vCusUserID.ErrorMessage = "UserID <b>" + sUserID + "</b> is not available. Please try another.";
                vCusUserID.IsValid = false;
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
        }
    }
    // ==========================================================
    protected void btRegData_Click(object sender, EventArgs e)
    {
        vCusRegData.ErrorMessage = "";

        int iCs1 = 0;
        if (int.TryParse(txCs1.Text, out iCs1) == false)
            iCs1 = 0;
        string sSql = "";
        string sCs1 = iCs1.ToString();
        string sUserID = txUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sEmail = txEmail.Text.Trim();

        string sCustType = "";
        string sResult = "";
        string sVerdict = "";
        DateTime datTemp = new DateTime();

        sVerdict = ServerSideVal_RegData();

        // If all is well, process...
        if (sVerdict == "VALID")
        {
            // REG, LRG, DLR, SS, SUB, BAK
            if (sPageLib == "L")
            {
                sCustType = wsLive.GetCustType(sfd.GetWsKey(), iCs1);
            }
            else
            {
                sCustType = wsTest.GetCustType(sfd.GetWsKey(), iCs1);
            }

            string sContactName = txContact.Text.Trim();
            string sPhone1 = txPhone1.Text.Trim();
            string sPhone2 = txPhone2.Text.Trim();
            string sPhone3 = txPhone3.Text.Trim();
            string sContactPhone = "(" + sPhone1 + ") " + sPhone2 + "-" + sPhone3;
            string sExtension = txExtension.Text.Trim();
            string sContactEmail = txEmail.Text.Trim();
            string sRole = "";
            string sErrValues = "";
            string sCreatorType = "Anonymous";

            try
            {
                // Create New User
                if (User.Identity.IsAuthenticated)
                    sCreatorType = "Admin";
                Membership.CreateUser(sUserID, sPassword, sEmail);

                //MembershipUser mu1 = Membership.GetUser("Steve.1");
                //MembershipUser mu = Membership.GetUser(sUserID);
                //string sUser = mu.UserName;

                if (chBxGrantAdmin.Checked == true)
                {
                    sRole = "EditorCustomer";
                }

                // Assign user to proper role
                if (sRole != "")
                    Roles.AddUserToRole(sUserID, sRole);

                // Set ProfileCommon: Personalization Properties
                ProfileCommon pc = new ProfileCommon();
                pc.Initialize(sUserID, true);
                pc.LoginCs1 = sCs1;
                pc.LoginType = sCustType; // REG, LRG, DLR, SSP, SSB, SUB, BAK
                pc.Save();

                // Set Customer Preference File
                if (sPageLib == "L")
                {
                    wsLive.EnsureCustPrefExists(sfd.GetWsKey(), iCs1);
                }
                else
                {
                    wsTest.EnsureCustPrefExists(sfd.GetWsKey(), iCs1);
                }

                // Update SQL Database
                try
                {
                    // Create command
                    sSql = "UPDATE aspnet_Users SET" +
                         " CompanyType=@CompanyType" +
                        ", ContactName=@ContactName" +
                        ", ContactPhone=@ContactPhone" +
                        ", PhoneAreaCode=@PhoneAreaCode" +
                        ", PhonePrefix=@PhonePrefix" +
                        ", PhoneSuffix=@PhoneSuffix" +
                        ", PhoneExtension=@PhoneExtension" +
                        ", ContactEmail=@ContactEmail" +
                        ", LoginCount=@LoginCount" +
                        ", PrimaryCs1=@PrimaryCs1" +
                        ", RegistrationDate=@RegistrationDate" +
                        " WHERE UserName=@UserID";
                    sqlCmd = new SqlCommand(sSql, sqlConn);

                    // Add command parameters
                    sqlCmd.Parameters.Add("@CompanyType", System.Data.SqlDbType.VarChar, 15);
                    sqlCmd.Parameters["@CompanyType"].Value = sCustType;

                    sqlCmd.Parameters.Add("@ContactName", System.Data.SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@ContactName"].Value = sContactName;

                    sqlCmd.Parameters.Add("@ContactPhone", System.Data.SqlDbType.VarChar, 15);
                    sqlCmd.Parameters["@ContactPhone"].Value = sContactPhone;

                    sqlCmd.Parameters.Add("@PhoneAreaCode", System.Data.SqlDbType.VarChar, 3);
                    sqlCmd.Parameters["@PhoneAreaCode"].Value = sPhone1;

                    sqlCmd.Parameters.Add("@PhonePrefix", System.Data.SqlDbType.VarChar, 3);
                    sqlCmd.Parameters["@PhonePrefix"].Value = sPhone2;

                    sqlCmd.Parameters.Add("@PhoneSuffix", System.Data.SqlDbType.VarChar, 4);
                    sqlCmd.Parameters["@PhoneSuffix"].Value = sPhone3;

                    sqlCmd.Parameters.Add("@PhoneExtension", System.Data.SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@PhoneExtension"].Value = sExtension;

                    sqlCmd.Parameters.Add("@ContactEmail", System.Data.SqlDbType.VarChar, 50);
                    sqlCmd.Parameters["@ContactEmail"].Value = sContactEmail;

                    sqlCmd.Parameters.Add("@LoginCount", System.Data.SqlDbType.Int);
                    sqlCmd.Parameters["@LoginCount"].Value = 1;

                    sqlCmd.Parameters.Add("@PrimaryCs1", System.Data.SqlDbType.Int);
                    sqlCmd.Parameters["@PrimaryCs1"].Value = iCs1;

                    datTemp = DateTime.Now;

                    sqlCmd.Parameters.Add("@RegistrationDate", System.Data.SqlDbType.DateTime);
                    sqlCmd.Parameters["@RegistrationDate"].Value = datTemp.ToString(); // have to use variable AND ToString() into DateTime

                    sqlCmd.Parameters.Add("@UserID", System.Data.SqlDbType.VarChar, 256);
                    sqlCmd.Parameters["@UserID"].Value = sUserID;

                    // Enclose database code in Try-Catch-Finally
                    try
                    {
                        // Open the connection
                        sqlConn.Open();
                        // Execute the command
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sResult = ex.ToString();
                        sErrValues = "Ex 1 -- ID: " + sUserID + "  PWD: " + sPassword + "   EML: " + sEmail;
                        SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
                        vCusRegData.IsValid = false;
                        vCusRegData.ErrorMessage = "Error updating registration details<br />";
                    }
                    finally
                    {

                        // Close the connection
                        sqlCmd.Dispose();
                        sqlConn.Close();

                    }
                    // ------------------------------ 
                }
                catch (Exception ex2)
                {
                    sResult = ex2.ToString();
                    sErrValues = "Ex 2 -- ID: " + sUserID + "  PWD: " + sPassword + "   EML: " + sEmail;
                    SaveError(ex2.Message.ToString(), ex2.ToString(), sErrValues);
                    vCusRegData.IsValid = false;
                    vCusRegData.ErrorMessage = "Error Creating SQL Command: " + ex2.ToString();
                }
            }
            catch (MembershipCreateUserException ex3)
            {
                sResult = ex3.ToString();
                sErrValues = "Ex 3 -- ID: " + sUserID + "  PWD: " + sPassword + "   EML: " + sEmail;
                SaveError(ex3.Message.ToString(), ex3.ToString(), sErrValues);
                vCusRegData.IsValid = false;
                vCusRegData.ErrorMessage = "Error Inserting New User: " + ex3.ToString();
            }

            if (vCusRegData.IsValid == true)
            {

                if (sCreatorType == "Admin") // Show admin them the newly created account
                    Response.Redirect("~/private/custAdmin/UserMaintenance.aspx", false);
                else
                    FormsAuthentication.RedirectFromLoginPage(sUserID, false);
            }
        }
    }
    // ==========================================================
    protected int GetCs1AccountCount()
    {
        int iAccountCount = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        string sUserName = "";
        int iCs1 = 0;
        if (int.TryParse(txCs1.Text, out iCs1) == false)
            iCs1 = 0;

        try
        {
            sSql = " select u.UserName" +
                " from aspnet_Users u" +
                " where u.PrimaryCs1 = @PrimaryCs1";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@PrimaryCs1", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@PrimaryCs1"].Value = iCs1.ToString();

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                sUserName = dataTable.Rows[iRowIdx]["UserName"].ToString().Trim();
                iRowIdx++;
            }
            iAccountCount = iRowIdx;

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
        return iAccountCount;
    }
    // ==========================================================
    protected int GetCs1AdminAccounts()
    {
        int iAdminCount = 0;
        string sUserID = "";
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        int iCs1 = 0;
        if (int.TryParse(txCs1.Text, out iCs1) == false)
            iCs1 = 0;

        try
        {
            sSql = " select u.UserName, r.RoleName" +
                " from aspnet_Applications v" +
                ", aspnet_Users u" +
                ", aspnet_Membership m" +
                ", aspnet_Roles r" +
                ", aspnet_UsersInRoles uir" +
                " where v.LoweredApplicationName = '/'" +
                " and   v.ApplicationId = u.ApplicationId" +
                " and   u.UserId = m.UserId" +
                " and   r.ApplicationId = v.ApplicationId" +
                " and   r.RoleId = uir.RoleId" +
                " and   m.UserId = uir.UserId" +
                " and   r.RoleName = 'EditorCustomer'" +
                " and   u.PrimaryCs1 = @PrimaryCs1";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@PrimaryCs1", System.Data.SqlDbType.VarChar, 256);
            sqlCmd.Parameters["@PrimaryCs1"].Value = iCs1;

            sqlConn.Open();

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            DataTable dataTable = new DataTable(sMethodName);
            dataTable.Load(sqlReader);

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                sUserID = dataTable.Rows[iRowIdx]["UserName"].ToString().Trim();
                iRowIdx++;
            }
            iAdminCount = iRowIdx;
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
        return iAdminCount;
    }
    // =========================================================
    protected string RegistrationOpenOrClosed(int cs1)
    {
        string sOpenOrClosed = "";
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iOpenOrClosed = 0;

        if (sPageLib == "L")
        {
            iOpenOrClosed = wsLive.GetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), cs1);
        }
        else
        {
            iOpenOrClosed = wsTest.GetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), cs1);
        }

        if (iOpenOrClosed >= 1)
            sOpenOrClosed = "OPEN";
        else
            sOpenOrClosed = "CLOSED";

        return sOpenOrClosed;
    }
    // =========================================================
    protected string ServerSideVal_RegData()
    {
        string sResult = "";
        int iNum = 0;

        try
        {
            if (vCusRegData.IsValid == true)
            {
                if (txContact.Text == "")
                {
                    vCusRegData.ErrorMessage = "Please enter a contact name";
                    vCusRegData.IsValid = false;
                    txContact.Focus();
                }
                else
                {
                    if (txContact.Text.Length > 30)
                    {
                        vCusRegData.ErrorMessage = "Your contact entry must be 30 characters or less";
                        vCusRegData.IsValid = false;
                        txContact.Focus();
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txPhone1.Text != "")
                {
                    if (int.TryParse(txPhone1.Text, out iNum) == false)
                    {
                        vCusRegData.ErrorMessage = "The area code must be a number";
                        vCusRegData.IsValid = false;
                        txPhone1.Focus();
                    }
                    else
                    {
                        if (txPhone1.Text.Length != 3)
                        {
                            vCusRegData.ErrorMessage = "The area code must be 3 digits";
                            vCusRegData.IsValid = false;
                            txPhone1.Focus();
                        }
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txPhone2.Text != "")
                {
                    if (int.TryParse(txPhone2.Text, out iNum) == false)
                    {
                        vCusRegData.ErrorMessage = "The phone prefix must be a number";
                        vCusRegData.IsValid = false;
                        txPhone2.Focus();
                    }
                    else
                    {
                        if (txPhone2.Text.Length != 3)
                        {
                            vCusRegData.ErrorMessage = "The phone prefix must be 3 digits";
                            vCusRegData.IsValid = false;
                            txPhone2.Focus();
                        }
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txPhone3.Text != "")
                {
                    if (int.TryParse(txPhone3.Text, out iNum) == false)
                    {
                        vCusRegData.ErrorMessage = "The phone suffix must be a number";
                        vCusRegData.IsValid = false;
                        txPhone3.Focus();
                    }
                    else
                    {
                        if (txPhone3.Text.Length != 4)
                        {
                            vCusRegData.ErrorMessage = "The phone suffix must be four digits";
                            vCusRegData.IsValid = false;
                            txPhone3.Focus();
                        }
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txExtension.Text != "")
                {
                    if (int.TryParse(txExtension.Text, out iNum) == false)
                    {
                        vCusRegData.ErrorMessage = "The extension must be a number";
                        vCusRegData.IsValid = false;
                        txExtension.Focus();
                    }
                    else
                    {
                        if (iNum > 99999999)
                        {
                            vCusRegData.ErrorMessage = "The extension may be no more than eight digits";
                            vCusRegData.IsValid = false;
                            txExtension.Focus();
                        }
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txEmail.Text == "")
                {
                    vCusRegData.ErrorMessage = "An email address is required";
                    vCusRegData.IsValid = false;
                    txEmail.Focus();
                }
                else
                {
                    if (txEmail.Text.Length > 50)
                    {
                        vCusRegData.ErrorMessage = "Your email entry must be 50 characters or less";
                        vCusRegData.IsValid = false;
                        txEmail.Focus();
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txEmail2.Text == "")
                {
                    vCusRegData.ErrorMessage = "An email confirmation is required";
                    vCusRegData.IsValid = false;
                    txEmail2.Focus();
                }
                else
                {
                    if (txEmail2.Text.Length > 50)
                    {
                        vCusRegData.ErrorMessage = "Your email confirmation must be 50 characters or less";
                        vCusRegData.IsValid = false;
                        txEmail2.Focus();
                    }
                }
            }

            if (vCusRegData.IsValid == true)
            {
                if (txPassword.Text == "")
                {
                    vCusRegData.ErrorMessage = "A password is required";
                    vCusRegData.IsValid = false;
                    txPassword.Focus();
                }
                else
                {
                    if (txPassword.Text.Length > 15)
                    {
                        vCusRegData.ErrorMessage = "Your password entry must be 15 characters or less";
                        vCusRegData.IsValid = false;
                        txPassword.Focus();
                    }
                }
            }
            if (vCusRegData.IsValid == true)
            {
                if (txPassword2.Text == "")
                {
                    vCusRegData.ErrorMessage = "Password confirmation is required";
                    vCusRegData.IsValid = false;
                    txPassword2.Focus();
                }
                else
                {
                    if (txPassword2.Text.Length > 15)
                    {
                        vCusRegData.ErrorMessage = "Your password confirmation must be 15 characters or less";
                        vCusRegData.IsValid = false;
                        txPassword2.Focus();
                    }
                }
            }

            if (vCusRegData.IsValid == true)
            {
                string sVerdict = sfd.ValidatePassword(txUserID.Text, txPassword.Text);
                if (sVerdict != "VALID")
                {
                    vCusRegData.ErrorMessage = sVerdict;
                    vCusRegData.IsValid = false;
                    txPassword.Focus();
                }
            }
            // -------------------
            if (vCusRegData.IsValid == true)
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            vCusRegData.ErrorMessage = "A unexpected system error has occurred.  The error has been logged and will be corrected soon.  We're sorry for the inconvenience.";
            vCusRegData.IsValid = false;
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }

    // =========================================================
    protected string GetAccessCodeToday()
    {
        string sAccessCode = "";

        if (sPageLib == "L")
        {
            sAccessCode = wsLive.GetRegistrationAccessCode(sfd.GetWsKey());
        }
        else
        {
            sAccessCode = wsTest.GetRegistrationAccessCode(sfd.GetWsKey());
        }

        return sAccessCode;
    }
    // ==========================================================
    // ==========================================================
}