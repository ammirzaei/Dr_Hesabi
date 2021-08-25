$("input.form-control , textarea.form-control,select.form-control").focusin(function () {
    $(this).css("border-color", "#ffab00");

});
$("input.form-control , textarea.form-control,select.form-control").focusout(function () {
    $(this).css("border-color", "#d3d2d2");
    if ($(this).val() === "") {
        $(this).css("border-color", "#ef5662");
    }
});
$(document).ready(function () {
    $("div.loading").css("display", "none");
    $("body").css("overflow", "auto");
    $("body").css("overflow-x", "hidden");
    console.clear();
});