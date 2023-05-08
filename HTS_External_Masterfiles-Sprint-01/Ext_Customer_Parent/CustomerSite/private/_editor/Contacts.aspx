<%@ Page Title="Customer Contacts" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Contacts.aspx.cs" 
    Inherits="private__editor_Contacts" %>

    <%-- 
    '<%# "Organization: " + Eval("Organization") + "\r\n" + "Title: " + Eval("Title") + "\r\n" + "Source Page: " + Eval("SourcePage") + "\r\n" + "Last Contact: " + Eval("DateEntered") + "\r\n" + "Opt Out Phone?: " + Eval("OptOutPhone") + "\r\n" + "Opt Out Email?: " + Eval("OptOutEmail") %>'
     --%> 

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Customer Contacts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <script type="text/javascript">
        function clearSearch() {
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txFirst.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txLast.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCity.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txST.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txZip.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txPhone.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txEmail.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSource.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txDate.value = "";
            return true;
        }
    </script>

    <%-- Search Panel --%>
    <asp:Panel ID="pnSearch" runat="server" DefaultButton="btSearch">
        <table class="tableWithLines" >
            <tr>
                <th>First</th>
                <th>Last</th>
                <th>City</th>
                <th>ST</th>
                <th>Zip</th>
                <th>Phone</th>
                <th>Email</th>
                <th>Source</th>
                <th>Date >=</th>
                <th colspan="2"></th>
            </tr>
            <tr>
                <th style="text-align: center;"><asp:TextBox ID="txFirst" runat="server" Width="70"  MaxLength="30" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txLast" runat="server" Width=" 90"  MaxLength="30" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txCity" runat="server" Width="110" MaxLength="30" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txST" runat="server" Width="20"  MaxLength="2" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txZip" runat="server" Width="50"  MaxLength="10" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txPhone" runat="server" Width="80" MaxLength="15" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txEmail" runat="server" Width="160" MaxLength="50" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txSource" runat="server" Width="70" /></th>
                <th style="text-align: center;"><asp:TextBox ID="txDate" runat="server" Width="70" MaxLength="8" /></th>
                <th style="text-align: center;"><asp:Button ID="btSearch" runat="server" Text="Search" onclick="btSearch_Click" /></th>
                <th style="text-align: center;"><input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearSearch();" /></th>
            </tr>
        </table>
    <div style="height: 15px; clear: both;"></div>
    </asp:Panel>

    <%-- Update Panel --%>
    <asp:Panel ID="pnUpdate" runat="server" Visible="false" DefaultButton="btHidden">
        <asp:Button ID="btHidden" runat="server" Visible="false" ValidationGroup="none" />
        <table class="tableWithLines" >
            <tr>
                <th style="width: 175px; background-color: #3a7728;" ><asp:Label ID="lbUpdate" runat="server" /></th>
                <th style="background-color: #3a7728;"><asp:Button ID="btUpdate" runat="server" Text="Update" onclick="btUpdate_Click" /></th>
                <td style="text-align: center;">
                    Comment &nbsp;<asp:TextBox ID="txUpdComment" runat="server" Width="400" MaxLength="500" />
                </td>
                <td style="text-align: center;">
                    Opt Out?
                    &nbsp;&nbsp;
                    <asp:CheckBox ID="chBxPhone" runat="server" Text="Phone" />
                    &nbsp;
                    <asp:CheckBox ID="chBxEmail" runat="server" Text="Email" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfUpdKey" runat="server" />
    <div style="height: 15px; clear: both;"></div>
    </asp:Panel>
    
    <%-- Table Panel --%> 
    <asp:Panel ID="pnTable" runat="server" Visible="true">

    <asp:GridView ID="gvContacts" runat="server"
        CssClass="tableWithLines"
        AutoGenerateColumns="False"
        PageSize="500"
        AllowSorting="true" 
        onsorting="gvSorting_Con"
        EmptyDataText="No matching records found...">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
        <asp:BoundField HeaderText="Contact Date" DataField="DateDisplay" SortExpression="DateEntered" ItemStyle-VerticalAlign="Top" ItemStyle-Width="75" />
        <asp:BoundField HeaderText="First" DataField="First" SortExpression="First" ItemStyle-VerticalAlign="Top" />
        <asp:TemplateField HeaderText="Last" SortExpression="Last" ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
                <asp:LinkButton ID="lkContactUpd" runat="server" 
                    Text='<%# Eval("Last") %>' 
                    onClick="lkUpdate_Click" 
                    ToolTip='<%# "\r\n" + "Organization: " + Eval("Organization") + "\r\n\r\n" + "Title: " + Eval("Title") + "\r\n" %>'
                    CommandArgument='<%# Eval("Key") + "|" + Eval("OptOutPhone")  + "|" + Eval("OptOutEmail") + "|" + Eval("First")   + "|" + Eval("Last")  + "|" + Eval("Comment")  %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="ST" DataField="ST" SortExpression="ST" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Zip" DataField="Zip" SortExpression="Zip" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Phone" DataField="Phone" SortExpression="Phone" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Ext" DataField="Ext" SortExpression="Ext" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Email" DataField="Email" SortExpression="Email" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Source" DataField="SourcePage" SortExpression="SourcePage" ItemStyle-VerticalAlign="Top" />
        <asp:BoundField HeaderText="Kelly" DataField="Comment" SortExpression="Comment" ItemStyle-VerticalAlign="Top" />
        <asp:TemplateField HeaderText="Comments" ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
                <asp:Label ID="lbComments" runat="server" Text='<%# Eval("CustComments") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>
    
    <div style="height: 20px; clear: both;"></div>

    <asp:Repeater ID="rpContacts" runat="server">
        <HeaderTemplate>
            <table class="tableWithLines" style="border-left: 1px solid #333333; border-right: 1px solid #333333;">
                <tr>
                    <th>First</th>
                    <th>Last</th>
                    <th>City</th>
                    <th>ST</th>
                    <th>Zip</th>
                    <th>Phone</th>
                    <th>Ext</th>
                    <th>Email</th>
                    <th>Source</th>
                    <th>LastDate</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="trColorReg">
                <asp:HiddenField ID="hfOptOutPhone" runat="server" Value='<%# Eval("OptOutPhone") %>' />
                <asp:HiddenField ID="hfOptOutEmail" runat="server" Value='<%# Eval("OptOutEmail") %>'  />
                <td><asp:Label ID="lbFirst" runat="server" Text='<%# Eval("First") %>' /></td>
                <td>
                    <asp:LinkButton ID="lkContactUpd" runat="server" 
                        Text='<%# Eval("Last") %>' 
                        onClick="lkUpdate_Click" 
                        ToolTip='<%# "\r\n" + "Organization: " + Eval("Organization") + "\r\n\r\n" + "Title: " + Eval("Title") + "\r\n\r\n" + "Comment: " + Eval("Comment") + "\r\n" %>'
                        CommandArgument='<%# Eval("Key") + "|" + Eval("OptOutPhone")  + "|" + Eval("OptOutEmail") + "|" + Eval("First")   + "|" + Eval("Last")  + "|" + Eval("Comment")  %>' />
                </td>
                <td><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbST" runat="server" Text='<%# Eval("ST") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbZip" runat="server" Text='<%# Eval("Zip") %>' /></td>
                <td><asp:Label ID="lbPhone" runat="server" Text='<%# Eval("Phone") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbExt" runat="server" Text='<%# Eval("Ext") %>' /></td>
                <td><asp:Label ID="lbEmail" runat="server" Text='<%# Eval("Email") %>' /></td>
                <td><asp:Label ID="lbSource" runat="server" Text='<%# Eval("SourcePage") %>' /></td>
                <td><asp:Label ID="lbDate" runat="server" Text='<%# Eval("DateEntered") %>' /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trColorAlt">
                <asp:HiddenField ID="hfOptOutPhone" runat="server" Value='<%# Eval("OptOutPhone") %>' />
                <asp:HiddenField ID="hfOptOutEmail" runat="server" Value='<%# Eval("OptOutEmail") %>'  />
                <td><asp:Label ID="lbFirst" runat="server" Text='<%# Eval("First") %>' /></td>
                <td>
                    <asp:LinkButton ID="lkContactUpd" runat="server" 
                        Text='<%# Eval("Last") %>' 
                        onClick="lkUpdate_Click" 
                        ToolTip='<%# "\r\n" + "Organization: " + Eval("Organization") + "\r\n\r\n" + "Title: " + Eval("Title") + "\r\n\r\n" + "Comment: " + Eval("Comment") + "\r\n" %>'
                        CommandArgument='<%# Eval("Key") + "|" + Eval("OptOutPhone")  + "|" + Eval("OptOutEmail") + "|" + Eval("First")   + "|" + Eval("Last")  + "|" + Eval("Comment")  %>' />                        
                </td>
                <td><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbST" runat="server" Text='<%# Eval("ST") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbZip" runat="server" Text='<%# Eval("Zip") %>' /></td>
                <td><asp:Label ID="lbPhone" runat="server" Text='<%# Eval("Phone") %>' /></td>
                <td style="padding-left: 0; text-align: center;"><asp:Label ID="lbExt" runat="server" Text='<%# Eval("Ext") %>' /></td>
                <td><asp:Label ID="lbEmail" runat="server" Text='<%# Eval("Email") %>' /></td>
                <td><asp:Label ID="lbSource" runat="server" Text='<%# Eval("SourcePage") %>' /></td>
                <td><asp:Label ID="lbDate" runat="server" Text='<%# Eval("DateEntered") %>' /></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    </asp:Panel>

    <center><asp:Label ID="lbMessage" runat="server" Visible="false" SkinID="labelInstructions" /></center>
</asp:Content>

