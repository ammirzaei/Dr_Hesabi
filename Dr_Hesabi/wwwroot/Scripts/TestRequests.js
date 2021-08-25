function ShowUser(id) {
    $.get("/Teacher/Tests/Users/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("اطلاعات کاربر");
            $("#Modal-body").html(res);
        });
}
function ChangeCondition(id) {
    $.get("/Teacher/Tests/ChangeRequest/" + id, (res) => {
        $("#MyModal").modal();
        $("#myModalLabel").text("تغییر وضعیت");
        $("#Modal-body").html(res);
    });
}
function OnSuccess() {
    $("#MyModal").modal('hide');
}
function OnFaild() {
    $("#MyModal").modal('hide');
}
function CopyLink(id) {
    var address = $("#AddressRequest").val();
    document.getElementById("AddressRequest").defaultValue = address;
    var element = document.getElementById("AddressRequest");
    element.select();
    element.setSelectionRange(0, 99999);
    document.execCommand("copy");
    alert("لینک با موفقیت کپی شد");
}