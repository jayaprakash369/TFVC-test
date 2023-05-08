using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_sc_Coverage : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txCity.Focus();
        }
    }
    // =========================================================
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        rpCoverage.Visible = false;

        string sValid = ServerSideVal_Coverage();

        if (sValid == "VALID") 
        {
            string sZip = txZip.Text;
            string sCity = txCity.Text;

            if (sPageLib == "L")
            {
                dataTable = wsLive.GetMaintenanceCoverage(sfd.GetWsKey(), sCity, sZip);  
            }
            else
            {
                dataTable = wsTest.GetMaintenanceCoverage(sfd.GetWsKey(), sCity, sZip);
            }

            if (dataTable.Rows.Count > 0)
            {
                rpCoverage.DataSource = dataTable;
                rpCoverage.DataBind();
                rpCoverage.Visible = true;
            }
            else 
            {
                vCus_Coverage.ErrorMessage = "No matching results were found.";
                vCus_Coverage.IsValid = false;
                txCity.Focus();
            }
        }
    }
    // =========================================================
    protected string ServerSideVal_Coverage()
    {
        string sValid = "";

        if ((txZip.Text != null) && (txZip.Text.Length > 5))
            txZip.Text = txZip.Text.Substring(0, 5);

        if ((txCity.Text != null) && (txCity.Text.Length > 30))
            txCity.Text = txCity.Text.Substring(0, 30);

        if (((txCity.Text == null) || (txCity.Text == "")) &&
            ((txZip.Text == null) || (txZip.Text == "")))
        {
            vCus_Coverage.ErrorMessage = "An entry is required";
            vCus_Coverage.IsValid = false;
            txCity.Focus();

        }

        if (Page.IsValid == true) 
        {
            sValid = "VALID";
        }

        return sValid;
    }
    // =========================================================
    // =========================================================
}