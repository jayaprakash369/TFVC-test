<%@ Page Title="Web Page Hit Count" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="HitCount.aspx.cs" 
    Inherits="private__editor_HitCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Web Page Hit Count
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    
    <%-- Hit Count Repeater --%> 
    <asp:Repeater ID="rpHitCount" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="border-left: 1px solid #333333; border-right: 1px solid #333333;">
                <tr>
                    <th>Page</th>
                    <th>Path</th>
                    <th>Count</th>
                    <th>Year</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg">
                <td><asp:Label ID="lbPage" runat="server" Text='<%# Eval("hitPage") %>' /></td>
                <td><asp:Label ID="lbPath" runat="server" Text='<%# Eval("hitPath") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbCount" runat="server" Text='<%# Eval("hitCount") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbYear" runat="server" Text='<%# Eval("hitYear") %>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt">
                <td><asp:Label ID="lbPage" runat="server" Text='<%# Eval("hitPage") %>' /></td>
                <td><asp:Label ID="lbPath" runat="server" Text='<%# Eval("hitPath") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbCount" runat="server" Text='<%# Eval("hitCount") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbYear" runat="server" Text='<%# Eval("hitYear") %>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>

