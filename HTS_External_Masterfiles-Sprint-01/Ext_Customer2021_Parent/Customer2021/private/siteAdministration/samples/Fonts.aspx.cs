using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using System.Data.Odbc;
//using System.Data.SqlClient;
using System.Configuration;

public partial class private_siteAdministration_samples_TemplateWhitePanel : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sTemp = "";
    
    // -------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        //EmailHandler emailHandler = new EmailHandler();
        //emailHandler.EmailIndividual("ServiceCommand.com TEST 1", "Inside to Inside", "steve.carlson@scantron.com", "adv320@scantron.com");
        //emailHandler.EmailIndividual("ServiceCommand.com TEST 2", "Inside to Outside", "s_d_carlson@yahoo.com", "steve.carlson@scantron.com");
        //emailHandler.EmailIndividual("ServiceCommand.com TEST 3", "Outside To Inside", "steve.carlson@scantron.com", "s_d_carlson@yahoo.com");
        //emailHandler.EmailIndividual("ServiceCommand.com TEST 4", "Outside To Outside", "s_d_carlson@yahoo.com", "scantrontechnologysolutions@gmail.com");
        //emailHandler = null;
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
