<%@ Page Title="Captcha Test" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="CaptchaTest.aspx.cs" 
    Inherits="dev_CaptchaTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Captcha Test
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <asp:Image ID="imCaptcha" runat="server" ImageUrl="~/dev/Captcha.aspx" />
    <br /><br /><asp:TextBox ID="txCaptcha" runat="server" />
    <br /><br /><asp:Button ID="btCaptcha" runat="server" Text="Submit" onclick="btCaptcha_Click" />
    <br /><br /><asp:Label ID="lbCaptcha" runat="server" />
</asp:Content>
