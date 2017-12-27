function openFunc() {
    //點擊Tree 後執行open
    closeAddDiv();
    $(".glyphicon-hand-right").remove();
    $(".liselect").removeClass('liselect');
    if ($(this).attr('ftype') == "D") {
        $("#addicon").show();
    } else {
        $("#addicon").hide();

    }
    $(this).parent().prepend($("<span>").addClass('glyphicon glyphicon-hand-right'));
    $(this).addClass('liselect');
    var path = $(this).attr('path').replace(/\\/g, '/');
    var file = $(this).children('.fname').text().trim('');
    var isvirt = $(this).attr('isvirt');
    var ftype = $(this).attr("ftype");
    var ahref = $(this).attr("href");
    $("#H1Title").text(file);
    donloadFunc(ftype, path, file, isvirt);
    viewFunc(ftype, path, file, isvirt);
    shortURIFunc(ftype, path, file, isvirt);
    LinkFunc(ftype, $(this).attr('href'), isvirt);
    $("#btnaddannotation").attr("onclick", 'show("EditPage.aspx?path=' + path + '&file=' + file + '&ftype=' + ftype + '&isVirt=' + isvirt + '","annotation","' + path + '")');
   // $("#btnaddcomments").attr("onclick", 'show("AddComments.aspx?path=' + path + '&file=' + file + '&ftype=' + ftype + '&isVirt=' + isvirt + '","comments","' + path + '")');
    meteringFunc(ftype, path, file, isvirt); //紀錄點擊次數
 
    fileDisplayFunc(ftype, (ftype == "L" ? ahref : path), file, isvirt);//如果是超連結，將路徑換為超連結路徑
    fileAnnotationFunc(ftype, path, isvirt);
    fileCommentsFunc(ftype, path, isvirt);
    rebind();
 }

function fileDisplayFunc(ftype, path, filename, isvirt) {
     //判斷檔案類型顯示檔案圖片
    $("#filedisplay").empty();
    var ext = getFileExtension(filename);
    if (ext != '' && checkViewExt(filename) && ftype == 'F') {
        var openurl = homelink + "/WebService/getFiler.ashx?path=" + encodeURIComponent(path) + "&ftype=" + encodeURIComponent(ftype) + "&show=view";

        $("#filedisplay").append($("<iframe>").attr("id", "ifaDis").width('100%').height('100%').attr("src", openurl)
                                              .attr("frameborder", 0).attr("border", 0));
                                               
        $("#ifaDis").height($("#content").height() * 0.6);

    }
    else if(ftype == 'L'){
        $("#filedisplay").append($("<iframe>").attr("id", "ifaDis").width('100%').height('100%').attr("src", path)
                                              .attr("frameborder", 0).attr("border", 0));
        $("#ifaDis").height($("#content").height() * 0.6);
     }
    else {
        $("#filedisplay").append($("<img>").attr("src", 'App_Themes/layout/icon/blank.png'));

    }
    function checkImageExt(ext) {
        var imgext = ["3dm", "3ds", "3g2", "3gp", "7z", "7zip", "aac", "ai", "aif", "angel", "apk", "app", "asf", "asp", "aspx", "asx", "avi", "bak", "bat", "bin", "blank", "bmp", "cab", "cad", "cdr", "cer", "cfg", "cfm", "cgi", "class", "com", "cpl", "cpp", "crx", "csr", "css", "csv", "cue", "cur", "dat", "db", "dbf", "dds", "debian", "dem", "demon", "dll", "dmg", "dmp", "doc", "docx", "drv", "dtd", "dwg", "dxf", "elf", "eml", "eps", "exe", "fla", "flash", "flv", "fnt", "fon", "gam", "gbr", "ged", "gif", "gpx", "gz", "gzip", "hqz", "html", "ibooks", "icns", "ico", "ics", "iff", "indd", "ipa", "iso", "jar", "jpg", "js", "jsp", "key", "kml", "kmz", "lnk", "log", "lua", "m3u", "m4a", "m4v", "mach", "max", "mdb", "mdf", "mid", "mim", "mov", "mp3", "mp4", "mpa", "mpg", "msg", "msi", "nes", "object", "odb", "odc", "odf", "odg", "odi", "odp", "ods", "odt", "odx", "ogg", "otf", "pages", "pct", "pdb", "pdf", "pif", "pkg", "pl", "png", "pps", "ppt", "pptx", "ps", "psd", "pub", "python", "ra", "rar", "raw", "rm", "rom", "rpm", "rss", "rtf", "sav", "sdf", "sitx", "sql", "srt", "svg", "swf", "sys", "tar", "tex", "tga", "thm", "tiff", "tmp", "torrent", "ttf", "txt", "uue", "vb", "vcd", "vcf", "vob", "wav", "wma", "wmv", "wpd", "wps", "wsf", "xhtml", "xlr", "xls", "xlsx", "xml", "yuv", "zip"];
        var hasext = false;
        $.each(imgext, function (index, value) {
            if (ext == value) {
                hasext = true;
                return false;
            }
        });
        return hasext;
    }
}

function fileAnnotationFunc(ftype, path, isvirt) {
    //顯示檔案說明與關鍵字
    $("#annotation").empty();
    $("#keyword").empty();

    $.ajax({
        context: this,
        url: homelink + "/WebService/getFileInfo.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            ftype: ftype,
            path: path,
            isvirt: isvirt,
            kind: "annotation"
        },
        success: function (msg) {
            if (msg == "") {
                $("#annotation").append($("<h2>").html('尚無檔案說明').attr("style", " text-align: left;"));
                $("#keyword").append($("<h6>").html('尚無關鍵字').attr("style", " text-align: left;"));

            } else {
                //必定只有一筆資料 直接輸出
                var data = JSON.parse(msg);

                $("#annotation").append($("<h2>").html("檔案說明：" + data[0]["annotation"]));
                var keyword = (data[0]["keyword"] != null ? data[0]["keyword"].split(',') : "");
                var keywordcontent = $("<h6>").html("關鍵字：").attr("style", " text-align: left;")
                $.each(keyword, function (index, value) {
                    if (index != 0) {
                        keywordcontent.append('，');
                    }
                    
                    var a = $("<a>").text(value)
                                    .attr('href', '#')
                                    .addClass('Keyword')
                                    .attr("path", path)
                                    .attr("ftype", ftype)
                                    .attr("isvirt",isvirt);
                    keywordcontent.append(a);
                });
                $("#keyword").append(keywordcontent);


            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    }).done(function () {
        rebind();
    });;
}

function fileCommentsFunc(ftype, path, isvirt) {
    //顯示檔案評論
    $("#comments").empty();
    $.ajax({
        context: this,
        url: homelink + "/WebService/getFileInfo.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            ftype: ftype,
            path: path,
            isvirt: isvirt,
            kind: "comments"
        },
        success: function (msg) {
            if (msg == "") {
                $("#comments").append($("<h2>").html('尚無檔案評論').attr("style", " text-align: left;"));

            } else {
                //必定只有一筆資料 直接輸出
                var data = JSON.parse(msg);
                var comments = $("<h2>").html("評論：").attr("style", " text-align: left;")
                $("#comments").append(comments);
                var ullist = $("<ul>").addClass('list-group');
                for (var i = 0; i <= data.length - 1; i++) {
                    ullist.append($("<li>").addClass('list-group-item').html("<p  style='font-size: 18px;color:blue;'>" + data[i]["CommentsAuthor"] + "</p>" + " Say:" + data[i]["Comments"]).append($("<span>").addClass('badge').text(data[i]["date"])));
                    //var div = $("<div>").append($("<h5>").html()).addClass('');


                }
                $("#comments").append($("<div>").attr("id",(data.length >5?"commentlist":"")).append(ullist));


            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    });
}

function show(val, type, path) {
    //顯示檔案說明或新增評論彈跳視窗
    if (type == "annotation") {
        $("#myModalLabel").text('檔案說明:' + path);
    } else if (type == "comments") {
        $("#myModalLabel").text('新增評論:' + path);
    }
    $("#myModal2").addClass("modal fade");
    $("#ifrmCal").attr("src", val);
    $("input[id$='btnopen']").click();
}

function closeModal() {
    //關閉彈跳視窗
    $('#myModal2').modal('hide');
}

function getFileExtension(filename) {
    //取得副檔名
    return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2).toLowerCase();
}

 