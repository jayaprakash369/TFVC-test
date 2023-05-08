using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NumberFormatter
/// </summary>
public class NumberFormatter
{
    // ========================================================================
	public NumberFormatter()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    // ========================================================================
    public string numFormat1(double dIn, int decimalPlaces, string currency)
    {
        string sOut = "";
        string sIn = "";
        string sNumFormat = "";
        int iIn = 0;
        int i = 0;
        string sNegative = "";
        /*
         * currency -> "$"  = $12
         * decimalPlaces = 2 = 123.00
         */

        // Handle negatives
        if (dIn < 0)
        {
            dIn = (dIn * -1);
            sNegative = "-";
        }

        iIn = Convert.ToInt32(dIn);
        sIn = iIn.ToString().Trim();
        for (i = 0; i < sIn.Length; i++)
        {
            if ((i == 3) || (i == 6) || (i == 9) || (i == 12))
                sNumFormat = "," + sNumFormat;
            sNumFormat = "0" + sNumFormat;
        }
        for (i = 0; i < decimalPlaces; i++)
        {
            if (i == 0)
                sNumFormat += ".";
            sNumFormat += "0";
        }

        sNumFormat = "{0:" + sNumFormat + "}";
        sOut = string.Format(sNumFormat, dIn);

        sOut = currency + sOut;
        sOut = sNegative + sOut;

        return sOut;

    }
    // ========================================================================
    public string phoneFormat1(string phone)
    {
        string[] saPhone = { "", "", "" };
        string sPhone = "";

        if (phone != "")
        {
            saPhone = phoneToThreeParts(phone);
            sPhone = "(" + saPhone[0] + ") " + saPhone[1] + "-" + saPhone[2];
        }

        return sPhone;
    }

    // ========================================================================
    public string phoneFormat2(string phone)
    {
        string[] saPhone = new string[3];
        string sPhone = "";

        if (sPhone != "")
        {
            saPhone = phoneToThreeParts(phone);
            sPhone = saPhone[0] + "/" + saPhone[1] + "-" + saPhone[2];
        }

        return sPhone;
    }
    // ========================================================================
    public string[] phoneToThreeParts(string phone)
    {
        string[] saPhone = new string[3];

        if (phone != "")
        {
            if (phone.Length >= 10)
            {
                saPhone[0] = phone.Substring(0, 3);
                saPhone[1] = phone.Substring(3, 3);
                saPhone[2] = phone.Substring(6, 4);
            }
        }


        return saPhone;
    }
    // ========================================================================
    // ========================================================================
}