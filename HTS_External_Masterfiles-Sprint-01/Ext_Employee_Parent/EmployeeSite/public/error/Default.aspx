<%@ Page Title="Error Page" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="public_error_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
Error Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
<div style="font-size: 20px; color: #3A7728; margin-bottom: 30px;">
Sorry, an error has occurred...
</div>

<p>Unfortunately an error has occurred during the processing of your page request. 
<br />Please be assured we log and review all errors, and we will endeavor to correct it. </p>

</asp:Content>

