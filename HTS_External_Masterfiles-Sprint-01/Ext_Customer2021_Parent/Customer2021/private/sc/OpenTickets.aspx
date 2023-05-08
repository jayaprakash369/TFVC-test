<%@ Page Title="Open Tickets" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="OpenTickets.aspx.cs" 
    Inherits="private_sc_OpenTickets" %>

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


            <asp:Panel ID="pnModel" runat="server" Visible="false">
            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <h4 style="color: #555555;">Model Summary From Open Tickets</h4>
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
        EmptyDataText="No tickets found">
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
           <div class="spacer40"></div>
        </asp:Panel>


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
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummary" runat="server" Text='<% #Eval("Summary") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model</td>
                        <td><asp:Label ID="lbModelA" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbSerialA" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket Xref</td>
                        <td><asp:Label ID="lbTicketXrefA" runat="server" Text='<% #Eval("TicketXrefsCombined") %>' /></td>
                    </tr>
                    <%--  
                    <tr class="trColorReg">
                        <td>Pri Service Code</td>
                        <td><asp:Label ID="lbPrimaryServiceCodeB" runat="server" Text='<% #Eval("PrimaryServiceCodeForCustomer") %>' /></td>
                    </tr>

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
                        <td>Summary</td>
                        <td><asp:Label ID="lbSummary" runat="server" Text='<% #Eval("Summary") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td><asp:Label ID="lbModelB" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbSerialB" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Ticket Xref</td>
                        <td><asp:Label ID="lbTicketXrefB" runat="server" Text='<% #Eval("TicketXrefsCombined") %>' /></td>
                    </tr>

                    <%--  
                    <tr class="trColorAlt">
                        <td>Pri Service Code</td>
                        <td><asp:Label ID="lbPrimaryServiceCodeB" runat="server" Text='<% #Eval("PrimaryServiceCodeForCustomer") %>' /></td>
                    </tr>
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
            <asp:BoundField HeaderText="Entered" DataField="DateEntered" SortExpression="DateEnteredSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Customer" DataField="CustName" SortExpression="CustName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Summary" DataField="Summary" SortExpression="Summary" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="9" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="9" />
            <asp:BoundField HeaderText="TckXrf" DataField="TicketXrefsCombined" SortExpression="TicketXrefsCombined" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="9" />
           
                    <%--  
            <asp:BoundField HeaderText="PriSvc" DataField="PrimaryServiceCodeForCustomer" SortExpression="PrimaryServiceCodeForCustomer" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="9" />
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
