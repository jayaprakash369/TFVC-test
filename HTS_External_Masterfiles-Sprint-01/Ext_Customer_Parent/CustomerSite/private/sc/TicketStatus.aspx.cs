using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class private_sc_TicketStatus : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForTicket sft = new SourceForTicket();
    SourceForDefaults sfd = new SourceForDefaults();
    string sCs1Changed = "";

    //    System.Drawing.Color myYellow = System.Drawing.ColorTranslator.FromHtml("#FFF8DC");

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;

    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        int iCs1ToUse = GetPrimaryCs1(); // Initialize admin fields
        lbError.Visible = false;

        if (!IsPostBack)
        {
            pnInput.Visible = true;
        }

        /* 
        if (User.Identity.IsAuthenticated)
        {
            sUserType = Profile.LoginType;
 
            string sName = User.Identity.Name.ToString();

            if (sName == "Canada")
            {
                Profile.LoginCs1 = "2462";
                Profile.LoginType = "LRG";
            }
            else if (sName == "NorthDakota")
            {
                Profile.LoginCs1 = "77323";
                Profile.LoginType = "REG";
            }
            else if (sName == "SouthDakota")
            {
                Profile.LoginCs1 = "77323";
                Profile.LoginType = "REG";
            }
            else if ((sName == "Sales.3") || (sName == "Admin.1") || (sName == "Admin.3") || (sName == "Steve.1") || (sName == "SteveCarlson"))
            {
                Profile.LoginCs1 = "99999";
                Profile.LoginType = "REG";
            }
        }
        */

        txCtr.Focus();

    }
    // =========================================================
    protected void btLocSearch_Click(object sender, EventArgs e)
    {
        string sValid = "";
        lbError.Text = "";

        // Check for hackers bypassing client validation
        if (Page.IsValid)
        {
            sValid = ServerSideVal_TicketSearch();

            // If all server side validation is also passed...
            if (sValid == "VALID")
            {
                loadTicket();
            }
        }

    }
    // =========================================================
    protected void lbTicket_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnInput.Visible = false;
            pnTickets.Visible = false;
            pnDisplay.Visible = true;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTicketDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected void lbStatus_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnInput.Visible = false;
            pnTickets.Visible = false;
            pnDisplay.Visible = true;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetTimestampDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected void lbPartUse_Click(object sender, EventArgs e)
    {
        // Get Keys passed from link button
        LinkButton linkControl = (LinkButton)sender;
        string sParms = linkControl.CommandArgument.ToString();
        string[] saArg = new string[2];
        char[] cSplitter = { '|' };
        saArg = sParms.Split(cSplitter);
        int iCtr = 0;
        int iTck = 0;
        if (int.TryParse(saArg[0], out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(saArg[1], out iTck) == false)
            iTck = 0;

        // If you have data, 
        if ((iCtr > 0) && (iTck > 0))
        {
            pnInput.Visible = false;
            pnTickets.Visible = false;
            pnDisplay.Visible = true;
            pnDisplay.Controls.Clear();
            Panel pnTemp = new Panel(); // use temp panel to hold each table to load, then drop in on pnDisplay

            pnTemp = sft.GetPartUseDisplayPanel(iCtr, iTck, iCs1User);
            pnDisplay.Controls.Add(pnTemp);

        }
    }
    // =========================================================
    protected string ServerSideVal_TicketSearch()
    {
        string sResult = "";
        lbError.Text = "";

        try
        {
            int iNum = 0;
            if (txCtr.Text != "")
            {
                if (int.TryParse(txCtr.Text, out iNum) == false)
                {
                    if (lbError.Text == "")
                    {
                        lbError.Text = "The center must be a number";
                        txCtr.Focus();
                    }
                }
                else
                {
                    if (iNum > 999)
                    {
                        if (lbError.Text == "")
                        {
                            lbError.Text = "Center entry must be 3 digits or less";
                            txCtr.Text = txCtr.Text.Substring(0, 3);
                            txCtr.Focus();
                        }
                    }
                }
            }

            if (txTck.Text != "")
            {
                if (int.TryParse(txTck.Text, out iNum) == false)
                {
                    if (lbError.Text == "")
                    {
                        lbError.Text = "The ticket must be a number";
                        txTck.Focus();
                    }
                }
                else
                {
                    if (iNum > 9999999)
                    {
                        if (lbError.Text == "")
                        {
                            lbError.Text = "Ticket entry must be 7 digits or less";
                            txTck.Text = txTck.Text.Substring(0, 7);
                            txTck.Focus();
                        }
                    }
                }
            }
            if (lbError.Text == "")
            {
                if (txXrf.Text != "")
                {
                    if (txXrf.Text.Length > 24)
                    {
                        lbError.Text = "Cross reference must be 24 characters or less";
                        txXrf.Text = txXrf.Text.Substring(0, 24);
                        txXrf.Focus();
                    }
                }
            }
            // -------------------
            if (lbError.Text == "")
            {
                sResult = "VALID";
            }
            // -------------------
        }
        catch (Exception ex)
        {
            sResult = ex.ToString();
            SaveError(ex.Message.ToString(), ex.ToString(), "");
            lbError.Text = "A unexpected system error has occurred";
        }
        finally
        {
            if (lbError.Text != "")
                lbError.Visible = true;
        }
        // --------------------------------
        return sResult;
    }
    // =========================================================
    protected void loadTicket()
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        Table tbTemp = new Table();

        pnTickets.Visible = false;

        int iCs1ToUse = GetPrimaryCs1();

        int iCtr = 0;
        int iTck = 0;
        string sXref = txXrf.Text.ToUpper();
        if (int.TryParse(txCtr.Text, out iCtr) == false)
            iCtr = 0;
        if (int.TryParse(txTck.Text, out iTck) == false)
            iTck = 0;
        if (iCtr > 0 && iTck > 0)
            txXrf.Text = "";
        if ((iCtr > 0 && iTck > 0) || ((sXref != null) && (sXref != "")))
        {
            lbError.Text = "";

            if (sPageLib == "L") 
               dataTable = wsLive.GetTicketSummary(sfd.GetWsKey(), iCs1ToUse, iCtr, iTck, sXref);            
            else
               dataTable = wsTest.GetTicketSummary(sfd.GetWsKey(), iCs1ToUse, iCtr, iTck, sXref);

            if (dataTable.Rows.Count > 0)
            {
                rpTickets.DataSource = dataTable;
                rpTickets.DataBind();
                pnTickets.Visible = true;
            }
            else
                lbError.Text = "No matching ticket was found...";
        }
        else
        {
            lbError.Text = "Please enter search values";
        }
        if (lbError.Text != "")
            lbError.Visible = true;
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
                    {
                        sCs1Changed = "YES";
                        pnInput.Visible = true;
                        pnTickets.Visible = false;
                        pnDisplay.Visible = false;
                        txCtr.Text = "";
                        txTck.Text = "";
                        txXrf.Text = "";
                    }
                }
            }
        }
        return sCs1Changed;
    }
    // =========================================================
    protected int GetPrimaryCs1()
    {
        CheckCs1Changed();

        if (int.TryParse(Profile.LoginCs1.ToString(), out iCs1User) == false)
            iCs1User = 0;

        int iPrimaryCs1 = iCs1User;
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
                            iPrimaryCs1 = iCs1Change;
                            if (sCs1Changed == "YES")
                            {
                                string sGoToMenu = sfd.checkGoToMenu("RegLrgDlrSsb", iPrimaryCs1);
                                if (sGoToMenu == "GO")
                                    Response.Redirect("~/private/shared/Menu.aspx", false);
                            }
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
                                iPrimaryCs1 = iCs1Change;
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

        return iPrimaryCs1;
    }

    // =========================================================
    protected void btCs1Change_Click(object sender, EventArgs e)
    {
        int iPrimaryCs1 = GetPrimaryCs1();
        pnInput.Visible = true;
        pnTickets.Visible = false;

    }
    // =========================================================
    // =========================================================
}
