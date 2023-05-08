// ===================================
// Google Map Functions
// ===================================
var jCs1;
var i;
var calls = 0;
var latlng;
var name;
var lat = 0;
var lng = 0;
var loops = 0;
var oldLat = 0;
var oldLng = 0;
var oldPoints = [];
var newPoints = [];
var oldMarkers = [];
var newMarker;
var markerOverlay;
var custNum;
var activityDate;
var tDtl;
var url;
var map;
var lineColor = "#ff0000"
var overlays = 0
var nLat = "";
var nLon = "";

// ===================================	
// Call this function when the page has been loaded  
function initialize() {
    map = new google.maps.Map2(document.getElementById("map"));
    map.addControl(new GLargeMapControl());
    map.addControl(new GMapTypeControl());
    map.setCenter(new GLatLng(41.236342, -100.185017), 6);
    go();
    return false;
}
// ===================================			
function load() {
    if (GBrowserIsCompatible()) {
        map = new GMap2(document.getElementById("map"));
    }
}
// ===================================			
function go() {
    try {
        var doc = document.forms[0];

        var jCs1 = doc.ctl00_ctl00_For_Body_A_For_Body_A_hfCs1.value;
        var jMod = doc.ctl00_ctl00_For_Body_A_For_Body_A_hfMod.value;
        var jKey = doc.ctl00_ctl00_For_Body_A_For_Body_A_hfKey.value;
        url = "http://www.Scantronts.com/private/sc/map/OpenTicketXml.aspx?cs1=" + jCs1 + "&mod=" + jMod;
        getPoint();
        //        url = "http://www.scantron.com/private/sc/map/OpenTicketXml.aspx?cs1=" + jCs1 + "&mod=" + jMod;
        //tryGet();

    }
    catch (ex) {
        alert("Help " + ex.toString());
    }
}
//http.Open "GET", "http://maps.google.com/maps/geo?q="&address&"&output=csv&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ", False
// ===================================				
function getPoint() {
    try {
        GDownloadUrl(url, processXML);
    }
    catch (ex) {
        alert("Fetch XML FAILURE: " + ex.toString());
    }
}

// ===================================			
function getXMLHttpRequestObject() {
    var xmlhttp;
    /* @cc_on   
    @if (@_jscript_version >= 5)     
    try 
    {       
    xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");     
    } 
    catch (e) 
    {       
    try {         
    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");       
    } 
    catch (E) 
    {         
    xmlhttp = false;       
    }     
    }   
    @else   xmlhttp = false;   
    @end @
    */

    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        try {
            xmlhttp = new XMLHttpRequest();
        }
        catch (e) {
            xmlhttp = false;
        }
    }
    return xmlhttp;
}

// ===================================
function tryGet(jAdr) {
    jAdr = jAdr.replace(/ /g, "+");
    var http = new getXMLHttpRequestObject();
    var url = "http://www.scantronts.com/private/sc/map/getLatLon.aspx?address=" + jAdr;
    //    http.open("GET", url + jAdr + parameters, true);
    http.open("GET", url, false); // false=wait for return before continuing true=don't wait, continue this code, let post finish when it can
    http.onreadystatechange = function () {
        //Handler function for call back on state change.     
        if (http.readyState == 4) {
            jResponse = http.responseText;
            var saLatLon = http.responseText.split("|");
            nLat = saLatLon[0];
            nLon = saLatLon[1];
        }
    }
    http.send(null);
}
//string sURL = "http://maps.google.com/maps/geo?q=" + sAdr + "&output=csv&key=ABQIAAAASm-XmuIcdy39DwcNIACgJRRfDI6lXUJSLG_DQSetZhcNwG00gBTeY0TH9pGfLEpitfbZ4wnJKVrzSQ";
// ===================================			
function processXML(response, responseCode) {

    var xmlResponse = GXml.parse(response.toString());
    var zips = "";
    var locationName;
    var address;
    var center;
    var ticket;
    var cust;
    var cLoc;
    var lat;
    var lng;
    var points = xmlResponse.getElementsByTagName("point");
    calls = points.length;
    try {
        if (calls > 0) {
            oldPoints = [];
            newPoints = [];
            for (i = 0; i < calls; i++) {
                var nodeList = points.item(i).childNodes;
                for (var j = 0; j < nodeList.length; j++) {
                    var node = nodeList.item(j);
                    if (node.nodeName == "center") {
                        center = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "ticket") {
                        ticket = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "cust") {
                        cust = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "cLoc") {
                        cLoc = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "name") {
                        locationName = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "address") {
                        address = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "lat") {
                        lat = node.firstChild.nodeValue;
                    }
                    if (node.nodeName == "lng") {
                        lng = node.firstChild.nodeValue;
                    }
                }
                //alert(address);
                nLat = "";
                nLon = "";
                tryGet(address);
                //alert("After Lat... " + nLat);
                //alert("After Lon... " + nLon);
                lat = nLat;
                lng = nLon;

                try {
                    var randomDecimal = Math.random();
                    var markerOffset = (randomDecimal - (randomDecimal.toFixed(3) - .001));
                    var offsetLngPos = ((lng * 1) + (markerOffset * 1));
                    var offsetLngNeg = lng - markerOffset;
                    var latlng = new GLatLng(lat, lng);
                    if ((Math.round(i / 2) * 2) == i) {
                        var markerLatlng = new GLatLng(lat, offsetLngPos.toFixed(6));
                    }
                    else {
                        var markerLatlng = new GLatLng(lat, offsetLngNeg.toFixed(6));
                    }
                    var jKey = encryptTicket(center, ticket);
                    var tDtl = "http://www.scantronts.com/public/sc/ticketdetail.aspx?key=" + jKey;
                    //var marker = createMarker(markerLatlng, "HTS Ticket: " + center + "-" + ticket + "&nbsp;&nbsp" + tDtl + "<br /> " + locationName + "<br />" + address, tDtl);
                    var marker = createMarker(markerLatlng, "HTS Ticket: " + center + "-" + ticket + "<br /> " + locationName + "<br />" + address + "&nbsp;&nbsp;&nbsp;", tDtl);
                    map.addOverlay(marker);
                }
                catch (ex) {
                    // alert("Blew up in processGCG: " + ex.toString()); // Was in original
                }
            } // end for each call
            //alert(calls+" open call(s) for customer # "+custNum);
            map.setCenter(new GLatLng(41.236342, -100.185017), 4);
        }
        else {

            alert(calls + " open call(s) for customer # " + cs1);
        }
    }
    catch (ex) {
        alert("Please return to select other tickets");
    }
}
// ===================================				
// Creates a marker at the given point with the given name
function createMarker(point, name, tDtl) {
    try {
        var tinyIcon = new GIcon();
        if (i == 0) {
            tinyIcon.image = "http://www.scantronts.com/media/scantron/images/support/map/blue-pushpin.png";
            tinyIcon.shadow = "http://www.scantronts.com/media/scantron/images/support/map/pushpin_shadow.png";
        }
        else {
            tinyIcon.image = "http://www.scantronts.com/media/scantron/images/support/map/map/blue-pushpin.png";
            tinyIcon.shadow = "http://www.scantronts.com/media/scantron/images/support/map/map/pushpin_shadow.png";
        }
        //tinyIcon.iconSize = new GSize(20, 34);
        //tinyIcon.shadowSize = new GSize(22, 20);
        tinyIcon.iconAnchor = new GPoint(6, 36);
        tinyIcon.infoWindowAnchor = new GPoint(6, 36);
        //var marker = new GMarker(point);
        //var marker = new GMarker(point, { icon: tinyIcon, draggable: true, bouncy: true });
        var marker = new GMarker(point, { icon: tinyIcon, draggable: true, bouncy: false });
        GEvent.addListener(marker, "click", function () {
            //alert("tDtl: " + tDtl);
            marker.openInfoWindowHtml("<tr><td valign='top'>" + name + "<a href=" + tDtl + ">Details </a></td></tr>", 500);

        });
        return marker;
    }
    catch (ex) { }
}
// ===================================				
function encryptTicket(sCtr, sTck) {

    var sEncrypted = "";
    var sCtrTck = "";
    var j = 0;

    for (j = 0; j < 7; j++) {
        if (sCtr.length < 3)
            sCtr = "0" + sCtr;
        if (sTck.length < 7)
            sTck = "0" + sTck;
    }
    sCtrTck = sCtr + sTck;

    var saCode = ["", "", "", "", "", "", "", "", "", ""];

    saCode[0] = "aBcDeFgHiJ";
    saCode[1] = "kLmNoPqRsT";
    saCode[2] = "uVwXyZAbCd";
    saCode[3] = "EfGhIjKlMn";
    saCode[4] = "OpQrStUvWx";
    saCode[5] = "YzaBcDeFgH";
    saCode[6] = "iJkLmNoPqR";
    saCode[7] = "sTuVwXyZAb";
    saCode[8] = "CdEfGhIjKl";
    saCode[9] = "MnOpQrStUv";

    var sNums = "5820416937";
    var iTckNum = 0;
    var sReplacementChar = "";
    sEncrypted = sNums;
    var sCurrCode = ""

    for (j = 0; j < 10; j++) {
        // for each pos 1-10 get number in ticket
        iTckNum = sCtrTck.substr(j, 1);
        // replacement character = a) array of codes[loop num] b) character at replacement position
        sCurrCode = saCode[j];
        sReplacementChar = sCurrCode.substr(iTckNum, 1);
        // use .Replace to move the replacement character to the new position in encrypted value
        sEncrypted = sEncrypted.replace(j.toString(), sReplacementChar);
    }

    return sEncrypted;
}
// ===================================
oldLat = lat;
oldLng = lng;
