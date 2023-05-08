<%@ Page Title="Service History" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ServiceHistory.aspx.cs" Inherits="private_sc_ServiceHistory" %>
<%--

--%>             
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <%--  --%>             
    <table style="width: 100%;">
        <tr>
            <td>Service History</td>
            <td style="display: block; float: right;">
                <asp:Panel ID="pnCs1Change" runat="server" DefaultButton="btCs1Change">
                    <asp:Table ID="tbCs1Change" runat="server" Visible="false">
                        <asp:TableRow VerticalAlign="Bottom">
                            <asp:TableCell>
                                <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="10" />
                                &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7"
                                    ValidationGroup="Cs1Change" />
                                   <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                        Operator="DataTypeCheck"
                                        Type="Integer"
                                        ControlToValidate="txCs1Change"
                                        ErrorMessage="Customer entry must be a number" 
                                        Text="*"
                                        SetFocusOnError="true" 
                                        ValidationGroup="Cs1Change"></asp:CompareValidator>
                                    &nbsp;
                                    <asp:Button ID="btCs1Change" runat="server" 
                                        Text="Change Customer" 
                                        onclick="btCs1Change_Click"
                                        ValidationGroup="Cs1Change" />
                                    <asp:ValidationSummary ID="vSumCs1Change" runat="server" ValidationGroup="Cs1Change" />
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></td></tr></table></asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
        // =============================================================
        function clearLocSearch() {
            var doc = document.forms[0];
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_ddCs1Family.value = "";
            }
            if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr != "undefined") {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txAdr.value = "";
            }
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCs2.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txNam.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txCit.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txSta.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txZip.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txPhn.value = "";
            doc.ctl00_ctl00_For_Body_A_For_Body_A_txXrf.value = "";
            return true;
        }
        function clearConditionInput() {
            var doc = document.forms[0];
            if (doc.ctl00_ctl00_For_Body_A_For_Body_A_txCtr.value != undefined) {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txCtr.value = "";
            }
            if (doc.ctl00_ctl00_For_Body_A_For_Body_A_txTck.value != undefined) {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txTck.value = "";
            }
            if (doc.ctl00_ctl00_For_Body_A_For_Body_A_txRef.value != undefined) {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_txRef.value = "";
            }
            if (doc.ctl00_ctl00_For_Body_A_For_Body_A_calStart.value != undefined) {
                doc.ctl00_ctl00_For_Body_A_For_Body_A_calStart.value = "";
                doc.ctl00_ctl00_For_Body_A_For_Body_A_calEnd.value = "";
            }
            return true;
        }
    </script>

<div id="dvInput">
    <asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />

<%-- Search Boxes to Refine Lower Location Table To Pick One Location (or All) --%>             
    <asp:Panel ID="pnOneOrAllLocs" runat="server" DefaultButton="btLocSearch">
    <asp:Table ID="tbInput" runat="server" CssClass="tableWithoutLines tableBorder" style="width: 100%">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="11" HorizontalAlign="Left">
                <asp:Label ID="lbSearchInstructions" runat="server" />
            </asp:TableCell></asp:TableRow><asp:TableHeaderRow>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell><asp:TableHeaderCell>
                <asp:Label ID="lbCust" runat="server" />
            </asp:TableHeaderCell><asp:TableHeaderCell>Loc</asp:TableHeaderCell><asp:TableHeaderCell>CustXRef</asp:TableHeaderCell><asp:TableHeaderCell>
                <asp:Label ID="lbAddress" runat="server" />
            </asp:TableHeaderCell><asp:TableHeaderCell>City</asp:TableHeaderCell><asp:TableHeaderCell>St</asp:TableHeaderCell><asp:TableHeaderCell>Zip</asp:TableHeaderCell><asp:TableHeaderCell>Phone</asp:TableHeaderCell><asp:TableHeaderCell ColumnSpan="3"></asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="txNam" runat="server" Width="110" MaxLength="40" />
                    <asp:CompareValidator id="vCustom_Cs2" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txCs2"
                        ErrorMessage="Location must be an integer" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="ddCs1Family" runat="server" 
                    CssClass="dropDownList1" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txCs2" runat="server" Width="40" MaxLength="3" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txXrf" runat="server"  Width="50" MaxLength="15" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txAdr" runat="server"  Width="100" MaxLength="30" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txCit" runat="server"  Width="100" MaxLength="30" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txSta" runat="server"  Width="25" MaxLength="2" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txZip" runat="server"  Width="40" MaxLength="9" />
                <asp:CompareValidator id="vCustom_Zip" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txZip"
                        ErrorMessage="Zip Code must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txPhn" runat="server"  Width="50" MaxLength="10" />
                    <asp:CompareValidator id="vCustom_Phn" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhn"
                        ErrorMessage="The phone must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="LocSearch"></asp:CompareValidator>
            </asp:TableCell><asp:TableCell>
                <asp:Button ID="btLocSearch" runat="server" 
                    Text="Search" 
                    onclick="btLocSearch_Click" 
                    ValidationGroup="LocSearch" />
            </asp:TableCell><asp:TableCell>
                <input id="btClear" type="button" value="Clear" class="buttonDefault" onclick="javascript: clearLocSearch();" />
            </asp:TableCell><asp:TableCell>
                <asp:Button ID="btAll" runat="server" 
                    Text="All Locations" 
                    ValidationGroup="AllSearch"
                    onclick="AllLocations_Click" />
                <asp:CustomValidator id="vCusAllLoc" runat="server" 
                    Display="None" 
                    EnableClientScript="False"
                    ValidationGroup="AllSearch" />
             </asp:TableCell></asp:TableRow></asp:Table><asp:ValidationSummary ID="vSummary_LocSearch" runat="server" ValidationGroup="LocSearch" />
    <asp:ValidationSummary ID="vSumAllSearch" runat="server" ValidationGroup="AllSearch" />
</asp:Panel>
                
<%-- Report Conditions --%>                             
<asp:Panel ID="pnConditions" runat="server" Visible="false" DefaultButton="btReport">
    <asp:Label ID="lbConditions" runat="server" Text="" SkinID="labelTitleColor1_Small" />
    <table class="tableWithoutLines" style="width: 100%">
        <tr style="vertical-align: top;">
            <td style="text-align: left;" class="auto-style2"><table>
                    <tr>
                        <td class="auto-style5"><%-- 
                                 style="width: 222px; vertical-align: top; height: 110px;"
                                Width="337px"
                                Font-Names="Verdana"
                                Font-Size="14px"
                                SkinId="listBox1" 
                                --%>                             
                            <asp:ListBox 
                                ID="lsBxReportType" runat="server"  
                                Rows="6" 
                                AutoPostBack="true"
                                onselectedindexchanged="lsBxReportType_SelectedIndexChanged" Height="115px">
                            <asp:ListItem Text="Open Tickets" Value="Open" />
                                <asp:ListItem Text="Tickets Closed in a Date Range" Value="ClosedRange" />
                                <asp:ListItem Text="Tickets Opened in a Date Range" Value="OpenRange" />
                                <asp:ListItem Text="By Model (Summary) Closed in a Date Range" Value="ClosedModel" />
                                <asp:ListItem Text="By Ticket Number" Value="ByTicket" />
                                <asp:ListItem Text="By Ticket Cross Reference" Value="ByXref" />
                            </asp:ListBox>
                        </td></tr>
                     <tr>
                        <td class="auto-style5"><asp:Label ID="lbTicketsType" runat="server" Text="Include Ticket types:" Font-Bold Font-Italic ForeColor="Teal"></asp:Label></tr><tr>
                        <td class="auto-style5"><asp:CheckBoxList ID="cbServiceType" runat="server">
                            <asp:ListItem Text="Onsite Service Tickets" Value="Onsite" />
                            <asp:ListItem Text="Depot and Advance Exchange Tickets" Value="ExpressDepot" />                              
                            <asp:ListItem Text="Managed IT Service Tickets" Value="MITS" />
                            <asp:ListItem Text="Managed Print Service Tickets" Value="MPrint" />                                
                            <asp:ListItem Text="Consumables Replenishment Tickets" Value="TonerReplenish" />                             
                            <asp:ListItem Text="Preventive Maintenance Tickets" Value="PMs" />
                            <asp:ListItem Text="Product Sale and Installation Tickets" Value="InstallSales" />
                            <asp:ListItem Text="Supply Sales Tickets" Value="Supplies" />
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style5"><asp:Button ID="btReport" runat="server" 
                                Text="Create Report" 
                                onclick="btReport_Click" Width="89px" />&nbsp; <asp:Button ID="btDownload" runat="server" 
                                Text="Download" 
                                onclick="btDownload_Click" Width="70px" />&nbsp; <asp:Button ID="btClearConditions" runat="server" 
                                Text="Reset Input" 
                                onclick="btClearConditions_Click" Width="73px"  /></td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Panel ID="pnCalendars" runat="server" Visible="false">
                    <table>
                        <tr valign="top">
                            <th class="auto-style3">Start Date</th><th class="auto-style3">End Date</th></tr><tr valign="top">
                            <td class="auto-style3"><asp:Calendar ID="calStart" runat="server" SkinID="calendarIsa"></asp:Calendar>
                            </td>
                            <td class="auto-style3"><asp:Calendar ID="calEnd" runat="server" SkinID="calendarIsa"></asp:Calendar>
                            </td>
                        </tr>
                   </table>
                </asp:Panel>
                <asp:Panel ID="pnCtrTck" runat="server" Visible="false">
                    <table>
                        <tr>
                            <th align="left">Ticket Number</th></tr><tr valign="top">
                            <td>
                                <asp:TextBox ID="txCtr" runat="server" Width="30" MaxLength="3" /> - <asp:TextBox ID="txTck" runat="server" Width="70"  MaxLength="7" />
                            </td>
                        </tr>
                   </table>
                </asp:Panel>
                <asp:Panel ID="pnCrossRef" runat="server" Visible="false">
                    <table>
                        <tr align="left">
                            <th align="left">Ticket Cross Reference</th></tr><tr valign="top">
                            <td>
                                <asp:TextBox ID="txRef" runat="server" Width="120" MaxLength="24" />
                            </td>
                        </tr>
                   </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
</div><!-- end parmDiv Div -->
<asp:Label ID="lbError" runat="server" SkinID="labelError" Visible="false" />
<%-- Show Selected Customer Locations          PagerStyle-ForeColor="Blue" --%> 
    <asp:Panel ID="pnLocations" runat="server" Visible="false">
      <asp:GridView ID="gvLocations" runat="server"
         AutoGenerateColumns="False" 
         CssClass="tableWithLines width100"
         AllowPaging="true"
         AllowSorting="True" 
         onpageindexchanging="gvPageIndexChanging_Loc" 
         onsorting="gvSorting_Loc">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="CustNam">
            <ItemTemplate>
                <asp:LinkButton ID="lkName" runat="server" OnClick="lkName_Click" 
                    CommandArgument='<%# Eval("CustNum") + "|" + Eval("CustLoc")%>'>
                    <%# Eval("CustNam") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="CustNum" HeaderText="Cust" SortExpression="CustNum" />
        <asp:BoundField DataField="CustLoc" HeaderText="Loc" SortExpression="CustLoc" />
        <asp:BoundField DataField="CustXrf" HeaderText="Cust XRef" SortExpression="CustXrf" />
        <asp:BoundField DataField="CustAdr" HeaderText="Address" SortExpression="CustAdr" />
        <asp:BoundField DataField="CustCit" HeaderText="City" SortExpression="CustCit" />
        <asp:BoundField DataField="CustSta" HeaderText="State" SortExpression="CustSta" />
        <asp:BoundField DataField="CustZip" HeaderText="Zip" SortExpression="CustZip" />
        <asp:BoundField DataField="CustPhn" HeaderText="Phone" SortExpression="CustPhn" />
    </Columns>
</asp:GridView>
</asp:Panel>


<asp:Panel ID="pnModel" runat="server" Visible="false">
  <%-- GRID VIEW MODELS (Container Store) ============================ --%>
        <asp:GridView ID="gvModels" runat="server" style="width: 50%" 
             AutoGenerateColumns="False" 
            CssClass="tableWithLines">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="ModelCount" HeaderText="Total Tickets" />
            </Columns>
        </asp:GridView>
        <%-- END GRID VIEW MODELS ========================================== --%>
</asp:Panel>



<%-- Show Selected Tickets for Selected Location --%> 
<asp:Panel ID="pnTickets" runat="server" Visible="false">
    <br /><br />
    <asp:GridView ID="gvTickets" runat="server" style="width: 100%"
     AutoGenerateColumns="False" 
     CssClass="tableWithLines"
     AllowPaging="true"
     PageSize="500" 
     AllowSorting="True" 
     onpageindexchanging="gvPageIndexChanging_Tck" 
     onsorting="gvSorting_Tck">
     <AlternatingRowStyle CssClass="trColorAlt" />
     <Columns>
        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
        <asp:BoundField DataField="CustXref" HeaderText="Cust XRef" SortExpression="CustXref" />
        <asp:BoundField DataField="DateEnteredText" HeaderText="Date Entered" SortExpression="DateEntered" />
        <asp:TemplateField HeaderText="Ticket" SortExpression="alpCtrTck">
            <ItemTemplate>
                <asp:LinkButton ID="lkTicket" runat="server" OnClick="lkTicket_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Center") + "-" + Eval("Ticket") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="PurchOrd" HeaderText="Ticket XRef" SortExpression="PurchOrd" />
<%--
        
        <asp:BoundField DataField="ModelXref" HeaderText="ModelXRef" SortExpression="ModelXref" />
--%>             
        <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />
        <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
        <asp:TemplateField HeaderText="Status" SortExpression="Remark">
            <ItemTemplate>
                <asp:LinkButton ID="lkStatus" runat="server" OnClick="lkStatus_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("Remark") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Part Use" SortExpression="PartsUsed">
            <ItemTemplate>
                <asp:LinkButton ID="lkPartUse" runat="server" OnClick="lkPartUse_Click" 
                    CommandArgument='<%# Eval("Center") + "|" + Eval("Ticket") %>'>
                    <%# Eval("PartsUsed") %>
                </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="Trips" HeaderText="Trips" />

      
       </Columns></asp:GridView></asp:Panel><!-- End pnTickets --><%-- Generic Display Panel to load with Requested Data from Ticket Table --%><div id="dvDisplay" runat="server">   
     <asp:Panel ID="pnDisplay" runat="server">
    </asp:Panel>
</div><!-- End dvDisplay ** DO NOT DELETE ** This is dynamically loaded with user selections -->
<%--  --%>             

<script type="text/javascript">
    //document.aspnetForm.ctl00_BodyContent_txCs2.focus();
</script>
</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="For_HtmlHead"><style type="text/css">                                                                                  
                                                                                  .auto-style2 {
                                                                                      width: 33%;
                                                                                  }
                                                                                  .auto-style3 {
                                                                                      width: 307px;
                                                                                  }
                                                                                  .auto-style5 {
                                                                                      width: 405px;
                                                                                  }
                                                                              </style></asp:Content>

