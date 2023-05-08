<%@ Page Title="Page Counts: Device Search" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="PageCountEqp.aspx.cs" 
    Inherits="private_mp_PageCountEqp" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Page Counts: Device Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
        // =============================================================
        function clearEqpSearch() {
            var doc = document.forms[0];
//            if (typeof doc.ctl00_BodyContent_ddCs1Family != "undefined") {
//                doc.ctl00_BodyContent_ddCs1Family.value = "";
            //            }
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCs2.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txNam.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCit.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSta.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCrf.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txMod.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSer.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txMrf.value = "";
            return true;
        }
    </script>



<asp:Panel ID="pnEqpSearch" runat="server">
    <table style="width: 100%;" class="tableWithoutLines">
        <tr>
            <th>Name</th>
            <th><asp:Label ID="lbCust" runat="server" /></th>
            <th>Loc</th>
            <th>CustXrf</th>
            <th>City</th>
            <th>St</th>
            <th>Model</th>
            <th>Serial</th>
            <th>EquipXrf</th>
            <th></th>
        </tr>
        <tr>
            <td><asp:TextBox ID="txNam" runat="server" Width="80" MaxLength="40" /></td>
            <td><asp:DropDownList ID="ddCs1Family" runat="server" CssClass="dropDownList1" Font-Size="9" /></td>
            <td><asp:TextBox ID="txCs2" runat="server" Width="25" />
                <asp:CompareValidator id="vCustom_Cs2" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txCs2"
                    ErrorMessage="Location must be an integer" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="EqpSearch"></asp:CompareValidator>
                
            </td>
            <td><asp:TextBox ID="txCrf" runat="server" Width="50" MaxLength="15" /></td>
            <td><asp:TextBox ID="txCit" runat="server" Width="80" MaxLength="30" /></td>
            <td><asp:TextBox ID="txSta" runat="server" Width="20" MaxLength="2" /></td>
            <td><asp:TextBox ID="txMod" runat="server" Width="70" MaxLength="15" /></td>
            <td><asp:TextBox ID="txSer" runat="server" Width="80" MaxLength="30" /></td>
            <td><asp:TextBox ID="txMrf" runat="server" Width="60" MaxLength="15" /></td>
            <td><input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearEqpSearch();" /></td>
            <td>
                <asp:Button ID="btEqpSearch" runat="server" 
                    Text="Search" 
                    ValidationGroup="LocSearch" 
                    onclick="btEqpSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:CustomValidator id="vCus_EqpSearch" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="EqpSearch" />
    <asp:ValidationSummary ID="vSum_EqpSearch" runat="server" ValidationGroup="EqpSearch" />
</asp:Panel><!-- end pnLocSearch -->

<asp:Panel ID="pnEquipment" runat="server" Visible="false">

  <asp:GridView ID="gvEquipment" runat="server"
    AutoGenerateColumns="False"
    CssClass="tableWithLines width100"
    AllowPaging="True"
    PageSize="750" 
    AllowSorting="True" 
    onpageindexchanging="gvPageIndexChanging_Eqp" 
    onsorting="gvSorting_Eqp">
    <AlternatingRowStyle CssClass="trColorAlt" />
    <Columns>
        <asp:BoundField DataField="Customer" HeaderText="Name" SortExpression="Customer" />
        <asp:BoundField DataField="Cs1" HeaderText="Cust" SortExpression="Cs1" />
        <asp:BoundField DataField="Cs2" HeaderText="Loc" SortExpression="Cs2" />
        <asp:BoundField DataField="CustXrf" HeaderText="CustXRef" SortExpression="CustXrf" />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
        <asp:BoundField DataField="State" HeaderText="St" SortExpression="State" />
        <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />
        <asp:TemplateField HeaderText="Serial" SortExpression="Serial">
            <ItemTemplate>
                <asp:LinkButton ID="lkSerial" runat="server" OnClick="lkSerial_Click" 
                    CommandArgument='<%# Eval("Unit") %>'>
                    <%# Eval("Serial") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ModXrf" HeaderText="EquipXrf" SortExpression="ModXrf" />
    </Columns>
</asp:GridView>
</asp:Panel>



<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />

</asp:Content>




