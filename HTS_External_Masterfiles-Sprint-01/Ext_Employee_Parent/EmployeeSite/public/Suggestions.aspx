<%@ Page Title="Virtual Suggestion Box" Language="C#" MasterPageFile="~/MasterBlank.master" AutoEventWireup="true" CodeFile="Suggestions.aspx.cs" 
    Inherits="public_Suggestions" %>
<%--
--%>             

<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">

    <table style="width: 100%;">
        <tr>
            <td>
                Virtual Suggestion Box
            </td>
            <td style="float: right;">
                <asp:Button ID="btNewEntry" runat="server" 
                    Text="New Submittal" 
                    onclick="btNewEntry_Click"
                    TabIndex="1" />
                <asp:Button ID="btAddEntry" runat="server" 
                    Text="Submit" 
                    onclick="btAddEntry_Click" 
                    ValidationGroup="Comment" 
                    Visible="false" 
                    style="margin-left: 5px;"
                    TabIndex="4"  />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <asp:Panel ID="pnAccess" runat="server" DefaultButton="btDoNothing">
        <asp:Button ID="btDoNothing" runat="server" Text="" Visible="false" />

    <div id="dvFloatBox" style="position: absolute; left: -1000px; top: -1000px;">
       <asp:Calendar ID="calEntryDate" runat="server" 
        SkinID="calendar1" 
        onselectionchanged="calEntryDate_SelectionChanged" />   
    </div>
    
    <asp:Panel ID="pnNewEntry" runat="server" 
        Visible="false">
        <table style="width: 100%;" class="tableInput">
            <tr>
                <td>Title</td>
                <td>
                    <asp:TextBox ID="txNewTitle" runat="server" 
                        Width="600px"
                        MaxLength="100" 
                        TabIndex="2" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="vReq_txNewTitle" runat="server" 
                        ControlToValidate="txNewTitle" 
                        ErrorMessage="A title is required." 
                        Display="Dynamic"
                        ValidationGroup="Comment" />
                </td>
            </tr>
            <tr>
                <td>Comment</td>
                <td>
                    <asp:TextBox ID="txNewEntry" runat="server" 
                        Width="600px" 
                        Height="100px" 
                        MaxLength="2000" 
                        TextMode="MultiLine" 
                        TabIndex="3"  />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="vReq_txNewEntry" runat="server" 
                        ControlToValidate="txNewEntry" 
                        ErrorMessage="A suggestion or comment is required." 
                        Display="Dynamic"
                        ValidationGroup="Comment" />
                        <div style="clear: both; height: 5px;"></div>
                    <asp:Label ID="lbIdentity" runat="server" 
                        Font-Size="7"
                        ForeColor="#999999"
                        Font-Italic="true"
                        Text="Submitter name is automatically attached to submittals" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnViewEntry" runat="server" 
        Visible="false">
        <table style="width: 100%;">
            <tr>
                <td style="width: 50%; vertical-align: top; padding-right: 30px;">
                    <table style="width: 100%;" class="tableInput">
                        <tr>
                            <td style="width: 100px;">Submitter</td>
                            <td><asp:Label ID="lbSubmitter" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Entry Date</td>
                            <td><asp:Label ID="lbSubmitDate" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Key</td>
                            <td><asp:Label ID="lbKey" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Title</td>
                            <td><asp:Label ID="lbTitle" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Comment</td>
                            <td><asp:Label ID="lbComment" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td style="width: 50%; vertical-align: top;">
                    <table style="width: 100%;" class="tableInput">
                        <tr>
                            <td style="width: 100px;">Assigned</td>
                            <td>
                                <asp:Label ID="lbAssigned" runat="server" />
                                <asp:DropDownList ID="ddAssigned" runat="server" 
                                    Visible="false" 
                                    AutoPostBack="true"
                                    onselectedindexchanged="ddAssigned_SelectedIndexChanged">
<%--
                                    <asp:ListItem Text="JANNING, BRIAN S." Value="1275" />
                                    <asp:ListItem Text="KATHOL, VERN J." Value="1119" />
                                    <asp:ListItem Text="SMUTNEY, JAMES, C." Value="1141" />
--%>             
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Response
                                <div style="height: 10px; clear: both;"></div>
                                <asp:Button ID="btResponse" runat="server" 
                                    Text="Update" 
                                    ValidationGroup="Response" 
                                    onclick="btResponse_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lbResponse" runat="server" />
                                <asp:TextBox ID="txNewResponse" runat="server" 
                                    Width="350px" 
                                    Height="150px" 
                                    MaxLength="2000" 
                                    TextMode="MultiLine" 
                                    ValidationGroup="Response"
                                    Visible="false" />
                                <div style="clear: both;"></div>
                                <asp:RequiredFieldValidator ID="vReq_Response" runat="server" 
                                    ControlToValidate="txNewResponse" 
                                    ErrorMessage="A response is required." 
                                    Display="Dynamic"
                                    ValidationGroup="Response" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table style="width: 100%;" class="tableForSearch">
        <tr>
            <th>
                Key
            </th>
            <th>
                Submitter
            </th>
            <th>
                Title
            </th>
            <th>
                Comment
            </th>
            <th>
                <asp:LinkButton ID="lkEntryDate" runat="server" 
                    Width="120px"
                    CssClass="OFF" 
                    Text="Entry Date >" 
                    onclick="lkEntryDate_Click" />
            </th>
            <th>
                Assignee
            </th>
            <th>
                <asp:Button ID="btClear" runat="server" Text="Clear" onclick="btClear_Click" />
                &nbsp;
                <asp:Button ID="btSearch" runat="server" Text="Search" onclick="btSearch_Click" />
            </th>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txKey" runat="server" MaxLength="5" Width="20" />
            </td>
            <td>
                <asp:TextBox ID="txEntryName" runat="server" MaxLength="30" Width="150"   />
            </td>
            <td>
                <asp:TextBox ID="txTitle" runat="server" MaxLength="50" Width="150" />
            </td>
            <td>
                <asp:TextBox ID="txComment" runat="server" MaxLength="100" Width="150" />
            </td>
            <td>
                <asp:TextBox ID="txEntryDate" runat="server" 
                    Width="100" 
                    ReadOnly="true" 
                    CssClass="readOnly"  />
            </td>
            <td>
                <asp:TextBox ID="txAssignName" runat="server" MaxLength="30" Width="150" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    
    <div>
    <asp:GridView ID="gvComments" runat="server"
        CssClass="tableWithLines"
        Width="998"
        AutoGenerateColumns="False"
        PageSize="25"
        AllowPaging="true" 
        AllowSorting="true" 
        onpageindexchanging="gvPageIndexChanging_Com" 
        onsorting="gvSorting_Com"
        EmptyDataText="No matching records found...">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Key" DataField="Key" SortExpression="Key" 
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Submitter" DataField="DisplaySubmitter" SortExpression="DisplaySubmitter" 
                ItemStyle-HorizontalAlign="Center"  />
            <asp:TemplateField HeaderText="Title" SortExpression="DisplayTitle">
                <ItemTemplate>
                    <asp:LinkButton ID="lkTitle" runat="server" 
                        OnClick="lkTitle_Click" 
                        CommandArgument='<%# Eval("Key") %>' 
                        Text='<%# Eval("DisplayTitle")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Comment" DataField="DisplayComment" SortExpression="DisplayComment" />
            <asp:BoundField HeaderText="Entry" DataField="DisplayEntryDate" SortExpression="EntryDate" 
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Assignee" DataField="DisplayAssignee" SortExpression="DisplayAssignee" 
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Response" DataField="DisplayResponse" SortExpression="DisplayResponse" />
        </Columns>
    </asp:GridView>
</div>
</asp:Panel>

    <asp:HiddenField ID="hfEmp" runat="server" Visible="false" />
    <asp:HiddenField ID="hfEditKey" runat="server" Visible="false" />
    <asp:HiddenField ID="hfReloadAssignments" runat="server" Visible="false" />
    
    <center><asp:Label ID="lbAccess" runat="server" Text="Default" Visible="false" /></center>
    
    <script type="text/javascript" language="javascript">
        setFloatBoxPos('ctl00_BodyContent_lkEntryDate', 'dvFloatBox');
    </script>

</asp:Content>

