<%@ Page Title="Hit Count By Page" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="HitCountByPage.aspx.cs" 
    Inherits="private_siteAdministration_HitCountByPage" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <%--  --%>
        <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .LocationHeaderElements {
            display: inline-block;
            float: left;
            padding-right: 10px;
            padding-bottom: 5px;
        }
        .UpdateElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
            vertical-align: bottom;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Hit Count By Page
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32" style="margin-left: 16px;">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (IP) ======================================================================================= -->
    <asp:Panel ID="pnPag" runat="server">


        <!-- START: SMALL SCREEN TABLE (IP) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_PagSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Page</td>
                        <td><asp:Label ID="Label21" runat="server" Text='<%# Eval("Page") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Path</td>
                        <td><asp:Label ID="lbLastAccess" runat="server" Text='<%# Eval("Path") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Count</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Count") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Year</td>
                        <td><asp:Label ID="Label14" runat="server" Text='<%# Eval("Year") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Page</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Page") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Path</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Path") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Count</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Count") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Year</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("Year") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
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
        <!-- END: SMALL SCREEN TABLE (LOCATION) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->


        <asp:GridView ID="gv_PagLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Pag"
            allowPaging="true"
            OnPageIndexChanging="gvPageIndexChanging_Pag"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Page" DataField="Page" SortExpression="Page" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Path" DataField="Path" SortExpression="Path" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Count" DataField="Count" SortExpression="CountSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Year" DataField="Year" SortExpression="Year" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="spacer30"></div>
</asp:Panel><!-- END: PANEL (LOCATION) ======================================================================================= -->

    </div>

    <%-- 
        --%>

        <%--  --%>
</div>
</asp:Content>

