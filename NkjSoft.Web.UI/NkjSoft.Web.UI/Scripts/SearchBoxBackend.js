

jQuery.ajaxSettings.traditional = true;

$(function () {
    //InitLeftMenu();
    if (typeof _initGrid != 'undefined' && _initGrid instanceof Function) {
        _initGrid();
    }

    InitSearchBox();
})

var commonDataGrid = "datagrid";

//初始化左侧
function InitSearchBox() {

    $(".search-panel .control-list")
    .append('<li><a href="javascript:void(0);" class="l-btn search-button easyui-linkbutton easyui-linkbutton-blue"><span class="l-btn-left"><span class="l-btn-text">搜索</span></span></a></li>');

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
    actualHeight = clientHeight - searchPanelHeight - footerPanelHeight - 41 - 30 - 27;

    $(id).datagrid('resize', {
        width: clientWidth,
        height: actualHeight
    });
}


function initialDataGrid(cols, gridId) {
    if (typeof gridId == "undefined" || gridId == null) {
        gridId = "#datagrid";
    }

    $(gridId).datagrid({
        singleSelect: true,
        remoteSort: false,
        columns: cols,
        toolbar: '#toolbar',
        pagination: true,
        rownumbers: false,
        contentType: "application/json",
        fitcolumns: false,
        traditional: true,
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
var mvcParamMatch = (function () {
    var MvcParameterAdaptive = {};
    //验证是否为数组
    MvcParameterAdaptive.isArray = Function.isArray || function (o) {
        return typeof o === "object" &&
                Object.prototype.toString.call(o) === "[object Array]";
    };

    //将数组转换为对象
    MvcParameterAdaptive.convertArrayToObject = function (/*数组名*/arrName, /*待转换的数组*/array, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, arr) {
            for (var i in arr) {
                if (!MvcParameterAdaptive.isArray(arr[i]) && typeof arr[i] === "object") {
                    for (var j in arr[i]) {
                        if (MvcParameterAdaptive.isArray(arr[i][j])) {
                            func(name + "[" + i + "]." + j, arr[i][j]);
                        } else if (typeof arr[i][j] === "object") {
                            MvcParameterAdaptive.convertObject(name + "[" + i + "]." + j + ".", arr[i][j], obj);
                        } else {
                            obj[name + "[" + i + "]." + j] = arr[i][j];
                        }
                    }
                } else {
                    obj[name + "[" + i + "]"] = arr[i];
                }
            }
        }

        func(arrName, array);

        return obj;
    };

    //转换对象
    MvcParameterAdaptive.convertObject = function (/*对象名*/objName,/*待转换的对象*/turnObj, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, tobj) {
            for (var i in tobj) {
                if (MvcParameterAdaptive.isArray(tobj[i])) {
                    MvcParameterAdaptive.convertArrayToObject(i, tobj[i], obj);
                } else if (typeof tobj[i] === "object") {
                    func(name + i + ".", tobj[i]);
                } else {
                    obj[name + i] = tobj[i];
                }
            }
        }

        func(objName, turnObj);
        return obj;
    };

    return function (json, arrName) {
        arrName = arrName || "";
        if (typeof json !== "object") throw new Error("请传入json对象");
        if (MvcParameterAdaptive.isArray(json) && !arrName) throw new Error("请指定数组名，对应Action中数组参数名称！");

        if (MvcParameterAdaptive.isArray(json)) {
            return MvcParameterAdaptive.convertArrayToObject(arrName, json);
        }
        return MvcParameterAdaptive.convertObject("", json);
    };
})();