<%@ Page Title="Change Contact Information" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ChangeContactInformation.aspx.cs" 
    Inherits="private_customerAdministration_ChangeContactInformation" %>
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
    Change Contact Information
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-16" style="margin-left: 16px;">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (LOCATION) ======================================================================================= -->
    <asp:Panel ID="pnLocation" runat="server">
    
        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnLocationSearch" runat="server" DefaultButton="btLocationSearchSubmit">
            
            <h3 class="titlePrimary">Select Location</h3>
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Customer<br />
                    <asp:TextBox ID="txSearchName" runat="server" Width="175" />
                </div>
                <div class="SearchPanelElements">
                    Contact<br />
                        <asp:TextBox ID="txSearchContact" runat="server" Width="150" />
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
                    Cust Loc Cross Ref<br />
                        <asp:TextBox ID="txSearchXref" runat="server" Width="120" />
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
                        <asp:TextBox ID="txSearchZip" runat="server" Width="80" />
                </div>
                <div class="SearchPanelElements">
                    Phone<br />
                        <asp:TextBox ID="txSearchPhone" runat="server" Width="140" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btLocationSearchSubmit" runat="server" Text="Search" OnClick="btLocationSearchSubmit_Click" SkinID="buttonPrimary" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btLocationSearchClear" runat="server" Text="Search" OnClick="btLocationSearchClear_Click" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>


   <asp:Panel ID="pnUpdateLocationContact" runat="server" Visible="false">
            
    <h4 class="titlePrimary" style="font-style: italic;">Update Contact</h4>
    <table class="tableWithoutLines tableBorderBackgroundLight">
        <tr>
            <td>
                <div class="UpdateElements">
                    Customer<br />
                    <asp:Label ID="lbUpdateLocationContact_Customer" runat="server" Text="" />        
                </div>
                <div class="UpdateElements">
                    Contact Name<br />
                    <asp:TextBox ID="txUpdateLocationContact_Name" runat="server" MaxLength="30" />
                </div>
                <div class="UpdateElements">
                    Location Primary Phone<br />
                    <asp:TextBox ID="txUpdateLocationContact_Phone" runat="server" MaxLength="15" />
                </div>
                <div class="UpdateElements">
                    <br />
                    <asp:Button ID="btUpdateLocationContact" runat="server" 
                        Text="Update" 
                        SkinID="buttonPrimary"
                        OnClick="btUpdateLocationContact_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btUpdateLocationContactClose" runat="server" 
                        Text="Close?" 
                        Font-Size="9" 
                        Font-Italic="true"
                        OnClick="btUpdateLocationContactClose_Click" />

                        <asp:HiddenField ID="hfUpdateLocationContact_Cs1Cs2" runat="server" />

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
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbLocationName" runat="server" Text='<%# Eval("CUSTNM") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Contact</td>
                        <td>
                            <asp:LinkButton ID="lkContact" runat="server" OnClick="lkLoadLocationContactForEdit_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")  + "|" + Eval("CONTNM")  + "|" + Eval("HPHONE")  %>'>
                                <%# Eval("CONTNM") %>
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
                        <td><asp:Label ID="Label13" runat="server" Text='<% #Eval("PhoneDisplay") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbLocationName" runat="server" Text='<% #Eval("CUSTNM") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Contact</td>
                        <td>
                            <asp:LinkButton ID="lkContact" runat="server" OnClick="lkLoadLocationContactForEdit_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")  + "|" + Eval("CONTNM")  + "|" + Eval("HPHONE")  %>'>
                                <%# Eval("CONTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Number</td>
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
                        <td><asp:Label ID="Label19" runat="server" Text='<% #Eval("PhoneDisplay") %>' /></td>
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
            <asp:BoundField HeaderText="Customer" DataField="CUSTNM" SortExpression="CUSTNM" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Contact" SortExpression="CONTNM" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkContact" runat="server" 
                        OnClick="lkLoadLocationContactForEdit_Click" 
                        CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")  + "|" + Eval("CONTNM")  + "|" + Eval("HPHONE") %>'
                        Text='<%# Eval("CONTNM")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Number" DataField="CSTRNR" SortExpression="CSTRNR" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CSTRCD" SortExpression="CSTRCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="CustLocXref" DataField="XREFCS" SortExpression="XREFCS" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="SADDR1" SortExpression="SADDR1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CITY" SortExpression="CITY" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="STATE" SortExpression="STATE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Zip" DataField="ZIPCD" SortExpression="ZIPCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="PhoneDisplay" SortExpression="HPHONE" ItemStyle-HorizontalAlign="Center" />

        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->

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

