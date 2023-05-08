<%@ Page Title="Hacker Watch" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="HitCountByIpAddress.aspx.cs" 
    Inherits="private_siteAdministration_HitCountByIpAddress" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <%--  --%>
        <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .LocationHeaderElements {
            display: inline-block;
            float: left;
            padding-right: 10px;
            padding-bottom: 5px;
        }
        .UpdateElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
            vertical-align: bottom;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Hacker Watch
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-16" style="margin-left: 16px;">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (IP) ======================================================================================= -->
    <asp:Panel ID="pnIpa" runat="server">
    
        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnIpaSearch" runat="server" DefaultButton="btSearchIpaSubmit">
            
            <h3 class="titlePrimary">Select IP</h3>
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    IP<br />
                    <asp:TextBox ID="txSearchIpa" runat="server" Width="175" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:CheckBox ID="chbxIpa_Blacklisted" runat="server" Text="Only Blacklisted Users?" SkinId="checkbox1" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchIpaSubmit" runat="server" Text="Search" OnClick="btSearchIpaSubmit_Click" SkinID="buttonPrimary" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchIpaClear" runat="server" Text="Clear" OnClick="btSearchIpaClear_Click" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>


   <asp:Panel ID="pnUpdateIpaComment" runat="server" Visible="false">
            
    <h4 class="titlePrimary" style="font-style: italic;">Update Comment</h4>
    <table class="tableWithoutLines tableBorderBackgroundLight">
        <tr>
            <td>
                <div class="UpdateElements">
                    IP Address<br />
                    <asp:Label ID="lbUpdateIpa_Address" runat="server" Text="" />        
                </div>
                <div class="UpdateElements">
                    Comment<br />
                    <asp:TextBox ID="txUpdateIpa_Comment" runat="server"  Width="250"/>
                </div>
                <div class="UpdateElements">
                    <br />
                    <asp:Button ID="btUpdateIpa_Comment" runat="server" 
                        Text="Update" 
                        OnClick="btUpdateIpa_Comment_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btUpdateIpa_CommentClose" runat="server" 
                        Text="Close?" 
                        Font-Size="9" 
                        Font-Italic="true"
                        OnClick="btUpdateIpa_CommentClose_Click" />

                        <asp:HiddenField ID="hfUpdateIpa_RecId" runat="server" />

                </div>
            </td>
        </tr>
    </table>
            <div class="spacer15"></div>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (IP) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_IpaSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Ip Lookup</td>
                        <td>
                            <asp:HyperLink ID="hlIp" runat="server" Text='<%# Eval("Ip") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("Ip") %>' Target="IpHitCount" />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Last Access</td>
                        <td><asp:Label ID="lbLastAccess" runat="server" Text='<%# Eval("LastAccess") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Period Hits</td>
                        <td><asp:Label ID="Label21" runat="server" Text='<%# Eval("PeriodHits") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Lifetime Hits</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("LifeHits") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Lockout Count</td>
                        <td><asp:Label ID="Label14" runat="server" Text='<%# Eval("TimeoutCount") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Blacklisted?</td>
                        <td><asp:Label ID="Label20" runat="server" Text='<%# Eval("Blacklisted") %>' /></td>
                    </tr>

                    <tr class="trColorReg">
                        <td>Toggle?</td>
                        <td>
                           <asp:LinkButton ID="lkBlacklist" runat="server" 
                                Text='<%# Eval("ToggleText") %>' 
                                onClick="lkBlacklistToggle_Click" 
                                CommandArgument='<%# Eval("Key") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>First Access</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("FirstUse") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>User</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("UserId") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Comment</td>
                        <td>
                        <asp:LinkButton ID="lkCommentUpd" runat="server" 
                        Text='<%# "- " + Eval("Comment") %>' 
                        onClick="lkCommentUpd_Click" 
                        CommandArgument='<%# Eval("Key") + "|" + Eval("Ip") + "|" + Eval("Comment")%>' />
                        </td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Ip Lookup</td>
                        <td>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Ip") %>' NavigateUrl='<%# "http://ip-address-lookup-v4.com/lookup.php?ip=" + Eval("Ip") %>' Target="IpHitCount" />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Last Access</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("LastAccess") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Period Hits</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("PeriodHits") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Lifetime Hits</td>
                        <td><asp:Label ID="Label6" runat="server" Text='<%# Eval("LifeHits") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Lockout Count</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<%# Eval("TimeoutCount") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Blacklisted?</td>
                        <td><asp:Label ID="Label8" runat="server" Text='<%# Eval("Blacklisted") %>' /></td>
                    </tr>

                    <tr class="trColorAlt">
                        <td>Toggle?</td>
                        <td>
                           <asp:LinkButton ID="LinkButton1" runat="server" 
                                Text='<%# Eval("ToggleText") %>' 
                                onClick="lkBlacklistToggle_Click" 
                                CommandArgument='<%# Eval("Key") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>First Access</td>
                        <td><asp:Label ID="Label9" runat="server" Text='<% #Eval("FirstUse") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>User</td>
                        <td><asp:Label ID="Label10" runat="server" Text='<% #Eval("UserId") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Comment</td>
                        <td>
                        <asp:LinkButton ID="LinkButton2" runat="server" 
                        Text='<%# "- " + Eval("Comment") %>' 
                        onClick="lkCommentUpd_Click" 
                        CommandArgument='<%# Eval("Key") + "|" + Eval("Ip") + "|" + Eval("Comment")%>' />
                        </td>
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
        <!-- END: SMALL SCREEN TABLE (LOCATION) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->


        <asp:GridView ID="gv_IpaLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Ipa"
            allowPaging="true"
            OnPageIndexChanging="gvPageIndexChanging_Ipa"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="IP Lookup" SortExpression="Ip" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbIp" runat="server" Text='<%# Eval("Ip") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Last Access" DataField="LastAccess" SortExpression="LastAccess" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Period Hits" DataField="PeriodHits" SortExpression="PeriodHits" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Lifetime Hits" DataField="LifeHits" SortExpression="LifeHits" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Lockout Count" DataField="TimeoutCount" SortExpression="TimeoutCount" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Blacklisted?" DataField="Blacklisted" SortExpression="Blacklisted" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Toggle" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton3" runat="server" 
                        Text='<%# Eval("ToggleText") %>' 
                        onClick="lkBlacklistToggle_Click" 
                        CommandArgument='<%# Eval("Key") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="User" DataField="UserId" SortExpression="UserId" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Comment" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <asp:LinkButton ID="LinkButton5" runat="server" 
                        Text='<%# "- " + Eval("Comment") %>' 
                        onClick="lkCommentUpd_Click" 
                        CommandArgument='<%# Eval("Key") + "|" + Eval("Ip") + "|" + Eval("Comment")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="spacer30"></div>
</asp:Panel><!-- END: PANEL (LOCATION) ======================================================================================= -->

    </div>

    <%-- 
        --%>

        <%--  --%>
</div>
</asp:Content>

