<%@ Page Title="Add Note To Ticket" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="AddNote.aspx.cs" 
    Inherits="public_sc_AddNote" %>
<%--  --%>             
<%-- BODY TITLE ========================================================================================================= --%>
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Add Note
</asp:Content>
<%-- BODY CONTENT ======================================================================================================== --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">


        <asp:Panel ID="pnNoteEntry" runat="server" style="clear: both;">
            <!-- 
        style="width: 100%; margin-left: auto; margin-right: auto; font-size: 14px; margin-bottom: 10px;"
                 style="text-align: right; padding-right: 20px; line-height: 20px; width: 80px;" 
                 style="line-height: 20px;"
                 style="text-align: right; padding-right: 20px; vertical-align: top; line-height: 20px;"
                 style="line-height: 20px;"
            -->
        <table class="tableWithoutLines tableBorder" style="margin-top: 10px; margin-bottom: 20px;">
            <tr>
                <td></td>
                <td><asp:Label ID="lbMsg" runat="server" SkinID="labelComment" Text="Urgent comments/notes should be routed through our contact center (1.800.228.3628)" /></td>
            </tr>
            <tr>
                <td>Subject</td>
                <td><asp:TextBox ID="txSubject" runat="server" Width="500" MaxLength="50" TabIndex="1" />
                    <asp:RequiredFieldValidator ID="vReq_txSubject" runat="server" 
                        ControlToValidate="txSubject" 
                        ErrorMessage="A subject is required." 
                        Text="*"
                        Display="Dynamic"
                        ValidationGroup="Note" />
                </td>
            </tr>
            <tr>
                <td>
                    Message
                    <div style="clear: both; height: 10px;"></div>
                    <asp:Button ID="btNoteEntry" runat="server"
                        TabIndex="3" 
                        Text="Submit" 
                        onclick="btNoteEntry_Click" 
                        ValidationGroup="Note" />
                </td>
                <td>
                    <asp:TextBox ID="txMessage" runat="server" TextMode="MultiLine" Height="130" Width="500" MaxLength="1000" TabIndex="2"  />
                    <asp:RequiredFieldValidator ID="vReq_txMessage" runat="server" 
                        ControlToValidate="txMessage" 
                        ErrorMessage="A message is required." 
                        Text="*"
                        Display="Dynamic"
                        ValidationGroup="Note" />
                    <asp:CustomValidator id="vCus_Note" runat="server" 
                        Display="None" 
                        EnableClientScript="False"
                        ValidationGroup="Note" />
                    <asp:ValidationSummary ID="vSum_Note" runat="server" 
                        ValidationGroup="Note" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                </td>
            </tr>
        </table>
    </asp:Panel>

<%-- PANEL DISPLAY ========================================================================================== --%>                
<asp:Panel ID="pnDisplay" runat="server">
</asp:Panel>
<!-- End pnDisplay ** DO NOT DELETE ** This is dynamically loaded with user selections -->
    <asp:HiddenField ID="hfCtr" runat="server" Visible="false" />
    <asp:HiddenField ID="hfTck" runat="server" Visible="false" />
</asp:Content>

