<%@ Page Title="Zip Code Zones" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ZipZones.aspx.cs" Inherits="public_utils_ZipZones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>
                Zip Code Zones <span style="font-size: 16px;">(OMR Hardware Version) </span>                
            </td>
            <td style="text-align: right; padding-right: 10px;">
                <span style="font-size: 12px;">Last full update: May 1st, 2019</span>                
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div class="cont_top"></div>
<div class="cont_body">
    <asp:Panel ID="pnZip" runat="server" DefaultButton="btZip">
        <table class="tableWithLines headerBorder">
            <tr>
                <th style="width: 320px;">Zip Code</th>
                <th style="width: 200px;">Zone</th>
                <th>Codes</th>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:TextBox ID="txZip" runat="server" Width="75" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btZip" runat="server" 
                        Text="Get Zone"
                        OnClick="btZip_Click" />
                </td>
                <td style="text-align: center; padding-top: 15px;">
                    <asp:Label ID="lbZoneLetter" runat="server" 
                        ForeColor="#94002c" 
                        Font-Size="50" />
                </td>
                <td style="padding-left: 50px;">
                    <asp:BulletedList ID="blCodes" runat="server">
                        <asp:ListItem Text="A = 0 -> 50 air miles" />
                        <asp:ListItem Text="B = 50 -> 100 air miles" />
                        <asp:ListItem Text="D = over 100 (Depot Service -- or Subcontract)" />
                        <asp:ListItem Text="X = Not accessible by road (island)" />
                    </asp:BulletedList>
                </td>
            </tr>
        </table>

        <asp:Panel ID="pnZipData" runat="server" Visible="false">

            <%-- Zip Detail Repeater --%> 
            <asp:Repeater ID="rpZipDetail" runat="server">
                <HeaderTemplate>
                    <table class="tableWithLines headerGray" style="border-left: 1px solid #333333; border-right: 1px solid #333333;">
                        <tr>
                            <th style="width: 43px;">Region</th>
                            <th style="width: 130px;">City</th>
                            <th style="width: 130px;">State</th>
                            <th style="width: 96px;">AreaCode</th>
                            <th style="width: 96px;">Population</th>
                            <th>TimeZone</th>
                            <th>DST</th>
                            <th>County</th>
                            <th>FIPS</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg rowCentered">
                        <td style="width: 43px;"><asp:Label ID="lbRegion" runat="server" Text='<%# Eval("Region") %>' /></td>
                        <td style="width: 130px;"><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                        <td style="width: 130px;"><asp:Label ID="lbStateName" runat="server" Text='<%# Eval("StateName") %>' /></td>
                        <td style="width: 96px;"><asp:Label ID="lbAreaCode" runat="server" Text='<%# Eval("AreaCode") %>' /></td>
                        <td style="width: 96px;"><asp:Label ID="lbPopulation" runat="server" Text='<%# Eval("Population") %>' /></td>
                        <td><asp:Label ID="lbTimeZone" runat="server" Text='<%# Eval("TimeZoneAbbr") %>' /></td>
                        <td><asp:Label ID="lbDaylightSavings" runat="server" Text='<%# Eval("DaylightSavings") %>' /></td>
                        <td><asp:Label ID="lbCounty" runat="server" Text='<%# Eval("County1") %>' /></td>
                        <td><asp:Label ID="lbFips" runat="server" Text='<%# Eval("FipsCode") %>' /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <%-- Zip Closest Market Repeater --%> 
            <div style="height: 30px; clear: both;"></div>
                <asp:Label ID="lbMarket" runat="server" 
                    Text="Closest Marketing Area: Center of City" 
                    SkinID="labelTitleColor2_Medium" />
            <asp:Repeater ID="rpZipMarket" runat="server">
                <HeaderTemplate>
                    <table class="tableWithLines">
                        <tr>
                            <th style="background-color: #3a7728;">Market Zip</th>
                            <th style="background-color: #3a7728;">Zone</th>
                            <th style="background-color: #3a7728;">Market</th>
                            <th style="background-color: #3a7728;">Air Miles</th>
                            <th style="background-color: #3a7728;">Road Miles</th>
                            <th style="background-color: #3a7728;">Road Time</th>
                            <th style="background-color: #3a7728;">Pop 50 Radius</th>
                            <th style="background-color: #3a7728;">Pop 100 Radius</th>
                            <th style="background-color: #3a7728;">Route</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td style="text-align: center;"><asp:Label ID="lbMarketZip" runat="server" Text='<%# Eval("MarketZip") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbMarketZone" runat="server" Text='<%# Eval("MarketZone") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbMarketName" runat="server" Font-Bold="true" Font-Size="12" Text='<%# Eval("MarketName") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbAirMiles" runat="server" Text='<%# Eval("AirMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadMiles" runat="server" Text='<%# Eval("RoadMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadTime" runat="server" Text='<%# Eval("RoadTimeAlpha") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbPop50" runat="server" Text='<%# Eval("Pop50Format") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbPop100" runat="server" Text='<%# Eval("Pop100Format") %>' /></td>
                        <td style="text-align: center;">
                            <asp:HyperLink ID="hlYahooMap" runat="server" 
                                Text="Map" Target="Map"
                                NavigateUrl='<%# "http://maps.yahoo.com/#mvt=m&q1=" + Eval("CustZip") + "&q2=" + Eval("MarketZip") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:Panel>

    </asp:Panel>
</div>
<div class="cont_bot"></div>

</asp:Content>

