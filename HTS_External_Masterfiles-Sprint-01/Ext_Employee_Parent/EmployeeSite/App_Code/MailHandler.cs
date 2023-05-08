using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;

/// <summary>
/// Email Tools
/// </summary>
public class MailHandler
{
    //string sMailServerIp = "10.41.30.5"; // Omaha Backup
   // string sMailServerIp = "10.40.14.79"; // Eagan main
    // string sMailServerIp = "10.41.30.6"; // Eagan main
    string sMailServerIp = "10.40.8.123"; // Added Jan 26th, 2016

    // ========================================================================
    public MailHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // ========================================================================
    public string EmailIndividual(string subject, string body, string emailTo, string htmlOrText)
    {
        string sResult = "";
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        string sBody = body;

        if (!String.IsNullOrEmpty(emailTo))
        {
            try
            {
                //sBody = sBody.Replace(Environment.NewLine, "<br />");
                //sBody = sBody.Replace("·\t", "&#8226;&nbsp;&nbsp;");
                //sBody = sBody.Replace("o\t", "&#8226;&nbsp;&nbsp;");

                MailAddress fromAddress = new MailAddress("adv320@scantronts.com", "www.oursts.com");

                message.From = fromAddress;
                message.Subject = subject;
                MailAddress toAddress = new MailAddress(emailTo);
                message.To.Add(toAddress);

                if (htmlOrText.ToUpper() == "HTML")
                    message.IsBodyHtml = true;
                else
                    message.IsBodyHtml = false;
                message.Body = sBody;
                //smtpClient.Host = "localhost";
                smtpClient.Host = sMailServerIp;
                smtpClient.Send(message);
                sResult = "SUCCESS";
            }
            catch (Exception ex)
            {
                sResult = ex.Message.ToString();
                ErrorHandler eh = new ErrorHandler();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Next messages will be Email To, then Body");
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), emailTo);
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sBody);
                eh = null;
            }
            finally
            {
            }
        }
        // -----------------------------------
        return sResult;
    }
    // ========================================================================
    public string EmailIndividual2(string subject, string body, string emailTo, string emailFrom, string htmlOrText)
    {
        string sResult = "";
        string sBody = body;
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        if (!String.IsNullOrEmpty(emailTo) && !String.IsNullOrEmpty(emailFrom))
        {
            try
            {
                sBody = sBody.Replace(Environment.NewLine, "<br />");
                sBody = sBody.Replace("·\t", "&#8226;&nbsp;&nbsp;");
                sBody = sBody.Replace("o\t", "&#8226;&nbsp;&nbsp;");

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
                //smtpClient.Host = "localhost";
                smtpClient.Host = sMailServerIp;
                smtpClient.Send(message);
                sResult = "SUCCESS";
            }
            catch (Exception ex)
            {
                sResult = ex.Message.ToString();
                ErrorHandler eh = new ErrorHandler();
                // Bogus email addresses are being sent...
                string sDebug = "Bad Email from [" + emailFrom + "]  To: [" + emailTo + "]  sbj: [" + subject + "]";
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), sDebug);
                eh = null;
            }
            finally
            {
            }
        }
        // -----------------------------------
        return sResult;
    }
    // =========================================================
    public string EmailGroup(string subject, string body, DataTable dtEmailTo, string emailFrom, string htmlOrText)
    {
        string sResult = "";
        string sBody = body;
        string sEmailTo = "";

        MailAddress toAddress;
        MailAddress fromAddress;

        if (!String.IsNullOrEmpty(emailFrom))
        {
            try
            {
                sBody = sBody.Replace(Environment.NewLine, "<br />");
                sBody = sBody.Replace("·\t", "&#8226;&nbsp;&nbsp;");
                sBody = sBody.Replace("o\t", "&#8226;&nbsp;&nbsp;");

                SmtpClient smtpClient = new SmtpClient();
                MailMessage mailMessage = new MailMessage();

                fromAddress = new MailAddress(emailFrom);
                mailMessage.From = fromAddress;

                int iRowIdx = 0;
                foreach (DataRow row in dtEmailTo.Rows)
                {
                    sEmailTo = dtEmailTo.Rows[iRowIdx][0].ToString().Trim();
                    if (!String.IsNullOrEmpty(sEmailTo)) 
                    {
                        toAddress = new MailAddress(sEmailTo);
                        mailMessage.To.Add(toAddress);
                    }
                    iRowIdx++;
                }

                mailMessage.Subject = subject;
                if (htmlOrText == "HTML")
                    mailMessage.IsBodyHtml = true;
                else
                    mailMessage.IsBodyHtml = false;
                mailMessage.Body = sBody;
                smtpClient.Host = sMailServerIp;
                if (mailMessage.To.Count > 0)
                    smtpClient.Send(mailMessage);
                sResult = "SUCCESS";
            }
            catch (Exception ex)
            {
                ErrorHandler eh = new ErrorHandler();
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Failing Email: " + sEmailTo);
                sResult = "FAILURE: " + ex.Message.ToString();
                eh = null;
            }
            finally
            {

            }
        }
        return sResult;
    }

    // ========================================================================
    // ========================================================================
}
