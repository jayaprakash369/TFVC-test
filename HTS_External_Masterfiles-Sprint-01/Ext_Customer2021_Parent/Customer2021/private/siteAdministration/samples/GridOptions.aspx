<%@ Page Title="Page Grid Options" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="GridOptions.aspx.cs" 
    Inherits="private_siteAdministration_samples_GridOptions" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Page Grid Options
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
	<div class="bodyPadding">

	<div class="w3-row w3-padding-32">
    <div class=" w3-container">

	
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

		<h3 class="w3-text-steel-blue">W3 sections to show/hide elements depending on screen size</h3>
        <%--  --%>

        <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
            <div class="w3-hide-medium w3-hide-large">
				elements to display when screen is small (Wrapped inside W3 Container)
        </div>
        <!-- END: SMALL SCREEN TABLE ======================================================================================= -->

        <!-- START: LARGE SCREEN TABLE ======================================================================================= -->
        <div class="w3-hide-small">
			ELEMENTS TO DISPLAY WHEN SCREEN IS BIG (Wrapped inside W3 container)
        </div>
        <!-- END: LARGE SCREEN TABLE ======================================================================================= -->
		<div class="spacer30"></div>

        <%--  --%>
    </div>

	    <div class="container-fluid p-0"> <!-- serves as section wrapper for one panel -->
			<div class="card"> <!-- makes panel background white -->
				<div class="card-body"> <!-- creates padding around upcoming elements -->

					<div class="row"> <!-- wraps all internal elements in a row -->
						<div class="col-md-12 text-left"> <!-- sets span to take 2 to 12 (12 total) sections of this row, 2nd class centers text -->
							<div class="border bg-light card"> <!--  card bg-light py-2 py-md-3 border / makes card inside specified section / sets background color (grey) and border (but still aligned on top) -->
								<div class="card-body"> <!--  sets minimum height to wrap internal elements in this row -->
									Changing Template
								</div>
							</div>
						</div>
					</div>
					<!-- /.row -->
				</div>
			</div>
		</div>


	    <div class="container-fluid p-0"> <!-- serves as section wrapper for one panel -->
			<div class="card"> <!-- makes panel background white -->
				<div class="card-body"> <!-- creates padding around upcoming elements -->

					<div class="row"> <!-- wraps all internal elements in a row -->
						<div class="col-md-12 text-left"> <!-- sets span to take 2 to 12 (12 total) sections of this row, 2nd class centers text -->
							<div class="card bg-light py-2 py-md-1 border"> <!--  card bg-light py-2 py-md-3 border / makes card inside specified section / sets background color (grey) and border (but still aligned on top) -->
								<div class="card-body"> <!--  sets minimum height to wrap internal elements in this row -->
									Test Template
								</div> <!-- -->
							</div>
						</div>
					</div>
					<!-- /.row -->
				</div>
			</div>
		</div>


		<h3 class="w3-text-steel-blue">W3 show/hide sections embeddded within the business template elements</h3>

	    <div class="container-fluid p-0"><!-- serves as section wrapper for one panel -->
			<div class="card"><!-- makes panel background white -->
				<div class="card-body"><!-- creates padding around upcoming elements -->

					<div class="row"><!-- wraps all internal elements in a row -->
						<div class="col-md-12 text-right"><!-- sets span to take 2 to 12 (12 total) sections of this row, 2nd class centers text -->
							<div class="card bg-light py-2 py-md-3 border"><!-- makes card inside specified section / sets background color (grey) and border (but still aligned on top) -->
								<div class="card-body"><!-- sets minimum height to wrap internal elements in this row -->
									

								    <!-- START: SMALL SCREEN TABLE ======================================================================================= -->
									<div class="w3-hide-medium w3-hide-large">
										elements to display when screen is small (Wrapped inside Business Container)
									</div>
									<!-- END: SMALL SCREEN TABLE ======================================================================================= -->

									<!-- START: LARGE SCREEN TABLE ======================================================================================= -->
									<div class="w3-hide-small">
										ELEMENTS TO DISPLAY WHEN SCREEN IS BIG (Wrapped inside Business Container)
									</div>
									<!-- END: LARGE SCREEN TABLE ======================================================================================= -->

								</div>
							</div>
						</div>
					</div>
					<!-- /.row -->

				</div>
			</div>
		</div>


		<h3 class="w3-text-steel-blue">Display of just one row from business template (easy to copy and paste)</h3>

	    <div class="container-fluid p-0"> <!-- serves as section wrapper for one panel -->
			<div class="card"> <!-- makes panel background white -->
				<div class="card-body"> <!-- creates padding around upcoming elements -->

					<div class="row"> <!-- wraps all internal elements in a row -->
						<div class="col-md-12 text-right"> <!-- sets span to take 2 to 12 (12 total) sections of this row, 2nd class centers text -->
							<div class="card bg-light py-2 py-md-1 border"> <!--  card bg-light py-2 py-md-3 border / makes card inside specified section / sets background color (grey) and border (but still aligned on top) -->
								<div class="card-body"> <!--  sets minimum height to wrap internal elements in this row -->
									.col-md-12
								</div> <!-- -->
							</div>
						</div>
					</div>
					<!-- /.row -->
				</div>
			</div>
		</div>

	<h3 class="w3-text-steel-blue">Full display of the business template elements</h3>

    <div class="container-fluid p-0">


					<div class="card">
						<div class="card-body">
							<div class="row">
								<div class="col-md-12 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-12
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-6 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-6
										</div>
									</div>
								</div>
								<div class="col-md-6 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-6
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-4 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-4
										</div>
									</div>
								</div>
								<div class="col-md-4 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-4
										</div>
									</div>
								</div>
								<div class="col-md-4 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-4
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
								<div class="col-md-2 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-2
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row d-none d-xxl-flex">
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
								<div class="col-md-1 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-1
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-8 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-8
										</div>
									</div>
								</div>
								<div class="col-md-4 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-4
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->

							<div class="row">
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
								<div class="col-md-6 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-6
										</div>
									</div>
								</div>
								<div class="col-md-3 text-center">
									<div class="card bg-light py-2 py-md-3 border">
										<div class="card-body">
											.col-md-3
										</div>
									</div>
								</div>
							</div>
							<!-- /.row -->
						</div>
					</div>

        <%--  --%>

		</div>
	</div>
</div>
</asp:Content>
