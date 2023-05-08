<%@ Page Title="Service Request: Result" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Result.aspx.cs" 
    Inherits="private_sc_req_Result" %>
<%--
--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Service Request
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<%-- Hidden Fields --%>             
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfPhone" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfExtension" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfContact" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfReqKey" runat="server" Value="" Visible="false" />


    <%-- 
        <asp:HiddenField ID="hfReqParmList" runat="server" Value="" Visible="false" />
        --%>

<asp:Panel ID="pnCs1Header" runat="server" Visible="false" />

<%-- RESULT: TICKET OR FAILURE --%>
<asp:Panel ID="pnResult" runat="server">

    <%-- Result GridView --%> 
    <asp:GridView ID="gvResult" runat="server" style="width: 100%"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        EmptyDataText="No ticket was created">
        <AlternatingRowStyle CssClass="trColorAlt" />
         <Columns>
            <asp:BoundField DataField="Part" HeaderText="Model" />
            <asp:BoundField DataField="Serial" HeaderText="Serial" />
            <asp:BoundField DataField="Problem" HeaderText="Problem" />
            <asp:TemplateField HeaderText="Ticket XRef">
                <ItemTemplate>
                    <div style="text-align: center;">
                        <asp:Label ID="lbTckXref" runat="server" Text='<%# Eval("CrossRef") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Service Type">
                <ItemTemplate>
                    <div style="text-align: center;">
                        <asp:Label ID="lbService" runat="server" Text='<%# Eval("ServiceType") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ticket">
                <ItemTemplate>
                    <div style="text-align: center;">
                        <asp:Label ID="lbTicket" runat="server" Text='<%# Eval("Center") + "-" + Eval("Ticket") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="No Ticket?">
                <ItemTemplate>
                    <div style="text-align: center;">
                        <asp:Label ID="lbDelay" runat="server" Text='<%# Eval("Delay") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="lbNoTicket" runat="server" Visible="false" />
        <div style="height: 20px; clear: both;"></div>
        <div style="width: 100%; text-align:center;"><asp:Button ID="btAnotherRequest" runat="server" Text="Submit Another Request" onclick="btAnotherRequest_Click" /></div>
    <div style="height: 20px; clear: both;"></div>
    <div style="height: 20px; clear: both;"></div>
    <div style="width: 100%; text-align:center;"><asp:Button ID="btOpenTickets" runat="server" Text="View My Open Tickets" onclick="btOpenTickets_Click" /></div>
    </asp:Panel>

</asp:Content>


