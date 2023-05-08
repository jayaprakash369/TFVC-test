<%@ Page Title="Customer Admin Utilities" Language="C#" MasterPageFile="~/Scantron_Body_A_Bar.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" 
    Inherits="private_custAdmin_Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_B" Runat="Server">

    <table style="width: 100%;">
        <tr>
            <td>Customer Administration</td>
            <td style="display: block; float: right;">
                <asp:Table ID="tbCs1Change" runat="server" Visible="false" HorizontalAlign="Right">
                    <asp:TableRow VerticalAlign="Bottom">
                        <asp:TableCell>
                            <asp:Label ID="lbCs1Change" runat="server" Text="Viewing customer..." Font-Size="11" />
                            &nbsp;<asp:TextBox ID="txCs1Change" runat="server" Width="50" MaxLength="7" Font-Size="10" 
                                ValidationGroup="Cs1Change" />
                                <asp:CompareValidator id="vCompare_Cs1" runat="server" 
                                    Operator="DataTypeCheck"
                                    Type="Integer"
                                    ControlToValidate="txCs1Change"
                                    ErrorMessage="Customer entry must be a number" 
                                    Text="*"
                                    SetFocusOnError="true"
                                    ValidationGroup="Cs1Change">
                                </asp:CompareValidator>
                                &nbsp;
                                <asp:Button ID="btCs1Change" runat="server" 
                                    Text="Change Customer" 
                                    onclick="btCs1Change_Click"
                                    ValidationGroup="Cs1Change" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Body_A" Runat="Server">


    <table style="width: 100%;">
        <tr style="vertical-align: top;">
            <td style="width: 50%; padding: 10px;">

                <table style="width: 100%;">
                    <tr>
                        <td>
                             <asp:HyperLink ID="hlUserMaintenance" runat="server" 
                                Text="User Maintenance" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/custAdmin/UserMaintenance.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/People2.png" alt="" class="menuImage"  />
                            Update information for the accounts used by your employees to access this web site. 
                            This includes contact information, phone and email, locking and unlocking accounts, 
                            as well as the ability to change user passwords.  
                            Take care to remember password changes.  
                            These are stored in an encrypted manner and once entered they cannot be retrieved if forgotten.
                            (Though they may be reset again using this utility) 
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="hlRegistration" runat="server" 
                                Text="User Registration" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/Registration.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/BlackPen.png" alt="" class="menuImage"  />
                            Create accounts for your employees to use ServiceCOMMAND utilities.  Some customers prefer to have one global account for all employees.  Other prefer multiple accounts.  The choice is yours.  Once created, these accounts can be managed through the User Maintenance utility below.
                            <div class="spacer15"></div>
                        </td>
                    </tr>
                    <%--   
                    --%>    
                    <tr>
                        <td>
                             <asp:HyperLink ID="hlChangeYourPassword" runat="server" 
                                Text="Change Your Password" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/shared/ChangeYourPassword.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Locked.png" alt="" class="menuImage"  />
                            Update your own password.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="hlContactInfo" runat="server" 
                                Text="Contact Information" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/custAdmin/ContactInformation.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ContactBook.png" alt="" class="menuImage"  />
                            Update the contact information for any location of your parent site. 
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="hlPreferences" runat="server" 
                                Text="Customer Preferences" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/custAdmin/Preferences.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Paint.png" alt="" class="menuImage"  />
                            This feature allows you to customize key control features for the web site.  
                            These include opening registration for additional basic accounts,  
                            and other features that determine they way your information is displayed within our utilities.
                            <div class="spacer5"></div>
                        </td>
                    </tr>

                </table>

            </td>
            <td style="width: 50%; padding: 20px;">
                <div style="width: 100%; text-align: center;">
                            <asp:Label ID="lbManagedPrint" runat="server" 
                                Text="Managed Print Customer Tools" 
                                SkinID="labelTitleColor2_Medium" 
                                Height="25" />
                                <br />
                            <asp:Label ID="lbManagedPrint2" runat="server" 
                                Text="MPowerPrint Customers Only" 
                                SkinId="labelComment"
                                Height="35" />
                </div>
                <div style="padding: 15px; border: 1px solid #dddddd;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="HyperLink9" runat="server" 
                                Text="Page Count History" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/mp/PageCountByFleet.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Truck.png" alt="" class="menuImage"  />
                            View page counts by month for your entire printer fleet.
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="HyperLink10" runat="server" 
                                Text="Page Counts By Device" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/mp/PageCountEqp.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/Magnifier.png" alt="" class="menuImage"  />
                            View page counts by month for a specific device. 
                            <div class="spacer0"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="HyperLink11" runat="server" 
                                Text="Device Utilization" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/mp/DeviceUtilization.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ColorBars.png" alt="" class="menuImage"  />
                            View most and least utilized devices within your fleet.
                            <div class="spacer0"></div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:HyperLink ID="HyperLink12" runat="server" 
                                Text="Update Toner Contact" 
                                SkinID="hyperLinkHeader" 
                                NavigateUrl="~/private/mp/UpdateTonerContact.aspx" />
                            <div class="spacer5"></div>
                            <img src="/media/scantron/art/menu/green/ArrowUp.png" alt="" class="menuImage"  />
                            Update the contact to receive toner replenishment for a specific device.
                            <div class="spacer0"></div>
                        </td>
                    </tr>

                </table>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>