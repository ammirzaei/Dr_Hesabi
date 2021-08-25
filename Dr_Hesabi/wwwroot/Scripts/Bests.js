function Create(id) {
    $.get("/Admin/Bests/Create/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن");
        $("#Modal-body").html(res);
    });
}
function Edit(id) {
    $.get("/Admin/Bests/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش");
        $("#Modal-body").html(res);
    });
}
function Delete(id) {
    $.get("/Admin/Bests/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف");
        $("#Modal-body").html(res);
    });
}