<%@ Page Title="Double Submit" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="DoubleSubmit.aspx.cs" Inherits="dev_DoubleSubmit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Stopping Double Submit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

    
    <asp:TextBox ID="TextBox1" runat="server" Text="READY" />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Stop Double Submit" onclick="Button1_Click" />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Default Text" />

    <script type="text/javascript">
        function startupTest() {
            alert("Doing startup Test...");
            var doc = document.forms[0];
            doc.ctl00_ctl00_For_Body_A_For_Body_A_Button1.value = "Setting Default Value";
        }
//        startupTest();
    </script>

</asp:Content>

