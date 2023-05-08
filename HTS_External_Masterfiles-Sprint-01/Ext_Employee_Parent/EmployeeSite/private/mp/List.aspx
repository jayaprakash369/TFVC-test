<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" 
    Inherits="private_mp_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Toner Shipment List</title>

<style type="text/css">
.smallHead
{
    font-size: 10px;
}
.verdictButton
{
    width: 25px;
    height: 20px;
    border: 2px solid #000000;
}
.verdictButtonOld
{
    font-size: 12px;
    padding: 3px;
}
.detailButton
{
    font-size: 12px;
    padding: 3px;
    height: 27px;
    background-color: #EEEEEE;
    color: #333333;
}
.inputTable td
{
    padding-right: 5px;
    padding-left: 5px;
}

.tableSpacer td
{
    border-bottom: 1px solid #333333;
    border-left: 1px solid #FFFFFF;
    border-right: 1px solid #FFFFFF;
}
.cartridgeCell
{
    margin: 0px;
    min-width: 650px;
}
.cartridgeTable
{
    min-width: 100%;
}
</style>
<script type="text/javascript">
    // =============================================================
    function clearInput() {
        var doc = document.forms[0];
        doc.ddCustomer.value = "";
        doc.ddDaysToEmpty.value = "";
        doc.ddSilentWeeks.value = "";
        doc.ddPriority.value = "";
        doc.ddShipOnly.value = "";
        doc.ddTonerLevel.value = "";
        doc.txFxa.value = "";
        doc.txMod.value = "";
        doc.txSer.value = "";
        doc.txKey.value = "";
        doc.txUnt.value = "";
        doc.ddSort.value = "Rank";
        doc.ddMaxRows.value = "250";
        //doc.ctl00_BodyContent_txOrderDate.value = "";
        return true;
    }
    // =============================================================
    function resetInput() {
        //var doc = document.forms[0] + ".ctl00_BodyContent_";
        var doc = document.forms[0];
        /*
        doc.ctl00_BodyContent_ddCustomer.value = "";
        */
        doc.ddCustomer.value = "";
        doc.ddDaysToEmpty.value = "14";
        doc.ddSilentWeeks.value = "8";
        doc.ddPriority.value = "";
        doc.ddShipOnly.value = "";
        doc.ddTonerLevel.value = "";
        doc.txFxa.value = "";
        doc.txMod.value = "";
        doc.txSer.value = "";
        doc.txKey.value = "";
        doc.txUnt.value = "";
        doc.ddSort.value = "Rank";
        doc.ddMaxRows.value = "250";
        return true;
    }
    // =============================================================
    function verdictClick(jName) {
        var objBtn;
        var btnStyle;
        var doc = document.forms[0];
        //        alert("Parm.. " + jName);
        //        var jName2 = "ctl00_BodyContent_~" + jName;
        var jName2 = "~" + jName;

        if (document.getElementById(jName2) != null) {
            objBtn = document.getElementById(jName2);
            btnStyle = objBtn.style;

            if (objBtn.value == "-") { // 0 = Undecided to 1 = Ship
                objBtn.value = ".";
                btnStyle.background = "#3CB371";  // light 33cc33 // 006400 // lime 00FF00
                btnStyle.color = "#3CB371";  // FFFFFF
                btnStyle.border = "#FFFFFF 2px solid";
            }
            else if (objBtn.value == ".") { // 2 = Wait to 0 = undecided
                objBtn.value = "-";
                btnStyle.background = "#DDDDDD"; // FFFFFF
                btnStyle.color = "#DDDDDD";  // 333333 // FFFFFF
                btnStyle.border = "#FFFFFF 2px solid";
            }
        }
        return true;
    }
    // =============================================================
    function detailClick(jKey, jSeq) {
        var doc = document.forms[0];
        doc.hfKey.value = jKey;
        doc.hfSeq.value = jSeq;
        doc.hfPanelToShow.value = "Detail";
        doc.submit();
        return true;
    }

    // =============================================================
    function resetTable() {
        var jSeq = 0;
        var doc = document.forms[0];
        if (doc.hfPanelToShow.value == "List") {
            if (doc.hfSeq.value.length > 0) {
                jSeq = doc.hfSeq.value;
                if (jSeq > 1) {
                    //jSeq = (jSeq - 2);
                    jSeq = (jSeq - 1);
                    window.location = "#A" + jSeq;
                }
            }
        }
    }
    // =============================================================
</script>
<%--
--%>             

</head>
<body>
    <form id="form1" runat="server">

<asp:HiddenField ID="hfKey" runat="server"  Value="" />
<asp:HiddenField ID="hfSeq" runat="server" Value="1" />
<asp:HiddenField ID="hfPanelToShow" runat="server" Value="" />

    <asp:Panel ID="pnListPage" runat="server">
    <div id="dvWhite" style="position:fixed; top:0px; left: 0px; width:100%; height:120px; z-index:20; border-bottom: 1px solid #333333; background-color:#FFFFFF;">
        <h2 style="position: relative; top: 25px; left: 40px; font-family: Verdana; font-size: 20px; ">Shipment List</h2>
    </div>

    <div id="dvInput" style="position:fixed; top:15px; left:250px; z-index: 30; font-size:12px;">
        <asp:Table ID="tbInput" runat="server" CssClass="inputTable">

            <asp:TableRow>

                <asp:TableCell HorizontalAlign="Right">Asset</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="txFxa" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Unit</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="txUnt" runat="server" Width="90" />
                </asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Hide Silent Weeks</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddSilentWeeks" runat="server" />
                    &nbsp;<input type="button" name="htmlReset" value="Reset" onclick="return resetInput();" />
                    &nbsp;<input type="button" name="htmlClear" value="Clear All" onclick="return clearInput();" />
                </asp:TableCell>
         
            </asp:TableRow>

            <asp:TableRow>

                <asp:TableCell HorizontalAlign="Right">Model</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="txMod" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Empty</asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddDaysToEmpty" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Max Rows</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddMaxRows" runat="server" >
                        <asp:ListItem Text="25" Value="25" />
                        <asp:ListItem Text="50" Value="50" />
                        <asp:ListItem Text="100" Value="100" />
                        <asp:ListItem Text="200" Value="200" />
                        <asp:ListItem Text="250" Value="250" Selected="True" />
                        <asp:ListItem Text="300" Value="300" />
                        <asp:ListItem Text="350" Value="350" />
                        <asp:ListItem Text="400" Value="400" />
                        <asp:ListItem Text="450" Value="450" />
                        <asp:ListItem Text="500" Value="500" />
                        <asp:ListItem Text="750" Value="750" />
                        <asp:ListItem Text="1000" Value="1000" />
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">Serial</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="txSer" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Level</asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddTonerLevel" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Show Ship Only?</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddShipOnly" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>

                <asp:TableCell HorizontalAlign="Right">Cust</asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddCustomer" runat="server" /></asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Priority</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddPriority" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                    </asp:DropDownList>
                    
                    &nbsp;&nbsp;
                    Key 
                    <asp:TextBox ID="txKey" runat="server" Width="40" />
                    
                </asp:TableCell>

                <asp:TableCell HorizontalAlign="Right">Sort Order</asp:TableCell>                
                <asp:TableCell>
                    <asp:DropDownList ID="ddSort" runat="server">
                        <asp:ListItem Text="Rank" Value="Rank"></asp:ListItem>
                        <asp:ListItem Text="Qty/Level" Value="Qty/Level"></asp:ListItem>
                        <asp:ListItem Text="Level" Value="Level"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txOrderDate" runat="server" Width="100" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btRun" Text="Reload" runat="server" onclick="btRun_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>

<%--  =====================================================================================================
--%>             

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dvShipList" style="position: relative; z-index:10; padding-top: 140px;  margin-bottom: 30px;">
                <center>
                    <asp:Table ID="tbShipList" runat="server" CssClass="tableWithLines" Width="96%">
                    </asp:Table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Panel>
<%--  =====================================================================================================
--%>             

    <asp:Panel ID="pnDetailPage" runat="server">
    <center>
    <div id="dvDetail" runat="server" style="margin-top: 40px; margin-bottom: 30px;">
        <asp:Table ID="Table1" runat="server" CssClass="tableWithoutLines">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btUpdate" runat="server" Text="Update" onclick="btUpdate_Click"  />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Right">Comment?</asp:TableCell>
                <asp:TableCell ColumnSpan="6">
                    <asp:TextBox ID="txUpdComment" runat="server" Width="500" Text="" MaxLength="250"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Right">Priority?</asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:DropDownList ID="ddUpdPriority" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>&nbsp;</asp:TableCell>
                <asp:TableCell>Qty On Hand:</asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    &nbsp;
                    Blk <asp:DropDownList ID="ddQohBlk" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    Mic <asp:DropDownList ID="ddQohMic" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    Cyn <asp:DropDownList ID="ddQohCyn" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    Mag <asp:DropDownList ID="ddQohMag" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    Ylw <asp:DropDownList ID="ddQohYlw" runat="server" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Right">Ship Via?</asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:DropDownList ID="ddVia" runat="server">
                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Second Day" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Next Day Saver" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Next Day Air" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
<%--
--%>             

<h2>Printer Detail</h2>
<br />
    <asp:GridView ID="gvHeader" runat="server" CssClass="tableWithLines" Width="900">
    </asp:GridView>

<br /><br />

<h2>Past Toner Orders</h2>
<br />
    <asp:GridView ID="gvOrders" runat="server" CssClass="tableWithLines" Width="900">
    </asp:GridView>

<br /><br />
<h2>Past Comments</h2>
<br />
<asp:GridView ID="gvComments" runat="server" CssClass="tableWithLines">
</asp:GridView>

<br /><br />
<h2>Daily Toner Level Scans</h2>
<br />
<asp:GridView ID="gvTonerLog" runat="server" CssClass="tableWithLines">
</asp:GridView>
</div><!-- End div detail... -->
</center>
    </asp:Panel>
<br />
<br />

<script type="text/javascript">
    resetTable();
</script>

    </form>
</body>
</html>


