<%@ Page Title="Email Management" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="EmailManagement.aspx.cs" 
    Inherits="private_sc_EmailManagement" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
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

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Email Management
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">
    
    <!--
        <div class="w3-container" style="margin-top: 32px; border: 1px solid #ad0034;">

    <div class="w3-row w3-padding-16" style="border: 1px solid #ad0034;">
        <div class="w3-third w3-container">
            AAA
        </div>
        <div class="w3-twothird w3-container">
            BBB
        </div>
    </div>

    <div class="w3-row w3-padding-16" style="border: 1px solid #ad0034;">
        <div class="w3-third w3-container">
            CCC
        </div>
        <div class="w3-twothird w3-container">
            DDD
            <p class="w3-border w3-padding-large w3-padding-32 w3-center">EEE</p>
            <p class="w3-border w3-padding-large w3-padding-32 w3-center">FFF</p>
        </div>
    </div>
         style="border: 1px solid #406080;"
    -->
    <div class="w3-container" style="margin-top: 32px;">

        <div class="w3-row" style="border-bottom: 1px solid #dddddd;">
            <div class="w3-third w3-container">
                <asp:Label ID="lbTogglePanels" runat="server" CssClass="titleSecondary" />
            </div>
            <div class="w3-twothird w3-container">
               <div class="SearchPanelElements">Switch to manage <asp:Button ID="btTogglePanels" runat="server" onClick="btTogglePanels_Click" /></div> 
               <div class="SearchPanelElements">                                                
                   <asp:RadioButtonList ID="rblUmberellaSize" runat="server" 
                        RepeatDirection="Vertical" 
                        Visible="false" 
                        SkinId="RadioButtonList1">
                        <asp:ListItem Text="All locations of ALL customer groups" Value="AllGroups" Selected="true" />
                        <asp:ListItem Text="All locations of ONE customer group" Value="OneGroup" />
                    </asp:RadioButtonList>
                </div> 
                <div class="SearchPanelElements"><asp:Label ID="lbError" runat="server" SkinId="labelError" Text="Error" /></div>
                <div class="SearchPanelElements"><asp:Label ID="lbMessage" runat="server" SkinId="labelMessage"  Text="Message" /></div>
            </div>
            <div class="w3-third w3-container">
                
            </div>
        </div>
<%--  --%>
        <asp:Panel ID="pnParentOrChildLocation" runat="server" Visible="false">
            <div class="w3-row w3-padding-16">
                <div class="w3-third w3-container">

                    <asp:Label ID="lbOpenCallTitle" runat="server" Text="When A Call Is OPENED: " CssClass="titlePrimary" />
                    <div class="spacer10"></div>

                    <%-- START: Open Calls: Add Email Address Table --%>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbOpen" runat="server" Text="Send an email?" />
                            </td>
                            <td style="padding-left: 15px; padding-top: 10px;">
                                <asp:RadioButtonList ID="rblOpen" runat="server" 
                                    RepeatDirection="Horizontal" 
                                    AutoPostBack="true" 
                                    SkinId="RadioButtonList1"
                                    OnSelectedIndexChanged="rblEmailSwitch_Click" >
                                    <asp:ListItem Text="Yes" Value="YES" />
                                    <asp:ListItem Text="No" Value="NO" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer10"></div>

                    <table class="tableWithoutLines tableBorder">
                        <tr>
                            <td>Add New Address?</td>
                            <td>
                                <asp:LinkButton ID="lkOpenEmailToAdd" runat="server" 
                                    Text="Add" 
                                    OnClick="lkEmailToAdd_Click" 
                                    CommandArgument="AtOpen" 
                                    />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txOpenAdd" runat="server" Width="225" MaxLength="50" />
                            </td>
                        </tr>
                    </table>
                    <div class="spacer15"></div>

                   <%-- END: Open Calls: Add Email Address Table --%>

                       <asp:GridView ID="gv_EmailsAtOpenToDelete" runat="server"
                            AutoGenerateColumns="False" 
                            CssClass="tableWithLines"
                            EmptyDataText="No matching records were found">
                            <AlternatingRowStyle CssClass="trColorAlt" />
                            <Columns>
                                <asp:BoundField HeaderText="Current OPEN Addresses" DataField="Email" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkCallOpenEmailToDelete" runat="server" 
                                            CommandArgument='<%# Eval("Email") + "|AtOpen" %>' 
                                            Text="Delete" 
                                            OnClick="lkEmailToDelete_Click" 
                                            />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    <div class="spacer30"></div>
                </div>



                <div class="w3-twothird w3-container"> 
            
                    <asp:Label ID="lbClosedCallTitle" runat="server" Text="When A Call Is CLOSED: " CssClass="titlePrimary" />
                    <div class="spacer10"></div>

                    <%-- START: Closed Calls: Add Email Address Table --%>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbClose" runat="server" Text="Send an email?" />
                            </td>
                            <td style="padding-left: 15px; padding-top: 10px;">
                               <asp:RadioButtonList ID="rblClose" runat="server" 
                                RepeatDirection="Horizontal"
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="rblEmailSwitch_Click" >
                                <asp:ListItem Text="Yes" Value="YES" />
                                <asp:ListItem Text="No" Value="NO" />
                            </asp:RadioButtonList>

                            </td>
                        </tr>
                    </table>
                    <div class="spacer10"></div>

                    <table class="tableWithoutLines tableBorder">
                        <tr>
                            <td>Add New Address?</td>
                            <td>
                                <asp:LinkButton ID="lkEmailToAddAtClose" runat="server" 
                                    Text="Add" 
                                    OnClick="lkEmailToAdd_Click" 
                                    CommandArgument="AtClose" 
                                    />

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txCloseAdd" runat="server" Width="225" MaxLength="50" />
                            </td>
                        </tr>
                    </table>
                    <div class="spacer15"></div>
                            

                            <%-- START: Close Add Email Table --%> 
                                        

                    <%-- END: Close Add Email Table --%>

                        <asp:GridView ID="gv_EmailsAtCloseToDelete" runat="server"
                            AutoGenerateColumns="False" 
                            CssClass="tableWithLines"
                            EmptyDataText="No matching records were found">
                            <AlternatingRowStyle CssClass="trColorAlt" />
                            <Columns>
                                <asp:BoundField HeaderText="Current Closed Addresses" DataField="Email" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkCallCloseEmailToDelete" runat="server" 
                                            CommandArgument='<%# Eval("Email") + "|AtClose" %>' 
                                            Text="Delete" 
                                            OnClick="lkEmailToDelete_Click" 
                                            />
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                        <div class="spacer20"></div>
                </div>
            </div>

            <div class="w3-row w3-padding-16">
                <div class="w3-third w3-container">
            
                </div>
                <div class="w3-twothird w3-container">
            
                </div>
            </div>
        </asp:Panel>
<%--  
        <asp:Panel ID="pnOneLocation" runat="server" Visible="false">
            <div class="w3-row w3-padding-16">
                <div class="w3-third w3-container">
            
                </div>
                <div class="w3-twothird w3-container">
            
                </div>
            </div>
        </asp:Panel>
--%>
<%--  --%>

        <asp:Panel ID="pnChildLocationSelection" runat="server" Visible="false">
            <div class="w3-container w3-row"> <!--  style="border: 1px solid #ad0034;" -->

        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnLocationSearch" runat="server" DefaultButton="btLocationSearchSubmit">
            
           <div class="searchPanelElements">
               <h3 class="titlePrimary">Select Location</h3>
           </div> 
           <div class="searchPanelElements">
                
           </div>
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Name<br />
                    <asp:TextBox ID="txSearchName" runat="server" Width="175" />
                </div>
                <asp:Panel ID="pnSearchCustomerFamily" runat="server" Visible="false">
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
                    Xref<br />
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
                        <asp:TextBox ID="txSearchZip" runat="server" Width="50" />
                </div>
                <div class="SearchPanelElements">
                    Phone<br />
                        <asp:TextBox ID="txSearchPhone" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Contact<br />
                        <asp:TextBox ID="txSearchContact" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btLocationSearchSubmit" runat="server" Text="Search" OnClick="btLocationSearchSubmit_Click" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
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
                            <asp:LinkButton ID="lkName" runat="server" OnClick="lkName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Qty</td>
                        <td>
                            <asp:Label ID="lbLocationName" runat="server" Text='<%# Eval("CombinedEqpCount") %>' />
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
                        <td><asp:Label ID="Label13" runat="server" Text='<% #Eval("HPHONE") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lkName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Qty</td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<% #Eval("CombinedEqpCount") %>' />
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
                    <asp:LinkButton ID="LinkButton3" runat="server" 
                        OnClick="lkName_Click" 
                        CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") %>'
                        Text='<%# Eval("CUSTNM") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Equip" SortExpression="CombinedEqpCountSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("CombinedEqpCount") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Customer" DataField="CSTRNR" SortExpression="CSTRNR" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Location" DataField="CSTRCD" SortExpression="CSTRCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="CustLocXref" DataField="XREFCS" SortExpression="XREFCS" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="SADDR1" SortExpression="SADDR1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CITY" SortExpression="CITY" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="STATE" SortExpression="STATE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Zip" DataField="ZIPCD" SortExpression="ZIPCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="HPHONE" SortExpression="HPHONE" ItemStyle-HorizontalAlign="Center" />

        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->

            </div>
        </asp:Panel>


        <%--  --%>


        <%--  --%>
    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfChosenCs1" runat="server" />
    <asp:HiddenField ID="hfChosenCs2" runat="server" />

</div>
</asp:Content>
