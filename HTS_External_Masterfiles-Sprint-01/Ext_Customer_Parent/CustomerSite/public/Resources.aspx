<%@ Page Title="STS: Resources" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="Resources.aspx.cs" 
    Inherits="public_Resources" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span class="bannerTitleDark">Resources</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
<p>Below we offer a variety of resources to help you learn more about Scantron Technology Solutions and our solutions.</p>
<asp:HyperLink ID="hlTechnologies" runat="server" 
     NavigateUrl="~/public/Technologies.aspx"
     Text="Technologies" />
<p>We offer technology solutions for over 260,000 pieces of hardware. We have the capabilities to provide services across markets for POS equipment, PCs, Check Scanners, Printers, Servers and technology peripherals.</p>

<asp:HyperLink ID="hlSecurity" runat="server" 
     NavigateUrl="~/public/Security.aspx"
     Text="Security and Compliance" />
<p>STS' Data Collection Agent(DCA), used to manage your Printer Fleet, is completely secure and compliant. Find more information about the DCA here.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>

