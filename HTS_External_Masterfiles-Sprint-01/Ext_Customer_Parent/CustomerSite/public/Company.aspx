<%@ Page Title="STS: Company" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="Company.aspx.cs" 
   Inherits="public_Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_A" Runat="Server">
    About Us
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <p>Scantron Technology Solutions is a division of Scantron Corporation, which is part of the Harland Clarke Holdings Corp. family of companies. We are a leading provider of IT services and solutions across the United States.</p>
    <h4>Scantron Technology Solutions provides:</h4>
    
    <div style="line-height: 25px; margin-left: 15px; margin-bottom: 10px;">
        Onsite Repair Service
        <br />Depot Repair Service
        <br />MPowerPrint®– Managed Print Services
        <br />ITManager– Managed Services 
            <ul class="ulDefault">
                <li>Net Admin Service</li>
                <li>Backup and Recovery Services</li>
                <li>Security Services</li>
            </ul>
        Scantron Proprietary Hardware Services
        <br />Implementation Services 
        <br />Supply Sales
        <br />Cooperative Self Service
    </div>

    <p>Our mission is to empower growth through intelligent, mission-critical assessment, technology, and data capture solutions for business, education, and certification clients worldwide.</p>

    <p>We provide IT solutions for over 10,000 customers across the United States. With over 165 strategically located service centers, we are able to provide consistent, reliable service from coast to coast. Scantron Technology Solutions is centrally headquartered in Omaha, NE.</p>

    <p>Our customers range from small to mid-sized enterprises across a variety of industries, including professional services, retail, government, financial services and healthcare offices--providing routine, remote and proactive support.</p> 

    <p>In today's fast paced, ever-changing technology environment, you need a trusted service and support provider that will keep your organization up and running with minimal downtime. We will take a proactive approach regarding your IT services and support. Whether you have computer hardware maintenance and support, or any of our Managed Services solutions, we are dedicated to providing the best possible experience for you. From call placement to call completion, we strive for an above average customer experience.</p>

    <p>Scantron Technology Solutions is also the provider of maintenance services for Scantron proprietary hardware. For more information about our services for Scantron proprietary hardware.  For more information about our services for Scantron proprietary hardware, please <asp:HyperLink ID="hlContact" runat="server" Text="click here" NavigateUrl="~/public/Contact.aspx" />.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>
