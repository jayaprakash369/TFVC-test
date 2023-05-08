<%@ Page Title="Backgrounds" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Backgrounds.aspx.cs" Inherits="private_siteAdministration_samples_Backgrounds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">
    <!-- ====================================================================================== -->
    <!-- Image header -->

    <div class="w3-row w3-padding-32">
        <div class=" w3-container">

  <div class="w3-display-container w3-container">
    <asp:Image ID="imOrange" runat="server" ImageUrl="~/media/images/OrangeClouds.jpg" alt="Clouds" style="width:100%" />
    <div class="w3-display-topleft w3-text-white" style="padding:24px 48px">
      <h1 class="w3-jumbo w3-hide-small">Picture Background</h1>
      <h1 class="w3-hide-large w3-hide-medium">Alternate text for small screen</h1>
      <h1 class="w3-hide-small">Primary text when screen large</h1>
      <p><a href="/private/siteAdministration/SiteAdministrationMenu.aspx" class="w3-button w3-black w3-padding-large w3-large">Link Example Here</a></p>
    </div>
  </div>
    <!-- ====================================================================================== -->

        </div>
    </div>
</div>
</asp:Content>

