<%@ Page Title="Web Administrator Menu" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" 
    Inherits="private__admin_Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Web Administration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink7" runat="server" 
                                Text="Hacker Check: IP" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/IpCount.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Caution.png" alt="" class="menuImage"  />
                            View excessive use from a specific IP address.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink8" runat="server" 
                                Text="Error Log" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/ErrorLog.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Flag.png" alt="" class="menuImage"  />
                            View errors from all sites
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink9" runat="server" 
                                Text="Show Site Styles" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/Styles.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ColorBars.png" alt="" class="menuImage"  />
                            Show control appearance styled by current theme.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink10" runat="server" 
                                Text="Hidden Utilities" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/Utilities.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/house.png" alt="" class="menuImage"  />
                            Show admin utilities hidden from general view.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink11" runat="server" 
                                Text="Print Files" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/PrintFiles.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/paintbrush.png" alt="" class="menuImage"  />
                            Display file data for quick copy or printing
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink12" runat="server" 
                                Text="Upload Files" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/_admin/upload.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/arrowup.png" alt="" class="menuImage"  />
                            Upload Files needed in the DMZ (to C:\OurSites\Uploads)
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

