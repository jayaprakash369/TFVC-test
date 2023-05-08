<%@ Page Title="Comments" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Comments.aspx.cs" 
    Inherits="private_sc_Comments" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
    <style type="text/css">
        .panelFormat {
            margin-top: 20px;
        }
    </style>

            <script type="text/javascript">
            function scrub()
            {
                var temp = '';                
                var i = 0;
                var replaced = '';

                if (document.getElementById("ctl00_BodyContent_txGeneral") != undefined &&
                    document.getElementById("ctl00_BodyContent_txGeneral") != null)
                {
                    var entryGeneral = document.getElementById("ctl00_BodyContent_txGeneral");
                    temp = entryGeneral.value;
                    for (i = 0; i < 20; i++)
                    {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    //if (temp != document.getElementById("ctl00_BodyContent_txGeneral").value)
                    //    alert("General was changed");
                    document.getElementById("ctl00_BodyContent_txGeneral").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txDelivery") != undefined &&
                    document.getElementById("ctl00_BodyContent_txDelivery") != null) {
                    var entryDelivery = document.getElementById("ctl00_BodyContent_txDelivery");
                    temp = entryDelivery.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txDelivery").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txLogistics") != undefined &&
                    document.getElementById("ctl00_BodyContent_txLogistics") != null) {
                    var entryLogistics = document.getElementById("ctl00_BodyContent_txLogistics");
                    temp = entryLogistics.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txLogistics").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txUtilities") != undefined &&
                    document.getElementById("ctl00_BodyContent_txUtilities") != null) {
                    var entryUtilities = document.getElementById("ctl00_BodyContent_txUtilities");
                    temp = entryUtilities.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txUtilities").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txAccounting") != undefined &&
                    document.getElementById("ctl00_BodyContent_txAccounting") != null) {
                    var entryAccounting = document.getElementById("ctl00_BodyContent_txAccounting");
                    temp = entryAccounting.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txAccounting").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txProducts") != undefined &&
                    document.getElementById("ctl00_BodyContent_txProducts") != null) {
                    var entryProducts = document.getElementById("ctl00_BodyContent_txProducts");
                    temp = entryProducts.value;
                    for (i = 0; i < 20; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txProducts").value = temp;
                }

                if (document.getElementById("ctl00_BodyContent_txEmail") != undefined &&
                    document.getElementById("ctl00_BodyContent_txEmail") != null) {
                    var entryEmail = document.getElementById("ctl00_BodyContent_txEmail");
                    temp = entryEmail.value;
                    for (i = 0; i < 3; i++) {
                        temp = temp.replace('<', '');
                        temp = temp.replace('>', '');
                    }
                    document.getElementById("ctl00_BodyContent_txEmail").value = temp;
                }
                // alert("End");
            }
            </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Comments
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-48">

        <%--  --%>

    <div class="w3-row">
        <div class="w3-container">
            <asp:Label ID="lbInstructions" runat="server" Font-Bold="true"
                 Text="If you would like a response to your comments, please provide an email address or phone number." />
            <div class="spacer5"></div>
            <asp:Label ID="lbMsg" runat="server" SkinID="labelError" Visible="true" />
        </div>
    </div>

    <div class="w3-row w3-padding-16">
        <div class="w3-third w3-container">
            
            <asp:CheckBox ID="chBxDelivery" runat="server" 
                Text="Service Delivery" 
                AutoPostBack="true"   SkinID="checkBox1" 
                oncheckedchanged="chBxGroup_CheckedChanged" />
            <div class="spacer0"></div>
            <asp:CheckBox ID="chBxLogistics" runat="server" 
                Text="Service Logistics" 
                AutoPostBack="true"   SkinID="checkBox1" 
                oncheckedchanged="chBxGroup_CheckedChanged" />
            <div class="spacer0"></div>
            <asp:CheckBox ID="chBxUtilities" runat="server" 
                Text="ServiceCOMMAND® Utilities" 
                AutoPostBack="true"  SkinID="checkBox1" 
                oncheckedchanged="chBxGroup_CheckedChanged" />
            <div class="spacer0"></div>
            <asp:CheckBox ID="chBxAccounting" runat="server" 
                Text="Accounting/Invoicing" 
                AutoPostBack="true"  SkinID="checkBox1" 
                oncheckedchanged="chBxGroup_CheckedChanged" />
            <div class="spacer0"></div>
            <asp:CheckBox ID="chBxProducts" runat="server" 
                Text="Service Offerings" 
                AutoPostBack="true" SkinID="checkBox1" 
                oncheckedchanged="chBxGroup_CheckedChanged" />
            <div class="spacer10"></div>
        </div>
        <div class="w3-twothird w3-container">

                        <table class="tableWithoutLines">
                            <tr>
                                <td>Contact Name</td>
                                <td><asp:TextBox ID="txContact" runat="server" Width="250" MaxLength="100" /></td>
                            </tr>
                            <tr>
                                <td>Phone</td>
                                <td>
                                    <asp:TextBox ID="txPhone" runat="server" Width="120" MaxLength="20" />
                                    &nbsp; Ext: <asp:TextBox ID="txExtension" runat="server" Width="50" MaxLength="8" />
                                </td>
                            </tr>
                            <tr>
                                <td>Email</td>
                                <td>
                                    <asp:TextBox ID="txEmail" runat="server" Width="250" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btSubmit" runat="server" 
                                        Text="Submit Comments" 
                                        SkinID="buttonPrimary"
                                        onClientClick="scrub()"
                                        onclick="btSubmit_Click" />
                                </td>
                            </tr>
                        </table>

        </div>
    </div>

    <div class="w3-row w3-padding-16">
        <div class="w3-third w3-container">

            <asp:Panel ID="pnGeneral" runat="server">
                <p><asp:Label ID="lbGeneral" runat="server" Text="General Comments" /></p>
                <asp:TextBox ID="txGeneral" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" /> 
            </asp:Panel>
            <asp:Panel ID="pnDelivery" runat="server" Visible="false" CssClass="panelFormat">
                <p><asp:Label ID="lbDelivery" runat="server" Text="Service Delivery" /></p>
                <asp:TextBox ID="txDelivery" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" /> 
            </asp:Panel>
            <asp:Panel ID="pnLogistics" runat="server" Visible="false" CssClass="panelFormat">
                <p><asp:Label ID="lbLogistics" runat="server" Text="Service Logistics" /></p>
                <asp:TextBox ID="txLogistics" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" />                            
            </asp:Panel>
            <asp:Panel ID="pnUtilities" runat="server" Visible="false" CssClass="panelFormat">
                <p><asp:Label ID="lbUtilities" runat="server" Text="ServiceCOMMAND® Utilities" /></p>
                <asp:TextBox ID="txUtilities" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" /> 
            </asp:Panel>
            <asp:Panel ID="pnAccounting" runat="server" Visible="false" CssClass="panelFormat">
                <p><asp:Label ID="lbAccounting" runat="server" Text="Accounting / Invoicing" /></p>
                <asp:TextBox ID="txAccounting" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" /> 
            </asp:Panel>
            <asp:Panel ID="pnProducts" runat="server" Visible="false" CssClass="panelFormat">
                <p><asp:Label ID="lbProducts" runat="server" Text="Service Offerings" /></p>
                <asp:TextBox ID="txProducts" runat="server" TextMode="MultiLine" Height="250px" Width="330px" MaxLength="1000" /> 
            </asp:Panel>

        </div>
        <div class="w3-twothird w3-container">
        </div>
    </div>

        <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
            <!-- -->
            <!-- -->
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->

        <%--  --%>
    </div>
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    
</div>
</asp:Content>
