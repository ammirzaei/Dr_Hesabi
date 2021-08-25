function CreateQuestions(id) {
    $.get("/Admin/SurveysQuestions/Create/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("افزودن سوال");
        $("#Modal-body").html(res);
    });
}
function EditQuestions(id) {
    $.get("/Admin/SurveysQuestions/Edit/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("ویرایش سوال");
        $("#Modal-body").html(res);
    });
}
function DeleteQuestions(id) {
    $.get("/Admin/SurveysQuestions/Delete/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف سوال");
        $("#Modal-body").html(res);
    });
}