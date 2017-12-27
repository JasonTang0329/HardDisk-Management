function meteringFunc(ftype, path, filename, isvirt) {
    //計算連結點擊次數
    $.ajax({
        context: this,
        url: homelink + "/WebService/Metering.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            ftype: ftype,
            path: path,
            filename: filename,
            isvirt: isvirt,
            kind: "path"
        },
        success: function (msg) {


        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    });
}

function meteringKeywordFunc(ftype, path,  isvirt, Keyword) {
    //計算連結點擊次數
    $.ajax({
        context: this,
        url: homelink + "/WebService/Metering.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            ftype: ftype,
            path: path,
            isvirt: isvirt,
            kind: "keyword",
            Keyword: Keyword
        },
        success: function (msg) {


        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    });
}
