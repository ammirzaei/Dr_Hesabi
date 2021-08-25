$("#Descritive").hide();
$("select").change(function () {
    Method();
});
function Method() {
    if ($("select :selected").val() === "تشریحی") {
        $("#Descritive").show();
    }
    else {
        $("#Descritive").hide();
        $("#ErrorMethodInput").text("");
    }
}
Method();
$("form").submit(function (e) {
    if ($("select :selected").val() === "تشریحی") {
        var Radios = $("input[type='radio']").select();
        if (Radios[1].checked === false && Radios[0].checked === false) {
            $("#ErrorMethodInput").text("لطفا نوع دریافت پاسخ را انتخاب کنید");
            e.preventDefault();
        } else {
            $("#ErrorMethodInput").text("");
        }
    }
    if ($("#Score").val() === "") {
        $("#ErrorScore").text("لطفا نمره سوال را وارد نمایید");
        e.preventDefault();
    }
    if ($("#Score").val().includes("/")) {
        $("#ErrorScore").text("لطفا اعشار را با '.' وارد نمایید");
        e.preventDefault();
    }
    if ($("#Score").val() !== "" && !$("#Score").val().includes("/")) {
        $("#ErrorScore").text("");
    }
});