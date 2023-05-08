<%@ Page Title="Website Lockout" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Lockout.aspx.cs" 
    Inherits="public_error_Lockout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
Access to our website is now locked.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div style="font-size: 20px; color: #3A7728; margin-bottom: 30px;">
For security purposes, we have set a limit to user access.
</div>

<p>Due to repeated timeouts, we have blocked access from this source to our website.
<br />
<br />If you feel this lockout has been triggered in error, please contact our call center at 1.800.228.3628 and ask to speak with a web administrator. 
</p>

</asp:Content>

