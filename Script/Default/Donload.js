function donloadFunc(ftype, path, filename, isvirt) {
    //判斷是否出現下載按鈕
    if (checkDonloadExt(filename) || isvirt == 'true') {
        donloadButtonEvent(false, "", ftype);
    } else {
        donloadButtonEvent(true, path, ftype);

    }

    function donloadButtonEvent(state, path, ftype) {
        if (state) {
            $("#btnDonload").show();
        } else {
            $("#btnDonload").hide();
        }
        $("#btnDonload").attr("onclick", 'javascript:download("' + path + '","' + ftype + '")');

    }
}

function download(file, ftype) {
    //下載按鈕連結
    var openurl = homelink + "/WebService/getFiler.ashx?path=" + encodeURIComponent(file) + "&ftype=" + encodeURIComponent(ftype);
    window.open(openurl);
}
function checkDonloadExt(filename) {
    //確認是為下載排除名單
    getFileExtension(filename)
    var canView = false;
    var ext = "." + getFileExtension(filename);
    $.each(disallowDonloadExtArr.split(','), function (index, value) {
        if (value == ext) {
            canView = true;
            return false;
        }
    });
    return canView;
}