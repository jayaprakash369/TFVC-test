<%@ Page Title="Admin Utilities" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Utilities.aspx.cs" Inherits="private__admin_Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Hidden Utility Pages
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <p><asp:HyperLink ID="HyperLink1" runat="server" Text="Zips For Sales" NavigateUrl="~/public/utils/ZipZones.aspx" /></p>
    <p><asp:HyperLink ID="HyperLink2" runat="server" Text="Zips For Managers" NavigateUrl="~/public/utils/ZipMgr.aspx" /></p>
    <p><asp:HyperLink ID="HyperLink3" runat="server" Text="Public Ticket Detail" NavigateUrl="~/public/sc/TicketDetail.aspx?key=a873hndjki" /></p>
    <p><asp:HyperLink ID="HyperLink4" runat="server" Text="Public Comment" NavigateUrl="~/public/sc/Comments.aspx?key=a873hndjki" /></p>
    <p><asp:HyperLink ID="HyperLink5" runat="server" Text="Progress Bar" NavigateUrl="~/private/_admin/test/ProgressBar.aspx" /></p>
</asp:Content>

