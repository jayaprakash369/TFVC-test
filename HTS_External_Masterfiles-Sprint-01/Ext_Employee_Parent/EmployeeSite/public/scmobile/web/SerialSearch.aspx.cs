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

public partial class public_scmobile_web_SerialSearch : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    string sLibrary = "OMDTALIB";
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
            if (Request.QueryString["usr"] != null && Request.QueryString["usr"].ToString() != "") { if (int.TryParse(Request.QueryString["usr"].ToString().Trim(), out iUsr) == false) iUsr = 0; else hfUsr.Value = iUsr.ToString(); }

            LoadSerials();
        }
    }
    // ================================================================
    protected void LoadSerials()
    {
        try
        {
            odbcConn.Open();
            rpMach.DataSource = GetSerials();
            rpMach.DataBind();
            rpMach.Visible = true;
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
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetSerials()
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        
        int iUsr = 0;
        if (int.TryParse(hfUsr.Value, out iUsr) == false)
            iUsr = 0;
 
        string sSerial = txSearchSerial.Text.Trim().ToUpper();

        if (!String.IsNullOrEmpty(sSerial))
        {
            try
            {

                // sLibrary = "OMDTALIB";
                sSql = "Select" +
                     // Cusequip
                     " CEPRT#" +
                    ", CESER#" +
                    ", CEMOD#" +
                    ", CESYS#" +
                    ", CERNR" +
                    ", CERCD" +
                    ", CESYS#" +
                    ", CUSTNM" +
                    ", CITY" +
                    ", STATE" +
                    " from " + sLibrary + ".CUSEQUIP, " + sLibrary + ".CUSTMAST" +
                    " Where CERNR = CSTRNR and CERCD = CSTRCD" +
                    " AND CESER# like ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Nam", sSerial + "%");
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                string sTemp = "";

                if (dt.Rows.Count == 1)
                    sTemp = "1 Serial# Found";
                else
                    sTemp = dt.Rows.Count.ToString() + " Serials Found";

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
    #endregion // end mySqls
    // ================================================================
    // ========================================================================
    #region actionEvents
    // =========================================================
    protected void btClear_Click(object sender, EventArgs e)
    {
        txSearchSerial.Text = "";
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        LoadSerials();
    }
    // ================================================================
    protected void lkSerial_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        LinkButton lkControl = (LinkButton)sender;
        string[] saParms = lkControl.CommandArgument.ToString().Split('|');

        string sMod = "";
        string sUnt = "";
        string sCs1 = "";
        string sCs2 = "";

        try
        {

            if (saParms.Length > 3)
            {
                sMod = HttpUtility.UrlEncode(saParms[0].Trim());
                sUnt = saParms[1].Trim();
                sCs1 = saParms[2].Trim();
                sCs2 = saParms[3].Trim();
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

        if (sMod != "" && sUnt != "") 
        {
            string sParms = "mdl=" + sMod + "&unt=" + sUnt + "&cs1=" + sCs1 + "&cs2=" + sCs2;
            Response.Redirect("~/public/scmobile/web/UnitDetail.aspx?" + sParms);
            Response.End();
        }

    }
    // ========================================================================
    #endregion // actionEvents
    // =========================================================
}