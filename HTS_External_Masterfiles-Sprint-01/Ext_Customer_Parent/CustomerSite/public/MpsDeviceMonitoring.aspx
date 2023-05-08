<%@ Page Title="STS: Monitoring Your Devices" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="MpsDeviceMonitoring.aspx.cs" 
    Inherits="public_MpsDeviceMonitoring" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span class="bannerTitleDark">MPS Device Monitoring</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive"><a href="/public/ManagedPrint.aspx">Managed Print</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive" class="active"><a style="pointer-events:none;">Monitoring Your Devices</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <h1>Environment Dashboards for Better Decision Making</h1>

    <p><b>Dashboard View</b></p>
    <p>Our MPowerPrint solution gives you dashboard views of your printer environment. This view, Device View, shows each of your location's monitored print devices, the supplies levels, overall status, page count, serial number, asset number, IP address and location.</p>
    <p style="text-align: center; padding-bottom: 40px;"><asp:Image ID="Image1" runat="server" ImageUrl="~/media/scantron/images/support/MpsDeviceDetail.jpg" /></p>
    
    <p><b>Device Detail View</b></p>
    <p>The Device Detail View allows MPowerPrint customers the ability to drill down to see specific details of a monitored device. You can also see the device image if it's available.</p>
    <p>The lower area of the Device Detail View has tabs for access complete meter breakdowns, supply levels, service information and model information.</p>
    <p style="text-align: center; padding-bottom: 40px;"><asp:Image ID="Image2" runat="server" ImageUrl="~/media/scantron/images/support/MpsTechnicalView.jpg" /></p>
        
    <p><b>Maps View</b></p>
    <p>Scantron Technology Solutions is able to upload floor plans for a monitored site and plot the specific locations of document output devices, computer devices, and people to give our MPowerPrint customers a more complete visual within a location allowing for better device to employee management.</p>
    <p style="text-align: center; padding-bottom: 40px;"><asp:Image ID="Image3" runat="server" ImageUrl="~/media/scantron/images/support/MpsMapView.jpg" /></p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>

