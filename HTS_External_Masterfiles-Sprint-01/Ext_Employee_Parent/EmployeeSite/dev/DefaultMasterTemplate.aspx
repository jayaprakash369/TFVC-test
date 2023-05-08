<%@ Page Title="Default Using Template Masterpage" Language="C#" MasterPageFile="~/MasterTemplate.master" AutoEventWireup="true" CodeFile="DefaultMasterTemplate.aspx.cs" Inherits="public_DefaultMasterTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Default using Template Masterpage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    Default content
    <br /><asp:Button ID="Button1" runat="server" Text="Click to reload Page" />
    <br /><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>

