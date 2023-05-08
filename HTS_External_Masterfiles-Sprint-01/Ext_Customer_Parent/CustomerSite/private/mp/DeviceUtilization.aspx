<%@ Page Title="Device Utilization" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="DeviceUtilization.aspx.cs" 
    Inherits="private_mp_DeviceUtilization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Device Utilization
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
<%--    --%>
<asp:Panel ID="pnUtiliz" runat="server">
    <div style="clear: both; text-align: center; margin-top: 30px; margin-bottom: 30px;">
<%--  DEVICES BY UTILIZATION: HIGH  --%>
        <asp:Chart ID="chartByUseHigh" runat="server" 
        Width="900px" 
        Height="500px" 
        Palette="BrightPastel" 
        BackColor="#CCCCCC" 
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
    </div>

    <div style="clear: both; text-align: center; margin-bottom: 30px;">
    <%-- DEVICES BY UTLIZATION: LOW --%>
        <asp:Chart ID="chartByUseLow" runat="server" 
            Width="900px" 
            Height="500px" 
            Palette="BrightPastel" 
            BackColor="#CCCCCC" 
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

    </div>
</asp:Panel>

</asp:Content>

