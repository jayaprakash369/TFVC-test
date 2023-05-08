using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;

/// <summary>
/// Email Tools
/// </summary>
public class EmailHandler
{
   // string sMailServerIp = "10.40.14.79"; // Eagan main
    //string sMailServerIp = "10.41.30.6"; // Eagan main
    string sMailServerIp = "10.40.8.123"; // Added Jan 26th, 2016

    // ========================================================================
    public EmailHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // ========================================================================
    public string EmailIndividual(string subject, string body, string emailTo, string emailFrom, string htmlOrText)
    {
        string sResult = "";
        string sBody = body;
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        try
        {
            MailAddress fromAddress = new MailAddress(emailFrom);
            message.From = fromAddress;
            message.Subject = subject;
            MailAddress toAddress = new MailAddress(emailTo);
            message.To.Add(toAddress);

            if (htmlOrText.ToUpper() == "HTML")
                message.IsBodyHtml = true;
            else
                message.IsBodyHtml = false;
            message.Body = sBody;
            smtpClient.Host = sMailServerIp;
            smtpClient.Send(message);
            sResult = "SUCCESS";
        }
        catch (Exception ex)
        {
            sResult = ex.Message.ToString();
            //ErrorHandler erh = new ErrorHandler();
            //string sDebug = "Bad Email from [" + emailFrom + "]  To: [" + emailTo + "]  sbj: [" + subject + "]";
            //erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sDebug);
            //erh = null;
        }
        finally
        {
        }
        // -----------------------------------
        return sResult;
    }
    // ========================================================================
    // ========================================================================
}
