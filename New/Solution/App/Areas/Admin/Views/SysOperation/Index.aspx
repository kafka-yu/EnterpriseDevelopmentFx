<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysOperation>" %>

<!DOCTYPE html>
<html>
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>操作</title>
    <script src="<%: Url.Content("~/Scripts/jquery.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/JScriptIndex.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Res/easyui/jquery.easyui.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Res/easyui/locale/easyui-lang-zh_CN.js") %>" type="text/javascript"></script>
    <link href="<%: Url.Content("~/Res/easyui/themes/metro/easyui.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%: Url.Content("~/Res/easyui/themes/icon.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%: Url.Content("~/Content/StyleSheet.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {         
                 
                $('#flexigridData').datagrid({
               // title: '操作', 列表的标题
                iconCls: 'icon-site',
                width: 'auto',
                height: 'auto',
                nowrap: false,
                striped: true,
             
                url: 'SysOperation/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',

               

                columns: [[
                   
                    
					{ field: 'Name', title: '<%: Html.LabelFor(model => model.Name) %>', width: 135 }
					,{ field: 'Function', title:  '<%: Html.LabelFor(model => model.Function) %>', width: 135 }
					,{ field: 'Iconic', title:  '<%: Html.LabelFor(model => model.Iconic) %>', width: 135 }
					,{ field: 'Sort', title:  '<%: Html.LabelFor(model => model.Sort) %>', width: 135 }
//					,{ field: 'Remark', title:  '<%: Html.LabelFor(model => model.Remark) %>', width: 135 }
//					,{ field: 'State', title:  '<%: Html.LabelFor(model => model.State) %>', width: 135 }
					,{ field: 'CreateTime', title:  '<%: Html.LabelFor(model => model.CreateTime) %>', width: 135
                    , formatter: function (value, rec) {
                        if (value) {
                            return dateConvert(value);
                        } 
                    } 
}
//					,{ field: 'CreatePerson', title:  '<%: Html.LabelFor(model => model.CreatePerson) %>', width: 135 }					//, { display: '<%: Html.LabelFor(model => model.SysMenuId) %>', name: 'SysMenuId', width: 135, sortable: false, align: 'left' }

                ]],
                pagination: true,
                rownumbers: true

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

            $('#myTree').tree({
            url: '/Admin/SysMenuTree/GetTree',
            onClick: function (node) {                        
                if (node != null && node.id != null && node.id != "undefined") {
                    if (node.iconCls != null) {//&& node.iconCls == 'icon-ok'
                        $("#hidtreeid").val('');
                        $("#flexigridData").datagrid("load"); //根目录刷新  
                    } else {
                        $("#hidtreeid").val(node.id);
                        $("#flexigridData").datagrid("load", { id: node.id });//子节点刷新
                    }

                }                       
            },
            onBeforeLoad: function (node, param) {
                if (node) {
                    param.parentid = node.id;
                }
            }
        });
    });
      

        //“选择”按钮，在其他（与此页面有关联）的页面中，此页面以弹出框的形式出现，选择页面中的数据
        function flexiSelect() {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0) {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Id);
            }
            arr.push("^");
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Name);
            }

            //主键列和显示列之间用 ^ 分割   每一项用 , 分割
            if (arr.length > 0) {//一条数据和多于一条
                returnParent(arr.join("&")); //每一项用 & 分割
            }
        }
        //导航到查看详细的按钮
        function getView() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "/Admin/SysOperation/Details/" + arr[0].Id;
               
            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }
        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "/Admin/SysOperation/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "/Admin/SysOperation/Edit/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;

        };
        //删除的按钮
        function flexiDelete() {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0) {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Id);
            }

            $.messager.confirm('操作提示', "确认删除这 " + arr.length + " 项吗？", function (r) {
                if (r) {
                    $.post("/Admin/SysOperation/Delete", { query: arr.join(",") }, function (res) {
                        if (res == "OK") {
                           //移除删除的数据
                            $("#flexigridData").datagrid("reload");
                            $("#flexigridData").datagrid("clearSelections");
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

        };

        //设置
        function setSysMenu() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "/Admin/SysOperation/SetSysMenu/" + arr[0].Id;
               
            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }
	 
    </script>
</head>
<body class="easyui-layout">
    <div region="west" split="true" title="菜单" style="width: 180px;">
        <ul id="myTree">
        </ul>
    </div>
    <div id="content" region="center" fit="true" title="操作" >
        <table id="flexigridData">
        </table>
    </div>
   
    <input id="hidtreeid" type="hidden" />
    <input id="SysMenuIdOld" type="hidden" />
</body>
</html>

