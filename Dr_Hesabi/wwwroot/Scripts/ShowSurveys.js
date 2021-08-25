function SaveVote(id) {
    $.get("/Surveys/VoteQuestion?id=" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("ثبت رأی");
            $("#Modal-body").html(res);
        });
}
function OnSuccessSaveVote() {
    $("#MyModal").modal('hide');
    $("#Error-Vote").text("");
    $("#Pen-Star").html("");
    $("html,body").delay(0).animate({ scrollTop: $("#ListSurveysQuestions").offset().top }, 700);
}
function OnFaildSaveVote(res) {
    $("#Modal-body").html(res);
}