<%@ Page Title="HTS Autotask Tickets" Language="C#" MasterPageFile="~/MasterBlank.master" AutoEventWireup="true" CodeFile="tickets.aspx.cs" 
    MaintainScrollPositionOnPostback="true" 
    Inherits="public_autotask_Tickets" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyTitle" Runat="Server">
<table>
    <tr style="vertical-align: bottom;">
        <td style="padding-right: 50px;">HTS Autotask Tickets</td>
        <td style="padding-right: 10px;">
            <asp:Label ID="lbSearchText" runat="server" Text="Ticket 'Date ID' like" Font-Size="9" />
            <br />
            <asp:TextBox ID="txSearchTid" runat="server" Font-Size="9" /></td>
        <td style="padding-right: 10px;">
            <asp:RadioButtonList ID="rblSearchStatus" runat="server" RepeatDirection="Horizontal" SkinID="radioButtonList0" style="font-size:10px;">
                <asp:ListItem Text="Open" Value="Open" Selected="True" />
                <asp:ListItem Text="Closed" Value="Closed" />
                <asp:ListItem Text="Both" Value="Both" />
            </asp:RadioButtonList>
        </td>
        <td style="padding-right: 10px;"><asp:Button ID="btSearch" runat="server" Text="Search" Font-Size="8" 
                onclick="btSearch_Click" /></td>
        </tr>
</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    
<%-- ALL TICKETS --%> 
<asp:Panel ID="pnTickets" runat="server" DefaultButton="Button1">
<div style="height: 1px; clear: both;"></div>
    <asp:GridView ID="gvTickets" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
        <asp:TemplateField HeaderText="(HTS Detail)">
            <ItemTemplate>
                <asp:LinkButton ID="lkDetail" runat="server" 
                    OnClick="lkDetail_Click" 
                    CommandArgument='<%# Eval("ATTICKET") %>' 
                    Text='<%# Eval("ATTICKET")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="HtsCust" DataField="ATCUSXRF" />
        <asp:BoundField HeaderText="HtsCall" DataField="DisplayCall" />
        <asp:BoundField HeaderText="Name" DataField="ACCTNAME" />
        <%-- 
                <asp:BoundField HeaderText="Contact" DataField="ACCTCON" />
        <asp:BoundField HeaderText="Phone" DataField="ACCTPHN" />
        --%> 
        <asp:BoundField HeaderText="City" DataField="ACCTCIT" />
        <asp:BoundField HeaderText="ST" DataField="ACCTSTA" />
        <asp:BoundField HeaderText="Title" DataField="TITLE" />
        <asp:BoundField HeaderText="Status" DataField="DisplayStatus" />
        
        <%-- 
        <asp:BoundField HeaderText="Center" DataField="CTR" />
        <asp:BoundField HeaderText="Ticket" DataField="TCK" />
        <asp:BoundField HeaderText="Created" DataField="DisplayCreated" />
        --%> 
    </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Button" Visible="false" />
    </asp:Panel>
    
    <%-- ONE TICKET DETAIL --%>
    
    <asp:Panel ID="pnDetail" runat="server" Visible="false">

        <%-- TICKET VALUES --%>
        <asp:Panel ID="pnTicket" runat="server" Visible="false">
        <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbTicket" runat="server" Text="Autotask Detail" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
            <table class="tableWithLines">
                <tr>
                    <td>AT Ticket ID</td>
                    <td><asp:Label ID="lbTckAid" runat="server" /></td>
                </tr>
                <tr>
                    <td>HTS Ticket ID</td>
                    <td><asp:Label ID="lbTckHid" runat="server" /></td>
                </tr>
                <tr>
                    <td>Account Name</td>
                    <td><asp:Label ID="lbTckNam" runat="server" /></td>
                </tr>
                <tr>
                    <td>Title</td>
                    <td><asp:Label ID="lbTckTit" runat="server" /></td>
                </tr>
                <tr>
                    <td>Description</td>
                    <td style="max-width: 350px;"><asp:Label ID="lbTckDsc" runat="server" /></td>
                </tr>
                <tr>
                    <td>Issue Type</td>
                    <td><asp:Label ID="lbTckTyp" runat="server" /></td>
                </tr>
                <tr>
                    <td>Issue Subtype</td>
                    <td><asp:Label ID="lbTckSub" runat="server" /></td>
                </tr>
                <tr>
                    <td>Contact</td>
                    <td><asp:Label ID="lbTckCon" runat="server" /></td>
                </tr>
                <tr>
                    <td>Phone</td>
                    <td><asp:Label ID="lbTckPhn" runat="server" /></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td><asp:Label ID="lbTckAdr" runat="server" /></td>
                </tr>
                <tr>
                    <td>HTS Cross Ref</td>
                    <td><asp:Label ID="lbTckXrf" runat="server" /></td>
                </tr>
                <tr>
                    <td>Contract</td>
                    <td><asp:Label ID="lbTckAgr" runat="server" /></td>
                </tr>
                <tr>
                    <td>Allocation Code</td>
                    <td><asp:Label ID="lbTckAlc" runat="server" /></td>
                </tr>
            </table>
            <div style="clear: both; height: 15px;"></div>
            </div>
        </asp:Panel>

        <%-- AUTOTASK NOTES --%>
        <asp:Panel ID="pnAtNotes" runat="server" Visible="false">
        <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbAtNotes" runat="server" Text="Autotask Notes" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
                <asp:GridView ID="gvAtNotes" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Title" DataField="TNTITLE" ItemStyle-Width="250" />
                        <asp:BoundField HeaderText="Description" DataField="TNTEXT" ItemStyle-Width="350"/>
                    </Columns>
                </asp:GridView>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>

        <%-- HTS NOTES --%>
        <asp:Panel ID="pnHtsNotes" runat="server" Visible="false">
        <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbHtsNotes" runat="server" Text="HTS Notes" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
                <asp:GridView ID="gvHtsNotes" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Subject" DataField="HNTITLE" ItemStyle-Width="250" />
                        <asp:BoundField HeaderText="Message" DataField="HNTEXT" ItemStyle-Width="350" />
                    </Columns>
                </asp:GridView>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>

        <%-- ERRORS  --%>
        <asp:Panel ID="pnErrors" runat="server" Visible="false">
        <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbErrors" runat="server" Text="Errors" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
            <asp:Repeater ID="rpErrors" runat="server">
                <HeaderTemplate>
                    <table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr style="background-color: #EEEEEE;">
                        <td><asp:Label ID="lbType" runat="server" Text='<%# Eval("ECTYPE") %>' /></td>
                        <td><asp:Label ID="lbStamp" runat="server" Text='<%# Eval("DisplayStamp") %>' /></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-bottom: 10px;">
                            <asp:Label ID="lbEvent" runat="server" Text='<%# Eval("ELEVENT") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>
        
        <%-- ONSITE LABOR --%>
        <asp:Panel ID="pnOnsiteLabor" runat="server" Visible="false">
            <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbOnsiteLabor" runat="server" Text="Onsite Labor" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
            <asp:GridView ID="gvOnsiteLabor" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLines"
                EmptyDataText="No matching records were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                <asp:BoundField HeaderText="Start Date" DataField="DisplayDate" ItemStyle-HorizontalAlign="Right"  />
                <asp:BoundField HeaderText="Start Time" DataField="DisplayStart" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="End Time" DataField="DisplayEnd" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Duration" DataField="Duration" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="Tech" DataField="Tech" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="Name" DataField="Name" />
            </Columns>
            </asp:GridView>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>

        <%-- TRAVEL LABOR --%>
        <asp:Panel ID="pnTravelLabor" runat="server" Visible="false">
            <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbTravelLabor" runat="server" Text="Travel Labor" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
            <asp:GridView ID="gvTravelLabor" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLines"
                EmptyDataText="No matching records were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                <asp:BoundField HeaderText="Start Date" DataField="DisplayDate" ItemStyle-HorizontalAlign="Right"  />
                <asp:BoundField HeaderText="Start Time" DataField="DisplayStart" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="End Time" DataField="DisplayEnd" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Duration" DataField="Duration" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="Tech" DataField="Tech" ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField HeaderText="Name" DataField="Name" />
            </Columns>
            </asp:GridView>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>

        <%-- PARTS USED --%>
        <asp:Panel ID="pnPartsUsed" runat="server" Visible="false">
            <div style="display: block; float: left; padding-right: 20px;">
            <asp:Label ID="lbPartsUsed" runat="server" Text="Parts Used" SkinID="TableTitle1" />
            <div style="clear: both; height: 5px;"></div>
            <asp:GridView ID="gvPartsUsed" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLines"
                EmptyDataText="No matching records were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                <asp:BoundField HeaderText="Part" DataField="Part" />
                <asp:BoundField HeaderText="Description" DataField="Description" />
                <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Serial" DataField="Serial" />
                <asp:BoundField HeaderText="From Loc" DataField="Location" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Date" DataField="DisplayDate" />
            </Columns>
            </asp:GridView>
            <div style="clear: both; height: 10px;"></div>
            </div>
        </asp:Panel>


    </asp:Panel>
    
    <div style="clear: both; height: 1px;"></div>

    <asp:HiddenField ID="hfTicketDateId" runat="server" />

</asp:Content>


