using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_Locations : MyPage
{
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // SetBanner("GlobalMap.jpg");
    }
    // --------------------------------------------------------------------------
    protected void SetBanner(string filename)
    {
        /*
        ContentPlaceHolder cp = this.Master.Master.FindControl("ParentBannerArea") as ContentPlaceHolder;
        if (cp != null)
        {
            Panel pn = cp.FindControl("dvChildBanner") as Panel;
            if (pn != null)
                pn.BackImageUrl = "~/media/scantron/images/banners/" + filename;
        }
        */
    }
    // =========================================================
}