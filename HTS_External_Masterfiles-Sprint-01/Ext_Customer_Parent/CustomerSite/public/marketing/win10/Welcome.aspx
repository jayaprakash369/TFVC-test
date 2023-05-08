<%@ Page Title="Welcome Valassis Customers | Harland Technology Services" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" 
    Inherits="public_marketing_valassis_Welcome" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe frameborder="0" width="930px" height="1500px" src="http://info.scantron.com/ValassisEmailLandingPage.html">
                    </iframe>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
