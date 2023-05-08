<%@ Page Title="Customer Preference Editor" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Preferences.aspx.cs" 
    Inherits="private_admCust_Preferences" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Customer Preference Editor</td>
            <td style="display: block; float: right;">
                <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                    <asp:TableRow VerticalAlign="Bottom">
                        <asp:TableCell>
                            <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
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
    <table class="tableWithoutLines">
        <tr>
            <td style="width: 60%; text-align: left;">
                <p><asp:Label ID="Label1" runat="server" Text="Open or Close New Account Registration" SkinID="labelTitleColor2_Medium" /></p>
                <p>All customers begin with registration CLOSED to more than one "basic" company account.  
                <br />if you would like to allow multiple basic accounts, you will need to OPEN registration.  
                <br />(Administrators may create accounts at any time.)</p>
            </td>
            <td style="width: 40%;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right; font-weight: bold; padding-right: 20px;">
                Set Registration Status
            </td>
            <td>
                <asp:RadioButtonList ID="rblRegOpenOrClosed" runat="server" 
                    RepeatDirection="Horizontal" >
                    <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Closed" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btUpdate" runat="server" Text="Update Preferences" onclick="btUpdate_Click" 
                    ValidationGroup="Preferences" />
            </td>
        </tr>
    </table>
</asp:Content>


