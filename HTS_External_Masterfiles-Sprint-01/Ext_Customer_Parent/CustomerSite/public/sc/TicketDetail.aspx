<%@ Page Title="STS Ticket Detail" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="TicketDetail.aspx.cs" Inherits="public_sc_TicketDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Ticket Detail
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<!-- Start pnDisplay ** DO NOT DELETE ** This is dynamically loaded ticket detail -->
<asp:Label ID="lbMessage" runat="server" Text=""></asp:Label>
<asp:Panel ID="pnDisplay" runat="server">
       
</asp:Panel>
<!-- End pnDisplay  -->

</asp:Content>

