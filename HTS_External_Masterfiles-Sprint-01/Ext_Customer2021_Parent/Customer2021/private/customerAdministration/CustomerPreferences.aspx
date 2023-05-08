<%@ Page Title="CustomerPreferences" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="CustomerPreferences.aspx.cs" 
    Inherits="private_customerAdministration_CustomerPreferences" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
     <style type="text/css">
        .FloatElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Customer Preferences
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">
    <%--  
    <div class="w3-row w3-padding-32" style="border: 1px solid #333333;">
        <div class="w3-twothird w3-container" style="border: 1px solid #ad0034;">
            Two Third container inside a row
        </div>
        <div class="w3-third w3-container" style="border: 1px solid green;">
            One Third container inside a row
        </div>
    </div>


    <div class="w3-row w3-padding-32" style="border: 1px solid #333333;">
        <div class="w3-container" style="border: 1px solid #333333;">
            Container inside a row with 32 padding
        </div>
    </div>

    <div class="w3-container" style="border: 1px solid #333333;">
        Just the container
    </div>

    <div class="w3-container" style="border: 1px solid #333333;">
        Second container
    </div>

    <div class="w3-container" style="border: 1px solid #333333;">
        Third container (not sure what a row adds for you...)
    </div>
--%>
    
        <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">
            <h3 class="titlePrimary">Open or Close New Account Registration</h3>

            <p>All customers begin with OPEN registration allowing creation of multiple accounts under your customer number.
                If at some point you would like to stop open registration, you will need to CLOSE registration for your company acccount.
                (Administrators are allowed to create additional accounts at any time.)</p>
            <div class="FloatElements">
                <b><asp:Label ID="lbCurrentRegistrationStatus_OpenOrClosed" runat="server" /></b>
            </div>
            <div class="FloatElements">
                <asp:Button ID="btUpdateRegistrationStatus_ToOpenOrClosed" runat="server" OnClick="btUpdateRegistrationStatus_ToOpenOrClosed_Click" />
            </div>
            <div class="FloatElements">
                <asp:Label ID="lbMsgUpdateRegistrationStatus" runat="server" SkinId="labelError" />
            </div>
        </div>
    </div>
    
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />

</div>
</asp:Content>
