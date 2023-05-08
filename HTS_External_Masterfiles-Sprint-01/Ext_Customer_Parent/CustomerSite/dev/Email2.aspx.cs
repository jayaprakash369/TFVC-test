using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class dev_Email2 : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    string sReturn = "";
    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    // =========================================================
    protected void btEmail_Click(object sender, EventArgs e)
    {
        string sTo = txTo.Text.Trim();
        string sSbj = txSbj.Text.Trim();
        string sMsg= txMsg.Text.Trim();

        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        try
        {
            MailAddress toAddress = new MailAddress(sTo);
            MailAddress fromAddress = new MailAddress("adv320@Scantronts.com");
            message.To.Add(toAddress);
            message.From = fromAddress;
            message.Subject = sSbj;
            message.Body = sMsg;
            //smtpClient.Host = "localhost";
            //smtpClient.Host = "10.41.30.5";
            //smtpClient.Host = "10.40.14.79";
            smtpClient.Host = "10.40.8.123";
            // Uncomment for SMTP servers that require authentication
            // smtpClient.Credentials = new System.Net.NetworkCredential("user", "password");
            smtpClient.Send(message);
            sReturn = "SUCCESS";
        }
        catch (Exception ex)
        {
            sReturn = ex.ToString();
        }
        finally
        {
        }

        lbMessage.Text = sReturn;
    }
    // =========================================================
    // =========================================================
}