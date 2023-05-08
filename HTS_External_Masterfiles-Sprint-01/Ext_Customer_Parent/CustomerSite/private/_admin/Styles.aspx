<%@ Page Title="Web Site Styles" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Styles.aspx.cs" 
    MaintainScrollPositionOnPostback="true"     
    Inherits="private__admin_Styles" %>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
<style type="text/css">
    .dvStyle {
        float: left;
        padding-right: 20px;
    }
</style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Available Styles For Web Site
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <!-- 
        <table><tr><td></td><td></td></tr></table>
    -->
    <table>
        <tr><td style="width: 150px; height: 50px;">scantronBlueMedium<br />#006FA1</td><td style="width: 400px; background-color: #006FA1"></td></tr>
        <tr><td style="height: 50px;">scantronRed <br />#AE132A</td><td style="background-color: #AE132A"></td></tr>
        <tr><td style="height: 50px;">scantronGrayMedium <br />#8C8D8E</td><td style="background-color: #8C8D8E"></td></tr>
        <tr><td style="height: 50px;">scantronGrayDark <br />#414042</td><td style="background-color: #414042"></td></tr>
        <tr><td style="height: 50px;">scantronYellowOrange <br />#FEBC11</td><td style="background-color: #FEBC11"></td></tr>
        <tr><td style="height: 50px;">scantronOrange <br />#F47B20</td><td style="background-color: #F47B20"></td></tr>
        <tr><td style="height: 50px;">scantronGreenBright <br />#7AC03D</td><td style="background-color: #7AC03D"></td></tr>
        <tr><td style="height: 50px;">scantronGreenForest <br />#007A40</td><td style="background-color: #007A40"></td></tr>
        <tr><td style="height: 50px;">scantronAqua <br />#00B1AC</td><td style="background-color: #00B1AC"></td></tr>
        <tr><td style="height: 50px;">scantronBlueRoyal <br />#00A0DF</td><td style="background-color: #00A0DF"></td></tr>
        <tr><td style="height: 50px;">scantronTealDark <br />#005961</td><td style="background-color: #005961"></td></tr>
        <tr><td style="height: 50px;">scantronPurpleDark <br />#4d407e</td><td style="background-color: #4d407e"></td></tr>
    </table>
    <div class="spacer15"></div>

    <div class="dvStyle">
        <asp:Panel ID="pnDefaultCss" runat="server" />
    </div>
    <div class="dvStyle">
        <asp:Panel ID="pnDefaultSkins" runat="server" />
    </div>
    <div class="spacer15"></div>
    <div class="dvStyle">
        <asp:Panel ID="pnCustomCss" runat="server" />
    </div>
    <div class="dvStyle">
        <asp:Panel ID="pnCustomSkins" runat="server" />
    </div>

    <div class="spacer20"></div>
    
    <div class="spacer1"></div>

</asp:Content>

