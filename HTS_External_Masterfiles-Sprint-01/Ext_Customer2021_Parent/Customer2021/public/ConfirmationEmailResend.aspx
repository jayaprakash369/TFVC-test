<%@ Page Title="Resend Confirmation Email" Language="C#" MasterPageFile="~/Responsive.Master" AutoEventWireup="true" CodeFile="ConfirmationEmailResend.aspx.cs" 
    Inherits="public_ConfirmationEmailResend" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="CodeForHtmlHeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyTitle" runat="server">
    Resend Confirmation Email
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="bodyPadding">

    <div class="w3-container w3-padding-32">
    <!-- <h1 class="w3-text-steel-blue">Sample Query</h1> -->

   	<div class="container-fluid p-0">

		<div class="row">
			<div class="col-12">
				<div class="card">
					<div class="card-body">
                        If you have a registered account that's not yet authorized and need a confirmation email resent enter the account email below.
                        <div class="spacer10"></div>
          				Email
                        <div class="spacer10"></div>
                        <asp:TextBox ID="txAccountEmail" runat="server" width="300" />
                        
                        <div class="spacer15"></div>
                        <asp:Button ID="btSubmission" runat="server" Text="Submit" OnClick="btSubmit_Click" />

					</div>
				</div>
			</div>
	</div>

                        <asp:Label ID="lbSummary" runat="server" SkinID="labelError" Font-Bold="true" />

                        <div class="spacer20"></div>
                        <asp:Label ID="lbDetail" runat="server" />


        <%--  --%>


        <%--  --%>
    </div>
    </div>

</div>
</asp:Content>
