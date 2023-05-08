<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallList.aspx.cs" 
Inherits="public_scmobile_web_CallList" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tech Call List</title>
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
    <center><asp:Label ID="lbTitle" runat="server" Text="Tech Call List" style="font-style: italic; color: #777777; font-size: 1.2em;" /></center>
    <div style="clear: both; height: 5px;"></div>
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>

    <div style="height: 10px; clear: both;"></div>

    <center><asp:Label ID="lbMsg" runat="server" ForeColor="Crimson" /></center>

<%--  --%>

    <asp:Panel ID="pnSearch" runat="server" DefaultButton="Button1" style="margin-bottom: 20px;">

       <center><table class="tbSearch">
            <tr>
                <td>Tech</td>
                <td>Ctr</td>
                <td><asp:Button ID="btClear" runat="server" Text="Clear" OnClick="btClear_Click" Font-Size="13" Style="padding: 5px" /></td>
                <%--  
                <td>Tck</td>
                <td>Cust</td>
                <td></td>
                --%>

                <td></td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txSearchEmp" runat="server" Width="60" Font-Size="16" /></td>
                <td><asp:TextBox ID="txSearchCtr" runat="server" Width="50" Font-Size="16" /></td>
                <td><asp:Button ID="btSearch" runat="server" Text="SUBMIT" OnClick="btSearch_Click" Font-Size="13" Style="padding: 5px" /></td>
                <%--  
                 
                <td><asp:TextBox ID="txSearchTck" runat="server" Width="70" Font-Size="16" /></td>
                <td colspan="3"><asp:TextBox ID="txSearchNam" runat="server" Width="120" Font-Size="16" /></td><td></td>
                    --%>
            </tr>
<%-- 
 --%>            
        </table></center>

    <asp:Repeater ID="rpCalls" runat="server">
        <HeaderTemplate>
        <table class="tableWithLines" style="width:100%">
            </HeaderTemplate>
            <ItemTemplate>

            <tr>
                <td>
                    <asp:LinkButton ID="lkCs1Name" runat="server" 
                        OnClick="lkName_Click" 
                        CommandArgument='<%# Eval("TCCENT") + "|" + Eval("TICKNR") + "|" %>' 
                        Text='<%# Eval("SDCSTN")  %>' />
                </td>
                <td style="text-align: right;"><asp:Label ID="lbCtrTck" runat="server" Text='<%# Eval("TCCENT") + "-" + Eval("TICKNR") %>' /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbCs1Cs2" runat="server" Text='<%# Eval("STCUS1") + "-" + Eval("STCUS2") %>' />
                    &nbsp;&nbsp; <asp:Label ID="lbCity" runat="server" Text='<%# Eval("SDCITY") %>' />,  <asp:Label ID="lbState" runat="server" Text='<%# Eval("SDSTAT") %>' />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbComment" runat="server" Text='<%# Eval("TCOMM1") + " " + Eval("TCOMM2") %>' />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbRemark" runat="server" Text='<%# Eval("REMARK") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color:#EEEEEE;">
                <td>
                    <asp:LinkButton ID="lkCs1Name" runat="server" 
                        OnClick="lkName_Click" 
                        CommandArgument='<%# Eval("TCCENT") + "|" + Eval("TICKNR") + "|" %>' 
                        Text='<%# Eval("SDCSTN")  %>' />
                </td>
                <td style="text-align: right;"><asp:Label ID="lbCtrTck" runat="server" Text='<%# Eval("TCCENT") + "-" + Eval("TICKNR") %>' /></td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td colspan="2">
                <asp:Label ID="lbCs1Cs2" runat="server" Text='<%# Eval("STCUS1") + "-" + Eval("STCUS2") %>' />
                &nbsp;&nbsp;
                <asp:Label ID="lbCity" runat="server" Text='<%# Eval("SDCITY") %>' />,  <asp:Label ID="lbState" runat="server" Text='<%# Eval("SDSTAT") %>' /></td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td colspan="2">
                    <asp:Label ID="lbComment" runat="server" Text='<%# Eval("TCOMM1") + " " + Eval("TCOMM2") %>' />
                </td>
            </tr>
            <tr style="background-color:#EEEEEE;">
                <td colspan="2">
                    <asp:Label ID="lbRemark" runat="server" Text='<%# Eval("REMARK") %>' />
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
