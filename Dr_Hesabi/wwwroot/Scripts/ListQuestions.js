function CreateQuestion(id) {
    $.get("/Teacher/Questions/Create/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن سوال");
        $("#Modal-body").html(res);
    });
}
function EditQuestion(id) {
    $.get("/Teacher/Questions/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش سوال");
        $("#Modal-body").html(res);
    });
}
function DeleteQuestion(id) {
    $.get("/Teacher/Questions/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف سوال");
        $("#Modal-body").html(res);
    });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}