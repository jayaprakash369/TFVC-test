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

public partial class public_scmobile_web_UnitDetail : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    string sLibrary = "OMDTALIB";
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
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        if (!IsPostBack)
        {
            string sMdl = "";
            if (Request.QueryString["mdl"] != null && Request.QueryString["mdl"].ToString() != "") 
            {
                sMdl = Request.QueryString["mdl"].ToString().Trim();
                hfMdl.Value = sMdl.Trim();
            }

            int iUnit = 0;
            if (Request.QueryString["unt"] != null && Request.QueryString["unt"].ToString() != "") { if (int.TryParse(Request.QueryString["unt"].ToString().Trim(), out iUnit) == false) iUnit = 0; else hfUnit.Value = iUnit.ToString(); }

            int iCs1 = 0;
            if (Request.QueryString["cs1"] != null && Request.QueryString["cs1"].ToString() != "") { if (int.TryParse(Request.QueryString["cs1"].ToString().Trim(), out iCs1) == false) iCs1 = 0; else hfCs1.Value = iCs1.ToString(); }

            int iCs2 = 0;
            if (Request.QueryString["cs2"] != null && Request.QueryString["cs2"].ToString() != "") { if (int.TryParse(Request.QueryString["cs2"].ToString().Trim(), out iCs2) == false) iCs2 = 0; else hfCs2.Value = iCs2.ToString(); }


            lbPageTitle.Text = "Equipment Detail: " + sMdl;

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);
            DataTable dt2 = new DataTable(sMethodName);

            try 
            {
                odbcConn.Open();

                Select_CustDetail(iCs1, iCs2);
                Select_Cusequip(iUnit);
                Select_Eqpcontr(iUnit);
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
    protected void Select_CustDetail(int cs1, int cs2)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {

            string sDat = "";
            DateTime datTemp = new DateTime();

            sSql = "Select" +
                 " CUSTNM" +
                ", CSTRNR" +
                ", CSTRCD" +
                ", SADDR1" +
                ", SADDR2" +
                ", CITY" +
                ", STATE" +
                ", ZIPCD" +
                ", CONTNM" +
                ", HPHONE" +
                ", CMPCD" +
                ", IFNULL((select CENTNM from " + sLibrary + ".CENTER# c2 where c2.CENTER = c.CMPCD),'') as CenterName" +
                " from " + sLibrary + ".CUSTMAST c" +
                " where CSTRNR = ?" +
                " and CSTRCD = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Cs1", cs1);
            odbcCmd.Parameters.AddWithValue("@Cs2", cs2);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            string sTemp = "";

            if (dt.Rows.Count > 0)
            {
                lbCs1Cs2.Text = cs1 + "-" + cs2;
                //GetResponseTime(iCs1, iCs2);
                lbCustName.Text = dt.Rows[0]["CUSTNM"].ToString().Trim();
                lbAddress1.Text = dt.Rows[0]["SADDR1"].ToString().Trim();
                lbAddress2.Text = dt.Rows[0]["SADDR2"].ToString().Trim();
                lbCtyStZp.Text = dt.Rows[0]["CITY"].ToString().Trim() + ", " + dt.Rows[0]["STATE"].ToString().Trim() + " " + dt.Rows[0]["ZIPCD"].ToString().Trim();
                lbContact.Text = dt.Rows[0]["CONTNM"].ToString().Trim();
                lbCenter.Text = dt.Rows[0]["CMPCD"].ToString().Trim();
                lbCenterName.Text = dt.Rows[0]["CenterName"].ToString().Trim();
                sTemp = dt.Rows[0]["HPHONE"].ToString().Trim();
                if (sTemp.Length == 10 && sTemp != "9999999999" && sTemp != "8888888888" && sTemp != "0000000000")
                {
                    try
                    {
                        sTemp = "(" + sTemp.Substring(0, 3) + ") " + sTemp.Substring(3, 3) + "-" + sTemp.Substring(6, 4);
                    }
                    catch (Exception ex) { string sError = ex.ToString(); }
                    lbPhone.Text = sTemp;
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
    }
    // ========================================================================
    protected int Select_Cusequip(int unit)
    {
        int iUnt = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " CESYS#" +
                ", CEPRT#" +
                ", CEMOD#" +
                ", CESER#" +
                ", CEFAA" +
                ", IMFDSC" +
                ", PMPROD" +
                " from " + sLibrary + ".CUSEQUIP, " + sLibrary + ".PRODMST" +
                " where CEPRT# = PARTNR" +
                " and CESYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unt", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                lbUnit.Text = unit.ToString();
                //   lbAgentId.Text = GetAgentId(iUnt);
                lbPart.Text = dt.Rows[0]["CEPRT#"].ToString().Trim();
                lbDescription.Text = dt.Rows[0]["IMFDSC"].ToString().Trim();
                lbSerial.Text = dt.Rows[0]["CESER#"].ToString().Trim();
                lbFixAsset.Text = dt.Rows[0]["CEFAA"].ToString().Trim();
                lbEqpType.Text = dt.Rows[0]["PMPROD"].ToString().Trim();
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
        return iUnt;
    }
    // ========================================================================
    protected int Select_Eqpcontr(int unit)
    {
        int iUnt = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " ECSYS#" +
                ", EPART" +
                ", ESERL" +
                ", ECCNTR" +
                ", ECNTYP" +
                ", PRITEC" +
                ", DATON" +
                ", IMFDSC" +
                ", PMPROD" +
                " from " + sLibrary + ".EQPCONTR e, " + sLibrary + ".PRODMST p" +
                " where EPART = PARTNR" +
                " and ECSYS# = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Unt", unit);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            string sAgr = "";
            string[] saDscBegEnd = { "", "", "" };
            string sDat = "";
            DateTime datTemp;

            if (dt.Rows.Count > 0)
            {
                sAgr = dt.Rows[0]["ECCNTR"].ToString().Trim();
                lbAgr.Text = sAgr;
                
                sDat = dt.Rows[0]["DATON"].ToString().Trim();
                if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                    sDat = datTemp.ToString("MMM d, yyyy");
                lbAddDte.Text = sDat;

                saDscBegEnd = Select_AgrData(sAgr);
                if (saDscBegEnd.Length > 2)
                {
                    lbAgrType.Text = dt.Rows[0]["ECNTYP"].ToString().Trim() + " " + saDscBegEnd[0];

                    sDat = saDscBegEnd[1];
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        sDat = datTemp.ToString("MMM d, yyyy");
                    lbAgrBeg.Text = sDat;

                    sDat = saDscBegEnd[2];
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true)
                        sDat = datTemp.ToString("MMM d, yyyy");
                    lbAgrEnd.Text = sDat;
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
        return iUnt;
    }
    // ========================================================================
    protected string[] Select_AgrData(string agr)
    {
        string[] saDscBegEnd = { "", "", "" };

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " CONDSC" +
                ", CDATE" +
                ", EDATE" +
                " from " + sLibrary + ".ISERVREQL1" +
                " where CONTNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Agr", agr);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
            {
                saDscBegEnd[0] = dt.Rows[0]["CONDSC"].ToString().Trim();
                saDscBegEnd[1] = dt.Rows[0]["CDATE"].ToString().Trim();
                saDscBegEnd[2] = dt.Rows[0]["EDATE"].ToString().Trim();
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
        return saDscBegEnd;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
}
