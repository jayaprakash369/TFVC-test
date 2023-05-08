<%@ Page Title="STS: Technology Partnerships" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="TechnologyPartnerships.aspx.cs" 
    Inherits="public_TechnologyPartnerships" %>
<%--  --%>
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
<style type="text/css">
.logoTable
{
    text-align: center;
}
.logoTable tr
{
}

.logoTable td
{
    min-width: 250px;
    vertical-align: middle;
    padding-top: 10px;
    padding-bottom: 25px;
}
</style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    Technology Partnerships
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Strategic Alliances
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top; border-right: 1px solid #CCCCCC;">
                <table class="logoTable">
                    <tr><td style="padding-top: 30px; width: 300px;"><asp:HyperLink ID="hlAxient" runat="server" Target="partners" NavigateUrl="http://www.axcient.com/" ImageUrl="~/media/scantron/images/logos/partners/axcient.png" /></td></tr>
                    <tr><td style="padding: 30px;">                       
                        <asp:HyperLink ID="hlSilverSky" runat="server" Target="partners" NavigateUrl="https://www.baesystems.com/en/home" ImageUrl="~/media/scantron/images/logos/partners/baesystems.png" />
                     </td></tr>                   
                    <tr><td><asp:HyperLink ID="hlFirstNational" runat="server" Target="partners" NavigateUrl="https://www.fnts.com/" ImageUrl="~/media/scantron/images/logos/partners/FirstNationalTechnologySolutions.png" /></td></tr>
                    <tr><td style="padding-top: 30px;"><asp:HyperLink ID="hlIBM" runat="server" Target="partners" NavigateUrl="http://www.ibm.com/us/en/" ImageUrl="~/media/scantron/images/logos/partners/IBM.png" /></td></tr>
                    <tr><td style="padding-top: 40px;""><asp:HyperLink ID="hlKaseya" runat="server" Target="partners" NavigateUrl="http://www.kaseya.com/" ImageUrl="~/media/scantron/images/logos/partners/Kaseya.png" /></td></tr>
                    <tr><td style="padding-top: 40px;"><asp:HyperLink ID="hlLexmark" runat="server" Target="partners" NavigateUrl="http://www.lexmark.com/" ImageUrl="~/media/scantron/images/logos/partners/Lexmark.png" /></td></tr>
                    <tr><td style="padding-top: 40px;"><asp:HyperLink ID="hlMicrosoft" runat="server" Target="partners" NavigateUrl="~/public/MicrosoftOffice365.aspx" ImageUrl="~/media/scantron/images/logos/partners/Microsoft.png" /></td></tr>                 
                    <tr><td style="padding-top: 40px; padding-bottom: 30px;"><asp:HyperLink ID="hlOpenDNS" runat="server" Target="partners" NavigateUrl="http://www.opendns.com/" ImageUrl="~/media/scantron/images/logos/partners/OpenDns.png" /></td></tr>     
               </table>
            </td>
            <td style="vertical-align: top;">
                <table class="logoTable">
                    
                    <tr><td style="padding-top: 30px;"><asp:HyperLink ID="hlPrintFleet" runat="server" Target="partners" NavigateUrl="http://www.printfleet.com" ImageUrl="~/media/scantron/images/logos/partners/PrintFleet.png" /></td></tr>               
                    <tr><td style="padding-top: 40px;"><asp:HyperLink ID="hlSymantec" runat="server" Target="partners" NavigateUrl="http://www.symantec.com/index.jsp" ImageUrl="~/media/scantron/images/logos/partners/Symantec.png" /></td></tr>
                    <tr><td style="padding-top: 40px;"><asp:HyperLink ID="hlVmWare" runat="server" Target="partners" NavigateUrl="http://www.vmware.com/" ImageUrl="~/media/scantron/images/logos/partners/VmWare.png" /></td></tr>
                    <tr><td style="padding-top: 25px;"><asp:HyperLink ID="hlSolarWinds" runat="server" Target="partners" NavigateUrl="http://www.solarwinds.com/" ImageUrl="~/media/scantron/images/logos/partners/SolarWinds.gif" /></td></tr>                
                    <tr><td style="padding-top: 25px;"><asp:HyperLink ID="hlXerox" runat="server" Target="partners" NavigateUrl="http://www.xerox.com/" ImageUrl="~/media/scantron/images/logos/partners/Xerox.png" /></td></tr>   
                    <tr><td style="padding-top: 40px;"><asp:HyperLink ID="hZebra" runat="server" Target="partners" NavigateUrl="http://www.zebra.com/" ImageUrl="~/media/scantron/images/logos/partners/Zebra.png" /></td></tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>

