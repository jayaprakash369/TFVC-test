// =============================================================
function checkCapsLock(ev) {
    if (isCapslock(ev)) {
        alert("YOUR CAPS LOCK IS ON.\r\n\r\nThis will prevent you from successfully logging in.");
    }
}
// =============================================================
function isCapslock(e) {

    e = (e) ? e : window.event;

    var charCode = false;
    if (e.which) {
        charCode = e.which;
    } else if (e.keyCode) {
        charCode = e.keyCode;
    }

    var shifton = false;
    if (e.shiftKey) {
        shifton = e.shiftKey;
    } else if (e.modifiers) {
        shifton = !!(e.modifiers & 4);
    }

    if (charCode >= 97 && charCode <= 122 && shifton) {
        return true;
    }

    if (charCode >= 65 && charCode <= 90 && !shifton) {
        return true;
    }

    return false;
}
// =============================================================