function clickFunc(event) {
    //點擊Meun觸發事件
    closeAddDiv();
    if (typeof ($(this).attr("ctr")) == "undefined") {

        $(this).attr("ctr", "open");
        $(this).find($(".glyphicon-triangle-right")).removeClass('glyphicon-triangle-right').addClass("glyphicon glyphicon-triangle-bottom");
        var path = $(this).parent().find($(".filer")).eq(0).attr('path');
        var callback;
        if (event.data && event.data.callback) {
            callback = event.data.callback;
        }
        $.ajax({
            context: this,
            url: homelink + "/WebService/Filer.ashx",
            type: "POST",
            dataType: 'text',
            data: {
                path: path,
                kind: "Nas"
            },
            success: function (msg) {

                if (msg != "") {
                    var data = JSON.parse(msg);
                    $(this).parent().append(menuAppendData(data));
                }
                rebind();
               //if (typeof (callback) === 'function') callback();
                if (callback) {
                    callback();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("Error" + thrownError);
            }
        });
    } else {

        if ($(this).attr("ctr") == "open") {
 
            $(this).find($(".glyphicon-triangle-bottom")).removeClass('glyphicon-triangle-bottom').addClass("glyphicon glyphicon-triangle-right");

            $(this).attr("ctr", "close");
            $(this).parent().find('ul').eq(0).attr("style", "display:none");

        } else if ($(this).attr("ctr") == "close") {
 
            $(this).find($(".glyphicon-triangle-right")).removeClass('glyphicon-triangle-right').addClass("glyphicon glyphicon-triangle-bottom");

            $(this).attr("ctr", "open");
            $(this).parent().find('ul').eq(0).attr("style", "");
        }
 
    }
}

function menuAppendData(data) {
    //產生新的Menu
    var ul = $("<ul>").addClass("menu");
    for (var j = 0; j < data.length; j++) {
        ulappenddata(ul, data[j]);
    }
    return ul;
}

function ulappenddata(ul, array) {
    //產生新的li連結
    var li = $("<li>");
    var span = $("<span>");
    var a = $("<a>").append($("<span>").append(" " + array.data).addClass("fname")).attr("href", array.link).attr("path", array.path).attr("onclick", "return false;").attr("ftype", array.Type).attr("isVirt", array.isVirt);
    //檔案夾
    if (array.Type == "D") {
        span.addClass("glyphicon glyphicon-folder-close");
        a.addClass("open filer");
        var loada = $("<a>").addClass("folder").attr("href", array.link).attr("onclick", "return false;");
        var loadspan = $("<span>").addClass("glyphicon glyphicon-triangle-right").attr("aria-hidden", "true");
        li.append(loada.append(loadspan));

    }
    //檔案
    if (array.Type == "F") {
        span.addClass("glyphicon glyphicon-open-file");
        a.addClass("open file");
    }
    //連結
    if (array.Type == "L") {
        span.addClass("glyphicon glyphicon-link");
        a.addClass("open file");
    }
    li.append(a.prepend(span));

    if (array.isVirt == "true") {
        var virtClass = "VirtDelete";
        var virtitle = (array.Type == "L" ? "刪除連結" : "刪除目錄");
        var iconclass = "glyphicon glyphicon-remove";
        var addicon = $("<a>").addClass("Delete").attr("href", array.link).attr("onclick", "return false;").addClass("pull-right").addClass(virtClass).attr("title", virtitle);
        var icon = $("<span>").addClass(iconclass).attr("aria-hidden", "true");
        li.append(addicon.append(icon));


    }

    if (!checkDonloadExt(array.data) && ((array.isVirt == "true" && array.Type != "D") || array.isVirt == "false")) {
        //出現連結或下載ICON在檔案名稱後
        var linkclass = (array.Type == "F" ? "opendonload" : (array.Type == "L" ? "openlink" : "copytocilp"));
        var linktitle = (array.Type == "F" ? "下載檔案" : (array.Type == "L" ? "開啟連結" : "複製路徑到剪貼簿"));
        var iconclass = (array.Type == "F" ? "glyphicon glyphicon glyphicon-save" : (array.Type == "L" ? "glyphicon glyphicon-globe" : "glyphicon glyphicon-duplicate"));
        var addicon = $("<a>").addClass("link").attr("href", array.link).attr("onclick", "return false;").addClass("pull-right").addClass(linkclass).attr("title", linktitle);
        var icon = $("<span>").addClass(iconclass).attr("aria-hidden", "true");
        li.append(addicon.append(icon));
    }
    ul.append(li);
}

function opendonload() {
    var file = $(this).parent().find($(".open")).eq(0).attr('path').replace(/\\/g, '/');
    var ftype = $(this).parent().find($("a")).eq(0).attr('ftype');
    download(file, ftype);

}
function openlink() {
    var file = $(this).parent().find($(".open")).eq(0).attr('href');
    var ftype = $(this).parent().find($("a")).eq(0).attr('ftype');

    Link(file, ftype);
}
function copytocilp() {
    var path = $(this).parent().find($(".open")).eq(0).attr('path');
    if (oBrowser.isCh && oBrowser.isSa) {//CHROME邊解碼方式不同會造成PATH加解密不同
        path = path;
    }
    new Clipboard(".copytocilp", {
        text: function (trigger) {
            return path;
        }
    });
    bootbox.alert({ message: "已將路徑:" + path + " 複製到剪貼簿!", backdrop: true });
}

function VirtDelete() {
    var li = $(this).parent();
    var Element = li.find($(".open")).eq(0);

    bootbox.confirm("確定要刪除??", function (confirmed) {
        if (confirmed) {
             var path = Element.attr('path').replace(/\\/g, '/');
             var ftype = Element.attr("ftype");
             var isVirt = Element.attr('isvirt');
             var filename = Element.text();
              $.ajax({
                 context: this,
                 url: homelink + "/WebService/DeleteFiler.ashx",
                 type: "POST",
                 dataType: 'text',
                 data: {
                     path: path,
                     ftype: ftype,
                     isVirt: isVirt
                 },
                 success: function (msg) {
             
                     if (msg != "") {
                         console.log(msg);
                         bootbox.alert('刪除失敗');

                     } else {
                         bootbox.alert('刪除成功');
                     }
                     rebind();
                     li.remove();
                 },
                 error: function (xhr, ajaxOptions, thrownError) {
                     console.log("Error" + thrownError);
                 }
             });
        }
    });

}