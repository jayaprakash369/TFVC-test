<%@ Page Title="Account Confirmation Pending" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ConfirmationPending.aspx.cs" 
    Inherits="public_Confirmation" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Account Confirmation Pending
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Label ID="lbSummary" runat="server" SkinID="labelError" />
        <div class="spacer20"></div>
        <asp:Label ID="lbDetail" runat="server" />


        <%--  --%>
    </div>

</div>
</asp:Content>
