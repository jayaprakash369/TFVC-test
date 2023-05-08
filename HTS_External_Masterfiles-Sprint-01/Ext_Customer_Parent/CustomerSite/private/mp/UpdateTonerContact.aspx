<%@ Page Title="Update Toner Contact" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="UpdateTonerContact.aspx.cs" 
    Inherits="private_mp_UpdateTonerContact" %>
<%-- --%>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Update Toner Contact
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
        function clearConSearch() {
            var doc = document.forms[0];
            ctl00_ctl00_For_Body_A_For_Body_A
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchLoc.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchFxa.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchSer.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchCon.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSearchEml.value = "";
            return true;
        }
    </script>
<%-- D2EEC9 E2F4DC F2FAEF / D6E1EA E7EEF3 --%>
<asp:Panel ID="pnUpdate" runat="server" Visible="false" style="margin-bottom: 20px;">
    <table style="width: 100%; background-color: #dddddd; border: 1px solid #bbbbbb; padding-bottom: 4px;" class="tableWithoutLines">
        <tr>
            <td colspan="6" style="font-size: 15px; padding: 5px;"><asp:Label ID="lbUpdKeys" runat="server" /></td>
        </tr>
        <tr style="font-weight: bold;">
            <td>Contact</td>
            <td>Title</td>
            <td>Phone</td>
            <td>Ext</td>
            <td>Email</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txContact" runat="server" Width="140"  MaxLength="40" />
                <asp:RequiredFieldValidator ID="vReq_Contact" runat="server" 
                    ControlToValidate="txContact" 
                    ErrorMessage="A contact name is required." 
                    Text="*"
                    Display="Dynamic"
                    ValidationGroup="Contact" />
            </td>
            <td><asp:TextBox ID="txTitle" runat="server" Width="140"  MaxLength="50" /></td>
            <td>
                (<asp:TextBox ID="txPh1" runat="server" Width="30" MaxLength="3" />)
                <asp:CompareValidator id="vComp_Ph1" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txPh1"
                    ErrorMessage="Area code must be a number" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="Contact">
                </asp:CompareValidator>
                &nbsp;<asp:TextBox ID="txPh2" runat="server" Width="30" MaxLength="3" />
                <asp:CompareValidator id="vComp_Ph2" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txPh2"
                    ErrorMessage="Phone prefix must be a number" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="Contact">
                </asp:CompareValidator>
                -&nbsp;<asp:TextBox ID="txPh3" runat="server" Width="40" MaxLength="4" />
                <asp:CompareValidator id="vComp_Ph3" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txPh3"
                    ErrorMessage="Phone suffix must be a number" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="Contact">
                </asp:CompareValidator>

            </td>
            <td>
                <asp:TextBox ID="txExt" runat="server" Width="45" MaxLength="8" />
                <asp:CompareValidator id="vComp_Ext" runat="server" 
                    Operator="DataTypeCheck"
                    Type="Integer"
                    ControlToValidate="txExt"
                    ErrorMessage="The extension must be a number" 
                    Text="*"
                    SetFocusOnError="true" 
                    ValidationGroup="Contact">
                </asp:CompareValidator>
            </td>
            <td>
                <asp:TextBox ID="txEmail" runat="server" Width="180" MaxLength="50"  />
                <asp:RegularExpressionValidator id="vReg_Email" runat="server"
                    ControlToValidate="txEmail"
                    ValidationExpression="^\S+@\S+\.\S+$"
                    ErrorMessage="Email address is not in a valid format" 
                    Text="*"
                    SetFocusOnError="true"
                    Display="Dynamic"
                    ValidationGroup="Contact" />
            </td>
            <td><asp:Button ID="btUpdate" runat="server" 
                Text="Update" 
                onclick="btUpdate_Click" 
                ValidationGroup="Contact" />
            </td>
        </tr>
    </table>
    <center>
        <asp:CustomValidator id="vCus_Contact" runat="server" 
            Display="None" 
            EnableClientScript="False"
            ValidationGroup="Contact" />
        <asp:ValidationSummary ID="vSum_Contact" runat="server" 
            ValidationGroup="Contact" />
    </center>
</asp:Panel>

<asp:Panel ID="pnContacts" runat="server" Visible="true">
<center>
    <table cellspacing="0" style="width: 100%; margin-bottom: 10px; background-color: #eeeeee; border: 1px solid #999999;" class="tableWithoutLines">
        <tr>
            <th>Loc</th>
            <th>Asset</th>
            <th>Serial</th>
            <th>Contact</th>
            <th>Email</th>
            <th>&nbsp;</th>
        </tr>
        <tr>
            <td><center><asp:TextBox ID="txSearchLoc" runat="server" Width="30" MaxLength="3" /></center></td>
            <td><center><asp:TextBox ID="txSearchFxa" runat="server" Width="125" MaxLength="15" /></center></td>
            <td><center><asp:TextBox ID="txSearchSer" runat="server" Width="150" MaxLength="25" /></center></td>
            <td><center><asp:TextBox ID="txSearchCon" runat="server" Width="175" MaxLength="40" /></center></td>
            <td><center><asp:TextBox ID="txSearchEml" runat="server" Width="200" MaxLength="50" /></center></td>
            <td><center>
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearConSearch();" />
                &nbsp;<asp:Button ID="btSearch" runat="server" Text="Search" onclick="btSearch_Click" /></center>
            </td>
        </tr>
    </table>
</center>
  <asp:GridView ID="gvContacts" runat="server"
    AutoGenerateColumns="False"
    CssClass="tableWithLines"
    AllowPaging="True"
    PageSize="750" 
    AllowSorting="True" 
    onpageindexchanging="gvPageIndexChanging_Con" 
    onsorting="gvSorting_Con"
    EmptyDataText="No toner contacts were found">
    <AlternatingRowStyle CssClass="trColorAlt" />
    <Columns>
        <asp:TemplateField HeaderText="Cust" SortExpression="Cs1" >
            <ItemTemplate>
                <center><asp:Label ID="lbCs1" runat="server" Text='<%# Eval("Cs1") %>' /></center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Loc" SortExpression="Cs2" >
            <ItemTemplate>
                <center><asp:Label ID="lbCs2" runat="server" Text='<%# Eval("Cs2") %>' /></center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Asset" SortExpression="Asset" >
            <ItemTemplate>
                <center><asp:Label ID="lbAsset" runat="server" Text='<%# Eval("Asset") %>' /></center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Serial" SortExpression="Serial" >
            <ItemTemplate>
                <center><asp:Label ID="lbSerial" runat="server" Text='<%# Eval("Serial") %>' /></center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Contact" SortExpression="Contact" >
            <ItemTemplate>
                <asp:LinkButton ID="lkContact" runat="server" 
                    CommandArgument='<%# Eval("Cs1") + "|" + Eval("Cs2") + "|" + Eval("Code") + "|" + Eval("Contact") + "|" + Eval("Unit") %>'  
                    Text='<%# Eval("Contact") %>'
                    OnClick="lkContact_Click">
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Title" SortExpression="Title" >
            <ItemTemplate>
                <asp:Label ID="lbTitle" runat="server" 
                    Text='<%# Eval("Title") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Phone" SortExpression="PhoneFormat" >
            <ItemTemplate>
                <asp:Label ID="lbPhoneFormat" runat="server" 
                    Text='<%# Eval("PhoneFormat") %>' />
                <asp:HiddenField ID="hfPhone" runat="server" Value='<%# Eval("Phone") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ext" SortExpression="Ext" >
            <ItemTemplate>
                <asp:Label ID="lbExt" runat="server" 
                    Text='<%# Eval("Ext") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Email" SortExpression="Email" >
            <ItemTemplate>
                <asp:Label ID="lbEmail" runat="server" 
                    Text='<%# Eval("Email") %>' />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
    <asp:HiddenField ID="hfCs1" runat="server" />
    <asp:HiddenField ID="hfCs2" runat="server" />
    <asp:HiddenField ID="hfTyp" runat="server" />
    <asp:HiddenField ID="hfCon" runat="server" />
    <asp:HiddenField ID="hfUnt" runat="server" />
</asp:Panel>


</asp:Content>

