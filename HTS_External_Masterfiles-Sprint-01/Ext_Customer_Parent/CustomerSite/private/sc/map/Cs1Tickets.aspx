<%@ Page Title="Map Ticket Locations" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Cs1Tickets.aspx.cs" 
    Inherits="private_sc_map_Cs1Tickets" %>
    <%@ PreviousPageType VirtualPath="~/private/sc/OpenTickets.aspx" %>
    <%-- 
    e5ecf9
    Map Ticket Locations

       <div style="padding-top:.25em; background-color: #ad0034; z-index: 5;">
       <b>STS Open Tickets for <asp:Label ID="lbCs1" runat="server" /></b>
   </div>

    --%>            
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
    <script type="text/javascript" src="http://maps.google.com/maps?file=api&v=2&sensor=false&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ"></script>
    <script type="text/javascript" src="http://www.scantronts.com/private/js/map/MapFunctions.js"></script>

   <script type="text/javascript">
       initialize();
       //document.getElementById("whiteTop").setAttribute("class", "");
       //document.getElementById("whiteMid").setAttribute("class", "");
       //document.getElementById("whiteEnd").setAttribute("class", "");
   </script>
</asp:Content>
 
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <asp:Label ID="lbCs1" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <div id="map" style="width: 930px; height: 40px" ></div>
    <asp:HiddenField ID="hfCs1" runat="server" />
    <asp:HiddenField ID="hfMod" runat="server" />
    <asp:HiddenField ID="hfKey" runat="server" />
</asp:Content>


