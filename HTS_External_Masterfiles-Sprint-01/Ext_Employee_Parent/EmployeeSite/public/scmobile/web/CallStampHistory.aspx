﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallStampHistory.aspx.cs" 
Inherits="public_scmobile_web_CallStampHistory" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
width: 480px;
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Timestamp History</title>
    <style type="text/css">
        body { } 
        .tbDetail { margin-top: 5px; margin-bottom: 10px;  }
        .tbDetail tr { vertical-align: top; }
        .tbDetail th { padding-top: 2px; padding-bottom: 4px; padding-right: 15px;  text-align: left; width: 100px; }
        .tbDetail td { padding-top: 2px; padding-bottom: 4px; padding-right: 10px; }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:20px; margin: 10px;">
    
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>

    <asp:Panel ID="pnTimestamps" runat="server" Visible="false" style="margin-bottom: 20px;">
        <asp:Repeater ID="rpTimestamps" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Stamp</td>
                        <td><%# Eval("DisplayStamp") + " (" + Eval("TIMTCH") + ")"%></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") + " " + Eval("DisplayTime") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Stamp</td>
                        <td><%# Eval("DisplayStamp") + " (" + Eval("TIMTCH") + ")"%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") + " " + Eval("DisplayTime") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

<%-- 

 --%>
    
    </div>
    <asp:HiddenField ID="hfCtr" runat="server" />
    <asp:HiddenField ID="hfTck" runat="server" />

    </form>
</body>
</html>
