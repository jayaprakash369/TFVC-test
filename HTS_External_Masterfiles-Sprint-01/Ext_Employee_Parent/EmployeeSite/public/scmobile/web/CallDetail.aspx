<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallDetail.aspx.cs" 
Inherits="public_scmobile_web_CallDetail" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
width: 480px;
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Call Detail</title>
    <style type="text/css">
        body { margin: 2px; } 
        .tbDetail { margin-top: 5px; margin-bottom: 10px;  }
        .tbDetail tr { vertical-align: top; }
        .tbDetail th { padding-top: 2px; padding-bottom: 4px; padding-right: 15px;  text-align: left; width: 100px; }
        .tbDetail td { padding-top: 2px; padding-bottom: 4px; padding-right: 10px; }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:20px; ">
    
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>
    <asp:Panel ID="pnCustomer" runat="server" style="margin-bottom: 20px; margin-top: 10px;">
    <center><asp:Label ID="lbCs1Data" runat="server" Text="Customer Information" SkinID="TableTitle1" /></center>
    <table class="tableWithLines" style="width:100%;">
	    <tr>
		    <td>Name</td>
		    <td><asp:Label ID="lbCustName" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Number</td>
		    <td><asp:Label ID="lbCs1Cs2" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Contact</td>
		    <td><asp:Label ID="lbContact" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Phone</td>
		    <td><asp:Label ID="lbPhoneExt" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Address</td>
		    <td><asp:Label ID="lbAddress1" runat="server" /><asp:Label ID="lbAddress2" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>&nbsp;</td>
		    <td><asp:Label ID="lbCityStateZip" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Avg Time</td>
		    <td>Open to onsite <asp:Label ID="lbAvgResponseTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Agreement</td>
		    <td><asp:Label ID="lbAgr" runat="server" />&nbsp; <asp:Label ID="lbAgrType" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Dealer</td>
		    <td><asp:Label ID="lbDealer" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Email WO</td>
		    <td><asp:Label ID="lbEmailWorkorder" runat="server" /></td>
	    </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="pnEquipment" runat="server" style="margin-bottom: 20px;">
    <center><asp:Label ID="lbEqpData" runat="server" Text="Equipment Information" SkinID="TableTitle1" /></center>

        <table class="tableWithLines" style="width:100%;">
	        <tr>
		        <td>Part</td>
		        <td><asp:Label ID="lbPart" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Desc</td>
		        <td><asp:Label ID="lbDescription" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Serial</td>
		        <td><asp:Label ID="lbSerial" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Unit</td>
		        <td><asp:Label ID="lbUnit" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Eqp Loc</td>
		        <td><asp:Label ID="lbEquipLoc" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Agent ID</td>
		        <td><asp:Label ID="lbAgentId" runat="server" /></td>
	        </tr>
        </table>
    </asp:Panel>

<asp:Panel ID="pnCall" runat="server" style="margin-bottom: 20px;">
    <center><asp:Label ID="lbCall" runat="server" Text="Call Information" SkinID="TableTitle1" /></center>
    <table class="tableWithLines" style="width:100%;">
	    <tr>
		    <td>Call Type</td>
		    <td><asp:Label ID="lbCallType" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Cust Tck Xref</td>
		    <td><asp:Label ID="lbTckXrf" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Entered</td>
		    <td><asp:Label ID="lbEntryDate" runat="server" /> &nbsp; <asp:Label ID="lbEntryTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Complete</td>
		    <td><asp:Label ID="lbCompleteDate" runat="server" /> &nbsp; <asp:Label ID="lbCompleteTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Closed</td>
		    <td><asp:Label ID="lbCloseDate" runat="server" /> &nbsp; <asp:Label ID="lbCloseTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Req Resp Tm</td>
		    <td><asp:Label ID="lbRequiredResponseTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Times</td>
		    <td>Open to notify <asp:Label ID="lbNotifyTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>&nbsp;</td>
		    <td>Open to onsite <asp:Label ID="lbOnsiteTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>&nbsp;</td>
		    <td>Open to complete <asp:Label ID="lbDurationTime" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Problem</td>
		    <td><asp:Label ID="lbComment" runat="server" /></td>
	    </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="pnTimestamps" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbTimestamps" runat="server" Text="Timestamps" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpTimestamps" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Stamp</td>
                        <td><%# Eval("DisplayStamp") + " (" + Eval("TIMTCH") + ")"%></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") + " " + Eval("DisplayTime") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Stamp</td>
                        <td><%# Eval("DisplayStamp") + " (" + Eval("TIMTCH") + ")"%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") + " " + Eval("DisplayTime") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel ID="pnTicketRevenue" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbTicketRevenue" runat="server" Text="Ticket Revenue" SkinID="TableTitle1" /></center>
        <table class="tableWithLines" style="width:100%;">
            <tr>
                <td>Trip</td>
                <td style="text-align: right;"><asp:Label ID="lbBilTrip" runat="server" /></td>
            </tr>
            <tr>
                <td>Material</td>
                <td style="text-align: right;"><asp:Label ID="lbBilMaterial" runat="server" /></td>
            </tr>
            <tr>
                <td>Labor</td>
                <td style="text-align: right;"><asp:Label ID="lbBilOnsLabor" runat="server" /></td>
            </tr>
            <tr>
                <td>Travel Labor</td>
                <td style="text-align: right;"><asp:Label ID="lbBilTrvLabor" runat="server" /></td>
            </tr>
            <tr>
                <td>Travel Mileage</td>
                <td style="text-align: right;"><asp:Label ID="lbBilTrvMilage" runat="server" /></td>
            </tr>
            <tr>
                <td>Misc Revenue</td>
                <td style="text-align: right;"><asp:Label ID="lbBilMisc" runat="server" /></td>
            </tr>
            <tr>
                <td>Sales Tax</td>
                <td style="text-align: right;"><asp:Label ID="lbBilTax" runat="server" /></td>
            </tr>
            <tr style="background-color: #DDDDDD;">
                <td style="font-weight: bold;">Total Due</td>
                <td style="text-align: right; font-weight: bold;"><asp:Label ID="lbTotDue" runat="server" /></td>
            </tr>
            <tr>
                <td>Balance Due</td>
                <td style="text-align: right;"><asp:Label ID="lbBalDue" runat="server" /></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnMiscRevenue" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbMiscRevenue" runat="server" Text="Miscellaneous Revenue Detail"  SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpMiscRevenue" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Code</td>
                        <td><%# Eval("MRDESC")%></td>
                    </tr>
                    <tr>
                        <td>Price</td>
                        <td><%# Eval("MRREVS")%></td>
                    </tr>
                    <tr>
                        <td>Cost</td>
                        <td><%# Eval("MRREVC")%></td>
                    </tr>
                    <tr>
                        <td>Type</td>
                        <td><%# Eval("TMRCOV")%></td>
                    </tr>
                    <tr>
                        <td>Comment</td>
                        <td><%# Eval("MRCOMM")%></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Code</td>
                        <td><%# Eval("MRDESC")%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Price</td>
                        <td><%# Eval("MRREVS")%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Cost</td>
                        <td><%# Eval("MRREVC")%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Type</td>
                        <td><%# Eval("TMRCOV")%></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Comment</td>
                        <td><%# Eval("MRCOMM")%></td>
                    </tr>
                     
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel ID="pnPartsUsed" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbPartsUsed" runat="server" Text="Parts Used" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpPartsUsed" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr>
                        <td>Description</td>
                        <td><%# Eval("Description") %></td>
                    </tr>
                    <tr>
                        <td>Qty</td>
                        <td><%# Eval("Qty") %></td>
                    </tr>
                    <tr>
                        <td>Serial</td>
                        <td><%# Eval("Serial") %></td>
                    </tr>
                    <tr>
                        <td>From Loc</td>
                        <td><%# Eval("Location") %></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Part</td>
                        <td><%# Eval("Part") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Description</td>
                        <td><%# Eval("Description") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Qty</td>
                        <td><%# Eval("Qty") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Serial</td>
                        <td><%# Eval("Serial") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>From Loc</td>
                        <td><%# Eval("Location") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel ID="pnOnsiteLabor" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbOnsiteLabor" runat="server" Text="Onsite Labor" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpOnsiteLabor" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Start Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
                    <tr>
                        <td>Start Time</td>
                        <td><%# Eval("DisplayStart") %></td>
                    </tr>
                    <tr>
                        <td>End Time</td>
                        <td><%# Eval("DisplayEnd") %></td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td><%# Eval("Duration") %></td>
                    </tr>
                    <tr>
                        <td>Tech</td>
                        <td><%# Eval("Tech") %></td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td><%# Eval("Name") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Start Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Start Time</td>
                        <td><%# Eval("DisplayStart") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>End Time</td>
                        <td><%# Eval("DisplayEnd") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Duration</td>
                        <td><%# Eval("Duration") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Tech</td>
                        <td><%# Eval("Tech") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Name</td>
                        <td><%# Eval("Name") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel ID="pnTravelLabor" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbTravelLabor" runat="server" Text="Travel Labor" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpTravelLabor" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Start Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
                    <tr>
                        <td>Start Time</td>
                        <td><%# Eval("DisplayStart") %></td>
                    </tr>
                    <tr>
                        <td>End Time</td>
                        <td><%# Eval("DisplayEnd") %></td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td><%# Eval("Duration") %></td>
                    </tr>
                    <tr>
                        <td>Tech</td>
                        <td><%# Eval("Tech") %></td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td><%# Eval("Name") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Start Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Start Time</td>
                        <td><%# Eval("DisplayStart") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>End Time</td>
                        <td><%# Eval("DisplayEnd") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Duration</td>
                        <td><%# Eval("Duration") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Tech</td>
                        <td><%# Eval("Tech") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Name</td>
                        <td><%# Eval("Name") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

    </asp:Panel>

    <asp:Panel ID="pnNotes" runat="server" Visible="false" style="margin-bottom: 20px;">
        <center><asp:Label ID="lbNotes" runat="server" Text="Notes" SkinID="TableTitle1" /></center>
        <asp:Repeater ID="rpNotes" runat="server">
            <HeaderTemplate>
                <table class="tableWithLines" style="width:100%;">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>Subject</td>
                        <td><%# Eval("DisplaySubject") %></td>
                    </tr>
                    <tr>
                        <td>Note</td>
                        <td><%# Eval("DisplayMessage") %></td>
                    </tr>
                    <tr>
                        <td>Emp</td>
                        <td><%# Eval("DisplayEmp") %></td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td><%# Eval("DisplayName") %></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                    <tr bgcolor="#eeeeee">
                        <td>Subject</td>
                        <td><%# Eval("DisplaySubject") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Note</td>
                        <td><%# Eval("DisplayMessage") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Emp</td>
                        <td><%# Eval("DisplayEmp") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Name</td>
                        <td><%# Eval("DisplayName") %></td>
                    </tr>
                    <tr bgcolor="#eeeeee">
                        <td>Date</td>
                        <td><%# Eval("DisplayDate") %></td>
                    </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

    </asp:Panel>

<div style="clear: both; height: 5px;"></div>

<asp:Panel ID="pnPhotos" runat="server">
    <div style="float: left; padding-right: 8px; padding-bottom: 8px; max-width: 350px;">
        <asp:ImageButton ID="imbt_Image1" runat="server" Width="350" AlternateText=" " OnClick="lkImage_Click" />
        <br /><asp:Label ID="lb_Image1" runat="server" />
    </div>
    <div style="float: left; padding-right: 8px; padding-bottom: 8px; max-width: 350px;">
        <asp:ImageButton ID="imbt_Image2" runat="server" Width="350" AlternateText=" " OnClick="lkImage_Click" />
        <br /><asp:Label ID="lb_Image2" runat="server" />
    </div>
    <div style="float: left; padding-right: 8px; padding-bottom: 8px; max-width: 350px;">
        <asp:ImageButton ID="imbt_Image3" runat="server" Width="350" AlternateText=" " OnClick="lkImage_Click" />
        <br /><asp:Label ID="lb_Image3" runat="server" />
    </div>
</asp:Panel>


<%-- 

 --%>
    
    </div>
    <asp:HiddenField ID="hfCtr" runat="server" />
    <asp:HiddenField ID="hfTck" runat="server" />

    </form>
</body>
</html>
