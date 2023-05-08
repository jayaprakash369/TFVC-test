<%@ Page Title="Open Tickets" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Timestamps.aspx.cs" 
    Inherits="private_sc_Timestamps" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Ticket Status: &nbsp;<asp:Label ID="lbTitleTicket" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
                            <asp:Repeater ID="rp_TimestampSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Status</td>
                        <td><asp:Label ID="lbStampStatus" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Time</td>
                        <td><asp:Label ID="lbStampTime" runat="server" Text='<% #Eval("TimestampFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tech</td>
                        <td><asp:Label ID="lbStampEmp" runat="server" Text='<% #Eval("TechNum") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbStampEmpName" runat="server" Text='<% #Eval("TechName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Schedule</td>
                        <td><asp:Label ID="lbStampSchedule" runat="server" Text='<% #Eval("ScheduleFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Status</td>
                        <td><asp:Label ID="lbStampStatus" runat="server" Text='<% #Eval("Status") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Time</td>
                        <td><asp:Label ID="lbStampTime" runat="server" Text='<% #Eval("TimestampFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tech</td>
                        <td><asp:Label ID="lbStampEmp" runat="server" Text='<% #Eval("TechNum") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbStampEmpName" runat="server" Text='<% #Eval("TechName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Schedule</td>
                        <td><asp:Label ID="lbStampSchedule" runat="server" Text='<% #Eval("ScheduleDate") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
                <asp:GridView ID="gv_TimestampLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Time" DataField="TimestampFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Tech" DataField="TechNum" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Tech Name" DataField="TechName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Schedule" DataField="ScheduleFormatted" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->

        <%--  --%>
    </div>
    <asp:HiddenField ID="hfPrimaryName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />

</div>
</asp:Content>
