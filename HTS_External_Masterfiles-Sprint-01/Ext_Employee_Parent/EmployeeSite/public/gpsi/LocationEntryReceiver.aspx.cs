using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;


public partial class public_gpsi_LocationEntryReceiver : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    SqlConnection sqlConn;
    SqlCommand sqlCmd;
    SqlDataReader sqlReader;

    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    ErrorHandler erh;
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sJson = "";

        using (var reader = new StreamReader(Request.InputStream))
            sJson = reader.ReadToEnd();

        DataTable dt = Get_LocationEntryListFromJson(sJson);
        DataTable dt2;

        string sVehicleId = "";
        string sVin = "";
        string[] saEnteredLandmarks = { "" };
        string sLandmarkId = "";
        string sDriverEmail = "";
        string sResult = "";
        string sDebug = "";

        int iLandmarkId = 0;
        int iStsNumberOfHome = 0;
        int iStsNumberOfVehicle = 0;
        int iRowsAffected = 0;

        if (dt.Rows.Count > 0) 
        {
            try 
            {
                odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);
                sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                sqlConn.Open();
                odbcConn.Open();
                sVehicleId = dt.Rows[0]["id"].ToString().Trim();
                // Vin being passed is not the car's vin, but the GSPI vehicle id (confusing, but they match now)
                sVin = dt.Rows[0]["vin"].ToString().Trim();
                saEnteredLandmarks = dt.Rows[0]["entered_landmarks"].ToString().Trim().Split('|');

                for (int i = 0; i < saEnteredLandmarks.Length; i++)
                {
                    sLandmarkId = saEnteredLandmarks[i];
                    if (int.TryParse(sLandmarkId, out iLandmarkId) == false)
                        iLandmarkId = -1;
                    if (iLandmarkId > 0) 
                    {
                        // 1) Check if landmark is in the home file
                        iStsNumberOfHome = Seek_HomeWorkfileStsNumber(iLandmarkId);
                        if (iStsNumberOfHome > 0) // Landmark is a home address, you have the emp number who lives there
                        {
                            dt2 = Seek_VehicleWorkfileRecord(sVehicleId);
                            if (dt2.Rows.Count > 0)
                            {
                                sDriverEmail = dt2.Rows[0]["vwDriverEmail"].ToString().ToLower().Trim();
                                if (int.TryParse(dt2.Rows[0]["vwStsNumber"].ToString().Trim(), out iStsNumberOfVehicle) == false)
                                    iStsNumberOfVehicle = -1;
                                if (iStsNumberOfHome == iStsNumberOfVehicle) // Vehicle driver entered his own home zone.
                                {
                                    // Email Steve (for now) 
                                    MailHandler emh = new MailHandler();
                                    string sEmailSubject = "FST Home: " + sDriverEmail + " (" + iStsNumberOfHome + ")";
                                    string sEmailBody = "LandmarkId: " + iLandmarkId + " " + DateTime.Now.ToString("MMM d yyyy, h:mm (tt)");
                                    //sResult = emh.EmailIndividual2(sEmailSubject, sEmailBody, "steve.carlson@scantron.com", "adv320@scantron.com", "HTML");
                                    /*
                                    if (

                                           sDriverEmail == "rafael.cabral@scantron.com"
                                        || sDriverEmail == "james.dollar@scantron.com"
                                        || sDriverEmail == "nel.hensen@scantron.com"
                                        || sDriverEmail == "john.curtis@scantron.com"
                                        || sDriverEmail == "raymond.dunlap@scantron.com"
                                        || sDriverEmail == "kirt.tegenkamp@scantron.com"
                                        || sDriverEmail == "paul.poole@scantron.com"
                                        || sDriverEmail == "madan.birdi@scantron.com"
                                        || sDriverEmail == "lance.gamble@scantron.com"
                                        || sDriverEmail == "lou.sullivan@scantron.com"

                                        || sDriverEmail == "dan.west@scantron.com"
                                        || sDriverEmail == "pat.hutt@scantron.com"
                                        sDriverEmail == "jeremy.glow@scantron.com"

                                    sDriverEmail == "steve.carlson@scantron.com"
                                        )
                                    {
                                        if (
                                               sDriverEmail == "paul.poole@scantron.com"
                                            || sDriverEmail == "dan.west@scantron.com"

                                            ) 
                                        {
                                            sResult = emh.EmailIndividual2(sEmailSubject, sEmailBody, sDriverEmail, "steve.carlson@scantron.com", "HTML");
                                        }
                                        
                                        sResult = emh.EmailIndividual2(sEmailSubject, sEmailBody, "steve.carlson@scantron.com", "adv320@scantron.com", "HTML");

                                        //iRowsAffected = Submit_Stamp(iStsNumberOfHome, "STAMP_HOME");
                                    }
                                    */
                                    // START SENDING ALL HOME HITS TO ALL FSTS
                                    iRowsAffected = Submit_Stamp(iStsNumberOfHome, "STAMP_HOME");

                                    emh = null;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex) 
            { 
            }
            finally 
            {
                sqlConn.Close();
                odbcConn.Close();
            }
        }

        //Read split location id list, and get the vehicle id which entered
        // Check if any of those locations are in the home workfile 
        // If so, get the sts employee number from the home record matching the location entered
        // Use the vehicle id to get the driver's employee number
        // If the driver's employee number = the home location employee number, POST HOME STAMP TO ANDROID

        sDebug = "";  // Just to stop and be able to look at the data table.
        // What should I do with the data?
        // Save to Sql workfile? If so, that's a lot of data 
        // Determine if location id is a tech home, who is the driver of the vehicle in the home zone? if it is the driver's home.  Post a home stamp to the Android

        //Response.Write("Input Received: " + DateTime.Now.ToString("h:mm tt") + "<br />" + sInput);
    }
    // =========================================================
    // =========================================================
    /*
     * Created on page
    https://www.gpsinsight.com/apidocs/#/service/webhook
    Used Webhook Api page, on testbed

url             https://e1.scantronts.com/public/gpsi/LocationEntryReceiver.aspx
email           steve.carlson@scantron.com
name            LocationEntryReceiver
type            landmark_enter
content_type    json

Received Back (use key to delete if needed)
{
  "data": {
    "message": "added",
    "id": 302666
  }
}

            https://api.gpsinsight.com/v2/webhook/delete?session_token=xxxx&id=302125
*/



    // ========================================================================
    protected DataTable Get_LocationEntryListFromJson(string json)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        if (!String.IsNullOrEmpty(json))
        {
            json = json.Replace("\"head\":", "\"head\":["); // start of head section
                                                              //sJson = sJson.Replace(",\"data\":", "],\"data\":"); // end of head section 
            json = json.Replace(",\"data\":", "],\"data\":["); // end of head section 
            json = json.Replace("}}", "}]}"); // end of data section

            dt.Columns.Add(MakeColumn("account_id"));
            dt.Columns.Add(MakeColumn("username"));
            dt.Columns.Add(MakeColumn("ref_id"));
            dt.Columns.Add(MakeColumn("webhook_type"));
            dt.Columns.Add(MakeColumn("webhook_id"));
            dt.Columns.Add(MakeColumn("retry_count"));
            dt.Columns.Add(MakeColumn("queue_depth"));
            dt.Columns.Add(MakeColumn("enqueue_time"));
            dt.Columns.Add(MakeColumn("request_time"));
            dt.Columns.Add(MakeColumn("entered_landmarks")); // using the usual pipe delimited array
            dt.Columns.Add(MakeColumn("fix_time"));
            dt.Columns.Add(MakeColumn("id"));
            dt.Columns.Add(MakeColumn("latitude"));
            dt.Columns.Add(MakeColumn("longitude"));
            dt.Columns.Add(MakeColumn("odometer"));
            dt.Columns.Add(MakeColumn("serial_number"));
            dt.Columns.Add(MakeColumn("vin")); // The vin being passed is actually the GPSI vehicle id instead

            DataRow dr;

            string sAccount_Id = "";
            string sUsername = "";
            string sRef_Id = "";
            string sWebhook_Type = "";
            string sWebhook_Id = "";
            string sRetry_Count = "";
            string sQueue_Depth = "";
            string sEnqueue_Time = "";
            string sRequest_Time = "";

            DateTime datTemp;
            string sDat = "";

            try
            {
                //ObjList objList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ObjList>(json);
                LocationEntry objList = new LocationEntry();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList = (LocationEntry)serializer.Deserialize(json, typeof(LocationEntry));


                if (objList == null)
                {
                    erh.SaveErrorText("GPSI Location Entry List JSON Unparseable", "JSON: " + HttpUtility.HtmlEncode(json), "");
                }
                else
                {
                    //if (objList.head != null)
                    foreach (Head head in objList.head)
                    {
                        sAccount_Id = head.account_id.ToString();
                        sUsername = head.username;
                        sRef_Id = head.ref_id;
                        sWebhook_Type = head.type;
                        sWebhook_Id = head.webhook_id.ToString();
                        sRetry_Count = head.retry_count.ToString();
                        sQueue_Depth = head.queue_depth.ToString();
                        sEnqueue_Time = head.enqueue_time.ToString();
                        sRequest_Time = head.request_time.ToString();

                    }

                    // GPSI is giving me redundant and mislabeled data
                    // ALL THREE ref_id, id and vin have the same value which is the vehicle ID starting with "CA"
                    int iSeq = 0;
                    foreach (Data data in objList.data)
                    {
                        dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        dt.Rows[iSeq]["account_id"] = sAccount_Id;
                        dt.Rows[iSeq]["username"] = sUsername;
                        dt.Rows[iSeq]["ref_id"] = sRef_Id;
                        dt.Rows[iSeq]["webhook_type"] = sWebhook_Type;
                        dt.Rows[iSeq]["webhook_id"] = sWebhook_Id;
                        dt.Rows[iSeq]["retry_count"] = sRetry_Count;
                        dt.Rows[iSeq]["queue_depth"] = sQueue_Depth;
                        dt.Rows[iSeq]["enqueue_time"] = sEnqueue_Time; // Milliseconds
                        dt.Rows[iSeq]["request_time"] = sRequest_Time; // Milliseconds

                        dt.Rows[iSeq]["entered_landmarks"] = "";
                        foreach (int landmarkId in data.entered_landmarks) 
                        {
                            if (!String.IsNullOrEmpty(dt.Rows[iSeq]["entered_landmarks"].ToString()))
                                dt.Rows[iSeq]["entered_landmarks"] += "|";
                            dt.Rows[iSeq]["entered_landmarks"] += landmarkId.ToString();
                        }
                        sDat = data.fix_time;
                        if (DateTime.TryParse(sDat, out datTemp) == true)
                            sDat = datTemp.ToString("o"); 
                        else
                            sDat = data.fix_time;
                        dt.Rows[iSeq]["fix_time"] = sDat; // Timestamp in Greenwich
                        dt.Rows[iSeq]["id"] = data.id; 
                        dt.Rows[iSeq]["latitude"] = data.latitude.ToString("0.0000000");
                        dt.Rows[iSeq]["longitude"] = data.longitude.ToString("0.0000000");
                        dt.Rows[iSeq]["odometer"] = data.odometer.ToString("0.0");
                        dt.Rows[iSeq]["serial_number"] = data.serial_number;
                        dt.Rows[iSeq]["vin"] = data.vin;

                        iSeq++;
                    }
                }
            }
            catch (Exception ex)
            {
                erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                dt.AcceptChanges();
            }
        }

        return dt;
    }
    // ========================================================================
    #region mySql
    // ========================================================================
    // ========================================================================
    protected int Seek_HomeWorkfileStsNumber(int locationId)
    {
        int iStsNumber = 0;

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                 " hwLocationId" +
                ", hwLabel" +
                ", hwStsNumber" +
                " from [dbo].[gpsi_HomeWorkfile]" +
                " where hwLocationId = @Id";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@Id", locationId);
            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(sqlReader);

            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["hwStsNumber"].ToString().Trim(), out iStsNumber) == false)
                    iStsNumber = -1;
            }
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return iStsNumber;
    }
    // ========================================================================
    protected DataTable Seek_VehicleWorkfileRecord(string vehicleId) 
    {
        int iStsNumber = 0;
        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sSql = "Select" +
                 " vwId" +
                ", vwVin" +
                ", vwLabel" +
                ", vwDriverId" +
                ", vwDriverEmail" +
                ", vwLabel" +
                ", vwStsNumber" +
                " from [dbo].[gpsi_VehicleWorkfile]" +
                " where vwId = @Vid";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@Vid", vehicleId);
            sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(sqlReader);
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    protected int Submit_Stamp(int emp, string job) // STAMP_HOME
    {
        int iRowsAffected = 0;
        //string sStamp = DateTime.Now.ToString("o").Substring(0, 27);
        string sStamp = DateTime.Now.ToString("o");
        try
        {
            iRowsAffected = AddParmsToTRIGMAST(
                "TRIGFMT"
                , "ANPUSHER"
                , job // What
                , "GPSI" // Why (Why is not being used currently...) 
                , "CREATED" // Fields (Text 3)
                , sStamp // Values (Text 4)
                , "" // Text 5
                , "" // Text 6
                , "" // Text 7
                , "" // Text 8
                , "" // Text 9
                , emp // Selected Tech Number
                , 0 // Num 2
                , 0 // Num 3
                , 0 // Num 4
                , 0 // Num 5
                , 0 // Num 6
                , 0 // Num 7
                , 0 // Num 8
                , 0 // Num 9
                , ""); // Text BIG 

            if (iRowsAffected <= 0) 
            {
                erh = new ErrorHandler();
                erh.SaveErrorText("GPSI Stamp Failed For Emp: " + emp + " - Stamp: " + job, "", "");
                erh = null;
            }
        }
        catch (Exception ex)
        {
            erh = new ErrorHandler();
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            erh = null;
        }
        finally
        {
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySql
    // ========================================================================
    // ========================================================================
    #region myClasses
    // ========================================================================
    public class LocationEntry
    {
        public List<Head> head { get; set; }
        public List<Data> data { get; set; }
    }
    // ========================================================================
    public class Head
    {
        public int account_id { get; set; }
        public string username { get; set; }
        public string ref_id { get; set; }
        public string type { get; set; }
        public int webhook_id { get; set; }
        public int retry_count { get; set; }
        public int queue_depth { get; set; }
        public int enqueue_time { get; set; }
        public int request_time { get; set; }

    }
    // ========================================================================
    public class Data
    {
        public List<int> entered_landmarks { get; set; }
        public string fix_time { get; set; }
        public string id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double odometer { get; set; }
        public string serial_number { get; set; }
        public string vin { get; set; }

    }

    // ========================================================================
    #endregion // end myClasses
    // ========================================================================

    // =========================================================
    // =========================================================
}