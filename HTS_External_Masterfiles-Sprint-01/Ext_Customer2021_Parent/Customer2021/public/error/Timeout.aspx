<%@ Page Title="Timeout" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="Timeout.aspx.cs" 
    Inherits="public_error_Timeout" %>

<asp:Content ID="Content3" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" runat="server">
    Timeout
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

  <div class="w3-row w3-padding-32">
    <div class="w3-twothird w3-container">
        <h3 class="w3-text-teal">Your current web use has exceeded our maximum limit.</h3>
        <p>Normal access will be restored following several hours of inactivity.</p>
        <p>If the site is accessed during the timeout period, the timeout is restarted from that point.</p>
        <p>If you feel this timeout has been triggered in error, please reach out to <a style='color: blue;' href="mailto:servicecommandsupport@scantron.com">ServiceCOMMAND Support</a> for help.</p>
    </div>
    <div class="w3-third w3-container">
    </div>
   </div>
</div>
</asp:Content>

