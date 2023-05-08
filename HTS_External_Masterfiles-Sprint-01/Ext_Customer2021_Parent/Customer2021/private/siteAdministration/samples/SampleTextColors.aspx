<%@ Page Title="Sample Text Colors" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="SampleTextColors.aspx.cs" 
    Inherits="private_siteAdministration_samples_SampleTextColors" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Sample Text Colors
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32">
        <div class=" w3-container">

        
        Here is an example of how the top line would be generated...<br />
        <asp:Label ID="lbHtml" runat="server" />

        <h1 class="w3-text-steel-blue">steel-blue H1</h1>
        
        <h1 class="w3-text-my-blue">my-blue</h1>
        <h2 class="w3-text-red">red H2</h2>
        <h3 class="w3-text-green">green H3</h3>
        <h4 class="w3-text-yellow">yellow H4</h4>
        <h5 class="w3-text-black">black H5</h5>
        <p class="w3-text-blue">blue</p>
        <p class="w3-text-light-steel-blue">light-steel-blue</p>
        <p class="w3-text-white" style="background-color: #777777;">white</p>
        <p class="w3-text-amber">amber</p>
        <p class="w3-text-aqua" style="background-color: #777777;">aqua</p>
        <p class="w3-text-light-blue">light-blue</p>
        <p class="w3-text-brown">brown</p>
        <p class="w3-text-cyan">cyan</p>
        <p class="w3-text-blue-grey">blue-grey</p>
        <p class="w3-text-light-green">light-green</p>
        <p class="w3-text-indigo">indigo</p>
        <p class="w3-text-khaki">khaki</p>
        <p class="w3-text-lime">lime</p>
        <p class="w3-text-orange">orange</p>
        <p class="w3-text-deep-orange">deep-orange</p>
        <p class="w3-text-pink">pink</p>
        <p class="w3-text-purple">purple</p>
        <p class="w3-text-deep-purple">deep-purple</p>
        <p class="w3-text-sand" style="background-color: #777777;">sand</p>
        <p class="w3-text-teal">teal</p>
        <p class="w3-text-light-grey">light-grey</p>
        <p class="w3-text-dark-grey">dark-grey</p>
    </div>
</div>
</div>
</asp:Content>
