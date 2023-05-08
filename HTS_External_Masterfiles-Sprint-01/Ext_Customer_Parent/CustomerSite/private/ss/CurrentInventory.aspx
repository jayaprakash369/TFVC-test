<%@ Page Title="Current Inventory" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="CurrentInventory.aspx.cs" 
    Inherits="private_ss_CurrentInventory" %>
<asp:Content ID="Content3" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Current Inventory</td>
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
    <asp:Panel ID="pnInput" runat="server" DefaultButton="btInput">
    <table style="width: 100%;" class="tableWithLines">
        <tr>
            <th>
                Stocking Location
            </th>
            <th>
                Display Report
            </th>
            <th>
                Download Report
            </th>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:DropDownList ID="ddStockLoc" runat="server" CssClass="dropDownList1" 
                    OnSelectedIndexChanged="ddStockLoc_Changed" 
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td style="text-align: center;">
                <asp:Button ID="btInput" runat="server" Text="Continue" onclick="btInput_Click" /> 
            </td>
            <td style="text-align: center;">
                <asp:Button ID="btDownload" runat="server" Text="Download" onclick="btDownload_Click" /> 
            </td>
        </tr>

    </table>
    <asp:DropDownList ID="ddTechNum" runat="server" CssClass="dropDownList1" 
        Visible="false" 
        OnSelectedIndexChanged="ddTechNum_Changed" 
        AutoPostBack="true">
    </asp:DropDownList>
    </asp:Panel>

    <asp:Panel ID="pnGoodBucket" runat="server">
        <div style="height: 30px;"></div>
        <asp:GridView ID="gvGoodBucket" runat="server" CssClass="tableWithLines">
        </asp:GridView>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
   
</asp:Content>
