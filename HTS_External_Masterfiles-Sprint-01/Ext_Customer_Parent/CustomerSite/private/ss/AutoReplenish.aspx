<%@ Page Title="Auto Replenishment" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="AutoReplenish.aspx.cs" 
    Inherits="private_ss_AutoReplenish" %>
<asp:Content ID="Content3" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Auto Replenishment</td>
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
<center>
<%-- PANEL INPUT ==================================================================================  --%>             
    <asp:Panel ID="pnInput" runat="server" DefaultButton="btInput">
    <asp:Label ID="lbError" runat="server" Visible="false" SkinID="labelError" />
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

<%-- PANEL AUTO REPLENISH LIST ==================================================================================  --%>             

    <asp:Panel ID="pnAutoReplenish" runat="server">
        <div style="height: 30px; clear: both;"></div>
        <table style="width: 100%;" class="tableWithoutLines">
            <tr>
                <td>
                    1) To update/delete lines: check their boxes
                </td>
                <td>
                    2) To add lines, select the number to add &nbsp;&nbsp;
                    <asp:DropDownList ID="ddNewLines" runat="server" CssClass="dropDownList1" />
                </td>
                <td>
                    <asp:Button ID="btUpdateList" runat="server" Text="Show Selections" onclick="btUpdateList_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvAutoReplenish" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines">
         <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:CheckBox ID="chBxModel" runat="server" Text='<%# Eval("Product Code") + "|" + Eval("Model") + "|" + Eval("Description") + "|" + Eval("Ideal Qty") + "|" + Eval("Reorder Point")  %>' 
                        />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Product Code" HeaderText="Product Code" />
            <asp:BoundField DataField="Model" HeaderText="Model" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Ideal Qty" HeaderText="Ideal Qty" />
            <asp:BoundField DataField="Reorder Point" HeaderText="Reorder Point" />
        </Columns>
    </asp:GridView>
    </asp:Panel>

<%-- PANEL UPDATE LIST ==================================================================================  --%>             

    <asp:Panel ID="pnUpdateList" runat="server" Visible="false" DefaultButton="btEmail">

        <table style="width: 100%;">
            <tr style="vertical-align: top; text-align: left;">
                <td>Email (if fulfillment response desired)
                </td>
                <td>Comments?</td>
                <td>&nbsp;</td>
            </tr>
            <tr style="vertical-align: top; text-align: left;">
                <td><asp:TextBox ID="txEmail" runat="server" Width="250" MaxLength="50" /></td>
                <td><asp:TextBox ID="txComment" runat="server" 
                    TextMode="MultiLine" 
                    Width="450" 
                    Height="50" 
                    MaxLength="2000" />
                    <asp:RequiredFieldValidator ID="vReqComment" runat="server" 
                        ErrorMessage="A comment is required"
                        Text="*"
                        ControlToValidate="txComment"
                        SetFocusOnError="true"
                        ValidationGroup="PartsEmail" />
                </td>
                <td>
                    <asp:Button ID="btEmail" runat="server" 
                        Text="Submit Request" 
                        onclick="btEmail_Click" 
                        ValidationGroup="PartsEmail" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnPartsNew" runat="server" Visible="false">
            <asp:Label ID="lbPartsNew" runat="server" SkinID="labelTitleColor1_Medium" />
        <asp:GridView ID="gvPartsNew" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines">
         <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:TemplateField HeaderText="Product Code">
                <ItemTemplate>
                    <center>
                        <asp:DropDownList ID="ddPrdNew" runat="server" CssClass="dropDownList1">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="CABLES" Value="CABLES"></asp:ListItem>
                            <asp:ListItem Text="DRIVE" Value="DRIVE"></asp:ListItem>
                            <asp:ListItem Text="IBM" Value="IBM"></asp:ListItem>
                            <asp:ListItem Text="PC" Value="PC"></asp:ListItem>
                            <asp:ListItem Text="PTR" Value="PTR"></asp:ListItem>
                            <asp:ListItem Text="TERM" Value="TERM"></asp:ListItem>
                            <asp:ListItem Text="UNIX" Value="UNIX"></asp:ListItem>
                            <asp:ListItem Text="UPS" Value="UPS"></asp:ListItem>
                        </asp:DropDownList>
                    </center>
                </ItemTemplate>
                
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Model">
                <ItemTemplate>
                    <center>
                        <asp:TextBox ID="txModNew" runat="server" Width="150" MaxLength="30" />
                        <asp:RequiredFieldValidator ID="vReqModNew" runat="server" 
                            ErrorMessage="A model is required"
                            Text="*"
                            ControlToValidate="txModNew"
                            SetFocusOnError="true"
                            ValidationGroup="PartsEmail" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <center>
                        <asp:TextBox ID="txDscNew" runat="server" Width="250" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="vReqDscNew" runat="server" 
                            ErrorMessage="A description is required" 
                            Text="*"
                            ControlToValidate="txDscNew"
                            SetFocusOnError="true" 
                            ValidationGroup="PartsEmail" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reorder Point">
                <ItemTemplate>
                    <center>
                    <asp:DropDownList ID="ddReorderPointNew" runat="server"
                     CssClass="dropDownList1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                        <asp:ListItem Text="13" Value="13" />
                        <asp:ListItem Text="14" Value="14" />
                        <asp:ListItem Text="15" Value="15" />
                        <asp:ListItem Text="16" Value="16" />
                        <asp:ListItem Text="17" Value="17" />
                        <asp:ListItem Text="18" Value="18" />
                        <asp:ListItem Text="19" Value="19" />
                        <asp:ListItem Text="20" Value="20" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vReqReorderPointNew" runat="server" 
                            ErrorMessage="A reorder point is required" 
                            Text="*"
                            ControlToValidate="ddReorderPointNew"
                            SetFocusOnError="true" 
                            ValidationGroup="PartsEmail" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reorder Qty">
                <ItemTemplate>
                    <center>
                    <asp:DropDownList ID="ddReorderQtyNew" runat="server"
                     CssClass="dropDownList1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                        <asp:ListItem Text="13" Value="13" />
                        <asp:ListItem Text="14" Value="14" />
                        <asp:ListItem Text="15" Value="15" />
                        <asp:ListItem Text="16" Value="16" />
                        <asp:ListItem Text="17" Value="17" />
                        <asp:ListItem Text="18" Value="18" />
                        <asp:ListItem Text="19" Value="19" />
                        <asp:ListItem Text="20" Value="20" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vReqReorderQtyNew" runat="server" 
                            ErrorMessage="A reorder quantity is required" 
                            Text="*"
                            ControlToValidate="ddReorderQtyNew"
                            SetFocusOnError="true" 
                            ValidationGroup="PartsEmail" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
            <asp:ValidationSummary ID="vSumPartsEmail" runat="server" 
                ValidationGroup="PartsEmail" />

    <div style="height: 10px; clear: both;"></div>
    </asp:Panel>
   
    <asp:Panel ID="pnPartsOld" runat="server" Visible="false">
    <asp:Label ID="lbPartsOld" runat="server" SkinID="labelTitleColor1_Medium" />
    <asp:GridView ID="gvPartsOld" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines">
         <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:TemplateField HeaderText="Product Code">
                <ItemTemplate>
                    <asp:Label ID="lbPrdOld" runat="server" Text='<%# Eval("Product Code") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Model">
                <ItemTemplate>
                    <asp:Label ID="lbModOld" runat="server" Text='<%# Eval("Model") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:Label ID="lbDscOld" runat="server" Text='<%# Eval("Description") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reorder Point">
                <ItemTemplate>
                    <center>
                    <asp:HiddenField ID="hfReorderPoint" runat="server" Value='<%# Eval("Reorder Point") %>' />
                    <asp:HiddenField ID="hfReorderQty" runat="server" Value='<%# Eval("Reorder Qty") %>' />
                    <span style="font-size: 11px;">Currently <%# Eval("Reorder Point") %></span>&nbsp; 
                    <asp:DropDownList ID="ddReorderPointOld" runat="server"
                     CssClass="dropDownList1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="0" Value="0" />
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                        <asp:ListItem Text="13" Value="13" />
                        <asp:ListItem Text="14" Value="14" />
                        <asp:ListItem Text="15" Value="15" />
                        <asp:ListItem Text="16" Value="16" />
                        <asp:ListItem Text="17" Value="17" />
                        <asp:ListItem Text="18" Value="18" />
                        <asp:ListItem Text="19" Value="19" />
                        <asp:ListItem Text="20" Value="20" />
                    </asp:DropDownList>
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reorder Qty">
                <ItemTemplate>
                    <center>
                    <span style="font-size: 11px;">Currently <%# Eval("Reorder Qty") %></span>&nbsp; 
                    <asp:DropDownList ID="ddReorderQtyOld" runat="server"
                     CssClass="dropDownList1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="0" Value="0" />
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                        <asp:ListItem Text="13" Value="13" />
                        <asp:ListItem Text="14" Value="14" />
                        <asp:ListItem Text="15" Value="15" />
                        <asp:ListItem Text="16" Value="16" />
                        <asp:ListItem Text="17" Value="17" />
                        <asp:ListItem Text="18" Value="18" />
                        <asp:ListItem Text="19" Value="19" />
                        <asp:ListItem Text="20" Value="20" />
                    </asp:DropDownList>
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete?">
                <ItemTemplate>
                    <center>
                        <asp:CheckBox ID="chBxDelete" runat="server" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </asp:Panel>
    </asp:Panel>
    </center>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
   
</asp:Content>

