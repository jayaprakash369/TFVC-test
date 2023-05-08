<%@ Page Title="IP Count" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="IpCount.aspx.cs" 
    Inherits="private__admin_IpCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
IP Address Hit Count
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    
    <script type="text/javascript">
        function clearIpSearch() {
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txIpSearch.value = "";
            return true;
        }
    </script>
    
        
    <%-- Old version... http://whatismyipaddress.com/ip/ --%>
    <%-- Search Panel --%>
    <div style="height: 15px; clear: both;"></div>
    <asp:Panel ID="pnSearch" runat="server" DefaultButton="btSearch">
        <table style="width: 100%;" class="tableWithLines" >
            <tr>
                <th>Find IP Like</th>
                <th style="text-align: left;"><asp:TextBox ID="txIpSearch" runat="server" Width="200" /></th>
                <th style="text-align: center;">
                    <asp:CheckBox ID="chbxBlacklisted" runat="server" Text="Blacklist Only?" /></th>
                <th>
                    <asp:DropDownList ID="ddSort" runat="server">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="PeriodHits" Value="PeriodHits" />
                        <asp:ListItem Text="LifetimeHits" Value="LifetimeHits" />
                        <asp:ListItem Text="LastAccess" Value="LastAccess" />
                        <asp:ListItem Text="PeriodStart" Value="PeriodStart" />
                        <asp:ListItem Text="Blacklisted" Value="Blacklisted" />
                    </asp:DropDownList>
                </th>
                <th style="text-align: center;"><input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearIpSearch();" /></th>
                <th style="text-align: center;"><asp:Button ID="btSearch" runat="server" Text="Search" onclick="btSearch_Click" /></th>
                <td style="padding-left: 10px;"><asp:Label ID="lbInfo" runat="server" Text="Watch for abnormal activity by hackers." /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lbIp" runat="server" />
    <%-- Update Panel --%>
    <div style="height: 15px; clear: both;"></div>
    <asp:Panel ID="pnCommentUpd" runat="server" Visible="false">
        <table style="width: 100%;" class="tableWithLines" >
            <tr>
                <th>IP Address</th>
                <th>Comment</th>
                <th>&nbsp;</th>
            </tr>
            <tr>
                <td style="text-align: center;"><asp:Label ID="lbUpdIp" runat="server" /></td>
                <td style="text-align: center;"><asp:TextBox ID="txUpdComment" runat="server" Width="400" /></td>
                <td style="text-align: center;"><asp:Button ID="btUpdSubmit" runat="server" Text="Update Comment" onclick="btUpdComment_Click" /></td>
            </tr>
        </table>
        <asp:HiddenField ID="hfUpdKey" runat="server" />
    </asp:Panel>
    
    <div style="height: 15px; clear: both;"></div>
    <%-- IP Address Hit Count Repeater --%> 
    <asp:Repeater ID="rpIpCount" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="border-left: 1px solid #333333; border-right: 1px solid #333333;">
                <tr>
                    <th></th>
                    <th>IP Lookup</th>
                    <th>Last Access</th>
                    <th>Period Hits</th>
                    <th>Lifetime Hits</th>
                    <th>Lockout Count</th>
                    <th>Blacklisted?</th>
                    <th>Toggle?</th>
                    <th>First Access</th>
                    <th>User</th>
                    <th>Comment</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg">
                <td style="text-align: center;"><asp:Label ID="lbIdx" runat="server" Text='<%# Eval("Idx") %>' /></td>
                <td><asp:HyperLink ID="hlIp" runat="server" Text='<%# Eval("Ip") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("Ip") %>' Target="IpHitCount" /></td>
                <td><asp:Label ID="lbLastAccess" runat="server" Text='<%# Eval("LastAccess") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbPeriodHits" runat="server" Text='<%# Eval("PeriodHits") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbLifeHits" runat="server" Text='<%# Eval("LifeHits") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbTimeouts" runat="server" Text='<%# Eval("TimeoutCount") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbBlacklisted" runat="server" Text='<%# Eval("Blacklisted") %>' /></td>
                <td style="text-align: center;">
                    <asp:LinkButton ID="lkBlacklist" runat="server" 
                        Text='<%# Eval("ToggleText") %>' 
                        onClick="lkBlacklistToggle_Click" 
                        CommandArgument='<%# Eval("Key") %>' />
                 </td>
                <td><asp:Label ID="lbFirstUse" runat="server" Text='<%# Eval("FirstUse") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbUser" runat="server" Text='<%# Eval("UserID") %>' /></td>
                <td>
                    <asp:LinkButton ID="lkCommentUpd" runat="server" 
                        Text='<%# "- " + Eval("Comment") %>' 
                        onClick="lkCommentUpd_Click" 
                        CommandArgument='<%# Eval("Key") + "|" + Eval("Ip") + "|" + Eval("Comment")%>' />
                 </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt">
                <td style="text-align: center;"><asp:Label ID="lbIdx" runat="server" Text='<%# Eval("Idx") %>' /></td>
                <td><asp:HyperLink ID="hlIp" runat="server" Text='<%# Eval("Ip") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("Ip") %>' Target="IpHitCount" /></td>
                <td><asp:Label ID="lbLastAccess" runat="server" Text='<%# Eval("LastAccess") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbPeriodHits" runat="server" Text='<%# Eval("PeriodHits") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbLifeHits" runat="server" Text='<%# Eval("LifeHits") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbTimeouts" runat="server" Text='<%# Eval("TimeoutCount") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbBlacklisted" runat="server" Text='<%# Eval("Blacklisted") %>' /></td>
                <td style="text-align: center;">
                    <asp:LinkButton ID="lkBlacklist" runat="server" 
                        Text='<%# Eval("ToggleText") %>' 
                        onClick="lkBlacklistToggle_Click" 
                        CommandArgument='<%# Eval("Key") %>' />
                 </td>
                <td><asp:Label ID="lbFirstUse" runat="server" Text='<%# Eval("FirstUse") %>' /></td>
                <td style="text-align: center;"><asp:Label ID="lbUser" runat="server" Text='<%# Eval("UserID") %>' /></td>
                <td>
                    <asp:LinkButton ID="lkCommentUpd" runat="server" 
                        Text='<%#  "- " + Eval("Comment") %>' 
                        onClick="lkCommentUpd_Click" 
                        CommandArgument='<%# Eval("Key") + "|" + Eval("Ip") + "|" + Eval("Comment")%>' />
                 </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>

