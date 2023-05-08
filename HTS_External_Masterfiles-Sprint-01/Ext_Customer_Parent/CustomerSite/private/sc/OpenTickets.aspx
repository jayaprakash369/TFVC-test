<%@ Page Title="Open Tickets" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="OpenTickets.aspx.cs" Inherits="private_sc_OpenTickets" %>
<%--  --%>             
<%--
<%@ Register TagPrefix="ctr" TagName="Cs1Change" Src="~/private/reg/inc/Cs1Change.ascx" %>
<script type="text/javascript" src="scripts/valuebanner.js" ></script>
<ctr:Cs1Change runat="server" id="Cs1Change" />
--%>             
<%-- BODY TITLE ========================================================================================================= --%>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Open Tickets</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server" Visible="true">
                    <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                                &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" 
                                    ValidationGroup="Cs1Change" />
                                    <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                        Operator="DataTypeCheck"
                                        Type="Integer"
                                        ControlToValidate="txCs1Change"
                                        ErrorMessage="Customer entry must be a number" 
                                        Text="*"
                                        SetFocusOnError="true" 
                                        ValidationGroup="Cs1Change"></asp:CompareValidator>
                                &nbsp;
                                <asp:Button ID="btCs1Change" runat="server" 
                                        Text="Change Customer" 
                                        onclick="btCs1Change_Click" 
                                        ValidationGroup="Cs1Change" />
                                <asp:ValidationSummary ID="vSummary_AdminCs1" runat="server" 
                                    ValidationGroup="Cs1Change" />
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table>
</asp:Content>
<%-- BODY CONTENT ======================================================================================================== --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">


<%-- PANEL TICKETS ======================================================================================================== --%>
<asp:Panel ID="pnTickets" runat="server" Visible="true">
    <asp:Label ID="lbError" runat="server" SkinID="labelError" Visible="false" />
    <%-- PANEL MAP (Container Store) ========================================================================================== --%>
    <asp:Panel ID="pnMap" runat="server" Visible="true">
        <table style="width: 100%;" class="tableWithoutLines">
            <tr>
                <td align="left">
                    <asp:Label ID="lbMapModTable" runat="server" Text="Map Tickets By Model" SkinID="labelTitleColor1_Medium"></asp:Label>                    
                </td>
                <td align="right">
                    <asp:Button ID="btMapAll" runat="server" 
                        Text="Map All Tickets" 
                        onclick="btMapAll_Click" 
                        />
                </td>
            </tr>
        </table>
        <%-- GRID VIEW MODELS (Container Store) ============================ --%>
        <asp:GridView ID="gvModels" runat="server" style="width: 100%" 
             AutoGenerateColumns="False" 
            CssClass="tableWithLines">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="Model">
                    <ItemTemplate>
                        <div style="text-align: left; text-indent: 10px;">
                        <asp:LinkButton ID="lkMapMod" runat="server" OnClick="lkMapModel_Click" 
                            CommandArgument='<%# Eval("Model") %>'>
                            <%# Eval("Model") %>
                        </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ModelCount" HeaderText="Total Tickets" />
            </Columns>
        </asp:GridView>
        <%-- END GRID VIEW MODELS ========================================== --%>
        <br />
        <%-- Special Label only shown to separate continer store table from normal table = --%>
        <table style="width: 100%;" class="tableWithoutLines">
            <tr>
                <td align="left">
                    <asp:Label ID="lbAllTickets" runat="server" Text="All Open Tickets" SkinID="labelTitleColor1_Medium"></asp:Label>                    
                </td>
            </tr>
        </table>

    </asp:Panel>
    <%-- End Panel Map ============================ --%>

    <%-- GRID VIEW TICKETS ========================================================================================== --%>
    <asp:GridView ID="gvTickets" runat="server" style="width: 100%"
     AutoGenerateColumns="False" 
     CssClass="tableWithLines"
     AllowPaging="true"
     PageSize="900" 
     AllowSorting="True" 
     onpageindexchanging="gvPageIndexChanging_Tck" 
     onsorting="gvSorting_Tck">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="DateEnteredText" HeaderText="Date Entered" SortExpression="DateEntered" />
        <asp:TemplateField HeaderText="Ticket" SortExpression="alpCtrTck">
            <ItemTemplate>
                <center>
                <asp:LinkButton ID="lkTicket" runat="server" OnClick="lbTicket_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Center") + "-" + Eval("Ticket") %>
                </asp:LinkButton>
                </center>
            </ItemTemplate>
       </asp:TemplateField>
       <asp:BoundField DataField="PurchOrd" HeaderText="Ticket XRef" SortExpression="PurchOrd" />
<%-- 
        <asp:BoundField DataField="CustXref" HeaderText="Cust XRef" SortExpression="CustXref" />
        <asp:BoundField DataField="ModelXref" HeaderText="Equip XRef" SortExpression="ModelXref" />
--%> 
        <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />
        <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
        <asp:TemplateField HeaderText="Status" SortExpression="Remark">
            <ItemTemplate>
                <asp:LinkButton ID="lkStatus" runat="server" OnClick="lbStatus_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Remark") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Part Use" SortExpression="PartsUsed">
            <ItemTemplate>
                <center>
                <asp:LinkButton ID="lkPartUse" runat="server" OnClick="lbPartUse_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("PartsUsed") %>
                </asp:LinkButton>
                </center>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
   </asp:GridView>
   <%-- End Grid View Tickets ============================ --%>

</asp:Panel>
<%-- End Panel Tickets ============================ --%>

<%-- PANEL DISPLAY ========================================================================================== --%>                
<asp:Panel ID="pnDisplay" runat="server" CssClass="up">
       
</asp:Panel>
<!-- End pnDisplay ** DO NOT DELETE ** This is dynamically loaded with user selections -->


</asp:Content>

