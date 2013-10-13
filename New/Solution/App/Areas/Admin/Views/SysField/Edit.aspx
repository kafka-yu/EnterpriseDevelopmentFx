<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysField>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 数据字典
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('SysField')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MyTexts) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.MyTexts) %>
                <%: Html.ValidationMessageFor(model => model.MyTexts) %>
            </div>       
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('ParentId','/Admin/SysField');">
                    <%: Html.LabelFor(model => model.ParentId) %>
                </a>：
            </div>
            <div class="editor-field">
                <div id="checkParentId">
                    <% if(Model!=null)
                        {
                        if (!string.IsNullOrWhiteSpace(Model.ParentId))
                        {%>
                    <table  id="<%: Model.ParentId %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img  alt="删除"  title="点击删除" src="/Images/deleteimge.png" onclick="deleteTable('<%: Model.ParentId %>','ParentId');"/>
                            </td>
                            <td>
                                <%: Model.SysField2.MyTexts%>
                            </td>
                        </tr>
                    </table>
                    <%}}%>
                </div>
                <%: Html.HiddenFor(model => model.ParentId)%>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MyTables) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.MyTables) %>
                <%: Html.ValidationMessageFor(model => model.MyTables) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MyColums) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.MyColums) %>
                <%: Html.ValidationMessageFor(model => model.MyColums) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Sort) %>：
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Sort, new {  onkeyup = "isInt(this)", @class="text-box single-line" })%>
                <%: Html.ValidationMessageFor(model => model.Sort) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Remark) %>
                <%: Html.ValidationMessageFor(model => model.Remark) %>
            </div><%: Html.HiddenFor(model => model.CreateTime ) %><%: Html.HiddenFor(model => model.CreatePerson ) %>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">

        $(function () {
            

        });
              

    </script>
</asp:Content>

