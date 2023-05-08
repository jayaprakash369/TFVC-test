using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;

/// <summary>
/// Email Tools
/// </summary>
public class DownloadHandler
{
    // ========================================================================
    public DownloadHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // =========================================================
    public string DataTableToExcelCsv(System.Data.DataTable dt)
    {
        string sCsv = "";
        string sTemp = "";

        int iRowIdx = 0; // row index
        int iColIdx = 0; // column index
        int iCelIdx = 0; // table cell index
        string sHeadingsPrinted = "";

        //Build the body

        foreach (DataRow row in dt.Rows)
        {
            for (iColIdx = 0; iColIdx < dt.Columns.Count; iColIdx++)
            {
                if (sHeadingsPrinted != "Y") // only do this one time...
                {
                    // Load column header titles as the first row.
                    for (iCelIdx = 0; iCelIdx < dt.Columns.Count; iCelIdx++)
                    {
                        sTemp = dt.Columns[iCelIdx].ColumnName.Trim();
                        if (sTemp.Contains("\""))
                        {
                            sTemp = sTemp.Replace("\"", "\"\"");
                            sTemp = "\"" + sTemp + "\""; // put quotes around outside
                        }
                        // if commas exists, put quotes around outside of text
                        if (sTemp.Contains(","))
                            sTemp = "\"" + sTemp + "\"";
                        // if no quotes exists at this point, put quotes anyway to make it text
                        if (!sTemp.Contains("\""))
                            sTemp = "\"" + sTemp + "\""; // put quotes around outside

                        // Add your comma delimeters
                        if (iCelIdx > 0)
                            sCsv += ",";
                        sCsv += sTemp;
                        if (iCelIdx == dt.Columns.Count - 1)
                            sCsv += "\r\n";
                    }
                    sHeadingsPrinted = "Y";
                }
                // Loop and load each row (This is what will take so much time...)
                sTemp = dt.Rows[iRowIdx][iColIdx].ToString().Trim();
                // Escape double quotes
                if (sTemp.Contains("\""))
                {
                    //sTemp = sTemp.Replace("\"", "\"\""); // escape quotes
                    //sTemp = "\"" + sTemp + "\""; // put quotes around outside
                    sTemp = "\"" + sTemp.Replace("\"", "\"\"") + "\"";
                }
                else
                    sTemp = "\"" + sTemp + "\""; // put quotes around outside
                /*
                // if commas exists, put text into quotes
                if (sTemp.Contains(","))
                    sTemp = "\"" + sTemp + "\""; // put quotes around outside
                
                // if no quotes exists at this point, put quotes anyway to make it text
                if (!sTemp.Contains("\"")) 
                    sTemp = "\"" + sTemp + "\""; // put quotes around outside
                */

                // Add your comma delimeters
                if (iColIdx > 0)
                    sCsv += ",";
                sCsv += sTemp;
                if (iColIdx == dt.Columns.Count - 1)
                    sCsv += "\r\n";
            }
            iRowIdx++;
        }

        return sCsv;
    }
    // ========================================================================
    // ========================================================================
}