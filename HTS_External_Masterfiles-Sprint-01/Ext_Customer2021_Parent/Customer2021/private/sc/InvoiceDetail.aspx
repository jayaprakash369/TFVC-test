<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="InvoiceDetail.aspx.cs" 
    Inherits="private_sc_InvoiceDetail" %>
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
    Invoice Detail
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div class="bodyPadding">

    <div class="w3-row w3-padding-32" style="margin-left: 16px; position: relative; top:-10px;">
        <asp:Label ID="lbMsg" runat="server" SkinID="labelError" Text="Error label" />
        <asp:Panel ID="pnBuilding1" runat="server" visible="false">
        <asp:Label ID="lbInvoiceNoBl1" Text="" runat="server" />  <br />   
        <asp:Label ID="lbInvoiceTotal" Text="" runat="server" />
        <!-- START: SMALL SCREEN TABLE (Agreement INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
        
            <asp:Repeater ID="rp_InvoiceDetailBld1Small" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Values</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbPartA" runat="server" Text='<% #Eval("sPart") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionA" runat="server" Text='<% #Eval("sDescription") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbSerialA" runat="server" Text='<% #Eval("sSerial") %>' /></td>
                    </tr>
                   <tr class="trColorReg">
                        <td>Amount</td>
                        <td><asp:Label ID="lbAmountA" runat="server" Text='<% #Eval("sInvoiceAmount") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tax Amount</td>
                        <td><asp:Label ID="lbTaxA" runat="server" Text='<% #Eval("sTaxAmount") %>' /></td>
                    </tr>    
                    <tr class="trColorReg">
                        <td>Total Amount</td>
                        <td><asp:Label ID="lbInvTotalA" runat="server" Text='<% #Eval("sTotalAmount") %>' /></td>
                    </tr>       
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbPartB" runat="server" Text='<% #Eval("sPart") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbDescriptionB" runat="server" Text='<% #Eval("sDescription") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbSerialB" runat="server" Text='<% #Eval("sSerial") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Invoice Amount</td>
                        <td><asp:Label ID="lbAmountB" runat="server" Text='<% #Eval("sInvoiceAmount") %>' /></td>
                    </tr>    
                      <tr class="trColorAlt">
                        <td>Tax Amount</td>
                        <td><asp:Label ID="lbTaxB" runat="server" Text='<% #Eval("sTaxAmount") %>' /></td>
                    </tr>    
                    <tr class="trColorAlt">
                        <td>Total Amount</td>
                        <td><asp:Label ID="lbInvTotalB" runat="server" Text='<% #Eval("sTotalAmount") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (Building 1 INVOICE) ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE (Building 1 INVOICE) ======================================================================================= -->
        <div class="w3-hide-small">
            <br />        
            <asp:GridView ID="gv_InvoiceDetailBld1Large" runat="server"
            AutoGenerateColumns="False" 
            CssClass="tableWithLines"
            PageSize="900"
            AllowSorting="true" 
            onsorting="gvSorting_InvDtlBld1"
            EmptyDataText="No matching records were found">
            <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:BoundField HeaderText="Part" DataField="sPart" ItemStyle-Width="150" SortExpression="sPart" ItemStyle-HorizontalAlign="Left" />  
                <asp:BoundField HeaderText="Description" DataField="sDescription" ItemStyle-Width="350" SortExpression="sDescription" ItemStyle-HorizontalAlign="Left" />  
                <asp:BoundField HeaderText="Serial" DataField="sSerial" ItemStyle-Width="150" SortExpression="sSerial" ItemStyle-HorizontalAlign= "Left"/>  
                <asp:BoundField HeaderText="Invoice Amount" DataField="sInvoiceAmount" ItemStyle-Width="150" ItemStyle-HorizontalAlign= "Right" />   
                <asp:BoundField HeaderText="Tax Amount" DataField="sTaxAmount" ItemStyle-Width="150" ItemStyle-HorizontalAlign= "Right" />  
                <asp:BoundField HeaderText="Total Amount" DataField="sTotalAmount" ItemStyle-Width="150" ItemStyle-HorizontalAlign= "Right" />  
            </Columns>
            </asp:GridView>
        </div>
    </asp:Panel> <!-- end of Building 1 Panel -->
     <!-- END: LARGE SCREEN TABLE (Building 1 INVOICES) ===================================================================================== -->

    <!-- START: LARGE SCREEN TABLE (Building 2 INVOICE) ======================================================================================= -->
    <div class="w3-container w3-padding-32">
     <asp:Panel ID="pnBuilding2" runat="server" visible="false">
          <asp:Label ID="lbInvoiceNoBl2" Text="" runat="server" />  <br />   
          <asp:Label ID="lbInvoiceTotalBl2" Text="" runat="server" />
        <!-- START: SMALL SCREEN TABLE (Building 2 INVOICE) ======================================================================================= -->
        <div class="w3-hide-medium w3-hide-large">
            <!-- -->
            <asp:Repeater ID="rp_InvoiceDetailBld2Small" runat="server">
                <HeaderTemplate>
                    <table style="width:100%;" class="tableWithLines">
                        <tr>
                            <th>Field</th>
                            <th>Values</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trColorReg">
                        <td>Part</td>
                        <td><asp:Label ID="lbProductA" runat="server" Text='<% #Eval("dspProduct") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Description</td>
                        <td><asp:Label ID="lbProdDescA" runat="server" Text='<% #Eval("dspDescription") %>' /></td>
                    </tr>
                     <tr class="trColorReg">
                        <td>Serial</td>
                        <td><asp:Label ID="lbQuantityA" runat="server" Text='<% #Eval("dspQuantity") %>' /></td>
                    </tr>
                   <tr class="trColorReg">
                        <td>Amount</td>
                        <td><asp:Label ID="lbUnitPriceA" runat="server" Text='<% #Eval("dspUnitPrice") %>' /></td>
                    </tr>
                    <tr class="trColorReg">
                        <td>Tax Amount</td>
                        <td><asp:Label ID="lbExtPriceA" runat="server" Text='<% #Eval("dspExtPrice") %>' /></td>
                    </tr>                 
                    <tr class="repeaterDivider">
                        <td colspan="2" style="line-height: 1px;"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trColorAlt">
                        <td>Part</td>
                        <td><asp:Label ID="lbProductB" runat="server" Text='<% #Eval("dspProduct") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Description</td>
                        <td><asp:Label ID="lbProdDescB" runat="server" Text='<% #Eval("dspDescription") %>' /></td>
                    </tr>
                     <tr class="trColorAlt">
                        <td>Serial</td>
                        <td><asp:Label ID="lbQuantityB" runat="server" Text='<% #Eval("dspQuantity") %>' /></td>
                    </tr>
                    <tr class="trColorAlt">
                        <td>Invoice Amount</td>
                        <td><asp:Label ID="lbUnitPriceB" runat="server" Text='<% #Eval("dspUnitPrice") %>' /></td>
                    </tr>    
                      <tr class="trColorAlt">
                        <td>Tax Amount</td>
                        <td><asp:Label ID="lbExtPriceB" runat="server" Text='<% #Eval("dspExtPrice") %>' /></td>
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
        <!-- END: SMALL SCREEN TABLE (Building 2 small INVOICE) ======================================================================================= -->
        <div class="w3-hide-small">
        <br />   
        <asp:GridView ID="gv_InvoiceDetailBld2Large" runat="server"
        AutoGenerateColumns="False" 
        CssClass="tableWithLines"
        PageSize="900"
        AllowSorting="true" 
        onsorting="gvSorting_InvDtlBld2"
        EmptyDataText="No Invoice Detail was found">
        <AlternatingRowStyle CssClass="trColorAlt" />
            <Columns>
                <asp:BoundField HeaderText="Product" DataField="dspProduct"  SortExpression="dspProduct" ItemStyle-HorizontalAlign="LEFT"  />
                <asp:BoundField HeaderText="Description" DataField="dspDescription" SortExpression="dspDescription" ItemStyle-HorizontalAlign="LEFT"  />
                <asp:BoundField HeaderText="Quantity" DataField="dspQuantity" SortExpression="dspQuantitySort" ItemStyle-HorizontalAlign="RIGHT"  />
                <asp:BoundField HeaderText="Unit Price" DataField="dspUnitPrice" ItemStyle-HorizontalAlign="RIGHT"  />
                <asp:BoundField HeaderText="Ext Price" DataField="dspExtPrice" ItemStyle-HorizontalAlign="RIGHT"  />           
            </Columns>
         </asp:GridView>
         </div>
         </asp:Panel> <!-- end of Building 2 Panel -->
        </div>

<!-- END: PANEL (DETAIL) ======================================================================================= -->
     
    </div>
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1" runat="server" />
    <asp:HiddenField ID="hfPrimaryCs1Type" runat="server" />
    <asp:HiddenField ID="hfPassedInvoice" runat="server" />
    <asp:HiddenField ID="hfPassedSource" runat="server" />    
    <%--  --%>
</div>
</asp:Content>

