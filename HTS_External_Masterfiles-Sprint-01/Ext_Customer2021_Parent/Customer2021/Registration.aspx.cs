using System;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

public partial class Registration : MyPage 
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    //string sResult = "";

    // ==========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        try
        {
            sqlConn.Open();
            lbError.Text = "";

            if (ShowNewCompanyName() == true)
                lbCustomerNumberInfo.Text = "<i>This is the ID Secur-Serv used to identify your company account</i>";
            else
                lbCustomerNumberInfo.Text = "<i>This is the ID STS uses to identify your company account</i>";

            int iRegistrationAttemptCount = 0;
            if (pnOne.Visible == true) 
            {
                iRegistrationAttemptCount = GetRegistrationAttemptCount();
            }
            if (iRegistrationAttemptCount >= 15)
            {
                // Combine with no other messages, only this text for this error.
                lbError.Text = "Sorry you're having trouble with registration.  Please reach out to <a style='color: blue;' href='mailto:servicecommandsupport@scantron.com'>ServiceCOMMAND Support</a> and ask for help with Customer Registration. (or try again after half an hour)";
                pnOne.Visible = false;
                //pnTwo.Visible = false;
                //pnThree.Visible = false;
            }
            else if (iRegistrationAttemptCount >= 10)
            {
                // Combine with no other messages, only this text for this error.
                lbError.Text = "You seem to be having trouble registering.  If needed, <a style='color: blue;' href='mailto:servicecommandsupport@scantron.com'>ServiceCOMMAND Support</a> can help.";
            }
            else
            {

                // ---------------------------------------------
                if (User.Identity.IsAuthenticated)
                {
                    lbAccessInfo.Visible = false;
                    lbAccess.Visible = false;
                    txCod.Visible = false;
                    Get_UserPrimaryCustomerNumber();
                    
                    if (User.IsInRole("SiteAdministrator") || User.IsInRole("CustomerAdministrator"))
                    {
                        txCs1.Text = hfRegistrationNumber.Value;
                        lbEmailConfirmation.Visible = true;
                    }
                }
                else
                {
                    lbAccessInfo.Visible = true;
                    lbAccess.Visible = true;
                    txCod.Visible = true;
                }

                // ---------------------------------------------
                if (!IsPostBack)
                {
                    txCs1.Focus();
                }
                else
                {
                }
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
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected void checkUserIdAvailability(string userId)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";
        string sUserID = userId.Trim().ToLower();

        try
        {
            sSql = "Select UserName" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users" + 
                " where lower(UserName) = @UserID";
            
            sqlCmd = new SqlCommand(sSql, sqlConn);
            
            sqlCmd.Parameters.AddWithValue("@UserID", sUserID);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                if (dt.Rows.Count == 0)  // name has not yet been taken
                {
                    // Check if they are logged in...
                    if (User.Identity.IsAuthenticated)
                    {
                        chBxGrantAdmin.Checked = false;
                        // Logged in as a privileged user?
                        if (User.IsInRole("CustomerAdministrator") || User.IsInRole("SiteAdministrator"))
                        {
                            lbGrantAdmin.Visible = true;
                            chBxGrantAdmin.Visible = true;
                        }
                    }
                    // Anonymous User
                    else
                    {
                        string[] saCodes = GetAdminAccessCodes().Split('|');
                        if (saCodes.Length > 0 && hfCod.Value.ToLower() == saCodes[0].ToLower())
                            chBxGrantAdmin.Checked = true;
                        else
                            chBxGrantAdmin.Visible = false;
                    }

                    lbUserIDDisplay.Text = sUserID;
                    txFirstName.Focus();
                    pnTwo.Visible = false;
                    pnThree.Visible = true;
                    txEmail.Text = userId;
                    txEmail2.Text = userId;
                }
                else
                {
                    // Since user id's are all known, you can't just allow a reset password if forgotten 
                    lbError.Text = "Login email <b>" + sUserID + "</b> is already registered. If this is your email address, and you remember a possible password, try to log in. Otherwise, you may initiate a password change to that email with a special link to update the account password. Otherwise, the Scantron Contact Center can help";
                }
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
    // ==========================================================
    protected int GetCs1AccountCount(string registrationPrefix, int registrationNumber)
    {
        int iAccountCount = 0;

        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        
        string sUserName = "";
        //int iCustomerNumber = 0;
        //if (int.TryParse(txCs1.Text, out iCustomerNumber) == false)
        //    iCustomerNumber = 0;

        try
        {
            sSql = " select u.UserName" +
                " from " + sSqlDbToUse_Customer + ".aspnet_Users u" +
                " where u.PrimaryCs1 = @PrimaryCs1";
            if (!String.IsNullOrEmpty(registrationPrefix))
                sSql += " and u.PrimaryPrefix = @PrimaryPrefix";
                

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@PrimaryCs1", registrationNumber); // was ToString() (on a numeric?)
            if (!String.IsNullOrEmpty(registrationPrefix))
                sqlCmd.Parameters.AddWithValue("@PrimaryPrefix", registrationPrefix);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    sUserName = row["UserName"].ToString().Trim();
                    iRowIdx++;
                }
                iAccountCount = iRowIdx;
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
        return iAccountCount;
    }
    // =========================================================
    protected int getUserIdCs1(string userName)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        int iCustomerNumber = 0;

        try
        {
            sqlConn.Open();

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
    // ==========================================================
    protected DataTable Select_RegistrationIp()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sIp = GetIp();
        string sSql = "";

        try
        {
            sSql = " select" + 
                 " r.raIp" +
                ", r.raCount" +
                ", r.raTimestamp" +
                " from " + sSqlDbToUse_Customer + ".RegistrationAttempts r" +
                " where r.raIp = @Ip";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Ip", sIp);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(sqlReader);
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
        return dt;
    }
    // ==========================================================
    protected int Insert_RegistrationIp()
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sIp = GetIp();
        string sSql = "";

        try
        {
            sSql = "Insert into " + sSqlDbToUse_Customer + ".RegistrationAttempts" +
                 " (raCount, raTimestamp, raIp)" +
                " values(@Count, @Timestamp, @Ip)";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Count", 1);
            sqlCmd.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
            sqlCmd.Parameters.AddWithValue("@Ip", sIp);

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
    // ==========================================================
    protected int Update_RegistrationIp(int newCount)
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sIp = GetIp();
        string sSql = "";

        try
        {
            sSql = "Update " + sSqlDbToUse_Customer + ".RegistrationAttempts set" +
                 " raCount = @Count" +
                ", raTimestamp = @Timestamp" +
                " where raIp = @Ip";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Count", newCount);
            sqlCmd.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
            sqlCmd.Parameters.AddWithValue("@Ip", sIp);

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
    // ==========================================================
    protected int Delete_RegistrationIp()
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sIp = GetIp();
        string sSql = "";

        try
        {
            sSql = "Delete from " + sSqlDbToUse_Customer + ".RegistrationAttempts" +
                " where raIp = @Ip";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@Ip", sIp);

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
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    protected DataTable ws_Get_B1CustomerRegistrationRecord(string customerNumber) 
    {
        DataTable dt = new DataTable("");

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1CustomerRegistrationRecord";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    protected string ws_Add_B1CustomerRegistrationRecord(string customerNumber, string loginEmail)
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber) && !String.IsNullOrEmpty(loginEmail))
        {
            string sJobName = "Add_B1CustomerRegistrationRecord";
            string sFieldList = "customerNumber|loginEmail|x";
            string sValueList = customerNumber + "|" + loginEmail + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ========================================================================
    protected string ws_Upd_B1CustomerRegistrationRecord(string customerNumber, string loginEmail, string totalRegisteredUsers)
    {
        string sResult = "";

        if (!String.IsNullOrEmpty(customerNumber) && !String.IsNullOrEmpty(totalRegisteredUsers))
        {
            string sJobName = "Upd_B1CustomerRegistrationRecord";
            string sFieldList = "customerNumber|loginEmail|totalRegisteredUsers|x";
            string sValueList = customerNumber + "|" + loginEmail + "|" + totalRegisteredUsers + "|x";

            sResult = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sResult;
    }
    // ========================================================================
    protected string ws_Get_B1LoginAuthorizationForUserCustomerNumber_YN(string customerNumber)
    {
        string sUserCustomerNumberIsAuthorizedToLogin = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "Get_B1LoginAuthorizationForUserCustomerNumber_YN";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber + "|x";

            sUserCustomerNumberIsAuthorizedToLogin = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sUserCustomerNumberIsAuthorizedToLogin;
    }
    // ========================================================================
    protected string ws_AddOrUpd_B1CustPrefForParentLocation(string customerNumber)
    {
        string sCustomerPrefRecStatus = "";

        if (!String.IsNullOrEmpty(customerNumber))
        {
            string sJobName = "AddOrUpd_B1CustPrefForParentLocation";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            sCustomerPrefRecStatus = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCustomerPrefRecStatus;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ----------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iPrimaryNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;

            string[] saPreCs1Typ = Get_UserAccountIds(hfUserName.Value);
            if (saPreCs1Typ.Length > 2) 
            {
                hfRegistrationPrefix.Value = saPreCs1Typ[0];
                hfRegistrationNumber.Value = saPreCs1Typ[1];
                hfRegistrationId.Value = saPreCs1Typ[0] + saPreCs1Typ[1];
                hfRegistrationUserType.Value = saPreCs1Typ[2];

                if (int.TryParse(saPreCs1Typ[1], out iPrimaryNumber) == false)
                    iPrimaryNumber = -1;
            }

            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
                {
                    hfRegistrationId.Value = Session["AdminCustomerNumber"].ToString().Trim();
                    if (
                        hfRegistrationId.Value.StartsWith("S") // ServrightGrandparentToBePaid or ServrightParentProvidingFsts
                        || hfRegistrationId.Value.StartsWith("T") // ServrightChildFst (Tech)
                        )
                    {
                        hfRegistrationPrefix.Value = hfRegistrationId.Value.Substring(0, 1).Trim();
                        if (hfRegistrationId.Value.Length > 1)
                            hfRegistrationNumber.Value = hfRegistrationId.Value.Substring(1).Trim();
                        else
                            hfRegistrationNumber.Value = "";
                    }
                    else // regular non-servright customer
                    {
                        hfRegistrationPrefix.Value = "";
                        hfRegistrationNumber.Value = hfRegistrationId.Value;
                    }
                    
                    if (int.TryParse(hfRegistrationNumber.Value, out iPrimaryNumber) == false)
                        iPrimaryNumber = -1;

                    hfRegistrationUserType.Value = Get_UserCompanyTypeById(hfRegistrationPrefix.Value, iPrimaryNumber);

                }
                // ----------------------------------------------------------------
            }
        }
    }
    // ----------------------------------------------------------------------------
    protected string Validate_UserIdPanel(string userId)
    {
        string sUserIdFormatIsValid = "N";

        string sUserID = userId.Trim();
        lbError.Text = "";
        if (sUserID == "")
        {
            lbError.Text = "A login email is required";
            txUserID.Focus();
        }
        else
        {
            int iUserIDLength = sUserID.Length;
            if (iUserIDLength < 10 || iUserIDLength > 50)
            {
                lbError.Text = "The login email address must be from 10 to 50 characters";
                txUserID.Focus();
            }
            else if (isEmailFormatValid(sUserID) != true)
            {
                lbError.Text = "User login name must be a valid email address.";
            }
            else if (sUserID.IndexOf(" ") != -1)
            {
                lbError.Text = "The login email may not contain blanks";
                txUserID.Focus();
            }
        }
        if (String.IsNullOrEmpty(lbError.Text))
            sUserIdFormatIsValid = "Y";

        return sUserIdFormatIsValid;
    }
    // ----------------------------------------------------------------------------
    protected string Validate_RegistrationDataPanel()
    {
        string sIsRegistrationDataValid = "N";
        //string sResult = "";
        int iNum = 0;
        lbError.Text = "";
        txPhone1.Text = "000";
        txPhone2.Text = "000";
        txPhone3.Text = "0000";

        try
        {
            if (txFirstName.Text == "")
            {
                lbError.Text += "Please enter your first name<br />";
                txFirstName.Focus();
            }
            else
            {
                if (txFirstName.Text.Length > 50)
                {
                    lbError.Text += "Your first name entry must be 50 characters or less<br />";
                    txFirstName.Focus();
                }
            }

            if (txLastName.Text == "")
            {
                lbError.Text += "Please enter your last name<br />";
                txLastName.Focus();
            }
            else
            {
                if (txLastName.Text.Length > 50)
                {
                    lbError.Text += "Your last name entry must be 50 characters or less<br />";
                    txLastName.Focus();
                }
            }

            if (txPhone1.Text != "")
            {
                if (int.TryParse(txPhone1.Text, out iNum) == false)
                {
                    lbError.Text += "The area code must be a number<br />";
                    txPhone1.Focus();
                }
                else
                {
                    if (txPhone1.Text.Length != 3)
                    {
                        lbError.Text += "The area code must be 3 digits<br />";
                        txPhone1.Focus();
                    }
                }
            }

            if (txPhone2.Text != "")
            {
                if (int.TryParse(txPhone2.Text, out iNum) == false)
                {
                    lbError.Text += "The phone prefix must be a number<br />";
                    txPhone2.Focus();
                }
                else
                {
                    if (txPhone2.Text.Length != 3)
                    {
                        lbError.Text += "The phone prefix must be 3 digits<br />";
                        txPhone2.Focus();
                    }
                }
            }

            if (txPhone3.Text != "")
            {
                if (int.TryParse(txPhone3.Text, out iNum) == false)
                {
                    lbError.Text += "The phone suffix must be a number<br />";
                    txPhone3.Focus();
                }
                else
                {
                    if (txPhone3.Text.Length != 4)
                    {
                        lbError.Text += "The phone suffix must be four digits<br />";
                        txPhone3.Focus();
                    }
                }
            }

            if (txExtension.Text != "")
            {
                if (int.TryParse(txExtension.Text, out iNum) == false)
                {
                    lbError.Text += "The extension must be a number<br />";
                    txExtension.Focus();
                }
                else
                {
                    if (iNum > 99999999)
                    {
                        lbError.Text += "The extension may be no more than eight digits<br />";
                        txExtension.Focus();
                    }
                }
            }

            if (txEmail.Text == "")
            {
                lbError.Text += "An email address is required<br />";
                txEmail.Focus();
            }
            else
            {
                if (txEmail.Text.Length > 50)
                {
                    lbError.Text += "Your email entry must be 50 characters or less<br />";
                    txEmail.Focus();
                }
            }

            if (txEmail2.Text == "")
            {
                lbError.Text += "An email confirmation is required<br />";
                txEmail2.Focus();
            }
            else
            {
                if (txEmail2.Text.Length > 50)
                {
                    lbError.Text += "Your email confirmation must be 50 characters or less<br />";
                    txEmail2.Focus();
                }
            }

            if (txPassword.Text == "")
            {
                lbError.Text += "A password is required<br />";
                txPassword.Focus();
            }
            else
            {
                if (txPassword.Text.Length < 7 || txPassword.Text.Length > 30)
                {
                    lbError.Text += "Your password entry must be between 7 and 30 characters<br />";
                    txPassword.Focus();
                }
            }

            if (txPassword2.Text == "")
            {
                lbError.Text += "Password confirmation is required<br />";
                txPassword2.Focus();
            }
            else
            {
                if (txPassword2.Text.Length < 7 || txPassword2.Text.Length > 30)
                {
                    lbError.Text += "Your password confirmation entry must be between 7 and 30 characters<br />";
                    txPassword2.Focus();
                }
            }

            string sIsPasswordFormatValid = ValidatePassword(txUserID.Text, txPassword.Text);
            if (sIsPasswordFormatValid != "Y")
            {
                lbError.Text += sIsPasswordFormatValid;
                txPassword.Focus();
            }

            // -------------------
            if (lbError.Text == "")
            {
                sIsRegistrationDataValid = "Y";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            lbError.Text = "A unexpected system error has occurred.  The error has been logged and will be corrected soon.  We're sorry for the inconvenience.<br />";
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        // --------------------------------
        return sIsRegistrationDataValid;
    }
    // ----------------------------------------------------------------------------
    protected int GetRegistrationAttemptCount()
    {
        int iRegistrationAttemptCount = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        //string sIp = ""; // You don't NEED the ip, it can always be retrieved in the queries
        int iRowsAffected = 0;
        string sDat = "";
        DateTime datTemp;
        dt = Select_RegistrationIp();

        if (dt.Rows.Count > 0)
        {
            if (int.TryParse(dt.Rows[0]["raCount"].ToString(), out iRegistrationAttemptCount) == false)
                iRegistrationAttemptCount = 0;
            iRegistrationAttemptCount++;
            iRowsAffected = Update_RegistrationIp(iRegistrationAttemptCount);
            sDat = dt.Rows[0]["raTimestamp"].ToString();
            if (DateTime.TryParse(dt.Rows[0]["raTimestamp"].ToString(), out datTemp) == false)
                datTemp = new DateTime();
            else
            {
                // If they tried over 30 minutes ago, clear the record and let them try again
                if (datTemp < DateTime.Now.AddMinutes(-30))
                {
                    iRowsAffected = Delete_RegistrationIp();
                    iRowsAffected = Insert_RegistrationIp();
                }
            }
        }
        else
        {
            iRowsAffected = Insert_RegistrationIp();
        }

        return iRegistrationAttemptCount;
    }
    // ----------------------------------------------------------------------------
    protected void Clear_RegistrationWarningIfNeeded()
    {
        DataTable dt = Select_RegistrationIp();
        int iRegistrationAttemptCount = 0;

        if (dt.Rows.Count > 0)
        {
            if (int.TryParse(dt.Rows[0]["raCount"].ToString(), out iRegistrationAttemptCount) == false)
                iRegistrationAttemptCount = 0;
            if (iRegistrationAttemptCount == 5 || iRegistrationAttemptCount == 6)
                lbError.Text = "";
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ==========================================================
    protected void btRegId_Click(object sender, EventArgs e)
    {
        string sCustomerType = "";
        string sCustomerName = "";
        string sUserAccessCodeEntry = "";
        string sIsUserRegistrationForCustomerOpenOrClosed = "";
        string sAdminBypass = "";
        string sAdminCodeCorrect = "";
        string sUserCustomerNumberIsAuthorizedToLogin = "";
        //string sRegistrationId = "";
        //string sRegistrationPrefix = "";
        //string sRegistrationNumber = "";
        // Validate Numeric
        int iAccountCount = 0;


        // ----------------------------------------------------------------
        // Parse User Entry to see if they added a prefix for Servright
        int iRegistrationNumber = 0;


        if (!String.IsNullOrEmpty(txCs1.Text.Trim()))
            hfRegistrationId.Value = txCs1.Text.Trim();
        else
            hfRegistrationId.Value = "";

        if (
               hfRegistrationId.Value.StartsWith("S") // ServrightGrandparentToBePaid or ServrightParentProvidingFsts
            || hfRegistrationId.Value.StartsWith("T") // ServrightChildFst (Tech)
            )
        {
            hfRegistrationPrefix.Value = hfRegistrationId.Value.Substring(0, 1).Trim();
            if (hfRegistrationId.Value.Length > 1)
                hfRegistrationNumber.Value = hfRegistrationId.Value.Substring(1).Trim();
            else
                hfRegistrationNumber.Value = "";
        }
        else 
        {
            hfRegistrationNumber.Value = hfRegistrationId.Value.Trim();
        }
        // ----------------------------------------------------------------
        if (!String.IsNullOrEmpty(hfRegistrationNumber.Value))
        {
            if (int.TryParse(hfRegistrationNumber.Value, out iRegistrationNumber) == false)
                iRegistrationNumber = 0;
        }

        hfCod.Value = txCod.Text.Trim();
        sUserAccessCodeEntry = hfCod.Value;

        try
        {
            sqlConn.Open();

            //Clear_RegistrationWarningIfNeeded();

            if (isAPositiveInteger(hfRegistrationNumber.Value.Trim()) == false)
                lbError.Text = "Account ID does not contain a positive integer.";
            else if (iRegistrationNumber > 9999999)
            {
                if (!String.IsNullOrEmpty(lbError.Text)) lbError.Text += "<br />";
                lbError.Text += "Our account IDs have a maximum of 7 digits.";
            }
            else
            {
                //if (sRegistrationPrefix == "S" || sRegistrationPrefix == "T")
                sUserCustomerNumberIsAuthorizedToLogin = ws_Get_B1LoginAuthorizationForUserCustomerNumber_YN(hfRegistrationId.Value);

                if (sUserCustomerNumberIsAuthorizedToLogin != "Y")
                {
                    if (!String.IsNullOrEmpty(lbError.Text)) lbError.Text += "<br />";
                    lbError.Text += "Your entry does not match an active account ID.";
                }
            }

            if (String.IsNullOrEmpty(lbError.Text))
            {
                // check for prior accounts
                iAccountCount = GetCs1AccountCount(hfRegistrationPrefix.Value, iRegistrationNumber);

                // Check if registration is open to multiple basic accounts
                if (hfRegistrationPrefix.Value == "S" || hfRegistrationPrefix.Value == "T")
                    sIsUserRegistrationForCustomerOpenOrClosed = "open";
                else 
                    sIsUserRegistrationForCustomerOpenOrClosed = ws_Get_B1CustPref_UserRegistrationForCustomer_OpenOrClosed(iRegistrationNumber);

                // Check if they have entered a valid admin access code
                if (sUserAccessCodeEntry != "")
                {
                    string[] saCodes = GetAdminAccessCodes().Split('|');
                    if (saCodes.Length > 0 && sUserAccessCodeEntry.ToLower() == saCodes[0].ToLower())
                        sAdminCodeCorrect = "YES";
                    else
                        sAdminCodeCorrect = "NO";
                }
                // Check if they are logged in as a privileged user
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("SiteAdministrator") || User.IsInRole("CustomerAdministrator"))
                    {
                        sAdminBypass = "YES";
                    }
                }
                // Get Cust Name and Type:
                // REG Regular
                // LRG Large Customer
                // DLR Deals
                // SSP Self Service Parts
                // SSB Self Service Both tickets and parts
                // SUB Sub-Contractor
                // BAK Back Door Access
                // SRG ServrightGrandparentToBePaid
                // SRP ServrightParentProvidingFsts
                // SRC ServrightChildFst
                
                if (iRegistrationNumber > 0)
                {
                    // xxxxx
                    sCustomerType = ws_Get_B1CustomerTypeForSqlUserTable(hfRegistrationPrefix.Value, iRegistrationNumber);
                    hfRegistrationUserType.Value = sCustomerType;
                    //sCustomerName = ws_Get_B1CustomerName(iRegistrationNumber.ToString(), "");
                    sCustomerName = ws_Get_B1AccountName(hfRegistrationPrefix.Value, hfRegistrationNumber.Value, "");
                }

                // DO VALIDATION ==========================
                // No customer for number entered 
                if (sCustomerType == "")
                {
                    //vCusRegId.ErrorMessage = "Access was not granted.<br>Please recheck your entries.";
                    if (!String.IsNullOrEmpty(lbError.Text)) lbError.Text += "<br />";
                    lbError.Text += "Your customer number entry is invalid.";
                }
                // Admin code entered, but BAD
                else if (sAdminCodeCorrect == "NO")
                {
                    if (!String.IsNullOrEmpty(lbError.Text)) lbError.Text += "<br />";
                    lbError.Text += "Your admin access code entry is not valid." +
                        "<br />Please reach out to servicecommandsupport@scantron.com to allow access.";
                }
                // Account already exists, it's closed and no bypass authority
                else if (
                       iAccountCount >= 1 // Someone has already registered for this customer (hopefully a customer site admin) 
                    && sIsUserRegistrationForCustomerOpenOrClosed.ToLower() != "open"  // not open
                    && (sAdminBypass != "YES"  // not an admin registering for another 
                        || sAdminCodeCorrect == "NO") // or someone who TRIED entering the admin pass code, but it wasn't right
                    
                    )
                {
                    if (!String.IsNullOrEmpty(lbError.Text)) lbError.Text += "<br />";
                    lbError.Text += "New account registration for this customer is currently closed." +
                        "<br />Those at your company with admin account privileges can open registration for additional accounts if needed.";
                }
                else
                {
                    // ACCESS GRANTED -- Continue...
                    lbStep2Cs1.Text = hfRegistrationId.Value + "&nbsp;&nbsp;" + sCustomerName;

                    txUserID.Focus();
                    pnOne.Visible = false;
                    pnTwo.Visible = true;
                    // Delete the registration failure counter
                    int iRowsAffected = Delete_RegistrationIp();

                }
            } // status checked...
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
    // ==========================================================
    protected void btUserID_Click(object sender, EventArgs e)
    {
        string sUserId = txUserID.Text.Trim();

        try 
        {
            sqlConn.Open();

            string sUserIdFormatIsValid = Validate_UserIdPanel(sUserId);

            if (sUserIdFormatIsValid == "Y") 
                checkUserIdAvailability(sUserId);
               
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
    // ==========================================================
    protected void btRegistrationSubmission_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        int iRegistrationNumber = 0;
        if (int.TryParse(hfRegistrationNumber.Value, out iRegistrationNumber) == false)
            iRegistrationNumber  = 0;
        
        int iRowsAffected = 0;

        string sSql = "";
        //string sRegistrationPrefix = hfRegistrationPrefix.Value;
        //string sRegistrationNumber = hfRegistrationNumber.Value;
        string sUserID = txUserID.Text.Trim();
        string sPassword = txPassword.Text.Trim();
        string sEmail = txUserID.Text.Trim(); // Always use the value in the user id


        //string sCustomerType = "";
        //string sResult = "";
        string sIsDataValid = "";
        //DateTime datTemp = new DateTime();

        sIsDataValid = Validate_RegistrationDataPanel();

        // If all is well, process...
        if (sIsDataValid == "Y")
        {
            DateTime datConfirmationDeadline = DateTime.Now.ToUniversalTime().AddMinutes(15);
            string sConfirmationCode = Make_ConfirmationCode(sUserID);

            //sCustomerType = ws_Get_B1CustomerType(iCustomerNumber);
            //sCustomerType = ws_Get_B1CustomerTypeForSqlUserTable(hfRegistrationPrefix.Value, iRegistrationNumber);

            string sFirstName = txFirstName.Text.Trim();
            string sLastName = txLastName.Text.Trim();
            string sPhone1 = txPhone1.Text.Trim();
            string sPhone2 = txPhone2.Text.Trim();
            string sPhone3 = txPhone3.Text.Trim();
            string sPhone = "(" + sPhone1 + ") " + sPhone2 + "-" + sPhone3;
            string sExtension = txExtension.Text.Trim();
            //string sEmail = txEmail.Text.Trim();
            string sRole = "";
            string sErrValues = "";
            string sCreatorType = "Anonymous";
            string sUserUrl = "";

            // First: Trying to create member record
            try
            {
                
                Membership.CreateUser(sUserID, sPassword, sEmail);

                // If successful, Second: Trying to load the database
                try
                {
                    sqlConn.Open();

                    // Create New User
                    if (User.Identity.IsAuthenticated)
                        sCreatorType = "AdministratorCreatingForAnother";

                    if (chBxGrantAdmin.Checked == true)
                    {
                        if (sUserID.ToLower() == "steve.carlson@scantron.com"
                            || sUserID.ToLower() == "isabel.labrador@scantron.com"
                            || sUserID.ToLower() == "sarah.engels@scantron.com"
                            || sUserID.ToLower() == "vern.kathol@scantron.com"
                            )
                            sRole = "SiteAdministrator";
                        else
                            sRole = "CustomerAdministrator";
                    }

                    if (hfRegistrationUserType.Value == "SRG")
                        sRole = "ServrightGrandparentToBePaid";
                    else if (hfRegistrationUserType.Value == "SRP")
                        sRole = "ServrightParentProvidingFsts";
                    else if (hfRegistrationUserType.Value == "SRC")
                        sRole = "ServrightChildFst";

                    // Assign user to proper role
                    if (sRole != "")
                        Roles.AddUserToRole(sUserID, sRole);

                    // Set Customer Preference File
                    string sCustomerPrefRecStatus = ws_AddOrUpd_B1CustPrefForParentLocation(hfRegistrationNumber.Value);

                    sSql = "UPDATE " + sSqlDbToUse_Customer + ".aspnet_Users SET" +
                            " CompanyType=@CompanyType" +
                        ", FirstName=@FirstName" +
                        ", LastName=@LastName" +
                        ", Phone=@Phone" +
                        ", PhoneAreaCode=@PhoneAreaCode" +
                        ", PhonePrefix=@PhonePrefix" +
                        ", PhoneSuffix=@PhoneSuffix" +
                        ", PhoneExtension=@PhoneExtension" +
                        ", Email=@Email" +
                        ", LoginCount=@LoginCount" +
                        ", PrimaryPrefix=@PrimaryPrefix" +
                        ", PrimaryCs1=@PrimaryCs1" +
                        ", RegistrationDate=@RegistrationDate" +
                        ", EmailConfirmed=@EmailConfirmed" +
                        ", ConfirmationCode=@ConfirmationCode" +
                        ", ConfirmationDeadline=@ConfirmationDeadline" +
                        " WHERE UserName=@UserID";
                    sqlCmd = new SqlCommand(sSql, sqlConn);

                    if (!String.IsNullOrEmpty(hfRegistrationUserType.Value) && hfRegistrationUserType.Value.Length > 15)
                        hfRegistrationUserType.Value = hfRegistrationUserType.Value.Substring(0, 15);
                    if (!String.IsNullOrEmpty(sFirstName) && sFirstName.Length > 50)
                        sFirstName = sFirstName.Substring(0, 50);
                    if (!String.IsNullOrEmpty(sLastName) && sLastName.Length > 50)
                        sLastName = sLastName.Substring(0, 50);
                    if (!String.IsNullOrEmpty(sPhone) && sPhone.Length > 15)
                        sPhone = sPhone.Substring(0, 15);
                    if (!String.IsNullOrEmpty(sPhone1) && sPhone1.Length > 3)
                        sPhone1 = sPhone1.Substring(0, 3);
                    if (!String.IsNullOrEmpty(sPhone2) && sPhone2.Length > 3)
                        sPhone2 = sPhone2.Substring(0, 3);
                    if (!String.IsNullOrEmpty(sPhone3) && sPhone3.Length > 4)
                        sPhone3 = sPhone3.Substring(0, 4);
                    if (!String.IsNullOrEmpty(sExtension) && sExtension.Length > 8)
                        sExtension = sExtension.Substring(0, 8);
                    if (!String.IsNullOrEmpty(sEmail) && sEmail.Length > 50)
                        sEmail = sEmail.Substring(0, 50);
                    if (!String.IsNullOrEmpty(sUserID) && sUserID.Length > 256)
                        sUserID = sUserID.Substring(0, 256);

                    sqlCmd.Parameters.AddWithValue("@CompanyType", hfRegistrationUserType.Value);
                    sqlCmd.Parameters.AddWithValue("@FirstName", sFirstName);
                    sqlCmd.Parameters.AddWithValue("@LastName", sLastName);
                    sqlCmd.Parameters.AddWithValue("@Phone", sPhone);
                    sqlCmd.Parameters.AddWithValue("@PhoneAreaCode", sPhone1);
                    sqlCmd.Parameters.AddWithValue("@PhonePrefix", sPhone2);
                    sqlCmd.Parameters.AddWithValue("@PhoneSuffix", sPhone3);
                    sqlCmd.Parameters.AddWithValue("@PhoneExtension", sExtension);
                    sqlCmd.Parameters.AddWithValue("@Email", sEmail);
                    sqlCmd.Parameters.AddWithValue("@LoginCount", 1);
                    sqlCmd.Parameters.AddWithValue("@PrimaryPrefix", hfRegistrationPrefix.Value);
                    sqlCmd.Parameters.AddWithValue("@PrimaryCs1", iRegistrationNumber);
                    sqlCmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now.ToString());
                    sqlCmd.Parameters.AddWithValue("@EmailConfirmed", 0);
                    sqlCmd.Parameters.AddWithValue("@ConfirmationCode", sConfirmationCode);
                    sqlCmd.Parameters.AddWithValue("@ConfirmationDeadline", datConfirmationDeadline);
                    sqlCmd.Parameters.AddWithValue("@UserID", sUserID);

                    iRowsAffected = sqlCmd.ExecuteNonQuery();
                    // ------------------------------ 
                    if (iRowsAffected > 0)
                    {
                        sUserUrl = HttpContext.Current.Request.Url.ToString().ToLower();
                        // sUserUrl.Replace("http:", "https:"); The port would change...
                        int iPos = sUserUrl.IndexOf("/registration.aspx");
                        if (iPos > -1)
                        {
                            sUserUrl = sUserUrl.Substring(0, iPos) + "/public/confirmation.aspx?code=" + sConfirmationCode;
                        }
                        // Email confirmation
                        EmailHandler emailHandler = new EmailHandler();

                        string sSubject = "";
                        if (ShowNewCompanyName() == true)
                            sSubject = "Account Email Confirmation (Secur-Serv)";
                        else
                            sSubject = "Account Email Confirmation (Scantron Technology Solutions)";

                        string sMessage = "To confirm your ServiceCOMMAND® account email address" +
                            "<br />" +
                            "please <a href='" + sUserUrl + "'>Click Here</a>" +
                        //"<br /><br /> <i>(The link will be valid only for the next 15 minutes)</i>";
                            "<br /><br /><ul>" +
                            "<li>Each requested confirmation link is unique</li>" +
                            "<li>Only the most recent will be valid</li>" +
                            "<li>Access will expire in 15 minutes</li>" + 
                            "</ul>";
                        string sEmailTo = sUserID;
                        string sEmailFrom = "adv320@scantron.com";
                        string sEmailResult = emailHandler.EmailIndividual(sSubject, sMessage, sEmailTo, sEmailFrom);
                        emailHandler = null;

                        // ----------------------------------------------------------------------------------------
                        // Update Registration Log file with this successful registration
                        // ----------------------------------------------------------------------------------------
                        string sResult = "";
                        int iTotalRegisteredUsers = 0;
                        // 1) Check if record for this customer id exists
                        dt = ws_Get_B1CustomerRegistrationRecord(iRegistrationNumber.ToString());
                        if (dt.Rows.Count > 0) 
                        {
                            // 2) If it does, update for 1st or later users
                            string sFirstRegisteredUserName = dt.Rows[0]["CRUSRNAM"].ToString().Trim();

                            if (int.TryParse(dt.Rows[0]["CRREGTOT"].ToString().Trim(), out iTotalRegisteredUsers) == false)
                                iTotalRegisteredUsers = -1;
                            if (!String.IsNullOrEmpty(sFirstRegisteredUserName) && iTotalRegisteredUsers > 0)
                            {
                                // Update for later user registration (Don't pass the current loginEmail because this is NOT the first)
                                iTotalRegisteredUsers++;
                                sResult = ws_Upd_B1CustomerRegistrationRecord(iRegistrationNumber.ToString(), "", iTotalRegisteredUsers.ToString());
                            }
                            else 
                            {
                                // Update for first user registration
                                iTotalRegisteredUsers = 1;
                                sResult = ws_Upd_B1CustomerRegistrationRecord(iRegistrationNumber.ToString(), sUserID, iTotalRegisteredUsers.ToString());
                            }
                        }
                        else 
                        {
                            // 3) If it does not, insert for 1st user. 
                            sResult = ws_Add_B1CustomerRegistrationRecord(iRegistrationNumber.ToString(), sUserID);
                        }


                        // ----------------------------------------------------------------------------------------
                    }
                    else 
                    {
                        lbError.Text = "Error: Sorry!  Something went wrong creating this user account";
                    }

                    // ------------------------------ 
                }
                catch (Exception ex)
                {
                    sErrValues = "Error loading registration details into sql database -- ID: " + sUserID + "  PWD: " + sPassword + "   EML: " + sEmail + " Error: " + ex.ToString();
                    SaveError(ex.Message.ToString(), ex.ToString(), sErrValues);
                    lbError.Text = "Error loading registration details into sql database<br />";
                }
                finally
                {
                    if (sqlCmd != null)
                        sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }
            catch (MembershipCreateUserException ex2)
            {
                sErrValues = "ID: " + sUserID + "  PWD: " + sPassword + "   EML: " + sEmail + " Error: " + ex2.ToString();
                SaveError(ex2.Message.ToString(), ex2.ToString(), sErrValues);
                //lbError.Text = "Error Creating New User: " + ex2.ToString();
                lbError.Text = "Error Creating New User: This registration issue has been logged and will be corrected soon.";
            }

            if (String.IsNullOrEmpty(lbError.Text))
            {
                if (sCreatorType == "AdministratorCreatingForAnother") // Show admin them the newly created account
                    Response.Redirect("~/private/customerAdministration/UserMaintenance.aspx", false);
                else 
                {
                    FormsAuthentication.SignOut(); // Sign out so they won't get all the menu options
                    Response.Redirect("~/public/ConfirmationPending.aspx", false);
                }
                
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                //FormsAuthentication.RedirectFromLoginPage(sUserID, false);
            }
        }
    }
    // ========================================================================
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================


    // ==========================================================
    // ==========================================================
}