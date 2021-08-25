function ChangeCondition(id) {
    $.get("/Teacher/Questions/ChangeCondition/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("تغییر وضعیت");
            $("#Modal-body").html(res);
        });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}

function OnFaild() {
    $("#Error").text("لطفا وضعیت پاسخ را غیر از در انتظار قرار دهید");
}
function ShowUser(id) {
    $.get("/Teacher/Questions/Users/" + id,
        function(res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("اطلاعات کاربر");
            $("#Modal-body").html(res);
        });
}