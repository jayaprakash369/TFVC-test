<%@ Page Title="Registration" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" %>
<%--   --%>    
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Registration
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <style type="text/css">
        .registrationTable {
            border: 1px solid #cccccc;
            background-color: #ffffff; /* #d8e4d4 -- 3a7728->d8e4d4 3a7268->d8e3e1  cornsilk beige LemonChiffon linen cornsilk */
            margin-bottom: 30px;
            margin-left: 0px; 
        }
        .registrationTable tr {
            vertical-align: top;
        }
        .registrationTable td {
            padding: 10px;
        }
           .registrationTextbox {
            padding-left: 5px;
        }

    </style>

    <div class="w3-row w3-padding-32">
        <div class=" w3-container">

		<div class="row">
			<div class="col-12">
                <div style="padding-left: 13px;">
                    <asp:Label ID="lbError" runat="server" SkinID="labelError" />
                    <asp:Label ID="lbDebug" runat="server" SkinID="labelMessage" />
                </div>
			</div>
		</div>
	</div>

    <!-- -->
    <div class="w3-row">

        <div class="w3-twothird w3-container">

            <div class="spacer10"></div>
            <%--   --%>    
            <asp:Panel ID="pnOne" runat="server" Visible="true" DefaultButton="btRegId">
                <table class="registrationTable">
                    <tr>
                        <td colspan="2">
                            <h3 class="w3-text-teal"><asp:Label ID="lbStep1" runat="server" Text="Step 1 of 3" /></h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="min-width: 80px;">
                            <asp:Label ID="lbCs1" runat="server" Text="Account ID" />
                        </td>
                        <td>
                            <asp:TextBox ID="txCs1" runat="server" CssClass="registrationTextbox" Width="250" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-size: 13px;">
                            <asp:Label ID="lbCustomerNumberInfo" runat="server" 
                                Text="<i>This is the STS ID which identifies your company account</i>"
                                Visible="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbAccess" runat="server"
                                Text="(Optional code for admin privileges)" 
                                Visible="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txCod" runat="server" 
                                TextMode="Password" 
                                MaxLength="30"
                                BackColor="#EEEEEE" 
                                Visible="false" 
                                Width="250"
                                CssClass="registrationTextbox"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-size: 13px;">
                            <asp:Label ID="lbAccessInfo" runat="server" 
                                Text="<i>If you happen to be the account administrator you may add administrative privileges to this account either now, or at a later time.  Just reach out to <a style='color: blue; font-style:normal' href='mailto:servicecommandsupport@scantron.com'>ServiceCOMMAND Support</a> and ask for help with 'Web Admin Registration'</i><br /><br /> Otherwise, please continue with basic registration by clicking the button below."
                                Visible="false" />
                            <hyperlink style="text-decoration: underline"></hyperlink>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btRegId" runat="server" 
                                Text="Continue" 
                                onclick="btRegId_Click" 
                                SkinID="buttonPrimary" 
                                />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfCod" runat="server" Visible="false" />
            </asp:Panel>
            <%--   --%>    

            <asp:Panel ID="pnTwo" runat="server" Visible="false" DefaultButton="btUserID">
                <table class="registrationTable">
                    <tr>
                        <td colspan="2">
                            <h3 class="w3-text-teal"><asp:Label ID="lbStep2" runat="server" Text="Step 2 of 3"  /></h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="min-width: 80px;">Account ID</td>
                        <td><asp:Label ID="lbStep2Cs1" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lbUserID" runat="server" Text="Email (for Login)" /></td>
                        <td><asp:TextBox ID="txUserID" runat="server" Width="250" MaxLength="50" CssClass="registrationTextbox"  /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-size: 13px;">
                            <asp:Label ID="lbUserFriendly" runat="server" 
                                Text="Please enter a valid email address (to be used as your login username.)" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btUserID" runat="server" Text="Check Availability" onclick="btUserID_Click"  SkinID="buttonPrimary" /></td>
                    </tr>
                </table>
            </asp:Panel>
 
            <%--   --%>    
            <asp:Panel ID="pnThree" runat="server" Visible="false" DefaultButton="btRegistrationSubmission">
                <table class="registrationTable">
                    <tr>
                        <td colspan="2"><h3 class="w3-text-teal"><asp:Label ID="lbStep3" runat="server" Text="Step 3 of 3"  /></h3></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="min-width: 80px;"><asp:Label ID="lbUserIDTitle" runat="server" Text="User ID" /></td>
                        <td><asp:Label ID="lbUserIDDisplay" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lbPwd" runat="server" Text="Account Password" /></td>
                        <td><asp:TextBox ID="txPassword" runat="server" TextMode="Password" Width="210" MaxLength="30" CssClass="registrationTextbox" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lbPwd2" runat="server" Text="Confirm Password" /></td>
                        <td><asp:TextBox ID="txPassword2" runat="server" TextMode="Password" Width="210" MaxLength="30" CssClass="registrationTextbox" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Label ID="lbPasswordRules" runat="server" Text="Password: 7 to 30 characters using uppercase, lowercase and a number" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lbFirstName" runat="server" AssociatedControlID="txFirstName" Text="First Name" /></td>
                        <td><asp:TextBox ID="txFirstName" runat="server" Width="210" MaxLength="50" CssClass="registrationTextbox" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lbLastName" runat="server" AssociatedControlID="txLastName" Text="Last Name" /></td>
                        <td><asp:TextBox ID="txLastName" runat="server" Width="210" MaxLength="50" CssClass="registrationTextbox" /></td>
                    </tr>
                    <asp:Panel ID="pnHideRegistrationEmail" runat="server" Visible="false">
                    <tr>
                        <td><asp:Label ID="lbPhone" runat="server" Text="Phone" /></td>
                        <td>
                            (<asp:TextBox ID="txPhone1" runat="server" Width="40" MaxLength="3" />)
                            <asp:TextBox ID="txPhone2" runat="server" Width="40" MaxLength="3" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="txPhone3" runat="server" Width="50" MaxLength="4" />
                            &nbsp; Ext: 
                            <asp:TextBox ID="txExtension" runat="server" Width="65" MaxLength="8" />
                        </td>
                    </tr>
                        <tr>
                            <td><asp:Label ID="lbEmail" runat="server" Text="Email" /></td>
                            <td><asp:TextBox ID="txEmail" runat="server" Width="210" MaxLength="50" /></td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="lbEmail2" runat="server" Text="Email Confirmation" /></td>
                            <td><asp:TextBox ID="txEmail2" runat="server" Width="210" MaxLength="50"  /></td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td><asp:Label ID="lbGrantAdmin" runat="server" Text="Grant Admin Privileges?" Visible="false" /></td>
                        <td><asp:CheckBox ID="chBxGrantAdmin" runat="server" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td></td>
                         <td><span class="w3-text-teal" style="font-size:18px;"><i><asp:Label ID="lbEmailConfirmation" runat="server" 
                             Text="A confirmation email will be sent to the address used for this account's User ID.  Please inform the new user of this email.  After they click the link to confirm, they may log in."
                           Visible="false" /></i></span>
                         </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btRegistrationSubmission" runat="server" Text="Submit Registration" onclick="btRegistrationSubmission_Click" SkinID="buttonPrimary" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:TextBox ID="access" runat="server" Visible="false" /></td>
                    </tr>
                </table>
            </asp:Panel>

            <%--   --%>    
        </div>
        <div class="w3-twothird w3-container">
        </div>
    
    <!-- -->


        <table id="tbContentBlock" class="contentBlock">
    <tr>
        <td>
            <asp:Panel ID="pnBodyRoundedFrame" runat="server" BackColor="White">
                <table id="tbBodyFrameExpander" runat="server" class="tableFrameExpander">
                    <tr>
                        <td style="padding-left: 7px; padding-right: 7px;">

                        <%-- START: INSIDE ROUNDED FRAME --%>

<!--  VALIDATION GROUP TWO  ================================================== -->
<%-- 

  --%>    
<!--  VALIDATION GROUP THREE: Registration Data ========================= -->

                        <%-- END: INSIDE ROUNDED FRAME --%>
                        </td>
                    </tr>
                </table><!-- end tbBodyFrameExpander -->
            </asp:Panel><!-- end pnBodyRoundedFrame -->          
        </td>
    </tr>
</table><!-- end tbContentBlock -->

            </div>
    </div>
        <asp:HiddenField ID="hfUserName" runat="server" />
        <asp:HiddenField ID="hfRegistrationPrefix" runat="server" Value="" />
        <asp:HiddenField ID="hfRegistrationNumber" runat="server" Value="" />
        <asp:HiddenField ID="hfRegistrationId" runat="server" Value="" />
        <asp:HiddenField ID="hfRegistrationUserType" runat="server" Value="" />

</div>
</asp:Content>

