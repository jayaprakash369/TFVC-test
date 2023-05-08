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
    //string sMailServerIp = "10.40.8.123"; // Added Jan 26th, 2016
    //string sMailServerIp = "10.40.8.163"; // Added Jan 27th, 2016
    string sMailServerIp = "10.40.8.123"; // Added April 18th, 2016
    int iMaxTries = 3;

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
        int iRetries = 0;

        try
        {
            sBody = sBody.Replace(Environment.NewLine, "<br />");
            sBody = sBody.Replace("·\t", "&#8226;&nbsp;&nbsp;");
            sBody = sBody.Replace("o\t", "&#8226;&nbsp;&nbsp;");

            if (String.IsNullOrEmpty(emailFrom))
                emailFrom = "adv320@harlandts.com";

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
            string sDone = "";
            sResult = ex.Message.ToString();            
            
            while (sDone != "Y")
            {
                iRetries++;

                sResult = EmailIndividual_Retry(subject, sBody, emailTo, emailFrom, htmlOrText);
                if (sResult == "SUCCESS" || iRetries >= iMaxTries) 
                    sDone = "Y";
            }
        }
        finally
        {
            if (sResult != "SUCCESS" && iRetries > 1)
            {
                if (emailTo != "htslog@yahoo.com")
                {
                    ErrorHandler erh = new ErrorHandler();
                    string sDebug = "Bad Email from [" + emailFrom + "]  To: [" + emailTo + "]  sbj: [" + subject + "]";
                    erh.SaveErrorText("DMZ Web Service Main Email Failure", sResult, sDebug);
                    erh = null;
                }
            }
        }
        // -----------------------------------
        return sResult;
    }
    // ========================================================================
    protected string EmailIndividual_Retry(string subject, string body, string emailTo, string emailFrom, string htmlOrText)
    {
        string sResult = "";
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        try
        {

            MailAddress fromAddress = new MailAddress(emailFrom);
            message.From = fromAddress;
            message.Subject = subject;
            MailAddress toAddress = new MailAddress(emailTo);
            message.To.Add(toAddress);
            message.IsBodyHtml = true;
            message.Body = body;
            smtpClient.Host = sMailServerIp;
            smtpClient.Send(message);
            sResult = "SUCCESS";
        }
        catch (Exception ex)
        {
            sResult = ex.Message.ToString();
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
