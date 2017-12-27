function viewFunc(ftype, path, filename, isvirt) {
    //預覽按鈕控制
    if (checkViewExt(filename) && ftype == 'F') {
        viewButtonEvent(true, path, ftype);

    } else {
        viewButtonEvent(false, "", ftype);

    }

    function viewButtonEvent(state, path, ftype) {
        //判斷預覽按鈕
        if (state) {
            $("#btnView").show();
        } else {
            $("#btnView").hide();
        }
        $("#btnView").attr("onclick", 'javascript:view("' +  path + '","' + ftype + '")');

    }
}

function view(file, ftype) {
    //按下預覽按鈕
    var openurl = homelink + "/WebService/getFiler.ashx?path=" + encodeURIComponent(file) + "&ftype=" + encodeURIComponent(ftype) + "&show=view";
    window.open(openurl);
}
function checkViewExt(filename) {

    var allowextarr = [".mp4", ".jpg", ".jpeg", ".gif", ".png", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".xml",".html",".txt"]
    var canView = false;
    var ext = "." + getFileExtension(filename);
    $.each(allowextarr, function (index, value) {
        if (value == ext) {
            canView = true;
            return false;
        }
    });
    return canView;
}
