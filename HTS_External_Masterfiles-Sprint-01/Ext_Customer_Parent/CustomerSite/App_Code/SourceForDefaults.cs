using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for SourceForDefaults
/// </summary>
public class SourceForDefaults
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    public int iDefaultTableWidth = 800;
    //(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$
    //Validates a strong password. It must be between 8 and 10 characters, contain at least one digit and one alphabetic character, and must not contain special characters.
    //^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$
    //Password must be at least 4 characters, no more than 8 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit.
    //public string sPasswordRequirementCode = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).[^\\s]{7,15}$";
    //public string sPasswordRequirementText = "Password Requirements: 7 to 15 non-blank characters including at least one uppercase character, one lowercase character, a number, and may not contain the username";
    public string sPasswordRequirementCode = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{7,15}$";  // had to double the \\ to get it to accept it.
    public string sPasswordRequirementText = "Password must be between 7 and 15 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit.";

    // ========================================================================
    public SourceForDefaults()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // ========================================================================
    public string GetTicketEncrypted(int ctr, int tck)
    {
        string sEncrypted = "";

        string sCtr = ctr.ToString();
        string sTck = tck.ToString();
        string sCtrTck = "";
        string[] saCode = new string[10];

        for (int i = 0; i < 7; i++)
        {
            if (sCtr.Length < 3)
                sCtr = "0" + sCtr;
            if (sTck.Length < 7)
                sTck = "0" + sTck;
        }

        sCtrTck = sCtr + sTck;
        saCode[0] = "aBcDeFgHiJ";
        saCode[1] = "kLmNoPqRsT";
        saCode[2] = "uVwXyZAbCd";
        saCode[3] = "EfGhIjKlMn";
        saCode[4] = "OpQrStUvWx";
        saCode[5] = "YzaBcDeFgH";
        saCode[6] = "iJkLmNoPqR";
        saCode[7] = "sTuVwXyZAb";
        saCode[8] = "CdEfGhIjKl";
        saCode[9] = "MnOpQrStUv";
        
        string sNums = "5820416937";
        int iTckNum = 0;
        string sReplacementChar = "";
        sEncrypted = sNums;

        for (int i = 0; i < 10; i++)
        {
            // for each pos 1-10 get number in ticket
            if (int.TryParse(sCtrTck.Substring(i, 1), out iTckNum) == false)
                iTckNum = 0;
            // replacement character = a) array of codes[loop num] b) character at replacement position
            sReplacementChar = saCode[i].Substring(iTckNum, 1);
            // use .Replace to move the replacement character to the new position in encrypted value            
            sEncrypted = sEncrypted.Replace(i.ToString(), sReplacementChar);
        }

        return sEncrypted;
    }
    // ========================================================================
    public int[] GetTicketDecrypted(string encrypted)
    {
        int[] iaCtrTck = new int[2];
        string[] saCode = new string[10];

        saCode[0] = "aBcDeFgHiJ";
        saCode[1] = "kLmNoPqRsT";
        saCode[2] = "uVwXyZAbCd";
        saCode[3] = "EfGhIjKlMn";
        saCode[4] = "OpQrStUvWx";
        saCode[5] = "YzaBcDeFgH";
        saCode[6] = "iJkLmNoPqR";
        saCode[7] = "sTuVwXyZAb";
        saCode[8] = "CdEfGhIjKl";
        saCode[9] = "MnOpQrStUv";

        string sNums = "5820416937";
        int iPosNum = 0;
        int iPosChar = 0;
        string sChar = "";
        string sOrigOrder = "";

        for (int i = 0; i < 10; i++)
        {
            if (int.TryParse(sNums.IndexOf(i.ToString()).ToString(), out iPosNum) == false)
                iPosNum = 0;
            sChar = encrypted.Substring(iPosNum, 1);
            iPosChar = saCode[i].IndexOf(sChar);
            sOrigOrder += iPosChar.ToString();
        }

        if (int.TryParse(sOrigOrder.Substring(0, 3), out iaCtrTck[0]) == false)
            iaCtrTck[0] = 0;
        if (int.TryParse(sOrigOrder.Substring(3, 7), out iaCtrTck[1]) == false)
            iaCtrTck[1] = 0;

        return iaCtrTck;
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
    public string ValidatePassword(string username, string password)
    {
        string sVerdict = "";
        string sError = "";
        int iUpper = 0;
        int iLower = 0;
        int iNumber = 0;
        int iUserInPwd = 0;
        int iPwdInUser = 0;
        int iPasswordLength = password.Length;
        string sUppercaseFound = "NO";
        string sLowercaseFound = "NO";
        string sNumberFound = "NO";
        string sChar = "";
        string sUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string sLower = "abcdefghijklmnopqrstuvwxyz";
        string sNumber = "0123456789";

        if (password == "")
        {
            sError = "A password is required.";
        }
        else
        {
            for (int i = 0; i < iPasswordLength; i++)
            {
                sChar = password.Substring(i, 1);
                iUpper = sUpper.IndexOf(sChar);
                iLower = sLower.IndexOf(sChar);
                iNumber = sNumber.IndexOf(sChar);

                if (iUpper > -1)
                    sUppercaseFound = "YES";
                if (iLower > -1)
                    sLowercaseFound = "YES";
                if (iNumber > -1)
                    sNumberFound = "YES";
            }
            if (iPasswordLength < 7)
                sError += "Password is too short (7 character minimum).<br />";
            else if (iPasswordLength > 15)
                sError += "Password is too long (15 character maximum).<br />";

            if (sUppercaseFound == "NO")
                sError += "No uppercase character found.<br />";
            if (sLowercaseFound == "NO")
                sError += "No lowercase character found.<br />";
            if (sNumberFound == "NO")
                sError += "No number was found.<br />";

            iUserInPwd = password.IndexOf(username);
            if (iUserInPwd > -1)
                sError += "The password may not contain the username.<br />";
            iPwdInUser = username.IndexOf(password);
            if (iPwdInUser > -1)
                sError += "The username may not contain the password.<br />";

        }
        if (sError == "")
            sVerdict = "VALID";
        else
        {
            sVerdict = "INVALID PASSWORD FORMAT<br />" + sError;
        }

        return sVerdict;
    }
    // =========================================================
    public string DataTableToExcel(System.Data.DataTable dt)
    {
        string sbTop = "";
        sbTop = "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ";
        sbTop += "xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">";
        sbTop += "<meta name=ProgId content=Excel.Sheet><meta name=Generator content=\"Microsoft Excel 9\"><!--[if gte mso 9]>";
        sbTop += "<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + dt.TableName + "</x:Name><x:WorksheetOptions>";
        sbTop += "<x:Selected/><x:ProtectContents>False</x:ProtectContents><x:ProtectObjects>False</x:ProtectObjects>";
        sbTop += "<x:ProtectScenarios>False</x:ProtectScenarios></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>";
        sbTop += "<x:ProtectStructure>False</x:ProtectStructure><x:ProtectWindows>False</x:ProtectWindows></x:ExcelWorkbook></xml>";
        sbTop += "<![endif]-->";
        //@page definition is used to store document layout settings for the entire document.
        //The line below will add a header & footer to the downloaded Excel sheet.
        sbTop += (@"<style>
                        @page
                        {
                        mso-header-data:'&R Date: &D Time: &T';
                        mso-footer-data:'&L Proprietary & Confidential &R Page &P of &N';
                        }
                        </style>"
                      );
        sbTop += ("</head><body><table>");
        string bottom = "</table></body></html>";
        string sb = "";
        //Build the body
        sb += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            sb += "<td>" + dt.Columns[i].ColumnName + "</td>";
        }
        sb += "</tr>";

        //Items
        for (int x = 0; x < dt.Rows.Count; x++)
        {
            sb += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb += "<td>" + dt.Rows[x][i] + "</td>";
            }
            sb += "</tr>";
        }

        string sSxml = sbTop + sb + bottom;

        return sSxml;
    }
    // ========================================================================
    public Label BuildTitleLabel(string sText)
    {
        Label lbTemp = new Label();
        lbTemp.SkinID = "labelTableTitle";
        lbTemp.Text = sText;

        return lbTemp;
    }
    // ========================================================================
    public string checkGoToMenu(string utilityType, int currentCs1)
    {
        string sGoToMenu = "";
        Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
        Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
        SiteHandler sh = new SiteHandler();

        string sCustType = "";

        string sClassLib = sh.getLibrary();

        if (sClassLib == "L")
        {
            sCustType = wsLive.GetCustType(GetWsKey(), currentCs1);
        }
        else
        {
            sCustType = wsTest.GetCustType(GetWsKey(), currentCs1);
        }
        if ((utilityType == "RegLrgDlrSsb") && ((sCustType != "REG") && (sCustType != "LRG") && (sCustType != "DLR") && (sCustType != "SSB")))
        {
            sGoToMenu = "GO";
        }
        if ((utilityType == "RegLrgDlr") && ((sCustType != "REG") && (sCustType != "LRG") && (sCustType != "DLR")))
        {
            sGoToMenu = "GO";
        }
        if ((utilityType == "SspSsb") && ((sCustType != "SSP") && (sCustType != "SSB")))
        {
            sGoToMenu = "GO";
        }
        if ((utilityType == "Ssb") && (sCustType != "SSB"))
        {
            sGoToMenu = "GO";
        }
        return sGoToMenu;
    }
    // ========================================================================
    // ========================================================================
}