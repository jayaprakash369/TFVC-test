using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

/// <summary>
/// MyPage overrides the standard Page class
/// </summary>
public class MyPage : System.Web.UI.Page
{
    // ==================================================
    private bool _RequireSSL; 
    public string sDevTestLive = "DEFAULT";
    public string sPageLib = "";
    public string sUserType = "";
    public string sWsKey = "";
    public int iCs1User = 0; // primary customer number, from which array of valid sub-customers will be gathered
    public string[] sAryMonthAbbrev = { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    public string[] sAryMonthName = { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    // Colors
    /* Primary
     * #006FA1 Blue Medium
     * #AE132A Red
     * #8C8D8E Gray Medium 
     * #414042 Gray Dark
     * 
     * Secondary Colors
     * #FEBC11 Yellow Orange
     * #7AC03D Green Bright
     * #00B1AC Aqua
     * #00A0DF Blue Royal
     * #F47B20 Orange
     * #007A40 Green Forest
     * #005961 Teal Dark
     * #4d407e Purple Dark
     */

    public System.Drawing.Color scantronBlueMedium = System.Drawing.ColorTranslator.FromHtml("#006FA1");
    public System.Drawing.Color scantronRed = System.Drawing.ColorTranslator.FromHtml("#AE132A");
    public System.Drawing.Color scantronGrayMedium = System.Drawing.ColorTranslator.FromHtml("#8C8D8E");
    public System.Drawing.Color scantronGrayDark = System.Drawing.ColorTranslator.FromHtml("#414042");

    public System.Drawing.Color scantronYellowOrange = System.Drawing.ColorTranslator.FromHtml("#FEBC11");
    public System.Drawing.Color scantronGreenBright = System.Drawing.ColorTranslator.FromHtml("#7AC03D");
    public System.Drawing.Color scantronAqua = System.Drawing.ColorTranslator.FromHtml("#00B1AC");
    public System.Drawing.Color scantronBlueRoyal = System.Drawing.ColorTranslator.FromHtml("#00A0DF");
    public System.Drawing.Color scantronOrange = System.Drawing.ColorTranslator.FromHtml("#F47B20");
    public System.Drawing.Color scantronGreenForest = System.Drawing.ColorTranslator.FromHtml("#007A40");
    public System.Drawing.Color scantronTealDark = System.Drawing.ColorTranslator.FromHtml("#005961");
    public System.Drawing.Color scantronPurpleDark = System.Drawing.ColorTranslator.FromHtml("#4d407e");

    // ==================================================
    public MyPage()
    {
        string sUserURL = HttpContext.Current.Request.Url.ToString();
        if (sUserURL.EndsWith("/Login.aspx?ReturnUrl=/")) {
            sUserURL = sUserURL.Replace("/Login.aspx?ReturnUrl=/", "/Default.aspx");
            HttpContext.Current.Response.Redirect(sUserURL);
        }

        /*
        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        string sUserURL = HttpContext.Current.Request.Url.ToString();
        if (sIpAddress == "::1" && !sUserURL.Contains("public/error/Default.aspx"))
        {
            // The redirected errors out, which goes to the error page.
            // but the error page will call this again and again...
            Response.Redirect("", false);
            //Response.End();
        }
         * */

        //string sVerdict = "";  
        //if (sUserURL.Contains("harlandts.com")) {
        //    sVerdict = EmailSteve("steve.carlson@scantron.com", "PushSSL URL just inside", sUserURL);
        //}

    }
    // ==================================================
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        PushSSL();
        SiteHandler sh = new SiteHandler();
        sDevTestLive = sh.getWebSite();
        
        //sDevTestLive = "LIVE";  // FOR DEBUG ONLY!!!

        if (sDevTestLive == "LIVE")
            sPageLib = "L";
        else
            sPageLib = "T";

        if (!IsPostBack)
        {
            //string sUserURL = Request.Url.ToString();
            
            SaveHitsToPages();
            SaveHitsFromSameIP();
        }
    }

    // ==================================================
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // For one time loads (only on first pass)
        if (!IsPostBack)
        {
        }
    }
    // ==================================================
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
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
        string sUserURL = Request.Url.ToString();
        //string sVerdict = "";  
        //if (sUserURL.Contains("harlandts.com")) {
        //    sVerdict = EmailSteve("steve.carlson@scantron.com", "PushSSL URL just inside", sUserURL);
        //}

        const string SECURE = "https://";
        const string UNSECURE = "http://";
        const string sLivUnSecure = ":80/";
        const string sLivSecure = ":443/";
        const string sTstUnSecure = ":180/";
        const string sTstSecure = ":4180/";
        const string sDevUnSecure = ":280/";
        const string sDevSecure = ":4280/";
        const string sIsaUnSecure = ":380/";
        const string sIsaSecure = ":4380/";
        const string sKoiUnSecure = ":480/";
        const string sKoiSecure = ":4480/";


        //Force required into secure channel
        if (!sUserURL.StartsWith("http://localhost"))
        {
            if (RequireSSL && Request.IsSecureConnection == false) 
            {
                // Update EVERY possibility (It will only effect the one matching site) 
                sUserURL = sUserURL.Replace(sIsaUnSecure, sIsaSecure);
                sUserURL = sUserURL.Replace(sKoiUnSecure, sKoiSecure);
                sUserURL = sUserURL.Replace(sDevUnSecure, sDevSecure);
                sUserURL = sUserURL.Replace(sTstUnSecure, sTstSecure);
                sUserURL = sUserURL.Replace(sLivUnSecure, sLivSecure);
                sUserURL = sUserURL.Replace(UNSECURE, SECURE);
                //if (sUserURL.Contains("harlandts.com"))
                //    sVerdict = EmailSteve("steve.carlson@scantron.com", "Requiring SSL but not secure yet", sUserURL);
                Response.Redirect(sUserURL, false);
            }
            //Force non-required out of secure channel
            if (!RequireSSL && Request.IsSecureConnection == true) 
            {
                // Update EVERY possibility (It will only effect the one matching site) 
                sUserURL = sUserURL.Replace(sIsaSecure, sIsaUnSecure);
                sUserURL = sUserURL.Replace(sKoiSecure, sKoiUnSecure);
                sUserURL = sUserURL.Replace(sDevSecure, sDevUnSecure);
                sUserURL = sUserURL.Replace(sTstSecure, sTstUnSecure);
                sUserURL = sUserURL.Replace(sLivSecure, sLivUnSecure);
                sUserURL = sUserURL.Replace(SECURE, UNSECURE);

                //if (sUserURL.Contains("harlandts.com"))
                //    sVerdict = EmailSteve("steve.carlson@scantron.com", "NOT requiring SSL but has secure link", sUserURL);

                Response.Redirect(sUserURL, false);
            }
        }
    }
    // =========================================================
    protected string EmailSteve(string adr, string sbj, string msg)
    {
        string sResult = "";

        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        try
        {
            MailAddress toAddress = new MailAddress(adr);
            MailAddress fromAddress = new MailAddress("adv320@scantronts.com");
            message.To.Add(toAddress);
            message.From = fromAddress;
            message.Subject = sbj;
            message.Body = msg;
            smtpClient.Host = "10.40.8.123";
            smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
        }
        finally
        {
        }

        return sResult;
    }
    // ==================================================
    public void SaveError(string errorSummary, string errorDescription, string errorValues)
    {
        Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
        Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();

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

        if (sPageLib == "L")
        {
            wsLive.SaveErrorText(errorSummary, errorDescription, errorValues, sUserId, sIpAddress, "CST LIVE");
        }
        else
        {
            wsTest.SaveErrorText(errorSummary, errorDescription, errorValues, sUserId, sIpAddress, "CST DEV");
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
    // ==================================================
    public void SaveHitsFromSameIP()
    {
        string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        string sParentPage = oInfo.Name;

        if ((sParentPage != "Timeout.aspx") && (sParentPage != "Lockout.aspx"))
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConn;
            SqlDataReader sqlReader;
            DataTable dataTable;

            DateTime datTemp = new DateTime();
            TimeSpan ts = new TimeSpan();
            string sSql = "";
            string sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            sqlConn = new SqlConnection(sConnectionString);

            double dTimeoutHours = 2.0;
            int iMaxPeriodHits = 500; // 500
            int iMaxTimeouts = 10;

            int iIpKey = 0;
            int iPeriodHits = 0;
            int iLifeHits = 0;
            int iTimeoutCount = 0;
            int iBlacklisted = 0;
            int iRowsAffected = 0;

            string sRedirect = "";
            string sUserIdLast = "";
            string sUserIdCurrent = "";
            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserIdCurrent = mu.UserName;
            }

            string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Trim();

            try
            {
                sqlConn.Open();

                sSql = "Select siKey, siPeriodStart, siPeriodHits, siLifeHits, siTimeoutCount, siBlacklisted, siUserID from HitsFromSameIP where siIpa = @UserIP";
                sqlCmd = new SqlCommand(sSql, sqlConn);
                sqlCmd.CommandText = sSql;
                sqlCmd.Parameters.Add("@UserIP", System.Data.SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@UserIP"].Value = sIpAddress;

                sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);

                dataTable = new DataTable();
                dataTable.Load(sqlReader);

                if (dataTable.Rows.Count > 0)
                {
                    // Update IP rec for this count
                    datTemp = Convert.ToDateTime(dataTable.Rows[0]["siPeriodStart"].ToString());

                    if (int.TryParse(dataTable.Rows[0]["siKey"].ToString(), out iIpKey) == false)
                        iIpKey = 0;
                    if (int.TryParse(dataTable.Rows[0]["siPeriodHits"].ToString(), out iPeriodHits) == false)
                        iPeriodHits = 0;
                    if (int.TryParse(dataTable.Rows[0]["siLifeHits"].ToString(), out iLifeHits) == false)
                        iLifeHits = 0;
                    if (int.TryParse(dataTable.Rows[0]["siTimeoutCount"].ToString(), out iTimeoutCount) == false)
                        iTimeoutCount = 0;
                    if (int.TryParse(dataTable.Rows[0]["siBlacklisted"].ToString(), out iBlacklisted) == false)
                        iBlacklisted = 0;
                    sUserIdLast = dataTable.Rows[0]["siUserID"].ToString();

                    if ((sUserIdLast != null) && (sUserIdLast != ""))
                    {
                        if (sUserIdLast.ToLower().Contains(sUserIdCurrent.ToLower()) == true)
                        {
                            sUserIdCurrent = sUserIdLast;
                        }
                        else if (sUserIdLast.Length < (99-sUserIdCurrent.Length))
                        {
                            sUserIdCurrent = sUserIdLast + " " + sUserIdCurrent;
                        }
                    }

                    ts = DateTime.Now - datTemp;
                    //if (iBlacklisted == 1)
                    //{
                    //    sRedirect = "LOCKOUT";
                    //}
                    //else if (iIpKey > 0)
                    if (iIpKey > 0)
                    {
                        if (Convert.ToDouble(ts.TotalHours.ToString()) < dTimeoutHours) // Inside 2 hour window
                        {
                            if (iPeriodHits < iMaxPeriodHits)
                            {
                                // Just Increment Hits during this period
                                sSql = "Update HitsFromSameIP" +
                                    " set siPeriodHits = @PeriodHits" +
                                    ", siLifeHits = @LifeHits" +
                                    ", siLastAccess = @LastAccess" +
                                    ", siUserID = @UserIdCurrent" +
                                    " where siKey = @IpKey";
                                sqlCmd = new SqlCommand(sSql, sqlConn);

                                sqlCmd.Parameters.Add("@PeriodHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@PeriodHits"].Value = iPeriodHits + 1;

                                sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                                sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                                sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                                sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                                sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                            }
                            else
                            {
                                // Increment Hits, Reset Temp Lock and Start time for 2 hour user timeout window
                                sSql = "Update HitsFromSameIP" +
                                    " set siPeriodHits = @PeriodHits" +
                                    ", siLifeHits = @LifeHits" +
                                    ", siLastAccess = @LastAccess" +
                                    ", siTimeoutCount = @TimeoutCount" +
                                    ", siBlacklisted = @Blacklisted" +
                                    ", siPeriodStart = @TimeStart" +
                                    ", siUserID = @UserIdCurrent" +
                                    " where siKey = @IpKey";
                                sqlCmd = new SqlCommand(sSql, sqlConn);

                                sqlCmd.Parameters.Add("@PeriodHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@PeriodHits"].Value = iPeriodHits + 1;

                                sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                                sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                                if (iPeriodHits == iMaxPeriodHits)
                                {
                                    iTimeoutCount++;
                                }

                                sqlCmd.Parameters.Add("@TimeoutCount", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@TimeoutCount"].Value = iTimeoutCount;

                                // 10 temp timeouts? Lock out this IP!
                                if (iTimeoutCount >= iMaxTimeouts)
                                {
                                    iBlacklisted = 1;
                                    sRedirect = "LOCKOUT";
                                }
                                else
                                {
                                    sRedirect = "TIMEOUT";
                                }

                                sqlCmd.Parameters.Add("@Blacklisted", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@Blacklisted"].Value = iBlacklisted;

                                sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                                sqlCmd.Parameters["@TimeStart"].Value = DateTime.Now;

                                sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                                sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                                sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                                sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                            }
                        }
                        else // Been more than 2 hours since last visit
                        {
                            // Reset start time for IP
                            sSql = "Update HitsFromSameIP" +
                                " set siPeriodHits = 1" +
                                ", siLifeHits = @LifeHits" +
                                ", siLastAccess = @LastAccess" +
                                ", siPeriodStart = @TimeStart" +
                                ", siUserID = @UserIdCurrent" +
                                " where siKey = @IpKey";
                            sqlCmd = new SqlCommand(sSql, sqlConn);

                            sqlCmd.Parameters.Add("@LifeHits", System.Data.SqlDbType.Int);
                            sqlCmd.Parameters["@LifeHits"].Value = iLifeHits + 1;

                            sqlCmd.Parameters.AddWithValue("@LastAccess", DateTime.Now);

                            datTemp = DateTime.Now;
                            sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                            sqlCmd.Parameters["@TimeStart"].Value = datTemp;

                            sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                            sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                            sqlCmd.Parameters.Add("@IpKey", System.Data.SqlDbType.Int);
                            sqlCmd.Parameters["@IpKey"].Value = iIpKey;
                        }
                        iRowsAffected = sqlCmd.ExecuteNonQuery();

                        // April 17th change, now logging activity but still locking out blacklisted users
                        if (iBlacklisted == 1)
                        {
                            sRedirect = "LOCKOUT";
                        }
                    }
                }
                else
                {
                    // Add first rec for this IP
                    sSql = "Insert into HitsFromSameIP" +
                        " (siIpa, siPeriodHits, siLifeHits, siPeriodStart, siFirstUse, siUserID)" +
                        " VALUES(@UserIP, 1, 1, @TimeStart, @FirstUse, @UserIdCurrent)";

                    sqlCmd = new SqlCommand(sSql, sqlConn);

                    sqlCmd.Parameters.Add("@UserIP", System.Data.SqlDbType.VarChar, 50);
                    sqlCmd.Parameters["@UserIP"].Value = sIpAddress;

                    datTemp = DateTime.Now;
                    sqlCmd.Parameters.Add("@TimeStart", System.Data.SqlDbType.DateTime);
                    sqlCmd.Parameters["@TimeStart"].Value = datTemp;

                    sqlCmd.Parameters.Add("@FirstUse", System.Data.SqlDbType.DateTime);
                    sqlCmd.Parameters["@FirstUse"].Value = datTemp;

                    sqlCmd.Parameters.Add("@UserIdCurrent", System.Data.SqlDbType.VarChar, 100);
                    sqlCmd.Parameters["@UserIdCurrent"].Value = sUserIdCurrent;

                    iRowsAffected = sqlCmd.ExecuteNonQuery();
                }
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
            try
            {
                if (sRedirect != "")
                {
                    if (sRedirect == "LOCKOUT")
                        Response.Redirect("~/public/error/Lockout.aspx", false);
                    else
                        Response.Redirect("~/public/error/Timeout.aspx", false);

                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                string sResult = ex.ToString();
            }
        }
    }
    // ========================================================================
    // dt.Columns.Add(MakeColumn("Links"));
    public DataColumn MakeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
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
    // ==================================================
    // ==================================================
}
