using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Scantron_Body_A_Bar : System.Web.UI.MasterPage
{
    protected System.Drawing.Color scantronBlueMedium = System.Drawing.ColorTranslator.FromHtml("#006FA1");
    protected System.Drawing.Color scantronRed = System.Drawing.ColorTranslator.FromHtml("#AE132A");
    protected System.Drawing.Color scantronGrayMedium = System.Drawing.ColorTranslator.FromHtml("#8C8D8E");
    protected System.Drawing.Color scantronGrayDark = System.Drawing.ColorTranslator.FromHtml("#414042");

    protected System.Drawing.Color scantronYellowOrange = System.Drawing.ColorTranslator.FromHtml("#FEBC11");
    protected System.Drawing.Color scantronGreenBright = System.Drawing.ColorTranslator.FromHtml("#7AC03D");
    protected System.Drawing.Color scantronAqua = System.Drawing.ColorTranslator.FromHtml("#00B1AC");
    protected System.Drawing.Color scantronBlueRoyal = System.Drawing.ColorTranslator.FromHtml("#00A0DF");
    protected System.Drawing.Color scantronOrange = System.Drawing.ColorTranslator.FromHtml("#F47B20");
    protected System.Drawing.Color scantronGreenForest = System.Drawing.ColorTranslator.FromHtml("#007A40");
    protected System.Drawing.Color scantronTealDark = System.Drawing.ColorTranslator.FromHtml("#005961");
    protected System.Drawing.Color scantronPurpleDark = System.Drawing.ColorTranslator.FromHtml("#4d407e");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

            if (sUserURL.Contains("/contactinformation.aspx"))
                pnUtility_ColorBar.BackColor = scantronPurpleDark;
            else if (sUserURL.Contains("/changeyourpassword.aspx"))
                pnUtility_ColorBar.BackColor = scantronPurpleDark;
            else if (sUserURL.Contains("/custadmin/menu.aspx"))
                pnUtility_ColorBar.BackColor = scantronGreenForest;
            else if (sUserURL.Contains("/errorlog.aspx"))
                pnUtility_ColorBar.BackColor = scantronAqua;
            else if (sUserURL.Contains("/login.aspx"))
                pnUtility_ColorBar.BackColor = scantronYellowOrange;
            else if (sUserURL.Contains("/preferences.aspx"))
                pnUtility_ColorBar.BackColor = scantronGrayMedium;
            else if (sUserURL.Contains("/registration.aspx"))
                pnUtility_ColorBar.BackColor = scantronGreenBright;
            else if (sUserURL.Contains("/req/contact.aspx"))
                pnUtility_ColorBar.BackColor = scantronBlueRoyal;
            // pnUtility_ColorBar.BackColor = scantronGreenForest;
            else if (sUserURL.Contains("/req/equipment.aspx")) // it still thinks it's the contact page...
                pnUtility_ColorBar.BackColor = scantronGrayMedium;
            else if (sUserURL.Contains("/req/location.aspx"))
                pnUtility_ColorBar.BackColor = scantronBlueRoyal;
            else if (sUserURL.Contains("/req/problem.aspx"))
                pnUtility_ColorBar.BackColor = scantronBlueRoyal;
            else if (sUserURL.Contains("/req/result.aspx"))
                pnUtility_ColorBar.BackColor = scantronAqua;
            else if (sUserURL.Contains("/req/serial.aspx"))
                pnUtility_ColorBar.BackColor = scantronAqua;
            else if (sUserURL.Contains("/servicehistory.aspx"))
                pnUtility_ColorBar.BackColor = scantronOrange;
            else if (sUserURL.Contains("/shared/menu.aspx"))
                pnUtility_ColorBar.BackColor = scantronGreenBright;
            else if (sUserURL.Contains("/updatetonercontact.aspx"))
                pnUtility_ColorBar.BackColor = scantronRed;
            else if (sUserURL.Contains("/usermaintenance.aspx"))
                pnUtility_ColorBar.BackColor = scantronYellowOrange;
            else
                pnUtility_ColorBar.BackColor = scantronAqua;
        }
    }
}
