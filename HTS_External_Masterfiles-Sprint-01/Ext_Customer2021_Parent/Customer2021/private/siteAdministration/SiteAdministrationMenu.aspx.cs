using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

public partial class private_siteAdministration_SiteAdministrationMenu : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    //SqlDataReader sqlReader;
    
    // -------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        lbError.Text = "";
        pnError.Visible = false;
        pnAdminAccessCode.Visible = false;
        if (!IsPostBack) 
        {
            string[] saCodes = GetAdminAccessCodes().Split('|');
            if (saCodes.Length > 1) 
            {
                lbAdminAccessCode.Text = "<b>" + saCodes[0] + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <i>(" + DateTime.Now.AddMonths(1).ToString("MMMM") + ": " + saCodes[1] + ")</i>";
                pnAdminAccessCode.Visible = true;
            }
        }
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
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected int Delete_LockedRegistrationRecords()
    {
        int iRowsAffected = 0;
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        string sIp = GetIp();
        string sSql = "";

        try
        {
            sSql = "Delete from " + sSqlDbToUse_Customer + ".RegistrationAttempts" +
                " where raCount >= @Attempts";

            sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.Parameters.AddWithValue("@Attempts", 7);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void lkClearRegistrationLock_Click(object sender, EventArgs e)
    {

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn.Open();
            int iRowsAffected = Delete_LockedRegistrationRecords();
            if (iRowsAffected > 0)
            {
                lbError.Text = "Customer lock succesfully cleared";
            }
            else
            {
                lbError.Text = "No existing registration locks found to be cleared";
            }
            pnError.Visible = true;

        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            sqlConn.Close();
        }

    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================

}