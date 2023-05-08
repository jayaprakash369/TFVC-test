<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PartsInRegion.aspx.cs" 
Inherits="public_scmobile_web_PartsInRegion" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
width: 480px;
        .tbDetail { margin-top: 5px; margin-bottom: 10px;  }
        .tbDetail tr { vertical-align: top; }
        .tbDetail th { font-size: 18px; text-align: left; }
        .tbDetail td { font-size: 18px; padding: 3px;  }
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Parts In Your Region</title>
    <style type="text/css">
        body { } 
        .tbSearch { margin-bottom: 15px;  }
        .tbSearch td { padding-bottom: 10px;}
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:18px; margin: 10px;">
        
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>

    <div style="height: 10px; clear: both;"></div>

    <center><asp:Label ID="lbMsg" runat="server" ForeColor="Crimson" /></center>

<%--  --%>

    <asp:Panel ID="pnSearch" runat="server" DefaultButton="Button1" style="margin-bottom: 20px;">

       <center><table class="tbSearch">
            <tr>
                <td>Part</td>
                <td><asp:TextBox ID="txSearchPart" runat="server" Width="100" Font-Size="16" /></td>
                <td style="text-align: center;"><asp:Button ID="btClear" runat="server" Text="Clear" OnClick="btClear_Click" Font-Size="13" Style="padding: 5px" /></td>
                <td style="text-align: right;"><asp:Button ID="btSearch" runat="server" Text="Search" OnClick="btSearch_Click" Font-Size="13" Style="padding: 5px" /></td>
            </tr>
            <tr>
                <td style="padding-right: 15px;">Desc</td>
                <td colspan="3"><asp:TextBox ID="txSearchDesc" runat="server" Width="250" Font-Size="16" /></td>
            </tr>
<%-- 
            <tr>
                <td style="padding-right: 15px;">Subt</td>
                <td><asp:TextBox ID="txSearchSubt" runat="server" Width="100" Font-Size="16" /></td>
            </tr>
 --%>            
            <tr>
                <td><asp:CheckBox ID="chbxQty" runat="server"  /></td>
                <td colspan="3" style="font-style:italic;">Display only where stock is found</td>
                
            </tr>
        </table></center>

    <asp:Repeater ID="rpPart" runat="server">
        <HeaderTemplate>
        <table class="tableWithLines" style="width:100%">
            <tr>
                <th>Part</th>
                <th>Subt</th>
                <th>Qty</th>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td>
                    <asp:HiddenField ID="hfQty" runat="server" Value='<%# Eval("QTY") %>'  />
                    <asp:Label ID="lbPart" runat="server" Text='<%# Eval("PARTNR") %>' />
                    <asp:LinkButton ID="lkPart" runat="server" 
                        OnClick="lkPart_Click" 
                        CommandArgument='<%# Eval("PARTNR") + "|" + Eval("IMFDSC") %>' 
                        Text='<%# Eval("PARTNR")%>' />
                </td>
                <td><asp:Label ID="lbSubt" runat="server" Text='<%# Eval("PESUBT") %>' /></td>
                <td style="text-align:center;"><asp:Label ID="lbQty" runat="server" Text='<%# Eval("QTY") %>' /></td>
            </tr>
            <tr>
                <td colspan="3" style="font-style:italic;"><asp:Label ID="lbDesc" runat="server" Text='<%# Eval("IMFDSC") %>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color:#EEEEEE;">
                <td>
                    <asp:HiddenField ID="hfQty" runat="server" Value='<%# Eval("QTY") %>'  />
                    <asp:Label ID="lbPart" runat="server" Text='<%# Eval("PARTNR") %>' />
                    <asp:LinkButton ID="lkPart" runat="server" 
                        OnClick="lkPart_Click" 
                        CommandArgument='<%# Eval("PARTNR") + "|" + Eval("IMFDSC") %>' 
                        Text='<%# Eval("PARTNR")%>' />
                </td>
                <td><asp:Label ID="lbSubt" runat="server" Text='<%# Eval("PESUBT") %>' /></td>
                <td style="text-align:center;"><asp:Label ID="lbQty" runat="server" Text='<%# Eval("QTY") %>' /></td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td colspan="3" style="font-style:italic;"><asp:Label ID="lbDesc" runat="server" Text='<%# Eval("IMFDSC") %>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <br />
    <asp:Button ID="Button1" runat="server" Text="Button" Visible="false" />

    </asp:Panel>

<%--  --%>

        <asp:Panel ID="pnTechs" runat="server" style="margin-top: 10px; margin-bottom: 20px;" Visible="false">

        <asp:Label ID="lbSelectedPart" runat="server" />

        <asp:GridView ID="gvTechs" runat="server" style="width:100%"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            EmptyDataText="No matching records were found">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
            <asp:TemplateField HeaderText="Tech">
                <ItemTemplate>
                    <asp:LinkButton ID="lkTech" runat="server" 
                        OnClick="lkTech_Click" 
                        CommandArgument='<%# Eval("EMPNUM") + "|" + Eval("EMPNAM") %>' 
                        Text='<%# Eval("EMPNAM")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Num" DataField="EMPNUM" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Qty" DataField="LOCQOH" ItemStyle-HorizontalAlign="Center" />
        </Columns>
        </asp:GridView>
    </asp:Panel>

<%--  --%>
<asp:Panel ID="pnEmail" runat="server" style="margin-bottom: 10px;" Visible="false">
<table style="width: 100%" class="tbSearch">
    <tr>
        <td>Tech</td>
        <td><asp:Label ID="lbEmailTech" runat="server" /></td>
    </tr>
    <tr>
        <td>Part</td>
        <td><asp:Label ID="lbEmailPart" runat="server" /></td>
    </tr>
    <tr>
        <td>Desc</td>
        <td><asp:Label ID="lbEmailDesc" runat="server" /></td>
    </tr>
    <tr>
        <td style="padding-right: 15px;">Phone</td>
        <td><asp:Label ID="lbEmailPhone" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:center;"><asp:TextBox ID="txEmailMsg" runat="server" TextMode="MultiLine" Height="75" Width="320" Font-Size="14" /></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:center;"><asp:Button ID="btEmailSubmit" runat="server" Text="Send Email" OnClick="btEmail_Click" Font-Size="13"  Style="padding: 5px" /></td>
    </tr>
</table>
</asp:Panel>
<%-- 

 --%>
    
    </div>
    <asp:HiddenField ID="hfUsr" runat="server" />
    <asp:HiddenField ID="hfRgn" runat="server" />

    <asp:HiddenField ID="hfPrt" runat="server" />
    <asp:HiddenField ID="hfDsc" runat="server" />
    
    <asp:HiddenField ID="hfNum" runat="server" />
    <asp:HiddenField ID="hfNam" runat="server" />

    </form>
</body>
</html>
