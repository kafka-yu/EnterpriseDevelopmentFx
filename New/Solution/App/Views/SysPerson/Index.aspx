<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysPerson>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    人员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divQuery">
        <div class="input_search">
            <%: Html.LabelFor(model => model.Name) %>：
            <input type='text' id='Name' />
        </div>
        <div class="input_search">
            <%: Html.LabelFor(model => model.MyName) %>：
            <input type='text' id='MyName' />
        </div>
          <a href="javascript:void(0);" onclick="flexiQuery()" class="easyui-linkbutton" data-options="iconCls:'icon-search'">
                查 询</a>
    </div>
 
    <br style="clear: both;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {

            $('#flexigridData').datagrid({
                title: '注意：删除人员前，请先通过修改功能移除其角色', //列表的标题
                iconCls: 'icon-site',
                //                fit: true, //列表自动适应宽度
                width: 'auto',
                height: 'auto',
                nowrap: false,
                striped: true,

                url: '/Admin/SysPerson/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',



                columns: [[


					{ field: 'Name', title: '<%: Html.LabelFor(model => model.Name) %>', width: 80 }
					, { field: 'MyName', title: '<%: Html.LabelFor(model => model.MyName) %>', width: 80 }
					, { field: 'Sex', title: '<%: Html.LabelFor(model => model.Sex) %>', width: 80 }
					, { field: 'SysDepartmentId', title: '<%: Html.LabelFor(model => model.SysDepartmentIdOld) %>', width: 80 }
            				, { field: 'SysRoleId', title: '角色', width: 220 }
					, { field: 'MobilePhoneNumber', title: '<%: Html.LabelFor(model => model.MobilePhoneNumber) %>', width: 80 }
					, { field: 'PhoneNumber', title: '<%: Html.LabelFor(model => model.PhoneNumber) %>', width: 80 }
                //					,{ field: 'Province', title:  '<%: Html.LabelFor(model => model.Province) %>', width: 37 }
                //					,{ field: 'City', title:  '<%: Html.LabelFor(model => model.City) %>', width: 37 }
                //					,{ field: 'Village', title:  '<%: Html.LabelFor(model => model.Village) %>', width: 37 }
                //					,{ field: 'Address', title:  '<%: Html.LabelFor(model => model.Address) %>', width: 37 }
                //					, { field: 'EmailAddress', title: '<%: Html.LabelFor(model => model.EmailAddress) %>', width: 80 }
                //					,{ field: 'Remark', title:  '<%: Html.LabelFor(model => model.Remark) %>', width: 37 }
					, { field: 'State', title: '<%: Html.LabelFor(model => model.State) %>', width: 80 }
					, { field: 'CreateTime', title: '<%: Html.LabelFor(model => model.CreateTime) %>', width: 80
                    , formatter: function (value, rec) {
                        if (value) {
                            return dateConvert(value);
                        }
                    }
					}
                //					, { field: 'CreatePerson', title: '<%: Html.LabelFor(model => model.CreatePerson) %>', width: 37 }
                //					, { field: 'UpdateTime', title: '<%: Html.LabelFor(model => model.UpdateTime) %>', width: 37
                //                    , formatter: function (value, rec) {
                //                        if (value) {
                //                            return dateConvert(value);
                //                        }
                //                    }
                //					}
                //					, { field: 'UpdatePerson', title: '<%: Html.LabelFor(model => model.UpdatePerson) %>', width: 37 }
                //					, { field: 'Version', title: '<%: Html.LabelFor(model => model.Version) %>', width: 37, hidden: true }				
                //                  , { display: '<%: Html.LabelFor(model => model.SysRoleId) %>', name: 'SysRoleId', width: 37, sortable: false, align: 'left' }

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
            //查询按钮样式
            $('.a4').mouseover(function () { this.style.background = 'rgb(113,4,4)'; }).mouseout(function () { this.style.background = 'rgb(131,0,0)'; });

        });

        //“查询”按钮，弹出查询框
        function flexiQuery() {

            //将查询条件按照分隔符拼接成字符串
            var search = "";
            $('#divQuery').find(":text,:selected,select,textarea,:hidden,:checked,:password").each(function () {
                search = search + this.id + "&" + this.value + "^";
            });
            //执行查询                        
            $('#flexigridData').datagrid('load', { search: search });

        };

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
                window.location.href = "/Admin/SysPerson/Details/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }
        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "/Admin/SysPerson/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "/Admin/SysPerson/Edit/" + arr[0].Id;

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
                    $.post("/Admin/SysPerson/Delete", { query: arr.join(",") }, function (res) {
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

    </script>
</asp:Content>
