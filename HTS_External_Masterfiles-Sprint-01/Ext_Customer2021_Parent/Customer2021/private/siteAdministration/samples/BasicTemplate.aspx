<%@ Page Title="Basic Template" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="BasicTemplate.aspx.cs" 
    Inherits="private_siteAdministration_samples_BasicTemplate" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Basic Template
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

<div class="w3-container w3-padding-32">

    <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
    <!-- START: PANEL (INVOICE) ======================================================================================= -->
    <asp:Panel ID="pnBasic" runat="server">

        <!-- START: SEARCH PANEL (INVOICE) ======================================================================================= -->
        <asp:Panel ID="pnBasicSearch" runat="server" DefaultButton="btBasicSearchSubmit">
            
            <table class="tableBorderBackgroundLight" style="margin-bottom: 10px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                        <div class="SearchPanelElements">
                            City<br />
                            <asp:TextBox ID="txSearchCity" runat="server" Width="100" />
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btBasicSearchSubmit" runat="server" Text="Search" SkinId="buttonPrimary" OnClick="btBasicSearchSubmit_Click" />
                            &nbsp;
                            &nbsp;
                            <asp:Button ID="btBasicSearchClear" runat="server" Text="Clear" OnClick="btBasicSearchClear_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->

            <asp:Repeater ID="rp_BasicSmall" runat="server">
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
                        <td><asp:Label ID="lbName" runat="server" Text='<% #Eval("CustomerName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Number</td>
                        <td><asp:Label ID="lbNumber" runat="server" Text='<% #Eval("CustomerNumber") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="lbLocation" runat="server" Text='<% #Eval("CustomerLocation") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Address</td>
                        <td><asp:Label ID="lbAddress1" runat="server" Text='<% #Eval("Address1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("CustomerName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Number</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("CustomerNumber") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("CustomerLocation") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Address</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("Address1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="Label6" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<% #Eval("State") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (INVOICE) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (INVOICE) ======================================================================================= -->
        <div class="w3-hide-small">

        <asp:GridView ID="gv_BasicLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Bas"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <%--
             <asp:TemplateField HeaderText="Basic" SortExpression="BasicSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkBasic" runat="server" 
                        OnClick="lkBasic_Click" 
                        CommandArgument='<%# Eval("Basic") + "|" + Eval("Source") %>'
                        Text='<%# Eval("Basic")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            --%>
            <asp:BoundField HeaderText="Name" DataField="CustomerName" SortExpression="CustomerName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Number" DataField="CustomerNumber" SortExpression="CustomerNumberSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CustomerLocation" SortExpression="CustomerLocationSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Address" DataField="Address1" SortExpression="Address1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>

            <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (INVOICES) ======================================================================================= -->

</asp:Panel><!-- END: PANEL (INVOICES) ======================================================================================= -->

        

    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    
    <%--  --%>
</div>
</asp:Content>

