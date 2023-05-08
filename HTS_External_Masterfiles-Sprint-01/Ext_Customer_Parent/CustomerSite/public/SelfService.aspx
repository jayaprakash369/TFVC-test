<%@ Page Title="STS: Cooperative Self Service" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="SelfService.aspx.cs" 
    Inherits="public_SelfService" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span class=bannerTitleDark>Cooperative Self Service</span> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive"><a href="/public/Onsite.aspx">Onsite Maintenance</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive"><a href="/public/ScantronProducts.aspx">Scantron Scanner Service</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl03_ddActive"><a href="/public/Depot.aspx">Depot Repair Services</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl04_ddActive" class="active"><a style="pointer-events:none;">Cooperative Self Service</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <h1>Leverage Your Internal Support Team</h1>

    <p>If you have internal resources but need access to quality parts, components, and technical phone support, Scantron Technology Solutions Cooperative (Co-op) Self Service solution is the right program for you.</p>

    <p>Our Co-op Self Service solution recognizes your need for self maintenance and Scantron Technology Solutions' has the ability to efficiently support this segment of the market.</p>

    <p>Over the last 40 years, we have become a premier service organization that provides onsite service agreements for our customer's computer networks. We have built a highly efficient infrastructure to support our own nationwide onsite field service force on multiple types of systems and printers. This support infrastructure combines the elements of:</p>
        <ul class="ulDefault">
            <li>Technical phone support</li>
            <li>Parts</li>
            <li>Training</li>
            <li>Program management systems</li>
            <li>Network monitoring</li>
            <li><asp:LinkButton ID="lkServiceCommand" runat="server" PostBackUrl="~/private/shared/Menu.aspx">ServiceCOMMAND</asp:LinkButton> online call placement and tracking</li>
        </ul>

    <p>Co-op Self Service is available for self maintainers across the U.S. We provide the support and infrastructure and your company provides the technician(s).  The Co-op Self Service solution can increase an organization's uptime and productivity, while reducing overall costs.</p>

    <p style="font-weight: bold;"><asp:HyperLink ID="hlContact" runat="server" Text="Contact STS" NavigateUrl="~/public/Contact.aspx" /> to control equipment repair costs and protect your environment.</p>
    
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>


