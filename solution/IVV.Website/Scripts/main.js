﻿function submitData(data, url) {
    $.post(
        url,
        data,
        function (data) {
            $("p.validation-note").css("display", "none");
            $("input.textbox").css("background", "#fff")
                    .css("border-color", "#d7d7d7");

            if (data.HasFormError) {

                $.each(data.ErrorList, function (i, err) {
                    $("#" + err.ElementId)
                        .css("background", "#fee")
                        .css("border-color", "#f33");

                    $("#" + err.ElementId + " ~ p.validation-note")
                        .css("display", "block")
                        .text(err.ErrorMessage);
                });
            } else if (data.IsRedirect) {
                window.location = data.RedirectUrl;
            } else if (data.IsActionDone) {
                var d = $("#dialog");
                if (d.length > 0) {
                    if (data.IsActionSuccess) {
                        d.dialog("option", "title", "操作成功");                        
                        d.html(data.ActionMsg);
                    } else {
                        d.dialog("option", "title", "操作失败");
                        d.html(data.ActionMsg);
                    }
                    d.dialog("open");
                }
            }
        }
    );
}

function test() {

}

function submitForm(formId, url) {
    var formData = $("#" + formId).serialize();
    submitData(formData, url);
}

$(document).ready(function () {
    $("table.table tbody a[title='删除']").click(function () {
        if (!confirm("确认要删除吗？")) {
            return false;
        }
    });
    //全选
    $("table.table thead input[type='checkbox']").click(function () {
        
        var s = $(this).prop("checked"); 
        $("table.table tbody input[type='checkbox']").prop("checked",s);
    });
    $("#abtnDelAll").click(function () {
        var sl = $("table.table tbody input:checked");
        if (sl.length == 0) {
            alert("请至少选择一项");
        }
        else {
            if (confirm("您确认要全部删除吗？")) {
                var t = $(this).attr("data-t");
                var ids = [];
                sl.each(function () {
                    ids.push($(this).val());
                });
                $.post("/admin/delall.aspx?t=" + t + "&ids=" + ids, function (d) {
                    if (d.Result) {
                        window.location.href = window.location.href;
                    }
                });
            }
        }
    });

});