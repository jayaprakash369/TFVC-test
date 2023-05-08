using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Security;

public partial class private_sc_req_Problem : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    SourceForCustomer sfc = new SourceForCustomer();

    string[] saCs1All = new string[1];
    string[] saCs1Nam = new string[1];

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
//        string sJScript = "";
        if (!IsPostBack)
        {
            //string sUserURL = Request.Url.ToString();
            //string sJsURL = sUserURL.Replace("/sc/req/Problem.aspx", "/js/ProgressBar.js");
            // Load javascript onto page (needed because localhost adds another folder otherwise it won't run on server)
            // Page.ClientScript.RegisterClientScriptInclude("jsEncode", "../../../public/js/encode.js");
            //Page.ClientScript.RegisterClientScriptInclude("jsGetXY", "../../../public/js/GetXY.js");
            //Page.ClientScript.RegisterClientScriptInclude("jsProgressBar", "../../../private/js/ReqProgress.js");
     //Page.ClientScript.RegisterClientScriptInclude("jsGetXY", "/public/js/scantron/GetXY.js");
     //Page.ClientScript.RegisterClientScriptInclude("jsProgressBar", "/private/js/ReqProgress.js");

            Response.Cookies["clientPage"].Value = "Problem";

            if (Session["reqSource"] != null &&
                (Session["reqSource"].ToString() == "Equipment" ||
                 Session["reqSource"].ToString() == "Serial" ||
                  Session["reqSource"].ToString() == "Xref" ||
                 Session["reqSource"].ToString() == "Contact"))
            {
                //sScript = @"alert('First Pass: Valid After Session check' + document.forms[0].ct100_BodyContent_hfCs1.value);";
                //sJScript = @"alert('ctl00_BodyContent_hfCs1');";
                GetParms();

                int iCs1 = 0;
                int iCs2 = 0;
                int.TryParse(hfCs1.Value, out iCs1);
                int.TryParse(hfCs2.Value, out iCs2);

                if (hfReqSource.Value == "Serial" || hfReqSource.Value == "Xref")
                    pnCs1Header.Controls.Add(sfc.GetCustDataTable(iCs1, iCs2, "", "", ""));
                else
                    pnCs1Header.Controls.Add(sfc.GetCustDataTable(iCs1, iCs2, hfContact.Value, hfPhone.Value, hfExtension.Value));

                if (hfReqSource.Value == "Contact")
                {
                    btProblem.Text = "Submit Manual Request";
                    btProblemForced.Visible = true;
                    rpProblemForced.Visible = true;
                    LoadPanelProblemForced();
                }
                else
                {
                    btProblem.Visible = true;
                    rpProblem.Visible = true;
                    LoadPanelProblem();
                }
            }
            else // not from proper source page (backup or bookmark) (User had perfect entry on 1st pass)
            {
                // sJScript = @"alert('First Pass: Bogus');";
                if (Session["reqSource"] != null)
                    Session["reqSource"] = null;
                //Response.Redirect("~/private/sc/req/Location.aspx?bounce=FirstPassBogus", false);
                //Response.Redirect("~/private/sc/req/Location.aspx?a=1", false); // Back button triggered this... and succeeded
                Response.Redirect("~/private/sc/req/Location.aspx", false); 
            }
        }
        else // PostBack (try to handle backing in... use for user with validation error/multiple passes)
        {
            if (Session["reqSource"] == null || Session["reqSource"].ToString() != "Problem")
            {
                //sJScript = @"alert('PostBack: Bogus... Trying to bounce...');";
                //Response.Redirect("~/private/sc/req/Location.aspx?bounce=PostBack", false);
                //Response.Redirect("~/private/sc/req/Location.aspx?b=2", false);
                Response.Redirect("~/private/sc/req/Location.aspx", false);
            }
            else 
            {
                //sJScript = @"alert('PostBack: Valid');";
            }
        }
        //sScript = @"alert('Testing..');";
        //sScript = @"alert('Hi..' + document.forms[0]['ctl00_BodyContent_hfCs1'].value);";
        //sScript = @"alert('Hi..' + document.forms[0]['ctl00_BodyContent_hfReqSource'].value);";
        //Response.Cookies["clientPage"].Value

        //sJScript = @"alert('Cookie... ' + ReadCookie('clientPage'));";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript1", sJScript, true);
    }
    // =========================================================
    protected void LoadPanelProblem()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        DataTable dTable = new DataTable(sMethodName);

        int iCs1 = 0;
        int iCs2 = 0;
        int.TryParse(hfCs1.Value, out iCs1);
        int.TryParse(hfCs2.Value, out iCs2);

        if (hfReqSource.Value == "Serial" || hfReqSource.Value == "Xref")
        {
            pnContactSer.Visible = true;
            txContactSer.Focus();
        }

        string[] saUnt = new string[1];
        string[] saUntAgr = new string[2];

        int iRowIdx = 0;
        int i = 0;
        int iUnt = 0;
        int iUntFile = 0;

        string sPrt = "";
        string sSer = "";
        string sPrdCod = "";
        string sPrtDsc = "";
        string sAgr = "";
        string sAgrCod = "";
        string sAgrDsc = "";
        string sPM = "";

        saUnt = hfUnitList.Value.Split('|');

        if (Session["reqListPm"] != null && Session["reqListPm"].ToString() == "PM")
            sPM = "PM";

        // Load Repeater
        for (i = 0; i < saUnt.Length; i++)
        {
            saUntAgr = saUnt[i].Split('~');
            if (saUntAgr.Length > 0) 
            {
                if (int.TryParse(saUntAgr[0], out iUnt) == false)
                    iUnt = 0;
            }
            if (saUntAgr.Length > 1)
            {
                sAgr = saUntAgr[1];
            }
            if (iUnt > 0)
            {
                if (sPageLib == "L")
                {
                    dataTable = wsLive.GetModelBasics(sfd.GetWsKey(), iUnt, sAgr);
                }
                else
                {
                    dataTable = wsTest.GetModelBasics(sfd.GetWsKey(), iUnt, sAgr);
                }
                if (dataTable.Rows.Count > 0)
                {
                    if (iRowIdx == 0)
                    {
                        dTable = dataTable;
                    }
                    else
                    {
                        if (int.TryParse(dataTable.Rows[0]["Unit"].ToString().Trim(), out iUntFile) == false)
                            iUntFile = 0;
                        if (int.TryParse(dataTable.Rows[0]["Customer"].ToString().Trim(), out iCs1) == false)
                            iCs1 = 0;
                        if (int.TryParse(dataTable.Rows[0]["Location"].ToString().Trim(), out iCs2) == false)
                            iCs2 = 0;
                        sAgr = dataTable.Rows[0]["Agreement"].ToString().Trim();
                        sPrt = dataTable.Rows[0]["Part"].ToString().ToUpper().Trim();
                        sSer = dataTable.Rows[0]["Serial"].ToString().ToUpper().Trim();
                        sPrdCod = dataTable.Rows[0]["ProdCode"].ToString().Trim();
                        sPrtDsc = dataTable.Rows[0]["PartDesc"].ToString().Trim();
                        sAgrCod = dataTable.Rows[0]["AgrCode"].ToString().Trim();
                        sAgrDsc = dataTable.Rows[0]["AgrDesc"].ToString().Trim();

                        dTable.Rows.Add(iUntFile, sAgr, sPrt, sSer, sAgrCod, iCs1, iCs2, sPrdCod, sPrtDsc, sAgrDsc);
                    }
                    iRowIdx++;
                }
            }
        }

        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = "Problem";
        dTable.Columns.Add(dc);
        if (sPM == "PM") 
        {
            for (i = 0; i < dTable.Rows.Count; i++) 
            {
                dTable.Rows[i]["Problem"] = "PM REQUESTED";
            }
            //txComment.Text = "PM REQUESTED";
            txComment.Text = "";
        }

        dTable.AcceptChanges();
        hfReqCount.Value = dTable.Rows.Count.ToString();
        rpProblem.DataSource = dTable;
        rpProblem.DataBind();
        CustomizeUnits_rp();
    }
    // =========================================================
    protected void LoadPanelProblemForced()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iCs1 = 0;
        int iCs2 = 0;
        int.TryParse(hfCs1.Value, out iCs1);
        int.TryParse(hfCs2.Value, out iCs2);

        // Load Repeater Forced
        DataColumn dc;

        dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = "Count";
        dataTable.Columns.Add(dc);
        dataTable.AcceptChanges();

        int iForcedQty = 0;
        if (int.TryParse(hfForcedQty.Value, out iForcedQty) == false)
            iForcedQty = 0;

        for (int i = 1; i <= iForcedQty; i++)
            dataTable.Rows.Add(i.ToString());

        dataTable.AcceptChanges();
        hfReqCount.Value = iForcedQty.ToString();
        rpProblemForced.DataSource = dataTable;
        rpProblemForced.DataBind();
    }
    // =========================================================
    // START CLICK METHODS
    // =========================================================
    protected void btProblem_Click(object sender, EventArgs e)
    {
        try
        {
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dataTable = new DataTable(sMethodName);
                
            string sValid = ServerSideVal_Problem();

            if (sValid == "VALID")
            {
                SaveRequests();
                //System.Threading.Thread.Sleep(10000);
                Session["reqReqKey"] = hfReqKey.Value.Trim();
                Session["reqCs1"] = hfCs1.Value.Trim();
                Session["reqCs2"] = hfCs2.Value.Trim();
                Session["reqPhone"] = hfPhone.Value.Trim();
                Session["reqExtension"] = hfExtension.Value.Trim();
                Session["reqContact"] = hfContact.Value.Trim();
                Session["reqMthdT"] = hfCommMethodType.Value;
                Session["reqMthdI"] = hfCommMethodInfo.Value.Trim();
                Session["reqMethodPhoneExt"] = hfCommMethodPhoneExt.Value.Trim();

                Response.Redirect("~/private/sc/req/Result.aspx", false); // false to cancel page execution
                //Server.Transfer("~/private/sc/req/Result.aspx", false);
                //Server.Execute("~/private/sc/req/Result.aspx", false);
                // Instead of Response.End() user HttpContext.Current.ApplicationInstance.CompleteRequest 
            }
        }
        //catch (System.Threading.ThreadAbortException taex)
        //{
        //    // Disregard this error (Ajax wants to continue, you redirected)
        //}
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "Req Submission Error...");
        }
        finally
        {
        }

    }
    // =========================================================
    // END CLICK METHODS
    // =========================================================
    // =========================================================
    // START TABLE WALKTHROUGH
    // =========================================================
    protected void GetSelectedUnits_rp(int key)
    {
        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        HiddenField hfTemp = new HiddenField();
        DropDownList ddTemp = new DropDownList();

        int iSeq = 0;
        int iUnt = 0;
        int iVia = 0;

        string sAgr = "";
        string sPrt = "";
        string sSer = "";
        string sPrb = "";
        string sXrf = "";
        string sCod = "";
        string sDsc = "";
        string sPM = "";

        string sPrinterInterface = "";

        string sControlType = "";
        string sResult = "";
        string sAutoOrForced = "AUTO";

        if (Session["reqListPm"] != null && Session["reqListPm"].ToString() == "PM") 
            sPM = "PM";

        // This reqParmList is to hold the key request values to identify the ticket in case the incrementing key fails to have a ctr-tck in "Result.aspx"
        //Session["reqParmList"] = "";

        foreach (Control c1 in rpProblem.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPart")
                            sPrt = lbTemp.Text.Trim().ToUpper();
                        if (lbTemp.ID == "lbSerial")
                            sSer = lbTemp.Text.Trim().ToUpper();
                        if (lbTemp.ID == "lbAgrDesc")
                            sDsc = lbTemp.Text.Trim().ToUpper();
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;
                        if (txTemp.ID == "txProblem")
                            sPrb = txTemp.Text.Trim().ToUpper();
                        if (txTemp.ID == "txCrossRef")
                            sXrf = txTemp.Text.Trim().ToUpper();
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        ddTemp = (DropDownList)c2;
                        if (ddTemp.ID == "ddInterface")
                        {
                            sPrinterInterface = ddTemp.SelectedValue;
                        }
                        if (ddTemp.ID == "ddVia")
                        {
                            if (int.TryParse(ddTemp.SelectedValue.ToString(), out iVia) == false)
                                iVia = 0;
                        }
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfAgreement")
                            sAgr = hfTemp.Value.Trim();
                        if (hfTemp.ID == "hfAgrCode")
                            sCod = hfTemp.Value.Trim();
                        if (hfTemp.ID == "hfUnit")
                        {
                            if (int.TryParse(hfTemp.Value, out iUnt) == false)
                                iUnt = 0;
                            if (iUnt > 0)
                            {
                                // Now Create Service Request
                                iSeq++;

                                if (sPageLib == "L")
                                {
                                    sResult = wsLive.AddRequestDetail(sfd.GetWsKey(), key, iSeq, sPrt, sSer, iUnt, sPrb, sXrf, sAgr, sCod, sDsc, sPrinterInterface, iVia, sPM, 0, sAutoOrForced, hfCreator.Value, "", "");
                                }
                                else
                                {
                                    sResult = wsTest.AddRequestDetail(sfd.GetWsKey(), key, iSeq, sPrt, sSer, iUnt, sPrb, sXrf, sAgr, sCod, sDsc, sPrinterInterface, iVia, sPM, 0, sAutoOrForced, hfCreator.Value, "", "");
                                }

                                // Loading keys for each request in case the increment key fails to find the ticket number
                                //if (Session["reqParmList"].ToString().Trim() != "")
                                //    Session["reqParmList"] += "|";
                                //Session["reqParmList"] += sPrt + "`" + sSer + "`" + iUnt + "`" + sPrb;

                                // Clear workfields
                                iUnt = 0;
                                sAgr = "";
                                sPrt = "";
                                sSer = "";
                                sPrb = "";
                                sXrf = "";
                                sCod = "";
                                sDsc = "";
                            }
                        }
                    }
                }
            }
        }
    }
    // =========================================================
    protected void CustomizeUnits_rp()
    {
        Label lbTemp = new Label();
        DropDownList ddTemp = new DropDownList();
        HiddenField hfTemp = new HiddenField();

        int iPriority = 0;
        int iUnit = 0;

        string sPrt = "";
        string sPrtHold = "Your selected part";
        string sDsc = "";
        string sProductCode = "";
        string sInterfaceNeeded = "";
        string sInterfaceFound = "";
        string sViaNeeded = "";
        string sViaFound = "";
        string sControlType = "";
        string sXeroxSubcontract = "";

        lbPriorityInfo.Text = "";

        foreach (Control c1 in rpProblem.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPart")
                        {
                            sPrt = lbTemp.Text.Trim();
                            sPrtHold = sPrt;
                            // Check if Unit is an express printer 
                            if (sPageLib == "L")
                            {
                                iPriority = wsLive.GetPartPriority(sfd.GetWsKey(), sPrt);
                            }
                            else
                            {
                                iPriority = wsTest.GetPartPriority(sfd.GetWsKey(), sPrt);
                            }
                            if (iPriority == 1)
                            {
                                lbPriorityInfo.Text += "<b>" + sPrt + " is generally considered a CRITICAL piece of equipment...</b><br />";
                            }
                        }

                        if (lbTemp.ID == "lbAgrDesc") // was laAgrDesc
                        {
                            sDsc = lbTemp.Text.Trim().ToUpper();
                            if ((sDsc == "EXPRESS") || (sDsc == "DEPOT"))
                                sViaNeeded = "Y";

                            // Check if Unit is an express printer 
                            if (sPageLib == "L")
                            {
                                sProductCode = wsLive.GetPartProductCode(sfd.GetWsKey(), sPrt);
                            }
                            else
                            {
                                sProductCode = wsTest.GetPartProductCode(sfd.GetWsKey(), sPrt);
                            }
                            if ((sProductCode == "PTR") && ((sDsc == "EXPRESS") || (sDsc == "PER-INCIDENT")))
                                sInterfaceNeeded = "Y";
                        }
                    }
                    //-----------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        ddTemp = (DropDownList)c2;
                        if (ddTemp.ID == "ddInterface")
                        {
                            if (sInterfaceNeeded == "Y")
                            {
                                ddTemp.Visible = true;
                                sInterfaceFound = "Y";
                            }
                        }
                        if (ddTemp.ID == "ddVia")
                        {
                            if (sViaNeeded == "Y")
                            {
                                ddTemp.Visible = true;
                                sViaFound = "Y";
                            }
                            // Clear workfields
                            sPrt = "";
                            sDsc = "";
                            sInterfaceNeeded = "";
                            sViaNeeded = "";
                        }
                    }
                    //-----------------------------------------------------------------------------
                    // /*
                    if (hfCs1.Value == "102360") // UNFI only
                    {
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfUnit")
                        {
                            iUnit = 0;
                            if (int.TryParse(hfTemp.Value, out iUnit) == false)
                                iUnit = 0;
                            if (iUnit > 0) 
                            {
                                // Check if machine is a Xerox which will be subcontracted
                                sXeroxSubcontract = "";
                                if (sPageLib == "L")
                                {
                                    sXeroxSubcontract = wsLive.CheckXeroxSubcontract(sfd.GetWsKey(), iUnit);
                                }
                                else
                                    sXeroxSubcontract = wsTest.CheckXeroxSubcontract(sfd.GetWsKey(), iUnit);
                                if (sXeroxSubcontract == "Y")
                                {
                                    lbPriorityInfo.Text = "XEROX";
                                }
                            }
                        }
                    }
                    }
                    // */
                    // -----------------------------
                }
            }
        }
        // ----                "1) If you spread two clips... It's <font color='#AD0034'>PARALLEL</FONT> " +
      // --  "<BR /> 2) If you remove two screws... It's <font color='#AD0034'>SERIAL</FONT>" + 
        
        if (lbPriorityInfo.Text == "XEROX") 
        {
            lbPriorityInfo.Text = "<b>URGENT:</b>  " + sPrtHold + " will require triage prior to technician dispatch.  " + 
                "<br />To expedite the dispatch process, please call STS at 1-800-892-8332 ext. 3276 " +
                "<br />with enduser on the phone" + 
                "<br />before clicking the \"Submit Automated Request\" button " + 
                "<br />to initiate a triage conversation with the contact listed on this service request." +
                "<br /><br />(Hours of operation Monday to Friday- 7 am to 7 pm CST)" +
                "<br /><br />";
            lbPriorityInfo.Visible = true;
        }
        else if (lbPriorityInfo.Text != "")
        {
            lbPriorityInfo.Text += "(You may proceed, but a live representative at 1-800-228-3628" +
                " can access all available support services.)" +
                "<br /><br />";
            lbPriorityInfo.Visible = true;
        }
        if (sInterfaceFound == "Y")
        {
            lbInterfaceInfo.Text = "To help determine your printer interface type... " +
                "<B>HOW DO YOU REMOVE YOUR CABLE?</B> " +
                "<table><tr><td style='text-align: left;'>" +
                "<BR /> 1) If it looks like a large phone jack... It's <font color='#AD0034'>NETWORK</FONT>" +
                "<BR /> 2) If it has a square connector... It's <font color='#AD0034'>USB</FONT>" +
                "</td></tr></table>";
            lbInterfaceInfo.Visible = true;
        }

        if ((sInterfaceFound == "Y") || (sViaFound == "Y"))
        {
            foreach (Control c1 in rpProblem.Controls)
            {
                sControlType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sControlType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                        {
                            lbTemp = (Label)c2;
                            if (lbTemp.ID == "lbInterfaceHeader")
                                lbTemp.Visible = true;
                            if (lbTemp.ID == "lbViaHeader")
                                lbTemp.Visible = true;
                        }
                        // -----------------------------
                    }
                }
            }
        }
        // --------------------------------------
    }
    // =========================================================
    protected void GetSelectedUnits_rpForced(int key)
    {
        TextBox txTemp = new TextBox();
        DropDownList ddTemp = new DropDownList();
        HiddenField hfTemp = new HiddenField();

        int iSeq = 0;
        int iUnt = 0;
        int iVia = 0;

        string sAgr = "";
        string sPrt = "";
        string sSer = "";
        string sPrb = "";
        string sXrf = "";
        string sCod = "";
        string sDsc = "";
        string sPM = "";

        string sPrinterInterface = "";
        string sControlType = "";
        string sAutoOrForced = "FORCED";
        string sResult = "";

        foreach (Control c1 in rpProblemForced.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;
                        if (txTemp.ID == "txPartForced")
                            sPrt = txTemp.Text.Trim().ToUpper();
                        if (txTemp.ID == "txSerialForced")
                            sSer = txTemp.Text.Trim().ToUpper();
                        if (txTemp.ID == "txProblemForced")
                            sPrb = txTemp.Text.Trim().ToUpper();
                        if (txTemp.ID == "txCrossRefForced")
                            sXrf = txTemp.Text.Trim().ToUpper();
                    }
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        ddTemp = (DropDownList)c2;
                        if (ddTemp.ID == "ddServiceTypeForced")
                        {
                            sDsc = ddTemp.SelectedValue;

                            // Minimum Entries
                            if ((sPrt != "") && (sSer != "") && (sPrb != ""))
                            {
                                // Now Create Forced Service Request
                                iSeq++;

                                if (sPageLib == "L")
                                {
                                    sResult = wsLive.AddRequestDetail(sfd.GetWsKey(), key, iSeq, sPrt, sSer, iUnt, sPrb, sXrf, sAgr, sCod, sDsc, sPrinterInterface, iVia, sPM, 0, sAutoOrForced, hfCreator.Value, "", "");
                                }
                                else
                                {
                                    sResult = wsTest.AddRequestDetail(sfd.GetWsKey(), key, iSeq, sPrt, sSer, iUnt, sPrb, sXrf, sAgr, sCod, sDsc, sPrinterInterface, iVia, sPM, 0, sAutoOrForced, hfCreator.Value, "", "");
                                }

                                // Clear workfields
                                iUnt = 0;
                                sAgr = "";
                                sPrt = "";
                                sSer = "";
                                sPrb = "";
                                sXrf = "";
                                sCod = "";
                                sDsc = "";
                            }
                        }
                    }
                }
            }
        }
    }
    // =========================================================
    // END TABLE WALKTHROUGH
    // =========================================================
    protected string ServerSideVal_Problem()
    {
        string sValid = "";

        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        DropDownList ddTemp = new DropDownList();
        CustomValidator cvTemp = new CustomValidator();

        string sPrt = "";
        string sDsc = "";
        string sProductCode = "";
        string sProblemMissing = "";
        string sCrossRefMissing = "";
        string sInterfaceNeeded = "";
        string sInterfaceMissing = "";
        string sViaNeeded = "";
        string sViaMissing = "";
        string sControlType = "";
        
        lbPriorityInfo.Text = "";

        foreach (Control c1 in rpProblem.Controls)
        {
            sControlType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sControlType = c2.GetType().ToString();
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID == "lbPart")
                        {
                            sPrt = lbTemp.Text.Trim();
                        }
                        if (lbTemp.ID == "lbAgrDesc")
                        {
                            sDsc = lbTemp.Text.Trim().ToUpper();
                            if ((sDsc == "EXPRESS") || (sDsc == "DEPOT"))
                                sViaNeeded = "Y";

                            // Check if Unit is an express printer 
                            if (sPageLib == "L")
                            {
                                sProductCode = wsLive.GetPartProductCode(sfd.GetWsKey(), sPrt);
                            }
                            else
                            {
                                sProductCode = wsTest.GetPartProductCode(sfd.GetWsKey(), sPrt);
                            }
                            if ((sProductCode == "PTR") && ((sDsc == "EXPRESS") || (sDsc == "PER-INCIDENT")))
                                sInterfaceNeeded = "Y";
                        }
                    }
                    // Textboxes
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        txTemp = (TextBox)c2;
                        if (txTemp.ID == "txProblem")
                        {
                            if (txTemp.Text == "") 
                            {
                                sProblemMissing = "Y";
                                if (sValid == "") 
                                {
                                    sValid = "INVALID";
                                    txTemp.Focus();
                                }
                            }
                        }
                        if (txTemp.ID == "txCrossRef")
                        {
                            if (txTemp.Text == ""
                                && hfCs1.Value == "79206" // Cinemark
                                //&& hfCs1.Value == "99999" // Test
                                )
                            {
                                sCrossRefMissing = "Y";
                                if (sValid == "")
                                {
                                    sValid = "INVALID";
                                    txTemp.Focus();
                                }
                            }
                        }
                        // ----------------------------------
                    }

                    // Now that you know whether interface of via is needed...
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        ddTemp = (DropDownList)c2;
                        if (ddTemp.ID == "ddInterface")
                        {
                            if (sInterfaceNeeded == "Y")
                            {
                                if (ddTemp.SelectedValue == "") 
                                { 
                                    sInterfaceMissing = "Y";
                                    if (sValid == "")
                                    {
                                        sValid = "INVALID";
                                        ddTemp.Focus();
                                    }
                                }
                            }
                        }
                        if (ddTemp.ID == "ddVia")
                        {
                            if (sViaNeeded == "Y")
                            {
                                if (ddTemp.SelectedValue == "")
                                {
                                    sViaMissing = "Y";
                                    if (sValid == "")
                                    {
                                        sValid = "INVALID";
                                        ddTemp.Focus();
                                    }
                                }
                            }
                        }
                    }
                    // If Face or Via missing, load validators
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.CustomValidator"))
                    {
                        cvTemp = (CustomValidator)c2;
                        if (cvTemp.ID == "vCustom_Problem")
                        {
                            if (sProblemMissing == "Y")
                            {
                                cvTemp.ErrorMessage = "A problem description is required";
                                cvTemp.IsValid = false;
                            }
                        }
                        if (cvTemp.ID == "vCustom_CrossRef")
                        {
                            if (sCrossRefMissing == "Y")
                            {
                                cvTemp.ErrorMessage = "A ticket cross reference is required";
                                cvTemp.IsValid = false;
                            }
                        }
                        if (cvTemp.ID == "vCustom_Face")
                        {
                            if ((sInterfaceNeeded == "Y") && (sInterfaceMissing == "Y"))
                            {
                                cvTemp.ErrorMessage = "The printer interface type is required";
                                cvTemp.IsValid = false;
                            }
                        }
                        if (cvTemp.ID == "vCustom_Via")
                        {
                            if ((sViaNeeded == "Y") && (sViaMissing == "Y"))
                            {
                                cvTemp.ErrorMessage = "A shipping method is required";
                                cvTemp.IsValid = false;
                            }
                            // Clear workfields now that you're on the last important field
                            sPrt = "";
                            sDsc = "";
                            sProblemMissing = "";
                            sInterfaceNeeded = "";
                            sInterfaceMissing = "";
                            sViaNeeded = "";
                            sViaMissing = "";
                        }
                    }
                    // -----------------------------
                }
            }
        }
        // --------------------------------------
        if (sValid == "") 
        {
            sValid = "VALID";
        }
        return sValid;
    }
    // =========================================================
    protected void SaveRequests()
    {
        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();
        TimeSpan ts = new TimeSpan();
        startTime = DateTime.Now;

        string sResult = "";
        string sUserId = "";

        int iNewKey = 0;
        int iReqCount = 0;
        int iPhone1 = 0;
        int iPhone2 = 0;
        int iPhone3 = 0;
        int iDealerNum = 0;
        string sPrimaryType = "";

        int iPri = 0;
        int iCs1 = 0;
        int iCs2 = 0;
        int.TryParse(hfPri.Value, out iPri);
        int.TryParse(hfCs1.Value, out iCs1);
        int.TryParse(hfCs2.Value, out iCs2);

        try
        {
            if (User.Identity.IsAuthenticated)
            {
                MembershipUser mu = Membership.GetUser();
                sUserId = mu.UserName;

                if (sPageLib == "L")
                {
                    sPrimaryType = wsLive.GetCustType(sfd.GetWsKey(), iCs1);
                }
                else
                {
                    sPrimaryType = wsTest.GetCustType(sfd.GetWsKey(), iCs1);
                }
                if (sPrimaryType == "DLR")
                    iDealerNum = iPri;
            }

            if (txContactSer.Text != "")
            {
                // Load new contact info into hidden fields
                hfPhone.Value = txPhone1Ser.Text + "" + txPhone2Ser.Text + "" + txPhone3Ser.Text;
                hfContact.Value = txContactSer.Text.Trim();
                hfExtension.Value = txExtensionSer.Text.Trim();
                hfEmail.Value = txEmailSer.Text.Trim();
                hfCreator.Value = txCreatorSer.Text.Trim();
            }

            string sComment = txComment.Text.Trim().ToUpper();
            string sPaymentMethod = hfReqType.Value;
            string sProgram = "private/sc/req/problem.aspx";

            if (int.TryParse(hfReqCount.Value, out iReqCount) == false)
                iReqCount = 0;

            if (iReqCount > 0)
            {
                if (hfPhone.Value != "")
                {
                    if (hfPhone.Value.Length != 10)
                        hfPhone.Value = "9999999999";

                    if (int.TryParse(hfPhone.Value.Substring(0, 3), out iPhone1) == false)
                        iPhone1 = 0;
                    if (int.TryParse(hfPhone.Value.Substring(3, 3), out iPhone2) == false)
                        iPhone2 = 0;
                    if (int.TryParse(hfPhone.Value.Substring(6, 4), out iPhone3) == false)
                        iPhone3 = 0;
                }
                string sCommunicationMethodData = "";
                if (hfCommMethodType.Value == "PHN" && hfCommMethodPhoneExt.Value != "")
                    sCommunicationMethodData = hfCommMethodInfo.Value.Trim() + "|" + hfCommMethodPhoneExt.Value.Trim();
                else if(hfCommMethodType.Value == "NON")
                    sCommunicationMethodData = "";
                else 
                    sCommunicationMethodData = hfCommMethodInfo.Value.Trim();

                if (sPageLib == "L")
                {
                    iNewKey = wsLive.SetNextRequestKey(sfd.GetWsKey());
                    sResult = wsLive.AddRequestHeaderMthd(sfd.GetWsKey(), iNewKey, iReqCount, iCs1, iCs2, hfContact.Value, iPhone1, iPhone2, iPhone3, hfExtension.Value, hfEmail.Value, hfReqType.Value, sProgram, sUserId, iDealerNum, sComment, hfCommMethodType.Value, sCommunicationMethodData);
                  //sResult = wsLive.AddRequestHeader(sfd.GetWsKey(), iNewKey, iReqCount, iCs1, iCs2, hfContact.Value, iPhone1, iPhone2, iPhone3, hfExtension.Value, hfEmail.Value, hfReqType.Value, sProgram, sUserId, iDealerNum, sComment);
                }
                else
                {
                    iNewKey = wsTest.SetNextRequestKey(sfd.GetWsKey());
                    sResult = wsTest.AddRequestHeaderMthd(sfd.GetWsKey(), iNewKey, iReqCount, iCs1, iCs2, hfContact.Value, iPhone1, iPhone2, iPhone3, hfExtension.Value, hfEmail.Value, hfReqType.Value, sProgram, sUserId, iDealerNum, sComment, hfCommMethodType.Value, sCommunicationMethodData);
                }

                // Save Request Key for cross page use. 
                hfReqKey.Value = iNewKey.ToString();

                // Get Units (to add them as detail recs) 
                if (hfReqType.Value == "") // Selected From List (Not Forced)
                {
                    GetSelectedUnits_rp(iNewKey);
                    // This calls SRQ3 to create the ticket
                    RunRequestThroughTriggerFile(iNewKey, iReqCount);
                }
                else
                {
                    GetSelectedUnits_rpForced(iNewKey);
                    // Send email to call center
                    if (sPageLib == "L")
                    {
                        sResult = wsLive.EmailServiceRequest(sfd.GetWsKey(), iNewKey);
                    }
                    else
                    {
                        sResult = wsTest.EmailServiceRequest(sfd.GetWsKey(), iNewKey);
                    }
                }
            }
            // Save Request Duration (in Seconds)
            // System.Threading.Thread.Sleep(30000);
            endTime = DateTime.Now;
            ts = endTime - startTime;
            string sDuration = "";
            double dDuration = 0.0;
            int iMinutes = 0;
            int iSeconds = 0;
            int iMilliseconds = 0;
            int.TryParse(ts.Minutes.ToString(), out iMinutes);
            int.TryParse(ts.Seconds.ToString(), out iSeconds);
            int.TryParse(ts.Milliseconds.ToString(), out iMilliseconds);
            iSeconds = (60 * iMinutes) + iSeconds;
            sDuration = iSeconds.ToString() + "." + iMilliseconds.ToString();
            double.TryParse(sDuration, out dDuration);
            int iRowsAffected = 0;
            if (sPageLib == "L")
            {
                iRowsAffected = wsLive.SetRequestDuration(sfd.GetWsKey(), iNewKey, dDuration);
            }
            else
            {
                iRowsAffected = wsTest.SetRequestDuration(sfd.GetWsKey(), iNewKey, dDuration);
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "SAVE Requests Error...");
        }
        finally
        {
        }
    }
    // =========================================================
    protected void GetParms()
    {
        if (Session["reqSource"] != null)
        {
            hfReqSource.Value = Session["reqSource"].ToString().Trim();
        }

        Session["reqSource"] = "Problem";

        // Load your hidden fields (from three possible previous programs) 
        // Don't load later (unload or they won't reach hidden fields)
        if (Session["reqPri"] != null)
        {
            hfPri.Value = Session["reqPri"].ToString().Trim();
            Session["reqPri"] = null;
        }
        if (Session["reqCs1"] != null)
        {
            hfCs1.Value = Session["reqCs1"].ToString().Trim();
            Session["reqCs1"] = null;
        }
        if (Session["reqCs2"] != null)
        {
            hfCs2.Value = Session["reqCs2"].ToString().Trim();
            Session["reqCs2"] = null;
        }
        if (Session["reqUnitList"] != null)
        {
            hfUnitList.Value = Session["reqUnitList"].ToString().Trim();
            Session["reqUnitList"] = null;
        }
        if (Session["reqPhone"] != null)
        {
            hfPhone.Value = Session["reqPhone"].ToString().Trim();
            Session["reqPhone"] = null;
        }
        if (Session["reqExtension"] != null)
        {
            hfExtension.Value = Session["reqExtension"].ToString().Trim();
            Session["reqExtension"] = null;
        }
        if (Session["reqContact"] != null)
        {
            hfContact.Value = Session["reqContact"].ToString().Trim();
            Session["reqContact"] = null;
        }
        if (Session["reqEmail"] != null)
        {
            hfEmail.Value = Session["reqEmail"].ToString().Trim();
            Session["reqEmail"] = null;
        }
        if (Session["reqCreator"] != null)
        {
            hfCreator.Value = Session["reqCreator"].ToString().Trim();
            Session["reqCreator"] = null;
        }
        if (Session["reqReqType"] != null)
        {
            hfReqType.Value = Session["reqReqType"].ToString().Trim();
            Session["reqReqType"] = null;
        }
        if (Session["reqForcedQty"] != null)
        {
            hfForcedQty.Value = Session["reqForcedQty"].ToString().Trim();
            Session["reqForcedQty"] = null;
        }
        if (Session["reqMthdT"] != null)
        {
            hfCommMethodType.Value = Session["reqMthdT"].ToString().Trim();
            Session["reqMthdT"] = null;
        }
        if (Session["reqMthdI"] != null)
        {
            hfCommMethodInfo.Value = Session["reqMthdI"].ToString().Trim();
            Session["reqMthdI"] = null;
        }
        if (Session["reqMethodPhoneExt"] != null)
        {
            hfCommMethodPhoneExt.Value = Session["reqMethodPhoneExt"].ToString().Trim();
            Session["reqMethodPhoneExt"] = null;
        }
    }
    // =========================================================
    protected void RunRequestThroughTriggerFile(int newKey, int reqCount)
    {
        string sResult = "";
        string testFlag = "";

        if (sPageLib == "L")
        {
            sResult = wsLive.TriggerServiceRequest(sfd.GetWsKey(), newKey, reqCount, testFlag);
        }
        else
        {
            testFlag = "Y";
            sResult = wsTest.TriggerServiceRequest(sfd.GetWsKey(), newKey, reqCount, testFlag);
        }

    }
    // =========================================================
    // =========================================================
}

