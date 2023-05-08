using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommonCodeLibrary_DEV
/// </summary>
public class CommonCodeLibrary
{
    // ===================================================================
	public CommonCodeLibrary()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    // ==================================================
    public string GetWsKey()
    {
        string sWsKey = "";

        string sText =
        "S5|FT|tM|XZ|8A|" +
        "Wj|Y6|Nk|GB|iR|" +
        "Cl|S1|bY|QE|hV|" +
        "mU|2D|PE|x7|gR|" +
        "uE|s2|Oq|Kn|yf|" +
        "FC|oX|7N|eO|Gv|" +
        "Qp|d6|Mj|wW|H3|" +
        "xH|tB|Iw|Lq|9c|" +
        "Ky|IJ|vP|Vb|4r|" +
        "aT|ZA|Ls|0z|UJ|" +
        "Qp|d6|Mj|wW|H3|" +
        "Cl|S1|bY|QE|hV|" +
        "aT|ZA|Ls|0z|UJ|" +
        "FC|oX|7N|eO|Gv|" +
        "S5|FT|tM|XZ|8A|" +
        "uE|s2|Oq|Kn|yf|" +
        "xH|tB|Iw|Lq|9c|" +
        "mU|2D|PE|x7|gR|" +
        "Ky|IJ|vP|Vb|4r|" +
        "Wj|Y6|Nk|GB|iR";

        char[] cSplitter = { '|' };
        string[] saNum = new string[1];
        string[] saCod = new string[1];

        int iNum = 0;
        DateTime datTemp = new DateTime();
        datTemp = DateTime.Now;

        long lNum = Convert.ToInt64(datTemp.ToString("yyyyMMdd")) * Convert.ToInt64(datTemp.DayOfYear + 1234);
        string sNumIn = lNum.ToString();
        string sNumOut = "";
        int j = 0;
        for (int i = 0; i < sNumIn.Length; i++)
        {
            sNumOut += sNumIn.Substring(i, 1);
            if (j == 0)
                j = 1;
            else
            {
                if ((i > 0) && (i < (sNumIn.Length - 1)))
                    sNumOut += "|";
                j = 0;
            }

        }
        saNum = sNumOut.Split(cSplitter);
        saCod = sText.Split(cSplitter);

        for (int i = 0; i < saNum.Length; i++)
        {
            if (int.TryParse(saNum[i], out iNum) == false)
                iNum = 0;
            else
                sWsKey += saCod[iNum];
        }

        return sWsKey;
    }
    // ==================================================
    public string Clean(string text, int maxLength)
    {
        string sText = text;
        // HttpUtility.UrlEncode 
        // HttpUtility.HtmlEncode 
        // Dangerous Characters = "' ; + < > % ? = ( ) . /";
        if (sText != null && sText != "")
        {
            // 1) Cut to max length
            if (maxLength > 0)
            {
                if (sText.Length > maxLength)
                    sText = sText.Substring(0, maxLength);
            }

            // 2) Allow only keyboard characters
            sText = KeyboardCharactersOnly(sText, sText.Length);

            // 3) Look for code in fields
            if (sText.Length > 20) // only do sql check on large strings (parameterized queries should stop or 1=1
            {
                // 4) Delete suspected sql injection
                sText = sText.Replace("-- ", " - "); // sql comment
                sText = sText.Replace("--%20", " - "); // sql comment with URL encoded space
                sText = ClearSuspiciousText(sText, "../"); // directory traversal
                sText = ClearSuspiciousText(sText, "union select");
                sText = ClearSuspiciousText(sText, "union (select");
                sText = ClearSuspiciousText(sText, "select *");
                sText = ClearSuspiciousText(sText, "select from");
                sText = ClearSuspiciousText(sText, "delete *");
                sText = ClearSuspiciousText(sText, "delete from");
                sText = ClearSuspiciousText(sText, "delete where");
                sText = ClearSuspiciousText(sText, "delete (select");
                sText = ClearSuspiciousText(sText, "insert into");
                sText = ClearSuspiciousText(sText, "onclick=");
            }
            // 4) Encode remaining characters
            sText = HttpUtility.HtmlEncode(sText);

            // 5) permit these common characters for AS400 display (Web already displays codes properly)
            sText = sText.Replace("&#39;", "`");
            sText = sText.Replace("&quot;", "`");
            sText = sText.Replace("&amp;", "&");

            // 5) Cut to length again if encoding has lengthened the string
            if (maxLength > 0)
            {
                if (sText.Length > maxLength)
                    sText = sText.Substring(0, maxLength);
            }
        }
        return sText;
    }
    // ==================================================
    public string CleanXSS(string text)
    {
        string sText = text;
        if (sText != null && sText != "")
        {
            // put <> back so you can match XSS
            sText = sText.Replace("&lt;", "<");
            sText = sText.Replace("&gt;", ">");
            sText = sText.Replace("&LT;", "<");
            sText = sText.Replace("&GT;", ">");

            // Look for Cross Site Scripting code in fields
            if (sText.Length > 10) // only do sql check on large strings (parameterized queries should stop or 1=1
            {
                // remove entries bypassing client validation to do cross site scripting
                //sText = ClearSuspiciousText(sText, "c:");  // trying to access root (too many false matches)
                sText = ClearSuspiciousText(sText, "<script");
                sText = ClearSuspiciousText(sText, "<html");
                sText = ClearSuspiciousText(sText, "<body");
                sText = ClearSuspiciousText(sText, "<applet");
                sText = ClearSuspiciousText(sText, "<embed");
                sText = ClearSuspiciousText(sText, "<frame");
                sText = ClearSuspiciousText(sText, "<iframe");
                sText = ClearSuspiciousText(sText, "<img");
                sText = ClearSuspiciousText(sText, "<frameset");
                sText = ClearSuspiciousText(sText, "<style");
                sText = ClearSuspiciousText(sText, "<layer");
                sText = ClearSuspiciousText(sText, "<ilayer");
                sText = ClearSuspiciousText(sText, "<link");
                sText = ClearSuspiciousText(sText, "<meta");
                sText = ClearSuspiciousText(sText, "<object");
                sText = ClearSuspiciousText(sText, "<mailto:");
            }
        }
        return sText;
    }
    // ========================================================================
    public string KeyboardCharactersOnly(string stringIn, int maxLength)
    {
        string sRtn = "";
        string sKeyboardChararacters = "0123456789 abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";

        if ((stringIn != null) && (stringIn != ""))
        {
            sRtn = stringIn.ToString().Trim();

            // 1) Trim to max length
            int iLength = sRtn.Length;
            if (iLength > maxLength)
            {
                sRtn = sRtn.Substring(0, maxLength);
                iLength = maxLength;
            }
            // 2) Ensure each character is a keyboard character
            string sChar = "";
            int iPos = 0;
            for (int i = 0; i < iLength; i++)
            {
                sChar = sRtn.Substring(i, 1);
                try
                {
                    iPos = sKeyboardChararacters.IndexOf(sChar);
                    if ((iPos < 0) || ((iPos == 0) && (sChar != "0")))
                        sRtn = sRtn.Replace(sChar, " ");
                }
                catch (Exception ex)
                {
                    string sEx = ex.ToString();
                    sRtn = sRtn.Replace(sChar, " ");
                }
            }
        }

        return sRtn;
    }
    // ========================================================================
    public string ClearSuspiciousText(string originalText, string suspiciousText)
    {
        string sCleanedText = "";
        string sReplacementCharacters = "~";
        string sErrLogged = "N";
        string sTextLower = originalText.ToLower();
        suspiciousText = suspiciousText.ToLower();
        int iStartPos = sTextLower.IndexOf(suspiciousText);
        while (iStartPos >= 0)
        {
            if (sErrLogged == "N")
            {
                string sUserURL = HttpContext.Current.Request.Url.ToString();
                // SaveErrorText("Suspicious Text", originalText, sUserURL, "", "", "Cleaner");
                sErrLogged = "Y";
            }
            // Code to DELETE all suspicious characters
            //sTextLower =    sTextLower.Substring(0, iStartPos) + 
            //                sTextLower.Substring(iStartPos + suspiciousText.Length);
            //originalText =  originalText.Substring(0, iStartPos) + 
            //                originalText.Substring(iStartPos + suspiciousText.Length);
            // Code to DISABLE EXECUTION of suspicious characters (yet retain ability to be read)
            sTextLower = sTextLower.Substring(0, iStartPos) +
                            sTextLower.Substring(iStartPos, 1) + sReplacementCharacters +
                            sTextLower.Substring(iStartPos + 1);
            originalText = originalText.Substring(0, iStartPos) +
                            originalText.Substring(iStartPos, 1) + sReplacementCharacters +
                            originalText.Substring(iStartPos + 1);

            iStartPos = sTextLower.IndexOf(suspiciousText);
        }
        sCleanedText = originalText;
        return sCleanedText;
    }
    // ========================================================================
    // ========================================================================
}