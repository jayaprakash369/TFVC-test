<%@ Page Title="Email Management" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="EmailManagement.aspx.cs" Inherits="private_sc_EmailManagement" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
<%--  --%>             
    <table style="width: 100%;">
        <tr>
            <td>Email Management</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server" DefaultButton="btCs1Change">
                    <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                                &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" />
                                   <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                        Operator="DataTypeCheck"
                                        Type="Integer"
                                        ControlToValidate="txCs1Change"
                                        ErrorMessage="Customer entry must be a number" 
                                        Text="*"
                                        SetFocusOnError="true" 
                                        ValidationGroup="LocSearch">
                                    </asp:CompareValidator>
                                    &nbsp;
                                    <asp:Button ID="btCs1Change" runat="server" 
                                        Text="Change Customer" 
                                        onclick="btCs1Change_Click"
                                        ValidationGroup="Cs1Change" />
                                <asp:ValidationSummary ID="vSummary_AdminCs1" runat="server" ValidationGroup="Cs1Change" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
        // =============================================================
        function clearLocSearch() {
            var doc = document.forms[0];
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family.value = "";
            }
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr.value = "";
            }
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCs2.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txNam.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCit.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSta.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txZip.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txPhn.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txXrf.value = "";
            return true;
        }
    </script>
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />

<%-- Search Boxes to Refine Lower Location Table To Pick One Location (or All) --%>             
    <asp:Panel ID="pnOneOrAllLocs" runat="server" DefaultButton="btLocSearch">
    <asp:Table ID="tbInput" runat="server" CssClass="tableWithoutLines">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="4" HorizontalAlign="Left">
                <asp:Label ID="lbSearchInstructions" runat="server" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="4" HorizontalAlign="Left">
                <asp:Button ID="btAll" runat="server" 
                    Text="Switch To Manage ALL Locations" 
                    ValidationGroup="AllSearch"
                    onclick="AllLocations_Click" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="3" HorizontalAlign="Left">
                <asp:RadioButton ID="rbFamilyGlobal" runat="server" 
                    GroupName="rbFamilyGroup" 
                    Text="All locations of <b>ALL</b> customer groups" 
                    Visible="false"
                    Checked="true" />
                <br />
                <asp:RadioButton ID="rbFamilyCs1" runat="server" 
                    GroupName="rbFamilyGroup" 
                    Text="All locations of <b>ONE</b> customer group" 
                    Visible="false"
                    />

                <asp:CustomValidator id="vCusAllLoc" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="AllSearch" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>
                <asp:Label ID="lbCust" runat="server" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>Loc</asp:TableHeaderCell>
            <asp:TableHeaderCell>
                <asp:Label ID="lbAddress" runat="server" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>City</asp:TableHeaderCell>
            <asp:TableHeaderCell>St</asp:TableHeaderCell>
            <asp:TableHeaderCell>Zip</asp:TableHeaderCell>
            <asp:TableHeaderCell>Phone</asp:TableHeaderCell>
            <asp:TableHeaderCell>XRef</asp:TableHeaderCell>
            <asp:TableHeaderCell ColumnSpan="2"></asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="txNam" runat="server" Columns="12" MaxLength="40" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="ddCs1Family" runat="server" 
                    CssClass="dropDownList1" />
            </asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txCs2" runat="server" Columns="3" MaxLength="3"></asp:TextBox>
               <asp:CompareValidator id="vCustom_Cs2" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txCs2"
                    ErrorMessage="Location must be an integer" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txAdr" runat="server" Columns="15" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txCit" runat="server" Columns="15" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txSta" runat="server" Columns="2" MaxLength="2" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txZip" runat="server" Columns="5" MaxLength="9" />
                    <asp:CompareValidator id="vCustom_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txPhn" runat="server" Columns="8" MaxLength="10" />
                    <asp:CompareValidator id="vCustom_Phn" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhn"
                        ErrorMessage="The phone must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txXrf" runat="server" Columns="8" MaxLength="15" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    onclick="btLocSearch_Click" />
            </asp:TableCell>
            <asp:TableCell>
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearLocSearch();" /></asp:TableCell><asp:TableCell>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ValidationSummary ID="vSummary_LocSearch" runat="server" ValidationGroup="LocSearch" />
    <asp:ValidationSummary ID="vSumAllSearch" runat="server" ValidationGroup="AllSearch" />
</asp:Panel>

<asp:Label ID="lbError" runat="server" SkinID="labelError" />
<asp:Label ID="lbMessage" runat="server" SkinID="labelInstructions" />

<%-- Show Selected Customer Locations --%> 
    <asp:Panel ID="pnLocations" runat="server" Visible="false">
    <br />
      <asp:GridView ID="gvLocations" runat="server"
         AutoGenerateColumns="False"
         SkinId="gridViewDefault"
         AllowPaging="true"
         PageSize="750" 
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Loc" 
         onsorting="gvSorting_Loc">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="CustNam">
            <ItemTemplate>
                <asp:LinkButton ID="lkName" runat="server" OnClick="lkName_Click" 
                    CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc")%>'>
                    <%# Eval("CustNam") %>
                </asp:LinkButton></ItemTemplate>
            </asp:TemplateField>
        <asp:BoundField DataField="CustNum" HeaderText="Customer" SortExpression="CustNum" />
        <asp:BoundField DataField="CustLoc" HeaderText="Location" SortExpression="CustLoc" />
        <asp:BoundField DataField="CustAdr" HeaderText="Address" SortExpression="CustAdr" />
        <asp:BoundField DataField="CustCit" HeaderText="City" SortExpression="CustCit" />
        <asp:BoundField DataField="CustSta" HeaderText="State" SortExpression="CustSta" />
        <asp:BoundField DataField="CustZip" HeaderText="Zip" SortExpression="CustZip" />
        <asp:BoundField DataField="CustPhn" HeaderText="Phone" SortExpression="CustPhn" />
        <asp:BoundField DataField="CustXrf" HeaderText="CustRef" SortExpression="CustXrf" />
    </Columns>
</asp:GridView>
</asp:Panel>

<%-- Panel for Email Lists  --%>             
<%-- CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>  --%> 
<asp:Panel ID="pnEmail" runat="server" Visible="false">
    <asp:Label ID="lbEmailCust" runat="server" Text="" SkinID="labelTitleColor1_Medium" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btToggleScreen" runat="server" 
        Text="" 
        ValidationGroup="AllSearch"
        onclick="btToggleScreen_Click" />
   
    <%-- START: Wrap Table --%> 
    <table style="width: 100%;" >
        <tr>
            <td colspan="2">
                <asp:Label ID="lbTarget" runat="server" SkinID="labelTitleColor1_Small" />
                <asp:ValidationSummary ID="vSummary_Open" runat="server" ValidationGroup="OpenEmailAdd" />
                <asp:ValidationSummary ID="vSummary_Close" runat="server" ValidationGroup="CloseEmailAdd" />
            </td>
        </tr>
        <tr valign="top">
            <td align="center">
                <%-- START: Left Column Table --%> 
                <table>
                    <tr class="tableWithoutLines">
                        <td align="right">
                            <asp:Label ID="lbOpen" runat="server" Text="Send an email when a call OPENS?" />
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblOpen" runat="server" 
                                RepeatDirection="Horizontal" 
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="rblEmailSwitch_Click" >
                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem><asp:ListItem Text="No" Value="NO"></asp:ListItem></asp:RadioButtonList></td></tr><tr>
                        <td colspan="2" align="center">
                            <%-- START: Open Add Email Table --%>
                            <asp:Panel ID="pnOpenAdd" runat="server" DefaultButton="lkOpenAdd">
                            <asp:Table ID="tbOpenAdd" runat="server" CssClass="tableWithLines">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell ColumnSpan="2">Open Emails: Add New Address</asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                                    <asp:TableCell>
                                        <asp:LinkButton ID="lkOpenAdd" runat="server" 
                                            Text="Add" 
                                            OnClick="add_Click" 
                                            CommandArgument="OPN" 
                                            ValidationGroup="OpenEmailAdd" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txOpenAdd" runat="server" Width="250" MaxLength="50" />
                                            <asp:RegularExpressionValidator id="vRegular_Email" runat="server"
                                                ControlToValidate="txOpenAdd"
                                                ValidationExpression="^\S+@\S+\.\S+$"
                                                ErrorMessage="The email address is not in a valid format" 
                                                Text="*"
                                                SetFocusOnError="true"
                                                Display="Dynamic"
                                                ValidationGroup="OpenEmailAdd">
                                            </asp:RegularExpressionValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            </asp:Panel>
                            <%-- END: Open Add Email Table --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="padding-top: 40px;">

                            <asp:Repeater ID="rpOpenDel" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <%-- START: Open Repeater Table --%> 
                                    <table class="tableWithLines">
                                        <tr>
                                          <th colspan="2">Open Emails: Current Addresses</th></tr></HeaderTemplate><ItemTemplate>
                                    <tr class="trColorReg">
                                      <td align="center">
                                        <asp:LinkButton ID="lkOpenEmail1" runat="server" OnClick="delete_Click" 
                                            CommandArgument='<%# Eval("Email") + "|OPN" %>'>
                                            Delete
                                        </asp:LinkButton></td><td style="text-align: left; padding-left: 10px;"><%# Eval("Email") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="trColorAlt">
                                      <td align="center">
                                        <asp:LinkButton ID="lkOpenEmail2" runat="server" OnClick="delete_Click" 
                                            CommandArgument='<%# Eval("Email") + "|OPN" %>'>
                                            Delete
                                        </asp:LinkButton></td><td style="text-align: left; padding-left: 10px;"><%# Eval("Email") %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </table>
                                    <%-- END: Open Repeater Table --%> 
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
                <%-- END: Left Column Table --%> 
            </td>
            <td align="center">
                <%-- START: Right Column Table --%> 
                <table>
                    <tr class="tableWithoutLines">
                        <td align="right">
                            <asp:Label ID="lbClose" runat="server" Text="Send an email when a call CLOSES?" />
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblClose" runat="server" 
                                RepeatDirection="Horizontal"
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="rblEmailSwitch_Click" >
                                <asp:ListItem Text="Yes" Value="YES" />
                                <asp:ListItem Text="No" Value="NO" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <%-- START: Close Add Email Table --%> 
                            <asp:Panel ID="pnCloseAdd" runat="server" DefaultButton="lkCloseAdd">
                            <asp:Table ID="tbCloseAdd" runat="server" CssClass="tableWithLines">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell ColumnSpan="2">Close Emails: Add New Address</asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                                    <asp:TableCell>
                                        <asp:LinkButton ID="lkCloseAdd" runat="server" 
                                            Text="Add" 
                                            OnClick="add_Click" 
                                            CommandArgument="CLS" 
                                            ValidationGroup="CloseEmailAdd" />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:TextBox ID="txCloseAdd" runat="server" Width="250" MaxLength="50" />
                                        <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                                                ControlToValidate="txCloseAdd"
                                                ValidationExpression="^\S+@\S+\.\S+$"
                                                ErrorMessage="The email address is not in a valid format" 
                                                Text="*"
                                                SetFocusOnError="true"
                                                Display="Dynamic"
                                                ValidationGroup="CloseEmailAdd">
                                            </asp:RegularExpressionValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    <%-- END: Close Add Email Table --%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" style="padding-top: 40px;">
                            <asp:Repeater ID="rpCloseDel" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <%-- START: Close Repeater Table --%> 
                                    <table class="tableWithLines">
                                        <tr>
                                          <th colspan="2">Close Emails: Current Addresses</th></tr></HeaderTemplate><ItemTemplate>
                                    <tr class="trColorReg">
                                      <td align="center">
                                        <asp:LinkButton ID="lkCloseEmail1" runat="server" OnClick="delete_Click" 
                                            CommandArgument='<%# Eval("Email") + "|CLS" %>'>
                                            Delete
                                        </asp:LinkButton></td><td style="text-align: left; padding-left: 10px;"><%# Eval("Email") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="trColorAlt">
                                      <td align="center">
                                        <asp:LinkButton ID="lkCloseEmail2" runat="server" OnClick="delete_Click" 
                                            CommandArgument='<%# Eval("Email") + "|CLS" %>'>
                                            Delete
                                        </asp:LinkButton></td><td style="text-align: left; padding-left: 10px;"><%# Eval("Email") %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </table>
                                    <%-- END: Close Repeater Table --%> 
                                </FooterTemplate>
                            </asp:Repeater>

                        </td>
                    </tr>
                </table>
                <%-- END: Right Column Table --%> 
            </td>
        </tr>
    </table>
    <%-- END: Wrap Table --%> 
</asp:Panel>

<%--  --%>             
</asp:Content>

