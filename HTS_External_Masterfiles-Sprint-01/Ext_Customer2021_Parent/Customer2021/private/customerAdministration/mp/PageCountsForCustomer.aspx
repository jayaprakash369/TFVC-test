<%@ Page Title="Page Count History" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="PageCountsForCustomer.aspx.cs" 
    Inherits="private_customerAdministration_mp_PageCountsForCustomer" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Page Count History
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

<div class="w3-container w3-padding-32">

    <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />

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
    
    <asp:Panel ID="pnMicr" runat="server" Visible="false">
       		<div class="col-xl-6 col-xxl-9">
					<div class="card-header">
					<h3 class="mb-0">Micr Pages</h3>
				</div>
				<div class="card-body py-3">
					<div class="chart chart-sm">
                        <canvas id="chart_Micr"></canvas>						
					</div>
				</div>
		</div>
    </asp:Panel>

    <canvas id="chart_Test"></canvas>						

</div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    
    <asp:HiddenField ID="hfChartMonoLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMonoData" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMonoIncrement" runat="server" />

    <asp:HiddenField ID="hfChartColorLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartColorData" runat="server" Value="" />
    <asp:HiddenField ID="hfChartColorIncrement" runat="server" />

    <asp:HiddenField ID="hfChartMicrLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMicrData" runat="server" Value="" />
    <asp:HiddenField ID="hfChartMicrIncrement" runat="server" />


    

    <script>
        /*
        function showAlert() { alert('make a chart'); };
            showAlert();
        */

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

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
            
            /*
            if (document.getElementById(' < % = hfChartDataAA.ClientID % > ') != null) {
            */

<%-- 
         --%>
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
                    jMonoIncrement = document.getElementById("ctl00_BodyContent_hfChartColorIncrement").value;

                new Chart(document.getElementById("chart_Color"), {
                    type: 'bar',
                    data: {
                        /* labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], */
                        labels: jaLabels,
                        datasets: [{
                            // label: "This month",
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
            if (document.getElementById("chart_Micr") != null)
            {
                if (document.getElementById("ctl00_BodyContent_hfChartMicrLabel") != null) {
                    jsLabels = document.getElementById("ctl00_BodyContent_hfChartMicrLabel").value;
                    jaLabels = jsLabels.split(",");
                }

                if (document.getElementById("ctl00_BodyContent_hfChartMicrData") != null) {
                    jsData = (document.getElementById("ctl00_BodyContent_hfChartMicrData").value);
                    jaData = jsData.split(",");
                }
                if (document.getElementById("ctl00_BodyContent_hfChartMicrIncrement") != null)
                    jMonoIncrement = document.getElementById("ctl00_BodyContent_hfChartMicrIncrement").value;

                new Chart(document.getElementById("chart_Micr"), {
                    type: 'bar',
                    data: {
                        /* labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], */
                        labels: jaLabels,
                        datasets: [{
                            // label: "This month",
                            backgroundColor: jaColors[3],
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
                                    stepSize: jMicrIncrement  /* 20 500000 */
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
            // ------------------------------------------------------------------------
        };

        showCharts();

        function showChartTest() {
            var jsLabels = "";
            var jsData = "";
            var jaLabels = ["", ""];
            var jaData = [0];
            var jaColors = ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"];


<%-- 
            // Bar chart
            //new Chart(document.getElementById("bar-chart"), {
            new Chart(document.getElementById("chart1"), {
                type: 'bar',
                data: {
                    labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
                    datasets: [
                        {
                            label: "Population (millions)",
                            backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                            data: [2478, 5267, 734, 784, 433]
                        }
                    ]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: 'Predicted world population (millions) in 2050'
                    }
                }
            });
         --%>
            // ------------------------------------------------------------------------
            if (document.getElementById("chart_Test") != null) {
                jsLabels = "One|Two|Three";
                jaLabels = jsLabels.split("|");

                jsData = "2700|3000|3200";
                jaData = jsData.split("|");

                jTestIncrement = "1500";

                new Chart(document.getElementById("chart_Test"), {
                    type: 'bar',
                    data: {
                        /* labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], */
                        labels: jaLabels,
                        datasets: [{
                            //label: "Column",
                            backgroundColor: jaColors[0],
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
                    options:
                    {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        tooltips:
                        {
                            callbacks:
                            {
                                label: function (tooltipItem, data)
                                {
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
                                    stepSize: jTestIncrement,  /* 20 500000 */
                                    beginAtZero: true,
                                    callback: function (label, index, values) {
                                        // return '$' + label; 
                                        return Intl.NumberFormat().format(label);
                                    }
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


        };

        // showChartTest();
    </script>

    <%--  --%>
</div>
</asp:Content>

