<%@ Page Title="Change Contact Information" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ContactInformation.aspx.cs" 
    Inherits="private_custAdmin_ContactInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Change Contact Information</td>
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
                        </asp:TableCell></asp:TableRow></asp:Table></td></tr></table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<%--
--%>             
    <script type="text/javascript">
        // =============================================================
        function clearInput() {
            var doc = document.forms[0];
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family.value = "";
            }
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr.value = "";
            }
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCs2.value = "";
            doc.ctl00_ctl00_ctl00_For_Body_A_For_Body_A_txNam.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCon.value = "";

            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCit.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSta.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txZip.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txPhn.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txXrf.value = "";
            return true;
        }
    </script>

<center>
    <asp:Panel ID="pnUpdate" runat="server" Visible="false">
        <div id="dvUpdate" style=" background-color: #f5f5f5; border: 1px solid #aaaaaa; padding: 5px;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="lbCustomerName" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbCs1" runat="server" /> - <asp:Label ID="lbCs2" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txContact" runat="server" Width="110"
                            MaxLength="30"
                            ValidationGroup="Update" />
                        <asp:RequiredFieldValidator ID="vReqContact" runat="server" 
                            ControlToValidate="txContact" 
                            ErrorMessage="A contact name is required." 
                            Text="*"
                            Display="Dynamic"
                            ValidationGroup="Update" />
                    </td>
                    <td>
                        <asp:TextBox ID="txPhone" runat="server"  Width="80"
                            MaxLength="10"
                            ValidationGroup="Update" />
                            <asp:RequiredFieldValidator ID="vReqPhone" runat="server" 
                                ControlToValidate="txPhone" 
                                ErrorMessage="A phone number is required." 
                                Text="*"
                                Display="Dynamic"
                                ValidationGroup="Update" />
                            <asp:CompareValidator id="vComPhoneNumeric" runat="server" 
                                Operator="DataTypeCheck"
                                Type="Integer"
                                ControlToValidate="txPhone"
                                ErrorMessage="Please enter just the 10 numeric digits" 
                                Text="*"
                                SetFocusOnError="true"
                                Display="Dynamic"
                                ValidationGroup="Update" />

                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btUpdate" runat="server" 
                            Text="Update" 
                            ValidationGroup="Update" onclick="btUpdate_Click" /></td>
                </tr>
                <tr>
                    <td colspan="5">
                    <asp:ValidationSummary ID="vSumUpdate" runat="server" 
                        ValidationGroup="Update" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

<div id="dvInput">
   
    <asp:Table ID="tbInput" runat="server" CssClass="tableWithoutLines">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Contact</asp:TableHeaderCell>
            <asp:TableHeaderCell ID="thcCust">
                <asp:Label ID="lbCust" runat="server" Text="" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>Loc</asp:TableHeaderCell>
            <asp:TableHeaderCell>
                <asp:Label ID="lbAddress" runat="server" Text="" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>City</asp:TableHeaderCell>
            <asp:TableHeaderCell>St</asp:TableHeaderCell>
            <asp:TableHeaderCell>Zip</asp:TableHeaderCell>
            <asp:TableHeaderCell>Phone</asp:TableHeaderCell>
            <asp:TableHeaderCell>XRef</asp:TableHeaderCell>
            <asp:TableHeaderCell ColumnSpan="2">&nbsp;</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="txNam" runat="server" Width="100" MaxLength="40" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txCon" runat="server" Width="100" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="ddCs1Family" runat="server" 
                    CssClass="dropDownList1" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txCs2" runat="server" Width="30" />
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
                <asp:TextBox ID="txAdr" runat="server" Width="100" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txCit" runat="server" Width="100" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txSta" runat="server" Width="20" MaxLength="2" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txZip" runat="server" Width="40" MaxLength="9" />
                    <asp:CompareValidator id="vCustom_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txPhn" runat="server" Width="70" MaxLength="10" />
                    <asp:CompareValidator id="vCustom_Phn" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhn"
                        ErrorMessage="The phone must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txXrf" runat="server" Width="60" MaxLength="15" /></asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    ValidationGroup="LocSearch" 
                    onclick="btLocSearch_Click" />
                </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearInput();" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</div><!-- end parmDiv Div -->
    <asp:ValidationSummary ID="vSummary_LocSearch" runat="server" ValidationGroup="LocSearch" />
    <asp:Label ID="lbError" runat="server" SkinID="labelError" />

<asp:Panel ID="pnLocations" runat="server" Visible="false">
      <asp:GridView ID="gvLocations" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines"
         AllowPaging="true"
         PageSize="500" 
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Loc" 
         onsorting="gvSorting_Loc"
         EmptyDataText="No matching locations were found">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="CustNam" HeaderText="Customer Name" SortExpression="CustNam" />
        <asp:TemplateField HeaderText="Contact" SortExpression="Contact">
            <ItemTemplate>
                <asp:LinkButton ID="lkName" runat="server" OnClick="lbName_Click" 
                    CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc")%>'>
                    <%# Eval("Contact") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="CustNum" HeaderText="Customer" SortExpression="CustNum" />
        <asp:BoundField DataField="CustLoc" HeaderText="Location" SortExpression="CustLoc" />
        <asp:BoundField DataField="CustXrf" HeaderText="CustRef" SortExpression="CustXrf" />
        <asp:BoundField DataField="CustAdr" HeaderText="Address" SortExpression="CustAdr" />
        <asp:BoundField DataField="CustCit" HeaderText="City" SortExpression="CustCit" />
        <asp:BoundField DataField="CustSta" HeaderText="State" SortExpression="CustSta" />
        <asp:BoundField DataField="CustZip" HeaderText="Zip" SortExpression="CustZip" />
        <asp:BoundField DataField="CustPhn" HeaderText="Phone" SortExpression="CustPhn" />
    </Columns>
</asp:GridView>
</asp:Panel>
</center>
</asp:Content>

