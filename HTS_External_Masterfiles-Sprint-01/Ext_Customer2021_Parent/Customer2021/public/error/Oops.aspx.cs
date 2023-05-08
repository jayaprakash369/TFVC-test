using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_error_oops : MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
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