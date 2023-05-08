<%@ Page Title="Upload Files" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" 
    Inherits="private__admin_Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Upload Files
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<div style="clear: both; height: 5px;"></div>
<asp:Label ID="lbTarget" runat="server" Text="Uploaded files will be found in D:\OurSites\Uploads\" SkinID="labelTitleColor1_Small" />
<div style="clear: both; height: 10px;"></div>

<asp:FileUpload ID="FileUpload1" runat="server" />
<br />
<asp:Button ID="Button1" runat="server" Text="Upload File" onclick="Button1_Click" />
<br />
<br />
<asp:Label ID="Label1" runat="server" />


</asp:Content>

