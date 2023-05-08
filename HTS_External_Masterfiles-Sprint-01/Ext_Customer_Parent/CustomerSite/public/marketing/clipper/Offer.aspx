<%@ Page Title="Welcome Clipper Customers | Scantron Technology Solutions" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Offer.aspx.cs" 
    Inherits="public_marketing_clipper_Offer" %>
    <%--
    <div style="height: 10px; clear: both;"></div> 
      --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Welcome Clipper Customers
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <asp:Panel ID="pnIFrame" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <iframe width="930px" height="1550px" frameborder="0" src="http://info.scantron.com/Clipper_Advertiser_Offer_LP.html" >
                    </iframe>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
