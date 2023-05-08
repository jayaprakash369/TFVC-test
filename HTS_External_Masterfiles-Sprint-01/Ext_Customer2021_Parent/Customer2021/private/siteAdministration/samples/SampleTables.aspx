<%@ Page Title="Sample Tables" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="SampleTables.aspx.cs" 
    Inherits="private_siteAdministration_samples_SampleTables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Sample Tables
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">
    <!-- ====================================================================================== -->


                        <!-- class="tableWithLines" style="border-left: 1px solid #333333; border-right: 1px solid #333333; width: 96%;" 
                        
                        <td><asp:Label ID="Label1" runat="server" Text=' < % # Eval("WEERR") % > ' /></td>
                        -->

    <style type="text/css">
        .demotable {
            border-collapse: collapse;
        }
        .demotable tr {
            vertical-align: top;
        }
        .demotable th {
            background-color: cornsilk;
            color: #333333;
            border: 1px solid #444444;
            padding: 3px;
        }
        .demotable td {
            min-width: 100px;
            border: 1px solid #aaaaaa;
            padding: 3px;
        }

    </style>

        <div class="w3-row w3-padding-32">
        <div class=" w3-container">

    <!--<div class="w3-twothird w3-container">-->
        <div class="w3-container">
      <!-- "w3-text-teal" -->
     <!-- <h1 class="w3-text-steel-blue">Responsive Table Testing</h1> -->
            <table class="w3-hide-medium w3-hide-large demotable" style="border: 1px solid #999999">
                <tr>
                    <th>Small Table</th>
                    <th>B</th>
                    <th>C</th>
                </tr>
                <tr>
                    <td>AA</td>
                    <td>BB</td>
                    <td>CC</td>
                </tr>
            </table>

            <table class="w3-hide-small w3-hide-large demotable" style="border: 1px solid #999999">
                <tr>
                    <th>Medium Table</th>
                    <th>B</th>
                    <th>C</th>
                    <th>D</th>
                    <th>E</th>
                    <th>F</th>
                </tr>
                <tr>
                    <td>AA</td>
                    <td>BB</td>
                    <td>CC</td>
                    <td>DD</td>
                    <td>EE</td>
                    <td>FF</td>
                </tr>
            </table>

            <table class="w3-hide-small w3-hide-medium demotable" style="border: 1px solid #999999">
                <tr>
                    <th>Large Table</th>
                    <th>B</th>
                    <th>C</th>
                    <th>D</th>
                    <th>E</th>
                    <th>F</th>
                    <th>G</th>
                    <th>H</th>
                    <th>I</th>
                </tr>
                <tr>
                    <td>AA</td>
                    <td>BB</td>
                    <td>CC</td>
                    <td>DD</td>
                    <td>EE</td>
                    <td>FF</td>
                    <td>GG</td>
                    <td>HH</td>
                    <td>II</td>
                </tr>
            </table>
<br />

        <!-- ======================================================================================= -->

        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_Small" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Field A</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("AA")%>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Field B</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("BB")%>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Field C</td><td><asp:Label ID="Label3" runat="server" Text='<% #Eval("CC")%>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Field D</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("DD")%>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Field A</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("AA")%>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Field B</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("BB")%>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Field C</td><td><asp:Label ID="Label3" runat="server" Text='<% #Eval("CC")%>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Field D</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("DD")%>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <!-- -->
        </div>
        <!-- ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
        <asp:Repeater ID="rp_Large" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="width:100%;">
                <tr>
                    <th>Col 1</th>
                    <th>Col 2</th>
                    <th>Col 3</th>
                    <th>Col 4</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr  class="trColorReg" style="vertical-align: top;">
                <td><asp:Label ID="lbA" runat="server" Text='<% #Eval("AA")%>' /></td>
                <td><asp:Label ID="lbB" runat="server" Text='<% #Eval("BB")%>' /></td>
                <td><asp:Label ID="lbC" runat="server" Text='<% #Eval("CC")%>' /></td>
                <td><asp:Label ID="lbD" runat="server" Text='<% #Eval("DD")%>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt" style="vertical-align: top;">
                <td><asp:Label ID="lbA" runat="server" Text='<% #Eval("AA")%>' /></td>
                <td><asp:Label ID="lbB" runat="server" Text='<% #Eval("BB")%>' /></td>
                <td><asp:Label ID="lbC" runat="server" Text='<% #Eval("CC")%>' /></td>
                <td><asp:Label ID="lbD" runat="server" Text='<% #Eval("DD")%>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
            <!-- -->
        </div>

            <!-- ============================================================================================================= -->
    </div>
    <!--<div class="w3-third w3-container">
    </div> -->
  </div>
  </div>

    <!-- ====================================================================================== -->
</div>
</asp:Content>
