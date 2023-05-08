<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PdfRequest.aspx.cs" 
    Inherits="private_mps_PdfRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDF Test</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>Create PDF Documents</h2>
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl="~/private/mps/PdfLoad.aspx?id=1">Hello PDF</asp:HyperLink>
        <br /><br />
        <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" NavigateUrl="~/private/mps/PdfLoad.aspx?id=2">PDF With Table</asp:HyperLink>
        <br /><br />
        <asp:HyperLink ID="HyperLink3" runat="server" Target="_blank" NavigateUrl="~/private/mps/PdfLoad.aspx?id=3">MPS PDF</asp:HyperLink>
    </div>
    </form>
</body>
</html>
