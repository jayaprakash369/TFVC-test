<%@ Page Title="Ticket Detail" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="B2TicketDetail.aspx.cs" 
    Inherits="public_sc_B2TicketDetail" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
    <style type="text/css">
        .table1 { 
        }
        .table1 th { 
            font-weight: bold;
            text-align: left;
            padding-right: 20px;
        }
        .table1 td { 
            color: #333333;  /* goldenrod? */
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Ticket Detail &nbsp; <asp:Label ID="lbTitleTicket" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

        <%--  --%>
    <div class="w3-row w3-padding-32">
            <div class="w3-container">
            <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
            <table class="tableWithoutLines table1">
                <tr style="vertical-align: top;">
                    <th>Name</th>
                    <td>
                        <asp:Label ID="lbCstName" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Address</th>
                    <td><asp:Label ID="lbCstAddress" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>City, State</th>
                    <td><asp:Label ID="lbCstCityStateZip" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Summary</th>
                    <td><asp:Label ID="lbTckSummary" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Status</th>
                    <td><asp:Label ID="lbTckStatus" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Contact</th>
                    <td><asp:Label ID="lbCstContact" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Phone</th>
                    <td><asp:Label ID="lbCstPhone" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Severity</th>
                    <td><asp:Label ID="lbTckSeverity" runat="server" /></td>
                </tr>

            </table>


        <%--  --%>
            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Panel ID="pnNotesSmall" runat="server">

            <h3 class="w3-text-teal">Notes</h3>
            <div style="padding-right: 25px;">
            <asp:Repeater ID="rp_B2TicketNotesSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Summary</td>
                        <td><asp:Label ID="lbNoteDetail" runat="server" Text='<% #Eval("ticket-summary") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Message</td>
                        <td><asp:Label ID="lbNoteMessage" runat="server" Text='<% #Eval("text") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Summary</td>
                        <td><asp:Label ID="lbNoteDetail" runat="server" Text='<% #Eval("ticket-summary") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Message</td>
                        <td><asp:Label ID="lbNoteMessage" runat="server" Text='<% #Eval("text") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            </div>
            </asp:Panel>
        </div>
            <!-- -->

        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->

        <asp:Panel ID="pnNotesLarge" runat="server">



        <h3 class="w3-text-teal">Notes</h3>
            <div style="padding-right: 25px;">
    <asp:GridView ID="gv_B2TicketNotesLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        EmptyDataText="No notes were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="Summary"  ItemStyle-HorizontalAlign="Left" >
                <ItemTemplate>
                    <asp:Label ID="lbNoteDetail" runat="server" Text='<%# Eval("ticket-summary") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Message" ItemStyle-HorizontalAlign="Left" >
                <ItemTemplate>
                    <asp:Label ID="lbNoteMessage" runat="server" Text='<%# Eval("text") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    </div>
    </asp:Panel>
        <!-- -->
        </div>

        </div>
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
<div class="spacer30"></div>

        <%--  --%>
 



        <%--  --%>

</div>
</asp:Content>
