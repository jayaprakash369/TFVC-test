<%@ Page Title="Comments" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Comments.aspx.cs" 
    Inherits="public_sc_Comments" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Comments
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <asp:HiddenField ID="hfCtr" runat="server" />
    <asp:HiddenField ID="hfTck" runat="server" />

<%-- Panel where ajax executes --%>             
<%-- onselectedindexchanged="chBxCategories_SelectedIndexChanged"> --%>             

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vSummary_Comment" runat="server" ValidationGroup="Comment" />
            <table>
                <tr valign="top">
                    <td colspan="2">
                        <asp:Label ID="lbInstructions" runat="server" Font-Bold="true"
                            Text="If you would like a response to your comments, please provide an email address or phone number." />
                    </td>
                </tr>
                <tr valign="top">
                    <td align="left">
                        <asp:CheckBox ID="chBxDelivery" runat="server" 
                        Text="Service Delivery" 
                        AutoPostBack="true" 
                        oncheckedchanged="chBxGroup_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="chBxLogistics" runat="server" 
                        Text="Service Logistics" 
                        AutoPostBack="true" 
                        oncheckedchanged="chBxGroup_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="chBxUtilities" runat="server" 
                        Text="ServiceCOMMAND® Utilities" 
                        AutoPostBack="true" 
                        oncheckedchanged="chBxGroup_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="chBxAccounting" runat="server" 
                        Text="Accounting/Invoicing" 
                        AutoPostBack="true" 
                        oncheckedchanged="chBxGroup_CheckedChanged" />
                        <br />
                        <asp:CheckBox ID="chBxProducts" runat="server" 
                        Text="Service Offerings" 
                        AutoPostBack="true" 
                        oncheckedchanged="chBxGroup_CheckedChanged" />
                    </td>
                    <td align="left" style="padding-left: 10px;">
                        <table cellpadding="0" cellspacing="10">
                            <tr>
                                <td align="right">Contact Name</td>
                                <td><asp:TextBox ID="txContact" runat="server" Width="250" MaxLength="100" /></td>
                            </tr>
                            <tr>
                                <td align="right">Phone</td>
                                <td>
                                    (
                                    <asp:TextBox ID="txPhone1" runat="server" Width="30" MaxLength="3" />
                                        <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                            Operator="DataTypeCheck"
                                            Type="Integer"
                                            ControlToValidate="txPhone1"
                                            ErrorMessage="Area code must be a number" 
                                            Text="*"
                                            SetFocusOnError="true" 
                                            ValidationGroup="Comment">
                                        </asp:CompareValidator>
                                    )
                                    &nbsp;<asp:TextBox ID="txPhone2" runat="server" Width="30" MaxLength="3"  />
                                        <asp:CompareValidator id="vCompare_txPhone2" runat="server" 
                                            Operator="DataTypeCheck"
                                            Type="Integer"
                                            ControlToValidate="txPhone2"
                                            ErrorMessage="Phone prefix must be a number" 
                                            Text="*"
                                            SetFocusOnError="true" 
                                            ValidationGroup="Comment">
                                        </asp:CompareValidator>
                                    -&nbsp;<asp:TextBox ID="txPhone3" runat="server" Width="40" MaxLength="4"  />
                                        <asp:CompareValidator id="vCompare_txPhone3" runat="server" 
                                            Operator="DataTypeCheck"
                                            Type="Integer"
                                            ControlToValidate="txPhone3"
                                            ErrorMessage="Phone suffix must be a number" 
                                            Text="*"
                                            SetFocusOnError="true" 
                                            ValidationGroup="Comment">
                                        </asp:CompareValidator>
                                    &nbsp; Ext: <asp:TextBox ID="txExtension" runat="server" Width="65" MaxLength="8" />
                                        <asp:CompareValidator id="vCompare_Extension" runat="server" 
                                            Operator="DataTypeCheck"
                                            Type="Integer"
                                            ControlToValidate="txExtension"
                                            ErrorMessage="The extension must be a number" 
                                            Text="*"
                                            SetFocusOnError="true" 
                                            ValidationGroup="Comment">
                                        </asp:CompareValidator>

                                </td>
                            </tr>
                            <tr>
                                <td align="right">Email</td>
                                <td>
                                    <asp:TextBox ID="txEmail" runat="server" Width="250" MaxLength="50" />
                                        <asp:RegularExpressionValidator id="vRegular_Email" runat="server"
                                            ControlToValidate="txEmail"
                                            ValidationExpression="^\S+@\S+\.\S+$"
                                            ErrorMessage="The email address is not in a valid format" 
                                            Text="*"
                                            SetFocusOnError="true"
                                            Display="Dynamic"
                                            ValidationGroup="Comment">
                                        </asp:RegularExpressionValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btSubmit" runat="server" 
                                        Text="Submit Comments" 
                                        ValidationGroup="Comment"
                                        onclick="btSubmit_Click" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lbError" runat="server" Text="" SkinID="labelError" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnGeneral" runat="server">
                            <p><asp:Label ID="lbGeneral" runat="server" Text="General Comments" SkinID="labelTitleColor2_Medium" /></p>
                            <asp:TextBox ID="txGeneral" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" /> 
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnDelivery" runat="server" Visible="false">
                            <p><asp:Label ID="lbDelivery" runat="server" Text="Service Delivery" SkinID="labelTitleColor2_Medium" /></p>
                            <asp:TextBox ID="txDelivery" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" /> 
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnLogistics" runat="server" Visible="false">
                            <p><asp:Label ID="lbLogistics" runat="server" Text="Service Logistics" SkinID="labelTitleColor2_Medium"  /></p>
                            <asp:TextBox ID="txLogistics" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" />                            
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnUtilities" runat="server" Visible="false">
                            <p><asp:Label ID="lbUtilities" runat="server" Text="ServiceCOMMAND® Utilities" SkinID="labelTitleColor2_Medium" /></p>
                            <asp:TextBox ID="txUtilities" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" /> 
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnAccounting" runat="server" Visible="false">
                            <p><asp:Label ID="lbAccounting" runat="server" Text="Accounting / Invoicing" SkinID="labelTitleColor2_Medium" /></p>
                            <asp:TextBox ID="txAccounting" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" /> 
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnProducts" runat="server" Visible="false">
                            <p><asp:Label ID="lbProducts" runat="server" Text="Service Offerings" SkinID="labelTitleColor2_Medium" /></p>
                            <asp:TextBox ID="txProducts" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="1000" /> 
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

