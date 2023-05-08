<%@ Page Title="STS: Scantron Technology Solutions" Language="C#" MasterPageFile="~/Scantron_Body_A_Nav.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--   --%>    

<asp:Content ID="Content1" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <%-- 
  <div style="width:960px; margin:0px auto; position:relative; left:-50px; top:-60px;" > 
        <asp:HyperLink ID="ImageButton1" runat="server" ImageUrl="~/media/scantron/images/banners/HTS-rename-STS-banner.jpg" NavigateUrl="~/media/scantron/pdf/StsRenameInfo.pdf" Target="Namechange" />
    </div>
        --%>    
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="For_Body_A" Runat="Server">
<!-- content begins -->     
 <%-- padding:  top right bottom left
     Starting Monday, December 3, you can find us and the ServiceCOMMAND login at scantron.com.  Starting December 3, you will be redirected automatically. You can check out our new site now <a href="https://www.scantron.com" target="scantron" >by clicking here...</a>  The ServiceCOMMAND login is available now under the Client Portals menu at the top-right on scantron.com." 
                 <td style="font-size: 20px; color: #AD0034; padding: 15px; line-height: 24px; width: 340px">
                The public side of our website is moving to Scantron.com 
                <span style="font-size: 13px; color: #777777; font-style: italic;"><br />(Our ServiceCOMMAND utilities will remain in place). </span>
            </td>

         <table class="tbBorder" style="background-color: #FFFFFF; border: 1px solid #ad0034; width: 96%; margin-bottom: 15px; margin-top: 10px">
        <tr>
            <td style="font-size: 15px; padding: 15px;">
                Starting Monday December 3rd you will be redirected automatically to our new site.  The link to the ServiceCOMMAND 
                login is available under the Client Portals menu at the top-right of the site.
                To check out our new site now, <a href="https://www.scantron.com" target="scantron" >click here.</a>
            </td>
        </tr>
    </table>

  --%>

    <table class="tableWithoutLines">
        <tr>
            <td><h4 style="padding-top: 0px;">Managed IT Services</h4></td>
            <td><h4 style="padding-top: 0px;">Managed Print Services</h4></td>
            <td><h4 style="padding-top: 0px;">Hardware Services</h4></td>
        </tr>
        <tr>
            <td><img src="/media/scantron/images/support/ServerRoom_288.jpg" class="image_support" alt="" /></td>
            <td><img src="/media/scantron/images/support/ColorToner_288.jpg" class="image_support" alt="" /></td>
            <td><img src="/media/scantron/images/support/BlueContact_288.png" class="image_support" alt="" /></td>
        </tr>
        <tr>
            <td style="padding-top: 5px;"><asp:LinkButton ID="LinkButton4" runat="server" SkinID="linkButtonHeader" PostBackUrl="~/public/ManagedServices.aspx">One Call for All IT Support Needs</asp:LinkButton></td>
            <td style="padding-top: 5px;"><asp:LinkButton ID="LinkButton5" runat="server" SkinID="linkButtonHeader" PostBackUrl="~/public/MpsBenefits.aspx">Reduce Print Costs, Increase Service Levels</asp:LinkButton></td>
            <td style="padding-top: 5px;"><asp:LinkButton ID="LinkButton6" runat="server" SkinID="linkButtonHeader" PostBackUrl="~/public/HardwareMaintenance.aspx">One Vendor to Repair or Replace All Hardware</asp:LinkButton></td>
        </tr>
        <tr>
            <td>Reduce your risk of business downtime. Take advantage of an affordable support provider who covers virtually everything with a fixed-cost subscription.</td>
            <td>Never let printer issues derail productivity again. Monitor and measure your printer fleet to streamline assets, reduce costs and proactively replace toner. </td>
            <td>Minimize downtime and control costs! Use a fixed-cost service to maintain servers, end point devices and more – remote and onsite. </td>
        </tr>
    </table>
<%--   
    <script type="text/javascript" language="javascript">
    var myUrl = document.location.href;
    if (document.location.href.substring(0, 17) != "http://localhost:") {
        fadeClock();
    }
</script>
--%>    

<!-- content ends Two --> 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
</asp:Content>



