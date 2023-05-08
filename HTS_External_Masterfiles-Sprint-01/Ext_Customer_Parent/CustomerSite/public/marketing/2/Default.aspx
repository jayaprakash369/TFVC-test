<%@ Page Title="Two" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="public_marketing_2_Default" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
 Two
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe frameborder="0" src="http://oma-dev-dmz/public/marketing/_src/Two.aspx" width="1000px" height="500px;">
                    </iframe>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
