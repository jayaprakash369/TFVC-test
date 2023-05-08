<%@ Page Title="STS: Locations" Language="C#" MasterPageFile="~/Scantron_Body_A_Nav.master" AutoEventWireup="true" CodeFile="Locations.aspx.cs" 
    Inherits="public_Locations" %>
<%-- --%>

<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
    <style type="text/css">
    .tbStatesWrap
    {
        border-collapse: collapse;
        width: 650px;
    }
    .tbStatesWrap table
    {
    }
    .tbStatesWrap tr
    {
    }
    .tbStatesWrap td
    {
        vertical-align: top;
        text-align: right;
        border: 1px solid #333333;
        padding-left: 10px;
        padding-right: 10px;
        padding-bottom: 10px;
    }
    .dvState
    {
        padding-top: 10px;
        text-align: left;
        font-weight: bold;
        clear: both;
        text-transform: uppercase;
    }
    .dvCenter
    {
        width: 100%;
        text-align: left;
        font-weight: normal;
        clear: both;
        text-indent: 10px;
    }
</style>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
<div class="spacer0"></div>
Locations

<!-- 
 <p>
Subordinate text below the title
</p>
-->
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
        <dl class="tabs">
        <dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl00_ddActive" class="active"><a style="pointer-events:none;">Service Centers</a></dd>
<!-- 
<dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl01_ddActive"><a href="/solutions/higher-ed/assessment/instructor-support">Tab Two</a></dd>
<dd id="scantron_content_0_scantron_body_0_repBreadcrumb_ctl02_ddActive"><a href="/solutions/higher-ed/assessment/cte">Tab Three</a></dd>
-->
    </dl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">
        <p>Scantron Technology Solutions is headquartered in Omaha, NE, and we have Service Centers located across the United States. Our company employed Field Service Technicians are strategically located in metros throughout the U.S.</p>
    <div class="spacer10"></div>
    <center>
    <%--
    <div style="float: right; padding-right: 20px; position: relative; top:-15px; left:-10px;"><div class="btn btn_green"><asp:HyperLink ID="hlButtonStatic" runat="server" Text="Contact Us" NavigateUrl="~/public/contact.aspx" /><span></span></div></div>
    <asp:CircleHotSpot NavigateUrl="javascript: alert('Test');" Radius="10" X="375" Y="185"  AlternateText="Minneapolis (X Techs)" HotSpotMode="NotSet" />
    // x=from left, y=from top 110->25= 85 less
    use &#10; for a break line
        was 675
     --%>

    <div style="padding-bottom: 30px;">
    <asp:ImageMap ID="imMapCenters" runat="server"
        Width="675"
        ImageUrl="~/media/scantron/images/support/UsCenterMapGreen.png">
        <asp:CircleHotSpot Radius="20" x="70" y="25"    AlternateText="Seattle"                 HotSpotMode="Inactive"  />
        <asp:CircleHotSpot Radius="20" x="60" y="60"    AlternateText="Portland"                HotSpotMode="Inactive"  />
        <asp:CircleHotSpot Radius="10" x="40" y="170"   AlternateText="Sacramento"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="25" Y="180"   AlternateText="San Francisco/Oakland"   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="30" Y="190"   AlternateText="San Jose"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="55" Y="210"   AlternateText="Fresno"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="60" Y="250"   AlternateText="Los Angeles"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="70" Y="270"   AlternateText="San Diego"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="105" Y="230"  AlternateText="Las Vegas"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="140" Y="275"  AlternateText="Phoenix"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="155" Y="160"  AlternateText="Salt Lake City"          HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="240" Y="185"  AlternateText="Denver"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="330" Y="75"   AlternateText="Fargo"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="330" Y="130"  AlternateText="Sioux Falls"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="375" Y="110"  AlternateText="Minneapolis"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="345" Y="120"  AlternateText="Southwest MN"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="340" Y="140"  AlternateText="Sioux City"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="330" Y="175"  AlternateText="Lincoln"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="340" Y="165"  AlternateText="Omaha"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="365" Y="160"  AlternateText="Des Moines"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="390" Y="155"  AlternateText="Cedar Rapids"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="400" Y="160"  AlternateText="Davenport"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="355" Y="195"  AlternateText="Kansas City"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="20" X="355" Y="220"  AlternateText="Joplin MO"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="415" Y="205"  AlternateText="St Louis"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="325" Y="230"  AlternateText="Witchita"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="340" Y="245"  AlternateText="Tulsa"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="325" Y="255"  AlternateText="Oklahoma City"           HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="385" Y="275"  AlternateText="Little Rock"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="330" Y="305"  AlternateText="Dallas"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="325" Y="340"  AlternateText="Austin"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="315" Y="355"  AlternateText="San Antonio"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="355" Y="340"  AlternateText="Houston"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="320" Y="400"  AlternateText="Brownsville (South Texas)" HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="405" Y="335"  AlternateText="Baton Rouge"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="425" Y="345"  AlternateText="New Orleans"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="450" Y="330"  AlternateText="Mobile"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="490" Y="315"  AlternateText="Dothan"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="535" Y="325"  AlternateText="Jacksonville"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="530" Y="365"  AlternateText="Tampa"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="550" Y="350"  AlternateText="Orlando"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="570" Y="385"  AlternateText="Miami"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="435" Y="110"  AlternateText="Green Bay"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="420" Y="135"  AlternateText="Madison"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="435" Y="130"  AlternateText="Milwaukee"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="420" Y="145"  AlternateText="Rockford"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="435" Y="150"  AlternateText="Chicago"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="420" Y="175"  AlternateText="Peoria"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="425" Y="185"  AlternateText="Decatur"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="455" Y="135"  AlternateText="Grand Rapids"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="490" Y="135"  AlternateText="Detroit"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="490" Y="155"  AlternateText="Toledo"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="510" Y="145"  AlternateText="Cleveland"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="515" Y="155"  AlternateText="Akron"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="470" Y="160"  AlternateText="Fort Wayne"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="460" Y="180"  AlternateText="Indianapolis"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="445" Y="215"  AlternateText="Evansville"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="500" Y="175"  AlternateText="Columbus"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="490" Y="180"  AlternateText="Dayton"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="480" Y="190"  AlternateText="Cincinnati"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="480" Y="205"  AlternateText="Lexington"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="425" Y="265"  AlternateText="Memphis"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="460" Y="245"  AlternateText="Nashville"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="495" Y="245"  AlternateText="Knoxville"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="480" Y="255"  AlternateText="Chattanooga"             HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="460" Y="285"  AlternateText="Birmingham"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="490" Y="265"  AlternateText="Atlanta"                 HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="500" Y="285"  AlternateText="Macon"                   HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="535" Y="170"  AlternateText="Pittsburgh"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="565" Y="160"  AlternateText="Harrisburg"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="580" Y="150"  AlternateText="Allentown"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="5"  X="585" Y="167"  AlternateText="Wilmington"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="5"  X="595" Y="160"  AlternateText="Philadelphia"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="15" X="565" Y="110"  AlternateText="Syracuse"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="595" Y="110"  AlternateText="Albany"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="620" Y="105"  AlternateText="Boston"                  HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="605" Y="120"  AlternateText="Hartford"                HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="620" Y="120"  AlternateText="Providence"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="595" Y="125"  AlternateText="White Plains"            HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="590" Y="145"  AlternateText="New Jersey"              HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="600" Y="145"  AlternateText="New York/Long Island"    HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="7"  X="575" Y="173"  AlternateText="Baltimore"               HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="565" Y="180"  AlternateText="Washington DC"           HotSpotMode="Inactive" />
        <asp:CircleHotSpot Radius="10" X="565" Y="205"  AlternateText="Richmond"                HotSpotMode="Inactive" />        
        <asp:CircleHotSpot Radius="10" X="565" Y="235"  AlternateText="Raleigh"                 HotSpotMode="Inactive" />  
        <asp:CircleHotSpot Radius="10" X="545" Y="235"  AlternateText="Greensboro"              HotSpotMode="Inactive" />  
        <asp:CircleHotSpot Radius="10" X="535" Y="250"  AlternateText="Charlotte"               HotSpotMode="Inactive" />  
        <asp:CircleHotSpot Radius="10" X="530" Y="265"  AlternateText="Columbia"                HotSpotMode="Inactive" />  
        <asp:CircleHotSpot Radius="10" X="520" Y="260"  AlternateText="Greenville"              HotSpotMode="Inactive" />
        </asp:ImageMap>
    </div>
    </center>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>

