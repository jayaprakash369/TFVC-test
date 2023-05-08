<%@ Page Title="ServiceCommand Utility Descriptions" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" 
    Inherits="private_Menu" %>
<%--   --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
	<%--   
		// Phone was 412x915
	<script type="text/javascript">
        function getResolution() {
            alert("Your screen resolution is: " + screen.width + "x" + screen.height);
        }
    </script>
		--%>    
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
	<div style="float: left; padding-right: 1px;">ServiceCOMMAND<span style='font-size: 18px; vertical-align: top; position: relative; top: 2px;'>®</span></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
	<div class="bodyPadding">

	<asp:Panel ID="pnServright" runat="server" Visible="false">
			<%--  
				~/media/images/triangles600.jpg
				~/media/images/BuildingCurvedColor.png
				~/media/images/TreeAtSunrise.png
				--%>    
	    <div class="w3-row w3-padding-32">
		    <div class="w3-twothird w3-container">
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="ServrightOpenTickets" runat="server" Text="Open Service Tickets" NavigateUrl="~/private/pre/OpenTickets.aspx" /><br />This feature allows a quick view of all currently active tickets for all customer locations.</p>
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="ServrightServiceHistory" runat="server" Text="Service History" NavigateUrl="~/private/pre/ServiceHistory.aspx" /><br />This option allows a review of all tickets over the last six months.</p>
			<%--  
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="ServrightTicketUpdate" runat="server" Text="Update Service Tickets" NavigateUrl="~/private/pre/UpdateTickets.aspx" /><br />This feature allows to View/Update a service ticket.</p>	
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="ServrightTicketHistory" runat="server" Text="Ticket History" NavigateUrl="~/private/sc/TicketHistory.aspx" /><br />This feature allows a quick view of Service History for the last 6 months.</p>	
				--%>    
	        </div>

			<div class="w3-third w3-container">
				<p style="margin-bottom: 20px;">
					<asp:Image ID="Image1" runat="server" ImageUrl="~/media/images/TreeAtSunrise.png" style="width:100%; border: 1px solid #999999;" />
				</p>
			  <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight">
				  <asp:HyperLink ID="ServrightChangePassword" runat="server" Text="Change Password" NavigateUrl="~/private/ChangeYourPassword.aspx" /><br />If you know your current password and would like to change it, this utility will allow the update.
			  </p>

			</div>
		</div>

	</asp:Panel> 


	<asp:Panel ID="pnRegular" runat="server" Visible="false">


    <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">
			
			  <p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink1" runat="server" Text="Service Request" NavigateUrl="~/private/sc/ServiceRequest.aspx" /><br />This feature allows our customers to submit requests for service using their web browser. After selecting the desired customer location and equipment from a real time display, the user provides relevant ticket information and submits the request. The ticket is then dispatched to the proper support or field service technician.</p>
			<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink3" runat="server" Text="Open Service Tickets" NavigateUrl="~/private/sc/OpenTickets.aspx" /><br />This feature allows a quick view of all currently active tickets for all customer locations.</p>  
			<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink2" runat="server" Text="Service History" NavigateUrl="~/private/sc/ServiceHistory.aspx" /><br />This feature allows our customers the ability to display or download real-time ticket history information. A ticket filter screen allows the user to select the specific request types or time frame desired. The user is provided information ranging from basic problem description and location to event chronology and parts used.</p>

			  <p style="margin-bottom: 20px;"><asp:HyperLink ID="hlTicketStatus" runat="server" Text="Service Ticket Status" NavigateUrl="~/private/sc/TicketStatus.aspx" /><br />This feature allows the user to retrieve information relative to a specific service.</p>

			<asp:Panel ID="pnEmailManagementLink" runat="server">
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink5" runat="server" Text="Email Management" NavigateUrl="~/private/sc/EmailManagement.aspx" /><br />With this utility, customers can elect to have email acknowledgements sent to an address or addresses of their choosing when a service request is opened and when it is closed. The two events, opening and closing the ticket, can be managed independently of one another and can be enabled for activity at all locations or for a specific location.</p>
			</asp:Panel>
            
			<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink4" runat="server" Text="Agreements" NavigateUrl="~/private/sc/Agreements.aspx" /><br />This displays all your active customer agreements.</p>
			<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink6" runat="server" Text="Agreement Detail" NavigateUrl="~/private/sc/AgreementLocations.aspx" /><br />This feature displays the equipment and services under contract with us at a given location.</p>

			<%--  
			<asp:Panel ID="pnInvoiceLink" runat="server">
				<p style="margin-bottom: 20px;"><asp:HyperLink ID="hlInvoices" runat="server" Text="Invoices" NavigateUrl="~/private/sc/Invoices.aspx" /><br />Recent invoices and their line item detail can be accessed through this feature.</p>
            </asp:Panel>
				--%>    

			<p style="margin-bottom: 20px;"><asp:HyperLink ID="HyperLink7" runat="server" Text="Comments" NavigateUrl="~/private/sc/Comments.aspx" /><br />With this utility, customers are able to submit comments related to any aspect of <asp:Label ID="lbCompanyName1" runat="server" /> service. For routing purposes, users are asked to select one or more categories for their comments. If desired, a response from the appropriate <asp:Label ID="lbCompanyName3" runat="server" /> manager may be requested.</p>			  
			<!--
			<p style="margin-bottom: 20px;"><asp:HyperLink ID="hlEsc" runat="server" Text="Call Escalation" NavigateUrl="~/private/sc/Escalation.aspx" /><br />Call escalation is part of our overall customer experience. Members of our Call Center team are empowered to escalate calls quickly and efficiently should the need arise.</p>
			-->
        </div>

        <div class="w3-third w3-container">
			<!-- 
			<p style="margin-bottom: 20px;" class="w3-border w3-padding-large w3-padding-32 w3-center"><asp:Image ID="Image2" runat="server" ImageUrl="~/media/images/frog.png" style="width:100%" /></p>  
				-->
			<p style="margin-bottom: 20px;">
                  <asp:Image ID="imTriangles" runat="server" ImageUrl="~/media/images/triangles600.jpg" style="width:100%; border: 1px solid #999999;" /></p>

			  <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><asp:HyperLink ID="HyperLink8" runat="server" Text="Onsite Service Coverage" NavigateUrl="~/public/sc/MaintenanceCoverage.aspx" /><br />Search by city or zip code for the type of service coverage offered in that area.</p>

		      <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><asp:HyperLink ID="hlEscalation" runat="server" Text="Escalation" NavigateUrl="~/private/sc/Escalation.aspx" /><br />Service Request Escalation is part of our overall customer experience. Members of our contact center team are empowered to escalate service tickets quickly and efficiently should the need arise.</p>

				<!--
				<p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><asp:HyperLink ID="HyperLink11" runat="server" Text="Invoices" NavigateUrl="~/private/sc/Invoices.aspx" /><br />Recent invoices and their line item detail can be accessed through this feature.</p>
				-->


			<asp:Panel ID="pnCustomerAdministrationLink" runat="server">
			  <p style="margin-bottom: 20px;" class="w3-border w3-padding w3-left-align tableBorderBackgroundLight"><asp:HyperLink ID="HyperLink9" runat="server" Text="Customer Administration" NavigateUrl="~/private/customerAdministration/CustomerAdministrationMenu.aspx" /><br />This special menu (which includes the Change Password Utility) is reserved for the individual(s) at your company responsible for managing your service agreement with <asp:Label ID="lbCompanyName2" runat="server" />. Such individual(s) should reach out to <a style='color: cornflowerblue; font-size: 14px;' href="mailto:servicecommandsupport@scantron.com">ServiceCOMMAND Support</a> to upgrade to an Administrative Account.</p>
			</asp:Panel>
        </div>

    </div>
	</asp:Panel> 

          <asp:Panel ID="pnAdmin" runat="server" Visible="false">            
				<table class="tableBorderBackgroundLight tableWithoutLines" style="margin-bottom: 50px; background-color:#f5f5f5;">
					<tr style="vertical-align: top;">
						<td style="width: 25%; padding:15px;">
					        <asp:Label ID="lbAdminCust" runat="server" />
							<div class="spacer5"></div>
							<b><asp:Label ID="lbCustomerInUse" runat="server" /></b>
							<div class="spacer5"></div>
					        <asp:Label ID="lbAccountType" runat="server" />
							<div class="spacer5"></div>

							<asp:TextBox ID="txAdminCustomerNumber" runat="server" Width="80" />
							<div class="spacer10"></div>
							<asp:Button ID="btAdminCustomerSubmit" runat="server" Text="Submit" OnClick="btAdminCustomerSubmit_Click" SkinId="buttonPrimary" />
						</td>
						<td style="width: 75%; padding:15px;">
							<ul style="padding-left: 10px;">
								<li><b>Regular</b> Town of Shirley: 56341, Container Store: 89866, ABC Supply: 105159, Cinemark: 79206</li>
								<li><b>Connectwise</b> Hancock & Dana: 118562, Oakland Financial: 119581, Advanced Derm: 119683,  Access Bank: 119550, Bravo Sports: 164103  </li>
								<li><b>Large</b> Midcom: 121, KeyBank: 125, Arca: 127</li>
								<li><b>Dealer</b> ECI-Spruce: 2635, Synnex: 3040, FP Mailing: 3039</li>
								<li><b>Grandparent</b> 12084, 1565</li>
								<li><b>Parent</b> 1077, 1028</li>
								<li><b>Child</b> 6034, 6858, </li>
							</ul>
						</td>
					</tr>
				</table>
			</asp:Panel>


    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />


</div>
</asp:Content>

