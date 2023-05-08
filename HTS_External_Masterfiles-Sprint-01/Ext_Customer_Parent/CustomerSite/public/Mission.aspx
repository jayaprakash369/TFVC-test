<%@ Page Title="STS: Mission" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" 
    CodeFile="Mission.aspx.cs" 
    Inherits="public_Mission" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    Mission
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Experience IT as It Should Be
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
<center>
    <table style="width: 100%">
        <tr style="vertical-align: top; text-align: left;">
            <td style="text-align:center; padding-top: 25px">
                <asp:Image ID="imLogo" runat="server" Width="500"
                    ImageUrl="~/media/scantron/images/logos/company/ScantronTechnologySolutions.jpg" />
            </td>
        </tr>
        <tr>  
            <td style="font-size: 22px; font-style: italic; line-height: 30px; padding-top: 25px; padding-bottom: 25px; padding-left: 15px; padding-right: 15px;">
                Our mission is to empower growth through intelligent, mission-critical assessment, technology, and data capture solutions for business, education, and certification clients worldwide.
            </td>
        </tr>
    </table>
</center>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>

