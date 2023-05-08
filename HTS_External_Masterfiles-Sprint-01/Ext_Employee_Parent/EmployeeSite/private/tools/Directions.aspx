<%@ Page Title="Driving Direction Loader" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Directions.aspx.cs" 
    Inherits="private_tools_Directions" %>
<%--
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
Driving Directions Loader
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:DropDownList ID="ddFilename" runat="server">
        <asp:ListItem Text="ZIP2FST" Value="ZIP2FST" />
        <asp:ListItem Text="ZIP2CTR" Value="ZIP2CTR" />
        <asp:ListItem Text="ZIP2MKT" Value="ZIP2MKT" />
    </asp:DropDownList>
    <br /><br />

    <asp:DropDownList ID="ddCountry" runat="server">
        <asp:ListItem Text="" Value="" />
        <asp:ListItem Text="CANADA" Value="CANADA" />
        <asp:ListItem Text="USA" Value="USA" />
    </asp:DropDownList>
    <br /><br />

    <asp:DropDownList ID="ddCodeQty" runat="server">
        <asp:ListItem Text="1" Value="1" />
        <asp:ListItem Text="2" Value="2" />
        <asp:ListItem Text="3" Value="3" />
        <asp:ListItem Text="5" Value="5" Selected="True" />
        <asp:ListItem Text="10" Value="10"/>
        <asp:ListItem Text="20" Value="20" />
        <asp:ListItem Text="50" Value="50" />
        <asp:ListItem Text="100" Value="100" />
        <asp:ListItem Text="250" Value="250" />
        <asp:ListItem Text="500" Value="500" />
        <asp:ListItem Text="750" Value="750" />
        <asp:ListItem Text="1,000" Value="1000" />
        <asp:ListItem Text="2,000" Value="2000" />
        <asp:ListItem Text="2,250" Value="2250" />
        <asp:ListItem Text="2,450" Value="2450" />
    </asp:DropDownList>

    <br /><br />
    <asp:Button ID="btCodeRun" runat="server" Text="Run Next Set" 
        onclick="btCodeRun_Click" />

    <br /><br />
    <asp:Literal ID="ltGoogle" runat="server" Text="Google 2,500 per day max --Example url: http://maps.googleapis.com/maps/api/directions/xml?origin=68130&destination=68132&region=us&units=imperial&sensor=false" Mode="Encode" />

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


