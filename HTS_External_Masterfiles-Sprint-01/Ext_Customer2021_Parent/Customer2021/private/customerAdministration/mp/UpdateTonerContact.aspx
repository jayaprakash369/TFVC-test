<%@ Page Title="Update Toner Contact" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="UpdateTonerContact.aspx.cs" 
    Inherits="private_customerAdministration_mp_UpdateTonerContact" %>
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
    Update Toner Contact
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-16" style="margin-left: 16px;">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (CONTACT) ======================================================================================= -->
    <asp:Panel ID="pnContact" runat="server">
    
        <!-- START: SEARCH PANEL (CONTACT) ======================================================================================= -->
        <asp:Panel ID="pnContactSearch" runat="server" DefaultButton="btContactSearchSubmit">
            
            <h3 class="titlePrimary">Select Contact</h3>
            <table class="tableBorderBackgroundLight" style="margin-bottom: 10px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Contact Name<br />
                    <asp:TextBox ID="txSearchContact" runat="server" Width="175" />
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
                    Serial<br />
                        <asp:TextBox ID="txSearchSerial" runat="server" Width="200" />
                </div>
                <div class="SearchPanelElements">
                    Asset<br />
                        <asp:TextBox ID="txSearchAsset" runat="server" Width="175" />
                </div>
                <div class="SearchPanelElements">
                    Email<br />
                        <asp:TextBox ID="txSearchEmail" runat="server" Width="230" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btContactSearchSubmit" runat="server" Text="Search" OnClick="btContactSearchSubmit_Click" SkinId="buttonPrimary" />
                    &nbsp;
                    <asp:Button ID="btContactSearchClear" runat="server" Text="Clear" OnClick="btContactSearchClear_Click" />
                </div>
            </td>
                </tr>
            </table>
        </asp:Panel>


   <asp:Panel ID="pnUpdateMachineContact" runat="server" Visible="false">
            
    <h4 class="titlePrimary" style="font-style: italic;">Update Contact For Machine</h4>
    <table class="tableWithoutLines tableBorderBackgroundLight">
        <tr>
            <td>
                <asp:Label ID="lbUpdateMachineContactHeader" runat="server" SkinId="labelMessage" />
            </td>
        </tr>
        <tr>
            <td>
                <div class="UpdateElements">
                    Contact Name<br />
                    <asp:TextBox ID="txUpdateMachineContactName" runat="server" MaxLength="30" Width="200" />
                </div>
                <div class="UpdateElements">
                    Title<br />
                    <asp:TextBox ID="txUpdateMachineContactTitle" runat="server" MaxLength="30" Width="200" />
                </div>
                <div class="UpdateElements">
                    Phone<br />
                    <asp:TextBox ID="txUpdateMachineContactPhone" runat="server" MaxLength="20" Width="100" />
                </div>
                <div class="UpdateElements">
                    Ext<br />
                    <asp:TextBox ID="txUpdateMachineContactExtension" runat="server" MaxLength="8" Width="50" />
                </div>
                <div class="UpdateElements">
                    Email<br />
                    <asp:TextBox ID="txUpdateMachineContactEmail" runat="server" MaxLength="50" Width="230" />
                </div>
                <div class="UpdateElements">
                    <br />
                    <asp:Button ID="btUpdateMachineContact" runat="server" 
                        Text="Update" 
                        OnClick="btUpdateMachineContact_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btUpdateMachineContactClose" runat="server" 
                        Text="Close?" 
                        Font-Size="9" 
                        Font-Italic="true"
                        OnClick="btUpdateMachineContactClose_Click" />

                        <asp:HiddenField ID="hfUpdateMachineContact_Cs1Cs2SerUntConFxa" runat="server" />

                </div>
            </td>
        </tr>
    </table>
            <div class="spacer15"></div>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (CONTACT) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_ContactSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Contact</td>
                        <td>
                            <asp:LinkButton ID="lkLoadContactForEditA" runat="server" 
                                OnClick="lkLoadContactForEdit_Click" 
                                Text='<%# Eval("ContactName") %>'
                                CommandArgument='<%# Eval("CustomerNumber") + "|" + Eval("CustomerLocation") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ContactName") + "|" + Eval("Title") + "|" + Eval("Phone") + "|" + Eval("Extension") + "|" + Eval("Email") + "|" + Eval("Asset") + "|x" %>'>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Title</td>
                        <td><asp:Label ID="lbContactTitleA" runat="server" Text='<% #Eval("Title") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbContactCustomerNumberA" runat="server" Text='<%# Eval("CustomerNumber") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td>
                            <asp:Label ID="lbContactCustomerLocationA" runat="server" Text='<%# Eval("CustomerLocation") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Asset</td>
                        <td>
                            <asp:Label ID="lbContactAssetA" runat="server" Text='<%# Eval("Asset") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="lbContactSerialA" runat="server" Text='<%# Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Machine Id</td>
                        <td>
                            <asp:Label ID="lbContactUnitA" runat="server" Text='<%# Eval("Unit") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Phone</td>
                        <td><asp:Label ID="lbContactPhoneA" runat="server" Text='<% #Eval("PhoneDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Extension</td>
                        <td><asp:Label ID="lbContactExtensionA" runat="server" Text='<% #Eval("Extension") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Email</td>
                        <td><asp:Label ID="lbContactEmailA" runat="server" Text='<% #Eval("Email") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Contact</td>
                        <td>
                            <asp:LinkButton ID="lkLoadContactForEditB" runat="server" 
                                OnClick="lkLoadContactForEdit_Click" 
                                Text='<%# Eval("ContactName") %>'
                                CommandArgument='<%# Eval("CustomerNumber") + "|" + Eval("CustomerLocation") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ContactName") + "|" + Eval("Title") + "|" + Eval("Phone") + "|" + Eval("Extension") + "|" + Eval("Email") + "|" + Eval("Asset") + "|x" %>'>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Title</td>
                        <td>
                            <asp:Label ID="lbContactTitleB" runat="server" Text='<%# Eval("Title") %>' />
                        </td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbContactCustomerNumberB" runat="server" Text='<%# Eval("CustomerNumber") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td>
                            <asp:Label ID="lbContactCustomerLocationB" runat="server" Text='<%# Eval("CustomerLocation") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Asset</td>
                        <td>
                            <asp:Label ID="lbContactAssetB" runat="server" Text='<%# Eval("Asset") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="lbContactSerialB" runat="server" Text='<%# Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Machine Id</td>
                        <td>
                            <asp:Label ID="lbContactUnitB" runat="server" Text='<%# Eval("Unit") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Phone</td>
                        <td><asp:Label ID="lbContactPhoneB" runat="server" Text='<% #Eval("PhoneDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Extension</td>
                        <td><asp:Label ID="lbContactExtensionB" runat="server" Text='<% #Eval("Extension") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Email</td>
                        <td><asp:Label ID="lbContactEmailB" runat="server" Text='<% #Eval("Email") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (CONTACT) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (CONTACT) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->


        <asp:GridView ID="gv_ContactLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Con"
            allowPaging="true"
            OnPageIndexChanging="gvPageIndexChanging_Con"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="Contact" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkLoadContactForEdit" runat="server" 
                        OnClick="lkLoadContactForEdit_Click" 
                        Text='<%# Eval("ContactName") %>'
                        CommandArgument='<%# Eval("CustomerNumber") + "|" + Eval("CustomerLocation") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("ContactName") + "|" + Eval("Title") + "|" + Eval("Phone") + "|" + Eval("Extension") + "|" + Eval("Email") + "|" + Eval("Asset") + "|x" %>'>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Title" DataField="Title" SortExpression="Title" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Customer" DataField="CustomerNumber" SortExpression="CustomerNumber" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CustomerLocation" SortExpression="CustomerLocation" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Asset" DataField="Asset" SortExpression="Asset" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Phone" DataField="PhoneDisplay" SortExpression="Phone" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Ext" DataField="Extension" SortExpression="Extension" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Email" DataField="Email" SortExpression="Email" ItemStyle-HorizontalAlign="Left" />
        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (CONTACT) ======================================================================================= -->

</asp:Panel><!-- END: PANEL (CONTACT) ======================================================================================= -->

    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfPreferenceToAllowLocationCrossRefUpdate" runat="server" />

        <%--  --%>
</div>
</asp:Content>

