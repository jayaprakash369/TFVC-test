using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dev_Test1 : MyPage
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

        //long lTime = 0;
        //DateTime datTemp = new DateTime(2013, 09, 09);
        //long unixTimestamp = datTemp.Ticks - new DateTime(1970, 1, 1).Ticks;
        //unixTimestamp /= TimeSpan.TicksPerSecond;

        DateTime datTemp = new DateTime(2013, 09, 09);
        Response.Write("<br /><br />Time: " + getUnixTimestamp(datTemp));
    }
    // =========================================================
    protected long getUnixTimestamp(DateTime dat)
    {
        //DateTime datTemp = new DateTime(2013, 09, 09);
        long unixTimestamp = dat.Ticks - new DateTime(1970, 1, 1).Ticks;
        unixTimestamp /= TimeSpan.TicksPerSecond;
       
        return unixTimestamp;

    }
    // =========================================================
    // =========================================================
}