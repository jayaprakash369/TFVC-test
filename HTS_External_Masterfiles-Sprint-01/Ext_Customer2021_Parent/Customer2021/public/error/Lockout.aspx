<%@ Page Title="Lockout" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="Lockout.aspx.cs" 
    Inherits="public_error_Lockout" %>

<asp:Content ID="Content3" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" runat="server">
    Lockout
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

  <div class="w3-row w3-padding-32">
    <div class="w3-twothird w3-container">
        <h3 class="w3-text-teal">Website access is now locked.</h3>
        <p>For security purposes, we have set a limit to user access.</p>
        <p>Since these limits appear to have been exceeded, we have blocked access to our website.</p>
        <p>If you feel this lockout has been triggered in error, please reach out to <a style='color: blue;' href="mailto:servicecommandsupport@scantron.com">ServiceCOMMAND Support</a> for help.</p>
    </div>
    <div class="w3-third w3-container">
    </div>
   </div>
</div>
</asp:Content>

