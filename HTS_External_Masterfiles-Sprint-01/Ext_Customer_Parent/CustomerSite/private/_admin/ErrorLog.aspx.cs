using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private__admin_ErrorLog : MyPage
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
        this.RequireSSL = false;
    }
    // ================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadErrorLog();
        }
    }
    // ================================================================
    protected void LoadErrorLog()
    {
        lbMessage.Text = "";
        DataTable dataTable = new DataTable();

        try
        {
            if (sPageLib == "L")
            {
                dataTable = wsLive.GetErrorLog(sfd.GetWsKey());
            }
            else
            {
                dataTable = wsTest.GetErrorLog(sfd.GetWsKey());
            }
            if (dataTable.Rows.Count > 0)
            {
                dataTable = ChopErrLines(dataTable);
                rpErrorLog.DataSource = dataTable;
                rpErrorLog.DataBind();
            }
            else
            {
                lbMessage.Text = "No errors were found...";

            }
        }
        catch (Exception ex)
        {
            string sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            if (lbMessage.Text != "")
                lbMessage.Visible = true;
            else
                lbMessage.Visible = false;
        }
        // ----------------------------------
    }
    // ================================================================
    protected DataTable ChopErrLines(DataTable dTable)
    {
        string sTemp = "";
        int iMaxStringSize = 100;
        int iLastSpace = 0;
        int i = 0;
        int iRowIdx = 0;

        try
        {
            foreach (DataRow row in dTable.Rows)
            {
                // sSql = "Select WEERR, WEDSC, WEVAL, WEWEB, WEIPA, COUNT(WEKEY) as Count" +
                sTemp = dTable.Rows[iRowIdx]["WEDSC"].ToString();
                iLastSpace = 0;
                for (i = 0; i < sTemp.Length; i++)
                {
                    if (sTemp.Substring(i, 1) == " ")
                        iLastSpace = 0;
                    if (iLastSpace >= iMaxStringSize)
                    {
                        // Skip Hex conversion blocks
                        if (i + 5 < sTemp.Length)
                        {
                            if (sTemp.Substring(i, 5) != "&#39;" && sTemp.Substring(i - 1, 5) != "&#39;" && sTemp.Substring(i - 2, 5) != "#39;" && sTemp.Substring(i - 3, 5) != "#39;" && sTemp.Substring(i - 4, 5) != "#39;" && sTemp.Substring(i - 5, 5) != "#39;")
                            {
                                sTemp = sTemp.Substring(0, i) + " " + sTemp.Substring(i);
                                iLastSpace = 0;
                            }
                        }
                    }
                    iLastSpace++;
                }

                dTable.Rows[iRowIdx]["WEDSC"] = sTemp;
                iRowIdx++;
            }
            dTable.AcceptChanges();
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }

        return dTable;
    }
    // ================================================================
    // ================================================================
}