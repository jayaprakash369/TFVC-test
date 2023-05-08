using System; 
using System.Collections.Generic; 
//using System.Linq; 
using System.Web; 
using System.Web.Services;

using System.IO;
using System.Xml;
using System.Data;
using System.Configuration;

/// <summary>
/// HTS Public Web Service -- 
/// How to Access: on an ASP.NET site, 
/// right click the App_WebReferences Folder, Select "Add Web Reference" -- 
/// in the pop up box address enter https://192.168.100.2/Main.asmx or https://ws.harlandts.com/Main.asmx
/// </summary>
[WebService(Namespace = "https://ws.harlandts.com/Main.asmx", Name = "HtsWebService_Main", Description = "Harland Technology Services Public Web Service: Main")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. // [System.Web.Script.Services.ScriptService]
// ===================================================================
public class Main : System.Web.Services.WebService 
{
    string sLibrary = ""; 
    string sWebService = ""; 
    string sMethodName = ""; 
    string sResult = "";
    string sDebug = "";
    
    //Emp_LIVE.EmployeeMenuSoapClient wsLiveEmp = new Emp_LIVE.EmployeeMenuSoapClient();
    //Emp_DEV.EmployeeMenuSoapClient wsDevEmp = new Emp_DEV.EmployeeMenuSoapClient();
    
    CommonCodeLibrary ccl = new CommonCodeLibrary();
    // ===================================================================
    public Main () {

        //Uncomment the following line if using designed components //InitializeComponent(); 
        //string connString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        //odbcConn = new OdbcConnection(connString);
        SiteHandler sh = new SiteHandler();
        sLibrary = sh.getLibrary();
        sh = null;
        if (sLibrary == "OMDTALIB")
            sWebService = "DMZ WS LIVE";
        else
            sWebService = "DMZ WS DEV";
    }
    // ========================================================================
    protected string GetCs1ListByAccessCode(string accessCode)
    {
        string sCs1List = "";

        if (accessCode == "RaiseTheCurtain")
            sCs1List = "79206";
        else if (accessCode == "WalkToThePark")
            sCs1List = "99996, 99999";

        return sCs1List;
    }
    // =========================================================================
    // Delete from here below and replace with test (or replace individual methods)
    // =========================================================================
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    // ========================================================================
    // Inner support methods
    // ========================================================================
    // ========================================================================
    protected XmlDocument ReqXml(string sts, string stsMsg, int ctr, int tck, int cs1, int cs2, int unt, string agr, string prt, string ser, string xrf, string svc, string prb, string com, string fac, string dad, string flx1, string flx2)
    {
        XmlDocument doc = new XmlDocument();

        XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
        doc.AppendChild(declaration);

        XmlElement root = doc.CreateElement("RequestResponse");
        doc.AppendChild(root);

        XmlElement status = doc.CreateElement("Status");
        status.InnerText = sts;
        root.AppendChild(status);

        if (stsMsg != "")
        {
            XmlElement statusMessage = doc.CreateElement("StatusMessage");
            statusMessage.InnerText = stsMsg;
            root.AppendChild(statusMessage);
        }
        if (sts == "OK")
        {
            XmlElement header = doc.CreateElement("RequestDetail");
            root.AppendChild(header);

            XmlElement center = doc.CreateElement("Center");
            center.InnerText = ctr.ToString();
            header.AppendChild(center);

            XmlElement ticket = doc.CreateElement("Ticket");
            ticket.InnerText = tck.ToString();
            header.AppendChild(ticket);

            XmlElement customer = doc.CreateElement("Customer");
            customer.InnerText = cs1.ToString();
            header.AppendChild(customer);

            XmlElement location = doc.CreateElement("Location");
            location.InnerText = cs2.ToString();
            header.AppendChild(location);

            XmlElement unit = doc.CreateElement("Unit");
            unit.InnerText = unt.ToString();
            header.AppendChild(unit);

            XmlElement agreement = doc.CreateElement("Agreement");
            agreement.InnerText = agr;
            header.AppendChild(agreement);

            XmlElement part = doc.CreateElement("Part");
            part.InnerText = prt;
            header.AppendChild(part);

            XmlElement serial = doc.CreateElement("Serial");
            serial.InnerText = ser;
            header.AppendChild(serial);

            XmlElement ticketXref = doc.CreateElement("TicketXref");
            ticketXref.InnerText = xrf;
            header.AppendChild(ticketXref);

            XmlElement serviceType = doc.CreateElement("ServiceType");
            serviceType.InnerText = svc;
            header.AppendChild(serviceType);

            XmlElement problem = doc.CreateElement("Problem");
            problem.InnerText = prb;
            header.AppendChild(problem);

            XmlElement comment = doc.CreateElement("Comment");
            comment.InnerText = com;
            header.AppendChild(comment);

            XmlElement printerInterface = doc.CreateElement("PrinterInterface");
            printerInterface.InnerText = fac;
            header.AppendChild(printerInterface);

            XmlElement creator = doc.CreateElement("Creator");
            creator.InnerText = dad;
            header.AppendChild(creator);

            XmlElement flex1 = doc.CreateElement("UserReference1");
            flex1.InnerText = flx1;
            header.AppendChild(flex1);

            XmlElement flex2 = doc.CreateElement("UserReference2");
            flex2.InnerText = flx2;
            header.AppendChild(flex2);

        }
        return doc;
    }
    // ========================================================================
    protected XmlDocument SampleXmlLocXrefLoad()
    {
        XmlDocument doc = new XmlDocument();

        XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
        doc.AppendChild(declaration);

        XmlElement root = doc.CreateElement("LocationSearch");
        doc.AppendChild(root);

        XmlElement accessCode = doc.CreateElement("AccessCode");
        accessCode.InnerText = "WalkToThePark";
        root.AppendChild(accessCode);

        XmlElement locCrossRef = doc.CreateElement("LocationCrossRef");
        locCrossRef.InnerText = "ORANGES";
        root.AppendChild(locCrossRef);

        return doc;
    }
    // ========================================================================
    //[WebMethod]
    protected XmlDocument SampleXmlRequestLoad()
    {
        XmlDocument doc = new XmlDocument();

        XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
        doc.AppendChild(declaration);

        XmlElement root = doc.CreateElement("ServiceRequest");
        XmlAttribute attr;

        attr = doc.CreateAttribute("xmlns");
        attr.Value = "https://ws.harlandts.com";
        root.Attributes.Append(attr);

        //        attr = doc.CreateAttribute("xmlns:xsi");
        //        attr.Value = "http://www.w3.org/2001/XMLSchema-instance";
        //        root.Attributes.Append(attr);

        attr = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        attr.Value = "https://ws.harlandts.com https://ws.harlandts.com/Request.xsd";
        root.Attributes.Append(attr);

        doc.AppendChild(root);

        XmlElement accessCode = doc.CreateElement("AccessCode");
        accessCode.InnerText = "WalkToThePark";
        root.AppendChild(accessCode);

        XmlElement agreement = doc.CreateElement("Agreement");
        agreement.InnerText = "92856";
        root.AppendChild(agreement);

        XmlElement unitId = doc.CreateElement("UnitId");
        unitId.InnerText = "1090881"; // Floppy Drive
        root.AppendChild(unitId);

        XmlElement problem = doc.CreateElement("Problem");
        problem.InnerText = "Brief customer problem description";
        root.AppendChild(problem);

        XmlElement ptrInterface = doc.CreateElement("PrinterInterface");
        ptrInterface.InnerText = "";
        root.AppendChild(ptrInterface);

        XmlElement ticketCrossRef = doc.CreateElement("TicketCrossRef");
        ticketCrossRef.InnerText = "INC0000078";
        root.AppendChild(ticketCrossRef);

        XmlElement comment = doc.CreateElement("Comment");
        comment.InnerText = "Extended customer comment concerning issue which becomes a note attached to the ticket";
        root.AppendChild(comment);

        XmlElement contact = doc.CreateElement("Contact");
        contact.InnerText = "Contact Name";
        root.AppendChild(contact);

        XmlElement phone = doc.CreateElement("Phone");
        phone.InnerText = "1112223333";
        root.AppendChild(phone);

        XmlElement extension = doc.CreateElement("Extension");
        extension.InnerText = "44444";
        root.AppendChild(extension);

        XmlElement email = doc.CreateElement("Email");
        email.InnerText = "htslog@yahoo.com";
        root.AppendChild(email);

        XmlElement creator = doc.CreateElement("Creator");
        creator.InnerText = "Steve.1";
        root.AppendChild(creator);

        XmlElement flexField = doc.CreateElement("FlexField");
        flexField.InnerText = "INC0000078";
        root.AppendChild(flexField);

        return doc;
    }
    // ===========================================================
    protected string[] LocXrefXmlUnload(XmlDocument doc)
    {
        string[] saCodXrf = { "", "" };

        try
        {
            XmlNode root = doc.DocumentElement;

            if (root.SelectSingleNode("AccessCode").ChildNodes[0] != null)
                saCodXrf[0] = root.SelectSingleNode("AccessCode").ChildNodes[0].Value;

            if (root.SelectSingleNode("LocationCrossRef").ChildNodes[0] != null)
                saCodXrf[1] = root.SelectSingleNode("LocationCrossRef").ChildNodes[0].Value;

        }
        catch (Exception ex)
        {
            //SendError(ex.Message.ToString(), ex.ToString(), "");
            string sDebug = ex.Message.ToString();
        }
        finally
        {
        }

        return saCodXrf;
    }
    // ===========================================================
    protected string[] RequestXmlUnload(XmlDocument doc)
    {
        string[] saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx = { "", "", "", "", "", "", "", "", "", "", "", "", "" };

        try
        {
            XmlNode root = doc.DocumentElement;

            if (root.SelectSingleNode("AccessCode") != null)
            {
                if (root.SelectSingleNode("AccessCode").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[0] = root.SelectSingleNode("AccessCode").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Agreement") != null)
            {
                if (root.SelectSingleNode("Agreement").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[1] = root.SelectSingleNode("Agreement").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("UnitId") != null)
            {
                if (root.SelectSingleNode("UnitId").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[2] = root.SelectSingleNode("UnitId").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Problem") != null)
            {
                if (root.SelectSingleNode("Problem").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[3] = root.SelectSingleNode("Problem").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("PrinterInterface") != null)
            {
                if (root.SelectSingleNode("PrinterInterface").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[4] = root.SelectSingleNode("PrinterInterface").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("TicketCrossRef") != null)
            {
                if (root.SelectSingleNode("TicketCrossRef").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[5] = root.SelectSingleNode("TicketCrossRef").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Comment") != null)
            {
                if (root.SelectSingleNode("Comment").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[6] = root.SelectSingleNode("Comment").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Contact") != null)
            {
                if (root.SelectSingleNode("Contact").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[7] = root.SelectSingleNode("Contact").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Phone") != null)
            {
                if (root.SelectSingleNode("Phone").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[8] = root.SelectSingleNode("Phone").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Extension") != null)
            {
                if (root.SelectSingleNode("Extension").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[9] = root.SelectSingleNode("Extension").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Email") != null)
            {
                if (root.SelectSingleNode("Email").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[10] = root.SelectSingleNode("Email").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("Creator") != null)
            {
                if (root.SelectSingleNode("Creator").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[11] = root.SelectSingleNode("Creator").ChildNodes[0].Value;
            }
            if (root.SelectSingleNode("FlexField") != null)
            {
                if (root.SelectSingleNode("FlexField").ChildNodes[0] != null)
                    saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx[12] = root.SelectSingleNode("FlexField").ChildNodes[0].Value;
            }
        }
        catch (Exception ex)
        {
            //SendError(ex.Message.ToString(), ex.ToString(), "");
            sDebug = ex.ToString();
        }
        finally
        {
        }

        return saCodAgrUntPrbFacXrfComConPhnExtEmlDadFlx;
    }
    // =======================================================
    // =======================================================
}
