(function ($) {
    $('#starRating').starRating({
        callback: function (value) {
            $("#Pen-Star").html(value);
            $("#Vote").val(value);
            $("#Error-Vote").text("").fadeOut(1000);
        }
    });
}(jQuery));
function AddVote() {
    var text = $("#Pen-Star").html();
    if (text === "0" || text === 0 || text === "") {
        $("#Error-Vote").text("لطفا رأی خود را انتخاب کنید").fadeIn();
    } else {
        $("form").submit();
    }
}
