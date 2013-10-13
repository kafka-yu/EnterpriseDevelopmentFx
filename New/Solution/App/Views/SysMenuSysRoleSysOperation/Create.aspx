<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysMenuSysRoleSysOperation>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
      创建 角色菜单操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('SysMenuSysRoleSysOperation')" value="返回" />
        </legend>
        <div class="bigdiv">
            
        <div class="editor-label">
            <a class="anUnderLine" onclick="showModalOnly('SysMenuId','/Admin/SysMenu');">
                <%: Html.LabelFor(model => model.SysMenuId) %>
            </a>
        </div>
        <div class="editor-field">
            <div id="checkSysMenuId">            
            </div>
            <%: Html.HiddenFor(model => model.SysMenuId)%>
        </div>
        <div class="editor-label">
            <a class="anUnderLine" onclick="showModalOnly('SysRoleId','/Admin/SysRole');">
                <%: Html.LabelFor(model => model.SysRoleId) %>
            </a>
        </div>
        <div class="editor-field">
            <div id="checkSysRoleId">            
            </div>
            <%: Html.HiddenFor(model => model.SysRoleId)%>
        </div>
        <div class="editor-label">
            <a class="anUnderLine" onclick="showModalOnly('SysOperationId','/Admin/SysOperation');">
                <%: Html.LabelFor(model => model.SysOperationId) %>
            </a>
        </div>
        <div class="editor-field">
            <div id="checkSysOperationId">            
            </div>
            <%: Html.HiddenFor(model => model.SysOperationId)%>
        </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">

        $(function () {
            

        });
              

    </script>
</asp:Content>

