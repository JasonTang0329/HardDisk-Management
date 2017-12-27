<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="true" CodeFile="ViewFile.aspx.cs" Inherits="ViewFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <style>
        video::-internal-media-controls-download-button {
            display: none;
        }

        video::-webkit-media-controls-enclosure {
            overflow: hidden;
        }

        video::-webkit-media-controls-panel {
            width: calc(100% + 30px); /* Adjust as needed */
        }
        div {
        text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceInForm" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            var ext = "<%= ext %>";

            if (ext == ".mp4") {
                $('video').bind('contextmenu', function () { return false; });
            }
        });
        var myApp;
        myApp = myApp || (function () {
            var pleaseWaitDiv = $('<div class="modal hide" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-header"><h1>Processing...</h1></div><div class="modal-body"><div class="progress progress-striped active"><div class="bar" style="width: 100%;"></div></div></div></div>');
            return {
                showPleaseWait: function () {
                    pleaseWaitDiv.modal();
                },
                hidePleaseWait: function () {
                    pleaseWaitDiv.modal('hide');
                },

            };
        })();
    </script>
   <div id="title" class="col-md-12"><h1><asp:Label ID="labtitle" runat="server" Text=""></asp:Label></h1></div>
    <video runat="server" id="movie" preload controls loop poster="poster.png" width="1280" height="720">
        <source src="" type="video/mp4" id="video" runat="server" />
        您的瀏覽器不支援HTML 5影片播放標籤 video 格式。
  Your browser doesn't support the  video  tag.
    </video>
    <img runat="server" src="" id="img" />
    <iframe id="docview" runat="server" ClientIDMode="Static" style="border:none; " width="100%" height=""  ></iframe>
    <script>
        $(document).ready(function () {
            frameResize();

            $(window).resize(function () {
                frameResize();
            });
        });
        function frameResize() {
            high = $(window).height() - 70;
            $("#docview").attr('height', high + 'px');


        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBody" runat="Server">
</asp:Content>

