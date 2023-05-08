<%@ Page Title="Equipment leasing Info " Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="AssetLeasingLookup.aspx.cs" 
    Inherits="private_sc_AssetLeasingLookup" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Asset Lookup</td>
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
                                        onclick="btCs1Change_Click" />
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table></asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    
    <script type="text/javascript">
        // =============================================================
        function clearLocSearch() {
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSer.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txLoc.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txXref.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txAsset.value = "";
            return true;
        }
        function clearEqpInput() {
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_ddPrd.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txMod.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txDsc.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSer.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txFxa.value = "";
            return true;
        }
    </script>

<asp:Panel ID="pnCstXrfUpd" runat="server" Visible="false">
    <table style="width: 100%; background-color: #F5F5F5; border: 1px solid #888888; padding: 2px; margin-bottom: 5px;" class="tableWithoutLines">
        <tr>
            <td>
                Customer: <asp:Label ID="lbCstXrfUpdCs1" runat="server" Text="" />-<asp:Label ID="lbCstXrfUpdCs2" runat="server" Text="" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Label ID="lbError" runat="server" SkinID="labelError" />

<asp:Panel ID="pnEqpSearch" runat="server"   Visible="false">
<asp:Panel ID="pnCs1Header" runat="server" />
    <table class="tableWithoutLines tableBorder" style="width: 100%;">
        <tr style="font-weight: bold;">
            <td>Loc</td><td>Cust Xref</td><td>Serial</td><td>Asset ID</td></tr><tr>
            <td><asp:TextBox ID="txLoc" runat="server" MaxLength="35" Width="100"  /></td>
            <td><asp:TextBox ID="txXref" runat="server" MaxLength="15" Width="100"  /></td>
            <td><asp:TextBox ID="txSer" runat="server" MaxLength="25" Width="100"  /></td>
            <td><asp:TextBox ID="txAsset" runat="server" MaxLength="25" Width="100"  /></td>
            <td style="text-align: right;">
                <asp:Button ID="btEqpSearch" runat="server" Text="Search" OnClick="btEqpSearch_Click" />
                &nbsp;&nbsp; <input id="btEqpClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearLocSearch();" /><%--     &nbsp;&nbsp; <asp:Button ID="btDownload" runat="server" Text="Download" onclick="btEqpSearch_Click" /> --%></td></tr></table></asp:Panel><%-- Show Equipment based on selection criteria for customer  --%><div style="height: 20px;"></div>
   <asp:Panel ID="pnEquipment" runat="server" Visible="false">
      <asp:Label ID="lbEquipment" runat="server" SkinID="labelTitleColor1_Medium" />
      <asp:GridView ID="gvEquipment" runat="server" Width="100%"
          CssClass="tableWithLines" 
          AutoGenerateColumns="False" 
          AllowPaging="true"
          PageSize="750" 
          AllowSorting="True" 
          onpageindexchanging="gvPageIndexChanging_Eqp" 
          onsorting="gvSorting_Eqp">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
             <asp:BoundField DataField="LocName" HeaderText="Location Name" SortExpression="LocName" />
             <asp:BoundField DataField="Location" HeaderText="Location #" SortExpression="Location" />
             <asp:BoundField DataField="XREFCS" HeaderText="Cust Xref" SortExpression="XREFCS" />
            <asp:BoundField DataField="Part" HeaderText="Model" SortExpression="Part" />
            <asp:BoundField DataField="PartDesc" HeaderText="Model Description" SortExpression="PartDesc" />
            <asp:TemplateField HeaderText="Serial" SortExpression="Serial">
             <ItemTemplate>
                 <asp:LinkButton ID="lkSerial" runat="server" OnClick="lkSerialPick_Click" 
                        CommandArgument='<%# Eval("Location") + "|" + Eval("Serial")+ "|" + Eval("Agreement") %>'>
                        <%# Eval("Serial") %>
                 </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Unit #" SortExpression="Unit">
             <ItemTemplate>
                 <asp:LinkButton ID="lkUnit" runat="server" OnClick="lkUnitPick_Click" 
                        CommandArgument='<%# Eval("Unit")+ "|" + Eval("Location") %>'>
                        <%# Eval("Unit") %>
                 </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Asset Id" SortExpression="Asset">
             <ItemTemplate>
                 <asp:LinkButton ID="lkAsset" runat="server" OnClick="lkAssetPick_Click" 
                        CommandArgument='<%# Eval("Asset")+ "|" + Eval("Agreement") %>'>
                        <%# Eval("Asset") %>
            </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="Agreement" HeaderText="Agreement" SortExpression="Agreement" />
            </Columns></asp:GridView></asp:Panel>
                                
     <%-- Show Selected Tickets for Selected Location --%> 
    <div style="height: 20px;"></div>
    <asp:Panel ID="pnTickets" runat="server" Visible="false">
      <asp:Label ID="lbTckHist" runat="server" SkinID="labelTitleColor1_Medium" />
    <asp:GridView ID="gvTickets" runat="server" width="100%"
     AutoGenerateColumns="False" 
     CssClass="tableWithLines"
     AllowPaging="true"
     PageSize="500">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="CustName" HeaderText="Customer" />
        <asp:BoundField DataField="State" HeaderText="State" />
        <asp:BoundField DataField="CustXref" HeaderText="Cust XRef" />
        <asp:BoundField DataField="DateEnteredText" HeaderText="Date Entered" />
        <asp:TemplateField HeaderText="Ticket">
            <ItemTemplate>
                <asp:LinkButton ID="lkTicket" runat="server" OnClick="lkTicket_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Center") + "-" + Eval("Ticket") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="PurchOrd" HeaderText="Ticket XRef" /></Columns>
    </asp:GridView>
    </asp:Panel>

   <%-- Show Great American Leasing Agreement info for that asset id --%> 
      <asp:Panel ID="pnLeasingDetails" runat="server" Visible="false">
<asp:Label ID="lbLeaseHdr" runat="server" SkinID="labelTitleColor1_Medium" Text="Lease Information"/>
                <div style="height: 20px;"></div> 
  <table class="tableBorder" style="font-size: 0.9em;">
    <tr style="vertical-align: top;">
       <td>
         <table>
           <tr>
              <td class="auto-style4"></td>
              <td style="width: 300px;"><b>Customer</b></td></tr><tr>
              <td class="auto-style4">Name</td><td><asp:TextBox ID="txAcctName" runat="server" Width="280px" /></td>
          </tr>
          <tr>
             <td class="auto-style4">Address</td><td><asp:TextBox ID="txAcctAddrs" runat="server" Width="280px" /></td>
          </tr>
          <tr>
             <td class="auto-style4">City</td><td><asp:TextBox ID="txAcctCity" runat="server" Width="280px" /></td>
          </tr>
          <tr>
            <td class="auto-style4">State</td><td><asp:TextBox ID="txAcctSt" runat="server" Width="280px" /></td>
          </tr>
          <tr>
            <td class="auto-style4">Zip</td><td><asp:TextBox ID="txAcctZip" runat="server" Width="280px" /></td>
          </tr>
          <tr>
             <td class="auto-style4"></td>
               <td style="width: 300px;"><b>Payment</b></td></tr><tr>
              <td class="auto-style4">Total Payment</td><td><asp:TextBox ID="txCstPayment" runat="server" Width="280px" /></td>
            </tr>
               <tr>
              <td class="auto-style4">Balance</td><td><asp:TextBox ID="txAgrBalance" runat="server" Width="280px"/></td>
            </tr>
            <tr>
               <td class="auto-style4">Rent Payment</td><td><asp:TextBox ID="txRentPayment" runat="server" Width="280px" /></td>             
            </tr>
            <tr>
               <td class="auto-style4">Paid To Date</td><td><asp:TextBox ID="txPaid2Date" runat="server" Width="280px" /></td>
            </tr>
            <tr>
               <td class="auto-style4">Next Inv Date</td><td><asp:TextBox ID="txNxtInvDate" runat="server" Width="280px" /></td>
            </tr>
         </table>
       </td>
       <td>
          <table>
            <tr>
               <td class="auto-style3"></td>
               <td class="auto-style1"><b>Contract</b></td></tr><tr>
               <td class="auto-style3">Number</td><td class="auto-style1"><asp:TextBox ID="txAgrNo" runat="server" Width="350px" /></td>
            </tr>
            <tr>
               <td class="auto-style3">Start Date</td><td class="auto-style1"><asp:TextBox ID="txStrtDate" runat="server" Width="350px" /></td>
            </tr>
            <tr>
               <td class="auto-style3">Term Date Days</td><td class="auto-style1"><asp:TextBox ID="txDays2Term" runat="server" Width="350px" /></td>
            </tr>
               <tr>
               <td class="auto-style3">Billing Cycle</td><td class="auto-style1"><asp:TextBox ID="txBillingCycle" runat="server" Width="350px" /></td>             
            </tr>
            <tr>
               <td class="auto-style3">Billing Type</td><td class="auto-style1"><asp:TextBox ID="txBillingType" runat="server" Width="350px" /></td>
            </tr>
               <tr>
               <td class="auto-style3">Lease Terms</td><td class="auto-style1"><asp:TextBox ID="txLeaseTerms" runat="server" Width="350px" /></td>             
            </tr>
            <tr>
               <td class="auto-style3">Lease Type</td><td class="auto-style1"><asp:TextBox ID="txLeaseType" runat="server" Width="350px" /></td>
            </tr>
            <tr>
              <td class="auto-style3">Program Type</td><td class="auto-style1"><asp:TextBox ID="txPrgType" runat="server" Width="350px" /></td>
            </tr>
               <tr>
               <td class="auto-style3">Insurance Rate</td><td class="auto-style1"><asp:TextBox ID="txInsRate" runat="server" /></td>
            </tr>
         </table></td>
</tr></table>
    <div style="height: 20px;"></div>         
    <asp:Label ID="lbLeasing" runat="server" SkinID="labelTitleColor1_Medium" />
    <div style="height: 20px;"></div> 
    <asp:GridView ID="gvLeasingDtls" runat="server" width="100%"
     AutoGenerateColumns="False" 
     CssClass="tableWithLines"
     AllowPaging="true"
     PageSize="500">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="laDAssetDescription" HeaderText="Asset Descritpion" />
        <asp:BoundField DataField="laDAssetModel" HeaderText="Model" />
        <asp:BoundField DataField="laDAssetSerial" HeaderText="Serial" />
        <asp:BoundField DataField="laDPurchaseOption" HeaderText="Purchase Option" />
        <asp:BoundField DataField="laDAccountAddress" HeaderText="Asset Address" />
        <asp:BoundField DataField="laDAssetMachineID" HeaderText="Machine ID" />
        <asp:BoundField DataField="laDFinance" HeaderText="Financed" />
        <asp:BoundField DataField="ladAssetStatus" HeaderText="Asset Status" />
     </Columns>    </asp:GridView>
    </asp:Panel>


   <%-- Show Serial log          <asp:BoundField DataField="dispTicket" HeaderText="Ticket" />  --%> 
    <div style="height: 20px;"></div>
    <asp:Panel ID="pnAssetLog" runat="server" Visible="false">
      <asp:Label ID="lbLog" runat="server" SkinID="labelTitleColor1_Medium" />
    <asp:GridView ID="gvAssetLog" runat="server" width="100%"
     AutoGenerateColumns="False" 
     CssClass="tableWithLines"
     AllowPaging="true"
     PageSize="500">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="NEWUNT" HeaderText="Current Unit ID" />
        <asp:BoundField DataField="NEWSRL" HeaderText="Current Serial ID" />
        <asp:BoundField DataField="NEWFXA" HeaderText="Current Asset ID" />
        <asp:BoundField DataField="NEWMDL" HeaderText="Current Model" />
        <asp:BoundField DataField="NEWCST" HeaderText="Current Customer" />
        <asp:BoundField DataField="NEWLOC" HeaderText="Current Loc ID" />
        <asp:BoundField DataField="Action" HeaderText="Action" />
         <asp:BoundField DataField="dispDate" HeaderText="Updated" />
        </Columns>
    </asp:GridView>
    </asp:Panel>
  <div id="dvDisplay" runat="server">
    <asp:Panel ID="pnDisplay" runat="server"></asp:Panel>
</div><!-- End dvDisplay ** DO NOT DELETE ** This is dynamically loaded with user selections -->

                              
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfAsset" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfUnitList" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfXrefLocEditor" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfXrefEqpEditor" runat="server" Value="" Visible="false" />

</asp:Content>



<asp:Content ID="Content3" runat="server" contentplaceholderid="For_HtmlHead"><style type="text/css">                                                                                  .auto-style1 {
                                                                                      width: 414px;
                                                                                  }
                                                                                  .auto-style2 {
                                                                                      width: 191px;
                                                                                  }
                                                                                  .auto-style3 {
                                                                                      width: 156px;
                                                                                  }
                                                                                  .auto-style4 {
                                                                                      width: 135px;
                                                                                  }
                                                                              </style></asp:Content>



