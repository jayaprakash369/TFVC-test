using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.Odbc;
using System.Data.SqlClient;

public partial class public_mp_load_Daily : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES                 4848
    // ---------------------------------------

    OdbcCommand odbcCmd;
    OdbcConnection odbcConn;
    OdbcDataReader odbcReader;

    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    // Comment and Uncomment the two references to this in the code below
    //    string sEndDate = "2010-12-27 01:01:01.000";  // Field used to control initial loading of all print fleet data
    string sConnectionString = "";
    string sErrMessage = "";
    string sErrValues = "";
    string sSql = "";
    double dMinimumChangeIndicatingNewCartridge = 50.0;
    int iMinimumDaysToCalcUse = 3;

    DateTime datToday = new DateTime();

    //int iCs1 = 99999;
    ErrorHandler eh;
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        eh = new ErrorHandler();
    }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e)
    {
        eh = null;
    }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            TimeSpan ts = new TimeSpan();
            datToday = DateTime.Now;
            startTime = DateTime.Now;
            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
            DataTable dataTable = new DataTable(sMethodName);
            DataTable dataTable2 = new DataTable(sMethodName);
            string sRunTime = "";
            int iUnitsDeleted = 0;

            int iTest = 0;

            ProgramStartup();
            if (iTest == 0)
            {
                // Section One: Get All MP Units, check if S2000 Workfile update needed
                try
                {
                    sqlConn.Open();
                    odbcConn.Open();

                    dataTable = GetUnitIDsFromMPUHD();
                    iUnitsDeleted = CheckForUnitsDeletedFromPrintFleet(dataTable);

                    // COMMENT FOR FASTER TESTING                
                    dataTable = GetUnitsFromPrintFleet();
                    // Purpose: Sync Today's Print Fleet to 400 Workfile
                    // COMMENT FOR FASTER TESTING                
                    UpdUnitsInMasterWorkfile(dataTable);
                    UpdSilentUnits();
                    // Purpose: Clear yesterday's picks for toner to be shipped
                    ClearVerdictFlags();
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    sqlConn.Close();
                    sqlReader.Close();
                    odbcCmd.Dispose();
                    odbcConn.Close();
                    odbcReader.Close();
                }
            }
            // Section 2: Print Fleet a) Meter Scans   b) Toner Scans  c) Last Toner Order
            try
            {
                sqlConn.Open();
                odbcConn.Open();
                dataTable = GetUnitsFromMasterWorkfile();
                // Purpose: Get Print Fleet Meter Readings
                WfUpd_Meters(dataTable);
                // Purpose: Get Print Fleet Toner Levels
                WfUpd_TonerLevels(dataTable);

                // Purpose: Get last toner order date, part, qty
                dataTable2 = GetLastTonerOrders();
                WfUpd_LastTonerOrders(dataTable2);
                WfUpd_DaysSinceLastOrder(dataTable);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                sqlConn.Close();
                odbcCmd.Dispose();
                odbcConn.Close();
                odbcReader.Close();
            }

            if (iTest == 0)
            {
                // Section 3: Calculate Days to Empty Cartridges from Toner History    
                try
                {
                    odbcConn.Open();
                    dataTable = GetUnitsFromMasterWorkfile();
                    // This one seemed longer (only 5 minutes or so in test) 
                    GetPastTonerLevels(dataTable);
                    GetLowCartridgeForEachUnit(dataTable);

                    SetRankHigh();
                    dataTable = CalcRankToRunOut();
                    SaveRankToRunOut(dataTable);
                    dataTable = GetHeaderKey();
                    GetPageCountsForAllUnits(dataTable);
                    // Replaced by 2nd Robot Job Call to Charts.aspx in RPGLE Pgm WEB06PFDAY
                    // CreateChartsForAllUnits(dataTable);

                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                    odbcConn.Close();
                    odbcReader.Close();
                }
            }
            endTime = DateTime.Now;
            ts = endTime - startTime;

            sRunTime = ts.Minutes.ToString() + " Min... " + ts.Seconds.ToString() + " Sec...";

            Response.Write("<br /> Run Time: " + sRunTime + "  -- Deleted... " + iUnitsDeleted.ToString());
        }
    }
    // ========================================================================
    protected void ProgramStartup()
    {
        // Server.ScriptTimeout = 120;
        // odbcCmd.CommandTimeout = 600; // 10 minutes

        // Set here once for all page SQLs
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);  // Create connection object here

        sConnectionString = ConfigurationManager.ConnectionStrings["PrintFleetConn"].ConnectionString;
        sqlConn = new SqlConnection(sConnectionString);

    }
    // ============================================================================
    public DataTable GetUnitsFromPrintFleet()
    {
        // Get Any Print Fleet Units Not Loaded
        // -----------------------------------
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        sqlCmd = new SqlCommand();

        try
        {

            sSql = "select" +
                 " h.htsCs1" +
                ", h.htsName" +
                ", d.customerid" +
                ", d.assetnumber" +
                ", d.ipaddress" +
                ", d.deviceid" +
                ", d.devicename" +
                ", d.hrdevicedescr" +
                ", d.serialnumber" +
                ", d.pagecountweek" +
                ", d.pagecountmonth" +
                ", d.lastactivedate" +
                " from device d, htsCustXref h" +
                " where (d.customerid = h.pfCs1 or d.customerid = h.pfCs1*-1)" +
                " and  ((h.htsCs1 <> 80446 and d.serialnumber not in ('CNBB012123', 'CNRC6BC2JD', 'CNBJN80261', 'CNBJM22849', 'CNBJR25395', 'CNBJS61980', 'CNRJN68317', 'CNB1823535'))" +
                "   or  (h.htsCs1 = 80446 and d.serialnumber in ('CNBB012123', 'CNRC6BC2JD', 'CNBJN80261', 'CNBJM22849', 'CNBJR25395', 'CNBJS61980', 'CNRJN68317', 'CNB1823535')))";

            //sSql += " and d.deviceid = '167451668665347'";

            //                " where (d.customerid = h.pfCs1" +
            //                " or d.customerid = h.pfCs1*-1)";    // include hidden units to see if just hidden
            //                sSql += " and d.deviceid = '152430534204'";
            // " and d.customerid = 497595452";  // Madix

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandTimeout = 600;
            // KOI: Sept 8th, 2011 trying to ensure it is closed before opening
            if ((sqlReader != null) && (sqlReader.IsClosed == false))
                sqlReader.Close();
            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            sqlCmd.Dispose();
        }
        // -----------------------------------
        return dataTable;
    }
    // ========================================================================
    protected void UpdUnitsInMasterWorkfile(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";

        int iHtsCs1 = 0;
        int iUnmanaged = 0;
        int iHidden = 0;
        int iMicr = 0;
        int iUnt = 0;
        int iPageCountWeek = 0;
        int iPageCountMonth = 0;
        int iDateLastActive = 0;

        string sPfCs1 = "";
        string sHtsName = "";
        string sAssetNumber = "";
        string sIpAddress = "";
        string sDeviceId = "";
        string sDeviceName = "";
        string sDeviceDesc = "";
        string sSerialNumber = "";
        string sLastActiveDate = "";

        int iNewKey = 0;
        int iRowIdx = 0;
        //DataTable dataTable = new DataTable(sMethodName);
        //DataTable htsTable = new DataTable(sMethodName);

        foreach (DataRow row in dTable.Rows)
        {
            DataTable dataTable = new DataTable(sMethodName);
            DataTable htsTable = new DataTable(sMethodName);
            iUnmanaged = 0;
            iHidden = 0;
            iMicr = 0;
            iDateLastActive = 0;

            iHtsCs1 = Int32.Parse(dTable.Rows[iRowIdx]["htsCs1"].ToString());
            iPageCountWeek = Int32.Parse(dTable.Rows[iRowIdx]["pagecountweek"].ToString());
            iPageCountMonth = Int32.Parse(dTable.Rows[iRowIdx]["pagecountmonth"].ToString());

            sDeviceId = dTable.Rows[iRowIdx]["deviceid"].ToString().Trim();
            sPfCs1 = dTable.Rows[iRowIdx]["customerid"].ToString().Trim();
            sHtsName = dTable.Rows[iRowIdx]["htsName"].ToString().Trim();

            sSerialNumber = dTable.Rows[iRowIdx]["serialnumber"].ToString().Trim();
            sSerialNumber = eh.KeyboardCharactersOnly(sSerialNumber, 25);

            sAssetNumber = dTable.Rows[iRowIdx]["assetnumber"].ToString().Trim();
            sIpAddress = dTable.Rows[iRowIdx]["ipaddress"].ToString().Trim();

            sDeviceName = dTable.Rows[iRowIdx]["devicename"].ToString().Trim();
            sDeviceName = eh.KeyboardCharactersOnly(sDeviceName, 75);

            sDeviceDesc = dTable.Rows[iRowIdx]["hrdevicedescr"].ToString().Trim();
            sDeviceDesc = eh.KeyboardCharactersOnly(sDeviceDesc, 75);

            sLastActiveDate = dTable.Rows[iRowIdx]["lastactivedate"].ToString().Trim();

            // ---------------------------------------------------
            // Check for missing units, or those where Fixed Asset or IP needs to be updated
            // ---------------------------------------------------
            try
            {

                if (sLastActiveDate != null)
                {
                    DateTime datActive = new DateTime();
                    datActive = Convert.ToDateTime(sLastActiveDate); // Feb 1st cutoff
                    iDateLastActive = Int32.Parse(datActive.ToString("yyyyMMdd"));
                }

                if (sDeviceId.Substring(0, 1) == "-")
                {
                    sDeviceId = sDeviceId.Substring(1);
                    iUnmanaged = 1;
                }

                if (sPfCs1.Substring(0, 1) == "-")
                {
                    sPfCs1 = sPfCs1.Substring(1);
                    iHidden = 1;
                }

                if (sAssetNumber.ToString().Length > 0)
                {
                    if (sAssetNumber.Substring(0, 1) == "%")
                    {
                        iMicr = 1;
                    }
                }

                sSql = "select hpf_uid, hpf_fxa, hpf_ipa, hpf_ser, hPgWeek, hPgMonth, hNotMan, hHidden, hCurMicr, hht_uid, hht_cs1, hht_cs2, hht_fxa, hht_mod, hht_ser, hht_agr, hht_agd, hDLstAct, hht_agd" +
                    " from " + sLibrary + ".MPUHD" +
                    " where hpf_uid = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@PfUid"].Value = sDeviceId;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                dataTable.Load(odbcReader);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }

            // ---------------------------------------------------
            // ADD NEW RECORD if no matching record exists (and not hidden or unmanaged)
            // ---------------------------------------------------
            if (dataTable.Rows.Count == 0)
            {
                if ((iUnmanaged == 0) && (iHidden == 0))
                {
                    string sTargetFilename = "MPUHD";
                    KeyHandler kh = new KeyHandler();
                    iNewKey = kh.MakeNewKey(sTargetFilename);

                    if (iNewKey > 0)
                    {
                        try
                        {
                            sSql = "insert into " + sLibrary + ".MPUHD (" +
                                 " HKEY" +
                                ", HHT_CS1" +
                                ", HHT_NAM" +
                                ", HPF_CS1" +
                                ", HPF_FXA" +
                                ", HPF_IPA" +
                                ", HPF_UID" +
                                ", HPF_MOD" +
                                ", HPF_DSC" +
                                ", HPF_SER" +
                                ", HPGWEEK" +
                                ", HPGMONTH" +
                                ", HNOTMAN" +
                                ", HCURMICR" +
                                ", HDLSTACT" +
                                ") VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                            odbcCmd = new OdbcCommand(sSql, odbcConn);

                            odbcCmd.Parameters.Add("@NewKey", OdbcType.Int);
                            odbcCmd.Parameters["@NewKey"].Value = iNewKey;

                            odbcCmd.Parameters.Add("@HtsCs1", OdbcType.Int);
                            odbcCmd.Parameters["@HtsCs1"].Value = iHtsCs1;

                            odbcCmd.Parameters.Add("@HtsName", OdbcType.VarChar, 30);
                            odbcCmd.Parameters["@HtsName"].Value = sHtsName;

                            odbcCmd.Parameters.Add("@PfCs1", OdbcType.VarChar, 25);
                            odbcCmd.Parameters["@PfCs1"].Value = sPfCs1;

                            odbcCmd.Parameters.Add("@PfFxa", OdbcType.VarChar, 25);
                            odbcCmd.Parameters["@PfFxa"].Value = sAssetNumber;

                            odbcCmd.Parameters.Add("@PfIpa", OdbcType.VarChar, 20);
                            odbcCmd.Parameters["@PfIpa"].Value = sIpAddress;

                            odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 25);
                            odbcCmd.Parameters["@PfUid"].Value = sDeviceId;

                            odbcCmd.Parameters.Add("@PfMod", OdbcType.VarChar, 75);
                            odbcCmd.Parameters["@PfMod"].Value = sDeviceName;

                            odbcCmd.Parameters.Add("@PfDsc", OdbcType.VarChar, 75);
                            odbcCmd.Parameters["@PfDsc"].Value = sDeviceDesc;

                            odbcCmd.Parameters.Add("@PfSer", OdbcType.VarChar, 25);
                            odbcCmd.Parameters["@PfSer"].Value = sSerialNumber;

                            odbcCmd.Parameters.Add("@PagesWeek", OdbcType.Int);
                            odbcCmd.Parameters["@PagesWeek"].Value = iPageCountWeek;

                            odbcCmd.Parameters.Add("@PagesMonth", OdbcType.Int);
                            odbcCmd.Parameters["@PagesMonth"].Value = iPageCountMonth;

                            odbcCmd.Parameters.Add("@UnManaged", OdbcType.Int);
                            odbcCmd.Parameters["@UnManaged"].Value = iUnmanaged;

                            odbcCmd.Parameters.Add("@Micr", OdbcType.Int);
                            odbcCmd.Parameters["@Micr"].Value = iMicr;

                            odbcCmd.Parameters.Add("@DateLastActive", OdbcType.Int);
                            odbcCmd.Parameters["@DateLastActive"].Value = iDateLastActive;

                            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                        }
                        catch (Exception ex)
                        {
                            // KOI: Aug 22, 2011 Column 8 bad
                            sErrValues = "PfId: " + sDeviceId +
                                " PfMod: " + sDeviceName +
                                " PfDesc: " + sDeviceDesc;

                            sErrMessage = ex.ToString();
                            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                        }
                        finally
                        {
                            odbcCmd.Dispose();
                        }
                        if (sAssetNumber != "")
                        {
                            htsTable = GetBestHtsEqpValues(sDeviceId, iHtsCs1, sAssetNumber, sSerialNumber);
                            if (htsTable.Rows.Count > 0)
                            {
                                iUnt = WfUpd_HtsEqpValues(htsTable, sDeviceId, sSerialNumber);
                            }
                        }

                    } // if you got a new key from keymast 
                } // Not inserting unmanaged or hidden units
                // ----------------------------------
            } // end if (dataTable.Rows.Count == 0) no record for Print fleet unit id exists

            else
            {
                // ---------------------------------------------------
                // ELSE MATCH WAS FOUND... CHECK IF UPDATE IS NEEDED
                // ---------------------------------------------------

                string sWorkfileHtsFxa = dataTable.Rows[0]["hHt_Fxa"].ToString().Trim();
                string sWorkfileHtsMod = dataTable.Rows[0]["hHt_Mod"].ToString().Trim();
                string sWorkfileHtsSer = dataTable.Rows[0]["hHt_Ser"].ToString().Trim();
                string sWorkfileHtsAgr = dataTable.Rows[0]["hHt_Agr"].ToString().Trim();

                string sWorkfilePfFxa = dataTable.Rows[0]["hPf_Fxa"].ToString().Trim();
                string sWorkfilePfIpa = dataTable.Rows[0]["hPf_Ipa"].ToString().Trim();
                string sWorkfilePfSer = dataTable.Rows[0]["hPf_Ser"].ToString().Trim();

                int iWorkfileHtsUid = Int32.Parse(dataTable.Rows[0]["hHt_Uid"].ToString().Trim());
                int iWorkfileHtsCs1 = Int32.Parse(dataTable.Rows[0]["hHt_Cs1"].ToString().Trim());
                int iWorkfileHtsCs2 = Int32.Parse(dataTable.Rows[0]["hHt_Cs2"].ToString().Trim());
                int iWorkfileHtsDateOn = Int32.Parse(dataTable.Rows[0]["hHt_AgD"].ToString().Trim());
                int iWorkfilePfPgWeek = Int32.Parse(dataTable.Rows[0]["hPGWEEK"].ToString().Trim());
                int iWorkfilePfPgMonth = Int32.Parse(dataTable.Rows[0]["hPGMONTH"].ToString().Trim());
                int iWorkfilePfNotManaged = Int32.Parse(dataTable.Rows[0]["hNotMan"].ToString().Trim());
                int iWorkfilePfHidden = Int32.Parse(dataTable.Rows[0]["hHidden"].ToString().Trim());
                int iWorkfilePfCurMicr = Int32.Parse(dataTable.Rows[0]["hCurMicr"].ToString().Trim());
                int iWorkfileDateLastActive = Int32.Parse(dataTable.Rows[0]["hDLstAct"].ToString().Trim());
                int iWorkfileHtsAgD = Int32.Parse(dataTable.Rows[0]["hHt_AgD"].ToString().Trim());

                // ------------------------------------------------------
                // Update With PRINT FLEET Values (ONLY IF DIFFERENT TODAY) 
                // ------------------------------------------------------ 
                if ((sWorkfilePfFxa != sAssetNumber) ||
                    (sWorkfilePfIpa != sIpAddress) ||
                    (sWorkfilePfSer != sSerialNumber) ||
                    (iWorkfileHtsCs1 != iHtsCs1) ||
                    (iWorkfilePfPgWeek != iPageCountWeek) ||
                    (iWorkfilePfPgMonth != iPageCountMonth) ||
                    (iWorkfilePfNotManaged != iUnmanaged) ||
                    (iWorkfilePfHidden != iHidden) ||
                    (iWorkfileDateLastActive != iDateLastActive) ||
                    (iWorkfilePfCurMicr != iMicr))
                {
                    try
                    {
                        sSql = "update " + sLibrary + ".MPUHD set" +
                             " HPF_FXA = ?" +
                            ", HPF_IPA = ?" +
                            ", HPF_SER = ?" +
                            ", HHT_NAM = ?" +
                            ", HHT_CS1 = ?" +
                            ", HPGWEEK = ?" +
                            ", HPGMONTH = ?" +
                            ", HNOTMAN = ?" +
                            ", HHIDDEN = ?" +
                            ", HCURMICR = ?" +
                            ", HDLSTACT = ?" +
                            " where HPF_UID = ?";

                        odbcCmd = new OdbcCommand(sSql, odbcConn);

                        odbcCmd.Parameters.Add("@PfFxa", OdbcType.VarChar, 25);
                        odbcCmd.Parameters["@PfFxa"].Value = sAssetNumber;

                        odbcCmd.Parameters.Add("@PfIpa", OdbcType.VarChar, 20);
                        odbcCmd.Parameters["@PfIpa"].Value = sIpAddress;

                        odbcCmd.Parameters.Add("@PfSer", OdbcType.VarChar, 25);
                        odbcCmd.Parameters["@PfSer"].Value = sSerialNumber;

                        odbcCmd.Parameters.Add("@HtsNam", OdbcType.VarChar, 30);
                        odbcCmd.Parameters["@HtsNam"].Value = sHtsName;

                        odbcCmd.Parameters.Add("@HtsCs1", OdbcType.Int);
                        odbcCmd.Parameters["@HtsCs1"].Value = iHtsCs1;

                        odbcCmd.Parameters.Add("@PagesWeek", OdbcType.Int);
                        odbcCmd.Parameters["@PagesWeek"].Value = iPageCountWeek;

                        odbcCmd.Parameters.Add("@PagesMonth", OdbcType.Int);
                        odbcCmd.Parameters["@PagesMonth"].Value = iPageCountMonth;

                        odbcCmd.Parameters.Add("@UnManaged", OdbcType.Int);
                        odbcCmd.Parameters["@UnManaged"].Value = iUnmanaged;

                        odbcCmd.Parameters.Add("@Hidden", OdbcType.Int);
                        odbcCmd.Parameters["@Hidden"].Value = iHidden;

                        odbcCmd.Parameters.Add("@Micr", OdbcType.Int);
                        odbcCmd.Parameters["@Micr"].Value = iMicr;

                        odbcCmd.Parameters.Add("@LastActive", OdbcType.Int);
                        odbcCmd.Parameters["@LastActive"].Value = iDateLastActive;

                        odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 25);
                        odbcCmd.Parameters["@PfUid"].Value = sDeviceId;

                        odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                    }
                    catch (Exception ex)
                    {
                        // KOI: Aug 22 2011 Contains null characters
                        sErrValues = " PFUid: " + sDeviceId +
                            " Fxa: " + sAssetNumber +
                            " IP: " + sIpAddress +
                            " Ser: " + sSerialNumber +
                            " HtsName: " + sHtsName +
                            " Cs1: " + iHtsCs1.ToString() +
                            " PgCtWk: " + iPageCountWeek.ToString() +
                            " PgCtMn: " + iPageCountMonth.ToString() +
                            " UnMan: " + iUnmanaged.ToString() +
                            " Hid: " + iHidden.ToString() +
                            " Mic: " + iMicr.ToString() +
                            " LastAct: " + iDateLastActive.ToString();

                        sErrMessage = ex.ToString();
                        eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                    }
                    finally
                    {
                        odbcCmd.Dispose();
                    }
                }

                // ---------------------------------------------------
                // Check if Workfile Needs Update With HTS Equipment Values
                // --------------------------------------------------- 

                int iHtsUid = 0;
                int iHtsDat = 0;
                int iHtsCs2 = 0;

                string sHtsPrt = "";
                string sHtsSer = "";
                string sHtsFxa = "";
                string sHtsAgr = "";

                if (sAssetNumber != "")
                {
                    htsTable = GetBestHtsEqpValues(sDeviceId, iHtsCs1, sAssetNumber, sSerialNumber);

                    if (htsTable.Rows.Count > 0)
                    {
                        sHtsAgr = htsTable.Rows[0]["Agr"].ToString().Trim();
                        iHtsUid = Int32.Parse(htsTable.Rows[0]["Unt"].ToString());
                        iHtsCs2 = Int32.Parse(htsTable.Rows[0]["Loc"].ToString());
                        iHtsDat = Int32.Parse(htsTable.Rows[0]["DateOnAgr"].ToString());
                        sHtsPrt = htsTable.Rows[0]["Prt"].ToString().Trim();
                        sHtsSer = htsTable.Rows[0]["Ser"].ToString().Trim();
                        sHtsFxa = htsTable.Rows[0]["Fxa"].ToString().Trim();

                        // If workfile needs to be updated...
                        if ((iWorkfileHtsUid != iHtsUid) ||
                            (sWorkfileHtsFxa != sHtsFxa) ||
                            (sWorkfileHtsMod != sHtsPrt) ||
                            (sWorkfileHtsSer != sHtsSer) ||
                            (sWorkfileHtsAgr != sHtsAgr) ||
                            (iWorkfileHtsAgD != iHtsDat) ||
                            (iWorkfileHtsCs2 != iHtsCs2))
                        {
                            // UpdWorkfileWithHtsValues()
                            if (htsTable.Rows.Count > 0)
                                iHtsUid = WfUpd_HtsEqpValues(htsTable, sDeviceId, sSerialNumber);
                        }
                    }
                }
            }
            // -------------------------------------------------------------------
            iRowIdx++;
        }
    }
    // ========================================================================
    protected DataTable GetBestHtsEqpValues(string pfUid, int htsCs1, string pfFxa, string pfSer)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            pfFxa = pfFxa.ToUpper();

            if (pfFxa.Trim().Length > 15)
                pfFxa = pfFxa.Substring(0, 15);
            // KOI: Aug 22 2011: Result of SELECT more than one row -- changed to max to get only one valid one
            // Dec 6th, 2011 Added ECNTYP as MP to narrow return
            sSql = "select cesys# as Unt, ceprt# as Prt, ceser# as Ser, cefaa as Fxa, cercd as Loc," +
                " ifnull((select max(eccntr) from " + sLibrary + ".eqpcontr where ecsys# = c.cesys# and ECNTYP = 'MP'), '') as Agr," +
                " ifnull((select max(daton) from " + sLibrary + ".eqpcontr where ecsys# = c.cesys# and ECNTYP = 'MP'), 0) as DateOnAgr" +
                " from " + sLibrary + ".CUSEQUIP c" +
                " where cernr = ?" +
                " and cefaa = ?" +
                " and cefaa <> ''" +
                " order by Agr desc";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@HtsCs1", OdbcType.Int);
            odbcCmd.Parameters["@HtsCs1"].Value = htsCs1;

            odbcCmd.Parameters.Add("@PfFxa", OdbcType.VarChar, 25);
            odbcCmd.Parameters["@PfFxa"].Value = pfFxa;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            string sHtsAgr = "";
            string sHtsSer = "";
            string sHtsSerMatch = "N";
            string sHtsAgrFound = "N";
            int iBestRow = -1;

            int iRowIdx = 0;

            if (dataTable.Rows.Count > 0)
            {

                // SORTED BY AGR desc (agr matches will come first, if multiple- newest first)
                foreach (DataRow row in dataTable.Rows)
                {
                    if ((sHtsAgrFound == "N") || (sHtsSerMatch == "N")) // Disregard all records if both agr and serial match are found
                    {
                        // Check for agreement number
                        sHtsAgr = dataTable.Rows[iRowIdx]["Agr"].ToString().Trim();
                        sHtsSer = dataTable.Rows[iRowIdx]["Ser"].ToString().Trim();
                        // Level 1) Agr Found? Go no deeper
                        if ((sHtsAgr != null) && (sHtsAgr != ""))
                        {
                            sHtsAgrFound = "Y";
                            iBestRow = iRowIdx;
                            if (sHtsSer == pfSer)
                                sHtsSerMatch = "Y";
                        }
                        else
                        {
                            // Level 2) Agr Trumps serial match No Agr found yet, no serial match? keep looking
                            if ((sHtsAgrFound == "N") && (sHtsSerMatch == "N"))
                            {
                                if (sHtsSer == pfSer)
                                {
                                    sHtsSerMatch = "Y";
                                    iBestRow = iRowIdx;
                                }
                                // No preference? Save each subsequent row as "best"
                                else
                                    iBestRow = iRowIdx;
                            }
                        }
                    }
                    iRowIdx++;
                }
                if (iBestRow >= 0)
                {
                    // Remove all rows except "best row" 
                    int iDelIdx = dataTable.Rows.Count - 1;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (iDelIdx != iBestRow)
                        {
                            dataTable.Rows[iDelIdx].Delete();
                        }
                        iDelIdx--;
                    }
                    dataTable.AcceptChanges();
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
        return dataTable;
    }
    // ========================================================================
    // going away (see below) 
    protected int UpdWorkfileWithCusequipEqpcontrValues(DataTable dTable, string pfUid, string pfSer)
    {
        int iUnt = 0;
        // ---------------------------------------------- 
        if (dTable.Rows.Count > 0)
        {
            int iRowIdx = 0;
            int iLoc = 0;
            string sPrt = "";
            string sSer = "";
            string sFxa = "";
            pfSer = pfSer.ToUpper();

            try
            {
                string sHighestSerialMatch = "N";
                string sHtsSerial = "";

                foreach (DataRow row in dTable.Rows)
                {
                    // dTable sorted by highest unit (most recent) first, matching serial trumps high unit
                    sHtsSerial = dTable.Rows[iRowIdx]["Ser"].ToString().Trim();
                    if (sHighestSerialMatch == "N")
                    {
                        if (sHtsSerial == pfSer)
                        {
                            sHighestSerialMatch = "Y";
                        }
                        if ((iRowIdx == 0) || (sHighestSerialMatch == "Y"))
                        {
                            iUnt = Int32.Parse(dTable.Rows[iRowIdx]["Unt"].ToString());
                            iLoc = Int32.Parse(dTable.Rows[iRowIdx]["Loc"].ToString());
                            sPrt = dTable.Rows[iRowIdx]["Prt"].ToString().Trim();
                            sSer = dTable.Rows[iRowIdx]["Ser"].ToString().Trim();
                            sFxa = dTable.Rows[iRowIdx]["Fxa"].ToString().Trim();
                        }
                    }
                    iRowIdx++;
                }
                // --------------------
                sSql = "update " + sLibrary + ".MPUHD set" +
                     " HHT_UID = ?" +
                    ", HHT_CS2 = ?" +
                    ", HHT_MOD = ?" +
                    ", HHT_SER = ?" +
                    ", HHT_FXA = ?" +
                    " where HPF_UID = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@HtsUid", OdbcType.Int);
                odbcCmd.Parameters["@HtsUid"].Value = iUnt;

                odbcCmd.Parameters.Add("@HtsCs2", OdbcType.Int);
                odbcCmd.Parameters["@HtsCs2"].Value = iLoc;

                odbcCmd.Parameters.Add("@HtsPrt", OdbcType.VarChar, 30);
                odbcCmd.Parameters["@HtsPrt"].Value = sPrt;

                odbcCmd.Parameters.Add("@HtsSer", OdbcType.VarChar, 30);
                odbcCmd.Parameters["@HtsSer"].Value = sSer;

                odbcCmd.Parameters.Add("@HtsFxa", OdbcType.VarChar, 30);
                odbcCmd.Parameters["@HtsFxa"].Value = sFxa;

                odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 30);
                odbcCmd.Parameters["@PfUid"].Value = pfUid;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            // ----------------------------------------------
        }
        return iUnt;
    }
    // ========================================================================
    protected int WfUpd_HtsEqpValues(DataTable dTable, string pfUid, string pfSer)
    {
        int iUnt = 0;
        int iLoc = 0;
        int iDat = 0;
        string sPrt = "";
        string sSer = "";
        string sFxa = "";
        string sAgr = "";
        pfSer = pfSer.ToUpper();

        try
        {
            iUnt = Int32.Parse(dTable.Rows[0]["Unt"].ToString());
            iLoc = Int32.Parse(dTable.Rows[0]["Loc"].ToString());
            iDat = Int32.Parse(dTable.Rows[0]["DateOnAgr"].ToString());
            sPrt = dTable.Rows[0]["Prt"].ToString().Trim();
            sSer = dTable.Rows[0]["Ser"].ToString().Trim();
            sFxa = dTable.Rows[0]["Fxa"].ToString().Trim();
            sAgr = dTable.Rows[0]["Agr"].ToString().Trim();

            // --------------------
            sSql = "update " + sLibrary + ".MPUHD set" +
                 " HHT_UID = ?" +
                ", HHT_CS2 = ?" +
                ", HHT_MOD = ?" +
                ", HHT_SER = ?" +
                ", HHT_FXA = ?" +
                ", HHT_AGR = ?" +
                ", HHT_AGD = ?" +
                " where HPF_UID = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@HtsUid", OdbcType.Int);
            odbcCmd.Parameters["@HtsUid"].Value = iUnt;

            odbcCmd.Parameters.Add("@HtsCs2", OdbcType.Int);
            odbcCmd.Parameters["@HtsCs2"].Value = iLoc;

            odbcCmd.Parameters.Add("@HtsPrt", OdbcType.VarChar, 30);
            odbcCmd.Parameters["@HtsPrt"].Value = sPrt;

            odbcCmd.Parameters.Add("@HtsSer", OdbcType.VarChar, 30);
            odbcCmd.Parameters["@HtsSer"].Value = sSer;

            odbcCmd.Parameters.Add("@HtsFxa", OdbcType.VarChar, 30);
            odbcCmd.Parameters["@HtsFxa"].Value = sFxa;

            odbcCmd.Parameters.Add("@HtsAgr", OdbcType.VarChar, 8);
            odbcCmd.Parameters["@HtsAgr"].Value = sAgr;

            odbcCmd.Parameters.Add("@HtsAgrDat", OdbcType.Int);
            odbcCmd.Parameters["@HtsAgrDat"].Value = iDat;

            odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 30);
            odbcCmd.Parameters["@PfUid"].Value = pfUid;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iUnt;
    }
    // ============================================================================
    public DataTable GetUnitsFromMasterWorkfile()
    {
        // Get Each Active Unit For Meter and Toner Updates
        // -----------------------------------
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select" +
                " hKey, hPf_Uid, hCurMicr" +
                " from " + sLibrary + ".MPUHD" +
                " where hNotMan = 0";

            //            sSql += " and hKey = 1893"; // 4848 4624 4370

            // " and hHidden = 0" +
            //" and hSilent = 0";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -----------------------------------
        return dataTable;
    }

    // ============================================================================
    public string GetHtsModel(string pfUid)
    {
        string sModel = "";

        try
        {
            sSql = "select" +
                " hht_mod" +
                " from " + sLibrary + ".MPUHD" +
                " where hPf_Uid = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@PfUid", OdbcType.VarChar, 50);
            odbcCmd.Parameters["@PfUid"].Value = pfUid;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            if (odbcReader.HasRows)
            {
                while (odbcReader.Read())
                {
                    sModel = odbcReader["hht_mod"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -----------------------------------
        return sModel;
    }
    // ========================================================================
    protected void WfUpd_Meters(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iRowIdx = 0;
        int iWorkfileKey = 0;
        int iPriorDate = 0;
        int iDateToUpdate = 0;
        int iStartDate = 0;
        int iCurrMicr = 0;

        int iManyDaysBack = 0;
        string sManyDaysBack = "";

        string sDateBefore = "";
        string sDateToUpdate = "";
        string sDateAfter = "";
        string sPfUid = "";
        string sDat = "";
        string sValidValueFound = "";
        int iCounter = 0; // counter is just for my view, it can be deleted 

        DateTime datToday = new DateTime();
        DateTime datTemp = new DateTime();
        DateTime datToLoad = new DateTime();
        datToday = DateTime.Now;
        //datToday = Convert.ToDateTime("2010-01-06 01:01:01.000"); // Feb 1st cutoff
        //datToday = Convert.ToDateTime(sEndDate); // Feb 1st cutoff

        // Data Table holds all active models
        iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iWorkfileKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString());
            iCurrMicr = Int32.Parse(dTable.Rows[iRowIdx]["hCurMicr"].ToString());
            sPfUid = dTable.Rows[iRowIdx]["hPf_Uid"].ToString().Trim();

            // --------------------------------------------------------            
            // UPDATE END METERS FOR UNIT
            // --------------------------------------------------------            
            // determine last loaded day
            iStartDate = GetNextDailyMeterDate(iWorkfileKey);
            if (iStartDate == 0)
                iStartDate = GetFirstPrintFleetMeterDate(sPfUid);

            if (iStartDate > 0)
            {
                sDat = iStartDate.ToString();
                datToLoad = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");

                iCounter = 0;
                while (datToLoad.Date < datToday.Date)
                {
                    // Feb 15th, 2012 for bad units talking in fits and starts i.e. 2315
                    datTemp = datToLoad.AddDays(-40);
                    iManyDaysBack = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                    sManyDaysBack = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    datTemp = datToLoad.AddDays(-1);
                    iPriorDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                    sDateBefore = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    datTemp = datTemp.AddDays(1);
                    iDateToUpdate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                    sDateToUpdate = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    datTemp = datTemp.AddDays(1);
                    sDateAfter = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    // -- Get/Set End Meters
                    //dataTable = GetEndMeters(sPfUid, sDateToUpdate, sDateAfter);
                    dataTable = GetEndMeters(sPfUid, sManyDaysBack, sDateAfter);
                    sValidValueFound = "NO";
                    if (dataTable.Rows.Count > 0)
                        sValidValueFound = SetEndMeters(dataTable, iDateToUpdate, iWorkfileKey);
                    if (sValidValueFound == "NO")
                        SetWithPriorDateMeters(iPriorDate, iDateToUpdate, iWorkfileKey);
                    // -----------------------
                    datToLoad = datToLoad.AddDays(1);
                    iCounter++;
                }
            }
            // --------------------------------------------------------            
            iRowIdx++;
        }
    }

    // ========================================================================
    protected void WfUpd_TonerLevels(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iRowIdx = 0;
        int iWorkfileKey = 0;
        int iPriorDate = 0;
        int iDateToUpdate = 0;
        int iStartDate = 0;
        int iCurrMicr = 0;

        string sDateBefore = "";
        string sDateToUpdate = "";
        string sDateAfter = "";
        string sPfUid = "";
        string sDat = "";
        string sValidValueFound = "";
        int iCounter = 0; // counter is just for my view, it can be deleted 

        DateTime datToday = new DateTime();
        DateTime datTemp = new DateTime();
        DateTime datToLoad = new DateTime();
        datToday = DateTime.Now;
        //datToday = Convert.ToDateTime("2010-01-06 01:01:01.000"); // Feb 1st cutoff
        //datToday = Convert.ToDateTime(sEndDate); // Feb 1st cutoff

        // Data Table holds all active models
        iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iWorkfileKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString());
            iCurrMicr = Int32.Parse(dTable.Rows[iRowIdx]["hCurMicr"].ToString());
            sPfUid = dTable.Rows[iRowIdx]["hPf_Uid"].ToString().Trim();

            // --------------------------------------------------------            
            // UPDATE TONER LEVELS FOR UNIT
            // --------------------------------------------------------            
            // determine last loaded day
            iStartDate = GetNextDailyTonerDate(iWorkfileKey);
            if (iStartDate == 0)
                iStartDate = GetFirstPrintFleetTonerDate(sPfUid);

            if (iStartDate > 0)
            {
                sDat = iStartDate.ToString();
                datToLoad = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");

                iCounter = 0;
                while (datToLoad.Date < datToday.Date)
                {
                    datTemp = datToLoad.AddDays(-1);
                    iPriorDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                    sDateBefore = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    datTemp = datTemp.AddDays(1);
                    iDateToUpdate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                    sDateToUpdate = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    datTemp = datTemp.AddDays(1);
                    sDateAfter = datTemp.ToString("yyyy") + "-" + datTemp.ToString("MM") + "-" + datTemp.ToString("dd");

                    // -- Get/Set Toner Levels
                    dataTable = GetPrintFleetTonerLevelsForDay(sPfUid, sDateToUpdate, sDateAfter);
                    sValidValueFound = "NO";
                    if (dataTable.Rows.Count > 0)
                        sValidValueFound = SetTonerLevels(dataTable, iDateToUpdate, iWorkfileKey, iCurrMicr);
                    if (sValidValueFound == "NO")
                        SetWithPriorDateTonerLevels(iPriorDate, iDateToUpdate, iWorkfileKey);
                    // -----------------------
                    datToLoad = datToLoad.AddDays(1);
                    iCounter++;
                }
            }
            // --------------------------------------------------------            
            iRowIdx++;
        }
    }

    // ========================================================================
    protected DataTable GetLastTonerOrders()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddDays(-7); /* -4 -7 -14 -120 */
        int iStartDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        try
        {
            sSql = "select distinct HKEY, HHT_UID, HCURMICR, OD1PRTOEM, OD1PRTQTY, OD1PRTDSC, MAX(OH1DAT) as LastOrder " +
                " from " + sLibrary + ".Sn3OrdHd1, " + sLibrary + ".Sn4OrdDt1, " + sLibrary + ".MPUHD, " + sLibrary + ".SVRTICK" +
                " where OH1Key = OD1Key" +
                " and (OH1UNT = HHT_UID" +
                "   or OH1MPK = HKEY)" +
                " and OH1CTR = TCCENT" +
                " and OH1TCK = TICKNR" +
                " and PROCCD <> 'V'" +
                " and SUBSTRING(OD1PRTDSC, 1, 6) = 'TONER,'" +
                " and OH1DAT >= ?" +
                " group by HKEY, HHT_UID, HCURMICR, OD1PRTOEM, OD1PRTQTY, OD1PRTDSC" +
                " order by HKEY, MAX(OH1DAT) desc"; // descending so only most recent order updates qty on hand

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.CommandTimeout = 600; // 10 minutes

            odbcCmd.Parameters.Add("@Yesterday", OdbcType.Int);
            odbcCmd.Parameters["@Yesterday"].Value = iStartDate;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
        return dataTable;
    }
    // ========================================================================
    protected int[] GetCartridgeQtyDatNul(int key, int seq)
    {
        int[] iaQtyDatNul = { 0, 0, 0 };

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select CQTYONHAND, CDATEORDER " +
                " from " + sLibrary + ".MPUCAR" +
                " where CKEY = ?" +
                " and CSEQ = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = seq;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                if (int.TryParse(dataTable.Rows[0]["cQtyOnHand"].ToString(), out iaQtyDatNul[0]) == false)
                    iaQtyDatNul[0] = 0;
                if (int.TryParse(dataTable.Rows[0]["cDateOrder"].ToString(), out iaQtyDatNul[1]) == false)
                    iaQtyDatNul[1] = 0;
            }
            else
                iaQtyDatNul[2] = 1;
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
        return iaQtyDatNul;
    }
    // ========================================================================
    protected void AddCartridgeRec(int key, int seq)
    {
        try
        {
            sSql = "Insert into " + sLibrary + ".MPUCAR " +
                " (cKey, cSeq, cDaysToEnd)" +
                " Values(?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = seq;

            odbcCmd.Parameters.Add("@DaysToEnd", OdbcType.Double);
            odbcCmd.Parameters["@DaysToEnd"].Value = 99999;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
    }
    // ========================================================================
    protected void WfUpd_LastTonerOrders(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iKey = 0;
        int iUnt = 0;
        int iQty = 0;
        int iDat = 0;
        int iMic = 0;
        string sPrt = "";
        string sDsc = "";
        int iColorSeq = 0;
        int[] iaQtyDatNul = { 0, 0, 0 };
        int iCurrQtyOnHand = 0;
        int iLastOrderDate = 0;
        int iCartRecNeeded = 0;
        int iNewQtyOnHand = 0;
        int iNewQohPos = 0;

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddDays(-1);
        int iYesterday = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        int iRowIdx = 0;

        // Data Table holds all active models
        iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            // distinct HKEY, HHT_UID, HCURMICR, OD1PRTOEM, OD1PRTQTY, OD1PRTDSC, MAX(OH1DAT) as LastOrder

            if (int.TryParse(dTable.Rows[iRowIdx]["hKey"].ToString(), out iKey) == false)
                iKey = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["hHT_Uid"].ToString(), out iUnt) == false)
                iUnt = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["OD1PRTQTY"].ToString(), out iQty) == false)
                iQty = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["LastOrder"].ToString(), out iDat) == false)
                iDat = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["HCURMICR"].ToString(), out iMic) == false)
                iMic = 0;
            sPrt = dTable.Rows[iRowIdx]["OD1PRTOEM"].ToString();
            sDsc = dTable.Rows[iRowIdx]["OD1PRTDSC"].ToString();

            // Dual Cartridge 
            if (sPrt == "CC530AD")
                iQty = 2 * iQty;

            if (sDsc.Substring(0, 9) == "TONER, BL")
            {
                if (iMic == 0)
                    iColorSeq = 1;
                else
                    iColorSeq = 2;
            }
            else if (sDsc.Substring(0, 11) == "TONER, CYAN")
                iColorSeq = 3;
            else if (sDsc.Substring(0, 14) == "TONER, MAGENTA")
                iColorSeq = 4;
            else if (sDsc.Substring(0, 10) == "TONER, YEL")
                iColorSeq = 5;

            iaQtyDatNul = GetCartridgeQtyDatNul(iKey, iColorSeq);
            iCurrQtyOnHand = iaQtyDatNul[0];
            iLastOrderDate = iaQtyDatNul[1];
            iCartRecNeeded = iaQtyDatNul[2];

            // Stop additional runs on same day from ruining Qty on hand
            // iDat = SQL Order Date Found (update only if date found more recent than file date) 
            if (iDat > iLastOrderDate)
            {
                iNewQtyOnHand = iCurrQtyOnHand + iQty;
                if (iNewQtyOnHand < 0)
                    iNewQohPos = 0;
                else
                    iNewQohPos = iNewQtyOnHand;

                try
                {
                    if (iCartRecNeeded == 1)
                        AddCartridgeRec(iKey, iColorSeq);

                    sSql = "Update " + sLibrary + ".MPUCAR set" +
                         " CDATEORDER = ?" +
                        ", CPARTNAME = ?" +
                        ", CQTYORDER = ?" +
                        ", CQTYONHAND = ?" +
                        ", CQOHPOS = ?" +
                        " where CKEY = ?" +
                        " and CSEQ = ?";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@Date", OdbcType.Int);
                    odbcCmd.Parameters["@Date"].Value = iDat;

                    odbcCmd.Parameters.Add("@Part", OdbcType.VarChar, 15);
                    odbcCmd.Parameters["@Part"].Value = sPrt;

                    odbcCmd.Parameters.Add("@Qty", OdbcType.Int);
                    odbcCmd.Parameters["@Qty"].Value = iQty;

                    odbcCmd.Parameters.Add("@QtyOnHand", OdbcType.Int);
                    odbcCmd.Parameters["@QtyOnHand"].Value = iNewQtyOnHand;

                    odbcCmd.Parameters.Add("@QohPos", OdbcType.Int);
                    odbcCmd.Parameters["@QohPos"].Value = iNewQohPos;

                    odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                    odbcCmd.Parameters["@Key"].Value = iKey;

                    odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
                    odbcCmd.Parameters["@Seq"].Value = iColorSeq;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }
            }
            // --------------------------------------------------------            
            iRowIdx++;
        }
    }
    // ========================================================================
    protected void WfUpd_DaysSinceLastOrder(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iRowIdx = 0;
        int iWorkfileKey = 0;

        // Data Table holds all active models
        iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iWorkfileKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString());

            // --------------------------------------------------------            
            // CALCULATE DAYS TO LAST SHIP DATE SHIP DATE FOR EACH CARTRIDGE
            // --------------------------------------------------------            
            dataTable = GetLastShipDateForUnitCartridges(iWorkfileKey);
            CalcDaysSinceLastShipment(dataTable);

            // --------------------------------------------------------            
            iRowIdx++;
        }
    }
    // ========================================================================
    protected DataTable GetEndMeters(string pfUid, string dateToStartReading, string dateAfter)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select top 10" +
                " PrintMono" +
                ", PrintColor" +
                ", CopierMono" +
                ", CopierColor" +
                ", FaxCount" +
                ", MonoCount" +
                ", ColorCount" +
                ", LifeCount" +
                ", ScanTime" +
                " from metric_pagecount" +
                " where DeviceId = @PfUid" +
                " and ScanTime > @StartDate" +
                " and ScanTime < @EndDate" +
                " and (PrintMono > 0 or PrintColor > 0 or CopierMono > 0 or CopierColor > 0 or FaxCount > 0 or LifeCount > 0)" +
                " order by ScanTime desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            // Koi Oct 24, 2011 Error Log said this was timing out here
            sqlCmd.CommandTimeout = 180;

            sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@PfUid"].Value = pfUid;

            sqlCmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@StartDate"].Value = dateToStartReading; // now 10 days back

            sqlCmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@EndDate"].Value = dateAfter;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dataTable;

    }
    // ========================================================================
    protected DataTable GetEndMetersOrig(string pfUid, string dateToRead, string dateAfter)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select" +
                " PrintMono" +
                ", PrintColor" +
                ", CopierMono" +
                ", CopierColor" +
                ", FaxCount" +
                ", MonoCount" +
                ", ColorCount" +
                ", LifeCount" +
                ", ScanTime" +
                " from metric_pagecount" +
                " where DeviceId = @PfUid" +
                " and ScanTime > @StartDate" +
                " and ScanTime < @EndDate" +
                " and (PrintMono > 0 or PrintColor > 0 or CopierMono > 0 or CopierColor > 0 or FaxCount > 0 or LifeCount > 0)" +
                " order by ScanTime desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            // Koi Oct 24, 2011 Error Log said this was timing out here
            sqlCmd.CommandTimeout = 180;

            sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@PfUid"].Value = pfUid;

            sqlCmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@StartDate"].Value = dateToRead;

            sqlCmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@EndDate"].Value = dateAfter;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dataTable;

    }
    // ========================================================================
    protected string SetEndMeters(DataTable dTable, int DataDate, int WorkfileKey)
    {
        DateTime datTemp = new DateTime();

        int iMonoCount = 0;
        int iColorCount = 0;
        int iLifeCount = 0;
        int iPrintMono = 0;
        int iPrintColor = 0;
        int iCopierMono = 0;
        int iCopierColor = 0;
        int iFaxCount = 0;
        int iRowIdx = 0;
        int hMono1 = 0;
        int hMono2 = 0;
        int hLife = 0;
        int iLastScanDate = 0;

        string sScanTime = "";
        string sSearchSatisfactory = "NO";

        if (dTable.Rows.Count > 0)
        {
            iRowIdx = 0;
            foreach (DataRow row in dTable.Rows)
            {
                if (sSearchSatisfactory != "YES")
                {
                    hMono1 = Int32.Parse(dTable.Rows[iRowIdx]["PrintMono"].ToString());
                    hMono2 = Int32.Parse(dTable.Rows[iRowIdx]["MonoCount"].ToString());
                    hLife = Int32.Parse(dTable.Rows[iRowIdx]["LifeCount"].ToString());

                    if (hMono1 > 0 || hMono2 > 0 || hLife > 0)
                    {
                        if (iPrintMono == 0)
                            iPrintMono = Int32.Parse(dTable.Rows[iRowIdx]["PrintMono"].ToString());
                        if (iPrintColor == 0)
                            iPrintColor = Int32.Parse(dTable.Rows[iRowIdx]["PrintColor"].ToString());
                        if (iCopierMono == 0)
                            iCopierMono = Int32.Parse(dTable.Rows[iRowIdx]["CopierMono"].ToString());
                        if (iCopierColor == 0)
                            iCopierColor = Int32.Parse(dTable.Rows[iRowIdx]["CopierColor"].ToString());
                        if (iFaxCount == 0)
                            iFaxCount = Int32.Parse(dTable.Rows[iRowIdx]["FaxCount"].ToString());
                        if (iMonoCount == 0)
                            iMonoCount = Int32.Parse(dTable.Rows[iRowIdx]["MonoCount"].ToString());
                        if (iColorCount == 0)
                            iColorCount = Int32.Parse(dTable.Rows[iRowIdx]["ColorCount"].ToString());
                        if (iLifeCount == 0)
                            iLifeCount = Int32.Parse(dTable.Rows[iRowIdx]["LifeCount"].ToString());
                        if (iLastScanDate == 0)
                        {
                            sScanTime = dTable.Rows[iRowIdx]["ScanTime"].ToString().Trim();
                            datTemp = Convert.ToDateTime(sScanTime);
                            iLastScanDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                        }
                    }
                    if (hMono1 > 0 && hMono2 > 0 && hLife > 0) // Found all three? leave loop!
                        sSearchSatisfactory = "YES";
                }
                iRowIdx++;
            }

            if (hMono1 > 0 || hMono2 > 0 || hLife > 0) // Done? Found ANY of the three? Satisfied...
                sSearchSatisfactory = "YES";

            if (sSearchSatisfactory == "YES")
            {
                // Update Meter Log File
                try
                {
                    sSql = "insert into " + sLibrary + ".MPUMETLG" +
                        " (MKEY" +
                        ", MDAT" +
                        ", MSCN" +
                        ", MPRTMON" +
                        ", MPRTCOL" +
                        ", MCPYMON" +
                        ", MCPYCOL" +
                        ", MFAXMON" +
                        ", MALLMON" +
                        ", MALLCOL" +
                        ", MALLTOT)" +
                        " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
                    odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

                    odbcCmd.Parameters.Add("@UpdateDate", OdbcType.Int);
                    odbcCmd.Parameters["@UpdateDate"].Value = DataDate;

                    odbcCmd.Parameters.Add("@ScanTime", OdbcType.VarChar, 25);
                    odbcCmd.Parameters["@ScanTime"].Value = sScanTime;

                    odbcCmd.Parameters.Add("@PrintMono", OdbcType.Int);
                    odbcCmd.Parameters["@PrintMono"].Value = iPrintMono;

                    odbcCmd.Parameters.Add("@PrintColor", OdbcType.Int);
                    odbcCmd.Parameters["@PrintColor"].Value = iPrintColor;

                    odbcCmd.Parameters.Add("@CopierMono", OdbcType.Int);
                    odbcCmd.Parameters["@CopierMono"].Value = iCopierMono;

                    odbcCmd.Parameters.Add("@CopierColor", OdbcType.Int);
                    odbcCmd.Parameters["@CopierColor"].Value = iCopierColor;

                    odbcCmd.Parameters.Add("@FaxCount", OdbcType.Int);
                    odbcCmd.Parameters["@FaxCount"].Value = iFaxCount;

                    odbcCmd.Parameters.Add("@MonoCount", OdbcType.Int);
                    odbcCmd.Parameters["@MonoCount"].Value = iMonoCount;

                    odbcCmd.Parameters.Add("@ColorCount", OdbcType.Int);
                    odbcCmd.Parameters["@ColorCount"].Value = iColorCount;

                    odbcCmd.Parameters.Add("@LifeCount", OdbcType.Int);
                    odbcCmd.Parameters["@LifeCount"].Value = iLifeCount;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }

                // Update Header File
                try
                {
                    sSql = "update " + sLibrary + ".MPUHD set" +
                        "  HMPRTMON = ?" +
                        ", HMPRTCOL = ?" +
                        ", HMCPYMON = ?" +
                        ", HMCPYCOL = ?" +
                        ", HMFAXMON = ?" +
                        ", HMALLMON = ?" +
                        ", HMALLCOL = ?" +
                        ", HMALLTOT = ?" +
                        ", HMLSTSCN = ?" +
                         " where HKEY = ?";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@PrintMono", OdbcType.Int);
                    odbcCmd.Parameters["@PrintMono"].Value = iPrintMono;

                    odbcCmd.Parameters.Add("@PrintColor", OdbcType.Int);
                    odbcCmd.Parameters["@PrintColor"].Value = iPrintColor;

                    odbcCmd.Parameters.Add("@CopierMono", OdbcType.Int);
                    odbcCmd.Parameters["@CopierMono"].Value = iCopierMono;

                    odbcCmd.Parameters.Add("@CopierColor", OdbcType.Int);
                    odbcCmd.Parameters["@CopierColor"].Value = iCopierColor;

                    odbcCmd.Parameters.Add("@FaxCount", OdbcType.Int);
                    odbcCmd.Parameters["@FaxCount"].Value = iFaxCount;

                    odbcCmd.Parameters.Add("@MonoCount", OdbcType.Int);
                    odbcCmd.Parameters["@MonoCount"].Value = iMonoCount;

                    odbcCmd.Parameters.Add("@ColorCount", OdbcType.Int);
                    odbcCmd.Parameters["@ColorCount"].Value = iColorCount;

                    odbcCmd.Parameters.Add("@LifeCount", OdbcType.Int);
                    odbcCmd.Parameters["@LifeCount"].Value = iLifeCount;

                    odbcCmd.Parameters.Add("@LastScan", OdbcType.Int);
                    odbcCmd.Parameters["@LastScan"].Value = iLastScanDate;

                    odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
                    odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }
            }
            // -----------------------------
        }
        return sSearchSatisfactory;
    }
    // ========================================================================
    protected void SetWithPriorDateMeters(int PriorDate, int DataDate, int WorkfileKey)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // ------------------------------
        // Get Prior Days Values
        // ------------------------------
        int iKey = 0;
        int iPrtMon = 0;
        int iPrtCol = 0;
        int iCpyMon = 0;
        int iCpyCol = 0;
        int iFaxMon = 0;
        int iAllMon = 0;
        int iAllCol = 0;
        int iAllTot = 0;

        try
        {
            sSql = "SELECT" +
                 " MKEY" +
                ", MPRTMON" +
                ", MPRTCOL" +
                ", MCPYMON" +
                ", MCPYCOL" +
                ", MFAXMON" +
                ", MALLMON" +
                ", MALLCOL" +
                ", MALLTOT" +
                " FROM " + sLibrary + ".MPUMETLG" +
                " WHERE MKEY = ?" +
                " AND MDAT = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
            odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

            odbcCmd.Parameters.Add("@PriorDate", OdbcType.Int);
            odbcCmd.Parameters["@PriorDate"].Value = PriorDate;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                iKey = Int32.Parse(dataTable.Rows[0]["mKey"].ToString());
                iPrtMon = Int32.Parse(dataTable.Rows[0]["mPrtMon"].ToString());
                iPrtCol = Int32.Parse(dataTable.Rows[0]["mPrtCol"].ToString());
                iCpyMon = Int32.Parse(dataTable.Rows[0]["mCpyMon"].ToString());
                iCpyCol = Int32.Parse(dataTable.Rows[0]["mCpyCol"].ToString());
                iFaxMon = Int32.Parse(dataTable.Rows[0]["mFaxMon"].ToString());
                iAllMon = Int32.Parse(dataTable.Rows[0]["mAllMon"].ToString());
                iAllCol = Int32.Parse(dataTable.Rows[0]["mAllCol"].ToString());
                iAllTot = Int32.Parse(dataTable.Rows[0]["mAllTot"].ToString());
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }

        // ------------------------------
        // Insert Prior Days Values to Next Day
        // ------------------------------

        if (dataTable.Rows.Count > 0)
        {

            try
            {
                sSql = "insert into " + sLibrary + ".MPUMETLG" +
                    " (MKEY" +
                    ", MDAT" +
                    ", MSCN" +
                    ", MPRTMON" +
                    ", MPRTCOL" +
                    ", MCPYMON" +
                    ", MCPYCOL" +
                    ", MFAXMON" +
                    ", MALLMON" +
                    ", MALLCOL" +
                    ", MALLTOT)" +
                    " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
                odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

                odbcCmd.Parameters.Add("@DataDate", OdbcType.Int);
                odbcCmd.Parameters["@DataDate"].Value = DataDate;

                odbcCmd.Parameters.Add("@Scantime", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@Scantime"].Value = "";

                odbcCmd.Parameters.Add("@PrtMon", OdbcType.Int);
                odbcCmd.Parameters["@PrtMon"].Value = iPrtMon;

                odbcCmd.Parameters.Add("@PrtCol", OdbcType.Int);
                odbcCmd.Parameters["@PrtCol"].Value = iPrtCol;

                odbcCmd.Parameters.Add("@CpyMon", OdbcType.Int);
                odbcCmd.Parameters["@CpyMon"].Value = iCpyMon;

                odbcCmd.Parameters.Add("@CpyCol", OdbcType.Int);
                odbcCmd.Parameters["@CpyCol"].Value = iCpyCol;

                odbcCmd.Parameters.Add("@FaxMon", OdbcType.Int);
                odbcCmd.Parameters["@FaxMon"].Value = iFaxMon;

                odbcCmd.Parameters.Add("@AllMon", OdbcType.Int);
                odbcCmd.Parameters["@AllMon"].Value = iAllMon;

                odbcCmd.Parameters.Add("@AllCol", OdbcType.Int);
                odbcCmd.Parameters["@AllCol"].Value = iAllCol;

                odbcCmd.Parameters.Add("@AllTot", OdbcType.Int);
                odbcCmd.Parameters["@AllTot"].Value = iAllTot;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
    }
    // ========================================================================
    protected void SetWithPriorDateTonerLevels(int PriorDate, int DataDate, int WorkfileKey)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        // ------------------------------
        // Get Prior Days Values
        // ------------------------------
        int iKey = 0;
        double dBlack = 0.0;
        double dCyan = 0.0;
        double dMagenta = 0.0;
        double dYellow = 0.0;

        try
        {
            sSql = "SELECT" +
                 " TKEY" +
                ", TBLACK" +
                ", TCYAN" +
                ", TMAGENTA" +
                ", TYELLOW" +
                " FROM " + sLibrary + ".MPUTONLG" +
                " WHERE TKEY = ?" +
                " AND TDAT = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
            odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

            odbcCmd.Parameters.Add("@PriorDate", OdbcType.Int);
            odbcCmd.Parameters["@PriorDate"].Value = PriorDate;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                iKey = Int32.Parse(dataTable.Rows[0]["tKey"].ToString());
                dBlack = double.Parse(dataTable.Rows[0]["tBlack"].ToString());
                dCyan = double.Parse(dataTable.Rows[0]["tCyan"].ToString());
                dMagenta = double.Parse(dataTable.Rows[0]["tMagenta"].ToString());
                dYellow = double.Parse(dataTable.Rows[0]["tYellow"].ToString());
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }

        // ------------------------------
        // Insert Prior Days Values to Next Day
        // ------------------------------

        if (dataTable.Rows.Count > 0)
        {
            int iDayOfWeek = GetDayOfWeek(DataDate);
            int iDayOfMonth = GetDayOfMonth(DataDate);

            try
            {
                sSql = "insert into " + sLibrary + ".MPUTONLG" +
                    " (TKEY" +
                    ", TDAT" +
                    ", TDOW" +
                    ", TDOM" +
                    ", TSCN" +
                    ", TBLACK" +
                    ", TCYAN" +
                    ", TMAGENTA" +
                    ", TYELLOW)" +
                    " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
                odbcCmd.Parameters["@WorkfileKey"].Value = WorkfileKey;

                odbcCmd.Parameters.Add("@DataDate", OdbcType.Int);
                odbcCmd.Parameters["@DataDate"].Value = DataDate;

                odbcCmd.Parameters.Add("@DayOfWeek", OdbcType.Int);
                odbcCmd.Parameters["@DayOfWeek"].Value = iDayOfWeek;

                odbcCmd.Parameters.Add("@DayOfMonth", OdbcType.Int);
                odbcCmd.Parameters["@DayOfMonth"].Value = iDayOfMonth;

                odbcCmd.Parameters.Add("@Scantime", OdbcType.VarChar, 25);
                odbcCmd.Parameters["@Scantime"].Value = "";

                odbcCmd.Parameters.Add("@Black", OdbcType.Double);
                odbcCmd.Parameters["@Black"].Value = dBlack;

                odbcCmd.Parameters.Add("@Cyan", OdbcType.Double);
                odbcCmd.Parameters["@Cyan"].Value = dCyan;

                odbcCmd.Parameters.Add("@Magenta", OdbcType.Double);
                odbcCmd.Parameters["@Magenta"].Value = dMagenta;

                odbcCmd.Parameters.Add("@Yellow", OdbcType.Double);
                odbcCmd.Parameters["@Yellow"].Value = dYellow;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
    }

    // ========================================================================
    protected DataTable GetPrintFleetTonerLevelsForDay(string pfUid, string dateToRead, string dateAfter)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select" +
                 " TonerLevel_Black" +
                ", TonerLevel_Cyan" +
                ", TonerLevel_Magenta" +
                ", TonerLevel_Yellow" +
                ", ScanTime" +
                " from Metric_TonerLevel" +
                " where DeviceId = @PfUid" +
                " and ScanTime > @StartDate" +
                " and ScanTime < @EndDate" +
                " and (TonerLevel_Black > 0 or TonerLevel_Cyan > 0 or TonerLevel_Magenta > 0 or TonerLevel_Yellow > 0)" +
                " order by ScanTime desc";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@PfUid"].Value = pfUid;

            sqlCmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@StartDate"].Value = dateToRead;

            sqlCmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@EndDate"].Value = dateAfter;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();

            dataTable.Columns.Add(MakeColumn("Model"));
            string sModel = GetHtsModel(pfUid);

            int iRowIdx = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                dataTable.Rows[iRowIdx]["Model"] = sModel;
                iRowIdx++;
            }
            dataTable.AcceptChanges();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dataTable;

    }
    // ========================================================================
    protected string SetTonerLevels(DataTable dTable, int dataDate, int workfileKey, int usingMicr)
    {
        DateTime datTemp = new DateTime();

        double dBlack = 0.0;
        double dCyan = 0.0;
        double dMagenta = 0.0;
        double dYellow = 0.0;
        double dLevel = 0.0;
        string sScanTime = "";
        string sDataDate = dataDate.ToString();
        string sValidValueFound = "NO";
        string sMod = "";

        int iCartridgeType = 0;
        int iDayOfWeek = GetDayOfWeek(dataDate);
        int iDayOfMonth = GetDayOfMonth(dataDate);
        int iLastScanDate = 0;

        double dBlackHold = 0.0;
        string sMatch = "";

        if (dTable.Rows.Count > 0)
        {
            int iRowIdx = 0;
            foreach (DataRow row in dTable.Rows)
            {
                if (sValidValueFound != "YES")
                {
                    sMod = dTable.Rows[iRowIdx]["Model"].ToString().Trim().ToUpper();
                    dBlackHold = 100 * double.Parse(dTable.Rows[iRowIdx]["TonerLevel_Black"].ToString());

                    sMatch = "N";
                    if (dBlackHold > 0 && dBlackHold < 100)
                        sMatch = "Y";
                    if (dBlackHold == 100)
                    {
                        if (sMod == "HPLSRJETP3015N") { }
                        else if (sMod == "HPLSRJET3015N") { }
                        else if (sMod == "HPLASERJET3015N") { }
                        else if (sMod == "HPLRJETP3015DTN") { }
                        else if (sMod == "HPLSRJETP3015DN") { }
                        else if (sMod.Contains("3015")) { }
                        else
                            sMatch = "Y";
                    }
                    if (sMatch == "Y")
                    {
                        if (dBlack == 0)
                        {
                            dBlack = 100 * double.Parse(dTable.Rows[iRowIdx]["TonerLevel_Black"].ToString());
                            dCyan = 100 * double.Parse(dTable.Rows[iRowIdx]["TonerLevel_Cyan"].ToString());
                            dMagenta = 100 * double.Parse(dTable.Rows[iRowIdx]["TonerLevel_Magenta"].ToString());
                            dYellow = 100 * double.Parse(dTable.Rows[iRowIdx]["TonerLevel_Yellow"].ToString());
                            sScanTime = dTable.Rows[iRowIdx]["ScanTime"].ToString().Trim();
                            sValidValueFound = "YES";
                        }
                    }
                }

                iRowIdx++;
            }

            if (dCyan < 0)
                dCyan = 0;
            if (dMagenta < 0)
                dMagenta = 0;
            if (dYellow < 0)
                dYellow = 0;

            if (dCyan >= 100)
                dCyan = 0;
            if (dMagenta >= 100)
                dMagenta = 0;
            if (dYellow >= 100)
                dYellow = 0;

            if (sValidValueFound == "YES")
            {
                // Add To Toner Log File
                try
                {
                    sSql = "insert into " + sLibrary + ".MPUTONLG" +
                        " (TKEY" +
                        ", TDAT" +
                        ", TDOW" +
                        ", TDOM" +
                        ", TSCN" +
                        ", TBLACK" +
                        ", TCYAN" +
                        ", TMAGENTA" +
                        ", TYELLOW)" +
                         " VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@WorkfileKey", OdbcType.Int);
                    odbcCmd.Parameters["@WorkfileKey"].Value = workfileKey;

                    odbcCmd.Parameters.Add("@UpdateDate", OdbcType.Int);
                    odbcCmd.Parameters["@UpdateDate"].Value = dataDate;

                    odbcCmd.Parameters.Add("@DayOfWeek", OdbcType.Int);
                    odbcCmd.Parameters["@DayOfWeek"].Value = iDayOfWeek;

                    odbcCmd.Parameters.Add("@DayOfMonth", OdbcType.Int);
                    odbcCmd.Parameters["@DayOfMonth"].Value = iDayOfMonth;

                    odbcCmd.Parameters.Add("@ScanTime", OdbcType.VarChar, 25);
                    odbcCmd.Parameters["@ScanTime"].Value = sScanTime;

                    odbcCmd.Parameters.Add("@LevelBlack", OdbcType.Double);
                    odbcCmd.Parameters["@LevelBlack"].Value = dBlack;

                    odbcCmd.Parameters.Add("@LevelCyan", OdbcType.Double);
                    odbcCmd.Parameters["@LevelCyan"].Value = dCyan;

                    odbcCmd.Parameters.Add("@LevelMagenta", OdbcType.Double);
                    odbcCmd.Parameters["@LevelMagenta"].Value = dMagenta;

                    odbcCmd.Parameters.Add("@LevelYellow", OdbcType.Double);
                    odbcCmd.Parameters["@LevelYellow"].Value = dYellow;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }

                // Check Cartridge File for Insert or Update
                if (dBlack > 0)
                {
                    if (usingMicr == 1)
                        iCartridgeType = 2;
                    else
                        iCartridgeType = 1;
                    dLevel = dBlack;
                    UpdCartridgeType(workfileKey, iCartridgeType, dLevel);
                }
                if (dCyan > 0)
                {
                    iCartridgeType = 3;
                    dLevel = dCyan;
                    UpdCartridgeType(workfileKey, iCartridgeType, dLevel);
                }
                if (dMagenta > 0)
                {
                    iCartridgeType = 4;
                    dLevel = dMagenta;
                    UpdCartridgeType(workfileKey, iCartridgeType, dLevel);
                }
                if (dYellow > 0)
                {
                    iCartridgeType = 5;
                    dLevel = dYellow;
                    UpdCartridgeType(workfileKey, iCartridgeType, dLevel);
                }
                // --- Save scan date to header
                datTemp = Convert.ToDateTime(sScanTime);
                iLastScanDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                UpdHeaderLastTonerScanDate(workfileKey, iLastScanDate);
            }
            // ----------------------------
        }
        return sValidValueFound;
    }
    // ========================================================================
    protected void UpdHeaderLastTonerScanDate(int workfileKey, int lastScanDate)
    {
        try
        {
            sSql = "update " + sLibrary + ".MPUHD" +
                " set hTLstScn = ?" +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@LastScan", OdbcType.Int);
            odbcCmd.Parameters["@LastScan"].Value = lastScanDate;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
    }
    // ========================================================================
    protected int GetNextDailyMeterDate(int workfileKey)
    {
        int iNextDailyDate = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select ifnull(max(mdat),0) as lastRecDate" +
                " from " + sLibrary + ".MPUMETLG" +
                " where mKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                int iLastDailyDate = Int32.Parse(dataTable.Rows[0]["lastRecDate"].ToString().Trim());
                if (iLastDailyDate > 0)
                {
                    string sDat = iLastDailyDate.ToString();
                    DateTime datTemp = new DateTime();
                    datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                    datTemp = datTemp.AddDays(1);
                    iNextDailyDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iNextDailyDate;
    }
    // ========================================================================
    protected int GetNextDailyTonerDate(int workfileKey)
    {
        int iNextDailyDate = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select ifnull(max(tdat),0) as lastRecDate" +
                " from " + sLibrary + ".MPUTONLG" +
                " where tKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);

            if (dataTable.Rows.Count > 0)
            {
                int iLastDailyDate = Int32.Parse(dataTable.Rows[0]["lastRecDate"].ToString().Trim());
                if (iLastDailyDate > 0)
                {
                    string sDat = iLastDailyDate.ToString();
                    DateTime datTemp = new DateTime();
                    datTemp = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 01:01:01.000");
                    datTemp = datTemp.AddDays(1);
                    iNextDailyDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iNextDailyDate;
    }

    // ========================================================================
    protected int GetFirstPrintFleetMeterDate(string pfUid)
    {
        int iFirstPfDate = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select min(scantime) as firstScan" +
                " from metric_pagecount" +
                " where deviceId = @PfUid" +
                " and scantime > '2010-06-01'";  // initially was 2009-06-01 when loading all history

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@PfUid"].Value = pfUid;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();

            if (dataTable.Rows.Count > 0)
            {
                string sFirstPfScan = dataTable.Rows[0]["firstScan"].ToString().Trim();
                if (sFirstPfScan.Length > 0)
                {
                    DateTime datTemp = new DateTime();
                    datTemp = Convert.ToDateTime(sFirstPfScan);
                    iFirstPfDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iFirstPfDate;
    }
    // ========================================================================
    protected int GetFirstPrintFleetTonerDate(string pfUid)
    {
        int iFirstPfDate = 0;
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select min(scantime) as firstScan" +
                " from metric_tonerlevel" +
                " where deviceId = @PfUid" +
                " and TonerLevel_Black > 0" +
                " and scantime > '2009-06-01'";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
            sqlCmd.Parameters["@PfUid"].Value = pfUid;

            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(sqlReader);
            sqlReader.Close();

            if (dataTable.Rows.Count > 0)
            {
                string sFirstPfScan = dataTable.Rows[0]["firstScan"].ToString().Trim();
                if (sFirstPfScan.Length > 0)
                {
                    DateTime datTemp = new DateTime();
                    datTemp = Convert.ToDateTime(sFirstPfScan);
                    iFirstPfDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
                }
            }
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return iFirstPfDate;
    }
    // ========================================================================
    protected int GetDayOfWeek(int iDate)
    {
        int iDayOfWeek = 0;

        string sDate = iDate.ToString();

        DateTime datTemp = new DateTime();
        datTemp = Convert.ToDateTime(sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2) + " 01:01:01.000");  //2010-11-30 11:41:37.000
        if (datTemp.DayOfWeek.ToString() == "Sunday")
            iDayOfWeek = 1;
        else if (datTemp.DayOfWeek.ToString() == "Monday")
            iDayOfWeek = 2;
        else if (datTemp.DayOfWeek.ToString() == "Tuesday")
            iDayOfWeek = 3;
        else if (datTemp.DayOfWeek.ToString() == "Wednesday")
            iDayOfWeek = 4;
        else if (datTemp.DayOfWeek.ToString() == "Thursday")
            iDayOfWeek = 5;
        else if (datTemp.DayOfWeek.ToString() == "Friday")
            iDayOfWeek = 6;
        else if (datTemp.DayOfWeek.ToString() == "Saturday")
            iDayOfWeek = 7;

        return iDayOfWeek;
    }
    // ========================================================================
    protected int GetDayOfMonth(int iDate)
    {
        int iDayOfMonth = 0;
        string sDate = iDate.ToString();
        DateTime datTemp = new DateTime();
        datTemp = Convert.ToDateTime(sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2) + " 01:01:01.000");  //2010-11-30 11:41:37.000
        iDayOfMonth = Int32.Parse(datTemp.ToString("dd"));

        return iDayOfMonth;
    }
    // ========================================================================
    protected void UpdCartridgeType(int iKey, int iType, double dLevel)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            // ---- Check if record exists
            sSql = "select CTONLEVEL" +
                " from " + sLibrary + ".MPUCAR" +
                " where CKEY = ?" +
                " and CSEQ = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = iKey;

            odbcCmd.Parameters.Add("@Type", OdbcType.Int);
            odbcCmd.Parameters["@Type"].Value = iType;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }

        // Record Found? Update: --> else Insert
        try
        {
            if (dataTable.Rows.Count > 0)
            {
                // Update Existing entry
                sSql = "update " + sLibrary + ".MPUCAR" +
                    " set CTONLEVEL = ?" +
                    " where CKEY = ?" +
                    " and CSEQ = ?";
            }
            else
            {
                // Insert First Entry 
                sSql = "insert into " + sLibrary + ".MPUCAR" +
                    " (CTONLEVEL, CKEY, CSEQ)" +
                    " VALUES(?, ?, ?)";
            }

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Level", OdbcType.Double);
            odbcCmd.Parameters["@Level"].Value = dLevel;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = iKey;

            odbcCmd.Parameters.Add("@Type", OdbcType.Int);
            odbcCmd.Parameters["@Type"].Value = iType;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------
    }
    // ========================================================================
    protected void GetPastTonerLevels(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable;

        int iWorkfileKey = 0;
        int iUsingMicr = 0;
        string sPfUid = "";
        string sCartridgeColor = "";

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddMonths(-12);
        int iStartDate = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        //int iStartDate = 20090601;
        //int iStartDate = 20100101;

        double[] dArrayDatQtyLvlDysLod = new double[5];
        double[] dArrayDysLif = new double[2];

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iWorkfileKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString().Trim());
            iUsingMicr = Int32.Parse(dTable.Rows[iRowIdx]["hCurMicr"].ToString().Trim());
            sPfUid = dTable.Rows[iRowIdx]["hPf_Uid"].ToString().Trim();

            if (sPfUid.Length > 0)
            {
                try
                {
                    // ---------------------------------------------------
                    // Get Toner Levels from Toner Log MPUTONLG
                    // ---------------------------------------------------

                    sSql = "select tDat, tBlack, tCyan, tMagenta, tYellow, hht_mod" + // added model to check for false spikes
                    " from " + sLibrary + ".MPUTONLG, " + sLibrary + ".MPUHD" +
                    " where tKey = hkey" +
                    " and tKey = ?" +
                    " and tDat > ?" +
                    " order by tDat desc";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                    odbcCmd.Parameters["@Key"].Value = iWorkfileKey;

                    odbcCmd.Parameters.Add("@Dat", OdbcType.Int);
                    odbcCmd.Parameters["@Dat"].Value = iStartDate;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

                    dataTable = new DataTable(sMethodName);
                    dataTable.Load(odbcReader);

                    int iOrderDate = 0;

                    if (iUsingMicr == 0)
                        sCartridgeColor = "BLACK";
                    else
                        sCartridgeColor = "MICR";

                    UpdCartridgeLifespan(dataTable, iWorkfileKey, sCartridgeColor, iOrderDate);

                    sCartridgeColor = "CYAN";
                    UpdCartridgeLifespan(dataTable, iWorkfileKey, sCartridgeColor, iOrderDate);

                    sCartridgeColor = "MAGENTA";
                    UpdCartridgeLifespan(dataTable, iWorkfileKey, sCartridgeColor, iOrderDate);

                    sCartridgeColor = "YELLOW";
                    UpdCartridgeLifespan(dataTable, iWorkfileKey, sCartridgeColor, iOrderDate);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }
            }
            iRowIdx++;
        }
    }
    // ========================================================================
    protected DataTable GetLastShipDateForUnitCartridges(int workfileKey)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            // ---------------------------------------------------
            // Get Workfile Unit IDs for Update with HTS Values
            // ---------------------------------------------------
            sSql = "Select CKEY, CSEQ, CDATEORDER" +
                " from " + sLibrary + ".MPUCAR" +
                " where CKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dataTable;
    }

    // ========================================================================
    protected void CalcDaysSinceLastShipment(DataTable dTable)
    {
        int iKey = 0;
        int iSeq = 0;
        int iOrderDate = 0;
        int iDaysSinceOrder = 0;

        string sTempDate = "";
        string sOrderDate = "";

        TimeSpan ts = new TimeSpan();
        DateTime datLastOrdered = new DateTime();

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["CKEY"].ToString().Trim());
            iSeq = Int32.Parse(dTable.Rows[iRowIdx]["CSEQ"].ToString().Trim());
            iOrderDate = Int32.Parse(dTable.Rows[iRowIdx]["CDATEORDER"].ToString().Trim());

            iDaysSinceOrder = 0;

            if (iOrderDate > 0)
            {
                try
                {
                    sOrderDate = iOrderDate.ToString();
                    sTempDate = sOrderDate.Substring(0, 4) + "-" + sOrderDate.Substring(4, 2) + "-" + sOrderDate.Substring(6, 2) + " 00:00:00.000";
                    datLastOrdered = Convert.ToDateTime(sTempDate);  //2010-11-30 11:41:37.000
                    ts = datToday.Subtract(datLastOrdered);
                    iDaysSinceOrder = ts.Days;

                    // ---------------------------------------------------
                    // Get Toner Levels from metric_tonerlevel
                    // ---------------------------------------------------
                    sSql = "update " + sLibrary + ".MPUCAR" +
                        " set CDAYSORDER = ?" +
                        " where CKEY = ?" +
                        " and CSEQ = ?";

                    odbcCmd = new OdbcCommand(sSql, odbcConn);

                    odbcCmd.Parameters.Add("@DaysSinceOrder", OdbcType.Int);
                    odbcCmd.Parameters["@DaysSinceOrder"].Value = iDaysSinceOrder;

                    odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                    odbcCmd.Parameters["@Key"].Value = iKey;

                    odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
                    odbcCmd.Parameters["@Seq"].Value = iSeq;

                    odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    sErrMessage = ex.ToString();
                    eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                }
                finally
                {
                    odbcCmd.Dispose();
                }

            }
            iRowIdx++;
        }
        // --------------------------------------------------
    }
    // ========================================================================
    protected void UpdHeaderWithLowestCartridgeData(int workfileKey, double lowestDaysLeft, int lowestLifespan, int lowestQtyOnHand, double lowestLevel, double lowestDaysSinceOrder, int lowestDateLoaded, int lowestDateOrdered, string lowestCartridgeColor)
    {
        try
        {
            int iLowQtyPositive = 0;

            if (lowestQtyOnHand < 0)
                iLowQtyPositive = 0;
            else
                iLowQtyPositive = lowestQtyOnHand;

            string sLowColor = "";
            if (lowestCartridgeColor != "")
            {
                if (lowestCartridgeColor == "MICR")
                    sLowColor = "R";
                else
                    sLowColor = lowestCartridgeColor.Substring(0, 1);
            }

            sSql = "update " + sLibrary + ".MPUHD set" +
                 " HLOWTYP = ?" +
                 ", HQTYONH = ?" +
                 ", HQTYPOS = ?" +
                 ", HLOWEND = ?" +
                 ", HLOWLIF = ?" +
                 ", HLOWLVL = ?" +
                 ", HLOWSHP = ?" +
                 ", HDATLOD = ?" +
                 ", HDATORD = ?" +
                 " where HKEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Color", OdbcType.VarChar, 1);
            odbcCmd.Parameters["@Color"].Value = sLowColor;

            odbcCmd.Parameters.Add("@QtyOnHand", OdbcType.Int);
            odbcCmd.Parameters["@QtyOnHand"].Value = lowestQtyOnHand;

            odbcCmd.Parameters.Add("@QtyPos", OdbcType.Int);
            odbcCmd.Parameters["@QtyPos"].Value = iLowQtyPositive;

            odbcCmd.Parameters.Add("@LowDaysLeft", OdbcType.Double);
            odbcCmd.Parameters["@LowDaysLeft"].Value = lowestDaysLeft;

            odbcCmd.Parameters.Add("@Lifespan", OdbcType.Int);
            odbcCmd.Parameters["@Lifespan"].Value = lowestLifespan;

            odbcCmd.Parameters.Add("@LowLevel", OdbcType.Double);
            odbcCmd.Parameters["@LowLevel"].Value = lowestLevel;

            odbcCmd.Parameters.Add("@LowDaysSinceOrder", OdbcType.Double);
            odbcCmd.Parameters["@LowDaysSinceOrder"].Value = lowestDaysSinceOrder;

            odbcCmd.Parameters.Add("@DateLoaded", OdbcType.Int);
            odbcCmd.Parameters["@DateLoaded"].Value = lowestDateLoaded;

            odbcCmd.Parameters.Add("@DateOrdered", OdbcType.Int);
            odbcCmd.Parameters["@DateOrdered"].Value = lowestDateOrdered;

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
    }
    // ========================================================================
    protected void SetRankHigh()
    {
        try
        {
            sSql = "update " + sLibrary + ".MPUHD" +
                " set hRank = 9999";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
    }

    // ========================================================================
    protected DataTable CalcRankToRunOut()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddMonths(-2);
        int iDateExcludeSilentUnits = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        try
        {
            sSql = "select hKey" +
                ", hQtyOnH" +
                ", hLowEnd" +
                ", hLowLif" +
                ", (hLowEnd+(hLowLif*hQtyPos)) as trueDaysToEmpty " + // have to calc curr life plus extra
                " from " + sLibrary + ".MPUHD" +
                " where hNotMan = 0" +
                " and hHidden = 0" +
                " and hSilent = 0" +
                " and hLowEnd > 0" +
                " and hHT_FXA <> ''" +
                " and hHT_AGR <> ''" +  // Added agreement to drop those now off agr
                " and hTLstScn > ? " +
                " and hKey in (select distinct cKey from " + sLibrary + ".MPUCAR)" +
                " order by trueDaysToEmpty";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Date", OdbcType.Int);
            odbcCmd.Parameters["@Date"].Value = iDateExcludeSilentUnits;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }


    // ========================================================================
    protected void SaveRankToRunOut(DataTable dTable)
    {
        int iKey = 0;
        int iRank = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString().Trim());
            iRank = iRowIdx + 1;

            try
            {
                sSql = "update " + sLibrary + ".MPUHD" +
                    " set hRank = ?" +
                    " where hKey = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Rank", OdbcType.Int);
                odbcCmd.Parameters["@Rank"].Value = iRank;

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = iKey;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            iRowIdx++;
        }
        // --------------------------------------------------

    }

    // ========================================================================
    protected DataTable GetHeaderKey()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select hKey" +
                " from " + sLibrary + ".MPUHD" +
                " where hNotMan = 0";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected void GetPageCountsForAllUnits(DataTable dTable)
    {
        int[] iArray6 = new int[6];
        int iKey = 0;
        int iPageCountWeekTot = 0;
        int iPageCountMonthTot = 0;

        int iPageCountWeekBlack = 0;
        int iPageCountMonthBlack = 0;

        int iPageCountWeekColor = 0;
        int iPageCountMonthColor = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString().Trim());
            iArray6 = GetPageCounts(iKey);

            iPageCountWeekTot = iArray6[0];
            iPageCountMonthTot = iArray6[1];
            iPageCountWeekBlack = iArray6[2];
            iPageCountMonthBlack = iArray6[3];
            iPageCountWeekColor = iArray6[4];
            iPageCountMonthColor = iArray6[5];

            // Aug 22 2011 -- PF Errors in at 10-14 million pages
            if (iPageCountWeekBlack > 999999)
                iPageCountWeekBlack = 0;
            if (iPageCountMonthBlack > 999999)
                iPageCountMonthBlack = 0;
            if (iPageCountWeekColor > 999999)
                iPageCountWeekColor = 0;
            if (iPageCountMonthColor > 999999)
                iPageCountMonthColor = 0;
            if (iPageCountWeekTot > 999999)
                iPageCountWeekTot = 0;
            if (iPageCountMonthTot > 999999)
                iPageCountMonthTot = 0;

            try
            {
                sSql = "update " + sLibrary + ".MPUHD set" +
                    " hPgWeek = ?" +
                    ", hPgMonth = ?" +
                    ", hPgWeekB = ?" +
                    ", hPgMonthB = ?" +
                    ", hPgWeekC = ?" +
                    ", hPgMonthC = ?" +
                    " where hKey = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@WeekTot", OdbcType.Int);
                odbcCmd.Parameters["@WeekTot"].Value = iPageCountWeekTot;

                odbcCmd.Parameters.Add("@MonthTot", OdbcType.Int);
                odbcCmd.Parameters["@MonthTot"].Value = iPageCountMonthTot;

                odbcCmd.Parameters.Add("@WeekBlack", OdbcType.Int);
                odbcCmd.Parameters["@WeekBlack"].Value = iPageCountWeekBlack;

                odbcCmd.Parameters.Add("@MonthBlack", OdbcType.Int);
                odbcCmd.Parameters["@MonthBlack"].Value = iPageCountMonthBlack;

                odbcCmd.Parameters.Add("@WeekColor", OdbcType.Int);
                odbcCmd.Parameters["@WeekColor"].Value = iPageCountWeekColor;

                odbcCmd.Parameters.Add("@MonthColor", OdbcType.Int);
                odbcCmd.Parameters["@MonthColor"].Value = iPageCountMonthColor;

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = iKey;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            iRowIdx++;
        }
        // --------------------------------------------------

    }
    // ========================================================================
    protected int[] GetPageCounts(int iKey)
    {
        int[] iArray6 = new int[6];
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        DateTime datTemp = new DateTime();
        int iDateYesterday = 0;
        int iDateWeekAgo = 0;
        int iDateMonthAgo = 0;

        int iMeterBlackYesterday = 0;
        int iMeterColorYesterday = 0;
        int iMeterBlackWeekAgo = 0;
        int iMeterColorWeekAgo = 0;
        int iMeterBlackMonthAgo = 0;
        int iMeterColorMonthAgo = 0;


        int iPagesAllWeek = 0;
        int iPagesAllMonth = 0;
        int iPagesBlackWeek = 0;
        int iPagesBlackMonth = 0;
        int iPagesColorWeek = 0;
        int iPagesColorMonth = 0;

        int iPrtMon = 0;
        int iPrtCol = 0;
        int iCpyMon = 0;
        int iCpyCol = 0;
        int iFaxMon = 0;
        int iAllMon = 0;
        int iAllCol = 0;
        int iAllTot = 0;

        datTemp = DateTime.Now;
        datTemp = datTemp.AddDays(-1);
        iDateYesterday = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        datTemp = datTemp.AddDays(-7);
        iDateWeekAgo = Int32.Parse(datTemp.ToString("yyyyMMdd"));
        datTemp = DateTime.Now;
        datTemp = datTemp.AddDays(-1);
        datTemp = datTemp.AddMonths(-1);
        iDateMonthAgo = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        // Get Meters Yesterday
        dataTable = GetPageCountsForDay(iKey, iDateYesterday);

        if (dataTable.Rows.Count > 0)
        {
            iPrtMon = Int32.Parse(dataTable.Rows[0]["mPrtMon"].ToString());
            iPrtCol = Int32.Parse(dataTable.Rows[0]["mPrtCol"].ToString());
            iCpyMon = Int32.Parse(dataTable.Rows[0]["mCpyMon"].ToString());
            iCpyCol = Int32.Parse(dataTable.Rows[0]["mCpyCol"].ToString());
            iFaxMon = Int32.Parse(dataTable.Rows[0]["mFaxMon"].ToString());
            iAllMon = Int32.Parse(dataTable.Rows[0]["mAllMon"].ToString());
            iAllCol = Int32.Parse(dataTable.Rows[0]["mAllCol"].ToString());
            iAllTot = Int32.Parse(dataTable.Rows[0]["mAllTot"].ToString());

            iMeterBlackYesterday = iPrtMon + iCpyMon + iFaxMon;
            if (iMeterBlackYesterday == 0)
                iMeterBlackYesterday = iAllMon;
            if (iMeterBlackYesterday == 0)
                iMeterBlackYesterday = iAllTot;
            iMeterColorYesterday = iPrtCol + iCpyCol;
            if (iMeterColorYesterday == 0)
                iMeterColorYesterday = iAllCol;
        }

        // Get Meters Week Ago
        dataTable = GetPageCountsForDay(iKey, iDateWeekAgo);

        if (dataTable.Rows.Count > 0)
        {
            iPrtMon = Int32.Parse(dataTable.Rows[0]["mPrtMon"].ToString());
            iPrtCol = Int32.Parse(dataTable.Rows[0]["mPrtCol"].ToString());
            iCpyMon = Int32.Parse(dataTable.Rows[0]["mCpyMon"].ToString());
            iCpyCol = Int32.Parse(dataTable.Rows[0]["mCpyCol"].ToString());
            iFaxMon = Int32.Parse(dataTable.Rows[0]["mFaxMon"].ToString());
            iAllMon = Int32.Parse(dataTable.Rows[0]["mAllMon"].ToString());
            iAllCol = Int32.Parse(dataTable.Rows[0]["mAllCol"].ToString());
            iAllTot = Int32.Parse(dataTable.Rows[0]["mAllTot"].ToString());

            iMeterBlackWeekAgo = iPrtMon + iCpyMon + iFaxMon;
            if (iMeterBlackWeekAgo == 0)
                iMeterBlackWeekAgo = iAllMon;
            if (iMeterBlackWeekAgo == 0)
                iMeterBlackWeekAgo = iAllTot;
            iMeterColorWeekAgo = iPrtCol + iCpyCol;
            if (iMeterColorWeekAgo == 0)
                iMeterColorWeekAgo = iAllCol;

            // Calculate Page Counts for Week
            if ((iMeterBlackYesterday > 0) && (iMeterBlackWeekAgo > 0) && (iMeterBlackYesterday > iMeterBlackWeekAgo))
                iPagesBlackWeek = (iMeterBlackYesterday - iMeterBlackWeekAgo);
            if ((iMeterColorYesterday > 0) && (iMeterColorWeekAgo > 0) && (iMeterColorYesterday > iMeterColorWeekAgo))
                iPagesColorWeek = (iMeterColorYesterday - iMeterColorWeekAgo);
            iPagesAllWeek = iPagesBlackWeek + iPagesColorWeek;
        }

        // Get Meters MonthAgo
        dataTable = GetPageCountsForDay(iKey, iDateMonthAgo);

        if (dataTable.Rows.Count > 0)
        {
            iPrtMon = Int32.Parse(dataTable.Rows[0]["mPrtMon"].ToString());
            iPrtCol = Int32.Parse(dataTable.Rows[0]["mPrtCol"].ToString());
            iCpyMon = Int32.Parse(dataTable.Rows[0]["mCpyMon"].ToString());
            iCpyCol = Int32.Parse(dataTable.Rows[0]["mCpyCol"].ToString());
            iFaxMon = Int32.Parse(dataTable.Rows[0]["mFaxMon"].ToString());
            iAllMon = Int32.Parse(dataTable.Rows[0]["mAllMon"].ToString());
            iAllCol = Int32.Parse(dataTable.Rows[0]["mAllCol"].ToString());
            iAllTot = Int32.Parse(dataTable.Rows[0]["mAllTot"].ToString());

            iMeterBlackMonthAgo = iPrtMon + iCpyMon + iFaxMon;
            if (iMeterBlackMonthAgo == 0)
                iMeterBlackMonthAgo = iAllMon;
            if (iMeterBlackMonthAgo == 0)
                iMeterBlackMonthAgo = iAllTot;
            iMeterColorMonthAgo = iPrtCol + iCpyCol;
            if (iMeterColorMonthAgo == 0)
                iMeterColorMonthAgo = iAllCol;

            // Calculate Page Counts for Month
            if ((iMeterBlackYesterday > 0) && (iMeterBlackMonthAgo > 0) && (iMeterBlackYesterday > iMeterBlackMonthAgo))
                iPagesBlackMonth = (iMeterBlackYesterday - iMeterBlackMonthAgo);
            if ((iMeterColorYesterday > 0) && (iMeterColorMonthAgo > 0) && (iMeterColorYesterday > iMeterColorMonthAgo))
                iPagesColorMonth = (iMeterColorYesterday - iMeterColorMonthAgo);
            iPagesAllMonth = iPagesBlackMonth + iPagesColorMonth;
        }

        iArray6[0] = iPagesAllWeek;
        iArray6[1] = iPagesAllMonth;
        iArray6[2] = iPagesBlackWeek;
        iArray6[3] = iPagesBlackMonth;
        iArray6[4] = iPagesColorWeek;
        iArray6[5] = iPagesColorMonth;

        // ----------------------------------------------
        return iArray6;
    }
    // ========================================================================
    protected DataTable GetPageCountsForDay(int key, int chosenDay)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select MPRTMON, MPRTCOL, MCPYMON, MCPYCOL, MFAXMON, MALLMON, MALLCOL, MALLTOT" +
                " from " + sLibrary + ".MPUMETLG" +
                " where mKey = ?" +
                " and mDat = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcCmd.Parameters.Add("@Day", OdbcType.Int);
            odbcCmd.Parameters["@Day"].Value = chosenDay;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }

        // ----------------------------------------------
        return dataTable;
    }
    // ========================================================================
    protected void UpdCartridgeLifespan(DataTable dTable, int workfileKey, string cartridgeColor, int orderDate)
    {
        TimeSpan ts = new TimeSpan();

        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;
        datTemp = datTemp.AddDays(-1);
        int iDateYesterday = Int32.Parse(datTemp.ToString("yyyyMMdd"));

        DateTime datLowestToner = new DateTime();       // Lowest scan date on each cartridge
        DateTime datHighestToner = new DateTime();      // Highest scan date on current cartridge
        DateTime datLastFilled = new DateTime();      // Last Date Cartridge loaded
        DateTime datLastOrdered = new DateTime();      // converted from input parm 

        string sScanTime = "";
        string sReduceQOH = "";

        int iScanDate = 0;
        int iColorSeq = 0;
        double dPercentUsed = 0.0;
        int iCurrCartridgeLifespan = 0;
        int iNewQtyOnHand = 0;
        int iCartridgeCount = 0;
        int iCartridgeDaysFound = 0;
        int iCartridgeLifespan = 0;
        int iDateFull = 0;
        int iCurrentQty = 0;
        int iDateLoaded = 0;
        int iCartRecNeeded = 0;
        int iNewQohPos = 0;

        double dLevel = 0.0;
        double dLevelLow = 0.0;
        double dLevelLowCurr = 0.0;
        double dLevelHigh = 0.0;
        double dDaysLeft = 0.0;
        double dUsePerDay = 0.0;
        double dUsePerDayCurr = 0.0;
        double dUsePerDayWeek = 0.0;

        string sScanLow = "";
        string sScanHigh = "";
        string sDat = "";
        string sMod = "";
        string sMatch = "N";

        if (orderDate > 0)
        {
            sDat = orderDate.ToString();
            datLastOrdered = Convert.ToDateTime(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 12:13:14.000");
        }
        int iRowIdx = 0;

        foreach (DataRow row in dTable.Rows)
        {
            iScanDate = Int32.Parse(dTable.Rows[iRowIdx]["tDat"].ToString());
            sScanTime = iScanDate.ToString().Trim();
            sScanTime = sScanTime.Substring(0, 4) + "-" + sScanTime.Substring(4, 2) + "-" + sScanTime.Substring(6, 2) + " 01:01:01.000";
            if ((cartridgeColor == "BLACK") || (cartridgeColor == "MICR"))
                dLevel = double.Parse(dTable.Rows[iRowIdx]["tblack"].ToString());
            else if (cartridgeColor == "CYAN")
                dLevel = double.Parse(dTable.Rows[iRowIdx]["tcyan"].ToString());
            else if (cartridgeColor == "MAGENTA")
                dLevel = double.Parse(dTable.Rows[iRowIdx]["tmagenta"].ToString());
            else if (cartridgeColor == "YELLOW")
                dLevel = double.Parse(dTable.Rows[iRowIdx]["tyellow"].ToString());
            sMod = dTable.Rows[iRowIdx]["hht_mod"].ToString();

            // ----------------------------
            // READ TONER HISTORY FOR CURRENT CARTRIDGE COLOR
            // ----------------------------
            // Start Off reading until you find a valid record
            if (dLevelLow == 0)
            {
                //if ((dLevel > 0) && (dLevel < 100)) // if 1st rec was 100 you'll never find any higher...
                if ((dLevel > 0) && (dLevel <= 100)) // try accepting 100% to start
                {
                    dLevelLow = dLevel;
                    dLevelLowCurr = dLevel;
                    sScanLow = sScanTime;
                }
            }
            // Valid start record found -- read rest.
            else
            {
                // ===============================================================================
                // Try to calculate the most recent usage (save first level change after a week) 
                if ((iRowIdx > 6) && (dUsePerDayWeek == 0))
                {
                    if ((dLevel > 0) && (dLevelLow > 0) && (dLevel > dLevelLow))
                        dUsePerDayWeek = (dLevel - dLevelLow) / (iRowIdx + 1);
                }
                if (dLevel >= dLevelHigh)
                {
                    //if (dLevel < 100) // I was skipping 100% full to avoid false readings...
                    sMatch = "N";
                    if (dLevel > 0 && dLevel < 100)
                        sMatch = "Y";
                    if (dLevel == 100)
                    {

                        if (sMod == "HPLSRJETP3015N") { }
                        else if (sMod == "HPLSRJET3015N") { }
                        else if (sMod == "HPLASERJET3015N") { }
                        else if (sMod == "HPLRJETP3015DTN") { }
                        else if (sMod == "HPLSRJETP3015DN") { }
                        else if (sMod.Contains("3015")) { }
                        else
                            sMatch = "Y";
                    }
                    if (sMatch == "Y")
                    {
                        dLevelHigh = dLevel;
                        sScanHigh = sScanTime;
                    }
                }
                // Prior record was a decrease: Possible Toner cartridge change?
                else
                {
                    // ===============================================================================
                    // New cartridge drop must be SIGNIFICANT (lots of false intermediate low readings)
                    if ((dLevel > 0) && (dLevel < (dLevelHigh - dMinimumChangeIndicatingNewCartridge)))
                    {
                        if (iDateFull == 0)
                        {
                            datLastFilled = Convert.ToDateTime(sScanHigh);  //2010-11-30 11:41:37.000
                            datLastFilled = datLastFilled.AddDays(1);
                            iDateFull = Int32.Parse(datLastFilled.ToString("yyyyMMdd"));
                        }
                        datLowestToner = Convert.ToDateTime(sScanLow);
                        datHighestToner = Convert.ToDateTime(sScanHigh);
                        ts = datLowestToner.Subtract(datHighestToner);
                        iCartridgeDaysFound = ts.Days;
                        iCartridgeCount++;

                        if (iCartridgeDaysFound > iMinimumDaysToCalcUse) // was > 0
                        {
                            dUsePerDay =
                                (
                                    (
                                        ((iCartridgeCount - 1) * dUsePerDay)
                                        +
                                        ((dLevelHigh - dLevelLow) / iCartridgeDaysFound)
                                    )
                                    / iCartridgeCount
                                );
                            dPercentUsed = (dLevelHigh - dLevelLow) * .01;
                            // Save rate on current cartridge (show whatever is running out faster) 
                            if ((dUsePerDayCurr == 0) && (iRowIdx >= 7))
                                dUsePerDayCurr = dUsePerDay;

                            try
                            {
                                sErrValues = "  Days Found: " + iCartridgeDaysFound.ToString() +
                                            "  Percent Used: " + dPercentUsed.ToString();
                                if (dPercentUsed > .001)
                                    iCurrCartridgeLifespan = Convert.ToInt32(Convert.ToDouble(iCartridgeDaysFound / dPercentUsed));
                                else
                                    iCurrCartridgeLifespan = 9999;
                                sErrValues = "";
                            }
                            catch (Exception ex)
                            {
                                sErrMessage = ex.ToString();
                                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
                            }
                            finally
                            {
                            }

                            iCartridgeLifespan =
                                (
                                    (
                                        iCurrCartridgeLifespan
                                        +
                                        ((iCartridgeCount - 1) * iCartridgeLifespan)
                                    )
                                    / iCartridgeCount
                                );
                        }
                        // -----------------
                        dLevelLow = dLevel;
                        dLevelHigh = dLevel;
                        sScanLow = sScanTime;
                        sScanHigh = sScanTime;
                    }
                }
            } // END CURRENT TONER HISTORY 
            iRowIdx++;
        }

        // Do calculation if printer did not even use one full cartridge
        if ((dUsePerDay == 0) && (dLevelLow > 0) && (dLevelHigh > 0) && (dLevelHigh > dLevelLow))
        {
            datLowestToner = Convert.ToDateTime(sScanLow);
            datHighestToner = Convert.ToDateTime(sScanHigh);

            // --- Calculate cartridge use 
            ts = datLowestToner.Subtract(datHighestToner);
            iCartridgeDaysFound = ts.Days;
            if (iCartridgeDaysFound > iMinimumDaysToCalcUse)
            {
                dUsePerDay = (dLevelHigh - dLevelLow) / iCartridgeDaysFound;
                double dbDays = ts.Days;
                if (dLevelHigh > dLevelLow)
                    iCartridgeLifespan = Convert.ToInt32(dbDays / ((dLevelHigh - dLevelLow) * .01));
            }
        }
        // Added to use fastest run out rate--  either current cart, or average over period
        if (dUsePerDayCurr > dUsePerDay)
            dUsePerDay = dUsePerDayCurr;
        if (dUsePerDayWeek > dUsePerDay)
            dUsePerDay = dUsePerDayWeek;

        if (dUsePerDay > .001)
            dDaysLeft = (dLevelLowCurr / dUsePerDay);
        else
            dDaysLeft = 99999;

        // KOI Nov 29 need reality check here.  If 99999 perhaps days would be percentage left?
        if (iCartridgeLifespan > 99999)
            iCartridgeLifespan = 99999;
        if (dDaysLeft >= 99999)
        {
            if (dLevelLowCurr > 0)
                dDaysLeft = dLevelLowCurr * 4; // Flat line cartridges...
            else
                dDaysLeft = 99999;
        }


        // ---------------------------------------------------
        // Update CARTRIDGE CURRENT STATUS WORKFILE
        // ---------------------------------------------------
        try
        {
            if (cartridgeColor == "BLACK")
                iColorSeq = 1;
            else if (cartridgeColor == "MICR")
                iColorSeq = 2;
            else if (cartridgeColor == "CYAN")
                iColorSeq = 3;
            else if (cartridgeColor == "MAGENTA")
                iColorSeq = 4;
            else if (cartridgeColor == "YELLOW")
                iColorSeq = 5;

            sReduceQOH = "";
            iNewQtyOnHand = 0;
            iNewQohPos = 0;
            iCartRecNeeded = 0;

            // Does this only load prior days shipments?  What if we miss a day?
            if (iDateFull == iDateYesterday)
            {
                int[] iaQtyDatNul = { 0, 0, 0 };
                iaQtyDatNul = GetCartridgeQtyDatNul(workfileKey, iColorSeq);

                iCurrentQty = iaQtyDatNul[0];
                iDateLoaded = iaQtyDatNul[1];
                iCartRecNeeded = iaQtyDatNul[2];
                if (iDateLoaded != iDateYesterday) // Prevent double dipping if run again on same day...
                {
                    sReduceQOH = "Y";
                    iNewQtyOnHand = iCurrentQty - 1;
                }
            }

            if (iCartRecNeeded == 1)
                AddCartridgeRec(workfileKey, iColorSeq);

            sSql = "update " + sLibrary + ".MPUCAR set" +
                 " CDAYSTOEND = ?" +
                ", CTONLIFESP = ?" +
                ", CUSEPERDAY = ?" +
                ", CDATELOAD = ?";
            if (sReduceQOH == "Y")
                sSql += ", CQTYONHAND = ?" +
                        ", CQOHPOS = ?";
            sSql += " where CKEY = ?" +
                " and CSEQ = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@DaysToEnd", OdbcType.Double);
            odbcCmd.Parameters["@DaysToEnd"].Value = dDaysLeft;

            odbcCmd.Parameters.Add("@Lifespan", OdbcType.Int);
            odbcCmd.Parameters["@Lifespan"].Value = iCartridgeLifespan;

            odbcCmd.Parameters.Add("@UsePerDay", OdbcType.Double);
            odbcCmd.Parameters["@UsePerDay"].Value = dUsePerDay;

            odbcCmd.Parameters.Add("@DateLoaded", OdbcType.Int);
            odbcCmd.Parameters["@DateLoaded"].Value = iDateFull;

            if (sReduceQOH == "Y")
            {
                odbcCmd.Parameters.Add("@QtyOnHand", OdbcType.Int);
                odbcCmd.Parameters["@QtyOnHand"].Value = iNewQtyOnHand;
                if (iNewQtyOnHand < 0)
                    iNewQohPos = 0;
                else
                    iNewQohPos = iNewQtyOnHand;

                odbcCmd.Parameters.Add("@QohPos", OdbcType.Int);
                odbcCmd.Parameters["@QohPos"].Value = iNewQohPos;
            }
            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = workfileKey;

            odbcCmd.Parameters.Add("@Seq", OdbcType.Int);
            odbcCmd.Parameters["@Seq"].Value = iColorSeq;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ------------------------------------
    }
    // ============================================================================
    public void ClearVerdictFlags()
    {
        try
        {
            sSql = "update " + sLibrary + ".MPUHD set" +
                " hVerdict = 0" +
                ", hShipVia = 0" +
                ", hClass = ''";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

            sSql = "update " + sLibrary + ".MPUCAR" +
                " set cVerdict = 0";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // -----------------------------------
    }
    // ========================================================================
    protected DataTable GetUnitIDsFromMPUHD()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        try
        {
            sSql = "select hKey, hPf_Uid" +
                " from " + sLibrary + ".MPUHD" +
                " where hPf_Uid <> ''" +
                " order by hKey";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dataTable.Load(odbcReader);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
        return dataTable;
    }
    // ============================================================================
    public int CheckForUnitsDeletedFromPrintFleet(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        sqlCmd = new SqlCommand();
        int iKey = 0;
        string sPfUid = "";
        int iDeleteCount = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString());
            sPfUid = dTable.Rows[iRowIdx]["hPf_Uid"].ToString().Trim();

            try
            {
                sSql = "select deviceid, assetnumber" +
                    " from device" +
                    " where deviceid = @PfUid" +
                    " or deviceid = @PfUidUnmanaged";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.Add("@PfUid", SqlDbType.VarChar, 25);
                sqlCmd.Parameters["@PfUid"].Value = sPfUid;

                sqlCmd.Parameters.Add("@PfUidUnmanaged", SqlDbType.VarChar, 25);
                sqlCmd.Parameters["@PfUidUnmanaged"].Value = "-" + sPfUid;
                // KOI: Aug 22 2011, added new sql Reader above because of WEBERRLOG 
                // KOI: Sept 8 2011, trying closing the reader before using it...
                if ((sqlReader != null) && (sqlReader.IsClosed == false))
                    sqlReader.Close();
                sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
                dataTable = new DataTable(sMethodName);
                dataTable.Load(sqlReader);
                sqlReader.Close();

                if (dataTable.Rows.Count == 0)
                {
                    DeleteOrphanedUnitsFromMPUHD(iKey);
                    iDeleteCount++;
                }
            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                sqlCmd.Dispose();
                if ((sqlReader != null) && (sqlReader.IsClosed == false))
                    sqlReader.Close();
            }

            iRowIdx++;
        }
        // -----------------------------------
        return iDeleteCount;
    }
    // ========================================================================
    protected void DeleteOrphanedUnitsFromMPUHD(int key)
    {
        try
        {
            sSql = "delete from " + sLibrary + ".MPUHD" +
                " where hKey = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.Add("@Key", OdbcType.Int);
            odbcCmd.Parameters["@Key"].Value = key;

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
        // ---------------------------------------------- 
    }
    // ========================================================================
    protected void GetLowCartridgeForEachUnit(DataTable dTable)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable;

        int iWorkfileKey = 0;
        int iSeq = 0;
        int iDateOrder = 0;
        int iQtyOrder = 0;
        int iQtyOnHand = 0;
        int iDateLoaded = 0;
        int iTonLifespan = 0;
        int iCurMicr = 0;

        double dTonLevel = 0.0;
        double dDaysSinceOrder = 0.0;
        double dDaysToEnd = 0.0;

        string sColor = "";

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            iWorkfileKey = Int32.Parse(dTable.Rows[iRowIdx]["hKey"].ToString().Trim());
            iCurMicr = Int32.Parse(dTable.Rows[iRowIdx]["hCurMicr"].ToString().Trim());

            try
            {
                sSql = "select cSeq, cQtyOnHand, cQohPos, cTonLifesp, cDaysToEnd, cDateOrder, cQtyOrder, cTonLevel, cDaysOrder, cDateLoad" +
                " from " + sLibrary + ".MPUCAR" +
                " where cKey = ?";
                if (iCurMicr == 1)
                    sSql += " and cSeq NOT IN (1)";
                else
                    sSql += " and cSeq NOT IN (2)";
                sSql += " order by cQohPos, cDaysToEnd";
                //" order by cQtyOnHand, cDaysToEnd";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.Add("@Key", OdbcType.Int);
                odbcCmd.Parameters["@Key"].Value = iWorkfileKey;

                odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);

                dataTable = new DataTable(sMethodName);
                dataTable.Load(odbcReader);

                if (dataTable.Rows.Count > 0)
                {

                    if (int.TryParse(dataTable.Rows[0]["cSeq"].ToString(), out iSeq) == false)
                        iSeq = 0;
                    if (int.TryParse(dataTable.Rows[0]["cDateOrder"].ToString(), out iDateOrder) == false)
                        iDateOrder = 0;
                    if (int.TryParse(dataTable.Rows[0]["cQtyOrder"].ToString(), out iQtyOrder) == false)
                        iQtyOrder = 0;
                    if (int.TryParse(dataTable.Rows[0]["cQtyOnHand"].ToString(), out iQtyOnHand) == false)
                        iQtyOnHand = 0;
                    if (int.TryParse(dataTable.Rows[0]["cTonLifesp"].ToString(), out iTonLifespan) == false)
                        iTonLifespan = 0;
                    if (int.TryParse(dataTable.Rows[0]["cDateLoad"].ToString(), out iDateLoaded) == false)
                        iDateLoaded = 0;
                    if (double.TryParse(dataTable.Rows[0]["cTonLevel"].ToString(), out dTonLevel) == false)
                        dTonLevel = 0;
                    if (double.TryParse(dataTable.Rows[0]["cDaysOrder"].ToString(), out dDaysSinceOrder) == false)
                        dDaysSinceOrder = 0;
                    if (double.TryParse(dataTable.Rows[0]["cDaysToEnd"].ToString(), out dDaysToEnd) == false)
                        dDaysToEnd = 0;

                    if (iSeq == 1)
                        sColor = "BLACK";
                    else if (iSeq == 2)
                        sColor = "MICR";
                    else if (iSeq == 3)
                        sColor = "CYAN";
                    else if (iSeq == 4)
                        sColor = "MAGENTA";
                    else if (iSeq == 5)
                        sColor = "YELLOW";

                    UpdHeaderWithLowestCartridgeData(iWorkfileKey, dDaysToEnd, iTonLifespan, iQtyOnHand, dTonLevel, dDaysSinceOrder, iDateLoaded, iDateOrder, sColor);
                }
            }
            catch (Exception ex)
            {
                sErrMessage = ex.ToString();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
            }
            finally
            {
                odbcCmd.Dispose();
            }
            iRowIdx++;
        }
    }
    // ========================================================================
    protected void UpdSilentUnits()
    {
        try
        {
            sSql = "update " + sLibrary + ".MPUHD" +
                " set HSILENT = 0";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.ExecuteNonQuery();

            sSql = "update " + sLibrary + ".MPUHD" +
                " set HSILENT = 1" +
                " where HHT_MOD IN " +
                " (select PEPART from " + sLibrary + ".PRODEQP where PETNRL = 'N')";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sErrMessage = ex.ToString();
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sErrValues);
        }
        finally
        {
            odbcCmd.Dispose();
        }
    }
    // ========================================================================
    // ========================================================================
}

