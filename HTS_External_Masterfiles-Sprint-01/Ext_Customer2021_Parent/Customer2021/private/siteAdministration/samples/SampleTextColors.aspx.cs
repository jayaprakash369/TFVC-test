using System;
using System.Web;
using System.Web.Util;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

public partial class private_siteAdministration_samples_SampleTextColors : MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sText = "<h1 class=\"w3-text-steel-blue\">steel-blue H1</h1>";
        sText = HttpUtility.HtmlEncode(sText);
        lbHtml.Text = sText;
    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
}
