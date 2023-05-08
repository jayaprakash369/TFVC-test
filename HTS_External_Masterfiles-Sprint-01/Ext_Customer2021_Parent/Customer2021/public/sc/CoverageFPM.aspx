<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CoverageFPM.aspx.cs" Inherits="public_sc_CoverageFPM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Coverage: FP Mailing</title>
        <style type="text/css">
            body {
                font-family: Verdana, Arial;
                color: #333333;
            }
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        /* TABLE: WITH LINES */
.tableWithLinesX {
    border-collapse: collapse;
}

    .tableWithLinesX tr {
        vertical-align: top;
    }

    .tableWithLinesX th {
        background-color: #bbbbbb; /* was #3A0028, 94002c*/
        color: #ffffff; /* cc0000 990000 */
        font-weight: normal;
        padding: 4px;
        border: 1px solid #555555;
    }
    /* DO NOT PUT PADDING IN TD -- PUT IN A or background will not fill! */
    .tableWithLinesX td {
        border: 1px solid #999999; /* #666666 */
        padding: 4px;
        font-size: 1.0em;
    }

        .tableWithLinesX td:hover {
        }
    /* But if you put padding in A, the rest of the fields have no padding!... */
    .tableWithLinesX a {
        text-decoration: none;
        display: block;
    }

        .tableWithLinesX a:hover {
            text-decoration: underline;
        }

    .tableWithLinesX th > a {
        color: #ffffff;
        font-weight: normal;
    }

    </style>
    <link rel="stylesheet" href="/css/w3.css" />
    <link rel="stylesheet" href="/App_Themes/Responsive/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 100%; border-bottom: 1px solid #CCCCCC;">
                <div style="padding-left: 40px; padding-top: 1px; padding-right: 40px; padding-bottom: 1px; ">
                    <!--  ImageHeight="75"  -->
                    <div style="padding-bottom: 5px;">
                        <asp:HyperLink ID="hlFpMailing" runat="server" ImageUrl="~/media/images/FpMailingLogo4.jpg" NavigateUrl="https://www.fp-usa.com" />
                    </div>
                </div>
            </div>
            <div style="padding-left: 80px; padding-right: 40px; padding-bottom: 40px; padding-top: 5px;">
        <!-- START: SEARCH PANEL ======================================================================================= -->
        <asp:Panel ID="pnSearch" runat="server" DefaultButton="btSearchSubmit">
            <div class="spacer10"></div>
            <h2>Maintenance Coverage</h2>
            <table class="tableBorder" style="border: 1px solid #cccccc;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                        <div class="SearchPanelElements">
                            City<br />
                            <asp:TextBox ID="txSearchCity" runat="server" Width="175" />
                        </div>
                        <div class="SearchPanelElements">
                            Zip<br />
                            <asp:TextBox ID="txSearchZip" runat="server" Width="75" />
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btSearchClear" runat="server" Text="Clear" OnClick="btSearchClear_Click" CssClass="button1" />
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btSearchSubmit" runat="server" Text="Search" OnClick="btSearchSubmit_Click" CssClass="button1" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="spacer20"></div>
        </asp:Panel>
        <!-- END: SEARCH PANEL ======================================================================================= -->

            <asp:GridView ID="gv_Coverage" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLinesX"
                EmptyDataText="No matching records were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                    <asp:BoundField HeaderText="City" DataField="City" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="State" DataField="StateName" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Zip" DataField="Zip" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Zone" DataField="Zone" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Service" DataField="Service" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
