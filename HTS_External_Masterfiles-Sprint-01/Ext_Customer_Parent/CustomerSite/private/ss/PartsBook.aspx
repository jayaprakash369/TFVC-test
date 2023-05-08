<%@ Page Title="Parts Book" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="PartsBook.aspx.cs" 
    Inherits="private_ss_PartsBook" %>
<asp:Content ID="Content3" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Parts Book</td>
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
        function clearEqpInput() {
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txPrt.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txDsc.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSty.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txMfr.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_ddPrd.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_ddDrv.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_ddPc.value = "";
            return true;
        }

    </script>

<center>
    <asp:Panel ID="pnInput" runat="server" DefaultButton="btInput">
    <table style="width: 100%;" class="tableWithLines">
        <tr>
            <th>Part</th>
            <th>Part Desc</th>
            <th>Mfr Name</th>
            <th>Style Name</th>
            <th>Product Code</th>
            <th>Drive Type</th>
            <th>PC Type</th>
            <th>&nbsp;</th>
        </tr>
        <tr>
            <td style="text-align: center;"><asp:TextBox ID="txPrt" runat="server" Width="75" MaxLength="15" /></td>
            <td style="text-align: center;"><asp:TextBox ID="txDsc" runat="server" Width="75" MaxLength="35"  /></td>
            <td style="text-align: center;"><asp:TextBox ID="txMfr" runat="server" Width="75"  MaxLength="15" /></td>
            <td style="text-align: center;"><asp:TextBox ID="txSty" runat="server" Width="75"  MaxLength="15" /></td>
            <td style="text-align: center;"><asp:DropDownList ID="ddPrd" runat="server" CssClass="dropDownList1" /></td>
            <td style="text-align: center;"><asp:DropDownList ID="ddDrv" runat="server" CssClass="dropDownList1" /></td>
            <td style="text-align: center;"><asp:DropDownList ID="ddPc" runat="server" CssClass="dropDownList1" /></td>
            <td style="text-align: center;">
                <asp:Button ID="btInput" runat="server" Text="Continue" onclick="btInput_Click" ValidationGroup="Parts" />
                &nbsp;&nbsp;
                <input id="btClearEqp" type="button" 
                    value="Clear" 
                    class="buttonDefault" 
                    onclick="javascript: clearEqpInput();" />
            </td>
        </tr>
    </table>
    <asp:CustomValidator id="vCus_Parts" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="Parts" />
        <asp:ValidationSummary ID="vSum_Parts" runat="server" ValidationGroup="Parts" />
    </asp:Panel>

    <asp:Label ID="lbError" runat="server" SkinID="labelError" visible="false" />

    <asp:Panel ID="pnEquipment" runat="server">
        <div style="height: 20px;"></div>
        <asp:Label ID="lbEquipment" runat="server" SkinID="labelTitleColor1_Medium" />
      <asp:GridView ID="gvEquipment" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines"
         AllowPaging="true"
         PageSize="500" 
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Eqp" 
         onsorting="gvSorting_Eqp">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField DataField="Part" HeaderText="Part" SortExpression="Part" />
            <asp:BoundField DataField="PartDesc" HeaderText="Part Desc" SortExpression="PartDesc" />
            <asp:BoundField DataField="MfrName" HeaderText="Mfr Name" SortExpression="MfrName" />
            <asp:BoundField DataField="Return" HeaderText="Returnable?" SortExpression="Return" />
        </Columns>
    </asp:GridView>
    </asp:Panel>
</center>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
   
</asp:Content>

