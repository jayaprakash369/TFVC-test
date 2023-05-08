<%@ Page Title="Call Escalation" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Escalation.aspx.cs" 
    Inherits="private_sc_Escalation" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
    <style type="text/css">
        h1, h2, h3, h4, h5 {
            margin-top: 0px;
            padding-top: 0px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Escalation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-48">
        <div class="w3-container">
			Service request escalation is part of our overall customer experience. Members of our Call Center team are empowered to escalate calls quickly and efficiently should the need arise. It is important to us to resolve issues as rapidly as possible. Our escalation proceedure is listed below.
        </div>
    </div>
    <!-- #009688; -->
    <div style="font-size: 24px; padding-left: 15px;" class="titlePrimary">First Step: 800.228.3628</div>
    <div class="w3-row w3-padding-8"">
        <div class="w3-third w3-container">
            <p>Notification of the respective Regional Service Manager (see below)</p>
        </div>
        <div class="w3-twothird w3-container">
            <i>If unavailable, please ask the Call Center Operator for any available Regional Service Manager</i>
        </div>
    </div>
    
    <div style="border: 1px solid #dddddd; margin-left: 15px; margin-right: 30px; padding: 5px; padding-bottom: 10px;">
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 1:&nbsp;&nbsp;<span class="titleSecondary">Rich Moore</span> (Ext: 3043) </div>
        </div>
        <div class="w3-twothird w3-container">
            Albany, Allentown, Boston, Buffalo, Harrisburg, Hartford, Long Island, New Jersey, Philadelphia, Providence, Syracuse, White Plains, Wilmington
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 2:&nbsp;&nbsp;<span class="titleSecondary">Paul Gajus</span> (Ext: 3185) </div>
        </div>
        <div class="w3-twothird w3-container" style="background-color: #f3f3f3;">
            Atlanta, Baltimore, Charlotte, Chattanooga, Columbia, Greensboro, Greenville, Jacksonville, Miami, Orlando, Raleigh, Richmond, Tampa, Washington, DC
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 3:&nbsp;&nbsp;<span class="titleSecondary">John Martinez</span> (Ext: 3161) </div>
        </div>
        <div class="w3-twothird w3-container">
            Akron, Cincinnati, Cleveland, Columbus, Dayton, Detroit, Fort Wayne, Grand Rapids, Indianapolis, Lexington, Pittsburgh, Toledo
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 4:&nbsp;&nbsp;<span class="titleSecondary">Bill Garrod</span> (Ext: 3037) </div>
        </div>
        <div class="w3-twothird w3-container" style="background-color: #f3f3f3;">
            Canada, Cedar Rapids, Chicago, Des Moines, Fargo, Madison, Milwaukee, Minneaspolis, Peoria, Sioux Falls, Sioux City
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 5:&nbsp;&nbsp;<span class="titleSecondary">Steve Baehr</span> (Ext: 3005) </div>
        </div>
        <div class="w3-twothird w3-container">
            Baton Rouge, Birmingham, Dothan, Evansville, Kansas City, Knoxville, Lincoln, Little Rock, Memphis, Mobile, Nashville, Omaha, Saint Louis
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 6:&nbsp;&nbsp;<span class="titleSecondary">Terry Sandrock</span> (Ext: 3190) </div>
        </div>
        <div class="w3-twothird w3-container" style="background-color: #f3f3f3;">
            Austin, Brownsville, Dallas, Denver, Fayetteville, Houston, Las Vegas, Oklahoma City, Phoenix, Salt Lake City, San Antonio, Southwest Missouri, Tulsa, Wichita
        </div>
    </div>
    <div class="w3-row w3-padding-8">
        <div class="w3-third w3-container">
            <div style="font-size: 16px;">Region 7:&nbsp;&nbsp;<span class="titleSecondary">Steve Greczmiel</span> (Ext: 3152) </div>
        </div>
        <div class="w3-twothird w3-container">
            Fresno, Los Angeles, Oakland, Portland, Sacramento, San Diego, San Francisco, San Jose, Seattle
        </div>
    </div>
    </div>
        <div style="font-size: 24px; padding-left: 15px; padding-top: 30px;" class="titlePrimary">Second Step:</div>
    <div class="w3-row w3-padding-8"">
        <div class="w3-third w3-container">
            <p>Notification of the Director of Field Operations</p>
        </div>
        <div class="w3-twothird w3-container">
            <div style="font-size: 20px;"><span class="titleSecondary">Paul Razewski</span>&nbsp;&nbsp;(Ext: 3151)  </div>
             
        </div>
    </div>

    <div style="font-size: 24px; padding-left: 15px;" class="titlePrimary">Third Step: </div>
    <div class="w3-row w3-padding-8"  style="margin-bottom: 50px;">
        <div class="w3-third w3-container">
            <p>Notification of the Vice President of Operations</p>
        </div>
        <div class="w3-twothird w3-container">
            <div style="font-size: 20px;"><span class="titleSecondary">Brian Janning</span>&nbsp;&nbsp;(Ext: 3054)  </div>
             
        </div>
    </div>

        <%--  
			--%>



</div>
</asp:Content>
