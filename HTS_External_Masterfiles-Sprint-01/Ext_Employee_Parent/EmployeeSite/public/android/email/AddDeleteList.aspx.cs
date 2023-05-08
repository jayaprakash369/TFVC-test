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

public partial class public_android_email_AddDeleteList : MyPage
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
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); df = new DateFormatter();  }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; df = null; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        if (!IsPostBack)
        {
            int iTech = 0;
            if (Request.QueryString["tech"] != null && Request.QueryString["tech"].ToString() != "") 
            { 
                if (int.TryParse(Request.QueryString["tech"].ToString().Trim(), out iTech) == false) 
                    iTech = 0; 
            }

            if (iTech > 0)
            {
                try
                {
                    odbcConn.Open();
                    GetAddDelete(iTech);
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
        Response.End();
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected void GetAddDelete(int tech)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                 " RWEMPNAM" +
                ", RWREGION" +
                ", RWPRITEC" +
                ", RWCUST#1" +
                ", RWCUST#2" +
                ", RWCUSTNM" +
                ", RWSADDR1" +
                ", RWCITY" +
                ", RWSTATE" +
                ", RWZIPCD" +
                ", RWHPHONE" +
                ", RWCONTNM" +
                ", RWAGREE#" +
                ", RWAGRDSC" +
                ", RWACTION" +
                ", RWPART#" +
                ", RWSER#" +
                ", RWUNIT#" +
                ", RWEQPTC" +
                ", RWDTADD" +
                ", RWDTAMM" +
                ", RWDTAYY" +
                ", RWEQPRA" +
                ", RWUSERID" +
                ", RWEQPSBT" +
                ", RWSLSMN#" +
                ", RWRGNSLS" +
                ", RWOPHONE" +
                ", RWIMFDSC" +
                ", RWDTEDD" +
                " from " + sLibrary + ".WRKADDDLT1, " + sLibrary + ".EMPMST" +
                " where RWPRITEC = ECENT" +
                " and EMPNUM = ?" +
                " and RWDTEDD > ?" +
                " and RWDTEDD <= ?" +
                " order by RWCUST#1, RWCUST#2";

            odbcCmd = new OdbcCommand(sSql, odbcConn);

            odbcCmd.Parameters.AddWithValue("@Tech", tech);

            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

//            string sTemp = "";
//            int iTemp = 0;
            int iRowIdx = 0;
            int iCs1Hold = 0;
            int iCs2Hold = 0;
            int iCs1 = 0;
            int iCs2 = 0;
            string sCs2Nam = "";
            string sAdr = "";
            string sCit = "";
            string sSta = "";
            string sZip = "";
            string sPhn = "";
            string sCon = "";
            string sAgr = "";
            string sAgrDsc = "";
            string sPrt = "";
            string sSer = "";
            string sPrtDsc = "";
            string sDat = "";
            string sRowClass = "backwhite";
            double dAmt = 0.0;
            int iDat = 0;
            DateTime datTemp;
            
            if (dt.Rows.Count > 0)
            {
                // ------------------------------------
                string sTable = "<table cellspacing=\"0\" cellpadding=\"2\" style=\"margin-bottom:20px; width: 98%; \">";
                // ------------------------------------
                foreach (DataRow row in dt.Rows)
                {
                    
                    if (int.TryParse(dt.Rows[iRowIdx]["RWCUST#1"].ToString().Trim(), out iCs1) == false)
                        iCs1 = -1;

                    if (int.TryParse(dt.Rows[iRowIdx]["RWCUST#2"].ToString().Trim(), out iCs2) == false)
                        iCs2 = -1;

                    if (int.TryParse(dt.Rows[iRowIdx]["RWDTEDD"].ToString().Trim(), out iDat) == false)
                        iDat = -1;
                    sDat = iDat.ToString();
                    if (sDat.Length == 8) 
                    {
                        DateTime.TryParse(sDat.Substring(0, 4) + "-" + sDat.Substring(4, 2) + "-" + sDat.Substring(6, 2) + " 00:00:00.000", out datTemp);
                        sDat = datTemp.ToString("MMM dd, yyyy");
                    }

                    sPrt = dt.Rows[iRowIdx]["RWPART#"].ToString().Trim();
                    sSer = dt.Rows[iRowIdx]["RWSER#"].ToString().Trim();
                    sPrtDsc = dt.Rows[iRowIdx]["RWIMFDSC"].ToString().Trim();

                    // Do customer header divider
                    if (iCs1 != iCs1Hold || iCs2 != iCs2Hold) 
                    {
                        sCs2Nam = dt.Rows[iRowIdx]["RWCUSTNM"].ToString().Trim();
                        sAdr = dt.Rows[iRowIdx]["RWSADDR1"].ToString().Trim();
                        sCit = dt.Rows[iRowIdx]["RWCITY"].ToString().Trim();
                        sSta = dt.Rows[iRowIdx]["RWSTATE"].ToString().Trim();
                        sZip = dt.Rows[iRowIdx]["RWZIPCD"].ToString().Trim();
                        sPhn = dt.Rows[iRowIdx]["RWHPHONE"].ToString().Trim();
                        sCon = dt.Rows[iRowIdx]["RWCONTNM"].ToString().Trim();
                        sAgr = dt.Rows[iRowIdx]["RWAGREE#"].ToString().Trim();
                        sAgrDsc = dt.Rows[iRowIdx]["RWAGRDSC"].ToString().Trim();

                        if (!String.IsNullOrEmpty(sZip) && sZip.Length > 5)
                            sZip = sZip.Substring(0, 5);
                        sTable += "<tr class='backblue'>" +
                            "<td>" + sCs2Nam + "</td>" +
                            "</tr>";
                        sTable += "<tr class='backblue'>" +
                            "<td>" + "Customer " + iCs1.ToString() + "-" + iCs2.ToString() + "</td>" +
                            "</tr>";
                        sTable += "<tr class='backblue'>" +
                            "<td style=\"padding-bottom: 10px;\">" + sCit + ", " + sSta + " " + sZip + "</td>" +
                            "</tr>";

                        sRowClass = "backwhite";
                    }

                    //if (double.TryParse(dt.Rows[iRowIdx]["RWEQPRA"].ToString().Trim(), out dAmt) == false)
                    //    dAmt = -1;

                    //if (sRowClass == "backwhite")
                    //    sRowClass = "backgray";
                    //else 
                    //    sRowClass = "backwhite";

                    sTable += "<tr class='" + sRowClass + "'>" +
                        "<td>" + sPrt + "  (" + sSer + ")</td>" +
                        "</tr>";

                    sTable += "<tr class='" + sRowClass + "'>" +
                        "<td>" + sPrtDsc + "</td>" +
                        "</tr>";

                    sTable += "<tr class='" + sRowClass + "'>" +
                        "<td>" + sDat + "</td>" +
                        "</tr>";

                    iCs1Hold = iCs1;
                    iCs2Hold = iCs2;

                    iRowIdx++;
                }
                // ------------------------------------
                sTable += "</table>";
                string sBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                "<head><title>Service History</title>" + 
                "<style>" + 
                "tr { font-size: 16px; } " + 
                "th { font-size: 16px; } " +
                "td { font-size: 16px; vertical-align: top; } " +
                ".backblue td { background-color: #D4DFED; color: #333333;}  " + // D4DFED // B0C4DE
                ".backgray td { background-color: #eeeeee; }  " + 
                ".backwhite td { background-color: #ffffff; } " + 
                "</style>" + 
                "</head><body style=\"font-size: 1; width:99%; \">" + //  style=\"width:100%;\"
                sTable +
                "</body></html>";

                string sSbj = "Add Delete Report: (" + tech + ") " + DateTime.Now.ToString("MMM yyyy");
                string sEmpEmail = GetEmpEmail(tech);
                EmailHandler eh = new EmailHandler();
                // sEmpEmail = "htslog@yahoo.com";
                eh.EmailIndividual(sSbj, sBody, sEmpEmail, "adv320@harlandts.com", "HTML");
                eh = null;
                // ------------------------------------
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
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
}
