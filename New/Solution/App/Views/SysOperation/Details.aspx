<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysOperation>" %>

<%@ Import Namespace="Common" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    详细 操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="button" onclick="window.location.href = 'javascript:history.go(-1)';"
                value="返回" />
        </legend>
        <div class="bigdiv">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Name) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Function) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Function) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Iconic) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Iconic) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Sort) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Sort) %>
            </div>
            <br style="clear: both;" />
            <div class="display-label">
                <%: Html.LabelFor(model => model.Remark) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Remark, new {  @readonly=true}) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.State) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.State) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CreateTime) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.CreateTime) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CreatePerson) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.CreatePerson) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SysMenuId) %>：
            </div>
            <div class="display-field">
                <% string ids10 = string.Empty;
                   foreach (var item10 in Model.SysMenu)
                   {
                       ids10 += item10.Name;
                       ids10 += " , ";
                %>
                <%}%>
                <%= ids10 %>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
