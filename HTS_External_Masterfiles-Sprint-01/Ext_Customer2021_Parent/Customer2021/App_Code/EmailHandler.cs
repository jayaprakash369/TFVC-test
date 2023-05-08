using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;

/// <summary>
/// Summary description for EmailHandler
/// </summary>
public class EmailHandler
{
    //private static string sMailServerIp = "10.40.8.123"; // Added Jan 26th, 2016
    private static string sMailServerIp = "SMTP6.Scantron.com"; // Updating Nov 18th, 2020

    // ========================================================================
    // Constructor (if you decide to pass in parms)
    public EmailHandler()
    {
    }

    // ========================================================================
    // If you make it "static" you can directly call it rather than creating an object first...
    public string EmailIndividual(string subject, string body, string emailTo, string emailFrom)
    {
        string sResult = "";
        string sBody = body;
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

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
            message.IsBodyHtml = true;
            message.Body = sBody;
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
    // ===========================================================================================
    public string EmailGroup(
        string subject, 
        string body, 
        DataTable dtEmailTo, 
        string emailFrom)
    {
        string sResult = "";
        string sBody = body;
        string sEmailTo = "";

        MailAddress fromAddress;
        SmtpClient smtpClient = new SmtpClient();
        MailMessage mailMessage = new MailMessage();

        try
        {
            if (String.IsNullOrEmpty(emailFrom))
                emailFrom = "adv320@scantron.com";

            sBody = scrub(sBody);
            sBody = sBody.Replace(Environment.NewLine, "<br />");

            fromAddress = new MailAddress(emailFrom);
            mailMessage.From = fromAddress;

            foreach (DataRow row in dtEmailTo.Rows)
            {
                sEmailTo = row[0].ToString().Trim();
                if (!String.IsNullOrEmpty(sEmailTo))
                {
                    mailMessage.To.Add(sEmailTo);
                }
            }

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = sBody;
            smtpClient.Host = sMailServerIp;

            if (mailMessage.To.Count > 0)
                smtpClient.Send(mailMessage);

            sResult = "SUCCESS";

        }
        catch (Exception ex)
        {
            sResult = ex.Message.ToString();
            MyPage myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "Group Email Error");
            myPage = null;
        }
        finally
        {
        }
        return sResult;
    }
    // ========================================================================
    public string scrub(string txt)
    {
        // Remember to also update any class version of scrub (noteHandler)
        string sTxt = txt;
        // “Quote” and then a ‘single’ quote from word
        sTxt = sTxt.Replace("“", "\"");
        sTxt = sTxt.Replace("”", "\"");
        sTxt = sTxt.Replace("‘", "'");
        sTxt = sTxt.Replace("’", "'");
        sTxt = sTxt.Replace("•", "");
        sTxt = sTxt.Replace("·\t", "&#8226;&nbsp;&nbsp;");
        sTxt = sTxt.Replace("o\t", "&#8226;&nbsp;&nbsp;");

        //sTxt = sTxt.Replace("<", "&lt;"); // this is pointless because it will Asp.Net security will make it crash before reaching this validation
        //sTxt = sTxt.Replace(">", "&gt;");
        sTxt = sTxt.Trim();

        return sTxt;
    }
    // ========================================================================
    // ========================================================================

}