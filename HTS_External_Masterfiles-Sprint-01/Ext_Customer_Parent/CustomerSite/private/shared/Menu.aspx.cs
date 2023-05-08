using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

public partial class private_shared_Menu : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    string sCs1Changed = "";

    //int iWidth = 200; // in pixels
    //int iIncrement = 2;
    //int iPercentComplete = 0;
    //ProgressBars pb = new ProgressBars();

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        //        Response.Write("My timeout... " + HttpContext.Current.Session.Timeout);

        CheckCs1Changed();
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields

        // ************************************************************************
        // NEW SITE MOVE
        // ************************************************************************
        string sMoveStage = Select_UserMoveStage(iCs1ToUse);
        if (sMoveStage == "MOVE") 
        {
            Response.Redirect("https://www.servicecommand.com");
        }
        else if (sMoveStage == "INFORM") 
        {
            // ----------------------------------------------------------------------------------
            pnNewSiteNotice.Visible = true;
            btShowNewSiteNotice.Visible = false;
            string sNewSiteNoticeVisibility = "";
            if (Session["NewSiteNoticeVisibility"] != null)
            {
                sNewSiteNoticeVisibility = Session["NewSiteNoticeVisibility"].ToString().Trim();
                if (sNewSiteNoticeVisibility == "HIDE") 
                { 
                    pnNewSiteNotice.Visible = false;
                    btShowNewSiteNotice.Visible = true;
                }
            }
            // ----------------------------------------------------------------------------------
        }
        // ************************************************************************

        if (User.IsInRole("Editor"))
        {
            string sAccessCode = "";

            if (sPageLib == "L") 
            {
                sAccessCode = wsLive.GetRegistrationAccessCode(sfd.GetWsKey());
            }
            else
            {
                sAccessCode = wsTest.GetRegistrationAccessCode(sfd.GetWsKey());
            }
            DateTime datTemp = DateTime.Now;
            int iAndroidAdmin = (datTemp.Day * datTemp.Month * (datTemp.Year - 2000)) +
                (datTemp.Year * 2);
            lbAccessCode.Text = "Today's Android Admin Code: " + iAndroidAdmin + "<br />This Website Admin Registration Code: " + sAccessCode;
            lbAccessCode.Visible = true;
        }
        if (!IsPostBack) 
        {
            setCustTypeMenu(iCs1ToUse);
            setCustTitle(iCs1ToUse);

            // Add OnClientClick="testJs" to a button to make it work (multiple commands all have to be on the same line...)
            //string myScript = @"function testJS() { document.getElementById('dvExit').style.visibility = 'visible'; alert('Pause to look...'); }";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myScript, true);
        }
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iPrimaryCs1 = iCs1User;
        int iCs1Change = 0;

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (pnCs1Change.Visible == false)
                    pnCs1Change.Visible = true;

                if (txCs1Change.Text != "")
                {
                    if (int.TryParse(txCs1Change.Text, out iCs1Change) == false)
                        iCs1Change = 0;
                    else
                    {
                        if (iCs1Change > 0)
                        {
                            Session["adminCs1"] = txCs1Change.Text;
                            iPrimaryCs1 = iCs1Change;
                            if (sCs1Changed == "YES") 
                            {
                                setCustTypeMenu(iPrimaryCs1);                                
                            }
                        }
                    }
                }
                else
                {
                    if (Session["adminCs1"] != null)
                    {
                        if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Change) == false)
                            iCs1Change = 0;
                        else
                        {
                            if (iCs1Change > 0)
                            {
                                txCs1Change.Text = iCs1Change.ToString();
                                iPrimaryCs1 = iCs1Change;
                            }
                        }
                    }
                    else
                    {
                        txCs1Change.Text = iCs1User.ToString();
                        Session["adminCs1"] = txCs1Change.Text;
                    }
                }
            }
        }
        return iPrimaryCs1;
    }
    // =========================================================
    protected string CheckCs1Changed()
    {
        sCs1Changed = "NO";

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (Session["adminCs1"] != null)
                {
                    // Admin changed cust but did not click change button
                    int iCs1Session = 0;
                    int iCs1Textbox = 0;
                    if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Session) == false)
                        iCs1Session = 0;
                    if (int.TryParse(txCs1Change.Text, out iCs1Textbox) == false)
                        iCs1Textbox = 0;

                    if ((iCs1Session != iCs1Textbox) && (txCs1Change.Text != ""))
                    {
                        sCs1Changed = "YES";
                        setCustTitle(iCs1Textbox);
                    }
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    protected void setCustTypeMenu(int currentCs1)
    {
        string sCustType = "";
        string sShowRenewals = "";

        if (sPageLib == "L")
        {
            sCustType = wsLive.GetCustType(sfd.GetWsKey(), currentCs1);
            sShowRenewals = wsLive.GetPrefRequestRenewals(sfd.GetWsKey(), currentCs1);
        }
        else
        {
            sCustType = wsTest.GetCustType(sfd.GetWsKey(), currentCs1);
            sShowRenewals = wsTest.GetPrefRequestRenewals(sfd.GetWsKey(), currentCs1);
        }

        if (sCustType == "SSP")
        {
            pnRegLrgDlr.Visible = false;
            pnDlrRenewals.Visible = false;
            pnSelfServiceP.Visible = true;
            pnSelfServiceB.Visible = false;
            lbMenuTitle.Text = "PartsCOMMAND<span style='font-size: 11px; vertical-align: top; position: relative; top: -4px;'>®</span>";
            Page.Title = "ServiceCOMMAND Self Service Parts Utilities";
        }
        else if (sCustType == "SSB")
        {
            pnRegLrgDlr.Visible = false;
            pnDlrRenewals.Visible = false;
            pnSelfServiceP.Visible = false;
            pnSelfServiceB.Visible = true;
            lbMenuTitle.Text = "Self Service Menu";
            Page.Title = "ServiceCOMMAND Self Service Utilities";
        }
        else if (sShowRenewals == "YES")
        {
            pnRegLrgDlr.Visible = false;
            pnDlrRenewals.Visible = true;
            pnSelfServiceP.Visible = false;
            pnSelfServiceB.Visible = false;
            lbMenuTitle.Text = "ServiceCOMMAND<span style='font-size: 11px; vertical-align: top; position: relative; top: -4px;'>®</span>";
            Page.Title = "ServiceCOMMAND";
        }
        else
        {
            pnRegLrgDlr.Visible = true;
            pnSelfServiceP.Visible = false;
            pnSelfServiceB.Visible = false;
            pnDlrRenewals.Visible = false;
            lbMenuTitle.Text = "ServiceCOMMAND<span style='font-size: 11px; vertical-align: top; position: relative; top: -4px;'>®</span>";
            Page.Title = "ServiceCOMMAND";
        }
    }
    // =========================================================
    protected void setCustTitle(int cs1)
    {
        if (sPageLib == "L")
        {
            lbCs1Name.Text = wsLive.GetCustName(sfd.GetWsKey(), cs1, 0);
        }
        else
        {
            lbCs1Name.Text = wsTest.GetCustName(sfd.GetWsKey(), cs1, 0);
        }
    }
    // ========================================================================
    protected void tmProgressBar_Tick(object sender, EventArgs e)
    {
        /*
        if (1 == 2) 
        { 
        string sStyle = "html";

        if (int.TryParse(hfBarWidth.Value, out iPercentComplete) == false)
            iPercentComplete = 0;
        else
        {
            if (iPercentComplete >= 100)
                iPercentComplete = 0;
            else
                iPercentComplete += iIncrement;
        }
        hfBarWidth.Value = iPercentComplete.ToString();

        ltProgressBar.Text = pb.GetBar(iPercentComplete, iWidth, sStyle);
        lbProgressBar.Text = iPercentComplete.ToString() + "% Complete";
        if (iPercentComplete >= 100)
            tmProgressBar.Enabled = false;
        }
         * */
    }
    // =========================================================
    protected void showProgressBar(object sender, EventArgs e)
    {
        /*
        tmProgressBar.Enabled = true;
        pnProgressBar.Visible = true;
        Response.Redirect("~/private/sc/ServiceRequest.aspx", false);
         * */
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected string Select_UserMoveStage(int loginId)
    {
        string sMoveStage = "";
        string sLibrary = "";

        string sSql = "";
        if (sPageLib == "L")
            sLibrary = "OMDTALIB";
        else
            sLibrary = "OMTDTALIB";

        //string sDebug = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                 " CRID" +
                ", CRCSTNAM" +
                ", CRCSTGRP" +
                ", CRINFORM" +
                ", CRMOVED" +
                " from " + sLibrary + ".CST21REGI" +
                " where CRID = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Id", loginId);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                if (odbcReader.HasRows) { }
                dt.Load(odbcReader);
            }
            string sCustName = "";
            string sCustGroup = "";
            string sDat = "";
            string sDatFormatted = "";
            int iDateToInform = 0;
            int iDateToMove = 0;
            int iDateToday = 0;
            if (int.TryParse(DateTime.Now.ToString("yyyyMMdd"), out iDateToday) == false)
                iDateToday = -1;

            DateTime datTemp;
            DateTime datToMove = DateTime.Now.AddYears(1);

            if (iDateToday > 0) 
            {
                if (dt.Rows.Count > 0)
                {
                    sCustName = dt.Rows[0]["CrCstNam"].ToString();
                    sCustGroup = dt.Rows[0]["CrCstGrp"].ToString();

                    if (int.TryParse(dt.Rows[0]["CrMoved"].ToString(), out iDateToMove) == false)
                        iDateToMove = -1;

                    if (iDateToMove > 0) 
                    {
                        sDat = iDateToMove.ToString();
                        if (sDat.Length == 8) 
                        {
                            if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == true) 
                            {
                                datToMove = datTemp;
                                sDatFormatted = datTemp.ToString("dddd MMMM d, yyyy");
                            }
                        }
                    }


                    if (iDateToMove > 0 && iDateToday >= iDateToMove) 
                    {
                        sMoveStage = "MOVE";
                    }
                    else 
                    {
                        if (int.TryParse(dt.Rows[0]["CrInform"].ToString(), out iDateToInform) == false)
                            iDateToInform = -1;
                        if (iDateToInform > 0 && iDateToday >= iDateToInform)
                        {
                            sMoveStage = "INFORM";
                            lbMoveCutoff.Text = sDatFormatted;
                            lbMoveCutoff.Font.Bold = true;
                            lbLoginCompanyId.Text = loginId.ToString();
                            // ------------------------------------------
                            TimeSpan ts = new TimeSpan();
                            ts = datToMove - DateTime.Now;
                            if (ts.TotalDays < 21) 
                            {
                                //lbReadyNow.Font.Bold = true;
                                lbReadyNow.Font.Size = 22;
                                lbReadyNow.ForeColor = System.Drawing.Color.Black;
                                lbReadyNow.CssClass = "siteEnding";

                                lbRetiringSoon.Font.Bold = true;
                                lbRetiringSoon.Font.Size = 22;
                                lbRetiringSoon.ForeColor =  System.Drawing.Color.Crimson;
                                lbRetiringSoon.CssClass = "siteEnding";

                                lbMoveCutoff.Font.Size = 22;
                                lbMoveCutoff.ForeColor = System.Drawing.Color.Blue;
                                lbMoveCutoff.CssClass = "siteEnding";

                                pnGoNow.BackColor = System.Drawing.Color.Cornsilk;
                                pnGoNow.BorderColor = System.Drawing.Color.Gray;
                                pnGoNow.BorderWidth = 2;
                                pnGoNow.CssClass = "siteEndingBox";
                            }


                            // ------------------------------------------
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (sPageLib == "L")
            {
                wsLive.SaveErrorText(ex.Message.ToString(), ex.ToString(), "", "", "", "CUST LIVE");
            }
            else
            {
                wsTest.SaveErrorText(ex.Message.ToString(), ex.ToString(), "", "", "", "CUST TEST");
            }
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return sMoveStage;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {

    }
    // -------------------------------------------------------------------------
    protected void btHideNewSiteNotice_Click(object sender, EventArgs e)
    {
        pnNewSiteNotice.Visible = false;
        btShowNewSiteNotice.Visible = true;
        Session["NewSiteNoticeVisibility"] = "HIDE";
    }
        // -------------------------------------------------------------------------
        protected void btShowNewSiteNotice_Click(object sender, EventArgs e)
    {
        pnNewSiteNotice.Visible = true;
        btShowNewSiteNotice.Visible = false;
        Session["NewSiteNoticeVisibility"] = "SHOW";
    }

    // =========================================================
    // =========================================================
    // ========================================================================
}