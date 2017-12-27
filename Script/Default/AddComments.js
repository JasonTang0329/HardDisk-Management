function AddComments() {
    $("#addcommentsarea").show();
    $("#txtcomments").val('');


}
function saveAddComments() {
    //儲存新增面板值
        var selector = $(".liselect");
        var path = selector.attr('path').replace(/\\/g, '/');
        var ftype = selector.attr("ftype");
        var isVirt = selector.attr('isvirt');
        var comments = $("#txtcomments").val();
        var file = selector.children('.fname').text().trim('');
        if (comments != "") {
            $.ajax({
                context: this,
                url: homelink + "/WebService/AddComments.ashx",
                type: "POST",
                dataType: 'text',
                data: {
                    ftype: ftype,
                    path: path,
                    isVirt: isVirt,
                    comments: comments
                },
                success: function (msg) {
                    if (msg != "") {
                        bootbox.alert('msg');

                    }
                    else {

                        fileCommentsFunc(ftype, path, isVirt);
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("Error" + thrownError);
                    bootbox.alert('新增失敗');
                }
            });
            closeAddComments();
        }
}
$(function () {
    $("#txtcomments").focus(function () {
        $("#commentsumit").show();
    });
    $("#txtcomments").keyup(function (evt) {
        var k = window.event.keyCode;
        var commit = $("#txtcomments").val();
        if (k == 13 && commit != "") {
            saveAddComments();
        }
    });
    $("#txtcomments").blur(function () {
        if ($(this).val() == "") {
            $("#commentsumit").hide();
        }
    });
})
function closeAddComments() {
    //關閉新增div
    //$("#addcommentsarea").hide();
    $("#commentsumit").hide();
    $("#txtcomments").val('');
    return false;
}