<%@ Page Title="Agreement Equipment Locations" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="AgreementLocations.aspx.cs" 
    Inherits="private_sc_AgreementLocations" %>
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

    <div class="w3-row w3-padding-16" style="margin-left: 16px;">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (LOCATION) ======================================================================================= -->
    <asp:Panel ID="pnLocation" runat="server">
    
        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnLocationSearch" runat="server" DefaultButton="btSearchLocationSubmit">
            
            <h3 class="titlePrimary">Select Location</h3>
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Name<br />
                    <asp:TextBox ID="txSearchName" runat="server" Width="175" />
                </div>
                <asp:Panel ID="pnSearchCustomerFamily" runat="server">
                    <div class="SearchPanelElements">
                        Customer<br />
                        <asp:DropDownList ID="ddSearchCustomerFamily" runat="server" />
                    </div>
                </asp:Panel>
                <div class="SearchPanelElements">
                    Location<br />
                        <asp:TextBox ID="txSearchLocation" runat="server" Width="40" />
                </div>
                <div class="SearchPanelElements">
                    Cust Loc Xref<br />
                        <asp:TextBox ID="txSearchXref" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Address<br />
                        <asp:TextBox ID="txSearchAddress" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    City<br />
                        <asp:TextBox ID="txSearchCity" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    State<br />
                        <asp:TextBox ID="txSearchState" runat="server" Width="30" />
                </div>
                <div class="SearchPanelElements">
                    Zip<br />
                        <asp:TextBox ID="txSearchZip" runat="server" Width="70" />
                </div>
                <div class="SearchPanelElements">
                    Phone<br />
                        <asp:TextBox ID="txSearchPhone" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    Contact<br />
                        <asp:TextBox ID="txSearchContact" runat="server" Width="175" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchLocationSubmit" runat="server" Text="Search" OnClick="btSearchLocationSubmit_Click" SkinID="buttonPrimary" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchLocationClear" runat="server" Text="Clear" OnClick="btSearchLocationClear_Click" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>


   <asp:Panel ID="pnUpdateLocationXref" runat="server" Visible="false">
            
    <h4 class="titlePrimary" style="font-style: italic;">Update Location Cross Ref</h4>
    <table class="tableWithoutLines tableBorderBackgroundLight">
        <tr>
            <td>
                <div class="UpdateElements">
                    Customer<br />
                    <asp:Label ID="lbUpdateLocationXref_Customer" runat="server" Text="" />        
                </div>
                <div class="UpdateElements">
                    Updated Location Cross Ref<br />
                    <asp:TextBox ID="txUpdateLocationXref_Xref" runat="server" MaxLength="15" />
                </div>
                <div class="UpdateElements">
                    <br />
                    <asp:Button ID="btUpdateLocationXref" runat="server" 
                        Text="Update" 
                        OnClick="btUpdateLocationXref_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btUpdateLocationXrefClose" runat="server" 
                        Text="Close?" 
                        Font-Size="9" 
                        Font-Italic="true"
                        OnClick="btUpdateLocationXrefClose_Click" />

                        <asp:HiddenField ID="hfUpdateLocationXref_Cs1Cs2" runat="server" />

                </div>
            </td>
        </tr>
    </table>
            <div class="spacer15"></div>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_LocationSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>

                    <tr class="trColorReg">
                        <td>Name</td>
                        <td>
                            <asp:HiddenField ID="hfCompanySegment" runat="server" Visible="false" Value='<%# Eval("FLAG8") %>' />
                            <asp:HiddenField ID="hfB1EqpCount" runat="server" Visible="false" Value='<%# Eval("B1EqpCount") %>' />
                            <asp:Label ID="lbLocationName" runat="server" Text='<%# Eval("CUSTNM") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Qty</td>
                        <td>
                            <asp:LinkButton ID="lkEquipCount" runat="server" OnClick="lkEquipCount_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>
                                <%# Eval("CombinedEqpCount") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("CSTRNR") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("CSTRCD") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>CustXref</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("XREFCS") %>' /></td>
                    </tr>
                    <asp:Panel ID="pnEditXref" runat="server">
                    <tr class="trColorReg">
                        <td>Edit Xref</td>
                        <td>
                            <asp:LinkButton ID="lkLoadLocationXrefForEdit" runat="server" 
                                OnClick="lkLoadLocationXrefForEdit_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") + "|" + Eval("XREFCS") %>'>
                                <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr class="trColorReg">
                        <td>Address</td>
                        <td><asp:Label ID="Label9" runat="server" Text='<% #Eval("SADDR1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="Label10" runat="server" Text='<% #Eval("CITY") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="Label11" runat="server" Text='<% #Eval("STATE") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Zip</td>
                        <td><asp:Label ID="Label12" runat="server" Text='<% #Eval("ZIPCD") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Phone</td>
                        <td><asp:Label ID="Label13" runat="server" Text='<% #Eval("HPHONE") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Contact</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("CONTNM") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td>
                            <asp:HiddenField ID="hfCompanySegment" runat="server" Visible="false" Value='<%# Eval("FLAG8") %>' />
                            <asp:HiddenField ID="hfB1EqpCount" runat="server" Visible="false" Value='<%# Eval("B1EqpCount") %>' />
                            <asp:Label ID="lbLocationName" runat="server" Text='<% #Eval("CUSTNM") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Qty</td>
                        <td>
                            <asp:LinkButton ID="lkEquipCount" runat="server" OnClick="lkEquipCount_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>
                                <%# Eval("CombinedEqpCount") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td><asp:Label ID="Label6" runat="server" Text='<% #Eval("CSTRNR") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<% #Eval("CSTRCD") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>CustXref</td>
                        <td><asp:Label ID="Label8" runat="server" Text='<% #Eval("XREFCS") %>' /></td>
                    </tr>
                    <asp:Panel ID="pnEditXref" runat="server">
                    <tr class="trColorAlt">
                        <td>Edit Xref</td>
                        <td>
                            <asp:LinkButton ID="lkLoadLocationXrefForEdit" runat="server" 
                                OnClick="lkLoadLocationXrefForEdit_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") + "|" + Eval("XREFCS") %>'>
                                <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr class="trColorAlt">
                        <td>Address</td>
                        <td><asp:Label ID="Label15" runat="server" Text='<% #Eval("SADDR1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="Label16" runat="server" Text='<% #Eval("CITY") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="Label17" runat="server" Text='<% #Eval("STATE") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Zip</td>
                        <td><asp:Label ID="Label18" runat="server" Text='<% #Eval("ZIPCD") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Phone</td>
                        <td><asp:Label ID="Label19" runat="server" Text='<% #Eval("HPHONE") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Contact</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("CONTNM") %>' /></td>
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


        <asp:GridView ID="gv_LocationLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Loc"
            allowPaging="true"
            OnPageIndexChanging="gvPageIndexChanging_Loc"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="Name" SortExpression="CUSTNM" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="hfCompanySegment" runat="server" Visible="false" Value='<%# Eval("FLAG8") %>' />
                    <asp:HiddenField ID="hfB1EqpCount" runat="server" Visible="false" Value='<%# Eval("B1EqpCount") %>' />
                    <asp:Label ID="lbLocationName" runat="server" Text='<%# Eval("CUSTNM") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Qty" SortExpression="CombinedEqpCountSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkEquipCount" runat="server" 
                        OnClick="lkEquipCount_Click" 
                        CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") %>'
                        Text='<%# Eval("CombinedEqpCount")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Customer" DataField="CSTRNR" SortExpression="CSTRNR" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Location" DataField="CSTRCD" SortExpression="CSTRCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="CustLocXref" DataField="XREFCS" SortExpression="XREFCS" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="EditXref" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkLoadLocationXrefForEdit" runat="server" 
                        OnClick="lkLoadLocationXrefForEdit_Click" 
                        CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") + "|" + Eval("XREFCS") %>'>
                        <asp:Image ID="imEquipXrefEdit" runat="server" ImageUrl="~/media/images/button_edit.png" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Address" DataField="SADDR1" SortExpression="SADDR1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CITY" SortExpression="CITY" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="STATE" SortExpression="STATE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Zip" DataField="ZIPCD" SortExpression="ZIPCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="HPHONE" SortExpression="HPHONE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Contact" DataField="CONTNM" SortExpression="CONTNM" ItemStyle-HorizontalAlign="Left" />

        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="spacer30"></div>
</asp:Panel><!-- END: PANEL (LOCATION) ======================================================================================= -->

    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfPreferenceToAllowLocationCrossRefUpdate" runat="server" />
    <%-- 
    <asp:HiddenField ID="hfChosenCs1" runat="server" />
    <asp:HiddenField ID="hfChosenCs2" runat="server" />
        --%>

        <%--  --%>
</div>
</asp:Content>

