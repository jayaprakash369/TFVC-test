<%@ Page Title="Geocode Data" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Geocode.aspx.cs" 
    Inherits="private_tools_Geocode" %>
<%--
        <asp:ListItem Text="Google" Value="GOOGLE"/>
        <asp:ListItem Text="GeoCoder" Value="GEOCODER" />
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
Geocode Data
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <asp:DropDownList ID="ddSite" runat="server">
        <asp:ListItem Text="Yahoo" Value="YAHOO" Selected="True" />
    </asp:DropDownList>
<br />
<br />
    <asp:DropDownList ID="ddCodeQty" runat="server">
        <asp:ListItem Text="1" Value="1" />
        <asp:ListItem Text="2" Value="2" />
        <asp:ListItem Text="3" Value="3" />
        <asp:ListItem Text="5" Value="5" />
        <asp:ListItem Text="10" Value="10"  Selected="True" />
        <asp:ListItem Text="20" Value="20" />
        <asp:ListItem Text="50" Value="50" />
        <asp:ListItem Text="100" Value="100" />
        <asp:ListItem Text="250" Value="250" />
        <asp:ListItem Text="500" Value="500" />
        <asp:ListItem Text="750" Value="750" />
        <asp:ListItem Text="1,000" Value="1000" />
        <asp:ListItem Text="2,000" Value="2000" />
        <asp:ListItem Text="5,000" Value="5000" />
        <asp:ListItem Text="10,000" Value="10000" />
        <asp:ListItem Text="20,000" Value="20000" />
        <asp:ListItem Text="40,000" Value="40000" />
        <asp:ListItem Text="49,000" Value="49000" />
    </asp:DropDownList>
    <br /><br />
    <asp:Button ID="btCodeRun" runat="server" Text="Run Next Set" 
        onclick="btCodeRun_Click" />

    <br /><br />
    <asp:Literal ID="ltGeoCoder" runat="server" Text="Geocoder 192 per day max -- Example url: http://geocoder.ca/?locate=A0A1G0" Mode="Encode" />
    <br /><br />
    <asp:Literal ID="ltGoogle" runat="server" Text="Google 2,500 per day max --Example url: http://maps.googleapis.com/maps/api/geocode/xml?address=A0A1G0&sensor=false&region=ca" Mode="Encode" />
    <br /><br />
    <asp:Literal ID="ltYahoo" runat="server" Text="Yahoo 50,000 per day max --Example url: http://where.yahooapis.com/geocode?q=1600+Pennsylvania+Avenue,+Washington,+DC&appid=[yourappidhere]" Mode="Encode" />

    <br /><br />
    <asp:Label ID="lbNextCode" runat="server" />

    <asp:Panel ID="pnResults" runat="server" Visible="false">
        <br /><br />
        <asp:Label ID="lbRecs" runat="server" />
        <br /><br />
        <asp:Label ID="lbTime" runat="server" />
        <br /><br />
        <asp:Label ID="lbMsg" runat="server" />
        <br /><br />
        <asp:Literal ID="ltPageSource" runat="server" Mode="Encode" />
    </asp:Panel>

</asp:Content>


