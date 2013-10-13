

jQuery.ajaxSettings.traditional = true;

$(function () {
    if (typeof _initGrid != 'undefined' && _initGrid instanceof Function) {
        _initGrid();
    }

    InitSearchBox();
})

var commonDataGrid = "datagrid";

//初始化左侧
function InitSearchBox() {
    //$(".search-panel .control-list")
    ////.append('<li><a href="javascript:void(0);" class="l-btn search-button easyui-linkbutton easyui-linkbutton-blue"><span class="l-btn-left"><span class="l-btn-text">搜索</span></span></a></li>');
    //.append($('<a href="javascript:void(0);" onclick="flexiQuery()" class="search-button" data-options="">查 询</a>'
    //).addClass("easyui-linkbutton easyui-linkbutton-blue"));

    $(".search-button").click(function () {
        __searchProxy();
    });

    $(window).resize(function () {
        fixTheGridSize('#datagrid');
    });
}

function fixTheGridSize(id) {

    var clientHeight = window.parent.document.body.clientHeight;
    var clientWidth = document.body.clientWidth;
    var actualHeight = 0;

    var searchPanelHeight = $(".search-panel").height();
    var footerPanelHeight = 30;
    actualHeight = clientHeight - searchPanelHeight - footerPanelHeight - 81 - 30 - 26;

    $(id).datagrid('resize', {
        width: clientWidth,
        height: actualHeight
    });
}


function initialDataGrid(cols, setting) {
    var gridId = "";
    if (typeof setting == "undefined" || typeof setting.gridId == "undefined" || setting.gridId == null) {
        gridId = "#datagrid";
    }
    var multipleSelect = false;
    if (typeof setting != "undefined") {
        if (setting.multipleSelect != "undefined") {
            multipleSelect = setting.multipleSelect;
        }
    }

    if (multipleSelect == true) {
        var additional =  { field: "Id", title: "", checkbox: true, align: 'center' };

        cols[0].unshift(additional);
    }

    $(gridId).datagrid({
        checkOnSelect: true,
        selectOnCheck: true,
        striped:true,
        singleSelect: !multipleSelect,
        remoteSort: false,
        columns: cols,
        toolbar: '#toolbar',
        pagination: true,
        rownumbers: false,
        fitcolumns: true,
        onBeforeLoad: function (param) {
            onBuildParams(param);
        }
    });
    fixTheGridSize(gridId);
}

function columnSorter(a, b) {
    a = a.split('/');
    b = b.split('/');
    if (a[2] == b[2]) {
        if (a[0] == b[0]) {
            return (a[1] > b[1] ? 1 : -1);
        } else {
            return (a[0] > b[0] ? 1 : -1);
        }
    } else {
        return (a[2] > b[2] ? 1 : -1);
    }
}


function onBuildParams(param) {
    if (typeof buildParams != 'undefined' && buildParams instanceof Function) {
        buildParams(param);
    }
}

function __searchProxy(param) {
    if (typeof _DoSearch != 'undefined' && _DoSearch instanceof Function) {
        _DoSearch(param);
    } else {
        $(tryGetGrid()).datagrid('load');
    }
}

function tryGetGrid(gridId) {
    if (typeof gridId == "undefined" || gridId == null) {
        gridId = "#datagrid";
    }
    return gridId;
}

function DoSearch(args, gridId) {
    if (typeof gridId == "undefined" || gridId == null) {
        gridId = "#datagrid";
    }

    $(gridId).datagrid('load', args);
}

function buildUpParams(param) {
    // <input data-setting="" type="text"/>

    var $paramsControls = $("[data-setting-field]");
    var queryParams = [];


    $paramsControls.each(function (index, it) {
        var $it = $(it);
        queryParams.push({
            Field1: $it.attr("data-setting-field"),
            Field2: $it.attr("data-setting-field-to"),
            Field1Type: $it.attr("data-setting-type"),
            Value1: $it.val(),
            Value2: $it.attr("data-setting-field-to-value"),
            Operation: $it.attr("data-setting-field-opt")
        });
    });
    param.queryParams = $.toJSON(queryParams);

}

//用于MVC参数适配JavaScript闭包函数
//2013-7-17 devotion 创建
/*
使用方式如下：
                $.ajax({
                    url: "@Url.Action("AjaxTest")",
                    data: mvcParamMatch(sendData),//在此转换json格式，用于mvc参数提交
                    dataType: "json",
                    type: "post",
                    success:function(result) {
                        alert(result.Message);
                    }
                });
*/