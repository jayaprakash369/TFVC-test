<%@ Page Title="Service History" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ServiceHistory.aspx.cs" 
    Inherits="private_sc_ServiceHistory" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
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
    Service History
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-16">

        <%--  --%>
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
    <!-- START: PANEL (LOCATION) ======================================================================================= -->
    <asp:Panel ID="pnLocation" runat="server">

        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnSearchLocation" runat="server" DefaultButton="btSearchLocationSubmit">
            
            <h3 class="titlePrimary">Select Location</h3>
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Name<br />
                    <asp:TextBox ID="txSearchLocationName" runat="server" Width="150" />
                </div>
                <asp:Panel ID="pnSearchCustomerFamily" runat="server">
                    <div class="SearchPanelElements">
                        Customer<br />
                        <asp:DropDownList ID="ddSearchCustomerFamily" runat="server" />
                    </div>
                </asp:Panel>
                <div class="SearchPanelElements">
                    Location<br />
                        <asp:TextBox ID="txSearchLocationNum" runat="server" Width="40" />
                </div>
                <div class="SearchPanelElements">
                    CustXref<br />
                        <asp:TextBox ID="txSearchLocationCustXref" runat="server" Width="90" />
                </div>
                <div class="SearchPanelElements">
                    Address<br />
                        <asp:TextBox ID="txSearchLocationAddress" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    City<br />
                        <asp:TextBox ID="txSearchLocationCity" runat="server" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    State<br />
                        <asp:TextBox ID="txSearchLocationState" runat="server" Width="25" />
                </div>
                <div class="SearchPanelElements">
                    Zip<br />
                        <asp:TextBox ID="txSearchLocationZip" runat="server" Width="50" />
                </div>
                <div class="SearchPanelElements">
                    Phone<br />
                        <asp:TextBox ID="txSearchLocationPhone" runat="server" Width="90" />
                </div>
                <div class="SearchPanelElements">
                    <div style="display:block; float:left; padding-right: 15px;">
                    <br />
                    <asp:Button ID="btSearchLocationSubmit" runat="server" Text="Search" OnClick="btSearchLocationSubmit_Click"  SkinID="buttonPrimary" />
                    </div>
                    <div style="display:block; float:left; padding-right: 15px;">
                    <br />
                    <asp:Button ID="BtSearchLocationClear" runat="server" Text="Clear" OnClick="btSearchLocationClear_Click" />
                    </div>
                    <div style="display:block; float:left">
                    <br />
                    <asp:Button ID="btSearchUseAllLocations" runat="server" Text="...Just Use All Locations" OnClick="btSearchUseAllLocations_Click" />
                    </div>
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
                            <asp:LinkButton ID="lkLocationName" runat="server" OnClick="lkLocationName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") + "|" + Eval("CUSTNM") %>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Eqp</td>
                        <td><asp:Label ID="Label14" runat="server" Text='<% #Eval("CombinedEqpCount") %>' /></td>
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
                        <td><asp:Label ID="Label13" runat="server" Text='<% #Eval("PhoneFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td>
                            <asp:LinkButton ID="lkLocationName" runat="server" OnClick="lkLocationName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")  + "|" + Eval("CUSTNM")  %>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Eqp</td>
                        <td><asp:Label ID="Label20" runat="server" Text='<% #Eval("CombinedEqpCount") %>' /></td>
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
                        <td><asp:Label ID="Label19" runat="server" Text='<% #Eval("PhoneFormatted") %>' /></td>
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
            AllowPaging="true" 
            OnPageIndexChanging="gvPageIndexChanging_Loc"
            EmptyDataText="No matching locations found">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="Name" SortExpression="CUSTNM" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkLocationName" runat="server" 
                            OnClick="lkLocationName_Click" 
                            CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")  + "|" + Eval("CUSTNM") %>'
                            Text='<%# Eval("CUSTNM") %>'
                             />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:BoundField HeaderText="Equip" DataField="CombinedEqpCount" SortExpression="CombinedEqpCountSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Cust" DataField="CSTRNR" SortExpression="CSTRNR" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CSTRCD" SortExpression="CSTRCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Cust XRef" DataField="XREFCS" SortExpression="XREFCS" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="SADDR1" SortExpression="SADDR1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CITY" SortExpression="CITY" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="STATE" SortExpression="STATE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Zip" DataField="ZIPCD" SortExpression="ZIPCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="PhoneFormatted" SortExpression="PhoneFormatted" ItemStyle-HorizontalAlign="Center" />
           
        </Columns>
    </asp:GridView>

            <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->

        <div class="spacer30"></div>
    </asp:Panel>
    <!-- END: PANEL (LOCATION) ======================================================================================= -->

        <%--  --%>

    <!-- START: PANEL (HISTORY) ======================================================================================= -->
    <asp:Panel ID="pnHistory" runat="server">


    <div class="w3-row">
        <h5 class="w3-text-teal">
            <asp:Label ID="lbHistoryCustomer" runat="server" /></h5>

            <table class="tableBorder" style="background-color: #F5F5F5; margin-bottom: 15px;">
                <tr>
                    <td style="vertical-align: top; padding: 15px;">
                        <div style="display:block; float:left; padding-right: 20px;">
                            <span style="font-style:italic;">Service Categories</span>
                            <div class="spacer5"></div>
                            <asp:CheckBoxList ID="cbServiceType" runat="server" >
                                <asp:ListItem Text="Onsite Service Tickets" Value="Onsite" Selected="True" />
                                <asp:ListItem Text="Depot and Advance Exchange Tickets" Value="ExpressDepot" Selected="True" />                              
                                <asp:ListItem Text="Managed IT Service Tickets" Value="MITS" Selected="True" />
                                <asp:ListItem Text="Managed Print Service Tickets" Value="MPrint" Selected="True" />                                
                                <asp:ListItem Text="Consumables Replenishment Tickets" Value="TonerReplenish" Selected="False" />                             
                                <asp:ListItem Text="Preventive Maintenance Tickets" Value="PMs" Selected="True" />
                                <asp:ListItem Text="Product Sale and Installation Tickets" Value="InstallSales" Selected="False" />
                                <asp:ListItem Text="Supply Sales Tickets" Value="Supplies" Selected="False" />
                            </asp:CheckBoxList>
                        </div>
                        <div style="display:block; float:left; padding-right: 20px;">
                            <span style="font-style:italic;">Ticket Types</span>
                            <div class="spacer5"></div>
                            <asp:ListBox 
                                ID="lsBxReportType" runat="server"  
                                Rows="7" 
                                AutoPostBack="true"
                                onselectedindexchanged="lsBxReportType_SelectedIndexChanged" Height="115px">
                                <asp:ListItem Text="Open Tickets" Value="Open" Selected="True" />
                                <asp:ListItem Text="Tickets Closed In Date Range" Value="ClosedRange" />
                                <asp:ListItem Text="Tickets Opened In Date Range" Value="OpenRange" />
                                <asp:ListItem Text="By Model Closed In Date Range" Value="ClosedModel" />
                                <asp:ListItem Text="By Ticket Number" Value="ByTicket" />
                                <asp:ListItem Text="By Ticket Cross Reference" Value="ByXref" />
                            </asp:ListBox>
                            <div class="spacer5"></div>
                            <asp:Button ID="btHistorySubmit" runat="server" Text="Search" SkinID="buttonPrimary" onClick="btHistorySubmit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btHistoryExcel" runat="server" Text="Excel" onClick="btExcelSubmit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btHistoryReset" runat="server" Text="Reset" onClick="btHistoryReset_Click" />

                        </div>
                        <div style="display:block; float:left; padding-right: 10px;">
                            <asp:Panel ID="pnHistoryCenterTicket" runat="server" Visible="false">
                                <table>
                                    <tr style="vertical-align:top;">
                                        <td><span style="font-style:italic;">Ticket Number</span></td>
                                    </tr>
                                    <tr style="vertical-align:top;">
                                        <td>
                                            <asp:TextBox ID="txHistoryCenterTicket" runat="server" Width="100" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnHistoryCustCallAlias" runat="server" Visible="false">
                                <table>
                                    <tr style="vertical-align:top;">
                                        <td><span style="font-style:italic;">Customer Ticket Cross Ref</span></td>
                                    </tr>
                                    <tr style="vertical-align:top;">
                                        <td>
                                            <asp:TextBox ID="txHistoryCustCallAlias" runat="server" Width="250" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnHistoryCalendars" runat="server" Visible="false">
                                <table>
                                    <tr style="vertical-align:top;">
                                        <td><span style="font-style:italic;">Start Date</span></td>
                                        <td><span style="font-style:italic;">End Date</span></td>
                                    </tr>
                                    <tr style="vertical-align:top;">
                                        <td style="padding-right: 20px;">
                                            <asp:TextBox ID="txHistoryStart" runat="server" Width="100" />
                                            <%--  
                                                <asp:Calendar ID="calHistoryStart" runat="server" SkinID="calendarBlue" />
                                                --%>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txHistoryEnd" runat="server" Width="100" />
                                                                                        <%--  
                                            <asp:Calendar ID="calHistoryEnd" runat="server" SkinID="calendarBlue" />
                                                --%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>

            <%--  --%>

        <asp:Panel ID="pnHistory_Reg" runat="server" Visible="true">

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_HistorySmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Entered</td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text='<%# Eval("DateEntered") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Completed</td>
                        <td>
                            <asp:Label ID="Label35" runat="server" Text='<%# Eval("DateCompleted") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket</td>
                        <td>
                            <asp:LinkButton ID="lkHistoryCall" runat="server" OnClick="lkHistoryCall_Click" 
                                Text='<%# Eval("Call") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("Call") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Summary</td>
                        <td>
                            <asp:Label ID="Label37" runat="server" Text='<%# Eval("Summary") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbHistoryCustomer" runat="server" Text='<%# Eval("CustomerName") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Address</td>
                        <td>
                            <asp:Label ID="Label38" runat="server" Text='<%# Eval("Address1") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td>
                            <asp:Label ID="lbHistoryCity" runat="server" Text='<%# Eval("City") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td>
                            <asp:Label ID="lbHistoryState" runat="server" Text='<%# Eval("State") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust XRef</td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("CustXref") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket XRef</td>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text='<%# Eval("TicketXRef") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model</td>
                        <td>
                            <asp:Label ID="Label23" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="Label24" runat="server" Text='<%# Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Status</td>
                        <td>
                            <asp:Label ID="Label25" runat="server" Text='<%# Eval("Status") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Part Use</td>
                        <td>
                            <asp:Label ID="Label26" runat="server" Text='<%# Eval("PartsUsed") %>' />
                        </td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Entered</td>
                        <td>
                            <asp:Label ID="Label30" runat="server" Text='<%# Eval("DateEntered") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Completed</td>
                        <td>
                            <asp:Label ID="Label36" runat="server" Text='<%# Eval("DateCompleted") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Ticket</td>
                        <td>
                            <asp:LinkButton ID="lkHistoryCall" runat="server" OnClick="lkHistoryCall_Click" 
                                Text='<%# Eval("Call") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("Call") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Summary</td>
                        <td>
                            <asp:Label ID="Label37" runat="server" Text='<%# Eval("Summary") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbHistoryCustomer" runat="server" Text='<%# Eval("CustomerName") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Address</td>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text='<%# Eval("Address1") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("City") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td>
                            <asp:Label ID="Label27" runat="server" Text='<%# Eval("State") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust XRef</td>
                        <td>
                            <asp:Label ID="Label28" runat="server" Text='<%# Eval("CustXref") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Ticket XRef</td>
                        <td>
                            <asp:Label ID="Label29" runat="server" Text='<%# Eval("TicketXref") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td>
                            <asp:Label ID="Label31" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td>
                            <asp:Label ID="Label32" runat="server" Text='<%# Eval("Serial") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Status</td>
                        <td>
                            <asp:Label ID="Label33" runat="server" Text='<%# Eval("Status") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Part Use</td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text='<%# Eval("PartsUsed") %>' />
                        </td>
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
                <asp:GridView ID="gv_HistoryLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_Hst"
        EmptyDataText="No matching tickets found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <%--  
            --%>
            <asp:BoundField HeaderText="Entered" DataField="DateEntered" SortExpression="SortEntered" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Completed" DataField="DateCompleted" SortExpression="SortCompleted" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Ticket" SortExpression="displayCall" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkHistoryCall" runat="server" 
                         OnClick="lkHistoryCall_Click" 
                         Text='<%# Eval("Call") %>'
                         CommandArgument='<%# Eval("Source") + "-" + Eval("Call") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Summary" DataField="Summary" SortExpression="Summary" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Customer" DataField="CustomerName" SortExpression="CustomerName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="Address1" SortExpression="Address1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="ST" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Cust Xref" DataField="CustXref" SortExpression="CustXref" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Ticket Xref" DataField="TicketXref" SortExpression="TicketXref" ItemStyle-HorizontalAlign="Left" />
            
            <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Part Use" DataField="PartsUsed" SortExpression="PartsUsed" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Trips" DataField="Trips" SortExpression="Trips" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>


                <!-- -->
            </div>
            <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
            <div class="spacer30"></div>
        </asp:Panel>
        <asp:Panel ID="pnHistory_Alt" runat="server" Visible="false">
            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_ModelSmall" runat="server">
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
                            <asp:Label ID="Label22" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Total Tickets</td>
                        <td>
                            <asp:Label ID="Label35" runat="server" Text='<%# Eval("ModelCount") %>' />
                        </td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td>
                            <asp:Label ID="Label30" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Total Tickets</td>
                        <td>
                            <asp:Label ID="Label36" runat="server" Text='<%# Eval("ModelCount") %>' />
                        </td>
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
                <asp:GridView ID="gv_ModelLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_Mod"
        EmptyDataText="No matching tickets found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <%--  
            --%>
            <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Total Tickets" DataField="ModelCount" SortExpression="ModelCount" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>


                <!-- -->
            </div>
            <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
            <div class="spacer30"></div>
        </asp:Panel>

    </div>
        <%--  --%>

    </asp:Panel>
    <!-- END: PANEL (HISTORY) ======================================================================================= -->

                <%--  --%>

        <%--  --%>
    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfChosenCs1" runat="server" />
    <asp:HiddenField ID="hfChosenCs2" runat="server" />
    <asp:HiddenField ID="hfChosenNam" runat="server" />

</div>
</asp:Content>
