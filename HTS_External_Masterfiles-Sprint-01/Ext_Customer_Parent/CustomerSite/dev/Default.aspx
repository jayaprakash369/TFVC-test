<%@ Page Title="Development Programs" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="dev_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Development
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <p><asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/dev/Email1.aspx" Text="Email through Web Service" /></p>
    <p><asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/dev/Email2.aspx" Text="Email directly from DMZ" /></p>
    <p><asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/dev/CaptchaTest.aspx" Text="Captcha Test" /></p>
    <p><asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/dev/DoubleSubmit.aspx" Text="Double Submit" /></p>
</asp:Content>

