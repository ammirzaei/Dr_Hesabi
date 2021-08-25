$("input[type='checkbox'].role").on('ifChecked',
    function () {
        var role = $(".role_" + this.value).text().trim();
        if (role === "معلم") {
            $("#role-teacher").show();
        }
    });
$("input[type='checkbox'].role").on('ifUnchecked',
    function () {
        var role = $(".role_" + this.value).text().trim();
        if (role === "معلم") {
            $("#role-teacher").hide();
        }
    });
var condition = $("#role-condition").val();
if (condition === "true") {
    $("#role-teacher").show();
}