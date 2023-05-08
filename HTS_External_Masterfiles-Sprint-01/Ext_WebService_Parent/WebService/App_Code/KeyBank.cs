using System; 
using System.Collections.Generic; 
using System.Web;
using System.Linq;
using System.Web.Services;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Configuration;
using System.Xml.Linq;

//using XE = System.Xml.Linq.XElement;
//using XA = System.Xml.Linq.XAttribute;

/// <summary>
/// HTS Public Web Service -- 
/// How to Access: on an ASP.NET site, 
/// right click the App_WebReferences Folder, Select "Add Web Reference" -- 
/// in the pop up box address enter https://oma-dev-dmz:70/KeyBank.asmx or https://ws.harlandts.com/KeyBank.asmx
/// </summary>
[WebService(Namespace = "https://ws.harlandts.com/KeyBank.asmx", Name = "HtsWebService_KeyBank", Description = "HTS Web Service For KeyBank")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
// ===================================================================
public class KeyBank : System.Web.Services.WebService 
{
    string sLibrary = ""; 
    string sWebService = "";
    string sErrorMessage = "";
    string sEnvironment = "";
    string sWsKey = "";
    string sHtsCs1 = "";
    string sHtsCs2 = "";
    string sHtsNam = "";
    string sHtsXrf = "";
    string sHtsAdr = "";
    string sHtsCit = "";
    string sHtsSta = "";
    string sHtsZip = "";
    string sHtsCnt = ""; // Count of records found...
    string sHtsUntNumList = "";
    string sHtsUntModList = "";
    string sHtsUntAgrList = "";
    string sHtsUntCs1List = "";
    string sHtsUntCs2List = "";
    string sHtsUntAdrList = "";
    string sHtsUntCitList = "";
    string sHtsUntStaList = "";
    string sHtsUntZipList = "";
    string sHtsUntNumBest = "";
    string sHtsUntModBest = "";
    string sHtsUntAgrBest = "";
    string sHtsUntCs1Best = "";
    string sHtsUntCs2Best = "";
    string sHtsUntAdrBest = "";
    string sHtsUntCitBest = "";
    string sHtsUntStaBest = "";
    string sHtsUntZipBest = "";
    string sXmlSqlId = "";
    string sCtr = "";
    string sTck = "";
    string sSrq = "";
    string sTechFirstName = "";
    string sTechLastName = "";
    string sHtsFullGreenwichStamp = "";
    int iCtr = 0;
    int iTck = 0;
    int iSrq = 0;
    int iXmlSqlId = 0;

    string sResponseToKey = "";
    string sResponseToHts = "";  // Used for minor failures (i.e. bad phone) but you still want to create the ticket, but inform HTS to update ticket
    string sResponseToKeyXml = "";
    string sCallCreationIsStillOk = "Y"; // Y or N
    string sSiteXref = "";
    string sFailureConsequence = "";
    string sCloseAcknowledged = "";
    string sCompletedDateStamp = "";    
    //string sResult = ""; 
    char[] cSplitter = { '|' };

    // Global Variables from XmlUnload for use throughout this program
    
    string sCity = "";
    string sCompany = "";
    string sContact_Type = "";
    string sCube = "";
    string sDeviceName = "";
    string sDeviceStatus = "";
    string sDocType = "";
    string sEmail = "";
    string sFirstName = "";
    string sLastName = "";
    string sModel = "";
    string sNotes = "";
    string sOpenedDateStamp = "";
    string sPhone = "";
    string sPhone_CountryCode = "";
    string sProblemDescription = "";
    string sReceiver = "";
    string sReceiver_Type = "";
    string sRefTicketNumber = "";
    string sRequestSyncType = "";
    string sRequest_DeploymentMode = "";
    string sSender = "";
    string sSender_Type = "";
    string sSentDateStamp = "";
    string sSerial = "";
    string sServiceAddress_Site = "";
    string sServiceDocument_Timestamp = "";
    string sServiceDocument_TransactionId = "";
    string sServiceDocument_Version = "";
    string sServiceRequest_Action = "";
    //string sSeverity = "";
    string sSeverity_Code = ""; // Also known as "Customer Priority" (P77 is indicated by a "4")
    string sState = "";
    //string sStatus = "";
    string sStatus_Code = "";    
    string sStreet = "";
    string sTicketInfo_TicketNumber = "";
    string sTransDateStamp = "";
    string sZip = "";

    double dHoursFromGreenwichCst = 0;
    double dHoursFromGreenwichEst = 0;

    //Customer_LIVE.CustomerMenu wsLiveCust = new Customer_LIVE.CustomerMenu();
    //Customer_DEV.CustomerMenu wsDevCust = new Customer_DEV.CustomerMenu();

    //Emp_LIVE.EmployeeMenuSoapClient wsLiveEmp = new Emp_LIVE.EmployeeMenuSoapClient();
    //Emp_DEV.EmployeeMenuSoapClient wsDevEmp = new Emp_DEV.EmployeeMenuSoapClient();

    KeyBank_DEV.KeyBankMenuSoapClient wsKeyDev = new KeyBank_DEV.KeyBankMenuSoapClient();
    KeyBank_LIVE.KeyBankMenuSoapClient wsKeyLive = new KeyBank_LIVE.KeyBankMenuSoapClient();

    EmailHandler emh;
    CommonCodeLibrary ccl = new CommonCodeLibrary();
    // ===================================================================
    public KeyBank () {

        // The HTS Library MUST NOT BE DETERMINED BY THE HTS ENVIRONMENT
        // but rather by KeyBank xml's directive for test/production
        // Key ONLY USES LIVE, so when they are doing development 
        // their requests go to our HTS LIVE but must disregard the actual environment and redirect to test (OMTDTALIB)
        // I only find KeyBank test/production AFTER I parse the inbound XML, at that point I must set the HTS library
        // and override the normal HTS environment which is set here in the contructor.

        //string connString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        //odbcConn = new OdbcConnection(connString);
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
        if (sLibrary == "OMDTALIB")
            sWebService = "DMZ WS LIVE: KEY";
        else
            sWebService = "DMZ WS DEV: KEY";
    }
    // =========================================================================
    // Public Methods
    // =========================================================================
    [WebMethod]
    public string HelloWorld()
    {
        /*
        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        if (sIpAddress != "10.41.30.9" && sIpAddress != "10.41.28.179" && sIpAddress != "::1")
        {
            emh = new EmailHandler();
            emh.EmailIndividual("Hello IP", "IP is: " + sIpAddress, "htslog@yahoo.com", "adv320@harlandts.com", "HMTL");
            emh = null;
        }
         * */
        return "Hello World";
    }
    // =========================================================================
//    [WebMethod]
//    public string HelloWorld_string(string sInput)
//    {
//        return "Hello " + sInput;
//    }
    // =========================================================================
    //[WebMethod]
    //public string Test1()
    protected string Test1()
    {
        string sTemp = "";
        // ----------------------------------------
/*
        string xml = @"
<levels>
    <level1 level1Att='1'>
        <level2a level2aAtt='2a'>
            <level3 level3Att='3A'>Level 3 Value A</level3>
            <level3 level3Att='3B'>Level 3 Value B</level3>
        </level2a>
        <level2b level2bAtt='2b'>Level 2b Value</level2b>
    </level1>
</levels>";
        XDocument doc = XDocument.Parse(xml);

        foreach (XElement element in doc.Descendants("level2b"))
        {
            sTemp = element.Name.ToString();
            sTemp = element.Attribute("level2bAtt").Value;
        }

        foreach (XElement element in doc.Descendants("level3"))
        {
            sTemp = element.Name.ToString();
            sTemp = element.Attribute("level3Att").Value;
        }

        // GREAT TEST OF FULL DOC PULL
        string sFullPath = Server.MapPath("~") + "ReqInValues1.xml";
        sTemp = System.IO.File.ReadAllText(sFullPath);
        sTemp = Unload_Xml_Received(sTemp);
 */ 
        // ----------------------------------------
        return sTemp;
    }
    // =========================================================================
    [WebMethod]
    public string RequestToHts_string(string request)
    {
        string sXmlTemp = "";
        sResponseToKeyXml = "";
        XmlDocument docToWs = new XmlDocument();
        //string sIpAddress = "";
        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        //DateTime datArrived = DateTime.Now;
        //sHtsFullGreenwichStamp = DateTime.Now.ToUniversalTime().ToString("o");
        DateTime datArrived = DateTime.Now.ToUniversalTime();
        sHtsFullGreenwichStamp = datArrived.ToString("o");

        // Write START Log seeking misfires
        //DbHandler dbh = new DbHandler();
        //dbh.LogEvent(datArrived, "START", "", request);
        //dbh = null;

        /*
        if (   sIpAddress != "156.77.111.16"
            && sIpAddress != "156.77.111.17"
            && sIpAddress != "156.77.111.18"
            && sIpAddress != "156.77.111.19"
            && sIpAddress != "156.77.79.16"
            && sIpAddress != "156.77.79.19"
            && sIpAddress != "156.77.79.20"
            && sIpAddress != "156.77.79.22" 
            && sIpAddress != "10.41.30.9" 
            && sIpAddress != "10.41.28.179" 
            && sIpAddress != "::1")
        {
            emh = new EmailHandler();
            emh.EmailIndividual("New Request IP", "IP is: " + sIpAddress, "htslog@yahoo.com", "adv320@harlandts.com", "HMTL");
            emh = null;
        }
         */

        // Valid IP Addresses
        // Debug on this server     ::1
        // Debug from Employee      10.41.30.9
        // KeyBank SIT = 156.77.111.17
        // Original test address = "156.77.79.21"

        try
        {
            
            sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            /* Possible Key Bank IP Ranges
             * 156.77.78.0/24
             * 156.77.79.0/24
             * 156.77.110.0/24
             * 156.77.111.0/24
             */

            if (   !sIpAddress.StartsWith("156.77.78.") 
                && !sIpAddress.StartsWith("156.77.79.")
                && !sIpAddress.StartsWith("156.77.110.")
                && !sIpAddress.StartsWith("156.77.111.") 
                && sIpAddress != "10.41.30.9" 
                && sIpAddress != "10.41.28.179" 
                && sIpAddress != "::1")
            {
                sResponseToKey += "Unauthorized Access: processing has been cancelled.\r\n\r\n ";
            }
            else
            {

                if ((String.IsNullOrEmpty(request))
                 &&  (sIpAddress == "10.41.30.9" || sIpAddress == "10.41.28.179" || sIpAddress == "::1")) // Use file xml if I'm running it
                {
                    // For testing, just pull xml from a file
                    string sFullPath = Server.MapPath("~") + "ReqInValues1.xml";
                    request = System.IO.File.ReadAllText(sFullPath);
                }

                try
                {
                    docToWs.LoadXml(request);
                }
                catch (Exception ex2)
                {
                    sResponseToKey += "The attempt to load the request received (as string) into an XML Document failed. " + ex2.Message.ToString() + "  Processing has been stopped.\r\n\r\n ";
                    // This error (if in live) will ALWAYS GO TO LIVE DB, we do not know yet if KeyBank is doing development in live
                    Send_Error(ex2.Message.ToString(), ex2.ToString(), sWebService + " - pre key xml");
                }

                if (String.IsNullOrEmpty(sResponseToKey)) // xml load did not fail
                {
                    // Process Request
                    //XmlNode nodeTemp = docToWs;
                    sXmlTemp = docToWs.InnerXml; // get a cleaned version (without \r\n if passed)
                    //nodeTemp = Process_Xml_Received(nodeTemp);
                    sResponseToKey = Process_Xml_Received(sXmlTemp);

                    // Convert Node to return as string here
                    //sXml = nodeTemp.InnerXml;
                }

                string sOmahaTime = DateTime.Now.ToString("MMM d, yyyy h:mm tt");
                //string sResponseForEmail = sResponseToKey;
                string sResponseForEmail = "";
                string sEmailResult = "";

                string[] saSucCtrTckSrq = { "", "", "", "" };
                string sEncryptedCall = "";

                // ============================================================================================================
                // Send FAILURE:ERROR or SUCCESS/WARNING Email
                if (!sResponseToKey.StartsWith("SUCCESS") || !String.IsNullOrEmpty(sResponseToHts))
                {
                    if (sResponseToKey.StartsWith("SUCCESS")) // but there are still HTS concerns...
                    {
                        // sResponseForEmail = sResponseToKey; //xxx
                        saSucCtrTckSrq = sResponseToKey.Split(cSplitter);
                        sResponseToKey = "";
                        if (saSucCtrTckSrq.Length > 2 && sServiceRequest_Action == "new")
                        {
                            if (sLibrary == "OMDTALIB")
                                sResponseForEmail = "HTS Call Created For Key Bank: " + saSucCtrTckSrq[1] + "-" + saSucCtrTckSrq[2] + " but REVIEW NEEDED \r\n";
                            else
                                sResponseForEmail = "TEST-TEST-TEST...HTS Call Created For Key Bank: " + saSucCtrTckSrq[1] + "-" + saSucCtrTckSrq[2] + " but REVIEW NEEDED \r\n";
                        }
                        else
                            sResponseForEmail = sResponseToKey;
                    }
                    else 
                    {
                        sResponseForEmail = sResponseToKey;
                    }

                    if (!String.IsNullOrEmpty(sResponseToHts))
                        sResponseForEmail += " \r\n " + sResponseToHts;
                    sResponseForEmail += " \r\nOmaha Time Received: " + sOmahaTime + "\r\n\r\n";
                    if (sEnvironment == "LIVE")
                        sResponseForEmail += "<a href=\"http://ourhts.com:90/private/keybank/Request.aspx?tck=" + sRefTicketNumber + "\">KeyBank Request Data</a>";
                    else
                        sResponseForEmail += "<a href=\"http://ourhts.com:190/private/keybank/Request.aspx?tck=" + sRefTicketNumber + "\">KeyBank Request Data</a>";

                    sEmailResult = "";
                    emh = new EmailHandler();
                    if (sLibrary == "OMDTALIB")
                    {
                        //sEmailResult = emh.EmailIndividual("Key Req ERROR: " + sOmahaTime, sResponseForEmail, "htslog@yahoo.com", "adv320@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req ERROR: " + sOmahaTime, sResponseForEmail, "swivelseat@harlandts.com", "steve.carlson@harlandts.com", "HTML");
                        sEmailResult = emh.EmailIndividual("Key Req ERROR Swivel Group: " + sOmahaTime, sResponseForEmail, "swivelseat@harlandts.com", "adv320@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req ERROR: Paula" + sOmahaTime, sResponseForEmail, "paula.costello@harlandts.com", "isabel.labrador@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req ERROR: Laurie" + sOmahaTime, sResponseForEmail, "laurie.bowles@harlandts.com", "isabel.labrador@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req ERROR: Ashlie" + sOmahaTime, sResponseForEmail, "ASHLIE.SKLENICKA@HARLANDTS.COM", "isabel.labrador@harlandts.com", "HTML");

                    }
                    else 
                    {
                        //sEmailResult = emh.EmailIndividual("TEST-TEST-TEST...Key Req ERROR: " + sOmahaTime, sResponseForEmail, "htslog@yahoo.com", "steve.carlson@harlands.com", "HTML");
                    }

                    //sEmailResult = emh.EmailIndividual("KeyBank Request Error: " + sOmahaTime, sResponseForEmail, "swivelseat@harlandts.com", "steve.carlson@harlandts.com", "HTML");
                    //sEmailResult = emh.EmailIndividual("KeyBank Request Error: " + sOmahaTime, sResponseForEmail, "ASHLIE.SKLENICKA@HARLANDTS.COM", "steve.carlson@harlandts.com", "HTML");
                    

                    emh = null;
                    // -------------------------------------
                    // Send error to KeyBank journal as notes
                    // -------------------------------------
                    if (!String.IsNullOrEmpty(sRefTicketNumber))
                    {
                        string sUrl = "";
                        string sPort = "";
                        if (sLibrary == "OMDTALIB")
                            sPort = "90";
                        else
                            sPort = "190";
                        
                        if (iCtr > 0 && iTck > 0)
                            sEncryptedCall = encryptTicket(iCtr.ToString(), iTck.ToString());

                        if (!String.IsNullOrEmpty(sResponseToKey))
                            sUrl = "http://ourhts.com:" + sPort + "/public/keybank/UpdateToKey.aspx?kid=" + sRefTicketNumber + "&tsk=UPD&fld=Note&val=" + sResponseToKey + "&pgm=dmz_ws_keybank";
                        else if (!String.IsNullOrEmpty(sEncryptedCall) && sServiceRequest_Action == "new")
                            sUrl = "http://ourhts.com:" + sPort + "/public/keybank/UpdateToKey.aspx?kid=" + sRefTicketNumber + "&tsk=UPD&fld=Note&val=http://www.harlandts.com/public/sc/ticketdetail.aspx?key=" + sEncryptedCall + "&pgm=dmz_ws_keybank";
                        else
                            sUrl = "http://ourhts.com:" + sPort + "/public/keybank/UpdateToKey.aspx?kid=" + sRefTicketNumber + "&tsk=UPD&pgm=dmz_ws_keybank";

                        //emh = new EmailHandler();
                        //emh.EmailIndividual("DMZ Sending secondary failure update to key: ", "", "htslog@yahoo.com", "adv320@harlands.com", "HTML");
                        //emh = null;

                        using (System.Net.WebClient client = new System.Net.WebClient())
                        {
                            //sResponse = client.DownloadString(sUrl);
                            client.DownloadString(sUrl);
                        }
                    }

                    // -------------------------------------

                }
                // ============================================================================================================
                else  // SIMIPLE SUCCESS: do secondary update
                {
                    // now do additional updates (you are doing this BEFORE you are sending the response to this function...) 

                    string sUrl = "";
                    string sPort = "";
                    //string sResponse = "";
                    if (sLibrary == "OMDTALIB")
                        sPort = "90";
                    else
                        sPort = "190";

                    sEncryptedCall = encryptTicket(iCtr.ToString(), iTck.ToString()); 

                    if (!String.IsNullOrEmpty(sEncryptedCall) && sServiceRequest_Action == "new")
                        sUrl = "http://ourhts.com:" + sPort + "/public/keybank/UpdateToKey.aspx?ctr=" + iCtr + "&tck=" + iTck + "&tsk=UPD&fld=Note&val=http://www.harlandts.com/public/sc/ticketdetail.aspx?key=" + sEncryptedCall + "&pgm=dmz_ws_keybank";
                    else
                        sUrl = "http://ourhts.com:" + sPort + "/public/keybank/UpdateToKey.aspx?ctr=" + iCtr + "&tck=" + iTck + "&tsk=UPD&pgm=dmz_ws_keybank";

                    //emh = new EmailHandler();
                    //emh.EmailIndividual("DMZ Sending secondary success update to key: ", "", "htslog@yahoo.com", "adv320@harlands.com", "HTML");
                    //emh = null;

                    using (System.Net.WebClient client = new System.Net.WebClient())
                    {
                        //sResponse = client.DownloadString(sUrl);
                        client.DownloadString(sUrl);
                    }
                }
                // ----------------------
                saSucCtrTckSrq = sResponseToKey.Split(cSplitter);
                if (saSucCtrTckSrq.Length > 3 && sServiceRequest_Action == "new") 
                {
                    if (sLibrary == "OMDTALIB")
                        sResponseForEmail = "HTS Call Created For Key Bank: " + saSucCtrTckSrq[1] + "-" + saSucCtrTckSrq[2];
                    else
                        sResponseForEmail = "TEST-TEST-TEST...HTS Call Created For Key Bank: " + saSucCtrTckSrq[1] + "-" + saSucCtrTckSrq[2];

                    sResponseForEmail += " \r\n\r\nKeyBank Ticket Number: " + sRefTicketNumber + "\r\n ";
                    sResponseForEmail += " \r\nOmaha Time Received: " + sOmahaTime + "\r\n\r\n";
                    if (sEnvironment == "LIVE")
                        sResponseForEmail += "<a href=\"http://ourhts.com:90/private/keybank/Request.aspx?tck=" + sRefTicketNumber + "\">KeyBank Request Data</a>";
                    else
                        sResponseForEmail += "<a href=\"http://ourhts.com:190/private/keybank/Request.aspx?tck=" + sRefTicketNumber + "\">KeyBank Request Data</a>";

                    sEmailResult = "";
                    emh = new EmailHandler();
                    if (sLibrary == "OMDTALIB")
                    {
                        //sEmailResult = emh.EmailIndividual("Key Req SUCCESS: " + sOmahaTime, sResponseForEmail, "htslog@yahoo.com", "adv320@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("KeyBank Request Error: " + sOmahaTime, sResponseForEmail, "ASHLIE.SKLENICKA@HARLANDTS.COM", "adv320@harlandts.com", "HTML");
                        sEmailResult = emh.EmailIndividual("Key Req SUCCESS: " + sOmahaTime, sResponseForEmail, "swivelseat@harlandts.com", "adv320@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req SUCCESS: Swivel Group" + sOmahaTime, sResponseForEmail, "swivelseat@harlandts.com", "isabel.labrador@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req SUCCESS: Paula" + sOmahaTime, sResponseForEmail, "paula.costello@harlandts.com", "isabel.labrador@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req SUCCESS: Laurie" + sOmahaTime, sResponseForEmail, "laurie.bowles@harlandts.com", "isabel.labrador@harlandts.com", "HTML");
                        //sEmailResult = emh.EmailIndividual("Key Req SUCCESS: Ashlie: " + sOmahaTime, sResponseForEmail, "ASHLIE.SKLENICKA@HARLANDTS.COM", "isabel.labrador@harlandts.com", "HTML");
                    }
                    else
                    {
                        //sEmailResult = emh.EmailIndividual("TEST-TEST-TEST...KeyBank Request SUCCESS: " + sOmahaTime, sResponseForEmail, "htslog@yahoo.com", "adv320@harlandts.com", "HTML");
                    }

                    emh = null;
                }
                // ----------------------
                // Normally XML string will already be loaded in child methods, 
                // but if xml could not be parsed here, it will need to be loaded here 
                if (String.IsNullOrEmpty(sResponseToKeyXml))
                    sResponseToKeyXml = Build_Xml_Response(sResponseToKey);
            
            }  // END IP Address Requirement...

            // Write end Log seeking misfires
            //dbh = new DbHandler();
            //dbh.LogEvent(datArrived, "END", sRefTicketNumber, sResponseToKey);
            //dbh = null;

        }
        catch (Exception ex)
        {
            Send_Error(ex.Message.ToString(), ex.ToString(), sWebService);
        }
        finally
        {
        }

        return sResponseToKeyXml;  // It is returned automatically as "string" function return statement
    }
    // ========================================================================
    // Protected Methods: Only accessible from other methods 
    // ========================================================================
    protected string Process_Xml_Received(string request)
    {
        // This is my unique field in place of (now non-unique) KeyBank TransactionId
        //sHtsFullGreenwichStamp = DateTime.Now.ToUniversalTime().ToString("o");
        XmlDocument xDocTemp = new XmlDocument();
        //XmlNode nodeToReturn = xDocTemp;
        int iRowsAffected = 0;
        string sXmlTemp = "";
        Boolean bXmlSuccessfullyLoaded = false;

        try
        {
            sWsKey = ccl.GetWsKey();

            //dHoursFromGreenwichCst = GetHoursFromGreenwich("Central Standard Time"); // To recreate Omaha time from required Greenwich
            //dHoursFromGreenwichEst = GetHoursFromGreenwich("Eastern Standard Time"); // To recreate KeyBank Server Transmission from Greenwich
            dHoursFromGreenwichCst = GetHoursFromGreenwich();
            dHoursFromGreenwichEst = dHoursFromGreenwichCst - 1.0;

            // Save Raw Xml to be saved after unload attempt
            //sXmlTemp = dataAsNode.InnerXml;

            // Unload Data and Validate
            Unload_Xml_Received(request);

            // Set SQL Server database library AGAIN (in case an error caused it not to be set in XML validation section) 
            if (sRequest_DeploymentMode != "test" && sRequest_DeploymentMode != "production") // if you know if you're in test or production load the files
                sRequest_DeploymentMode = "test";  // you may want to load these in live assuming failures are from key in live (or hackers)...
            
            // THIS ENVIRONMENT UPDATE IS THE "SECOND" PASS (Only an error message above would have used the default live library from live if testing from live...) 
            Set_Environment(sRequest_DeploymentMode);

            // Replace Unpassable Characters for pass into Network WebService
            sXmlTemp = request;
            sXmlTemp = sXmlTemp.Replace("<", "&lt");
            sXmlTemp = sXmlTemp.Replace(">", "&gt");

            // Save XML transmission received
            if (sLibrary == "OMDTALIB")
            {
                iRowsAffected = wsKeyLive.InsertInboundRequestXml(sWsKey, sHtsFullGreenwichStamp, sRefTicketNumber, sServiceDocument_TransactionId, sServiceRequest_Action, sXmlTemp, sEnvironment);
                iXmlSqlId = wsKeyLive.GetRequestXmlSqlId(sWsKey, sHtsFullGreenwichStamp, sEnvironment);
            }
            else
            {
                iRowsAffected = wsKeyDev.InsertInboundRequestXml(sWsKey, sHtsFullGreenwichStamp, sRefTicketNumber, sServiceDocument_TransactionId, sServiceRequest_Action, sXmlTemp, sEnvironment);
                iXmlSqlId = wsKeyDev.GetRequestXmlSqlId(sWsKey, sHtsFullGreenwichStamp, sEnvironment);
            }

            // If raw xml save failed...
            if (iRowsAffected == 0)
            {
                bXmlSuccessfullyLoaded = false;
                sResponseToKey += "ERROR: Though values retrieved from Xml, an error occured saving the raw Xml to the HTS log file. " + sFailureConsequence + "\r\n\r\n ";
            }
            else {
                sXmlSqlId = iXmlSqlId.ToString();
                bXmlSuccessfullyLoaded = true;
            }
                

            // If xml saved successfully, also save the values
            if (bXmlSuccessfullyLoaded == true) 
            {
                if (
                    !String.IsNullOrEmpty(sResponseToKey) // you have an error...
                    || (iCtr > 0 && iTck > 0) // you already have a ticket...
                    )
                {
                    sCallCreationIsStillOk = "N";
                }
                if (sCloseAcknowledged != "1" && sCloseAcknowledged != "2" && sCloseAcknowledged != "3")  // Stop update if close already acknowledged by key
                {
                    sResponseToKey += Save_Values();
                }
                
            }
                    

            // ---------------------
            // Prepare Xml Response
            // ---------------------
            if (!sResponseToKey.StartsWith("SUCCESS") && !String.IsNullOrEmpty(sResponseToKey))
            {
                if (!String.IsNullOrEmpty(sRefTicketNumber))
                    sResponseToKey += "\r\n\r\n KeyBank refTicketNumber: " + sRefTicketNumber + "\r\n ";
            }

            sResponseToKeyXml = Build_Xml_Response(sResponseToKey);
            // xDocTemp.LoadXml(sXmlTemp);
            // nodeToReturn = xDocTemp;

            if (bXmlSuccessfullyLoaded == true) // Meaning you have an xml record to update
            {
                int iSrqTemp = iSrq;
                if (sCallCreationIsStillOk == "N")
                    iSrqTemp = 0;

                // Replace Unpassable Characters for web service pass
                sXmlTemp = sResponseToKeyXml;
                sXmlTemp = sXmlTemp.Replace("<", "&lt");
                sXmlTemp = sXmlTemp.Replace(">", "&gt");

                if (sLibrary == "OMDTALIB")
                {
                    iRowsAffected = wsKeyLive.UpdateInboundRequestXml(sWsKey, iXmlSqlId, sResponseToKey, sXmlTemp, sRequest_DeploymentMode);
                    // You need to update the VALUES file with center ticket and request key
                    iRowsAffected = wsKeyLive.UpdateInboundRequestValuesFromCall(sWsKey, iCtr, iTck, iSrqTemp, sRefTicketNumber, sRequest_DeploymentMode);
                }
                else
                {
                    iRowsAffected = wsKeyDev.UpdateInboundRequestXml(sWsKey, iXmlSqlId, sResponseToKey, sXmlTemp, sRequest_DeploymentMode);
                    // You need to update the VALUES file with center ticket and request key
                    iRowsAffected = wsKeyDev.UpdateInboundRequestValuesFromCall(sWsKey, iCtr, iTck, iSrqTemp, sRefTicketNumber, sRequest_DeploymentMode);
                }
            }
        }
        catch (Exception ex)
        {
            Send_Error(ex.Message.ToString(), ex.ToString(), sWebService);
            // you may need to load node with error here
        }
        finally
        {
        }

        if (String.IsNullOrEmpty(sResponseToKey)) 
        {
            if (sServiceRequest_Action == "new")
            {
                sResponseToKey = "ERROR: transaction result ended blank (with no success or failure message).  HTS has been notified and will manually handle this request.\r\n\r\n ";
            }
        }

        return sResponseToKey;
    }
    // ========================================================================
    protected string Save_Values()
    {
        string sResult = "";

        // xxx
        //EmailHandler emh2 = new EmailHandler();
        //emh2.EmailIndividual("DMZ Lib at point of save", "Library: " + sLibrary, "htslog@yahoo.com", "adv320@harlandts.com", "HMTL");
        //emh2 = null;

        if (sLibrary == "OMDTALIB")
        {
            sResult = wsKeyLive.SaveInboundRequestValues(
                  sWsKey
                , sCallCreationIsStillOk
                , sCity
                , sCompany
                //, sContact_Type
                , sCube
                , sDeviceName 
                , sDeviceStatus 
                //, sDocType
                , sEmail
                , sFirstName
                , sLastName
                , sModel
                , sNotes
                , sOpenedDateStamp
                , sPhone
                //, sPhone_CountryCode
                , sProblemDescription
                , sReceiver
                //, sReceiver_Type
                , sRefTicketNumber
                , sRequest_DeploymentMode
                , sRequestSyncType
                , sSender
                //, sSender_Type
                , sSentDateStamp
                , sSerial
                , sServiceAddress_Site
                , sServiceDocument_Timestamp
                , sServiceDocument_TransactionId
                , sServiceDocument_Version
                , sServiceRequest_Action
                //, ""
                , sSeverity_Code
                , sState
                //, sStatus
                , sStatus_Code
                , sStreet
                , sTicketInfo_TicketNumber
                , sTransDateStamp
                , sZip
                , sHtsCs1
                , sHtsCs2
                , sHtsNam
                , sHtsUntNumBest
                , sHtsUntModBest
                , sHtsUntAgrBest
                , sXmlSqlId
                , sHtsFullGreenwichStamp
                );
        } 
        else
        {
            sResult = wsKeyDev.SaveInboundRequestValues(
                  sWsKey
                , sCallCreationIsStillOk
                , sCity
                , sCompany
                //, sContact_Type
                , sCube
                , sDeviceName
                , sDeviceStatus
                //, sDocType
                , sEmail
                , sFirstName
                , sLastName
                , sModel
                , sNotes
                , sOpenedDateStamp
                , sPhone
                //, sPhone_CountryCode
                , sProblemDescription
                , sReceiver
                //, sReceiver_Type
                , sRefTicketNumber
                , sRequest_DeploymentMode
                , sRequestSyncType
                , sSender
                //, sSender_Type
                , sSentDateStamp
                , sSerial
                , sServiceAddress_Site
                , sServiceDocument_Timestamp
                , sServiceDocument_TransactionId
                , sServiceDocument_Version
                , sServiceRequest_Action
                //, ""
                , sSeverity_Code
                , sState
                //, sStatus
                , sStatus_Code
                , sStreet
                , sTicketInfo_TicketNumber
                , sTransDateStamp
                , sZip
                , sHtsCs1
                , sHtsCs2
                , sHtsNam
                , sHtsUntNumBest
                , sHtsUntModBest
                , sHtsUntAgrBest
                , sXmlSqlId
                , sHtsFullGreenwichStamp
                );
        }

        return sResult;
    }
    // ========================================================================
    protected void Send_Error(string errorSummary, string errorDescription, string errorValues)
    {
        if (sLibrary == "OMDTALIB")
        {
            wsKeyLive.SaveErrorText(errorSummary, errorDescription, errorValues, sWebService);
        }
        else
        {
            wsKeyDev.SaveErrorText(errorSummary, errorDescription, errorValues, sWebService);
        }
    }
    // ========================================================================
    protected string Check_Password(string password)
    {
        string sVerdict = "";
        if (password == "LionOnRock")
        {
            sVerdict = "VALID";
        }
        else
        {
            sVerdict = "INVALID";
        }
        return sVerdict;
    }
    // ========================================================================
    protected void Build_Error(string message)
    {
        if (!String.IsNullOrEmpty(sErrorMessage))
            sErrorMessage += "|";
        sErrorMessage += message;
    }
    // ========================================================================
    protected void Set_Environment(string environment)
    {
        // Key's requested enviroment must OVERRULE the natural HTS enviroment because their development is running in LIVE
        if (environment == "production")
        {
            sEnvironment = "LIVE";
            sLibrary = "OMDTALIB";
            sWebService = "DMZ WS LIVE: KEY";
        }
        else 
        {
            sEnvironment = "TEST";
            sLibrary = "OMTDTALIB";
            sWebService = "DMZ WS DEV: KEY";
        }
            
    }
    // -----------------------------------------------------------    
    protected void Unload_Xml_Received(string xmlReceived)
    {
        XDocument doc = XDocument.Parse(xmlReceived);

        Boolean bTransactionId_Received = false;
        Boolean bTimestamp_Received = false;
        Boolean bDocType_Received = false;
        Boolean bSender_Received = false;
        Boolean bReceiver_Received = false;
        Boolean bRequestSyncType_Received = false;
        Boolean bDeploymentMode_Received = false;
        Boolean bTransDateStamp_Received = false;
        Boolean bOpenedDateStamp_Received = false;
        Boolean bSentDateStamp_Received = false;
        Boolean bRefTicketNumber_Received = false;
        Boolean bCompany_Received = false;
        Boolean bFirstName_Received = false;
        Boolean bLastName_Received = false;
        Boolean bCountryCode_Received = false;
        Boolean bPhone_Received = false;
        Boolean bStreet_Received = false;
        Boolean bCity_Received = false;
        Boolean bState_Received = false;
        Boolean bZip_Received = false;
        Boolean bProblemDescription_Received = false;
        Boolean bServiceAddress_Site_Received = false;
        Boolean bServiceRequest_Action_Received = false;
        
        DateTime datZero = new DateTime();
        DateTime datNowGreenwich = DateTime.Now.ToUniversalTime();
        DateTime datOpened = new DateTime();
        DateTime datSent = new DateTime();
        DateTime datTransmission = new DateTime(); // this xml transmission timestamp
        DateTime datTransaction = new DateTime();  // key new timestamp event / key update stampstamp event

        try
        {
            // ========================================================
            // FIRST: GET ESSENTIAL VALUES FOR INITIAL PROCESSING (alpha search later...) 
            // ========================================================

            // get 'new' or 'update' first to be used during validation
            // April 5th, 2016 -- New or Update are both irrelevent now, both are accepted then add/update and call creation are based on prior file values
            // This was done in an attempt to limit "failures" to make as many successful tickets as possible 
            foreach (XElement element in doc.Descendants("serviceRequest"))
            {
                sServiceRequest_Action = element.Attribute("action").Value;
                if (!String.IsNullOrEmpty(sServiceRequest_Action))
                {
                    bServiceRequest_Action_Received = true;
                    if (sServiceRequest_Action != "new" && sServiceRequest_Action != "update")
                    {
                        sResponseToKey += "ERROR: serviceRequest action (" + sServiceRequest_Action + ") is not a recognized value, so we don't know how to process this transaction.\r\n\r\n ";
                    }
                }
            }

            // Get test or live...
            foreach (XElement element in doc.Descendants("request"))
            {
                sRequest_DeploymentMode = element.Attribute("deploymentMode").Value;

                if (!String.IsNullOrEmpty(sRequest_DeploymentMode))
                {
                    bDeploymentMode_Received = true;
                    if (sRequest_DeploymentMode.Length > 10)
                        sResponseToKey += "ERROR: request deploymentMode (" + sRequest_DeploymentMode + ") exceeded the 10 character max and has been truncated.\r\n\r\n ";
                    if (sRequest_DeploymentMode != "test" && sRequest_DeploymentMode != "production")
                        sResponseToKey += "ERROR: request deploymentMode (" + sRequest_DeploymentMode + ") was invalid.  Only 'test' or 'production' are valid.\r\n\r\n ";
                }

                else
                {
                    sResponseToKey += "ERROR: Element request attribute deploymentMode was blank or null.\r\n\r\n ";
                }
            }

            if (sRequest_DeploymentMode != "test" && sRequest_DeploymentMode != "production") // if you know if you're in test or production load the files
                sRequest_DeploymentMode = "test";  // you may want to load these in live assuming failures are from key in live (or hackers)...

            // STAY IN TEST
            //sRequest_DeploymentMode = "test";
            // THIS ENVIRONMENT UPDATE IS THE FIRST PASS (in case of error) 
            Set_Environment(sRequest_DeploymentMode);  

            // Get KeyBank Ticket Number
            foreach (XElement element in doc.Descendants("refTicketNumber"))
            {
                sRefTicketNumber = element.Value;
                if (!String.IsNullOrEmpty(sRefTicketNumber))
                {
                    bRefTicketNumber_Received = true;
                    if (sRefTicketNumber.Length > 30)
                    {
                        sResponseToKey += "ERROR: refTicketNumber (" + sRefTicketNumber + ") exceeded the 30 character max and has been truncated.\r\n\r\n ";
                        sRefTicketNumber = sRefTicketNumber.Substring(0, 30);
                    }
                }
            }

            // Check to see if you already have an HTS Call Number
            DataTable dt = new DataTable();

            iCtr = 0;
            iTck = 0;
            //if (sServiceRequest_Action == "update")
            if (!String.IsNullOrEmpty(sRefTicketNumber))
            {
                // See if you have an existing center/ticket    
                if (sLibrary == "OMDTALIB")
                {
                    dt = wsKeyLive.GetInboundRequestValues(sWsKey, sRefTicketNumber, sEnvironment);
                }
                else
                {
                    dt = wsKeyDev.GetInboundRequestValues(sWsKey, sRefTicketNumber, sEnvironment);
                }

                if (dt.Rows.Count > 0)
                {
                    if (int.TryParse(dt.Rows[0]["rv_Ctr"].ToString().Trim(), out iCtr) == false)
                        iCtr = 0;
                    if (int.TryParse(dt.Rows[0]["rv_Tck"].ToString().Trim(), out iTck) == false)
                        iTck = 0;
                    sCompletedDateStamp = dt.Rows[0]["rv_CompletedDateStamp"].ToString().Trim();
                    sCloseAcknowledged = dt.Rows[0]["rv_CloseAcknowledged"].ToString().Trim();
                }
            }

            // ========================================================
            // Alpha from here down
            // ========================================================

            foreach (XElement element in doc.Descendants("city"))
            {
                sCity = element.Value;
                if (!String.IsNullOrEmpty(sCity))
                {
                    bCity_Received = true;
                    if (sCity.Length > 30)
                    {
                        sResponseToKey += "ERROR: city (" + sCity + ") exceeded the 30 character max and has been truncated.\r\n\r\n ";
                        sCity = sCity.Substring(0, 30);
                    }
                    sCity = ccl.Clean(sCity, 30);
                }
            }

            foreach (XElement element in doc.Descendants("cube"))
            {
                sCube = element.Value;
                sCube = ccl.Clean(sCube, 20);
            }

            foreach (XElement element in doc.Descendants("contact"))
            {
                sContact_Type = element.Attribute("type").Value;
            }

            foreach (XElement element in doc.Descendants("company"))
            {
                sCompany = element.Value;
                if (!String.IsNullOrEmpty(sCompany))
                {
                    bCompany_Received = true;
                    if (sCompany.Length > 80)
                    {
                        sResponseToKey += "ERROR: company (" + sCompany + ") exceeded the 80 character max and has been truncated.\r\n\r\n ";
                        sCompany = sCompany.Substring(0, 80);
                    }
                    sCompany = ccl.Clean(sCompany, 80);
                }
            }

            foreach (XElement element in doc.Descendants("deviceName"))
            {
                sDeviceName = element.Value;
                sDeviceName = ccl.Clean(sDeviceName, 100);
            }

            foreach (XElement element in doc.Descendants("deviceStatus"))
            {
                sDeviceStatus = element.Value;
                sDeviceStatus = ccl.Clean(sDeviceStatus, 40);
            }

            foreach (XElement element in doc.Descendants("docType"))
            {
                sDocType = element.Value;
                if (!String.IsNullOrEmpty(sDocType))
                {
                    bDocType_Received = true;
                    if (sDocType.Length > 15)
                    {
                        sResponseToKey += "ERROR: docType (" + sDocType + ") exceeded the 15 character max and has been truncated.\r\n\r\n ";
                        sDocType = sDocType.Substring(0, 15);
                    }
                }
            }

            foreach (XElement element in doc.Descendants("email"))
            {
                sEmail = element.Value;
                sEmail = ccl.Clean(sEmail, 80);
            }

            foreach (XElement element in doc.Descendants("firstName"))
            {
                sFirstName = element.Value;
                if (!String.IsNullOrEmpty(sFirstName))
                {
                    bFirstName_Received = true;
                    if (sFirstName.Length > 30)
                    {
                        sResponseToKey += "ERROR: firstName (" + sFirstName + ") exceeded the 30 character max and has been truncated.\r\n\r\n ";
                        sFirstName = sFirstName.Substring(0, 30);
                    }
                    sFirstName = ccl.Clean(sFirstName, 30);
                }
            }

            foreach (XElement element in doc.Descendants("lastName"))
            {
                sLastName = element.Value;
                if (!String.IsNullOrEmpty(sLastName))
                {
                    bLastName_Received = true;
                    if (sLastName.Length > 30)
                    {
                        sResponseToKey += "ERROR: lastName (" + sLastName + ") exceeded the 30 character max and has been truncated.\r\n\r\n ";
                        sLastName = sLastName.Substring(0, 30);
                    }
                    sLastName = ccl.Clean(sLastName, 30);
                }
            }

            foreach (XElement element in doc.Descendants("model"))
            {
                sModel = element.Value;
                sModel = ccl.Clean(sModel, 30);
            }

            foreach (XElement element in doc.Descendants("notes"))
            {
                sNotes = element.Value;
                sNotes = ccl.Clean(sNotes, 2000);
            }

            foreach (XElement element in doc.Descendants("openedDateStamp"))
            {
                sOpenedDateStamp = element.Value;
                if (!String.IsNullOrEmpty(sOpenedDateStamp))
                {
                    bOpenedDateStamp_Received = true;
                    if (DateTime.TryParse(sOpenedDateStamp, out datOpened) == false)
                    {
                        sResponseToKey += "ERROR: openedDateStamp (" + sOpenedDateStamp + ") could not be parsed as a valid date (i.e February 31st).\r\n\r\n ";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("phone"))
            {
                sPhone_CountryCode = element.Attribute("countryCode").Value;
                bCountryCode_Received = true;
                if (!String.IsNullOrEmpty(sPhone_CountryCode))
                {
                    if (sPhone_CountryCode.Length > 5)
                    {
                        sResponseToKey += "ERROR: phone countryCode (" + sPhone_CountryCode + ") exceeded the 5 character max and has been truncated.\r\n\r\n ";
                        sPhone_CountryCode = sPhone_CountryCode.Substring(0, 5);
                    }
                    int iTemp = 0;
                    if (int.TryParse(sPhone_CountryCode, out iTemp) == false)
                        sResponseToKey += "ERROR: phone countryCode (" + sPhone_CountryCode + ") must be an integer (if not blank).\r\n\r\n ";
                }

                sPhone = element.Value;
                sPhone = ccl.Clean(sPhone, 20);
                if (!String.IsNullOrEmpty(sPhone))
                {
                    bPhone_Received = true;

                    sPhone = sPhone.Replace("-", "");
                    sPhone = sPhone.Replace("(", "");
                    sPhone = sPhone.Replace(")", "");
                    sPhone = sPhone.Replace(" ", "");

                    if (sPhone.Length > 20)
                    {
                        sResponseToKey += "ERROR: phone (" + sPhone + ") exceeded the 20 character max and has been truncated.\r\n\r\n ";
                        sPhone = sPhone.Substring(0, 20);
                    }
                    if (sPhone.Length != 10)
                    {
                        if (iCtr == 0 && iTck == 0) // if ticket already created, don't send this again....
                        {
                            sResponseToHts += "MINOR ERROR: phone (" + sPhone + ") was not the expected 10 numbers. Update will be needed.  Call creation will still be attempted.\r\n\r\n ";
                        }
                        sPhone = "8888888888";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("problemDescription"))
            {
                sProblemDescription = element.Value.ToUpper();
                if (!String.IsNullOrEmpty(sProblemDescription))
                {
                    bProblemDescription_Received = true;
                    if (sProblemDescription.Length > 10000)
                    {
                        sResponseToKey += "ERROR: problemDescription (" + sProblemDescription + ") exceeded the 10,000 character max and has been truncated.\r\n\r\n ";
                        sProblemDescription = sProblemDescription.Substring(0, 10000);
                    }
                    sProblemDescription = ccl.Clean(sProblemDescription, 2000);
                }

            }

            foreach (XElement element in doc.Descendants("receiver"))
            {
                sReceiver = element.Value;
                if (!String.IsNullOrEmpty(sReceiver))
                {
                    bReceiver_Received = true;
                    if (sReceiver.Length > 3)
                    {
                        sResponseToKey += "ERROR: receiver (" + sReceiver + ") exceeded the 3 character max and has been truncated.\r\n\r\n ";
                        sReceiver = sReceiver.Substring(0, 3);
                    }
                }

                sReceiver_Type = element.Attribute("type").Value;
            }

            foreach (XElement element in doc.Descendants("sender"))
            {
                sSender = element.Value;
                if (!String.IsNullOrEmpty(sSender))
                {
                    bSender_Received = true;
                    if (sSender.Length > 3)
                    {
                        sResponseToKey += "ERROR: sender (" + sSender + ") exceeded the 3 character max and has been truncated.\r\n\r\n ";
                        sSender = sSender.Substring(0, 3);
                    }
                }

                sSender_Type = element.Attribute("type").Value;
            }

            foreach (XElement element in doc.Descendants("sentDateStamp"))
            {
                sSentDateStamp = element.Value;
                if (!String.IsNullOrEmpty(sSentDateStamp))
                {
                    bSentDateStamp_Received = true;
                    if (DateTime.TryParse(sSentDateStamp, out datSent) == false)
                    {
                        sResponseToKey += "ERROR: sentDateStamp (" + sSentDateStamp + ") could not be parsed as a valid date (i.e February 31st).\r\n\r\n ";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("serial"))
            {
                sSerial = element.Value.ToUpper();
                sSerial = ccl.Clean(sSerial, 20);
            }

            foreach (XElement element in doc.Descendants("serviceAddress"))
            {
                sServiceAddress_Site = element.Attribute("site").Value;
                sServiceAddress_Site = ccl.Clean(sServiceAddress_Site, 40);
                if (!String.IsNullOrEmpty(sServiceAddress_Site))
                    bServiceAddress_Site_Received = true;
            }

            foreach (XElement element in doc.Descendants("serviceDocument"))
            {
                sServiceDocument_Timestamp = element.Attribute("timestamp").Value;
                if (!String.IsNullOrEmpty(sServiceDocument_Timestamp))
                {
                    bTimestamp_Received = true;
                    if (DateTime.TryParse(sServiceDocument_Timestamp, out datTransmission) == false)
                    {
                        sResponseToKey += "ERROR: serviceDocument timestamp (" + sServiceDocument_Timestamp + ") could not be parsed as a valid date (i.e February 31st).\r\n\r\n ";
                    }
                }
                sServiceDocument_TransactionId = element.Attribute("transactionId").Value;
                if (!String.IsNullOrEmpty(sServiceDocument_TransactionId))
                {
                    bTransactionId_Received = true;
                    if (sServiceDocument_TransactionId.Length > 100)
                    {
                        sResponseToKey += "ERROR: transactionId (" + sServiceDocument_TransactionId + ") exceeded the 100 character max and has been truncated.\r\n\r\n ";
                        sServiceDocument_TransactionId = sServiceDocument_TransactionId.Substring(0, 100);
                    }
                }
                sServiceDocument_Version = element.Attribute("version").Value;
            }

            foreach (XElement element in doc.Descendants("severity"))
            {
                sSeverity_Code = element.Attribute("code").Value;
            }

            foreach (XElement element in doc.Descendants("state"))
            {
                sState = element.Value.ToUpper();
                if (!String.IsNullOrEmpty(sState))
                {
                    bState_Received = true;
                    if (sState.Length > 2)
                    {
                        sResponseToKey += "ERROR: state (" + sState + ") exceeded the 2 character max and has been truncated.\r\n\r\n ";
                        sState = sState.Substring(0, 2);
                    }
                    if (sState != "AK"
                        && sState != "AL"
                        && sState != "AR"
                        && sState != "AZ"
                        && sState != "CA"
                        && sState != "CO"
                        && sState != "CT"
                        && sState != "DC"
                        && sState != "DE"
                        && sState != "FL"
                        && sState != "GA"
                        && sState != "HI"
                        && sState != "IA"
                        && sState != "ID"
                        && sState != "IL"
                        && sState != "IN"
                        && sState != "KS"
                        && sState != "KY"
                        && sState != "LA"
                        && sState != "MA"
                        && sState != "MD"
                        && sState != "ME"
                        && sState != "MI"
                        && sState != "MN"
                        && sState != "MO"
                        && sState != "MS"
                        && sState != "MT"
                        && sState != "NC"
                        && sState != "ND"
                        && sState != "NE"
                        && sState != "NH"
                        && sState != "NJ"
                        && sState != "NM"
                        && sState != "NV"
                        && sState != "NY"
                        && sState != "OH"
                        && sState != "OK"
                        && sState != "OR"
                        && sState != "PA"
                        && sState != "RI"
                        && sState != "SC"
                        && sState != "SD"
                        && sState != "TN"
                        && sState != "TX"
                        && sState != "UT"
                        && sState != "VA"
                        && sState != "VT"
                        && sState != "WA"
                        && sState != "WI"
                        && sState != "WV"
                        && sState != "WY"
                        )
                    {
                        sResponseToKey += "ERROR: state (" + sState + ") is not a known US state.\r\n\r\n ";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("status"))
            {
                sStatus_Code = element.Attribute("code").Value;
            }

            foreach (XElement element in doc.Descendants("street"))
            {
                sStreet = element.Value.ToUpper();
                if (!String.IsNullOrEmpty(sStreet))
                {
                    bStreet_Received = true;
                    if (sStreet.Length > 200)
                    {
                        sResponseToKey += "ERROR: street (" + sStreet + ") exceeded the 200 character max and has been truncated.\r\n\r\n ";
                        sStreet = sStreet.Substring(0, 200);
                    }
                    sStreet = ccl.Clean(sStreet, 200);
                }
            }

            foreach (XElement element in doc.Descendants("requestSyncType"))
            {
                sRequestSyncType = element.Value;
                if (!String.IsNullOrEmpty(sRequestSyncType))
                {
                    bRequestSyncType_Received = true;
                    if (sRequestSyncType != "synchronous" && sRequestSyncType != "asynchronous")
                    {
                        sResponseToKey += "ERROR: requestSyncType 'action' (" + sRequestSyncType + ") is not one of the two valid values 'synchronous' and 'asynchronous'.\r\n\r\n ";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("ticketInfo"))
            {
                if (element.HasAttributes == true)
                {
                    if (element.Attribute("ticketNumber") != null && element.Attribute("ticketNumber").Value != null)
                        sTicketInfo_TicketNumber = element.Attribute("ticketNumber").Value;
                }
            }

            foreach (XElement element in doc.Descendants("transDateStamp"))
            {
                sTransDateStamp = element.Value;
                if (!String.IsNullOrEmpty(sTransDateStamp))
                {
                    bTransDateStamp_Received = true;
                    if (DateTime.TryParse(sTransDateStamp, out datTransaction) == false)
                    {
                        sResponseToKey += "ERROR: transDateStamp (" + sTransDateStamp + ") could not be parsed as a valid date (i.e February 31st).\r\n\r\n ";
                    }
                }
            }

            foreach (XElement element in doc.Descendants("zip"))
            {
                sZip = element.Value.ToUpper();
                if (!String.IsNullOrEmpty(sZip))
                {
                    bZip_Received = true;
                    if (sZip.Length > 200)
                    {
                        sResponseToKey += "ERROR: zip (" + sZip + ") exceeded the 20 character max and has been truncated.\r\n\r\n ";
                        sZip = sZip.Substring(0, 20);
                    }
                    sZip = ccl.Clean(sZip, 20);
                }

                bZip_Received = true;
            }

            // ==========================================================
            // VALIDATION: KeyBank Concerns (Manditory Fields)
            // ==========================================================
            if (bCompany_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory contact 'company' element not received.\r\n\r\n ";
            }
            if (bCountryCode_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'phone' element countryCode attribute not received.\r\n\r\n ";
            }
            if (bDeploymentMode_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory request element 'deploymentMode' attribute not received.\r\n\r\n ";
            }
            if (bDocType_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'docType' element not received.\r\n\r\n ";
            }
            if (bFirstName_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory contact 'firstName' element not received.\r\n\r\n ";
            }
            if (bLastName_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory contact 'lastName' element not received.\r\n\r\n ";
            }
            if (bOpenedDateStamp_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'openedDateStamp' element not received.\r\n\r\n ";
            }
            if (bPhone_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory contact 'phone' element not received.\r\n\r\n ";
            }
            if (bProblemDescription_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'problemDescription' element not received.\r\n\r\n ";
            }
            if (bReceiver_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'receiver' element not received.\r\n\r\n ";
            }
            if (bRefTicketNumber_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'refTicketNumber' element not received.\r\n\r\n ";
            }
            if (bRequestSyncType_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'requestSyncType' element not received.\r\n\r\n ";
            }
            if (bSender_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'sender' element not received.\r\n\r\n ";
            }
            if (bSentDateStamp_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'sentDateStamp' element not received.\r\n\r\n ";
            }
            if (bTransactionId_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory serviceDocument element 'transactionId' attribute not received.\r\n\r\n ";
            }
            if (bTimestamp_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory serviceDocument element 'timestamp' attribute not received.\r\n\r\n ";
            }
            if (bTransDateStamp_Received == false)
            {
                sResponseToKey += "ERROR: Mandatory 'transDateStamp' element not received.\r\n\r\n ";
            }
            // ==========================================================
            // CONDITIONAL REQUIREMENTS
            // ==========================================================
            // Customize the error message between "new" and "update" posts
            if (sServiceRequest_Action == "update")
                sFailureConsequence = "ticket update was stopped";
            else
                sFailureConsequence = "ticket placement was stopped";

            if (bServiceAddress_Site_Received == false)
            {
                if (bStreet_Received == false)
                {
                    sResponseToKey += "ERROR: Mandatory 'street' element not received (Required when no serviceAddress site received).\r\n\r\n ";
                }
                if (bCity_Received == false)
                {
                    sResponseToKey += "ERROR: Mandatory 'city' element not received (Required when no serviceAddress site received).\r\n\r\n ";
                }
                if (bState_Received == false)
                {
                    sResponseToKey += "ERROR: Mandatory 'state' element not received (Required when no serviceAddress site received).\r\n\r\n ";
                }
                if (bZip_Received == false)
                {
                    sResponseToKey += "ERROR: Mandatory 'zip' element not received (Required when no serviceAddress site received).\r\n\r\n ";
                }
            }


            // Isabel 05/02/2016
            // 12am Midnight transmission in Cleveland would equal 4am in Greenwich so you ADD offset hours
            if (datTransmission > datZero && datTransmission.AddHours(dHoursFromGreenwichEst) < datOpened)
            {
                sResponseToKey += "ERROR: Stamps imply transmission occurred before ticket was opened.  xml transmission stamp: serviceDocument timestamp (" + sServiceDocument_Timestamp + ") must not be prior to the openedDateStamp (" + sOpenedDateStamp + ").\r\n\r\n ";
            }

            if (datOpened > datZero && datOpened > datNowGreenwich)
            {
                sResponseToKey += "ERROR: openedDateStamp received appears to be in the future (" + sOpenedDateStamp + ").  It must not be greater than the current Greenwich time (" + datNowGreenwich.ToString("o").Substring(0, 19) + ").\r\n\r\n ";
            }
            if (datSent > datZero && datSent < datOpened)
            {
                sResponseToKey += "ERROR: Stamps imply request sent before ticket was opened.  sentDateStamp (" + sSentDateStamp + ") must not be prior to the opened stamp (" + sOpenedDateStamp + ").\r\n\r\n ";
            }
            if (bServiceRequest_Action_Received == false)
            {
                sResponseToKey += "ERROR: serviceRequest element 'action' attribute not received.\r\n\r\n ";
            }
            // ==========================================================
            // PRIMARY VALIDATION (Deal Breakers)
            // ==========================================================
            // Even after ctr/tck created (Manual by HTS) I have to keep trying to find matching cust/machines to see what the current state is on the key bank side
            // So you can't globally block this section off, just block error message if call exists
            //if (iCtr == 0 && iTck == 0)  // This means you MUST find a match above to stop this validation...
            //{

            if (sCloseAcknowledged == "1")
            {
                sResponseToKey += "ERROR: Ticket was closed at " + sCompletedDateStamp + ".  No further update is permitted.\r\n\r\n ";
            }
            // Ensure you can find customer service location
            if (String.IsNullOrEmpty(sCompany) && String.IsNullOrEmpty(sServiceAddress_Site))
            {
                if (iCtr == 0 && iTck == 0)
                    sResponseToKey += "ERROR: Both 'company' and serviceAddress 'site' were blank or null.  " + sFailureConsequence + "\r\n\r\n ";
            }
            else
            {
                string[] saCs1Cs2NamXrfAdrCitStaZipCnt = new string[1];
                sSiteXref = "";
                if (!String.IsNullOrEmpty(sServiceAddress_Site)) // Use site first
                    sSiteXref = sServiceAddress_Site;
                else // use company second
                    sSiteXref = sCompany;

                string[] saCs1Cs2NamAdrCitStaZip = { "", "", "", "", "", "", "" };

                // The validation is being repeated TWICE: once for live and again for test (complex, but seems to be needed)
                // The additional check for city and state was probably not needed here, because unit is not chosen until later
                if (sLibrary == "OMDTALIB")
                {
                    saCs1Cs2NamXrfAdrCitStaZipCnt = wsKeyLive.GetCs1Cs2NamXrfAdrCitStaZipCnt_BySerXrf(sWsKey, sSerial, sSiteXref);
                    if (saCs1Cs2NamXrfAdrCitStaZipCnt.Length > 8) 
                    {
                        if (saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0")) // Serial + Xref search returned "0" records
                        {
                            saCs1Cs2NamXrfAdrCitStaZipCnt = wsKeyLive.GetCs1Cs2NamXrfAdrCitStaZipCnt_BySer(sWsKey, sSerial);
                            if (!String.IsNullOrEmpty(sSerial) && !saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0") && !saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1"))
                                sResponseToHts += "ERROR: DUPLICATE KeyBank serial (" + sSerial + ").  The closest match to city and state will be selected but it may be WRONG.\r\n\r\n ";
                        }
                        else if (!String.IsNullOrEmpty(sSerial) && !saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1"))
                            sResponseToHts += "ERROR: DUPLICATE KeyBank serial (" + sSerial + ") and KeyBank Xref (" + sSiteXref + ").  The closest match to city and state will be selected but it may be WRONG.\r\n\r\n ";

                        if (saCs1Cs2NamXrfAdrCitStaZipCnt.Length > 8) 
                        {
                            sHtsCs1 = saCs1Cs2NamXrfAdrCitStaZipCnt[0];
                            sHtsCs2 = saCs1Cs2NamXrfAdrCitStaZipCnt[1];
                            sHtsNam = saCs1Cs2NamXrfAdrCitStaZipCnt[2];
                            sHtsXrf = saCs1Cs2NamXrfAdrCitStaZipCnt[3];
                            sHtsAdr = saCs1Cs2NamXrfAdrCitStaZipCnt[4];
                            sHtsCit = saCs1Cs2NamXrfAdrCitStaZipCnt[5];
                            sHtsSta = saCs1Cs2NamXrfAdrCitStaZipCnt[6];
                            sHtsZip = saCs1Cs2NamXrfAdrCitStaZipCnt[7];
                            sHtsCnt = saCs1Cs2NamXrfAdrCitStaZipCnt[8];
                        }

                        if (saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0"))
                            sResponseToKey += "ERROR: KeyBank serial (" + sSerial + ") not found at HTS. Manual HTS call placement will be needed.\r\n\r\n ";
                        else 
                        {
                            if (!String.IsNullOrEmpty(sSerial) && !saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1")) // Multiple matches
                                sResponseToHts += "WARNING ERROR: KeyBank serial (" + sSerial + ") found at multiple locations.\r\n\r\n ";
                            if (!String.IsNullOrEmpty(sSerial) && sHtsXrf != sSiteXref && sSiteXref != "") 
                            {
                                saCs1Cs2NamAdrCitStaZip = wsKeyLive.GetCs1Cs2NamAdrCitStaZip_ByXrf(sWsKey, sSiteXref);
                                string sXrfAdr = "";
                                if (saCs1Cs2NamAdrCitStaZip.Length == 7) 
                                {
                                    sXrfAdr = saCs1Cs2NamAdrCitStaZip[3] + " " +
                                        saCs1Cs2NamAdrCitStaZip[4] + " " +
                                        saCs1Cs2NamAdrCitStaZip[5] + " " +
                                        saCs1Cs2NamAdrCitStaZip[6] + " " +
                                        saCs1Cs2NamAdrCitStaZip[2] + "  Cust: " +
                                        saCs1Cs2NamAdrCitStaZip[0] + "-" +
                                        saCs1Cs2NamAdrCitStaZip[1];
                                }
                                    
                                
                                //sResponseToHts += "WARNING ERROR: KeyBank serial (" + sSerial + ") was matched -- but the serial's HTS Xref (" + sHtsXrf + ") does not match the KeyBank Xrf (" + sSiteXref + ").  Call creation will still be attempted based on the HTS serial location.\r\n\r\n ";
                                sResponseToHts += "WARNING ERROR: KeyBank serial (" + sSerial + ") matched -- but serial's HTS Xref \r\n(" +
                                    sHtsXrf + " -- " + sHtsAdr + " " + sHtsCit + " " + sHtsSta + " " + sHtsZip +
                                    ") does not match the KeyBank Xrf \r\n(" +
                                    sSiteXref + " -- " + sXrfAdr +
                                    ").  " +
                                    ")\r\n For comparison the typed out KeyBank Ticket street address is \r\n(" +
                                    sStreet + " " + sCity + " " + sState + " " + sZip +
                                    ").  " + 
                                    "\r\nPlease review.\r\n\r\n ";
                            }
                        }
                    }
                }
                else // RUN FOR TEST
                {
                    saCs1Cs2NamXrfAdrCitStaZipCnt = wsKeyDev.GetCs1Cs2NamXrfAdrCitStaZipCnt_BySerXrf(sWsKey, sSerial, sSiteXref);
                    if (saCs1Cs2NamXrfAdrCitStaZipCnt.Length > 8) 
                    {
                        if (saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0")) // Serial + Xref search returned "0" records
                        {
                            saCs1Cs2NamXrfAdrCitStaZipCnt = wsKeyDev.GetCs1Cs2NamXrfAdrCitStaZipCnt_BySer(sWsKey, sSerial);
                            if (!saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0") && !saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1"))
                                sResponseToHts += "ERROR: DUPLICATE KeyBank serial (" + sSerial + "). The closest match to city and state will be selected but it may be WRONG.\r\n\r\n ";
                        }
                        else if (!saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1"))
                            sResponseToHts += "ERROR: DUPLICATE KeyBank serial (" + sSerial + ") and KeyBank Xref (" + sSiteXref + ").  The closest match to city and state will be selected but it may be WRONG.\r\n\r\n ";

                        if (saCs1Cs2NamXrfAdrCitStaZipCnt.Length > 8) 
                        {
                            sHtsCs1 = saCs1Cs2NamXrfAdrCitStaZipCnt[0];
                            sHtsCs2 = saCs1Cs2NamXrfAdrCitStaZipCnt[1];
                            sHtsNam = saCs1Cs2NamXrfAdrCitStaZipCnt[2];
                            sHtsXrf = saCs1Cs2NamXrfAdrCitStaZipCnt[3];
                            sHtsAdr = saCs1Cs2NamXrfAdrCitStaZipCnt[4];
                            sHtsCit = saCs1Cs2NamXrfAdrCitStaZipCnt[5];
                            sHtsSta = saCs1Cs2NamXrfAdrCitStaZipCnt[6];
                            sHtsZip = saCs1Cs2NamXrfAdrCitStaZipCnt[7];
                            sHtsCnt = saCs1Cs2NamXrfAdrCitStaZipCnt[8];
                        }

                        if (saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("0"))
                            sResponseToKey += "ERROR: serial (" + sSerial + ") not found at HTS. HTS manual placement required.\r\n\r\n ";
                        else 
                        {
                            if (!saCs1Cs2NamXrfAdrCitStaZipCnt[8].Equals("1")) // Multiple matches
                                sResponseToHts += "WARNING ERROR: KeyBank serial (" + sSerial + ") found at multiple locations.\r\n\r\n ";
                            if (sHtsXrf != sSiteXref) 
                            {
                                saCs1Cs2NamAdrCitStaZip = wsKeyDev.GetCs1Cs2NamAdrCitStaZip_ByXrf(sWsKey, sSiteXref);
                                string sXrfAdr = "";
                                if (saCs1Cs2NamAdrCitStaZip.Length == 7)
                                {
                                    sXrfAdr = saCs1Cs2NamAdrCitStaZip[3] + " " +
                                        saCs1Cs2NamAdrCitStaZip[4] + " " +
                                        saCs1Cs2NamAdrCitStaZip[5] + " " +
                                        saCs1Cs2NamAdrCitStaZip[6] + " " +
                                        saCs1Cs2NamAdrCitStaZip[2] + "  Cust: " +
                                        saCs1Cs2NamAdrCitStaZip[0] + "-" +
                                        saCs1Cs2NamAdrCitStaZip[1];
                                }


                                //sResponseToHts += "WARNING ERROR: KeyBank serial (" + sSerial + ") was matched -- but the serial's HTS Xref (" + sHtsXrf + ") does not match the KeyBank Xrf (" + sSiteXref + ").  Call creation will still be attempted based on the HTS serial location.\r\n\r\n ";
                                sResponseToHts += "The serial's HTS Xref \r\n(" +
                                    sHtsXrf + " -- " + sHtsAdr + " " + sHtsCit + " " + sHtsSta + " " + sHtsZip +
                                    ") does not match the KeyBank Xrf \r\n(" +
                                    sSiteXref + " -- " + sXrfAdr +
                                    ").  " +
                                    ")\r\n For comparison: KeyBank Ticket typed out street address \r\n(" +
                                    sStreet + " " + sCity + " " + sState + " " + sZip +
                                    ").  " +
                                    "\r\nCall creation will still be attempted based on the HTS serial location.\r\n\r\n ";
                            }
                        }
                    }
                }
            }

            // NOW PROCESS FOR THE SERIAL NUMBER
            // Ensure KeyBank model has single corresponding HTS machine
            if (String.IsNullOrEmpty(sSerial))
            {
                if (iCtr == 0 && iTck == 0)
                    sResponseToKey += "ERROR: the KeyBank serial was blank or null.  " + sFailureConsequence + "\r\n\r\n ";
            }
            else
            {
                // if no ticket created yet (auto or HTS manual) validate and try to create
                // THIS MEANS THE SERIAL ALONE DETERMINES CS1 CS2 LOCATION USED, 
                // THE KEY COMPANY/SITE REFERENCE IS DISREGARDED (other than error messaging warning of differences) 
                // BUT THE HARD CODED KEY ADDRESS WILL BE USED IN SVRTICKD TO DIRECT TO THE SITE
                string[] saUntModAgrCs1Cs2AdrCitStaZip = new string[1];

                if (sLibrary == "OMDTALIB")
                {
                    saUntModAgrCs1Cs2AdrCitStaZip = wsKeyLive.GetUntModAgrCs1Cs2AdrCitStaZip(sWsKey, sSerial);
                }
                else
                {
                    saUntModAgrCs1Cs2AdrCitStaZip = wsKeyDev.GetUntModAgrCs1Cs2AdrCitStaZip(sWsKey, sSerial);
                }

                if (saUntModAgrCs1Cs2AdrCitStaZip.Length > 8)  // Found at least 1 matching machine
                {
                    sHtsUntNumList = saUntModAgrCs1Cs2AdrCitStaZip[0];
                    sHtsUntModList = saUntModAgrCs1Cs2AdrCitStaZip[1];
                    sHtsUntAgrList = saUntModAgrCs1Cs2AdrCitStaZip[2];
                    sHtsUntCs1List = saUntModAgrCs1Cs2AdrCitStaZip[3];
                    sHtsUntCs2List = saUntModAgrCs1Cs2AdrCitStaZip[4];
                    sHtsUntAdrList = saUntModAgrCs1Cs2AdrCitStaZip[5];
                    sHtsUntCitList = saUntModAgrCs1Cs2AdrCitStaZip[6];
                    sHtsUntStaList = saUntModAgrCs1Cs2AdrCitStaZip[7];
                    sHtsUntZipList = saUntModAgrCs1Cs2AdrCitStaZip[8];

                    string[] saUnt = new string[1];
                    saUnt = sHtsUntNumList.Split(cSplitter);
                    string[] saMod = new string[1];
                    saMod = sHtsUntModList.Split(cSplitter);
                    string[] saAgr = new string[1];
                    saAgr = sHtsUntAgrList.Split(cSplitter);
                    string[] saCs1 = new string[1];
                    saCs1 = sHtsUntCs1List.Split(cSplitter);
                    string[] saCs2 = new string[1];
                    saCs2 = sHtsUntCs2List.Split(cSplitter);
                    string[] saAdr = new string[1];
                    saAdr = sHtsUntAdrList.Split(cSplitter);
                    string[] saCit = new string[1];
                    saCit = sHtsUntCitList.Split(cSplitter);
                    string[] saSta = new string[1];
                    saSta = sHtsUntStaList.Split(cSplitter);
                    string[] saZip = new string[1];
                    saZip = sHtsUntZipList.Split(cSplitter);

                    int idx = 0;
                    if (saUnt.Length > 0)
                    {
                        for (idx = 0; idx < saUnt.Length; idx++) // loop through ALL duplicate serial units
                        {
                            
                            if (
                                    String.IsNullOrEmpty(sHtsUntNumBest) // take SOMETHING from what was found regardless
                                    ||
                                    (
                                           sHtsUntCitBest != sCity                      // you're not at risk of abandoning a perfect city/state match
                                        && sHtsUntStaBest != sState                     // 
                                        && saSta.Length > idx && saSta[idx] == sState   // and the current state is the key service state
                                    )
                                )
                            if (saUnt.Length > idx)
                                sHtsUntNumBest = saUnt[idx];
                            if (saMod.Length > idx)
                                sHtsUntModBest = saMod[idx];
                            if (saAgr.Length > idx)
                                sHtsUntAgrBest = saAgr[idx];
                            if (saCs1.Length > idx)
                                sHtsUntCs1Best = saCs1[idx];
                            if (saCs2.Length > idx)
                                sHtsUntCs2Best = saCs2[idx];
                            if (saAdr.Length > idx)
                                sHtsUntAdrBest = saAdr[idx];
                            if (saCit.Length > idx)
                                sHtsUntCitBest = saCit[idx];
                            if (saSta.Length > idx)
                                sHtsUntStaBest = saSta[idx];
                            if (saZip.Length > idx)
                                sHtsUntZipBest = saZip[idx];
                        }
                    }

                    // Only do this part if center and ticket have yet to be loaded
                    if (iCtr == 0 && iTck == 0)
                    {
                        if (saUnt.Length > 1) // Multiple units found matching key serial
                        {
                            //sResponseToKey += "ERROR: Multiple HTS machines were found having KeyBank serial " + sSerial + ".  Call placement will be manually handled after inquiry by HTS.\r\n\r\n ";
                            sResponseToHts += "ERROR: Duplicate KeyBank serial " + sSerial + " found.  Review needed.\r\n\r\n ";

                            for (int i = 0; i < saUnt.Length; i++)
                            {
                                sResponseToHts += "\r\n Hts Unit " + (i + 1).ToString() + ") " + saUnt[i];
                                if (saMod.Length > i)
                                    sResponseToHts += " Model " + saMod[i];
                                if (saAgr.Length > i)
                                    sResponseToHts += " Agr " + saAgr[i];
                                if (saCs1.Length > i)
                                    sResponseToHts += " Cs1 " + saCs1[i];
                                if (saCs2.Length > i)
                                    sResponseToHts += " Cs2 " + saCs2[i];
                            }
                            sResponseToHts += "\r\n\r\n ";
                            sResponseToHts += "The one selected was " + 
                                "\r\nUnit: " + sHtsUntNumBest + 
                                "\r\nModel: " + sHtsUntNumBest +
                                "\r\nAgree: " + sHtsUntAgrBest +
                                "\r\nCust: " + sHtsUntCs1Best +
                                "\r\nLoc: " + sHtsUntCs2Best +
                                "\r\nAdr: " + sHtsUntAdrBest +
                                "\r\nCity: " + sHtsUntCitBest +
                                "\r\nState: " + sHtsUntStaBest +
                                "\r\nZip: " + sHtsUntZipBest;
                            sResponseToHts += "\r\n\r\n ";
                        }
                        // check if machine cs1/cs2 is the same as the service site cs1/cs2
                        else if (sHtsCnt == "1" && (String.IsNullOrEmpty(sHtsCs1) || String.IsNullOrEmpty(sHtsCs2)))
                        {
                            sResponseToHts += "ERROR: the service location cross ref " + sSiteXref + " did not match any cross ref currently loaded in our customer master file.\r\n\r\n ";
                        }
                        else if (sHtsCs1 != sHtsUntCs1Best || sHtsCs2 != sHtsUntCs2Best)
                        {
                            sResponseToKey += "ERROR: The HTS location of the request machine with serial " + sSerial + " (Hts Customer " + sHtsUntCs1Best + "-" + sHtsUntCs2Best + ")" +
                                " does not match the KeyBank request service address.  (Hts Customer " + sHtsCs1 + "-" + sHtsCs2 + ").  If call placement succeeds, please confirm the tech is being sent to the right location.\r\n\r\n ";
                        }
                        if (!String.IsNullOrEmpty(sState) && sHtsUntStaBest != sState.ToUpper() && sHtsUntStaBest != "") // attempting to stop blank notices
                        {
                            sResponseToKey += "ERROR: The State (" + sHtsUntStaBest + ") of the machine with serial number (" + sSerial + ") does not match the state (" + sState.ToUpper() + ") of the KeyBank request's service address.  " +
                            "Manual HTS review and call placement required\r\n\r\n ";
                        }

                        /*
                        if (sHtsUntCitBest != sCity.ToUpper() || sHtsUntStaBest != sState.ToUpper()) 
                        {
                            sResponseToKey += "ERROR: The City (" + sHtsUntCitBest + ") and/or State (" + sHtsUntStaBest + ") of the machine with serial number (" + sSerial + ") does not match the city (" + sCity.ToUpper() + ") and/or state (" + sState.ToUpper() + ") of the KeyBank request's service address.  " +
                            "Call placement will be manually handled after inquiry by HTS\r\n\r\n ";
                        }
                        */

                        if (
                            (!String.IsNullOrEmpty(sCity) && sHtsUntCitBest != sCity.ToUpper() && sHtsUntCitBest != "")
                            || (!String.IsNullOrEmpty(sState) && sHtsUntStaBest != sState.ToUpper() && sHtsUntStaBest != "")
                            ) 
                        {
                            sResponseToHts += "ADDRESS MISMATCH: Serial (" + sSerial + ") assigned to HTS location  \r\n(" +
                                sHtsUntAdrBest + " " + sHtsUntCitBest + ", " + sHtsUntStaBest + " " + sHtsUntZipBest + 
                                ") but KeyBank service address is \r\n(" +
                                sStreet.ToUpper() + " " + sCity.ToUpper() + " " + sState.ToUpper() + " " + sZip.ToUpper() + ") " +
                            " \r\nReview needed \r\n\r\n ";
                        }

                    } // end larger section only containing error messages (no data lookups) 
                }
                else // NO MACHINES FOUND MATCHING SERIAL
                {
                    if (iCtr == 0 && iTck == 0)
                        sResponseToKey += "ERROR: KeyBank serial " + sSerial + " not found.  HTS review and manual call placement required.\r\n\r\n ";
                }
            }
            // Even after ctr/tck created (Manual by HTS) I have to keep trying to find maching cust/machines to see what the current state is on the key bank side
            //} // if no HTS Call exists yet
        }
        catch (Exception ex)
        {
            Send_Error(ex.Message.ToString(), ex.ToString(), sWebService);
        }
        finally
        {
        }
        // -----------------------
    }
    // -----------------------------------------------------------    
    protected string Build_Xml_Response(string responseToKey)
    {
        string sXml = "";
        string sGreenwichStamp = DateTime.Now.ToUniversalTime().ToString("o").Substring(0, 19);
        string[] saLastFirst = { "", "" };

        XElement xe1, xe2, xe3, xe4;
        XAttribute xa;
        string[] saVal = new string[1];
        //string sCall = "";

        // Create a root node
        xe1 = new XElement("serviceDocument");
        // Add attributes
        xa = new XAttribute("version", "2.1");
        xe1.Add(xa);

        xa = new XAttribute("transactionId", sServiceDocument_TransactionId);
        xe1.Add(xa);

        xa = new XAttribute("timestamp", sGreenwichStamp);
        xe1.Add(xa);

        // Level 2 elements
        xe2 = new XElement("response");

        // Level 3 elements
        string sStatus_code = "";
        string sStatusValue = "";


        // ** Create the call all at once during network processing and pass back "SUCCESS|123-45678"
        if (responseToKey.StartsWith("SUCCESS"))
        {
            saVal = responseToKey.Split(cSplitter);
            if (saVal.Length > 1)
                sCtr = saVal[1];
            if (saVal.Length > 2)
                sTck = saVal[2];
            if (saVal.Length > 3)
                sSrq = saVal[3];

            if (saVal.Length > 3) 
            {
                if (int.TryParse(sCtr, out iCtr) == false)
                    iCtr = 0;
                if (int.TryParse(sTck, out iTck) == false)
                    iTck = 0;
                if (int.TryParse(sSrq, out iSrq) == false)
                    iSrq = 0;
            }

            sStatus_code = "200";
            sStatusValue = "OK";

            // parse call number from Result for insertion in xml

            /*
            if (sLibrary == "OMDTALIB")
                saLastFirst = wsKeyLive.GetFstLastFirstNames(iCtr, iTck);
            else
                saLastFirst = wsKeyDev.GetFstLastFirstNames(iCtr, iTck);

            if (saLastFirst.Length > 0)
                sTechLastName = saLastFirst[0];
            if (saLastFirst.Length > 1)
                sTechFirstName = saLastFirst[1];
             */ 
        }
        else
        {
            sStatus_code = "500";
            sStatusValue = responseToKey;

            // -------------------------------------------------
            // Try to retrieve SRQ workfile key if it was created, but ticket creation failed.
            // This will be saved during the standard update save UpdateInboundRequestValuesFromCall
            if (sResponseToKey.StartsWith("FAILURE"))
            {
                string[] saFaiSrqNumErr = sResponseToKey.Split(cSplitter);
                if (saFaiSrqNumErr.Length > 3)
                {
                    if (int.TryParse(saFaiSrqNumErr[2], out iSrq) == false)
                        iSrq = 0;
                    sResponseToKey = saFaiSrqNumErr[3];
                }
            }
            // -------------------------------------------------

        }

        xe3 = new XElement("status", sStatusValue);
        xa = new XAttribute("code", sStatus_code);
        xe3.Add(xa);
        xe2.Add(xe3);

        if (responseToKey.StartsWith("SUCCESS"))
        {
            string sCtrTck = "";
            if (sCtr != "" && sTck != "")
                sCtrTck = sCtr + "-" + sTck;
            xe3 = new XElement("ticketNumber", sCtrTck);
            xe2.Add(xe3);

            /*

            // BEGIN: technician (Level 3) ------------------------------
            xe3 = new XElement("technician");

            // BEGIN: (Level 4 elements) ------------------------------
            xe4 = new XElement("firstName", sTechFirstName);
            xe3.Add(xe4);

            xe4 = new XElement("lastName", sTechLastName);
            xe3.Add(xe4);
            // END: (Level 4 elements) ------------------------------

            xe2.Add(xe3);
            // END: technician (Level 3) ------------------------------


            // BEGIN: ticketInfo (Level 3) ------------------------------
            xe3 = new XElement("ticketInfo");

            // BEGIN: (Level 4 elements) ------------------------------
            xe4 = new XElement("status");
            xa = new XAttribute("code", "Pending Vendor");
            xe4.Add(xa);
            xe3.Add(xe4);
            // END: (Level 4 elements) ------------------------------

            xe2.Add(xe3);
            // END: ticketInfo (Level 3) ------------------------------

            */

        }
        // Add loaded element 2 to element 1
        xe1.Add(xe2);

        sXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n\r\n  ";
        sXml += xe1.ToString();

        //string sFullPath = Server.MapPath("~") + "ReqInResponse1.xml";
        //sXmlTemp = System.IO.File.ReadAllText(sFullPath);

        XmlDocument xDocTemp = new XmlDocument();
        xDocTemp.LoadXml(sXml);
        // this splits off all the newline characters
        sXml = xDocTemp.InnerXml;

        return sXml;
    }
    // -----------------------------------------------------------    
    protected string encryptTicket(string ctr, string tck)
    {
        string sEncrypted = "";
        string sCtrTck = "";
        int j = 0;

        for (j = 0; j < 7; j++) {
            if (ctr.Length < 3)
                ctr = "0" + ctr;
            if (tck.Length < 7)
                tck = "0" + tck;
        }
        sCtrTck = ctr + tck;

        string[] saCode = {"", "", "", "", "", "", "", "", "", ""};

        saCode[0] = "aBcDeFgHiJ";
        saCode[1] = "kLmNoPqRsT";
        saCode[2] = "uVwXyZAbCd";
        saCode[3] = "EfGhIjKlMn";
        saCode[4] = "OpQrStUvWx";
        saCode[5] = "YzaBcDeFgH";
        saCode[6] = "iJkLmNoPqR";
        saCode[7] = "sTuVwXyZAb";
        saCode[8] = "CdEfGhIjKl";
        saCode[9] = "MnOpQrStUv";

        string sNums = "5820416937";
        int iTckNum = 0;
        string sReplacementChar = "";
        sEncrypted = sNums;
        string sCurrCode = "";

        for (j = 0; j < 10; j++) 
        {
            // for each pos 1-10 get number in ticket
            if (int.TryParse(sCtrTck.Substring(j, 1), out iTckNum) == false)
                iTckNum = 0;
            // replacement character = a) array of codes[loop num] b) character at replacement position
            sCurrCode = saCode[j];
            sReplacementChar = sCurrCode.Substring(iTckNum, 1);
            // use .Replace to move the replacement character to the new position in encrypted value
            sEncrypted = sEncrypted.Replace(j.ToString(), sReplacementChar);
        }

        return sEncrypted;
    }
    // -----------------------------------------------------------    
    protected int GetHoursFromGreenwich()
    {
        int iOffset = 0;

        string sTemp = DateTime.Now.ToString("o");
        string sOffset = "";
        if (sTemp.Length > 27)
        {
            sOffset = sTemp.Substring(28, 2);
            if (int.TryParse(sOffset, out iOffset) == false)
                iOffset = 0;
        }

        return iOffset;
    }
    // =======================================================
    // =======================================================
}

