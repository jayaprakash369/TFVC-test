using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Obj_ServiceRequest
/// </summary>
public class Obj_ServiceRequest
{
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string address3 { get; set; }
    public string city { get; set; }
    public string commentForAllRequestItems { get; set; }
    public string companyPhone { get; set; }
    public string companyPhoneExtension { get; set; }
    public string contactName { get; set; }
    public string creatorUsername { get; set; }
    public string customerLocation { get; set; }
    public string customerName { get; set; }
    public string customerNumber { get; set; }
    public string dealerNumber { get; set; }
    public string defaultContactName { get; set; }
    public string emailForAcknowledgement { get; set; }
    public string keyForWorkfiles { get; set; }
    public string equipmentQtyOnRequest { get; set; }
    public string payingByAgrOrTM { get; set; }
    public string preferredTechContactMethod { get; set; }
    public string preferredTechContactDetail { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
    
    public List<Equipment> equipmentList { get; set; }

    // --------------------------------------------
    public Obj_ServiceRequest()
    {
        //
        // TODO: Initialize Object Values (so they are not null) 
        //
    }
    // --------------------------------------------

    public class Equipment
    {
        public string agreementCode { get; set; }
        public string agreementDescription { get; set; }
        public string agreementNumber { get; set; }
        public string alternateRequestType { get; set; }
        public string assignToSpecificTech { get; set; }
        public string callPaymentCode { get; set; }
        public string center { get; set; }
        public string flexField { get; set; }
        public string itemAutoSubmitted { get; set; }
        public string itemOnContract { get; set; }
        public string itemSequence { get; set; }
        public string model { get; set; }
        public string priority { get; set; }
        public string purchaseOrder { get; set; }
        public string serial { get; set; }
        public string problemSubtype { get; set; }
        public string problemSummary { get; set; }
        public string customerCallAlias { get; set; }
        public string printerInterfaceType { get; set; }
        public string shipVia { get; set; }
        public string source { get; set; }
        public string ticket { get; set; }
        public string unit { get; set; }
        
        // protected string massTicketNote { get; set; } // never called from the customer site...
    }
    // -------------------------------------------------------

    // ============================================
    // ============================================
}