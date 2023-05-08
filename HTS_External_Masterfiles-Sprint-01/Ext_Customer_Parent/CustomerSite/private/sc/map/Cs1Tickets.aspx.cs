using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class private_sc_map_Cs1Tickets : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------

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
            int iCs1 = 0;
            if (null != Session["mapCs1"])
            {
                if (int.TryParse(Session["mapCs1"].ToString(), out iCs1) == false)
                    iCs1 = 0;
            }
            hfMod.Value = "";
            if (null != Session["mapMod"])
            {
                hfMod.Value = Session["mapMod"].ToString();
            }
            hfCs1.Value = iCs1.ToString();
            lbCs1.Text = "Open Tickets for Customer " + hfCs1.Value;

            if (hfMod.Value != "")
            {
                lbCs1.Text += " (Model " + hfMod.Value + ")";
            }
        }
            //ddCustomer.Attributes.Add("onChange", "return clearOtherDropDown('ddCustomer')");
    }
    // =========================================================
    // =========================================================
}