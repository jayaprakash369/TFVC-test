<%@ Page Title="User Account Maintenance" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="UserMaintenance.aspx.cs" 
    Inherits="private_admCust_UserMaintenance" %>
<%-- --%>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>User Account Maintenance</td>
            <td style="display: block; float: right;">
                <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                    <asp:TableRow VerticalAlign="Bottom">
                        <asp:TableCell>
                            <asp:CheckBox ID="chBxAll" runat="server" Text="  Show All Accounts" Font-Size="11" ForeColor="#3A7728" OnCheckedChanged="chBxAllChange_Click" AutoPostBack="true" />
                        </asp:TableCell>
                        <asp:TableCell Width="25">
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbCs1Change" runat="server" Text="Primary customer..." Font-Size="11" />
                            &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" 
                                ValidationGroup="Cs1Change" />
                                <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                    Operator="DataTypeCheck"
                                    Type="Integer"
                                    ControlToValidate="txCs1Change"
                                    ErrorMessage="Customer entry must be a number" 
                                    Text="*"
                                    SetFocusOnError="true" 
                                    ValidationGroup="Cs1Change">
                                </asp:CompareValidator>
                                &nbsp;
                                <asp:Button ID="btCs1Change" runat="server" 
                                    Text="Change Customer" 
                                    onclick="btCs1Change_Click"
                                    ValidationGroup="Cs1Change" />
                                <asp:ValidationSummary ID="vSumCs1Change" runat="server" ValidationGroup="Cs1Change" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
        
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
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
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchUser.value = "";
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchEmail.value = "";
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchCs1.value = "";
        return true;
    }
    // =============================================================

</script>
<center>
    <asp:Panel ID="pnUpdateUser" runat="server" 
        Visible="false">
        <table class="tableWithoutLines" style="width: 100%">
            <tr>
                <td colspan="5" style="text-align: left;"><b>User ID</b>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbUserID" runat="server" SkinID="labelInstructions" />
                </td>
            </tr>
            <tr>
                <th style="text-align: left;">
                    <asp:Button ID="btUpdateContact" runat="server" 
                        Text="Update Contact" 
                        onclick="btUpdateContact_Click" 
                        ValidationGroup="Contact" />
                </th>
                <th style="text-align: left;">
                    <asp:Button ID="btUpdatePhone" runat="server" 
                        Text="Update Phone" 
                        onclick="btUpdatePhone_Click" 
                        ValidationGroup="Phone" />
                </th>
                <th style="text-align: left;">
                    <asp:Button ID="btUpdateEmail" runat="server" 
                        Text="Update Email" 
                        onclick="btUpdateEmail_Click" 
                        ValidationGroup="Email" />
                </th>
                <th style="text-align: left;">
                    <asp:Button ID="btUpdatePassword" runat="server" 
                        Text="Update Password" 
                        onclick="btUpdatePassword_Click" 
                        ValidationGroup="Password" />
                </th>
                <th rowspan="2" style="text-align: left;" >
                     <asp:Button ID="btToggleLock" runat="server" 
                        onclick="btToggleLock_Click" />
                        <div style="height: 5px;"></div>
                     <asp:Button ID="btToggleAdmin" runat="server" 
                        onclick="btToggleAdmin_Click" />
                        <div style="height: 5px;"></div>
                     <asp:Button ID="btDelete" runat="server" 
                        Text="Del User"
                        OnClientClick="return confirmDelete();"
                        onclick="btDelete_Click" />
                </th>
            </tr>
            <tr style="vertical-align: top;">
                <td>
                    <asp:TextBox ID="txContact" runat="server"
                        Width="120"
                        MaxLength="30"
                        ValidationGroup="Update" />
                    <asp:RequiredFieldValidator ID="vReqContact" runat="server" 
                        ControlToValidate="txContact" 
                        ErrorMessage="A contact name is required." 
                        Text="*"
                        Display="Dynamic" 
                        SetFocusOnError="true"
                        ValidationGroup="Contact" />
                    <asp:ValidationSummary ID="vSumContact" runat="server" 
                        ValidationGroup="Contact" />
                </td>
                <td>
                (<asp:TextBox ID="txPhone1" runat="server" Width="30" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vReq_Phone1" runat="server" 
                        ControlToValidate="txPhone1" 
                        ErrorMessage="Please include an area code." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Phone" />
                    <asp:CompareValidator id="CompareValidator1" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone1"
                        ErrorMessage="Area code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Phone">
                    </asp:CompareValidator>)&nbsp;<asp:TextBox ID="txPhone2" runat="server" Width="30" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vReq_Phone2" runat="server" 
                        ControlToValidate="txPhone2" 
                        ErrorMessage="Please include a phone prefix." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Phone" />
                    <asp:CompareValidator id="vCompare_txPhone2" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone2"
                        ErrorMessage="Phone prefix must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Phone">
                    </asp:CompareValidator>
                -&nbsp;<asp:TextBox ID="txPhone3" runat="server" Width="35" MaxLength="4" />
                    <asp:RequiredFieldValidator ID="vReq_Phone3" runat="server" 
                        ControlToValidate="txPhone3" 
                        ErrorMessage="Please include a phone suffix." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Phone" />
                    <asp:CompareValidator id="vCompare_txPhone3" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone3"
                        ErrorMessage="Phone suffix must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Phone">
                    </asp:CompareValidator>
                &nbsp;Ex:<asp:TextBox ID="txExtension" runat="server" Width="35" MaxLength="8" />
                    <asp:CompareValidator id="vCompare_Extension" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txExtension"
                        ErrorMessage="The extension must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Phone">
                    </asp:CompareValidator>
                    <asp:ValidationSummary ID="vSumPhone" runat="server" 
                        ValidationGroup="Phone" />
                </td>
                <td>
                    <asp:TextBox ID="txEmail" runat="server" 
                        Width="230"
                        MaxLength="50"
                        ValidationGroup="Update" />
                    <asp:RequiredFieldValidator ID="vReqEmail" runat="server" 
                        ControlToValidate="txEmail" 
                        ErrorMessage="An email address is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Email" />
                    <asp:RegularExpressionValidator id="vRegEmail" runat="server"
                        ControlToValidate="txEmail"
                        ValidationExpression="^\S+@\S+\.\S+$"
                        ErrorMessage="The email address is not in a valid format" 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        AutoCompleteType="Disabled" 
                        ValidationGroup="Email" />
                    <br />
                    <asp:TextBox ID="txEmail2" runat="server" 
                        Width="230"
                        ValidationGroup="Update" />
                    <asp:RequiredFieldValidator ID="vReqEmail2" runat="server" 
                        ControlToValidate="txEmail2" 
                        ErrorMessage="Email confirmation is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        AutoCompleteType="Disabled" 
                        ValidationGroup="Email" />
                    <asp:CompareValidator ID="vComEmail" runat="server" 
                        ControlToCompare="txEmail" 
                        ControlToValidate="txEmail2" 
                        ErrorMessage="The email and confirmation do not match." 
                        Text="*"
                        Display="Dynamic" 
                        SetFocusOnError="true"
                        ValidationGroup="Email" />
                    <asp:ValidationSummary ID="vSumEmail" runat="server" 
                        ValidationGroup="Email" />
                </td>
                <td>
                    <asp:TextBox ID="txPassword" runat="server" 
                        TextMode="Password" 
                        Width="100"
                        MaxLength="15"
                        AutoCompleteType="Disabled" 
                        ValidationGroup="Password" />
                    <asp:RequiredFieldValidator ID="vReqPassword" runat="server" 
                        ControlToValidate="txPassword" 
                        ErrorMessage="A password is required." 
                        Text="*"
                        Display="Dynamic" 
                        SetFocusOnError="true"
                        ValidationGroup="Password" />
                     <asp:CustomValidator id="vCusPassword" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="Password" />
                    <br />
                    <asp:TextBox ID="txPassword2" runat="server" 
                        TextMode="Password" 
                        Width="100"
                        AutoCompleteType="Disabled" 
                        ValidationGroup="Password" />
                    <asp:RequiredFieldValidator ID="vReqPassword2" runat="server" 
                        ControlToValidate="txPassword2" 
                        ErrorMessage="Password confirmation is required." 
                        Text="*"
                        Display="Dynamic" 
                        SetFocusOnError="true"
                        ValidationGroup="Password" />
                    <asp:CompareValidator ID="vComPassword" runat="server" 
                        ControlToCompare="txPassword" 
                        ControlToValidate="txPassword2" 
                        ErrorMessage="The password and confirmation do not match." 
                        Text="*"
                        Display="Dynamic" 
                        SetFocusOnError="true"
                        ValidationGroup="Password" />
                    <asp:ValidationSummary ID="vSumPassword" runat="server" 
                        ValidationGroup="Password" />
                </td>
            </tr>
        </table>

    </asp:Panel>
    <asp:Label ID="lbMessage" runat="server" SkinID="labelError" />

<asp:Panel ID="pnSearch" runat="server" Visible="true" DefaultButton="btSearch">
<table style="width: 100%; border: 1px solid #BBBBBB; background-color:#EEEEEE;">
    <tr>
        <td>
            User Id &nbsp;
            <asp:TextBox ID="txSearchUser" runat="server" MaxLength="20" Width="100" />
        </td>
        <td>
            Cust &nbsp;
            <asp:TextBox ID="txSearchCs1" runat="server" MaxLength="7" Width="40" />
        </td>
        <td>
            Email
             &nbsp;
            <asp:TextBox ID="txSearchEmail" runat="server" MaxLength="50" Width="150" />
        </td>
        <td>
            <asp:Button ID="btSearch" runat="server" Text="Search" onclick="btSearch_Click" />
            &nbsp;&nbsp;
            <input id="btSearchClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearSearchInput();" />
        </td>

    </tr>
</table>
<div style="height: 10px; clear: both;"></div>
</asp:Panel>
    <asp:Panel ID="pnUsers" runat="server">
        <asp:GridView ID="gvUsr" runat="server" style="width: 100%"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            AllowPaging="True"
            PageSize="500" 
            AllowSorting="True" 
            onpageindexchanging="gvPageIndexChanging" 
            onsorting="gvSorting">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="User ID" SortExpression="UserName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkUserID" runat="server" 
                            OnClick="userPick_Click" 
                            CommandArgument='<%# Eval("UserName") %>'>
                            <%# Eval("UserName") %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PrimaryCs1" HeaderText="Cust" SortExpression="PrimaryCs1" />
                <asp:BoundField DataField="Cs1Name" HeaderText="Name" SortExpression="Cs1Name" />
                <asp:BoundField DataField="Cs1Type" HeaderText="Type" SortExpression="Cs1Type" />
                <asp:BoundField DataField="ContactName" HeaderText="Contact" SortExpression="ContactName" />
                <asp:BoundField DataField="ContactPhone" HeaderText="Phone" SortExpression="ContactPhone" />
                <asp:BoundField DataField="PhoneExtension" HeaderText="Ext" SortExpression="PhoneExtension" />
                <asp:BoundField DataField="ContactEmail" HeaderText="Email" SortExpression="ContactEmail" />
                <asp:BoundField DataField="LoginCount" HeaderText="Logins" SortExpression="LoginCount" />
                <asp:BoundField DataField="LastLogin" HeaderText="Last Login" SortExpression="LastLoginNum" />
                <asp:BoundField DataField="CustomerAdmin" HeaderText="Admin" SortExpression="CustomerAdmin" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</center>
</asp:Content>


