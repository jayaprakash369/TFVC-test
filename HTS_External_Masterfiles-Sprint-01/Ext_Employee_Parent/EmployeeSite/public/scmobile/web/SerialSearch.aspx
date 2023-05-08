<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SerialSearch.aspx.cs" 
Inherits="public_scmobile_web_SerialSearch" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Serial Number Search</title>
    <style type="text/css">
        body { margin: 2px; } 
        .tbSearch { margin-bottom: 15px;  }
        .tbSearch td { padding-bottom: 10px;}
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:18px;">
    <div style="clear: both; height: 3px;"></div>
    <div style="clear: both; height: 5px;"></div>
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>

    <div style="height: 10px; clear: both;"></div>

    <center><asp:Label ID="lbMsg" runat="server" ForeColor="Crimson" /></center>

<%--  --%>

    <asp:Panel ID="pnSearch" runat="server" DefaultButton="Button1" style="margin-bottom: 20px;">

       <center><table class="tbSearch">
            <tr>
                <td>Serial #</td>
                <td><asp:Button ID="btClear" runat="server" Text="Clear" OnClick="btClear_Click" Font-Size="13" Style="padding: 5px" /></td>
                <td><asp:Button ID="btSearch" runat="server" Text="Search" OnClick="btSearch_Click" Font-Size="13" Style="padding: 5px" /></td>
            </tr>
            <tr>
                <td colspan="3"><asp:TextBox ID="txSearchSerial" runat="server" Width="175" Font-Size="16" /></td><td></td>
            </tr>
<%-- 
 --%>            
        </table></center>

    <asp:Repeater ID="rpMach" runat="server">
        <HeaderTemplate>
        <table class="tableWithLines" style="width:100%">
            </HeaderTemplate>
            <ItemTemplate>
              
            <tr>
                <td>MOD</td>
                <td style="text-align: right;"><asp:Label ID="lbPart" runat="server" Text='<%# Eval("CEPRT#") %>' /></td>
            </tr>
            <tr>
                <td>SER</td>
                <td>
                    <asp:LinkButton ID="lkSerial" runat="server" 
                        OnClick="lkSerial_Click" 
                        CommandArgument='<%# Eval("CEPRT#") + "|" + Eval("CESYS#") + "|" + Eval("CERNR") + "|" +  Eval("CERCD") %>' 
                        Text='<%# Eval("CESER#")  %>' />
                </td>
            </tr>
            <tr>
                <td>PRT</td>
                <td>
                    <asp:Label ID="lbModel" runat="server" Text='<%# Eval("CEMOD#") %>' />
                </td>
            </tr>
            <tr>
                <td>CST</td>
                <td>
                    <asp:Label ID="lbCust" runat="server" Text='<%# Eval("CERNR") + " " + Eval("CERCD") %>' />
                </td>
            </tr>
            <tr>
                <td>NAM</td>
                <td>
                    <asp:Label ID="lbCstName" runat="server" Text='<%# Eval("custnm") %>' />
                </td>
            </tr>
            <tr>
                <td>CSZ</td>
                <td>
                    <asp:Label ID="CitySt" runat="server" Text='<%# Eval("City") + ", " + Eval("State") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color:#EEEEEE;">
                 <td>MOD</td>
                 <td style="text-align: right;"><asp:Label ID="lbPart" runat="server" Text='<%# Eval("CEPRT#") %>' /></td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td>SER</td>
                <td>
                    <asp:LinkButton ID="lkSerial" runat="server" 
                        OnClick="lkSerial_Click" 
                        CommandArgument='<%# Eval("CEPRT#") + "|" + Eval("CESYS#") + "|" + Eval("CERNR") + "|" +  Eval("CERCD") %>' 
                        Text='<%# Eval("CESER#")  %>' />
                </td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td>PRT</td>
                <td>
                    <asp:Label ID="lbModel" runat="server" Text='<%# Eval("CEMOD#") %>' />
                </td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td>CST</td>
                <td>
                     <asp:Label ID="lbCust" runat="server" Text='<%# Eval("CERNR") + " " + Eval("CERCD") %>' />
                </td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td>NAM</td>
                <td>
                     <asp:Label ID="lbCstName" runat="server" Text='<%# Eval("custnm") %>' /> 
                </td>
            </tr>
            <tr style="background-color:#EEEEEE;">
             <td>CSZ</td>
             <td>
                <asp:Label ID="CitySt" runat="server" Text='<%# Eval("City") + ", " + Eval("State") %>' />
             </td>
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
    
    </div>
    <asp:HiddenField ID="hfUsr" runat="server" />
    </form>
</body>
</html>
