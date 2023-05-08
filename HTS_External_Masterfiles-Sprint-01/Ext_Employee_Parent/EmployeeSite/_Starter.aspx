<%@ Page Title="Starter" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="_Starter.aspx.cs" 
    MaintainScrollPositionOnPostback="true" 
    Inherits="private_ms__Starter" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
Starter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    
<%-- 
<asp:Panel ID="Panel1" runat="server" DefaultButton="Button1">
<div style="height: 20px; clear: both;"></div>
    <asp:GridView ID="gvStarter" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
        <asp:BoundField HeaderText="One" DataField="CustName" />
        <asp:TemplateField HeaderText="Two">
            <ItemTemplate>
                <asp:LinkButton ID="lkStudent" runat="server" 
                    OnClick="lkStudent_Click" 
                    CommandArgument='<%# Eval("EMPNUM") %>' 
                    Text='<%# Eval("EMPNAM")%>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Button" Visible="false" />
    </asp:Panel>
    <div style="height: 20px; clear: both;"></div>
 --%> 

    <asp:HiddenField ID="hfHtsNum" runat="server" />
    <asp:HiddenField ID="hfUserName" runat="server" />

</asp:Content>


