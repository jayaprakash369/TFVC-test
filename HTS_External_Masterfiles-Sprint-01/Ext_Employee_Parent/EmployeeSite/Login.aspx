<%@ Page Title="HTS Employee Login" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" 
    Inherits="Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
Login
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">

<script type="text/javascript">
    // --------------------------------------------
    function checkCapsLock(ev) {
        if (isCapslock(ev)) {
            alert("YOUR CAPS LOCK IS ON.\r\n\r\nThis will prevent you from successfully logging in.");
        }
    }
    // --------------------------------------------
    function isCapslock(e) {

        e = (e) ? e : window.event;

        var charCode = false;
        if (e.which) {
            charCode = e.which;
        } else if (e.keyCode) {
            charCode = e.keyCode;
        }

        var shifton = false;
        if (e.shiftKey) {
            shifton = e.shiftKey;
        } else if (e.modifiers) {
            shifton = !!(e.modifiers & 4);
        }

        if (charCode >= 97 && charCode <= 122 && shifton) {
            return true;
        }

        if (charCode >= 65 && charCode <= 90 && !shifton) {
            return true;
        }

        return false;

    }
    // --------------------------------------------
    function clearInput() {
        var doc = document.forms[0];
        if (typeof doc.ctl00_BodyContent_txUserID != "undefined") {
            alert("clearing user id");
            doc.ctl00_BodyContent_txUserID.value = "";
        }
        if (typeof doc.ctl00_BodyContent_txPassword != "undefined") {
            alert("clearing password");
            doc.ctl00_BodyContent_txPassword.value = "";
        }
    }
    // --------------------------------------------
</script>
    <asp:Panel ID="pnLogin" runat="server" 
        DefaultButton="btLogin">

        <table>
            <tr>
                <td style="width: 80px; vertical-align: top;">
                    User ID
                </td>
                <td style="padding-bottom: 10px;">
                    <asp:TextBox ID="txUserID" runat="server" 
                        AutoCompleteType="Disabled" />
                    <asp:RequiredFieldValidator ID="vReqUserID" runat="server" 
                        ControlToValidate="txUserID" 
                        ErrorMessage="User ID is required." 
                        MaxLength="20"
                        Text="*"
                        ValidationGroup="Login" />
                </td>
            </tr>
            <tr>
                <td style="width: 80px; vertical-align: top;">
                    Password
                </td>
                <td style="padding-bottom: 10px;">
                    <asp:TextBox ID="txPassword" runat="server"
                        TextMode="Password"
                        MaxLength="15"
                        AutoCompleteType="Disabled" />
                    <asp:RequiredFieldValidator ID="vReqPassword" runat="server" 
                        ControlToValidate="txPassword" 
                        ErrorMessage="Password is required." 
                        Text="*"
                        ValidationGroup="Login" />
                </td>
            </tr>
            <tr>
                <td style="width: 80px; vertical-align: top;">
                    &nbsp;
                </td>
                <td style="padding-bottom: 10px;">
                    <asp:Button ID="btLogin" runat="server" 
                        Text="Continue" 
                        ValidationGroup="Login" 
                        onclick="btLogin_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:CustomValidator id="vCusLogin" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="Login" />
                    <asp:Label ID="lbMessage" runat="server" Text="" SkinID="labelMessage" />
                    <div style="height: 10px; clear: both;"></div>
                    <asp:ValidationSummary ID="vSumLogin" runat="server" ValidationGroup="Login" />
                </td>
            </tr>
        </table>

    </asp:Panel>

</asp:Content>

