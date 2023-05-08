<%@ Page Title="Template White Panel With Gray Box" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="TemplateWhitePanelWithGrayBox.aspx.cs" 
    Inherits="private_siteAdministration_samples_TemplateWhitePanelWithGrayBox" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Template White Panel With Gray Box
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
	<div class="bodyPadding">

	 <div class="w3-row w3-padding-32">
        <div class=" w3-container">

	    
        <%--  --%>
		<%-- 
			If you wrap this in the W3-Container the box width will stay limited to the size of the inner contents (rather than spreading to the right of the page)
			<div class="w3-container">  --%>

	    <div class="container-fluid p-0"> <!-- serves as section wrapper for one panel -->
			<div class="card"> <!-- makes panel background white -->
				<div class="card-body"> <!-- creates padding around upcoming elements -->

					<div class="row"> <!-- wraps all internal elements in a row -->
						<div class="col-md-12 text-left"> <!-- sets span to take 2 to 12 (12 total) sections of this row, 2nd class centers text -->
							<!-- IF YOU ADD THE 'CARD' CLASS to the div below, the corners get rounded and the standard bottom margin is added below the inner gray box  
								if you remove them, the margin on bottom disappears, and the border corners are square
								-->
							<div class="border bg-light"> <!--  card bg-light py-2 py-md-3 border / makes card inside specified section / sets background color (grey) and border (but still aligned on top) -->
								<div class="card-body"> <!--  sets minimum height to wrap internal elements in this row -->
									Sample Template from Business Model
								</div>
							</div>
						</div>
					</div>
					<!-- /.row -->
				</div>
			</div>
		</div>


        <%--  --%>
		<%-- </div> --%>
</div>
</div>		 
</div>
</asp:Content>
