<%@ Page Title="Enter Parts Used" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="PartsUsed.aspx.cs" 
    Inherits="private_ss_PartsUsed" %>
<asp:Content ID="Content3" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
        <table style="width: 100%;">
        <tr>
            <td>Enter Parts Used</td>
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
<asp:HiddenField ID="hfPickUnit" runat="server" Visible="false" />
<asp:HiddenField ID="hfEmpNam" runat="server" Visible="false" />
<asp:HiddenField ID="hfEmpLoc" runat="server" Visible="false" />
<asp:HiddenField ID="hfCtr" runat="server" Visible="false" />
<asp:HiddenField ID="hfTck" runat="server" Visible="false" />
<center>
    <%-- PANEL PICK  ==================================================================================  --%>             
    <asp:Panel ID="pnPick" runat="server">
        <asp:Table ID="tbPartsUsed" runat="server" SkinID="tableWithLines">
            <asp:TableHeaderRow BorderColor="#333333" BorderWidth="1">
                <asp:TableHeaderCell Width="150" Text="Current Ticket"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150"><asp:Label ID="lbPickEmpHead" runat="server" Text="Technician" Visible="false" /></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150"><asp:Label ID="lbPickModHead" runat="server" Text="Broken Unit Model" Visible="false" /></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150"><asp:Label ID="lbPickSerHead" runat="server" Text="Broken Unit Serial" Visible="false" /></asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell><center><asp:Label ID="lbCtrTck" runat="server" /></center></asp:TableCell>
                <asp:TableCell><center><asp:Label ID="lbPickEmp" runat="server" Visible="false" /></center></asp:TableCell>
                <asp:TableCell><center><asp:Label ID="lbPickMod" runat="server" Visible="false" /></center></asp:TableCell>
                <asp:TableCell><center><asp:Label ID="lbPickSer" runat="server" Visible="false" /></center></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>

    <%-- PANEL PARTS PROCESSED  ===========================================================================  --%>             
    <asp:Panel ID="pnPartsProcessed" runat="server">
        <div style="height: 15px; clear: both;"></div>
        <asp:GridView ID="gvPartsProcessed" runat="server"
            Visible="false"
            AutoGenerateColumns="False"
            CssClass="tableWithLines">
            <HeaderStyle BorderColor="#333333" BorderWidth="1" />
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:BoundField DataField="Part" HeaderText="Current Part Use" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Emp" HeaderText="Tech" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Loc" HeaderText="Loc" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Returnable" HeaderText="Rtnable" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="SendingBack" HeaderText="Returning?" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Transfer" HeaderText="Transfer" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="Sequence" HeaderText="Seq" HeaderStyle-BackColor="#3a7728" />
                <asp:BoundField DataField="ReasonNotReturned" HeaderText="Reason Returnable Not Returned?" HeaderStyle-BackColor="#3a7728" />
            </Columns>
        </asp:GridView>
        <div style="height: 15px; clear: both;"></div>
    </asp:Panel>


    <%-- PANEL EMPLOYEE IDENTIFICATION  ==================================================================================  --%>             
    <asp:Panel ID="pnEmp" runat="server" DefaultButton="btEmp">
        <div style="height: 15px; clear: both;"></div>
        <asp:Label ID="lbEmp" runat="server" Text="1) Select a technician" SkinID="labelTitleColor1_Medium" />
        <table style="width: 100%; border: 1px solid #aaaaaa; padding: 5px;">
            <tr>
                <td>
                    <asp:DropDownList ID="ddTechNum" runat="server" CssClass="dropDownList1" 
                        OnSelectedIndexChanged="ddTechNum_Changed" 
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btEmp" runat="server" Text="Continue" OnClick="btEmp_Click" />
                </td>
            </tr>
        </table>

    </asp:Panel>
    <%-- PANEL CONTRACT UNIT SEARCH ==================================================================================  --%>             
    <asp:Panel ID="pnContractSearch" runat="server" Visible="false" DefaultButton="btContractSearch">
    <div style="height: 15px; clear: both;"></div>
    <asp:Label ID="lbContractSearch" runat="server" 
        Text="2) Search for Broken Unit Recieving Part Use" 
        SkinID="labelTitleColor1_Small" />
    <table class="tableWithoutLines" style="width: 100%; border: 1px solid #aaaaaa;">
        <tr style="">
            <td style="text-align: center;">
                Model like &nbsp; <asp:TextBox ID="txMod" runat="server" Width="100" MaxLength="15" />
            </td>
            <td style="text-align: center;">
                Description like &nbsp; <asp:TextBox ID="txDsc" runat="server" Width="150" MaxLength="35" />
            </td>
            <td style="text-align: center;">
                Serial like &nbsp; <asp:TextBox ID="txSer" runat="server" Width="150" MaxLength="25" />
            </td>
            <td style="text-align: center;">
                <asp:Button ID="btContractSearch" runat="server" Text="Search" onclick="btContractSearch_Click" /> 
            </td>
        </tr>

    </table>
    </asp:Panel>

    <%-- PANEL CONTRACT UNITS ==================================================================================  --%>             
    <asp:Panel ID="pnContractUnits" runat="server">
        <div style="height: 15px;"></div>
        <asp:GridView ID="gvContractUnits" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines">
         <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:BoundField DataField="Part" HeaderText="Model" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:TemplateField HeaderText="Serial">
                <ItemTemplate>
                    <asp:LinkButton ID="lkSerial" runat="server" OnClick="lkSerial_Click" 
                        CommandArgument='<%# Eval("Part") + "|" + Eval("Serial") + "|" + Eval("Unit") %>'>
                        <%# Eval("Serial") %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    </asp:Panel>



    <%-- PANEL REPLACEMENT PARTS ==================================================================================  --%>             
    <asp:Panel ID="pnReplacementParts" runat="server" Visible="false">
        <div style="height: 25px; clear: both;"></div>
        <asp:Label ID="lbPartsList" runat="server" Text="3) Search for Parts Installed on Broken Unit" SkinID="labelTitleColor1_Small" />
        <table style="width: 100%; border: 1px solid #bbbbbb; padding: 5px;" class="tableWithoutLines">
            <tr>
                <td>
                    <asp:RadioButton ID="rbLocEmp" runat="server" 
                        Text="Show my loc" 
                        GroupName="PartSource" />
                </td>
                <td>
                    <asp:RadioButton ID="rbLocCs1" runat="server" 
                        Checked="true"
                        Text="Show loc" 
                        GroupName="PartSource" />
                    &nbsp;
                    <asp:DropDownList ID="ddStockLoc" runat="server" 
                        CssClass="dropDownList1" 
                        OnSelectedIndexChanged="ddStockLoc_Changed" 
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RadioButton ID="rbLocPrt" runat="server" 
                        Text="Show parts like" 
                        GroupName="PartSource" />
                        &nbsp;
                    <asp:TextBox ID="txStockPart" runat="server" />
                    <asp:CustomValidator id="vCus_Part" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="StockSearch" />
                </td>
                <td>
                    <asp:RadioButton ID="rbLetMeType" runat="server" 
                        Text="Let me type" 
                        GroupName="PartSource" />
                </td>
                <td>
                    <asp:Button ID="btStockSearch" runat="server" 
                        Text="Display Part Source" 
                        ValidationGroup="StockSearch"
                        OnClick="btStockSearch_Click" /> 
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vSum_StockSearch" runat="server" ValidationGroup="StockSearch" Visible="false" />

        <div style="height: 10px;"></div>
        <asp:GridView ID="gvPartsList" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="Add">
                    <ItemTemplate>
                        <center>
                            <asp:Button ID="btAddPart" runat="server" 
                                CommandArgument='<%# Eval("Part") + "|" + Eval("StockLoc") + "|" + Eval("QtyOnHand")+ "|" + Eval("Returnable") %>' 
                                OnClick="btAddPart_Click" />
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="Part" HeaderText="Model" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="StockLoc" HeaderText="Stock Loc" />
                <asp:BoundField DataField="QtyOnHand" HeaderText="Qty on Hand" />
                <asp:BoundField DataField="Returnable" HeaderText="Returnable?" />
            </Columns>
        </asp:GridView>

    </asp:Panel>

    <%-- PANEL PART USE ENTRY ==================================================================================  --%>             
    <asp:Panel ID="pnPartUseEntry" runat="server" Visible="false">
        <div style="height: 15px;"></div>
        <asp:Label ID="lbPartUseEntry" runat="server" Text="4) Enter Part Use" SkinID="labelTitleColor1_Medium" />
        <asp:Table ID="tbPartUseEntry" runat="server" SkinID="tableWithLines">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Text="Part" />
                <asp:TableHeaderCell Text="Stock Loc" />
                <asp:TableHeaderCell Text="Qty Used" />
                <asp:TableHeaderCell Text="Returnable?" />
                <asp:TableHeaderCell Text="Returning" />
                <asp:TableHeaderCell Text="Reason Part Will Not Be Returned" />
                <asp:TableHeaderCell Text="" />
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell Width="160">
                    <center>
                        <asp:TextBox ID="txUsedPart" runat="server" Visible="false" />
                        <asp:Label ID="lbUsedPart" runat="server" Visible="false" />
                    </center>
                </asp:TableCell>
                <asp:TableCell Width="70">
                    <center>
                        <asp:Label ID="lbUsedLoc" runat="server" Visible="false" />
                        <asp:TextBox ID="txUsedLoc" runat="server" Width="50" />
                            <asp:RequiredFieldValidator ID="Cs1Req" runat="server" 
                                ControlToValidate="txUsedLoc" 
                                ErrorMessage="A stocking location is required." 
                                Text="*"
                                Display="Dynamic"
                                ValidationGroup="PartUseEntry" />
                            <asp:CompareValidator id="vCom_LocNumeric" runat="server" 
                                Operator="DataTypeCheck"
                                Type="Integer"
                                ControlToValidate="txUsedLoc"
                                ErrorMessage="Stocking location must be an integer" 
                                Text="*"
                                SetFocusOnError="true"
                                Display="Dynamic"
                                ValidationGroup="PartUseEntry" />
                    </center>
                </asp:TableCell>
                <asp:TableCell Width="80">
                    <center>
                        <asp:DropDownList ID="ddUsedQty" runat="server" CssClass="dropDownList1">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfLocQty" runat="server" Visible="false" />
                    </center>
                </asp:TableCell>
                <asp:TableCell Width="80">
                    <center>
                        <asp:Label ID="lbUsedReturnable" runat="server" />
                    </center>
                </asp:TableCell>
                <asp:TableCell>
                    <center>
                        <asp:DropDownList ID="ddUsedReturning" runat="server" 
                            ValidationGroup="PartUseEntry"
                            CssClass="dropDownList1">
                            <asp:ListItem Text="" Value="" />
                            <asp:ListItem Text="YES" Value="YES" />
                            <asp:ListItem Text="NO" Value="NO" />
                        </asp:DropDownList>
                    </center>
                </asp:TableCell>
                <asp:TableCell>
                    <center>
                        <asp:DropDownList ID="ddUsedReasonNotReturning" runat="server" 
                            ValidationGroup="PartUseEntry"
                            CssClass="dropDownList1">
                            <asp:ListItem Text="" Value="" />
                            <asp:ListItem Text="Item was sold" Value="Item was sold" />
                            <asp:ListItem Text="Replaced onboard component" Value="Replaced onboard component" />
                            <asp:ListItem Text="Insurance company needed" Value="Insurance company needed" />
                            <asp:ListItem Text="Customer requested to keep" Value="Customer requested to keep" />
                            <asp:ListItem Text="Damaged beyond repair" Value="Damaged beyond repair" />
                            <asp:ListItem Text="Part returned to vendor" Value="Part returned to vendor" />
                            <asp:ListItem Text="Other..." Value="Other" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txUsedTextNotReturning" runat="server" 
                            Width="150" 
                            MaxLength="30" 
                            ValidationGroup="PartUseEntry" />
                    </center>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Center">
                    <div style="float: right; padding-right: 10px;">
                        <asp:Button ID="btUsedSubmission" runat="server" 
                            Text="Submit" 
                            ValidationGroup="PartUseEntry"
                            OnClick="btUsedSubmission_Click" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:CustomValidator ID="vCus_PartUseEntry" runat="server" 
            Display="None" 
            EnableClientScript="False"
            ValidationGroup="PartUseEntry" />
        <center><asp:ValidationSummary ID="vSum_PartUseEntry" runat="server" ValidationGroup="PartUseEntry" /></center>

    </asp:Panel>

</center>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="For_Body_C" Runat="Server">
   
</asp:Content>

