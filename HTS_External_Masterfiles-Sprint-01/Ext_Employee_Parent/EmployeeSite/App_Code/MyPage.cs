using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Odbc;

/// <summary>
/// MyPage overrides the standard Page class
/// </summary>
public class MyPage : System.Web.UI.Page
{
    // ==================================================
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sConnectionString = "";
    string sSql = "";

    private bool _RequireSSL;
    public string sDevTestLive = "DEFAULT";
    public string sWsKey = "";
    public string sLibrary = "OMDTALIB";
    public string[] sMonthAbbrev = { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    public string[] sMonthName = { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    public string sOpenDnsApiKey = "C72A49CD82889B273BC844053158270E";
    public string sOpenDnsToken = "D24E19E4E589F9C7D9AE19241258F7C5";
    public string sOpenDnsHtsId = "1278219";

    // ==================================================
    public MyPage()
    {
        SiteHandler sh = new SiteHandler();
        sDevTestLive = sh.getWebSite();
        if (sDevTestLive == "LIVE")
        {
            sLibrary = "OMDTALIB";
        }
        else
        {
            sLibrary = "OMTDTALIB";
        }
    }
    // ==================================================
    //    [Description("Indicates whether or not this page should be forced into or out of SSL")]
    //    [Browsable(true)]
    public virtual bool RequireSSL
    {
        get
        {
            return _RequireSSL;
        }
        set
        {
            _RequireSSL = value;
        }
    }
    // ==================================================
    // 1st Option prevents debugger from walking through code (leave this on in non SSL development: off for live) 
    // [System.Diagnostics.DebuggerStepThrough()]
    // 2nd Option only allows it to run in secure environment
    // [System.Diagnostics.Conditional("SECURE")]
    private void PushSSL()
    {
        const string SECURE = "https://";
        const string UNSECURE = "http://";
        const string sLivUnSecure = ":80/";
        const string sLivSecure = ":443/";
        const string sLi2UnSecure = ":90/";
        const string sLi2Secure = ":4090/";
        const string sTstUnSecure = ":190/";
        const string sTstSecure = ":4190/";
        const string sDevUnSecure = ":290/";
        const string sDevSecure = ":4290/";
        const string sIsaUnSecure = ":390/";
        const string sIsaSecure = ":4390/";
        const string sSteUnSecure = ":490/";
        const string sSteSecure = ":4490/";

        string sUserURL = Request.Url.ToString();

        //Force required into secure channel

        string sWWWRedirect = "NO";
        if (!sUserURL.StartsWith("http://localhost"))
        {

            if (RequireSSL && Request.IsSecureConnection == false)
            {
                sUserURL = sUserURL.Replace(sLivUnSecure, sLivSecure);
                sUserURL = sUserURL.Replace(sLi2UnSecure, sLi2Secure);
                sUserURL = sUserURL.Replace(sTstUnSecure, sTstSecure);
                sUserURL = sUserURL.Replace(sDevUnSecure, sDevSecure);
                sUserURL = sUserURL.Replace(sIsaUnSecure, sIsaSecure);
                sUserURL = sUserURL.Replace(sSteUnSecure, sSteSecure);
                sUserURL = sUserURL.Replace(UNSECURE, SECURE);
                Response.Redirect(sUserURL);
            }
            //Force non-required out of secure channel
            if (!RequireSSL && Request.IsSecureConnection == true)
            {
                sUserURL = sUserURL.Replace(sLivSecure, sLivUnSecure);
                sUserURL = sUserURL.Replace(sLi2Secure, sLi2UnSecure);
                sUserURL = sUserURL.Replace(sTstSecure, sTstUnSecure);
                sUserURL = sUserURL.Replace(sDevSecure, sDevUnSecure);
                sUserURL = sUserURL.Replace(sIsaSecure, sIsaUnSecure);
                sUserURL = sUserURL.Replace(sSteSecure, sSteUnSecure);

                sUserURL = sUserURL.Replace(SECURE, UNSECURE);
                Response.Redirect(sUserURL);
            }
            if (sWWWRedirect == "YES")
                Response.Redirect(sUserURL);
        }
    }
    // ==================================================
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    // ==================================================
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        PushSSL();

        SiteHandler sh = new SiteHandler();
        sDevTestLive = sh.getWebSite();
        /*
        if (sDevTestLive == "LIVE")
            sLibrary = "OMDTALIB";
        else
            sLibrary = "OMTDTALIB";
        */
        if (!IsPostBack)
        {
            // SaveHitsToPages();
            // SaveHitsFromSameIP();
        }
    }
    // ==================================================
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    // ==================================================
    public void SaveError(string errorSummary, string errorDescription, string errorValues)
    {
        Emp_LIVE.EmployeeMenuSoapClient wsLive = new Emp_LIVE.EmployeeMenuSoapClient();
        Emp_DEV.EmployeeMenuSoapClient wsTest = new Emp_DEV.EmployeeMenuSoapClient();

        string sUserId = "";
        if (User.Identity.IsAuthenticated)
        {
            MembershipUser mu = Membership.GetUser();
            sUserId = mu.UserName;
        }
        else
            sUserId = "Anonymous";

        //sUserId = HttpContext.Current.Request.ServerVariables["LOGON_USER"]; ;
        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        if (sLibrary == "OMDTALIB")
        {
            wsLive.SaveErrorText(errorSummary, errorDescription, errorValues, sUserId, sIpAddress, "EMP LIVE");
        }
        else
        {
            wsTest.SaveErrorText(errorSummary, errorDescription, errorValues, sUserId, sIpAddress, "EMP DEV");
        }
    }
    // ==================================================
    public void SaveHitsToPages()
    {
        string sParentPagePath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sParentPagePath);
        string sParentPageName = oInfo.Name;

        SqlCommand sqlCmd = new SqlCommand();
        SqlConnection sqlConn;
        SqlDataReader sqlReader;
        DataTable dataTable;

        string sSql = "";

        string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

        try
        {
            sqlConn.Open();

            sSql = "Select hcKey, hcCount from HitCount where hcPath = @PagePath and hcYear = @ThisYear";
            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandText = sSql;

            sqlCmd.Parameters.Add("@PagePath", System.Data.SqlDbType.VarChar, 100);
            sqlCmd.Parameters["@PagePath"].Value = sParentPagePath;

            int iYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            sqlCmd.Parameters.Add("@ThisYear", System.Data.SqlDbType.Int);
            sqlCmd.Parameters["@ThisYear"].Value = iYear;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

            dataTable = new DataTable();
            dataTable.Load(sqlReader);

            int iKey = 0;
            int iCount = 0;

            if (dataTable.Rows.Count > 0)
            {
                // update hit count rec
                if (int.TryParse(dataTable.Rows[0]["hcKey"].ToString(), out iKey) == false)
                    iKey = 0;
                if (int.TryParse(dataTable.Rows[0]["hcCount"].ToString(), out iCount) == false)
                    iCount = 0;

                sSql = "Update HitCount" +
                    " set hcCount = @HitCount" +
                    " where hcKey = @HitKey";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.Add("@HitCount", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@HitCount"].Value = iCount + 1;

                sqlCmd.Parameters.Add("@HitKey", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@HitKey"].Value = iKey;
            }
            else
            {
                // Add first rec for this page
                sSql = "Insert into HitCount" +
                    " (hcPath, hcPage, hcCount, hcYear)" +
                    " VALUES(@Path, @Name, @Count, @Year)";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar, 200);
                sqlCmd.Parameters["@Path"].Value = sParentPagePath;

                sqlCmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100);
                sqlCmd.Parameters["@Name"].Value = sParentPageName;

                sqlCmd.Parameters.Add("@Count", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@Count"].Value = 1;

                sqlCmd.Parameters.Add("@Year", System.Data.SqlDbType.Int);
                sqlCmd.Parameters["@Year"].Value = iYear;
            }
            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlConn.Close();
        }
        // ----------------------------------
    }
    // ===========================================================
    public static string ScreenScrape(string url)
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            // set properties of the client
            return client.DownloadString(url);
        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    public string GetEmpName(int empNum)
    {
        string sEmpName = "";

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@EmpNum", empNum);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sEmpName = dt.Rows[0]["EMPNAM"].ToString().Trim();
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return sEmpName;
    }
    // ========================================================================
    public string GetEmpEmail(int empNum)
    {
        string sEmpEmail = "";

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " EEMAIL" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@EmpNum", empNum);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sEmpEmail = dt.Rows[0]["EEMAIL"].ToString().Trim();
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return sEmpEmail;
    }
    // ========================================================================
    public int AddParmsToTRIGMAST(
        string pgmToFormat,
        string pgmToRun,
        string txt1,
        string txt2,
        string txt3,
        string txt4,
        string txt5,
        string txt6,
        string txt7,
        string txt8,
        string txt9,
        double num1,
        double num2,
        double num3,
        double num4,
        double num5,
        double num6,
        double num7,
        double num8,
        double num9,
        string txtBig)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        int iRowsAffected = 0;
        string sTest = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        if (!String.IsNullOrEmpty(txt1)) { if (txt1.Length > 100) txt1 = txt1.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt2)) { if (txt2.Length > 100) txt2 = txt2.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt3)) { if (txt3.Length > 100) txt3 = txt3.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt4)) { if (txt4.Length > 100) txt4 = txt4.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt5)) { if (txt5.Length > 100) txt5 = txt5.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt6)) { if (txt6.Length > 100) txt6 = txt6.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt7)) { if (txt7.Length > 100) txt7 = txt7.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt8)) { if (txt8.Length > 100) txt8 = txt8.Substring(0, 100); }
        if (!String.IsNullOrEmpty(txt9)) { if (txt9.Length > 100) txt9 = txt9.Substring(0, 100); }
        //if (!String.IsNullOrEmpty(txtBig)) { if (txtBig.Length > 9900) txtBig = txtBig.Substring(0, 9900); txtBig = HttpUtility.HtmlEncode(txtBig); }
        if (!String.IsNullOrEmpty(txtBig)) { if (txtBig.Length > 9900) txtBig = txtBig.Substring(0, 9900); }

        string sUrl = "";
        // Override if needed while running from personal folder
        if (sLibrary == "OMTDTALIB")
            sUrl = "http://oursts.com:190/public/GetKey.aspx?nam=TRIGMAST";
        else
            sUrl = "http://oursts.com:90/public/GetKey.aspx?nam=TRIGMAST";
        string sNextKey = ScreenScrape(sUrl);
        int iNextKey = 0;
        if (int.TryParse(sNextKey, out iNextKey) == false)
            iNextKey = -1;
        if (iNextKey > 0)
        {
            string sSql2 = "";

            try
            {
                odbcConn.Open();

                if (sLibrary != "OMDTALIB")
                {
                    sLibrary = "OMTDTALIB";
                    sTest = "Y";
                }

                // Replace any NON-AS400 characters from error log here! (which are causing crashes)
                txtBig = txtBig.Replace("–", "-");
                txtBig = txtBig.Replace("•", "-");

                txtBig = HttpUtility.HtmlEncode(txtBig);

                if (!String.IsNullOrEmpty(txtBig) && txtBig.Length > 10000)
                    txtBig = txtBig.Substring(0, 10000);


                sSql = "insert into " + sLibrary + ".TRIGMAST (" +
                      "TMKEY" + // newKey
                    ", TMPGM" + // TRIGFMT (has always been used to format parms you are loading)
                    ", TMRUN" + // Program to run the job
                    ", TMTX1" +
                    ", TMTX2" +
                    ", TMTX3" +
                    ", TMTX4" +
                    ", TMTX5" +
                    ", TMTX6" +
                    ", TMTX7" +
                    ", TMTX8" +
                    ", TMTX9" +
                    ", TMNM1" +
                    ", TMNM2" +
                    ", TMNM3" +
                    ", TMNM4" +
                    ", TMNM5" +
                    ", TMNM6" +
                    ", TMNM7" +
                    ", TMNM8" +
                    ", TMNM9" +
                    ", TMBIG" +
                    ", TMTST" + // Y = run in test
                    ") values(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                sSql2 =
                    "Key: " + iNextKey +
                    "-- Pgm: " + pgmToFormat +
                    "-- Run: " + pgmToRun +
                    "-- Tx1: " + txt1 +
                    "-- Tx2: " + txt2 +
                    "-- Tx3: " + txt3 +
                    "-- Tx4: " + txt4 +
                    "-- Tx5: " + txt5 +
                    "-- Tx6: " + txt6 +
                    "-- Tx7: " + txt7 +
                    "-- Tx8: " + txt8 +
                    "-- Tx9: " + txt9 +
                    "-- Nm1: " + num1 +
                    "-- Nm2: " + num2 +
                    "-- Nm3: " + num3 +
                    "-- Nm4: " + num4 +
                    "-- Nm5: " + num5 +
                    "-- Nm6: " + num6 +
                    "-- Nm7: " + num7 +
                    "-- Nm8: " + num8 +
                    "-- Nm9: " + num9 +
                    "-- TxtBig: " + txtBig +
                    "-- Test?: " + sTest;

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Key", iNextKey);
                odbcCmd.Parameters.AddWithValue("@Pgm", pgmToFormat);
                odbcCmd.Parameters.AddWithValue("@Run", pgmToRun);
                odbcCmd.Parameters.AddWithValue("@Tx1", txt1);
                odbcCmd.Parameters.AddWithValue("@Tx2", txt2);
                odbcCmd.Parameters.AddWithValue("@Tx3", txt3);
                odbcCmd.Parameters.AddWithValue("@Tx4", txt4);
                odbcCmd.Parameters.AddWithValue("@Tx5", txt5);
                odbcCmd.Parameters.AddWithValue("@Tx6", txt6);
                odbcCmd.Parameters.AddWithValue("@Tx7", txt7);
                odbcCmd.Parameters.AddWithValue("@Tx8", txt8);
                odbcCmd.Parameters.AddWithValue("@Tx9", txt9);
                odbcCmd.Parameters.AddWithValue("@Nm1", num1);
                odbcCmd.Parameters.AddWithValue("@Nm2", num2);
                odbcCmd.Parameters.AddWithValue("@Nm3", num3);
                odbcCmd.Parameters.AddWithValue("@Nm4", num4);
                odbcCmd.Parameters.AddWithValue("@Nm5", num5);
                odbcCmd.Parameters.AddWithValue("@Nm6", num6);
                odbcCmd.Parameters.AddWithValue("@Nm7", num7);
                odbcCmd.Parameters.AddWithValue("@Nm8", num8);
                odbcCmd.Parameters.AddWithValue("@Nm9", num9);
                odbcCmd.Parameters.AddWithValue("@Big", txtBig);
                odbcCmd.Parameters.AddWithValue("@Tst", sTest);

                // MailHandler emh = new MailHandler();
                // emh.EmailIndividual("Before Trigger Insert:  " + txt3, txtBig, "htslog@yahoo.com", "HTML");
                // emh = null;

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorHandler erh = new ErrorHandler();
                erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Sql Values: " + sSql2 + " ----- Sql: " + sSql);
                erh = null;
                iRowsAffected = -1;
            }
            finally
            {
                odbcCmd.Dispose();
                odbcConn.Close();
            }
        }

        return iRowsAffected;
    }

    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    public DataColumn MakeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
    // ==================================================
    // ==================================================
}
