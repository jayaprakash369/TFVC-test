<%@ Page Title="Service Request" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ServiceRequest.aspx.cs" 
    Inherits="private_sc_ServiceRequest" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
            <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .WrapPanelLabelElements {
            display: inline-block;
            float: left;
            padding-right: 40px;
            padding-bottom: 5px;
            width: 500px;
        }
        .WrapPanelDataElements {
            display: inline-block;
            float: left;
            padding-right: 10px;
            padding-bottom: 20px;
            width: 400px;
        }
        .UpdateElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
            vertical-align: bottom;
        }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Service Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">

        <asp:Label ID="lbMsg" runat="server" SkinId="labelMessage" Visible="false" />

        <%-- START: PANEL SELECTED --%>
        <asp:Panel ID="pnSelected" runat="server" Visible="false" >
            <table style="border: 1px solid #dddddd; padding: 5px; margin-bottom: 15px;">
                <tr>
                    <td style="padding-left: 8px; padding-right: 8px; padding-top: 5px; padding-bottom: 5px;">
                        <table>
                            <asp:Panel ID="pnSelectedLocation" runat="server" Visible="false" >
                                <tr style="vertical-align: top;">
                                    <td style="padding: 3px; padding-left: 13px;"><span style="position:relative; left:-10px"><b>Location: </b></span><br /><asp:Label ID="lbSelectedLocation" runat="server" /></td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnSelectedContact" runat="server" Visible="false" >
                                <tr style="vertical-align: top;">
                                    <td style="padding: 3px; padding-left: 13px;"><span style="position:relative; left:-10px"><b>Contact: </b></span><br /><asp:Label ID="lbSelectedContact" runat="server" /></td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnSelectedContactEntry" runat="server" Visible="false" >
                                <tr style="vertical-align: top;">
                                    <td style="padding: 3px; padding-left: 13px;"><span style="position:relative; left:-10px"><b>Contact: </b></span></td>
                                </tr>
                                <tr style="vertical-align: top;">
                                    <td style="padding: 3px; padding-left: 13px;">
                                        <table class="tableBorderBackgroundLight">
                                            <tr>
                                                <td style="padding: 10px;">
                                        <div class="SearchPanelElements">
                                            Contact Name&nbsp;&nbsp;<span style="color: crimson;">*</span><br />
                                            <asp:TextBox ID="txProblemContactName" runat="server" Width="175" />
                                        </div>
                                        <div class="SearchPanelElements">
                                            Contact Phone&nbsp;&nbsp;<span style="color: crimson;">*</span><br />
                                                <asp:TextBox ID="txProblemContactPhone" runat="server" Width="125" />
                                        </div>
                                        <div class="SearchPanelElements">
                                            Ext<br />
                                            <asp:TextBox ID="txProblemContactExtension" runat="server" Width="60" />
                                        </div>
                                        <div class="SearchPanelElements">
                                            Email For Acknowledgement<br />
                                                <asp:TextBox ID="txProblemEmailAcknowledgement" runat="server" Width="250" />
                                        </div>
                                        <div class="SearchPanelElements">
                                            Name Of Ticket Requestor<br />
                                                <asp:TextBox ID="txProblemTicketCreator" runat="server" Width="175" />
                                        </div>
                                        <div class="SearchPanelElements">
                                            <asp:Label ID="lbProblemContactError" runat="server" Text="" SkinID="labelError" />
                                        </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnSelectedEquipment" runat="server" Visible="false">
                                <tr style="vertical-align: top;">
                                    <!-- <td style="padding: 3px; padding-right: 15px;"><b>Equipment: </b></td> -->
                                    <td style="padding: 3px; padding-left: 13px;"><span style="position:relative; left:-10px"><b>Equipment: </b></span><br /><asp:Label ID="lbSelectedEquipment" runat="server" /></td>
                                </tr>
                            </asp:Panel>
                        </table>

                    </td>
                </tr>
            </table>

        </asp:Panel>
        <%-- END: PANEL SELECTED --%>

        <%-- START: PANEL LOCATION --%>
       <asp:Panel ID="pnLocation" runat="server" Visible="false" >

        <!-- START: SEARCH PANEL (LOCATION) ======================================================================================= -->
        <asp:Panel ID="pnSearchLocation" runat="server" DefaultButton="btSearchLocationSubmit">
            

            <div class="titlePrimary SearchPanelElements">
                Select Location
            </div>
            <div class="SearchPanelElements" style="padding-top:4px; font-size: 0.9em;">
                 (or if the item's location is not certain, you may <asp:LinkButton ID="lkSearchBySerial" runat="server" Text="search globally" OnClick="lkSearchBySerial_Click" /> by serial or cross reference)
            </div>
            <div class="spacer0"></div>

            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Name<br />
                    <asp:TextBox ID="txSearchLocationName" runat="server" Width="175" />
                </div>
                <asp:Panel ID="pnSearchCustomerFamily" runat="server">
                    <div class="SearchPanelElements">
                        Customer<br />
                        <asp:DropDownList ID="ddSearchCustomerFamily" runat="server" />
                    </div>
                </asp:Panel>
                <div class="SearchPanelElements">
                    Location<br />
                        <asp:TextBox ID="txSearchLocationNum" runat="server" Width="40" />
                </div>
                <div class="SearchPanelElements">
                    Cust Xref<br />
                        <asp:TextBox ID="txSearchLocationXref" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Address<br />
                        <asp:TextBox ID="txSearchLocationAddress" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    City<br />
                        <asp:TextBox ID="txSearchLocationCity" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    State<br />
                        <asp:TextBox ID="txSearchLocationState" runat="server" Width="30" />
                </div>
                <div class="SearchPanelElements">
                    Zip<br />
                        <asp:TextBox ID="txSearchLocationZip" runat="server" Width="50" />
                </div>
                <div class="SearchPanelElements">
                    Phone<br />
                        <asp:TextBox ID="txSearchLocationPhone" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchLocationSubmit" runat="server" Text="Search" OnClick="btSearchLocationSubmit_Click" SkinID="buttonPrimary" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchLocationClear" runat="server" Text="Clear" OnClick="btSearchLocationClear_Click"  />
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>


        <!-- START: SMALL SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_LocationSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Name</td>
                        <td>
                            <asp:LinkButton ID="lkLocationName" runat="server" OnClick="lkLocationName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") %>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Qty</td>
                        <td><asp:Label ID="Label14" runat="server" Text='<% #Eval("CombinedEqpCount") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("CSTRNR") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="Label3" runat="server" Text='<% #Eval("CSTRCD") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>CustXref</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("XREFCS") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Edit Xref</td>
                        <td></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Address</td>
                        <td><asp:Label ID="Label9" runat="server" Text='<% #Eval("SADDR1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="Label10" runat="server" Text='<% #Eval("CITY") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="Label11" runat="server" Text='<% #Eval("STATE") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Zip</td>
                        <td><asp:Label ID="Label12" runat="server" Text='<% #Eval("ZIPCD") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Phone</td>
                        <td><asp:Label ID="Label13" runat="server" Text='<% #Eval("PhoneFormatted") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Name</td>
                        <td>
                            <asp:LinkButton ID="lkLocationName" runat="server" OnClick="lkLocationName_Click" 
                                CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD")%>'>
                                <%# Eval("CUSTNM") %>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Qty</td>
                        <td><asp:Label ID="Label20" runat="server" Text='<% #Eval("CombinedEqpCount") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td><asp:Label ID="Label6" runat="server" Text='<% #Eval("CSTRNR") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td><asp:Label ID="Label7" runat="server" Text='<% #Eval("CSTRCD") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>CustXref</td>
                        <td><asp:Label ID="Label8" runat="server" Text='<% #Eval("XREFCS") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Edit Xref</td>
                        <td></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Address</td>
                        <td><asp:Label ID="Label15" runat="server" Text='<% #Eval("SADDR1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="Label16" runat="server" Text='<% #Eval("CITY") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="Label17" runat="server" Text='<% #Eval("STATE") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Zip</td>
                        <td><asp:Label ID="Label18" runat="server" Text='<% #Eval("ZIPCD") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Phone</td>
                        <td><asp:Label ID="Label19" runat="server" Text='<% #Eval("PhoneFormatted") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (LOCATION) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->

    <asp:GridView ID="gv_LocationLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_Loc"
        AllowPaging="true" 
        OnPageIndexChanging="gvPageIndexChanging_Loc"
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:TemplateField HeaderText="Name" SortExpression="CombinedEqpCountSort" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="hfCompanySegment" runat="server" Visible="false" Value='<%# Eval("FLAG8") %>' />
                    <asp:HiddenField ID="hfB1EqpCount" runat="server" Visible="false" Value='<%# Eval("B1EqpCount") %>' />
                    <asp:LinkButton ID="lkLocationName" runat="server" 
                        OnClick="lkLocationName_Click" 
                        CommandArgument='<%# Eval("CSTRNR") + "|" + Eval("CSTRCD") %>'
                        Text='<%# Eval("CUSTNM")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Equip" SortExpression="CombinedEqpCount" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbEquipmentCount" runat="server" Text='<%# Eval("CombinedEqpCount") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Cust" DataField="CSTRNR" SortExpression="CSTRNR" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CSTRCD" SortExpression="CSTRCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Cust Xref" DataField="XREFCS" SortExpression="XREFCS" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="SADDR1" SortExpression="SADDR1" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="CITY" SortExpression="CITY" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="ST" DataField="STATE" SortExpression="STATE" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Zip" DataField="ZIPCD" SortExpression="ZIPCD" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="PhoneFormatted" SortExpression="PhoneFormatted" ItemStyle-HorizontalAlign="Center" />

        </Columns>
    </asp:GridView>


    </div>
        <!-- END: LARGE SCREEN TABLE (LOCATION) ======================================================================================= -->

<div class="spacer30"></div>

        </asp:Panel>

                    </asp:Panel>
        <%-- END: PANEL LOCATION --%>


        <%-- START: PANEL CONTACT --%>
                    <%--  --%>
                    <asp:Panel ID="pnContact" runat="server" Visible="false" >
                        <%-- // xxx --%>
                        <div class="SearchPanelElements" style="padding-right: 20px;">
                            <h3 class="titlePrimary">Contact Information</h3>
                        </div>
                        <div class="SearchPanelElements">
                            <asp:Label ID="lbMsgContact" runat="server" SkinID="labelError" />
                        </div>
                        <div class="spacer0"></div>
                        <table>
                            <tr>
                                <td>
                                    <div class="WrapPanelLabelElements">
                                        <asp:Label ID="lbContactNameTitle" runat="server" />
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:TextBox ID="txContactName" runat="server" Width="300" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="WrapPanelLabelElements">
                                        <asp:Label ID="lbContactPhoneTitle" runat="server" />
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:TextBox ID="txCompanyPhone" runat="server" Width="140" /> &nbsp; Ext: &nbsp;
                                        <asp:TextBox ID="txCompanyPhoneExtension" runat="server" Width="60" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    <div class="WrapPanelLabelElements">
                                        How would you like our tech to contact you?
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:DropDownList runat="server" ID="ddContactMethodPreference" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddContactMethodPreference_SelectedIndexChanged">
                                            <asp:ListItem Text="No Preference (phone above will be used)" Value="NON" />
                                            <asp:ListItem Text="By Email (provide address below)" Value="EML" />
                                            <asp:ListItem Text="By Text Message (provide mobile number)" Value="TXT" />
                                            <asp:ListItem Text="By Phone Call (provide preferred number)" Value="PHN" />
                                        </asp:DropDownList>

                                        <div style="clear: both; height: 5px;"></div>
                                        <asp:Label ID="lbContactMethodDetailTitle" runat="server" Visible="false" />&nbsp;
                                        <asp:TextBox ID="txContactMethodDetail" runat="server" Width="280" MaxLength="50" Visible="false" /> <!-- change size from 300 to 230 &nbsp; Ext? &nbsp; -->
                                        &nbsp;<asp:Label ID="lbContactMethodDetailExtentionTitle" runat="server" Visible="false" />&nbsp;<asp:TextBox ID="txContactMethodPhoneExt" runat="server" Width="60" MaxLength="6" Visible="false" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="WrapPanelLabelElements">
                                        Enter person requesting ticket (opt)
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:TextBox ID="txContactRequestorName" runat="server" Width="300" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="WrapPanelLabelElements">
                                        For email acknowledgement, enter email (opt)
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:TextBox ID="txContactEmailAcknowledgement" runat="server" Width="300" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="WrapPanelLabelElements">
                                        <asp:Label ID="lbContactListPickTitle" runat="server"  />
                                        <span style="font-size: 13px; color: #3A7728; padding-left: 10px;">
                                            <br />This option automatically generates a service ticket
                                        </span>
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:RadioButton ID="rbContactListForReg" runat="server" GroupName="rbServiceTypeGroup" Text="Select from your list" Checked="true" SkinId="RadioButton1" />
                                        <div class="spacer5"></div>
                                        <asp:RadioButton ID="rbContactListForPm" runat="server" GroupName="rbServiceTypeGroup" Text="Preventative maintenance request" Visible="false"  SkinId="RadioButton1" />
                                    </div>
                                </td>
                            </tr>
                            <!--  style="background-color: #F5F5F5; border: 1px solid #aaaaaa;"
                                 style="padding:50px;"
                                -->
                            <tr>
                                <td>
                                    
                                    <div class="WrapPanelLabelElements" style="">
                                        <div style="border-top: 1px solid #dddddd; padding-top:10px;">
                                        <span style="font-style: italic; font-size: 0.9em;"><b>OR</b> to manually enter your request, <br />indicate the service type and number of items</span> 
                                        <span style="font-size: 13px; color: #3A7728; padding-left: 10px;">
                                            <br />These options generate an email to our <br />Customer Service Reps -- T&M will be created as a <br />time & materials service ticket
                                        </span>
                                        </div>
                                    </div>
                                     
                                    <div class="WrapPanelDataElements">
                                        <div style="border-top: 1px solid #dddddd; padding-top:10px;">
                                        <asp:RadioButton ID="rbContactManualPayingByContract" runat="server" GroupName="rbServiceTypeGroup" Text="Contract"  SkinId="RadioButton1" />
                                            &nbsp;&nbsp;<asp:RadioButton ID="rbContactManualPayingByTm" runat="server" GroupName="rbServiceTypeGroup" Text="T & M"  SkinId="RadioButton1" />
                                            &nbsp;&nbsp;<asp:DropDownList runat="server" ID="ddContactManualEntryQty" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 10px;">
                                    <div class="WrapPanelLabelElements">
                                    </div>
                                    <div class="WrapPanelDataElements">
                                        <asp:Button ID="btContactSubmit" runat="server" Text="Continue" onClick="btContactSubmit_Click"  SkinID="buttonPrimary" />
                                    </div>
                                </td>
                            </tr>

                        </table>


                    </asp:Panel>
        <%-- END: PANEL CONTACT --%>




        <%-- START: PANEL EQUIPMENT --%>
        <%--  --%>
        <asp:Panel ID="pnEquipment" runat="server" Visible="false" >

        <!-- START: SEARCH PANEL (EQUIPMENT) ======================================================================================= -->
        <asp:Panel ID="pnSearchEquipment" runat="server" DefaultButton="btSearchEquipmentSubmit">
            
            <div class="SearchPanelElements"><h3 class="titlePrimary">Select Units For Service</h3></div>
            <div class="SearchPanelElements">
                <asp:Button ID="btEquipmentSubmit" runat="server" Text="Continue" onClick="btEquipmentSubmit_Click" SkinID="buttonPrimary" />
            </div>
            <div class="SearchPanelElements" style="max-width:500px;"><i>Please do your best to identify the correct unit.  An incorrectly selected piece of equipment can cause a delay in addressing the service issue.</i></div>
            <div class="spacer5"></div>
            <table class="tableBorderBackgroundMedium">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                <asp:Panel ID="pnSearchProductCodes" runat="server" Visible="false">
                    <div class="SearchPanelElements">
                        Category<br />
                        <asp:DropDownList ID="ddSearchEquipmentCategory" runat="server" />
                    </div>
                </asp:Panel>
                <div class="SearchPanelElements">
                    Model<br />
                        <asp:TextBox ID="txSearchEquipmentModel" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Model Description<br />
                        <asp:TextBox ID="txSearchEquipmentModelDescription" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Serial<br />
                        <asp:TextBox ID="txSearchEquipmentSerial" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    Equip XRef<br />
                        <asp:TextBox ID="txSearchEquipmentEquipmentXref" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    AgentId<br />
                        <asp:TextBox ID="txSearchEquipmentAgentId" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    <div style="display:block; float:left; padding-right: 15px;">
                    <br />
                    <asp:Button ID="BtSearchEquipmentClear" runat="server" Text="Clear" OnClick="btSearchEquipmentClear_Click" />
                    </div>
                    <div style="display:block; float:left; padding-right: 15px;">
                    <br />
                    <asp:Button ID="btSearchEquipmentSubmit" runat="server" Text="Search" OnClick="btSearchEquipmentSubmit_Click" />
                    </div>
                </div>
            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (EQUIPMENT) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rpEquipment_Small" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Model &nbsp;
                          <asp:CheckBox ID="chBxEqp" runat="server" Text='<%# Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Source") %>' 
                            SkinID="checkBoxHidingText" />
                        </td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="Label5" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="Label21" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Xref</td>
                        <td><asp:Label ID="Label31" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Unit Id</td>
                        <td><asp:Label ID="Label41" runat="server" Text='<% #Eval("Unit") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Agr Type</td>
                        <td><asp:Label ID="Label42" runat="server" Text='<% #Eval("AgreementDescription") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>AgentId</td>
                        <td><asp:Label ID="Label43" runat="server" Text='<% #Eval("AgentId") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Model &nbsp;
                              <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Source") %>' 
                            SkinID="checkBoxHidingText" />
                        </td>
                        <td><asp:Label ID="Label44" runat="server" Text='<% #Eval("Model") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="Label45" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="Label46" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Xref</td>
                        <td><asp:Label ID="Label47" runat="server" Text='<% #Eval("ModelXref") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Unit Id</td>
                        <td><asp:Label ID="Label48" runat="server" Text='<% #Eval("Unit") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Agr Type</td>
                        <td><asp:Label ID="Label49" runat="server" Text='<% #Eval("AgreementDescription") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>AgentId</td>
                        <td><asp:Label ID="Label50" runat="server" Text='<% #Eval("AgentId") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (EQUIPMENT) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (EQUIPMENT) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->

        <asp:GridView ID="gv_EquipmentLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Eqp"
            AllowPaging="true" 
            OnPageIndexChanging="gvPageIndexChanging_Eqp"
            EmptyDataText="No matching units were found">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:CheckBox ID="chBxEqp" runat="server" Text='<%# Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Source") %>' 
                            SkinID="checkBoxHidingText" />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Model Description" DataField="ModelDescription" SortExpression="ModelDescription" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Equip XRef" DataField="ModelXref" SortExpression="ModelXref" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Unit ID" DataField="Unit" SortExpression="Unit" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Contract Type" DataField="AgreementDescription" SortExpression="AgreementDescription" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="AgentId" DataField="AgentId" SortExpression="AgentId" ItemStyle-HorizontalAlign="Left" />
        </Columns>
    </asp:GridView>

            <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (EQUIPMENT) ======================================================================================= -->
            <div style="width: 100%; text-align: center; padding-top: 15px;">
                <asp:Button ID="btEquipmentSubmitBottom" runat="server" Text="Continue" onClick="btEquipmentSubmit_Click" SkinID="buttonPrimary" />
            </div>

        </asp:Panel>
        <%-- END: PANEL EQUIPMENT --%>



        <%-- START: PANEL SERIAL --%>
        <%--  --%>
        <asp:Panel ID="pnSerial" runat="server" Visible="true" >

            <div class="w3-row">
            <asp:Panel ID="pnSerialSearch" runat="server">
                <div class="titlePrimary SearchPanelElements">Search By Serial (or Cross Ref)</div>
                <div class="SearchPanelElements"><asp:TextBox ID="txSearchSerialOrAsset" runat="server" Width="200" /></div>
                <div class="SearchPanelElements"><asp:Button ID="btSearchSerialOrAssetSubmit" runat="server" Text="Search" OnClick="btSearchSerialOrAssetSubmit_Click" /></div>
                <div class="SearchPanelElements"><asp:Label ID="lbSearchSerialOrAssetMsg" runat="server" SkinID="labelError" /></div>
                <div class="spacer3"></div>
            </asp:Panel>
            </div>

            <div class="w3-row">
                <asp:Panel ID="pnSerialRequestType" runat="server" Visible="false" >
                    <div class="w3-third w3-container" style="border: 1px solid #bbbbbb; margin-right: 20px; padding-top: 10px; margin-bottom:15px; max-width: 300px;">
                        <div class="SearchPanelElements"><b>To continue with a service request click the link in the item name below</b></div>
                        <div class="spacer5"></div>
                        <div class="SearchPanelElements">
                            <span style="color: #3a7728; padding-left: 3px; font-size: 16px;">Service Type For This Request?</span>
                            <div class="spacer5"></div>
                            <asp:RadioButtonList ID="rblRequestServiceType" runat="server" RepeatDirection="Vertical" SkinID="RadioButtonList1">
                                <asp:ListItem Text="Standard Request" Value="STD" Selected="True" />
                                <asp:ListItem Text="PM (preventative maintenance)" Value="PM" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnSerialEquipmentMove" runat="server" Visible="false" >
                    <div class="w3-twothird w3-container" style="padding-top: 10px; border: 1px solid #bbbbbb; background-color: #f5f5f5"> 
                        <div><b>...or if items have been moved: reassign to the correct location:</b></div>
                        <div class="spacer5"></div>
                        <div class="SearchPanelElements">1)&nbsp;&nbsp;Identify new location <asp:DropDownList ID="ddSerialMoveLocation" runat="server" Font-Size="Small" /></div>
                        <div class="SearchPanelElements">
                            <div class="SearchPanelElements">
                                2)&nbsp;&nbsp;Include contact name <asp:TextBox ID="txSerialMoveName" runat="server" Width="200" MaxLength="50" />
                            </div>

                            <div class="SearchPanelElements">
                                and phone <asp:TextBox ID="txSerialMovePhone" runat="server" Width="125" MaxLength="15" /> 
                            </div>
                            <div class="SearchPanelElements">
                                extension? <asp:TextBox ID="txSerialMoveExtension" runat="server" Width="60" MaxLength="6" /> 
                            </div>
                       </div>
                        <div class="SearchPanelElements">3)&nbsp;&nbsp;Click checkboxes below of items which have moved</div>
                        <div class="spacer1"></div>
                        <div class="SearchPanelElements">4)&nbsp;&nbsp;<asp:Button ID="btSerialUpdateItemLocationSubmit" runat="server" Text="Update Item Location" OnClick="btSerialUpdateItemLocationSubmit_Click" /></div>
                    </div>
                </asp:Panel>
            </div>

            <div class="spacer15"></div>

            <div class="w3-row">
        <!-- START: SMALL SCREEN TABLE (SERIAL) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_SerialSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Item</td>
                        <td>
                            <asp:LinkButton ID="lkModel" runat="server" OnClick="lkSerialRequestPick_Click" 
                                    CommandArgument='<%# Eval("Cs1") + "|" + Eval("Cs2") + "|" + Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>'>
                                    <%# Eval("Model") %>
                             </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model Description</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Agreement Type</td>
                        <td><asp:Label ID="Label29" runat="server" Text='<% #Eval("AgreementDescription") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="Label22" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cross Ref</td>
                        <td><asp:Label ID="Label23" runat="server" Text='<% #Eval("Asset") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Upd?</td>
                        <td>
                            <asp:CheckBox ID="chBxMove" runat="server" 
                                Text='<%# Eval("Cs1") + "~" + Eval("Cs2") + "~" + Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Model") + "~" + Eval("Serial") + "~x" %>' 
                                SkinID="checkBoxHidingText" />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust Name</td>
                        <td><asp:Label ID="Label25" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Cust Num</td>
                        <td><asp:Label ID="Label24" runat="server" Text='<% #Eval("Cs1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td><asp:Label ID="Label26" runat="server" Text='<% #Eval("Cs2") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td><asp:Label ID="Label27" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td><asp:Label ID="Label28" runat="server" Text='<% #Eval("State") %>' /></td>
                    </tr>

                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Item</td>
                        <td>
                            <asp:LinkButton ID="lkModel" runat="server" OnClick="lkSerialRequestPick_Click" 
                                    CommandArgument='<%# Eval("Cs1") + "|" + Eval("Cs2") + "|" + Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>'>
                                    <%# Eval("Model") %>
                             </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model Description</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("ModelDescription") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Agreement Type</td>
                        <td><asp:Label ID="Label29" runat="server" Text='<% #Eval("AgreementDescription") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="Label22" runat="server" Text='<% #Eval("Serial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cross Ref</td>
                        <td><asp:Label ID="Label23" runat="server" Text='<% #Eval("Asset") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Upd?</td>
                        <td>
                            <asp:CheckBox ID="chBxMove" runat="server" 
                                Text='<%# Eval("Cs1") + "~" + Eval("Cs2") + "~" + Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Model") + "~" + Eval("Serial") + "~x" %>' 
                                SkinID="checkBoxHidingText" />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust Name</td>
                        <td><asp:Label ID="Label25" runat="server" Text='<% #Eval("CustName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Cust Num</td>
                        <td><asp:Label ID="Label24" runat="server" Text='<% #Eval("Cs1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td><asp:Label ID="Label26" runat="server" Text='<% #Eval("Cs2") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td><asp:Label ID="Label27" runat="server" Text='<% #Eval("City") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td><asp:Label ID="Label28" runat="server" Text='<% #Eval("State") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (SERIAL) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (SERIAL) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->

                 <asp:GridView ID="gv_SerialLarge" runat="server"
                     AutoGenerateColumns="False" 
                     CssClass="tableWithLines" 
                     PageSize="900"
                     AllowSorting="true" 
                     onsorting="gvSorting_Ser"
                     AllowPaging="true" 
                     OnPageIndexChanging="gvPageIndexChanging_Ser"
                     EmptyDataText="No matching records were found">
                     <AlternatingRowStyle CssClass="trColorAlt" />
                     <Columns>
                        <asp:TemplateField HeaderText="Item" SortExpression="Model" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lkModel" runat="server" OnClick="lkSerialRequestPick_Click" 
                                    CommandArgument='<%# Eval("Cs1") + "|" + Eval("Cs2") + "|" + Eval("Unit") + "|" + Eval("Agreement") + "|" + Eval("Source") %>'>
                                    <%# Eval("Model") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ModelDescription" HeaderText="Description" SortExpression="ModelDescription"  />
                         <asp:BoundField DataField="AgreementDescription" HeaderText="Agr Type" SortExpression="AgreementDescription"  />
                         <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial"  />
                        <asp:BoundField DataField="Asset" HeaderText="Cross Ref" SortExpression="Asset"  />
                        <asp:TemplateField HeaderText="Upd?" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                <asp:CheckBox ID="chBxMove" runat="server" 
                                    Text='<%# Eval("Cs1") + "~" + Eval("Cs2") + "~" + Eval("Unit") + "~" + Eval("Agreement") + "~" + Eval("Model") + "~" + Eval("Serial") + "~x" %>' 
                                    SkinID="checkBoxHidingText" />
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustName" HeaderText="Name" SortExpression="CustName"  />
                        <asp:BoundField DataField="Cs1" HeaderText="Num" SortExpression="Cs1" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Cs2" HeaderText="Loc" SortExpression="Cs2" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City"  />
                        <asp:BoundField DataField="State" HeaderText="ST" SortExpression="State" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
    </div>
        <!-- END: LARGE SCREEN TABLE (SERIAL) ======================================================================================= -->




            </div>
        </asp:Panel>
        <%-- END: PANEL SERIAL --%>



        <%-- START: PANEL PROBLEM --%>
		<%--  --%>
        <asp:Panel ID="pnProblem" runat="server" Visible="false">

       <asp:Label ID="lbPriorityInfo" runat="server" SkinID="labelError" Visible="false" />
       <asp:Label ID="lbInterfaceInfo" runat="server" SkinID="labelTitleColor1_Small" Visible="false" />

       <div class="SearchPanelElements"><h3 class="titlePrimary">Service Issue</h3></div>
            <%-- WAS "Submit Automated Request" but when you include B2 units, those will no longer be automated  --%>
       <div class="SearchPanelElements">
           <asp:Button ID="btProblemSubmit" runat="server" Text="Submit Request" onClick="btProblemSubmit_Click" SkinID="buttonPrimary" />
       </div>
            <div class="SearchPanelElements">
                <asp:Label ID="lbProblemComment" runat="server" Text="Comments or Remarks?" Font-Bold="true" />
                <div class="spacer5"></div>
                <asp:TextBox ID="txProblemComment" runat="server" 
                    TextMode="MultiLine" 
                    Width="350" 
                    Height="75" 
                    MaxLength="1000" 
                    />
            </div>
            <div class="SearchPanelElements">
                <span style="font-style: italic">Required Fields</span> = <span style="color: crimson;">*</span>
            </div>
            <div class="spacer5"></div>

            <asp:Repeater ID="rp_Problem" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <asp:HiddenField ID="hfProblemSourceA" runat="server" Value='<%# Eval("Source") %>' />
                        <asp:HiddenField ID="hfProblemAgreementNumberA" runat="server" Value='<%# Eval("Agreement") %>' />
                        <asp:HiddenField ID="hfProblemAgreementCodeA" runat="server" Value='<%# Eval("AgreementCode") %>' />
                        <asp:HiddenField ID="hfProblemUnitA" runat="server" Value='<%# Eval("Unit") %>' />
                    <asp:Panel ID="pnProblemErrorA" runat="server" Visible="false">
                    <tr class="trColorReg">
                        <td style="font-weight: bold;">Error</td>
                        <td><asp:Label ID="lbProblemErrorA" runat="server" Text="" SkinId="labelError" Visible="true" /></td>
                    </tr>
                    </asp:Panel>
                    <tr class="trColorReg">
                        <td><div style="float: left;">Model</div><div style="float: left;"><asp:Panel ID="pnProblemModelA" Visible="false" runat="server">&nbsp;<span style="color: crimson;">*</span></asp:Panel></div>
                        </td>
                        <td><asp:Label ID="lbProblemModelA" runat="server" Text='<%# Eval("Model") %>' />
                            <asp:TextBox ID="txProblemModelA" runat="server" Width="200" Visible="false" />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbProblemSerialA" runat="server" Text='<%# Eval("Serial")  %>' />
                            <asp:TextBox ID="txProblemSerialA" runat="server" Width="275" Visible="false"  />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td><div style="float: left;">Service Type</div><div style="float: left;"><asp:Panel ID="pnProblemServiceTypeA" Visible="false" runat="server">&nbsp;<span style="color: crimson;">*</span></asp:Panel></div></td>
                        <td><asp:Label ID="lbProblemAgreementDescriptionA" runat="server" Text='<%# Eval("AgreementDescription") %>' />
                            <asp:DropDownList ID="ddProblemServiceTypeA" runat="server" Visible="false">
                                <asp:ListItem Text="ONSITE" Value="ONSITE" />
                                <asp:ListItem Text="DEPOT" Value="DEPOT" />
                                <asp:ListItem Text="EXPRESS" Value="EXPRESS" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Problem Desc&nbsp;<span style="color: crimson;">*</span></td>
                        <td><asp:TextBox ID="txProblemDescriptionA" runat="server" Width="275" /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Ticket Xref</td>
                        <td><asp:TextBox ID="txProblemTicketXrefA" runat="server" Width="150" /></td>
                    </tr>
                    <asp:Panel ID="pnProblemInterfaceA" runat="server" Visible="false">
                    <tr class="trColorReg">
                        <td>Interface&nbsp;<span style="color: crimson;">*</span></td>
                        <td>
                            <asp:DropDownList ID="ddProblemInterfaceA" runat="server">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Network" Value="NETWORK" />
                                <asp:ListItem Text="USB" Value="USB" />
                                <asp:ListItem Text="Other" Value="OTHER" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnProblemViaA" runat="server" Visible="false">
                    <tr class="trColorReg">
                        <td>Ship Via&nbsp;<span style="color: crimson;">*</span></td>
                        <td>
                            <asp:DropDownList ID="ddProblemViaA" runat="server">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="UPS Ground" Value="1" />
                                <asp:ListItem Text="UPS 2nd Day" Value="3" />
                                <asp:ListItem Text="Next Day Saver 5pm" Value="5" />
                                <asp:ListItem Text="Next Day Air 10am" Value="4" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:HiddenField ID="hfProblemProcessorA" runat="server" Value="" Visible="false" />
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                            <asp:HiddenField ID="hfProblemSourceB" runat="server" Value='<%# Eval("Source") %>' />
                            <asp:HiddenField ID="hfProblemAgreementNumberB" runat="server" Value='<%# Eval("Agreement") %>' />
                            <asp:HiddenField ID="hfProblemAgreementCodeB" runat="server" Value='<%# Eval("AgreementCode") %>' />
                            <asp:HiddenField ID="hfProblemUnitB" runat="server" Value='<%# Eval("Unit") %>' />
                    <asp:Panel ID="pnProblemErrorB" runat="server" Visible="false">
                        <tr class="trColorAlt">
                            <td style="font-weight: bold;">Error</td>
                            <td><asp:Label ID="lbProblemErrorB" runat="server" Text="" SkinId="labelError" Visible="true" /></td>
                        </tr>
                    </asp:Panel>
                    <tr class="trColorAlt">
                        <td>Model<asp:Panel ID="pnProblemModelB" Visible="false" runat="server">&nbsp;<span style="color: crimson;">*</span></asp:Panel></td>
                        <td><asp:Label ID="lbProblemModelB" runat="server" Text='<%# Eval("Model") %>' />
                            <asp:TextBox ID="txProblemModelB" runat="server" Width="200" Visible="false" />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbProblemSerialB" runat="server" Text='<%# Eval("Serial") %>' />
                            <asp:TextBox ID="txProblemSerialB" runat="server" Width="275" Visible="false" />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Service Type<asp:Panel ID="pnProblemServiceTypeB" Visible="false" runat="server">&nbsp;<span style="color: crimson;">*</span></asp:Panel></td></td>
                        <td><asp:Label ID="lbProblemAgreementDescriptionB" runat="server" Text='<%# Eval("AgreementDescription") %>' />
                            <asp:DropDownList ID="ddProblemServiceTypeB" runat="server" Visible="false">
                                <asp:ListItem Text="ONSITE" Value="ONSITE" />
                                <asp:ListItem Text="DEPOT" Value="DEPOT" />
                                <asp:ListItem Text="EXPRESS" Value="EXPRESS" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Problem Desc&nbsp;<span style="color: crimson;">*</span></td>
                        <td><asp:TextBox ID="txProblemDescriptionB" runat="server" Width="275" /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Ticket Xref</td>
                        <td><asp:TextBox ID="txProblemTicketXrefB" runat="server" Width="150" /></td>
                    </tr>
                    <asp:Panel ID="pnProblemInterfaceB" runat="server" Visible="false">
                    <tr class="trColorAlt">
                        <td>Interface&nbsp;<span style="color: crimson;">*</span></td>
                        <td>
                             <asp:DropDownList ID="ddProblemInterfaceB" runat="server" CssClass="dropDownList1">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Network" Value="NETWORK" />
                                <asp:ListItem Text="USB" Value="USB" />
                                <asp:ListItem Text="Other" Value="OTHER" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnProblemViaB" runat="server" Visible="false">
                    <tr class="trColorAlt">
                        <td>Ship Via&nbsp;<span style="color: crimson;">*</span></td>
                        <td>
                            <asp:DropDownList ID="ddProblemViaB" runat="server">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="UPS Ground" Value="1" />
                                <asp:ListItem Text="UPS 2nd Day" Value="3" />
                                <asp:ListItem Text="Next Day Saver 5pm" Value="5" />
                                <asp:ListItem Text="Next Day Air 10am" Value="4" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:HiddenField ID="hfProblemProcessorB" runat="server" Value="" Visible="false" />
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>




                    </asp:Panel>
        <%-- END: PANEL PROBLEM --%>

        <%-- START: PANEL RESULT --%>
                    <%--  --%>
                    <asp:Panel ID="pnResult" runat="server" Visible="false" >
                        <asp:Label ID="lbResultMsg" runat="server" />
                            <asp:GridView ID="gv_Result" runat="server"
                                AutoGenerateColumns="False" 
                                CssClass="tableWithLines"
                                EmptyDataText="Error: No Ticket Was Created">
                                <AlternatingRowStyle CssClass="trColorAlt" />
                                <Columns>
                                <%-- 
                                    <asp:TemplateField HeaderText="Ticket" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbResultCall" runat="server" Text='<%# Eval("Center") + "-" + Eval("Ticket") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                --%>
                                <asp:BoundField HeaderText="Ticket" DataField="Call" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="Model" DataField="Model" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Problem" DataField="Problem" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                        <div class="spacer20"></div>
                        <div class="SearchPanelElements">
                            <asp:Button ID="btSubmitAnotherRequest" runat="server" Text="Submit Another Request" OnClick="btSubmitAnotherRequest_Click" /></div>
                        <div class="SearchPanelElements"><asp:Button ID="btViewMyOpenTickets" runat="server" Text="View My Open Tickets" OnClick="btViewMyOpenTickets_Click"  /></div>
                    </asp:Panel>
        <%-- END: PANEL RESULT --%>
                    <%--  --%>
	</div>

        <%--  --%>


    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfChosenCs1" runat="server" />
    <asp:HiddenField ID="hfChosenCs2" runat="server" />
    <asp:HiddenField ID="hfCurrentPage" runat="server" />
    <asp:HiddenField ID="hfRequestHeader" runat="server" />
    <asp:HiddenField ID="hfRequestEquipmentList" runat="server" />
    <asp:HiddenField ID="hfUnitList" runat="server" />
    <asp:HiddenField ID="hfMoveList" runat="server" />
    <asp:HiddenField ID="hfOracleParentId" runat="server" />
    <asp:HiddenField ID="hfOracleChildId" runat="server" />
    <asp:HiddenField ID="hfRequestResultList" runat="server" />
    <asp:HiddenField ID="hfSerialPickRequestType" runat="server" />
    <asp:HiddenField ID="hfRequestSourcePage" runat="server" />
    <asp:HiddenField ID="hfEmailUserName" runat="server" />
</div>
</asp:Content>
