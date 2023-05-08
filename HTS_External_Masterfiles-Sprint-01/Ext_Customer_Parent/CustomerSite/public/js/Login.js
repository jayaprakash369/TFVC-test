function clearInput() {
    var doc = document.forms[0];
    if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_txUserID != "undefined") {
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txUserID.value = "";
    }
    if (typeof doc.ctl00_ctl00_For_Body_A_For_Body_A_txPassword != "undefined") {
        doc.ctl00_ctl00_For_Body_A_For_Body_A_txPassword.value = "";
    }
}
