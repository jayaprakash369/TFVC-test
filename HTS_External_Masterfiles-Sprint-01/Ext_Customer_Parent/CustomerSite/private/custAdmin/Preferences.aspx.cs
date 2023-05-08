using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class private_admCust_Preferences : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    //DataTable dataTable;
    string sCs1Changed = "";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1();

        if (!IsPostBack)
        {
            GetPreferences();
        }
    }
    // =========================================================
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        int iCs1 = GetPrimaryCs1();

        int iOpenOrClosed = 0;

        if (int.TryParse(rblRegOpenOrClosed.SelectedValue.ToString(), out iOpenOrClosed) == false)
            iOpenOrClosed = 0;

        if (sPageLib == "L")
        {
            wsLive.SetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), iCs1, iOpenOrClosed);
        }
        else
        {
            wsTest.SetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), iCs1, iOpenOrClosed);
        }
        GetPreferences();
    }
    // =========================================================
    protected void GetPreferences()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        int iCs1 = GetPrimaryCs1();

        int iOpenOrClosed = 0;

        if (sPageLib == "L")
        {
            iOpenOrClosed = wsLive.GetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), iCs1);
        }
        else
        {
            iOpenOrClosed = wsTest.GetPrefRegistrationOpenOrClosed(sfd.GetWsKey(), iCs1);
        }
        if (iOpenOrClosed >= 1)
            rblRegOpenOrClosed.SelectedValue = "1";
        else
            rblRegOpenOrClosed.SelectedValue = "0";

    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        CheckCs1Changed();

        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iCs1ToUse = iCs1User;
        int iCs1Change = 0;

        if (User.Identity.IsAuthenticated) 
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor")) 
            {
                if (tbCs1Change.Visible == false)
                    tbCs1Change.Visible = true;

                if (txCs1Change.Text != "")
                {
                    if (int.TryParse(txCs1Change.Text, out iCs1Change) == false)
                        iCs1Change = 0;
                    else
                    {
                        if (iCs1Change > 0)
                        {
                            Session["adminCs1"] = txCs1Change.Text;
                            iCs1ToUse = iCs1Change;
                        }
                    }
                }
                else
                {
                    if (Session["adminCs1"] != null)
                    {
                        if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Change) == false)
                            iCs1Change = 0;
                        else 
                        {
                            if (iCs1Change > 0)
                            {
                                txCs1Change.Text = iCs1Change.ToString();
                                iCs1ToUse = iCs1Change;
                            }
                        }
                    }
                    else
                    {
                        txCs1Change.Text = iCs1User.ToString();
                        Session["adminCs1"] = txCs1Change.Text;
                    }
                }
            }
        }

        return iCs1ToUse;
    }
    // =========================================================
    protected string CheckCs1Changed()
    {
        sCs1Changed = "NO";

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                if (Session["adminCs1"] != null)
                {
                    // Admin changed cust but did not click change button
                    int iCs1Session = 0;
                    int iCs1Textbox = 0;
                    if (int.TryParse(Session["adminCs1"].ToString(), out iCs1Session) == false)
                        iCs1Session = 0;
                    if (int.TryParse(txCs1Change.Text, out iCs1Textbox) == false)
                        iCs1Textbox = 0;

                    if (iCs1Session != iCs1Textbox)
                        sCs1Changed = "YES";
                }
            }
        }
        return sCs1Changed;
    }
    // ================================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        GetPreferences();
    }
    // =========================================================
}