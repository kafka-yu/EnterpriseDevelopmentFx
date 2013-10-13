<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysRole>" %>

<%@ Import Namespace="Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    菜单
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {

            $('#flexigridData').treegrid({
                title: ' 当前角色是：<%= ViewData["myname"] %>  ',
                iconCls: 'icon-site',
                url: '/Admin/SysMenu/GetAllMetadata2/' + $("#SysRoleId").val(),
                idField: 'Id',
                treeField: 'Name',
                rownumbers: true,

                toolbar: [
                    {
                        text: '保存',
                        iconCls: 'icon-save',
                        handler: function () {
                            return getSave();
                        }
                    }, {
                        text: '全选',
                        iconCls: 'icon-ok',
                        handler: function () {
                            return flexiCreate();
                        }
                    }, {
                        text: '全不选',
                        iconCls: 'icon-remove',
                        handler: function () {
                            return flexiDelete();
                        }
                    }],

                columns: [[

                    	{ field: 'Name', title: '菜单', width: 205
                                        , formatter: function (value, rec) {
                                            if (value) {
                                                return '<input id="' + rec.Id + '" type="checkbox">' + (value);
                                            }
                                        }
                    	}
					, { field: 'isCheck', title: '操作', width: 466, formatter: function (value, rec) {
					    if (value) {
					        var index = value.split(","); //分割符 , 的位置
					        if (index[0] == null || index[0] == "undefined" || index[0].length < 1) {
					            return;
					        }
					        var content = ""; //需要添加到check中的内容 
					        for (var i = 0; i < index.length; i++) {
					            var view = index[i].split('^'); //显示值
					            if (view != null) {
					                content += '<input id="' + rec.Id + '^' + view[0] + '" type="checkbox">' + view[1];
					            }
					        }
					        return content;
					    }
					}
					}
				    ]],
                onLoadSuccess: function (row, data) {
                    if (data) {
                        $.ajaxSetup({
                            cache: false //关闭AJAX相应的缓存
                        });
                        $.getJSON('/Admin/SysMenu/GetAllMetadata23/' + $("#SysRoleId").val(), function (checks) {
                            $.each(checks, function (i, item) {
                                var c = document.getElementById(item);
                                c.checked = true;
                            });
                        });

                    }
                }

            });


        });


        //保存
        function getSave() {
            var datas = '';
            $("input[type='checkbox']").each(function () {
                if ($(this).is(":checked"))
                    datas += ',' + $(this).attr('id');
            });
            $.post("/Admin/../SysRole/Save", { id: $("#SysRoleId").val(), ids: datas }, function (res) {
                if (res == "OK") {
                     
                    if ($.messager) {
                        $.messager.defaults.ok = '继续';
                        $.messager.defaults.cancel = '关闭';

                        $.messager.confirm('操作提示', '保存成功!', function (r) {
                            if (!r) {
                                window.parent.closeMyWindow();
                            }
                        });
                    }
                     
                }
                else {
                    if (res == "") {
                        $.messager.alert('操作提示', '保存失败!请联系管理员。', 'info');
                    }
                    else {
                        $.messager.alert('操作提示', res, 'info');
                    }
                }
            });



            return false;
        }
        //全选
        function flexiCreate() {

            $("input[type='checkbox']").each(function () {
                $(this).attr("checked", true);
            });

            return false;
        }
        //全不选
        function flexiModify() {

            window.location.href = "/Admin/../SysRole";



            return false;
        };
        //返回
        function flexiDelete() {
            $("input[type='checkbox']").each(function () {
                $(this).attr("checked", false);
            });
            return false;
        };
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="SysRoleId" value="<%= ViewData["myid"] %>" />
</asp:Content>
