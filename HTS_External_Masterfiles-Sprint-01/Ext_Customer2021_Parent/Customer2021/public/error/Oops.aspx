<%@ Page Title="Timeout" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="Oops.aspx.cs" 
    Inherits="public_error_oops" %>

<asp:Content ID="Content3" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" runat="server">
    Something went wrong...
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">
        
  <div class="w3-row w3-padding-32">
    <div class="w3-twothird w3-container">
        <h2 class="w3-text-teal">Oops...</h2>

        <table>
            <tr>
                <td style="padding-right: 40px;"><asp:Image ID="imDragon" runat="server" ImageUrl="~/media/images/oops_dragon290.png" style="width: 200px;" /></td>
                <td style="width: 400px; font-size: 1.2em;"><p>Trying to reach ServiceCOMMAND.com? 
                    <div class="spacer10"></div>
                    Please click <asp:HyperLink ID="htHome" runat="server" Font-Size="18"  NavigateUrl="~/private/menu.aspx">here</asp:HyperLink> . . .<!-- Somehow you ended up here (but we're not sure why). --></p></td>
            </tr>
        </table>

        <div class="spacer100"></div>
        <p style="font-style: italic; font-size: 14px;">If this continues to happen, please let us know at <a style='color: blue; font-size: 13px' href="mailto:servicecommandsupport@scantron.com">ServiceCOMMAND Support</a>.</p>
    </div>
    <div class="w3-third w3-container">
    </div>
   </div>
</div>
</asp:Content>

