<%@ Page Title="Zip Code Zones (Mgr Ver)" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="ZipMgr.aspx.cs" 
    Inherits="public_utils_ZipMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td>
                Zip Code Zones
            </td>
            <td style="text-align: right; padding-right: 10px;">
                <span style="font-size: 12px;">Last update: May 28th, 2013</span>                
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<div class="cont_top"></div>
<div class="cont_body">
    <asp:Panel ID="pnZip" runat="server" DefaultButton="btZip">
        <table class="tableWithLines headerBorder rowCentered">
            <tr>
                <th style="width: 320px;">Zip Code</th>
                <th style="width: 200px;">Zone</th>
                <th>Codes</th>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txZip" runat="server" Width="75" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btZip" runat="server" 
                        Text="Get Zone"
                        OnClick="btZip_Click" />
                </td>
                <td style="padding-top: 15px;">
                    <asp:Label ID="lbZoneLetter" runat="server" 
                        ForeColor="#94002c" 
                        Font-Size="50" />
                </td>
                <td style="text-align: left; padding-left: 50px;">
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
                    <table class="tableWithLines headerGray">
                        <tr style="border-left: 1px solid #333333; border-right: 1px solid #333333;">
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
                    <tr class="trColorReg">
                        <td style="text-align: center;"><asp:Label ID="lbRegion" runat="server" Text='<%# Eval("Region") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCity" runat="server" Text='<%# Eval("City") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbStateName" runat="server" Text='<%# Eval("StateName") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbAreaCode" runat="server" Text='<%# Eval("AreaCode") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbPopulation" runat="server" Text='<%# Eval("Population") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbTimeZone" runat="server" Text='<%# Eval("TimeZoneAbbr") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbDaylightSavings" runat="server" Text='<%# Eval("DaylightSavings") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCounty" runat="server" Text='<%# Eval("County1") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFips" runat="server" Text='<%# Eval("FipsCode") %>' /></td>
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
                    <table class="tableWithLines headerBorder headerGreen rowCentered">
                        <tr>
                            <th>Market Zip</th>
                            <th>Zone</th>
                            <th>Market</th>
                            <th>Air Miles</th>
                            <th>Road Miles</th>
                            <th>Road Time</th>
                            <th>Pop 50 Radius</th>
                            <th>Pop 100 Radius</th>
                            <th>Route</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td><asp:Label ID="lbMarketZip" runat="server" Text='<%# Eval("MarketZip") %>' /></td>
                        <td><asp:Label ID="lbMarketZone" runat="server" Text='<%# Eval("MarketZone") %>' /></td>
                        <td><asp:Label ID="lbMarketName" runat="server" Font-Bold="true" Font-Size="Large" Text='<%# Eval("MarketName") %>' /></td>
                        <td><asp:Label ID="lbAirMiles" runat="server" Text='<%# Eval("AirMiles") %>' /></td>
                        <td><asp:Label ID="lbRoadMiles" runat="server" Text='<%# Eval("RoadMiles") %>' /></td>
                        <td><asp:Label ID="lbRoadTime" runat="server" Text='<%# Eval("RoadTimeAlpha") %>' /></td>
                        <td><asp:Label ID="lbPop50" runat="server" Text='<%# Eval("Pop50Format") %>' /></td>
                        <td><asp:Label ID="lbPop100" runat="server" Text='<%# Eval("Pop100Format") %>' /></td>
                        <td>
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
        

            <%-- Zip Closest Center Repeater --%> 
            <div style="height: 30px; clear: both;"></div>
                <asp:Label ID="lbCenter" runat="server" 
                    Text="Closest STS Center: Center of Center" 
                    SkinID="labelTitleColor2_Medium" />
            <asp:Repeater ID="rpZipCenter" runat="server">
                <HeaderTemplate>
                    <table class="tableWithLines headerGray headerBorder">
                        <tr>
                            <th>Ctr Zip</th>
                            <th>Ctr Type</th>
                            <th>Ctr</th>
                            <th>Ctr Name</th>
                            <th>City</th>
                            <th>State</th>
                            <th>Air Miles</th>
                            <th>Road Miles</th>
                            <th>Road Time</th>
                            <th>Pop 50 Radius</th>
                            <th>Pop 100 Radius</th>
                            <th>Route</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td style="text-align: center;"><asp:Label ID="lbCtrZip" runat="server" Text='<%# Eval("CtrZip") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrType" runat="server" Text='<%# Eval("CtrTypeAlpha") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrNum" runat="server" Text='<%# Eval("CtrNum") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrName" runat="server" Text='<%# Eval("CtrName") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrCity" runat="server" Text='<%# Eval("CtrCity") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrState" runat="server" Text='<%# Eval("CtrState") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbAirMiles" runat="server" Text='<%# Eval("AirMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadMiles" runat="server" Text='<%# Eval("RoadMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadTime" runat="server" Text='<%# Eval("RoadTimeAlpha") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbPop50" runat="server" Text='<%# Eval("Pop50Format") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbPop100" runat="server" Text='<%# Eval("Pop100Format") %>' /></td>
                        <td style="text-align: center;">
                            <asp:HyperLink ID="hlYahooMap" runat="server" 
                                Text="Map" Target="Map"
                                NavigateUrl='<%# "http://maps.yahoo.com/#mvt=m&q1=" + Eval("CustZip") + "&q2=" + Eval("CtrZip") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <%-- Zip Closest FST Repeater --%> 
            <div style="height: 30px; clear: both;"></div>
                <asp:Label ID="Label1" runat="server" 
                    Text="Closest Field Service Tech: FST Homes" 
                    SkinID="labelTitleColor2_Medium" />
            <asp:Repeater ID="rpZipFst" runat="server">
                <HeaderTemplate>
                    <table class="tableWithLines headerBorder headerGray">
                        <tr>
                            <th>HomeZip</th>
                            <th>Closest</th>
                            <th>Ctr</th>
                            <th>Ctr Name</th>
                            <th>AirMiles</th>
                            <th>RoadMiles</th>
                            <th>RoadTime</th>
                            <th>Tech</th>
                            <th>Name</th>
                            <th>City</th>
                            <th>State</th>
                            <th>Route</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td style="text-align: center;"><asp:Label ID="lbFstZip" runat="server" Text='<%# Eval("FstZip") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFstRank" runat="server" Text='<%# Eval("FstRank") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrNum" runat="server" Text='<%# Eval("FstCtr") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbCtrName" runat="server" Text='<%# Eval("FstName") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbAirMiles" runat="server" Text='<%# Eval("AirMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadMiles" runat="server" Text='<%# Eval("RoadMiles") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbRoadTime" runat="server" Text='<%# Eval("RoadTimeAlpha") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFstNum" runat="server" Text='<%# Eval("FstNum") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFstName" runat="server" Text='<%# Eval("FstName") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFstCity" runat="server" Text='<%# Eval("FstCity") %>' /></td>
                        <td style="text-align: center;"><asp:Label ID="lbFstState" runat="server" Text='<%# Eval("FstState") %>' /></td>
                        <td style="text-align: center;">
                            <asp:HyperLink ID="hlYahooMap" runat="server" 
                                Text="Map" Target="Map"
                                NavigateUrl='<%# "http://maps.yahoo.com/#mvt=m&q1=" + Eval("CustZip") + "&q2=" + Eval("FstZip") %>' />
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

