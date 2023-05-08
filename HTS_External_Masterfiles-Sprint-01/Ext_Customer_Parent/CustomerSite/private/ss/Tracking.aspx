<%@ Page Title="Shipment Tracking" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Tracking.aspx.cs" 
    Inherits="private_ss_Tracking" %>
<%--  --%> 
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Shipment Tracking</td>
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
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<center>
    <asp:Panel ID="pnInput" runat="server" DefaultButton="btInput">
    <table style="width: 100%; text-align: center;">
        <tr>
            <th style="width: 25%;">Stocking Location</th>
            <th style="width: 25%;">Start Date</th>
            <th style="width: 25%;">End Date</th>
            <th style="width: 25%;">&nbsp;</th>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <asp:DropDownList ID="ddStockLoc" runat="server" CssClass="dropDownList1" 
                    OnSelectedIndexChanged="ddStockLoc_Changed" 
                    AutoPostBack="true" />
            </td>
            <td align="center"><asp:Calendar ID="calStart" runat="server" SkinID="calendarBlue" /></td>
            <td align="center"><asp:Calendar ID="calEnd" runat="server" SkinID="calendarBlue" /></td>
            <td><asp:Button ID="btInput" runat="server" Text="Continue" onclick="btInput_Click" /> </td>
        </tr>
    </table>

    <asp:DropDownList ID="ddTechNum" runat="server" CssClass="dropDownList1" 
        Visible="false" 
        OnSelectedIndexChanged="ddTechNum_Changed" 
        AutoPostBack="true" />
    </asp:Panel>

    <asp:Panel ID="pnShipments" runat="server">
        <div style="height: 30px;"></div>
        <asp:GridView ID="gvShipments" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="DateOrdered" HeaderText="Date Ordered" />
        <asp:BoundField DataField="DateShipped" HeaderText="Date Shipped" />
        <asp:TemplateField HeaderText="Transfer">
            <ItemTemplate>
                <asp:LinkButton ID="lkTransfer" runat="server" OnClick="lkTransfer_Click" 
                    CommandArgument='<%# Eval("Transfer")%>'>
                    <%# Eval("Transfer") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="aCtrTck" HeaderText="Ticket" />
        <asp:TemplateField HeaderText="Shipping Info">
            <ItemTemplate>
                <asp:LinkButton ID="lkShipTo" runat="server" OnClick="lkShipTo_Click" 
                    CommandArgument='<%# Eval("Transfer")%>'>
                    <%# Eval("Carrier")%>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tracking Number">
            <ItemTemplate>
                <asp:HyperLink ID="hlTracking" runat="server" Target="_blank" NavigateUrl='<%# "https://wwwapps.ups.com/WebTracking/track?track=yes&trackNums=" + Eval("TrackingNumber") %>'><%# Eval("TrackingNumber") %></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Expedited" HeaderText="Expedited?" />
    </Columns>
</asp:GridView>

    </asp:Panel>
    <asp:Panel ID="pnTransfer" runat="server" Visible="false">
        <asp:Label ID="lbTransfer" runat="server"
            SkinId="labelTitleColor1_Medium" />
        <div style="height: 20px;"></div>
        <asp:GridView ID="gvTransfer" runat="server" 
            AutoGenerateColumns="False" 
            CssClass="tableWithLines">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="Sequence" HeaderText="Seq" />
        <asp:BoundField DataField="Part" HeaderText="Part" />
        <asp:BoundField DataField="OrderQty" HeaderText="Qty Ordered" />
        <asp:BoundField DataField="FillShipQty" HeaderText="Qty Shipped" />
        <asp:BoundField DataField="BackorderQty" HeaderText="Qty Backordered" />
        <asp:BoundField DataField="ReceivedQty" HeaderText="Shipped" />
        <asp:BoundField DataField="DateReceived" HeaderText="Date Shipped" />
     </Columns>
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel ID="pnShipTo" runat="server">
        <asp:Label ID="lbShipTo" runat="server"
            SkinId="labelTitleColor1_Medium" />
        <div style="height: 20px;"></div>
        <asp:Repeater ID="rpShipTo" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <table class="tableVerticalList" style="width: 100%;">
                    <tr>
                        <th>Ship to Name</th>
                        <td><asp:Label ID="lbName" runat="server" Text='<%# Eval("Name") %>' /></td>
                    </tr>
                    <tr>
                        <th>Address</th>
                        <td><asp:Label ID="lbAddress" runat="server" Text='<%# Eval("Address") %>' /></td>
                    </tr>
                    <tr>
                        <th>City</th>
                        <td><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                    </tr>
                    <tr>
                        <th>State</th>
                        <td><asp:Label ID="lbState" runat="server" Text='<%# Eval("State") %>' /></td>
                    </tr>
                    <tr>
                        <th>Zip Code</th>
                        <td><asp:Label ID="lbZip" runat="server" Text='<%# Eval("Zip") %>' /></td>
                    </tr>
                    <tr>
                        <th>Phone</th>
                        <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Phone") %>' /></td>
                    </tr>
                    <tr>
                        <th>Attention</th>
                        <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Attention") %>' /></td>
                    </tr>
                    <tr>
                        <th>Instructions</th>
                        <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Instructions") %>' /></td>
                    </tr>
                    <tr>
                        <th>Shipping Method</th>
                        <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("ShipVia") %>' /></td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>

    </asp:Panel>
</center>
</asp:Content>

