<%@ Page Title="STS: Onsite Maintenance Services" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="Onsite.aspx.cs" Inherits="public_Onsite" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    Onsite Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive" class="active"><a style="pointer-events:none;">Onsite Maintenance</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive"><a href="/public/ScantronProducts.aspx">Scantron Scanner Service</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl03_ddActive"><a href="/public/Depot.aspx">Depot Repair Services</a></dd>
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl04_ddActive"><a href="/public/SelfService.aspx">Cooperative Self Service</a></dd>
    </dl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <h1>Nationwide Local Onsite Service</h1>
<p>Universally, organizations are evaluating ways to manage costs and balance demand for reliable service delivery.  Scantron Technology Solutions realizes this challenge and offers flexible maintenance solutions at a reasonable cost to organizations. With our onsite service, you can be assured that any IT issues you encounter will be diagnosed and repaired quickly.</p>

<h4>Local Technicians</h4>

<p>We have been providing onsite hardware maintenance services for over 40 years. We have technicians located in strategically selected locations across the United States. With our national footprint, we are able to deliver high quality service with fast response times for our customers.</p>

<h4>Local Parts</h4>
<p>Not only do we have a local presence, but we have local inventory as well. Based on the needs of the customers in each Field Service Technician's territory, a local parts inventory is stocked. These inventories are monitored, and as service requirements change due to the addition and deletion of equipment, we will make the necessary adjustments to inventory to ensure part availability.</p>

<h4>Technical Phone Support</h4>
<p>Scantron Technology Solutions provides toll free (800) phone support between the hours of 7:00 a.m. and 7:00 p.m. CST, Monday through Friday, excluding holidays.  Our customers can call and get direct access to an expert on their hardware and system software.  Our performance and response to our customers' needs is the reason for our success.</p>
    
<h4>Online Call Tracking</h4>
<p>We use the latest technology to route your service call to a technician in your area quickly and efficiently. Online call placement with <asp:LinkButton ID="lkServiceCommand" runat="server" PostBackUrl="~/private/shared/Menu.aspx">ServiceCOMMAND®</asp:LinkButton> drastically reduces call routing time and ensures a quick response time for you, the customer. </p>

<h4>Fixed Costs</h4>
<p>With onsite services from Scantron Technology Solutions, your IT costs will not fluctuate from month-to-month. Our maintenance agreements include all parts, labor and travel expenses. </p>

<h4>Preventive Maintenance</h4>
<p>Scantron Technology Solutions' onsite maintenance program also includes preventive maintenance cleaning and inspections.  These "PMs" increase uptime and prolong the life of your equipment.  We recognize that in the long run, this represents a cost savings to us as well as our customers.</p>

<p>During our scheduled preventive maintenance routines, we completely clean and inspect the entire system and replace any worn parts.  </p>

<h4>Failure Preparation Routine</h4>
<p>In today's fast-paced environment, no single incident can be as crippling to an organization as a failed server. Organizations that take a proactive approach to protect themselves will be able to recover quickly in a time of disaster.</p>

<p><asp:Image ID="imAdobePdf" runat="server" CssClass="down3" ImageUrl="~/media/scantron/images/support/AdobeReader16.png" />
    &nbsp;
<asp:HyperLink ID="HyperLink1" runat="server" 
        NavigateUrl="~/media/scantron/pdf/HTSMaintenance2016.pdf"
        Target="_blank"
        Text="Failure Preparation and Preventive Maintenance Schedule" />
</p>

    <p style="font-weight: bold;"><asp:HyperLink ID="hlContact" runat="server" Text="Contact STS" NavigateUrl="~/public/Contact.aspx" /> to control equipment repair costs and protect your environment.</p>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>


