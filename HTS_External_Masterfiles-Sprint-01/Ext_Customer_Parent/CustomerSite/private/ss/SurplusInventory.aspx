<%@ Page Title="Surplus Inventory" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="SurplusInventory.aspx.cs" 
    Inherits="private_ss_SurplusInventory" %>
<asp:Content ID="Content3" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>Surplus Inventory</td>
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
    <asp:Label ID="lbError" runat="server" Text="" Visible="false" SkinID="labelError" />
    <asp:Panel ID="pnInput" runat="server" DefaultButton="btInput">
    <table style="width: 100%;" class="tableWithLines">
        <tr>
            <th>Stocking Location</th>
            <th>Display Report</th>
            <th>Download Report</th>
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


    <asp:Panel ID="pnSurplus" runat="server">
        <div style="height: 5px;"></div>
        
        <table style="width: 100%;">
            <tr>
                <td align="left">
                    <asp:Label ID="lbEmail" runat="server" 
                        SkinId="labelTitleColor1_Small"
                        Text="To email regarding parts in your list, click their checkboxes." />
                </td>
                <td align="right" valign="bottom">
                    <asp:Label ID="lbSurplus" runat="server" 
                        SkinId="labelTitleColor1_Small"
                        Text="List shows surplus items worth $75 or more (PTR items $25 or more)" />                    
                        &nbsp;
                    <asp:Button ID="btSelections" runat="server" 
                        Text="Show Selections" 
                        onclick="btSelections_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvSurplus" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
        <asp:TemplateField HeaderText="Chk">
            <ItemTemplate>
                <asp:CheckBox ID="chBxEmail" runat="server"
                 Text='<%# Eval("Part") + "~" + Eval("Qty On Hand") %>' 
                  >
                </asp:CheckBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Product Code" HeaderText="Product Code" SortExpression="Product Code" />
        <asp:BoundField DataField="Part" HeaderText="Part" SortExpression="Part" />
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
        <asp:BoundField DataField="Qty On Hand" HeaderText="Qty On Hand" SortExpression="Qty On Hand" />
        <asp:BoundField DataField="Last Date" HeaderText="Last Date" SortExpression="Last Date" />
    </Columns>
</asp:GridView>
<asp:HiddenField ID="hfUnitList" runat="server" />
</asp:Panel>
    

<asp:Panel ID="pnEmail" runat="server" DefaultButton="btEmail" Visible="false">

    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" 
                    SkinId="labelSteps"
                    Text="Please enter specific information concerning each part" />
            </td>
            <td style="text-align: right;">
                <asp:Button ID="btEmail" runat="server" 
                    Text="Submit Email" 
                    ValidationGroup="Email"
                    OnClick="btEmail_Click" />
            </td>
        </tr>
    </table>

    <%-- Surplus Email Repeater --%>
    <asp:Repeater ID="rpEmail" runat="server">
        <HeaderTemplate>
            <table class="tableWithoutLines" style="width: 100%;">
                <tr>
                    <th align="left">Part</th>
                    <th align="left">Qty</th>
                    <th align="left">
                        <b>Suggestions for helpful comments:</b>
                            <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If keeping the part for a customer, list their account number.
                            <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If parts are sent back, include date and tracking number. 
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td align="left"><asp:Label ID="lbPart" runat="server" Text='<%# Eval("Part") %>' /></td>
                    <td align="left"><asp:Label ID="lbQty" runat="server" Text='<%# Eval("Qty") %>' /></td>
                    <td align="left">
                        <asp:TextBox ID="txComment" runat="server" 
                            TextMode="MultiLine"
                            Height="60" 
                            Width="600" 
                            MaxLength="500" />
                            <asp:RequiredFieldValidator ID="vReq_Comment" runat="server" 
                                ControlToValidate="txComment" 
                                ErrorMessage="A comment is required." 
                                Text="*"
                                ValidationGroup="Email" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

    <table style="width: 100%; margin-top: 20px;">
        <tr>
            <td style="vertical-align: top;">
                <asp:Label ID="Label2" runat="server" 
                    SkinId="labelTitleColor1_Small"
                    Text="If a response is desired, enter your email address" />
            </td>
            <td style="text-align: right;">
                <asp:TextBox ID="txEmailReply" runat="server" Width="600" MaxLength="50" />
                <asp:RegularExpressionValidator id="vReg_Email" runat="server"
                        ControlToValidate="txEmailReply"
                        ValidationExpression="^\S+@\S+\.\S+$"
                        ErrorMessage="The email address is not in a valid format" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Email">
                    </asp:RegularExpressionValidator>
                    
            </td>
        </tr>
    </table>
    <center><asp:ValidationSummary ID="vSum_Email" runat="server" ValidationGroup="Email" /></center>
</asp:Panel>
</center>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
   
</asp:Content>


