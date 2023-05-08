using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for SourceForCustomer
/// </summary>
public class SourceForCustomer
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sClassLib = "";
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();
    System.Drawing.Color greenDark = System.Drawing.ColorTranslator.FromHtml("#3A7728");
    System.Drawing.Color greenLight = System.Drawing.ColorTranslator.FromHtml("#5BBF21");
    System.Drawing.Color blueDark = System.Drawing.ColorTranslator.FromHtml("#406080");  // #003893
    // ========================================================================
	public SourceForCustomer()
	{
		//
		// TODO: Add constructor logic here
		//
        
        SiteHandler sh = new SiteHandler();
        sClassLib = sh.getLibrary();

	}
    // ========================================================================
    public Table GetCustHeaderTable(int cs1, int cs2)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        Label lbTemp = new Label();
        Table tbTemp = new Table();
        tbTemp.CssClass = "tableWithoutLines";

        NumberFormatter nf = new NumberFormatter();

        if (sClassLib == "L")
        {
            dataTable = wsLive.GetCustBasics(sfd.GetWsKey(), cs1, cs2);
        }
        else
            dataTable = wsTest.GetCustBasics(sfd.GetWsKey(), cs1, cs2);

        if (dataTable.Rows.Count > 0)
        {
            int iCs1 = 0;
            int iCs2 = 0;
            if (int.TryParse(dataTable.Rows[0]["Cs1"].ToString().Trim(), out iCs1) == false)
                iCs1 = 0;
            if (int.TryParse(dataTable.Rows[0]["Cs2"].ToString().Trim(), out iCs2) == false)
                iCs2 = 0;
            string sName = dataTable.Rows[0]["Name"].ToString().Trim();
            string sAddress1 = dataTable.Rows[0]["Address1"].ToString().Trim();
            string sAddress2 = dataTable.Rows[0]["Address2"].ToString().Trim();
            string sAddress3 = dataTable.Rows[0]["Address3"].ToString().Trim();
            string sCity = dataTable.Rows[0]["City"].ToString().Trim();
            string sState = dataTable.Rows[0]["State"].ToString().Trim();
            string sZip = dataTable.Rows[0]["Zip"].ToString().Trim();
            string sContact = dataTable.Rows[0]["Contact"].ToString().Trim();
            string sPhone = dataTable.Rows[0]["Phone"].ToString().Trim();
            string sCrossRef = dataTable.Rows[0]["CrossRef"].ToString().Trim();

            TableRow tRow;
            TableCell tCell;

            // Row 1
            tRow = new TableHeaderRow();
            tbTemp.Rows.Add(tRow);

            tCell = new TableHeaderCell();
            lbTemp = new Label();
            lbTemp.Text = sName;
            lbTemp.SkinID = "labelTitleColor2_Medium";
            tCell.Controls.Add(lbTemp);
            tRow.Cells.Add(tCell);

            // Row 2
            tRow = new TableHeaderRow();
            tbTemp.Rows.Add(tRow);

            tCell = new TableHeaderCell();
            lbTemp = new Label();
            lbTemp.Text = sAddress1 + " " + 
                sAddress2 + " " + 
                sAddress3 + " " + 
                sCity + ", " + 
                sState + 
                "&nbsp;&nbsp;" + sZip +
                "&nbsp;&nbsp;&nbsp;&nbsp;<b>Customer:</b>&nbsp;" + iCs1.ToString() + "-" + iCs2.ToString() +
                "&nbsp;&nbsp;&nbsp;&nbsp;<b>Contact:</b>&nbsp;" + sContact +
                "&nbsp;&nbsp;&nbsp;" + nf.phoneFormat1(sPhone);
            lbTemp.SkinID = "labelSubHeadTiny";
            tCell.VerticalAlign = VerticalAlign.Top;
            tCell.Height = 25;
            tCell.Controls.Add(lbTemp);
            tRow.Cells.Add(tCell);

        }
        return tbTemp;
    }

    // ========================================================================
    public Table GetCustDataTable(int cs1, int cs2, string contact, string phone, string ext)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        Label lbTemp = new Label();
        Table tbTemp = new Table();
        tbTemp.CssClass = "tableWithoutLines";

        NumberFormatter nf = new NumberFormatter();

        if (sClassLib == "L")
        {
            dataTable = wsLive.GetCustBasics(sfd.GetWsKey(), cs1, cs2);
        }
        else
            dataTable = wsTest.GetCustBasics(sfd.GetWsKey(), cs1, cs2);

        if (dataTable.Rows.Count > 0)
        {
            int iCs1 = 0;
            int iCs2 = 0;
            if (int.TryParse(dataTable.Rows[0]["Cs1"].ToString().Trim(), out iCs1) == false)
                iCs1 = 0;
            if (int.TryParse(dataTable.Rows[0]["Cs2"].ToString().Trim(), out iCs2) == false)
                iCs2 = 0;
            string sName = dataTable.Rows[0]["Name"].ToString().Trim();
            string sAddress1 = dataTable.Rows[0]["Address1"].ToString().Trim();
            string sAddress2 = dataTable.Rows[0]["Address2"].ToString().Trim();
            string sAddress3 = dataTable.Rows[0]["Address3"].ToString().Trim();
            string sCity = dataTable.Rows[0]["City"].ToString().Trim();
            string sState = dataTable.Rows[0]["State"].ToString().Trim();
            string sZip = dataTable.Rows[0]["Zip"].ToString().Trim();
            string sContact = dataTable.Rows[0]["Contact"].ToString().Trim();
            string sPhone = dataTable.Rows[0]["Phone"].ToString().Trim();
            string sCrossRef = dataTable.Rows[0]["CrossRef"].ToString().Trim();

            if (contact != "")
                sContact = contact;
            if (phone != "")
                sPhone = phone;

            TableRow tRow;
            TableCell tCell;

            // Row 1
            tRow = new TableHeaderRow();
            tbTemp.Rows.Add(tRow);

            tCell = new TableHeaderCell();
            lbTemp = new Label();
            lbTemp.Text = sName;
            lbTemp.SkinID = "labelTitleColor2_Medium";
            //lbTemp.Font.Size = 12;
            tCell.Controls.Add(lbTemp);
            tRow.Cells.Add(tCell);

            // Row 2
            tRow = new TableHeaderRow();
            tbTemp.Rows.Add(tRow);

            tCell = new TableHeaderCell();
            lbTemp = new Label();
            string sText = 
                sAddress1 + " " +
                sAddress2 + " " +
                sAddress3 + " " +
                sCity + ", " +
                sState +
                "&nbsp;&nbsp;" + sZip +
                "&nbsp;&nbsp;&nbsp;&nbsp;<i>Customer:</i>&nbsp;" + iCs1.ToString() + "-" + iCs2.ToString() +
                "&nbsp;&nbsp;&nbsp;&nbsp;<i>Contact:</i>&nbsp;" + sContact +
                "&nbsp;&nbsp;&nbsp;" + nf.phoneFormat1(sPhone);
            if (ext != "")
                sText += "&nbsp;&nbsp;&nbsp;<b>Ext:</b> " + ext;
            lbTemp.Text = sText;
            //lbTemp.SkinID = "labelTitleColor2_Small";
            lbTemp.Font.Size = 9;
            tCell.VerticalAlign = VerticalAlign.Top;
            tCell.Height = 25;
            tCell.Controls.Add(lbTemp);
            tRow.Cells.Add(tCell);
        }
        return tbTemp;
    }

    // ========================================================================
    // ========================================================================
}
