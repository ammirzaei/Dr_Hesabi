function MoreData() {
    var take = $("#Take").val();
    take = +take + 12;
    var q = $("#TxtSearch").val();
    $.get("/Admin/Users/ListUserViewComponent?Take=" + take + "&q=" + q,
        function (res) {
            $("#ListUser").html(res);
        });
}
function ShowProfile(id) {
    $.get("/Admin/Users/Profile/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("پروفایل");
            $("#Modal-body").html(res);
        });
}
$("#Search").click(function () {
    var q = $("#TxtSearch").val();
    $.get("/Admin/Users/Search?q=" + q,
        function (res) {
            $("#ListUser").html(res);
        });
}
);
function ShowConditionProfile(id) {
    $.get("/Admin/Users/ConditionProfile/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text(" وضعیت پروفایل");
            $("#Modal-body").html(res);
        });
};
function ChangeCondition(id, command) {
    var take = $("#Take").val();
    var q = $("#TxtSearch").val();
    $.get("/Admin/Users/ChangeConditionProfile/" + id + "?command=" + command + "&Take=" + take + "&q=" + q,
        function (res) {
            $("#ListUser").html(res);
            $("#MyModal").modal('hide');
        });
};

function ShowConditionProfileRequest(id) {
    $.get("/Admin/Users/ConditionProfileRequest/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("درخواست ویرایش پروفایل");
            $("#Modal-body").html(res);
        });
};

function ChangeConditionProfileRequest(id, command) {
    var take = $("#Take").val();
    var q = $("#TxtSearch").val();
    $.get("/Admin/Users/ChangeConditionProfileRequest/" + id + "?command=" + command + "&Take=" + take + "&q=" + q, function (res) {
        $("#ListUser").html(res);
        $("#MyModal").modal('hide');
    });
}