<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceInForm" runat="Server">
    <script type="text/javascript" src="Script/Default/AddDiv.js"></script>
    <script type="text/javascript" src="Script/Default/Donload.js"></script>
    <script type="text/javascript" src="Script/Default/FileInfo.js"></script>
    <script type="text/javascript" src="Script/Default/OpenLink.js"></script>
    <script type="text/javascript" src="Script/Default/FoderOepn.js"></script>
    <script type="text/javascript" src="Script/Default/Metering.js"></script>
    <script type="text/javascript" src="Script/Default/WindowsResize.js"></script>
    <script type="text/javascript" src="Script/Default/ViewFile.js"></script>
    <script type="text/javascript" src="Script/Default/SearchKeyword.js"></script>
    <script type="text/javascript" src="Script/Default/AddComments.js"></script>
    <script type="text/javascript" src="Script/Default/ShortUri.js"></script>
    <script type="text/javascript">
        var homelink = "<%= System.Configuration.ConfigurationManager.AppSettings["HomePageUrl"] %>";
        var disallowDonloadExtArr = "<%= System.Configuration.ConfigurationManager.AppSettings["filterFileExt"] %>";//不允許下載的附檔名

        $(document).ready(function () {
            //綁訂Click event
            $('.open').on("click", openFunc);
            $('.folder').on("click", clickFunc);
            $('.add').on("click", addFunc);
            $('.Keyword').on("click", KeywordClick);
            $('.opendonload').on("click", opendonload);
            $('.openlink').on("click", openlink);
            $('.copytocilp').on("click", copytocilp);
            $('.addcomment').on("click", AddComments);
            $('.VirtDelete').on("click", VirtDelete);

            $(window).resize(function () {
                //當視窗SIZE改變，調整子DIV與搜尋列
                $("#searchresult").width($("#txt_search").width());
                $("#searchbox .std").width($("#txt_search").width() - 2);
                $("#searchbox .line").width($("#txt_search").width() - 2);
                $("#searchbox .hover").width($("#txt_search").width() - 2);
                divResize();
            });
        });

        function rebind() {
            //重新綁訂Click event
            $('.open').off("click", openFunc);
            $('.folder').off("click", clickFunc);
            $('.add').off("click", addFunc);
            $('.Keyword').off("click", KeywordClick);
            $('.opendonload').off("click", opendonload);
            $('.openlink').off("click", openlink);
            $('.copytocilp').off("click", copytocilp);
            $('.addcomment').off("click", AddComments);
            $('.refkeyword').off("click", refKeywordClick);
            $('.VirtDelete').off("click", VirtDelete);

            $('.open').on("click", openFunc);
            $('.folder').on("click", clickFunc);
            $('.add').on("click", addFunc);
            $('.Keyword').on("click", KeywordClick);
            $('.opendonload').on("click", opendonload);
            $('.openlink').on("click", openlink);
            $('.copytocilp').on("click", copytocilp);
            $('.addcomment').on("click", AddComments);
            $('.refkeyword').on("click", refKeywordClick);
            $('.VirtDelete').on("click", VirtDelete);

        }
        $(window).load(function () {
            var root = decodeURIComponent('<%=rootpath%>');
            var Suri = '<%=HttpUtility.JavaScriptStringEncode(ShortURI)%>';
            var path = "";

            if (Suri != "") {
                $.each(root.split('\\'), function (index, value) {
                    if (value != "") {
                        path += (path == "" ? "\\\\\\\\" : "\\\\") + value;
                    }
                });
                Suri = decodeURIComponent(Suri).replace(root, "");

                var Surilength = Suri.split('\\').length - 1;
                $("#foderRoot").unbind("click");
                $('#foderRoot').click({
                    callback: function () {
                        openTree(path, Suri.split('\\'), 0, Surilength);
                    }
                }, clickFunc);
                $('#foderRoot').click();
                $('.open')[0].click();

            } else {
                $('#foderRoot').click();
                $('.open')[0].click();

            }


            divResize();


        });
    </script>
    <style type="text/css">
        body {
            overflow: hidden;
        }
    </style>

    <div id="main">
        <div id="header" class="col-xs-12 well" style="margin-bottom: 0px;">
            <div>
                <h1 style="display: inline">TM</h1>
                <h2 style="display: inline">&nbsp&nbspKnowledge Management</h2>
            </div>
        </div>
        <div id="menu" class="col-xs-12  col-sm-6  col-md-4 well" style="padding: 0px;">
            <ul class="nav nav-tabs">
                <li class="active"><a id="hometab" data-toggle="tab" href="#home">目錄</a></li>
                <li><a id="searchtab" data-toggle="tab" href="#searchbox">搜尋</a></li>
            </ul>
            <div class="tab-content">
                <div id="home" class="tab-pane fade in active" onselectstart="return false">
                    <div id="addicon" class="text-right">
                        <a id="addfolder" href="#" class="add" onclick="return false;">
                            <img src="App_Themes/layout/icon/add-folder.png" alt="新增資料夾" title="新增資料夾" onclick="return false;" /></a>
                        <a id="addlink" href="#" class="add" onclick="return false;">
                            <img src="App_Themes/layout/icon/add-link.png" alt="新增超連結" title="新增超連結" onclick="return false;" /></a>

                    </div>
                    <div id="adddiv" style="display: none">
                        <input type="hidden" id="addstate" value="">
                        <div class="form-group">
                            <label id="spantxt" for="txtname"></label>
                            <input id="txtname" class="form-control" type="text">
                        </div>
                        <div id="divpath" class="form-group">
                            <label for="txtpath">路徑(Path)：</label>
                            <input id="txtpath" class="form-control" type="text">
                        </div>
                        <div class="text-right">

                            <input id="btnSubmit" type="button" class="btn-md btn-success" value="新增" onclick="saveAdd();" style="border: none" />
                            <input id="btnCancel" type="button" class="btn-md btn-danger" value="取消" onclick="closeAddDiv();" style="border: none" />
                        </div>

                    </div>
                    <div style="width:90%">
                        <ul class="menu">
                            <li>
                                <a class="folder" id="foderRoot" onclick="return false;" href="#">
                                    <span class="glyphicon glyphicon-triangle-right" aria-hidden="true"></span>
                                </a>
                                <a class="open filer" ftype="D" path='<%=HttpUtility.UrlDecode(rootpath) %>' isvirt="false" onclick="return false;" href="#">
                                    <span class="glyphicon glyphicon-folder-close" aria-hidden="true"></span>
                                    <span class="fname">&nbsp<%= System.IO.Path.GetFileName(HttpUtility.UrlDecode(rootpath)) %></span>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <h6><small><span class="glyphicon glyphicon-copyright-mark" aria-hidden="true"></span>由於版權問題，Mp4檔案暫不提供下載</small></h6>

                </div>
                <div id="searchbox" class="tab-pane fade">
                    <div id="search" class="form-group">
                        <label for="txt_search">搜尋</label>
                        <div class="form-inline">
                            <input class="form-control input-lg" id="txt_search" value="" type="text">
                            <div id="searchresult" style="display: none;"></div>
                            <input id="searchsubmit" type="button" class="btn btn-primary" onclick="SubmitSearch();" value="搜尋" />
                        </div>
                    </div>
                    <div id="refKeyword" class="text-left">
                    </div>
                    <div id="searchlists" class="tab-content">
                    </div>
                    <div id="pagination" class="text-center">
                    </div>
                </div>
            </div>
        </div>

        <div id="content" class="col-xs-12 col-sm-6 col-md-8 well">

            <div class="text-right">
                <div style="display: inline">
                    <input id="btnView" type="button" class="btn-lg btn-info" value="檔案預覽" style="border: none" />
                    <input id="btnDonload" type="button" class="btn-lg btn-info" value="檔案下載" style="border: none" />
                    <input id="btnOpen" type="button" class="btn-lg btn-primary" value="開啟連結" style="border: none" />
                    <input id="btnURL" type="button" class="btn-lg btn-primary" value="取得連結" style="border: none" />

                    <input id="btnaddannotation" type="button" data-toggle="modal" data-target="#myModal2" class="btn-lg btn-success" value="檔案說明" style="border: none" />
                </div>
            </div>
            <div class="text-left">
                <h1 id="H1Title"></h1>
            </div>

            <div id="filedisplay"></div>
            <div id="annotation" class="annotation"></div>
            <div id="keyword"></div>
            <div id="comments"></div>
            <div id="addcomments">
                <div class="form-group">
                    <div>
                        <input id="txtcomments" class="form-control input-lg" type="text" placeholder="新增評論">
                        <div id="commentsumit" class="form-inline" style="display: none;">
                            <a id="commentSubmit" onclick="saveAddComments();" href="#" title="新增" class="comdes"><span style="font-size: 1.2em;" class="glyphicon glyphicon-ok"></span></a>
                            <a id="commentCancel" onclick="closeAddComments();" href="#" title="取消" class="comdes"><span style="font-size: 1.2em;" class="glyphicon glyphicon-remove"></span></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 彈跳Modal -->
    <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel"></h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="ifrmCal" width="100%" class="frmClass" marginwidth="0" marginheight="0" style="overflow-x: hidden" scrolling="no" frameborder="0"></iframe>

                </div>
            </div>
        </div>
    </div>
    <!-- 彈跳Modal -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBody" runat="Server">
</asp:Content>

