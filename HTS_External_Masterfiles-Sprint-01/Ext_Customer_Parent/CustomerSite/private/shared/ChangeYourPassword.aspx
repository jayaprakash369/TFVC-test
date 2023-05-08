<%@ Page Title="Change Your Password" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ChangeYourPassword.aspx.cs" 
    Inherits="private_shared_ChangeYourPassword" %>
    <%--   --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Change Your Password
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

        <table class="tableWithoutLines">
            <tr>
                <td>
                    Account ID
                </td>
                <td>
                    <asp:Label ID="lbUserID" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td>
                    Current Password
                </td>
                <td>
                    <asp:TextBox ID="txPwdOld" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="vReqPwdOld" runat="server" 
                        ControlToValidate="txPwdOld" 
                        ErrorMessage="Your old password is required." 
                        Text="*"
                        Display="Dynamic"
                        ValidationGroup="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    New Password
                </td>
                <td>
                    <asp:TextBox ID="txPwdNew" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="vReqPwdNew" runat="server" 
                        ControlToValidate="txPwdNew" 
                        ErrorMessage="A new password is required." 
                        Text="*"
                        Display="Dynamic"
                        ValidationGroup="Password" />
                     <asp:CustomValidator id="vCusPassword" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    Confirm New Password
                </td>
                <td>
                    <asp:TextBox ID="txPwdNew2" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="vReqPwdNew2" runat="server" 
                        ControlToValidate="txPwdNew2" 
                        ErrorMessage="A password confirmation is required." 
                        Text="*"
                        Display="Dynamic"
                        ValidationGroup="Password" />
                    <asp:CompareValidator ID="vComPassword" runat="server" 
                        ControlToCompare="txPwdNew" 
                        ControlToValidate="txPwdNew2" 
                        Display="Dynamic" 
                        ErrorMessage="The new password and confirmation must match." 
                        Text="*"
                        ValidationGroup="Password" />
                </td>
            </tr>
            <tr style="padding-top: 20px;">
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btChangePassword" runat="server" 
                        Text="Change Password" 
                        onclick="btChangePassword_Click" 
                        ValidationGroup="Password" />
                </td>
            </tr>
            <tr style="padding-top: 5px;">
                <th>
                    &nbsp;
                </th>
                <td>
                <asp:Label ID="lbResult" runat="server" Text="" SkinID="labelError" />
                <asp:ValidationSummary ID="vSumPassword" runat="server" 
                    ValidationGroup="Password" />
                </td>
            </tr>
        </table>
</asp:Content>

