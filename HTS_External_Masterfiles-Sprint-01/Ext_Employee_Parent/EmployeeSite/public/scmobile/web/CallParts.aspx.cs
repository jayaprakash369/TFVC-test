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

public partial class public_scmobile_web_CallParts : MyPage
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
        lbMsg.Text = "";
        lbMsg.Visible = false;

        if (!IsPostBack)
        {
            int iCtr = 0;
            if (Request.QueryString["ctr"] != null && Request.QueryString["ctr"].ToString() != "") { if (int.TryParse(Request.QueryString["ctr"].ToString().Trim(), out iCtr) == false) iCtr = 0; else hfCtr.Value = iCtr.ToString(); }
            int iTck = 0;
            if (Request.QueryString["tck"] != null && Request.QueryString["tck"].ToString() != "") { if (int.TryParse(Request.QueryString["tck"].ToString().Trim(), out iTck) == false) iTck = 0; else hfTck.Value = iTck.ToString(); }

            //iCtr = 4;
            //hfCtr.Value = iCtr.ToString();
            //iTck = 16440;
            //hfTck.Value = iTck.ToString();

            lbPageTitle.Text = "Parts: " + iCtr + "-" + iTck;

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dt = new DataTable(sMethodName);

            try 
            {
                odbcConn.Open();
                
                // ---------------------------
                dt = GetPartsUsed();
                if (dt.Rows.Count > 0)
                {
                    pnPartsUsed.Visible = true;
                    rpPartsUsed.DataSource = dt;
                    rpPartsUsed.DataBind();
                }
                else 
                {
                    lbMsg.Text += "No part use was found<br />";
                    lbMsg.Visible = true;
                }
                    
               // ---------------------------
                dt = GetPartsShipped();
                if (dt.Rows.Count > 0)
                {
                    pnPartsShipped.Visible = true;
                    rpPartsShipped.DataSource = dt;
                    rpPartsShipped.DataBind();
                }
                else 
                {
                    lbMsg.Text += "No part shipment was found<br />";
                    lbMsg.Visible = true;
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
    protected DataTable GetPartsUsed()
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
                " TPRT# as Part" +
                ", TPDESC as Description" +
                ", TPQTY as Qty" +
                ", TPSER# as Serial" +
                ", TPLOC as Location" +
                ", TPDATE as DateUsed" +
                " from " + sLibrary + ".TICKPART" +
                " where TPCENT = ?" +
                " and TCKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
            odbcCmd.Parameters.AddWithValue("@Tck", iTck);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));

            int iDat = 0;

            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["DateUsed"].ToString(), out iDat);
                if (iDat > 20000101 && iDat < 20990101)
                    dt.Rows[iRowIdx]["DisplayDate"] = df.FormatDate(iDat, "Mon dd, YYYY");

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
    protected DataTable GetPartsShipped()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        int iCtr = 0;
        int iTck = 0;
        int.TryParse(hfCtr.Value, out iCtr);
        int.TryParse(hfTck.Value, out iTck);

        if (iCtr > 0)
        {
            try
            {
                sSql = "Select" +
                    " PTISEQ as Seq" +
                    ", PTIPRT as Part" +
                    ", PTIORQ as QtyOrdered" +
                    ", PTIFSQ as QtyFilled" +
                    ", PTIBKQ as QtyBackordered" +
                    //", PTXQTY as QtyShipped" +
                    //", PTXDAT as DateShipped" +
                    ", IFNULL((select PTXQTY from " + sLibrary + ".PTXTRA where PTXTRN = i.PTITRN and PTXTSQ = i.PTISEQ and PTXRSQ = 1),0) as QtyShipped" +
                    ", IFNULL((select PTXDAT from " + sLibrary + ".PTXTRA where PTXTRN = i.PTITRN and PTXTSQ = i.PTISEQ and PTXRSQ = 1),0) as DateShipped" +
                    //" from " + sLibrary + ".PTHEAD h, " +  sLibrary + ".PTITEM i, " + sLibrary + ".PTXTRA" +
                    " from " + sLibrary + ".PTHEAD h, " +  sLibrary + ".PTITEM i" +
                    " where PTHTRN = PTITRN" +
                    // " and PTITRN = PTXTRN" +
                    " and PTHVCD = ?" +
                    " and PTHVRF = ?" +
                    " order by PTISEQ";

                //Response.Write(sSql);

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Cod", "ST");
                odbcCmd.Parameters.AddWithValue("@Ref", iCtr.ToString("000") + iTck.ToString("0000000"));
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                dt.Columns.Add(MakeColumn("DisplayShipped"));

                DateTime datTemp;
                string sDat = "";
                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    sDat = dt.Rows[iRowIdx]["DateShipped"].ToString().Trim();
                    if (sDat != "" && sDat.Length == 8)
                    {
                        DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp);
                        dt.Rows[iRowIdx]["DisplayShipped"] = datTemp.ToString("MMM d, yyyy");
                    }

                    iRowIdx++;
                }

                dt.AcceptChanges();
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
        return dt;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
}