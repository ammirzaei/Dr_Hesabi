function Create() {
    $.get("/Admin/Attachments/Create",
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("افزودن فایل");
            $("#Modal-body").html(res);
        });
}
function Delete(id) {
    $.get("/Admin/Attachments/Delete/" + id,
        function (res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("حذف فایل");
            $("#Modal-body").html(res);
        });
}
function ShowFile(id) {
    $.get("/Admin/Attachments/ShowFile/" + id,
        function(res) {
            $("#MyModal").modal();
            $("#myModalLabel").text("نمایش فایل");
            $("#Modal-body").html(res);
        });
}
function CopyLink(id, link) {
    var myDomain = $("#MyDomain").val();
    var result = myDomain + "/Images/Attachments/" + link;
    document.getElementById(id).defaultValue = result;
    var copyText = document.getElementById(id);
    copyText.select();
    copyText.setSelectionRange(0, 99999);
    document.execCommand("copy");
    alert("لینک با موفقیت کپی شد");
}