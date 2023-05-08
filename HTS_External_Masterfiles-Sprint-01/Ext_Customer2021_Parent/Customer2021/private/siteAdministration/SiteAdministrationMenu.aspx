<%@ Page Title="Site Administration Utilities" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="SiteAdministrationMenu.aspx.cs" 
    Inherits="private_siteAdministration_SiteAdministrationMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Site Administration Utilities
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32">

    <asp:Panel ID="pnError" runat="server" Visible="false">
        <div style="padding-left: 20px; padding-bottom: 10px;">
            <asp:Label ID="lbError" runat="server" SkinID="labelMessage" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnAdminAccessCode" runat="server" Visible="true">
        <div style="padding-left: 20px; padding-bottom: 10px;">
            <asp:Label ID="lbAdminAccessTitle" runat="server" SkinID="labelMessage" Text="Admin Access Code: " />&nbsp;&nbsp;<asp:Label ID="lbAdminAccessCode" runat="server" />
        </div>
    </asp:Panel>

  <div class="w3-row">
    <div class="w3-twothird w3-container">
        <p style="margin-bottom: 20px;"><span data-feather="disc" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink8" runat="server" Text="Buttons" NavigateUrl="~/private/siteAdministration/samples/Buttons.aspx" /><br />Examples of how provided button types will appear</p>
        <p style="margin-bottom: 20px;"><span data-feather="minus-circle" style="margin-right: 10px;"></span><asp:LinkButton ID="lkClearRegistrationLock" runat="server" Text="Clear Locked Registration" OnClick="lkClearRegistrationLock_Click" /><br />Clear lock for valid customers having trouble registering themselves.</p>
        <p style="margin-bottom: 20px;"><span data-feather="check-square" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink14" runat="server" Text="Manual New User Confirmation" NavigateUrl="~/private/siteAdministration/ManualNewUserConfirmation.aspx" /><br />New user having email confirmation problems? You can confirm their account for them using  this tool.</p>
        <p style="margin-bottom: 20px;"><span data-feather="type" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink9" runat="server" Text="Fonts" NavigateUrl="~/private/siteAdministration/samples/Fonts.aspx" /><br />Examples of how default fonts will appear</p>
        <p style="margin-bottom: 20px;"><span data-feather="eye" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink15" runat="server" Text="Hacker Watch" NavigateUrl="~/private/siteAdministration/hitCountByIpAddress.aspx" /><br />View count of page calls from the same IP address. The purpose is to watch for a hacker programmatically hammering our site. Blacklisting will not stop them, but always send them to an error page to discourage them. </p>
        <p style="margin-bottom: 20px;"><span data-feather="bar-chart-2" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink16" runat="server" Text="Hit Count By Page" NavigateUrl="~/private/siteAdministration/hitCountByPage.aspx" /><br />View which pages are accessed the most and the least.</p>
        <p style="margin-bottom: 20px;"><span data-feather="mail" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink7" runat="server" Text="FP Mailing Maintenance Coverage" NavigateUrl="~/public/sc/CoverageFPM.aspx" /><br />Copy of our maintenance coverage page redesigned with another customer's look and feel for use from their site.</p>
        <p style="margin-bottom: 20px;"><span data-feather="anchor" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink10" runat="server" Text="Icons" NavigateUrl="~/private/siteAdministration/samples/Icons.aspx" /><br />Options for icons, primarily to use with menu items</p>
        <p style="margin-bottom: 20px;"><span data-feather="image" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink1" runat="server" Text="Photo Backgrounds" NavigateUrl="~/private/siteAdministration/samples/Backgrounds.aspx" /><br />Page using a photo background</p>
        <p style="margin-bottom: 20px;"><span data-feather="sidebar" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink2" runat="server" Text="Thirds" NavigateUrl="~/private/siteAdministration/samples/Thirds.aspx" /><br />Pages holding data in containers taking a third, or two thirds of the row</p>
        <p style="margin-bottom: 20px;"><span data-feather="grid" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink6" runat="server" Text="Page Grid Options" NavigateUrl="~/private/siteAdministration/samples/GridOptions.aspx" /><br />Working demo of the various ways elements can be laid out on a screen.</p>
    </div>
    <div class="w3-third w3-container">
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="copy" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink13" runat="server" Text="Basic Page Template" NavigateUrl="~/private/siteAdministration/samples/BasicTemplate.aspx" /><br />A sample starter template with search capability to be copied for building future pages.</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="sunrise" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink3" runat="server" Text="Template To Show/Hide Items" NavigateUrl="~/private/siteAdministration/samples/TemplateShowHide.aspx" /><br />Template with sections to be displayed sized for phone, tablet or desktop</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="cloud" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink11" runat="server" Text="Template With White Panel" NavigateUrl="~/private/siteAdministration/samples/TemplateWhitePanel.aspx" /><br />Template with a simple white panel</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="cloud-rain" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink12" runat="server" Text="Template With White Panel and Grey Box" NavigateUrl="~/private/siteAdministration/samples/TemplateWhitePanelWithGrayBox.aspx" /><br />Template with a simple white panel and an initial grey inner box</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="codepen" style="margin-right: 10px;"></span><asp:HyperLink ID="hlQuery" runat="server" Text="Sample Query" NavigateUrl="~/private/siteAdministration/samples/SampleQuery.aspx" /><br />Template for a direct query</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="credit-card" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink4" runat="server" Text="Sample Tables" NavigateUrl="~/private/siteAdministration/samples/SampleTables.aspx" /><br />Template for a responsive tables.  Since most customer site content is table based I built a sample tables which properly appear/disappear depending on the screen display size.</p>
        <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><span data-feather="droplet" style="margin-right: 10px;"></span><asp:HyperLink ID="HyperLink5" runat="server" Text="Sample Text Colors" NavigateUrl="~/private/siteAdministration/samples/SampleTextColors.aspx" /><br />Available CSS colors</p>
    </div>
  
    </div>
    </div>

</div>
</asp:Content>

