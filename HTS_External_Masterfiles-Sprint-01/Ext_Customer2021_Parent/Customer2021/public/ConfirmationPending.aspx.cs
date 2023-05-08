using System;
using System.Data;
using System.Configuration;

public partial class public_Confirmation : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------

    string sTemp = "";
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            Confirmation_Information();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
    }
    // -------------------------------------------------
    protected void Confirmation_Information()
    {
        lbSummary.Text = "An email with a confirmation link has been sent to the email provided.";
        lbDetail.Text = "Please locate that email and click the confirmation link to activate your account. ";
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
