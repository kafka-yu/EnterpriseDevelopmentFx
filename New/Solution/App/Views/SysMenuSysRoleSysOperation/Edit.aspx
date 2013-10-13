<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysMenuSysRoleSysOperation>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 角色菜单操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('SysMenuSysRoleSysOperation')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>      
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('SysMenuId','/Admin/SysMenu');">
                    <%: Html.LabelFor(model => model.SysMenuId) %>
                </a>
            </div>
            <div class="editor-field">
                <div id="checkSysMenuId">
                    <%  if(Model!=null)
                        {
                        if (null != Model.SysMenuId)                      
                        {%>
                    <table id="<%: Model.SysMenuId %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除"  title="点击删除" onclick="deleteTable('<%: Model.SysMenuId %>','SysMenuId');" src="/Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: Model.SysMenu.Name%>
                            </td>
                        </tr>
                    </table>
                    <%}}%>
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
                    <%  if(Model!=null)
                        {
                        if (null != Model.SysRoleId)                      
                        {%>
                    <table id="<%: Model.SysRoleId %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除"  title="点击删除" onclick="deleteTable('<%: Model.SysRoleId %>','SysRoleId');" src="/Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: Model.SysRole.Name%>
                            </td>
                        </tr>
                    </table>
                    <%}}%>
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
                    <%  if(Model!=null)
                        {
                        if (null != Model.SysOperationId)                      
                        {%>
                    <table id="<%: Model.SysOperationId %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除"  title="点击删除" onclick="deleteTable('<%: Model.SysOperationId %>','SysOperationId');" src="/Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: Model.SysOperation.Name%>
                            </td>
                        </tr>
                    </table>
                    <%}}%>
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

