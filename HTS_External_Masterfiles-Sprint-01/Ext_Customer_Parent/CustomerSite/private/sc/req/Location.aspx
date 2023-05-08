<%@ Page Title="Service Request: Location Search" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Location.aspx.cs" 
    Inherits="private_sc_req_Location" %>
<%--
--%>
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
            
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
<%-- Page Title (And Admin Entry)  --%>             
    <table style="width: 100%;">
        <tr>
            <td>Service Request</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server" Visible="false" DefaultButton="btCs1Change">
                    <asp:Table ID="tbCs1Change" runat="server" HorizontalAlign="Right">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                                &nbsp;
                                <asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" 
                                    ValidationGroup="Cs1Change" />
                                    <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                        Operator="DataTypeCheck"
                                        Type="Integer"
                                        ControlToValidate="txCs1Change"
                                        ErrorMessage="Customer entry must be a number" 
                                        Text="*"
                                        SetFocusOnError="true" 
                                        ValidationGroup="Cs1Change"></asp:CompareValidator>
                                    &nbsp;
                                    <asp:Button ID="btCs1Change" runat="server" 
                                        Text="Change Customer" 
                                        onclick="btCs1Change_Click"
                                        ValidationGroup="Cs1Change" />
                                    <asp:ValidationSummary ID="vSumCs1Change" runat="server" ValidationGroup="Cs1Change" />
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table>
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
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

<%-- Hidden Fields  --%>             
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfPri" runat="server" Value="" Visible="false" />

<table class="tableAsFence">
    <tr>
        <td>

<asp:Panel ID="pnCs1Header" runat="server" Visible="false" />

<%-- Search Boxes to Refine Lower Location Table To Pick One Location (or All) --%>             
<asp:Panel ID="pnLocSearch" runat="server" DefaultButton="btLocSearch">

<span style="width: 100%; text-align: left;">
    <asp:Label ID="lbStep1" runat="server" SkinID="labelSteps" Text="Step 1" />
    &nbsp;&nbsp; <b>Find and select the location requiring service below</b> &nbsp;&nbsp;&nbsp; (or if the unit's location is not certain, you may search globally by <asp:LinkButton ID="lkSerialSearch" runat="server" Text="Serial or Asset Cross Ref" PostBackUrl="~/private/sc/req/Serial.aspx" />)</span> <asp:Table ID="tbInput" runat="server" CssClass="tableWithoutLines">
        <asp:TableRow VerticalAlign="Top">
            <asp:TableCell ColumnSpan="12" HorizontalAlign="Left" />
        </asp:TableRow>
        <asp:TableHeaderRow>
            <asp:TableHeaderCell Text="Name" />
            <asp:TableHeaderCell>
                <asp:Label ID="lbCust" runat="server" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell Text="Loc" />
            <asp:TableHeaderCell Text="CustXRef" />
            <asp:TableHeaderCell>
                <asp:Label ID="lbAddress" runat="server" />
            </asp:TableHeaderCell>
            <asp:TableHeaderCell Text="City" />
            <asp:TableHeaderCell Text="St" />
            <asp:TableHeaderCell Text="Zip" />
            <asp:TableHeaderCell Text="Phone" />
            <asp:TableHeaderCell ColumnSpan="3" />
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="txNam" runat="server" Width="110" MaxLength="40" Text="" />
            </asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="ddCs1Family" runat="server" 
                    CssClass="dropDownList1" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txCs2" runat="server" Width="35" MaxLength="3" />
                    <asp:CompareValidator id="vCompare_Cs2" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txCs2"
                        ErrorMessage="Location must be an integer" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txXrf" runat="server" Width="60" MaxLength="15" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txAdr" runat="server" Width="100" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txCit" runat="server" Width="120" MaxLength="30" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txSta" runat="server" Width="25" MaxLength="2" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txZip" runat="server" Width="40" MaxLength="9" />
                    <asp:CompareValidator id="vCompare_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txPhn" runat="server" Width="70" MaxLength="10" />
                    <asp:CompareValidator id="vCompare_Phn" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Double"
                        ControlToValidate="txPhn"
                        ErrorMessage="Phone entry must only contain numbers" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    ValidationGroup="LocSearch" 
                    onclick="btLocSearch_Click" />
            </asp:TableCell>
            <asp:TableCell>
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearLocSearch();" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:CustomValidator id="vCustom_LocSearch" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="LocSearch" />
    <asp:ValidationSummary ID="vSum_LocSearch" runat="server" ValidationGroup="LocSearch" />
    <div style="height: 10px;">&nbsp;</div>
</asp:Panel>

<%-- Show Selected Customer Locations --%>
<asp:Panel ID="pnLocations" runat="server" Visible="false">

      <asp:GridView ID="gvLocations" runat="server" style="width:100%"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines"
         AllowPaging="true"
         PageSize="50" 
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Loc" 
         onsorting="gvSorting_Loc"
         EmptyDataText="No matching locations were found">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:TemplateField HeaderText="Eqp?" SortExpression="EqpCount">
            <ItemTemplate>
                <center>
                    <asp:Label ID="lbCount" runat="server" Text='<%# Eval("EqpCount") %>' />
                </center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name" SortExpression="CustNam">
            <ItemTemplate>
                <asp:LinkButton ID="lkName" runat="server" 
                    CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc")%>'
                    OnClick="lkName_Click" 
                    PostBackUrl="~/private/sc/req/Contact.aspx" 
                    Text='<%# Eval("CustNam") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CustNum" HeaderText="Customer" SortExpression="CustNum" />
        <asp:BoundField DataField="CustLoc" HeaderText="Location" SortExpression="CustLoc" />
        <asp:BoundField DataField="CustXrf" HeaderText="CustXRef" SortExpression="CustXrf" />
        <asp:BoundField DataField="CustAdr" HeaderText="Address" SortExpression="CustAdr" />
        <asp:BoundField DataField="CustCit" HeaderText="City" SortExpression="CustCit" />
        <asp:BoundField DataField="CustSta" HeaderText="State" SortExpression="CustSta" />
        <asp:BoundField DataField="CustZip" HeaderText="Zip" SortExpression="CustZip" />
        <asp:BoundField DataField="CustPhn" HeaderText="Phone" SortExpression="CustPhn" />
        
    </Columns>
</asp:GridView>
</asp:Panel>

</table>
        </td>
    </tr>
</table>

</asp:Content>


