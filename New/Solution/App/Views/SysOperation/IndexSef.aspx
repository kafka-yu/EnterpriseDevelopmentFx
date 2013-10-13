<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysOperation>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divQuery" title="查询列表" class="easyui-dialog" closed="true" modal="false"
        iconcls="icon-search">
         
            <div class="input">
                <div class="editor-label-search">
                    <%: Html.LabelFor(model => model.Iconic) %>
                </div>
                <div class="editor-field-search">
                    <input type='text' id='IconicEnd_String'/>
                </div>
            </div>
            <div class="input">
                <div class="editor-label-search">
                    <%: Html.LabelFor(model => model.State) %>
                </div>
                <div class="editor-field-search">
                    <%=Html.DropDownList("StateDDL_String", Models.SysFieldModels.GetSysField("SysOperation","State"),"请选择",new { id = "StateDDL_String" })%>
                </div>
            </div> 
            <br style="clear: both;" />
            <div class="editor-label-search">
                <%: Html.LabelFor(model => model.CreateTime) %>
            </div>
            <div class="editor-field-to">
                <input type="text" id="CreateTimeStart_Time" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'CreateTimeEnd_Time\');}'})"  />
                到
                <input type="text" id="CreateTimeEnd_Time" onclick="WdatePicker({minDate:'#F{$dp.$D(\'CreateTimeStart_Time\');}'})" />
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
                               <script src="<%: Url.Content("~/Res/My97DatePicker/WdatePicker.js") %>" type="text/javascript"></script>
                                 
    <script type="text/javascript" language="javascript">
        $(function () {

            $('#flexigridData').datagrid({
                title: '操作', //列表的标题
                iconCls: 'icon-site',
                width: 'auto',
                height: 'auto',
                nowrap: false,
                striped: true,
                collapsible: true,
                url: 'SysOperation/GetData', //获取数据的url
                sortName: '^m_Id^',
                sortOrder: 'desc',
                idField: '^m_Id^',

                toolbar: [
                 {
                     text: '查询',
                     iconCls: 'icon-search',
                     handler: function () {
                         flexiQuery();
                     }
                 }],

                columns: [[
                   
                    
					{ field: 'Name', title: '<%: Html.LabelFor(model => model.Name) %>', width: 135 }
					,{ field: 'Function', title:  '<%: Html.LabelFor(model => model.Function) %>', width: 135 }
					,{ field: 'Iconic', title:  '<%: Html.LabelFor(model => model.Iconic) %>', width: 135 }
					,{ field: 'Sort', title:  '<%: Html.LabelFor(model => model.Sort) %>', width: 135 }
					,{ field: 'Remark', title:  '<%: Html.LabelFor(model => model.Remark) %>', width: 135 }
					,{ field: 'State', title:  '<%: Html.LabelFor(model => model.State) %>', width: 135 }
					,{ field: 'CreateTime', title:  '<%: Html.LabelFor(model => model.CreateTime) %>', width: 135
                    , formatter: function (value, rec) {
                        if (value) {
                            return dateConvert(value);
                        } 
                    } 
}
					,{ field: 'CreatePerson', title:  '<%: Html.LabelFor(model => model.CreatePerson) %>', width: 135 }					//, { display: '<%: Html.LabelFor(model => model.SysMenuId) %>', name: 'SysMenuId', width: 135, sortable: false, align: 'left' }

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

        });

        //“查询”按钮，弹出查询框
        function flexiQuery() {
            $('#divQuery').dialog({          
                buttons: [{
                    text: '查询',
                    iconCls: 'icon-ok',
                    handler: function () {
                       //将查询条件按照分隔符拼接成字符串
                        var search = "";
                        $('#divQuery').find(":text,:selected,select,textarea,:hidden,:checked,:password").each(function () {
                            search = search + this.id + "&" + this.value + "^";
                        });
                        //执行查询                        
                        $('#flexigridData').datagrid('reload', { search: search });
                    }
                },
                     {
                         text: '取消',
                         iconCls: 'icon-cancel',
                         handler: function () {
                             $('#divQuery').dialog("close");
                         }
                     }]
            });
            $('#divQuery').dialog("open");
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
                arr.push(rows[i].^m_Id^);
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
                window.location.href = "/Admin/SysOperation/Details/" + arr[0].^m_Id^;
               
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
                window.location.href = "/Admin/SysOperation/Edit/" + arr[0].^m_Id^;

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
                arr.push(rows[i].^m_Id^);
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

    </script>
</asp:Content>

