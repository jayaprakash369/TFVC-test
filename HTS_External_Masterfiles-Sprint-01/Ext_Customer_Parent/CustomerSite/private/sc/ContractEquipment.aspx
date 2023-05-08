<%@ Page Title="Contract Equipment" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ContractEquipment.aspx.cs" 
    Inherits="private_sc_ContractEquipment" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Contract Equipment</td>
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
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table></asp:Content>

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
            <td>
                Your Customer Location Cross Reference:&nbsp;&nbsp; <asp:TextBox ID="txCstXrfUpd" runat="server" MaxLength="15" />
            </td>
            <td>
                <asp:Button ID="btCstXrfUpd" runat="server" 
                    Text="Update" 
                    OnClick="btCstXrfUpd_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnLocSearch" runat="server">

    <asp:Table ID="tbLocSearch" runat="server" CssClass="tableWithoutLines" style="width: 100%;">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell><asp:TableHeaderCell>
                <asp:Label ID="lbCust" runat="server" />
            </asp:TableHeaderCell><asp:TableHeaderCell>Loc</asp:TableHeaderCell><asp:TableHeaderCell>CustXRef</asp:TableHeaderCell><asp:TableHeaderCell>
                <asp:Label ID="lbAddress" runat="server" />
            </asp:TableHeaderCell><asp:TableHeaderCell>City</asp:TableHeaderCell><asp:TableHeaderCell>St</asp:TableHeaderCell><asp:TableHeaderCell>Zip</asp:TableHeaderCell><asp:TableHeaderCell>Phone</asp:TableHeaderCell><asp:TableHeaderCell></asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
            <asp:TableCell><asp:TextBox ID="txNam" runat="server" Columns="12" MaxLength="40" /></asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="ddCs1Family" runat="server" 
                    CssClass="dropDownList1" />
            </asp:TableCell><asp:TableCell><asp:TextBox ID="txCs2" runat="server" Columns="3" />
                <asp:CompareValidator id="vCustom_Cs2" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txCs2"
                    ErrorMessage="Location must be an integer" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txXrf" runat="server" Columns="8" MaxLength="15" />
            </asp:TableCell><asp:TableCell><asp:TextBox ID="txAdr" runat="server" Columns="15" MaxLength="30" /></asp:TableCell><asp:TableCell><asp:TextBox ID="txCit" runat="server" Columns="15" MaxLength="30" /></asp:TableCell><asp:TableCell><asp:TextBox ID="txSta" runat="server" Columns="2" MaxLength="2" /></asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txZip" runat="server" Columns="5" MaxLength="9" />
                    <asp:CompareValidator id="vCustom_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txPhn" runat="server" Columns="8" MaxLength="10" />
                    <asp:CompareValidator id="vCustom_Phn" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhn"
                        ErrorMessage="The phone must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    ValidationGroup="LocSearch" 
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
       
        <asp:TemplateField HeaderText="Name" SortExpression="CustNam">
            <ItemTemplate>
                <asp:LinkButton ID="lkName" runat="server" OnClick="lkName_Click" 
                    CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc")%>'>
                    <%# Eval("CustNam") %>
         </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="CustNum" HeaderText="Customer" SortExpression="CustNum" />
         <asp:BoundField DataField="CustLoc" HeaderText="Location" SortExpression="CustLoc" />
         <asp:BoundField DataField="CustXrf" HeaderText="CustXref" SortExpression="CustXrf" />

       <asp:TemplateField HeaderText="Edit Xref">
            <ItemTemplate>
                 <asp:LinkButton ID="lkCstXrf" runat="server" 
                   CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc") + "|" + Eval("CustXrf")%>'  
                    OnClick="lkCstXrfEdit_Click">
                 <asp:Image ID="imCstXrfEdit" runat="server" ImageUrl="~/media/scantron/art/button_edit.png" />
        </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="CustAdr" HeaderText="Address" SortExpression="CustAdr" />
        <asp:BoundField DataField="CustCit" HeaderText="City" SortExpression="CustCit" />
        <asp:BoundField DataField="CustSta" HeaderText="State" SortExpression="CustSta" />
        <asp:BoundField DataField="CustZip" HeaderText="Zip" SortExpression="CustZip" />
        <asp:BoundField DataField="CustPhn" HeaderText="Phone" SortExpression="CustPhn" />
    </Columns>
</asp:GridView>
</asp:Panel>

<asp:Panel ID="pnEqpXrfUpd" runat="server" Visible="false">
    <table style="width: 100%; background-color: #F5F5F5; border: 1px solid #888888; padding: 2px; margin-bottom: 5px;" class="tableWithoutLines">
        <tr>
            <td>
                Model: <asp:Label ID="lbEqpXrfUpdMod" runat="server" Text="" />
            </td>
            <td>
                Serial: <asp:Label ID="lbEqpXrfUpdSer" runat="server" Text="" />
            </td>
            <td>
                Your Equipment Cross Reference:&nbsp;&nbsp; <asp:TextBox ID="txEqpXrfUpd" runat="server" MaxLength="15" />
            </td>
            <td>
                <asp:Button ID="btEqpXrfUpd" runat="server" 
                    Text="Update" 
                    OnClick="btEqpXrfUpd_Click" />
                    <asp:HiddenField ID="hfEqpXrfUpdUnt" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnEqpSearch" runat="server" 
    Visible="false">
<asp:Panel ID="pnCs1Header" runat="server" />
    <table class="tableWithoutLines tableBorder" style="width: 100%;">
        <tr style="font-weight: bold;">
            <td>Category</td><td>Model</td><td>Model Description</td><td>Serial</td><td>Equip XRef</td><td>AgentId</td><td>&nbsp;</td></tr><tr>
            <td><asp:DropDownList ID="ddPrd" runat="server" CssClass="dropDownList1" /></td>
            <td><asp:TextBox ID="txMod" runat="server" MaxLength="15" Width="100" /></td>
            <td><asp:TextBox ID="txDsc" runat="server" MaxLength="35" Width="100"  /></td>
            <td><asp:TextBox ID="txSer" runat="server" MaxLength="25" Width="100"  /></td>
            <td><asp:TextBox ID="txFxa" runat="server" MaxLength="15" Width="100"  /></td>
            <td><asp:TextBox ID="txAgn" runat="server" MaxLength="25" Width="100"  /></td>
            <td style="text-align: right;">
                <asp:Button ID="btEqpSearch" runat="server" Text="Search" OnClick="btEqpSearch_Click" />
                &nbsp;&nbsp; <input id="btEqpClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearEqpInput();" />&nbsp;&nbsp; <asp:Button ID="btDownload" runat="server" 
                    Text="Download" 
                    onclick="btEqpSearch_Click" />
            </td>

        </tr>
    </table>
</asp:Panel>

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
            <asp:BoundField DataField="Part" HeaderText="Model" SortExpression="Part" />
            <asp:BoundField DataField="PartDesc" HeaderText="Model Description" SortExpression="PartDesc" />
            <asp:TemplateField HeaderText="Serial (Make Ticket)" SortExpression="Serial">
             <ItemTemplate>
                 <asp:LinkButton ID="lkXref" runat="server" OnClick="lkXrefPick_Click" 
                        CommandArgument='<%# Eval("Unit")+ "|" + Eval("Agreement") %>'>
                        <%# Eval("Serial") %>
                 </asp:LinkButton></ItemTemplate></asp:TemplateField>
            <asp:BoundField DataField="Asset" HeaderText="Asset" SortExpression="Asset" />            
            <asp:TemplateField HeaderText="Edit Xref">
                <ItemTemplate>
                    <asp:LinkButton ID="lkEqpXrf" runat="server" 
                        CommandArgument='<%# Eval("Part") + "|" + Eval("Serial") + "|" + Eval("Unit") + "|" + Eval("Asset")%>'  
                        OnClick="lkEqpXrfEdit_Click">
                        <asp:Image ID="imEqpXrfEdit" runat="server" ImageUrl="~/media/scantron/art/button_edit.png" />
                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="AgrDesc" HeaderText="Contract Type" SortExpression="AgrDesc" />
            <asp:BoundField DataField="AgentId" HeaderText="AgentId" SortExpression="AgentId" />
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



