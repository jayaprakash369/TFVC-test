using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Scantron_BannerNav : System.Web.UI.MasterPage
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
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        //ControlMarketing();
        //MoveNotice();
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // This appears to only work when loaded in top parent...
        //Page.ClientScript.RegisterClientScriptInclude("jsGetXY", "/public/js/scantron/GetXY.js");
        //Page.ClientScript.RegisterClientScriptInclude("jsParentNav", "/public/js/scantron/ParentNav.js");
        if (!IsPostBack)
        {
            string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();

            if (sUserURL.Contains("/affiancesuite.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronYellowOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactCenter3.png";
              // may 02, 2018  pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/HigherEd.jpg";
            }
            else if (sUserURL.Contains("/authos.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronPurpleDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ServerRoom.png";
                // May 02 -  pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactCenter3.png";
            }
            else if (sUserURL.Contains("/certifications.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/WomanInHall.png";
            }
            else if (sUserURL.Contains("/company.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SixPortraits.jpg";
            }
            else if (sUserURL.Contains("/contact.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronYellowOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactRep.png";
            }
            else if (sUserURL.Contains("/depot.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGrayDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/MotherboardFan2.png";
                //  pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SmilingGuyatLaptop.png";
            }
            else if (sUserURL.Contains("/escalation.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGrayDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactRep.png";
            }
            else if (sUserURL.Contains("/hardwaremaintenance.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueRoyal;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/MotherboardFan2.png";
            }
            else if (sUserURL.Contains("/implementation.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGrayMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ServerRoom2.png";
            }
            else if (sUserURL.Contains("/industries.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/GlobalMap.jpg";
            }
            else if (sUserURL.Contains("/library.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/LadyOnPhone.png";
            }
            else if (sUserURL.Contains("/locations.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/GlobalMap.jpg";
            }
            else if (sUserURL.Contains("/managedprint.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ColorToner.jpg";
                // may 02  pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/OpenPrinterAtAngle.png";
            }
            else if (sUserURL.Contains("/managedservices.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/Network.png";
            }
            else if (sUserURL.Contains("/mpsbenefits.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/RatingCheck.jpg";
            }
            else if (sUserURL.Contains("/mission.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactOnPhone.png";
            }
            else if (sUserURL.Contains("/mpsdevicemonitoring.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/BlueContactWorker.png";
            }
            else if (sUserURL.Contains("/onsite.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGrayMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/Wires.png";
            }
            else if (sUserURL.Contains("/partnershipbenefits.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGreenForest;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/TwoAtTable.png";
            }
            else if (sUserURL.Contains("/partnerships.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/RatingCheck.jpg";
            }
            else if (sUserURL.Contains("/pressreleases.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronRed;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SpeakerGesture.jpg";
            }
            else if (sUserURL.Contains("/privacy.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/CollegeTesting.jpg";
            }
            else if (sUserURL.Contains("/resources.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronPurpleDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SmilingGuyAtLaptop.png";
                // may 02  pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/CollegeTesting.jpg";
            }
            else if (sUserURL.Contains("/scantronproducts.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronRed;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/BlackOmrScanner.jpg";
            }
            else if (sUserURL.Contains("/security.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SmilingGuyAtLaptop.png";
            }
            else if (sUserURL.Contains("/selfservice.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGreenForest;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/StripesTonerInstall.png";
            }
            else if (sUserURL.Contains("/supplies.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronYellowOrange;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/YellowBlueWorkersOnRight.png";
            }
            else if (sUserURL.Contains("/technologies.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronGreenBright;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/ContactCenter3.png";
                // pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/HigherEd.jpg";
            }
            else if (sUserURL.Contains("/technologypartnerships.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronPurpleDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SixPortraits.jpg";
            }
            else if (sUserURL.Contains("/terms.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/SpeakerGesture.jpg";
            }
            else if (sUserURL.Contains("/testimonials.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/RatingCheck.jpg";
            }
            else if (sUserURL.Contains("/whysts.aspx"))
            {
                pnChild_Banner_Region.BackColor = scantronTealDark;
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/RatingCheck.jpg";
            }
            else // if (sUserURL.Contains("/default.aspx"))  home page often has no link
            {
                pnChild_Banner_Region.BackColor = scantronBlueMedium;
                //pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/PrivateSchoolTesting.jpg";
                pnChild_Banner_ContentStyling.BackImageUrl = "~/media/scantron/images/banners/YellowBlueWorkersOnRight.png";
            }
            // -----------------------------------------------
        }
    }
    // ========================================================================
    /*
    protected void ControlMarketing()
    {
        // -------------------------------------
        // Control Marketing Visibility
        // -------------------------------------
        int iCurrentMinute = DateTime.Now.Minute;
        if (
               iCurrentMinute == 0
            || iCurrentMinute == 5
            || iCurrentMinute == 10
            || iCurrentMinute == 15
            || iCurrentMinute == 20
            || iCurrentMinute == 25
            || iCurrentMinute == 30
            || iCurrentMinute == 35
            || iCurrentMinute == 40
            || iCurrentMinute == 45
            || iCurrentMinute == 50
            || iCurrentMinute == 55
            )
        {
            For_Body_C.Visible = true;
        }
        else
        {
            For_Body_C.Visible = false;
        }
    }
    */
    // ========================================================================
    /*
    protected void MoveNotice()
    {
        // -------------------------------------
        // Control Marketing Visibility
        // -------------------------------------

        string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();
        if (
            sUserURL.Equals("http://www.scantronts.com/default.aspx")
            || sUserURL.Contains("default.aspx")
            )
        {
            lbMove.Visible = true;
        }
        else
        {
            lbMove.Visible = false;
        }
    }
    */
    // ------------------------------------------------------------------
    // ------------------------------------------------------------------
}
