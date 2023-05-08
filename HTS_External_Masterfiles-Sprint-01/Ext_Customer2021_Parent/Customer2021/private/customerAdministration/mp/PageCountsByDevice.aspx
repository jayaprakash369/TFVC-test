<%@ Page Title="Page Counts By Device" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="PageCountsByDevice.aspx.cs" 
    Inherits="private_customerAdministration_mp_PageCountsByDevice" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <%--  --%>
        <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .UpdateElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
            vertical-align: bottom;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Page Counts By Device
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32"">
    <asp:Label ID="lbMsg" runat="server" SkinId="labelError" />
    <!-- START: PANEL (DEVICE) ======================================================================================= -->
    <asp:Panel ID="pnDevice" runat="server">
    
        <!-- START: SEARCH PANEL (DEVICE) ======================================================================================= -->
        <asp:Panel ID="pnDeviceSearch" runat="server" DefaultButton="btDeviceSearchSubmit">
            
            <h3 class="titlePrimary">Select Device</h3>
            <table class="tableBorderBackgroundLight" style="margin-bottom: 10px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Location Name<br />
                    <asp:TextBox ID="txSearchLocationName" runat="server" Width="175" />
                </div>
                <div class="SearchPanelElements">
                    Model<br />
                        <asp:TextBox ID="txSearchModel" runat="server" Width="200" />
                </div>
                <div class="SearchPanelElements">
                    Serial<br />
                        <asp:TextBox ID="txSearchSerial" runat="server" Width="200" />
                </div>
                <div class="SearchPanelElements">
                    Equip Xref<br />
                        <asp:TextBox ID="txSearchEquipmentCrossRef" runat="server" Width="175" />
                </div>
                <asp:Panel ID="pnSearchCustomerFamily" runat="server">
                    <div class="SearchPanelElements">
                        Customer<br />
                        <asp:DropDownList ID="ddSearchCustomerFamily" runat="server" />
                    </div>
                </asp:Panel>
                <div class="SearchPanelElements">
                    Loc<br />
                        <asp:TextBox ID="txSearchLocation" runat="server" Width="40" />
                </div>
                <div class="SearchPanelElements">
                    Loc Xref<br />
                        <asp:TextBox ID="txSearchLocationCrossRef" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    City<br />
                        <asp:TextBox ID="txSearchCity" runat="server" Width="150" />
                </div>
                <div class="SearchPanelElements">
                    State<br />
                        <asp:TextBox ID="txSearchState" runat="server" Width="40" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btDeviceSearchSubmit" runat="server" Text="Search" OnClick="btDeviceSearchSubmit_Click" SkinId="buttonPrimary" />
                    &nbsp;
                    <asp:Button ID="btDeviceSearchClear" runat="server" Text="Clear" OnClick="btDeviceSearchClear_Click" />
                </div>
            </td>
                </tr>
            </table>
        </asp:Panel>


        <!-- START: SMALL SCREEN TABLE (DEVICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_DeviceSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Location Name</td>
                        <td><asp:Label ID="lbDeviceLocationNameA" runat="server" Text='<% #Eval("LocationName") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Model</td>
                        <td>
                            <asp:Label ID="lbDeviceModelA" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Serial</td>
                        <td>
                            <asp:LinkButton ID="lkDeviceSerialA" runat="server" 
                                OnClick="lkDeviceSerial_Click" 
                                Text='<%# Eval("Serial") %>'
                                CommandArgument='<%# Eval("Unit") %>'>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Equip Xref</td>
                        <td>
                            <asp:Label ID="lbDeviceEquipmentCrossRefA" runat="server" Text='<%# Eval("EquipmentCrossRef") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbDeviceCustomerNumberA" runat="server" Text='<%# Eval("CustomerNumber") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location</td>
                        <td>
                            <asp:Label ID="lbDeviceCustomerLocationA" runat="server" Text='<%# Eval("CustomerLocation") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Loc Xref</td>
                        <td>
                            <asp:Label ID="lbDeviceLocationCrossRefA" runat="server" Text='<%# Eval("LocationCrossRef") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>City</td>
                        <td>
                            <asp:Label ID="lbDeviceCityA" runat="server" Text='<%# Eval("City") %>' />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>State</td>
                        <td>
                            <asp:Label ID="lbDeviceStateA" runat="server" Text='<%# Eval("State") %>' />
                        </td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Location Name</td>
                        <td><asp:Label ID="lbDeviceLocationNameB" runat="server" Text='<% #Eval("LocationName") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Model</td>
                        <td>
                            <asp:Label ID="lbDeviceModelB" runat="server" Text='<%# Eval("Model") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Serial</td>
                        <td>
                            <asp:LinkButton ID="lkDeviceSerialB" runat="server" 
                                OnClick="lkDeviceSerial_Click" 
                                Text='<%# Eval("Serial") %>'
                                CommandArgument='<%# Eval("Unit") %>'>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Equip Xref</td>
                        <td>
                            <asp:Label ID="lbDeviceEquipmentCrossRefB" runat="server" Text='<%# Eval("EquipmentCrossRef") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Customer</td>
                        <td>
                            <asp:Label ID="lbDeviceCustomerNumberB" runat="server" Text='<%# Eval("CustomerNumber") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location</td>
                        <td>
                            <asp:Label ID="lbDeviceCustomerLocationB" runat="server" Text='<%# Eval("CustomerLocation") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Loc Xref</td>
                        <td>
                            <asp:Label ID="lbDeviceLocationCrossRefB" runat="server" Text='<%# Eval("LocationCrossRef") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>City</td>
                        <td>
                            <asp:Label ID="lbDeviceCityB" runat="server" Text='<%# Eval("City") %>' />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>State</td>
                        <td>
                            <asp:Label ID="lbDeviceStateB" runat="server" Text='<%# Eval("State") %>' />
                        </td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE (DEVICE) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (DEVICE) ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->


        <asp:GridView ID="gv_DeviceLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_Dev"
            allowPaging="true"
            OnPageIndexChanging="gvPageIndexChanging_Dev"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Location Name" DataField="LocationName" SortExpression="LocationName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Serial" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkDeviceSerial" runat="server" 
                        OnClick="lkDeviceSerial_Click" 
                        Text='<%# Eval("Serial") %>'
                        CommandArgument='<%# Eval("Unit")  %>'>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Equip Xref" DataField="EquipmentCrossRef" SortExpression="EquipmentCrossRef" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Customer" DataField="CustomerNumber" SortExpression="CustomerNumberSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc" DataField="CustomerLocation" SortExpression="CustomerLocationSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc Xref" DataField="LocationCrossRef" SortExpression="LocationCrossRef" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" ItemStyle-HorizontalAlign="Left" />
            
        </Columns>
    </asp:GridView>

    <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (DEVICE) ======================================================================================= -->

</asp:Panel><!-- END: PANEL (DEVICE) ======================================================================================= -->
    
    <!-- START: PANEL (PAGES) ======================================================================================= -->
    <asp:Panel ID="pnPages" runat="server" Visible="false">

            <asp:Panel ID="pnMono" runat="server" Visible="false">
 		<div class="col-xl-6 col-xxl-9">
			<!--<div class="card flex-fill w-100">-->
					<div class="card-header">
					<h3 class="mb-0">Mono Pages</h3>
				</div>
				<div class="card-body py-3">
					<div class="chart chart-sm">
			<canvas id="chart_Mono"></canvas>							        
					</div>
				</div>
			<!-- </div> --> 
		</div>
        </asp:Panel>

    <asp:Panel ID="pnColor" runat="server" Visible="false">
   		<div class="col-xl-6 col-xxl-9">
					<div class="card-header">
					<h3 class="mb-0">Color Pages</h3>
				</div>
				<div class="card-body py-3">
					<div class="chart chart-sm">
                        <canvas id="chart_Color"></canvas>						
					</div>
				</div>
		</div>
        </asp:Panel>


    </asp:Panel><!-- END: PANEL (PAGES) ======================================================================================= -->


    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfPreferenceToAllowLocationCrossRefUpdate" runat="server" />

    <asp:HiddenField ID="hfChartMonoLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMonoData" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMonoIncrement" runat="server" />

    <asp:HiddenField ID="hfChartColorLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartColorData" runat="server" Value="" />
    <asp:HiddenField ID="hfChartColorIncrement" runat="server" />

        <script>

        function showCharts()
        {
            var jsLabels =  "" ;
            var jsData = "";
            var jMonoIncrement = 0;
            var jColorIncrement = 0;
            var jMicrIncrement = 0;
            //var jaLabels = [""];
            //var jaData = [0];
            var jaLabels = ["", ""];
            var jaData = [0];
            var jaColors = ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"];
           
            //alert("START");
            // ------------------------------------------------------------------------
            if (document.getElementById("chart_Mono") != null) {

                if (document.getElementById("ctl00_BodyContent_hfChartMonoLabel") != null) {
                    jsLabels = document.getElementById("ctl00_BodyContent_hfChartMonoLabel").value;
                    jaLabels = jsLabels.split(",");
                }

                if (document.getElementById("ctl00_BodyContent_hfChartMonoData") != null) {
                    jsData = (document.getElementById("ctl00_BodyContent_hfChartMonoData").value);
                    jaData = jsData.split(",");
                }
                if (document.getElementById("ctl00_BodyContent_hfChartMonoIncrement") != null)
                    jMonoIncrement = document.getElementById("ctl00_BodyContent_hfChartMonoIncrement").value;


                new Chart(document.getElementById("chart_Mono"), {
                    type: 'bar',
                    data: {
                        /* labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], */
                        labels: jaLabels,
                        datasets: [{
                            // label: "This month",
                            backgroundColor: jaColors[0],
                            /*
                             * backgroundColor: window.theme.primary,
                            borderColor: window.theme.primary,
                            hoverBackgroundColor: window.theme.primary,
                            hoverBorderColor: window.theme.primary,
                            */
                            /* data: [54, 67, 41, 55, 62, 45, 55, 73, 60, 76, 48, 79], */
                            data: jaData,
                            barPercentage: .75,
                            categoryPercentage: .5
                        }]
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        tooltips:
                        {
                            callbacks:
                            {
                                label: function (tooltipItem, data) {
                                    //return tootipItem.yLabel; // default value
                                    return " Pages: " + Intl.NumberFormat().format(tooltipItem.yLabel);
                                }
                            }
                        },
                        scales: {
                            yAxes: [{
                                gridLines: {
                                    display: true
                                },
                                stacked: true,
                                ticks: {
                                    stepSize: jMonoIncrement  /* 20 500000 */
                                    , callback: function (label, index, values) {
                                        return Intl.NumberFormat().format(label);
                                    }
                                    /*, beginAtZero: true */
                                }
                            }],
                            xAxes: [{
                                stacked: false,
                                gridLines: {
                                    color: "transparent" /* "#406080" "green" "transparent" */
                                }
                            }]
                        }
                    }
                });
            }
            else
            {

            }
            // ------------------------------------------------------------------------
            //alert("COLOR");
            if (document.getElementById("chart_Color") != null)
            {
                if (document.getElementById("ctl00_BodyContent_hfChartColorLabel") != null) {
                    jsLabels = document.getElementById("ctl00_BodyContent_hfChartColorLabel").value;
                    jaLabels = jsLabels.split(",");
                }

                if (document.getElementById("ctl00_BodyContent_hfChartColorData") != null) {
                    jsData = (document.getElementById("ctl00_BodyContent_hfChartColorData").value);
                    jaData = jsData.split(",");
                }
                if (document.getElementById("ctl00_BodyContent_hfChartColorIncrement") != null)
                    jMonoIncrement = document.getElementById("ctl00_BodyContent_hfChartMonoIncrement").value;

                new Chart(document.getElementById("chart_Color"), {
                    type: 'bar',
                    data: {
                        /* labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], */
                        labels: jaLabels,
                        datasets: [{
                            //label: "This month",
                            backgroundColor: jaColors[1],
                            /*backgroundColor: window.theme.primary,
                            borderColor: window.theme.primary,
                            hoverBackgroundColor: window.theme.primary,
                            hoverBorderColor: window.theme.primary,*/
                            /* data: [54, 67, 41, 55, 62, 45, 55, 73, 60, 76, 48, 79], */
                            data: jaData,
                            barPercentage: .75,
                            categoryPercentage: .5
                        }]
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        tooltips:
                        {
                            callbacks:
                            {
                                label: function (tooltipItem, data) {
                                    //return tootipItem.yLabel; // default value
                                    return " Pages: " + Intl.NumberFormat().format(tooltipItem.yLabel);
                                }
                            }
                        },
                        scales: {
                            yAxes: [{
                                gridLines: {
                                    display: true
                                },
                                stacked: true,
                                ticks: {
                                    stepSize: jColorIncrement  /* 20 500000 */
                                    , callback: function (label, index, values) {
                                        return Intl.NumberFormat().format(label);
                                    }
                                    /*, beginAtZero: true */
                                }
                            }],
                            xAxes: [{
                                stacked: false,
                                gridLines: {
                                    color: "transparent" /* "#406080" "green" "transparent" */
                                }
                            }]
                        }
                    }
                });
            }
            // ------------------------------------------------------------------------
            //alert("END");
        };

        showCharts();
        </script>


        <%--  --%>
</div>
</asp:Content>


