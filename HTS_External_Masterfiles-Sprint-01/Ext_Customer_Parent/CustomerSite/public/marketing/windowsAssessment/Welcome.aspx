<%@ Page Title="Windows Assessment Webinar | Harland Technology Services" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" 
    Inherits="public_marketing_windowsAssessment_Welcome" %>
    <%-- 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe frameborder="0" width="930px" height="1180px" src="http://info.scantron.com/Windows-Upgrade-Assessment-Webinar.html">
                    </iframe>

                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
