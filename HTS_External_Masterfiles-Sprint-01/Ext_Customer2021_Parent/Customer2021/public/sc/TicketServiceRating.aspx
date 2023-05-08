<%@ Page Title="Service Rating" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="TicketServiceRating.aspx.cs" 
    Inherits="public_sc_TicketServiceRating" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
        <style type="text/css">
        .panelFormat {
            margin-top: 20px;
        }
            .auto-style1 {
                width: 909px;
            }
            .auto-style2 {
                width: 988px;
            }
    </style>

        <script type="text/javascript">
            function scrub() {
                var temp = '';
                var i = 0;
                var replaced = '';

                if (document.getElementById("ctl00_BodyContent_TextBox1") != undefined &&
                    document.getElementById("ctl00_BodyContent_TextBox1") != null)
                {
                    var entryGeneral = document.getElementById("ctl00_BodyContent_TextBox1");
                    temp = entryGeneral.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    //if (temp != document.getElementById("ctl00_BodyContent_TextBox1").value)
                  //  alert("General was changed");  // to show you that the code went thru JavaScript
                    document.getElementById("ctl00_BodyContent_TextBox1").value = temp;
                }
                // alert("End");
            }
        </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Service Rating
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-row">
        <div class="w3-container">
            <asp:Label ID="lbMsg" runat="server" SkinID="labelError" Visible="true" />
        </div>
    </div>

    <div class="w3-row w3-padding-16">
        <div class="w3-third w3-container">
            <asp:Panel ID="pnScantron" runat="server" Visible="false">
                <table class="auto-style2">
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1">
                Your feedback helps us serve you better. &nbsp; Please rate our service.
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1"> On a scale of 1 to 10, 10 being the best, how LIKELY are you to RECOMMEND Scantron services to another colleague or company?
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1">
                <asp:RadioButtonList ID="rblTickRating" RepeatDirection="Horizontal" runat="server" SkinID="RadioButtonList1" Width="600px">
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2" />
                    <asp:ListItem Text="3" Value="3" />
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                    <asp:ListItem Text="6" Value="6" />
                    <asp:ListItem Text="7" Value="7" />
                    <asp:ListItem Text="8" Value="8" />                   
                    <asp:ListItem Text="9" Value="9" />
                    <asp:ListItem Text="10" Value="10" />
                 </asp:RadioButtonList>
                </td>        
                <td>&nbsp;</td>
                </tr>
                <tr>
                <td>
                <asp:Panel ID="Panel1" runat="server">
                <p><asp:Label ID="Label1" runat="server" Text="General Comments" /></p>
                <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="250px" Width="440px" MaxLength="1000" /> 
                </asp:Panel>
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr>
                <td>&nbsp;</td>
                <td>
                <asp:Button ID="btSubmit" runat="server" 
                 Text="Submit" SkinID="buttonPrimary" 
                 onClientClick="scrub()"
                 onclick="btSubmit_Click" />
                </td>
                </tr>
            </table>
        </asp:Panel>
        </div>

        <div class="w3-third w3-container">
            <asp:Panel ID="pnSecur" runat="server" Visible="false">
                <table class="tableWithoutLines tableBorder">
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1">
                    Your feedback helps us serve you better. &nbsp; Please rate our service.
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1"> On a scale of 1 to 10, 10 being the best. How LIKELY are you to RECOMMEND Secur-Serv services to another colleague or company?
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align: bottom;" > 
                <td class="auto-style1">
                <asp:RadioButtonList ID="RadioButtonList1" RepeatDirection="Horizontal" runat="server" SkinID="RadioButtonList1" Width="600px">
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2" />
                    <asp:ListItem Text="3" Value="3" />
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                    <asp:ListItem Text="6" Value="6" />
                    <asp:ListItem Text="7" Value="7" />
                    <asp:ListItem Text="8" Value="8" />                   
                    <asp:ListItem Text="9" Value="9" />
                    <asp:ListItem Text="10" Value="10" />
                 </asp:RadioButtonList>
                </td>        
                <td>&nbsp;</td>
                </tr>
                <tr>
                <td>
                <asp:Panel ID="Panel3" runat="server">
                <p><asp:Label ID="Label2" runat="server" Text="General Comments" /></p>
                <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="250px" Width="440px" MaxLength="1000" /> 
                </asp:Panel>
                </td>
                <td>&nbsp;</td>
                </tr>
                <tr>
                <td>&nbsp;</td>
                <td>
                <asp:Button ID="Button1" runat="server" 
                 Text="Submit" SkinID="buttonPrimary" 
                 onClientClick="scrub()"
                 onclick="btSubmit_Click" />
                </td>
                </tr>
            </table>
        </asp:Panel>
        </div>
    </div>
</div>

        <asp:HiddenField ID="hfCenter" runat="server" />
        <asp:HiddenField ID="hfTicket" runat="server" />
        <asp:HiddenField ID="hfUserName" runat="server" />
        <asp:HiddenField ID="hfStsNum" runat="server" />

    <script type="text/javascript">
    //scrub();
    </script>

</asp:Content>
