using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;     // for AS400 Access
using System.Configuration; // for Connection String
using System.Data;          // for DataTable

/// <summary>
/// Summary description for DbHandler
/// </summary>
public class DbHandler
{
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    string sLibrary = "";

    // ========================================================================
    public DbHandler()
    {
        //
        // TODO: Add constructor logic here
        //
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
    }
    // ==================================================
    public void LogEvent(DateTime datArrived, string eventStage, string eventKey, string textToSave)
    {
        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
        DataTable dt = new DataTable();
        
        try
        {
            odbcConn.Open();

            //string sSql = "";
            int iRowsAffected = 0;
            int iRowCount = 0;

            string sDatArrived = datArrived.ToString("o");
            string sEventStage = eventStage;
            string sEventKey = eventKey;
            string sTextToSave = textToSave;
            string sEnvironment = "";
            if (sLibrary == "OMDTALIB")
                sEnvironment = "LIVE";
            else
                sEnvironment = "TEST";

            if (sEventKey.Length > 25)
                sEventKey = sEventKey.Substring(0, 25);
            if (sEventStage == "START")
            {
                if (sTextToSave.Length > 3000)
                    sTextToSave = sTextToSave.Substring(0, 3000);
            }
            else 
            {
                if (sTextToSave.Length > 350)
                    sTextToSave = sTextToSave.Substring(0, 350);
            }

            if (sEventStage == "START")
            {
                iRowCount = selectEvent(sDatArrived);

                if (iRowCount == 0) // Insert new record
                {
                    iRowsAffected = insertEventStart(sDatArrived, sEnvironment);

                    if (iRowsAffected > 0) 
                    {
                        iRowsAffected = updateEventStart(sTextToSave, sDatArrived);
                    }

                }
                else // just update existing record (this should never happen on a start event...)
                {
                    iRowsAffected = updateEventStart(sTextToSave, sDatArrived);
                }

            }
            else // This is and "END" event
            {
                iRowsAffected = updateEventEnd(sEventKey, sTextToSave, sDatArrived);
            }
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            //odbcCmd.Dispose();
            odbcConn.Close();
        }
        // ---------------------------------------
    }
    // ========================================================================
    protected int selectEvent(string datArrived)
    {
        int iRowCount = 0;

        DataTable dt = new DataTable();

        //int iRowsAffected = 0;
        try
        {
            string sSql = "Select" +
                " EVSTAMP" +
                " from STEVEC.KBEVENTS" +
                " where EVSTAMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Stamp", datArrived);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);

            iRowCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowCount;
    }

    // ========================================================================
    protected int insertEventStart(string datArrived, string environment)
    {
        int iRowsAffected = 0;

        DataTable dt = new DataTable();

        try
        {
            // first just load a record at that time to ensure you have some evidence of an event at that time
            string sSql = "insert into STEVEC.KBEVENTS" +
                " (EVSTAMP, EVENVIR)" +
                " values (?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Stamp", datArrived);
            odbcCmd.Parameters.AddWithValue("@Envir", environment);
            iRowsAffected = odbcCmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int updateEventStart(string inboundXml, string datArrived)
    {
        int iRowsAffected = 0;

        DataTable dt = new DataTable();

        try
        {
            string sSql = "update STEVEC.KBEVENTS set" +
             " EVKBXML = ?" +
            " where EVSTAMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Xml", inboundXml);
            odbcCmd.Parameters.AddWithValue("@Stamp", datArrived);
            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int updateEventEnd(string eventKey, string textToSave, string datArrived)
    {
        int iRowsAffected = 0;

        DataTable dt = new DataTable();

        try
        {
            string sSql = "update STEVEC.KBEVENTS set" +
                 " EVKBTCK = ?" +
                ", EVKBEND = ?" +
                " where EVSTAMP = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", eventKey);
            odbcCmd.Parameters.AddWithValue("@Result", textToSave);
            odbcCmd.Parameters.AddWithValue("@Stamp", datArrived);
            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ErrorHandler erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    // ========================================================================
}