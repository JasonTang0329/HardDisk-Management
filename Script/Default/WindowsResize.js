function detectBrowser() {
    //判斷瀏覽器版本
    var sAgent = navigator.userAgent.toLowerCase();
    this.isIE = (sAgent.indexOf("msie") != -1); //IE6.0-7
    this.isFF = (sAgent.indexOf("firefox") != -1); //firefox
    this.isSa = (sAgent.indexOf("safari") != -1); //safari
    this.isOp = (sAgent.indexOf("opera") != -1); //opera
    this.isNN = (sAgent.indexOf("netscape") != -1); //netscape
    this.isCh = (sAgent.indexOf("chrome") != -1); //chrome
    this.isMa = this.isIE; //marthon
    this.isOther = (!this.isIE && !this.isFF && !this.isSa && !this.isOp && !this.isNN && !this.isSa); //unknown Browser
}
var oBrowser = new detectBrowser();

function resize() {
    //Iframe resize to fit
    var iFrameID = document.getElementById('ifrmMis');
    if (iFrameID) {
        if (oBrowser.isCh && oBrowser.isSa) {
            $("#ifrmMis").height((iFrameID.contentDocument.documentElement.scrollHeight + 30) + "px");
        } else {
            $("#ifrmMis").height((iFrameID.contentWindow.document.body.scrollHeight + 30) + "px");

        }

    }
    $(window).resize();

}

function modelresize() {
    //bootstrap modal resize 
    setTimeout(function () {
        var f = document.getElementById("ifrmCal");
        if (f.contentDocument) {
            f.height = f.contentDocument.documentElement.scrollHeight + 100; //FF 3.0.11, Opera 9.63, and Chrome
        } else {
            f.height = f.contentWindow.document.body.scrollHeight + 100; //IE6, IE7 and Chrome

        }
    }, 500);
}
function divResize() {
    //if ($(document).height() > 600) {
        //$("#header").height($(window).height() * 0.1);
        $("#menu").height($(window).height() * 0.8);
        $("#content").height($(window).height() * 0.8);
        if ($("#ifaDis").exists()) {
            $("#ifaDis").height($("#content").height() * 0.7);

        }
    //} else {
    //    var headerSize = 70;
    //    $("#header").height(headerSize);
    //    $("#menu").height($(window).height() - headerSize);
    //    $("#content").height($(window).height() - headerSize);

    //}

}