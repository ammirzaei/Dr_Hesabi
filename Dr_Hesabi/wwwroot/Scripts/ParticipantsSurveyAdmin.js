function ShowProfile(id) {
    $.get("/Admin/Users/Profile/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("پروفایل");
            $("#Modal-body").html(res);
        });
}
function DeleteParticipant(id) {
    $.get("/Admin/Surveys/DeleteParticipant/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف رأی");
        $("#Modal-body").html(res);
    });
}