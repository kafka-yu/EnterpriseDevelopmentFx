<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysPerson>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 人员
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
                      <%: Html.LabelFor(model => model.MyName) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MyName) %>
                </div>      
                 
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Sex) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Sex) %>
                </div>        
                <div class="display-label">
                      <%: Html.LabelFor(model => model.SysDepartmentId) %>：
                </div>
                <div class="display-field">
                    <% if (Model.SysDepartment != null && null!=(Model.SysDepartment.Name))
                       { %>
                    <%: Model.SysDepartment.Name%>
                    <%} %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Position) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Position) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.MobilePhoneNumber) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.MobilePhoneNumber) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.PhoneNumber) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.PhoneNumber) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Province) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Province) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.City) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.City) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Village) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Village) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Address) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Address) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.EmailAddress) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.EmailAddress) %>
                </div>      
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Remark) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Remark) %>
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
                <div class="display-label">
                      <%: Html.LabelFor(model => model.SysRoleId) %>：
                </div>
                <div class="display-field">            
                    <% string ids23 = string.Empty;
                       foreach (var item23 in Model.SysRole)
                       {
                           ids23 += item23.Name;
                           ids23 += " , ";
                    %>               
                    <%}%>
                    <div class="display-field">
                        <%= ids23 %>   
                    </div>
                </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>

