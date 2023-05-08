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
/// Summary description for SourceForTicket
/// </summary>
public class SourceForTicket
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sClassLib = "";
    Customer_LIVE.CustomerMenuSoapClient wsLive = new Customer_LIVE.CustomerMenuSoapClient();
    Customer_DEV.CustomerMenuSoapClient wsTest = new Customer_DEV.CustomerMenuSoapClient();
    SourceForDefaults sfd = new SourceForDefaults();

//    System.Drawing.Color myWhite = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
//    System.Drawing.Color myGrayE = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
//    System.Drawing.Color myBlue = System.Drawing.ColorTranslator.FromHtml("#406080"); // EBEBE0
//    System.Drawing.Color myBeige = System.Drawing.ColorTranslator.FromHtml("#EBEBE0");
//    System.Drawing.Color myYellow = System.Drawing.ColorTranslator.FromHtml("#FFF8DC");
//    System.Drawing.Color currentRowColor;

    // ========================================================================
	public SourceForTicket()
	{
		//
		// TODO: Add constructor logic here
		//
        SiteHandler sh = new SiteHandler();
        sClassLib = sh.getLibrary();
	}
    // =========================================================
    public Panel GetTicketDisplayPanel(int ctr, int tck, int cs1)
    {
        Panel pnDisplay = new Panel(); // use temp panel to clear and recreate data to display
        Label lbTemp = new Label();
        string sLineBreak = "<div class='spacer20'></div>";
        //string sLineBreak = "<br /><br /><br /><br />";
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);
// xxx
        pnDisplay.Controls.Add(BuildTitleLabel("Ticket Information"));

        if (sClassLib == "L")
        {
            dataTable = wsLive.GetTicketDetail(sfd.GetWsKey(), ctr, tck, cs1);        
        }
        else
        {
            dataTable = wsTest.GetTicketDetail(sfd.GetWsKey(), ctr, tck, cs1);
        }

        pnDisplay.Controls.Add(LoadTicketDetailTable(dataTable));
        lbTemp = new Label();
        lbTemp.Text = sLineBreak;
        pnDisplay.Controls.Add(lbTemp);

        if (sClassLib == "L") 
        {  // oma-dev-int/webservices/app_Code/cust.cs/"find method"
            dataTable = wsLive.GetModelSummary(sfd.GetWsKey(), ctr, tck, cs1);        
        }
        else 
        { 
            dataTable = wsTest.GetModelSummary(sfd.GetWsKey(), ctr, tck, cs1);
        }
        if (dataTable.Rows.Count > 0)
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Model Information"));
            pnDisplay.Controls.Add(LoadModelSummaryTable(dataTable));

            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        if (sClassLib == "L")
            dataTable = wsLive.GetTimestamps(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetTimestamps(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0) 
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Timestamps"));
            pnDisplay.Controls.Add(LoadTimestampTable(dataTable));

            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);
        }


        if (sClassLib == "L")
            dataTable = wsLive.GetLaborOnsite(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetLaborOnsite(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0) 
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Onsite Labor"));
            pnDisplay.Controls.Add(LoadLaborTable(dataTable));

            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);
        }


        if (sClassLib == "L")
            dataTable = wsLive.GetLaborTravel(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetLaborTravel(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0)
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Travel Labor"));
            pnDisplay.Controls.Add(LoadLaborTable(dataTable));

            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        if (sClassLib == "L")
            dataTable = wsLive.GetPartsUsed(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetPartsUsed(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0) 
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Parts Used"));
            pnDisplay.Controls.Add(LoadPartsUsedTable(dataTable));
            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        if (sClassLib == "L")
            dataTable = wsLive.GetTicketPartTracking(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetTicketPartTracking(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0)
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Package Tracking"));
            pnDisplay.Controls.Add(LoadPackageTrackingTable(dataTable));
            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        if (sClassLib == "L")
            dataTable = wsLive.GetNotes(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetNotes(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0) 
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Customer Notes"));
            pnDisplay.Controls.Add(LoadNotesTable(dataTable));
            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        string sShowAddNote = "";
        if (sClassLib == "L")
        {
            sShowAddNote = wsLive.GetNoteAddStatus(sfd.GetWsKey(), ctr, tck);
        }
        else
        {
            sShowAddNote = wsTest.GetNoteAddStatus(sfd.GetWsKey(), ctr, tck);
        }

        if (sShowAddNote == "Y")
        {
            Button btAddNote = new Button();
            btAddNote.Text = "Add Ticket Note";
            //btAddNote.SkinID = "ButtonTopSpace";

            string sTckEncrypt = sfd.GetTicketEncrypted(ctr, tck);
            btAddNote.PostBackUrl = "~/public/sc/AddNote.aspx?key=" + sTckEncrypt;
            pnDisplay.Controls.Add(btAddNote);
            lbTemp = new Label();
            lbTemp.Text = sLineBreak;
            pnDisplay.Controls.Add(lbTemp);

        }

        return pnDisplay;
    }
    // =========================================================
    public Panel GetTimestampDisplayPanel(int ctr, int tck, int cs1User)
    {
        Panel pnDisplay = new Panel(); // use temp panel to clear and recreate data to display
        Label lbTemp = new Label();
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);

        if (sClassLib == "L")
            dataTable = wsLive.GetTimestamps(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetTimestamps(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0)
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Timestamps"));
            pnDisplay.Controls.Add(LoadTimestampTable(dataTable));
        }
        else 
        {
            pnDisplay.Controls.Add(BuildTitleLabel("No Timestamps Found For Ticket: " + ctr.ToString() + "-" + tck.ToString()));
        }

        return pnDisplay;
    }

    // =========================================================
    public Panel GetPartUseDisplayPanel(int ctr, int tck, int cs1User)
    {
        Panel pnDisplay = new Panel(); // use temp panel to clear and recreate data to display
        Label lbTemp = new Label();
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dataTable = new DataTable(sMethodName);


        if (sClassLib == "L")
            dataTable = wsLive.GetPartsUsed(sfd.GetWsKey(), ctr, tck);
        else
            dataTable = wsTest.GetPartsUsed(sfd.GetWsKey(), ctr, tck);
        if (dataTable.Rows.Count > 0)
        {
            pnDisplay.Controls.Add(BuildTitleLabel("Parts Used"));
            pnDisplay.Controls.Add(LoadPartsUsedTable(dataTable));
        }
        else
        {
            pnDisplay.Controls.Add(BuildTitleLabel("No Part Use Found For Ticket: " + ctr.ToString() + "-" + tck.ToString()));
        }

        return pnDisplay;
    }
 
    // ========================================================================
    public Table LoadTicketDetailTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        Table tbLeft = new Table();
        Table tbRight = new Table();
        //tbTemp.CssClass = "tableVerticalList";
        tbTemp.SkinID = "tableWithoutLines";

        DateFormatter df = new DateFormatter();
        StringFormatter sf = new StringFormatter();
        string sFormat = "";

        int iCs1 = 0;
        int iCs2 = 0;
        int iCtr = 0;
        int iTck = 0;
        int iDateEntered = 0;
        int iDateCompleted = 0;
        int iDateClosed = 0;

        double dTimeEntered = 0.0;
        double dTimeCompleted = 0.0;
        double dTimeClosed = 0.0;

        string sRemark = "";
        string sCallType = "";
        string sComment = "";
        string sCustName = "";
        string sCstXrf = "";
        string sTckXrf = "";
        string sAddress1 = "";
        string sAddress2 = "";
        string sCity = "";
        string sState = "";
        string sZip = "";
        string sRequestedBy = "";
        string sContact = "";
        string sPhone = "";
        string sExtension = "";
//        string sAgr = "";
        string sAgrDesc = "";
        string sDateFormat = "Mon dd, YYYY";
        string sTimeFormat = ": pm";

        TableRow tRow;
        TableHeaderCell thCell;
        TableCell tCell;

        // Main Content Section
        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (int.TryParse(dTable.Rows[iRowIdx]["CustNum"].ToString(), out iCs1) == false)
                iCs1 = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["CustLoc"].ToString(), out iCs2) == false)
                iCs2 = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["Center"].ToString(), out iCtr) == false)
                iCtr = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["Ticket"].ToString(), out iTck) == false)
                iTck = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["DateEntered"].ToString(), out iDateEntered) == false)
                iDateEntered = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["TimeEntered"].ToString(), out dTimeEntered) == false)
                dTimeEntered = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["DateCompleted"].ToString(), out iDateCompleted) == false)
                iDateCompleted = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["TimeCompleted"].ToString(), out dTimeCompleted) == false)
                dTimeCompleted = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["DateClosed"].ToString(), out iDateClosed) == false)
                iDateClosed = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["TimeClosed"].ToString(), out dTimeClosed) == false)
                dTimeClosed = 0;

            sTckXrf = dTable.Rows[iRowIdx]["PurchOrd"].ToString().Trim();
            sCstXrf = dTable.Rows[iRowIdx]["CrossRef"].ToString().Trim();
            sCallType = dTable.Rows[iRowIdx]["CallType"].ToString().Trim();
            sCustName = dTable.Rows[iRowIdx]["CustName"].ToString().Trim();
            sRemark = dTable.Rows[iRowIdx]["Remark"].ToString().Trim();
            sComment = dTable.Rows[iRowIdx]["Comment1"].ToString().Trim() + " " + dTable.Rows[iRowIdx]["Comment2"].ToString().Trim();
            sAddress1 = dTable.Rows[iRowIdx]["Address1"].ToString().Trim();
            sAddress2 = dTable.Rows[iRowIdx]["Address2"].ToString().Trim();
            sCity = dTable.Rows[iRowIdx]["City"].ToString().Trim();
            sState = dTable.Rows[iRowIdx]["State"].ToString().Trim();
            sZip = dTable.Rows[iRowIdx]["Zip"].ToString().Trim();
            sRequestedBy = dTable.Rows[iRowIdx]["RequestedBy"].ToString().Trim();
            sContact = dTable.Rows[iRowIdx]["Contact"].ToString().Trim();
            sPhone = dTable.Rows[iRowIdx]["Phone"].ToString().Trim();
            sExtension = dTable.Rows[iRowIdx]["Extension"].ToString().Trim();
            sAgrDesc = dTable.Rows[iRowIdx]["AgrDescription"].ToString().Trim();

            iRowIdx++;
        }

        // ----------------------
        // Outer Table: Create Row, Add Two TD Cells, and put an empty table in each
        // ----------------------
        tRow = new TableRow();
        tbTemp.Rows.Add(tRow);

        tCell = new TableCell();
        tCell.Controls.Add(tbLeft);
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Width = 15; // This controls the space between the left and right tables
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Controls.Add(tbRight);
        tRow.Cells.Add(tCell);

        // ----------------------
        // Left Table
        // ----------------------

        // Row 1
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Customer";
        thCell.Width = 100;
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = iCs1.ToString() + " - " + iCs2.ToString();
        tRow.Cells.Add(tCell);

        // Row 2
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Cust Cross Ref";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sCstXrf;
        tRow.Cells.Add(tCell);

        // Row 3
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Name";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sCustName;
        tRow.Cells.Add(tCell);

        // Row 4
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Address";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sAddress1 + " " + sAddress2;
        tRow.Cells.Add(tCell);

        // Row 5
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "City, State, Zip";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sCity + ", " + sState + "  " + sZip;
        tRow.Cells.Add(tCell);

        // Row 6
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Contact";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sContact;
        tRow.Cells.Add(tCell);

        // Row 7
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Phone";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        sFormat = "()-";
        tCell.Text = sf.formatPhone(sPhone, sFormat);
        if (sExtension != "")
            tCell.Text += " Ext: " + sExtension;
        tRow.Cells.Add(tCell);

        // Row 8
        tRow = new TableRow();
        tbLeft.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Requested By";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sRequestedBy;
        tRow.Cells.Add(tCell);

        // ----------------------
        // Right Table
        // ----------------------

        // Row 1
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Ticket";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        thCell.Width = 120;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = iCtr.ToString() + "-" + iTck.ToString();
        tRow.Cells.Add(tCell);

        // Row 2
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Ticket Cross Ref";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sTckXrf;
        tRow.Cells.Add(tCell);

        // Row 3
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Comment";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sComment;
        tRow.Cells.Add(tCell);

        // Row 4
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Call Type";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        string sCallDesc = "";
        if (sCallType == "R")
            sCallDesc = "AGREEMENT";
        else if (sCallType == "$")
            sCallDesc = "TIME & MATERIALS";
        else if (sCallType == "V")
            sCallDesc = "COVERAGE BEING VERIFIED";
        else if (sCallType == "C")
            sCallDesc = "AGREEMENT";

        tCell = new TableCell();
        tCell.Text = sCallDesc;
        tRow.Cells.Add(tCell);

        // Row 5
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Agreement Type";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = sAgrDesc;  
        tRow.Cells.Add(tCell);
/* RECOMBINE DATE AND TIME WHEN YOU GET ANOTHER COLUMN
        // Row 6
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Entry Date/Time";
        thCell.VerticalAlign = VerticalAlign.Top;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = df.FormatDate(iDateEntered, sDateFormat) + "&nbsp;&nbsp;&nbsp;" + df.FormatTime(dTimeEntered, sTimeFormat);
        tRow.Cells.Add(tCell);
*/
        // Row 6
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Entry Date";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = df.FormatDate(iDateEntered, sDateFormat);
        tRow.Cells.Add(tCell);

        // Row 7
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Entry Time";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        tCell.Text = df.FormatTime(dTimeEntered, sTimeFormat);
        tRow.Cells.Add(tCell);

        // Row 8
        tRow = new TableRow();
        tbRight.Rows.Add(tRow);

        thCell = new TableHeaderCell();
        //thCell.Text = "Close Date/Time";
        thCell.Text = "Close Date";
        thCell.VerticalAlign = VerticalAlign.Top;
        thCell.HorizontalAlign = HorizontalAlign.Left;
        tRow.Cells.Add(thCell);

        tCell = new TableCell();
        string sCloseDateTime = "";
        if (iDateCompleted > 0)
            sCloseDateTime = df.FormatDate(iDateCompleted, sDateFormat);
        if (dTimeCompleted > 0)
            sCloseDateTime += "&nbsp;&nbsp;&nbsp;" + df.FormatTime(dTimeCompleted, sTimeFormat);
        tCell.Text = sCloseDateTime;
        //tCell.Text = df.FormatDate(iDateCompleted, sDateFormat) + "&nbsp;&nbsp;&nbsp;" + df.FormatTime(dTimeCompleted, sTimeFormat);
        tRow.Cells.Add(tCell);
        
        return tbTemp;
    }

    // ========================================================================
    public Table LoadModelSummaryTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";
        
        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        string sMod = "";
        string sDsc = "";
        string sSer = "";
        string sXref = "";
        string sSrc = "";  // Model Source

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Equipment";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Description";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Serial";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Identifier";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            sMod = dTable.Rows[iRowIdx]["Model"].ToString().Trim();
            sDsc = dTable.Rows[iRowIdx]["Description"].ToString().Trim();
            sSer = dTable.Rows[iRowIdx]["Serial"].ToString().Trim();
            sSrc = dTable.Rows[iRowIdx]["ModelSource"].ToString().Trim();
            sXref = dTable.Rows[iRowIdx]["ModelXRef"].ToString().Trim();
            // Detail
            tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            tCell = new TableCell();
            tCell.Text = sMod;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sDsc;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSer;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sXref;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);
            iRowIdx++;
        }
        
        return tbTemp;
    }
    // ========================================================================
    public Table LoadTimestampTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";
        
        DateFormatter df = new DateFormatter();
        string sDateFormat = "Mon dd, YYYY";
        string sTimeFormat = ": pm";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;
        int iTechNum = 0;
        int iStampDate = 0;
        int iScheduleDate = 0;
        double dStampTime = 0;
        string sStatus = "";

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Tech";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Date Completed";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Time (Central)";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Status";
        thRow.Cells.Add(thCell);

        if (dTable.Columns.Count == 5)
        {
            thCell = new TableHeaderCell();
            thCell.Text = "Planned Start Date";
            thRow.Cells.Add(thCell);
        }

        // Main Content Section
        int iCurrentRowColor = 0;
    
        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (int.TryParse(dTable.Rows[iRowIdx]["TechNum"].ToString(), out iTechNum) == false)
                iTechNum = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["StampDate"].ToString(), out iStampDate) == false)
                iStampDate = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["StampTime"].ToString(), out dStampTime) == false)
                dStampTime = 0.0;

            sStatus = dTable.Rows[iRowIdx]["Status"].ToString().Trim();


            if (dTable.Columns.Count == 5)
            {
                if (int.TryParse(dTable.Rows[iRowIdx]["ScheduleDate"].ToString(), out iScheduleDate) == false)
                    iScheduleDate = 0;
            }
            // Detail
            tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            if (iCurrentRowColor == 0)
            {
                iCurrentRowColor = 1;
                tRow.CssClass = "trColorReg";
            }
            else 
            {
                iCurrentRowColor = 0;
                tRow.CssClass = "trColorAlt";
            }

            tCell = new TableCell();
            tCell.Text = iTechNum.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatDate(iStampDate, sDateFormat);
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatTime(dStampTime, sTimeFormat);
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sStatus;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            if (dTable.Columns.Count == 5)
            {
                tCell = new TableCell();
                tCell.Text = df.FormatDate(iScheduleDate, sDateFormat);
                tRow.Cells.Add(tCell);
            }

            iRowIdx++;
        }
        
        return tbTemp;
    }
    // ========================================================================
    public Table LoadLaborTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";

        DateFormatter df = new DateFormatter();
        string sDateFormat = "Mon dd, YYYY";
        string sTimeFormat = ": pm";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        int iEmpNum = 0;
        int iStartDate = 0;
        double dStartTime = 0;
        double dEndTime = 0;

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Tech";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Start Date";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Start Time (Central)";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "End Time (Central)";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iCurrentRowColor = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (int.TryParse(dTable.Rows[iRowIdx]["EmpNum"].ToString(), out iEmpNum) == false)
                iEmpNum = 0;
            if (int.TryParse(dTable.Rows[iRowIdx]["StartDate"].ToString(), out iStartDate) == false)
                iStartDate = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["StartTime"].ToString(), out dStartTime) == false)
                dStartTime = 0.0;
            if (double.TryParse(dTable.Rows[iRowIdx]["EndTime"].ToString(), out dEndTime) == false)
                dEndTime = 0.0;

            // Detail
            tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            if (iCurrentRowColor == 0)
            {
                iCurrentRowColor = 1;
                tRow.CssClass = "trColorReg";
            }
            else
            {
                iCurrentRowColor = 0;
                tRow.CssClass = "trColorAlt";
            }

            tCell = new TableCell();
            tCell.Text = iEmpNum.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatDate(iStartDate, sDateFormat);
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatTime(dStartTime, sTimeFormat);
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatTime(dEndTime, sTimeFormat);
            tRow.Cells.Add(tCell);

            iRowIdx++;
        }

        return tbTemp;
    }

    // ========================================================================
    public Table LoadTripsTakenTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        string sDate = "";
        string sStrTm = "";
        string sEndTm = "";
        string sTech = "";

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Travel Date";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Start Time";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Arrival Time";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Tech";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iCurrentRowColor = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (dTable.Rows[iRowIdx]["TimSts"].ToString() == "T")
                sStrTm = dTable.Rows[iRowIdx]["TimTst"].ToString().Trim();

            if (((dTable.Rows[iRowIdx]["TimSts"].ToString() == "S") || (dTable.Rows[iRowIdx]["TimSts"].ToString() == "C")) && (sStrTm != ""))
            {
                sDate = dTable.Rows[iRowIdx]["TimDst"].ToString().Trim();
                sEndTm = dTable.Rows[iRowIdx]["TimTst"].ToString().Trim();
                sTech = dTable.Rows[iRowIdx]["TimEmp"].ToString().Trim();

                // Detail
                tRow = new TableRow();
                tbTemp.Rows.Add(tRow);

                if (iCurrentRowColor == 0)
                {
                    iCurrentRowColor = 1;
                    tRow.CssClass = "trColorReg";
                }
                else
                {
                    iCurrentRowColor = 0;
                    tRow.CssClass = "trColorAlt";
                }

                tCell = new TableCell();
                tCell.Text = sDate;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = sStrTm.ToString();
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = sEndTm.ToString();
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = sTech;
                tCell.HorizontalAlign = HorizontalAlign.Left;
                tRow.Cells.Add(tCell);
            }
            else
            {
                if ((dTable.Rows[iRowIdx]["TimSts"].ToString() == "H") && (dTable.Rows[iRowIdx]["TIMRSN"].ToString() != "NA")  && (sStrTm != ""))
                {
                    sDate = dTable.Rows[iRowIdx]["TimDst"].ToString().Trim();
                    sEndTm = dTable.Rows[iRowIdx]["TimTst2"].ToString().Trim();
                    sTech = dTable.Rows[iRowIdx]["TimEmp"].ToString().Trim();

                    // Detail
                    tRow = new TableRow();
                    tbTemp.Rows.Add(tRow);

                    if (iCurrentRowColor == 0)
                    {
                        iCurrentRowColor = 1;
                        tRow.CssClass = "trColorReg";
                    }
                    else
                    {
                        iCurrentRowColor = 0;
                        tRow.CssClass = "trColorAlt";
                    }

                    tCell = new TableCell();
                    tCell.Text = sDate;
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();
                    tCell.Text = sStrTm.ToString();
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();
                    tCell.Text = sEndTm.ToString();
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();
                    tCell.Text = sTech;
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tRow.Cells.Add(tCell);
                }
            }

            iRowIdx++;
        }

        return tbTemp;
    }
    // ========================================================================
    public Table LoadPartsUsedTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        int iQty = 0;
        string sPrt = "";
        string sDsc = "";
        string sSer = "";

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Part Description";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Qty";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Serial";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iCurrentRowColor = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (int.TryParse(dTable.Rows[iRowIdx]["Qty"].ToString(), out iQty) == false)
                iQty = 0;
            sPrt = dTable.Rows[iRowIdx]["Part"].ToString().Trim();
            sDsc = dTable.Rows[iRowIdx]["Description"].ToString().Trim();
            sSer = dTable.Rows[iRowIdx]["Serial"].ToString().Trim();

            // Detail
            tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            if (iCurrentRowColor == 0)
            {
                iCurrentRowColor = 1;
                tRow.CssClass = "trColorReg";
            }
            else
            {
                iCurrentRowColor = 0;
                tRow.CssClass = "trColorAlt";
            }

            tCell = new TableCell();
            tCell.Text = sDsc;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = iQty.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSer;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            iRowIdx++;
        }

        return tbTemp;
    }

    // ========================================================================
    public Table LoadPackageTrackingTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        string sTrackLink = "";
        string sCarrier = "";
        string sTracking = "";
        double dWeight = 0.0;

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Carrier";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Weight";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Tracking Number";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iCurrentRowColor = 0;

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            sCarrier = dTable.Rows[iRowIdx]["Carrier"].ToString().Trim();
            
            double.TryParse(dTable.Rows[iRowIdx]["Weight"].ToString(), out dWeight);
            
            sTracking = dTable.Rows[iRowIdx]["Tracking"].ToString().Trim();

            // Detail
            tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            if (iCurrentRowColor == 0)
            {
                iCurrentRowColor = 1;
                tRow.CssClass = "trColorReg";
            }
            else
            {
                iCurrentRowColor = 0;
                tRow.CssClass = "trColorAlt";
            }

            tCell = new TableCell();
            tCell.Text = sCarrier;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = dWeight.ToString();
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            if (sCarrier == "FEDX")
            {
                sTrackLink = "<A href=\"http://www.fedex.com/Tracking?ascend_header=1&clienttype=dotcom&cntry_code=us&language=english&tracknumbers=" +
                    sTracking + "\"  target=\"track\">" + sTracking + "</a>";
            }
            else 
            {
                sTrackLink = "<center><a href=\"https://wwwapps.ups.com/WebTracking/track?track=yes&trackNums=" +
                    sTracking + "\" target=\"track\">" + sTracking + "</a></center>";
            }
            tCell.Text = sTrackLink;
            tRow.Cells.Add(tCell);

            iRowIdx++;
        }

        return tbTemp;
    }
    // ========================================================================
    public Table LoadNotesTable(DataTable dTable)
    {
        Table tbTemp = new Table();
        tbTemp.SkinID = "tableWithLines";

        DateFormatter df = new DateFormatter();
        string sDateFormat = "Mon dd, YYYY";
        string sTimeFormat = ": pm";

        TableHeaderRow thRow;
        TableHeaderCell thCell;
        TableRow tRow;
        TableCell tCell;

        int iDate = 0;
        double dTime = 0;
        string sAuthor = "";
        string sSubject = "";
        string sMessage = "";

        // Header Row
        thRow = new TableHeaderRow();
        tbTemp.Rows.Add(thRow);

        thCell = new TableHeaderCell();
        thCell.Text = "Date";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Time";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Author";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Subject";
        thRow.Cells.Add(thCell);

        thCell = new TableHeaderCell();
        thCell.Text = "Message";
        thRow.Cells.Add(thCell);

        // Main Content Section
        int iCurrentRowColor = 0;

        string sTemp = "";
        string sRemainder = "";
        int iPos = 0;
        string sTrackNum = "";
        string sLinkBeg = " <a href=\"https://wwwapps.ups.com/WebTracking/track?track=yes&trackNums=";
        string sLinkMid = "\" target=\"_blank\" >";
        string sLinkEnd = "</a> ";

        string sLinkFed = " <a href=\"https://www.fedex.com/apps/fedextrack/?action=track&language=english&cntry_code=us&tracknumbers=";

        int iRowIdx = 0;
        foreach (DataRow row in dTable.Rows)
        {
            if (int.TryParse(dTable.Rows[iRowIdx]["Date"].ToString(), out iDate) == false)
                iDate = 0;
            if (double.TryParse(dTable.Rows[iRowIdx]["Time"].ToString(), out dTime) == false)
                dTime = 0.0;
            sAuthor = dTable.Rows[iRowIdx]["Author"].ToString().ToString();
            sSubject = dTable.Rows[iRowIdx]["Subject"].ToString().ToString();
            sMessage = dTable.Rows[iRowIdx]["Message"].ToString().ToString();
            sRemainder = sMessage;
            if (sRemainder.Contains("Tracking: ") == true)
            {
                if (sRemainder.Contains("1Z") == true)
                {
                    iPos = sRemainder.IndexOf("1Z");
                    while (iPos > -1)
                    {
                        sTemp += sRemainder.Substring(0, iPos);
                        sRemainder = sRemainder.Substring(iPos);
                        if (sRemainder.Length >= 18)
                        {
                            sTrackNum = sRemainder.Substring(0, 18);
                            sRemainder = sRemainder.Substring(18);
                            sTemp += sLinkBeg + sTrackNum + sLinkMid + sTrackNum + sLinkEnd;
                        }
                        else
                            sRemainder = "";

                        iPos = sRemainder.IndexOf("1Z");
                    };
                    if (sRemainder != "")
                        sTemp += sRemainder;
                    sMessage = sTemp;
                }
                else // Fedex
                {
                    iPos = sRemainder.IndexOf("Tracking: ") + 10;
                    while (iPos > -1)
                    {
                        sTemp += sRemainder.Substring(0, iPos);
                        sRemainder = sRemainder.Substring(iPos);
                        if (sRemainder.Length >= 12)
                        {
                            sTrackNum = sRemainder.Substring(0, 12);
                            sRemainder = sRemainder.Substring(12);
                            sTemp += sLinkFed + sTrackNum + sLinkMid + sTrackNum + sLinkEnd;
                        }
                        else
                            sRemainder = "";

                        iPos = sRemainder.IndexOf("Tracking: ");
                    };
                    if (sRemainder != "")
                        sTemp += sRemainder;
                    sMessage = sTemp;
                }
            }

            //    sTracking += "<a href=\"https://www.fedex.com/apps/fedextrack/?action=track&language=english&cntry_code=us&tracknumbers=" +
          //     sTemp + "\" target=\"track\">" + sTemp + "</a>";

                // Detail
                tRow = new TableRow();
            tbTemp.Rows.Add(tRow);

            if (iCurrentRowColor == 0)
            {
                iCurrentRowColor = 1;
                tRow.CssClass = "trColorReg";
            }
            else
            {
                iCurrentRowColor = 0;
                tRow.CssClass = "trColorAlt";
            }

            tCell = new TableCell();
            tCell.Text = df.FormatDate(iDate, sDateFormat);
            tCell.Width = 85;
            tCell.VerticalAlign = VerticalAlign.Top;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = df.FormatTime(dTime, sTimeFormat);
            tCell.Width = 65;
            tCell.VerticalAlign = VerticalAlign.Top;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sAuthor;
            tCell.VerticalAlign = VerticalAlign.Top;
            tCell.HorizontalAlign = HorizontalAlign.Center;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sSubject;
            tCell.VerticalAlign = VerticalAlign.Top;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            tCell = new TableCell();
            tCell.Text = sMessage;
            tCell.VerticalAlign = VerticalAlign.Top;
            tCell.HorizontalAlign = HorizontalAlign.Left;
            tRow.Cells.Add(tCell);

            iRowIdx++;
        }

        return tbTemp;
    }
    // ========================================================================
    public Label BuildTitleLabel(string sText)
    {
        Label lbTemp = new Label();
        lbTemp.CssClass = "labelTitleMedium";
        lbTemp.Text = sText;

        return lbTemp;
    }

    // ========================================================================
    // ========================================================================
}