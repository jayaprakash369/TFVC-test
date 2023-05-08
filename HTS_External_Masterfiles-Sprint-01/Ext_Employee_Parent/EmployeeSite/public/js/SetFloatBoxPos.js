function setFloatBoxPos(sourceName, targetName) {
    //var jObj = document.getElementById("ctl00_BodyContent_" + sourceName);
    var sourceObj, targetObj, posLeft, posTop;

    if (document.getElementById(sourceName) != null) {
        sourceObj = document.getElementById(sourceName);
        posLeft = (findPosX(sourceObj) + 0) + "px";
        posTop = (findPosY(sourceObj) + 25) + "px";
    }
    if (document.getElementById(targetName) != null) {
        targetObj = document.getElementById(targetName);
        if (sourceObj.className == "ON") {
            targetObj.style.top = posTop;
            targetObj.style.left = posLeft;
        }
        else {
            targetObj.style.top = "-1000px";
            targetObj.style.left = "-1000px";
        }
    }
}
