<%@ Page Title="Website Editor Menu" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" 
    Inherits="private__editor_Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
STS Administration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div id="dvUtilities">
    <table id="tbOuter" style="width: 970px;">
        <tr style="vertical-align: top;">
            <td style="width: 48%; padding-right: 20px;">
                <!-- ====================== -->
                <div class="iconWrap">
                    <div class="btn btnColor_1 btnRight float_right">
                        <asp:HyperLink ID="hlChangeAnyPassword" runat="server" 
                            Text="Change Any Password" 
                            NavigateUrl="~/private/_editor/ChangeAnyPassword.aspx" />
                            <span></span>
                    </div>
                    <div class="iconBox">
                        <asp:Image ID="Image1" runat="server" 
                            ImageUrl="~/media/scantron/art/menu/green/Locked.png" />
                    </div>
                    This utility allows you to reset the password for any user account without needing to know the prior password.
                </div>
                <!-- ====================== -->
                <div style="clear: both; height: 20px;"></div>
                <div class="iconWrap">
                    <div class="btn btnColor_1 btnRight float_right">
                        <asp:HyperLink ID="HyperLink2" runat="server" 
                            Text="Customer Contacts"
                            NavigateUrl="~/private/_editor/Contacts.aspx" />
                            <span></span>
                    </div>
                    <div class="iconBox">
                        <asp:Image ID="Image6" runat="server" 
                            ImageUrl="~/media/scantron/art/menu/green/ContactBook.png" />
                    </div>
                    View customer contacts, and manage opt out list.
                </div>
                <!-- ====================== -->
            </td>
            <td style="width: 4%;">
            </td>
            <td style="width: 48%;">
                <!-- ====================== -->
                <!-- ====================== -->
                <div class="iconWrap">
                    <div class="btn btnColor_1 btnRight float_right">
                        <asp:HyperLink ID="HyperLink1" runat="server" 
                            Text="Hit Count"
                            NavigateUrl="~/private/_editor/HitCount.aspx" />
                            <span></span>
                    </div>
                    <div class="iconBox">
                        <asp:Image ID="Image2" runat="server" 
                            ImageUrl="~/media/scantron/art/menu/green/Plus.png" />
                    </div>
                    View hit counts for each page on the web site.
                </div>
                <!-- ====================== -->
            </td>
        </tr>
    </table>
</div><!-- end dvUtilities -->

</asp:Content>

