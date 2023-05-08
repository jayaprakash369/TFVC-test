<%@ Page Title="" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="javascript.aspx.cs" Inherits="private__admin_test_javascript" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Javascript Testing
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">
    <script type="text/javascript">
        function ReadCookie(cookieName) {
            var theCookie = "" + document.cookie;
            var ind = theCookie.indexOf(cookieName);
            if (ind == -1 || cookieName == "") return "";
            var ind1 = theCookie.indexOf(';', ind);
            if (ind1 == -1) ind1 = theCookie.length;
            return unescape(theCookie.substring(ind + cookieName.length + 1, ind1));
        }
        // ----------------------------
        var i = 0;
        var jRem = 0;
        var jPick = "";
        for (i = 0; i < 10; i++) {
            jRem = i % 4;
            alert(jRem + " and index...  " + i);
            if (jRem == 0) {
                alert("Match");
                jPick += i + " and ";
            }

        }
        alert(jPick);
       
    </script>
</asp:Content>

