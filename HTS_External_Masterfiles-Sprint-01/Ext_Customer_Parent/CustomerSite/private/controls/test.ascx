<%@ Control Language="C#" AutoEventWireup="true" CodeFile="test.ascx.cs" Inherits="private_controls_test" %>
<%--
How to use a control
1) Add this line to top to target page
    <%@ Register TagPrefix="TestControl" TagName="MyTest" Src="~/private/controls/test.ascx" %>
2) Add this line where you want it to appear in the code
    <TestControl:MyTest ID="myTest1" runat="server" />
(... you don't seem to have the ability to interact with these values from the target page
You have to use this code behind file to handle control events

--%>             
<b>Test</b> content from a control file
<asp:Button ID="btTest" runat="server" Text="Test Button" onclick="btTest_Click" />

