<%@ Page Title="Page Not Found" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="404_PageNotFound.aspx.cs" 
    Inherits="public_error_404_PageNotFound" %>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
Page Not Found
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div style="font-size: 20px; color: #3A7728; padding-top: 15px; padding-bottom: 15px;">
Sorry, <asp:Label ID="lbPageNotFound" runat="server" /> was not found. 
</div>

<p>We are continually updating our web site.
<br/>Perhaps the page was bookmarked from an older version, or may be typed incorrectly.
<br/>Regardless, we're sorry for the inconvenience. 
<br />Please use our navigation menu to search for the desired page. 
</p>

</asp:Content>

