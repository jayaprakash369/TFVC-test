<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoundedFrame.aspx.cs" 
    Inherits="dev_RoundedFrame" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rounded Outer Box Test</title>
    <!--    width: 100%;  
                padding-top: 10px;
            padding-bottom: 10px;
      -->
    <style>
        .innerObj
        {
            width: 1100px;
            height: 100px;
            background-color: #ffffcc;            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />

    <div style="padding: 0px;">

<br /><br /> TABLE WITH INNER DIVS
        <table id="tbMasterRounded" runat="server" class="tableMasterRounded">
            <tr>
                <td>
                    <table id="tbMasterBoundary" runat="server" class="tableMasterBoundary">
                        <tr>
                            <td>
                                <div class="innerObj">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

     <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server"
        TargetControlID="tbMasterRounded"
        Radius="10"
        Corners="All" />
<%-- 
--%>
    </div>
    </form>
</body>
</html>
