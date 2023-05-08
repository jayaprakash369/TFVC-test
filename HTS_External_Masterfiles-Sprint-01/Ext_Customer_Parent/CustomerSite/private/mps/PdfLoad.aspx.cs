using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class private_mps_PdfLoad : System.Web.UI.Page
{
    // --------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == "1") 
            Show_Hello();
        else if(Request.QueryString["id"] == "2") 
            Show_Table();
        else if (Request.QueryString["id"] == "3")
            Show_MPS();
    }
    // --------------------------------------------------------
    protected void Show_Hello()
    {
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "\\media\\scantron\\workfiles\\Hello.pdf", FileMode.Create));
        //PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "\\private\\mps\\4.pdf", FileMode.CreateNew));
        doc.Open();
        doc.Add(new Paragraph("Hello World From STS PDF in public folder"));
        doc.Close();
        Response.Redirect("~/public/pdf/Hello.pdf", false);
    }
    // --------------------------------------------------------
    protected void Show_Table()
    {
        //System.Web.UI.WebControls.Table Table1 = new System.Web.UI.WebControls.Table();
        iTextSharp.text.Table table;
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "\\media\\scantron\\workfiles\\Table.pdf", FileMode.Create));
        doc.Open();

        string rootPath = Server.MapPath("~");
        string fullPath = Server.MapPath("~") + "\\media\\scantron\\images\\logos\\company\\ScantronTechnologySolutions.jpg";
        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(fullPath);

        Paragraph para = new Paragraph();

        para.Add("This is the paragraph text");
        para.Font.Color = Color.RED;
        doc.Add(para);
        doc.Add(new Paragraph("Header for PDF Table Example"));

        table = new iTextSharp.text.Table(3);
        table.BorderWidth = 1;
        table.BorderColor = new Color(0, 0, 255);
        table.Padding = 3;
        table.Spacing = 1;
        Cell cell = new Cell("header");
        cell.Header = true;
        cell.Colspan = 3;
        table.AddCell(cell);

        cell = new Cell("example cell with colspan 1 and rowspan 2");
        cell.Rowspan = 2;
        cell.BorderColor = new Color(255, 0, 0);
        table.AddCell(cell);

        table.AddCell("1.1");
        table.AddCell("2.1");
        table.AddCell("1.2");
        //table.AddCell("2.2 <br />" + logo);
        cell = new Cell("Image");
        cell.Add(logo);
        table.AddCell(cell);
        table.AddCell("cell test1");
        cell = new Cell("big cell");
        cell.Rowspan = 2;
        cell.Colspan = 2;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell.BackgroundColor = new Color(192, 192, 192);
        table.AddCell(cell);
        table.AddCell("cell test2");
        doc.Add(table);
        doc.Add(logo);

        /*
        tCell = new TableCell();
        tCell.Text = "Header";
//        tCell.ColumnSpan.Equals(3);
        tCell.HorizontalAlign = HorizontalAlign.Center;
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "City";
        tRow.Cells.Add(tCell);

        tCell = new TableCell();
        tCell.Text = "State";
        tRow.Cells.Add(tCell);

        doc.AddHeader("Header Name", "Header Content");
        doc.Add(new Paragraph("Trying to build the table PDF"));
        doc.Add(Table1);
        */
        doc.Close();
        Response.Redirect("~/public/pdf/Table.pdf", false);

/*
        Dim table As Table = New Table(3)
        table.BorderWidth = 1
        table.BorderColor = New Color(0, 0, 255)
        table.Padding = 3
        table.Spacing = 1
        Dim cell As Cell = New Cell("header")
        cell.Header = True
        cell.Colspan = 3
        table.AddCell(cell)
        cell = New Cell("example cell with colspan 1 and rowspan 2")
        cell.Rowspan = 2
        cell.BorderColor = New Color(255, 0, 0)
        table.AddCell(cell)
        table.AddCell("1.1")
        table.AddCell("2.1")
        table.AddCell("1.2")
        table.AddCell("2.2")
        table.AddCell("cell test1")
        cell = New Cell("big cell")
        cell.Rowspan = 2
        cell.Colspan = 2
        cell.HorizontalAlignment = Element.ALIGN_CENTER
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.BackgroundColor = New Color(192, 192, 192)
        table.AddCell(cell)
        table.AddCell("cell test2")
        doc.Add(table)
        doc.Close()
        Response.Redirect("~/2.pdf", false)
*/


    }
    // --------------------------------------------------------
    protected void Show_MPS()
    {

        //iTextSharp.text.pdf.PdfPTable table2 = new iTextSharp.text.pdf.PdfPTable(1);
        iTextSharp.text.pdf.PdfPTable table;
        PdfPCell cell;
        Phrase phrase;

        string fullPath = "";
        string rootPath = Server.MapPath("~") + "\\media\\scantron\\pdf\\";

        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "\\media\\scantron\\workfiles\\mp\\MpsReview.pdf", FileMode.Create));

        doc.Open();
        // -----------------------------------------------------------------

        table = new iTextSharp.text.pdf.PdfPTable(2);
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        table.SpacingAfter = 20f;

        phrase = new Phrase("Managed Print Review");
        phrase.Font.Color = Color.RED;
        cell = new PdfPCell(phrase);
        cell.Colspan = 2;
        cell.BorderColor = Color.WHITE;
        cell.Padding = 5f;
        table.AddCell(cell);
        
        // Row 2
        phrase = new Phrase("Cell One");
        cell = new PdfPCell(phrase);
        cell.Colspan = 1;
        cell.BorderColor = Color.LIGHT_GRAY;
        cell.Padding = 5f;
        table.AddCell(cell);

        phrase = new Phrase("Cell Two");
        cell = new PdfPCell(phrase);
        cell.Colspan = 1;
        cell.BorderColor = Color.LIGHT_GRAY;
        cell.Padding = 5f;
        table.AddCell(cell);

        doc.Add(table);

        // ------------------------------------------------
        fullPath = rootPath + "ByType.bmp";
        iTextSharp.text.Image byType = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByModel.bmp";
        iTextSharp.text.Image byModel = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByManufacturer.bmp";
        iTextSharp.text.Image byManufacturer = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByCount.bmp";
        iTextSharp.text.Image byCount = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByToner.bmp";
        iTextSharp.text.Image byToner = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByHighUse.bmp";
        iTextSharp.text.Image byHighUse = iTextSharp.text.Image.GetInstance(fullPath);

        fullPath = rootPath + "ByLowUse.bmp";
        iTextSharp.text.Image byLowUse = iTextSharp.text.Image.GetInstance(fullPath);
        
        // -----------------------------------------------------------------
        // Detail Table
        // -----------------------------------------------------------------
        
        // -----------------------------------------------------------------

        fullPath = rootPath + "ByCategory.bmp";
        iTextSharp.text.Image byCategory = iTextSharp.text.Image.GetInstance(fullPath);
        
        // -----------------------------------------------------------------
        PdfPCell cell2;
        iTextSharp.text.pdf.PdfPTable table2 = new iTextSharp.text.pdf.PdfPTable(2);
        table2.HorizontalAlignment = 1;
        table2.TotalWidth = 500f;
        table2.LockedWidth = true;
        

        //PdfPCell cell2 = new PdfPCell(new Phrase("Header spanning 3 columns"));
        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byType);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byModel);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byManufacturer);
        table2.AddCell(cell2);


        cell2 = new PdfPCell();
        cell2.HorizontalAlignment = 1;
        cell2.AddElement(byToner);
        cell2.PaddingRight = 5f;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.HorizontalAlignment = 1;
        cell2.AddElement(byCount);
        cell2.PaddingLeft = 5f;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        table2.AddCell(cell2);

        
        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byHighUse);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byLowUse);
        table2.AddCell(cell2);

        cell2 = new PdfPCell();
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = 1;
        cell2.PaddingBottom = 20f;
        cell2.BorderColor = Color.WHITE;
        cell2.AddElement(byCategory);
        table2.AddCell(cell2);

        doc.Add(table2);

        // -----------------------------------------------------------------
        doc.Close();
        Response.Redirect("~/public/pdf/MpsReview.pdf", false);

    }
    // --------------------------------------------------------
}