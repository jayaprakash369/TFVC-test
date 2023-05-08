using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.IO;
using System.Data.Odbc;
// using System.Data.SqlClient;

using System.Net.Mail;

public partial class public_Suggestions : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
//    EmployeeSite_DEV.EmployeeSiteTESTData wsTest = new EmployeeSite_DEV.EmployeeSiteTESTData();
//    EmployeeSite_LIVE.EmployeeSiteLIVEData wsLive = new EmployeeSite_LIVE.EmployeeSiteLIVEData();

//    SqlCommand sqlCmd;
//    SqlConnection sqlConn;
//    SqlDataReader sqlReader;

    OdbcCommand odbcCmd;
    OdbcConnection odbcConn;
    OdbcDataReader odbcReader;

    DateTime datTemp;

    string sConnectionString = "";
    string sSql = "";
    string sMethodName = "";

    ErrorHandler eh;
    //string sLibrary = "OMDTALIB";
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
        eh = new ErrorHandler();
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e)
    {
        eh = null;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("jsGetXY", "../public/js/GetXY.js");
        Page.ClientScript.RegisterClientScriptInclude("jsSetFloatBoxPos", "../public/js/SetFloatBoxPos.js");

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack) 
        {
            if (Request.Form["emp"] != null && Request.Form["emp"] != "")
            {
                hfEmp.Value = Request.Form["emp"].Trim();
            }
            else 
            {
                if (sLibrary == "OMDTALIB")
                {
                    lbAccess.Text = "<br />No employee credentials were found.  <br /><br />Please access this utility using the link on the Info Center Utility Page";
                    lbAccess.Visible = true;
                    pnAccess.Visible = false;
                    btNewEntry.Visible = false;
                    hfEmp.Value = "0";
                }
                else 
                {
                    hfEmp.Value = "1862";
                }
            }

            try
            {
                odbcConn.Open();

                ddAssigned.DataSource = getMgrsForAssignment();
                ddAssigned.DataTextField = "empnam";
                ddAssigned.DataValueField = "empnum";
                ddAssigned.DataBind();
                // ddAssigned.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));

                ViewState["vsDataTable_Com"] = null;
                BindGrid_Com();
                if (Request.Form["key"] != null && Request.Form["key"] != "")
                {
                    int iEntryKey = 0;
                    int.TryParse(Request.Form["key"].Trim(), out iEntryKey);
                    if (iEntryKey > 0) 
                    {
                        displayComment(iEntryKey);
                    }
                }
            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
            }
        }
    }
        // ========================================================================
    protected void displayComment(int key)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        string sDate = "";

        pnViewEntry.Visible = true;

        try
        {
            dataTable = getComment(key);

            if (dataTable.Rows.Count == 1)
            {
                hfEditKey.Value = dataTable.Rows[0]["cmkey"].ToString().Trim();
                lbSubmitter.Text = dataTable.Rows[0]["cm1emp"].ToString().Trim() + " " + dataTable.Rows[0]["cm1nam"].ToString().Trim();
                lbAssigned.Text = dataTable.Rows[0]["cm2emp"].ToString().Trim() + " " + dataTable.Rows[0]["cm2nam"].ToString().Trim();

                sDate = dataTable.Rows[0]["cm1dat"].ToString().Trim();
                datTemp = Convert.ToDateTime(sDate);
                lbSubmitDate.Text = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();

                lbKey.Text = dataTable.Rows[0]["cmkey"].ToString().Trim();
                lbTitle.Text = dataTable.Rows[0]["cm1tit"].ToString().Trim();
                lbComment.Text = dataTable.Rows[0]["cm1txt"].ToString().Trim();
                lbResponse.Text = dataTable.Rows[0]["cm2txt"].ToString().Trim();
                txNewResponse.Text = dataTable.Rows[0]["cm2txt"].ToString().Trim();
                int iAssigned = 0;
                int.TryParse(dataTable.Rows[0]["cm2emp"].ToString(), out iAssigned);
                string sAssigned = dataTable.Rows[0]["cm2nam"].ToString().Trim();

                dataTable = getUserData("Is Assigned Emp In Current List", iAssigned);
                // If currently assigned employee is still a TC2 (reassignment emp) they'll be in ddAssigned, so just select them.
                if (dataTable.Rows.Count == 1)
                    ddAssigned.SelectedValue = dataTable.Rows[0]["mgrNum"].ToString().Trim();
                // Currently assigned employee is NOT a TC2 and won't be there to be selected, reload, then add them to top of list.
                else 
                {
                    hfReloadAssignments.Value = "RELOAD";
                    ddAssigned.DataSource = getMgrsForAssignment();
                    ddAssigned.DataTextField = "empnam";
                    ddAssigned.DataValueField = "empnum";
                    ddAssigned.DataBind();
                    ddAssigned.Items.Insert(0, new System.Web.UI.WebControls.ListItem(sAssigned, iAssigned.ToString()));
                    ddAssigned.SelectedValue = iAssigned.ToString();
                }
            }
            else
            {
                hfEditKey.Value = "";
                lbSubmitter.Text = "";
                lbAssigned.Text = "";
                lbSubmitDate.Text = "";
                lbTitle.Text = "";
                lbComment.Text = "";
                lbResponse.Text = "";
                txNewResponse.Text = "";
                ddAssigned.SelectedValue = "";
            }
            int iUser = 0;
            int.TryParse(hfEmp.Value, out iUser);
            string sAdmin = isUserAdmin(iUser);
            
            // Set screen for normal user
            lbAssigned.Visible = true;
            ddAssigned.Visible = false;
            lbResponse.Visible = true;
            txNewResponse.Visible = false;
            btResponse.Visible = false;

            if (sAdmin == "YES" || hfEmp.Value == ddAssigned.SelectedValue)
            {
                lbAssigned.Visible = false;
                ddAssigned.Visible = true;
                if (hfEmp.Value == ddAssigned.SelectedValue)
                {
                    lbResponse.Visible = false;
                    txNewResponse.Visible = true;
                    btResponse.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
    }
    // =========================================================
    protected void sendEmail(int key, string task)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        string sAddressFrom = "adv320@harlandts.com";
        string sSbj = "";
        string sMsg = "";
        string sAddress = "";
        string sEmpName = "";
        string sMgrName = "";
        string sTitle = "";
        string sComment = "";
        string sResponse = "";
        string sDate = "";
        string sEntryDate = "";
        string sAssignDate = "";
        string sResponseDate = "";
        string sDuplCheck = "";

        int iEmpNum = 0;
        int iMgrNum = 0;
        int iKey = 0;

        MailAddress toAddress;
        MailAddress fromAddress;

        try
        {
            dataTable = getComment(key);

            if (dataTable.Rows.Count > 0)
            {
                if (int.TryParse(dataTable.Rows[0]["cmkey"].ToString(), out iKey) == false)
                    iKey = 0;

                if (int.TryParse(dataTable.Rows[0]["cm1emp"].ToString(), out iEmpNum) == false)
                    iEmpNum = 0;

                if (int.TryParse(dataTable.Rows[0]["cm2emp"].ToString(), out iMgrNum) == false)
                    iMgrNum = 0;

                sEmpName = dataTable.Rows[0]["cm1nam"].ToString().Trim();
                sMgrName = dataTable.Rows[0]["cm2nam"].ToString().Trim();

                sTitle = dataTable.Rows[0]["cm1tit"].ToString().Trim();
                sComment = dataTable.Rows[0]["cm1txt"].ToString().Trim();
                sResponse = dataTable.Rows[0]["cm2txt"].ToString().Trim();

                sDate = dataTable.Rows[0]["cm1dat"].ToString().Trim();
                datTemp = Convert.ToDateTime(sDate);
                sEntryDate = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();

                sAssignDate = dataTable.Rows[0]["cm2dat"].ToString().Trim();
                if (sDate != "")
                {
                    datTemp = Convert.ToDateTime(sDate);
                    sAssignDate = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                }

                sResponseDate = dataTable.Rows[0]["cm3dat"].ToString().Trim();
                if (sDate != "")
                {
                    datTemp = Convert.ToDateTime(sDate);
                    sResponseDate = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                }
            }

            sSbj = "SUGGESTION: " + task;

            // Build HTML Email Content

            sMsg = "<html><head><title>Suggestion or Comment: " + task + "</title>" +
            "<style>" +
            "body { font-family: verdana; font-size: 13px; margin-left: 40px; color: #444444; }" +
            "table { font-family: verdana; }" +
            "th { font-size: 14px; padding: 4px; }" +
            "td { font-size: 13px; padding: 5px; }" +
            "</style>" +
            "</head><body>";

            sMsg += "<div style=\"padding: 15px; margin: 20px; border: 1px solid #dddddd; float: left; \">"; // BEE4B4
            sMsg += "<table style=\"width: 700px; max-width: 700px; \" >";
            sMsg += "<tr><td style=\"font-family: Verdana; color: #3a7728; font-size: 17px; font-weight: normal; \">" + sTitle + "</td></tr>"; //5F9EA0
            sMsg += "<tr><td style=\"padding-top: 7px; font-family: Verdana; color: #333333; font-weight: bold; \">" + iEmpNum.ToString() + " " + sEmpName +  "</td></tr>";
            sMsg += "<tr><td style=\"padding-top: 7px; font-family: Verdana; color: #333333; font-weight: normal; \">" + sEntryDate + "</td></tr>";
            sMsg += "<tr><td style=\"padding-top: 7px; font-family: Verdana; color: #333333; font-weight: normal; \">Key: " + iKey.ToString() + "</td></tr>";
            sMsg += "<tr><td style=\"padding-top: 10px; font-size: 14px;\">" + sComment + "</td></tr>";
            sMsg += "</table>";
            sMsg += "</div>";
           
            sMsg += "<div style=\"clear: both; margin-left: 40px; margin-top: 5px; float: left; width: 700px; max-width: 700px; \">";
            if (task == "New Submittal" || task == "Assignment Change")
            {
                sMsg += "<b>Assigned: " + iMgrNum.ToString() + " " + sMgrName + "</b>";
                sMsg += "<div style=\"clear: both; height: 5px;\"></div>";
                sMsg += "<a href=\"https://199.186.132.225/intranet/ssg/suggestions.asp?key=" + key.ToString() + "\" >Review Suggestion or Comment</a>";
            }
            if (task == "New Response")
            {
                sMsg += "<b>RESPONSE: " + iMgrNum.ToString() + " " + sMgrName + "</b>";
                sMsg += "<div style=\"clear: both; height: 5px;\"></div>";
                sMsg += sResponse;
            }
            sMsg += "</div>";
            sMsg += "</body>";
            sMsg += "</html>";

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            // Get submitter email (it won't be in TC1 or TC2 lists) 
            if (task == "New Response")
            {
                sAddress = getEmpEmail(iEmpNum);
                sDuplCheck = sAddress;
                toAddress = new MailAddress(sAddress);
                message.To.Add(toAddress);
            }
            
            // Get all TC1 and if assigned is TC2 include them
            if (task == "Assignment Change")
                dataTable = getUserData(task, iMgrNum);
            else
                dataTable = getUserData(task, 0);

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                sAddress = dataTable.Rows[iRowIdx]["mgrEmail"].ToString().Trim();
                if (sDuplCheck.Contains(sAddress) == false) 
                {
                    sDuplCheck += "," + sAddress;
                    //                sAddress = "htslog@yahoo.com";
                    toAddress = new MailAddress(sAddress);
                    message.To.Add(toAddress);
                }
                iRowIdx++;
            }

            if (message.To.Count > 0)
            {
                fromAddress = new MailAddress(sAddressFrom);
                message.From = fromAddress;

                message.Subject = sSbj;
                message.IsBodyHtml = true;
                message.Body = sMsg;

                //smtpClient.Host = "localhost";
                smtpClient.Host = "10.40.14.79";
                // Uncomment for SMTP servers that require authentication
                // smtpClient.Credentials = new System.Net.NetworkCredential("user", "password");
                smtpClient.Send(message);
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sDuplCheck);
        }
        finally
        {

        }
    }
    // =========================================================
    // START COMMENT GRID
    // =========================================================
    protected void gvPageIndexChanging_Com(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;
        gvComments.PageIndex = newPageIndex;
        BindGrid_Com();
    }
    // =========================================================
    protected void BindGrid_Com()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // Load only when new or when parms have changed
        if (ViewState["vsDataTable_Com"] == null)
        {
            int iKey = 0;
            int.TryParse(txKey.Text, out iKey);
            string sSubmitter = txEntryName.Text.ToUpper();
            string sAssignee = txAssignName.Text.ToUpper();
            string sTitle = txTitle.Text;
            string sComment = txComment.Text;
            string sEntryDate = "";
            if (txEntryDate.Text == "")
                calEntryDate.SelectedDate = DateTime.MinValue;
            else
                sEntryDate = calEntryDate.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //calEntryDate.SelectedDate = "1/1/0001 12:00:00 AM";
            dataTable = getComments(iKey, sSubmitter, sAssignee, sTitle, sEntryDate, sComment); 

            // Store the data in memory (so you don't have to keep getting it) 
            ViewState["vsDataTable_Com"] = dataTable;
        }
        else
        {
            dataTable = (DataTable)ViewState["vsDataTable_Com"];
        }
        // Prepare the sort expression using the gridSortDirection and gridSortExpression properties
        string sortExpression_Com;
        if (gridSortDirection_Com == SortDirection.Ascending)
        {
            sortExpression_Com = gridSortExpression_Com + " ASC";
        }
        else
        {
            sortExpression_Com = gridSortExpression_Com + " DESC";
        }
        // Sort the data
        // If using a data set you can have multiple data tables, here you're just trying to use one.
        if (dataTable.Rows.Count > 0)
            dataTable.DefaultView.Sort = sortExpression_Com;

        gvComments.DataSource = dataTable.DefaultView;
        gvComments.DataBind();

    }
    // =========================================================
    protected void gvSorting_Com(object sender, GridViewSortEventArgs e)
    {
        // Retrieve the name of the clicked column
        string sortExpression_Com = e.SortExpression;
        // Decide and save the new sort direction
        if (sortExpression_Com == e.SortExpression)
        {
            if (gridSortDirection_Com == SortDirection.Ascending)
                gridSortDirection_Com = SortDirection.Descending;
            else
                gridSortDirection_Com = SortDirection.Ascending;
        }
        else
            gridSortDirection_Com = SortDirection.Ascending;
        // Save the new sort expression
        gridSortExpression_Com = sortExpression_Com;
        // Rebind the grid to its data source
        BindGrid_Com();
    }
    private SortDirection gridSortDirection_Com
    {
        get
        {
            // Initial state is Descending here...
            if (ViewState["GridSortDirection_Com"] == null)
            {
                //ViewState["GridSortDirection_Com"] = SortDirection.Ascending;
                ViewState["GridSortDirection_Com"] = SortDirection.Descending;
            }
            // return the state
            return (SortDirection)ViewState["GridSortDirection_Com"];
        }
        set
        {
            ViewState["GridSortDirection_Com"] = value;
        }
    }
    private string gridSortExpression_Com
    {
        get
        {
            // Initial sort expression is...
            if (ViewState["GridSortExpression_Com"] == null)
            {
                ViewState["GridSortExpression_Com"] = "Key"; // INITIAL SORT BY FIELD
            }
            return (string)ViewState["GridSortExpression_Com"];
        }
        set
        {
            ViewState["GridSortExpression_Com"] = value;
        }
    }
    // =========================================================
    // END COMMENT GRID 
    // =========================================================
    // ========================================================================
    // BEGIN: SQLS
    // ========================================================================
    protected string getEmpName(int empNum)
    {
        string sEmpName = "";

        try
        {
            sSql = "select empnam" + 
                " from " + sLibrary + ".EMPMST" +
                " where empnum = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@EmpNum", OdbcType.Int);
            odbcCmd.Parameters["@EmpNum"].Value = empNum;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            if (odbcReader.HasRows)
            {
                while (odbcReader.Read())
                {
                    if (sEmpName == "")
                        sEmpName = odbcReader["empnam"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return sEmpName;
    }
    // ========================================================================
    protected string getEmpEmail(int empNum)
    {
        string sEmpEmail = "";

        try
        {
            sSql = "select eemail" +
                " from " + sLibrary + ".EMPMST" +
                " where empnum = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@EmpNum", OdbcType.Int);
            odbcCmd.Parameters["@EmpNum"].Value = empNum;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            if (odbcReader.HasRows)
            {
                while (odbcReader.Read())
                {
                    if (sEmpEmail == "")
                        sEmpEmail = odbcReader["eemail"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return sEmpEmail;
    }
    // ========================================================================
    protected DataTable getUserData(string dataEvent, int mgrNum)
    {
        // dataEvents
        // 1) New Submittal
        // 2) Assignment Change
        // 3) New Response
        // 4) Is Assigned Emp In Current List
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select distinct emleml as mgrEmail, emlemp as mgrNum" +
                " from " + sLibrary + ".EMPMEML3" +
                " where EMLOFF <> 'OFF'" +
                " and EMLEML <> ''";
            
            // All TC1s
            if (dataEvent == "New Submittal")
                sSql += " and EMLCDE = 'TC1'";
            
            // All TC1s (and If Assigned is not a TC1, but a TC2, include them also)
            else if (dataEvent == "Assignment Change")
                sSql += " and (EMLCDE = 'TC1' or (EMLCDE = 'TC2' and EMLEMP = ?))";
            
            // All TC2s (and the submitter which will be loaded in "send Email" since they will not be in this file
            else if (dataEvent == "New Response")
                sSql += " and EMLCDE = 'TC2'";
            
            // Check if currently assigned emp is still in TC2 list (assignment emp) (or needs manual addition)
            else if (dataEvent == "Is Assigned Emp In Current List")
                sSql += " and EMLCDE = 'TC2' and EMLEMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            if (mgrNum > 0) 
            { 
                odbcCmd.Parameters.Add("@EmpNum", OdbcType.Int);
                odbcCmd.Parameters["@EmpNum"].Value = mgrNum;
            }

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected string isUserAdmin(int user)
    {
        string sAdmin = "";

        try
        {
            sSql = "select emlemp" +
                " from " + sLibrary + ".EMPMEML3" +
                " where EMLOFF <> 'OFF'" +
                " and EMLEML <> ''" +
                " and EMLCDE = 'TC1'" +
                " and EMLEMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.Add("@EmpNum", OdbcType.Int);
            odbcCmd.Parameters["@EmpNum"].Value = user;
            
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            if (odbcReader.HasRows)
            {
                sAdmin = "YES";
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return sAdmin;
    }
    // ========================================================================
    protected DataTable getMgrsForAssignment()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select distinct empnam, empnum" +
                " from " + sLibrary + ".EMPMST, " + sLibrary + ".EMPMEML3" +
                " where EMPNUM = EMLEMP" +
                " and EMLOFF <> 'OFF'" +
                " and EMLEML <> ''" +
                " and EMLCDE = 'TC2'" +
                " order by empnam";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected int insertComment(int empNum, string title, string comment)
    {
        int iKey = 0;
        int iRowsAffected = 0;
        KeyHandler kh = new KeyHandler();
        iKey = kh.MakeNewKey("COMMENTS");

        int iVernNum = 1119;
        string sVernName = "KATHOL, VERN J.";

        string sEmpName = getEmpName(empNum);

        if (title != "" && title.Length > 100)
            title = title.Substring(0, 100);
        if (comment != "" && comment.Length > 2000)
            comment = comment.Substring(0, 2000);

        try
        {
            datTemp = DateTime.Now;
            string sDate = datTemp.ToString("yyyy-MM-dd HH:mm:ss.fff");

            sSql = "insert into " + sLibrary + ".COMMENTS" +
                " (CMKEY, CM1DAT, CM1EMP, CM1NAM, CM1TIT, CM1TXT, CM2EMP, CM2NAM, CM2DAT)" +
                " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = iKey;

            odbcCmd.Parameters.Add("@EntryDate", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@EntryDate"].Value = sDate;

            odbcCmd.Parameters.Add("@Emp1Num", OdbcType.Int);
            odbcCmd.Parameters["@Emp1Num"].Value = empNum;

            odbcCmd.Parameters.Add("@Emp1Name", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@Emp1Name"].Value = sEmpName;

            odbcCmd.Parameters.Add("@Title", OdbcType.VarChar, 100);
            odbcCmd.Parameters["@Title"].Value = title;

            odbcCmd.Parameters.Add("@Comment", OdbcType.VarChar, 2000);
            odbcCmd.Parameters["@Comment"].Value = comment;

            odbcCmd.Parameters.Add("@Emp2Num", OdbcType.Int);
            odbcCmd.Parameters["@Emp2Num"].Value = iVernNum;

            odbcCmd.Parameters.Add("@Emp2Name", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@Emp2Name"].Value = sVernName;

            odbcCmd.Parameters.Add("@AssignDate", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@AssignDate"].Value = sDate;

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iKey;
    }
    // ========================================================================
    protected DataTable getComments(int key, string submitter, string assignee, string title, string entryDate, string comment)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select cmkey, cm1dat, cm1emp, cm1nam, cm1tit, cm1txt, cm2nam, cm2txt" +
                " from " + sLibrary + ".COMMENTS" +
                " where cmkey > 0";

            if (key > 0)
                sSql += " and cmkey = ?";
            if (submitter != "")
                sSql += " and cm1nam like ?";
            if (assignee != "")
                sSql += " and cm2nam like ?";
            if (title != "")
                sSql += " and UCASE(cm1tit) like ?";
            //if (entryDate != "" && entryDate != "0001-01-01 00:00:00.000")
            if (entryDate != "")
                sSql += " and cm1dat > ?";
            if (comment != "")
                sSql += " and UCASE(cm1txt) like ?";
            
            sSql += " order by cmkey desc";

            // Apply completed SQL string to command object
            odbcCmd = new OdbcCommand(sSql, odbcConn);

            // Apply Parameters to command object
            if (key > 0) 
            {
                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = key;
            }

            if (submitter != "")
            {
                odbcCmd.Parameters.Add("@Submitter", OdbcType.VarChar, 50);
                odbcCmd.Parameters["@Submitter"].Value = submitter.ToUpper() + "%";
            }
            if (assignee != "")
            {
                odbcCmd.Parameters.Add("@Assignee", OdbcType.VarChar, 50);
                odbcCmd.Parameters["@Assignee"].Value = assignee.ToUpper() + "%";
            }
            if (title != "")
            {
                odbcCmd.Parameters.Add("@Title", OdbcType.VarChar, 50);
                odbcCmd.Parameters["@Title"].Value = "%" + title.ToUpper() + "%";
            }

            //if (entryDate != "" && entryDate != "0001-01-01 00:00:00.000")
            if (entryDate != "")
            {
                odbcCmd.Parameters.Add("@EntryDate", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@EntryDate"].Value = entryDate;
            }
            if (comment != "")
            {
                odbcCmd.Parameters.Add("@Comment", OdbcType.VarChar, 200);
                odbcCmd.Parameters["@Comment"].Value = "%" + comment.ToUpper() + "%";
            }

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            dataTable.Columns.Add(MakeColumn("DisplayEntryDate"));
            dataTable.Columns.Add(MakeColumn("DisplayTitle"));
            dataTable.Columns.Add(MakeColumn("DisplayComment"));
            dataTable.Columns.Add(MakeColumn("DisplayResponse"));
            dataTable.Columns.Add(MakeColumn("DisplaySubmitter"));
            dataTable.Columns.Add(MakeColumn("DisplayAssignee"));
            string sDate = "";
            string sTemp = "";

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                sDate = dataTable.Rows[iRowIdx]["cm1dat"].ToString().Trim();
                datTemp = Convert.ToDateTime(sDate);  // "2012-12-31 01:01:01.000"
                //dataTable.Rows[iRowIdx]["DisplayEntryDate"] = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                dataTable.Rows[iRowIdx]["DisplayEntryDate"] = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString();
                
                sTemp = dataTable.Rows[iRowIdx]["cm1nam"].ToString().Trim();
                dataTable.Rows[iRowIdx]["DisplaySubmitter"] = sTemp.Substring(0, sTemp.IndexOf(","));

                sTemp = dataTable.Rows[iRowIdx]["cm2nam"].ToString().Trim();
                dataTable.Rows[iRowIdx]["DisplayAssignee"] = sTemp.Substring(0, sTemp.IndexOf(","));
                
                sTemp = dataTable.Rows[iRowIdx]["cm1tit"].ToString().Trim();
                if (sTemp != "" && sTemp.Length > 40)
                    sTemp = sTemp.Substring(0, 40);
                dataTable.Rows[iRowIdx]["DisplayTitle"] = sTemp;

                sTemp = dataTable.Rows[iRowIdx]["cm1txt"].ToString().Trim();
                if (sTemp != "" && sTemp.Length > 30)
                    sTemp = sTemp.Substring(0, 30);
                dataTable.Rows[iRowIdx]["DisplayComment"] = sTemp;

                sTemp = dataTable.Rows[iRowIdx]["cm2txt"].ToString().Trim();
                if (sTemp != "" && sTemp.Length > 30)
                    sTemp = sTemp.Substring(0, 30);
                dataTable.Rows[iRowIdx]["DisplayResponse"] = sTemp;

                iRowIdx++;
            }
            //cmkey, cm1dat, cm1emp, cm1nam, cm1tit, cm2nam
            dataTable.Columns.Remove("cm1dat");
            dataTable.Columns.Remove("cm1emp");
            dataTable.Columns.Remove("cm1nam");
            dataTable.Columns.Remove("cm1tit");
            dataTable.Columns.Remove("cm1txt");
            dataTable.Columns.Remove("cm2nam");
            dataTable.Columns.Remove("cm2txt");

            dataTable.Columns["cmkey"].ColumnName = "Key";
            //dataTable.Columns["cm1dat"].ColumnName = "EntryDate";
            //dataTable.Columns["cm1emp"].ColumnName = "SubmitterNum";
            //dataTable.Columns["cm1nam"].ColumnName = "Submitter";
            //dataTable.Columns["cm2nam"].ColumnName = "Assignee";
            //dataTable.Columns["cm1tit"].ColumnName = "Title";
            dataTable.Columns["DISPLAYSUBMITTER"].ColumnName = "DisplaySubmitter";
            dataTable.Columns["DISPLAYASSIGNEE"].ColumnName = "DisplayAssignee";
            dataTable.Columns["DISPLAYTITLE"].ColumnName = "DisplayTitle";
            dataTable.Columns["DISPLAYCOMMENT"].ColumnName = "DisplayComment";
            dataTable.Columns["DISPLAYRESPONSE"].ColumnName = "DisplayResponse";
            dataTable.Columns["DISPLAYENTRYDATE"].ColumnName = "DisplayEntryDate";

            dataTable.AcceptChanges();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected DataTable getComment(int key)
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select cmkey, cm1dat, cm1emp, cm1nam, cm1tit, cm1txt, cm2emp, cm2nam, cm2txt, cm2dat, cm3dat" +
                " from " + sLibrary + ".COMMENTS" +
                " where cmkey = ?";

            // Apply completed SQL string to command object
            odbcCmd = new OdbcCommand(sSql, odbcConn);

            // Apply Parameters to command object
            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected int updAssigned(int key, int assignedNum, string assignedName)
    {
        int iRowsAffected = 0;

        try
        {
            datTemp = DateTime.Now;
            string sDate = datTemp.ToString("yyyy-MM-dd HH:mm:ss.fff");

            sSql = "update " + sLibrary + ".COMMENTS set" +
                 " cm2emp = ?" +
                ", cm2nam = ?" +
                ", cm2dat = ?" +
                " where cmkey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@EmpNum", OdbcType.Int);
            odbcCmd.Parameters["@EmpNum"].Value = assignedNum;

            odbcCmd.Parameters.Add("@EmpName", OdbcType.VarChar, 50);
            odbcCmd.Parameters["@EmpName"].Value = assignedName;

            odbcCmd.Parameters.Add("@DateTime", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@DateTime"].Value = sDate;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iRowsAffected;
    }
    // ========================================================================
    protected int updResponse(int key, string response)
    {
        int iRowsAffected = 0;

        try
        {
            datTemp = DateTime.Now;
            string sDate = datTemp.ToString("yyyy-MM-dd HH:mm:ss.fff");

            sSql = "update " + sLibrary + ".COMMENTS set" +
                 " cm2txt = ?" +
                ", cm3dat = ?" +
                " where cmkey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Response", OdbcType.VarChar, 2000);
            odbcCmd.Parameters["@Response"].Value = response;

            odbcCmd.Parameters.Add("@DateTime", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@DateTime"].Value = sDate;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iRowsAffected;
    }
    // =========================================================
    // BEGIN: EVENT RESPONSES
    // =========================================================
    protected void btNewEntry_Click(object sender, EventArgs e)
    {
        pnViewEntry.Visible = false;

        if (btNewEntry.Text == "New Submittal")
        {
            pnNewEntry.Visible = true;
            btNewEntry.Text = "Cancel";
            btAddEntry.Visible = true;
            txNewTitle.Focus();
        }
        else
        {
            txNewTitle.Text = "";
            txNewEntry.Text = "";
            pnNewEntry.Visible = false;
            btAddEntry.Visible = false;
            btNewEntry.Text = "New Submittal";
        }
    }
    // =========================================================
    protected void btAddEntry_Click(object sender, EventArgs e)
    {
        try
        {
            odbcConn.Open();
            
            int iKey = 0;
            int iEmpNum = 0;
            int.TryParse(hfEmp.Value, out iEmpNum);

            string sTitle = scrub(txNewTitle.Text);
            string sComment = scrub(txNewEntry.Text);

            iKey = insertComment(iEmpNum, sTitle, sComment);
            sendEmail(iKey, "New Submittal");
            
            ViewState["vsDataTable_Com"] = null;
            BindGrid_Com();
            
            pnNewEntry.Visible = false;
            btAddEntry.Visible = false;
            btNewEntry.Text = "New Submittal";
            txNewTitle.Text = "";
            txNewEntry.Text = "";

        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
    }
    // =========================================================
    protected void calEntryDate_SelectionChanged(object sender, EventArgs e)
    {
        txEntryDate.Text = sMonthAbbrev[calEntryDate.SelectedDate.Month] + " " + calEntryDate.SelectedDate.Day.ToString() + ", " + calEntryDate.SelectedDate.Year.ToString();
//        txEntryDate.Visible = true;
        lkEntryDate.CssClass = "OFF";
        lkEntryDate.Text = "Entry Date >";
    }
    // =========================================================
    protected void lkEntryDate_Click(object sender, EventArgs e)
    {
        if (lkEntryDate.Text == "Cancel")
        {
            lkEntryDate.CssClass = "OFF";
            lkEntryDate.Text = "Entry Date >";
            txEntryDate.Text = "";
            calEntryDate.SelectedDate = DateTime.MinValue;
            txNewTitle.Text = "";
            txNewEntry.Text = "";
        }
        else
        {
            lkEntryDate.CssClass = "ON";
            lkEntryDate.Text = "Cancel";
        }
    }
    // =========================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        try
        {
            pnViewEntry.Visible = false;
            pnNewEntry.Visible = false;

            odbcConn.Open();
            ViewState["vsDataTable_Com"] = null;
            BindGrid_Com();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
    }
    // =========================================================
    protected void btClear_Click(object sender, EventArgs e)
    {
        txAssignName.Text = "";
        txEntryName.Text = "";
        txKey.Text = "";
        txTitle.Text = "";
        txComment.Text = "";
        txEntryDate.Text = "";
        calEntryDate.SelectedDate = DateTime.MinValue;
    }
    // =========================================================
    protected void lkTitle_Click(object sender, EventArgs e)
    {
        LinkButton lkControl = (LinkButton)sender;
        int iKey = 0;
        int.TryParse(lkControl.CommandArgument.ToString(), out iKey);

        try
        {
            odbcConn.Open();
            txNewTitle.Text = "";
            txNewEntry.Text = "";
            pnNewEntry.Visible = false;
            btAddEntry.Visible = false;
            btNewEntry.Text = "New Submittal";
            displayComment(iKey);
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
       
    }
    // =========================================================
    protected void ddAssigned_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iKey = 0;
        int.TryParse(hfEditKey.Value, out iKey);
        int iAssigned = 0;
        int.TryParse(ddAssigned.SelectedValue, out iAssigned);
        string sAssigned = "";
        int iRowsAffected = 0;

        if (iKey > 0)
        {
            try
            {
                odbcConn.Open();
                sAssigned = getEmpName(iAssigned);
                iRowsAffected = updAssigned(iKey, iAssigned, sAssigned);
                sendEmail(iKey, "Assignment Change");

                ViewState["vsDataTable_Com"] = null;
                BindGrid_Com();
                if (hfReloadAssignments.Value == "RELOAD") 
                {
                    hfReloadAssignments.Value = "";
                    ddAssigned.DataSource = getMgrsForAssignment();
                    ddAssigned.DataTextField = "empnam";
                    ddAssigned.DataValueField = "empnum";
                    ddAssigned.DataBind();
                }
                ddAssigned.SelectedValue = iAssigned.ToString();
                if (hfEmp.Value == ddAssigned.SelectedValue)
                {
                    lbResponse.Visible = false;
                    txNewResponse.Visible = true;
                    btResponse.Visible = true;
                }
                else 
                {
                    lbResponse.Visible = true;
                    txNewResponse.Visible = false;
                    btResponse.Visible = false;
                }
            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
            }
        }
    }
    // =========================================================
    protected void btResponse_Click(object sender, EventArgs e)
    {
        int iKey = 0;
        int.TryParse(hfEditKey.Value, out iKey);
        string sResponse = scrub(txNewResponse.Text);
        int iRowsAffected = 0;

        if (iKey > 0)
        {
            try
            {
                odbcConn.Open();
                iRowsAffected = updResponse(iKey, sResponse);
                sendEmail(iKey, "New Response");

                ViewState["vsDataTable_Com"] = null;
                BindGrid_Com();

                pnViewEntry.Visible = false;
            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
            }
        }
    }
    // =========================================================
    // END: EVENT RESPONSES
    // =========================================================
    protected string scrub(string sTextIn)
    {
        string sText = sTextIn;

        sText = sText.Replace("’", "'");
        sText = sText.Replace("“", "\"");
        sText = sText.Replace("”", "\"");
        sText = sText.Trim();

        return sText;
    }
    // =========================================================
}