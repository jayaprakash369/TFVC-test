<%@ Page Title="Agreements" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Agreements.aspx.cs" 
    Inherits="private_sc_Agreements" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <%--  --%>
        <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
        .LocationHeaderElements {
            display: inline-block;
            float: left;
            padding-right: 10px;
            padding-bottom: 5px;
        }
        .UpdateElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
            vertical-align: bottom;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Agreements
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">


    <div class="w3-container w3-padding-32">
        <!--
            <div class="w3-row w3-padding-32">
        <div class="w3-twothird w3-container">
            -->

        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
    <!-- START: PANEL (AGREEMENTS) ======================================================================================= -->
    <asp:Panel ID="pnAgreements" runat="server">
    

        <!-- START: SEARCH PANEL (AGREEMENT) ======================================================================================= -->
        <asp:Panel ID="pnAgreementSearch" runat="server" DefaultButton="btSearchAgreementSubmit">
            
            <table class="tableBorderBackgroundLight">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">

                <div class="SearchPanelElements">
                    Agreement<br />
                    <asp:TextBox ID="txSearchAgreement" runat="server" Width="100" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchAgreementSubmit" runat="server" Text="Search" OnClick="btSearchAgreementSubmit_Click" SkinId="buttonPrimary" />
                </div>
                <div class="SearchPanelElements">
                    <br />
                    <asp:Button ID="btSearchAgreementClear" runat="server" Text="Clear" OnClick="btSearchAgreementClear_Click" />
                </div>

            </td>
                </tr>
            </table>
            <div class="spacer10"></div>
        </asp:Panel>

        <!-- START: SMALL SCREEN TABLE (AGREEMENT) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->

            <asp:Repeater ID="rp_AgreementSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Agreement</td>
                        <td><asp:Label ID="lbAgreement" runat="server" Text='<% #Eval("AgreementId") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Qty</td>
                        <td>
                            <asp:LinkButton ID="lkQty" runat="server" OnClick="lkQty_Click" 
                                Text='<%# Eval("EquipmentCount") %>'
                                CommandArgument='<%# Eval("Source") + "|" + Eval("AgreementId") %>'  />
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Agreement Type</td>
                        <td><asp:Label ID="lbAgreementDescription" runat="server" Text='<% #Eval("AgreementType") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Start Date</td>
                        <td><asp:Label ID="lbDateStarting" runat="server" Text='<% #Eval("DateStarting") %>' /></td>
                    </tr>
                    <%-- 
                    <tr class="trColorReg">
                        <td>End Date</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("DateEnding") %>' /></td>
                    </tr>
                     --%>
                    <tr class="trColorReg">
                        <td>Location Id 1</td>
                        <td><asp:Label ID="lbOracleParentId" runat="server" Text='<% #Eval("LocationId1") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Location Id 2</td>
                        <td><asp:Label ID="lbOracleChildId" runat="server" Text='<% #Eval("LocationId2") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>For One Loc?</td>
                        <td><asp:Label ID="lbDedicated" runat="server" Text='<%# Eval("B2Address1") + " " + Eval("B2City") + " " + Eval("B2State")  %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Agreement</td>
                        <td><asp:Label ID="lbAgreement" runat="server" Text='<% #Eval("AgreementId") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Qty</td>
                        <td>
                            <asp:LinkButton ID="lkQty" runat="server" OnClick="lkQty_Click" 
                                Text='<%# Eval("EquipmentCount") %>'
                                CommandArgument='<%# Eval("Source") + "|" + Eval("AgreementId") %>'  />
                        </td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Agreement Type</td>
                        <td><asp:Label ID="Label2" runat="server" Text='<% #Eval("AgreementType") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Start Date</td>
                        <td><asp:Label ID="Label1" runat="server" Text='<% #Eval("DateStarting") %>' /></td>
                    </tr>
                    <%-- 
                    <tr class="trColorAlt">
                        <td>End Date</td>
                        <td><asp:Label ID="Label4" runat="server" Text='<% #Eval("DateEnding") %>' /></td>
                    </tr>
                        
                     --%>
                    <tr class="trColorAlt">
                        <td>Location Id 1</td>
                        <td><asp:Label ID="lbOracleParentId" runat="server" Text='<% #Eval("LocationId1") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Location Id 2</td>
                        <td><asp:Label ID="lbOracleChildId" runat="server" Text='<% #Eval("LocationId2") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>For One Loc?</td>
                        <td><asp:Label ID="lbDedicated" runat="server" Text='<%# Eval("B2Address1") + " " + Eval("B2City") + " " + Eval("B2State")  %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>


            <!-- -->
        </div>
        <!-- END: SMALL SCREEN TABLE (AGREEMENT) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (AGREEMENT) ======================================================================================= -->
        <div class="w3-hide-small">

        <asp:GridView ID="gv_AgreementLarge" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_Agr"
        EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
            <%--  
                <asp:BoundField HeaderText="Agreement id" DataField="AgreementId" SortExpression="AgreementId" ItemStyle-HorizontalAlign="Center" />
            --%>
            <asp:BoundField HeaderText="Agreement" DataField="AgreementId" SortExpression="AgreementId" ItemStyle-HorizontalAlign="Center" />
             <asp:TemplateField HeaderText="Qty" SortExpression="EquipmentCountSort" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lkQty" runat="server" 
                        OnClick="lkQty_Click" 
                        CommandArgument='<%# Eval("Source") + "|" + Eval("AgreementId") + "|" + Eval("CustomerNumber") + "|" + Eval("CustomerLocation") %>'
                        Text='<%# Eval("EquipmentCount")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="AgreementType" DataField="AgreementType" SortExpression="AgreementType" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Date Starting" DataField="DateStarting" SortExpression="DateStartingSort" ItemStyle-HorizontalAlign="Center" />
            <%--  
            <asp:BoundField HeaderText="Date Ending" DataField="DateEnding" SortExpression="DateEndingSort" ItemStyle-HorizontalAlign="Center" />
            --%>
            <asp:BoundField HeaderText="Loc Id 1" DataField="LocationId1" SortExpression="LocationId1" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Loc Id 2" DataField="LocationId2" SortExpression="LocationId2" ItemStyle-HorizontalAlign="Center" />
             <asp:TemplateField HeaderText="For Just One Location?" SortExpression="B2Address1" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("B2Address1") + " " + Eval("B2City") + " " + Eval("B2State")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

            <!-- -->
    </div>
        <!-- END: LARGE SCREEN TABLE (AGREEMENT) ======================================================================================= -->
        <div class="spacer30"></div>
</asp:Panel><!-- END: PANEL (AGREEMENTS) ======================================================================================= -->

        

    </div>

    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <%-- 
        --%>
    
    <%--  --%>
</div>
</asp:Content>

