<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>管理信息系统</title>
    <script src="<%: Url.Content("~/Scripts/jquery.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Res/easyui/jquery.easyui.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Res/easyui/locale/easyui-lang-zh_CN.js") %>" type="text/javascript"></script>
    <link href="<%: Url.Content("~/Res/easyui/themes/icon.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%: Url.Content("~/Res/easyui/themes/default/easyui.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%: Url.Content("~/Content/default.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">


        $(function () {

            tabCloseEven();
            addTab("我的工作台", "http://www.NkjSoft.com/blog", "tu1112", false);
            $('ul li a').click(function () {
                var tabTitle = $(this).text();
                var url = $(this).attr("rel"); //获取地址
                var id = $(this).attr("id"); //获取id
                var icon = $(this).attr("icon"); //获取图标
                if (icon == "") {
                    icon = "icon-save";
                }
                addTab(tabTitle, url, icon, true, id);

            });
            $('#loginOut').click(function () {

                $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {

                    if (r) {
                        location.href = '/Account/LogOff';
                    }
                });
            });  
             slide();
            $('.ChangePassword').click(function () {
                showMyWindow("修改密码", "Account/ChangePassword");

            });
         
        })
        //右下角弹出框;

        function slide() {

            var dt = '<%= ViewData["Show"]%>';
            jQuery.messager.show({
                height: 170,
                title: '当前登录情况:',

                msg: dt + '温馨提示：为了账户的安全，如果上面的登录情况不正常，建议您马上<a href="javascript:void(0);" style="color:red;" class="ChangePassword" >修改密码</a>',

                timeout: 8000,

                showType: 'slide'

            });

        }

        function addTab(subtitle, url, icon, closable, id) {

            if (!$('#tabs').tabs('exists', subtitle)) {
                $('#tabs').tabs('add', {
                    title: subtitle,
                    content: createFrame(url, id),
                    closable: closable
                    , icon: icon
                });
            } else {
                $('#tabs').tabs('select', subtitle);

            }
            tabClose();
        }

        function createFrame(url, id) {
            var s = '<iframe id="' + id + '" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:99%;overflow-y: auto; "></iframe>';
            return s;
        }
        function tabClose() {
            /*双击关闭TAB选项卡*/
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").text();
                $('#tabs').tabs('close', subtitle);
            })
            /*为选项卡绑定右键*/
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").text();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        //绑定右键菜单事件
        function tabCloseEven() {
            //刷新
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                var id = $(currTab.panel('options').content).attr('id'); ; //获取id

                $('#tabs').tabs('update', {
                    tab: currTab,
                    options: {
                        content: createFrame(url, id)
                    }
                })
            })
            //关闭
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                $('#tabs').tabs('close', currtab_title);
            })


            //退出
            $("#mm-exit").click(function () {
                $('#mm').menu('hide');
            })
        }
        function showMyWindow(title, href, width, height, modal, minimizable, maximizable) {

            $('#myWindow').window({
                title: title,
                width: width === undefined ? 600 : width,
                height: height === undefined ? 450 : height,
                content: '<iframe scrolling="yes" frameborder="0"  src="' + href + '" style="width:100%;height:98%;"></iframe>',
                //        href: href === undefined ? null : href, 
                modal: modal === undefined ? true : modal,
                minimizable: minimizable === undefined ? false : minimizable,
                maximizable: maximizable === undefined ? false : maximizable,
                shadow: false,
                cache: false,
                closed: false,
                collapsible: false,
                resizable: false,
                loadingMessage: '正在加载数据，请稍等片刻......'
            });

        }
    </script>
    <style type="text/css">
        body
        {
            font-family: 微软雅黑,新宋体;
        }
        a
        {
            color: Black;
            text-decoration: none;
        }
        .easyui-tree li
        {
            margin: 5px 0px 0px 0px;
            padding: 1px;
        }
        
        #mainlogo
        {
            position: absolute;
            top: 0px;
            left: 0px;
            width: 575px;
            height: 72px;
        }
        #center
        {
            padding-left: 575px;
            padding-right: 425px;
        }
        #mainctrl
        {
            position: absolute;
            top: 0px;
            right: 0px;
            height: 72px;
            width: 425px;
        }
        
        
        .wel
        {
            height: 30px;
            line-height: 30px;
            color: #FFFFFF;
            font-size: 14px;
            text-align: right;
            padding-right: 5px;
        }
        .ctr
        {
            vertical-align: middle;
            margin-top: 18px;
            height: 22px;
            text-align: right;
            float: right;
        }
        
        .ctr li
        {
            float: left;
            list-style: none;
        }
        .zi
        {
            /*在1.4版本中修改*/
            background-image: url(/images/beijing.gif);
            background-repeat: repeat-x;
            padding-right: 6px;
            padding-left: 3px;
        }
        
        
        a.v1:visited, a.v1:active, a.v1:link
        {
            font-size: 14px;
            color: #000;
            text-decoration: none;
        }
        a.v1:hover
        {
            font-size: 14px;
            color: #005500;
            text-decoration: none;
        }
    </style>
</head>
<body class="easyui-layout">
    <noscript>
        <div style="position: absolute; z-index: 100000; height: 2046px; top: 0px; left: 0px;
            width: 100%; background: white; text-align: center;">
            <img src="/images/noscript.gif" alt='抱歉，请开启脚本支持！' />
        </div>
    </noscript>
    <div region="north" split="true" border="false" style="overflow: hidden; height: 76px;
        line-height: 20px; background-color: green; font-family: 微软雅黑,黑体">
        <div id="mainlogo">
            <%--<img src="/images/main.gif" width="575" height="72" alt="此处放置一个带有系统名称的图片，大小为“500x72”" />--%>
        </div>
        <div id="center">
        </div>
        <div id="mainctrl">
            <div class="wel">
                <%= ViewData["PersonName"]%>
                ,欢迎您的光临！</div>
            <div class="ctr">
                <li>
                    <img src="/images/yuanjiao.png" alt="" /></li>
                <li><a href="javascript:void(0);">
                    <img src="/images/mimaxiugai.gif" alt="" border="0" /></a></li>
                <li class="zi"><a href="javascript:void(0);" class="ChangePassword" >修改密码</a></li>
                <li><a href="javascript:void(0);">
                    <img src="/images/anquantuichu.gif" alt="" border="0" /></a></li>
                <li class="zi"><a href="javascript:void(0);" id="loginOut" >安全退出</a></li>
                <li><a href="javascript:void(0);">
                    <img src="/images/bangzhu.gif" border="0"></a></li>
                <li class="zi"><a href="http://www.NkjSoft.com" >帮助中心</a></li>
                <li><a href="javascript:void(0);">
                    <img src="/images/duanxinxi.gif" alt="" border="0" /></a></li>
                <li class="zi"><a href="javascript:void(0);" >短消息（0）</a></li>
            </div>
        </div>
    </div>
    <div region="west" hide="true" split="true" title="导航菜单" style="width: 180px;" id="west">
        <div class="easyui-accordion" fit="true" border="false">
            <%= ViewData["Menu"] %>
        </div>
    </div>
    <div id="mainPanle" region="center" style="background: #eee; overflow-y: hidden;">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
        </div>
    </div>
    <div region="south" split="true" style="height: 29px;">
        <div style="padding: 0px; margin-left: 50%;">
            技术支持 NkjSoft.com
        </div>
        <div id="win" class="easyui-window" title="My Window" closed="true" data-options="left:'100px',top:'0px'"
            style="width: 300px; height: 100px; padding: 5px;">
            adfgasd
        </div>
    </div>
    <div id="mm" class="easyui-menu" style="width: 150px;">
        <div id="mm-tabupdate">
            刷新</div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabclose">
            关闭</div>
    </div>
    <div id="myWindow">
    </div>
</body>
</html>
