<%@ Page Title="Sample Query" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="SampleQuery.aspx.cs" 
    Inherits="private_siteAdministration_samples_SampleQuery" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Sample Query
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

        <div class="w3-row w3-padding-32">
        <div class=" w3-container">

    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Panel ID="pnSearch" runat="server">
        <table>
            <tr style="vertical-align: bottom;">
                <td>Employee Name (like)<br />
                    <asp:TextBox ID="txSearchName" runat="server" Width="150" /></td>
                <td style="padding-left: 30px;">
                    <asp:Button ID="btSearchSubmit" runat="server" Text="Search" OnClick="btSearchSubmit_Click" /></td>
            </tr>
        </table>
            <div class="spacer10"></div>
        </asp:Panel>

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_Small" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Name</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("EMPNAM") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Number</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("EMPNUM") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td><td><asp:Label ID="Label3" runat="server" Text='<% #Eval("ECITY") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td><td><asp:Label ID="Label4" runat="server" Text='<% #Eval("ESTATE") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("EMPNAM") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Number</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("EMPNUM") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td><td><asp:Label ID="Label3" runat="server" Text='<% #Eval("ECITY") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td><td><asp:Label ID="Label5" runat="server" Text='<% #Eval("ESTATE") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
        <asp:Repeater ID="rp_Large" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="width:100%;">
                <tr>
                    <th>Name</th>
                    <th>Number</th>
                    <th>City</th>
                    <th>State</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr  class="trColorReg" style="vertical-align: top;">
                <td><asp:Label ID="lbName" runat="server" Text='<% #Eval("EMPNAM") %>' /></td>
                <td><asp:Label ID="lbNumber" runat="server" Text='<% #Eval("EMPNUM") %>' /></td>
                <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("ECITY") %>' /></td>
                <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("ESTATE") %>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt" style="vertical-align: top;">
                <td><asp:Label ID="lbName" runat="server" Text='<% #Eval("EMPNAM") %>' /></td>
                <td><asp:Label ID="lbNumber" runat="server" Text='<% #Eval("EMPNUM") %>' /></td>
                <td><asp:Label ID="lbCity" runat="server" Text='<% #Eval("ECITY") %>' /></td>
                <td><asp:Label ID="lbState" runat="server" Text='<% #Eval("ESTATE") %>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->

        <%--  --%>
    </div>
    </div>

</div>
</asp:Content>
