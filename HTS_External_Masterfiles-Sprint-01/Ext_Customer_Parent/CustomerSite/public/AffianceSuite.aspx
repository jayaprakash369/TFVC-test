<%@ Page Title="STS: AffianceSUITE support for Small and Medium Businesses" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="AffianceSuite.aspx.cs" 
    Inherits="public_AffianceSuite" %>
<%--  --%>    
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span style="text-transform:none;">affianceSUITE</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive" class="active"><a style="pointer-events:none;">Affiance SUITE</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive"><a href="/public/Authos.aspx">Authos</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <h1>Support for Small and Medium Businesses</h1>

    <div class="spacer15"></div>
     <asp:Image ID="imStaticBanner" runat="server" 
        ImageUrl="~/media/scantron/images/logos/company/affianceSUITE.png" />
    <div class="spacer15"></div>

    <p style="font-size: 18px; font-weight: bold; border: 1px solid #bbbbbb; padding: 10px; line-height: 24px; margin: 25px; margin-top: 20px;"><span style="color: #333333;">affiance. noun.</span><span style="color: #3399CC; padding-left: 20px;">af fi ance. \ə-ˈfī-ən(t)s\ : 1. Trust, confidence. 2. Feeling experienced by STS customers.</span></p>

    <p>As a manager in a small to medium sized organization, you have an endless list of responsibilities. Too often, your to-dos are IT-related and not essential to your core business. You may even spend more time diagnosing and fixing, or hunting down support, than evaluating and improving IT systems and processes. </p>
    <p>Move out of reactive mode and stop performing technology triage. Focus on your business and let us handle the IT. affianceSUITE™ is a subscription-based, comprehensive technology support service with unlimited access to our support team. You enjoy the confidence and trust that a reliable partner is handling your technology support needs.</p>

    <p><asp:HyperLink ID="HyperLink1" SkinID="hyperLinkHeader" runat="server" Text="Download the affianceSUITE brochure." NavigateUrl="~/media/scantron/pdf/lib/affianceSUITE_Brochure.pdf" /></p>

<h4>What's included in our offering?</h4>
    <ul class="ulPL">
        <li>Virtual helpdesk support</li>
        <li>Network monitoring and management: performance and availability</li>
        <li>Layered security: spam, malware, phishing detection (email level); continuous web content filtering & antivirus threat monitoring and response at firewall level; antivirus and anti-malware at desktop level and negative impacts to productivity</li>
        <li>Email and mobile device service and support</li>
        <li>Standard backup and recovery of systems</li>
        <li>Reporting on monthly utilization</li>
    </ul>

<h4>Advanced Services Available</h4>
        <ul class="ulPL">
        <li>Enhanced back up and disaster recovery
            <ul>
                <li>Cloud backup: virtualized images for rapid restoration of critical systems</li>
                <li>Remote backup to certified data center also available</li>
                <li>Email archiving per compliance requirements, offsite or cloud</li>
            </ul>
        </li>
        <li>Monthly remote vulnerability assessments with online reporting</li>
        <li>Internal vulnerability assessments with online reporting and comparison to standards</li>
        <li>Enhanced email functionality, encryption, journaling</li>
        <li>Data encryption </li>
    </ul>

<h4>Remove the Bottlenecks to Immediate Support</h4>
<p><strong>It's unlimited access.</strong> Empower your team to request help via phone, email or online, without worrying about incremental costs. We can remotely connect to your computers to tackle any issue that is disrupting business. </p>
<p>A live dispatch coordinator will qualify a problem and route you to the right person to efficiently resolve your problems.</p>

<h4>Detect, Diagnose and Resolve Problems Before They Impact You</h4>
<p>We monitor dozens of health points on your systems in real time. When we detect a problem, we work behind the scenes to resolve it. Unlike IT support providers with less extensive infrastructure, we always have a team member monitoring your network. When problems are detected, the support engineer receives an alert, diagnoses the situation and initiates resolution.</p>

<h4>Don't Be a Statistic: Prevent Internet and Email Attacks on Your Systems</h4>
<p>Using our industry-standard tools, all email is filtered for virus, spam, phishing attacks, and worms before they can enter your business systems. End point computers are protected by a managed and monitored anti-virus service. We fully test critical Microsoft security patches in our labs and apply them soon after release. Enjoy the highest level of protection.</p>

<h4>Backup Your Mission-Critical Systems and Data</h4>
<p>Make your data backup process foolproof. Never let outages or loss of data slow down or interrupt your business. We backup your systems for you automatically in the cloud or remotely, per your compliance requirements. Our secure datacenter stores all of your information where it is always available for restoration of everything from a single file to a complete system restoration. </p>

<h4>Security/Firewall Management</h4>
<p>You understand that your data is one of your most important assets. Next are your technology systems that control everything going on behind the scenes. Your customers, vendors, employees and partners all trust you to maintain the integrity and confidentiality of your data and systems. </p>
<p>affianceSUITE security and firewall monitoring and response blocks invasive software and intruder access from entering your network, allowing only legitimate communications to pass through, using the latest technology of filters, application layers and proxy servers. </p>

<p><strong><asp:HyperLink ID="hlContact" runat="server" Text="Contact STS" NavigateUrl="~/public/Contact.aspx" /> to assess your company's support needs and potential fit with affianceSUITE.</strong></p>
  
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>
