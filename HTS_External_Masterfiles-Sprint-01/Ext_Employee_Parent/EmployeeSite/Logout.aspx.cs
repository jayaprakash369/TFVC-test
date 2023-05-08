using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;

public partial class Logout : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;

        /*
        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        // If from an internal address: login to both sites
        if (sIpAddress.StartsWith("10.") == true ||
            sIpAddress.StartsWith("127.") == true ||
            sIpAddress.StartsWith("172.16.") == true ||
            sIpAddress.StartsWith("172.17.") == true ||
            sIpAddress.StartsWith("172.18.") == true ||
            sIpAddress.StartsWith("172.19.") == true ||
            sIpAddress.StartsWith("172.20.") == true ||
            sIpAddress.StartsWith("172.21.") == true ||
            sIpAddress.StartsWith("172.22.") == true ||
            sIpAddress.StartsWith("172.23.") == true ||
            sIpAddress.StartsWith("172.24.") == true ||
            sIpAddress.StartsWith("172.25.") == true ||
            sIpAddress.StartsWith("172.26.") == true ||
            sIpAddress.StartsWith("172.27.") == true ||
            sIpAddress.StartsWith("172.28.") == true ||
            sIpAddress.StartsWith("172.29.") == true ||
            sIpAddress.StartsWith("172.30.") == true ||
            sIpAddress.StartsWith("172.31.") == true ||
            sIpAddress.StartsWith("192.168.") == true)
        {
            MembershipUser mu = Membership.GetUser();
            string sUserID = mu.UserName.ToString();


            string sPort = "";
            if (sDevTestLive == "LIVE")
                sPort = "80";
            else if (sDevTestLive == "TEST")
                sPort = "80";
            else
                sPort = "81";
            string sLoginLink = "http://10.41.30.9:" +
                sPort +
                "/public/tools/singleSignOut.aspx?uid=" + sUserID;

            FormsAuthentication.SignOut();

            Response.Redirect(sLoginLink);
        }
        else 
        {
            Response.Redirect("~/Default.aspx");
        }
        */
        FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx");

        Response.End();
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    // =========================================================
    // =========================================================
}