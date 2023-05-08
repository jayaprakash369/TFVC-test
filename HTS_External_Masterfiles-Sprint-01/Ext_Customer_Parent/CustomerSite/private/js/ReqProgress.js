// ---------------------------------------------
function jHideButton(jButton) {

    // First check for invalid characters that need to be encoded
    //alert("starting Ajax check: " + jButton);
    var jObj, jValue;
    //var jAspxName = "ctl00_BodyContent_txComment";
    var jAspxName = "ctl00_ctl00_For_Body_A_For_Body_A_txComment";
    
    // Checking if the note text is OK (for html)
    if (document.getElementById(jAspxName) != null) {
        jObj = document.getElementById(jAspxName);
        jValue = jObj.value;
        if ((jValue.indexOf("<") != -1) || (jValue.indexOf(">") != -1)) {
            jValue = jValue.replace(/</g, "&lt;");
            jValue = jValue.replace(/>/g, "&gt;");
            jObj.value = jValue;
            alert("Your entry contains characters which are being encoded for security purposes");
            return false;
        }
        else {
            // Hide button behind the progress bar.
            var doc = document.forms[0];
            var posLeft = 0;
            var posTop = 0;
            if (jButton.value == "Submit Manual Request") {
                //posLeft = (findPosX(jButton) - 136) + "px"; 
                posLeft = (findPosX(jButton) - 10) + "px"; 
            }
            else {
                //posLeft = (findPosX(jButton) - 124) + "px";
                posLeft = (findPosX(jButton) - 10) + "px";
            }

            //posTop = (findPosY(jButton) - 10) + "px";  // -5 -20
            posTop = (findPosY(jButton) - 16) + "px";  // -5 -20
            //posLeft = -10000;
            //posTop = -10000;
            var objBox;

            if (document.getElementById("objHideButton") != null) {
                objBox = document.getElementById("objHideButton");
                objBox.style.top = posTop;
                objBox.style.left = posLeft;
            }
            if (document.getElementById("tdBar") != undefined) {
                barClock();
            }
        }
    }
    //alert("leaving Ajax check");
}
// ---------------------------------------------
var idx = 0;
var objTxt;
var objBar;

function barClock() {
    //objTxt = document.getElementById("ctl00_BodyContent_txAjax");
    objTxt = document.getElementById("ctl00_ctl00_For_Body_A_For_Body_A_txAjax");
    objBar = document.getElementById("tdBar");
    if (idx <= 100) {
        //if ((idx % 4) == 0) {
        if ((idx % 6) == 0 || idx == 2 || idx == 5 || idx == 8 || idx == 9 || idx == 13 || idx == 15 || idx == 21 || idx == 27 || idx == 68 || idx == 75 || idx == 87 || idx == 93 || idx == 99) {
            objTxt.value = (1 * idx) + "% Complete";
            objBar.style.width = (4 * idx) + "px";
        }
        if (idx == 30) {
            idx = 32;
        }
        if (idx == 50) {
            idx = 52;
        }
    }
    else if (idx > 330) {
        objTxt.value = "Sorry.  Please call 1-800-228-3628 about this...";
    }
    else if (idx > 260) {
        objTxt.value = "I really am tracking how long this has taken...";
    }
    else if (idx > 240) {
        objTxt.value = "You are a very patient person...";
    }
    else if (idx > 220) {
        objTxt.value = "Any second...";
    }
    else if (idx > 190) {
        objTxt.value = "It should be finished ANY second now...";
    }
    else if (idx > 160) {
        objTxt.value = "Our server appears to be currently very busy.";
    }
    else if (idx > 140) {
        objTxt.style.width = "370px";
        objTxt.value = "This request is REALLY taking a long time...";
    }
    idx += 1;
    setTimeout("barClock(" + idx + ")", 250);
}
// ---------------------------------------------
function ReadCookie(cookieName) {
    var theCookie = "" + document.cookie;
    var ind = theCookie.indexOf(cookieName);
    if (ind == -1 || cookieName == "") return "";
    var ind1 = theCookie.indexOf(';', ind);
    if (ind1 == -1) ind1 = theCookie.length;
    return unescape(theCookie.substring(ind + cookieName.length + 1, ind1));
}
// ---------------------------------------------        
// Use client cookie for firefox to return to start page
if (ReadCookie('clientPage') == "Result") {
    window.location = "location.aspx";
}
// ===================================
