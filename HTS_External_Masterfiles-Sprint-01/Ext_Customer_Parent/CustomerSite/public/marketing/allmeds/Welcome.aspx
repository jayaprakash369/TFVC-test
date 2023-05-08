<%@ Page Title="Welcome AllMeds Users | Scantron Technology Solutions" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" 
    Inherits="public_marketing_allmeds_Welcome" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Welcome AllMeds Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe width="930px" height="2100px" frameborder="0" src="http://info.scantron.com/AllMedsLandingPage.html">
                    </iframe>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
