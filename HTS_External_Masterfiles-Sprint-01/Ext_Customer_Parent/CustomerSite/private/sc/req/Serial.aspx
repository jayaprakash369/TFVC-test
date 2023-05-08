<%@ Page Title="Service Request: Serial Search" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Serial.aspx.cs" 
    Inherits="private_sc_req_Serial" %>
<%@ PreviousPageType VirtualPath="~/private/sc/req/Location.aspx" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Service Request
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<%-- Hidden Fields --%>             
<asp:HiddenField ID="hfPri" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfUnitList" runat="server" Value="" Visible="false" />

<%-- SERIAL: Global Serial Search  --%> 
<asp:Panel ID="pnSerial" runat="server" DefaultButton="btSerialSearch">
    
<table class="tableWithoutLines" style="width: 100%; padding: 3px;">
    <tr>
        <th>Enter a partial or full value to determine the unit's current location by </th>
         <td>  <asp:RadioButtonList ID="rdSel" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Value="Serial" Selected="True">serial</asp:ListItem> 
            <%-- 
                <asp:ListItem Value="CrossRef">     xref</asp:ListItem>
                --%> 
             
             <asp:ListItem Value="Asset">     asset xref</asp:ListItem>
            </asp:RadioButtonList>   
        </td>
        <td>
        <asp:TextBox ID="txSerialSearch" runat="server" Width="150" MaxLength="25" />
            <asp:RequiredFieldValidator ID="vReqSerialSearch" runat="server" 
                        ControlToValidate="txSerialSearch" 
                        ErrorMessage="A search entry is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="SerialSearch" />
            <asp:Button ID="btSerialSearch" runat="server" Text="Search" ValidationGroup="SerialSearch" onclick="btSerialSearch_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="text-align: center;">
            <asp:ValidationSummary ID="vSumSerialSearch" runat="server" ValidationGroup="SerialSearch" />
        </td>
    </tr>
</table>

<%-- MOVE: Panel  --%> 
<asp:Panel ID="pnMove" runat="server" Visible="false" DefaultButton="btMove">
<table class="tableWithoutLines" style="width: 100%; padding: 3px;">
    <tr style="vertical-align: top;">
        <th style="border: 1px solid #AAAAAA; text-align: left; width: 250px;">To continue with a service request<br/> click the model name below.
        <asp:Panel ID="pnPM" runat="server" Visible="false">
            <div style="clear: both; height: 20px;"></div>
                <span style="color: #ad0034; padding-left: 3px;">Request Type?</span>
            <asp:RadioButtonList ID="rblPmRequest" runat="server" RepeatDirection="Vertical" SkinID="">
                <asp:ListItem Text="Standard Request" Value="STD" Selected="True" />
                <asp:ListItem Text="PM (preventative maintenance)" Value="PM" />
            </asp:RadioButtonList>
        </asp:Panel>
        </th>
        <td style="border: 1px solid #AAAAAA; text-align: left;">
            <b><font color="#AD0034">OR</font> to have units reassigned to another location...</b> 
            <br /><br />
            <table class="tableVerticalList" style="width: 100%; padding: 3px;">
                <tr>
                    <th style="text-align: left;">1)&nbsp;&nbsp;Identify New Location</th>
                    <td>
                        <asp:DropDownList ID="ddLocMove" runat="server" CssClass="dropDownList1" />
                        <asp:RequiredFieldValidator ID="vReq_ddLocMove" runat="server" 
                        ControlToValidate="ddLocMove" 
                        ErrorMessage="The new equipment location must be selected." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Move" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: left;">2)&nbsp;&nbsp;Include name and phone</th>
                    <td>
                        <asp:TextBox ID="txNameMove" runat="server" Width="150" MaxLength="50" />
                        <asp:RequiredFieldValidator ID="vReq_txNameMove" runat="server" 
                        ControlToValidate="txNameMove" 
                        ErrorMessage="A contact name is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Move" />
                ( <asp:TextBox ID="txPhone1Move" runat="server" Width="30" MaxLength="3" />
                    <asp:RequiredFieldValidator ID="vReq_Phone1Move" runat="server" 
                        ControlToValidate="txPhone1Move" 
                        ErrorMessage="The area code is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Move" />
                    <asp:CompareValidator id="CompareValidator1" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone1Move"
                        ErrorMessage="The area code must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Move" />
                    )&nbsp; <asp:TextBox ID="txPhone2Move" runat="server" Width="30" MaxLength="3" /> 
                    <asp:RequiredFieldValidator ID="vReq_Phone2Move" runat="server" 
                        ControlToValidate="txPhone2Move" 
                        ErrorMessage="The phone prefix is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Move" />
                    <asp:CompareValidator id="vCom_Phone2Move" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone2Move"
                        ErrorMessage="The phone prefix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Move" />
                    - <asp:TextBox ID="txPhone3Move" runat="server" Width="35" MaxLength="4" /> 
                    <asp:RequiredFieldValidator ID="vReq_txPhone3Move" runat="server" 
                        ControlToValidate="txPhone3Move" 
                        ErrorMessage="The phone suffix is required" 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Move" />
                    <asp:CompareValidator id="vCom_txPhone3Move" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone3Move"
                        ErrorMessage="The phone suffix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Move" />
                    <asp:RangeValidator ID="vRng_txPhone3Move" runat="server" 
                        ControlToValidate="txPhone3Move"
                        ErrorMessage="The phone suffix must be 9999 or less"
                        Text="*" 
                        MinimumValue="0" 
                        MaximumValue="9999"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Move" />
                    &nbsp; Ext: <asp:TextBox ID="txExtMove" runat="server" Width="35" MaxLength="8" />
                    <asp:CompareValidator id="vCom_txExtMove" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txExtMove"
                        ErrorMessage="The phone extension must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Move" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2"  style="text-align: left;">3)&nbsp;&nbsp;Click checkboxes of the units to move</th>
                </tr>
                <tr>
                    <th colspan="2" style="text-align: left;">4)&nbsp;&nbsp; 
                        <asp:Button ID="btMove" runat="server" 
                            Text="MoveUnits" 
                            ValidationGroup="Move" 
                            onclick="btMove_Click" />
                    </th>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                    <asp:CustomValidator id="vCus_Move" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="Move" />
                        <asp:ValidationSummary ID="vSum_Move" runat="server" ValidationGroup="Move" />
                        <asp:Label ID="lbMove" runat="server" Visible="false" SkinID="labelError" />
                    </td>

                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>

    <asp:GridView ID="gvSerial" runat="server" style="width: 100%"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines" >
         <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:TemplateField HeaderText="Model">
                <ItemTemplate>
                    <asp:LinkButton ID="lkModel" runat="server" OnClick="lkSerialPick_Click" 
                        CommandArgument='<%# Eval("Cs1") + "|" + Eval("Cs2") + "|" + Eval("Unit") + "|" + Eval("Agreement")%>'>
                        <%# Eval("Model") %>
                    </asp:LinkButton></ItemTemplate></asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Description" />
             <asp:BoundField DataField="Serial" HeaderText="Serial" />
            <asp:BoundField DataField="Asset" HeaderText="Asset xRef" />
            <asp:TemplateField HeaderText="Move">
                <ItemTemplate>
                    <center>
                    <asp:CheckBox ID="chBxMove" runat="server" 
                        Text='<%# Eval("Cs1") + "~" + Eval("Cs2") + "~" + Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Model") + "~" + Eval("Serial") %>' 
                        SkinID="checkBoxHidingText" />
                    </center>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CustName" HeaderText="Customer Name" />
            <asp:BoundField DataField="Cs1" HeaderText="Cust Num" />
            <asp:BoundField DataField="Cs2" HeaderText="Location" />
            <asp:BoundField DataField="City" HeaderText="City" />
            <asp:BoundField DataField="State" HeaderText="State" />
        </Columns>
    </asp:GridView>

</asp:Panel>
</asp:Content>


