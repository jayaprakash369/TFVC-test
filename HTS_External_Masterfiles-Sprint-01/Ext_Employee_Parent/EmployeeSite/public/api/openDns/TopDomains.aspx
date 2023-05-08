<%@ Page Title="OpenDns: Top Domains" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="TopDomains.aspx.cs" 
    Inherits="public_api_openDns_TopDomains" %>
<%--
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
OpenDns: Top Domains
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    
    <div style="clear: both; height: 10px;" ></div>
    <asp:DropDownList ID="ddDnsCustomers" runat="server"
        onselectedindexchanged="ddDnsCustomers_SelectedIndexChanged" 
        AutoPostBack="true" /> 
    &nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="ddDnsCustLocs" runat="server" 
         /> 
    &nbsp;&nbsp;&nbsp; Days: 
    <asp:DropDownList ID="ddDays" runat="server" >
        <asp:ListItem Text="1" Value="1" />
        <asp:ListItem Text="7" Value="7" />
        <asp:ListItem Text="14" Value="14" Selected="True" />
        <asp:ListItem Text="30" Value="30" />
        <asp:ListItem Text="60" Value="60" />
        <asp:ListItem Text="90" Value="90" />
        <asp:ListItem Text="120" Value="120" />
        <asp:ListItem Text="240" Value="240" />
        <asp:ListItem Text="365" Value="365" />
    </asp:DropDownList>

    &nbsp;&nbsp;&nbsp; Records: 
    <asp:DropDownList ID="ddRecs" runat="server" >
        <asp:ListItem Text="10" Value="10" />
        <asp:ListItem Text="25" Value="25" />
        <asp:ListItem Text="50" Value="50" Selected="True" />
        <asp:ListItem Text="100" Value="100" />
        <asp:ListItem Text="200" Value="200" />
        <asp:ListItem Text="300" Value="300" />
        <asp:ListItem Text="400" Value="400" />
        <asp:ListItem Text="500" Value="500" />
    </asp:DropDownList>

<%--
        onselectedindexchanged="ddFilter_SelectedIndexChanged"
--%>             

    &nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="ddFilter" runat="server" 
        AutoPostBack="true" >
        <asp:ListItem Text="Everything Allowed and Blocked" Value="-1" />
        <asp:ListItem Text="Allowed, for any reason" Value="2049" />
        <asp:ListItem Text="Blocked, by Domain List" Value="0" />
        <asp:ListItem Text="Blocked, for any reason" Value="2048" />
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btRun" runat="server" Text="Submit" 
        onclick="btRun_Click" />
    
    <div style="clear: both; height: 20px;" ></div>
    <table>
        <tr style="vertical-align:top">
            <td>
                <asp:Panel ID="pnCategories" runat="server" Visible="true">
                <asp:Label ID="lbChbxlHelp" runat="server" Text="Limit Results To..." SkinID="LabelError1" />
                <div style="clear: both; height: 5px;"></div>
                <asp:CheckBoxList ID="cbxlCategories" runat="server" style="min-width: 200px; background-color:#eeeeee;" 
                    RepeatColumns="1" SkinID="checkBoxList1" 
                    RepeatDirection="Vertical" />
                </asp:Panel>
            </td>
            <td style="padding-left: 10px;"><asp:Label ID="lbMsg" runat="server" /></td>
        </tr>
    </table>

    <div style="clear: both; height: 1px;" ></div>

    <asp:Panel ID="pnCategoryDescriptions" runat="server" Visible="true">
    <asp:GridView ID="gvCategories" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        style="margin-top: 20px;"
        EmptyDataText="No categories were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
        <asp:BoundField HeaderText="Category" DataField="ocTitle" ItemStyle-Width="200" />
        <asp:BoundField HeaderText="Description" DataField="ocDesc" />
    </Columns>
    </asp:GridView>

    </asp:Panel>
<%--
--%>             

<%--

--%>             

</asp:Content>


