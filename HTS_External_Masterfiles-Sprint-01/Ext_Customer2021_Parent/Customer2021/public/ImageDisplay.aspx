<%@ Page Title="Image Display" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ImageDisplay.aspx.cs" 
    Inherits="public_ImageDisplay" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
	    <style type="text/css">
        .imageFormat {
            width: 100%; 
            /*max-width: 1600px; */
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    <asp:Label ID="lbPageTitle" runat="server" Text="Image Display" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
	<div class="bodyPadding">
		<div class="w3-row w3-padding-32">
            <asp:Image ID="imRequested" runat="server" CssClass="imageFormat"></asp:Image>
		</div>
	    
        <%--  --%>
	</div>
</asp:Content>
