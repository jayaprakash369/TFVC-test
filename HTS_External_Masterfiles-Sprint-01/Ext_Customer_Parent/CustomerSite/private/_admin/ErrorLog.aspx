<%@ Page Title="Error Log" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ErrorLog.aspx.cs" 
    Inherits="private__admin_ErrorLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Error Log
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <%-- Error Log Repeater http://whatismyipaddress.com/ip/ --%> 
    <asp:Repeater ID="rpErrorLog" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="border-left: 1px solid #333333; border-right: 1px solid #333333; width: 100%;" >
                <tr>
                    <th>Summary</th>
                    <th>Description</th>
                    <th>Parms</th>
                    <th>Site</th>
                    <th>IP Address</th>
                    <th>Count</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg" style="vertical-align: top;">
                <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("WEERR") %>' /></td>
                <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("WEDSC") %>' /></td>
                <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("WEVAL") %>' /></td>
                <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("WEWEB") %>' /></td>
                <td><asp:HyperLink ID="hlIp" runat="server" Text='<%# Eval("WEIPA") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("WEIPA") %>' Target="IpHitCount" /></td>
                <td style="text-align: center;"><asp:Label ID="Label5" runat="server" Text='<%# Eval("Count") %>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt" style="vertical-align: top;">
                <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("WEERR") %>' /></td>
                <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("WEDSC") %>' /></td>
                <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("WEVAL") %>' /></td>
                <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("WEWEB") %>' /></td>
                <td><asp:HyperLink ID="hlIp" runat="server" Text='<%# Eval("WEIPA") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("WEIPA") %>' Target="IpHitCount" /></td>
                <td style="text-align: center;"><asp:Label ID="Label5" runat="server" Text='<%# Eval("Count") %>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Label ID="lbMessage" runat="server" SkinID="labelTitleColor2_Medium" />
</asp:Content>

