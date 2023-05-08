<%@ Page Title="Partner Renewals" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="DealerCustomers.aspx.cs" 
    Inherits="private_sc_DealerCustomers" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Partner Renewals</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server">
                    <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                                &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" ValidationGroup="Cs1Change" />
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
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table></asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    
    <script type="text/javascript">
        // =============================================================
        function clearLocSearch()
        {
            var doc = document.forms[0];
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family != "undefined")
            {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family.value = "";
            }
          
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txDlrNam.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txContract.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txExpDate.value = "";
            return true;
        }
      
    </script>


<asp:Panel ID="pnLocSearch" runat="server">

    <asp:Table ID="tbLocSearch" runat="server" CssClass="tableWithoutLines" style="width: 100%;">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Contract</asp:TableHeaderCell><asp:TableHeaderCell>Expiring prior to: (YYYYMMDD) </asp:TableHeaderCell><asp:TableHeaderCell>Dealer Name</asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
           <asp:TableCell><asp:TextBox ID="txContract" runat="server"  MaxLength="8" /></asp:TableCell><asp:TableCell><asp:TextBox ID="txExpDate" runat="server" MaxLength="8" /></asp:TableCell> <asp:TableCell><asp:TextBox ID="txDlrNam" runat="server"  MaxLength="40" /></asp:TableCell><asp:TableCell>
            <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    ValidationGroup="LocSearch" 
                    onclick="btLocSearch_Click" />
            </asp:TableCell><asp:TableCell>
            <asp:Button ID="btDownLoad" runat="server" 
                    Text="Download" 
                    onclick="btLocSearch_Click" />
            </asp:TableCell><asp:TableCell>
                <input id="btClearLocSearch" type="button" 
                    value="Clear" 
                    class="buttonDefault" 
                    onclick="javascript: clearLocSearch();" /></asp:TableCell></asp:TableRow></asp:Table><asp:ValidationSummary ID="vSummary_LocSearch" runat="server" ValidationGroup="LocSearch" />
</asp:Panel><!-- end pnLocSearch -->

<asp:Label ID="lbError" runat="server" SkinID="labelError" />

<asp:Panel ID="pnLocations" runat="server" Visible="false">

  <asp:GridView ID="gvLocations" runat="server" Width="100%"
        CssClass="tableWithLines" 
        AutoGenerateColumns="False" 
        AllowPaging="True" 
        PageSize="750" 
        AllowSorting="True" 
        onpageindexchanging="gvPageIndexChanging_Loc" 
        onsorting="gvSorting_Loc" 
        onselectedindexchanged="gvLocations_SelectedIndexChanged">
      <AlternatingRowStyle CssClass="trColorAlt" />
    <Columns>
        <asp:BoundField DataField="Contract" HeaderText="Contract" SortExpression="Contract" />
        <asp:BoundField DataField="Start Date" HeaderText="Start Date" SortExpression="Start Date" />
        <asp:BoundField DataField="End Date" HeaderText="End Date" SortExpression="End Date" />   
        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
        <asp:BoundField DataField="DealerName" HeaderText="Dealer Name" SortExpression="DealerName" />
         <asp:BoundField DataField="Part" HeaderText="Equipment" SortExpression="Part" />   
         <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc" />   
        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
    </Columns>
</asp:GridView>
</asp:Panel>


<div id="dvDisplay" runat="server">
    <asp:Panel ID="pnDisplay" runat="server"></asp:Panel>
</div><!-- End dvDisplay ** DO NOT DELETE ** This is dynamically loaded with user selections -->

<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfXref" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfUnitList" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfXrefLocEditor" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfXrefEqpEditor" runat="server" Value="" Visible="false" />

</asp:Content>



