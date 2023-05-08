using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class private__admin_Styles : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES
    // ---------------------------------------
    string sText = "Sample text for the control";
    string sList = "Default AAA|Default BBB|Default CCC";
    string sList2 = "Default AAA|Default BBB|Default CCC|Default DDD|Default EEE|Default FFF";

    // =========================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = false;
    }
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {

        Load_DefaultCss();
        Load_CustomCss();
        Load_DefaultSkins();
        Load_CustomSkins();

    }
    // =========================================================
    protected void Load_DefaultCss()
    {
        Label lbTemp;

        lbTemp = new Label();
        lbTemp.Text = "<h1>Default CSS Styles</h1><div class='spacer5'></div>";
        pnDefaultCss.Controls.Add(lbTemp);

        pnDefaultCss.Controls.Add(Load_StyleRow("body", "Label", "Sample of default text for the body"));
        pnDefaultCss.Controls.Add(Load_StyleRow("p", "Label", "<p>Paragraph text sample<br /> with more to show text spacing and bottom padding</p>"));
        pnDefaultCss.Controls.Add(Load_StyleRow("H1", "Label", "<h1>" + sText + "</h1>"));
        pnDefaultCss.Controls.Add(Load_StyleRow("H2", "Label", "<h2>" + sText + "</h2>"));
        pnDefaultCss.Controls.Add(Load_StyleRow("H3", "Label", "<h3>" + sText + "</h3>"));
        pnDefaultCss.Controls.Add(Load_StyleRow("H4", "Label", "<h4>" + sText + "</h4>"));
        pnDefaultCss.Controls.Add(Load_StyleRow("H5", "Label", "<h5>" + sText + "</h5>"));
    }
    // =========================================================
    protected void Load_CustomCss()
    {
        Label lbTemp;
        string sTemp = "";

        lbTemp = new Label();
        lbTemp.Text = "<h1>Custom CSS Styles</h1><div class='spacer5'></div>";
        pnCustomCss.Controls.Add(lbTemp);
        
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor1_Large", "Label", sText));
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor1_Medium", "Label", sText));
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor1_Small", "Label", sText));
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor2_Large", "Label", sText));
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor2_Medium", "Label", sText));
        pnCustomCss.Controls.Add(Load_StyleRow("labelTitleColor2_Small", "Label", sText));

        sTemp = "<table class='tableWithLines'>";
        sTemp += "<tr><th>Aaa</th><th>Bbb</th></tr>";
        sTemp += "<tr><td>A One</td><td>B One</td></tr>";
        sTemp += "<tr><td>A Two</td><td>B Two</td></tr>";
        sTemp += "</table>";
        pnCustomCss.Controls.Add(Load_StyleRow("tableWithLines", "Label", sTemp));

        sTemp = "<table class='tableWithoutLines'>";
        sTemp += "<tr><th>Aaa</th><th>Bbb</th></tr>";
        sTemp += "<tr><td>A One</td><td>B One</td></tr>";
        sTemp += "<tr><td>A Two</td><td>B Two</td></tr>";
        sTemp += "</table>";
        pnCustomCss.Controls.Add(Load_StyleRow("tableWithoutLines", "Label", sTemp));

        sTemp = "<ul class='ulDefault'>";
        sTemp += "<li>Level One A</li>";
        sTemp += "<li>Level One B";
        sTemp += "<ul>";
        sTemp += "<li>Level Two A";
        sTemp += "<ul>";
        sTemp += "<li>Level Three A</li>";
        sTemp += "<li>Level Three B</li>";
        sTemp += "</ul>";
        sTemp += "</li>";
        sTemp += "<li>Level Two B</li>";
        sTemp += "</ul>";
        sTemp += "</li>";
        sTemp += "<li>Level One C</li>";
        sTemp += "</table>";
        pnCustomCss.Controls.Add(Load_StyleRow("ulDefault", "Label", sTemp));

        sTemp = "<ul class='ulPL'>";
        sTemp += "<li>Level One A</li>";
        sTemp += "<li>Level One B";
        sTemp += "<ul>";
        sTemp += "<li>Level Two A";
        sTemp += "<ul>";
        sTemp += "<li>Level Three A</li>";
        sTemp += "<li>Level Three B</li>";
        sTemp += "</ul>";
        sTemp += "</li>";
        sTemp += "<li>Level Two B</li>";
        sTemp += "</ul>";
        sTemp += "</li>";
        sTemp += "<li>Level One C</li>";
        sTemp += "</table>";
        pnCustomCss.Controls.Add(Load_StyleRow("ulPL", "Label", sTemp));

    }
    // =========================================================
    protected void Load_DefaultSkins()
    {
        Label lbTemp;
        lbTemp = new Label();
        lbTemp.Text = "<h1>Default Skins</h1><div class='spacer5'></div>";
        pnDefaultSkins.Controls.Add(lbTemp);

        pnDefaultSkins.Controls.Add(Load_StyleRow("Button", "Button", "Default Button"));
        pnDefaultSkins.Controls.Add(Load_StyleRow("Calendar", "Calendar", ""));
        pnDefaultSkins.Controls.Add(Load_StyleRow("CheckBox", "CheckBox", "Default CheckBox"));
        pnDefaultSkins.Controls.Add(Load_StyleRow("CheckBoxList", "CheckBoxList", sList));
        pnDefaultSkins.Controls.Add(Load_StyleRow("DropDownList", "DropDownList", sList));
        pnDefaultSkins.Controls.Add(Load_StyleRow("HyperLink", "HyperLink", "Default HyperLink"));
        pnDefaultSkins.Controls.Add(Load_StyleRow("LinkButton", "LinkButton", "Default LinkButton"));
        pnDefaultSkins.Controls.Add(Load_StyleRow("ListBox", "ListBox", sList2));
        pnDefaultSkins.Controls.Add(Load_StyleRow("RadioButton", "RadioButton", "Default RadioButton"));
        pnDefaultSkins.Controls.Add(Load_StyleRow("RadioButtonList", "RadioButtonList", ""));
        pnDefaultSkins.Controls.Add(Load_StyleRow("TextBox", "TextBox", "Default TextBox"));
    }
    // =========================================================
    protected void Load_CustomSkins()
    {
        Label lbTemp;

        lbTemp = new Label();
        lbTemp.Text = "<h1>Custom Skins</h1><div class='spacer5'></div>";
        pnCustomSkins.Controls.Add(lbTemp);

        pnCustomSkins.Controls.Add(Load_StyleRow("calendarBlue", "Calendar", ""));
        pnCustomSkins.Controls.Add(Load_StyleRow("calendarGreen", "Calendar", ""));
        pnCustomSkins.Controls.Add(Load_StyleRow("calendarOrange", "Calendar", ""));

        pnCustomSkins.Controls.Add(Load_StyleRow("checkBoxHidingText", "CheckBox", sText));

        pnCustomSkins.Controls.Add(Load_StyleRow("hyperLinkHeader", "HyperLink", sText));

        pnCustomSkins.Controls.Add(Load_StyleRow("labelError", "Label", sText));
        pnCustomSkins.Controls.Add(Load_StyleRow("labelInstructions", "Label", sText));
        pnCustomSkins.Controls.Add(Load_StyleRow("labelComment", "Label", sText));

        pnCustomSkins.Controls.Add(Load_StyleRow("labelUserQuote", "Label", "This is a longer text to show the full user quote to see <br />the way it would be formatted when displayed on a web page.<br /> This would be an additional second line. "));

        pnCustomSkins.Controls.Add(Load_StyleRow("linkButtonDark_LightText", "LinkButton", sText));
        pnCustomSkins.Controls.Add(Load_StyleRow("linkButtonHeader", "LinkButton", sText));

    }

    // =========================================================
    protected Panel Load_StyleRow(string title, string rowControlType, string textList)  // could be aaa|bbb|ccc
    {
        Panel pnRow = new Panel();

        // Row Label 
        pnRow.Controls.Add(Load_StyleCell(title, "Label", title, "panelStyleTitle"));
        // Row Styled Control Example 
        pnRow.Controls.Add(Load_StyleCell(title, rowControlType, textList, "panelStyleBox"));
        // Label to force new line
        Label lbTemp = new Label();
        string sNewLine = "<div class='spacer5'></div>";
        lbTemp.Text = sNewLine;
        pnRow.Controls.Add(lbTemp);

        return pnRow;
    }
    // =========================================================
    protected Panel Load_StyleCell(string title, string controlNeeded, string textList, string boxStyle)
    {
        Panel pnCell = new Panel();
        int i = 0;
        ListItem liTemp;
        
        string[] saTemp = textList.Split('|');

        pnCell.SkinID = boxStyle;  // "panelStyleBox";

        if (controlNeeded == "Button")
        {
            Button btTemp = new Button();
            btTemp.Text = saTemp[0];
            pnCell.Controls.Add(btTemp);
            btTemp = null;
        }
        else if (controlNeeded == "Calendar")
        {
            Calendar clTemp = new Calendar();

            if (
                   title == "calendarBlue"
                || title == "calendarGreen"
                || title == "calendarOrange"
                )
                clTemp.SkinID = title;

            pnCell.Controls.Add(clTemp);
            clTemp = null;
        }
        else if (controlNeeded == "CheckBox")
        {
            CheckBox cbTemp = new CheckBox();
            cbTemp.Text = saTemp[0];

            if (
                    title == "checkBoxHidingText"
                 || title == "ch..."
                )
                cbTemp.SkinID = title;


            pnCell.Controls.Add(cbTemp);
            cbTemp = null;
        }
        else if (controlNeeded == "CheckBoxList")
        {
            CheckBoxList cblTemp = new CheckBoxList();
            for (i = 0; i < saTemp.Length; i++)
            {
                liTemp = new ListItem();
                liTemp.Text = saTemp[i];
                liTemp.Value = saTemp[i];
                cblTemp.Items.Add(liTemp);
            }
            liTemp = null;
            pnCell.Controls.Add(cblTemp);
            cblTemp = null;
        }
        else if (controlNeeded == "DropDownList")
        {
            DropDownList ddlTemp = new DropDownList();
            for (i = 0; i < saTemp.Length; i++)
            {
                liTemp = new ListItem();
                liTemp.Text = saTemp[i];
                liTemp.Value = saTemp[i];
                ddlTemp.Items.Add(liTemp);
            }
            liTemp = null;
            pnCell.Controls.Add(ddlTemp);
            ddlTemp = null;
        }
        else if (controlNeeded == "HyperLink")
        {
            HyperLink hlTemp = new HyperLink();
            hlTemp.Text = saTemp[0];

            if (
                    title == "hyperLinkHeader"
                 || title == "hyperLinkHeader"
                )
                hlTemp.SkinID = title;

            hlTemp.NavigateUrl = "~/Default.aspx";
            pnCell.Controls.Add(hlTemp);
            hlTemp = null;
        }
        else if (controlNeeded == "Label")
        {
            Label lbTemp = new Label();
            lbTemp.Text = saTemp[0];
            if (
                   title == "labelError"
                || title == "labelInstructions"
                || title == "labelComment"
                || title == "labelUserQuote"
                || title == "labelTitleColor1_Large"
                || title == "labelTitleColor1_Medium"
                || title == "labelTitleColor1_Small"
                || title == "labelTitleColor2_Large"
                || title == "labelTitleColor2_Medium"
                || title == "labelTitleColor2_Small"
                )
                lbTemp.SkinID = title;

            pnCell.Controls.Add(lbTemp);
            lbTemp = null;
        }
        else if (controlNeeded == "LinkButton")
        {
            LinkButton lkTemp = new LinkButton();
            lkTemp.Text = saTemp[0];
            if (title == "linkButtonDark_LightText")
            {
                lkTemp.SkinID = title;
                lkTemp.BackColor = System.Drawing.Color.Black;
            }
            else if (title == "linkButtonHeader")
            {
            }

            pnCell.Controls.Add(lkTemp);
            lkTemp = null;
        }
        else if (controlNeeded == "ListBox")
        {
            ListBox lbxTemp = new ListBox();
            for (i = 0; i < saTemp.Length; i++)
            {
                liTemp = new ListItem();
                liTemp.Text = saTemp[i];
                liTemp.Value = saTemp[i];
                lbxTemp.Items.Add(liTemp);
            }
            if (
                    title == "listBox1"
                 || title == "listBox2"
                )
                lbxTemp.SkinID = title;

            lbxTemp.Text = saTemp[0];
            pnCell.Controls.Add(lbxTemp);
            lbxTemp = null;
        }
        else if (controlNeeded == "RadioButton")
        {
            RadioButton rbTemp = new RadioButton();
            rbTemp.Text = saTemp[0];
            pnCell.Controls.Add(rbTemp);
            rbTemp = null;
        }
        else if (controlNeeded == "RadioButtonList")
        {
            RadioButtonList rblTemp = new RadioButtonList();
            for (i = 0; i < saTemp.Length; i++)
            {
                liTemp = new ListItem();
                liTemp.Text = saTemp[i];
                liTemp.Value = saTemp[i];
                rblTemp.Items.Add(liTemp);
                
            }
            liTemp = null;
            pnCell.Controls.Add(rblTemp);
            rblTemp = null;
        }
        else if (controlNeeded == "TextBox")
        {
            TextBox txTemp = new TextBox();
            txTemp.Text = saTemp[0];
            pnCell.Controls.Add(txTemp);
            txTemp = null;
        }

        return pnCell;
    }
    // =========================================================
    // =========================================================
}
