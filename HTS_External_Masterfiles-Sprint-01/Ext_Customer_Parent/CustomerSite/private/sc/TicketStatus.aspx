<%@ Page Title="Ticket Status" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="TicketStatus.aspx.cs" Inherits="private_sc_TicketStatus" %>
<%--
--%>             
<%-- BODY TITLE ========================================================================================================= --%>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Ticket Status</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server">
                    <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                                &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" ValidationGroup="Cs1Change" />
                                    <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                        Operator="DataTypeCheck"
                                        Type="Integer"
                                        ControlToValidate="txCs1Change"
                                        ErrorMessage="Customer entry must be a number" 
                                        Text="*"
                                        SetFocusOnError="true" 
                                        ValidationGroup="Cs1Change">
                                    </asp:CompareValidator>
                                    &nbsp;
                                    <asp:Button ID="btCs1Change" runat="server" 
                                        Text="Change Customer" 
                                        onclick="btCs1Change_Click"
                                        ValidationGroup="Cs1Change" />
                                    <asp:ValidationSummary ID="vSumCs1Change" runat="server" ValidationGroup="Cs1Change" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>

<%-- BODY CONTENT ========================================================================================================= --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<script type="text/javascript">
    function clearInput() {
        var doc = document.forms[0];
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txCtr.value = "";
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txTck.value = "";
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txXrf.value = "";
        return true;
    }
</script>

<%-- PANEL INPUT ========================================================================================================= --%>
<asp:Panel ID="pnInput" runat="server" Visible="false" DefaultButton="btLocSearch">
    <asp:Table ID="tbInput" runat="server" CssClass="tableWithoutLines">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell HorizontalAlign="Left">Enter the two part ticket number</asp:TableHeaderCell>
            <asp:TableHeaderCell HorizontalAlign="Left"><div style="width: 30px;">&nbsp;</div></asp:TableHeaderCell>
            <asp:TableHeaderCell HorizontalAlign="Center" ColumnSpan="1">OR your ticket cross reference</asp:TableHeaderCell>
            <asp:TableHeaderCell HorizontalAlign="Left">&nbsp;</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <table class="tableWithLines" style="width: 200px;">
                    <tr>
                        <th>
                            i.e. 111 - 2222
                        </th>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txCtr" runat="server" Width="30" MaxLength="3" />
                                <asp:CompareValidator id="vCompare_Ctr" runat="server" 
                                    Operator="DataTypeCheck"
                                    Type="Integer"
                                    ControlToValidate="txCtr"
                                    ErrorMessage="Center must be an integer" 
                                    Text="*"
                                    SetFocusOnError="true" 
                                    ValidationGroup="TicketSearch"></asp:CompareValidator>
                            &nbsp;-&nbsp;
                             <asp:TextBox ID="txTck" runat="server" Width="70"  MaxLength="7" />
                                 <asp:CompareValidator id="vCompare_Tck" runat="server" 
                                    Operator="DataTypeCheck"
                                    Type="Integer"
                                    ControlToValidate="txTck"
                                    ErrorMessage="Ticket must be an integer" 
                                    Text="*"
                                    SetFocusOnError="true" 
                                    ValidationGroup="TicketSearch"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
            <asp:TableCell>
            &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                <table class="tableWithLines" style="width: 200px;">
                    <tr>
                        <th>
                            i.e. MY CROSS REF
                        </th>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txXrf" runat="server" Width="120" MaxLength="24" />
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    onclick="btLocSearch_Click" 
                    ValidationGroup="TicketSearch" />
            </asp:TableCell>
            <asp:TableCell>
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearInput();" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ValidationSummary ID="vSummary_LocSearch" runat="server" ValidationGroup="TicketSearch" />
    <asp:Label ID="lbError" runat="server" SkinID="labelError" />
</asp:Panel>
<%-- END PANEL INPUT ===== --%>

<%-- PANEL TICKETS ========================================================================================================= --%>
<asp:Panel ID="pnTickets" runat="server" Visible="false">
    <div style="height: 20px;"></div>
    <asp:Repeater ID="rpTickets" runat="server">
        <HeaderTemplate>
          <table id="htmlTbTickets" class="tableWithLines" style="width: 100%">
            <tr>
              <th>Customer</th>
              <th>Entry Date</th>
              <th>Ticket</th>
              <th>Ticket XRef</th>
              <th>Cust XRef</th>
              <th>Model</th>
              <th>Serial</th>
              <th>Status</th>
              <th>Part Use</th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg">
              <td align="center"><%# Eval("CustName") %></td>
              <td align="center"><%# Eval("DateEntered") %></td>
              <td align="center">
                <asp:LinkButton ID="lkTicket1" runat="server" OnClick="lbTicket_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Center") + "-" + Eval("Ticket") %>
                </asp:LinkButton>
              </td>
              <td><%# Eval("PurchOrd") %></td>
              <td><%# Eval("CustXref") %></td>
              <td><%# Eval("Model") %></td>
              <td><%# Eval("Serial") %></td>
              <td align="center">
                <asp:LinkButton ID="lkStatus1" runat="server" OnClick="lbStatus_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Remark") %>
                </asp:LinkButton>
              </td>
              <td align="center">
                <asp:LinkButton ID="lkPartUse1" runat="server" OnClick="lbPartUse_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("PartsUsed") %>
                </asp:LinkButton>
              </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt">
              <td align="center"><%# Eval("CustName") %></td>
              <td align="center"><%# Eval("DateEntered") %></td>
              <td align="center">
                <asp:LinkButton ID="lkTicket2" runat="server" OnClick="lbTicket_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Center") + "-" + Eval("Ticket") %>
                </asp:LinkButton>
              </td>
              <td><%# Eval("PurchOrd") %></td>
              <td><%# Eval("CustXref") %></td>
              <td><%# Eval("Model") %></td>
              <td><%# Eval("Serial") %></td>
              <td align="center">
                <asp:LinkButton ID="lkStatus2" runat="server" OnClick="lbStatus_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Remark") %>
                </asp:LinkButton>
              </td>
              <td align="center">
                <asp:LinkButton ID="lkPartUse2" runat="server" OnClick="lbPartUse_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("PartsUsed") %>
                </asp:LinkButton>
              </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
        </table>
        </FooterTemplate>
      </asp:Repeater>

</asp:Panel>
<%-- END PANEL TICKETS ============ --%>

<%-- PANEL DISPLAY ========================================================================================================= --%>
<asp:Panel ID="pnDisplay" runat="server" CssClass="up">
       
</asp:Panel>
</asp:Content>

