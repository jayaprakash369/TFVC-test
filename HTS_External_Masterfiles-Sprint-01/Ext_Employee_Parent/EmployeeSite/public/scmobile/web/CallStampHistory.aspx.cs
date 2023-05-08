using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Web.Security;

public partial class public_scmobile_web_CallStampHistory : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    // string sLibrary = "OMDTALIB";
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;
    DateFormatter df;
    ErrorHandler eh;
    string sConnectionString = "";
    string sSql = "";

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); df = new DateFormatter(); this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; df = null; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack)
        {
            int iCtr = 0;
            if (Request.QueryString["ctr"] != null && Request.QueryString["ctr"].ToString() != "") { if (int.TryParse(Request.QueryString["ctr"].ToString().Trim(), out iCtr) == false) iCtr = 0; else hfCtr.Value = iCtr.ToString(); }
            int iTck = 0;
            if (Request.QueryString["tck"] != null && Request.QueryString["tck"].ToString() != "") { if (int.TryParse(Request.QueryString["tck"].ToString().Trim(), out iTck) == false) iTck = 0; else hfTck.Value = iTck.ToString(); }

            lbPageTitle.Text = "Timestamp History: " + iCtr + "-" + iTck;

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            DataTable dt2 = new DataTable(sMethodName);

            try 
            {
                odbcConn.Open();

                dt = GetTimestamps();
                if (dt.Rows.Count > 0)
                {
                    pnTimestamps.Visible = true;
                    rpTimestamps.DataSource = dt;
                    rpTimestamps.DataBind();
                }
               // ---------------------------

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
    #region mySqls
    // ========================================================================
    protected DataTable GetTimestamps()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            int iCtr = 0;
            int iTck = 0;
            int.TryParse(hfCtr.Value, out iCtr);
            int.TryParse(hfTck.Value, out iTck);

            sSql = "Select" +
                " TIMTCH" +
                ", TIMSTS" +
                ", TIMDST" +
                ", TIMTST" +
                ", TIMRSC" +
                ", STSCDL" +
                ", STSCDD" +
                ", RSNINT" +
                ", EMPDEP" +
                " from " + sLibrary + ".TIMESTMP t, " + sLibrary + ".STSCDEPF s, " + sLibrary + ".RSNCODPF r, " + sLibrary + ".EMPMST e" +
                " where TIMSTS = STSCDE" +
                " and TIMRSC = RSNCD" +
                " and TIMTCH = EMPNUM" +
                " and TIMDST <> 0" +
                " and TIMCTR = ?" +
                " and TIMTCK = ?" +
                " order by TIMDST, TIMTST";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayStamp"));
            dt.Columns.Add(MakeColumn("DisplayDate"));
            dt.Columns.Add(MakeColumn("DisplayTime"));

            string sStamp = "";
            string sDat = "";
            string sTim = "";
            double dTim = 0.0;
            int iDep = 0;
            DateTime datTemp = new DateTime();

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                sStamp = dt.Rows[iRowIdx]["TIMSTS"].ToString().Trim();
                sDat = dt.Rows[iRowIdx]["TIMDST"].ToString().Trim();
                sTim = dt.Rows[iRowIdx]["TIMTST"].ToString().Trim();

                iDep = 0;
                if (int.TryParse(dt.Rows[iRowIdx]["EMPDEP"].ToString().Trim(), out iDep) == false)
                    iDep = 0;

                if (iCtr <= 400 || iCtr == 420 || iCtr > 900 ||
                    ((iDep >= 401 && iDep <= 408) || iDep == 851)
                )
                {
                    if (sStamp != "H")
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["STSCDL"].ToString().Trim();
                    else
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["RSNINT"].ToString().Trim();
                }
                else
                {
                    if (sStamp != "H")
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["STSCDD"].ToString().Trim();
                    else
                        dt.Rows[iRowIdx]["DisplayStamp"] = dt.Rows[iRowIdx]["RSNINT"].ToString().Trim();
                }
                if (sDat.Length == 8)
                {
                    try
                    {
                        datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                        dt.Rows[iRowIdx]["DisplayDate"] = sMonthAbbrev[datTemp.Month] + " " + datTemp.Day.ToString() + ", " + datTemp.Year.ToString();
                    }
                    catch (Exception ex)
                    {
                        string sError = ex.ToString(); // usually a bad date i.e February 31st.
                    }
                }
                if (double.TryParse(dt.Rows[iRowIdx]["TIMTST"].ToString().Trim(), out dTim) == true)
                    dt.Rows[iRowIdx]["DisplayTime"] = df.FormatTime(dTim, ": pm");
                iRowIdx++;
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
        return dt;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
}