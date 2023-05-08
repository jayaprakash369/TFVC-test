using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;
using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;

public partial class public_sc_TicketServiceRating : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;
    char[] cSplitter = { '|' };
    string sConnectionString = "";
    string sSql = "";
    //   string sLibrary = "OMtDTALIB";
       // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // ------------------------------------------------------------------------
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        //sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        lbMsg.Text = "";
        string sEncrypted = "";
        int[] iaCtrTck = new int[2];
        int iCenter = 0;
        int iTicket = 0;
        DateTime NewNameDate = DateTime.Now;
        string newNameDt = NewNameDate.ToString("yyyyMMdd");
        int inewNameDt = int.Parse(newNameDt);
     //  string key = "cluBOkmpEs";

        if (!IsPostBack)
        {
            odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

            try
            {

                if (Request.QueryString["key"] != null && Request.QueryString["key"].ToString() != "")
                    sEncrypted = Request.QueryString["key"];

                if ((sEncrypted == null) || (sEncrypted == ""))
                {
                    lbMsg.Text = "A key must be passed to access service rating... ";
                }
                else
                {

                    odbcConn.Open();

                    if (Page.User.Identity.IsAuthenticated)
                    {
                        hfUserName.Value = User.Identity.Name;
                    }
                    else
                    {
                        hfUserName.Value = "";
                        hfStsNum.Value = "";
                    }

                    iaCtrTck = GetTicketDecrypted(sEncrypted);
                    
                    if (iaCtrTck.Length > 1 && iaCtrTck[0] > 0 && iaCtrTck[1] > 0)
                    {
                        iCenter = iaCtrTck[0];
                        iTicket = iaCtrTck[1];

                       hfCenter.Value = iCenter.ToString();
                        hfTicket.Value = iTicket.ToString();
                        if (inewNameDt >= 20230424)
                            pnSecur.Visible = true;
                        else
                            pnScantron.Visible = true; 
                    }
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
    }
    // -------------------------------------------------
    protected void ResetPage()
    {
       pnScantron.Visible = false;
       pnSecur.Visible = false;   
    }
    // ========================================================================
    protected int Write_Comment(string rateComment, int recSeq)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        int iRowsAffected = 0;
        string sResult = "";
        string tckTck = "";
        string tckCtr = "";
        int tckTckN = 0;
        int tckCtrN = 0;

        if (hfCenter.Value == "")
            tckCtrN = 0;
        else
        {
            tckCtr = hfCenter.Value;
            tckCtrN = int.Parse(tckCtr);
        }

        if (hfTicket.Value == "")
            tckTckN = 0;
        else
        {
            tckTck = hfTicket.Value;
            tckTckN = int.Parse(tckTck);
        }

        string jobName = "";
        try
        {
            odbcConn.Open();

            sSql = "insert into " + sLibrary + ".CBDATA2" +
                " (CBCTR2" +
                ", CBTKT2" +
                ", CBSEQ2" +
                ", CBCMNT" +
                ", CBJOB2 " +
               ")" +
               " values(?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@parm1", tckCtr);
            odbcCmd.Parameters.AddWithValue("@parm2", tckTck);
            odbcCmd.Parameters.AddWithValue("@parm3", recSeq);
            odbcCmd.Parameters.AddWithValue("@parm4", rateComment);
            odbcCmd.Parameters.AddWithValue("@parm4", jobName);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text = "A unexpected system error has occurred";
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
            odbcConn.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Write_Rating()
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        int iRowsAffected = 0;
        string sResult = "";
        string tckCtr = "";
        int tckCtrN = 0;
        string tckTck = "";
        int tckTckN = 0;
        string custA = "";
        string locA = "";
        int custN = 0;
        int locN = 0;
        int cstID = 0;

        if (hfCenter.Value == "")
            tckCtrN = 0;
        else
        {
            tckCtr = hfCenter.Value;
            tckCtrN = int.Parse(tckCtr);
        }

        if (hfTicket.Value == "")
            tckTckN = 0;
        else
        {
            tckTck = hfTicket.Value;
            tckTckN = int.Parse(tckTck);
        }
      
        int servRateA = int.Parse(rblTickRating.SelectedValue);
    
        if (hfStsNum.Value == "")
            cstID = 2070;
        else
            cstID = int.Parse(hfStsNum.Value);

        DateTime datTemp;
        datTemp = DateTime.Now;      
        string sDate = datTemp.ToString("yyyyMMdd");
        string sHours = datTemp.Hour.ToString();
        string sMinutes = datTemp.Minute.ToString();
        string sSeconds = datTemp.Second.ToString();
        string sTime = sHours + sMinutes + sSeconds;
        string sTimeF = datTemp.ToString("HH:mm:ss");
        string sDateF = DateTime.Now.Date.ToString("MM/dd/yyyy");

        try
        {
            odbcConn.Open();
            sResult = Get_Customer(tckCtrN, tckTckN);
            int pos = sResult.IndexOf("|");
            if (pos > 0)
            {
                custA = sResult.Substring(0, pos);
                locA = sResult.Substring(pos + 1);
                custN = int.Parse(custA);
                locN = int.Parse(locA);
                sResult = "";
            }           

            sSql = "insert into " + sLibrary + ".CBDATA1" +
                " (CBCNTR" +
                ", CBTCKT" +
                ", CBCS1" +
                ", CBCS2" +
                ", CBEMPL" +
                ", CBDATE" +
                ", CBRGTM" +
                ", CBFMTM" +
                ", CBFMDT" +
                ", CBSCOR" +
                ", CBAL20" +
               ")" +
               " values(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@parm1", tckCtr);
            odbcCmd.Parameters.AddWithValue("@parm2", tckTck);
            odbcCmd.Parameters.AddWithValue("@parm3", custN);
            odbcCmd.Parameters.AddWithValue("@parm4", locN);
            odbcCmd.Parameters.AddWithValue("@parm5", cstID);
            odbcCmd.Parameters.AddWithValue("@parm6", sDate);
            odbcCmd.Parameters.AddWithValue("@parm7", sTime);
            odbcCmd.Parameters.AddWithValue("@parm9", sTimeF);
            odbcCmd.Parameters.AddWithValue("@parm8", sDateF);
            odbcCmd.Parameters.AddWithValue("@parm10", servRateA);
            odbcCmd.Parameters.AddWithValue("@parm11", "DONE");
            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text = "Sorry we are having issues processing your response";
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
            odbcConn.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    public string Get_Customer(int center, int ticket)
    {
        string sCstLoc = "";

        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                " STCUS1, STCUS2" +
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
                sCstLoc = dt.Rows[0]["STCUS1"].ToString().Trim() + "|" + dt.Rows[0]["STCUS2"].ToString().Trim();
            }

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return sCstLoc;
    }

    // ------------------------------------------------------------------------
    protected string Validate_Comment()
    {
        string sResult = "";
        lbMsg.Text = "";
        string txComment = scrub(TextBox1.Text);

        try
        {

            if (rblTickRating.SelectedIndex < 0)
                lbMsg.Text = "A rating from 1- 10 is Required";

            if (txComment != "")
            {               
                if (txComment.Length > 1000)
                {
                    if (lbMsg.Text != "") lbMsg.Text += "<br />";
                    lbMsg.Text = "Comments must be 1000 characters or less";
                    txComment = TextBox1.Text.Substring(0, 1000);
                    TextBox1.Focus();
                }
            }

            // -------------------
            if (lbMsg.Text == "")
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbMsg.Text = "A unexpected system error has occurred";
        }
        finally
        {
        }
        // --------------------------------
        return sResult;
    }
   
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ------------------------------------------------------------------------
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string sValid = Validate_Comment();
        int iCommentLen = 0;
        int iSeq = 1;
        int jj = 0;
        int charLeft = 0;
        string tckComment = TextBox1.Text.Trim();
        string sGeneral = "";
        int RecUpdated = 0;

        if (sValid == "VALID")
        {

            try
            {
                RecUpdated = Write_Rating();
                iCommentLen = tckComment.Length;
                if (iCommentLen >= 75)
                {
                    jj = iCommentLen / 75;
                    charLeft = iCommentLen - (jj * 75);
                    for (int i = 0; i < jj; i++)
                    {
                        sGeneral = "";
                        if (iCommentLen > 75)
                        {
                            sGeneral = tckComment.Substring(0, 75);
                            RecUpdated = Write_Comment(sGeneral, iSeq);
                            iSeq++;
                            tckComment = tckComment.Substring(75);
                        }
                    }
                    if (charLeft > 0)
                    {
                        sGeneral = tckComment;
                        RecUpdated = Write_Comment(sGeneral, iSeq);
                    }
                }
                else
                {
                    sGeneral = tckComment;
                    RecUpdated = Write_Comment(sGeneral, iSeq);                   
                }                

                lbMsg.Text = "Your feedback is important.  Thank you for your response!";
                ResetPage();
            }
            catch (Exception ex)
            {
                SaveError(ex.Message.ToString(), ex.ToString(), "");
                lbMsg.Text = "Error: There was a problem processing your response";
            }
            finally
            {
            }
        }
    }
    // ------------------------------------------------------------------------
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ========================================================================

}
