<%@ Page Title="ServiceCommand Login" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" 
    Inherits="Login" %>
    <%--   --%>    

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
        <style type="text/css">
        .loginTable {
            /* border: 1px solid #cccccc; */
            /* background-color: #ffffff; */ /* was green d8e4d4,  gray eeeeee 
                            width: 100%;
            */
            margin-bottom: 15px;
        }
        .loginTable tr {
            vertical-align: top;
        }
        .loginTable td {
            padding: 5px;
            padding-left: 15px;
            padding-right: 15px;
        }
            .loginTextbox {
                padding-left: 5px;
                font-size: 14px;
                /* font-size: 13px;*/
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
        <%-- 
          --%>

    <!--style="font-size: 30px;" -->
    <!-- <span>ServiceCOMMAND<span style='font-size: 18px; vertical-align: top; position: relative; top: 2px;'>®</span> Login</span> -->
    Login
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

        <%-- 
          --%>

<!--
        <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">
            <h1 class="w3-text-teal">Heading</h1>
            <p>MasterPage: Responsive Master </p>
        </div>
        <div class="w3-third w3-container">
            <p class="w3-border w3-padding-large w3-padding-32 w3-center">AD</p>
            <p class="w3-border w3-padding-large w3-padding-64 w3-center">AD</p>
        </div>
    </div>

    <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">
        </div>
        <div class="w3-third w3-container">
        </div>
    </div>

    <div class="w3-row w3-padding-32">
        <div class="w3-third w3-container">
        </div>
        <div class="w3-twothird w3-container">
        </div>
    </div>
-->

    <div class="w3-row w3-padding-32">
        <div class="w3-third w3-container" style="min-width: 300px;">
            <!-- <h1 class="w3-text-teal">Login</h1> -->
            <asp:Panel ID="pnLoginBox" runat="server" DefaultButton="btLogin">  
                    <table class="loginTable tableBorderBackgroundLight">
                        <tr>
                            <td>Email
                                <div style="height: 3px; clear: both;"></div>
                                <asp:TextBox ID="txUserID" runat="server" CssClass="loginTextbox"
                                    Width="230" /> 
                            </td>
                        </tr>
                        <tr>
                            <td>Password
                                <div style="height: 3px; clear: both;"></div>
                                <asp:TextBox ID="txPassword" runat="server"
                                        CssClass="loginTextbox"
                                    TextMode="Password" 
                                    Width="230"
                                    MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td style="max-width: 110px;">
                                <asp:Button ID="btLogin" runat="server" 
                                    Text="Continue" 
                                    SkinID="buttonPrimary" 
                                    onclick="btLogin_Click" />
                                <div style="height: 10px; clear: both;"></div>
                                <span style="font-style: italic; font-size: 12px;" >
                                <asp:Label ID="lbTimeout" runat="server" Text="User sessions will time out after a period of inactivity"  />
                                </span>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbError" runat="server" SkinId="labelError"  />
                            </td>
                        </tr>
                    </table>
                <div class="spacer5"></div>
            </asp:Panel>

            <p><asp:HyperLink ID="hlTemporaryPassword" runat="server" Text="Need to reset a forgotten password?" Font-Italic="true" Font-Size="Medium" NavigateUrl="~/TemporaryPassword.aspx" /></p>
                            <div class="spacer5"></div>
            <p>Whether you submit your service request over the phone or via the internet, <asp:Label ID="lbCompanyName1" runat="server" /> ServiceCOMMAND® offers you the ability to track your request from submission to completion. <!--to view all open requests or retrieve detail from service history.--></p>
            <div class="spacer20"></div>

        </div>
        <div class="w3-twothird w3-container">
            <asp:Panel ID="pnNotice" runat="server">
                <p class="w3-border w3-padding-large w3-padding-16 w3-center-align myMaintenanceMessageTheme">
                    <span class="w3-text-sand" style="font-size: 24px;">Maintenance Notification</span>
                    <br />
                    <span class="w3-text-white" style="font-size: 16px;"><asp:Label ID="lbNotice" runat="server" /></span>
                </p>
            </asp:Panel>

        <p class="w3-border w3-padding-large w3-padding-16 w3-left-align tableBorderBackgroundLight" style="margin-bottom: 20px;">
              <span class="w3-text-teal">Online Service Request Submission</span><br />
              When you need to submit a service request, simply login to your ServiceCOMMAND® account to get started. You'll then select your location and equipment from a real-time display and provide contact information and summarize the service issue.
            <br /><br />
            <span class="w3-text-teal">Service Request Dispatching</span><br />
            Once you have submitted your service request, the ticket is dispatched to the appropriate field or support technician.
            <br /><br />
            <span class="w3-text-teal">Reporting Capabilities</span><br />
            Display and/or download service request history information by customer number.  Additionally, you may access a complete list of equipment and services covered under your agreement. 
            <br /><br />
            <span class="w3-text-teal">Service Ticket Status</span><br />
            Research information relative to a specific ticket via the <asp:Label ID="lbCompanyName2" runat="server" /> ticket number or your ticket cross reference number.
            <br /><br />
            </p>

            <!-- <p class="w3-border w3-padding-large w3-padding-32 w3-center"></p> -->
            
        </div>
    </div>

    <%-- 
          --%>
</div>
</asp:Content>



