function CreateStaff(id) {
    $.get("/Admin/Staffs/Create/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("افزودن");
            $("#Modal-body").html(res);
        });
}
function onSuccess() {
    $("#MyModal").modal('hide');
}
function EditStaff(id) {
    $.get("/Admin/Staffs/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش");
        $("#Modal-body").html(res);
    });
}
function DeleteStaff(id) {
    $.get("/Admin/Staffs/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف");
        $("#Modal-body").html(res);
    });
}
function ChangeStaffParent(id, isNative, isChange) {
    $.get("/Admin/Staffs/SetStaffToParent/" + id + "?isNative=" + isNative, function (res) {
        $("#MyModal").modal();
        if (isChange)
            $("#myModalLabel").text("تغییر سرگروه");
        else
            $("#myModalLabel").text("تعیین سرگروه");
        $("#Modal-body").html(res);
    });
}
function SetUserToStaff(id) {
    $.get("/Admin/Staffs/SetUserToStaff/" + id,
        function(res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("تعیین کاربر");
            $("#Modal-body").html(res);
        });
}