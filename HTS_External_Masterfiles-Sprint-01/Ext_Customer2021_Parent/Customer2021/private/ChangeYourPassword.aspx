<%@ Page Title="Change Your Password" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ChangeYourPassword.aspx.cs" 
    Inherits="private_ChangeYourPassword" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Change Your Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32">
    <div class="w3-container">

        <table class="tableWithoutLines">
            <tr style="vertical-align: top;">
                <td>
                    Account ID
                </td>
                <td>
                    <asp:Label ID="lbUserIdEmail" runat="server" Text="" />
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td>
                    Current Password
                </td>
                <td>
                    <asp:TextBox ID="txPasswordCurrent" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td style="padding-top: 25px;">
                    New Password
                </td>
                <td style="padding-top: 25px;">
                    <asp:TextBox ID="txPasswordNew" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td style="padding-right: 10px;">
                    <i>Confirm New Password</i>
                </td>
                <td>
                    <asp:TextBox ID="txPasswordNew2" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="padding-top: 20px;">
                    <asp:Button ID="btChangePassword" runat="server" 
                        Text="Change Password" 
                        SkinID="buttonPrimary" 
                        onclick="btChangePassword_Click" 
                        />
                </td>
            </tr>
            <tr>
                <th>
                    &nbsp;
                </th>
                <td style="padding-top: 10px;">
                    <asp:Label ID="lbResult" runat="server" Text="" SkinID="labelError" />
                </td>
            </tr>
        </table>

        <%--  --%>
    </div>
</div>

</div>
</asp:Content>
