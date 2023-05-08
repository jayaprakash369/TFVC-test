<%@ Page Title="Thirds" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Thirds.aspx.cs" 
    Inherits="private_siteAdministration_samples_Thirds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Thirds
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32">
        <div class=" w3-container">

    <div class="w3-twothird w3-container">
      <h3 class="w3-text-teal">Breaking the page into thirds</h3>
              <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum
        dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
              <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum
        dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
    </div>
    <div class="w3-third w3-container">
      <p style="margin-bottom: 20px;"  class="w3-border w3-padding-large w3-padding-32 w3-center">Extra Text Here</p>
      <p style="margin-bottom: 20px;" class="w3-border w3-center"><asp:Image ID="Image1" runat="server" ImageUrl="~/media/images/WhiteTree.png" style="width:100%" /></p>
    </div>
  </div>
<!-- ====================================================================================== -->
  <div class="w3-row" style="margin-bottom: 20px;">
    <div class="w3-third w3-container">
      <p style="margin-bottom: 20px;" class="w3-border w3-padding-large w3-padding-32 w3-center"><asp:Image ID="Image2" runat="server" ImageUrl="~/media/images/frog.png" style="width:100%" /></p>
    </div>
    <div class="w3-twothird w3-container">
      <h3 class="w3-text-purple">Other Ideas Here</h3>
      <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum
        dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
    </div>

    <!-- ====================================================================================== -->
  </div>
</div>

</div>
</asp:Content>

