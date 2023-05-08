<%@ Page Title="Service Request: Service Issue" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Problem.aspx.cs" 
    Inherits="private_sc_req_Problem" %>
<%--
--%>      
<asp:Content ID="Content0" ContentPlaceHolderID="For_HtmlHead" Runat="Server">
</asp:Content>
       
<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">
Service Request
</asp:Content>
<%-- Body --%>
<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">

<%-- Hidden Fields --%>             
<asp:HiddenField ID="hfPri" runat="server" Value="" Visible="true" />
<asp:HiddenField ID="hfCs1" runat="server" Value="" Visible="true" />
<asp:HiddenField ID="hfCs2" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfPhone" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfExtension" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfContact" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfEmail" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCreator" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfReqType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfForcedQty" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfUnitList" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfReqCount" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodInfo" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="hfCommMethodPhoneExt" runat="server" Value="" Visible="false" />

<asp:HiddenField ID="hfReqKey" runat="server" Value="" Visible="true" />
<asp:HiddenField ID="hfReqSource" runat="server" Value="" Visible="true" />


<asp:Panel ID="pnCs1Header" runat="server" />
 
 <%-- PROBLEM --%>
 <asp:Panel ID="pnProblem" runat="server" DefaultButton="btProblem">

     <%-- 
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
         --%>

        <ContentTemplate>

       <asp:CustomValidator id="vCustom_ContactSer" runat="server" 
            Display="Dynamic"
            EnableClientScript="False"
            ValidationGroup="Problem"  
            />
       <asp:ValidationSummary ID="vSummary_Problem" runat="server" ValidationGroup="Problem" />
 
        <%-- Serial Search Contact --%> 
        <asp:Panel ID="pnContactSer" runat="server" Visible="false">
        <table class="tableWithoutLines" style="width: 100%;">
            <tr>
                <th align="left">
                    Contact Name
                    <br /><asp:TextBox ID="txContactSer" runat="server" MaxLength="25" Width="180" />
                    <asp:RequiredFieldValidator ID="vReqContactSer" runat="server" 
                        ControlToValidate="txContactSer" 
                        ErrorMessage="A contact name is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Problem" />
                </th>
                <th align="left">
                    Contact Phone 
                    <br />(&nbsp;<asp:TextBox ID="txPhone1Ser" runat="server"  MaxLength="3" Width="40" />
                    <asp:RequiredFieldValidator ID="vReqPhone1Ser" runat="server" 
                        ControlToValidate="txPhone1Ser" 
                        ErrorMessage="The area code is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Problem" />
                    <asp:CompareValidator id="vComPhone1Ser" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone1Ser"
                        ErrorMessage="The area code must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Problem" />
                    )
                    &nbsp;&nbsp; <asp:TextBox ID="txPhone2Ser" runat="server"  MaxLength="3"  Width="40" />
                    <asp:RequiredFieldValidator ID="vReqPhone2Ser" runat="server" 
                        ControlToValidate="txPhone2Ser" 
                        ErrorMessage="The phone prefix is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Problem" />
                    <asp:CompareValidator id="vComPhone2Ser" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone2Ser"
                        ErrorMessage="The phone prefix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Problem" />
                    &nbsp;-&nbsp; <asp:TextBox ID="txPhone3Ser" runat="server"  MaxLength="4"  Width="50"/>
                    <asp:RequiredFieldValidator ID="vReqPhone3Ser" runat="server" 
                        ControlToValidate="txPhone3Ser" 
                        ErrorMessage="The phone suffix is required." 
                        Text="*"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="Problem" />
                    <asp:CompareValidator id="vComPhone3Ser" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txPhone3Ser"
                        ErrorMessage="The phone suffix must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Problem" />
                    &nbsp;&nbsp; Ext:&nbsp; <asp:TextBox ID="txExtensionSer" runat="server"  MaxLength="8" Width="50" />
                    <asp:CompareValidator id="vComExtensionSer" runat="server" 
                        Operator="DataTypeCheck"
                        Type="Integer"
                        ControlToValidate="txExtensionser"
                        ErrorMessage="The extension must be an integer" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Problem" />
                    </th>
                    <th align="left">
                    Email acknowledgement?
                    <br /><asp:TextBox ID="txEmailSer" runat="server" MaxLength="50" Width="180" />
                    <asp:RegularExpressionValidator id="vRegEmailSer" runat="server"
                        ControlToValidate="txEmailSer"
                        ValidationExpression="^\S+@\S+\.\S+$"
                        ErrorMessage="The email address is not in a valid format" 
                        Text="*"
                        SetFocusOnError="true"
                        Display="Dynamic"
                        ValidationGroup="Problem" />
                    </th>
                    <th align="left">
                        Save ticket creator?
                    <br /><asp:TextBox ID="txCreatorSer" runat="server" MaxLength="30" Width="180" />
                    </tr>
                    </table>
                    <div style="height: 20px">&nbsp;</div></asp:Panel>
        <asp:Label ID="lbPriorityInfo" runat="server" SkinID="labelError" Visible="false" />
       <asp:Label ID="lbInterfaceInfo" runat="server" SkinID="labelTitleColor1_Small" Visible="false" />

                    <%-- Problem Repeater (Contract Request) --%>
        <asp:Repeater ID="rpProblem" runat="server" Visible="false">
            <HeaderTemplate>
                <table class="tableWithoutLines" style="width: 100%;">
                    <tr>
                        <th align="left">Model</th>
                        <th align="left">Serial</th>
                        <th align="left">Problem Description</th>
                        <th align="left">Ticket XRef</th>
                        <th align="left">Service Type</th>
                        <th align="left">
                            <asp:Label ID="lbInterfaceHeader" runat="server" Text="Interface" Visible="false"></asp:Label>
                        </th>
                        <th align="left">
                            <asp:Label ID="lbViaHeader" runat="server" Text="ShipVia" Visible="false"></asp:Label>
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="left"><asp:Label ID="lbPart" runat="server" Text='<%# Eval("Part") %>' /></td>
                    <td align="left"><asp:Label ID="lbSerial" runat="server" Text='<%# Eval("Serial") %>' /></td>
                    <td align="left">
                        <asp:TextBox ID="txProblem" runat="server" Width="250" MaxLength="50" Text='<%# Eval("Problem") %>' />
                         <asp:CustomValidator id="vCustom_Problem" runat="server" 
                            ControlToValidate="txProblem"
                            Display="None" 
                            EnableClientScript="False"
                            ValidationGroup="Problem" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txCrossRef" runat="server" Width="75" MaxLength="24" />
                         <asp:CustomValidator id="vCustom_CrossRef" runat="server" 
                            ControlToValidate="txCrossRef"
                            Display="None" 
                            EnableClientScript="False"
                            ValidationGroup="Problem" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lbAgrDesc" runat="server" Text='<%# Eval("AgrDesc") %>' />
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddInterface" runat="server" CssClass="dropDownList1" Visible="false">
                            <asp:ListItem Text="" Value="" />
                            <asp:ListItem Text="Network" Value="NETWORK" />
                            <asp:ListItem Text="USB" Value="USB" />
                            <asp:ListItem Text="Other" Value="OTHER" />
                        </asp:DropDownList>
                         <asp:CustomValidator id="vCustom_Face" runat="server" 
                            ControlToValidate="ddInterface"
                            Display="None" 
                            EnableClientScript="False"
                            ValidationGroup="Problem" />
                        </td>
                        <td align="left">
                        <asp:DropDownList ID="ddVia" runat="server" CssClass="dropDownList1" Visible="false">
                            <asp:ListItem Text="" Value="" />
                            <asp:ListItem Text="UPS Ground" Value="1" />
                            <asp:ListItem Text="UPS 2nd Day" Value="3" />
                            <asp:ListItem Text="Next Day Saver 5pm" Value="5" />
                            <asp:ListItem Text="Next Day Air 10am" Value="4" />
                        </asp:DropDownList>
                        <asp:CustomValidator id="vCustom_Via" runat="server" 
                            ControlToValidate="ddVia"
                            Display="None" 
                            EnableClientScript="False"
                            ValidationGroup="Problem" />
                        </td>
                     </tr>
                 <asp:HiddenField ID="hfAgreement" runat="server" Value='<%# Eval("Agreement") %>' />
                <asp:HiddenField ID="hfAgrCode" runat="server" Value='<%# Eval("AgrCode") %>' />
                <asp:HiddenField ID="hfUnit" runat="server" Value='<%# Eval("Unit") %>' />
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>


<%-- Problem Repeater (Forced Request) --%> 
        <asp:ValidationSummary ID="vSummary_ProblemForced" runat="server" ValidationGroup="ProblemForced" />
        <asp:Repeater ID="rpProblemForced" runat="server" Visible="false">
            <HeaderTemplate>
                <table class="tableWithoutLines">
                    <tr>
                        <th align="left">&nbsp;</th><th align="left">Equipment Model or Desc</th><th align="left">Serial Number</th><th align="left">Problem Description</th><th align="left">Equip XRef</th><th align="left">Service Type</th></tr></HeaderTemplate><ItemTemplate>
                <asp:HiddenField ID="hfForcedCount" runat="server" Value='<%# Eval("Count") %>' />
                <tr>
                    <td align="left"><asp:Label ID="lbCount" runat="server" Text='<%# Eval("Count") %>' /></td>
                    <td align="left">
                        <asp:TextBox ID="txPartForced" runat="server" Width="200" MaxLength="35" />
                        <asp:RequiredFieldValidator ID="vRequired_PartForced" runat="server" 
                            ControlToValidate="txPartForced" 
                            ErrorMessage="A model name or description is required" 
                            Text="*"
                            ValidationGroup="ProblemForced">
                        </asp:RequiredFieldValidator></td><td align="left">
                        <asp:TextBox ID="txSerialForced" runat="server" Width="160" MaxLength="25" />
                        <asp:RequiredFieldValidator ID="vRequired_SerialForced" runat="server" 
                            ControlToValidate="txSerialForced" 
                            ErrorMessage="A serial number is required." 
                            Text="*"
                            ValidationGroup="ProblemForced">
                        </asp:RequiredFieldValidator></td><td align="left">
                        <asp:TextBox ID="txProblemForced" runat="server" Width="200" MaxLength="50" />
                        <asp:RequiredFieldValidator ID="vRequired_ProblemForced" runat="server" 
                            ControlToValidate="txProblemForced" 
                            ErrorMessage="A problem description is required." 
                            Text="*"
                            ValidationGroup="ProblemForced">
                        </asp:RequiredFieldValidator></td><td align="left">
                        <asp:TextBox ID="txCrossRefForced" runat="server" Width="60" MaxLength="24" />
                    </td>
                    <td align="left">
                        <asp:DropDownList runat="server" ID="ddServiceTypeForced" CssClass="dropDownList1" >
                            <asp:ListItem Text="ONSITE" Value="ONSITE" />
                            <asp:ListItem Text="DEPOT" Value="DEPOT" />
                            <asp:ListItem Text="EXPRESS" Value="EXPRESS" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
            <table style="width:100%">
                <tr>
                    <td style="width:50%">
                        <asp:Label ID="lbComment" runat="server" Text="Comments or Remarks?" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txComment" runat="server" 
                            TextMode="MultiLine" 
                            Width="400" 
                            Height="75" 
                            MaxLength="1000" 
                            ValidationGroup="Problem" />
                    </td>
                    <td style="width:50%; vertical-align: bottom;">
                        <%-- Ajax Double Click Handling 
                            OnClientClick="jHideButton(this);"                            
                            OnClientClick="jHideButton(this);"

                        <table class="tableBorder">
                            <tr>
                                <td style="color: #ad0034; font-size: 16px;">Programmer Design Experiment</td>
                            </tr>
                            <tr>
                                <td>Previously when clicking the submit button below, you have seen a "yellow progress bar". 
                                    <b>Currently, that yellow bar is gone</b> 
                                    leaving the submit button visible while your tickets are processing.  
                                    Like always, processing may take 5-25 seconds to complete.  
                                    Once the ticket has been created a page will be returned with the ticket number and the option to submit additional tickets, or to view open tickets.  
                                    Please wait for that page to appear.</td>
                            </tr>
                        </table>
                            --%> 
                        <div class="spacer15"></div>
                        <asp:Button ID="btProblem" runat="server" 
                            Text="Submit Automated Request" 
                            Font-Size="14"
                            onclick="btProblem_Click" 
                            ValidationGroup="Problem"
                            Visible="false"
                            />
                        <asp:Button ID="btProblemForced" runat="server" 
                            Text="Submit Manual Request" 
                            Font-Size="14"
                            onclick="btProblem_Click" 
                            ValidationGroup="ProblemForced" 
                            Visible="false" />
                        <asp:Button ID="btDoesNothing" runat="server" Visible="false" Enabled="false" />
                        <%-- 
                        <asp:Label ID="lbOnlyOnce" runat="server" SkinID="labelInstructions" Text="<br /><br />Please click the submit button ONLY ONCE <br />(or multiple duplicate requests will be placed.) <br /><b>The screen will not change or advance for 10 to 15 seconds</b> while the request is processing. (Thank you for your consideration.)" />
                             --%> 
                        <%-- 
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
        <ProgressTemplate>
            <div id="objHideButton" class="hideButton">
                <div style="text-align: center; margin-top: 5px;">
                        <asp:TextBox ID="txAjax" runat="server" 
                            Text="" 
                            Width="200" 
                            BackColor="#ffffcc" 
                            BorderColor="#ffffcc" 
                            ForeColor="#333333" 
                            ReadOnly="true" />
                </div>
                <table>
                    <tr>
                        <td id="tdBar" style="margin-left: 4px; margin-top: 15px; width: 0px; height: 15px; background-color: #3a7728; border: 1px solid #333333;">
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
                             --%> 
                    </td>
                </tr>
            </table>
   
        </ContentTemplate>

               <%-- 
   <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btProblem" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btProblemForced" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>     
 --%>

     



<%-- --%> 

</asp:Panel>

</asp:Content>


