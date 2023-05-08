using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.Security;
using CoreWs.Helpers;
using System.Diagnostics;
using System.Net.Sockets;

public partial class public_sc_B1TicketDetail : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    // string sLibrary = "OMDTALIB";
    //SqlCommand sqlCmd;
    //SqlConnection sqlConn;
    //SqlDataReader sqlReader;
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    FileHandler fileHandler;
    ErrorEventHandler erh;

    string sTemp = "";
    string sSql = "";
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        //sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
        
        lbMsg.Text = "";
        lbAddFileResult.Text = "";
        string sEncrypted = "";
        int iRowsAffected = 0;
        int[] iaCtrTck = new int[2];
        int iCenter = 0;
        int iTicket = 0;

        if (!IsPostBack) 
        {
            odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

            try
            {

                if (Request.QueryString["key"] != null && Request.QueryString["key"].ToString() != "")
                    sEncrypted = Request.QueryString["key"];

                if ((sEncrypted == null) || (sEncrypted == ""))
                {

                }
                else
                {
                    iaCtrTck = GetTicketDecrypted(sEncrypted);

                    if (iaCtrTck.Length > 1 && iaCtrTck[0] > 0 && iaCtrTck[1] > 0)
                    {
                        hfCallIsClosed.Value = ws_Get_B1CallIsClosed_Y(iaCtrTck[0], iaCtrTck[1]);
                    }

                }

                odbcConn.Open();

                hfUserIsOmahaAdmin.Value = "";
                hfDeleteIsAuthorized.Value = "";
                btToggleFileEntry.Visible = false;
                pnDisplayFiles.Visible = false;

                if (Page.User.Identity.IsAuthenticated)
                {
                    hfUserName.Value = User.Identity.Name;



                    string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
                    if (saPreNumTyp.Length > 2)
                    {
                        int iRegistrationCs1OrEmp = 0;
                        if (int.TryParse(saPreNumTyp[1], out iRegistrationCs1OrEmp) == false)
                            iRegistrationCs1OrEmp = 0;
                        if (iRegistrationCs1OrEmp > 0)
                        {
                            if (saPreNumTyp[2] != "" && saPreNumTyp[2] != "SRG" && saPreNumTyp[2] != "SRP")
                                hfStsNum.Value = iRegistrationCs1OrEmp.ToString("0");
                        }

                        hfUserAccountType.Value = saPreNumTyp[2]; // Type (REG, LRG, SRC/P/G servright child parent grandparent determines file visibility)
                    }

                    Get_UserPrimaryCustomerNumber();

                    if (hfUserAccountType.Value == "SRG"
                        || hfUserAccountType.Value == "SRP"
                        || hfUserAccountType.Value == "SRC"
                        )
                    {
                        if (hfCallIsClosed.Value == "Y") 
                        {
                            pnNoteType.Visible = false;
                            btToggleStampEntry.Visible = false;
                            btToggleTicketEntry.Visible = false;
                        }
                        else 
                        {
                            pnNoteType.Visible = true;
                            btToggleStampEntry.Visible = true;
                            btToggleTicketEntry.Visible = true;
                        }
                        pnServrightTicketDetail.Visible = true;
                    }
                    else
                    {
                        pnNoteType.Visible = false;
                        btToggleStampEntry.Visible = false;
                        btToggleTicketEntry.Visible = false;
                        pnServrightTicketDetail.Visible = false;
                    }
                    if (User.IsInRole("CustomerAdministrator") || User.IsInRole("SiteAdministrator"))
                    {
                        hfDeleteIsAuthorized.Value = "Authorized";
                        if (User.IsInRole("SiteAdministrator"))
                        {
                            btToggleFileEntry.Visible = true;
                            pnDisplayFiles.Visible = true;
                            hfUserIsOmahaAdmin.Value = "ADMIN";
                        }

                    }
                    //if (User.IsInRole("ServrightGrandparentToBePaid") || User.IsInRole("ServrightParentProvidingFsts") || User.IsInRole("ServrightChildFst"))
                    if (hfUserAccountType.Value == "SRG" || hfUserAccountType.Value == "SRP" || hfUserAccountType.Value == "SRC")
                    {
                        if (hfCallIsClosed.Value == "Y")
                        {
                            btToggleFileEntry.Visible = false;
                        }
                        else 
                        {
                            btToggleFileEntry.Visible = true;
                        }
                        pnDisplayFiles.Visible = true;
                    }
                }
                else 
                {
                    hfUserName.Value = "";
                    hfStsNum.Value = "";
                    hfPrimaryCs1.Value = ""; // new for admin alias
                    btToggleFileEntry.Visible = false;
                    btToggleStampEntry.Visible = false;
                    btToggleTicketEntry.Visible = false;
                }
                    

                btToggleNoteEntry.Text = "Add Note?";
                btToggleFileEntry.Text = "Add File?";
                btToggleStampEntry.Text = "Add Timestamp?";
                btToggleTicketEntry.Text = "Upd Ticket?";

                //lbAddFileResult.Visible = false;  // Don't need to hide it if it is initialized with no text

                //if (Request.QueryString["key"] != null && Request.QueryString["key"].ToString() != "")
                //    sEncrypted = Request.QueryString["key"];

                //if ((sEncrypted == null) || (sEncrypted == ""))
                //{
                //    lbMsg.Text = "A key must be passed to access ticket detail... ";
                //}
                //else
                if (iaCtrTck.Length > 1 && iaCtrTck[0] > 0 && iaCtrTck[1] > 0)
                {
                    iCenter = iaCtrTck[0];
                    iTicket = iaCtrTck[1];

                    //iaCtrTck = GetTicketDecrypted(sEncrypted);
                    //iCenter = iaCtrTck[0];
                    //iTicket = iaCtrTck[1];
                    //            iCenter = 445;
                    //           iTicket = 32657;
                    //iCenter = 112;
                    //iTicket = 44691;

                    hfCenter.Value = iCenter.ToString();
                    hfTicket.Value = iTicket.ToString();
                    hfTicketId.Value = iCenter + "-" + iTicket;

                    // Check to make sure a ticksup record exists for tracking
                    DataTable dt = Select_TICKSUP(iCenter, iTicket);
                    if (dt.Rows.Count == 0) 
                    {
                        iRowsAffected = Insert_Ticksup(iCenter, iTicket);
                    }


                    Load_AllTables();
                }
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
            }

        }
        // ----------------
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    public DataTable GetEmpDetail(int empNum)
    {
        string sSql = "";

        string sEmpName = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " EMPNAM" +
                " from " + sLibrary + ".EMPMST" +
                " where EFIRE = 0" +
                " and PLIMIT in (1, 2)" +
                " and EMPNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@EmpNum", empNum);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            odbcCmd.Dispose();

            if (dt.Rows.Count > 0)
                sEmpName = dt.Rows[0]["EMPNAM"].ToString().Trim();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    public int Select_Mileage(int center, int ticket)
    {
        int iMileage = 0;
        double dMileage = 0.0;

        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " TRPMIL" +
                " from " + sLibrary + ".SVRTICK" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            odbcCmd.Dispose();

            if (dt.Rows.Count > 0) 
            {
                if (double.TryParse(dt.Rows[0]["TRPMIL"].ToString().Trim(), out dMileage) == false)
                    dMileage = 0;

                if (dMileage > 0) 
                {
                    if (int.TryParse(dMileage.ToString("0"), out iMileage) == false)
                        iMileage = 0;
                }
            }
                
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return iMileage;
    }
    // ========================================================================
    public long Select_Lifecount(int center, int ticket)
    {
        long lLifecount = 0;
        double dLifecount = 0.0;

        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " SDXTRE" +
                " from " + sLibrary + ".SVRTICKD" +
                " where SDCENT = ?" +
                " and SDTNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            odbcCmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (double.TryParse(dt.Rows[0]["SDXTRE"].ToString().Trim(), out dLifecount) == false)
                    dLifecount = 0.0;

                if (dLifecount > 0) 
                {
                    if (long.TryParse(dLifecount.ToString("0"), out lLifecount) == false)
                        lLifecount = 0;
                }
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return lLifecount;
    }
    // ========================================================================
    public DataTable Select_TICKSUP(int center, int ticket)
    {
        string sTracking = "";

        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " SUPTRK" +
                " from " + sLibrary + ".TICKSUP" +
                " where SUPCEN = ?" +
                " and SUPTCK = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            odbcCmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                sTracking = dt.Rows[0]["SUPTRK"].ToString().Trim();
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }

    // ========================================================================
    public string Select_Tracking(int center, int ticket)
    {
        string sTracking = "";

        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " SUPTRK" +
                " from " + sLibrary + ".TICKSUP" +
                " where SUPCEN = ?" +
                " and SUPTCK = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
            {
                dt.Load(odbcReader);
            }

            odbcCmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                sTracking = dt.Rows[0]["SUPTRK"].ToString().Trim();
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return sTracking;
    }

    // ========================================================================
    protected int Update_Mileage(int center, int ticket, int mileage)
    {
        int iRowsAffected = 0;

        try
        {
            string sSql = "update " + sLibrary + ".SVRTICK set" +
                " TRPMIL = ?" +
                " where TCCENT = ?" +
                " and TICKNR = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Mileage", mileage);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Update_Lifecount(int center, int ticket, long lifecount)
    {
        int iRowsAffected = 0;

        try
        {
            string sSql = "update " + sLibrary + ".SVRTICKD set" +
                " SDXTRE = ?" +
                " where SDCENT = ?" +
                " and SDTNUM = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Lifecount", lifecount);
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Update_Tracking(int center, int ticket, string tracking)
    {
        int iRowsAffected = 0;
        string sSql = "";
        string sSql2 = "";
        tracking = scrub(tracking);

        if (center > 999)
            lbMsg.Text = "Center number is too big (3 num max)";
        else if (ticket > 9999999)
            lbMsg.Text = "Ticket number is too big (7 num max)";
        else if (!String.IsNullOrEmpty(tracking) && tracking.Length > 25)
            lbMsg.Text = "Tracking number entry is limited to 25 characters";
        else
        {
            try
            {
                sSql = "update " + sLibrary + ".TICKSUP set" +
                    " SUPTRK = ?" +
                    " where SUPCEN = ?" +
                    " and SUPTCK = ?";

                sSql2 = "update " + sLibrary + ".TICKSUP set" +
    " SUPTRK = " + tracking + 
    " where SUPCEN = " + center + 
    " and SUPTCK = " + ticket;

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                odbcCmd.Parameters.AddWithValue("@Mileage", tracking);
                odbcCmd.Parameters.AddWithValue("@Center", center);
                odbcCmd.Parameters.AddWithValue("@Ticket", ticket);

                iRowsAffected = odbcCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "SQL: " + sSql2);
            }
            finally
            {
                odbcCmd.Dispose();
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Insert_Ticksup(int center, int ticket)
    {
        int iRowsAffected = 0;
        string sSql = "";
        try
        {
            sSql = "insert into " + sLibrary + ".TICKSUP" +
                " (SUPCEN, SUPTCK, SUPFL0)" +
                " values (?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            
            odbcCmd.Parameters.AddWithValue("@Center", center);
            odbcCmd.Parameters.AddWithValue("@Ticket", ticket);
            odbcCmd.Parameters.AddWithValue("@CtrTck", center.ToString("0") + ticket.ToString("0"));

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Update_Timestamp(
          string statusCode
        , string reasonCode
        , int stampDate
        , double stampTime
        , int techNum
        , int origCenter
        , int origTicket
        , string origStatusCode
        , string origReasonCode
        , int origStampDate
        , double origStampTime
        , int origTechNum
        )
    {
        int iRowsAffected = 0;
        string sSql = "";
        try
        {
            sSql = "update " + sLibrary + ".TIMESTMP set" +
                 " TIMSTS = ?" +
                ", TIMRSC = ?" +
                ", TIMDST = ?" +
                ", TIMTST = ?" +
                ", TIMTCH = ?" +
                " where" +
                " TIMCTR = ?" +
                " and TIMTCK = ?" +
                " and TIMSTS = ?" +
                " and TIMRSC = ?" +
                " and TIMDST = ?" +
                " and TIMTST = ?" +
                " and TIMTCH = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@StatusCode", statusCode);
            odbcCmd.Parameters.AddWithValue("@ReasonCode", reasonCode);
            odbcCmd.Parameters.AddWithValue("@StampDate", stampDate);
            odbcCmd.Parameters.AddWithValue("@StampTime", stampTime);
            odbcCmd.Parameters.AddWithValue("@TechNum", techNum);

            odbcCmd.Parameters.AddWithValue("@OrigCenter", origCenter);
            odbcCmd.Parameters.AddWithValue("@OrigTicket", origTicket);
            odbcCmd.Parameters.AddWithValue("@OrigStatusCode", origStatusCode);
            odbcCmd.Parameters.AddWithValue("@OrigReasonCode", origReasonCode);
            odbcCmd.Parameters.AddWithValue("@OrigStampDate", origStampDate);
            odbcCmd.Parameters.AddWithValue("@OrigStampTime", origStampTime);
            odbcCmd.Parameters.AddWithValue("@OrigTechNum", origTechNum);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Update_TskDta(
          string statusCode
        , string reasonCode
        , int stampDate
        , double stampTime
        , int techNum
        , int origCenter
        , int origTicket
        , string origStatusCode
        , string origReasonCode
        , int origStampDate
        , double origStampTime
        , int origTechNum
        )
    {
        int iRowsAffected = 0;
        string sSql = "";
        try
        {
            sSql = "update " + sLibrary + ".TSKDTA set" +
                 " TDWEB = ?" +
                //", TD = ?" +
                ", TDDTD = ?" +
                ", TDTMD = ?" +
                ", TDEMP = ?" +
                " where" +
                " TDCTR = ?" +
                " and TDTCK = ?" +
                " and TDWEB = ?" +
                //" and TD = ?" +
                " and TDDTD = ?" +
                " and TDTMD = ?" +
                " and TDEMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@StatusCode", statusCode);
            //odbcCmd.Parameters.AddWithValue("@ReasonCode", reasonCode);
            odbcCmd.Parameters.AddWithValue("@StampDate", stampDate);
            odbcCmd.Parameters.AddWithValue("@StampTime", stampTime);
            odbcCmd.Parameters.AddWithValue("@TechNum", techNum);

            odbcCmd.Parameters.AddWithValue("@OrigCenter", origCenter);
            odbcCmd.Parameters.AddWithValue("@OrigTicket", origTicket);
            odbcCmd.Parameters.AddWithValue("@OrigStatusCode", origStatusCode);
            //odbcCmd.Parameters.AddWithValue("@OrigReasonCode", origReasonCode);
            odbcCmd.Parameters.AddWithValue("@OrigStampDate", origStampDate);
            odbcCmd.Parameters.AddWithValue("@OrigStampTime", origStampTime);
            odbcCmd.Parameters.AddWithValue("@OrigTechNum", origTechNum);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected DataTable Select_PartsShipped(int center, int ticket)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sCtr = center.ToString("000");
            string sTicket = ticket.ToString("0000000");
            string transferId = sCtr + sTicket;

            sSql = "Select" +
                 " PTITRN as TransferId" +
                ", Trim(PTIPRT) as Part" +
                ", Trim(IMFDSC) as Description" +
                ", PTISEQ" +
                ", PTIORQ" +
                ", PTIBKQ" +
                ", PTHEDT as DateOrdered" +
                ", Trim(PSLONG) as SRPART" +
                " from " + sLibrary + ".PTHEAD, " + sLibrary + ".PTITEM, " + sLibrary + ".PRODMST, " + sLibrary + ".PRODSUP" +
                " where PTHVCD = 'ST'" +
                " and PTHVRF = ?" +
                " and PTHTRN = PTITRN" +
                " and PTIPRT = PARTNR" +
                " and PTIPRT = PSPRT#" +
                " order by 1";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@TrnId", transferId);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            dt.Columns.Add(MakeColumn("DisplayDate"));

            int iDat = 0;
            int iRowIdx = 0;
            string sDat = "";
            DateTime datTemp;

            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(dt.Rows[iRowIdx]["DateOrdered"].ToString(), out iDat);
                if (iDat > 20220101 && iDat < 20990101) 
                {
                    sDat = iDat.ToString("00000000"); 
                    if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + "T00:00:00.000", out datTemp) == true)
                        dt.Rows[iRowIdx]["DisplayDate"] = datTemp.ToString("MMM d, yyyy");
                }

                iRowIdx++;
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
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
    #region myWebServiceCalls
    // ========================================================================
    protected int ws_Get_B1TicketCustomerNumber(int center, int ticket) 
    {
        int iCustomerNumber = 0;
        string sCustomerNumber = "";

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketCustomerNumber";
            string sFieldList = "center|ticket|x";
            string sValueList = center + "|" + ticket + "|x";

            sCustomerNumber = Call_WebService_ForString(sJobName, sFieldList, sValueList);
            if (!String.IsNullOrEmpty(sCustomerNumber)) 
            { 
                if (int.TryParse(sCustomerNumber, out iCustomerNumber) == false)
                    iCustomerNumber = -1;
            }

        }

        return iCustomerNumber;
    }
    // -------------------------------------------------------------------------------------------------------
    protected string ws_Get_B1CallIsClosed_Y(int center, int ticket)
    {
        string sCallIsClosed_Y = "";

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1CallIsClosed_Y";
            string sFieldList = "center|ticket|x";
            string sValueList = center + "|" + ticket + "|x";

            sCallIsClosed_Y = Call_WebService_ForString(sJobName, sFieldList, sValueList);
        }

        return sCallIsClosed_Y;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketDetail(int customerNumber, int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0 && center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketDetail";
            string sFieldList = "customerNumber|center|ticket|x";
            string sValueList = customerNumber.ToString("") + "|" + center.ToString("")  + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketModel(int customerNumber, int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0 && center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketModel";
            string sFieldList = "customerNumber|center|ticket|x";
            string sValueList = customerNumber.ToString("") + "|" + center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketSummaryForOpenCalls(int customerNumber)
    {
        DataTable dt = new DataTable();

        if (customerNumber > 0)
        {
            string sJobName = "Get_B1TicketSummaryForOpenCalls";
            string sFieldList = "customerNumber|x";
            string sValueList = customerNumber.ToString() + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_SRTicketTimestamps(int center, int ticket, int tech)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_SRTicketTimestamps";
            string sFieldList = "center|ticket|tech|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|" + tech.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketTimestamps(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketTimestamps";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1LaborOnsite(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1LaborOnsite";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1LaborTravel(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1LaborTravel";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketPartsUsed(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketPartsUsed";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketPartTracking(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketPartTracking";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketPartTrackingFromTransfers(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketPartTrackingFromTransfers";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1ReturnLabel(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1ReturnLabel";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_B1TicketNotes(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_B1TicketNotes";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // -------------------------------------------------------------------------------------------------------
    protected DataTable ws_Get_SRTicketNotes(int center, int ticket)
    {
        DataTable dt = new DataTable();

        if (center > 0 && ticket > 0)
        {
            string sJobName = "Get_SRTicketNotes";
            string sFieldList = "center|ticket|x";
            string sValueList = center.ToString("") + "|" + ticket.ToString("") + "|x";

            dt = Call_WebService_ForDataTable(sJobName, sFieldList, sValueList);
        }

        return dt;
    }
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================
    // ========================================================================
    #region tableChanges
    // ========================================================================
    protected DataTable gvUpd_Files()
    {
        HiddenField hfTemp = new HiddenField();
        HyperLink hlTemp = new HyperLink();

        //LinkButton lkTemp = new LinkButton();
        //Label lbTemp = new Label();
        //TextBox txTemp = new TextBox();
        //DataControlFieldCell dcfcTemp;

        string sType = "";
        string sFileType = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            foreach (Control c1 in gv_Files.Controls)
            {
                sType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.ChildTable"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.GridViewRow"))
                        {
                            foreach (Control c3 in c2.Controls)
                            {
                                sType = c3.GetType().ToString();
                                if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
                                {
                                    foreach (Control c4 in c3.Controls)
                                    {
                                        sType = c4.GetType().ToString();
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                                        {
                                            hfTemp = (HiddenField)c4;
                                            if (hfTemp.ID == "hfFileTypeForLink")
                                            {
                                                sFileType = hfTemp.Value;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                                        {
                                            hlTemp = (HyperLink)c4;
                                            if (hlTemp.ID == "hlAreaItemImage")
                                            {
                                                if (sFileType == "image")
                                                {
                                                    hlTemp.Visible = true;
                                                }
                                                else
                                                {
                                                    hlTemp.Visible = false;
                                                }
                                                sFileType = "";
                                            }
                                        }
                                        // -------------------------------------
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // ========================================================================
    protected DataTable gvUpd_Timestamps()
    {
        string sUserAccountType = hfUserAccountType.Value;

        LinkButton lkTemp = new LinkButton();
        Label lbTemp = new Label();

        string sType = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            foreach (Control c1 in gv_B1TimestampLarge.Controls)
            {
                sType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.ChildTable"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.GridViewRow"))
                        {
                            foreach (Control c3 in c2.Controls)
                            {
                                sType = c3.GetType().ToString();
                                if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
                                {
                                    foreach (Control c4 in c3.Controls)
                                    {
                                        sType = c4.GetType().ToString();
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                        {
                                            lbTemp = (Label)c4;
                                            if (lbTemp.ID == "lbTimestampFormatted")
                                            {
                                                if ((sUserAccountType == "SRC"
                                                    || sUserAccountType == "SRP"
                                                    || sUserAccountType == "SRG"
                                                    )
                                                    && hfCallIsClosed.Value != "Y"
                                                    )
                                                    lbTemp.Visible = false;
                                                else
                                                    lbTemp.Visible = true;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                                        {
                                            lkTemp = (LinkButton)c4;
                                            if (lkTemp.ID == "lkTimestampFormatted")
                                            {
                                                if ((sUserAccountType == "SRC"
                                                    || sUserAccountType == "SRP"
                                                    || sUserAccountType == "SRG"
                                                    )
                                                    && hfCallIsClosed.Value != "Y"
                                                    )
                                                    lkTemp.Visible = true;
                                                else
                                                    lkTemp.Visible = false;
                                            }
                                        }
                                        // -------------------------------------
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
    // =========================================================
    protected void rpUpd_Timestamps()
    {
        string sUserAccountType = hfUserAccountType.Value;

        Label lbTemp = new Label();
        LinkButton lkTemp = new LinkButton();

        string sType = "";

        foreach (Control c1 in rp_B1TimestampSmall.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                    {
                        lbTemp = (Label)c2;
                        if (lbTemp.ID.StartsWith("lbTimestampFormatted")) 
                        {
                            if (sUserAccountType == "SRC"
                                || sUserAccountType == "SRP"
                                || sUserAccountType == "SRG"
                                )
                                lbTemp.Visible = false;
                            else
                                lbTemp.Visible = true;

                        }

                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                    {
                        lkTemp = (LinkButton)c2;
                        if (lkTemp.ID.StartsWith("lkTimestampFormatted"))
                        {
                            if (sUserAccountType == "SRC"
                                || sUserAccountType == "SRP"
                                || sUserAccountType == "SRG"
                                )
                                lkTemp.Visible = true;
                            else
                                lkTemp.Visible = false;

                        }
                    }
                    //-------------------------------------------------------------------------
                    //-------------------------------------------------------------------------
                }
            }
        }
    }

    // ========================================================================
    #endregion // end tableChanges
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; fileHandler = new FileHandler(sLibrary); }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { fileHandler = null; }
    // -------------------------------------------------------------------------------------------------------
    protected void Load_AllTables()
    {
        DataTable dt = new DataTable();
        int iCenter = 0;
        int iTicket = 0;
        int iTech = 0;
        int iCustomerNumber = 0;

        if (int.TryParse(hfCenter.Value, out iCenter) == false)
            iCenter = -1;
        if (int.TryParse(hfTicket.Value, out iTicket) == false)
            iTicket = -1;
        //if (int.TryParse(hfStsNum.Value, out iTech) == false)
        if (int.TryParse(hfPrimaryCs1.Value, out iTech) == false)
            iTech = -1;

        try
        {

            if (iCenter > 0 && iTicket > 0)
            {
                // To debug for a specific ticket...
                //iCenter = 180;
                //iTicket = 6428; //  6375
                // ----------------------------------------------------------------------
                lbTitleTicket.Text = iCenter + "-" + iTicket;
                // Save the the ticket number in case the user wants to create a note.
                //lbMsg.Text = "Ticket Decrypted: " + iCenter + "-" + iTicket;

                iCustomerNumber = ws_Get_B1TicketCustomerNumber(iCenter, iTicket);

                dt = ws_Get_B1TicketDetail(iCustomerNumber, iCenter, iTicket);
                Load_B1TicketData(dt);


                dt = ws_Get_B1TicketModel(iCustomerNumber, iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1ModelSmall.DataSource = dt;
                    rp_B1ModelSmall.DataBind();
                    gv_B1ModelLarge.DataSource = dt;
                    gv_B1ModelLarge.DataBind();
                    pn_B1ModelSmall.Visible = true;
                    pn_B1ModelLarge.Visible = true;
                }
                else
                {
                    pn_B1ModelSmall.Visible = false;
                    pn_B1ModelLarge.Visible = false;
                }

                if (hfUserAccountType.Value == "SRC") 
                    dt = ws_Get_SRTicketTimestamps(iCenter, iTicket, iTech);
                else
                    dt = ws_Get_B1TicketTimestamps(iCenter, iTicket);

                if (dt.Rows.Count > 0)
                {
                    rp_B1TimestampSmall.DataSource = dt;
                    rp_B1TimestampSmall.DataBind();
                    rpUpd_Timestamps();
                    
                    gv_B1TimestampLarge.DataSource = dt;
                    gv_B1TimestampLarge.DataBind();
                    gvUpd_Timestamps();

                    string sScheduleDateFound = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!String.IsNullOrEmpty(row["ScheduleFormatted"].ToString().Trim()))
                            sScheduleDateFound = "Y";
                    }
                    if (sScheduleDateFound == "Y")
                        gv_B1TimestampLarge.Columns[5].Visible = true;
                    else
                        gv_B1TimestampLarge.Columns[5].Visible = false;

                    pn_B1TimestampSmall.Visible = true;
                    pn_B1TimestampLarge.Visible = true;
                }
                else 
                {
                    pn_B1TimestampSmall.Visible = false;
                    pn_B1TimestampLarge.Visible = false;
                }


                dt = ws_Get_B1LaborTravel(iCenter, iTicket);
                    if (dt.Rows.Count > 0)
                    {
                        rp_B1TravelSmall.DataSource = dt;
                        rp_B1TravelSmall.DataBind();
                        gv_B1TravelLarge.DataSource = dt;
                        gv_B1TravelLarge.DataBind();

                        pn_B1TravelSmall.Visible = true;
                        pn_B1TravelLarge.Visible = true;
                    }
                    else
                    {
                        pn_B1TravelSmall.Visible = false;
                        pn_B1TravelLarge.Visible = false;
                    }

                    dt = ws_Get_B1LaborOnsite(iCenter, iTicket);
                    if (dt.Rows.Count > 0)
                    {
                        rp_B1OnsiteSmall.DataSource = dt;
                        rp_B1OnsiteSmall.DataBind();
                        gv_B1OnsiteLarge.DataSource = dt;
                        gv_B1OnsiteLarge.DataBind();

                        pn_B1OnsiteSmall.Visible = true;
                        pn_B1OnsiteLarge.Visible = true;
                    }
                    else
                    {
                        pn_B1OnsiteSmall.Visible = false;
                        pn_B1OnsiteLarge.Visible = false;
                    }

                    dt = ws_Get_B1TicketPartsUsed(iCenter, iTicket);
                    if (dt.Rows.Count > 0)
                    {
                        rp_B1PartUseSmall.DataSource = dt;
                        rp_B1PartUseSmall.DataBind();
                        gv_B1PartUseLarge.DataSource = dt;
                        gv_B1PartUseLarge.DataBind();

                        pn_B1PartUseSmall.Visible = true;
                        pn_B1PartUseLarge.Visible = true;
                    }
                    else
                    {
                        pn_B1PartUseSmall.Visible = false;
                        pn_B1PartUseLarge.Visible = false;
                    }

                dt = Select_PartsShipped(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1PartsShippedSmall.DataSource = dt; 
                    rp_B1PartsShippedSmall.DataBind();
                    gv_B1PartsShippedLarge.DataSource = dt;
                    gv_B1PartsShippedLarge.DataBind();

                    pn_B1PartsShippedSmall.Visible = true;
                    pn_B1PartsShippedLarge.Visible = true;
                }
                else
                {
                    pn_B1PartsShippedSmall.Visible = false;
                    pn_B1PartsShippedLarge.Visible = false;
                }


                dt = ws_Get_B1TicketPartTrackingFromTransfers(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1TrackingSmall.DataSource = dt;
                    rp_B1TrackingSmall.DataBind();
                    gv_B1TrackingLarge.DataSource = dt;
                    gv_B1TrackingLarge.DataBind();

                    pn_B1TrackingSmall.Visible = true;
                    pn_B1TrackingLarge.Visible = true;
                }
                else
                {
                    pn_B1TrackingSmall.Visible = false;
                    pn_B1TrackingLarge.Visible = false;
                }

                dt = ws_Get_B1TicketPartTracking(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1TrackingPbSmall.DataSource = dt;
                    rp_B1TrackingPbSmall.DataBind();
                    gv_B1TrackingPbLarge.DataSource = dt;
                    gv_B1TrackingPbLarge.DataBind();

                    pn_B1TrackingPbSmall.Visible = true;
                    pn_B1TrackingPbLarge.Visible = true;
                }
                else
                {
                    pn_B1TrackingPbSmall.Visible = false;
                    pn_B1TrackingPbLarge.Visible = false;
                }


                dt = ws_Get_B1ReturnLabel(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1ReturnLabelSmall.DataSource = dt;
                    rp_B1ReturnLabelSmall.DataBind();
                    gv_B1ReturnLabelLarge.DataSource = dt;
                    gv_B1ReturnLabelLarge.DataBind();

                    pn_B1ReturnLabelSmall.Visible = true;
                    pn_B1ReturnLabelLarge.Visible = true;
                }
                else
                {
                    pn_B1ReturnLabelSmall.Visible = false;
                    pn_B1ReturnLabelLarge.Visible = false;
                }

                if (hfUserAccountType.Value == "SRC")
                    dt = ws_Get_SRTicketNotes(iCenter, iTicket);
                else
                    dt = ws_Get_B1TicketNotes(iCenter, iTicket);
                if (dt.Rows.Count > 0)
                {
                    rp_B1NotesSmall.DataSource = dt;
                    rp_B1NotesSmall.DataBind();
                    gv_B1NotesLarge.DataSource = dt;
                    gv_B1NotesLarge.DataBind();

                    pn_B1NotesSmall.Visible = true;
                    pn_B1NotesLarge.Visible = true;
                }
                else
                {
                    pn_B1NotesSmall.Visible = false;
                    pn_B1NotesLarge.Visible = false;
                }
                // ----------------------------------------------------------------------
                dt = fileHandler.Select_Files(
                    "Ticket",
                    hfTicketId.Value,
                    "small",
                    "",
                    "",
                    "",
                    "",
                    0,
                    hfUserAccountType.Value);

                if (dt.Rows.Count > 0)
                {
                    gv_Files.DataSource = dt;
                    gv_Files.DataBind();
                    gvUpd_Files();

                    pn_B1File.Visible = true;
                }
                else
                {
                    pn_B1File.Visible = false;
                }
                // ----------------------------------------------------------------------

            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }

    }
    // -------------------------------------------------------------------------------------------------------
    protected void Load_B1TicketData(DataTable dt) 
    {
        DateTime datTemp;
        string sNum = "";
        string sNam = "";
        string sEml = "";
        string sPhn = "";
        string sTemp = "";
        int iCustomerNumber = 0;

        if (dt.Rows.Count > 0)
        {
            lbCstName.Text = dt.Rows[0]["CustName"].ToString().Trim();
            lbCstNum.Text = dt.Rows[0]["CustNum"].ToString().Trim() + "-" + dt.Rows[0]["CustLoc"].ToString().Trim();
            if (int.TryParse(dt.Rows[0]["CustNum"].ToString().Trim(), out iCustomerNumber) == false)
                iCustomerNumber = 0;
            lbCstCrossRef.Text = dt.Rows[0]["CustCrossRef"].ToString().Trim();
            lbCstAddress.Text = dt.Rows[0]["Address1"].ToString().Trim() + " " + dt.Rows[0]["Address2"].ToString().Trim();
            lbCstCityStateZip.Text = dt.Rows[0]["City"].ToString().Trim() + " " + dt.Rows[0]["State"].ToString().Trim() + " " + dt.Rows[0]["ZipFormatted"].ToString().Trim();
            lbCstContact.Text = dt.Rows[0]["Contact"].ToString().Trim();
            lbCstPhone.Text = dt.Rows[0]["PhoneFormatted"].ToString().Trim();
            lbCstRequestedBy.Text = dt.Rows[0]["RequestedBy"].ToString().Trim();

            lbTckCenterTicket.Text = dt.Rows[0]["Center"].ToString().Trim() + "-" + dt.Rows[0]["Ticket"].ToString().Trim();
            sTemp = dt.Rows[0]["TicketCrossRef"].ToString().Trim();
            if (!sTemp.Contains(dt.Rows[0]["TicketXref2"].ToString().Trim()))
            {
                if (!String.IsNullOrEmpty(sTemp))
                    sTemp += " ";
                sTemp += dt.Rows[0]["TicketXref2"].ToString().Trim();
            }
            if (!sTemp.Contains(dt.Rows[0]["TicketXref3"].ToString().Trim()))
            {
                if (!String.IsNullOrEmpty(sTemp))
                    sTemp += " ";
                sTemp += dt.Rows[0]["TicketXref3"].ToString().Trim();
            }

            lbTckCrossRef.Text = sTemp;
            lbTckComment.Text = dt.Rows[0]["Comment1"].ToString().Trim() + " " + dt.Rows[0]["Comment2"].ToString().Trim();
            lbTckCallType.Text = dt.Rows[0]["CallTypeDescription"].ToString().Trim(); // CallType?
            lbTckAgreementType.Text = dt.Rows[0]["AgrDescription"].ToString().Trim();

            if (DateTime.TryParse(dt.Rows[0]["StampEntered"].ToString().Trim(), out datTemp) == true)
            {
                lbTckStampEntered.Text = datTemp.ToString("MMM d, yyyy (h:mm tt)") + " CST";
            }
            if (DateTime.TryParse(dt.Rows[0]["StampCompleted"].ToString().Trim(), out datTemp) == true)
            {
                lbTckStampCompleted.Text = datTemp.ToString("MMM d, yyyy (h:mm tt)") + " CST";
            }

            if (DateTime.TryParse(dt.Rows[0]["StampEta"].ToString().Trim(), out datTemp) == true)
            {
                if (datTemp > DateTime.Now.AddYears(-1)) 
                {
                    lbTckStampEta.Text = datTemp.ToString("MMM d, yyyy (h:mm tt)");
                    if (iCustomerNumber == 165017
                        || iCustomerNumber == 165018
                        || iCustomerNumber == 165019
                        || iCustomerNumber == 165020
                        )
                    {
                        lbTckStampEta.Text += " <i>Tech local time</i>";
                    }
                    else 
                    {
                        lbTckStampEta.Text += " CST";
                    }
                }
            }

            sNum = dt.Rows[0]["CallManagerNumber"].ToString().Trim();
            sNam = dt.Rows[0]["CallManagerName"].ToString().Trim();
            sEml = dt.Rows[0]["CallManagerEmail"].ToString().Trim();
            sPhn = dt.Rows[0]["CallManagerPhoneFormatted"].ToString().Trim();

            lbTckCallManager.Text = "";
            if (!String.IsNullOrEmpty(sNum) && sNum != "0")
            {
                if (!String.IsNullOrEmpty(sNam))
                    lbTckCallManager.Text = sNam;
                if (!String.IsNullOrEmpty(sNum))
                {
                    if (!String.IsNullOrEmpty(lbTckCallManager.Text))
                        lbTckCallManager.Text += " (" + sNum + ")";
                    else
                        lbTckCallManager.Text = "(" + sNum + ")";
                }
                if (!String.IsNullOrEmpty(sPhn) && sPhn != "(000) 000-0000")
                {
                    if (!String.IsNullOrEmpty(lbTckCallManager.Text))
                        lbTckCallManager.Text += "<br />" + sPhn;
                    else
                        lbTckCallManager.Text = sPhn;
                }
                if (!String.IsNullOrEmpty(sEml))
                {
                    if (!String.IsNullOrEmpty(lbTckCallManager.Text))
                        lbTckCallManager.Text += "<br />" + sEml;
                    else
                        lbTckCallManager.Text = sEml;
                }            }

           
            sNum = dt.Rows[0]["SeniorEngineerNumber"].ToString().Trim();
            sNam = dt.Rows[0]["SeniorEngineerName"].ToString().Trim();
            sEml = dt.Rows[0]["SeniorEngineerEmail"].ToString().Trim();
            sPhn = dt.Rows[0]["SeniorEngineerPhoneFormatted"].ToString().Trim();

            lbTckSeniorEngineer.Text = "";

            if (!String.IsNullOrEmpty(sNum) && sNum != "0")
            {
                if (!String.IsNullOrEmpty(sNam))
                    lbTckSeniorEngineer.Text = sNam;
                if (!String.IsNullOrEmpty(sNum))
                {
                    if (!String.IsNullOrEmpty(lbTckSeniorEngineer.Text))
                        lbTckSeniorEngineer.Text += " (" + sNum + ")";
                    else
                        lbTckSeniorEngineer.Text = "(" + sNum + ")";
                }
                if (!String.IsNullOrEmpty(sPhn) && sPhn != "(000) 000-0000")
                {
                    if (!String.IsNullOrEmpty(lbTckSeniorEngineer.Text))
                        lbTckSeniorEngineer.Text += "<br />" + sPhn;
                    else
                        lbTckSeniorEngineer.Text = sPhn;
                }
                if (!String.IsNullOrEmpty(sEml))
                {
                    if (!String.IsNullOrEmpty(lbTckSeniorEngineer.Text))
                        lbTckSeniorEngineer.Text += "<br />" + sEml;
                    else
                        lbTckSeniorEngineer.Text = sEml;
                }
            }
            // xxxx 

            //if (DateTime.TryParse(dt.Rows[0]["StampClosed"].ToString().Trim(), out datTemp) == true)
            //{
            //    lbTckStampClosed.Text = datTemp.ToString("MMM d, yyyy (h:mm tt)");
            //}
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void Load_B1NoteData(DataTable dt)
    {
    }
    // ----------------------------------------------------------------------------
    protected void Get_UserPrimaryCustomerNumber()
    {
        // PrimaryCs1 vs (ChosenCs1 + Chosen Cs2)
        // PrimaryCs1: the default customer associated with the users account -- from either customer, dealer, large customer or sts admin (who can change it)
        // ChosenCs1 + ChosenCs2 is the specific selection of the sub customer off the options on the screen
        int iCustomerNumber = 0;
        if (Page.User.Identity.IsAuthenticated)
        {
            hfUserName.Value = User.Identity.Name;
            string[] saPreNumTyp = Get_UserAccountIds(hfUserName.Value);
            if (saPreNumTyp.Length > 2)
            {
                hfPrimaryCs1.Value = saPreNumTyp[1];
                hfPrimaryCs1Type.Value = saPreNumTyp[2];
            }

            int iAdminCustomerNumber = 0;
            if (Session["AdminCustomerNumber"] != null && Session["AdminCustomerNumber"].ToString().Trim() != "")
            {
                if (int.TryParse(Session["AdminCustomerNumber"].ToString().Trim(), out iAdminCustomerNumber) == false)
                    iAdminCustomerNumber = -1;
                if (iAdminCustomerNumber > 0)
                {
                    hfPrimaryCs1.Value = iAdminCustomerNumber.ToString(); // Switch to use STS admin's customer they switched to
                    if (Session["AdminCustomerType"] != null && Session["AdminCustomerType"].ToString().Trim() != "")
                    {
                        hfPrimaryCs1Type.Value = Session["AdminCustomerType"].ToString().Trim();
                        hfUserAccountType.Value = Session["AdminCustomerType"].ToString().Trim();
                    }
                    else 
                    {
                        hfPrimaryCs1Type.Value = "REG";
                        hfUserAccountType.Value = "REG";
                    }
                }
            }
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btToggleNoteEntry_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        hfAddStampEditKeys.Value = "";

        try
        {
            if (myControl.Text == "Add Note?")
            {
                myControl.Text = "Hide Note Entry";
                pnAddNote.Visible = true;

                // Hide the other panels
                pnAddFile.Visible = false;
                pnAddStamp.Visible = false;
                pnUpdTicket.Visible = false;

                // Reset other buttons to default
                btToggleFileEntry.Text = "Add File?";
                btToggleStampEntry.Text = "Add Timestamp?";
                btToggleTicketEntry.Text = "Upd Ticket?";
            }
            else 
            {
                myControl.Text = "Add Note?";
                pnAddNote.Visible = false;
            }
                
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ========================================================================
    protected void btToggleFileEntry_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        hfAddStampEditKeys.Value = "";

        try
        {
            if (myControl.Text == "Add File?")
            {
                myControl.Text = "Hide File Entry";
                pnAddFile.Visible = true;

                // Hide the other panels
                pnAddNote.Visible = false;
                pnAddStamp.Visible = false;
                pnUpdTicket.Visible = false;


                // Reset other buttons to default
                btToggleNoteEntry.Text = "Add Note?";
                btToggleStampEntry.Text = "Add Timestamp?";
                btToggleTicketEntry.Text = "Upd Ticket?";

            }
            else
            {
                myControl.Text = "Add File?";
                pnAddFile.Visible = false;
                txFileDescription.Text = "";
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // ========================================================================
    protected void btToggleStampEntry_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        //odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        
        hfAddStampEditKeys.Value = "";

        try
        {
            //odbcConn.Open();
            if (myControl.Text == "Add Timestamp?")
            {
                myControl.Text = "Hide Timestamp Entry";
                pnAddStamp.Visible = true;
                lbTimestampAddOrEdit.Text = "Add New Timestamp";

                DateTime datTemp = DateTime.Now.ToLocalTime();
                // Initialize fields in box
                ddAddStampType.SelectedValue = ""; 
                txAddStampDate.Text = datTemp.ToString("yyyyMMdd");
                txAddStampTime.Text = datTemp.ToString("HH.mm");
                //txAddStampEmp.Text = hfStsNum.Value;
                txAddStampEmp.Text = hfPrimaryCs1.Value;

                // Hide the other panels
                pnAddNote.Visible = false;
                pnAddFile.Visible = false;
                pnUpdTicket.Visible = false;

                // Reset other buttons to default
                btToggleNoteEntry.Text = "Add Note?";
                btToggleFileEntry.Text = "Add File?";
                btToggleTicketEntry.Text = "Upd Ticket?";

            }
            else
            {
                myControl.Text = "Add Timestamp?";
                pnAddStamp.Visible = false;
                lbTimestampAddOrEdit.Text = "";
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
        }
    }
    // ========================================================================
    protected void btToggleTicketEntry_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        
        hfAddStampEditKeys.Value = "";

        try
        {
            odbcConn.Open();

            if (myControl.Text == "Upd Ticket?")
            {
                myControl.Text = "Hide Ticket Entry";
                pnUpdTicket.Visible = true;

                int iCenter = 0;
                int iTicket = 0;
                if (int.TryParse(hfCenter.Value, out iCenter) == false)
                    iCenter = -1;
                if (int.TryParse(hfTicket.Value, out iTicket) == false)
                    iTicket = -1;
                if (iCenter > 0 && iTicket > 0) 
                {
                    int iMileage = Select_Mileage(iCenter, iTicket);
                    if (iMileage > 0)
                        txUpdTicketMiles.Text = iMileage.ToString("");
                    else
                        txUpdTicketMiles.Text = "";
                    long lLifecount = Select_Lifecount(iCenter, iTicket);
                    if (lLifecount > 0)
                        txUpdTicketLifecount.Text = lLifecount.ToString("");
                    else
                        txUpdTicketLifecount.Text = "";

                    txUpdTicketTracking.Text = Select_Tracking(iCenter, iTicket);
                }
                else 
                {
                    txUpdTicketMiles.Text = "";
                    txUpdTicketLifecount.Text = "";
                    txUpdTicketTracking.Text = "";
                }

                // Hide the other panels
                pnAddNote.Visible = false;
                pnAddFile.Visible = false;
                pnAddStamp.Visible = false;

                // Reset other buttons to default
                btToggleNoteEntry.Text = "Add Note?";
                btToggleFileEntry.Text = "Add File?";
                btToggleStampEntry.Text = "Add Timestamp?";
            }
            else
            {
                myControl.Text = "Upd Ticket?";
                pnUpdTicket.Visible = false;
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void btAddNoteSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iCenter = 0;
            int iTicket = 0;
            if (int.TryParse(hfCenter.Value, out iCenter) == false)
                iCenter = -1;
            if (int.TryParse(hfTicket.Value, out iTicket) == false)
                iTicket = -1;

            if (iCenter <= 0 || iTicket <= 0)
            {
                lbMsg.Text = "Error: there is a problem with the key fields.  The note cannot be processed.";
            }
            else if (String.IsNullOrEmpty(txAddNoteSubject.Text))
            {
                lbMsg.Text = "A subject is required";
                txAddNoteSubject.Focus();
            }
            else if (String.IsNullOrEmpty(txAddNoteMessage.Text))
            {
                lbMsg.Text = "A message is required";
                txAddNoteMessage.Focus();
            }
            else if (txAddNoteMessage.Text.Length > 1000)
            {
                txAddNoteMessage.Text = txAddNoteMessage.Text.Substring(0, 1000);
                lbMsg.Text = "Message length is limited to 1000 characters. (Entry truncated to the max length.)";
                txAddNoteMessage.Focus();
            }
            else
            {
                string sSubject = txAddNoteSubject.Text;
                string sMessage = txAddNoteMessage.Text;

                string sSuccessOrFailureMessage = "";
                string sDevTestLive = "";
                string sNoteType = "";
                int iEmployeeNumber = 0;

                if (hfUserAccountType.Value == "SRG"
                    || hfUserAccountType.Value == "SRP"
                    || hfUserAccountType.Value == "SRC"
                    )
                {
                    //sNoteType = "FST" or "WORK";
                    sNoteType = ddNoteType.SelectedValue;
                    //if (int.TryParse(hfStsNum.Value, out iEmployeeNumber) == false) // Load Member's STS emp number
                    if (int.TryParse(hfPrimaryCs1.Value, out iEmployeeNumber) == false) // Load Member's STS emp number
                        iEmployeeNumber = 2222; // Failure?  Just load the default
                }
                else 
                {
                    sNoteType = "CUST";
                    iEmployeeNumber = 2222;
                }
                
                //2222; // Internet
                int iNoteNumber = 0;
                int iDate8 = 0;
                double dTime42 = 0.0;
                DateTime datTemp = DateTime.Now;

                // --------------------------------------------------------------------------------
                // USE TRIGGER TO PROCESS 
                // --------------------------------------------------------------------------------
                // STEVE CARLSON -- Sept 16, 2021: Experience with a trigger 2 file
                // a) it would not finish
                // b) I assumed (wrongly) that the trigger was not firing
                // c) Instead the test data area holding the NOTESPF key was out of sync, (offering duplicate keys) 
                // d) CALL EMAILMGR to ensure you have an email entry for 'TRG' so you will get the *PSSR crash messages (to know it's running, and what's wrong) 
                // e) ALSO: to debug, you can't walk through greenscreen debug when inserting from this (or any) web page
                //      instead, you have to create the two trigger files here below (then DON'T delete the records in this program -- comment lines 82-83 in trigger handler)
                //      next start a greenscreen debug session, look at the three fields in T2TRIGGER, delete them, then add them right back 
                //      it's your insert in the same greenscreen session that will allow you to debug in that session to see what's happening. 

                if (sLibrary == "OMDTALIB")
                    sDevTestLive = "LIVE";
                else
                    sDevTestLive = "TEST";  

                // sDevTestLive = "DEV";  // if you want to debug using a program in dev

                DateTime datNow = DateTime.Now;
                if (int.TryParse(datNow.ToString("yyyyMMdd"), out iDate8) == false)
                {
                    if (int.TryParse(DateTime.Now.ToString("yyyyMMdd"), out iDate8) == false)
                        iDate8 = 0;
                }
                if (double.TryParse(datNow.ToString("HH.mm"), out dTime42) == false)
                {
                    if (double.TryParse(DateTime.Now.ToString("HH.mm"), out dTime42) == false)
                        dTime42 = 0.0;
                }

                // --------------------------------------------------------------------------------
                // Get Key For Trigger File
                // --------------------------------------------------------------------------------
                KeyRetriever kr = new KeyRetriever(sLibrary);
                int iNextKeyForFile = kr.GetKey("T2TRIGGER");
                kr = null;

                // --------------------------------------------------------------------------------
                // Get Trigger Object and Load It
                // --------------------------------------------------------------------------------
                TriggerHandler triggerHandler = new TriggerHandler(sDevTestLive);
                // This just retrieves an initialized object
                TriggerHandler.TriggerObject triggerObject = triggerHandler.Get_EmptyTriggerObjectToLoad();

                triggerObject.key = iNextKeyForFile; // New unique key
                triggerObject.programToRun = "T2ADDNOTE"; // Name of program to be called
                triggerObject.DEV_TEST_LIVE = sDevTestLive; // Set As400 Library list in an attempt to run in target environment

                triggerObject.stringToSend_01 = sNoteType;

                triggerObject.stringToSend_02 = sSubject.Trim();
                triggerObject.doubleToSend_01 = iCenter;
                triggerObject.doubleToSend_02 = iTicket;
                triggerObject.doubleToSend_03 = iEmployeeNumber;
                triggerObject.doubleToSend_04 = iDate8;
                triggerObject.doubleToSend_05 = dTime42;

                triggerObject.stringToSend_10K = sMessage.Trim();

                triggerObject = triggerHandler.Save_ObjectToTriggerFile(triggerObject);

                // View object to see values returned
                sSuccessOrFailureMessage = triggerObject.stringReturned.Trim();
                iNoteNumber = (int)triggerObject.doubleReturned_01;

                // Release resources tied to objects when done with them
                triggerObject = null;
                triggerHandler = null;
                // --------------------------------------------------------------------------------

                if (iNoteNumber > 0)
                {
                    DataTable dt = ws_Get_B1TicketNotes(iCenter, iTicket);
                    if (dt.Rows.Count > 0)
                    {
                        rp_B1NotesSmall.DataSource = dt;
                        rp_B1NotesSmall.DataBind();
                        gv_B1NotesLarge.DataSource = dt;
                        gv_B1NotesLarge.DataBind();

                        pn_B1NotesSmall.Visible = true;
                        pn_B1NotesLarge.Visible = true;
                    }
                    else
                    {
                        pn_B1NotesSmall.Visible = false;
                        pn_B1NotesLarge.Visible = false;
                    }

                    string sSubjectTruncated = sSubject.Trim();
                    if (sSubjectTruncated.Length > 20) 
                    {
                        sSubjectTruncated = sSubjectTruncated.Substring(0, 20) + "...";
                    }

                    lbMsg.Text = "SUCCESS: Note added to ticket (with subject: " + sSubjectTruncated + ")";
                    txAddNoteSubject.Text = "";
                    txAddNoteMessage.Text = "";
                    pnAddNote.Visible = false;
                    btToggleNoteEntry.Text = "Add Note?";
                }
                else 
                {
                    lbMsg.Text = "Error: Note creation attempt failed. ";
                }

                // --------------------------------------------------------------------------------
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
  
    // -------------------------------------------------------------------------------------------------------
    protected void btAddFileSubmit_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sFileType = "";
        string sFileName = "";

        if (fuFile.PostedFile.ContentLength > 0)
            sFileName = fuFile.PostedFile.FileName.Trim();

        if (fuFile.PostedFile.ContentLength == 0)
        {
            lbAddFileResult.Text = "A file to upload is required";
            fuFile.Focus();
        }
        else if (String.IsNullOrEmpty(txFileDescription.Text))
        {
            lbAddFileResult.Text = "A description is required";
            txFileDescription.Focus();
        }
        else if (
            !sFileName.EndsWith(".jpg")
            && !sFileName.EndsWith(".gif")
            && !sFileName.EndsWith(".bmp")
            && !sFileName.EndsWith(".png")
            && !sFileName.EndsWith(".pdf")
            && !sFileName.EndsWith(".xls")
            && !sFileName.EndsWith(".xlsx")
            && !sFileName.EndsWith(".doc")
            && !sFileName.EndsWith(".docx")
            && !sFileName.EndsWith(".txt")
            )
        {
            lbAddFileResult.Text = "Currently, only files of the following types are accepted (jpg, gif, bmp, png, pdf, xls, slsx, doc, docx, txt)";
            fuFile.Focus();
        }
        else
        {

            try
            {
                if (
                    sFileName.EndsWith(".jpg") 
                    || sFileName.EndsWith(".gif") 
                    || sFileName.EndsWith(".bmp") 
                    || sFileName.EndsWith(".png") 
                    )
                    sFileType = "image";
                else if (sFileName.EndsWith(".pdf")) 
                    sFileType = "pdf";
                else if (sFileName.EndsWith(".xlsx") || sFileName.EndsWith(".xls"))
                    sFileType = "excel";
                else if (sFileName.EndsWith(".docx") || sFileName.EndsWith(".doc"))
                    sFileType = "word";
                else if (sFileName.EndsWith(".txt") || sFileName.EndsWith(".doc"))
                    sFileType = "text";
                else
                    sFileType = "text"; // If it passed validation but didn't match, just make it text 

                string sFileExtension = Path.GetExtension(sFileName);

                string sScrubbedFileName = scrubFileName(sFileName, sFileExtension);

                string sResult = fileHandler.Insert_FileForArea(
                    fuFile.PostedFile,
                    sScrubbedFileName,
                    txFileDescription.Text.Trim(),
                    hfUserName.Value.Trim(),
                    //hfStsNum.Value.Trim(),
                    hfPrimaryCs1.Value.Trim(),
                    sFileType, // image, pdf, excel, word
                    "ticket",
                    hfTicketId.Value.Trim(),
                    "one"
                    );

                if (sResult.StartsWith("Error:"))
                {
                    sResult = sResult.Replace("Error: ", "");
                    lbAddFileResult.Text = sResult;

                    if (sResult.Contains("Description has exceeded"))
                        txFileDescription.Text = txFileDescription.Text.Substring(0, 500);
                }
                else
                {
                    int iNewId = 0;
                    if (int.TryParse(sResult, out iNewId) == false)
                        iNewId = -1;

                    if (iNewId > 0)
                    {
                        dt = fileHandler.Select_Files(
                            "ticket", 
                            hfTicketId.Value.Trim(), 
                            "small", 
                            "", 
                            "", 
                            "", 
                            "", 
                            0, 
                            hfUserAccountType.Value
                            );

                        if (dt.Rows.Count > 0)
                        {
                            gv_Files.DataSource = dt;
                            gv_Files.DataBind();
                            gvUpd_Files();

                            pn_B1File.Visible = true;
                        }
                        else
                        {
                            pn_B1File.Visible = false;
                        }

                        btToggleFileEntry.Text = "Add File?";
                        pnAddFile.Visible = false;
                        txFileDescription.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
            }
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void btUpdTicketSubmit_Click(object sender, EventArgs e)
    {
        int iRowsMileage = 0;
        int iRowsLifecount = 0;
        int iRowsTracking = 0;

        int iMileage = 0;
        long lLifecount = 0;
        string sTracking = "";

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        try
        {
            odbcConn.Open();
            int iCenter = 0;
            int iTicket = 0;
            if (int.TryParse(hfCenter.Value, out iCenter) == false)
                iCenter = -1;
            if (int.TryParse(hfTicket.Value, out iTicket) == false)
                iTicket = -1;

            if (int.TryParse(txUpdTicketMiles.Text, out iMileage) == false)
                iMileage = -1;
            if (long.TryParse(txUpdTicketLifecount.Text, out lLifecount) == false)
                lLifecount = -1;
            sTracking = txUpdTicketTracking.Text.Trim();

            if (iCenter <= 0 || iTicket <= 0)
            {
                lbMsg.Text = "Error: there is a problem with the key fields.  The update cannot be processed.";
            }
            if (String.IsNullOrEmpty(txUpdTicketMiles.Text))
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "A mileage entry is required (even if zero)";
                txUpdTicketMiles.Focus();
            }
            if (!String.IsNullOrEmpty(txUpdTicketMiles.Text) && iMileage == -1)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "Mileage entry must be an integer";
                txUpdTicketMiles.Focus();
            }
            if (!String.IsNullOrEmpty(txUpdTicketMiles.Text) && iMileage > 1000)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "Mileage is beyond the standard range.  (Is the entry wrong?)";
                txUpdTicketMiles.Focus();
            }
            if (String.IsNullOrEmpty(txUpdTicketLifecount.Text))
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "A lifecount entry is required (even if zero)";
                txUpdTicketMiles.Focus();
            }
            if (!String.IsNullOrEmpty(txUpdTicketLifecount.Text) && lLifecount == -1)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "Lifecount entry must be an integer";
                txUpdTicketLifecount.Focus();
            }
            if (!String.IsNullOrEmpty(txUpdTicketTracking.Text) && txUpdTicketTracking.Text.Length > 25)
            {
                if (lbMsg.Text != "") lbMsg.Text += "<br />";
                lbMsg.Text += "The tracking entry is limited to 25 characters";
                txUpdTicketTracking.Focus();
            }

            if (String.IsNullOrWhiteSpace(lbMsg.Text))
            {
                // Update Ticket Fields
                iRowsMileage = Update_Mileage(iCenter, iTicket, iMileage);
                iRowsLifecount = Update_Lifecount(iCenter, iTicket, lLifecount);
                iRowsTracking = Update_Tracking(iCenter, iTicket, sTracking);

                txUpdTicketMiles.Text = "";
                txUpdTicketLifecount.Text = "";
                txUpdTicketTracking.Text = "";

                pnUpdTicket.Visible = false;
                btToggleTicketEntry.Text = "Upd Ticket?";

                if (iRowsMileage > 0 && iRowsLifecount > 0 && iRowsTracking > 0) 
                {
                    lbMsg.Text = "SUCCESS: Ticket fields updated";
                }
                else
                {
                    lbMsg.Text = "Error: Ticket update was not successful. ";
                }

                // --------------------------------------------------------------------------------
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
        }
    }
    // ========================================================================
    protected void lkFileDelete_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        int iRowsAffected = 0;
        int iRecordId = 0;
        if (int.TryParse(myControl.CommandArgument.ToString().Trim(), out iRecordId) == false)
            iRecordId = -1;
        if (iRecordId > 0) 
        {
            iRowsAffected = fileHandler.Delete_File(iRecordId);

            if (iRowsAffected > 0) 
            {
                dt = fileHandler.Select_Files(
                    "ticket", 
                    hfTicketId.Value.Trim(), 
                    "small", 
                    "", 
                    "", 
                    "", 
                    "", 
                    0,
                    hfUserAccountType.Value
                    );

                if (dt.Rows.Count > 0)
                {
                    gv_Files.DataSource = dt;
                    gv_Files.DataBind();
                    gvUpd_Files();

                    pn_B1File.Visible = true;
                }
                else
                {
                    pn_B1File.Visible = false;
                }
                btToggleFileEntry.Text = "Add File?";
                pnAddFile.Visible = false;
                txFileDescription.Text = "";
            }
        }
    }
    // ========================================================================
    protected void btImageButton_Click(object sender, EventArgs e)
    {
        ImageButton myControl = (ImageButton)sender;
        string[] saParms = myControl.CommandArgument.ToString().Split('|');
        string sRecordId = "";
        string sTicketId = "";
        if (saParms.Length > 1) 
        {
            sRecordId = saParms[0];
            sTicketId = saParms[1];
            //Response.Redirect("~/public/ImageDisplay.aspx?id=" + sRecordId + "&tck=" + sTicketId);
            Response.Redirect("~/public/ImageDisplay.aspx?id=" + sRecordId);
        }
    }
    // ========================================================================
    protected void lkFile_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        string[] saParms = myControl.CommandArgument.ToString().Split('|');
        string sRecordId = "";
        string sFileType = "";
        string sFileName = "";
        string sFileExtension = "";
        //string sRootPath = HttpContext.Current.Server.MapPath(@"~\media\workfiles\");
        string sFullPhysicalPath = "";
        //byte[] byteArray = null;

        if (saParms.Length > 3)
        {
            sRecordId = saParms[0];
            sFileType = saParms[1];
            sFileName = saParms[2];
            sFileExtension = saParms[3];

            //string sWebLink = "";

            try
            {
                //DataTable dt = fileHandler.Get_Files("ticket", "400-3948", "pdf");
                sFullPhysicalPath = fileHandler.Save_BinaryToDiskReturnPath(sRecordId);
                if (!String.IsNullOrEmpty(sFullPhysicalPath))
                {
                    //sFileName = dt.Rows[0]["flFileName"].ToString().Trim();
                    //byteArray = Convert.FromBase64String(dt.Rows[0]["Base64AsString"].ToString().Trim());
                    //sFullPath = sRootPath + sFileName;
                    //System.IO.File.WriteAllBytes(sFullPhysicalPath, byteArray); // now being written inside FileHandler

                    //sWebLink = "~/media/workfiles/TestPdfFile.pdf";

                    Response.ClearContent();
                    //Response.ContentType = "application/ms-excel";
                    //Response.ContentType = "Application/pdf";


                    if (sFileExtension == ".jpg"
                        || sFileExtension == ".jpeg"
                        || sFileExtension == ".png"
                        || sFileExtension == ".gif"
                        || sFileExtension == ".bmp"
                        )
                        Response.ContentType = "data:Image/" + sFileExtension + ";base64,";
                    else if (sFileExtension == ".pdf")
                        Response.ContentType =  "Application/pdf;base64,";
                    else if (sFileExtension == ".xlsx" || sFileExtension == ".xls")
                        Response.ContentType = "Application/x-msexcel;base64,";
                    else if (sFileExtension == ".docx" || sFileExtension == ".doc")
                        Response.ContentType = "Application/msword;base64,";
                    else // if (sFileExtension == "txt")
                        Response.ContentType = "text/plain;base64,";

                    Response.AddHeader("content-disposition", "attachment; filename= " + sFileName);
                    //Response.Write(sCsv);

                    Response.WriteFile(sFullPhysicalPath);
                    //Response.WriteFile(sWebLink);

                    // It was deleted prior to creation in the FileHandler class
                    // if (File.Exists(sFullPath)) 
                    //    File.Delete(sFullPath);
                }

                //fileHandler = null;
            }
            catch (Exception ex)
            {
                //throw ex;
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
            }
            Response.End();

        }
    }
    // ------------------------------------------------------------------------
    protected void lkFileDownload_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        string[] saParms = myControl.CommandArgument.ToString().Split('|');
        string sRecordId = "";
        string sFileType = "";
        string sFileName = "";
        string sFileExtension = "";
        string sFullPhysicalPath = "";

        if (saParms.Length > 3)
        {
            sRecordId = saParms[0];
            sFileType = saParms[1];
            sFileName = saParms[2];
            sFileExtension = saParms[3];

            //string sWebLink = "";

            try
            {
                sFullPhysicalPath = fileHandler.Save_BinaryToDiskReturnPath(sRecordId);
                if (!String.IsNullOrEmpty(sFullPhysicalPath))
                {
                    Response.ClearContent();
                    //Response.ContentType = "application/ms-excel";
                    //Response.ContentType = "Application/pdf";


                    if (sFileExtension == ".jpg"
                        || sFileExtension == ".jpeg"
                        || sFileExtension == ".png"
                        || sFileExtension == ".gif"
                        || sFileExtension == ".bmp"
                        )
                        Response.ContentType = "data:Image/" + sFileExtension + ";base64,";
                    else if (sFileExtension == ".pdf")
                        Response.ContentType = "Application/pdf;base64,";
                    else if (sFileExtension == ".xlsx" || sFileExtension == ".xls")
                        Response.ContentType = "Application/x-msexcel;base64,";
                    else if (sFileExtension == ".docx" || sFileExtension == ".doc")
                        Response.ContentType = "Application/msword;base64,";
                    else // if (sFileExtension == "txt")
                        Response.ContentType = "text/plain;base64,";

                    Response.AddHeader("content-disposition", "attachment; filename= " + sFileName);
                    Response.WriteFile(sFullPhysicalPath);

                }
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
            }
            Response.End();
        }
    }
    // ------------------------------------------------------------------------
    protected void lkLoadTimestampForEdit_Click(object sender, EventArgs e)
    {
        LinkButton myControl = (LinkButton)sender;
        hfAddStampEditKeys.Value = myControl.CommandArgument.ToString().Trim();
        string[] saParms = myControl.CommandArgument.ToString().Split('|');
        lbTimestampAddOrEdit.Text = "Edit Existing Timestamp";

        string sCenter = "";
        string sTicket = "";
        string sStatusCode = "";
        string sReasonCode = "";
        string sStampDate = "";
        string sStampTime = "";
        string sTechNum = "";
        string sSource = "";

        if (saParms.Length > 7)
        {
            sCenter = saParms[0];
            sTicket = saParms[1];
            sStatusCode = saParms[2];
            sReasonCode = saParms[3];
            sStampDate = saParms[4];
            sStampTime = saParms[5];
            sTechNum = saParms[6];
            sSource = saParms[7];

            try
            {
                if (sStatusCode == "N") ddAddStampType.SelectedValue = "N";
                else if (sStatusCode == "T") ddAddStampType.SelectedValue = "T";
                else if (sStatusCode == "S") ddAddStampType.SelectedValue = "S";
                else if (sStatusCode == "C") ddAddStampType.SelectedValue = "C";
                else if (sStatusCode == "H" && sReasonCode == "IN") ddAddStampType.SelectedValue = "IN";
                else if (sStatusCode == "H" && sReasonCode == "NA") ddAddStampType.SelectedValue = "NA";
                else if (sStatusCode == "H" && sReasonCode == "WP") ddAddStampType.SelectedValue = "WP";
                else if (sStatusCode == "H" && sReasonCode == "TE") ddAddStampType.SelectedValue = "TE";
                else ddAddStampType.SelectedValue = "";

                txAddStampDate.Text = sStampDate;

                double dStampTime = 0.0;
                if (double.TryParse(sStampTime, out dStampTime) == true)
                    txAddStampTime.Text = dStampTime.ToString("00.00");

                txAddStampEmp.Text = sTechNum;

                // --------------------------------------------------------
                btToggleStampEntry.Text = "Hide Timestamp Entry";
                pnAddStamp.Visible = true;

                // Hide the other panels
                pnAddNote.Visible = false;
                pnAddFile.Visible = false;
                pnUpdTicket.Visible = false;

                // Reset other buttons to default
                btToggleNoteEntry.Text = "Add Note?";
                btToggleFileEntry.Text = "Add File?";
                btToggleTicketEntry.Text = "Upd Ticket?";

                // --------------------------------------------------------
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
            }
        }
    }
    // -------------------------------------------------------------------------------------------------------
    protected void btTimestampEntrySubmit_Click(object sender, EventArgs e)
    {
        int iStampDate = 0;
        double dStampTime = 0.0;
        int iStampEmp = 0;
        string sStampType = "";
        string sStatusCode = "";
        string sReasonCode = "";

        string sOrigCenter = "";
        int iOrigCenter = 0;
        string sOrigTicket = "";
        int iOrigTicket = 0;
        string sOrigStatusCode = "";
        string sOrigReasonCode = "";
        string sOrigStampDate = "";
        int iOrigStampDate = 0;
        string sOrigStampTime = "";
        double dOrigStampTime = 0.0;
        string sOrigTechNum = "";
        int iOrigTechNum = 0;
        string sOrigSource = "";
        int iRowsAffected = 0;
        string sDat = "";
        string sTim = "";
        string sAddOrEdit = "";
        string sResult = "";

        DateTime datTemp = new DateTime();

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        try
        {
            odbcConn.Open();
            // ---------------------------------------------------------
            if (String.IsNullOrEmpty(hfAddStampEditKeys.Value))
                sAddOrEdit = "Add";
            else
            {
                sAddOrEdit = "Edit";
                string[] saParms = hfAddStampEditKeys.Value.Split('|');

                if (saParms.Length > 7)
                {
                    sOrigCenter = saParms[0];
                    if (int.TryParse(sOrigCenter, out iOrigCenter) == false)
                        iOrigCenter = 0;
                    sOrigTicket = saParms[1];
                    if (int.TryParse(sOrigTicket, out iOrigTicket) == false)
                        iOrigTicket = 0;
                    sOrigStatusCode = saParms[2];
                    sOrigReasonCode = saParms[3];
                    sOrigStampDate = saParms[4];
                    if (int.TryParse(sOrigStampDate, out iOrigStampDate) == false)
                        iOrigStampDate = 0;
                    sOrigStampTime = saParms[5];
                    if (double.TryParse(sOrigStampTime, out dOrigStampTime) == false)
                        dOrigStampTime = 0.0;
                    sOrigTechNum = saParms[6];
                    if (int.TryParse(sOrigTechNum, out iOrigTechNum) == false)
                        iOrigTechNum = 0;
                    sOrigSource = saParms[7];
                }
            }

            // ---------------------------------------------------------

            int iCenter = 0;
            int iTicket = 0;
            int iTech = 0;
            if (int.TryParse(hfCenter.Value, out iCenter) == false)
                iCenter = -1;
            if (int.TryParse(hfTicket.Value, out iTicket) == false)
                iTicket = -1;
            //if (int.TryParse(hfStsNum.Value, out iTech) == false)
            if (int.TryParse(hfPrimaryCs1.Value, out iTech) == false)
                iTech = -1;

            if (int.TryParse(txAddStampDate.Text, out iStampDate) == false)
                iStampDate = -1;
            if (double.TryParse(txAddStampTime.Text, out dStampTime) == false)
                dStampTime = -1.0;
            if (int.TryParse(txAddStampEmp.Text, out iStampEmp) == false)
                iStampEmp = -1;

            sStampType = ddAddStampType.SelectedValue;
            DataTable dtEmp = GetEmpDetail(iStampEmp);

            if (iStampDate.ToString().Length == 8 && iStampDate > 20220601)
            {
                sDat = iStampDate.ToString("");
                if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == false)
                    datTemp = new DateTime();
            }

            if (iCenter <= 0 || iTicket <= 0)
            {
                lbMsg.Text = "Error: there is a problem with the key fields.  The stamp cannot be processed.";
            }
            else if (String.IsNullOrEmpty(ddAddStampType.SelectedValue))
            {
                lbMsg.Text = "A stamp type selection is required";
                ddAddStampType.Focus();
            }
            else if (String.IsNullOrEmpty(txAddStampDate.Text))
            {
                lbMsg.Text = "A date is required";
                txAddStampDate.Focus();
            }
            else if (datTemp == new DateTime())
            {
                lbMsg.Text = "The stamp date does not look like an actual YYYYMMDD calendar date";
                txAddStampDate.Focus();
            }
            else if (String.IsNullOrEmpty(txAddStampTime.Text))
            {
                lbMsg.Text = "A time is required";
                txAddStampTime.Focus();
            }
            else if (dStampTime == -1)
            {
                lbMsg.Text = "The time entry does to appear to be an HH.MM number";
                txAddStampTime.Focus();
            }
            else if (dStampTime >= 24.00)
            {
                lbMsg.Text = "The time entry is too high to be a valid time of day";
                txAddStampTime.Focus();
            }
            else if (String.IsNullOrEmpty(txAddStampEmp.Text))
            {
                if (ShowNewCompanyName() == true)
                    lbMsg.Text = "The Secur-Serv number of the employee involved is required";
                else
                    lbMsg.Text = "The STS number of the employee involved is required";
                txAddStampEmp.Focus();
            }
            else if (iStampEmp == -1)
            {
                if (ShowNewCompanyName() == true)
                    lbMsg.Text = "The entry for the Secur-Serv employee does not appear to be numeric";
                else
                    lbMsg.Text = "The entry for the STS employee does not appear to be numeric";
                txAddStampEmp.Focus();
            }
            else if (dtEmp.Rows.Count == 0)
            {
                if (ShowNewCompanyName() == true)
                    lbMsg.Text = "The entry for the Secur-Serv employee does not appear to be an active employee";
                else
                    lbMsg.Text = "The entry for the STS employee does not appear to be an active employee";
                txAddStampEmp.Focus();
            }
            else
            {

                sDat = iStampDate.ToString("");
                sTim = dStampTime.ToString("00.00");

                if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp) == false)
                    datTemp = new DateTime();
                if (DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " " + sTim.Substring(0, 2) + ":" + sTim.Substring(3, 2) + ":00.000", out datTemp) == false)
                    datTemp = DateTime.Now.ToLocalTime();

                if (sStampType == "N") // Notify
                {
                    sStatusCode = "N";
                    sReasonCode = "";
                }
                else if (sStampType == "T") // Travel
                {
                    sStatusCode = "T";
                    sReasonCode = "";
                }
                else if (sStampType == "S") // Start
                {
                    sStatusCode = "S";
                    sReasonCode = "";
                }
                else if (sStampType == "C") // Complete
                {
                    sStatusCode = "C";
                    sReasonCode = "";
                }
                else if (sStampType == "IN") // Hold: Incomplete
                {
                    sStatusCode = "H";
                    sReasonCode = "IN";
                }
                else if (sStampType == "NA") //  Hold: Never Arrived
                {
                    sStatusCode = "H";
                    sReasonCode = "NA";
                }
                else if (sStampType == "WP") // Hold: Parts Ordered
                {
                    sStatusCode = "H";
                    sReasonCode = "WP";
                }
                else if (sStampType == "TE") // Hold: Testing
                {
                    sStatusCode = "H";
                    sReasonCode = "TE";
                }

                // Add Timestamp
                if (sAddOrEdit == "Add")
                {
                    TimestampHandler timestampHandler = new TimestampHandler(sLibrary);
                    sResult = timestampHandler.ProcessTimestamp(iStampEmp, iCenter, iTicket, sStatusCode, sReasonCode, datTemp);
                    timestampHandler = null;
                }
                else
                {
                    if (sOrigSource == "TIMESTMP")
                    {
                        iRowsAffected = Update_Timestamp(
                          sStatusCode
                        , sReasonCode
                        , iStampDate
                        , dStampTime
                        , iStampEmp
                        , iOrigCenter
                        , iOrigTicket
                        , sOrigStatusCode
                        , sOrigReasonCode
                        , iOrigStampDate
                        , dOrigStampTime
                        , iOrigTechNum
                        );
                        if (iRowsAffected > 0) sResult = "SUCCESS";
                    }
                    else // TSKDTA
                    {
                        iRowsAffected = Update_TskDta(
                          sStatusCode
                        , sReasonCode
                        , iStampDate
                        , dStampTime
                        , iStampEmp
                        , iOrigCenter
                        , iOrigTicket
                        , sOrigStatusCode
                        , sOrigReasonCode
                        , iOrigStampDate
                        , dOrigStampTime
                        , iOrigTechNum
                        );
                        if (iRowsAffected > 0) sResult = "SUCCESS";
                    }
                }
                sResult = "SUCCESS"; // Just hard code for now...
                if (sResult != "")  // Just reload regardless of what comes back
                {
                    DataTable dt = ws_Get_SRTicketTimestamps(iCenter, iTicket, iTech);
                    if (dt.Rows.Count > 0)
                    {
                        rp_B1TimestampSmall.DataSource = dt;
                        rp_B1TimestampSmall.DataBind();
                        rpUpd_Timestamps();

                        gv_B1TimestampLarge.DataSource = dt;
                        gv_B1TimestampLarge.DataBind();
                        gvUpd_Timestamps();

                        pn_B1TimestampSmall.Visible = true;
                        pn_B1TimestampLarge.Visible = true;

                    }
                    else
                    {
                        pn_B1TimestampSmall.Visible = false;
                        pn_B1TimestampLarge.Visible = false;
                    }

                    if (sAddOrEdit == "Add")
                    {
                        lbMsg.Text = "SUCCESS: Timestamp Created (" + ddAddStampType.SelectedItem.ToString() + " added to ticket)";
                    }
                    else
                    {
                        lbMsg.Text = "SUCCESS: Timestamp Updated";
                    }

                    ddAddStampType.SelectedIndex = 0;
                    txAddStampDate.Text = "";
                    txAddStampTime.Text = "";
                    txAddStampEmp.Text = "";

                    pnAddStamp.Visible = false;
                    btToggleStampEntry.Text = "Add Timestamp?";
                }
                else
                {

                    if (sAddOrEdit == "Add")
                    {
                        lbMsg.Text = "Error: Timestamp creation attempt failed. ";
                    }
                    else
                    {
                        lbMsg.Text = "Error: Timestamp update attempt failed. ";
                    }

                }

                // --------------------------------------------------------------------------------
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcConn.Close();
            // Ensure add or edit, the edit hold field is cleared!
            hfAddStampEditKeys.Value = "";
            lbTimestampAddOrEdit.Text = "";
        }
    }

    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
