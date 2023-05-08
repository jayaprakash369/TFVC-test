using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Timer
/// </summary>
public class Timer
{
    protected DateTime datBeg = new DateTime();
    protected DateTime datEnd = new DateTime();
    public TimeSpan ts = new TimeSpan();
    public int iDurationInMinutes = 0;
    public double dDurationInHoursMinutes = 0.0;
    public string sHrMnSc = "";
    public string sMnSc = "";

    // ========================================================================
    public Timer()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // ========================================================================
    public void Start()
    {
        datBeg = DateTime.Now;
    }
    // ========================================================================
    public void Stop()
    {
        datEnd = DateTime.Now;
        ts = datEnd - datBeg;
        iDurationInMinutes = (int)ts.TotalMinutes;
        string sTemp = Math.Floor(ts.TotalHours).ToString("0");
        int iTemp = 0;
        if (int.TryParse(sTemp, out iTemp) == false)
            iTemp = 0;
        dDurationInHoursMinutes = iTemp + (ts.Minutes * .01);
        sHrMnSc = sTemp + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + " (Hr:Mn:Sc)";
        sMnSc = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + " (Mn:Sc)";
    }
    // ========================================================================
    public string Show()
    {
        string sDuration = "";

        //sDuration = ts.TotalMinutes.ToString("0") + " minutes, " + ts.Seconds.ToString("0") + " seconds";
        sDuration = ts.TotalSeconds.ToString("0") + " sec, " + ts.Milliseconds.ToString("0") + " mill";

        return sDuration;
    }
    // ========================================================================
    // ========================================================================
}