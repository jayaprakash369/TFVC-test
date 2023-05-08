<%@ Page Title="Outstanding Equipment" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="OutstandingEquipment.aspx.cs" 
    Inherits="private_sc_OutstandingEquipment" %>
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
    Outstanding Equipment Details
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">


    <div class="w3-row w3-padding-32" style="margin-left: 16px; position: relative; top:-10px;">
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />

        <%--  --%>

    <!-- START: PANEL (EQUIPMENT) ======================================================================================= -->
    <asp:Panel ID="pnEquipment" runat="server">
    
        <!-- START: PANEL SEARCH (EQUIPMENT) ======================================================================================= -->
        <asp:Panel ID="pnEquipmentSearch" runat="server" DefaultButton="btEquipmentSearchSubmit">

            <asp:Panel ID="pnLocationHeader" runat="server" Visible="false">
                <table style="margin-bottom: 10px;">
                    <tr style="vertical-align: bottom;">
                        <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                            <asp:Label ID="lbLocationName" runat="server" Font-Bold="true" Font-Size="Large"  />
                        </td>
                    </tr>
                    <tr style="vertical-align: bottom;">
                        <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                            <div class="LocationHeaderElements">
                                <asp:Label ID="lbLocationAddress" runat="server"  />
                            </div>
                            <div class="LocationHeaderElements">
                                Customer: <asp:Label ID="lbLocationId" runat="server"  />
                            </div>
                            <div class="LocationHeaderElements">
                                Contact: <asp:Label ID="lbLocationContact" runat="server"  />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <h3 class="titleSecondary">
                <asp:Label ID="lbEquipmentSelectionTitle" runat="server" /></h3>
            <table class="tableBorderBackgroundLight" style="max-width: 800px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                 <div class="SearchPanelElements">
                    Installed Part<br />
                    <asp:TextBox ID="txSearchModel" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    City<br />
                    <asp:TextBox ID="txSearchModelDescription" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    State<br />
                    <asp:TextBox ID="txSearchSerial" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    PO #<br />
                    <asp:TextBox ID="txSearchXref" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Tracking<br />
                    <asp:TextBox ID="txSearchTracking" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btEquipmentSearchSubmit" runat="server" Text="Search" OnClick="btEquipmentSearchSubmit_Click" SkinID="buttonPrimary" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btEquipmentSearchClear" runat="server" Text="Clear" OnClick="btEquipmentSearchClear_Click" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btEquipmentExcelSubmit" runat="server" Text="Excel" OnClick="btEquipmentExcelSubmit_Click" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>
        <!-- END: PANEL SEARCH (EQUIPMENT) ======================================================================================= -->

        <!-- START: PANEL (Outstanding Equipment) ======================================================================================= -->
  <asp:Panel ID="pnOutstanding" runat="server" visible="false">
        <!-- START: SMALL SCREEN TABLE (EQUIPMENT)  ======= <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("SupTrkDsp") %>' /></td>================================================================================ -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_EquipmentSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                      <tr class="trColorReg">
                        <td>Cinemark PO</td>
                        <td>
                            <asp:Label ID="lbXref" runat="server" Text='<% #Eval("XrefDsp") %>' />
                        </td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("LocationDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("CityDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td>
                            <asp:Label ID="lbState" runat="server" Text='<% #Eval("StateDsp") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Installed Part</td>
                        <td><asp:Label ID="lbPartInst" runat="server" Text='<% #Eval("PartDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescr" runat="server" Text='<% #Eval("DescDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket</td>
                        <td><asp:Label ID="lbTicket" runat="server" Text='<% #Eval("TicketDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ship Date</td>
                        <td><asp:Label ID="lbShipped" runat="server" Text='<% #Eval("ShipDsp") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Tracking</td>
                          <td><asp:HyperLink ID="HyperLink2" runat="server" Text='<%# Eval("SupTrkDsp") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("SupTrkDsp") + "&cntry_code=us" %>'/></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                   <tr class="trColorReg">
                        <td>Cross Ref/Po#</td>
                        <td>
                            <asp:Label ID="lbXref" runat="server" Text='<% #Eval("XrefDsp") %>' />
                        </td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("LocationDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("CityDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td>
                            <asp:Label ID="lbState" runat="server" Text='<% #Eval("StateDsp") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Installed Part</td>
                        <td><asp:Label ID="lbPartInst" runat="server" Text='<% #Eval("PartDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescr" runat="server" Text='<% #Eval("DescDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket</td>
                        <td><asp:Label ID="lbTicket" runat="server" Text='<% #Eval("TicketDsp") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ship Date</td>
                        <td><asp:Label ID="lbShipped" runat="server" Text='<% #Eval("ShipDsp") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Tracking</td>
                        <td><asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("SupTrkDsp") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("SupTrkDsp") + "&cntry_code=us" %>'/></td>
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
        <!-- END: SMALL SCREEN TABLE (EQUIPMENT) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (EQUIPMENT) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->

        <asp:GridView ID="gv_EquipmentLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Eqp"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Cinemark PO" DataField="XrefDsp" SortExpression="XrefDsp" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Location" DataField="LocationDsp" SortExpression="LocationDsp" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CityDsp" SortExpression="CityDsp" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="StateDsp" SortExpression="StateDsp" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Installed Part" DataField="PartDsp" SortExpression="PartDsp" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Description" DataField="DescDsp" SortExpression="DescrDsp" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Ticket" DataField="TicketDsp" SortExpression="TicketSort" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Date Shipped" DataField="ShipDsp" SortExpression="ShipSort" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Tracking" SortExpression="SupTrkDsp" >
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink3" runat="server" 
                        Text='<%# Eval("SupTrkDsp") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("SupTrkDsp") + "&cntry_code=us" %>'>                         
                    </asp:HyperLink>
                </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
            <!-- -->
          
    </div>
        <!-- END: LARGE SCREEN TABLE (EQUIPMENT) ======================================================================================= -->
    </asp:Panel> <!-- END PANEL (Outstanding EQUIPMENT) == --->

</asp:Panel><!-- END PANEL (EQUIPMENT) ======================================================================================= -->

        <%--  --%>
        <div class="spacer30"></div>
    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfPassedCs1" runat="server" />
    <asp:HiddenField ID="hfPassedCs2" runat="server" />
    <asp:HiddenField ID="hfPassedAgr" runat="server" />
    <asp:HiddenField ID="hfPassedSrc" runat="server" />

</div>
</asp:Content>

