using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_sc_map_OpenTicketXml : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadXml();
        }
    }
    // =========================================================
    protected void loadXml()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        string sXml = "";
        int iCs1 = 0;
        int iCs2 = 0;
        int iCtr = 0;
        int iTck = 0;
        string sName = "";
        string sAddress1 = "";
        string sAddress2 = "";
        string sCity = "";
        string sState = "";
        string sZip = "";
        string sLatZip = "";
        string sLngZip = "";
        int iLatAdr = 0;
        int iLngAdr = 0;
        //decimal dcLatAdr = 0;
        //decimal dcLngAdr = 0;

        string sCs1 = Request.QueryString["cs1"];
        string sMod = Request.QueryString["mod"];

        if (int.TryParse(sCs1, out iCs1) == false)
            iCs1 = 0;

        if (sPageLib == "L")
        {
            dataTable = wsLive.GetMapData(sfd.GetWsKey(), iCs1, sMod);
        }
        else
        {
            dataTable = wsTest.GetMapData(sfd.GetWsKey(), iCs1, sMod);
        }

        if (dataTable.Rows.Count > 0)
        {
            sXml = "" +
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + System.Environment.NewLine +
                "<points>";

            int iRowIdx = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (int.TryParse(dataTable.Rows[iRowIdx]["Ctr"].ToString(), out iCtr) == false)
                    iCtr = 0;
                if (int.TryParse(dataTable.Rows[iRowIdx]["Tck"].ToString(), out iTck) == false)
                    iTck = 0;
                sName = dataTable.Rows[iRowIdx]["Name"].ToString().Trim();
                sAddress1 = dataTable.Rows[iRowIdx]["Address1"].ToString().Trim();
                sAddress2 = dataTable.Rows[iRowIdx]["Address2"].ToString().Trim();
                sCity = dataTable.Rows[iRowIdx]["City"].ToString().Trim();
                sState = dataTable.Rows[iRowIdx]["State"].ToString().Trim();
                sZip = dataTable.Rows[iRowIdx]["Zip"].ToString().Trim();
                sLatZip = dataTable.Rows[iRowIdx]["LatZip"].ToString().Trim();
                sLngZip = dataTable.Rows[iRowIdx]["LngZip"].ToString().Trim();
                if (int.TryParse(dataTable.Rows[iRowIdx]["LatAdr"].ToString(), out iLatAdr) == false)
                    iLatAdr = 0;
                if (int.TryParse(dataTable.Rows[iRowIdx]["LngAdr"].ToString(), out iLngAdr) == false)
                    iLngAdr = 0;

                sXml += "<point>" +
                        "<center>" + iCtr.ToString() + "</center>" +
                        "<ticket>" + iTck.ToString() + "</ticket>" +
                        "<cust>" + iCs1.ToString() + "</cust>" +
                        "<cLoc>" + iCs2.ToString() + "</cLoc>" +
                        "<name>" + sName + "</name>" +
                        "<address>" + sAddress1 + " " + sAddress2 + " " + sCity + " " + sState + " " + sZip +  "</address>" +
                        "<lat>" + sLatZip + "</lat>" +
                        "<lng>" + sLngZip + "</lng>" +
                        "</point>";
                iRowIdx++;
            }

            sXml += "</points>";

        }
        Response.ContentType = "text/html";
        Response.Write(sXml);
        Response.End();
    }
    // =========================================================
    // =========================================================
}
