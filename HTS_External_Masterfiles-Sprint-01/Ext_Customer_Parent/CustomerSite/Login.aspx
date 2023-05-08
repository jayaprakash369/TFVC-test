﻿<%@ Page Title="STS Login Page" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" 
    Inherits="Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%--   --%>    

<asp:Content ID="Content1" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_A" Runat="Server">
    
    <%--   
    <div style="width:960px; margin:0px auto;" > 
         <asp:HyperLink ID="ImageButton1" runat="server" ImageUrl="~/media/scantron/images/banners/HTS-rename-STS-banner.jpg" NavigateUrl="~/media/scantron/pdf/StsRenameInfo.pdf" Target="Namechange" />
    </div>
        --%>    
</asp:Content>
    <%-- 
         Height="15" BackColor="Wheat"
<asp:Content ID="Content2" ContentPlaceHolderID="ParentBannerArea" Runat="Server">
</asp:Content>
             <table style="width: 100%; margin: 0px; padding: 0px;">
        <tr>
            <td></td>
        </tr>
         </table>
          --%>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <asp:Label ID="lbMenuTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <asp:Panel ID="pnLogin" runat="server" 
        DefaultButton="btLogin">
        
        <table class="tableAsFence">
            <tr style="vertical-align: top;">
                <td style="padding-right: 30px;">
                        <asp:Panel ID="pnLoginBox" runat="server">  
                            <table class="tableWithoutLines">
                                <tr>
                                    <td>Username</td>
                                    <td>
                                        <asp:TextBox ID="txUserID" runat="server" 
                                            AutoCompleteType="Disabled" 
                                            Width="120" />
                                        <asp:RequiredFieldValidator ID="vReqUserID" runat="server" 
                                            ControlToValidate="txUserID" 
                                            ErrorMessage="User ID is required." 
                                            Text="*"
                                            ValidationGroup="Login" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password</td>
                                    <td>
                                        <asp:TextBox ID="txPassword" runat="server"
                                            TextMode="Password" 
                                            Width="120"
                                            MaxLength="15"
                                            AutoCompleteType="Disabled" />
                                        <asp:RequiredFieldValidator ID="vReqPassword" runat="server" 
                                            ControlToValidate="txPassword" 
                                            ErrorMessage="Password is required." 
                                            Text="*"
                                            ValidationGroup="Login" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <div class="spacer10"></div>
                                        <asp:Button ID="btLogin" runat="server" 
                                            Text="Continue" 
                                            ValidationGroup="Login" 
                                            onclick="btLogin_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                            <asp:CustomValidator id="vCusLogin" runat="server" 
                                                Display="None" 
                                                EnableClientScript="False"
                                                ValidationGroup="Login" />
                                            <asp:Label ID="lbMsg" runat="server" Text="" SkinId="labelTitleColor1_Small" />
                                            <div style="height: 10px; clear: both;"></div>
                                            <asp:ValidationSummary ID="vSumLogin" runat="server" ValidationGroup="Login" />

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                            <%-- 
                        
                        --%>
                    </td><!-- end left column -->

                <td style="width: 640px; padding-right: 10px;">
<%--  BEGIN: NOTICE SECTION 
            <div class="spacer15"></div>
     --%>    

<asp:Panel ID="pnNotice" runat="server" Visible="false">
<div style="margin: 30px;">
    <center>
    <asp:Panel ID="pnMsgBox" runat="server"
        Width="600"
        BackColor="#333333">
        <center>
            <table style="width: 100%; padding: 15px;">
                <tr>
                    <td style="color: #eeeeee; font-size: 28px; font-weight: bold; text-align: center; line-height: 30px; padding-top: 10px;">
                        ServiceCOMMAND Advisory 
                    </td>
                </tr>
                <tr>
                    <td><center>
                        <div style="margin: 15px; padding: 10px; border: 1px solid #000000; background-color: #ffffff; color: #333333; font-size: 16px; font-weight: bold; line-height: 25px;">
                            <asp:Label ID="lbMaintenance" runat="server" />
                        </div>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td style="color: #ffffff; text-align: left; padding-left: 15px; font-size: 15px;">
                        <center>
                        <p> Please direct service issues to our contact center at<br /> 800.228.3628.</p>
                        </center>
                    </td>
                </tr>
            </table>
        </center>

    <asp:DropShadowExtender ID="DropShadowExtender1" runat="server" 
        TargetControlID="pnMsgBox"
        Width="7" 
        Rounded="true" 
        Radius="10" 
        Opacity="0.5" 
        TrackPosition="true" />
</asp:Panel>
<%--  END: NOTICE SECTION  --%>    
   </center>
    </div>
</asp:Panel>
    <p>Whether you place your call over the phone or via the internet, Scantron Technology Solutions' ServiceCOMMAND® offers you the ability to track your call from placement to completion, to view tickets statuses, and to register for auto-email notifications.</p>

    <h4>Online Service Call Placement</h4>
    <p>When you need to place a service request, simply login to your account through ServiceCOMMAND® to get started. You'll then select your location and equipment from a real-time display and provide contact information.</p>

    <h4>Call Dispatching</h4>
    <p>Once you have placed your service request, the call is dispatched to a Field Service Technician via his/her Android®. If you requested an email acknowledgement, it will be sent to you at this time.</p>

    <h4>Reporting Capabilities</h4>
    <p>Display and/or download call history information by customer number and access a complete list of equipment covered under your agreement by location. </p>

    <h4>Ticket Status</h4>
    <p>Research information relative to a specific ticket via the STS ticket number or your ticket cross reference number.</p>

    <h4>Auto Email Reporting</h4>
    <p>You can elect to have email notifications sent to an address(es) of your choice. When a ticket is opened or closed, an alert will be sent to the email address that is entered. </p>
<%-- background-color: #eeeeee; border: 1px solid #aaaaaa; vertical-align: top; max-width: 262px;" --%>    
                    </td>
                </tr>
            </table>
        </asp:Panel>

</asp:Content>



