function EditComment(id) {
    $.get("/Admin/Home/EditComment/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش نظر");
        $("#Modal-body").html(res);
    });
}
function DeleteComment(id) {
    $.get("/Admin/Home/DeleteComment/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف نظر");
        $("#Modal-body").html(res);
    });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}
function ShowProfile(id) {
    $.get("/Admin/Users/Profile/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("پروفایل");
            $("#Modal-body").html(res);
        });
}