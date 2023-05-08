<%@ Page Title="STS Utilities Menu" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" 
    Inherits="private_shared_Menu" %>
<%--   --%>    

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%; margin: 0px; padding: 0px;">
        <tr>
            <td><asp:Label ID="lbMenuTitle" runat="server" /></td>
                
            <td style="text-align: right;">
            <asp:Panel ID="pnCs1Change" runat="server" DefaultButton="btCs1Change">
                    <asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" ValidationGroup="Cs1Change" />
                    <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txCs1Change"
                        ErrorMessage="Customer entry must be a number" 
                        Text="*"
                        SetFocusOnError="true" 
                        ValidationGroup="Cs1Change">
                    </asp:CompareValidator>
                    <asp:Button ID="btCs1Change" runat="server" 
                        Text="Change Cust" 
                        onclick="btCs1Change_Click"
                        ValidationGroup="Cs1Change" />
                    <asp:ValidationSummary ID="vSumCs1Change" runat="server" ValidationGroup="Cs1Change" />
            </asp:Panel>
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lbCs1Name" runat="server" SkinID="labelTitleColor2_Medium" /></td>
        </tr>
    </table>
</asp:Content>
    
<%-- --%>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<%--
     <table class="tbBorder" style="background-color: #EEEEEE; color: #777777; border: 2px solid #cccccc; width: 98%; margin-bottom: 15px; margin-top: 10px; line-height: 24px;">
        <tr>
            <td style="font-size: 18px; padding: 15px;">
                As the public portion of our website (our home and static informational pages) will be moving soon to the new <a href="https://www.scantron.com" >Scantron.com</a> site, 
                our ServiceCOMMAND Login and utililites will remain here in place. 
                <br /><span style="font-size: 13px; font-style: italic;"> A link to this ServiceCOMMAND page will be found on the new home page on the top right navigation area -- within the "Client Portals" drop down list.</span>
            </td>
        </tr>
    </table>
    3a7728
    36 and 30
     --%>        

        <asp:Panel ID="pnNewSiteNotice" runat="server" Visible="false">
         <span style="font-size: 16px; line-height: 28px; color: #333333">
        <table class="tableWithoutLines tableBorder" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; background-color: #ffffff;" cellspacing="10">
            <tr>
                <td style="padding-right: 30px; font-size: 26px; color: #ad0034">Announcing:</td>
                <td style="font-size: 26px; color: #333333">
                    Our New ServiceCOMMAND<span style='font-size: 11px; vertical-align: top; position: relative; top: -4px;'>®</span> Website
                    <div style="display:block; float:right;">
                        <asp:Button ID="btHideNewSiteNotice" runat="server" Text="Hide New Site Notice" OnClick="btHideNewSiteNotice_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan=2 style="height:1px; border-top: 1px solid #bbbbbb;"></td>
            </tr>
            <tr>
                <td style="padding-right: 36px; font-size: 18px; color: #406080">Where?</td>
                <td style="">
                    <asp:HyperLink ID="hlNewSite" runat="server" Target="_sc" NavigateUrl="https://www.servicecommand.com/private/menu.aspx" Font-Size="18">ServiceCOMMAND.com</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="padding-right: 36px; font-size: 18px; color: #406080">When?</td>
                <td>
                    <asp:Panel ID="pnGoNow" runat="server">
                    <%-- 
                        Now!  We encourage you to go ahead and register for the new site.  However, you are still free to use this older site <b>until <asp:Label ID="lbMoveCutoff" runat="server" /></b>
                        --%>
                    <asp:Label ID="lbReadyNow" runat="server" Text="It's ready now!" />  <asp:Label ID="lbRetiringSoon" runat="server" Text="We encourage you to register for the new site today since access to this site will end" /> <asp:Label ID="lbMoveCutoff" runat="server" />
                    </asp:Panel>
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 36px; font-size: 18px; color: #406080">Access? </td>
                <td>
                    Will the new site accept my current credentials?  <br />&nbsp;&nbsp;&nbsp;&nbsp;Unfortunately, no. Due to increased security, you will need to register again.  <br />What will I enter? <br />&nbsp;&nbsp;&nbsp;&nbsp;Your login name will be an active email address, <br />&nbsp;&nbsp;&nbsp;&nbsp;and to identify your company, <br />&nbsp;&nbsp;&nbsp;&nbsp;please use <span style="font-size: 24px; font-weight: bold;"><asp:Label ID="lbLoginCompanyId" runat="server" /></span> when asked to enter your 'Customer Number'</td>
            </tr>
            <tr>
                <td style="padding-right: 36px; font-size: 18px; color: #406080">Questions?</td>
                <td>
                    <div style="display: block; float:left; padding-right: 5px;">For assistance please email</div>  
                    <div style="display: block; float:left; padding-right: 10px;"><a style='color: blue;' href="mailto:servicecommandsupport@scantron.com">Service Command Support</a></div>
                    <%--   <div style="display: block; float:left; padding-right: 10px;">or call 800.228.3628 and ask to speak to "ServiceCOMMAND support"</div>--%>
                    </td>
            </tr>
        </table>
        </span>
        <div style="clear: both; height: 10px;"></div>
    </asp:Panel>

    <asp:Panel ID="pnRegLrgDlr" runat="server">

    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                Text="Service Request" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/req/location.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            This feature allows our customers to place requests for service using their web browser. After selecting the desired customer location and equipment from a real time display, the user provides relevant call information and submits the request. The ticket is then dispatched to the proper Field Service Technician via our Mobile Service Delivery system and an optional email acknowledgement is sent to a user-provided address.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink19" runat="server" 
                                Text="Service History" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ServiceHistory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Clock.png" alt=""  class="menuImage"  />
                            This feature allows our customers the ability to display or download real-time ticket history information. A ticket filter screen allows the user to select the specific request types or time frame desired. The user is provided information ranging from basic problem description and location to event chronology and parts used.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <asp:HyperLink ID="HyperLink3" runat="server" 
                                Text="Open Tickets" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/OpenTickets.aspx" />
                                <div class="spacer5"></div>
                                <img src="/media/scantron/art/menu/green/CameraView.png" alt="" class="menuImage" />
                                This feature allows a quick view of all currently active tickets for all customer locations.
                            <div class="spacer0"></div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            
                            <asp:HyperLink ID="HyperLink2" runat="server" 
                            Text="Ticket Status" 
                                SkinID="hyperLinkHeader" 
                            NavigateUrl="~/private/sc/TicketStatus.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Magnifier.png" alt="" class="menuImage"  />
                            This feature allows the user to retrieve information relative to a specific service ticket. The information provided is identical to that produced by the Service History utility.
                            <div class="spacer5"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink28" runat="server" 
                                Text="Call Escalation" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/public/Escalation.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Flag.png" alt=""  class="menuImage"  />
                            Call escalation is part of our overall customer experience. Members of our Call Center team are empowered to escalate calls quickly and efficiently should the need arise.
                            <div class="spacer1"></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; padding: 20px;">
                <table>
                    <tr>
                        <td>
                            
                            <asp:HyperLink ID="HyperLink5" runat="server" 
                                Text="Contract Equipment" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ContractEquipment.aspx" />
                            <div style="display:block; float:right;"><asp:Button ID="btShowNewSiteNotice" runat="server" Text="Show New Site Notice" OnClick="btShowNewSiteNotice_Click" Visible="false" /></div>
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Monitor.png" alt=""  class="menuImage" />
                                This feature displays the equipment under contract with us at a given location.
                            <div class="spacer0"></div>                           
                        </td>
                    </tr>

                    <tr>
                        <td>                          
                            <asp:HyperLink ID="HyperLink6" runat="server" 
                                Text="Email Management" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/EmailManagement.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/EmailArrowLeft.png" alt=""  class="menuImage" />
                                With this utility, customers can elect to have email acknowledgements sent to an address or addresses of their choosing when a service request is opened and when it is closed. The two events, call opening and call closing, can be managed independently of one another and can be enabled for activity at all locations or for a specific location.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>                         
                            <asp:HyperLink ID="HyperLink27" runat="server"  
                                Text="Maintenance Coverage" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/Coverage.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Paint.png" alt=""  class="menuImage" />
                                Search by city or zip code for the type of maintenance coverage offered in that area.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                            <asp:HyperLink ID="HyperLink7" runat="server"  
                                Text="Comments" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/Comments.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Pencil.png" alt=""  class="menuImage"  />
                            With this utility, customers are able to submit comments related to any aspect of Scantron Technology Solutions' service. For routing purposes, users are asked to select one or more categories for their comments. If desired, a response from the appropriate Scantron Technology Solutions manager may be requested.
                            <div class="spacer15"></div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink8" runat="server" 
                            Text="Customer Administration" 
                                SkinID="hyperLinkHeader" 
                            NavigateUrl="~/private/custAdmin/Menu.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Locked.png" alt=""  class="menuImage"  />
                            This special menu (which includes the Change Password Utility) is reserved for the individual(s) at your company responsible for managing your service agreement with Scantron Technology Solutions. Such individual(s) should call (800) 228-3628 to speak with their Scantron Technology Solutions sales representative about establishing an Administrator Account.
                            <div class="spacer5"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>

  <asp:Panel ID="pnDlrRenewals" runat="server">

    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink20" runat="server" 
                                Text="Service Request" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/req/location.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            This feature allows our customers to place requests for service using their web browser. After selecting the desired customer location and equipment from a real time display, the user provides relevant call information and submits the request. The ticket is then dispatched to the proper Field Service Technician via our Mobile Service Delivery system and an optional email acknowledgement is sent to a user-provided address.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink21" runat="server" 
                                Text="Service History" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ServiceHistory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Clock.png" alt=""  class="menuImage"  />
                            This feature allows our customers the ability to display or download real-time ticket history information. A ticket filter screen allows the user to select the specific request types or time frame desired. The user is provided information ranging from basic problem description and location to event chronology and parts used.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <asp:HyperLink ID="HyperLink22" runat="server" 
                                Text="Open Tickets" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/OpenTickets.aspx" />
                                <div class="spacer5"></div>
                                <img src="/media/scantron/art/menu/green/CameraView.png" alt="" class="menuImage" />
                                This feature allows a quick view of all currently active tickets for all customer locations.
                            <div class="spacer0"></div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            
                            <asp:HyperLink ID="HyperLink23" runat="server" 
                            Text="Ticket Status" 
                                SkinID="hyperLinkHeader" 
                            NavigateUrl="~/private/sc/TicketStatus.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Magnifier.png" alt="" class="menuImage"  />
                            This feature allows the user to retrieve information relative to a specific service ticket. The information provided is identical to that produced by the Service History utility.
                            <div class="spacer5"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink24" runat="server" 
                                Text="Call Escalation" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/public/Escalation.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Flag.png" alt=""  class="menuImage"  />
                            Call escalation is part of our overall customer experience. Members of our Call Center team are empowered to escalate calls quickly and efficiently should the need arise.
                            <div class="spacer1"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink26" runat="server" 
                                Text="Renewals" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/DealerCustomers.aspx" Visible="true" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/StackPlus.png" alt="" class="menuImage" />
                            View and filter all contract equipment with associated renewal dates.
                            <div class="spacer0"></div>                           
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; padding: 20px;">
                <table>
                    <tr>
                        <td>                          
                            <asp:HyperLink ID="HyperLink32" runat="server" 
                                Text="Contract Equipment" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ContractEquipment.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Monitor.png" alt=""  class="menuImage" />
                                This feature displays the equipment under contract with us at a given location.
                            <div class="spacer0"></div>                           
                        </td>
                    </tr>

                    <tr>
                        <td>                          
                            <asp:HyperLink ID="HyperLink33" runat="server" 
                                Text="Email Management" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/EmailManagement.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/EmailArrowLeft.png" alt=""  class="menuImage" />
                                With this utility, customers can elect to have email acknowledgements sent to an address or addresses of their choosing when a service request is opened and when it is closed. The two events, call opening and call closing, can be managed independently of one another and can be enabled for activity at all locations or for a specific location.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>                         
                            <asp:HyperLink ID="HyperLink37" runat="server"  
                                Text="Maintenance Coverage" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/Coverage.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Paint.png" alt=""  class="menuImage" />
                                Search by city or zip code for the type of maintenance coverage offered in that area.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                            <asp:HyperLink ID="HyperLink38" runat="server"  
                                Text="Comments" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/Comments.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Pencil.png" alt=""  class="menuImage"  />
                            With this utility, customers are able to submit comments related to any aspect of Scantron Technology Solutions' service. For routing purposes, users are asked to select one or more categories for their comments. If desired, a response from the appropriate Scantron Technology Solutions manager may be requested.
                            <div class="spacer15"></div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink39" runat="server" 
                            Text="Customer Administration" 
                                SkinID="hyperLinkHeader" 
                            NavigateUrl="~/private/custAdmin/Menu.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Locked.png" alt=""  class="menuImage"  />
                            This special menu (which includes the Change Password Utility) is reserved for the individual(s) at your company responsible for managing your service agreement with Scantron Technology Solutions. Such individual(s) should call (800) 228-3628 to speak with their Scantron Technology Solutions sales representative about establishing an Administrator Account.
                            <div class="spacer5"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>

<%--  SELF SERVICE SSP MENU (Parts Command Only) ==============================================================================   --%>
<asp:Panel ID="pnSelfServiceP" runat="server">

    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink29" runat="server" 
                                Text="Current Inventory" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/CurrentInventory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            View the inventory currently assigned to you in our files.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink30" runat="server" 
                                Text="Auto Replenishment" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/AutoReplenish.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Paint.png" alt="" class="menuImage"  />
                            View and maintain parts currently assigned to your automatic replenishment list. 
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink31" runat="server" 
                                Text="Parts Book" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/PartsBook.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Monitor.png" alt="" class="menuImage"  />
                            Search our catalog of available parts.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; padding: 20px;">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink34" runat="server" 
                                Text="Surplus Inventory" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/SurplusInventory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/StackArrow.png" alt="" class="menuImage"  />
                            View used inventory needing to be returned to STS. 
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink35" runat="server" 
                                Text="Shipment Tracking" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/Tracking.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ColorBars.png" alt="" class="menuImage"  />
                            View data related to shipments to a selected stocking location.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink36" runat="server" 
                                Text="Enter Parts Used" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/PartsUsed.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Pencil.png" alt="" class="menuImage"  />
                            Complete your part use reporting online.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

<%--  SELF SERVICE SSB MENU (Both Parts Command and Service Command) ==========================================================================   --%>
<asp:Panel ID="pnSelfServiceB" runat="server">

    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">
                <asp:Label ID="lbSsPartsCommand" runat="server" Text="PartsCOMMAND" SkinID="labelTitleColor1_Medium" />
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink4" runat="server" 
                                Text="Current Inventory" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/CurrentInventory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            View the inventory currently assigned to you in our files.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink9" runat="server" 
                                Text="Surplus Inventory" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/SurplusInventory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/StackArrow.png" alt="" class="menuImage"  />
                            View used inventory needing to be returned to STS. 
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink12" runat="server" 
                                Text="Auto Replenishment" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/AutoReplenish.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Paint.png" alt="" class="menuImage"  />
                            View and maintain parts currently assigned to your automatic replenishment list. 
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink10" runat="server" 
                                Text="Shipment Tracking" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/Tracking.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ColorBars.png" alt="" class="menuImage"  />
                            View data related to shipments to a selected stocking location.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink11" runat="server" 
                                Text="Parts Book" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/PartsBook.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Monitor.png" alt="" class="menuImage"  />
                            Search our catalog of available parts.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink15" runat="server" 
                                Text="Enter Parts Used" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/ss/PartsUsed.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Pencil.png" alt="" class="menuImage"  />
                            Complete your part use reporting online.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; padding: 20px;">
                <asp:Label ID="lbServiceCommand" runat="server" Text="ServiceCOMMAND" SkinID="labelTitleColor1_Small" />
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink14" runat="server" 
                                Text="Service Request" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/req/location.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            This feature allows our customers to place requests for service using their web browser. After selecting the desired customer location and equipment from a real time display, the user provides relevant call information and submits the request. The ticket is then dispatched to the proper Field Service Technician via our Mobile Service Delivery system and an optional email acknowledgement is sent to a user-provided address.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink18" runat="server" 
                                Text="Service History" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ServiceHistory.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Clock.png" alt="" class="menuImage"  />
                            This feature allows our customers the ability to display or download real-time ticket history information. A ticket filter screen allows the user to select the specific request types or time frame desired. The user is provided information ranging from basic problem description and location to event chronology and parts used.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink25" runat="server" 
                                Text="Open Tickets" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/OpenTickets.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/CameraView.png" alt="" class="menuImage"  />
                            This feature allows a quick view of all currently active tickets for all customer locations.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink13" runat="server" 
                                Text="Ticket Status" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/TicketStatus.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Magnifier.png" alt="" class="menuImage"  />
                            This feature allows the user to retrieve information relative to a specific service ticket. The information provided is identical to that produced by the Service History utility.
                            <div class="spacer10"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink16" runat="server" 
                                Text="Contract Equipment" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/ContractEquipment.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Monitor.png" alt="" class="menuImage"  />
                            This feature allows our customers to view the equipment under contract with us at a given location.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="HyperLink17" runat="server" 
                                Text="Email Management" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/sc/EmailManagement.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/EmailArrowLeft.png" alt="" class="menuImage"  />
                            With this utility, customers can elect to have email acknowledgements sent to an address or addresses of their choosing when a service request is opened and when it is closed. The two events, call opening and call closing, can be managed independently of one another and can be enabled for activity at all locations or for a specific location.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Panel>
<%--    --%>

<div style="width: 100%; text-align: center; font-size: 0.9em; color: #406080;">
    <asp:Label ID="lbAccessCode" runat="server" Visible="false" />
</div>

    <asp:Label ID="lbDebug" runat="server" />
</asp:Content>

