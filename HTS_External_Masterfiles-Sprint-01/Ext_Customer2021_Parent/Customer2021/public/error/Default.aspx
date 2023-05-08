<%@ Page Title="Error Page" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="public_error_Default" %>

<asp:Content ID="Content3" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" runat="server">
    Error Page
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

  <div class="w3-row w3-padding-32">
    <div class="w3-twothird w3-container">
        <h3 class="w3-text-teal">Sorry, an error has occurred...</h3>
        <p>Unfortunately an error has occurred during the processing of your page request. </p>
        <p>Please be assured we log and review all errors, and we will endeavor to correct it.</p>
    </div>
    <div class="w3-third w3-container">
    </div>
   </div>
</div>
</asp:Content>

