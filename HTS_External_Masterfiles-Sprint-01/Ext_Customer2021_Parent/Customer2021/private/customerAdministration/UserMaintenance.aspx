<%@ Page Title="User Maintenance" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="UserMaintenance.aspx.cs" 
    Inherits="private_customerAdministration_UserMaintenance" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
            <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .SearchPanelElementLeft {
            display: inline-block;
            float: left;
            padding: 10px;
            border: 1px solid #cccccc;
            height: 150px;
        }
        .SearchPanelElementBodyReg {
            display: inline-block;
            float: left;
            padding: 10px;
            border: 1px solid #cccccc;
            height: 150px;
        }
        .SearchPanelElementBodyShaded {
            display: inline-block;
            float: left;
            padding: 10px;
            background-color: #f1f1f1;
            border: 1px solid #cccccc;
            height: 150px;
        }
    </style>

    <script type="text/javascript">
    // =============================================================
    function confirmDelete() {
        var jDelete = window.confirm("Are you certain you want to delete this user account?");
        if (jDelete != true)
            return false;
    }
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
    User Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">

        <asp:Label ID="lbMessage" runat="server" SkinID="labelError" />
        <div class="spacer5"></div>

        <!-- START: UPDATE PANEL (USER)  ======================================================================================= -->

    <asp:Panel ID="pnUpdateUser" runat="server" Visible="false">

    <div class="w3-row">
        	<!--  START: UPDATE CONTENTS  ======================================================================================= -->

                  <b>Updating:</b>
                        &nbsp;&nbsp;<asp:Label ID="lbLoginEmail" runat="server" SkinID="labelMessage" />
                        &nbsp;&nbsp;<asp:Button ID="btCloseUpdateUserPanel" runat="server" Text="Close Panel?" onclick="btCloseUpdateUserPanel_Click" Font-Italic="true" Font-Size="9" />
                        <div class="spacer15"></div>

                        <div class="SearchPanelElementLeft">
                            Administer
                            <div class="spacer5"></div>
                             <asp:Button ID="btToggleLock" runat="server" onclick="btToggleLock_Click" />
                                <div class="spacer5"></div>
                             <asp:Button ID="btToggleAdmin" runat="server" onclick="btToggleAdmin_Click" />
                                <div class="spacer5"></div>
                             <asp:Button ID="btDelete" runat="server" Text="Delete User" OnClientClick="return confirmDelete();" onclick="btDelete_Click" />
                        </div>

                        <div class="SearchPanelElementBodyShaded">
                            New login email (and confirmation)
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdateEmail" runat="server" Width="230" MaxLength="50" />
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdateEmail2" runat="server" Width="230" MaxLength="50" />
                            <div class="spacer5"></div>
                            <asp:Button ID="btUpdateEmail" runat="server" Text="Update Email" onclick="btUpdateEmail_Click" />
                        </div>

                        <div class="SearchPanelElementBodyReg">
                            New password (and confirmation)
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdatePassword" runat="server" TextMode="Password" Width="180" MaxLength="50" AutoCompleteType="Disabled" />
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdatePassword2" runat="server" TextMode="Password" Width="180" MaxLength="50" AutoCompleteType="Disabled" />
                            <div class="spacer5"></div>
                            <asp:Button ID="btUpdatePassword" runat="server" Text="Update Password" onclick="btUpdatePassword_Click" />
                        </div>

                        <div class="SearchPanelElementBodyShaded" style="padding-top:32px;">
                            New firstname
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdateFirstName" runat="server" Width="100" MaxLength="50" />
                            <div class="spacer5"></div>
                            <asp:Button ID="btUpdateFirstName" runat="server" Text="Update First" onclick="btUpdateFirstName_Click" />
                        </div>

                        <div class="SearchPanelElementBodyReg" style="padding-top:32px;">
                            New lastname
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdateLastName" runat="server" Width="100" MaxLength="50" />
                            <div class="spacer5"></div>
                            <asp:Button ID="btUpdateLastName" runat="server" Text="Update Last" onclick="btUpdateLastName_Click" />
                        </div>

                        <div class="SearchPanelElementBodyShaded" style="padding-top:32px;">
                            New customer id
                            <div class="spacer5"></div>
                            <asp:TextBox ID="txUpdateCustNum" runat="server" Width="100" MaxLength="50" />
                            <div class="spacer5"></div>
                            <asp:Button ID="btUpdateCustNum" runat="server" Text="Update Cust" onclick="btUpdateCustNum_Click" />
                        </div>
                        <!-- END: UPDATE CONTENTS  ======================================================================================= -->
    </div>
        <div class="spacer20"></div>
    </asp:Panel>
        <!-- END: UPDATE PANEL (USER)  ======================================================================================= -->




        <!-- START: SEARCH PANEL (USER)  ======================================================================================= -->
        <asp:Panel ID="pnSearchUser" runat="server" DefaultButton="btSearchUser">
        <div class="w3-row">

            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                <div class="SearchPanelElements">
                    User Email<br />
                    <asp:TextBox ID="txSearchEmail" runat="server" MaxLength="50" Width="250" />
                </div>
                <div class="SearchPanelElements">
                    First Name<br />
                    <asp:TextBox ID="txSearchFirstName" runat="server" MaxLength="50" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Last Name<br />
                    <asp:TextBox ID="txSearchLastName" runat="server" MaxLength="50" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Cust Num<br />
                    <asp:TextBox ID="txSearchCs1" runat="server" MaxLength="10" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chBxAll" runat="server" Text="Show All Accounts" OnCheckedChanged="chBxAllChange_Click" AutoPostBack="true" SkinID="checkBox1" />
                                <div class="spacer5"></div>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Button ID="btSearchUser" runat="server" Text="Search" onclick="btSearchUser_Click"  SkinID="buttonPrimary" /></td>
                            <td><input id="btSearchClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearSearchInput();" /></td>
                        </tr>
                    </table>
                </div>
                        <!-- 

                <div class="SearchPanelElements">
                </div>
                            -->
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
            </div>
        </asp:Panel>
        <!-- END: SEARCH PANEL (USER)  ======================================================================================= -->


    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

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
                        <td>User Id (An Email)</td>
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
                        <td>Account ID</td>
                        <td><asp:Label ID="lbCustNum" runat="server" Text='<% #Eval("AccountId") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Account Name</td>
                        <td><asp:Label ID="lbCustName" runat="server" Text='<% #Eval("Cs1Name") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Company Type</td>
                        <td><asp:Label ID="lbCompanyType" runat="server" Text='<% #Eval("CompanyType") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Login Count</td>
                        <td><asp:Label ID="lbLoginCount" runat="server" Text='<% #Eval("LoginCount") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Last Login</td>
                        <td><asp:Label ID="lbLastLogin" runat="server" Text='<% #Eval("LastLogin") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer Administrator?</td>
                        <td><asp:Label ID="lbCustomerAdministrator" runat="server" Text='<% #Eval("CustomerAdministrator") %>' /></td>
                    </tr>

                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>User Id (An Email)</td>
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
                        <td>AccountID</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("AccountId") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Account Name</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("Cs1Name") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Company Type</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("CompanyType") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Login Count</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("LoginCount") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Last Login</td>
                        <td><asp:Label ID="Label6" runat="server" Text='<% #Eval("LastLogin") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer Administrator?</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<% #Eval("CustomerAdministrator") %>' /></td>
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
    <asp:Panel ID="pnUsers" runat="server">
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
                <asp:TemplateField HeaderText="User Id (An Email)" SortExpression="UserName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkLoginEmail" runat="server" 
                            OnClick="lkLoginEmail_Click" 
                            CommandArgument='<%# Eval("UserName") %>'>
                            <%# Eval("UserName") %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="First" DataField="FirstName" SortExpression="FirstName" />
                <asp:BoundField HeaderText="Last" DataField="LastName" SortExpression="LastName" />
                <asp:BoundField HeaderText="Account ID" DataField="AccountId" SortExpression="AccountId" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Account Name" DataField="Cs1Name" SortExpression="Cs1Name" />
                <asp:BoundField HeaderText="Type" DataField="CompanyType" SortExpression="CompanyType" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Logins" DataField="LoginCount" SortExpression="LoginCount" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Last Login" DataField="LastLogin" SortExpression="LastLoginSort" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Cust Admin" DataField="CustomerAdministrator" SortExpression="CustomerAdministrator" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Registered: But Unconfirmed?" DataField="RegistrationNotConfirmed" SortExpression="RegistrationNotConfirmed" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
    </asp:Panel>

            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
            </div>

<div class="spacer30"></div>



        <%--  
    
    <asp:HiddenField ID="hfCurrentCs1" runat="server" /> 
            --%>
    </div>

    <%-- Always use "CurrentCs1" for everybody, if admin has switched user, it will be loaded in current  --%>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfUserEmailName" runat="server" />

</div>
</asp:Content>
