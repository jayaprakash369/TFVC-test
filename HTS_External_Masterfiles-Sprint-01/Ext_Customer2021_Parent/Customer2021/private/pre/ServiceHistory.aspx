<%@ Page Title="Service History" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ServiceHistory.aspx.cs" 
    Inherits="private_pre_ServiceHistory" %>

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

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        

        <!-- START: PANEL SEARCH (TICKET) ======================================================================================= -->
        <asp:Panel ID="pnTicketSearch" runat="server" DefaultButton="btSearchTicketSubmit">
            <table>
                <tr style="vertical-align: bottom;">
                    <td>
                        <h3 class="titleSecondary"><asp:Label ID="lbTicketSearchTitle" runat="server" Text="Search" /></h3>
                    </td>
                    <td style="padding-left: 30px; padding-bottom: 8px;"><asp:Label ID="lbMsg" runat="server" SkinID="labelError" /></td>
                </tr>
            </table>
            <table class="tableBorderBackgroundLight" style="max-width: 1050px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                        <div class="SearchPanelElements">
                            Center<br />
                            <asp:TextBox ID="txSearchCenter" runat="server" Width="40" />
                        </div>
                        <div class="SearchPanelElements">
                            Ticket<br />
                            <asp:TextBox ID="txSearchTicket" runat="server" Width="70" />
                        </div>
                        <div class="SearchPanelElements">
                            Ticket Xref<br />
                            <asp:TextBox ID="txSearchTicketXref" runat="server" Width="90" />
                        </div>
                        <div class="SearchPanelElements">
                            Customer Name (like)<br />
                            <asp:TextBox ID="txSearchCustomerName" runat="server" Width="150" />
                        </div>
                        <div class="SearchPanelElements">
                            Cust Num<br />
                            <asp:TextBox ID="txSearchCustomerNum" runat="server" Width="70" />
                        </div>
                        <div class="SearchPanelElements">
                            Loc<br />
                            <asp:TextBox ID="txSearchCustomerLoc" runat="server" Width="40" />
                        </div>
                        <div class="SearchPanelElements">
                            City (like)<br />
                            <asp:TextBox ID="txSearchCity" runat="server" Width="120" />
                        </div>
                        <div class="SearchPanelElements">
                            State<br />
                            <asp:TextBox ID="txSearchState" runat="server" Width="30" />
                        </div>
                        <div class="SearchPanelElements">
                            Tech<br />
                            <asp:TextBox ID="txSearchTech" runat="server" Width="50" />
                        </div>
                        <div class="SearchPanelElements">
                            Status (like)<br />
                            <asp:TextBox ID="txSearchStatus" runat="server" Width="150" />
                        </div>
                        <div class="SearchPanelElements">
                            Summary (contains)<br />
                            <asp:TextBox ID="txSearchSummary" runat="server" Width="170" />
                        </div>
                        <div class="SearchPanelElements">
                            Entered > <span style='font-size: 10px;'>(yyyymmdd)</span><br />
                            <asp:TextBox ID="txSearchEnteredGT" runat="server" Width="90" />
                        </div>
                        <div class="SearchPanelElements">
                            Entered <<br />
                            <asp:TextBox ID="txSearchEnteredLT" runat="server" Width="90" />
                        </div>
                        <div class="SearchPanelElements">
                            Closed ><br />
                            <asp:TextBox ID="txSearchClosedGT" runat="server" Width="90" />
                        </div>
                        <div class="SearchPanelElements">
                            Closed <<br />
                            <asp:TextBox ID="txSearchClosedLT" runat="server" Width="90" />
                        </div>
                        <div class="SearchPanelElements">
                            Open/Closed?<br />
                            <asp:DropDownList ID="ddSearchOpenClosed" runat="server">
                                <asp:ListItem Text="Open" Value="open" />
                                <asp:ListItem Text="Closed" Value="closed" />
                                <asp:ListItem Text="Both" Value="any" Selected="True" />
                            </asp:DropDownList>
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btSearchTicketClear" runat="server" Text="Clear" OnClick="btSearchTicketClear_Click" />
                        </div>
                        <div class="SearchPanelElements">
                            <br />
                            <asp:Button ID="btSearchTicketSubmit" runat="server" Text="Search" OnClick="btSearchTicketSubmit_Click" SkinID="buttonPrimary" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>
        <!-- END: PANEL SEARCH (EQUIPMENT) ======================================================================================= -->

<%--  --%>

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->

            <asp:Repeater ID="rp_TicketSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Ctr-Tck</td>
                        <td>
                            <asp:LinkButton ID="lkTicketA" runat="server" OnClick="lkTicket_Click" 
                                Text='<%# Eval("TicketId") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>'  />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tck Xref</td>
                        <td><asp:Label ID="lbTicketXrefA" runat="server" Text='<% #Eval("TckXrefCust") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer Name</td>
                        <td><asp:Label ID="lbCustomerNameA" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust Num-Loc</td>
                        <td><asp:Label ID="lbCustomerNumA" runat="server" Text='<% #Eval("CustNum") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCityA" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="lbStateA" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tech</td>
                        <td><asp:Label ID="lbAssignedA" runat="server" Text='<% #Eval("Assigned") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Status</td>
                        <td><asp:Label ID="lbStatusA" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummaryA" runat="server" Text='<% #Eval("Summary") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Entered</td>
                        <td><asp:Label ID="lbDateEnteredA" runat="server" Text='<% #Eval("DateEntered") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Closed</td>
                        <td><asp:Label ID="lbDateCompletedA" runat="server" Text='<% #Eval("DateCompleted") %>' /></td>
                    </tr>
                    <%--  
                    <tr class="trColorReg">
                        <td>Source</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("Source") %>' /></td>
                    </tr>
                        --%>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Center-Ticket</td>
                        <td>
                            <asp:LinkButton ID="lkTicketB" runat="server" OnClick="lkTicket_Click" 
                                Text='<%# Eval("TicketId") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tck Xref</td>
                        <td><asp:Label ID="lbTicketXrefb" runat="server" Text='<% #Eval("TckXrefCust") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer Name</td>
                        <td><asp:Label ID="lbCustomerNameB" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust Num-Loc</td>
                        <td><asp:Label ID="lbCustomerNumB" runat="server" Text='<% #Eval("CustNum") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="lbCityB" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="lbStateB" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tech</td>
                        <td><asp:Label ID="lbAssignedB" runat="server" Text='<% #Eval("Assigned") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Status</td>
                        <td><asp:Label ID="lbStatusB" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummaryB" runat="server" Text='<% #Eval("Summary") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Entered</td>
                        <td><asp:Label ID="lbDateEnteredB" runat="server" Text='<% #Eval("DateEntered") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Closed</td>
                        <td><asp:Label ID="lbDateCompletedB" runat="server" Text='<% #Eval("DateCompleted") %>' /></td>
                    </tr>
                    <%--  
                    <tr class="trColorAlt">
                        <td>Source</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("Source") %>' /></td>
                    </tr>
                        --%>
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

   <asp:GridView ID="gv_TicketLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_Tck"
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="Ctr-Tck" SortExpression="TicketIdSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkTicket" runat="server" 
                        OnClick="lkTicket_Click" 
                        CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>'
                        Text='<%# Eval("TicketId")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Tck Xref" DataField="TckXrefCust" SortExpression="TckXrefCust" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Customer Name" DataField="CustName" SortExpression="CustName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Cust Num-Loc" DataField="CustNum" SortExpression="CustSort" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Tech" DataField="Assigned" SortExpression="AssignedSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Summary" DataField="Summary" SortExpression="Summary" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Entered" DataField="DateEntered" SortExpression="DateEnteredSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Closed" DataField="DateCompleted" SortExpression="DateCompletedSort" ItemStyle-HorizontalAlign="Center" />
                    <%--  
            <asp:BoundField HeaderText="Source" DataField="Source" SortExpression="Source" ItemStyle-HorizontalAlign="Center" />
                        --%>
        </Columns>
    </asp:GridView>

            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->



        <div class="spacer30"></div>

        <%--  --%>
    </div>
    <asp:HiddenField ID="hfUserName" runat="server" />
        <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    

</div>
</asp:Content>
