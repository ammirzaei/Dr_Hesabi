function CreateChoice(id) {
    $.get("/Teacher/Choices/Create/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن گزینه");
        $("#Modal-body").html(res);
    });
}
function EditChoice(id) {
    $.get("/Teacher/Choices/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش گزینه");
        $("#Modal-body").html(res);
    });
}
function DeleteChoice(id) {
    $.get("/Teacher/Choices/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف گزینه");
        $("#Modal-body").html(res);
    });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}

function OnFaild() {
    $(".ErrorModal").html("لطفا در وارد کردن اطلاعات دقت کنید");

}