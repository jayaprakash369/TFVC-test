<%@ Page Title="Page Count History" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="PageCountByFleet.aspx.cs" 
    Inherits="private_mp_PageCountByFleet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Page Count History
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<asp:Panel ID="pnPageCounts" runat="server">
    <%-- PAGE COUNTS: MONOCHROME --%>
    <div style="clear: both; text-align: center; margin-bottom: 30px; margin-top: 30px;">
	    <asp:CHART id="chartByPagesMono" runat="server" 
                Width="900px" 
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
    </div>
    
    <%-- PAGE COUNTS: COLOR --%>
    <div style="clear: both; text-align: center; margin-bottom: 30px;">
		<asp:CHART id="chartByPagesColor" runat="server" 
                Width="900px" 
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
    </div>

    <%-- PAGE COUNTS: MICR  --%>
    <div style="clear: both; text-align: center; margin-bottom: 30px;">
		<asp:CHART id="chartByPagesMicr" runat="server" 
                Width="900px" 
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
    </div>
</asp:Panel>

</asp:Content>

