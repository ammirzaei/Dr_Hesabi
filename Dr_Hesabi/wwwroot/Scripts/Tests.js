var endDateTime = $("#EndDateTimeS").val();
var startDateTime = $("#StartDateTimeS").val();
var testCommand = false;
function RemainingTime() {
    var dateTimeS = GetDateTimeNowInSecond();
    if (testCommand === false)
        CommandTest(dateTimeS);

    if (testCommand === true) {
        var over = parseInt(endDateTime) - parseInt(dateTimeS);
        if (over === 0 || over < 0) {
            window.location.assign("/");
        }

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
            $("#overTime").text(hour + ':' + minute + ':' + second + ' زمان باقی مانده تا پایان ');
        } else {
            $("#overTime").text(minute + ':' + second + ' زمان باقی مانده تا پایان ');
        }
    } else {
        var over = parseInt(startDateTime) - parseInt(dateTimeS);
        if (over === 0 || over < 0) {
            window.location.assign("/");
        }
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
            $("#overTime").text(hour + ':' + minute + ':' + second + ' زمان باقی مانده تا شروع ');
        } else {
            $("#overTime").text(minute + ':' + second + ' زمان باقی مانده تا شروع ');
        }
    }
    SetBgColorTime();
};

function SetBgColorTime() {
    if (testCommand === true) {
        $(".Tests-title").css('background-color', '#2a9d8f');
        $("#Participating").removeClass('disabled');
    } else {
        $(".Tests-title").css('background-color', '#e76f51');
        $("#Participating").addClass('disabled');
    }
}
function CommandTest(dateTimeS) {
    if (startDateTime >= dateTimeS && endDateTime <= dateTimeS)
        testCommand = false;
    else if (startDateTime <= dateTimeS && endDateTime >= dateTimeS)
        testCommand = true;
}
function GetDateTimeNowInSecond() {
    var date = new Date();
    var dateTime = (date.getHours() * 3600) + (date.getMinutes() * 60) + date.getSeconds();
    return dateTime;
}

RemainingTime();
setInterval(function () {
    RemainingTime();
},
    1000);