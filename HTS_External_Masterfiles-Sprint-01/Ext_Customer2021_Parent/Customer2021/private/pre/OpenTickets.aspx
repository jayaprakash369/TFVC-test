<%@ Page Title="Open Tickets" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="OpenTickets.aspx.cs" 
    Inherits="private_pre_OpenTickets" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Open Service Tickets
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />

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
                        <td>TicketId</td>
                        <td>
                            <asp:LinkButton ID="lkTicket" runat="server" OnClick="lkTicket_Click" 
                                Text='<%# Eval("TicketId") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>'  />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Date Entered</td>
                        <td><asp:Label ID="lbDateEntered" runat="server" Text='<% #Eval("DateEntered") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td><asp:Label ID="lbCustomer" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>

                    <tr class="trColorReg">
                        <td>Status</td>
                        <td><asp:Label ID="lbStatus" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Eta</td>
                        <td><asp:Label ID="lbEtaA" runat="server" Text='<% #Eval("EtaDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummary" runat="server" Text='<% #Eval("Summary") %>' /></td>
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
                        <td>TicketId</td>
                        <td>
                            <asp:LinkButton ID="lkTicket" runat="server" OnClick="lkTicket_Click" 
                                Text='<%# Eval("TicketId") %>'
                                CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Date Entered</td>
                        <td><asp:Label ID="lbDateEntered" runat="server" Text='<% #Eval("DateEntered") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td><asp:Label ID="lbCustomer" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>

                    <tr class="trColorAlt">
                        <td>Status</td>
                        <td><asp:Label ID="lbStatus" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Eta</td>
                        <td><asp:Label ID="lbEtaB" runat="server" Text='<% #Eval("EtaDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummary" runat="server" Text='<% #Eval("Summary") %>' /></td>
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
            <asp:TemplateField HeaderText="Ticket" SortExpression="TicketIdSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkTicket" runat="server" 
                        OnClick="lkTicket_Click" 
                        CommandArgument='<%# Eval("Source") + "-" + Eval("TicketId") %>'
                        Text='<%# Eval("TicketId")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Date Entered" DataField="DateEntered" SortExpression="DateEnteredSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Customer" DataField="CustName" SortExpression="CustName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Eta" DataField="EtaDisplay" SortExpression="Eta Sort" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Summary" DataField="Summary" SortExpression="Summary" ItemStyle-HorizontalAlign="Left" />
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
