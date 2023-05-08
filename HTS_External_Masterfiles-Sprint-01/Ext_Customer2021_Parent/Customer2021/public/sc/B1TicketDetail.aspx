<%@ Page Title="Ticket Detail" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="B1TicketDetail.aspx.cs" 
    Inherits="public_sc_B1TicketDetail" %>

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
        .buttonPadding {
            padding-bottom: 15px;
            padding-top: 13px;
            padding-left: 4px;
            padding-right: 4px;
        }
        .imageFormat {
            width: 100%;
            max-width: 90px; 
            padding-bottom: 4px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Ticket Detail 
    &nbsp; <asp:Label ID="lbTitleTicket" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">

        <%--  --%>
    

    <div class="w3-row" style="margin-bottom: 20px;">
        <div class="w3-third w3-container">
          <h4 class="titlePrimary">Customer</h4>
            <table class="tableWithoutLines table1">
                <tr style="vertical-align: top;">
                    <th>Name</th>
                    <td>
                        <asp:Label ID="lbCstName" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Id</th>
                    <td><asp:Label ID="lbCstNum" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Cross Ref</th>
                    <td><asp:Label ID="lbCstCrossRef" runat="server" /></td>
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
                    <th>Contact</th>
                    <td><asp:Label ID="lbCstContact" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Phone</th>
                    <td><asp:Label ID="lbCstPhone" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Requested By</th>
                    <td><asp:Label ID="lbCstRequestedBy" runat="server" /></td>
                </tr>

            </table>

        <h4 class="titlePrimary" style="margin-top: 30px;">Ticket</h4>
            <table class="tableWithoutLines table1">
                <tr style="vertical-align: top;">
                    <th>Ticket Id</th>
                    <td><asp:Label ID="lbTckCenterTicket" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Cross Ref</th>
                    <td><asp:Label ID="lbTckCrossRef" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Issue</th>
                    <td><asp:Label ID="lbTckComment" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Call Type</th>
                    <td><asp:Label ID="lbTckCallType" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Agr Type</th>
                    <td><asp:Label ID="lbTckAgreementType" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Entered</th>
                    <td><asp:Label ID="lbTckStampEntered" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Completed</th>
                    <td><asp:Label ID="lbTckStampCompleted" runat="server" /></td>
                </tr>

                <asp:Panel ID="pnServrightTicketDetail" runat="server">
                <tr style="vertical-align: top;">
                    <th>Req ETA</th>
                    <td><asp:Label ID="lbTckStampEta" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Call Mgr</th>
                    <td><asp:Label ID="lbTckCallManager" runat="server" /></td>
                </tr>
                <tr style="vertical-align: top;">
                    <th>Senior Eng</th>
                    <td><asp:Label ID="lbTckSeniorEngineer" runat="server" /></td>
                </tr>
                </asp:Panel>

                <%-- 
                <tr>
                    <th>Closed</th>
                    <td><asp:Label ID="lbTckStampClosed" runat="server" /></td>
                </tr>
                --%>
            </table>
            <div class="spacer10"></div>
        </div>

        <div class="w3-twothird w3-container">
          
            <div style="display:block; float:left; padding-right:30px;"><asp:Button ID="btToggleNoteEntry" runat="server" OnClick="btToggleNoteEntry_Click" /></div>
            <div style="display:block; float:left; padding-right: 30px;"><asp:Button ID="btToggleFileEntry" runat="server" OnClick="btToggleFileEntry_Click" /></div>
            <div style="display:block; float:left; padding-right: 30px;"><asp:Button ID="btToggleStampEntry" runat="server" OnClick="btToggleStampEntry_Click" /></div>
            <div style="display:block; float:left; padding-right: 30px;"><asp:Button ID="btToggleTicketEntry" runat="server" OnClick="btToggleTicketEntry_Click" /></div>

            <div style="display:block; float:left;">
                <div class="spacer10"></div>
                <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
            </div>
            <!-- <div class="spacer5"></div> -->
            <div class="spacer10"></div>

            <!-- BOX: ADD NOTE ======================================================================================= -->

            <asp:Panel ID="pnAddNote" runat="server" Visible="false">
                <table class="tableBorder tableWithoutLines">
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">Subject</td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txAddNoteSubject" runat="server" TextMode="MultiLine" Width="300" Height="50" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td  style="padding-left: 15px; padding-right: 15px;">Message
                            <asp:Panel ID="pnNoteType" runat="server" Visible="false">
                                <div class="spacer20"></div>
                                <asp:DropDownList ID="ddNoteType" runat="server">
                                    <asp:ListItem Text="FST Internal Note" Value="FST" />
                                    <asp:ListItem Text="Work Done Note" Value="WORK" />
                                </asp:DropDownList>
                            </asp:Panel>
                            <div class="spacer20"></div>
                            <asp:Button ID="btAddNoteSubmit" runat="server" Text="Submit" OnClick="btAddNoteSubmit_Click" />
                        </td>
                        <td style="padding-left: 15px; padding-right: 15px;">
                            <asp:TextBox ID="txAddNoteMessage" runat="server" TextMode="MultiLine" Width="300" Height="175" />
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td></td>
                        <td style="font-style: italic; font-size: 11px; max-width: 300px; padding-bottom: 15px; padding-left: 15px; padding-right: 15px;">Urgent comments/notes should be routed through our contact center (800.228.3628)</td>
                    </tr>
                </table>
            </asp:Panel>

            <!-- BOX: ADD FILE ======================================================================================= -->

            <asp:Panel ID="pnAddFile" runat="server" Visible="false">
                <table class="tableBorder tableWithoutLines">
                    <tr style="vertical-align: top;">
                        <td colspan="2" style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">
                            <asp:FileUpload ID="fuFile" runat="server" />
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">Description</td>
                        <td style="padding-right:15px;"><asp:TextBox ID="txFileDescription" Width="300" runat="server" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px; padding-bottom: 10px;"><asp:Button ID="btAddFileSubmit" runat="server" Text="Save File" OnClick="btAddFileSubmit_Click" /></td>
                        <td><asp:Label ID="lbAddFileResult" runat="server" SkinID="labelError" /></td>
                    </tr>
                </table>
            </asp:Panel>

            <!-- BOX: ADD TIMESTAMP ======================================================================================= -->

            <asp:Panel ID="pnAddStamp" runat="server" Visible="false">
                <table class="tableBorder tableWithoutLines">
                    <tr style="vertical-align: top;">
                        <td colspan="2" style="padding-left: 15px; padding-top: 10px;">
                            <asp:Label ID="lbTimestampAddOrEdit" runat="server" Text="Add New Timestamp" Font-Bold="true" />
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">Timestamp Type
                            <asp:HiddenField ID="hfAddStampEditKeys" runat="server" />
                        </td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">
                                <asp:DropDownList ID="ddAddStampType" runat="server">
                                    <asp:ListItem Text="" Value="" />
                                    <asp:ListItem Text="Technician Notified" Value="N" />
                                    <asp:ListItem Text="Technician En-Route" Value="T" />
                                    <asp:ListItem Text="Technician Onsite" Value="S" />
                                    <asp:ListItem Text="Work Done-Awaits Closure" Value="C" />
                                    <asp:ListItem Text="Hold: Incomplete" Value="IN" />
                                    <asp:ListItem Text="Hold: Never Arrived" Value="NA" />
                                    <asp:ListItem Text="Hold: Part Ordered" Value="WP" />
                                    <asp:ListItem Text="Hold: Repair Being Tested" Value="TE" />
                                </asp:DropDownList>
			        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">Timestamp Date</td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txAddStampDate" runat="server" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">Time (24 HH.MM as CST)</td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txAddStampTime" runat="server" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">For Employee Num</td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txAddStampEmp" runat="server" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td  style="padding-left: 15px; padding-right: 15px;">
                            <asp:Button ID="btTimestampEntrySubmit" runat="server" Text="Submit" OnClick="btTimestampEntrySubmit_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <!-- BOX: ADD FILE ======================================================================================= -->
            <!-- 
                 class="tableBorder tableWithoutLines"
                 style="padding-top: 30px; border: 3px solid #dddddd; background-color: oldlace;"
                <div style="border: 1px solid #dddddd; padding:15px;">
                </div>
                -->
            <asp:Panel ID="pnUpdTicket" runat="server" Visible="false">
                <div class="spacer10"></div>
                <table class="tableBorder tableWithoutLines" style="background-color: #f5f5f5;">
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 20px; padding-right: 15px; padding-top: 20px;">Travel Mileage <span style="color:#ad0034;">*</span></td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 20px;"><asp:TextBox ID="txUpdTicketMiles" runat="server" Width="70" /></td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="padding-left: 20px; padding-right: 15px; padding-top: 10px;">Device Life Count <span style="color:#ad0034;">*</span></td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txUpdTicketLifecount" runat="server" Width="100" /></td>
                    </tr>

                    <tr style="vertical-align: top;">
                        <td style="padding-left: 20px; padding-right: 15px; padding-top: 10px;">Tracking? (Parts Returned)</td>
                        <td style="padding-left: 15px; padding-right: 15px; padding-top: 10px;"><asp:TextBox ID="txUpdTicketTracking" runat="server" Width="230" /></td>
                    </tr>

                    <tr style="vertical-align: top;">
                        <td  style="padding-left: 20px; padding-right: 15px; padding-bottom: 15px;">
                            <asp:Button ID="btUpdTicketSubmit" runat="server" Text="Submit" OnClick="btUpdTicketSubmit_Click" SkinID="buttonPrimary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>


        <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">


            
            
            <asp:Panel ID="pn_B1TimestampSmall" runat="server">
            <h4 class="titleSecondary">Timestamps</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1TimestampSmall" runat="server">
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
                        <td>
                                <asp:Label ID="lbTimestampFormattedA" runat="server" Text='<%# Eval("TimestampFormatted") %>'  />
                                <asp:LinkButton ID="lkTimestampFormattedA" runat="server" 
                                    OnClick="lkLoadTimestampForEdit_Click" 
                                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") + "|" + Eval("StatusCode") + "|" + Eval("ReasonCode") + "|" + Eval("StampDate") + "|" + Eval("StampTime") + "|" + Eval("TechNum") + "|" + Eval("Source") %>' 
                                    Text='<%# "[edit?] " + Eval("TimestampFormatted") %>' />
                        </td>
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
                        <td>
                            <asp:Label ID="lbTimestampFormattedB" runat="server" Text='<%# Eval("TimestampFormatted") %>'  />
                            <asp:LinkButton ID="lkTimestampFormattedB" runat="server" 
                                OnClick="lkLoadTimestampForEdit_Click" 
                                CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") + "|" + Eval("StatusCode") + "|" + Eval("ReasonCode") + "|" + Eval("StampDate") + "|" + Eval("StampTime") + "|" + Eval("TechNum") + "|" + Eval("Source") %>' 
                                Text='<%# "[edit?] " + Eval("TimestampFormatted") %>' />
                        </td>
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
                <div class="spacer15"></div>
            </asp:Panel>
            <!-- -->


            <asp:Panel ID="pn_B1ModelSmall" runat="server">

            <h4 class="titleSecondary">Model</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1ModelSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Model</td>
                        <td><asp:Label ID="lbModelName" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbModelDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbModelSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model Xref</td>
                        <td><asp:Label ID="lbModelXref" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model Source</td>
                        <td><asp:Label ID="lbModelSource" runat="server" Text='<% #Eval("ModelSource") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td><asp:Label ID="lbModelName" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbModelDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbModelSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model Xref</td>
                        <td><asp:Label ID="lbModelXref" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model Source</td>
                        <td><asp:Label ID="lbModelSource" runat="server" Text='<% #Eval("ModelSource") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                <div class="spacer15"></div>
            </asp:Panel>
            <!-- -->

                
            <asp:Panel ID="pn_B1TravelSmall" runat="server">
            <!-- -->
                <h4 class="titleSecondary">Travel Labor</h4>
            <asp:Repeater ID="rp_B1TravelSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Tech</td>
                        <td><asp:Label ID="lbTrvTech" runat="server" Text='<% #Eval("EmpNum") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbTrvTechName" runat="server" Text='<% #Eval("EmpName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="tbTrvDate" runat="server" Text='<% #Eval("LaborDateFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Start (CST)</td>
                        <td><asp:Label ID="tbTrvStart" runat="server" Text='<% #Eval("LaborStartTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>End (CST)</td>
                        <td><asp:Label ID="tbTrvEnd" runat="server" Text='<% #Eval("LaborEndTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Duration</td>
                        <td><asp:Label ID="lbTrvDuration" runat="server" Text='<% #Eval("LaborDurationFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Tech</td>
                        <td><asp:Label ID="lbTrvTech" runat="server" Text='<% #Eval("EmpNum") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbTrvTechName" runat="server" Text='<% #Eval("EmpName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbTrvDate" runat="server" Text='<% #Eval("LaborDateFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Start (CST)</td>
                        <td><asp:Label ID="lbTrvStart" runat="server" Text='<% #Eval("LaborStartTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>End (CST)</td>
                        <td><asp:Label ID="lbTrvEnd" runat="server" Text='<% #Eval("LaborEndTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Duration</td>
                        <td><asp:Label ID="lbTrvDuration" runat="server" Text='<% #Eval("LaborDurationFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                <div class="spacer15"></div>
                </asp:Panel>
            <!-- -->


            <asp:Panel ID="pn_B1OnsiteSmall" runat="server">

            <h4 class="titleSecondary">Onsite Labor</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1OnsiteSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Tech</td>
                        <td><asp:Label ID="lbOnsTech" runat="server" Text='<% #Eval("EmpNum") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbOnsTechName" runat="server" Text='<% #Eval("EmpName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="lbOnsDate" runat="server" Text='<% #Eval("LaborDateFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Start (CST)</td>
                        <td><asp:Label ID="lbOnsStart" runat="server" Text='<% #Eval("LaborStartTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>End (CST)</td>
                        <td><asp:Label ID="lbOnsEnd" runat="server" Text='<% #Eval("LaborEndTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Duration</td>
                        <td><asp:Label ID="lbOnsDuration" runat="server" Text='<% #Eval("LaborDurationFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Tech</td>
                        <td><asp:Label ID="lbOnsTech" runat="server" Text='<% #Eval("EmpNum") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tech Name</td>
                        <td><asp:Label ID="lbOnsTechName" runat="server" Text='<% #Eval("EmpName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbOnsDate" runat="server" Text='<% #Eval("LaborDateFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Start (CST)</td>
                        <td><asp:Label ID="lbOnsStart" runat="server" Text='<% #Eval("LaborStartTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>End (CST)</td>
                        <td><asp:Label ID="lbOnsEnd" runat="server" Text='<% #Eval("LaborEndTimeFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Duration</td>
                        <td><asp:Label ID="lbOnsDuration" runat="server" Text='<% #Eval("LaborDurationFormatted") %>' /></td>
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
                <div class="spacer15"></div>
            </asp:Panel>


            <asp:Panel ID="pn_B1PartUseSmall" runat="server">

            <h4 class="titleSecondary">Parts Used</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1PartUseSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbPrtName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbPrtDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbPrtSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Quantity</td>
                        <td><asp:Label ID="lbPrtQuantity" runat="server" Text='<% #Eval("Qty") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbPrtName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbPrtDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbPrtSerial" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Quantity</td>
                        <td><asp:Label ID="lbPrtQuantity" runat="server" Text='<% #Eval("Qty") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                <div class="spacer15"></div>
                </asp:Panel>
            <!-- -->
            

            <asp:Panel ID="pn_B1PartsShippedSmall" runat="server">

            <h4 class="titleSecondary">Parts Shipped</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1PartsShippedSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Transfer</td>
                        <td><asp:Label ID="lbShpTransfer" runat="server" Text='<% #Eval("TransferId") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Line</td>
                        <td><asp:Label ID="lbShpLine" runat="server" Text='<% #Eval("PTISEQ") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbShpName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Part Xref</td>
                        <td><asp:Label ID="lbShpServright" runat="server" Text='<% #Eval("SRPART") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbShpDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Order Qty</td>
                        <td><asp:Label ID="lbShpQtyOrdered" runat="server" Text='<% #Eval("PTIORQ") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Backorder Qty</td>
                        <td><asp:Label ID="lbShpQtyBackordered" runat="server" Text='<% #Eval("PTIBKQ") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="lbShpDate" runat="server" Text='<% #Eval("DisplayDate") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Transfer</td>
                        <td><asp:Label ID="lbShpTransfer" runat="server" Text='<% #Eval("TransferId") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Line</td>
                        <td><asp:Label ID="lbShpLine" runat="server" Text='<% #Eval("PTISEQ") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbShpName" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Part Xref</td>
                        <td><asp:Label ID="lbShpServright" runat="server" Text='<% #Eval("SRPART") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbShpDescription" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Order Qty</td>
                        <td><asp:Label ID="lbShpQtyOrdered" runat="server" Text='<% #Eval("PTIORQ") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Backorder Qty</td>
                        <td><asp:Label ID="lbShpQtyBackordered" runat="server" Text='<% #Eval("PTIBKQ") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbShpDate" runat="server" Text='<% #Eval("DisplayDate") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                <div class="spacer15"></div>
                </asp:Panel>
            <!-- -->

            <asp:Panel ID="pn_B1TrackingSmall" runat="server">
            <h4 class="titleSecondary">Part Tracking</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1TrackingSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbTrkPartA" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbTrkDescriptionA" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tracking</td>
                        <td><asp:Label ID="lbTrkNumberA" runat="server" Text='<% #Eval("TrackingLink") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="lbTrkDateA" runat="server" Text='<% #Eval("DisplayDate") %>' /></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbTrkPartB" runat="server" Text='<% #Eval("Part") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbTrkDescriptionB" runat="server" Text='<% #Eval("Description") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tracking</td>
                        <td><asp:Label ID="LbTrkNumberB" runat="server" Text='<% #Eval("TrackingLink") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbTrkDateB" runat="server" Text='<% #Eval("DisplayDate") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
    </asp:Panel>


     <asp:Panel ID="pn_B1TrackingPbSmall" runat="server">
            <h4 class="titleSecondary">Shipment Tracking</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1TrackingPbSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Carrier</td>
                        <td><asp:Label ID="lbTrkCarrier" runat="server" Text='<% #Eval("Carrier") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tracking</td>
                        <td><asp:Label ID="lbTrkNumber" runat="server" Text='<% #Eval("TrackingLink") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Weight</td>
                        <td><asp:Label ID="lbTrkWeight" runat="server" Text='<% #Eval("Weight") %>' /></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Carrier</td>
                        <td><asp:Label ID="lbTrkCarrier" runat="server" Text='<% #Eval("Carrier") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tracking</td>
                        <td><asp:Label ID="lbTrkNumber" runat="server" Text='<% #Eval("TrackingLink") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Weight</td>
                        <td><asp:Label ID="lbTrkWeight" runat="server" Text='<% #Eval("Weight") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
    </asp:Panel>


      <!-- Return Shipping tracking number/link -->
           <asp:Panel ID="pn_B1ReturnLabelSmall" runat="server">
            <h4 class="titleSecondary">Return Label Tracking</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1ReturnLabelSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg"> 
                        <td>Tracking</td>
                         <td><asp:HyperLink ID="ReturnLabel" runat="server" Text='<%# Eval("Tracking") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("Tracking") + "&cntry_code=us" %>'/></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbRtnSrl" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    
                    <tr class="trColorAlt">
                     <td>Tracking</td>
                            <td><asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Tracking") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("Tracking") + "&cntry_code=us" %>'/></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <!-- -->
            <div class="spacer15"></div>
            </asp:Panel>
            <!-- end of Return label Small -->

            <asp:Panel ID="pn_B1NotesSmall" runat="server">
            <h4 class="titleSecondary">Notes</h4>
            <!-- -->
            <asp:Repeater ID="rp_B1NotesSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Time</td>
                        <td><asp:Label ID="lbNotTime" runat="server" Text='<% #Eval("TimestampFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Subject</td>
                        <td><asp:Label ID="lbNotSubject" runat="server" Text='<% #Eval("Subject") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Message</td>
                        <td><asp:Label ID="lbNotMessage" runat="server" Text='<% #Eval("Message") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Creator</td>
                        <td><asp:Label ID="lbNotAuthor" runat="server" Text='<% #Eval("Author") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Type</td>
                        <td><asp:Label ID="lbNotFormat" runat="server" Text='<% #Eval("Format") %>' /></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Time</td>
                        <td><asp:Label ID="lbNotTime" runat="server" Text='<% #Eval("TimestampFormatted") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Subject</td>
                        <td><asp:Label ID="lbNotSubject" runat="server" Text='<% #Eval("Subject") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Message</td>
                        <td><asp:Label ID="lbNotMessage" runat="server" Text='<% #Eval("Message") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Creator</td>
                        <td><asp:Label ID="lbNotAuthor" runat="server" Text='<% #Eval("Author") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Type</td>
                        <td><asp:Label ID="lbNotFormat" runat="server" Text='<% #Eval("Format") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="spacer15"></div>
            </asp:Panel>
            <!-- -->
            

        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
                <div class="spacer15"></div>
      <asp:Panel ID="pn_B1TimestampLarge" runat="server">
            <h4 class="titleSecondary">Timestamps</h4>
            <!-- -->
                <asp:GridView ID="gv_B1TimestampLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                         <asp:BoundField HeaderText="SchedDate" DataField="TIMDST" ItemStyle-HorizontalAlign="Left" visible="false" />
                        <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-HorizontalAlign="Left" />

                        <asp:TemplateField HeaderText="Time" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lbTimestampFormatted" runat="server" Text='<%# Eval("TimestampFormatted") %>'  />
                                <asp:LinkButton ID="lkTimestampFormatted" runat="server" 
                                    OnClick="lkLoadTimestampForEdit_Click" 
                                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") + "|" + Eval("StatusCode") + "|" + Eval("ReasonCode") + "|" + Eval("StampDate") + "|" + Eval("StampTime") + "|" + Eval("TechNum") + "|" + Eval("Source") %>' 
                                    Text='<%#  "[edit?] " + Eval("TimestampFormatted") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Text='<%# Eval("TimestampFormatted") + " (edit?)" %>' --%>
                        <asp:BoundField HeaderText="Tech" DataField="TechNum" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Tech Name" DataField="TechName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Scheduled" DataField="ScheduleFormatted" ItemStyle-HorizontalAlign="Center" />             
                    </Columns>
                </asp:GridView>
          <div class="spacer15"></div>
        </asp:Panel>
  
        
    <!----->

            <!--  ==================================================================== -->        

            <asp:Panel ID="pn_B1ModelLarge" runat="server">

            <h4 class="titleSecondary">Model</h4>
            <!-- -->
                <asp:GridView ID="gv_B1ModelLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Model" DataField="Model" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Serial" DataField="Serial" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Model Xref" DataField="ModelXref" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Model Source" DataField="ModelSource" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
            <div class="spacer15"></div>
            <!-- -->
            </asp:Panel>
         <!-- -->
         <!-- -->
         <!-- -->
               
    <!--  ==================================================================== -->        

            <asp:Panel ID="pn_B1TravelLarge" runat="server">
            <h4 class="titleSecondary">Travel Labor</h4>
            <!-- -->
                <asp:GridView ID="gv_B1TravelLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Tech" DataField="EmpNum" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Tech Name" DataField="EmpName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Date" DataField="LaborDateFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Start (CST)" DataField="LaborStartTimeFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="End (CST)" DataField="LaborEndTimeFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Duration" DataField="LaborDurationFormatted" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                        <!-- -->
                </asp:Panel>

            <asp:Panel ID="pn_B1OnsiteLarge" runat="server">
            <h4 class="titleSecondary">Onsite Labor</h4>
            <!-- -->
                <asp:GridView ID="gv_B1OnsiteLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No matching records were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Tech" DataField="EmpNum" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Tech Name" DataField="EmpName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Date" DataField="LaborDateFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Start (CST)" DataField="LaborStartTimeFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="End (CST)" DataField="LaborEndTimeFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Duration" DataField="LaborDurationFormatted" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>

            <asp:Panel ID="pn_B1PartUseLarge" runat="server">
            <h4 class="titleSecondary">Parts Used</h4>
            <!-- -->
                <asp:GridView ID="gv_B1PartUseLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No part use was found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Part" DataField="Part" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Serial" DataField="Serial" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>

            <asp:Panel ID="pn_B1PartsShippedLarge" runat="server">
            <h4 class="titleSecondary">Parts Shipped</h4>
            <!-- -->
                <asp:GridView ID="gv_B1PartsShippedLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No part shipments were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Transfer" DataField="TransferId" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Line" DataField="PTISEQ" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Part" DataField="Part" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Xref" DataField="SrPart" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Ord Q" DataField="PTIORQ" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BkOr Q" DataField="PTIBKQ" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Date" DataField="DisplayDate" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
            </asp:Panel>

            <asp:Panel ID="pn_B1TrackingLarge" runat="server">
            <h4 class="titleSecondary">Part Tracking</h4>
            <!-- -->
                <asp:GridView ID="gv_B1TrackingLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No tracking information was found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Part" DataField="Part" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Tracking">
                            <ItemTemplate>
                                <asp:Label ID="lbTrackingNumber" runat="server" Text='<%# Eval("TrackingLink") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Date" DataField="DisplayDate" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>

            <%-- Pitney Bowes Shipments --%>
            <asp:Panel ID="pn_B1TrackingPbLarge" runat="server">
            <h4 class="titleSecondary">Shipment Tracking</h4>
            <!-- -->
                <asp:GridView ID="gv_B1TrackingPbLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No tracking information was found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Carrier" DataField="Carrier" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Tracking" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lbTrackingNumberPb" runat="server" Text='<%# Eval("TrackingLink") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Weight" DataField="Weight" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>


            <%-- Return Label --%>
            <asp:Panel ID="pn_B1ReturnLabelLarge" runat="server">
            <h4 class="titleSecondary">Return Label Tracking</h4>
            <!-- -->
                <asp:GridView ID="gv_B1ReturnLabelLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No Return Label information was found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns> 
                      <asp:TemplateField HeaderText="Tracking">
                        <ItemTemplate>
                             <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Tracking") %>' NavigateUrl='<%# "Https://www.fedex.com/fedextrack/?action=track&tracknumbers=" + Eval("Tracking") + "&cntry_code=us" %>'/>                            
                        </ItemTemplate>
                      </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Serial No" DataField="Serial" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>

            <asp:Panel ID="pn_B1NotesLarge" runat="server">
                <h4 class="titleSecondary">Notes</h4>
            <!-- -->
                <asp:GridView ID="gv_B1NotesLarge" runat="server"
                    AutoGenerateColumns="False" 
                    CssClass="tableWithLines"
                    EmptyDataText="No notes were found">
                    <AlternatingRowStyle CssClass="trColorAlt" />
                    <Columns>
                        <asp:BoundField HeaderText="Time" DataField="TimestampFormatted" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Subject" DataField="Subject" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Message">
	                        <ItemTemplate>
                                <asp:Label ID="lbNotesLargeMessage" runat="server" Text='<%# Eval("Message") %>' ></asp:Label>
	                        </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField HeaderText="Creator" DataField="Author" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Format" DataField="Format" ItemStyle-HorizontalAlign="Center" />

                    </Columns>
                </asp:GridView>
                <div class="spacer15"></div>
                </asp:Panel>
            


        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
        
         <asp:Panel ID="pnDisplayFiles" runat="server">

        
         <asp:Panel ID="pn_B1File" runat="server">

            <h4 class="titleSecondary">Files</h4>
            <!-- -->

            <asp:GridView ID="gv_Files" runat="server"
                AutoGenerateColumns="False" 
                CssClass="tableWithLines"
                EmptyDataText="No attached files were found">
                <AlternatingRowStyle CssClass="trColorAlt" />
                <Columns>
                    <asp:TemplateField HeaderText="Download Copy" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkAreaItemFileLink" runat="server" 
                                OnClick="lkFileDownload_Click" 
                                CommandArgument='<%  # Eval("flId") + "|" + Eval("flFileType") + "|" + Eval("flFileName") + "|" + Eval("flFileExtension") %>'
                                Text='<%# Eval("flFileName") %>' 
                                />
                        </ItemTemplate>
                    </asp:TemplateField>
          <%--  
              <asp:BoundField HeaderText="Ext" DataField="flFileExtension" ItemStyle-HorizontalAlign="Center" />
              --%>
                    
                    <asp:BoundField HeaderText="Type" DataField="flFileType" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Description" DataField="flFileDescription" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Creator" DataField="flCreatorEmployeeNum" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="View Image" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfFileTypeForLink" runat="server" Value='<%# Eval("flFileType") %>' />
                            <asp:HyperLink ID="hlAreaItemImage" runat="server" 
                                Target="_Image" 
                                Text="View"
                                NavigateUrl='<% #"~/public/ImageDisplay.aspx?id=" + Eval("flId") + "&grp=" + Eval("flAreaWhereUsed") + "&item=" +  Eval("wuAreaItemId") %>'  
                                    >
                                <%--  --%>
                                <%--  
                                    <asp:Image ID="imAreaItemImage" runat="server" ImageUrl='<% #Eval("Base64ForLink") + "" + Eval("Base64AsString") %>' CssClass="imageFormat" />
                                    --%>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
             <!-- -->
                <div class="spacer15"></div>
                </asp:Panel>
            </asp:Panel>
            <!-- -->

        </div> <!-- END: RIGHT SIDE COLUMN ======================================================================================= -->
    </div>

<div class="spacer30"></div>

        <%--  --%>
    </div>
        <asp:HiddenField ID="hfCenter" runat="server" />
        <asp:HiddenField ID="hfTicket" runat="server" />
        <asp:HiddenField ID="hfUserName" runat="server" />
        <asp:HiddenField ID="hfDeleteIsAuthorized" runat="server" Visible="false" />
        <asp:HiddenField ID="hfNoteCenter" runat="server" />
        <asp:HiddenField ID="hfNoteTicket" runat="server" />
        <asp:HiddenField ID="hfTicketId" runat="server" />
        <asp:HiddenField ID="hfStsNum" runat="server" />
        <asp:HiddenField ID="hfUserAccountType" runat="server" Value="" Visible="false" /> <%-- REG, LRG, DLR, BAK, SRG, SRP, SRC --%>
        <asp:HiddenField ID="hfUserIsOmahaAdmin" runat="server" Value="" Visible="false" /> 
        <asp:HiddenField ID="hfCallIsClosed" runat="server" Value="" Visible="false" /> 

        <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
        <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />


     <!-- Timestamp hidden fields -->   
                <%-- 
  <asp:HiddenField ID="hfStatus" runat="server" />
  <asp:HiddenField ID="hfRefDate" runat="server" />
  <asp:HiddenField ID="hfRefTime" runat="server" />
  <asp:HiddenField ID="hfSchdDate" runat="server" />
  <asp:HiddenField ID="hfSchdTime" runat="server" />
    <asp:HiddenField ID="hfTech" runat="server" />
  <asp:HiddenField ID="hfReason" runat="server" />
                   
                    --%>

</div>
</asp:Content>
