<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitDetail.aspx.cs" 
Inherits="public_scmobile_web_UnitDetail" %>

<%-- 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
width: 480px;
 --%>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Equipment Information</title>
    <style type="text/css">
        body { margin: 2px; } 
        .tbDetail { margin-top: 5px; margin-bottom: 10px;  }
        .tbDetail tr { vertical-align: top; }
        .tbDetail th { padding-top: 2px; padding-bottom: 4px; padding-right: 15px;  text-align: left; width: 100px; }
        .tbDetail td { padding-top: 2px; padding-bottom: 4px; padding-right: 10px; }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:20px; ">
    
    <center><asp:Label ID="lbPageTitle" runat="server" SkinID="TableTitle0" /></center>

    <asp:Panel ID="pnEquipment" runat="server" style="margin-bottom: 20px;">
    <center><asp:Label ID="lbEqpData" runat="server" Text="Equipment Information" SkinID="TableTitle1" /></center>

        <table class="tableWithLines" style="width:100%;">
	        <tr>
		        <td>Model</td>
		        <td><asp:Label ID="lbPart" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Desc</td>
		        <td><asp:Label ID="lbDescription" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Unit</td>
		        <td><asp:Label ID="lbUnit" runat="server" /></td>
	        </tr>
            <tr>
		        <td>Serial</td>
		        <td><asp:Label ID="lbSerial" runat="server" /></td>
	        </tr>
            <tr>
		        <td>Fx Asset</td>
		        <td><asp:Label ID="lbFixAsset" runat="server" /></td>
	        </tr>
	        <tr>
		        <td>Eqp Type</td>
		        <td><asp:Label ID="lbEqpType" runat="server" /></td>
	        </tr>
      	    <tr>
		    <td>Agreement</td>
		    <td><asp:Label ID="lbAgr" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Agr Type</td>
		    <td><asp:Label ID="lbAgrType" runat="server" /></td>
	    </tr>
        <tr>
		    <td>Agr Start</td>
		    <td><asp:Label ID="lbAgrBeg" runat="server" /></td>
	    </tr>
        <tr>
		    <td>Agr End</td>
		    <td><asp:Label ID="lbAgrEnd" runat="server" /></td>
	    </tr>
        <tr>
		    <td>Date Added</td>
		    <td><asp:Label ID="lbAddDte" runat="server" /></td>
	    </tr>
        </table>
    </asp:Panel>



    <asp:Panel ID="pnCustomer" runat="server" style="margin-bottom: 20px; margin-top: 10px;">
    <center><asp:Label ID="lbCs1Data" runat="server" Text="Customer Information" SkinID="TableTitle1" /></center>
    <table class="tableWithLines" style="width:100%;">
	    <tr>
		    <td>Cust Name</td>
		    <td><asp:Label ID="lbCustName" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Cust No.</td>
		    <td><asp:Label ID="lbCs1Cs2" runat="server" /></td>
	    </tr>
        <tr>
		    <td>Address</td>
		    <td><asp:Label ID="lbAddress1" runat="server" /><asp:Label ID="lbAddress2" runat="server" /></td>
	    </tr>
        <tr>
		    <td>CSZ</td>
		    <td><asp:Label ID="lbCtyStZp" runat="server" /></td>
	    </tr>
	    
	    <tr>
		    <td>Contact</td>
		    <td><asp:Label ID="lbContact" runat="server" /></td>
	    </tr>
	    <tr>
		    <td>Phone</td>
		    <td><asp:Label ID="lbPhone" runat="server" /></td>
	    </tr>
	    <tr>
            <td>Center</td>
		    <td><asp:Label ID="lbCenter" runat="server" />&nbsp;&nbsp;<asp:Label ID="lbCenterName" runat="server" /></td>
	    </tr>
    </table>
    </asp:Panel>

<%-- 

 --%>
    
    </div>
    <asp:HiddenField ID="hfMdl" runat="server" />
    <asp:HiddenField ID="hfUnit" runat="server" />
    <asp:HiddenField ID="hfCs1" runat="server" />
    <asp:HiddenField ID="hfCs2" runat="server" />

    </form>
</body>
</html>
