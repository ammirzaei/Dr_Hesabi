function CreareParent(id, parentID) {
    $.get("/Teacher/Contents/CreateParent/" + id + "?parentID=" + parentID, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن مجموعه");
        $("#Modal-body").html(res);
    });
}
function EditParent(id) {
    $.get("/Teacher/Contents/EditParent/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش مجموعه");
        $("#Modal-body").html(res);
    });
}
function DeleteParent(id) {
    $.get("/Teacher/Contents/DeleteParent/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف مجموعه");
        $("#Modal-body").html(res);
    });
}