<%@ Page Title="ServiceCOMMAND Demo" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Demo.aspx.cs" 
    Inherits="public_Demo" %>
    <%--   --%>    

<asp:Content ID="Content1" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_A" Runat="Server">
    
    <%--   
        --%>    
</asp:Content>
    <%-- 
                <asp:HyperLink ID="hlDemo" runat="server" Text="ServiceCOMMAND Demonstration Video" Font-Size="X-Large" NavigateUrl="https://myscantron.sharepoint.com/:v:/r/sites/STSApplicationDevelopmentTeamSite/Shared%20Documents/General/ServiceCommandOverview20201012.mp4?csf=1&web=1&e=RZsJzN" />
          --%>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <asp:Label ID="lbMenuTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <asp:Panel ID="pnDemo" runat="server" DefaultButton="btDemo">
          <asp:Button ID="btDemo" runat="server" Visible="false" />

        <div style="padding: 30px;">
        <asp:HyperLink ID="hlDemo" runat="server" Text="ServiceCOMMAND Demonstration Video" Font-Size="X-Large" NavigateUrl="~/media/video/ServiceCommandOverview_20201015.mp4" />
            </div>
        <div style="clear: both; height: 200px;"></div>
    </asp:Panel>

</asp:Content>



