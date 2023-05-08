using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_LoadLocQty : MyPage
{
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        //EmailHandler emailHandler = new EmailHandler();
        //Timer timer = new Timer();
        //string sUserURL = HttpContext.Current.Request.Url.ToString().ToLower();
        string sResult = "";
        int iSeconds = 60;
        int iMinutes = 60;
        int iHours = 1;
        Session.Timeout = iSeconds * iMinutes * iHours;

        //emailHandler.EmailIndividual("Location Qty Duration: STARTED (Re-enabled)", "Running At : " + sUserURL + " (but this message is launched from customer 2021 site...)", "steve.carlson@scantron.com", "adv320@scantron.com");

        //timer.Start();

        sResult = Load_LocationQuantity();
        
        //timer.Stop();

        //emailHandler.EmailIndividual("Location Qty Duration: ENDED (Re-enabled)", "Result: " + sResult +  "<br />Duration: " + timer.sMnSc + " (mm:ss)", "steve.carlson@scantron.com", "adv320@scantron.com");

        //emailHandler = null;
        //timer = null;

        Response.End();

    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { this.RequireSSL = false; }
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Unload(object sender, EventArgs e) { }
    // -------------------------------------------------------------------------------------------------------
    protected string Load_LocationQuantity()
    {
        string sResult = "";

        try
        {
            sResult = ws_Upd_CombinedCustomerLocationQuantities();
        }
        catch (Exception ex)
        {
            sResult = ex.Message.ToString();
        }
        finally
        {
        }
        return sResult;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // ========================================================================
    #region myWebServiceCalls
    // ========================================================================
    // ========================================================================
    // WS: STRINGS (start)
    // ========================================================================
    protected string ws_Upd_CombinedCustomerLocationQuantities()
    {
        string sResponse = "";

        string sJobName = "Upd_CombinedCustomerLocationQuantities";
        string sFieldList = "x";
        string sValueList = "x"; 

        sResponse = Call_WebService_ForString(sJobName, sFieldList, sValueList);

        return sResponse;
    }
    // ========================================================================
    // WS: STRINGS (end)
    // ========================================================================
    // ========================================================================
    #endregion // end myWebServiceCalls
    // ========================================================================

    // -----------------------------------------------------------
    // -----------------------------------------------------------

}