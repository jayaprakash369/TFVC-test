<%@ Page Title="Service Request: Contact Information" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" 
    Inherits="private_sc_req_Contact" %>
<%@ PreviousPageType VirtualPath="~/private/sc/req/Location.aspx" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Service Request
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<%-- Hidden Fields --%>             
<asp:HiddenField ID="hfPri" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />

<%-- Standard Customer Header Info (Changes if user changes contact info)  --%> 
<asp:Panel ID="pnCs1Header" runat="server" />

<%-- Contact Information  --%> 
<asp:Panel ID="pnContact" runat="server" DefaultButton="btContact">

    <table class="tableWithoutLines">
        <tr>
            <td align="right" class="auto-style2"><asp:Label ID="lbStep2" runat="server" Text="Step 2" SkinID="labelSteps" /></td>

            <td class="auto-style4"><asp:Label ID="lbContactEntry" runat="server" /></td>
            <td>
                <asp:TextBox ID="txContact" runat="server" Width="300" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="vRequired_Contact" runat="server" 
                        ControlToValidate="txContact" 
                        ErrorMessage="A contact name is required." 
                        Text="*"
                        ValidationGroup="Contact" />
            </td>
        </tr>
        <tr>

            <td class="auto-style2"><asp:Label ID="Label1" runat="server" Text="" SkinID="labelSteps" /></td>
            <td class="auto-style4"><asp:Label ID="lbPhoneEntry" runat="server" /></td>
            <td>
                ( <asp:TextBox ID="txPhone1" runat="server" Width="40" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vRequired_Phone1" runat="server" 
                        ControlToValidate="txPhone1" 
                        ErrorMessage="The area code is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Contact">
                    </asp:RequiredFieldValidator><asp:CompareValidator id="vCompare_Phone1" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone1"
                        ErrorMessage="The area code must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact">
                    </asp:CompareValidator>) 
                &nbsp; <asp:TextBox ID="txPhone2" runat="server" Width="40" MaxLength="3" /> 
                    <asp:RequiredFieldValidator ID="vRequired_Phone2" runat="server" 
                        ControlToValidate="txPhone2" 
                        ErrorMessage="The phone prefix is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Contact">
                    </asp:RequiredFieldValidator><asp:CompareValidator id="vCompare_Phone2" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone2"
                        ErrorMessage="The phone prefix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact">
                    </asp:CompareValidator>- <asp:TextBox ID="txPhone3" runat="server" Width="50" MaxLength="4" /> 
                    <asp:RequiredFieldValidator ID="vRequired_Phone3" runat="server" 
                        ControlToValidate="txPhone3" 
                        ErrorMessage="The phone suffix is required" 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Contact">
                    </asp:RequiredFieldValidator><asp:CompareValidator id="vCompare_Phone3" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone3"
                        ErrorMessage="The phone suffix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact">
                    </asp:CompareValidator><asp:RangeValidator ID="vRange_Phone3" runat="server" 
                        ControlToValidate="txPhone3"
                        ErrorMessage="The phone suffix must be 9999 or less"
                        Text="*" 
                        MinimumValue="0" 
                        MaximumValue="9999"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact"></asp:RangeValidator>&nbsp; 
                Ext: <asp:TextBox ID="txExtension" runat="server" Width="65" MaxLength="8" />
                    <asp:CompareValidator id="vCompare_Extension" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txExtension"
                        ErrorMessage="The phone extension must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact" />
            </td>
        </tr>
        <tr><!--
            aria-multiline="True"
            Enter preferred method for our technician to contact you, along with phone#, text# or email address
            -->
            <td align="right" class="auto-style2"><asp:Label ID="lbStep4" runat="server" Text="Step 3" SkinID="labelSteps" /></td>
            <td class="auto-style4">How would you like our technician to contact you? </td><td>
              <asp:DropDownList runat="server" ID="ddMethod" CssClass="dropDownList1" />
                <div style="clear: both; height: 5px;"></div>
                <asp:TextBox ID="txMethodInfo" runat="server" Width="300" MaxLength="50" /> <!-- change size from 300 to 230 &nbsp; Ext? &nbsp; -->
                <asp:TextBox ID="txMethodPhoneExt" runat="server" Width="35" MaxLength="6" Visible="false" />
            </td>
        </tr>
        <tr valign="top">
            <td align="right" class="auto-style2"><asp:Label ID="lbStep5" runat="server" Text="Step 4" SkinID="labelSteps" /></td>
            <td class="auto-style4">Optional: Enter name of person requesting ticket</td><td>
                <asp:TextBox ID="txCreator" runat="server" Width="300" MaxLength="30" />
            </td>
        </tr>
        <tr valign="top">
            <td align="right" class="auto-style2"><asp:Label ID="lbStep6" runat="server" Text="Step 5" SkinID="labelSteps" /></td>
            <td class="auto-style4">Optional: For automated email acknowledgement, enter one address</td><td>
                <asp:TextBox ID="txEmail" runat="server" Width="300" MaxLength="50" />
                    <asp:RegularExpressionValidator id="vRegular_Email" runat="server"
                        ControlToValidate="txEmail"
                        ValidationExpression="^\S+@\S+\.\S+$"
                        ErrorMessage="The email address is not in a valid format" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Contact" />
            </td>
        </tr>
<%--
--%>             
        <tr valign="top">
            <td align="right" class="auto-style2"><asp:Label ID="lbStep7" runat="server" Text="Step 6" SkinID="labelSteps" /></td>
            <td class="auto-style4">
                <asp:Label ID="lbListTitle" runat="server" Text="Select units from this location's equipment list" />
                &nbsp; (recommended)
                <p style="font-size: 11px; color: #3A7728; padding-top: 5px;">
                This option automatically generates a service ticket</p></td><td>
                <asp:RadioButton ID="rbList" runat="server" GroupName="rbServiceTypeGroup" Text="Select From Your List" Checked="true" />
                <div style="clear: both; height: 5px;"></div>
                <asp:RadioButton ID="rbPm" runat="server" GroupName="rbServiceTypeGroup" Text="Preventative Maintenance Request from your list" Visible="false" />
            </td>
        </tr>
        <tr valign="top">
            <td align="right" class="auto-style2">&nbsp;</td><td class="auto-style4"><b>OR</b> indicate the service type and number of units to manually enter your request <p style="font-size: 11px; color: #3A7728; padding-top: 5px;">
                    These options generate an email to our Customer Service Reps <br />T&M will be created as a time & materials service ticket</p></td><td>
                <asp:RadioButton ID="rbContract" runat="server" GroupName="rbServiceTypeGroup" Text="Contract" />
                &nbsp;&nbsp; <asp:RadioButton ID="rbTM" runat="server" GroupName="rbServiceTypeGroup" Text="T & M" />
                &nbsp;&nbsp; <asp:DropDownList runat="server" ID="ddForcedQty" CssClass="dropDownList1" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td><td class="auto-style4">&nbsp;</td><td>
            <asp:Button ID="btContact" runat="server" 
                Text="Continue" 
                ValidationGroup="Contact" 
                onclick="btContact_Click" />
            </td>
        </tr>
    </table>

    <div style="height: 20px; clear: both;" />
    <asp:CustomValidator id="vCustom_Contact" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="Contact" />
    <asp:ValidationSummary ID="vSummary_Contact" runat="server" ValidationGroup="Contact" />
</asp:Panel>

</asp:Content>


<asp:Content ID="Content3" runat="server" contentplaceholderid="For_HtmlHead">
    <style type="text/css">
        .auto-style2 {
            width: 62px;
        }
        .auto-style4 {
            width: 483px;
        }
    </style>
</asp:Content>



