using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

public partial class dev_Captcha : System.Web.UI.Page
{
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Return_Number();
    }
    // =========================================================
    protected void Return_Number()
    {
        Random rNum1 = new Random();
        Random rNum2 = new Random();
        int iNumQ1 = 0;
        int iNumQ2 = 0;
        string sQString = "";

        iNumQ1 = rNum1.Next(10, 15);
        iNumQ2 = rNum1.Next(17, 31);

        sQString = iNumQ1.ToString() + " + " + iNumQ2.ToString() + " = ";
        Session["answer"] = (iNumQ1 + iNumQ2).ToString();

        Bitmap btmObj;
        Graphics gfxObj;
        int iWidth = 120;
        int iHeight = 35;

        btmObj = new Bitmap(iWidth, iHeight);
        gfxObj = Graphics.FromImage(btmObj);
        Font font = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Pixel);
        Rectangle rect = new Rectangle(0, 0, iWidth-1, iHeight-1); // Shrink for 1px border

        gfxObj.FillRectangle(Brushes.BlanchedAlmond, rect); //
        gfxObj.DrawRectangle(Pens.BurlyWood, rect); // Border
        gfxObj.DrawString(sQString, font, Brushes.DarkSlateGray, 4, 7); // from left, from top, Azure
        Response.ContentType = "Image/jpeg";
        btmObj.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        btmObj.Dispose();
        gfxObj.Dispose();
        Response.End();
    }
    //------------------------------------------------------
}



