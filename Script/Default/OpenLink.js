function LinkFunc(ftype, path, isvirt) {
    //超連結按鈕事件
    if (ftype != "L" || isvirt != 'true') {
        LinkdButtonEvent(false, "", ftype);
    } else {
        LinkdButtonEvent(true, path, ftype);

    }

    function LinkdButtonEvent(state, path, ftype) {
        //判斷是否開啟按鈕
        if (state) {
            $("#btnOpen").show();
        } else {
            $("#btnOpen").hide();
        }
        $("#btnOpen").attr("onclick", "javascript:Link('" + path + "','" + ftype + "')");

    }
}

function Link(path, ftype) {
    //開啟新連結
    var openurl = path;
    window.open(openurl);
}