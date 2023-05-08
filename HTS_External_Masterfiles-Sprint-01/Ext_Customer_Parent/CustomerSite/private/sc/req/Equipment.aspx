<%@ Page Title="Service Request: Equipment Selection" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Equipment.aspx.cs" 
    Inherits="private_sc_req_Equipment" %>
<%@ PreviousPageType VirtualPath="~/private/sc/req/Contact.aspx" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Service Request
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <script type="text/javascript">
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

<%-- Hidden Fields --%>             
<asp:HiddenField ID="hfPri" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfPhone" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfExtension" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfContact" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfEmail" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCreator" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfReqType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfForcedQty" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfUnitList" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodInfo" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodPhoneExt" runat="server" Value="" Visible="false" />
<asp:Panel ID="pnCs1Header" runat="server" />

<%-- EQUIPMENT LIST  --%>
<asp:Panel ID="pnEquipment" runat="server" DefaultButton="btEquipment">
    <table style="width: 100%; text-align: left; padding-bottom: 10px; margin: 0; padding: 0;">
        <tr>
            <td>
                <table class="tableWithoutLines" style="margin: 0; width: 100%">
                    <tr valign="baseline">
                        <td colspan="3" style="text-align: left;">
                            <asp:Label ID="lbCareful" runat="server" 
                                Text="Please do your best to identify the correct unit.<br /> An incorrectly selected piece of equipment can cause a delay in addressing the service issue." 
                                SkinID="labelComment" 
                                Font-Bold="false">
                            </asp:Label>
                        </td></tr><tr valign="baseline">
                        <td><asp:Label ID="lbStep7" runat="server" Text="Step 7" SkinID="labelSteps"></asp:Label></td><td><asp:Label ID="laEquipment" runat="server" Text="Select units for service (if needed, refine your search using the boxes below)" SkinID="labelTitleColor1_Small" /></td>
                        <td align="right">
                            <asp:Button ID="btEquipment" runat="server" 
                                Text="Continue" 
                                onclick="btEquipment_Click"
                                ValidationGroup="Equipment" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="border: 1px solid #CCCCCC; padding: 0px;">
                            <table border="1" style="width: 100%; padding: 0px; background-color: #EEEEEE;">
                                <tr>
                                    <td>
                                        Category<br /><asp:DropDownList ID="ddPrd" runat="server" CssClass="dropDownList1" />
                                    </td>
                                    <td>
                                        Model<br /> <asp:TextBox ID="txMod" runat="server" MaxLength="15" Width="110" />
                                    </td>
                                    <td>
                                        Model Description <br /><asp:TextBox ID="txDsc" runat="server" MaxLength="35" Width="110" />
                                    </td>
                                    <td>
                                        Serial <br /><asp:TextBox ID="txSer" runat="server" MaxLength="25" Width="120" />
                                    </td>
                                    <td>
                                        Equip XRef <br /><asp:TextBox ID="txFxa" runat="server" MaxLength="15" Width="110" />
                                    </td>
                                    <td>
                                        AgentId<br /><asp:TextBox ID="txAgn" runat="server" MaxLength="25" Width="150" />
                                    </td>
                                    <td>
                                        <br />
                                        <input id="btEqpClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearEqpInput();" />
                                    </td>
                                    <td>
                                        <br />
                                        <asp:Button ID="btEqpSearch" runat="server" Text="Search" OnClick="btEqpSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:CustomValidator id="vCustom_Equipment" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="Equipment" 
        />
    <asp:ValidationSummary ID="vSummary_Equipment" runat="server" 
        ValidationGroup="Equipment" />

    <asp:GridView ID="gvEquipment" runat="server" style="width: 100%"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines"
         AllowPaging="true"
         PageSize="500" 
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Eqp" 
         onsorting="gvSorting_Eqp"
         EmptyDataText="No matching equipment was found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="" SortExpression="Unit">
                <ItemTemplate>
                    <asp:CheckBox ID="chBxEqp" runat="server" Text='<%# Eval("Unit") + "~" + Eval("Agreement") %>' 
                        SkinID="checkBoxHidingText" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Part" HeaderText="Model" SortExpression="Part" />
            <asp:BoundField DataField="PartDesc" HeaderText="Model Description" SortExpression="PartDesc" />
            <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
            <asp:BoundField DataField="Asset" HeaderText="Equip XRef" SortExpression="Asset" />
            <asp:BoundField DataField="Unit" HeaderText="Unit ID" SortExpression="Unit" />
            <asp:BoundField DataField="AgrDesc" HeaderText="Contract Type" SortExpression="AgrDesc" />
            <asp:BoundField DataField="AgentId" HeaderText="AgentId" SortExpression="AgentId" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btEquipment2" runat="server" 
        Text="Continue" 
        onclick="btEquipment_Click" />
</asp:Panel>

</asp:Content>


