<%@ Page Title="Print Files" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="PrintFiles.aspx.cs" 
    Inherits="private__admin_PrintFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    Print Files
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <table style="width: 100%;">
        <tr>
            <td><asp:Button ID="btSql" runat="server" Text="Submit" onclick="btSql_Click" /></td>
            <td>
                <asp:DropDownList ID="ddSql" runat="server" 
                    AutoPostBack="true" 
                    CssClass="dropDownList1" Width="800" 
                    onselectedindexchanged="ddSql_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txSql" runat="server" TextMode="MultiLine" Height="150" Width="900" />
            </td>
        </tr>
    </table>

<div style="clear: both; height: 25px;"></div>
<asp:GridView ID="gvSql" runat="server" 
    CssClass="tableWithLines" 
    EmptyDataText="No results were found" 
    AutoGenerateColumns="true">
</asp:GridView>
<div style="clear: both; height: 25px;"></div>

</asp:Content>

