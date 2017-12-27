<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="true" CodeFile="EditPage.aspx.cs" Inherits="EditPage" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<%--    <script src="ckeditor/ckeditor.js"></script>--%>
    <style type="text/css">
        #body {
            width: 976px;
            margin-top: 7px;
            margin-right: 0;
            margin-left: 0;
            position: relative;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceInForm" runat="Server">
    <h3>檔案說明：</h3><br />
 
    <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server">
    </CKEditor:CKEditorControl><br />
    <h6><small>關鍵字(請以,分隔)：</small><asp:TextBox ID="txtkeyword" runat="server"></asp:TextBox></h6>
    <script type="text/javascript">
        $(window).load(function () {
            parent.modelresize();
        });
    </script>
    <br />
    <asp:Label ID="labmodify" runat="server" Text=""></asp:Label>
        <br />
    <div class="col-md-4 center-block">
        <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
    </div >
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBody" runat="Server">
</asp:Content>

