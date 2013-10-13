<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysException>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    异常处理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divQuery">
        <div class="input_search">
            
                <%: Html.LabelFor(model => model.Message)%>：
           
                <input type='text' id='Message' />
           
        </div>
        <div class="input_search">
          
                <%: Html.LabelFor(model => model.CreateTime) %>：
          
                <input type="text" id="CreateTimeStart_Time" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'CreateTimeEnd_Time\');}'})" />
                到
                <input type="text" id="CreateTimeEnd_Time" onclick="WdatePicker({minDate:'#F{$dp.$D(\'CreateTimeStart_Time\');}'})" />
           
        </div>
        <a href="javascript:void(0);" onclick="flexiQuery()" class="easyui-linkbutton" data-options="iconCls:'icon-search'">
            查 询</a>
    </div>    <br style="clear: both;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%: Url.Content("~/Res/My97DatePicker/WdatePicker.js") %>" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            $('#flexigridData').datagrid({
                 //title: '异常处理',列表的标题
                iconCls: 'icon-site',
                width: 'auto',
                height: 'auto',
                nowrap: false,
                striped: true,
                
                url: 'SysException/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',

                toolbar: [

                  {
                      text: '详细',
                      iconCls: 'icon-details',
                      handler: function () {
                          return getView();
                      }
                  }],

                columns: [[

                    { field: 'Message', title: '<%: Html.LabelFor(model => model.Message) %>', width: 669 }
					//, { field: 'LeiXing', title: '<%: Html.LabelFor(model => model.LeiXing) %>', width: 89 }

                //					,{ field: 'Result', title:  '<%: Html.LabelFor(model => model.Result) %>', width: 169 }
                //					,{ field: 'Remark', title:  '<%: Html.LabelFor(model => model.Remark) %>', width: 169 }
                //					,{ field: 'State', title:  '<%: Html.LabelFor(model => model.State) %>', width: 169 }
					, { field: 'CreateTime', title: '<%: Html.LabelFor(model => model.CreateTime) %>', width: 89
                    , formatter: function (value, rec) {
                        if (value) {
                            return dateConvert(value);
                        }
                    }
					}
                //					,{ field: 'CreatePerson', title:  '<%: Html.LabelFor(model => model.CreatePerson) %>', width: 169 }
                ]],
                pagination: true,
                rownumbers: true

            });



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
                arr.push(rows[i].LeiXing);
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
                window.location.href = "/Admin/SysException/Details/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }
        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "/Admin/SysException/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "/Admin/SysException/Edit/" + arr[0].Id;

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
                    $.post("/Admin/SysException/Delete", { query: arr.join(",") }, function (res) {
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
