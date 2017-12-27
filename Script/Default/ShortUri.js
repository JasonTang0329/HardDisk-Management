function openTree(path, Suri, i, long) {

    if (Suri[i] == "") {
        openTree(path, Suri, i + 1, long);
    } else {
        if (i <= long) {
            path += "\\\\" + Suri[i].replace('%20', ' ');
            var a = $("[path$='" + path + "']");
            console.log(a);

             // 這個click是為了開啟檔案資訊用 主要透過Bind() 做Event bind，實際bind時機透過clickFunc.bind() call default.aspx bind();
            // a 在哪邊bind click function?
            a.click();
            var afolder = a.parent().find('a').eq(0);

            if (typeof (afolder.attr("ctr")) == "undefined") {

                afolder.unbind('click');
                afolder.click({
                    callback: function () {
                        openTree(path, Suri, i + 1, long);
                    }
                }, clickFunc);
                afolder.click();
            } else {
                if (afolder.attr("ctr") == "open") {

                    afolder.find($(".glyphicon-triangle-bottom")).removeClass('glyphicon-triangle-bottom').addClass("glyphicon glyphicon-triangle-right");

                    afolder.attr("ctr", "close");
                    afolder.parent().find('ul').eq(0).attr("style", "display:none");

                } else if (afolder.attr("ctr") == "close") {

                    afolder.find($(".glyphicon-triangle-right")).removeClass('glyphicon-triangle-right').addClass("glyphicon glyphicon-triangle-bottom");

                    afolder.attr("ctr", "open");
                    afolder.parent().find('ul').eq(0).attr("style", "");
                }


            }
            //if (typeof (a.attr("ctr")) == "undefined" || a.attr("ctr") != "open") {
            //    ///a.parent().find('a').eq(0).click(function () {
            //    ///    clickFunc(function () {
            //    ///        func;
            //    ///    });
            //    ///
            //    ///});
            //    debugger;

            //    // 貌似有點下去的動作 但是沒有執行到
            //    a.unbind('click');
            //    a.parent().find('a').eq(0).click({
            //        callback: function () {
            //            openTree(path, Suri, i + 1, long);

            //        }
            //    }, clickFunc);
            //    a.click();
            //}
        } else {
            rebind();
        }

    }

}




function shortURIFunc(ftype, path, filename, isvirt) {
    var grid = "";
    $.ajax({
        context: this,
        url: homelink + "/WebService/getFileInfo.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            ftype: ftype,
            path: path,
            isvirt: isvirt,
            kind: "gird"
        },
        success: function (msg) {
            grid = msg;
            $("#btnURL").attr("onclick", 'javascript:ShortURI("' + grid + '")');

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    });



}

function ShortURI(grid) {
    if (grid != "")
    {
        path = homelink + "?uri=" + grid;
        new Clipboard("#btnURL", {
            text: function (trigger) {
                return path;
            }
        });
        bootbox.alert({ message: "已將短網址:" + path + " 複製到剪貼簿!", backdrop: true });

    }
}