using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dev_DoubleSubmit : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
//    CustomerSite_LIVE.CustomerSiteLIVEData wsLive = new CustomerSite_LIVE.CustomerSiteLIVEData();
//    CustomerSite_DEV.CustomerSiteTESTData wsTest = new CustomerSite_DEV.CustomerSiteTESTData();
    SourceForDefaults sfd = new SourceForDefaults();
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Button1.Attributes.Add("onclick", "this.value='Processing Request: Please Wait...';this.style.background='#FFFFCC';this.style.color='#406080';this.style.fontSize='14';");
            Session["processing"] = "NO";
            Button1.Text = "Submit Task";
        }
        else 
        {
            if (Session["processing"].ToString() == "YES")
                Button1.Text = "Task Already Submitted";
        }
    }
    // =========================================================
    protected void Button1_Click(object sender, EventArgs e)
    {
        try 
        {
            // Fails validation?
            if (TextBox1.Text == "READY")
            {
                if (Session["processing"].ToString() != "YES")
                {
                    Session["processing"] = "YES";
                    for (int i = 0; i < 19999999; i++)
                    {
                        string sTest = "Loop... " + i.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveError(ex.Message.ToString(), ex.ToString(), "");
        }
        finally 
        {
            Label1.Text = Session["processing"].ToString();
        }
    }
    // =========================================================
    // =========================================================
}