<%@ Page Title="Demo2" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Demo2.aspx.cs" 
    Inherits="private_sc_Demo2" %>

<%--  --%>
<asp:Content ID="CodeSection" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="TitleSection" ContentPlaceHolderID="BodyTitle" runat="server">
    Demo Two Title
</asp:Content>

<asp:Content ID="BodySection" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
                Small Screen Text
            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
            Large Screen Text
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
<div class="spacer30"></div>

        <%--  --%>
    </div>

</div>
</asp:Content>
