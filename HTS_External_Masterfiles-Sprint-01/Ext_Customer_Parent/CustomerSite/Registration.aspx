<%@ Page Title="Customer Registration" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" 
    Inherits="Registration" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Registration 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<%-- BODY CONTENT --%>
<%-- --%>
    <table id="tbContentBlock" class="contentBlock">
    <tr>
        <td>
            <asp:Panel ID="pnBodyRoundedFrame" runat="server" BackColor="White">
                <table id="tbBodyFrameExpander" runat="server" class="tableFrameExpander">
                    <tr>
                        <td style="padding-left: 7px; padding-right: 7px;">

                        <%-- START: INSIDE ROUNDED FRAME --%>
                            <asp:Panel ID="pnOne" runat="server" Visible="true" 
        DefaultButton="btRegId">
    <table style="width: 100%" class="tableWithoutLines">
<!--  VALIDATION GROUP ONE  ================================================== -->
        <tr>
            <td align="right" style="width: 300px;">
                <asp:Label ID="lbStep1" runat="server" 
                    Text="Step 1 of 3"
                    SkinID="labelSteps"  />
            </td>
            <td align="left" style="width: 500px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbCs1" runat="server" 
                    Text="Customer Number" 
                    AssociatedControlID="txCs1" />
            </td>
            <td align="left">
                <asp:TextBox ID="txCs1" runat="server" />
                <asp:RequiredFieldValidator ID="Cs1Req" runat="server" 
                    ControlToValidate="txCs1" 
                    ErrorMessage="Your customer number is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="RegId" />
                <asp:CompareValidator id="vComCs1Numeric" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txCs1"
                    ErrorMessage="Customer number must be an integer" 
                    Text="*"
                    SetFocusOnError="true"
                    Display="Dynamic"
                    ValidationGroup="RegId"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">&nbsp;</td>
            <td style="text-align: left;">
                <asp:Label ID="lbAccessInfo" runat="server" 
                    Text="<i>If you happen to be the account administrator<br />you may add administrative privileges to this account either now, or at a later time.<br />Just call (800) 228-3628 and ask for help with 'Web Admin Registration'</i><br /> Otherwise, please continue with basic registration by clicking the button below."
                    Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbAccess" runat="server" Font-Size="8" 
                    Text="(Optional code for admin privileges)" 
                    AssociatedControlID="txCod" 
                    Visible="false" />
            </td>
            <td align="left">
                <asp:TextBox ID="txCod" runat="server" 
                    TextMode="Password" 
                    MaxLength="30"
                    BackColor="#EEEEEE" 
                    Visible="false" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">&nbsp;</td>
            <td style="text-align: left; vertical-align: top;">
                <asp:Button ID="btRegId" runat="server" 
                    Text="Continue" 
                    onclick="btRegId_Click" 
                    ValidationGroup="RegId" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left">
                <asp:CustomValidator id="vCusRegId" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="RegId" />
                <asp:ValidationSummary ID="vSumRegId" runat="server" 
                    ValidationGroup="RegId" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfCod" runat="server" Visible="false" />
    </asp:Panel>

<!--  VALIDATION GROUP TWO  ================================================== -->
    <asp:Panel ID="pnTwo" runat="server" Visible="false" 
        DefaultButton="btUserID">
    <table width="800px" class="tableWithoutLines">
        <tr>
            <td align="right" style="width: 300px;">
                <asp:Label ID="lbStep2" runat="server" 
                    Text="Step 2 of 3"
                    SkinID="labelSteps"  />
            </td>
        </tr>
        <tr>
            <td align="right">
                Customer Number
            </td>
            <td align="left" style="width: 500px;">
                <asp:Label ID="lbStep2Cs1" runat="server" Text="" 
                    SkinID="labelInformation" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 300px;">
                <asp:Label ID="lbUserID" runat="server" 
                    Text="Select a UserID" 
                    AssociatedControlID="txUserID" />
            </td>
            <td align="left" style="width: 500px;">
                <asp:TextBox ID="txUserID" runat="server" />
                <asp:RequiredFieldValidator ID="vReqUserID" runat="server" 
                    ControlToValidate="txUserID" 
                    ErrorMessage="A User ID is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="UserID" />
            </td>
        </tr>
                <tr>
            <td align="right" style="width: 300px;">
            </td>
            <td align="left" style="width: 500px;">
                <asp:Label ID="lbUserFriendly" runat="server" 
                    Text="Although a purely numeric UserID is permitted, please consider a more memorable and user friendly name for your account." />
            </td>
        </tr>

        <tr>
            <td></td>
            <td align="left">
                <asp:Button ID="btUserID" runat="server" 
                    Text="Check Availability" 
                    onclick="btUserID_Click" 
                    ValidationGroup="UserID" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left">
                <asp:CustomValidator id="vCusUserID" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="UserID" />
                <asp:ValidationSummary ID="vSumUserID" runat="server" 
                    ValidationGroup="UserID" />
            </td>
        </tr>

    </table>
</asp:Panel>
 <%-- 

  --%>    
<!--  VALIDATION GROUP THREE: Registration Data ========================= -->
    <asp:Panel ID="pnThree" runat="server" Visible="false" 
        DefaultButton="btRegData">
    <table width="800px" class="tableWithoutLines">
        <tr>
            <td align="right" style="width: 300px;">
                <asp:Label ID="lbStep3" runat="server" 
                    Text="Step 3 of 3"
                    SkinID="labelSteps"  />
            </td>
            <td align="left" style="width: 500px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbUserIDTitle" runat="server" 
                    Text="User ID" />
            </td>
            <td align="left">
                <asp:Label ID="lbUserIDDisplay" runat="server" SkinID="labelSteps" />
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="lbContact" runat="server" 
                    AssociatedControlID="txContact" 
                    Text="Contact Name" />
            </td>
            <td align="left" >
                <asp:TextBox ID="txContact" runat="server" 
                    Width="300" MaxLength="30" />
                <asp:RequiredFieldValidator ID="ContactReq" runat="server" 
                    ControlToValidate="txContact" 
                    ErrorMessage="A contact name is required." 
                    Text="*"
                    ValidationGroup="RegData" />
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="lbPhone" runat="server" 
                    Text="Contact Phone" />
            </td>
            <td align="left" >
                (
                <asp:TextBox ID="txPhone1" runat="server" Width="40" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vReq_Phone1" runat="server" 
                        ControlToValidate="txPhone1" 
                        ErrorMessage="Please include an area code." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="RegData" />
                    <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone1"
                        ErrorMessage="Area code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="RegData">
                    </asp:CompareValidator>
                )
                &nbsp;<asp:TextBox ID="txPhone2" runat="server" Width="40" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vReq_Phone2" runat="server" 
                        ControlToValidate="txPhone2" 
                        ErrorMessage="Please include a phone prefix." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="RegData" />
                    <asp:CompareValidator id="vCompare_txPhone2" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone2"
                        ErrorMessage="Phone prefix must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="RegData">
                    </asp:CompareValidator>
                -&nbsp;<asp:TextBox ID="txPhone3" runat="server" Width="50" MaxLength="4" />
                    <asp:RequiredFieldValidator ID="vReq_Phone3" runat="server" 
                        ControlToValidate="txPhone3" 
                        ErrorMessage="Please include a phone suffix." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="RegData" />
                    <asp:CompareValidator id="vCompare_txPhone3" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone3"
                        ErrorMessage="Phone suffix must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="RegData">
                    </asp:CompareValidator>
                &nbsp; Ext: <asp:TextBox ID="txExtension" runat="server" Width="65" MaxLength="8" />
                    <asp:CompareValidator id="vCompare_Extension" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txExtension"
                        ErrorMessage="The extension must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="RegData">
                    </asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbEmail" runat="server" 
                    AssociatedControlID="txEmail" 
                    Text="Email" />
            </td>
            <td align="left" >
                <asp:TextBox ID="txEmail" runat="server" 
                    Width="300" 
                    MaxLength="50" />
                <asp:RequiredFieldValidator ID="vReqEmail" runat="server" 
                    ControlToValidate="txEmail" 
                    ErrorMessage="E-mail is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="RegData" />
                <asp:RegularExpressionValidator id="vRegEmail" runat="server"
                    ControlToValidate="txEmail"
                    ValidationExpression="^\S+@\S+\.\S+$"
                    ErrorMessage="Email address is not in a valid format" 
                    Text="*"
                    SetFocusOnError="true"
                    Display="Dynamic"
                    ValidationGroup="RegData" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbEmail2" runat="server" 
                    AssociatedControlID="txEmail2"
                    Text="Email Confirmation" />
            </td>
            <td align="left" >
                <asp:TextBox ID="txEmail2" runat="server" 
                    Width="300" 
                    MaxLength="50"  />
                <asp:RequiredFieldValidator ID="vReqEmail2" runat="server" 
                    ControlToValidate="txEmail2" 
                    ErrorMessage="E-mail confirmation is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="RegData" />
                <asp:CompareValidator ID="vComEmail2" runat="server" 
                    ControlToCompare="txEmail" 
                    ControlToValidate="txEmail2" 
                    Display="Dynamic" 
                    ErrorMessage="The email and confirmation must match." 
                    Text="*"
                    ValidationGroup="RegData" />
            </td>
        </tr>
                <tr>
            <td align="right">
                <asp:Label ID="lbPwd" runat="server" 
                    AssociatedControlID="txPassword" 
                    Text="Account Password" />
            </td>
            <td align="left">
                <asp:TextBox ID="txPassword" runat="server" 
                    TextMode="Password" 
                    Width="300" 
                    MaxLength="15" />
                <asp:RequiredFieldValidator ID="vReqPassword" runat="server" 
                    ControlToValidate="txPassword" 
                    ErrorMessage="Password is required." 
                    Text="*"
                    display="dynamic"
                    ValidationGroup="RegData" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbPwd2" runat="server" 
                    AssociatedControlID="txPassword2" 
                    Text="Confirm Password" />
            </td>
            <td align="left" >
                <asp:TextBox ID="txPassword2" runat="server" 
                    TextMode="Password" 
                    Width="300"
                    MaxLength="15" />
                <asp:RequiredFieldValidator ID="vReqPwd2" runat="server" 
                    ControlToValidate="txPassword2" 
                    ErrorMessage="Password confirmation is required." 
                    Text="*"
                    ValidationGroup="RegData" />
                <asp:CompareValidator ID="vComPassword" runat="server" 
                    ControlToCompare="txPassword" 
                    ControlToValidate="txPassword2" 
                    Display="Dynamic" 
                    ErrorMessage="The password and confirmation must match." 
                    Text="*"
                    ValidationGroup="RegData" />
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td align="left" >
                <asp:Label ID="lbPasswordRules" runat="server" 
                    Text="Password: 7 to 15 characters using uppercase, lowercase and a number" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbGrantAdmin" runat="server" 
                    AssociatedControlID="chBxGrantAdmin" 
                    Text="Grant Administrative Privileges?" 
                    Visible="false" />
            </td>
            <td align="left" >
                <asp:CheckBox ID="chBxGrantAdmin" runat="server" Visible="false" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left" >
                <asp:Button ID="btRegData" runat="server" 
                    Text="Submit Registration" 
                    onclick="btRegData_Click" 
                    ValidationGroup="RegData" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left">
                <asp:CustomValidator id="vCusRegData" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="RegData" />
                <asp:ValidationSummary ID="vSumRegData" runat="server" ValidationGroup="RegData" />
            </td>
        </tr>

<!-- ============================================================ -->

        <tr>
            <td colspan="2">
                <asp:TextBox ID="access" runat="server" 
                    Visible="false" />
            </td>
        </tr>
    </table>
    </asp:Panel>


                        <%-- END: INSIDE ROUNDED FRAME --%>
                        </td>
                    </tr>
                </table><!-- end tbBodyFrameExpander -->
            </asp:Panel><!-- end pnBodyRoundedFrame -->          
        </td>
    </tr>
</table><!-- end tbContentBlock -->

<asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server"
    TargetControlID="pnBodyRoundedFrame"
    Radius="7"
    Corners="All" >
</asp:RoundedCornersExtender>

</asp:Content>

