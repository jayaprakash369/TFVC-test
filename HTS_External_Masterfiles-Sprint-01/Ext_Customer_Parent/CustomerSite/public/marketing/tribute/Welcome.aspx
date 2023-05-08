<%@ Page Title="Welcome Tribute Users | Scantron Technology Solutions" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" 
    Inherits="public_marketing_tribute_Welcome" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Welcome Tribute Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe width="930px" height="2300px" frameborder="0" src="http://info.scantron.com/TributeLandingPage.html">
                    </iframe>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
