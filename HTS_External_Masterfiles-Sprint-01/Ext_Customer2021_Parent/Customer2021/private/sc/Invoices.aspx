<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="Invoices.aspx.cs" 
    Inherits="private_sc_Invoices" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" Runat="Server">
    <style type="text/css">
        .SearchPanelElements {
            display: inline-block;
            float: left;
            padding-right: 20px;
            padding-bottom: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyTitle" Runat="Server">
    Invoices
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

<div class="w3-container w3-padding-32">

    <asp:Label ID="lbMsg" runat="server" SkinID="labelError" />
    <!-- START: PANEL (INVOICE) ======================================================================================= -->
    <asp:Panel ID="pnInvoice" runat="server">
        <!-- START: SEARCH PANEL (INVOICE) ====================================================================================== -->
        <asp:Panel ID="pnInvoiceSearch" runat="server" >
            
            <table class="tableBorderBackgroundLight" style="margin-bottom: 10px;">
                <tr style="vertical-align: bottom;">
                    <td style="padding: 4px; padding-left: 8px; padding-right: 8px;">
                        <div class="SearchPanelElements">
                            Invoice<br /> <asp:TextBox ID="txSearchInvoice" runat="server" Width="100" />
                        </div>
                        <div class="SearchPanelElements">
                        Date Range<br />
                        <asp:DropDownList ID="ddSearchDateRange" runat="server" />
                        </div>
                        <div class="SearchPanelElements">
                        <div style="display:block; float:left; padding-right: 15px;">
                        <br />
                        <asp:Button ID="btSearchLocationSubmit" runat="server" Text="Search" OnClick="btSearchInvoiceSubmit_Click"  SkinID="buttonPrimary" />
                        </div>
                        <div style="display:block; float:left; padding-right: 15px;">
                        <br />
                        <asp:Button ID="BtSearchLocationClear" runat="server" Text="Clear" OnClick="btSearchInvoiceClear_Click" />
                        </div>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
  <asp:Panel ID="pnAgreement" runat="server" visible="false">
        <!-- START: SMALL SCREEN TABLE (Agreement INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <h3 class="titlePrimary"><asp:Literal ID="AgreementInvoice" Text="Agreement Invoices" runat="server" /></h3>
            <asp:Repeater ID="rp_AgreementInvoiceSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Values</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="lbDateA" runat="server" Text='<% #Eval("agrInvoiceDateDisplay") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Type</td>
                        <td><asp:Label ID="lbTypeA" runat="server" Text='<% #Eval("agrTypeDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Invoice</td>
                        <td>
                            <asp:LinkButton ID="lkInvAgreementA" runat="server" 
                                OnClick="lkInvoice_Click" 
                                Text='<%# Eval("Invoice") %>'
                                CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source")  %>' />
                        </td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountA" runat="server" Text='<% #Eval("agrAmountDisplay") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceA" runat="server" Text='<% #Eval("agrReferenceDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td style="max-width: 500px;"><asp:Label ID="lbDescriptionA" runat="server" Text='<% #Eval("agrDescriptionDisplay") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbDateB" runat="server" Text='<% #Eval("agrInvoiceDateDisplay") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Type</td>
                        <td><asp:Label ID="lbTypeB" runat="server" Text='<% #Eval("agrTypeDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Invoice</td>
                        <td>
                            <asp:LinkButton ID="lkInvAgreementB" runat="server" 
                                OnClick="lkInvoice_Click" 
                                Text='<%# Eval("Invoice") %>'
                                CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source") %>' />
                        </td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountB" runat="server" Text='<% #Eval("agrAmountDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceB" runat="server" Text='<% #Eval("agrReferenceDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td style="max-width: 500px;"><asp:Label ID="lbDescriptionB" runat="server" Text='<% #Eval("agrDescriptionDisplay") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (Agreement INVOICE) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (Agreement INVOICE) ======================================================================================= -->
        <div class="w3-hide-small">

            <h3 class="titlePrimary">Agreement Invoices</h3>
            <asp:GridView ID="gv_AgreementInvoiceLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_InvAgr"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
              <asp:BoundField HeaderText="Date" DataField="agrInvoiceDateDisplay" SortExpression="agrInvoiceDateSort" ItemStyle-HorizontalAlign="Left" />
              <asp:BoundField HeaderText="Type" DataField="agrTypeDisplay" SortExpression="agrTypeDisplay" ItemStyle-HorizontalAlign="Left" />
             <asp:TemplateField HeaderText="Invoice" SortExpression="agrInvoiceSort" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkInvAgreement" runat="server" 
                        OnClick="lkInvoice_Click" 
                        CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source")  %>'
                        Text='<%# Eval("Invoice")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:BoundField HeaderText="Amount" DataField="agrAmountDisplay" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Reference" DataField="agrReferenceDisplay" SortExpression="agrReferenceDisplay" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Description" DataField="agrDescriptionDisplay" SortExpression="agrDescriptionDisplay" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500" />
        </Columns>
    </asp:GridView>
    </div>
  </asp:Panel> <!-- end of Agreement Panel -->
        <!-- END: LARGE SCREEN TABLE (Agreement INVOICES) ===================================================================================== -->
    <asp:Panel ID="pnTicket" runat="server" visible="false">     
    
     <br />
        <!-- START: SMALL SCREEN TABLE (Ticket INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <h3 class="titlePrimary"><asp:Literal ID="TicketInvoices" Text="Ticket Invoices" runat="server" /></h3>
            <asp:Repeater ID="rp_TicketInvoiceSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">                       
                        <td>Date</td>
                        <td><asp:Label ID="lbDateA" runat="server" Text='<% #Eval("tckBillingDateDisplay") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Call Type</td>
                        <td><asp:Label ID="lbTypeA" runat="server" Text='<% #Eval("tckTypeDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Invoice</td>
                        <td>
                            <asp:LinkButton ID="lkInvTicketA" runat="server" 
                                OnClick="lkInvoice_Click" 
                                Text='<%# Eval("Invoice") %>'
                                CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source") %>'></asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountA" runat="server" Text='<% #Eval("tckAmountDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tax</td>
                        <td><asp:Label ID="lbTaxA" runat="server" Text='<% #Eval("tckTaxDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceA" runat="server" Text='<% #Eval("tckReferenceDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionA" runat="server" Text='<% #Eval("tckDescriptionDisplay") %>' /></td>
                    </tr>
                   
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                     <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbDateB" runat="server" Text='<% #Eval("tckBillingDateDisplay") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Type</td>
                        <td><asp:Label ID="lbTypeB" runat="server" Text='<% #Eval("tckTypeDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Invoice</td>
                        <td>
                            <asp:LinkButton ID="lkInvTicketB" runat="server" 
                                OnClick="lkInvoice_Click" 
                                Text='<%# Eval("Invoice") %>'
                                CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source") %>' />
                        </td>
                    </tr>
                   
                    <tr class="trColorAlt">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountB" runat="server" Text='<% #Eval("tckAmountDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tax</td>
                        <td><asp:Label ID="lbTaxB" runat="server" Text='<% #Eval("tckTaxDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceB" runat="server" Text='<% #Eval("tckReferenceDisplay") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionB" runat="server" Text='<% #Eval("tckDescriptionDisplay") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <!-- END: SMALL SCREEN TABLE (Ticket INVOICE) =============== <tr class="trColorReg">
                        <td>Call Type</td>
                        <td><asp:Label ID="lbCallTypeA" runat="server" Text='<% #Eval("tckCallTypeDisplay") %>' /></td>
                    </tr>======================================================================== -->

        <!-- START: LARGE SCREEN TABLE (Ticket INVOICE) ===================================================================================== -->
        <div class="w3-hide-small">

            <h3 class="titlePrimary"><asp:Literal ID="TicketInvoices0" runat="server" Text="Ticket Invoices" /></h3>

        <asp:GridView ID="gv_TicketInvoiceLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_InvTck"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
              <asp:BoundField HeaderText="Date" DataField="tckBillingDateDisplay" SortExpression="tckBillingDateSort" ItemStyle-HorizontalAlign="Left" />
             <asp:BoundField HeaderText="Type" DataField="tckTypeDisplay" SortExpression="tckTypeSort" ItemStyle-HorizontalAlign="Left" />
             <asp:TemplateField HeaderText="Invoice" SortExpression="InvoiceSort" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lkInvTicket" runat="server" 
                        OnClick="lkInvoice_Click" 
                        CommandArgument='<%# Eval("Invoice") + "|" + Eval("Source") %>'
                        Text='<%# Eval("Invoice")  %>'  />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Amount" DataField="tckAmountDisplay" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Tax" DataField="tckTaxDisplay" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Reference" DataField="tckReferenceDisplay" SortExpression="tckReferenceSort" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Billing Description" DataField="tckDescriptionDisplay" SortExpression="tckDescriptionSort" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500" />
        </Columns>
    </asp:GridView>
    </div>
   </asp:Panel> <!-- end of Ticket Panel -->
        <!-- END: LARGE SCREEN TABLE (Ticket INVOICES) ======================================================================================= -->
<asp:Panel ID="pnMiscellaneous" runat="server" visible="false">
        <br />
        <!-- START: SMALL SCREEN TABLE (Miscellaneous INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
             <asp:Literal ID="MiscellaneousInvoices" Text="Miscellaneous Invoices" runat="server" />
      
            <asp:Repeater ID="rp_MiscellaneousInvoicesSmall" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Values</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Date</td>
                        <td><asp:Label ID="lbBillingDateA" runat="server" Text='<% #Eval("INVDATE") %>' /></td>
                    </tr>                  
                   <tr class="trColorReg">
                        <td>Invoice</td>
                        <td><asp:Label ID="lbInvoiceA" runat="server" Text='<% #Eval("INVNUM") %>' /></td>
                     </tr>
                     <tr class="trColorReg">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountA" runat="server" Text='<% #Eval("INVOAM") %>' /></td>
                     </tr>
                    <tr class="trColorReg">
                        <td>Tax</td>
                        <td><asp:Label ID="lbTaxA" runat="server" Text='<% #Eval("INVTX1") %>' /></td>
                     </tr>
                     <tr class="trColorReg">
                        <td>Center</td>
                        <td><asp:Label ID="lbCenterA" runat="server" Text='<% #Eval("INVCTR") %>' /></td>
                     </tr>
                     <tr class="trColorReg">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceA" runat="server" Text='<% #Eval("INVJOB") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionA" runat="server" Text='<% #Eval("INVDSR") %>' /></td>
                    </tr>
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Date</td>
                        <td><asp:Label ID="lbBillingDateB" runat="server" Text='<% #Eval("INVDATE") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Invoice</td>
                        <td><asp:Label ID="lbInvoiceB" runat="server" Text='<% #Eval("INVNUM") %>' /></td>
                     </tr>
                     <tr class="trColorAlt">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountB" runat="server" Text='<% #Eval("INVOAM") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Tax</td>
                        <td><asp:Label ID="lbTaxB" runat="server" Text='<% #Eval("INVTX1") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Center</td>
                        <td><asp:Label ID="lbCenterB" runat="server" Text='<% #Eval("INVCTR") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Reference</td>
                        <td><asp:Label ID="lbReferenceB" runat="server" Text='<% #Eval("INVJOB") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionB" runat="server" Text='<% #Eval("INVDSR") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (Miscellaneous INVOICE) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (Miscellaneous INVOICE) ======================================================================================= -->
        <div class="w3-hide-small">
    <br />
        <asp:Literal ID="Literal1" Text="Miscellaneous Invoices" runat="server" />
        <asp:GridView ID="gv_MiscellaneousInvoiceLarge" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_InvMisc"
            EmptyDataText="No matching records were found">
        <AlternatingRowStyle CssClass="trColorAlt" />
        <Columns>
             <asp:BoundField HeaderText="Date" DataField="INVDATE" SortExpression="DateSort" ItemStyle-HorizontalAlign="Center" />          
             <asp:BoundField HeaderText="Invoice" DataField="INVNUM" SortExpression="invoiceSort" ItemStyle-HorizontalAlign="Center" />          
             <asp:BoundField HeaderText="Amount" DataField="INVOAM" ItemStyle-HorizontalAlign="Center" />
             <asp:BoundField HeaderText="Tax" DataField="INVTX1" ItemStyle-HorizontalAlign="Center" />
             <asp:BoundField HeaderText="Center" DataField="INVCTR" SortExpression="CenterSort" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Reference" DataField="INVJOB" SortExpression="INVJOB" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Description" DataField="INVDSR" SortExpression="INVDSR" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500" />
        </Columns>
    </asp:GridView>
    </div>
     </asp:Panel> <!-- end of Miscellaneous Panel -->
        <!-- END: LARGE SCREEN TABLE (Agreement INVOICES) ===================================================================================== -->
        
</asp:Panel><!-- END: PANEL (INVOICES) ======================================================================================= -->   

    </div>
        <div class="spacer30"></div>
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfOracleParentId" runat="server" />
    <asp:HiddenField ID="hfOracleChildId" runat="server" />
    <%--  --%>

</div>
</asp:Content>

