<%@ Page Title="Device Utilization" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="DeviceUtilization.aspx.cs" 
    Inherits="private_customerAdministration_mp_DeviceUtilization" %>
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
    Device Utilization
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

<div class="w3-container w3-padding-32">

    <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />

    <asp:Panel ID="pnHigh" runat="server" Visible="false">
    <h3 class="mb-0">Highest Utilization: > 6%</h3>
    <canvas id="chart_High" style="width: 100%;max-width:900px;"></canvas>							        
        <div class="spacer20"></div>

       <asp:GridView ID="gv_High" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="false" 
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Model" DataField="Part" SortExpression="Part" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <%--  
            <asp:BoundField HeaderText="Asset" DataField="FixedAsset" SortExpression="FixedAsset" ItemStyle-HorizontalAlign="Left" />
                --%>
            <asp:BoundField HeaderText="Loc" DataField="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Util" DataField="UtilPercent" SortExpression="UtilPercent" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
        <div class="spacer40"></div>

    </asp:Panel>
    
    
    <asp:Panel ID="pnLow" runat="server" Visible="false">    
    <h3 class="mb-0">Lowest Utilization: < 1%</h3>
    <canvas id="chart_Low" style="width: 100%;max-width:900px;"></canvas>
        <div class="spacer20"></div>
               <asp:GridView ID="gv_Low" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="false" 
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <asp:BoundField HeaderText="Model" DataField="Part" SortExpression="Part" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Serial" DataField="Serial" SortExpression="Serial" ItemStyle-HorizontalAlign="Left" />
            <%-- 
            <asp:BoundField HeaderText="Asset" DataField="FixedAsset" SortExpression="FixedAsset" ItemStyle-HorizontalAlign="Left" />
                 --%>
            <asp:BoundField HeaderText="Loc" DataField="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Util" DataField="UtilPercent" SortExpression="UtilPercent" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>

    </asp:Panel>
    


</div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    
    <asp:HiddenField ID="hfChartHighLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartHighData" runat="server" Value="" />

    <asp:HiddenField ID="hfChartLowLabel" runat="server" Value="" />
    <asp:HiddenField ID="hfChartLowData" runat="server" Value="" />

    <script>
/*
        var xValues = ["Italy 12345", "France ABC-123", "Spain More Here", "USA WILL BE VERY LONG", "Argentina TO SEE WHAT IT DOES"];
        var yValues = [55, 49, 44, 24, 17];
        var barColors = [
            "#406080",
            "#ad0034",
            "#ffffcc",
            "#3a7728",
            "#8e5ea2"
        ];
        */
        var xValues = "";
        var yValues = "";
        var barColors = ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850", "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"];

        if (document.getElementById("chart_High") != null)
        {
            var canvas = document.getElementById("chart_High");
            

            if (document.getElementById("ctl00_BodyContent_hfChartHighLabel") != null) {
                xValues = document.getElementById("ctl00_BodyContent_hfChartHighLabel").value;
                xValues = xValues.split(",");
            }

            if (document.getElementById("ctl00_BodyContent_hfChartHighData") != null) {
                yValues = (document.getElementById("ctl00_BodyContent_hfChartHighData").value);
                yValues = yValues.split(",");
            }

            //xValues = ["Italy 12345", "France ABC-123", "Spain More Here", "USA WILL BE VERY LONG", "Argentina TO SEE WHAT IT DOES"];
            //yValues = [55, 49, 44, 24, 17];


            new Chart("chart_High", {
                type: "horizontalBar",
                data: {
                    labels: xValues,
                    datasets: [{
                        backgroundColor: barColors,
                        data: yValues
                    }]
                },
                options: {
                    legend: { display: false },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }],
                    }
                }
            });
        }


        if (document.getElementById("chart_Low") != null) {
            if (document.getElementById("ctl00_BodyContent_hfChartLowLabel") != null) {
                xValues = document.getElementById("ctl00_BodyContent_hfChartLowLabel").value;
                xValues = xValues.split(",");
            }

            if (document.getElementById("ctl00_BodyContent_hfChartLowData") != null) {
                yValues = (document.getElementById("ctl00_BodyContent_hfChartLowData").value);
                yValues = yValues.split(",");
            }
            //xValues = ["Italy 12345", "France ABC-123", "Spain More Here", "USA WILL BE VERY LONG", "Argentina TO SEE WHAT IT DOES"];
            //yValues = [55, 49, 44, 24, 17];


            new Chart("chart_Low", {
                type: "horizontalBar",
                data: {
                    labels: xValues,
                    datasets: [{
                        backgroundColor: barColors,
                        data: yValues
                    }]
                },
                options: {
                    legend: { display: false },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: false
                            }
                        }]
                    }
                }
            });
        }


    </script>

    <%--  --%>
</div>
</asp:Content>

