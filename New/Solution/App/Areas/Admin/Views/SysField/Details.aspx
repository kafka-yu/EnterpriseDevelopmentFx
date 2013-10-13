<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysField>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 数据字典
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
                <div class="display-label">
                      <%: Html.LabelFor(model => model.MyTexts) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MyTexts) %>
                </div>        
                <div class="display-label">
                      <%: Html.LabelFor(model => model.ParentId) %>：
                </div>
                <div class="display-field">
                    <% if (Model.SysField2 != null && null!=(Model.SysField2.MyTexts))
                       { %>
                    <%: Model.SysField2.MyTexts%>
                    <%} %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.MyTables) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MyTables) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.MyColums) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MyColums) %>
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
                      <%: Html.LabelFor(model => model.UpdateTime) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.UpdateTime) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.UpdatePerson) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.UpdatePerson) %>
                </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>

