<%@ Page Title="STS: Managed Print Services" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="ManagedPrint.aspx.cs" Inherits="public_ManagedPrint" %>
<%--     <div style="position: relative; top: 7px; left: 15px;">
    </div>
 --%>    
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    Managed Print
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive" class="active"><a style="pointer-events:none;">Managed Print</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive"><a href="/public/MpsDeviceMonitoring.aspx">Monitoring Your Devices</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
<h1 style="margin-top: 15px;">Reduce Printing Costs and Increase Service Levels</h1>
     
    <div class="spacer15"></div>
     <asp:Image ID="imStaticBanner" runat="server" 
        ImageUrl="~/media/scantron/images/logos/company/MPowerPrintHeader.png" />
    <div class="spacer15"></div>

    <div style="font-style:italic; margin-bottom: 20px; margin-left: 25px; font-size: 14px; line-height: 23px;">
            MPower your finance team with data.
            <br />MPower your IT team with time.
            <br />MPower your employees with maximum uptime.
    </div>

    <p>With Scantron Technology Solutions' MPowerPrint Managed Print Service, we provide custom solutions for all of your print fleet needs. We can help you manage your printing costs by using our secure, innovative printer monitoring software. This software helps us deliver pro-active printer service, toner replenishment and strategic fleet planning on a cost-per-page basis. It allows us to provide you with real, tangible data about your print fleet. Scantron Technology Solutions will assess and deploy a managed print service solution to help minimize the ongoing expenses of your organization's print environment.</p>
  
 <p><asp:HyperLink ID="HyperLink1" SkinID="hyperLinkHeader" runat="server" Text="Download the MPowerPrint brochure." NavigateUrl="~/media/scantron/pdf/lib/MPowerPrint_Brochure.pdf" /></p>

    <b>Using our powerful MPowerPrint remote monitoring software, we can help you:</b>
        <ul class="ulDefault">
            <li>Maximize uptime</li>
            <li>Proactively maintain devices</li>
            <li>Predict printing expenses</li>
            <li>Maximize the ROI of your print technology</li>
        </ul>

    <p>MPowerPrint is an inclusive cost-per-page program that helps manage print volumes and measure toner supply for easy auto-replenishment. </p>

    <b>Our complete managed print services program includes:</b>
        <ul class="ulDefault">
            <li>Onsite device maintenance</li>
            <li>Proactive toner replenishment</li>
            <li>Assessment/advice/support</li>
            <li>One solution, One monthly invoice</li>
        </ul>

    <p style="font-weight: bold;"><asp:HyperLink ID="hlContact" runat="server" Text="Contact STS" NavigateUrl="~/public/Contact.aspx" /> to inquire about your potential for cost savings using MPowerPrint.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>
