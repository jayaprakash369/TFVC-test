using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DateFormatter
/// GREAT REFERENCE: http://www.dotnetperls.com/datetime-format
/// </summary>
public class DateFormatter
{
    public string[] sMonthAbbrev = { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    public string[] sMonthName = { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    // ========================================================================
    public DateFormatter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // ========================================================================

    // ========================================================================
    public string FormatDate(int date8, string dateFormat)
    {
        string sDate = "";
        DateTime datTemp = new DateTime();
        int iMon = 0;

        if (date8 > 0)
        {
            sDate = date8.ToString();
            datTemp = Convert.ToDateTime(sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2) + " 01:01:01.000");

            if (dateFormat == "Mon dd")
            {
                iMon = Int32.Parse(datTemp.Month.ToString());
                sDate = sMonthAbbrev[iMon] + " " + datTemp.Day;
            }
            else if (dateFormat == "Mon dd, YYYY")
            {
                iMon = Int32.Parse(datTemp.Month.ToString());
                sDate = sMonthAbbrev[iMon] + " " + datTemp.Day + ", " + datTemp.Year;
            }

        }
        return sDate;
    }
    // ========================================================================
    public string FormatTime(double dTime, string timeFormat)
    {
        string sTime = "";
        string sAmPm = "";
        double dTemp = 12.00;

        if (timeFormat == ":")
        {
            sTime = String.Format("{0:#.00}", dTime);
            sTime = sTime.Replace(".", ":");
        }
        else if ((timeFormat == ": pm") || (timeFormat == ": PM"))
        {
            sAmPm = "am";
            if (dTime < 1)
            {
                dTime = dTime + dTemp;
            }
            else if ((dTime >= 12) && (dTime < 13))
            {
                sAmPm = "pm";
            }
            else if (dTime >= 13)
            {
                dTime = dTime - dTemp;
                sAmPm = "pm";
            }
            if (timeFormat == ": PM")
                sAmPm = sAmPm.ToUpper();
            sTime = String.Format("{0:#.00}", dTime);
            sTime = sTime.Replace(".", ":") + " " + sAmPm;

        }

        return sTime;
    }
    // ========================================================================
    // ========================================================================
}