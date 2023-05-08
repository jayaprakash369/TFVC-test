<%@ Page Title="Welcome Wrk-Systems Customers | Scantron Technology Solutions" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" 
    Inherits="public_marketing_wrkSystems_Welcome" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe width="930px" height="1200px" frameborder="0" src="http://info.scantron.com/HTS-WRK-Systems.html">
                    </iframe>
                    </td>
                </tr>
            </table>
        </asp:Panel>

</asp:Content>
