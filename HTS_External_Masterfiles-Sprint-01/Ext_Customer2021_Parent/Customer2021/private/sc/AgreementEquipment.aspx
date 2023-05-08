<%@ Page Title="Agreement Equipment" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="AgreementEquipment.aspx.cs" 
    Inherits="private_sc_AgreementEquipment" %>
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
    Agreement Detail
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
                        <asp:Panel ID="pnSearchProductCodes" runat="server" Visible="false">
                            <div class="SearchPanelElements">
                                Category<br />
                                <asp:DropDownList ID="ddSearchProductCodes" runat="server" />
                            </div>
                        </asp:Panel>
                <div class="SearchPanelElements">
                    Model<br />
                    <asp:TextBox ID="txSearchModel" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    Model Description<br />
                    <asp:TextBox ID="txSearchModelDescription" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    Serial<br />
                    <asp:TextBox ID="txSearchSerial" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Equip Xref<br />
                    <asp:TextBox ID="txSearchAsset" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Agent Id<br />
                    <asp:TextBox ID="txSearchAgentId" runat="server" Width="100" />
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

        <!-- START: PANEL SEARCH (EQUIPMENT) ======================================================================================= -->

        <asp:Panel ID="pnEquipXrefUpdate" runat="server" Visible="false">
            
    <h4 class="titlePrimary" style="font-style: italic;">Update Model Cross Ref</h4>
    <table class="tableWithoutLines tableBorderBackgroundLight">
        <tr>
            <td>
                <div class="UpdateElements">
                    Model<br />
                    <asp:Label ID="lbEquipXrefUpdate_Model" runat="server" Text="" />        
                </div>
                <div class="UpdateElements">
                    Serial<br />
                    <asp:Label ID="lbEquipXrefUpdate_Serial" runat="server" Text="" />
                </div>
                <div class="UpdateElements">
                    New Equipment Cross Reference<br />
                    <asp:TextBox ID="txEquipXrefUpdate_Xref" runat="server" MaxLength="15" />
                </div>
                <div class="UpdateElements">
                    <br />
                    <asp:Button ID="btEquipXrefUpdate" runat="server" 
                        Text="Update" 
                        OnClick="btEquipXrefUpdate_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btEquipXrefClose" runat="server" 
                        Text="Close?" 
                        Font-Size="9" 
                        Font-Italic="true"
                        OnClick="btEquipXrefClose_Click" />

                        <asp:HiddenField ID="hfEquipXrefUpdate_Unit" runat="server" />


                </div>
            </td>
        </tr>
    </table>
            <div class="spacer15"></div>
        </asp:Panel>


        <!-- START: SMALL SCREEN TABLE (EQUIPMENT)  ======================================================================================= -->
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
                        <td>Model</td>
                        <td>
                            <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />

                            <asp:LinkButton ID="lkModelForTicket" runat="server" OnClick="lkModelForTicket_Click" 
                                CommandArgument='<%# Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>'>
                                <%# Eval("Model") %>
                            </asp:LinkButton>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="lbSerial" runat="server" Text='<% #Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model Description</td>
                        <td><asp:Label ID="lbModelDescription" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Item Id</td>
                        <td><asp:Label ID="lbUnit" runat="server" Text='<% #Eval("Unit") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Xref</td>
                        <td><asp:Label ID="lbAsset" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <asp:Panel ID="pnEditXref" runat="server">
                    <tr class="trColorReg">
                        <td>Edit Xref</td>
                        <td>
                            <asp:LinkButton ID="lkLoadEquipXrefForEdit" runat="server" 
                                CommandArgument='<%# Eval("Model") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ModelXref")%>'  
                                OnClick="lkLoadEquipXrefForEdit_Click">
                                <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr class="trColorReg">
                        <td>AgentId</td>
                        <td><asp:Label ID="lbAgentId" runat="server" Text='<% #Eval("AgentId") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td>
                            <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
                            <asp:LinkButton ID="lkModelForTicket" runat="server" OnClick="lkModelForTicket_Click" 
                                CommandArgument='<%# Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>'>
                                <%# Eval("Model") %>
                            </asp:LinkButton>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="lbSerial" runat="server" Text='<% #Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model Description</td>
                        <td><asp:Label ID="lbModelDescription" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Item Id</td>
                        <td><asp:Label ID="lbUnit" runat="server" Text='<% #Eval("Unit") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Xref</td>
                        <td><asp:Label ID="lbAsset" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <asp:Panel ID="pnEditXref" runat="server">
                    <tr class="trColorAlt">
                        <td>Edit Xref</td>
                        <td>
                            <asp:LinkButton ID="lkLoadEquipXrefForEdit" runat="server" 
                                CommandArgument='<%# Eval("Model") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ModelXref")%>'  
                                OnClick="lkLoadEquipXrefForEdit_Click">
                                <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr class="trColorAlt">
                        <td>AgentId</td>
                        <td><asp:Label ID="lbAgentId" runat="server" Text='<% #Eval("AgentId") %>' /></td>
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
            <asp:TemplateField HeaderText="Model (Make Tck)" ItemStyle-HorizontalAlign="Left" SortExpression="Model">
                <ItemTemplate>
                    <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
                    <asp:LinkButton ID="lkModelForTicket" runat="server" 
                        OnClick="lkModelForTicket_Click" 
                        CommandArgument='<%# Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>' 
                        Text='<%# Eval("Model") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Model Description" DataField="ModelDescription" SortExpression="ModelDescription" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Item Id" DataField="Unit" SortExpression="UnitSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Equip Xref" DataField="ModelXref" SortExpression="ModelXref" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Edit Xref" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkLoadEquipXrefForEdit" runat="server" 
                        CommandArgument='<%# Eval("Model") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ModelXref")%>'  
                        OnClick="lkLoadEquipXrefForEdit_Click">
                        <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="AgentId" DataField="AgentId" SortExpression="AgentId" ItemStyle-HorizontalAlign="Left" />

        </Columns>
    </asp:GridView>
            <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (EQUIPMENT) ======================================================================================= -->

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
    <asp:HiddenField ID="hfOracleParentId" runat="server" />
    <asp:HiddenField ID="hfOracleChildId" runat="server" />
    <asp:HiddenField ID="hfPreferenceToAllowEquipmentCrossRefUpdate" runat="server" />
</div>
</asp:Content>

