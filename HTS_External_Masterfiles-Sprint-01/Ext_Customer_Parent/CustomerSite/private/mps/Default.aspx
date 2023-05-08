<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="private_mps_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Managed Print Review</title>
<style type="text/css">
* {
    margin:0;
    padding:0;
} 
body {
    font-family:Verdana;
    font-size:.8em;
    background-color: #F8F8FF;
}
td 
{
    padding-right: 15px;
}
html, body, form, #panelWrap {
    height: 100%;
}
.myH1 
{
    font-family: Verdana;
    font-size: 1.9em;
    margin-bottom: 30px;
}
.myH2 
{
    font-family: Verdana;
    font-size: 1.5em;
    margin-bottom: 10px;
}
.buttonA
{
    padding-left: 12px;
    padding-right: 12px;
    height: 22px;
    background-color:#DDDDDD;
    color: #000000;
    border: 1px solid #555555;
}

.buttonA:hover
{
    background-color:#777777;
    color: #FFFFFF;
}

#panelWrap {
	min-height: 100%;
}
#panelMain {
	min-height: 100%;
	position:relative; /* my add */
	overflow:visible; /* was auto */
	padding-bottom:1px;  /* must be same height as the footer-- 150 to start */
    margin-top: 0px;  /* was 80... extra for testing , 130 */
    width: 800px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom:0px; /* my add -- long tables overlap the footer without this! was 100 */
    background-color: #FFFFFF;
    padding-top:30px;
    padding-right:30px;
    padding-left:30px;
    padding-bottom:30px;
    border-left: 1px solid #AAAAAA;
    border-right: 1px solid #AAAAAA;
    z-index: 10;
}	
#panelFooter {
	position:relative;
	margin-top:-1px; /* negative value of footer height */
	height:1px;
	clear:both;
	text-align:center; /* my add */
	background-color: #FFFFFF;
    width: 800px;
    padding-right:30px;
    padding-left:30px;
    margin-left: auto;
    margin-right: auto;
    border-left: 1px solid #AAAAAA;
    border-right: 1px solid #AAAAAA;
} 

.boxGray {
	position:relative; 
	overflow:auto;
    margin-top: 0px;  
    margin-bottom: 30px; 
    border: 1px solid #CCCCCC;
    padding: 8px;
    background-color: #EEEEEE;
}	
.boxWhite {
	position:relative; 
	overflow:auto;
    margin-top: 0px;  
    margin-bottom: 30px; 
    border: 1px solid #CCCCCC;
    padding: 8px;
    margin-left: auto;
    margin-right: auto;
    background-color: #FFFFFF;
}	
.boxChart td
{
   padding-bottom: 30px;
}

.ddStyle 
{
    border: 1px solid #BBBBBB;
}

.myCenter {
    margin-left: auto;
    margin-right: auto;
}

.myTitle 
{
    position: relative;
    top: -6px;
    right: -1px;
}

.myHomeLink 
{
    position: relative;
    top: -20px;
    color: #333333;
}

</style>
    <script type="text/javascript">

        function clearOtherDropDown(jField) {
            var doc = document.forms[0];
            if (jField == "ddCustomer") {
                if (doc.ddCustomer.value > 0)
                    doc.ddAgreement.value = "";
            }
            if (jField == "ddAgreement") {
                if (doc.ddAgreement.value != "")
                    //document.forms[0].ddCustomer.value = 0;
                    doc.ddCustomer.value = 0;
            }
            return true;
        }

        function doClientVal() {
            var objCustomer = document.getElementById("ddCustomer");
            var objAgreement = document.getElementById("ddAgreement");
            if ((objCustomer.options[objCustomer.selectedIndex].value == 0) && (objAgreement.options[objAgreement.selectedIndex].value == "")) {
                alert("A customer or specific agreement must be selected");
                objCustomer.focus();
                return false;
            }
        }

        /*
        <asp:Chart 
        Pallette = Group of colors
        BackColor = Background color
        BackSecondaryColor = End shade
        BackGradientStyle = TopToBottom
        Label="#PERCENT{P1}"
        <asp:Title Text="Devices By Type" 
        <asp:Series 
        ChartType="Pie" 
        BorderColor="180, 26, 59, 105"              // makes lines appear outside labels and legend boxes
        CustomProperties="PieDrawingStyle=Concave"  // or SoftEdge
        Label="#PERCENT{P1}"                       // to show percent instead of text
        <asp:ChartArea Area3DStyle-Enable3D="true" 
        <Area3DStyle Enable3D="False"
        <borderskin SkinStyle="Emboss"                  // Edge Style

        */

</script>

</head>
<body>
    <form id="form2" runat="server">
<%--       
MAIN WRAP PANEL
--%>

<asp:Panel id="panelWrap" runat="server">
    <asp:Panel id="panelMain" runat="server">
        <asp:Table ID="Table1" runat="server" Width="800">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:LinkButton ID="lkHome" runat="server" 
                        Font-Size="8" 
                        CssClass="myHomeLink"
                        PostBackUrl="~/Default.aspx">Home</asp:LinkButton>
                    <asp:Label ID="lbTitle" runat="server" Text="Managed Print Review" Font-Size="20" Font-Names="Helvetica" CssClass="myTitle" ></asp:Label>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Image ID="imLogo" runat="server" ImageUrl="~/media/scantron/images/logos/company/MPowerPrint.png" Width="350" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
<%--       
INPUT PANEL
--%>



        <asp:Panel ID="panelInput" runat="server" CssClass="boxGray">
        <asp:Table ID="tbInput" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Table ID="tbInputLeft" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>By Customer<br /><asp:DropDownList ID="ddCustomer" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Or Specific Agreement<br /><asp:DropDownList ID="ddAgreement" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Button ID="buttonLoad" runat="server" Text="Load" CssClass="buttonDefault" onclick="buttonLoad_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="buttonPDF" runat="server" Text="Create PDF" CssClass="buttonDefault" onclick="buttonPDF_Click"  /></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>

                <asp:TableCell>
                    <asp:Table ID="tbInputRight" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>Date Range</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right"><asp:DropDownList ID="ddDateStart" runat="server" CssClass="ddStyle" /></asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right"><asp:DropDownList ID="ddDateEnd" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Utilization Threshold</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right">Low&nbsp;<asp:DropDownList ID="ddUseLow" runat="server" CssClass="ddStyle" /></asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right">High&nbsp<asp:DropDownList ID="ddUseHigh" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">Service Request Threshold</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right"><asp:DropDownList ID="ddRequests" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">Paper Jam Threshold</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right"><asp:DropDownList ID="ddPaperJams" runat="server" CssClass="ddStyle" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">Sort Locations By </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:DropDownList ID="ddChildSort" runat="server" CssClass="ddStyle" >
                                    <asp:ListItem Text="STS Location" Value="LOC" Selected="True" />
                                    <asp:ListItem Text="Cust Cross Ref" Value="XRF" />
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        </asp:Panel>
 <%--       
HEADER PANEL: Cust Detail
--%><asp:Panel id="panelHeader" runat="server" CssClass="boxWhite">
    <center>
        <asp:Table ID="tbCustDetail" runat="server" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center">
                    <asp:Label ID="lbCustName" runat="server" Font-Size="13" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center">
                    <asp:Label ID="lbAddress" runat="server" Text=""></asp:Label>
                    &nbsp;
                    <asp:Label ID="lbCityStateZip" runat="server" Text=""></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lbCustNumTitle" runat="server" Text="Customer" Font-Bold="true" />&nbsp;<asp:Label ID="lbCustNum" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbAgreementsTitle" runat="server" Text="Agreements" Font-Bold="true" />&nbsp;<asp:Label ID="lbAgreements" runat="server" Text=""></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbAgrLocationsTitle" runat="server" Text="Locations" Font-Bold="true" />&nbsp;<asp:Label ID="lbAgrLocations" runat="server" Text=""></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbAgrDevicesTitle" runat="server" Text="Devices" Font-Bold="true" />&nbsp;<asp:Label ID="lbAgrDevices" runat="server" Text=""></asp:Label>
                </asp:TableCell>
             </asp:TableRow>
        </asp:Table>
        </center>
    </asp:Panel><%--       
FLEET PANEL
--%><asp:Panel id="panelFleet" runat="server">
        <asp:Table ID="tbFleet" runat="server" HorizontalAlign="Center" CssClass="boxChart">


<%--       
PAGE COUNTS: ALL
<asp:TableRow>
                <asp:TableCell ColumnSpan="2">

						<asp:CHART id="chartByPagesAll" runat="server" 
                                Width="800px" 
                                Height="600px" 
                                Palette="BrightPastel" 
                                BackColor="#D3DFF0" 
                                BorderDashStyle="Solid" 
                                BackGradientStyle="TopBottom" 
                                BorderWidth="2" 
                                BorderColor="181, 64, 1">
							<titles>
								<asp:Title 
                                    ShadowColor="32, 0, 0, 0" 
                                    Font="Trebuchet MS, 14.25pt, style=Bold" 
                                    ShadowOffset="3" 
                                    Text="Page Counts By Month" 
                                    Name="Title1" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
							</titles>
							<legends>
								<asp:Legend 
                                    LegendStyle="Row"
                                    Docking="Bottom" 
                                    Alignment="Center"
                                    TitleFont="Microsoft Sans Serif, 12pt" 
                                    BackColor="Transparent" 
                                    Font="Trebuchet MS, 12.00pt" 
                                    IsTextAutoFit="False" 
                                    Enabled="False" 
                                    Name="Legend1">
                                </asp:Legend>
							</legends>
							<borderskin SkinStyle="Emboss"></borderskin>
							<series>
								<asp:Series 
                                    Name="SeriesMono"  
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
								<asp:Series 
                                    Name="SeriesColor" 
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
								<asp:Series 
                                    Name="SeriesMicr" 
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
							</series>
							<chartareas>
								<asp:ChartArea Name="ChartArea1" 
                                    BorderColor="64, 64, 64, 64" 
                                    BackSecondaryColor="White" 
                                    BackColor="#D3DFF0" 
                                    ShadowColor="Transparent" 
                                    BackGradientStyle="TopBottom">
									<area3dstyle 
                                        Rotation="10" 
                                        Perspective="10" 
                                        Inclination="15" 
                                        IsRightAngleAxes="False" 
                                        WallWidth="0" 
                                        IsClustered="False" />
									<axisy 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle Font="Trebuchet MS, 10.00pt" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisy>
									<axisx 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle 
                                            Font="Trebuchet MS, 12.00pt" 
                                            IsEndLabelVisible="False" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisx>
								</asp:ChartArea>
							</chartareas>
						</asp:CHART>
        
                </asp:TableCell>
            </asp:TableRow>
--%>
<%--       
PAGE COUNTS: MONOCHROME
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">

						<asp:CHART id="chartByPagesMono" runat="server" 
                                Width="800px" 
                                Height="450px" 
                                Palette="BrightPastel" 
                                BackColor="#D3DFF0" 
                                BorderDashStyle="Solid" 
                                BackGradientStyle="TopBottom" 
                                BorderWidth="2" 
                                BorderColor="181, 64, 1">
							<titles>
								<asp:Title 
                                    ShadowColor="32, 0, 0, 0" 
                                    Font="Trebuchet MS, 14.25pt, style=Bold" 
                                    ShadowOffset="3" 
                                    Text="Monochrome Pages By Month" 
                                    Name="Title1" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
								<asp:Title 
                                    Font="Trebuchet MS, 12.00pt" 
                                    Text="Total" 
                                    Name="Title2" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
							</titles>
							<legends>
								<asp:Legend 
                                    LegendStyle="Row"
                                    Docking="Bottom" 
                                    Alignment="Center"
                                    TitleFont="Microsoft Sans Serif, 12pt" 
                                    BackColor="Transparent" 
                                    Font="Trebuchet MS, 12.00pt" 
                                    IsTextAutoFit="False" 
                                    Enabled="False" 
                                    Name="Legend1">
                                </asp:Legend>
							</legends>
							<borderskin SkinStyle="Emboss"></borderskin>
							<series>
								<asp:Series 
                                    Name="SeriesMono"  
                                    IsValueShownAsLabel="true" 
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
							</series>
							<chartareas>
								<asp:ChartArea Name="ChartArea1" 
                                    BorderColor="64, 64, 64, 64" 
                                    BackSecondaryColor="White" 
                                    BackColor="#D3DFF0" 
                                    ShadowColor="Transparent" 
                                    BackGradientStyle="TopBottom">
									<area3dstyle 
                                        Rotation="10" 
                                        Perspective="10" 
                                        Inclination="15" 
                                        IsRightAngleAxes="False" 
                                        WallWidth="0" 
                                        IsClustered="False" />
									<axisy 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle Font="Trebuchet MS, 10.00pt" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisy>
									<axisx
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle 
                                            Font="Trebuchet MS, 12.00pt" 
                                            IsEndLabelVisible="False" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisx>
								</asp:ChartArea>
							</chartareas>
						</asp:CHART>
        
                </asp:TableCell>
            </asp:TableRow>

<%--       
PAGE COUNTS: COLOR
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">

						<asp:CHART id="chartByPagesColor" runat="server" 
                                Width="800px" 
                                Height="450px" 
                                Palette="BrightPastel" 
                                BackColor="#D3DFF0" 
                                BorderDashStyle="Solid" 
                                BackGradientStyle="TopBottom" 
                                BorderWidth="2" 
                                BorderColor="181, 64, 1">
							<titles>
								<asp:Title 
                                    ShadowColor="32, 0, 0, 0" 
                                    Font="Trebuchet MS, 14.25pt, style=Bold" 
                                    ShadowOffset="3" 
                                    Text="Color Pages By Month" 
                                    Name="Title1" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
								<asp:Title 
                                    Font="Trebuchet MS, 12.00pt" 
                                    Text="Total" 
                                    Name="Title2" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
							</titles>
							<legends>
								<asp:Legend 
                                    LegendStyle="Row"
                                    Docking="Bottom" 
                                    Alignment="Center"
                                    TitleFont="Microsoft Sans Serif, 12pt" 
                                    BackColor="Transparent" 
                                    Font="Trebuchet MS, 12.00pt" 
                                    IsTextAutoFit="False" 
                                    Enabled="False" 
                                    Name="Legend1">
                                </asp:Legend>
							</legends>
							<borderskin SkinStyle="Emboss"></borderskin>
							<series>
                                <asp:Series Name="S1"></asp:Series>
								<asp:Series 
                                    Name="SeriesColor"  
                                    IsValueShownAsLabel="true" 
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
							</series>
							<chartareas>
								<asp:ChartArea Name="ChartArea1" 
                                    BorderColor="64, 64, 64, 64" 
                                    BackSecondaryColor="White" 
                                    BackColor="#D3DFF0" 
                                    ShadowColor="Transparent" 
                                    BackGradientStyle="TopBottom">
									<area3dstyle 
                                        Rotation="10" 
                                        Perspective="10" 
                                        Inclination="15" 
                                        IsRightAngleAxes="False" 
                                        WallWidth="0" 
                                        IsClustered="False" />
									<axisy 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle Font="Trebuchet MS, 10.00pt" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisy>
									<axisx 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle 
                                            Font="Trebuchet MS, 12.00pt" 
                                            IsEndLabelVisible="False" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisx>
								</asp:ChartArea>
							</chartareas>
						</asp:CHART>
        
                </asp:TableCell>
            </asp:TableRow>


<%--       
PAGE COUNTS: MICR
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">

						<asp:CHART id="chartByPagesMicr" runat="server" 
                                Width="800px" 
                                Height="450px" 
                                Palette="BrightPastel" 
                                BackColor="#D3DFF0" 
                                BorderDashStyle="Solid" 
                                BackGradientStyle="TopBottom" 
                                BorderWidth="2" 
                                BorderColor="181, 64, 1">
							<titles>
								<asp:Title 
                                    ShadowColor="32, 0, 0, 0" 
                                    Font="Trebuchet MS, 14.25pt, style=Bold" 
                                    ShadowOffset="3" 
                                    Text="Micr Pages By Month" 
                                    Name="Title1" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
								<asp:Title 
                                    Font="Trebuchet MS, 12.00pt" 
                                    Text="Total" 
                                    Name="Title2" 
                                    ForeColor="26, 59, 105">
                                </asp:Title>
							</titles>
							<legends>
								<asp:Legend 
                                    LegendStyle="Row"
                                    Docking="Bottom" 
                                    Alignment="Center"
                                    TitleFont="Microsoft Sans Serif, 12pt" 
                                    BackColor="Transparent" 
                                    Font="Trebuchet MS, 12.00pt" 
                                    IsTextAutoFit="False" 
                                    Enabled="False" 
                                    Name="Legend1">
                                </asp:Legend>
							</legends>
							<borderskin SkinStyle="Emboss"></borderskin>
							<series>
                                <asp:Series Name="S1"></asp:Series>
								<asp:Series Name="S2"></asp:Series>
								<asp:Series 
                                    Name="SeriesMicr"  
                                    IsValueShownAsLabel="true" 
                                    BorderColor="180, 26, 59, 105">
								</asp:Series>
							</series>
							<chartareas>
								<asp:ChartArea Name="ChartArea1" 
                                    BorderColor="64, 64, 64, 64" 
                                    BackSecondaryColor="White" 
                                    BackColor="#D3DFF0" 
                                    ShadowColor="Transparent" 
                                    BackGradientStyle="TopBottom">
									<area3dstyle 
                                        Rotation="10" 
                                        Perspective="10" 
                                        Inclination="15" 
                                        IsRightAngleAxes="False" 
                                        WallWidth="0" 
                                        IsClustered="False" />
									<axisy 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle Font="Trebuchet MS, 10.00pt" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisy>
									<axisx 
                                        LineColor="64, 64, 64, 64"  
                                        LabelAutoFitMaxFontSize="12">
										<LabelStyle 
                                            Font="Trebuchet MS, 12.00pt" 
                                            IsEndLabelVisible="False" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisx>
								</asp:ChartArea>
							</chartareas>
						</asp:CHART>
        
                </asp:TableCell>
            </asp:TableRow>




<%--       
DEVICES BY TYPE 
--%>

            <asp:TableRow >
                <asp:TableCell ColumnSpan="2">
        
                    <asp:Chart ID="chartByType" runat="server" 
                        Width="800px" 
                        Height="400px" 
                        Palette="BrightPastel"
                        BackColor="#D3DFF0" 
                        BackSecondaryColor="White" 
                        BackGradientStyle="TopBottom" 
                        BorderlineDashStyle="Solid" 
                        BorderWidth="2" 
                        BorderColor="26, 59, 105">
                        <titles>
						    <asp:Title 
                                Text="Devices By Type" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>
                        <Series>
                            <asp:Series Name="Series1" 
                                ChartType="Pie" 
                                BorderColor="180, 26, 59, 105"
                                Color="220, 65, 140, 240">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </asp:TableCell></asp:TableRow><%--       
DEVICES BY MANUFACTURER
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByManufacturer" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel" 
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105" 
                    >
                        <titles>
						    <asp:Title 
                                Text="Devices By Manufacturer" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="Series1"
                            ChartType="Pie"
                            CustomProperties="PieDrawingStyle=SoftEdge" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1" 
                            BackColor="Transparent">
                            <Area3dstyle Rotation="20" />
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell></asp:TableRow>
<%--       
DEVICES BY MODEL
    Series  Label="#PERCENT{P1}"
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByModel" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel"
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Devices By Model" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="Series1"
                            ChartType="Pie"
                            CustomProperties="PieDrawingStyle=SoftEdge" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1" 
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell>
            </asp:TableRow>                
               
<%--       
DEVICES BY MANAGEABILITY
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByManageabilityCount" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel"
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Devices By Manageability" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						    <asp:Title 
                                Text="Reporting Page Counts" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 12.0pt" 
                                Name="Title2" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="LegendCount">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="SeriesCount"
                            ChartType="Pie"
                            CustomProperties="PieDrawingStyle=SoftEdge" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240" 
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1"
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell>
            </asp:TableRow>
<%--       
DEVICES BY MANAGEABILITY: TONER
--%>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByManageabilityToner" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel"
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Devices By Manageability" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						    <asp:Title 
                                Text="Reporting Toner Levels" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 12.0pt" 
                                Name="Title2" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="LegendToner">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="SeriesToner"
                            ChartType="Pie"
                            CustomProperties="PieDrawingStyle=SoftEdge" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1"
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell></asp:TableRow><%--       
DEVICES BY UTILIZATION: HIGH
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByUseHigh" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel" 
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Default High" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Bottom" 
                                LegendStyle="Row" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="Series1"
                            ChartType="Bar" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1" 
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
        						<axisy LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="8">
									<LabelStyle Font="Trebuchet MS, 8.0pt" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</axisy>
								<axisx LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="12">
									<LabelStyle Font="Trebuchet MS, 12.0pt" IsEndLabelVisible="False" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</axisx>
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell></asp:TableRow><%--       
DEVICES BY UTLIZATION: LOW
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByUseLow" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel" 
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Default Low" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Bottom" 
                                LegendStyle="Row" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="Series1"
                            ChartType="Bar"
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1" 
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
        						<axisy LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="8">
									<LabelStyle Font="Trebuchet MS, 8.0pt" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</axisy>
								<axisx LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="12">
									<LabelStyle Font="Trebuchet MS, 12.0pt" IsEndLabelVisible="False" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</axisx>
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell></asp:TableRow></asp:Table><%-- 
ALL DEVICE DETAIL
--%>

<asp:Table ID="tbDeviceWrap" runat="server">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" CssClass="myCenter">
                    <br />
                    <asp:Table ID="tbColorLegend" runat="server" SkinID="tableWithLines">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="800" ColumnSpan="6">Highlighted Results</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow HorizontalAlign="Center">
                            <asp:TableCell BackColor="Thistle">Color Device</asp:TableCell>
                            <asp:TableCell BackColor="MistyRose">Micr Device</asp:TableCell>
                            <asp:TableCell BackColor="LightBlue">Low Utilization</asp:TableCell>
                            <asp:TableCell BackColor="Khaki">High Utilization</asp:TableCell>
                            <asp:TableCell BackColor="Burlywood">High Service</asp:TableCell>
                            <asp:TableCell BackColor="DarkSeaGreen">High Jams</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Table ID="tbDevices" runat="server" SkinID="tableWithLines" Width="800">
                    </asp:Table>
                </asp:TableCell></asp:TableRow><%-- 
DEVICES BY...
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                </asp:TableCell></asp:TableRow></asp:Table><asp:Label ID="lbFleet" runat="server" Text=" ">
        </asp:Label></asp:Panel><br /><br /><%-- ABC  --%><%--        
--%><%--       
SERVICE PANEL
--%><asp:Panel id="panelService" runat="server" >
         <asp:Table ID="tbService" runat="server" HorizontalAlign="Center">
         <%--       
SERVICE REQUESTS BY CLOSE CATEGORY 
--%><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
        
                  <asp:Chart ID="chartByCategory" runat="server" 
                    Width="800px" 
                    Height="500px" 
                    Palette="BrightPastel"
                    BackColor="#D3DFF0" 
                    BackSecondaryColor="White" 
                    BackGradientStyle="TopBottom" 
                    BorderlineDashStyle="Solid" 
                    BorderWidth="2" 
                    BorderColor="26, 59, 105"
                    >
                        <titles>
						    <asp:Title 
                                Text="Requests By Category" 
                                ShadowColor="32, 0, 0, 0" 
                                Font="Trebuchet MS, 14.25pt, style=Bold" 
                                ShadowOffset="3" 
                                Name="Title1" 
                                ForeColor="26, 59, 105">
                            </asp:Title>
						</titles>
						<legends>
							<asp:Legend 
                                BackColor="Transparent" 
                                Docking="Left" 
                                LegendStyle="Column" 
                                Alignment="Center" 
                                Font="Trebuchet MS, 12.0pt" 
                                IsTextAutoFit="False" 
                                Name="Default">
                            </asp:Legend>
						</legends>
						<borderskin SkinStyle="Emboss"></borderskin>

                      <Series>
                          <asp:Series Name="Series1"
                            ChartType="Pie"
                            CustomProperties="PieDrawingStyle=SoftEdge" 
                            BorderColor="180, 26, 59, 105"
                            Color="220, 65, 140, 240"
                            >
                          </asp:Series>
                      </Series>
                      <ChartAreas>
                          <asp:ChartArea 
                            Name="ChartArea1" 
                            BackColor="Transparent">
                            <Area3dstyle Rotation="0" />
                          </asp:ChartArea>
                      </ChartAreas>
                  </asp:Chart>

                </asp:TableCell></asp:TableRow></asp:Table><asp:Label ID="lbService" runat="server" Text=" ">
         </asp:Label></asp:Panel><asp:Panel id="panelHidden" runat="server">

         </asp:Panel>
         <%--       
END PANEL MAIN 
--%></asp:Panel><%--       
FOOTER PANEL
--%><asp:Panel id="panelFooter" runat="server"></asp:Panel><%--       
END PANEL WRAP
--%></asp:Panel>

    <asp:HiddenField ID="hfMonoFound" runat="server" />
    <asp:HiddenField ID="hfColorFound" runat="server" />
    <asp:HiddenField ID="hfMicrFound" runat="server" />

<%--       
END SCRIPT
--%><script type="text/javascript">
        //document.aspnetForm.ctl00_BodyContent_ddAgreement.focus();
    </script></form></body></html>