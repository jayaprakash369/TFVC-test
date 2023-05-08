<%@ Page Title="Invoice Original" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="InvoiceOrig.aspx.cs" 
    Inherits="private_siteAdministration_samples_InvoiceOrig" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Invoice Original
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
	<div class="bodyPadding">

	    
        <%--  --%>

    <div class="w3-row w3-padding-32">
        <div class="w3-container">



					<div class="row">
						<div class="col-12">
							<div class="card">
								<div class="card-body m-sm-3 m-md-5">
									<div class="mb-4">
										This is the receipt for a payment of <strong>$268.00</strong> (USD) you made to Secur-Serv.
									</div>

									<div class="row">
										<div class="col-md-6">
											<div class="text-muted">Payment No.</div>
											<strong>741037024</strong>
										</div>
										<div class="col-md-6 text-md-right">
											<div class="text-muted">Payment Date</div>
											<strong>June 12, 2021 - 03:45 pm</strong>
										</div>
									</div>

									<hr class="my-4" />

									<div class="row mb-4">
										<div class="col-md-6">
											<div class="text-muted">Client</div>
											<strong>
              Adam Anderson
            </strong>
											<p>
												4183 Forest Avenue <br> New York City <br> 10011 <br> USA <br>
												<a href="#">
                adam.anderson@gmail.com
              </a>
											</p>
										</div>
										<div class="col-md-6 text-md-right">
											<div class="text-muted">Payment To</div>
											<strong>
              Scantron Technology Solutions
            </strong>
											<p>
												2020 South 156th Circle <br /> Omaha <br /> NE <br /> 68130 <br /> USA <br />
												<a href="#">
                accounting@scantrontechnologysolutions.com
              </a>
											</p>
										</div>
									</div>

									<table class="table table-sm">
										<thead>
											<tr>
												<th>Description</th>
												<th>Quantity</th>
												<th class="text-right">Amount</th>
											</tr>
										</thead>
										<tbody>
											<tr>
												<td>Managed Service</td>
												<td>2</td>
												<td class="text-right">$150.00</td>
											</tr>
											<tr>
												<td>Managed Print</td>
												<td>3</td>
												<td class="text-right">$25.00</td>
											</tr>
											<tr>
												<td>Additional Services</td>
												<td>1</td>
												<td class="text-right">$100.00</td>
											</tr>
											<tr>
												<th>&nbsp;</th>
												<th>Subtotal </th>
												<th class="text-right">$275.00</th>
											</tr>
											<tr>
												<th>&nbsp;</th>
												<th>Shipping </th>
												<th class="text-right">$8.00</th>
											</tr>
											<tr>
												<th>&nbsp;</th>
												<th>Discount </th>
												<th class="text-right">5%</th>
											</tr>
											<tr>
												<th>&nbsp;</th>
												<th>Total </th>
												<th class="text-right">$268.85</th>
											</tr>
										</tbody>
									</table>

									<div class="text-center">
										<p class="text-sm">
											<strong>Extra note:</strong> Please send all items at the same time to the shipping address. Thanks in advance.
										</p>

										<a href="#" class="btn btn-primary">
            Print this receipt
          </a>
									</div>
								</div>
							</div>
						</div>
					</div>

        </div>
    </div>


<div class="spacer30"></div>

        <%--  --%>
				

</div>
</asp:Content>
