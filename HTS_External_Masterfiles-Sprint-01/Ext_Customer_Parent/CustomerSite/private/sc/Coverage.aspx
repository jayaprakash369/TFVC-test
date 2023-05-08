<%@ Page Title="" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Coverage.aspx.cs" 
    Inherits="private_sc_Coverage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Maintenance Coverage
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<script type="text/javascript">
    function clearCoverageInput() {
        var doc = document.forms[0];
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txCity.value = "";
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txZip.value = "";
        return true;
    }
</script>

<asp:Panel ID="pnCoverage" runat="server" DefaultButton="btSubmit">
    <table style="width: 100%;" class="tableWithLines">
        <tr>
            <th>
                City
            </th>
            <th>
                Zip
            </th>
            <th>
                &nbsp;
            </th>
        </tr>
        <tr>
            <td>
                <center><asp:TextBox ID="txCity" runat="server" Width="250" MaxLength="30" /></center>
            </td>
            <td align="center">
                <center>
                    <asp:TextBox ID="txZip" runat="server" Width="100" MaxLength="10" />
                    <asp:CompareValidator id="vCompare_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code entry must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Coverage" />
                </center>
            </td>
            <td>
                <center>
                <asp:Button ID="btSubmit" runat="server" Text="Search" onclick="btSubmit_Click" ValidationGroup="Coverage" />
                &nbsp;&nbsp; <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearCoverageInput();" />
                </center>   
            </td>
        </tr>
    </table>

    <asp:CustomValidator id="vCus_Coverage" runat="server" 
        Display="None" 
        EnableClientScript="False"
        ValidationGroup="Coverage" />
    
    <br />
    <center><asp:ValidationSummary ID="vSum_Coverage" runat="server" ValidationGroup="Coverage" /></center>
    <br />
</asp:Panel>
<div style="clear: both; height: 15px;"></div>
    <%-- Coverage Repeater --%> 
    <asp:Repeater ID="rpCoverage" runat="server" Visible="false">
        <HeaderTemplate>
            <table class="tableWithLines" style="width: 100%">
                <tr>
                    <th>City</th><th>State</th><th>Zip</th><th>Coverage</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg">
                <td><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                <td><asp:Label ID="lbState" runat="server" Text='<%# Eval("StateName") %>' /></td>
                <td><center><asp:Label ID="lbZip" runat="server" Text='<%# Eval("Zip") %>' /></center></td>
                <td><center><asp:Label ID="lbService" runat="server" Text='<%# Eval("Service") %>' /></center></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt">
                <td><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                <td><asp:Label ID="lbState" runat="server" Text='<%# Eval("StateName") %>' /></td>
                <td><center><asp:Label ID="lbZip" runat="server" Text='<%# Eval("Zip") %>' /></center></td>
                <td><center><asp:Label ID="lbService" runat="server" Text='<%# Eval("Service") %>' /></center></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>

