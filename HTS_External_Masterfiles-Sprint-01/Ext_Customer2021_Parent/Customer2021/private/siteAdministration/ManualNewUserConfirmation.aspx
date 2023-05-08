<%@ Page Title="Manual New User Confirmation" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ManualNewUserConfirmation.aspx.cs" 
    Inherits="private_siteAdministration_ManualNewUserConfirmation" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
            <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>

    <script type="text/javascript">
    // =============================================================
    function clearSearchInput() {
        var doc = document.forms[0];
        doc.ctl00_BodyContent_txSearchEmail.value = "";
        doc.ctl00_BodyContent_txSearchCs1.value = "";
        doc.ctl00_BodyContent_txSearchFirstName.value = "";
        doc.ctl00_BodyContent_txSearchLastName.value = "";
        return true;
    }
    // =============================================================
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Manual New User Confirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
<div class="bodyPadding">

    <div class="w3-container w3-padding-32">

        <asp:Label ID="lbMessage" runat="server" SkinID="labelError" />
        <div class="spacer5"></div>

        <!-- START: SEARCH PANEL (USER)  ======================================================================================= -->
        <asp:Panel ID="pnSearchUser" runat="server" DefaultButton="btSearchUser">
        <div class="w3-row">

        <h5>
            <asp:Label ID="lbSearchInfo" runat="server" Text="Use partial or full entries to find account needing confirmation" Visible="true" />
        </h5>

            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                <div class="SearchPanelElements">
                    User Email<br />
                    <asp:TextBox ID="txSearchEmail" runat="server" MaxLength="70" Width="250" />
                </div>
                <div class="SearchPanelElements">
                    First Name<br />
                    <asp:TextBox ID="txSearchFirstName" runat="server" MaxLength="50" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    Last Name<br />
                    <asp:TextBox ID="txSearchLastName" runat="server" MaxLength="50" Width="120" />
                </div>
                <div class="SearchPanelElements">
                    Cust Num<br />
                    <asp:TextBox ID="txSearchCs1" runat="server" MaxLength="7" Width="80" />
                </div>
                <div class="SearchPanelElements"><br />
                    <asp:Button ID="btSearchUser" runat="server" Text="Search" onclick="btSearchUser_Click" SkinId="buttonPrimary" />
                </div>
                <div class="SearchPanelElements"><br />
                    <input id="btSearchClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearSearchInput();" />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
            </div>
        </asp:Panel>
        <!-- END: SEARCH PANEL (USER)  ======================================================================================= -->

        <asp:Panel ID="pnUsers" runat="server">        
        <h5>
            <asp:Label ID="lbClickToConfirm" runat="server" Visible="true" />
        </h5>
        <!-- START: SMALL AND LARGE PANELS (USER)  ======================================================================================= -->

        <%--  --%>
     <div class="w3-row">

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
                <asp:Repeater ID="rp_UserSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>User Id</td>
                        <td>
                            <asp:LinkButton ID="LkLoginEmail" runat="server" 
                            OnClick="lkLoginEmail_Click" 
                            CommandArgument='<%# Eval("UserName") %>'>
                            <%# Eval("UserName") %>
                        </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>User Name</td>
                        <td><asp:Label ID="lbUserName" runat="server" Text='<% #Eval("FirstName") + " " + Eval("LastName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust Num</td>
                        <td><asp:Label ID="lbCustNum" runat="server" Text='<% #Eval("PrimaryCs1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust Name</td>
                        <td><asp:Label ID="lbCustName" runat="server" Text='<% #Eval("Cs1Name") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Company Type</td>
                        <td><asp:Label ID="lbCompanyType" runat="server" Text='<% #Eval("CompanyType") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer Administrator?</td>
                        <td><asp:Label ID="lbCustomerAdministrator" runat="server" Text='<% #Eval("CustomerAdministrator") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Registered</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("Registered") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>User Id</td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" 
                            OnClick="lkLoginEmail_Click" 
                            CommandArgument='<%# Eval("UserName") %>'>
                            <%# Eval("UserName") %>
                        </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>User Name</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("FirstName") + " " + Eval("LastName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust Num</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("PrimaryCs1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust Name</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("Cs1Name") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Company Type</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("CompanyType") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer Administrator?</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<% #Eval("CustomerAdministrator") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Registered</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("Registered") %>' /></td>
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

        <asp:GridView ID="gv_UsrLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            AllowPaging="True"
            PageSize="100" 
            AllowSorting="True" 
            onpageindexchanging="gvPageIndexChanging_Usr" 
            onsorting="gvSorting_Usr">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="User Id" SortExpression="UserName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkLoginEmail" runat="server" 
                            OnClick="lkLoginEmail_Click" 
                            CommandArgument='<%# Eval("UserName") %>'>
                            <%# Eval("UserName") %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FirstName" HeaderText="First" SortExpression="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="Last" SortExpression="LastName" />
                <asp:BoundField DataField="PrimaryCs1" HeaderText="Cust Num" SortExpression="PrimaryCs1" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Cs1Name" HeaderText="Cust Name" SortExpression="Cs1Name" />
                <asp:BoundField DataField="CompanyType" HeaderText="Type" SortExpression="CompanyType" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CustomerAdministrator" HeaderText="Cust Admin" SortExpression="CustomerAdministrator" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Registered" HeaderText="Registered" SortExpression="RegistrationDate" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
    

            <!-- -->
         </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
    </div>
    </asp:Panel>

<div class="spacer30"></div>



        <%--  --%>

    <asp:HiddenField ID="hfUserEmailName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfCurrentCs1" runat="server" /> <%-- Always use "CurrentCs1" for everybody, if admin has switched user, it will be loaded in current  --%>
</div>
</div>
</asp:Content>
