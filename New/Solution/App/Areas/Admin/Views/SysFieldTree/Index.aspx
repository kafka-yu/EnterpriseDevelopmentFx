<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>数据字典</title>
    <script src="<%: Url.Content("~/Scripts/jquery.min.js") %>" type="text/javascript"></script>
       <link href="<%: Url.Content("~/Res/easyui/themes/icon.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%: Url.Content("~/Res/easyui/easyloader.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/JScriptCommon.js") %>" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            easyloader.locale = "zh_CN"; // 本地化设置
            //easyloader.theme = "gray"; // 设置主题
            using('tree', function () {
                $('#myTree').tree({
                    checkbox: true,
                    url: '/Admin/SysFieldTree/GetTree',
                    onClick: function (node) {
                        //  $(this).tree('toggle', node.target);
                    },
                    onBeforeLoad: function (node, param) {
                        if (node) {
                            param.parentid = node.id;
                        }
                    }
                });
            });
        });
        function getchecked() {
            //取得所有选中的节点，返回节点对象的集合
            var checkes = $("#myTree").tree("getChecked");
            var arr = new Array(0); //放置id和名称的数组           
            if (checkes != null) {
                if (checkes.length > 0) {
                    for (var i = 0; i < checkes.length; i++) {
                        arr.push(checkes[i].id);
                    }
                    arr.push("^"); //主键列和显示列的分割符 ^ 

                    for (var i = 0; i < checkes.length; i++) {
                        arr.push(checkes[i].text);
                    }
                    //先是用 ^ 分割开主键和名称列，再使用 & 分割开主键之间
                    returnParent(arr.join("&")); // 返回值
                } else {
                    alert("请至少选择一个");
                }
            }
        }
    </script>
</head>
<body>
    <fieldset style="margin: 10px 3px 0px 3px">
        <legend><a class="easyui-linkbutton" iconcls="icon-ok" onclick="getchecked()">选择</a>
        </legend>
        <ul style="margin-top: 6px;" id="myTree">
        </ul>
    </fieldset>
</body>
</html>

