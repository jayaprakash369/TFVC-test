<%@ Page Title="Customer Administration Menu" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="CustomerAdministrationMenu.aspx.cs" 
    Inherits="private_customerAdministration_CustomerAdministrationMenu" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Customer Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

  <div class="w3-row w3-padding-32">
    <div class="w3-twothird w3-container">
      <p style="margin-bottom: 20px;"><span data-feather="users" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink8" runat="server" Text="User Maintenance" NavigateUrl="~/private/customerAdministration/UserMaintenance.aspx" /><br />Update information for the accounts used by your employees to access this web site. This includes contact information, phone and email, locking and unlocking accounts, as well as the ability to change user passwords. Take care to remember password changes. These are stored in an encrypted manner and once entered they cannot be retrieved if forgotten. (Though they may be reset again using this utility)</p>
      <p style="margin-bottom: 20px;"><span data-feather="clipboard" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink9" runat="server" Text="User Registration" NavigateUrl="~/Registration.aspx" /><br />Create accounts for your employees to use ServiceCOMMAND<span style='font-size: 12px; vertical-align: top; position: relative; top: 1px;'>®</span> utilities. Once created, these accounts can be managed through the User Maintenance utility.</p>
      <p style="margin-bottom: 20px;"><span data-feather="info" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink1" runat="server" Text="Change Contact Information" NavigateUrl="~/private/customerAdministration/ChangeContactInformation.aspx" /><br />Update the contact information for any location of your parent site.</p>
      <p style="margin-bottom: 20px;"><span data-feather="link" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink10" runat="server" Text="Change Your Password" NavigateUrl="~/private/ChangeYourPassword.aspx" /><br />Update your own password.</p>
      <p style="margin-bottom: 20px;"><span data-feather="toggle-right" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink2" runat="server" Text="Customer Preferences" NavigateUrl="~/private/customerAdministration/CustomerPreferences.aspx" /><br />Provides the ability to open or close registration to allow others to complete individual registration. </p>
        <asp:Panel ID="pnCinemark" runat="server">  <p style="margin-bottom: 20px;"><span data-feather="toggle-right" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink4" runat="server" Text="Outstanding Equipment" NavigateUrl="~/private/customerAdministration/OutstandingEquipment.aspx" /><br />Provides a list of all Outstanding Equipment that has not been received in our warehouse. </p></asp:Panel>
    </div>
    <div class="w3-third w3-container" style="border: 1px solid #dddddd;">
        <h4 style="color: #777777"><asp:Label ID="lbManagedPrintSectionTitle" runat="server" Text="Managed Print Utilities" /></h4>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="layers" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink3" runat="server" Text="Page Count History" NavigateUrl="~/private/customerAdministration/mp/PageCountsForCustomer.aspx" /><br />View page counts by month for your entire printer fleet.</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="printer" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink11" runat="server" Text="Page Counts By Device" NavigateUrl="~/private/customerAdministration/mp/PageCountsByDevice.aspx" /><br />View page counts by month for a specific device.</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="bar-chart" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink12" runat="server" Text="Device Utilization" NavigateUrl="~/private/customerAdministration/mp/DeviceUtilization.aspx" /><br />View most and least utilized devices within your fleet.</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="edit" style="margin-right: 10px;"></span><asp:HyperLink ID="hlQuery" runat="server" Text="Update Toner Contact" NavigateUrl="~/private/customerAdministration/mp/UpdateTonerContact.aspx" /><br />Update the contact to receive toner replenishment for a specific device.</p>
    </div>
  </div>
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
        <%--  --%>
</div>
</asp:Content>
