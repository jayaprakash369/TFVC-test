<%@ Page Title="Timeout Has Begun" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Timeout.aspx.cs" 
    Inherits="public_error_Timeout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
Your current web use has exceeded our maximum limit.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div style="font-size: 20px; color: #3A7728; margin-bottom: 30px;">
For security purposes, we have set a limit to user access.
</div>

<p>Normal access will be restored following two hours of inactivity. 
<br />If the site is accessed during the timeout period, the timeout is restarted from that point.
<br />
<br />If you feel this timeout has been triggered in error, please contact our call center at 1.800.228.3628 and ask to speak with a web administrator. 
</p>

</asp:Content>

