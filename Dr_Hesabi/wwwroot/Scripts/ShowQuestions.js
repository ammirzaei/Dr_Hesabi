var Time = $("#Time").val();
function RemainingTime() {
    var date = new Date();
    var dateTime = (date.getHours() * 3600) + (date.getMinutes() * 60) + date.getSeconds();
    var over = parseInt(Time) - parseInt(dateTime);

    var minute = over / 60;
    var second = over - (parseInt(minute) * 60);
    if (minute > 60) {
        var hour = minute / 60;
        minute = parseInt(minute) - (parseInt(hour) * 60);
    }
    minute = ("0" + parseInt(minute)).slice(-2);
    second = ("0" + parseInt(second)).slice(-2);
    if (hour > 0) {
        hour = ("0" + parseInt(hour)).slice(-2);
        $("#TestInfo span").text(hour + ':' + minute + ':' + second + ' زمان باقی مانده ');
    } else {
        $("#TestInfo span").text(minute + ':' + second + ' زمان باقی مانده ');
    }
    if (over === 0 || over < 0) {
        Ultimate(true);
    }
}
RemainingTime();
setInterval(function () {
    RemainingTime();
},
    1000);
$("input[type='radio']").on('ifChecked',
    function () {
        var Choice = $(this).val();
        var QuestionID = $(this).attr('QuestionID');
        $.get("/Tests/SaveReplyOptional?ChoiceID=" + Choice, function (res) {
            Output(res, QuestionID);
        }).error(function () {
            location.reload();
        });
    });
function Ultimate(endDate) {
    var testID = $("#TestID").val();
    if (endDate === true) {
        $.get("/Tests/ExistTest/" + testID,
            function (res) {
                if (res === false) {
                    $("#TestInfo span").text("زمان شما به اتمام رسید");
                    setTimeout(2000);
                    window.location.assign("/Tests/Ultimate/" + testID);
                }
                setTimeout(500);
            });
    } else {
        window.location.assign("/Tests/Ultimate/" + testID).delay(1000);
    }
}
function SaveReplyDescriptive(id) {
    var text = $("#Text_" + id).val();
    if (text === "") {
        $("#ErrorText_" + id).text("پاسخ خود را وارد کنید").fadeIn('slow');
    } else {
        $("#ErrorText_" + id).text("").fadeOut('slow');
        $.get("/Tests/SaveReplyDescriptive?QuestionID=" + id + "&Text=" + text).done(function (res) {
            Output(res, id);
        }).error(function () {
            location.reload();
        });
    }
}
function Output(res, id) {
    if (res === 0) {
        var TestID = $("#Test_" + id);
        $(TestID).css({
            border: "1px solid #4caf50"
        });
        $("#ErrorText_" + id).text("").fadeOut('slow');
        $("#btn_" + id).fadeOut('slow');
    }
    if (res === 1) {
        $("#ErrorText_" + id).text("مشکلی به وجود آمده است").fadeIn('slow');
    }
    if (res === 2) {
        $("#ErrorText_" + id).text("زمان آزمون به پایان رسیده است").fadeIn('slow');
        Ultimate(true);
    }
    if (res === 3) {
        location.reload();
    }
    if (res === 4) {
        $("#ErrorText_" + id).text("لطفا فقط تصویر آپلود کنید").fadeIn('slow');
    }
    if (res === 5) {
        window.location.assign("/");
    }
    $("#WaitText_" + id).text("").fadeOut('slow');
}
$("textarea").focusin(function () {
    var questionid = $(this).attr('QuestionID');
    $("#btn_" + questionid).fadeIn('slow');
});
$("textarea").focusout(function () {
    var questionid = $(this).attr('QuestionID');
    $("#btn_" + questionid).fadeOut('slow');
});
$("input[type='file']").change(function () {
    var QuestionID = $(this).attr("QuestionID");
    $("#Form_" + QuestionID).submit();
});
$("form").submit(function (e) {
    e.preventDefault();
    var QuestionID = $(this).attr("QuestionID");
    var data = new FormData(this);
    data.append('file', this.files);
    $("#WaitText_" + QuestionID).text("لطفا صبر کنید").fadeIn('slow');
    $.ajax({
        url: "/Tests/SaveReplyDescriptive?QuestionID=" + QuestionID,
        type: 'Post',
        data: data,
        cache: false,
        contentType: false,
        processData: false
    }).done(function (res) {
        if (res === 0) {
            readURL(e.target[0], QuestionID);
        }
        Output(res, QuestionID);
    }).error(function () {
        location.reload();
    });
});
var htmlModal = $("#HtmlModal").html();
$("#Modal-body").html(htmlModal);
$("#myModalLabel").text("هشدار");
function readURL(input, id) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var img = $('#Img_' + id);
            img.attr('src', e.target.result);
            img.attr('width', 300);
            img.attr('height', 100);
        }
        reader.readAsDataURL(input.files[0]);
    }
    $("#WaitText_" + id).text("").fadeOut('slow');
}
function CloseModal() {
    $("#MyModal").modal('hide');
}