<%@ Page Title="Maintenance Coverage" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="MaintenanceCoverage.aspx.cs" 
    Inherits="public_sc_MaintenanceCoverage" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
    <%--  --%>
    <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Onsite Service Coverage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

     <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">


    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>

                <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnSearch" runat="server" DefaultButton="btSearchSubmit">
            
            <table class="tableBorderBackgroundLight">
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
                            <asp:Button ID="btSearchSubmit" runat="server" Text="Search" OnClick="btSearchSubmit_Click" SkinID="buttonPrimary" />
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btSearchClear" runat="server" Text="Clear" OnClick="btSearchClear_Click" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>


            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_CoverageSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("StateName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Zip</td>
                        <td><asp:Label ID="lbZip" runat="server" Text='<% #Eval("Zip") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Service</td>
                        <td><asp:Label ID="lbService" runat="server" Text='<% #Eval("Service") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("StateName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Zip</td>
                        <td><asp:Label ID="lbZip" runat="server" Text='<% #Eval("Zip") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Service</td>
                        <td><asp:Label ID="lbService" runat="server" Text='<% #Eval("Service") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
            <asp:GridView ID="gv_CoverageLarge" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLines"
                EmptyDataText="No matching records were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                    <asp:BoundField HeaderText="City" DataField="City" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="State" DataField="StateName" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Zip" DataField="Zip" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Service" DataField="Service" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
        <%--  --%>
    </div>
</div>

<div class="spacer30"></div>
</div>
</asp:Content>
