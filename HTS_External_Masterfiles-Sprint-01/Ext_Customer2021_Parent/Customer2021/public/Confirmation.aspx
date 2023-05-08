<%@ Page Title="Account Confirmation" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Confirmation.aspx.cs" 
    Inherits="public_Confirmation" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Account Confirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Label ID="lbSummary" runat="server" SkinID="labelError" />
        <div class="spacer20"></div>
        <asp:Label ID="lbDetail" runat="server" />
        <div class="spacer5"></div>
        <asp:HyperLink ID="hlLogin" runat="server" Text="Login" Visible="false" SkinID="hyperlinkLarge" NavigateUrl="~/Login.aspx" />


        <%--  --%>
    </div>
    <asp:HiddenField ID="hfRedirectToLoginPage_YN" runat="server" Value="N" />
</div>
</asp:Content>
