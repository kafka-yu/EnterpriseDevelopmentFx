<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysDepartment>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 部门
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Name) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Name) %>
                </div>        
                <div class="display-label">
                      <%: Html.LabelFor(model => model.ParentId) %>：
                </div>
                <div class="display-field">
                    <% if (Model.SysDepartment2 != null && null!=(Model.SysDepartment2.Name))
                       { %>
                    <%: Model.SysDepartment2.Name%>
                    <%} %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Address) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Address) %>
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

