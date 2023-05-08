<%@ Page Title="Change Any Password" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ChangeAnyPassword.aspx.cs" 
    Inherits="private__editor_ChangeAnyPassword" %>
<%--   --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Change Any Password
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<div id="dvPasswordChange">

    <table style="width: 500px;" class="tableWithoutLines">
        <tr>
            <td align="right" style="width: 150px;">
                User ID
            </td>
            <td align="left" style="width: 340px;">
                <asp:TextBox ID="txUserID" runat="server" Width="100" MaxLength="15" />
                <asp:RequiredFieldValidator ID="vReqUserID" runat="server" 
                    ControlToValidate="txUserID" 
                    ErrorMessage="A user ID is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="PasswordChange" />
            </td>
        </tr>
        <tr>
            <td align="right">
                New Password
            </td>
            <td align="left">
                <asp:TextBox ID="txPassword" runat="server" TextMode="Password" Width="100" MaxLength="20" />
                <asp:RequiredFieldValidator ID="vReqPassword" runat="server" 
                    ControlToValidate="txPassword" 
                    ErrorMessage="A new password is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="PasswordChange" />
                <asp:CustomValidator id="vCusPassword" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="PasswordChange" />
            </td>
        </tr>
        <tr>
            <td align="right">
                Retype Password
            </td>
            <td align="left">
                <asp:TextBox ID="txPasswordMatch" runat="server" TextMode="Password" Width="100" MaxLength="20" />
                <asp:RequiredFieldValidator ID="vReqPasswordMatch" runat="server" 
                    ControlToValidate="txPasswordMatch" 
                    ErrorMessage="Password confirmation is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="PasswordChange" />
                 <asp:CompareValidator ID="vCom" runat="server" 
                    ControlToCompare="txPassword" 
                    ControlToValidate="txPasswordMatch" 
                    Display="Dynamic" 
                    ErrorMessage="The password and confirmation must match." 
                    Text="*"
                    ValidationGroup="PasswordChange">
                </asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="left">
                <asp:Button ID="btReset" runat="server" 
                    Text="Reset Password" 
                    onclick="btReset_Click" 
                    ValidationGroup="PasswordChange" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="left">
                <asp:Label ID="lbResult" runat="server" Text=""></asp:Label>
                <asp:ValidationSummary ID="vSumPasswordChange" runat="server" ValidationGroup="PasswordChange" />
            </td>
        </tr>
    </table>

    
    
</div>

</asp:Content>

