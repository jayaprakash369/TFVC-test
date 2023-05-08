using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;

public partial class public_ImageDisplay : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    //string sTemp = "";
    
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        int iRecordId = 0;

        if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "") 
        {
            if (int.TryParse(Request.QueryString["id"].ToString().Trim(), out iRecordId) == false)
                iRecordId = -1;
        }
        if (iRecordId > 0) 
        {
            lbPageTitle.Text = "Image: " + iRecordId;

            FileHandler fileHandler = new FileHandler(sLibrary);
            imRequested.ImageUrl = fileHandler.Get_ImageFile(iRecordId, "large");
            fileHandler = null;
        }
    }

    // ========================================================================
    #region mySqls
    // ========================================================================
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = true; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) {  }
    // ========================================================================
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================

    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

    // ========================================================================
    // ========================================================================

}
