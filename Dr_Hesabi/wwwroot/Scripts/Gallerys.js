function Create(id) {
    $.get("/Admin/Gallerys/Create/"+id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن");
        $("#Modal-body").html(res);
    });
}
function Edit(id) {
    $.get("/Admin/Gallerys/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش");
        $("#Modal-body").html(res);
    });
}
function Delete(id) {
    $.get("/Admin/Gallerys/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف");
        $("#Modal-body").html(res);
    });
}