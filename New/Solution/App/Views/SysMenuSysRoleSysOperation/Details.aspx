<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysMenuSysRoleSysOperation>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 角色菜单操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                    
                <div class="display-label">
                      <%: Html.LabelFor(model => model.SysMenuId) %>
                </div>
                <div class="display-field">
                    <% if (Model.SysMenu != null && null!=(Model.SysMenu.Name))
                       { %>
                    <%: Model.SysMenu.Name%>
                    <%} %>
                </div>        
                <div class="display-label">
                      <%: Html.LabelFor(model => model.SysRoleId) %>
                </div>
                <div class="display-field">
                    <% if (Model.SysRole != null && null!=(Model.SysRole.Name))
                       { %>
                    <%: Model.SysRole.Name%>
                    <%} %>
                </div>        
                <div class="display-label">
                      <%: Html.LabelFor(model => model.SysOperationId) %>
                </div>
                <div class="display-field">
                    <% if (Model.SysOperation != null && null!=(Model.SysOperation.Name))
                       { %>
                    <%: Model.SysOperation.Name%>
                    <%} %>
                </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>

