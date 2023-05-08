<%@ Page Title="Template Show Hide" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="TemplateShowHide.aspx.cs" 
    Inherits="private_siteAdministration_samples_TemplateShowHide" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Template To Show and Hide Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

 <div class="w3-row w3-padding-32">
    <div class=" w3-container">

    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

        <%--  --%>

            <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->

        <%--  --%>
    </div>
</div>

</div>
</asp:Content>
