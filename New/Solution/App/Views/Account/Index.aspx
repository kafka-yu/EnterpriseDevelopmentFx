<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Models.LogOnModel>" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>登 录</title>
    <style type="text/css">
        body
        {
            font-family: 微软雅黑, Verdana;
        }
        fieldset
        {
            margin: 0px 0px 0px 213px;
            padding: 9px;
            border: 1px solid #CCC;
            width: 287px;
        }
        
        input[type="text"], input[type="password"]
        {
            font-size: 1.1em;
            height: 22px;
            width: 160px;
            border: 1px solid #CCC;
        }
        .editor-label
        {
            margin: 1em 0 0 0;
        }
        
        .personname input
        {
            background: url(/images/username_bg.gif) left no-repeat #FFF;
            border: 1px #ccc solid;
            padding-left: 28px;
        }
        .password input
        {
            background: url(/images/password_bg.gif) left no-repeat #FFF;
            border: 1px #ccc solid;
            padding-left: 28px;
        }
        .validatecode img
        {
            vertical-align: middle;
        }
        
        .validatecode input
        {
            vertical-align: middle;
            width: 94px;
            border: 1px #ccc solid;
            padding-left: 28px;
        }
        .field-validation-error
        {
            color: #ff0000;
        }
        .login
        {
            width: 545px;
            margin-top: 90px;
            margin-left: 25%;
        }
        .login .top
        {
            background: url(/images/login_top.gif) left top no-repeat;
            width: 545px;
            height: 86px;
        }
        .login .middle
        {
            background: url(/images/login_midbg.gif) left top repeat-y;
            width: 545px;
        }
        .login .middle .left
        {
            float: left;
            width: 204px !important;
            width: 194px;
            padding-left: 6px !important;
            padding-left: 0px;
            margin-left: 6px;
            overflow: hidden;
        }
        
        .submitcss
        {
            background: url(/images/buttonOn_login.gif) left top no-repeat;
            height: 21px;
            width: 109px;
            text-align: center;
            border: 0px;
            font-family: 微软雅黑,新宋体;
            font-size: 14px;
            cursor: pointer;
        }
        .login .bottom
        {
            background: url(/images/login_bottom.gif) left top no-repeat;
            width: 545px;
            height: 18px;
        }
    </style>
    <script type="text/javascript">
        /*在1.5版本中修改 预加载功能*/
        function preload() {

            var scr = document.createElement("script");
            scr.src = '<%: Url.Content("~/Scripts/jquery.min.js") %>';
            document.getElementsByTagName('head')[0].appendChild(scr);
//            var scr2 = document.createElement("script");
//            scr2.src = '<%: Url.Content("~/Res/easyui/jquery.easyui.min.js") %>';
//            document.getElementsByTagName('head')[0].appendChild(scr2);

            var pre = new Image(0, 0);
             pre.src = '<%: Url.Content("~/images/tabicons.png") %>';

            var lnk = document.createElement("link");
            lnk.rel = "stylesheet";
            lnk.type = "text/css";
            lnk.href = '<%: Url.Content("~/Res/easyui/themes/default/easyui.css") %>';
            document.getElementsByTagName('head')[0].appendChild(lnk);

            var lnk2 = document.createElement("link");
            lnk2.rel = "stylesheet";
            lnk2.type = "text/css";

            lnk2.href = '<%: Url.Content("~/Content/default.css") %>';
            document.getElementsByTagName('head')[0].appendChild(lnk2);
            var lnk3 = document.createElement("link");
            lnk3.rel = "stylesheet";
            lnk3.type = "text/css";

            lnk3.href = '<%: Url.Content("~/Res/easyui/themes/icon.css") %>';
            document.getElementsByTagName('head')[0].appendChild(lnk3);

            

        }

        function RefreshValidateCode(obj) {
            obj.src = "/Account/ValidateCode/" + Math.floor(Math.random() * 10000);
        } </script>
</head>
<%-- /*在1.5版本中修改*/--%>
<body onload="preload()">
如有无法登陆，请联系qq350020527,将数据初始化
<br/>
官方网站<a href="http://www.NkjSoft.com" target="_blank">www.NkjSoft.com</a> 
<br/>用户名 Admin，密码 123456 

    <form id="Form1" runat="server">
    <div class="login">
        <div class="top">
        </div>
        <div class="middle">
            <div class="left">
                <img alt="登录" src="/Admin/images/login_pic.gif"></div>
            <div class="right">
                <div class="right2">
                    <fieldset style="border-top: 0px; border-left: #e8e8e8 1px solid; border-right: #e8e8e8 1px solid;
                        border-bottom: #e8e8e8 1px solid;">
                        <div class="editor-label">
                            用户名：
                        </div>
                        <div class="personname">
                            <%: Html.TextBoxFor(m => m.PersonName)%>
                        </div>
                        <div class="editor-label">
                            密&nbsp;&nbsp; 码：
                        </div>
                        <div class="password">
                            <%: Html.PasswordFor(m => m.Password) %>
                        </div>
                        <div class="editor-label">
                            验证码：
                        </div>
                        <div class="validatecode">
                            <%: Html.TextBoxFor(m => m.ValidateCode, new { MaxLength = "4"})%>
                            <img alt="点击刷新验证码！" title="点击刷新验证码！" src="/Account/ValidateCode" style="cursor: pointer;"
                                onclick="RefreshValidateCode(this);" />
                        </div>
                        <p>
                            <input class="submitcss" type="submit" value="登录系统" />
                        </p>
                        <p>
                            <%: Html.ValidationMessageFor(m => m.PersonName) %><br />
                            <%: Html.ValidationMessageFor(m => m.Password) %>
                        </p>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="bottom">
        </div>
    </div>
    </form>
</body>
</html>
