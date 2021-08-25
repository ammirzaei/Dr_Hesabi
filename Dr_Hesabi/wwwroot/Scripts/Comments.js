function OnSuccessComment() {
    $("#Text").val("");
    $("#Text").text("");
    $("#CommentDanger").text("");
    var command = $("#CommentID").val();
    if (command !== "0") {
        $("#CommentSuccess").text("نظر شما با موفقیت ویرایش شد").delay(3000).fadeOut('slow');
    } else {
        $("#CommentSuccess").text("نظر شما با موفقیت ثبت شد").delay(3000).fadeOut('slow');
    }
    $("#CommentID").val("");
    $(".btn-success").val("ثبت نظر");
    grecaptcha.reset();
}
function OnFaildComment() {
    $("#CommentSuccess").text("");
    var command = $("#CommentID").val();
    if (command !== "0") {
        $("#CommentDanger").text("نظر شما ویرایش نشد").delay(3000).fadeOut('slow');
    } else {
        $("#CommentDanger").text("نظر شما ثبت نشد").delay(3000).fadeOut('slow');
    }
}
function EditComment(id, text) {
    $("#CommentID").val(id);
    $("#Text").val(text);
    $("#Text").text(text);
    $(".btn-success").val("ویرایش نظر");
    $("html,body").delay(0).animate({ scrollTop: $(".View-BodyComment").offset().top }, 700);
}
function DeleteComment(id) {
    $.get("/Panel/DeleteComment/" + id, function (res) {
        $("#MyModal").modal();
        $("#myModalLabel").text("حذف نظر");
        $("#Modal-body").html(res);
    });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}
var IsActiveComment = false;
var scrollViewComment = $(".View-BodyComment").offset().top - $(".ViewBody-Main").offset().top + 150;
$(window).scroll(function () {
    if (IsActiveComment === false) {
        var Comments = $(window).scrollTop();
        if (Comments > scrollViewComment) {
            var ID = $("#ID").val();
            var Method = $("#Method").val();
            $.get("/Panel/ListComments/" + ID + "?Method=" + Method,
                function (res) {
                    $("#ListComments").html(res);
                });
           //$.get("/Panel/CreateComment/" + ID + "?Method=" + Method,
           //     function (res) {
           //         $("#CreateComment").html(res);
           //     });
            IsActiveComment = true;
        }
    }
});