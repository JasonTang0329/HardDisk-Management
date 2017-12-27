function addFunc() {
    //添加動作顯示操作區域
    $("#adddiv").show();
    $("#txtname").val('');
    $("#txtpath").val('');
    var clickid = $(this).attr("id");
    if (clickid == "addfolder") {
        $("#spantxt").text('資料夾名稱：');
        $("#divpath").hide();
        $("#addstate").val('folder');

    } else if (clickid == "addlink") {
        $("#spantxt").text('路徑名稱：');
        $("#divpath").show();
        $("#addstate").val('link');

    }

}

function saveAdd() {
    //儲存新增面板值
    var state = $("#addstate").val();
    if (state != "") {
        var selector = $(".liselect");
        var path = selector.attr('path');
        var file = $("#txtname").val();
        var link = $("#txtpath").val();
        var ftype = (state == "folder" ? "D" : state == "link" ? "L" : "");
        $.ajax({
            context: this,
            url: homelink + "/WebService/addFiler.ashx",
            type: "POST",
            dataType: 'text',
            data: {
                ftype: ftype,
                path: path,
                file: file,
                Link: link
            },
            success: function (msg) {
                if (msg == "") {
                    var findmenu = $(".liselect").parent().find('ul').eq(0);
                    var adddata = {
                        Type: ftype,
                        data: file,
                        isVirt: "true",
                        link: (link == "" ? "#" : link),
                        path: path + "\\" + file
                    };
                    if (findmenu.length != 0) {
                        ulappenddata(findmenu, adddata);

                    }

                    if (typeof (selector.attr("ctr")) == "undefined" || selector.attr("ctr") != "open") {
                        selector.parent().find('a').eq(0).click();
                    }

                    rebind();

                    bootbox.alert('新增成功');
                } else {
                    bootbox.alert(msg);

                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("Error" + thrownError);
                bootbox.alert('新增失敗');
            }
        });
        closeAddDiv();
    }
}

function closeAddDiv() {
    //關閉新增div
    $("#adddiv").hide();
    $("#txtname").val('');
    $("#txtpath").val('');
    $("#addstate").val('');

}