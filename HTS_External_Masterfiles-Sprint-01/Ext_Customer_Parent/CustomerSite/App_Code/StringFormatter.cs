using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StringFormatter
/// </summary>
public class StringFormatter
{
    // ========================================================================
	public StringFormatter()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    // ========================================================================
    public string formatPhone(string phone, string format)
    {
        string sPhone = "";
        sPhone = phone;

        if (sPhone != "")
        {
            if (sPhone.Length >= 10)
            {
                if (format == "/-")
                    sPhone = phone.Substring(0, 3) + "/" + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
                else if (format == "()-")
                    sPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
                else
                    sPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
            }
        }

        return sPhone;
    }

    // ========================================================================
    // ========================================================================
}