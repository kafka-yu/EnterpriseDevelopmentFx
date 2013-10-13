<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysLog>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 日志
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
             <%--   <div class="display-label">
                      <%: Html.LabelFor(model => model.PersonId) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.PersonId) %>
                </div>
                <br style="clear: both;" />--%>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Message) %>：
                </div>
                <div class="textarea-box">
                    <%: Html.TextAreaFor(model => model.Message, new {  @readonly=true}) %>                  
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Result) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Result) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.MenuId) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MenuId) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Ip) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Ip) %>
                </div>
            <%--    <br style="clear: both;" />
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Remark) %>：
                </div>
                <div class="textarea-box">
                    <%: Html.TextAreaFor(model => model.Remark, new {  @readonly=true}) %>                  
                </div>  --%>    
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
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>

