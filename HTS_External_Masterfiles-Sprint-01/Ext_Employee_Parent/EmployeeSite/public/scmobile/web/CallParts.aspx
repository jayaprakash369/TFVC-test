<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallParts.aspx.cs" 
Inherits="public_scmobile_web_CallParts" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
width: 480px;
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Parts Used/Shipped</title>
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
    <div style="clear: both; height:5px;"></div>

    <center><asp:Label ID="lbMsg" runat="server" SkinID="LabelError1" Visible="false" /></center>

    <asp:Panel ID="pnPartsUsed" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbPartsUsed" runat="server" Text="Parts Used" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpPartsUsed" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr>
                        <td>Description</td>
                        <td><%# Eval("Description") %></td>
                    </tr>
                    <tr>
                        <td>Qty</td>
                        <td><%# Eval("Qty") %></td>
                    </tr>
                    <tr>
                        <td>Serial</td>
                        <td><%# Eval("Serial") %></td>
                    </tr>
                    <tr>
                        <td>From Loc</td>
                        <td><%# Eval("Location") %></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Description</td>
                        <td><%# Eval("Description") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Qty</td>
                        <td><%# Eval("Qty") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Serial</td>
                        <td><%# Eval("Serial") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>From Loc</td>
                        <td><%# Eval("Location") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel ID="pnPartsShipped" runat="server" Visible="false" style="margin-bottom: 20px;">

        <center><asp:Label ID="lbPartsShipped" runat="server" Text="Parts Shipped" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpPartsShipped" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Seq</td>
                        <td><%# Eval("Seq") %></td>
                    </tr>
                    <tr>
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr>
                        <td>Ordered</td>
                        <td><%# Eval("QtyOrdered") %></td>
                    </tr>
                    <tr>
                        <td>Filled</td>
                        <td><%# Eval("QtyFilled") %></td>
                    </tr>
                    <tr>
                        <td>Backordered</td>
                        <td><%# Eval("QtyBackordered") %></td>
                    </tr>
                    <tr>
                        <td>Shipped</td>
                        <td><%# Eval("QtyShipped") %></td>
                    </tr>
                    <tr>
                        <td>Date Shipped</td>
                        <td><%# Eval("DisplayShipped") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Seq</td>
                        <td><%# Eval("Seq") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Ordered</td>
                        <td><%# Eval("QtyOrdered") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Filled</td>
                        <td><%# Eval("QtyFilled") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Backordered</td>
                        <td><%# Eval("QtyBackordered") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Shipped</td>
                        <td><%# Eval("QtyShipped") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date Shipped</td>
                        <td><%# Eval("DisplayShipped") %></td>
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
