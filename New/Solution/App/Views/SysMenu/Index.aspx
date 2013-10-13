<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysMenu>" %>

<%@ Import Namespace="Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    菜单管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: Url.Content("~/Content/Default.css") %>" rel="stylesheet" type="text/css" />
    <style>
        .icon-site
        {
            background: url('/Admin/Images/direction.gif') no-repeat;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $('#flexigridData').treegrid({
                title: '注意：请先删除子节点，然后删除父节点',
                iconCls: 'icon-site',
                //                    url: '/Admin/SysMenu/GetAllMetadata',
                idField: 'Id',
                treeField: 'Name',
                rownumbers: true,
                toolbar: [],

                columns: [[
                    	{ field: 'Name', title: '名称', width: 215 }

					, { field: 'Url', title: '<%: Html.LabelFor(model => model.Url) %>', width: 215 }

					, { field: 'Sort', title: '<%: Html.LabelFor(model => model.Sort) %>', width: 84 }

					, { field: 'State', title: '<%: Html.LabelFor(model => model.State) %>', width: 84 }
                //					, { field: 'CreatePerson', title: '<%: Html.LabelFor(model => model.CreatePerson) %>', width: 84 }
					, { field: 'CreateTime', title: '<%: Html.LabelFor(model => model.CreateTime) %>', width: 84
                    , formatter: function (value, rec) {
                        if (value) {
                            return dateConvert(value);
                        }
                    }
					}
				    ]]
                     ,
                onBeforeLoad: function (row, param) {

                    if (row) {
                        $(this).treegrid('options').url = '/Admin/SysMenu/GetAllMetadata/' + row.Id;
                    } else {
                        $(this).treegrid('options').url = '/Admin/SysMenu/GetAllMetadata';
                    }
                }
            });

            //如果列表页出现在弹出框中，则只显示查询和选择按钮 
            var parent = window.dialogArguments; //获取父页面
            //异步获取按钮          
            if (parent == "undefined" || parent == null) {
                //首先获取iframe标签的id值
                var iframeid = window.parent.$('#tabs').tabs('getSelected').find('iframe').attr("id");

                //然后关闭AJAX相应的缓存
                $.ajaxSetup({
                    cache: false
                });

                //获取按钮值
                $.getJSON("/Admin/Home/GetToolbar", { id: iframeid }, function (data) {
                    if (data == null) {
                        return;
                    }
                    $('#flexigridData').datagrid("addToolbarItem", data);

                });

            } else {
                //添加选择按钮
                $('#flexigridData').datagrid("addToolbarItem", [{ "text": "选择", "iconCls": "icon-ok", handler: function () { flexiSelect(); } }]);
            }

        });

        function flexiSelect() {
            var node = $('#flexigridData').treegrid('getSelected');
            if (!node) {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
                return false;
            }
            var arr = new Array(0);
            arr.push(node.Id);
            arr.push("^"); //主键列和显示列的分割符 ^ 
            arr.push(node.Name);
            //主键列和显示列之间用 ^ 分割   每一项用 , 分割
            if (arr.length == 3) {//一条数据和多于一条
                returnParent(arr.join("&")); //每一项用 & 分割
            }
            return false;
        }
        //导航到查看详细的按钮
        function getView() {

            var arr = $('#flexigridData').treegrid('getSelected');

            if (arr) {
                window.location.href = "/Admin/SysMenu/Details/" + arr.Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }
        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "/Admin/SysMenu/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').treegrid('getSelected');

            if (arr) {
                window.location.href = "/Admin/SysMenu/Edit/" + arr.Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        };
        //删除的按钮
        function flexiDelete() {

            var node = $('#flexigridData').treegrid('getSelected');
            if (!node) {
                $.messager.alert('操作提示', '请选择数据!', 'warning');

            } else {
                $.messager.confirm('操作提示', "确认删除这1项吗？", function (r) {
                    if (r) {
                        $.post("/Admin/SysMenu/Delete", { query: node.Id }, function (res) {
                            if (res == "OK") {
                                remove();
                                $.messager.alert('操作提示', '删除成功!', 'info');
                            }
                            else {
                                if (res == "") {
                                    $.messager.alert('操作提示', '删除失败!请查看该数据与其他模块下的信息的关联，或联系管理员。', 'info');
                                }
                                else {
                                    $.messager.alert('操作提示', res, 'info');
                                }
                            }
                        });
                    }
                });
            }
            return false;
        };


        function remove() {
            var node = $('#flexigridData').treegrid('getSelected');
            if (node) {
                $('#flexigridData').treegrid('remove', node.Id);
            }
        }
    </script>
</asp:Content>
