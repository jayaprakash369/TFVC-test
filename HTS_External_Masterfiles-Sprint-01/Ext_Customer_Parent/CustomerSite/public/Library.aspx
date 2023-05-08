<%@ Page Title="STS: Library" Language="C#" MasterPageFile="~/Scantron_Body_AB_Nav.master" AutoEventWireup="true" CodeFile="Library.aspx.cs" 
    Inherits="public_Library" %>

<asp:Content ID="Content1" ContentPlaceHolderID="For_Title_A" Runat="Server">
    <span class="bannerTitleDark">Library</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="For_Title_B" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="For_Body_A" Runat="Server">

    <table>
        <tr>
            <td>
    <h2 style="margin-top: 5px;">Brochures</h2>
<asp:HyperLink ID="hlAccPartnership" runat="server" 
     Text="Accelerated Partnerships (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/Accelerated_Partnerships_Scantron.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink0" runat="server" 
     Text="AffianceSUITE Managed IT Services for SMB (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/affianceSUITE_Brochure.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink1" runat="server" 
     Text="Cloud Recovery Services (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/HTS_CloudRecoveryParadigm.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink4" runat="server" 
     Text="MPowerPrint Managed Print Services (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/MPowerPrint_Brochure.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink5" runat="server" 
     Text="Nationwide Field Service Technician Sites (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/HTS_NationalMap.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink6" runat="server" 
     Text="Network Implementation Services (PDF) "
     NavigateUrl="~/media/scantron/pdf/lib/HTS_NetworkImplementation.pdf"
     Target="new"  />
<div class="spacer7"></div>

 <asp:HyperLink ID="HyperLink2" runat="server" 
     Text="Scantron Technology Solutions Overview (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/STS_Solutions-Line-Card.pdf"
     Target="new"  />
 <div class="spacer7"></div>

 <asp:HyperLink ID="HyperLink9" runat="server" 
     Text="Security Services (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/HTS_SecurityServices.pdf"
     Target="new"  />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink3" runat="server" 
     Text="Specialty Hardware Business Partnerships (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/STS_Specialty_Hardware_Partnership.pdf"
     Target="new"  />
<div class="spacer7"></div>

    <asp:HyperLink ID="HyperLink10" runat="server" 
     Text="Supply Services (PDF)"
     NavigateUrl="~/media/scantron/pdf/lib/HTS_SupplyServices.pdf"
     Target="new"  />
<div class="spacer7"></div>

<h2 style="margin-top: 10px;">Customer Success Stories</h2>
<asp:HyperLink ID="HyperLink21" runat="server" 
     Text="Bradley Corporation Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/CS-STS_Bradley_Corp_MPS.pdf"
     Target="new" />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink18" runat="server" 
     Text="Caring Medical & Rehabilitation Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/CS-STS_Caring-Medical_Managed-IT.pdf"
     Target="new" />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink11" runat="server" 
     Text="Cokato Manor Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/Cokato_Managed_IT_Case_Study.pdf"
     Target="new" />
<div class="spacer7"></div>
              
<asp:HyperLink ID="HyperLink20" runat="server" 
     Text="Echo Group Inc. Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/Echo_Group-MPS_CS.pdf"
     Target="new" />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink13" runat="server" 
     Text="Grossman & Deitch ENT Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/Grossman_Deitch_Managed_IT_CS.pdf"
     Target="new" />
<div class="spacer7"></div>
              
<asp:HyperLink ID="HyperLink12" runat="server" 
     Text="Nu-Lite Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/Nu-Lite_Managed_IT_Case_Study.pdf"
     Target="new" />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink14" runat="server" 
     Text="Southern Hills Community Bank Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/CS_STS_Southern-Hills_Managed-IT.pdf"
     Target="new" />
<div class="spacer7"></div>

<asp:HyperLink ID="HyperLink19" runat="server" 
     Text="Sparr Building and Farm Supply Case Study (PDF)"
     NavigateUrl="~/media/scantron/pdf/caseStudies/STS_Sparr-HW-MITS.pdf"
     Target="new" />
<div class="spacer7"></div>

 <h2 style="margin-top: 10px;">Articles</h2>
    <asp:HyperLink ID="HyperLink7" runat="server" 
     Text="Five Faulty Assumptions Small Business Owners Make About (PDF)"
     NavigateUrl="~/media/scantron/pdf/customers/Five_Faulty_IT_Assumptions_Small_Business.pdf"
     Target="new" />
<div class="spacer7"></div>
     <asp:HyperLink ID="HyperLink8" runat="server" 
     Text="How to Develop a Print Management Strategy (PDF)"
     NavigateUrl="~/media/scantron/pdf/customers/How_to_Develop_Print_Strategy.pdf"
     Target="new" />
<div class="spacer7"></div>
     <asp:HyperLink ID="HyperLink16" runat="server" 
     Text="Top 10 Things Your IT Provider Won’t Tell You (PDF)"
     NavigateUrl="~/media/scantron/pdf/customers/Top_10_Things_IT_Provider_Won't_Tell_You.pdf"
     Target="new" />
<div class="spacer7"></div>
     <asp:HyperLink ID="HyperLink17" runat="server" 
     Text="Windows End of Support Executive Summary: What you need to know about end of support for Microsoft Windows 7 and Windows Server 2008 (PDF)"
     NavigateUrl="~/media/scantron/pdf/customers/Windows_End_of_Support_Executive_Summary.pdf"
     Target="new" />
<div class="spacer7"></div>
     
<h2 style="margin-top: 10px;">Partner Solutions</h2>
<asp:HyperLink ID="HyperLink15" runat="server" 
     Text="ARCA Cash Recycler (PDF)"
     NavigateUrl="~/media/scantron/pdf/customers/HTS_ArcaFinancialBrochure.pdf"
     Target="new" />
<div class="spacer7"></div>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="For_Body_B" Runat="Server">

</asp:Content>

