$(function () {
    $("#txt_search").bind('input propertychange', function () {
         ChangeCoords(); //控制查詢結果div坐標
        //var k = window.event ? evt.keyCode : evt.which;
        if ($("#txt_search").val().trim() != "") { //&& k != 38 && k != 40 && k != 13
            $.ajax({
                context: this,
                url: homelink + "/WebService/getKeyword.ashx",
                type: "POST",
                dataType: 'text',
                data: {
                    txt: $("#txt_search").val(),
                    mode: "Tip"
                },
                success: function (data) {
                    if (data == "") {
                        clearsearchresult();
                    } else {
                        var objData = JSON.parse(data);
                        if (objData.length > 0) {
                            var layer = "";
                            layer = "<table id='aa'>";
                            $.each(objData, function (idx, item) {
                                layer += "<tr class='line'><td class='std'>" + item.keyword + "</td></tr>";
                            });
                            layer += "</table>";
                            //將結果添加到div中
                            $("#searchresult").empty();
                            $("#searchresult").append(layer);
                            $(".line:first").addClass("hover");
                            $("#searchresult").css("display", "");
                            //鼠標移動事件
                            $(".line").hover(function () {
                                $(".line").removeClass("hover");
                                $(this).addClass("hover");
                            }, function () {
                                $(this).removeClass("hover");
                                //$("#searchresult").css("display", "none");
                            });
                            //鼠標點擊事件
                            $(".line").click(function () {
                                $("#txt_search").val($(this).text());
                                $("#searchresult").css("display", "none");
                            });
                        } else {
                            clearsearchresult();
                        }
                    }

                    function clearsearchresult() {
                        $("#searchresult").empty();
                        $("#searchresult").css("display", "none");

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("Error" + thrownError);
                    //bootbox.alert('新增失敗');
                }
            });

        //} else if (k == 38) { //上箭頭
        //    $('#aa tr.hover').prev().addClass("hover");
        //    $('#aa tr.hover').next().removeClass("hover");
        //    $('#txt_search').val($('#aa tr.hover').text());
        //} else if (k == 40) { //下箭頭
        //    $('#aa tr.hover').next().addClass("hover");
        //    $('#aa tr.hover').prev().removeClass("hover");
        //    $('#txt_search').val($('#aa tr.hover').text());
        //} else if (k == 13) { //回車
        //    console.log($('#aa tr.hover').text());
        //    $('#txt_search').val($('#aa tr.hover').text());
        //    SubmitSearch();
        //    $("#searchresult").empty();
        //    $("#searchresult").hide;
         } else {
            $("#searchresult").empty();
            $("#searchresult").hide();
        }
    });
 
    $("#searchresult").bind("mouseleave", function () {
        $("#searchresult").empty();
        $("#searchresult").css("display", "none");
    });
});
//設置查詢結果div坐標
function ChangeCoords() {
    // var left = $("#txt_search")[0].offsetLeft; //獲取距離最左端的距離，像素，整型
    // var top = $("#txt_search")[0].offsetTop + 26; //獲取距離最頂端的距離，像素，整型（20為搜索輸入框的高度）
    var left = $("#txt_search").position().left; //獲取距離最左端的距離，像素，整型
    var top = $("#txt_search").position().top + 45; //獲取距離最頂端的距離，像素，整型（20為搜索輸入框的高度）
    $("#searchresult").css("left", left + "px"); //重新定義CSS屬性
    $("#searchresult").css("top", top + "px"); //同上
}

function KeywordClick() {
    var path = $(this).attr('path').replace(/\\/g, '/');
    var isvirt = $(this).attr('isvirt');
    var ftype = $(this).attr("ftype");
    var Keyword = $(this).text();

    SearchKeyword($(this).text());
    meteringKeywordFunc(ftype, path, isvirt, Keyword);
    $("#txt_search").val($(this).text());
    $("#searchtab").trigger("click");;

}

function SubmitSearch() {
    SearchKeyword($("#txt_search").val());
}

function SearchKeyword(Searchword) {
    $("#searchlists").empty();
    $("#pagination").empty();
    $("#refKeyword").empty();

    if (Searchword.trim() == "") {
        $("#searchlists").html("<p><em><mark>請輸入查詢內容.</mark></em></p>");
    } else {
        $.ajax({
            context: this,
            url: homelink + "/WebService/getKeyword.ashx",
            type: "POST",
            dataType: 'text',
            data: {
                txt: Searchword,
                mode: "GetPath"
            },
            success: function (msg) {
                if (msg != "") {
                    var data = JSON.parse(msg);
                    makeSearchListWithPage(data);
                    makeSuggestKeywordWithMetering(Searchword);

                    rebind();
                } else {
                    $("#searchlists").html("<p class='lead text-center'>查無資料.</p>");

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("Error" + thrownError);
            }
        });
    }
}
var num = 1;
var total
var paging = 10; //分頁數

function makeSearchListWithPage(data) {
    var divlist = [];
    var datacount = data.length;
    var datapageconut = Math.ceil(datacount / paging);
    total = datapageconut;
    var tabli = [];
    var innerdiv = [];

    tabli.push($("<li>")
        .append($("<a>").attr("id", "atop")
            .attr("href", "#")
            .text("<<")));
    tabli.push($("<li>")
        .append($("<a>").attr("id", "aprev")
            .attr("href", "#")
            .text("<")));
    for (var i = 1; i <= datapageconut; i++) {

        //li tab
        var li = $("<li>").addClass(i == 1 ? "active" : "")
            .append($("<a>").attr("id", "a" + i)
                .attr("data-toggle", "pill")
                .attr("href", "#tab" + i)
                .text(i));
        tabli.push(li)
        //li tab
        var indiv = $("<div>").attr("id", "tab" + i)
            .addClass("tab-pane fade  " + (i == 1 ? " in active" : ""))
            .append(makeSearchList(i, data));
        innerdiv.push(indiv);
    }
    tabli.push($("<li>").append($("<a>").attr("id", "anext")
        .attr("href", "#")
        .text(">")));
    tabli.push($("<li>").append($("<a>").attr("id", "adown")
        .attr("href", "#")
        .text(">>")));
    $("#pagination").append($("<ul>").addClass("pagination").attr("id", "pagin").append(tabli));
    $("#searchlists").append(innerdiv);

    TabClickEvent();

    function makeSearchList(i, data) {
        var bqlist = [];
        for (var j = ((i - 1) * paging) ; j < (paging * i) ; j++) {
            if (j > data.length - 1) {
                break;
            }
            var bq = $("<blockquote>");
            bqappenddata(bq, data[j]);
            bqlist.push(bq);

        }
        return bqlist;
    }

    function bqappenddata(bq, array) {

        //產生新的li連結
        var footers = $("<footer>").append($("<cite>").addClass('Source Title').html(array.Annotation));
        var span = $("<span>");
        var a = $("<a>").append($("<span>").append(" " + array.file_name).addClass("fname"))
            .attr("href", array.link)
            .attr("path", array.file_path + "\\" + array.file_name)
            .attr("onclick", "return false;")
            .attr("ftype", array.file_type)
            .attr("isVirt", array.isVirt)
            .addClass("open file");

        //檔案夾
        if (array.file_type == "D") {
            span.addClass("glyphicon glyphicon-folder-close");
        }
        //檔案
        if (array.file_type == "F") {
            span.addClass("glyphicon glyphicon-open-file");
        }
        //連結
        if (array.file_type == "L") {
            span.addClass("glyphicon glyphicon-link");
        }
        bq.append(a.prepend(span)).append($("<small>").html(array.file_path)).append(footers);
    }
}

function TabClickEvent() {
    function AddActive(tab) {
        $('#a' + tab).click();
        num = tab;
    }
    $('#pagin  a').click(function (e) {
        var str = $(this).attr('id').replace("a", "");
        if (!isNaN(str)) {
            num = str;
        } else {
 
            switch (str) {
                case "top":
                    if (num != 1) {
                        AddActive(1);
                    }
                    break;
                case "prev":
                    if (num != 1) {
                        AddActive((parseInt(num) - 1));
                    }
                    break;
                case "next":

                    if (num != total) {
                        AddActive((parseInt(num) + 1));
                    }
                    break;
                case "down":
                    if (num != total) {
                        AddActive((total));
                    }
                    break;
                default:
            }

        }
    });




}

function makeSuggestKeywordWithMetering(Keyword) {
    //關聯字詞建議(使用結巴斷詞)
    $.ajax({
        context: this,
        url: homelink + "/WebService/getKeyword.ashx",
        type: "POST",
        dataType: 'text',
        data: {
            txt: Keyword,
            mode: "GetSuggestKeyword"
        },
        success: function (msg) {
            if (msg != "") {
                var data = JSON.parse(msg);
                $("#refKeyword").text("REF:");
                $.each(data, function (index, value) {
                    if (index < 5) {
                        $("#refKeyword").append((index != 0 ? "、" : "")).append($("<a>").addClass("refkeyword")
                            .attr("onclick", "return false;")
                            .attr("href", "#")
                            .append($("<mark>").text("#" + value.keyword)));
                    } else {
                        return false;
                    }
                });
                $('.refkeyword').on("click", refKeywordClick);

            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error" + thrownError);
        }
    });
}

function refKeywordClick() {
    var RefText = $(this).text().substring(1, $(this).text().length);
    SearchKeyword(RefText);
    $("#txt_search").val(RefText);
    $("#searchtab").trigger("click");

}