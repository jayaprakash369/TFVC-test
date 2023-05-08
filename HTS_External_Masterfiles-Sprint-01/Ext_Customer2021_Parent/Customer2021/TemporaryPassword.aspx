<%@ Page Title="Generate Temporary Password" Language="C#" MasterPageFile="~/Responsive.master" AutoEventWireup="true" CodeFile="TemporaryPassword.aspx.cs" 
    Inherits="TemporaryPassword" %>
    <%--   --%>    

<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
        <style type="text/css">
            .myTd { 
                padding-left: 20px;
                padding-right: 20px;
                padding-top: 20px;
                padding-bottom: 20px;
                background-color: #ffffbb;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Generate Recovery Password
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

        <%-- 
          --%>

    <div class="w3-row w3-padding-32">
        <div class="w3-container">
            
         <table class="tableWithoutLines tableBorder">
         <tr>
            <td style="padding: 15px; padding-bottom: 5px;">Enter the email you use for your account ID</td>
        </tr>

        <tr>
           <td style="padding: 15px; padding-top: 5px; padding-bottom: 15px;"><asp:TextBox ID="txUserIdEmail" runat="server" Width="300" /></td>
            </tr>
            <tr>
               <td style="padding: 15px; padding-top: 15px; "><asp:Button ID="btReset" runat="server" Text="Email Temporary Password" 
                    onclick="btReset_Click" />&nbsp;</td>
        </tr>

        <tr>
        </tr>
    </table>

   <div style="height: 20px; clear: both;"></div>

    <ul style="margin-left: 30px;">
        <li style="padding: 5px;">A temporary random password will be generated and emailed to the address entered above.</li>
        <li style="padding: 5px;">Copy the temporary password you receive, and return to log in.</li>
        <li style="padding: 5px;">Once logged back in, click the "Change Your Password" link on the left navigation menu to update your password as you prefer.</li>
    </ul> 

    <div style="height: 20px; clear: both;"></div>
    <asp:Label ID="lbResult" runat="server" SkinID="labelError" />



            <div class="spacer20"></div>

        </div>
    </div>

    <%-- 
          --%>
</div>
</asp:Content>



