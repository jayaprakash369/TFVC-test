using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class private__admin_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                //FileUpload1.SaveAs(MapPath(fileName));
                FileUpload1.SaveAs("D:\\OurSites\\Uploads\\" + fileName);
                Label1.Text = "File " + fileName + " uploaded.";
            }
            else
            {
                Label1.Text = "File upload failed";
            }
        }
        catch (Exception ex)
        {
            String sDebug = ex.Message.ToString();
        }
        finally
        {
        }

    }
}