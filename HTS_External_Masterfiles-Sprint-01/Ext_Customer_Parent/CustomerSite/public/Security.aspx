<%@ Page Title="STS: Security" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="Security.aspx.cs" 
    Inherits="public_Security" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span class="bannerTitleDark">Security</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Data Security and Personally Identifiable Information
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

<asp:Label ID="Label1" runat="server" Text="The Secure Way to Gather Information and Manage your Printer Fleet" SkinID="labelTitleColor2_Medium" />

<p>Scantron Technology Solutions' Managed Print Services (MPS) software collects printer metric data at regular intervals from printer and multi-function peripheral devices on the network using the Data Collection Agent (DCA). The information collected by the DCA is obtained from the memory of devices, such as page counts, device description and device status. No personal or user data is collected with the DCA.</p>

<p>The DCA is a software application that is installed on a networked computer running Windows®  2000/XP or higher; it does not require a dedicated computer. The DCA runs as a Windows® service, allowing it to operate continuously. You can customize the transmission interval to determine how often the DCA will perform a device discovery. After each discovery, the collected data will be sent to the Scantron secure web server.</p>

<p>The DCA uses one-way communication; two-way communication can be enabled for Intelligent Updates. The collected data is sent via port 443, 80, or 21 to the Scantron Technology Solutions web server. The DCA collects device data using SNMP, ICMP, and HTTPS. Data can also be collected on some devices connected to external print servers; however, only one device per external print server can be detected.</p>

<p>The DCA has an Intelligent Update option that allows a Scantron Print Management administrator to post actions, such as DCA software updates or configuration changes, on the Scantron Technology Solutions MPowerPrint server for the DCA to receive. The Intelligent Update option is enabled and disabled at the end user location.</p>

<asp:Label ID="Label2" runat="server" Text="Compliance Overview" SkinID="labelTitleColor2_Medium" />

<p>Scantron Technology Solutions' MPS software applications will not have an impact on compliance with the Health Insurance Portability & Accountability Act (HIPAA), Sarbanes-Oxley, the Gramm-Leach-Bliley Act (GLBA), or the Federal Information Security Management Act (FISMA) for covered entities.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>

