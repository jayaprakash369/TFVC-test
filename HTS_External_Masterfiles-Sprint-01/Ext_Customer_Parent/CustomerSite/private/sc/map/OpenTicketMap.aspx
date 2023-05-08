<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OpenTicketMap.aspx.cs" 
    Inherits="private_sc_map_OpenTicketMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Tickets Locations</title>  
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <style>
      html, body, #map-canvas {
        height: 100%;
        margin: 0px;
        padding: 0px
      }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="map-canvas">
    </div>
    </form>
</body>
</html>

