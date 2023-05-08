<%@ Page Title="STS: Call Escalation" Language="C#" MasterPageFile="~/Scantron_Body_A_Nav.master" AutoEventWireup="true" CodeFile="Escalation.aspx.cs" 
    Inherits="public_Escalation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
<span class="bannerTitleDark">Call Escalation</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
 Multiple Support Levels
 </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
Call escalation is part of our overall customer experience. Members of our Call Center team are empowered to escalate calls quickly and efficiently should the need arise. It is important to us to resolve issues as rapidly as possible.  Our published escalation chart can be accessed right here.
    <%-- ImageUrl="~/media/scantron/images/support/EscalationChart2019.png"  --%>    
<center>
<asp:Image ID="imEscalation" runat="server" 
      ImageUrl="~/media/scantron/images/support/EscalationChart_2020.png" />
</center>
</asp:Content>


