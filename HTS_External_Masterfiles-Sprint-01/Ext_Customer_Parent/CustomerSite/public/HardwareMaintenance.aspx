<%@ Page Title="STS: Hardware Maintenance" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="HardwareMaintenance.aspx.cs" Inherits="public_HardwareMaintenance" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    Hardware Services
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl00_ddActive"><a href="/public/ManagedServices.aspx">Managed Services</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive"><a href="/public/MpsBenefits.aspx">Managed Print</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive" class="active"><a  style="pointer-events:none;">Hardware Services</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl03_ddActive"><a href="/public/Implementation.aspx">Implementation Services</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl04_ddActive"><a href="/public/Supplies.aspx">Consumable Products</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <h1>Make One Call to Maintain or Replace All Hardware</h1>
    <p>Our comprehensive hardware maintenance service solutions are designed to protect your multivendor environment. We provide you with a single point of accountability with efficient call processing. Our highly trained technical support team can assess and help resolve these issues rapidly and conveniently.</p>

    <p>Whether you are looking for maintenance services on your network, servers, printers, or a combination of those three, we have a flexible solution that can be tailored to your organization's individual needs.</p>

    <p>Our multivendor hardware services include support for major Operating System platforms such as Windows, UNIX, Citrix, Linux and others. Several services can be combined to produce the optimum solution.</p>

    <h4>Highlights</h4>
    <ul class="ulDefault">
        <li>Fixed monthly costs</li>
        <li>Problem management with dedicated Field Service Technicians</li>
        <li>Minimize costly downtime and system outages</li>
        <li>Consistent services and solutions provided nationwide</li>
    </ul>
    
    <p style="font-weight: bold;"><asp:HyperLink ID="hlContact" runat="server" Text="Contact STS" NavigateUrl="~/public/Contact.aspx" /> to safely delegate your hardware management tasks and protect your environment.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>
