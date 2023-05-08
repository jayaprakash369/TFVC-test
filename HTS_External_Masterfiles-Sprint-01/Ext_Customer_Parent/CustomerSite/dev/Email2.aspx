<%@ Page Title="Email directly from DMZ" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Email2.aspx.cs" 
    Inherits="dev_Email2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Send Email (Direct from DMZ)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <p>To<br /><asp:TextBox ID="txTo" runat="server" Width="400" Text="s_d_carlson@yahoo.com" /></p>
    <p>Subject<br /><asp:TextBox ID="txSbj" runat="server" Width="400" Text="Test directly from DMZ Server" /></p>
    <p>Message<br /><asp:TextBox ID="txMsg" runat="server" TextMode="MultiLine" Width="400" Height="200" Text="Test message from DMZ server" /></p>
    <p><asp:Button ID="btEmail" runat="server" Text="Send Email" onclick="btEmail_Click" /></p>
    <p><asp:Label ID="lbMessage" runat="server" Text=""></asp:Label></p>

</asp:Content>


