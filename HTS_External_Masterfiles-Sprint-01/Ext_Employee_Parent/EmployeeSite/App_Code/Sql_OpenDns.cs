using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for Sql_OpenDns
/// Database Retrieval for Open DNS
/// </summary>
public class Sql_OpenDns
{
    // ========================================================================
    // Global Variables
    // ========================================================================
    string sLibrary = "";

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler erh;

    string sConnectionString = "";
    string sSql = "";

    string sMethodName = "";

    // ========================================================================
    // Constructor
    // ========================================================================
    public Sql_OpenDns()
    {
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;

    }
    // ========================================================================
    // PUBLIC METHODS
    // ========================================================================
    // ========================================================================
    #region mySqls
    // ========================================================================
    public DataTable GetOpenDnsCustomers()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                 " ocDnsCs1" +
                ", ocHtsCs1" +
                ", trim(ocDnsNam) as ocDnsNam" +
                ", trim(ocHtsNam) as ocHtsNam" +
                //" FROM " + sLibrary + ".MRODNSCS1" +
                " FROM STEVEC.MRODNSCS1" +
                " Order By ocDnsNam";

            //EmailHandler emh = new EmailHandler();
            //emh.EmailIndividual("Cust SQL", "SQL IS: " + sSql, "htslog@yahoo.com", "adv320@harlandts.com", "HTML");
            //emh = null;

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
/*
            int iCs1 = 0;
            int iRowIdx = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(dt.Rows[iRowIdx]["ocHtsCs1"].ToString().Trim(), out iCs1) == false)
                    iCs1 = -1;

                iRowIdx++;
            }
*/ 
        }
        catch (Exception ex)
        {
            erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sSql);
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return dt;
    }
    // ========================================================================
    public DataTable GetOpenDnsCategories()
    {
        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                 " ocCode" +
                ", trim(ocTitle) as ocTitle" +
                ", trim(ocDesc) as ocDesc" +
                //" FROM " + sLibrary + ".MRODNSCAT" +
                " FROM STEVEC.MRODNSCAT" +
                " where ocTitle not like 'Custom Integration%'" +
                " and ocTitle not like 'Partner Integration%'" +
                " Order By ocTitle";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            /*
                        int iCode = 0;
                        int iRowIdx = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (int.TryParse(dt.Rows[iRowIdx]["ocCode"].ToString().Trim(), out iCode) == false)
                                iCode = -1;

                            iRowIdx++;
                        }
            */
        }
        catch (Exception ex)
        {
            erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sSql);
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return dt;
    }
    // ========================================================================
    public string GetOpenDnsCategoryTitle(int code)
    {
        string sTitle = "";

        sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        odbcConn = new OdbcConnection(sConnectionString);

        try
        {
            odbcConn.Open();

            sSql = "Select" +
                " trim(ocTitle) as ocTitle" +
                //" FROM " + sLibrary + ".MRODNSCAT" +
                " FROM STEVEC.MRODNSCAT" +
                " where ocCode = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Code", code);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            if (dt.Rows.Count > 0)
                sTitle = dt.Rows[0]["ocTitle"].ToString().Trim();
        }
        catch (Exception ex)
        {
            erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sSql);
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
            odbcConn.Close();
        }
        return sTitle;
    }
     // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
}