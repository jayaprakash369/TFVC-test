using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Net;

public partial class private_sc_map_GetLatLon : System.Web.UI.Page
{
    SourceForDefaults sfd = new SourceForDefaults();
    string sDebug = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sAdr = Request.QueryString["address"].ToString();
            char[] cSplitter = { ',' };
            string[] saLatLon = new string[4];

            //string sAdr = "2427+Crestline Drive,+Bellingham+WA+98292";
            //string sURL = "http://maps.google.com/maps/geo?q=" + sAdr + "&output=csv&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ";
            // former version...
            string sURL = "http://maps.google.com/maps/geo?q=" + sAdr + "&output=csv&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ";
            //string sURL = "http://maps.google.com/maps/place/" + sAdr;
            //   sURL = "http://www.carlsonshome.com";

            // Open the requested URL
            WebRequest req = WebRequest.Create(sURL);

            // Get the stream from the returned web response
            StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());

            // Get the stream from the returned web response
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string strLine;
            // Read the stream a line at a time and place each one
            // into the stringbuilder
            while ((strLine = stream.ReadLine()) != null)
            {
                // Ignore blank lines
                if (strLine.Length > 0)
                    sb.Append(strLine);
            }
            // Finished with the stream so close it now
            stream.Close();

            // Cache the streamed site now so it can be used
            // without reconnecting later
            //string m_strSite = sb.ToString();
            string sSource = sb.ToString();
            saLatLon = sSource.Split(cSplitter);

            decimal dcLat = Decimal.Parse(saLatLon[2]);
            decimal dcLon = Decimal.Parse(saLatLon[3]);

            //        Response.Write("<br />Lat: " + dcLat.ToString());
            //        Response.Write("<br />Lon: " + dcLon.ToString());
            Response.Write(dcLat.ToString() + "|" + dcLon.ToString());
            // 
            //Response.Write(sSource);
        }
        catch (Exception ex)
        {
            //sfd.Save SaveError(ex.Message.ToString(), ex.ToString(), "");
            sDebug = ex.Message.ToString();
        }
        finally
        {
        }
        Response.End();

    }
}