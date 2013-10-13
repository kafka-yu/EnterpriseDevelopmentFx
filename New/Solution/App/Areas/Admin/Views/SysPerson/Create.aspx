<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<NkjSoft.DAL.SysPerson>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="NkjSoft.App.Models" %>
<%@ Import Namespace="Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
    创建 人员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('SysPerson')" value="返 回" />
        </legend>
        <div class="bigdiv">
            <div class="editor-label">
                <span style="margin: 2px 0 0 50px;">*</span>
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            <div class="editor-label">
                <span  style="margin: 2px 0 0 60px;">*</span>
                <%: Html.LabelFor(model => model.MyName) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.MyName) %>
                <%: Html.ValidationMessageFor(model => model.MyName) %>
            </div>
            <div class="editor-label">
                <span style="margin: 2px 0 0 60px;">*</span>
                <%: Html.LabelFor(model => model.Password) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Password) %>
                <%: Html.ValidationMessageFor(model => model.Password) %>
            </div>
            <div class="editor-label">
                <span style="margin: 2px 0 0 35px;">*</span>
                <%: Html.LabelFor(model => model.SurePassword) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.SurePassword) %>
                <%: Html.ValidationMessageFor(model => model.SurePassword) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Sex) %>：
            </div>
            <div class="editor-field">
                <%=Html.DropDownListFor(model => model.Sex,Models.SysFieldModels.GetSysField("SysPerson","Sex"),"请选择")%>
            </div>
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('SysDepartmentId','/Admin/SysDepartment');">
                    <%: Html.LabelFor(model => model.SysDepartmentId) %>
                </a>：
            </div>
            <div class="editor-field">
                <div id="checkSysDepartmentId">
                </div>
                <%: Html.HiddenFor(model => model.SysDepartmentId)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Position) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Position) %>
                <%: Html.ValidationMessageFor(model => model.Position) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MobilePhoneNumber) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.MobilePhoneNumber) %>
                <%: Html.ValidationMessageFor(model => model.MobilePhoneNumber) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PhoneNumber) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.PhoneNumber) %>
                <%: Html.ValidationMessageFor(model => model.PhoneNumber) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Province) %>：
            </div>
            <div class="editor-field">
                <%=Html.DropDownListFor(model => model.Province,Models.SysFieldModels.GetSysField("SysPerson","Province"),"请选择")%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.City) %>：
            </div>
            <div class="editor-field">
                <select id="City" name="City">
                </select>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Village) %>：
            </div>
            <div class="editor-field">
                <select id="Village" name="Village">
                </select>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Address) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Address) %>
                <%: Html.ValidationMessageFor(model => model.Address) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EmailAddress) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.EmailAddress) %>
                <%: Html.ValidationMessageFor(model => model.EmailAddress) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Remark) %>
                <%: Html.ValidationMessageFor(model => model.Remark) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.State) %>：
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonListFor(model => model.State,Models.SysFieldModels.GetSysField("SysPerson","State"),false) %>
            </div>
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('SysRoleId','/Admin/SysRole');">
                    <%: Html.LabelFor(model => model.SysRoleId) %>
                </a>：
            </div>
            <div class="editor-field">
                <div id="checkSysRoleId">
                    <% 
                        if (Model != null && !string.IsNullOrWhiteSpace(Model.SysRoleId))
                        {
                            foreach (var item23 in Model.SysRoleId.Split('^'))
                            {
                                string[] it = item23.Split('&');
                                if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                                {                        
                    %>
                    <table id="<%: item23 %>" class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item23  %>','SysRoleId');" src="/Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: it[1] %>
                            </td>
                        </tr>
                    </table>
                    <%}
                            }
                        }%>
                    <%: Html.HiddenFor(model => model.SysRoleId) %>
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $(function () {

            getCity("#City");
            $("#Province").change(function () { getCity("#City"); });

            getVillage("#Village");
            $("#City").change(function () { getVillage("#Village"); });


        });

        function getCity(City) {
            $(City).empty();
            $("<option></option>")
                    .val("")
                    .text("请选择")
                    .appendTo($(City));
            bindDropDownList(City, "#Province");
            $(City).change();
        }

        function getVillage(Village) {
            $(Village).empty();
            $("<option></option>")
                    .val("")
                    .text("请选择")
                    .appendTo($(Village));
            bindDropDownList(Village, "#City");
            $(Village).change();
        }
      

    </script>
</asp:Content>
