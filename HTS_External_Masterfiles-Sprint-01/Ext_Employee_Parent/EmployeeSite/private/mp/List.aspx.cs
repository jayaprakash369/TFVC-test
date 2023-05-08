using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.Odbc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

// using System.Web.UI.DataVisualization.Charting.ChartHttpHandler;

public partial class private_mp_List : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sConnectionString = "";
    string sErrMessage = "";
    //string sErrValues = "";
    string sSql = "";
    //    double dRatioCutoff = 1.5;
    //    int iDaysCutoff = 6;
    //    int iXAxisInterval = 30;
    int iKey = 0;

    //ErrorHandler eh;
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
        //eh = new ErrorHandler();
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e)
    {
        //eh = null;
    }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set here once for all page SQLs
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        // Set Javascript client side actions here
        //        ddCustomer.Attributes.Add("onChange", "return clearOtherDropDown('ddCustomer')");
        //btClear.Attributes.Add("onClick", "return resetInput()");

        // ===============================================================
        if (!IsPostBack)
        {
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dataTable = new DataTable(sMethodName);

            try
            {
                odbcConn.Open();
                int i = 0;
                // Load Customers
                dataTable = getNameNumForAllCustomers();
                ddCustomer.DataSource = dataTable;
                ddCustomer.DataValueField = "CSTRNR";
                ddCustomer.DataTextField = "NAME_ABBR";
                ddCustomer.DataBind();
                ddCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
                // ddCustomer.Items.Insert(26, new System.Web.UI.WebControls.ListItem("OMNICARE INC 2", "80446")); // Text, Value

                // Load Days To Empty
                for (i = 0; i < 30; i++)
                {
                    ddDaysToEmpty.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString() + " days", i.ToString()));
                }
                ddDaysToEmpty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
                ddDaysToEmpty.Items[15].Selected = true;

                // Load Toner Level
                for (i = 0; i < 99; i++)
                {
                    ddTonerLevel.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString() + "%", i.ToString()));
                }
                ddTonerLevel.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
                //            ddTonerLevel.Items[11].Selected = true;

                // Load Weeks Since Last Scan
                for (i = 0; i < 30; i++)
                {
                    ddSilentWeeks.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                }
                ddSilentWeeks.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
                ddSilentWeeks.Items[9].Selected = true;

                // Load Cartridge Qty On Hand
                for (i = 0; i < 40; i++)
                {
                    ddQohBlk.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                    ddQohMic.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                    ddQohCyn.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                    ddQohMag.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                    ddQohYlw.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                }

                ddQohBlk.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "")); // Text, Value
                ddQohMic.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
                ddQohCyn.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
                ddQohMag.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
                ddQohYlw.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
                odbcConn.Close();
            }
        }

    }
    // ========================================================================
    protected void Page_PreRender(object sender, EventArgs e)
    {

        // ===============================================================
        if (hfPanelToShow.Value == "Detail")
        {
            try
            {
                odbcConn.Open();
                DoDetailLoad();
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcConn.Close();
                odbcCmd.Dispose();
            }
        }
        // ===============================================================
        else
        {
            try
            {
                odbcConn.Open();
                DoListLoad();
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
                odbcConn.Close();
            }
        }
        // --------------------------------------
    }
    // ========================================================================
    protected void btRun_Click(object sender, EventArgs e)
    {
        hfPanelToShow.Value = "List";
        hfSeq.Value = "";
    }

    // ========================================================================
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            odbcConn.Open();
            int iKey = 0;
            int iSeq = 0;
            int iVia = 0;
            int iUpdPriority = 0;
            string sUpdComment = "";

            int iQohBlk = 0;
            int iQohMic = 0;
            int iQohCyn = 0;
            int iQohMag = 0;
            int iQohYlw = 0;

            if (int.TryParse(hfKey.Value, out iKey) == false)
                iKey = 0;
            if (int.TryParse(hfSeq.Value, out iSeq) == false)
                iSeq = 0;
            if (int.TryParse(ddVia.SelectedItem.Value.ToString(), out iVia) == false)
                iVia = 0;
            if (int.TryParse(ddUpdPriority.SelectedItem.Value.ToString(), out iUpdPriority) == false)
                iUpdPriority = 0;

            if (int.TryParse(ddQohBlk.SelectedItem.Value.ToString(), out iQohBlk) == false)
                iQohBlk = 0;
            if (int.TryParse(ddQohMic.SelectedItem.Value.ToString(), out iQohMic) == false)
                iQohMic = 0;
            if (int.TryParse(ddQohCyn.SelectedItem.Value.ToString(), out iQohCyn) == false)
                iQohCyn = 0;
            if (int.TryParse(ddQohMag.SelectedItem.Value.ToString(), out iQohMag) == false)
                iQohMag = 0;
            if (int.TryParse(ddQohYlw.SelectedItem.Value.ToString(), out iQohYlw) == false)
                iQohYlw = 0;

            sUpdComment = txUpdComment.Text.Trim();

            // ----------------------
            // Save new comment in log
            if ((sUpdComment != "") && (sUpdComment != GetComment(iKey)))
            {
                DateTime datTemp = new DateTime();
                datTemp = DateTime.Now;
                int iDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                int iTime = Int32.Parse(datTemp.ToString("HHmmss"));

                sSql = "insert into " + sLibrary + ".MPUNOTLG" +
                    " (nKey, nDat, nTim, nStp, nCom)" +
                    " Values(?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = iKey;

                odbcCmd.Parameters.Add("@Date", OdbcType.Int);
                odbcCmd.Parameters["@Date"].Value = iDate;

                odbcCmd.Parameters.Add("@Time", OdbcType.Int);
                odbcCmd.Parameters["@Time"].Value = iTime;

                odbcCmd.Parameters.Add("@Timestamp", OdbcType.VarChar, 100);
                odbcCmd.Parameters["@Timestamp"].Value = datTemp.ToString().Trim();

                odbcCmd.Parameters.Add("@Comment", OdbcType.VarChar, 250);
                odbcCmd.Parameters["@Comment"].Value = sUpdComment;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            // ----------------------


            sSql = "update " + sLibrary + ".MPUHD set" +
                 " hPriority = ?" +
                ", hShipVia = ?" +
                ", hComment = ?" +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Priority", OdbcType.Int);
            odbcCmd.Parameters["@Priority"].Value = iUpdPriority;

            odbcCmd.Parameters.Add("@ShipVia", OdbcType.Int);
            odbcCmd.Parameters["@ShipVia"].Value = iVia;

            odbcCmd.Parameters.Add("@Comment", OdbcType.VarChar, 250);
            odbcCmd.Parameters["@Comment"].Value = sUpdComment;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = iKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            // ----------------------
            int iCurrQoh = 0;
            if (int.TryParse(GetCartridgeQtyOnHand(iKey, 1), out iCurrQoh) == false)
                iCurrQoh = 0;
            else if ((ddQohBlk.SelectedItem.Value.ToString() != "") && (iCurrQoh != iQohBlk))
                UpdCartridgeQtyOnHand(iKey, 1, iQohBlk);

            if (int.TryParse(GetCartridgeQtyOnHand(iKey, 2), out iCurrQoh) == false)
                iCurrQoh = 0;
            else if ((ddQohMic.SelectedItem.Value.ToString() != "") && (iCurrQoh != iQohMic))
                UpdCartridgeQtyOnHand(iKey, 2, iQohMic);

            if (int.TryParse(GetCartridgeQtyOnHand(iKey, 3), out iCurrQoh) == false)
                iCurrQoh = 0;
            else if ((ddQohCyn.SelectedItem.Value.ToString() != "") && (iCurrQoh != iQohCyn))
                UpdCartridgeQtyOnHand(iKey, 3, iQohCyn);

            if (int.TryParse(GetCartridgeQtyOnHand(iKey, 4), out iCurrQoh) == false)
                iCurrQoh = 0;
            else if ((ddQohMag.SelectedItem.Value.ToString() != "") && (iCurrQoh != iQohMag))
                UpdCartridgeQtyOnHand(iKey, 4, iQohMag);

            if (int.TryParse(GetCartridgeQtyOnHand(iKey, 5), out iCurrQoh) == false)
                iCurrQoh = 0;
            else if ((ddQohYlw.SelectedItem.Value.ToString() != "") && (iCurrQoh != iQohYlw))
                UpdCartridgeQtyOnHand(iKey, 5, iQohYlw);

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        // ----------------------
        hfPanelToShow.Value = "List";
    }
    // ========================================================================
    protected void UpdCartridgeQtyOnHand(int key, int seq, int qty)
    {
        try
        {
            sSql = "update " + sLibrary + ".MPUCAR set" +
                 " cQtyOnHand = ?" +
                " where cKey = ?" +
                " and cSeq = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Qty", OdbcType.Int);
            odbcCmd.Parameters["@Qty"].Value = qty;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = seq;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------
    }
    // ========================================================================
    protected void ajaxVerdictUpdate(int verdict, int key, int seq)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here
        SiteHandler sh = new SiteHandler();
        //string sLibAjax = sh.getLibrary();
        string sLibAjax = "OMDTALIB";

        try
        {
            odbcConn.Open();

            sSql = "update " + sLibAjax + ".MPUCAR set" +
                " CVERDICT = ?" +
                " where CKEY = ?" +
                " and CSEQ = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Verdict", OdbcType.Int);
            odbcCmd.Parameters["@Verdict"].Value = verdict;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = seq;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            /* */
            sSql = "update " + sLibAjax + ".MPUHD set" +
                " HVERDICT = 1" +
                " where HKEY = ?" +
                " and ((select count(cSeq) from " + sLibAjax + ".mpucar where cKey = ? and cVerdict = 1) > 0) ";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key1", OdbcType.Int);
            odbcCmd.Parameters["@Key1"].Value = key;

            odbcCmd.Parameters.Add("@Key2", OdbcType.Int);
            odbcCmd.Parameters["@Key2"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            sSql = "update " + sLibAjax + ".MPUHD set" +
                " HVERDICT = 0" +
                " where HKEY = ?" +
                " and ((select count(cSeq) from " + sLibAjax + ".mpucar where cKey = ? and cVerdict = 1) = 0) ";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key1", OdbcType.Int);
            odbcCmd.Parameters["@Key1"].Value = key;

            odbcCmd.Parameters.Add("@Key2", OdbcType.Int);
            odbcCmd.Parameters["@Key2"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        // ----------------------
    }
    // ========================================================================
    protected override void CreateChildControls()
    {
        Control myControl1 = new Control();

        base.CreateChildControls();
        if (ScriptManager1.IsInAsyncPostBack)
        {
            string controlName = Request[ScriptManager1.UniqueID];
            if (controlName != "")
            {
                char[] cSplitter = { '~' };
                char[] cSplitter2 = { '_' };
                string[] saArg = new string[5];
                string[] saVal = new string[4];

                // =================================================
                if (controlName.Contains("vUpd_"))
                {
                    string sKey = "";
                    string sSeq = "";
                    saArg = controlName.Split(cSplitter);
                    int iKey = 0;
                    int iSeq = 0;
                    int iVerdict = 0;
                    string sVerdict = "";

                    // vUpd_2_1234_1 Verdict_Key_Seq  Verdict(0=Undecided, 1=Ship, 2=Wait)
                    string sName = saArg[1];
                    saVal = sName.Split(cSplitter2);

                    sVerdict = saVal[1];
                    if (int.TryParse(sVerdict, out iVerdict) == false)
                    {
                        iVerdict = 0;
                    }

                    sKey = saVal[2];
                    if (int.TryParse(sKey, out iKey) == false)
                    {
                        iKey = 0;
                    }

                    sSeq = saVal[3];
                    if (int.TryParse(sSeq, out iSeq) == false)
                    {
                        iSeq = 0;
                    }

                    ajaxVerdictUpdate(iVerdict, iKey, iSeq);
                }
                // ===================================================
            }
        }
    }
    // ========================================================================
    protected DataTable GetHeader()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iCs1 = 0;
        int iSilentWeeks = 0;
        int iLastScanDate = 0;
        int iKey = 0;
        int iOrderDate = 0;
        int iUnt = 0;
        double dDaysToEmpty = 0;
        double dTonerLevel = 0.0;
        string sFxa = "";
        string sSer = "";
        string sMod = "";
        string sPriorityOnly = "";
        string sShipOnly = "";
        string sSort = "";

        if (ddTonerLevel.SelectedItem.Value.ToString() != "")
            dTonerLevel = Int32.Parse(ddTonerLevel.SelectedItem.Value.ToString());

        if (ddCustomer.SelectedItem.Value.ToString() != "")
            iCs1 = Int32.Parse(ddCustomer.SelectedItem.Value.ToString());

        if (ddDaysToEmpty.SelectedItem.Value.ToString() != "")
            dDaysToEmpty = double.Parse(ddDaysToEmpty.SelectedItem.Value.ToString());

        if (ddSilentWeeks.SelectedItem.Value.ToString() != "")
        {
            iSilentWeeks = Int32.Parse(ddSilentWeeks.SelectedItem.Value.ToString());
            int iSilentDays = iSilentWeeks * 7;
            DateTime datTemp = new DateTime();
            datTemp = DateTime.Today;
            datTemp = datTemp.AddDays(-iSilentDays);
            iLastScanDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        }

        if (ddSort.SelectedItem.Value.ToString() != "")
            sSort = ddSort.SelectedItem.Value.ToString();

        sPriorityOnly = ddPriority.SelectedItem.Value.ToString();
        sShipOnly = ddShipOnly.SelectedItem.Value.ToString();
        sFxa = txFxa.Text.ToUpper();
        sSer = txSer.Text.ToUpper();
        sMod = txMod.Text.ToUpper();

        if (int.TryParse(txOrderDate.Text, out iOrderDate) == false)
        {
            iOrderDate = 0;
        }
        if (int.TryParse(txKey.Text, out iKey) == false)
        {
            iKey = 0;
        }
        if (int.TryParse(txUnt.Text, out iUnt) == false)
        {
            iUnt = 0;
        }

        try
        {
            sSql = "Select distinct" +
                " hKey" +
                ", hLowLif" +
                ", hLowShp" +
                ", hLowLvl" +
                ", hLowEnd" +
                ", hHt_Cs1" +
                ", hHt_Nam" +
                ", hHt_Uid" +
                ", hHt_Mod" +
                ", hHt_Ser" +
                ", hHt_Fxa" +
                ", hPf_Ser" +
                ", hPf_Fxa" +
                ", hPf_Mod" +
                ", hPgWeekB" +
                ", hPgMonthB" +
                ", hPgWeekC" +
                ", hPgMonthC" +
                ", hTLstScn" +
                ", hVerdict" +
                ", hDatLod" +
                ", hDatOrd" +
                ", hRank" +
                ", hComment" +
                ", hQtyPos" +
                ", hPF_UID" +
                ", hShipVia";
            //", IFNULL((select PETNRL from " + sLibrary + ".PRODEQP where PEPART = h.hHt_Mod and PEPART <> ''),'') as reportingIn";
            if (iOrderDate > 0)
            {
                sSql += " from " + sLibrary + ".MPUHD h, " + sLibrary + ".SN3ORDHD1 s" +
                    " where hHt_Uid = OH1UNT";
            }
            else if (sShipOnly == "Y")
            {
                sSql += " from " + sLibrary + ".MPUHD h, " + sLibrary + ".MPUCAR c" +
                    " where hKey = cKey";
            }
            else
            {
                sSql += " from " + sLibrary + ".MPUHD h" +
                    " where hKey > 0";
            }

            sSql += " and hNotMan = 0";
            //sSql += " and hHT_Cs1 not in (102693)"; // Room store is not paying their bills

            if (iKey == 0 && iUnt == 0 && sFxa == "" && sSer == "")
            {
                sSql += " and hHidden = 0" +
                    " and hSilent = 0" +
                    " and hHT_FXA <> ''";
            }
            // global where statments
            sSql += " and hHT_AGR <> ''";

            if (dDaysToEmpty > 0)
                sSql += " and hLowEnd <= ?";
            if (iLastScanDate > 0)
                sSql += " and hTLstScn > ?";
            if (dTonerLevel > 0)
                sSql += " and hLowLvl <= ?";
            if (iCs1 > 0)
                sSql += " and hHt_Cs1 = ?";
            if (sFxa != "")
                sSql += " and ucase(hPf_Fxa) like ?";
            if (sSer != "")
                sSql += " and ucase(hPf_Ser) like ?";
            if (sMod != "")
                sSql += " and hHt_Mod like ?";
            if (sPriorityOnly == "Y")
                sSql += " and hPriority > 0";
            else if ((sFxa == "") && (sSer == "") && (sMod == "") && (iCs1 == 0) && (iKey == 0))
                sSql += " and hPriority = 0";
            if (sShipOnly == "Y")
                sSql += " and cVerdict = 1";
            if (iOrderDate > 0)
                sSql += " and OH1DAT = ?";
            if (iKey > 0)
                sSql += " and hKey = ?";
            if (iUnt > 0)
                sSql += " and hHt_Uid = ?";

            if (sSort == "Qty/Level")
                sSql += " order by hQtyPos, hLowLvl, hRank";
            else if (sSort == "Level")
                sSql += " order by hLowLvl, hRank";
            else
                sSql += " order by hRank";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            if (dDaysToEmpty > 0)
            {
                odbcCmd.Parameters.Add("@Empty", OdbcType.Double);
                odbcCmd.Parameters["@Empty"].Value = dDaysToEmpty;
            }

            if (iLastScanDate > 0)
            {
                odbcCmd.Parameters.Add("@LastScan", OdbcType.Double);
                odbcCmd.Parameters["@LastScan"].Value = iLastScanDate;
            }

            if (dTonerLevel > 0)
            {
                odbcCmd.Parameters.Add("@Level", OdbcType.Double);
                odbcCmd.Parameters["@Level"].Value = dTonerLevel;
            }

            if (iCs1 > 0)
            {
                odbcCmd.Parameters.Add("@Cs1", OdbcType.Int);
                odbcCmd.Parameters["@Cs1"].Value = iCs1;
            }
            if (sFxa != "")
            {
                odbcCmd.Parameters.Add("@Fxa", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@Fxa"].Value = sFxa.Trim() + "%";
            }

            if (sSer != "")
            {
                odbcCmd.Parameters.Add("@Ser", OdbcType.VarChar, 35);
                odbcCmd.Parameters["@Ser"].Value = sSer.Trim() + "%";
            }

            if (sMod != "")
            {
                odbcCmd.Parameters.Add("@Mod", OdbcType.VarChar, 35);
                odbcCmd.Parameters["@Mod"].Value = "%" + sMod.Trim() + "%";
            }
            if (iOrderDate > 0)
            {
                odbcCmd.Parameters.Add("@DaysSinceOrd", OdbcType.Int);
                odbcCmd.Parameters["@DaysSinceOrd"].Value = iOrderDate;
            }
            if (iKey > 0)
            {
                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = iKey;
            }

            if (iUnt > 0)
            {
                odbcCmd.Parameters.Add("@Unt", OdbcType.Int);
                odbcCmd.Parameters["@Unt"].Value = iUnt;
            }

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }
    // ========================================================================
    protected void LoadTableRows(DataTable dTable)
    {

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iRowColor = 0;

        int iCs1 = 0;
        int iHeaderKey = 0;
        int iPgWeekB = 0;
        int iPgMonthB = 0;
        int iPgWeekC = 0;
        int iPgMonthC = 0;
        int iVerdict = 0;
        int iDateLoad = 0;
        int iDateOrder = 0;
        int iUid = 0;
        int iRank = 0;
        int iVia = 0;
        int iDateLastAct = 0;
        double dLowEnd = 0.0;
        double dLowLvl = 0.0;
        double dLowLif = 0.0;
        double dLowShp = 0.0;
        string sNam = "";
        string sMod = "";
        string sSer = "";
        string sFxa = "";
        string sSerPf = "";
        string sFxaPf = "";
        string sModPf = "";
        string sUidPf = "";
        string sComment = "";
        //        string sReportingIn = "";
        string sDateFormat = "Mon dd";

        //TableHeaderRow thRow;
        //TableHeaderCell thCell;
        HyperLink hlPf = new HyperLink();
        TableRow tRow;
        TableCell tCell;

        System.Drawing.Color myWhite = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#406080"); // EBEBE0
        System.Drawing.Color myBeige = System.Drawing.ColorTranslator.FromHtml("#EBEBE0");
        System.Drawing.Color myYellow = System.Drawing.ColorTranslator.FromHtml("#FFF8DC");
        System.Drawing.Color myBlueShipped = System.Drawing.ColorTranslator.FromHtml("#A2B3C3");  //, orig dark brown C3C3A2, BBBB95 , 8399AF dark blue, A2B3C3 lighter blue

        System.Drawing.Color currentColor;

        NumberFormatter nf = new NumberFormatter();

        // Main Content Section
        int iRowIdx = 0;
        int iMaxRows = 0;
        int.TryParse(ddMaxRows.SelectedValue, out iMaxRows);
        if (iMaxRows == 0)
            iMaxRows = 250;

        foreach (DataRow row in dTable.Rows)
        {
            if (iRowIdx < iMaxRows) // Only show the max rows desired
            {
                iHeaderKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString());
                iCs1 = Int32.Parse(dTable.Rows[iRowIdx]["hHt_Cs1"].ToString());
                iUid = Int32.Parse(dTable.Rows[iRowIdx]["hHt_Uid"].ToString());
                iPgWeekB = Int32.Parse(dTable.Rows[iRowIdx]["hPgWeekB"].ToString());
                iPgMonthB = Int32.Parse(dTable.Rows[iRowIdx]["hPgMonthB"].ToString());
                iPgWeekC = Int32.Parse(dTable.Rows[iRowIdx]["hPgWeekC"].ToString());
                iPgMonthC = Int32.Parse(dTable.Rows[iRowIdx]["hPgMonthC"].ToString());
                iVerdict = Int32.Parse(dTable.Rows[iRowIdx]["hVerdict"].ToString());
                iDateLoad = Int32.Parse(dTable.Rows[iRowIdx]["hDatLod"].ToString());
                iDateOrder = Int32.Parse(dTable.Rows[iRowIdx]["hDatOrd"].ToString());
                iDateLastAct = Int32.Parse(dTable.Rows[iRowIdx]["hTLstScn"].ToString());
                iVia = Int32.Parse(dTable.Rows[iRowIdx]["hShipVia"].ToString());
                iRank = Int32.Parse(dTable.Rows[iRowIdx]["hRank"].ToString());
                dLowLvl = double.Parse(dTable.Rows[iRowIdx]["hLowLvl"].ToString());
                dLowEnd = double.Parse(dTable.Rows[iRowIdx]["hLowEnd"].ToString());
                dLowLif = double.Parse(dTable.Rows[iRowIdx]["hLowLif"].ToString());
                dLowShp = double.Parse(dTable.Rows[iRowIdx]["hLowShp"].ToString());
                sNam = dTable.Rows[iRowIdx]["hHt_Nam"].ToString().Trim();
                sMod = dTable.Rows[iRowIdx]["hHt_Mod"].ToString().Trim();
                sSer = dTable.Rows[iRowIdx]["hHt_Ser"].ToString().Trim();
                sFxa = dTable.Rows[iRowIdx]["hHt_Fxa"].ToString().Trim();
                sSerPf = dTable.Rows[iRowIdx]["hPf_Ser"].ToString().Trim();
                sFxaPf = dTable.Rows[iRowIdx]["hPf_Fxa"].ToString().Trim();
                sModPf = dTable.Rows[iRowIdx]["hPf_Mod"].ToString().Trim();
                sUidPf = dTable.Rows[iRowIdx]["hPf_Uid"].ToString().Trim();
                sComment = dTable.Rows[iRowIdx]["hComment"].ToString().Trim();
                //sReportingIn = dTable.Rows[iRowIdx]["reportingIn"].ToString().Trim();

                if (sNam.Length > 11)
                    sNam = sNam.Substring(0, 11);

                if (iRowColor == 0)
                {
                    currentColor = myBeige;
                    iRowColor = 1;
                }
                else
                {
                    currentColor = myWhite;
                    iRowColor = 0;
                }

                // ----------------------
                // New Header Row
                // ----------------------
                tRow = new TableRow();
                tbShipList.Rows.Add(tRow);
                tRow.BackColor = currentColor;

                tCell = new TableCell();
                tCell.Text = "<a name=\"A" + (iRowIdx + 1).ToString() + "\"></a>" +
                "<input type=\"button\" class=\"detailButton\" value=\"U\"" +
                " style=\"padding: 2px;\"" +
                " onclick=\"return detailClick(" + iHeaderKey + ", " + (iRowIdx + 1) + ");\">";
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Seq: " + (iRowIdx + 1).ToString() + "<br />Rank:" + iRank.ToString();
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "DysLeft<br /><b>" + dLowEnd.ToString() + "</b>";
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Lvl<br />" + dLowLvl.ToString() + "%";
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Wk: " + nf.formatNum(Convert.ToDouble(iPgWeekB), 0, "") + "<span style='color:#94002C'> " + nf.formatNum(Convert.ToDouble(iPgWeekC), 0, "");
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Mo: " + nf.formatNum(Convert.ToDouble(iPgMonthB), 0, "") + "<span style='color:#94002C'> " + nf.formatNum(Convert.ToDouble(iPgMonthC), 0, "");
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "LastAct<br />" + FormatDate(iDateLastAct, sDateFormat);
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Key<br />" + iHeaderKey.ToString();
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Unit<br />" + iUid.ToString();
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                string sTemp = "";
                //                if (sReportingIn == "N")
                //                    sTemp = "ZZZ <span style=\"color:#AD0034;\">" + sComment + "</span>";
                //                else
                sTemp = "<span style=\"color:#AD0034;\">" + sComment + "</span>";
                if (iVia == 3)
                    sTemp += "&nbsp;2ndDay";
                else if (iVia == 4)
                    sTemp += "&nbsp;Next Day Saver";
                else if (iVia == 5)
                    sTemp += "&nbsp;Next Day Air";
                tCell.Text = sTemp.Trim();
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                if (sFxa == "")
                    tCell.Text = "Asset<br /><span style='color:#94002C'>" + sFxaPf + "</font>";
                else
                    tCell.Text = "Asset<br />" + sFxa;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                if (sMod == "")
                    tCell.Text = "Model<br /><span style='color:#94002C'>" + sModPf + "</font>";
                else
                    tCell.Text = "Model<br />" + sMod;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                if (sFxa == "")
                    tCell.Text = "Serial<br /><span style='color:#94002C'>" + sSerPf + "</font>";
                else
                    tCell.Text = "Serial<br />" + sSer;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Cust " + iCs1.ToString() + "<br />" + sNam;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                hlPf = new HyperLink();
                hlPf.NavigateUrl = "http://pf.harlandts.com/device_view.aspx?id=" + sUidPf;
                hlPf.Text = "Cov";
                hlPf.Target = "PfCoverage";
                tCell.Controls.Add(hlPf);
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                // ------------------------------
                // Cartridge and Chart Row...
                // ------------------------------

                tRow = new TableRow();
                tbShipList.Rows.Add(tRow);
                tRow.BackColor = currentColor;

                tCell = new TableCell();
                tCell.ColumnSpan = 9;
                tCell.CssClass = "cartridgeCell";
                dataTable = GetDetail(iHeaderKey);
                Table tbCartridges = new Table();
                tbCartridges = LoadCartridgeDetailRows(dataTable, currentColor, dLowEnd);
                tbCartridges.CssClass = "cartridgeTable";
                tCell.Controls.Add(tbCartridges);
                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);

                // Create Reset Positioning Row
                tCell = new TableCell();
                // tCell.Text = "<br /><br /><br /><br /><br /><br /><br /><br /><a name=\"A" + (iRowIdx + 1).ToString() + "\"></a>";
                //tCell.Text = "<a name=\"A" + (iRowIdx + 1).ToString() + "\"></a>";
                tCell.Width = 0;
                tRow.Cells.Add(tCell);

                // Create Chart Control //
                tCell = new TableCell();
                tCell.ColumnSpan = 6;

                string sFilePath = CreateImageFilePath(iHeaderKey);
                System.Web.UI.WebControls.Image imTonerLevels = new System.Web.UI.WebControls.Image();
                imTonerLevels.ImageUrl = sFilePath;
                //imTonerLevels.ImageUrl = "Swoop.jpg";
                //imTonerLevels.ImageUrl = "~/media/charts/mp/tonerHist/chart_00012.bmp";
                //imTonerLevels.AlternateText = "Image_" + iHeaderKey.ToString();
                tCell.Controls.Add(imTonerLevels);
                //tCell.Text = "<i>";

                tCell.HorizontalAlign = HorizontalAlign.Center;
                tRow.Cells.Add(tCell);
            }
            iRowIdx++;
        }
        nf = null;  // delete number formatter object
    }
    // ========================================================================
    protected DataTable GetDetail(int headerKey)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " cKey" +
                ", cSeq" +
                ", cDaysToEnd" +
                ", cTonLevel" +
                ", cTonLifesp" +
                ", cDaysOrder" +
                ", cDateOrder" +
                ", cQtyOrder" +
                ", cDateLoad" +
                ", cPartName" +
                ", cQtyOnHand" +
                ", cVerdict" +
                ", IFNULL((select PEMLIF from " + sLibrary + ".PRODEQP where PEPART = c.cPartName and PEPART <> ''),0) as cartridgePages" +
                " from " + sLibrary + ".MPUCAR c, " + sLibrary + ".MPUHD m " +
                " where hKey = cKey" +
                " and (" +
                   " (hCurMicr = 0 and cSeq = 1) " +
                " or (hCurMicr = 1 and cSeq = 2) " +
                " or (cSeq in (3, 4, 5))" +
                " )" +
                " and cKey = ?" +
                " order by cSeq";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = headerKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }
    // ========================================================================
    protected Table LoadCartridgeDetailRows(DataTable dTable, System.Drawing.Color rowColor, double closestToEmpty)
    {
        int iKey = 0;
        int iSeq = 0;
        double dDaysToEnd = 0.0;
        double dTonLevel = 0.0;
        int iTonLifesp = 0;
        int iDaysOrder = 0;
        int iDateOrder = 0;
        int iQtyOrdered = 0;
        int iQtyOnHand = 0;
        int iDateLoad = 0;
        int iVerdict = 0;
        int iCartridgePages = 0;
        //int iTonerQty = 0;
        string sPartName = "";
        string sDateFormat = "Mon dd";

        Table tbToReturn = new Table();
        tbToReturn.Width = 600;

        TableRow tRow;
        TableCell tCell;

        System.Drawing.Color myGray7 = System.Drawing.ColorTranslator.FromHtml("#777777");
        System.Drawing.Color myPurple = System.Drawing.ColorTranslator.FromHtml("#600080");
        System.Drawing.Color myYellow = System.Drawing.ColorTranslator.FromHtml("#F0E68C");
        System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#7194B7");
        System.Drawing.Color myRed7 = System.Drawing.ColorTranslator.FromHtml("#7A0025");
        System.Drawing.Color myCrimson = System.Drawing.ColorTranslator.FromHtml("#DC143C");
        System.Drawing.Color myWhite = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        System.Drawing.Color myThistle = System.Drawing.ColorTranslator.FromHtml("#D8BFD8");
        System.Drawing.Color myOrange = System.Drawing.ColorTranslator.FromHtml("#FF8C00");  // Goldenrod DAA520 // dark or FF8C00  // Choc D2691E
        System.Drawing.Color myJustShippedColor = System.Drawing.ColorTranslator.FromHtml("#F0E68C"); // Cornsilk #FFF8DC, Burly #DEB887

        System.Drawing.Color myBlack = System.Drawing.ColorTranslator.FromHtml("#333333");

        System.Drawing.Color myRed9 = System.Drawing.ColorTranslator.FromHtml("#94002C");
        System.Drawing.Color myGrayE = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
        System.Drawing.Color myGreen = System.Drawing.ColorTranslator.FromHtml("#3CB371"); // dark green 006400 med sea 3CB371
        System.Drawing.Color colorBack = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        System.Drawing.Color colorText = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

        tRow = new TableRow();
        tRow.BackColor = rowColor;
        tbToReturn.Rows.Add(tRow);

        tCell = new TableCell();
        tCell.Text = "Type";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Days Left";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Level";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Load Date";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Order Date";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);
        /*
                tCell = new TableCell();
                tCell.Text = "Since Ord";
                tCell.CssClass = "smallHead";
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = "Life Span";
                tCell.CssClass = "smallHead";
                tRow.Cells.Add(tCell);
        */
        tCell = new TableCell();
        tCell.Text = "Cartridge";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Pg";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Qty";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "Qoh";
        tCell.CssClass = "smallHead";
        tRow.Cells.Add(tCell);

        NumberFormatter nf = new NumberFormatter();

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["cKey"].ToString());
            iSeq = Int32.Parse(dTable.Rows[iRowIdx]["cSeq"].ToString());
            dDaysToEnd = double.Parse(dTable.Rows[iRowIdx]["cDaysToEnd"].ToString());
            dTonLevel = double.Parse(dTable.Rows[iRowIdx]["cTonLevel"].ToString());
            iTonLifesp = Int32.Parse(dTable.Rows[iRowIdx]["cTonLifesp"].ToString());
            iDaysOrder = Int32.Parse(dTable.Rows[iRowIdx]["cDaysOrder"].ToString());
            iDateOrder = Int32.Parse(dTable.Rows[iRowIdx]["cDateOrder"].ToString());
            iQtyOrdered = Int32.Parse(dTable.Rows[iRowIdx]["cQtyOrder"].ToString());
            iQtyOnHand = Int32.Parse(dTable.Rows[iRowIdx]["cQtyOnHand"].ToString());
            iVerdict = Int32.Parse(dTable.Rows[iRowIdx]["cVerdict"].ToString());
            iCartridgePages = Int32.Parse(dTable.Rows[iRowIdx]["cartridgePages"].ToString());
            iDateLoad = Int32.Parse(dTable.Rows[iRowIdx]["cDateLoad"].ToString());
            sPartName = dTable.Rows[iRowIdx]["cPartName"].ToString().Trim();

            string sJustSent = "";
            double dJustShippedCutoff = 0.0;
            double dDaysOrder = Convert.ToDouble(iDaysOrder);
            double dTonLifesp = Convert.ToDouble(iTonLifesp);
            if ((dDaysOrder > 0) && (dTonLifesp > 0))
                dJustShippedCutoff = (dTonLifesp / dDaysOrder);

            if (((iDateOrder >= iDateLoad) && (iDateOrder > 0)) || ((iDateOrder > 0) && (iDateLoad == 0)))
            {
                sJustSent = "Y";
            }


            // Main Content Section
            string sVerdictText = "";
            string sVerdictIncrement = "";
            if (iVerdict == 1)
            {
                sVerdictText = ".";
                colorText = myGreen;
                colorBack = myGreen;
                sVerdictIncrement = "0"; // was 2
            }
            /*
             * else if (iVerdict == 2)
                        {
                            sVerdictText = "..";
                            colorText = myOrange;
                            colorBack = myOrange;
                            sVerdictIncrement = "0";
                        }

             */
            else
            {
                sVerdictText = "-";
                colorText = myGrayE;
                colorBack = myGrayE;
                sVerdictIncrement = "1";
            }

            tRow = new TableRow();
            tRow.BackColor = rowColor;
            tbToReturn.Rows.Add(tRow);

            tCell = new TableCell();

            Button btVerdict = new Button();
            string btVerdictName = "vUpd_" + sVerdictIncrement + "_" + iKey.ToString() + "_" + iSeq.ToString();
            btVerdict.ID = "~" + btVerdictName;
            btVerdict.Text = sVerdictText;
            btVerdict.CssClass = "verdictButton";
            btVerdict.BackColor = colorBack;
            btVerdict.ForeColor = colorText;
            btVerdict.BorderWidth = 2;
            btVerdict.BorderColor = myBlack;
            btVerdict.OnClientClick = "verdictClick('" + btVerdictName + "')";
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tCell.Controls.Add(btVerdict);

            if (iSeq == 1)
            {
                //tCell.Text = "Blk";
                tCell.BackColor = myGray7;
                tCell.ForeColor = myWhite;
                tRow.Cells.Add(tCell);
            }
            else if (iSeq == 2)
            {
                //tCell.Text = "Mic"; 
                tCell.BackColor = myPurple; // myGray7
                tCell.ForeColor = myWhite;
                tRow.Cells.Add(tCell);
            }
            else if (iSeq == 3)
            {
                //tCell.Text = "Cyn";
                tCell.BackColor = myBlue;
                tCell.ForeColor = myWhite;
                tRow.Cells.Add(tCell);
            }
            else if (iSeq == 4)
            {
                //tCell.Text = "Mag";
                tCell.BackColor = myRed7;
                tCell.ForeColor = myWhite;
                tRow.Cells.Add(tCell);
            }
            else if (iSeq == 5)
            {
                //tCell.Text = "Ylw";
                tCell.BackColor = myYellow;
                tCell.ForeColor = myGray7;
                tRow.Cells.Add(tCell);
            }

            tCell = new TableCell();
            if (dDaysToEnd == closestToEmpty)
                tCell.Text = "<b>" + dDaysToEnd.ToString() + "</b>";
            else
                tCell.Text = dDaysToEnd.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = dTonLevel.ToString() + "%";
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = FormatDate(iDateLoad, sDateFormat);
            //if ((iDateLoad > 0 && iDateOrder > 0) && (iDateLoad <= iDateOrder))
            //if (((iDateOrder >= iDateLoad) && (iDateOrder > 0)) || ((iDateOrder > 0) && (iDateLoad == 0)))
            if (sJustSent == "Y")
            {
                tCell.BackColor = myJustShippedColor;
            }
            else
            {
                tCell.BackColor = rowColor;
            }
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = FormatDate(iDateOrder, sDateFormat);
            //if ((iDateLoad > 0 && iDateOrder > 0) && (iDateLoad <= iDateOrder))
            //if (((iDateOrder >= iDateLoad) && (iDateOrder > 0)) || ((iDateOrder > 0) && (iDateLoad == 0)))
            if (sJustSent == "Y")
            {
                tCell.BackColor = myJustShippedColor;
            }
            else
            {
                tCell.BackColor = rowColor;
            }

            tRow.Cells.Add(tCell);

            /*
                        tCell = new TableCell();
                        tCell.Text = iDaysOrder.ToString();
                        if (iSeq <= 2) tRow.Cells.Add(tCell);
                        else tRow.Cells.Add(tCell);

                        tCell = new TableCell();
                        tCell.Text = iTonLifesp.ToString();
                        tRow.Cells.Add(tCell);
            */
            tCell = new TableCell();
            tCell.Text = sPartName;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            if (iCartridgePages > 0)
                tCell.Text = nf.formatNum(Convert.ToDouble(iCartridgePages), 0, "");
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iQtyOrdered.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iQtyOnHand.ToString();
            tRow.Cells.Add(tCell);

            iRowIdx++;
        }

        nf = null;  // delete number formatter object

        // Create Filler Rows for "Black and White" Printers
        /*
        if (iRowIdx < 4)
        {
            if (iRowIdx < 1)
            {
                tRow = new TableRow();
                tRow.BackColor = rowColor;
                tCell = new TableCell();
                tCell.Text = "";
                tCell.RowSpan = 1;
                tCell.ColumnSpan = 11;
                tRow.Cells.Add(tCell);
                tbToReturn.Rows.Add(tRow);
                iRowIdx++;
            }

            int i = 0;
            for (i = iRowIdx; i < 4; i++) 
            {
                tRow = new TableRow();
                tRow.BackColor = rowColor;
                tbToReturn.Rows.Add(tRow);
                tCell = new TableCell();
                tCell.Text = "";
                tCell.ColumnSpan = 11;
                tRow.Cells.Add(tCell);
            }
        }
         * */
        // ---------------------------------------
        return tbToReturn;
    }
    // ========================================================================
    protected string FormatDate(int date8, string dateFormat)
    {
        string sDate = "";
        string sMonth = "";
        string sMonthFormat = "";
        DateTime datTemp = new DateTime();
        int iMon = 0;

        if (date8 > 0)
        {
            sDate = date8.ToString();
            datTemp = Convert.ToDateTime(sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2) + " 01:01:01.000");

            if (dateFormat == "Mon dd")
            {
                iMon = Int32.Parse(datTemp.Month.ToString());
                sMonthFormat = "Mon";
                sMonth = FormatMonth(iMon, sMonthFormat);
                sDate = sMonth + " " + datTemp.Day;
            }
        }

        return sDate;
    }
    // ========================================================================
    protected string FormatMonth(int month, string monthFormat)
    {
        string sMonth = "";

        if (monthFormat == "Mon")
        {
            if (month == 1)
                sMonth = "Jan";
            else if (month == 2)
                sMonth = "Feb";
            else if (month == 3)
                sMonth = "Mar";
            else if (month == 4)
                sMonth = "Apr";
            else if (month == 5)
                sMonth = "May";
            else if (month == 6)
                sMonth = "Jun";
            else if (month == 7)
                sMonth = "Jul";
            else if (month == 8)
                sMonth = "Aug";
            else if (month == 9)
                sMonth = "Sep";
            else if (month == 10)
                sMonth = "Oct";
            else if (month == 11)
                sMonth = "Nov";
            else if (month == 12)
                sMonth = "Dec";
        }

        return sMonth;
    }
    // ========================================================================
    protected DataTable getNameNumForAllCustomers()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "Select distinct" +
                " CUSTNM" +
                ", CSTRNR" +
                ", CSTRCD" +
                ", SUBSTRING(CUSTNM, 0, 30) as NAME_ABBR" +
                " from " + sLibrary + ".CUSTMAST, " + sLibrary + ".HEADER1" +
                " where CNTRNR = CSTRNR" +
                " and CNTRCD = CSTRCD" +
                " and CONTYP = 'MP'" +
                // " and (CONTYP = 'MP' or contnr in ('00189186'))" + // Omnicare
                " order by CUSTNM";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }
    // ========================================================================
    protected DataTable GetPastTonerLevels(int key, string sortDirection)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddMonths(-6);
        int iStartDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        if (key > 0)
        {
            try
            {
                // ---------------------------------------------------
                // Get Toner Levels for unit passed from metric_tonerlevel
                // ---------------------------------------------------
                sSql = "select 'aaaaaaaaaaaa' as CalDate, tDat, tBlack, tCyan, tMagenta, tYellow, tScn" +
                " from " + sLibrary + ".MPUTONLG" +
                " where tKey = ?" +
                " and tDat > ?" +
                " order by tDat " + sortDirection;

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = key;

                odbcCmd.Parameters.Add("@StartDate", OdbcType.Int);
                odbcCmd.Parameters["@StartDate"].Value = iStartDate;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dataTable.Load(odbcReader);
                if (sortDirection == "Desc") // Just showing table
                {
                    int iRowIdx = 0;
                    int iDate = 0;
                    string sDate = "";
                    string sDateFormat = "Mon dd";
                    foreach (DataRow row in dataTable.Rows)
                    {
                        iDate = Int32.Parse(dataTable.Rows[iRowIdx]["tDat"].ToString());
                        sDate = FormatDate(iDate, sDateFormat);
                        dataTable.Rows[iRowIdx]["CalDate"] = sDate;
                        dataTable.AcceptChanges();
                        iRowIdx++;
                    }
                    dataTable.Columns.Remove("tDat");
                }

            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return dataTable;
    }
    // ========================================================================
    protected DataTable GetUnitDetail(int Key)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            // ---------------------------------------------------
            // Get Detail for Unit from Passed PFUid
            // ---------------------------------------------------
            sSql = "Select hKey as Key, hHt_Cs1 as Customer, hHt_Nam as Name, hHt_Mod as Model, hHt_Ser as Serial, hPf_Fxa as Asset, hHt_Uid as Unit" +
                " from " + sLibrary + ".MPUHD" +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@PfUid", OdbcType.Int);
            odbcCmd.Parameters["@PfUid"].Value = Key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }

    // ========================================================================
    protected DataTable GetTonerOrders(int Key, int Unt)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            // ---------------------------------------------------
            // Get Past Toner Orders
            // ---------------------------------------------------
            sSql = "Select 'aaaaaaaaaaaa' as ShipDate, OH1DAT as NumericDate, OH1EMP as Emp, CONCAT(CONCAT(TRIM(CHAR(OH1CTR)),'-'),TRIM(CHAR(OH1TCK))) as Ticket, OH1PON as PO, OH1PID as Printer, OH1FXA as Asset, OD1PRTOEM as Toner, OD1PRTDSC as Description, OD1PRTQTY as Qty" +
                " from " + sLibrary + ".SN3ORDHD1, " + sLibrary + ".SN4ORDDT1" +
                " where OH1KEY = OD1KEY" +
                " and SUBSTRING(OD1PRTDSC, 1, 5) = 'TONER'" +
                " and (OH1MPK = ?" +
                  " or OH1UNT = ?)" +
                " order by OH1DAT desc, OH1TIM desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
            odbcCmd.Parameters["@WorkfileKey"].Value = Key;

            odbcCmd.Parameters.Add("@PfUid", OdbcType.Int);
            odbcCmd.Parameters["@PfUid"].Value = Unt;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
            /*
                        DataColumn myDataColumn;

                        myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "ShipDate";
                        dataTable.Columns.Add(myDataColumn);
            */
            int iRowIdx = 0;
            int iDate = 0;
            string sDate = "";
            string sDateFormat = "Mon dd";
            foreach (DataRow row in dataTable.Rows)
            {
                iDate = Int32.Parse(dataTable.Rows[iRowIdx]["NumericDate"].ToString());
                sDate = FormatDate(iDate, sDateFormat);
                dataTable.Rows[iRowIdx]["ShipDate"] = sDate;
                dataTable.AcceptChanges();
                iRowIdx++;
            }
            dataTable.Columns.Remove("NumericDate");
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }

    // ========================================================================
    protected DataTable GetComments(int key)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            // ---------------------------------------------------
            // Get Past Toner Orders
            // ---------------------------------------------------
            sSql = "Select nStp as EntryDate, nCom as Comment, nDat, nTim " +
                " from " + sLibrary + ".MPUNOTLG" +
                " where nKey = ?" +
                " order by nDat desc, nTim desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
            dataTable.Columns.Remove("nDat");
            dataTable.Columns.Remove("nTim");

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }

    // ========================================================================
    protected void LoadUpdateFieldsWithCurrentValues(int Key)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "Select hPriority, hShipVia, hComment" +
                " from " + sLibrary + ".MPUHD" +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = Key;

            dataTable = new DataTable("myTable");

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            int iUpdPriority = 0;
            int iVia = 0;
            string sUpdComment = "";
            if (dataTable.Rows.Count > 0)
            {
                iUpdPriority = Int32.Parse(dataTable.Rows[0]["hPriority"].ToString());
                iVia = Int32.Parse(dataTable.Rows[0]["hShipVia"].ToString());
                sUpdComment = dataTable.Rows[0]["hComment"].ToString().Trim();

                if (iVia == 3)
                    ddVia.SelectedIndex = 1;
                else if (iVia == 4)
                    ddVia.SelectedIndex = 2;
                else if (iVia == 5)
                    ddVia.SelectedIndex = 3;
                else
                    ddVia.SelectedIndex = 0;

                ddUpdPriority.SelectedIndex = iUpdPriority;
                txUpdComment.Text = sUpdComment;
            }

            int iQty = 0;
            if (int.TryParse(GetCartridgeQtyOnHand(Key, 1), out iQty) == false)
                iQty = 0;
            else if (iQty > 0 && iQty < 39)
            {
                ddQohBlk.SelectedIndex = iQty + 1;
            }

            if (int.TryParse(GetCartridgeQtyOnHand(Key, 2), out iQty) == false)
                iQty = 0;
            else if (iQty > 0 && iQty < 39)
            {
                ddQohMic.SelectedIndex = iQty + 1;
            }

            if (int.TryParse(GetCartridgeQtyOnHand(Key, 3), out iQty) == false)
                iQty = 0;
            else if (iQty > 0 && iQty < 39)
            {
                ddQohCyn.SelectedIndex = iQty + 1;
            }

            if (int.TryParse(GetCartridgeQtyOnHand(Key, 4), out iQty) == false)
                iQty = 0;
            else if (iQty > 0 && iQty < 39)
            {
                ddQohMag.SelectedIndex = iQty + 1;
            }

            if (int.TryParse(GetCartridgeQtyOnHand(Key, 5), out iQty) == false)
                iQty = 0;
            else if (iQty > 0 && iQty < 39)
            {
                ddQohYlw.SelectedIndex = iQty + 1;
            }

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }

    }
    // ========================================================================
    protected string GetCartridgeQtyOnHand(int key, int seq)
    {
        string sQtyOnHand = "";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "Select cSeq, cQtyOnHand" +
                " from " + sLibrary + ".MPUCAR" +
                " where cKey = ?" +
                " and cSeq = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = seq;

            dataTable = new DataTable("myTable");

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
                sQtyOnHand = dataTable.Rows[0]["cQtyOnHand"].ToString().Trim();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sQtyOnHand;
    }

    // =========================================================================
    protected string CreateImageFilePath(int key)
    {
        string sFilePath = "";
        string sRootPath = "";
        //string sRootPath = Server.MapPath("~") + "\\media\\charts\\mp\\tonerHist\\";
        sRootPath = "~/media/charts/mp/tonerHist/";
        //sRootPath = "http://www.htsweb1.com/media/charts/mp/tonerHist/";
        //sRootPath = "http://192.168.100.2:93/media/mp/charts/tonerHist/";
        string sFilename = "chart_00000";
        string sExtension = ".gif";
        string sKey = "";

        sKey = key.ToString().Trim();
        if (key >= 10000)
            sFilename = sFilename.Substring(0, 6) + sKey + sExtension;
        else if (key >= 1000)
            sFilename = sFilename.Substring(0, 7) + sKey + sExtension;
        else if (key >= 100)
            sFilename = sFilename.Substring(0, 8) + sKey + sExtension;
        else if (key >= 10)
            sFilename = sFilename.Substring(0, 9) + sKey + sExtension;
        else
            sFilename = sFilename.Substring(0, 10) + sKey + sExtension;

        sFilePath = sRootPath + sFilename;

        return sFilePath;
    }
    // ========================================================================
    protected void DoDetailLoad()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        string sSortDirection = "";
        pnListPage.Visible = false;

        int iUnt = 0;

        if (hfKey.Value != null)
        {
            iKey = Int32.Parse(hfKey.Value);
            LoadUpdateFieldsWithCurrentValues(iKey);
        }

        dataTable = new DataTable();
        dataTable = GetUnitDetail(iKey);
        gvHeader.DataSource = dataTable;
        gvHeader.DataBind();

        if (dataTable.Rows.Count > 0)
        {
            iUnt = Int32.Parse(dataTable.Rows[0]["Unit"].ToString().Trim());
        }

        dataTable = GetTonerOrders(iKey, iUnt);
        gvOrders.DataSource = dataTable;
        gvOrders.DataBind();

        dataTable = GetComments(iKey);
        gvComments.DataSource = dataTable;
        gvComments.DataBind();

        sSortDirection = "Desc";
        dataTable = GetPastTonerLevels(iKey, sSortDirection);
        gvTonerLog.DataSource = dataTable;
        gvTonerLog.DataBind();

        pnDetailPage.Visible = true;
    }
    // ========================================================================
    protected void DoListLoad()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        pnDetailPage.Visible = false;
        dataTable = GetHeader();
        LoadTableRows(dataTable);
        pnListPage.Visible = true;
    }
    // ========================================================================
    protected string GetComment(int key)
    {
        string sComment = "";

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " hComment" +
                " from " + sLibrary + ".MPUHD " +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
            if (dataTable.Rows.Count > 0)
                sComment = dataTable.Rows[0]["hComment"].ToString().Trim();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            //eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return sComment;
    }

    // ========================================================================
    protected void newMethod()
    {
    }
    // ========================================================================
    // ========================================================================
}
