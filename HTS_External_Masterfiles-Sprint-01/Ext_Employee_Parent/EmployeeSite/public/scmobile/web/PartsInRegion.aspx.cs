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

public partial class public_scmobile_web_PartsInRegion : MyPage
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

        if (!IsPostBack)
        {
            int iUsr = 0;
            if (Request.QueryString["usr"] != null && Request.QueryString["usr"].ToString() != "") { if (int.TryParse(Request.QueryString["usr"].ToString().Trim(), out iUsr) == false) iUsr = 0; else hfUsr.Value = iUsr.ToString(); }
            int iRgn = 0;
            if (Request.QueryString["rgn"] != null && Request.QueryString["rgn"].ToString() != "") { if (int.TryParse(Request.QueryString["rgn"].ToString().Trim(), out iRgn) == false) iRgn = 0; else hfRgn.Value = iRgn.ToString(); }

            lbPageTitle.Text = "Parts In Region " + iRgn;
        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetPart()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        string sPart = txSearchPart.Text.Trim().ToUpper();
        string sDesc = txSearchDesc.Text.Trim().ToUpper();
//        string sSubt = txSearchSubt.Text.Trim().ToUpper();

        //if (String.IsNullOrEmpty(sPart) && String.IsNullOrEmpty(sPart) && String.IsNullOrEmpty(sSubt))
        if (String.IsNullOrEmpty(sPart) && String.IsNullOrEmpty(sDesc))
        {
            lbMsg.Text = "A search entry is required.";
            txSearchPart.Focus();
        }
        else 
        {
            try
            {

                int iUsr = 0;
                int iRgn = 0;
                int.TryParse(hfUsr.Value, out iUsr);
                int.TryParse(hfRgn.Value, out iRgn);

                sLibrary = "OMDTALIB";

                sSql = "Select" +
                    " PARTNR" +
                    ", IMFDSC" +
                    ", STDCST" +
                    ", PESUBT" +
                    ", IFNULL((select sum(LOCQOH) from " + sLibrary + ".SLOCITM, " + sLibrary + ".STCKLOC, " + sLibrary + ".EMPMST where LOCNUM = SLCOD# and SLEMP# = EMPNUM and EFIRE = 0 and SLWRH# = 1 and LOCPNR = pm.PARTNR and LOCQOH > 0 and SLDEPT = 40" + iRgn + "),0) as AllQty" +
                    " from " + sLibrary + ".PRODMST pm, " + sLibrary + ".PRODEQP pe" +
                    " where PARTNR = PEPART" +
                    " and PMEQP <> 'Y'";
                if (!String.IsNullOrEmpty(sPart))
                    sSql += " and PARTNR like ?";
                if (!String.IsNullOrEmpty(sDesc))
                    sSql += " and IMFDSC like ?";
//                if (!String.IsNullOrEmpty(sSubt))
//                    sSql += " and PESUBT like ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                if (!String.IsNullOrEmpty(sPart))
                    odbcCmd.Parameters.AddWithValue("@Part", sPart + "%");
                if (!String.IsNullOrEmpty(sDesc))
                    odbcCmd.Parameters.AddWithValue("@Desc", "%" + sDesc + "%");
//                if (!String.IsNullOrEmpty(sSubt))
//                    odbcCmd.Parameters.AddWithValue("@Type", sSubt + "%");

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);

                dt.Columns.Add(MakeColumn("Qty"));
                string sQty = "";

                // string sTemp = "";
                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    sQty = dt.Rows[iRowIdx]["AllQty"].ToString().Trim();
                    if (sQty != "0")
                        dt.Rows[iRowIdx]["Qty"] = sQty;
                    else {
                        if (chbxQty.Checked == true)
                            row.Delete();
                    }

                    iRowIdx++;
                }
                dt.AcceptChanges();

                // lbMsg.Text = sSql;
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
    protected DataTable GetTechs(string part)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

            try
            {

                int iUsr = 0;
                int iRgn = 0;
                int.TryParse(hfUsr.Value, out iUsr);
                int.TryParse(hfRgn.Value, out iRgn);

                sLibrary = "OMDTALIB";

                sSql = "Select" +
                     " EMPNAM" +
                    ", EMPNUM" +
                    ", LOCNUM" +
                    ", LOCQOH" +
                    " from " + sLibrary + ".SLOCITM sli, " + sLibrary + ".STCKLOC sl, " + sLibrary + ".EMPMST em" +
                    " where LOCNUM = SLCOD#" +
                    " and SLEMP# = EMPNUM" +
                    " and EFIRE = 0" +
                    " and SLDEPT = 40" + iRgn +
                    " and LOCQOH > 0" +
                    " and LOCPNR = ?" +
                    " order by EMPNAM";

                odbcCmd = new OdbcCommand(sSql, odbcConn);
                odbcCmd.Parameters.AddWithValue("@Part", part);
                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dt.Load(odbcReader);
/*
                // string sTemp = "";
                int iRowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    // dt.Rows[iRowIdx]["Qty"] = sQty;

                    iRowIdx++;
                }
                dt.AcceptChanges();
*/
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
    public string GetEmpPhone(int empNum)
    {
        string sEmpPhone = "1234567890";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " APGR#" +
                " from " + sLibrary + ".EMPMST" +
                " where EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@EmpNum", empNum);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0) {
                sEmpPhone = "2345678901";
                sEmpPhone = dt.Rows[0]["APGR#"].ToString().Trim();
                if (sEmpPhone.Length == 10) 
                {
                    sEmpPhone = "3456789012";
                    try
                    {
                        sEmpPhone = "(" + sEmpPhone.Substring(0, 3) + ") " + sEmpPhone.Substring(3, 3) + "-" + sEmpPhone.Substring(6, 4);
                    }
                    catch (Exception ex) 
                    {
                        sEmpPhone = "4567890123";
                        eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
                    }
                }
            }
                
        }
        catch (Exception ex)
        {
            ErrorHandler eh = new ErrorHandler();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            eh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sEmpPhone;
    }
    // ========================================================================
    #endregion // end mySqls
    // ================================================================
    // ========================================================================
    #region tableChanges
    // =========================================================
    // =========================================================
    protected void UpdPart_rp()
    {
        // ((RepeaterItem)rpSummary.Controls[i]).NamingContainer.ToString();
        HiddenField hfTemp = new HiddenField();
        Label lbTemp = new Label();
        LinkButton lkTemp = new LinkButton();

        string sType = "";
        string sShowLink = "";

        foreach (Control c1 in rpPart.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfQty") 
                        {
                            if (!String.IsNullOrEmpty(hfTemp.Value))
                                sShowLink = "Y";
                        }
                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPart")
                        {
                            if (sShowLink == "Y")
                                lbTemp.Visible = false;
                            else
                                lbTemp.Visible = true;
                        }
                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                    {
                        lkTemp = (LinkButton)c2;
                        if (lkTemp.ID == "lkPart")
                        {
                            if (sShowLink == "Y")
                                lkTemp.Visible = true;
                            else
                                lkTemp.Visible = false;
                            // After last time it could be used on this line, reset it for next record
                            sShowLink = "";
                        }
                    }
                    //-------------------------------------------------------------------------
                    //-------------------------------------------------------------------------
                }
            }
        }
    }
    // =========================================================
    #endregion // end tableChanges
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // =========================================================
    protected void btClear_Click(object sender, EventArgs e)
    {
        txSearchPart.Text = "";
        txSearchDesc.Text = "";
//        txSearchSubt.Text = "";
    }
    // ================================================================
    protected void btSearch_Click(object sender, EventArgs e)
    {
        try
        {
            odbcConn.Open();
            rpPart.DataSource = GetPart();
            rpPart.DataBind();
            UpdPart_rp();
            rpPart.Visible = true;
            pnTechs.Visible = false;
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
    // ================================================================
    protected void lkPart_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {

            LinkButton lkControl = (LinkButton)sender;
            char[] cSplitter = { '|' };
            string[] saParms = lkControl.CommandArgument.ToString().Split(cSplitter);
            hfPrt.Value = saParms[0].Trim();
            hfDsc.Value = saParms[1].Trim();

            odbcConn.Open();
            gvTechs.DataSource = GetTechs(hfPrt.Value);
            gvTechs.DataBind();
            pnSearch.Visible = false;
            pnTechs.Visible = true;
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
    // ================================================================
    protected void lkTech_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {

            LinkButton lkControl = (LinkButton)sender;
            char[] cSplitter = { '|' };
            string[] saParms = lkControl.CommandArgument.ToString().Split(cSplitter);
            hfNum.Value = saParms[0];
            hfNam.Value = saParms[1];

            int iTech = 0;
            int.TryParse(hfNum.Value, out iTech);

            odbcConn.Open();

            lbEmailTech.Text = hfNam.Value + " (" + hfNum.Value + ")";
            lbEmailPart.Text = hfPrt.Value;
            lbEmailDesc.Text = hfDsc.Value;
            string sTargetPhone = GetEmpPhone(iTech);
            lbEmailPhone.Text = sTargetPhone;
            // lbMsg.Text = hfNum.Value + " / " + hfNam.Value + " / " + hfPrt.Value;
            
            pnTechs.Visible = false;
            pnEmail.Visible = true;
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
    // ================================================================
    protected void btEmail_Click(object sender, EventArgs e)
    {
        try
        {
            odbcConn.Open();
            int iEmp = 0;
            int.TryParse(hfNum.Value, out iEmp);
            if (iEmp > 0) {
                string sTargetEmail = GetEmpEmail(iEmp);
                sTargetEmail = "htslog@yahoo.com";
                
                int.TryParse(hfUsr.Value, out iEmp);
                string sSourceEmail = GetEmpEmail(iEmp);
                string sSourceName = GetEmpName(iEmp);
                string sSourcePhone = GetEmpPhone(iEmp);
                EmailHandler eh = new EmailHandler();
                string sSubject = hfPrt.Value + " requested by " + sSourceName;
                string sMessage = "<html><body><div style=\"font-size:16px;\">Part: " + hfPrt.Value + "<br /><br />" + "Desc: " + hfDsc.Value + "<br /><br />" + "Phone: " + sSourcePhone + "<br /><br />" + txEmailMsg.Text.Trim() + "</div></body></html>";
                eh.EmailIndividual(sSubject, sMessage, sTargetEmail, sSourceEmail, "HTML");
                eh = null;
            }

            pnEmail.Visible = false;
            pnSearch.Visible = true;
            rpPart.Visible = false;
            
            txSearchPart.Text = "";
            txSearchDesc.Text = "";
//            txSearchSubt.Text = "";

            lbMsg.Text = "Email Submitted";

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
    // ========================================================================
    #endregion // actionEvents
    // =========================================================
}