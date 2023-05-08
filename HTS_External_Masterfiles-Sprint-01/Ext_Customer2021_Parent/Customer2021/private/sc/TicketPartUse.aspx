<%@ Page Title="Ticket Part Use" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="TicketPartUse.aspx.cs" 
    Inherits="private_sc_TicketPartUse" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Ticket Part Use: &nbsp;<asp:Label ID="lbTitleTicket" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->

                            <asp:Repeater ID="rp_PartUseSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbPrtName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbPrtDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbPrtSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Quantity</td>
                        <td><asp:Label ID="lbPrtQuantity" runat="server" Text='<% #Eval("Qty") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbPrtName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbPrtDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbPrtSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Quantity</td>
                        <td><asp:Label ID="lbPrtQuantity" runat="server" Text='<% #Eval("Qty") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
                <asp:GridView ID="gv_PartUseLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No part use was found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Part" DataField="Part" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Serial" DataField="Serial" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->


        <%--  --%>
    </div>
    <asp:HiddenField ID="hfPrimaryName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />

</div>
</asp:Content>
