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

public partial class public_scmobile_web_CallList : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    // string sLibrary = "OMDTALIB";
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;
    ErrorHandler erh;
    string sConnectionString = "";
    string sSql = "";

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { erh = new ErrorHandler(); this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { erh = null;  }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        lbMsg.Text = "";

        if (!IsPostBack)
        {
            int iUsr = 0;
            if (Request.QueryString["usr"] != null && Request.QueryString["usr"].ToString() != "")
            {
                if (int.TryParse(Request.QueryString["usr"].ToString().Trim(), out iUsr) == false)
                    iUsr = 0;
                else
                {
                    hfUsr.Value = iUsr.ToString();
                    txSearchEmp.Text = iUsr.ToString();
                }
            }

            LoadCalls();
        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetCalls()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        
        int iUsr = 0;
        //if (int.TryParse(hfUsr.Value, out iUsr) == false)
        if (int.TryParse(txSearchEmp.Text.Trim(), out iUsr) == false)
            iUsr = 0;
        
        int iCtr = 0;
        if (int.TryParse(txSearchCtr.Text.Trim(), out iCtr) == false)
            iCtr = 0;
        
        //int iTck = 0;
        //if (int.TryParse(txSearchTck.Text.Trim(), out iTck) == false)
        //    iTck = 0;

        //string sNam = txSearchNam.Text.Trim().ToUpper();

        if (iCtr < 0 || iCtr > 999)
            iCtr = 0;
        //if (iTck < 0 || iTck > 9999999)
        //    iTck = 0;
        if (iUsr == 0 && iCtr == 0)
            lbMsg.Text = "I need some search values...";
        else
        {
            try
            {

                // sLibrary = "OMDTALIB";

                sSql = "Select" +
                     // SVRTICK
                     " TCCENT" +
                    ", TICKNR" +
                    ", TRESP" +
                    ", STCUS1" +
                    ", STCUS2" +
                    ", CONTNR" +
                    ", CALLCD" +
                    ", PBMSCD" +
                    ", TCOMM1" +
                    ", TCOMM2" +
                    ", REMARK" +
                    ", TEDATE" +
                    ", TCKTIM" +
                    ", TPRIO" +
                    ", STPRCH" +
                    // SVRTICKD
                    ", SDCSTN" +
                    //", SDADR1" +
                    //", SDADR2" +
                    //", SDADR3" +
                    ", SDCITY" +
                    ", SDSTAT" +
                    ", SDZIPC" +
                    //", SDCONT" +
                    //", SDPHN#" +
                    //", SDPHNE" +
                    // ", IFNULL((select PEMLIF from " + sLibrary + ".PRODEQP where PEPART = c.cPartName and PEPART <> ''),0) as cartridgePages" +
                    " from " + sLibrary + ".SVRTICK s, " + sLibrary + ".SVRTICKD d, " + sLibrary + ".ASSTCKOP a" +
                    " where TCCENT = SDCENT" +
                    " and TICKNR = SDTNUM" +
                    " and TCCENT = TACENT" +
                    " and TICKNR = ATCK#" +
                    " and PBMSCD < 90";

                if (iUsr > 0)
                    sSql += " and AEMP# = ?";
                if (iCtr > 0)
                    sSql += " and TCCENT = ?";
                //if (iTck > 0)
                //    sSql += " and TICKNR = ?";
                //if (!String.IsNullOrEmpty(sNam))
                //    sSql += " and SDCSTN like ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                if (iUsr > 0)
                    odbcCmd.Parameters.AddWithValue("@Emp", iUsr);
                if (iCtr > 0)
                    odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
                //if (iTck > 0)
                //    odbcCmd.Parameters.AddWithValue("@Tck", iTck);
                //if (!String.IsNullOrEmpty(sNam))
                //    odbcCmd.Parameters.AddWithValue("@Nam", sNam + "%");

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                string sTemp = "";
                /*

                            int iRowIdx = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                sQty = dt.Rows[iRowIdx]["AllQty"].ToString().Trim();
                                iRowIdx++;
                            }
                            dt.AcceptChanges();
                */
                // lbMsg.Text = sSql;

                string sName = "";
                if (iUsr > 0)
                    sName = Select_EmpName(iUsr);
                else if (iCtr > 0)
                    sName = Select_EmpCtr(iCtr);

                if (dt.Rows.Count == 1)
                    sTemp = "1 Call (" + sName + ")";
                else
                    sTemp = dt.Rows.Count.ToString() + " Calls (" + sName + ")";

                lbPageTitle.Text = sTemp;
            }
            catch (Exception ex)
            {
                erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return dt;
    }
    // ========================================================================
    protected string Select_EmpName(int emp)
    {
        string sName = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                 " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Emp", emp);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                sName = dt.Rows[0]["EMPNAM"].ToString().Trim();
                if (!String.IsNullOrEmpty(sName) && sName.Length > 15)
                    sName = sName.Substring(0, 15);
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sName;
    }
    // ========================================================================
    protected string Select_EmpCtr(int ctr)
    {
        string sName = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                 " CENTNM" +
                " from " + sLibrary + ".CENTER#" +
                " where CENTER = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ctr", ctr);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                sName = dt.Rows[0]["CENTNM"].ToString().Trim();
                if (!String.IsNullOrEmpty(sName) && sName.Length > 15)
                    sName = sName.Substring(0, 15);
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sName;
    }
    // ========================================================================
    #endregion // end mySqls
    // ================================================================
    // ========================================================================
    #region actionEvents
    // =========================================================
    protected void btClear_Click(object sender, EventArgs e)
    {
        txSearchEmp.Text = "";
        txSearchCtr.Text = "";
        //txSearchTck.Text = "";
        //txSearchNam.Text = "";
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        LoadCalls();
    }
    // ================================================================
    protected void LoadCalls()
    {
        try
        {
            odbcConn.Open();
            rpCalls.DataSource = GetCalls();
            rpCalls.DataBind();
            rpCalls.Visible = true;
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
    }
    // ================================================================
    protected void lkName_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        int iCtr = 0;
        int iTck = 0;

        try
        {

            LinkButton lkControl = (LinkButton)sender;
            char[] cSplitter = { '|' };
            string[] saParms = lkControl.CommandArgument.ToString().Split(cSplitter);
            if (saParms.Length > 1) 
            {
                if (int.TryParse(saParms[0].Trim(), out iCtr) == false)
                    iCtr = 0;
                if (int.TryParse(saParms[1].Trim(), out iTck) == false)
                    iTck = 0;
            }

        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

        if (iCtr > 0 && iTck > 0) 
        {
            Response.Redirect("~/public/scmobile/web/CallDetail.aspx?ctr=" + iCtr + "&tck=" + iTck);
            Response.End();
        }

    }
    // ========================================================================
    #endregion // actionEvents
    // =========================================================
}