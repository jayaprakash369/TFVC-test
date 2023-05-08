// ===================================
var i;
var calls=0;
var latlng;
var name;
var lat=0;
var lng=0;
var loops=0;
var oldLat=0;
var oldLng=0;
var oldPoints=[];
var newPoints = [];
var oldMarkers=[];
var newMarker;
var markerOverlay;
var custNum;
var activityDate;
var tDtl;
var url;
var map;
var lineColor="#ff0000"
var overlays=0
google.load("maps", "2.x");     
	
// ===================================	
// Call this function when the page has been loaded  
function initialize() {
		map = new google.maps.Map2(document.getElementById("map"));   
		map.addControl(new GLargeMapControl());
		map.addControl(new GMapTypeControl());
		map.setCenter(new GLatLng(41.236342, -100.185017), 6);
		//alert("inside initialize");
		go(); 
		return false;
	} 
// ===================================		
google.setOnLoadCallback(initialize);
//window.setInterval('go()', 5000); //used for auto-refresh
// ===================================			
function load() {
	if (GBrowserIsCompatible()) {
		map = new GMap2(document.getElementById("map"));
	}
}
// ===================================			
function go(){
	try{
	    //		custNum =<%response.write(custNum)%>
	    custNum = 99999
		url = "http://www.scantronts.com/getCustomerOpenCalls.asp?custNum=" + custNum;
		getPoint();
	}
	catch(ex){
		alert("Help"+ex.toString());
	}
}
// ===================================				
oldLat=lat;
oldLng=lng;
// ===================================				
function getPoint(){
	try{
		GDownloadUrl(url, processXML);
	}
	catch(ex){
		alert("Fetch XML FAILURE: "+ex.toString());
	}
}
// ===================================			
function processXML(response, responseCode){
	    
	xmlResponse=GXml.parse(response);  
	var zips="";
	var locationName;
	var address;
	var center;
	var ticket;
	var cust;
	var cLoc;		
	var lat;
	var lng;
	var points = xmlResponse.getElementsByTagName("point");
	calls=points.length;
	try{
		if (calls>0){
			oldPoints=[];
			newPoints=[];
			for (i = 0; i < calls; i++) {
				var nodeList = points.item(i).childNodes;
				for (var j = 0; j < nodeList.length ; j++) {
					var node = nodeList.item(j);
					if (node.nodeName == "center") {
						center=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "ticket") {
						ticket=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "cust") {
						cust=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "cLoc") {
						cLoc=node.firstChild.nodeValue; 
					}					
					if (node.nodeName == "name") {
						locationName=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "address") {
						address=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "lat") {
						lat=node.firstChild.nodeValue; 
					}
					if (node.nodeName == "lng") {
						lng=node.firstChild.nodeValue; 
					}
				}
					
				try{
					var randomDecimal=Math.random();
					var markerOffset=(randomDecimal-(randomDecimal.toFixed(3)-.001));
					var offsetLngPos=((lng*1)+(markerOffset*1));
					var offsetLngNeg=lng-markerOffset;
					var latlng=new GLatLng(lat,lng);
					if ((Math.round(i/2)*2)==i){
						var markerLatlng=new GLatLng(lat,offsetLngPos.toFixed(6));
					}
					else{
						var markerLatlng=new GLatLng(lat,offsetLngNeg.toFixed(6));
					}
						
					var tDtl = "http://www.scantronssg.com/cgi-bin/emp/TicketDetailOrg.d2w/report?cust="+cust+"&custl="+cLoc+"&center="+center+"&ticket="+ticket;
					var marker = createMarker(markerLatlng,"STS Ticket #: "+center+"-"+ticket+"<br>"+locationName+"<br>"+address, tDtl);
					map.addOverlay(marker);

					}
				catch(ex){
					alert("Blew up in processGCG: "+ex.toString());
				}
					
			}
			//alert(calls+" open call(s) for customer # "+custNum);
			map.setCenter(new GLatLng(41.236342, -100.185017), 4);
		}
		else{
			 
			alert(calls+" open call(s) for customer # "+custNum);
		}
	}
	catch(ex){
		alert("Blew up here");
	}
}
// ===================================				
// Creates a marker at the given point with the given name
function createMarker(point, name, tDtl) {
	try{
		var tinyIcon = new GIcon(); 
		if(i==0){
			tinyIcon.image = "http://www.scantronts.com/googlemapsicons/blue-pushpin.png";
		  	tinyIcon.shadow = "http://www.scantronts.com/googlemapsicons/pushpin_shadow.png";
		  	}
		else{
			tinyIcon.image = "http://www.scantronts.com/googlemapsicons/blue-pushpin.png";
			tinyIcon.shadow = "http://www.scantronts.com/googlemapsicons/pushpin_shadow.png";
			}
		//tinyIcon.iconSize = new GSize(20, 34);
		//tinyIcon.shadowSize = new GSize(22, 20);
		tinyIcon.iconAnchor = new GPoint(6, 36);
		tinyIcon.infoWindowAnchor = new GPoint(6, 36);
		//var marker = new GMarker(point);
		var marker=new GMarker(point, {icon:tinyIcon,draggable:true, bouncy:true});
		GEvent.addListener(marker, "click", function() {
		//alert("tDtl: " + tDtl);
		marker.openInfoWindowHtml("<tr><td valign='top'>"+name+ "<a href="+tDtl+">Details </a></td></tr>",500);
			
		});
		return marker;
	}
	catch(ex){}
}
// ===================================