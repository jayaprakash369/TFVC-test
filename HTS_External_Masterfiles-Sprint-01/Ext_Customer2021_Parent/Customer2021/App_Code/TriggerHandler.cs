using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for TriggerHandler
/// </summary>
public class TriggerHandler
{
    // ---------------------------------------
    // GLOBAL VARIABLES (Changed for roll test)  
    // ---------------------------------------

    //    SqlConnection sqlConn;  
    //    SqlCommand sqlCmd; 
    //    SqlDataReader sqlReader;

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    private string sLibrary = "OMDTALIB";
    //private string sTemp = "";

    DateTime datNow;
    // ========================================================================
    public TriggerHandler(string DEV_TEST_LIVE)
    {
        //
        // TODO: Add constructor logic here
        //

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        DEV_TEST_LIVE = DEV_TEST_LIVE.ToUpper();
        datNow = DateTime.Now;

        if (DEV_TEST_LIVE == "LIVE")
        {
            sLibrary = "OMDTALIB";
        }
        else if (DEV_TEST_LIVE == "TEST")
        {
            sLibrary = "OMTDTALIB";
        }
        else // DEV, KOI, ISA etc
        {
            sLibrary = "OMTDTALIB";
        }
    }
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    public TriggerObject Get_EmptyTriggerObjectToLoad()
    {
        return Initialize_TriggerObject();
    }
    // ========================================================================
    public TriggerObject Save_ObjectToTriggerFile(TriggerObject triggerObject)
    {

        try
        {
            odbcConn.Open();  // Open on your "gate" methods (normally public)

            triggerObject = Insert_ObjectIntoT2Parms(triggerObject);

            if (String.IsNullOrEmpty(triggerObject.processingError))
            {

                triggerObject = Insert_ObjectIntoT2Trigger(triggerObject);
                triggerObject = Update_ObjectWithReturnedT2Parms(triggerObject);
                int iRowsAffected = 0;
                // Since the return values are passed back in the object, you can clean up the file. 
                // If calls from other sites/web services use the files, you may need the expiration dates to clean up (so you may need to comment later)
                iRowsAffected = Delete_T2TriggerRecord(triggerObject.key); // Always ok to delete small trigger file
                iRowsAffected = Delete_T2ParmsRecord(triggerObject.key);
            }
        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcConn.Close();
        }

        return triggerObject;
    }
    // ========================================================================
    protected TriggerObject Initialize_TriggerObject()
    {
        TriggerObject to = new TriggerObject();

        to.DEV_TEST_LIVE = "";
        to.key = 0;
        to.stringToSend_10K = "";
        //to.creationStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK");
        //to.expirationStamp = DateTime.Now.AddMinutes(10).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK");
        to.programToRun = "";

        to.stringToSend_01 = "";
        to.stringToSend_02 = "";
        to.stringToSend_03 = "";
        to.stringToSend_04 = "";
        to.stringToSend_05 = "";
        to.stringToSend_06 = "";
        to.stringToSend_07 = "";
        to.stringToSend_08 = "";
        to.stringToSend_09 = "";

        to.doubleToSend_01 = 0;
        to.doubleToSend_02 = 0;
        to.doubleToSend_03 = 0;
        to.doubleToSend_04 = 0;
        to.doubleToSend_05 = 0;
        to.doubleToSend_06 = 0;
        to.doubleToSend_07 = 0;
        to.doubleToSend_08 = 0;
        to.doubleToSend_09 = 0;


        to.stringToSend_10K = "";

        to.stringReturned = "";

        //to.doubleArrayReturned = new double[5];
        to.doubleReturned_01 = 0.0;
        to.doubleReturned_02 = 0.0;
        to.doubleReturned_03 = 0.0;
        to.doubleReturned_04 = 0.0;
        to.doubleReturned_05 = 0.0;

        to.processingError = "";


        return to;
    }
    // ========================================================================
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    #region mySqls
    // ========================================================================
    private TriggerObject Insert_ObjectIntoT2Parms(TriggerObject to)
    {

        int iRowsAffected = 0;
        string sSql = "";
        string sSql2 = "";

        KeyRetriever kq = new KeyRetriever(sLibrary);
        to.key = kq.GetKey("T2TRIGGER");
        kq = null;
        
        MyPage myPage = new MyPage();

        try
        {
            if (!String.IsNullOrEmpty(to.stringToSend_01) && to.stringToSend_01.Length > 100) to.stringToSend_01 = to.stringToSend_01.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_02) && to.stringToSend_02.Length > 100) to.stringToSend_02 = to.stringToSend_02.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_03) && to.stringToSend_03.Length > 100) to.stringToSend_03 = to.stringToSend_03.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_04) && to.stringToSend_04.Length > 100) to.stringToSend_04 = to.stringToSend_04.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_05) && to.stringToSend_05.Length > 100) to.stringToSend_05 = to.stringToSend_05.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_06) && to.stringToSend_06.Length > 100) to.stringToSend_06 = to.stringToSend_06.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_07) && to.stringToSend_07.Length > 100) to.stringToSend_07 = to.stringToSend_07.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_08) && to.stringToSend_08.Length > 100) to.stringToSend_08 = to.stringToSend_08.Substring(0, 100);
            if (!String.IsNullOrEmpty(to.stringToSend_09) && to.stringToSend_09.Length > 100) to.stringToSend_09 = to.stringToSend_09.Substring(0, 100);

            // Double array will always be numbers (never null)

            if (!String.IsNullOrEmpty(to.stringToSend_10K)) { if (to.stringToSend_10K.Length > 10000) to.stringToSend_10K = to.stringToSend_10K.Substring(0, 10000); }

            // Replace any NON-AS400 characters from error log here! (which are causing crashes)

            to.stringToSend_01 = to.stringToSend_01.Replace("–", "-");
            to.stringToSend_01 = to.stringToSend_01.Replace("•", "-");
            to.stringToSend_01 = to.stringToSend_01.Replace("’", "'");
            to.stringToSend_01 = to.stringToSend_01.Replace("”", "\"");
            to.stringToSend_01 = myPage.scrub(to.stringToSend_01.Replace("”", "\""));

            to.stringToSend_02 = to.stringToSend_02.Replace("–", "-");
            to.stringToSend_02 = to.stringToSend_02.Replace("•", "-");
            to.stringToSend_02 = to.stringToSend_02.Replace("’", "'");
            to.stringToSend_02 = to.stringToSend_02.Replace("”", "\"");
            to.stringToSend_02 = myPage.scrub(to.stringToSend_02.Replace("”", "\""));

            to.stringToSend_10K = to.stringToSend_10K.Replace("–", "-");
            to.stringToSend_10K = to.stringToSend_10K.Replace("•", "-");
            to.stringToSend_10K = to.stringToSend_10K.Replace("’", "'");
            to.stringToSend_10K = to.stringToSend_10K.Replace("”", "\"");
            to.stringToSend_10K = myPage.scrub(to.stringToSend_10K.Replace("”", "\""));

            //// Don't think I need to do this yet...
            //to.stringToSend_10K = HttpUtility.HtmlEncode(to.stringToSend_10K);

            sSql = "insert into " + sLibrary + ".T2PARMS (" +
                  "T_KEY" + // newKey
                ", T_RUNPGM" + // Program to run this particular job
                ", T_ENVIRO" + // Dev, Test, Live
                               //", T_CREATE" + // When created by this class
                               //", T_EXPIRE" + // When to expire record, 10 minutes, if by this class, but user can decide and update field
                ", T_TXT_01" +
                ", T_TXT_02" +
                ", T_TXT_03" +
                ", T_TXT_04" +
                ", T_TXT_05" +
                ", T_TXT_06" +
                ", T_TXT_07" +
                ", T_TXT_08" +
                ", T_TXT_09" +
                ", T_NUM_01" +
                ", T_NUM_02" +
                ", T_NUM_03" +
                ", T_NUM_04" +
                ", T_NUM_05" +
                ", T_NUM_06" +
                ", T_NUM_07" +
                ", T_NUM_08" +
                ", T_NUM_09" +
                ", T_BIG" +
                ", T_BAKALP" +
                ", T_BAKNM1" +
                ", T_BAKNM2" +
                ", T_BAKNM3" +
                ", T_BAKNM4" +
                ", T_BAKNM5" +
                ") values(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"; // 30 so far


            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", to.key);
            odbcCmd.Parameters.AddWithValue("@Run", to.programToRun);
            odbcCmd.Parameters.AddWithValue("@Env", to.DEV_TEST_LIVE);
            //odbcCmd.Parameters.AddWithValue("@Create", to.creationStamp);
            //odbcCmd.Parameters.AddWithValue("@Expire", to.expirationStamp);
            odbcCmd.Parameters.AddWithValue("@Tx1", to.stringToSend_01.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx2", to.stringToSend_02.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx3", to.stringToSend_03.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx4", to.stringToSend_04.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx5", to.stringToSend_05.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx6", to.stringToSend_06.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx7", to.stringToSend_07.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx8", to.stringToSend_08.Trim());
            odbcCmd.Parameters.AddWithValue("@Tx9", to.stringToSend_09.Trim());
            odbcCmd.Parameters.AddWithValue("@Nm1", to.doubleToSend_01);
            odbcCmd.Parameters.AddWithValue("@Nm2", to.doubleToSend_02);
            odbcCmd.Parameters.AddWithValue("@Nm3", to.doubleToSend_03);
            odbcCmd.Parameters.AddWithValue("@Nm4", to.doubleToSend_04);
            odbcCmd.Parameters.AddWithValue("@Nm5", to.doubleToSend_05);
            odbcCmd.Parameters.AddWithValue("@Nm6", to.doubleToSend_06);
            odbcCmd.Parameters.AddWithValue("@Nm7", to.doubleToSend_07);
            odbcCmd.Parameters.AddWithValue("@Nm8", to.doubleToSend_08);
            odbcCmd.Parameters.AddWithValue("@Nm9", to.doubleToSend_09);
            odbcCmd.Parameters.AddWithValue("@Big", to.stringToSend_10K);
            odbcCmd.Parameters.AddWithValue("@RtnAlp", to.stringReturned);
            // These fields don't really need to be inserted yet because they're not loaded, just initializing
            odbcCmd.Parameters.AddWithValue("@RtnNum1", to.doubleReturned_01);
            odbcCmd.Parameters.AddWithValue("@RtnNum2", to.doubleReturned_02);
            odbcCmd.Parameters.AddWithValue("@RtnNum3", to.doubleReturned_03);
            odbcCmd.Parameters.AddWithValue("@RtnNum4", to.doubleReturned_04);
            odbcCmd.Parameters.AddWithValue("@RtnNum5", to.doubleReturned_05);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sSql2 =
    "Key: " + to.key +
    "-- Run: " + to.programToRun +
    "-- Env: " + to.DEV_TEST_LIVE +
    //"-- Cre: " + to.creationStamp +
    //"-- Exp: " + to.expirationStamp +
    "-- Tx1: " + to.stringToSend_01.Trim() +
    "-- Tx2: " + to.stringToSend_02.Trim() +
    "-- Tx3: " + to.stringToSend_03.Trim() +
    "-- Tx4: " + to.stringToSend_04.Trim() +
    "-- Tx5: " + to.stringToSend_05.Trim() +
    "-- Tx6: " + to.stringToSend_06.Trim() +
    "-- Tx7: " + to.stringToSend_07.Trim() +
    "-- Tx8: " + to.stringToSend_08.Trim() +
    "-- Tx9: " + to.stringToSend_09.Trim() +
    "-- Nm1: " + to.doubleToSend_01 +
    "-- Nm2: " + to.doubleToSend_02 +
    "-- Nm3: " + to.doubleToSend_03 +
    "-- Nm4: " + to.doubleToSend_04 +
    "-- Nm5: " + to.doubleToSend_05 +
    "-- Nm6: " + to.doubleToSend_06 +
    "-- Nm7: " + to.doubleToSend_07 +
    "-- Nm8: " + to.doubleToSend_08 +
    "-- Nm9: " + to.doubleToSend_09 +
    "-- RtnAlp: " + to.stringReturned +
    "-- Rtn1: " + to.doubleReturned_01 +
    "-- Rtn2: " + to.doubleReturned_02 +
    "-- Rtn3: " + to.doubleReturned_03 +
    "-- Rtn4: " + to.doubleReturned_04 +
    "-- Rtn5: " + to.doubleReturned_05 +
    "-- Big10K: " + to.stringToSend_10K;

            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Sql Values: " + sSql2 + " ----- Sql: " + sSql);
            to.processingError = to.processingError += sSql2;
        }
        finally
        {
            odbcCmd.Dispose();
            myPage = null;
        }
        return to;
    }
    // ========================================================================
    private TriggerObject Insert_ObjectIntoT2Trigger(TriggerObject to)
    {
        int iRowsAffected = 0;
        string sSql = "";
        string sSql2 = "";

        try
        {
            sSql = "insert into " + sLibrary + ".T2TRIGGER (" +
                  "T_KEY" + // newKey
                ", T_RUN" + // Program to run this particular job
                ", T_ENV" + // Dev, Test, Live
                ") values(?, ?, ?)"; // 3 so far


            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", to.key);
            odbcCmd.Parameters.AddWithValue("@Run", to.programToRun);
            odbcCmd.Parameters.AddWithValue("@Env", to.DEV_TEST_LIVE);


            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sSql2 =
                "Key: " + to.key +
                "-- Run: " + to.programToRun +
                "-- Env: " + to.DEV_TEST_LIVE;

            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Sql Values: " + sSql2 + " ----- Sql: " + sSql);
            to.processingError = to.processingError += sSql2;
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return to;
    }
    // ========================================================================
    private TriggerObject Update_ObjectWithReturnedT2Parms(TriggerObject to)
    {
        string sSql = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {

            sSql = "Select" +
             " T_BAKNM1" +
            ", T_BAKNM2" +
            ", T_BAKNM3" +
            ", T_BAKNM4" +
            ", T_BAKNM5" +
            ", T_BAKALP" +
            " from " + sLibrary + ".T2PARMS" +
            " where T_Key = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", to.key);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            double dTemp = 0;
            if (dt.Rows.Count > 0)
            {
                to.stringReturned = dt.Rows[0]["T_BAKALP"].ToString();

                if (double.TryParse(dt.Rows[0]["T_BAKNM1"].ToString(), out dTemp) == false)
                    dTemp = 0.0;
                to.doubleReturned_01 = dTemp;

                if (double.TryParse(dt.Rows[0]["T_BAKNM2"].ToString(), out dTemp) == false)
                    dTemp = 0.0;
                to.doubleReturned_02 = dTemp;

                if (double.TryParse(dt.Rows[0]["T_BAKNM3"].ToString(), out dTemp) == false)
                    dTemp = 0.0;
                to.doubleReturned_03 = dTemp;

                if (double.TryParse(dt.Rows[0]["T_BAKNM4"].ToString(), out dTemp) == false)
                    dTemp = 0.0;
                to.doubleReturned_04 = dTemp;

                if (double.TryParse(dt.Rows[0]["T_BAKNM5"].ToString(), out dTemp) == false)
                    dTemp = 0.0;
                to.doubleReturned_05 = dTemp;
            }

        }
        catch (Exception ex)
        {
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return to;
    }
    // ========================================================================
    private int Delete_T2TriggerRecord(int key)
    {
        int iRowsAffected = 0;
        string sSql = "";

        try
        {
            sSql = "delete" +
                " from " + sLibrary + ".T2TRIGGER" +
                " where T_KEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", key);
            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            iRowsAffected = -1;

            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    private int Delete_T2ParmsRecord(int key)
    {
        int iRowsAffected = 0;
        string sSql = "";

        try
        {
            sSql = "delete" +
                " from " + sLibrary + ".T2PARMS" +
                " where T_KEY = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Key", key);
            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            iRowsAffected = -1;

            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    // ========================================================================
    #region myClasses
    // ========================================================================
    public class TriggerObject
    {
        public int key { get; set; }
        public string programToRun { get; set; }
        public string DEV_TEST_LIVE { get; set; }
        //        public string creationStamp { get; set; }
        //        public string expirationStamp { get; set; }
        // I Originally built this object with proper arrays, but viewing the data was cumbersome
        // and what you wanted to see (return values) was always buried
        // So I'm reverting to using individual numbers variables for easy viewing of values
        //public string[] stringArray { get; set; }
        public string stringToSend_01 { get; set; }
        public string stringToSend_02 { get; set; }
        public string stringToSend_03 { get; set; }
        public string stringToSend_04 { get; set; }
        public string stringToSend_05 { get; set; }
        public string stringToSend_06 { get; set; }
        public string stringToSend_07 { get; set; }
        public string stringToSend_08 { get; set; }
        public string stringToSend_09 { get; set; }


        //public double[] doubleArray { get; set; }
        public double doubleToSend_01 { get; set; }
        public double doubleToSend_02 { get; set; }
        public double doubleToSend_03 { get; set; }
        public double doubleToSend_04 { get; set; }
        public double doubleToSend_05 { get; set; }
        public double doubleToSend_06 { get; set; }
        public double doubleToSend_07 { get; set; }
        public double doubleToSend_08 { get; set; }
        public double doubleToSend_09 { get; set; }

        public string stringToSend_10K { get; set; }
        public string stringReturned { get; set; }

        //public double[] doubleArrayReturned { get; set; }
        public double doubleReturned_01 { get; set; }
        public double doubleReturned_02 { get; set; }
        public double doubleReturned_03 { get; set; }
        public double doubleReturned_04 { get; set; }
        public double doubleReturned_05 { get; set; }

        public string processingError { get; set; }
        //public string fieldUseOutbound { get; set; }
        //public string fieldUseReturned { get; set; }
    }
    // ========================================================================
    // ========================================================================
    #endregion // end myClasses
    // ========================================================================

    // ========================================================================
    // ========================================================================

}