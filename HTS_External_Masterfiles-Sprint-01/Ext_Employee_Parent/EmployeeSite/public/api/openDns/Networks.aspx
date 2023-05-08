<%@ Page Title="OpenDns: Networks" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Networks.aspx.cs" 
    Inherits="public_api_openDns_Networks" %>
<%--
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
OpenDns: Networks
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <div style="clear: both; height: 10px;"></div>
    <asp:DropDownList ID="ddDnsCustomers" runat="server" /> 
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btRun" runat="server" Text="Get Customer Networks" 
        onclick="btRun_Click" />
    
    <div style="clear: both; height: 20px;" ></div>
    <asp:Label ID="lbMsg" runat="server" />

</asp:Content>


