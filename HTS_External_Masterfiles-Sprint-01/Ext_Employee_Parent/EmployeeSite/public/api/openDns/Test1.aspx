<%@ Page Title="OpenDns Test1" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Test1.aspx.cs" 
    Inherits="public_api_openDns_Test1" %>
<%--
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
OpenDns Test 1
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">


    <asp:Literal ID="ltLink" runat="server" Text="https://www.openDns.com" Mode="Encode" />
    
    <div style="clear: both; height: 10px;" ></div>
    <asp:Button ID="btRun" runat="server" Text="Run Api" 
        onclick="btRun_Click" />

    <div style="clear: both; height: 10px;" ></div>
    <asp:Literal ID="ltMsg" runat="server" Mode="Encode" />
    
    <div style="clear: both; height: 10px;" ></div>
    <asp:Label ID="lbMsg" runat="server" />

</asp:Content>


