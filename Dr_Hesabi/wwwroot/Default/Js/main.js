$(".Menu").click(function () {
    $(this).find('ul').slideToggle(400);
    if ($(this).hasClass("active-Menu")) {
        $(this).removeClass("active-Menu");
    } else {
        $(this).addClass("active-Menu");
    }
});
$("input.form-control , textarea.form-control,select.form-control").focusin(function () {
    $(this).css("border-color", "#ffab00");
});
$("input.form-control , textarea.form-control,select.form-control").focusout(function () {
    $(this).css("border-color", "#d3d2d2");
    if ($(this).val() === "") {
        $(this).css("border-color", "#ef5662");
    }
});
$("#Account").click(function (e) {
    $(this).find('ul').fadeToggle('slow');
    if ($("#Account-title .mdi-chevron-down").hasClass("rotate")) {
        $("#Account-title .mdi-chevron-down").removeClass("rotate");
        $("#Account-title .mdi-chevron-down").addClass("rotate-re");
        $("#Account-title").removeClass('border-account');
    } else {
        $("#Account-title .mdi-chevron-down").addClass("rotate");
        $("#Account-title .mdi-chevron-down").removeClass("rotate-re");
        $("#Account-title").addClass('border-account');
    }
});
$("#Account ul a").click(function (e) {
    $("#Account ul").fadeIn('slow');
});

/// Start Navbar
$("#Navbar .menu").click(function () {
    var e = $(this).parent('li');
    var find = e.find("ul");
    find.slideToggle();
    if ($("#Navbar .menu .mdi-chevron-down").hasClass("rotate")) {
        $("#Navbar .menu .mdi-chevron-down").removeClass("rotate");
        $("#Navbar .menu .mdi-chevron-down").addClass("rotate-re");
    } else {
        $("#Navbar .menu .mdi-chevron-down").addClass("rotate");
        $("#Navbar .menu .mdi-chevron-down").removeClass("rotate-re");
    }
});
$("#Navbar .MenuItem a").click(function () {
    var e = $(this).parent('li');
    var find = e.parent("ul");
    find.slideToggle();
});
$("#navbar-menu a").click(function () {
    NavbarCommand();
});
$("#navbar-title a").click(function () {
    NavbarCommand();
});
$("#Navbar-closing").click(function () {
    NavbarCommand();
});
function NavbarCommand() {
    if ($("#Navbar >ul").hasClass('openNav')) {
        $("#Navbar >ul").addClass("closeNav");
        $("#Navbar >ul").removeClass("openNav");
        $("body").css("overflow", "auto");
        $("#Navbar-closing").css("display", "none");
    } else {
        $("#Navbar >ul").addClass("openNav");
        $("#Navbar >ul").removeClass("closeNav");
        $("body").css("overflow", "hidden");
        $("#Navbar-closing").css("display", "block");
    }
}
/// End Navbar 
/// start slider
var slideIndex = 0;
function Slider(type) {
    var Slide_Items = $("#Slider_Items").children('.Slide_Item');
    var lengthSlide = $(".Slide_Item").length;
    if (type === "Next") {
        slideIndex++;
        if (slideIndex === lengthSlide) {
            slideIndex = 0;
        }
    } else {
        if (slideIndex == 0) {
            slideIndex = lengthSlide - 1;
        } else {
            slideIndex--;
        }
    }
    $(".Slide_Item").removeClass('active');
    Slide_Items.eq(slideIndex).toggleClass('active');
};
var autorun = setInterval(function () {
    Slider('Next');
}, 10000);
function ChangeAutoSlider() {
    clearInterval(autorun);
    autorun = setInterval(function () {
        Slider('Next');
    }, 10000);
}
$(".Slider_Left").click(function () {
    Slider("Prev");
    ChangeAutoSlider();
});
$(".Slider_Right").click(function () {
    Slider("Next");
    ChangeAutoSlider();
});
/// end slider
/// start lazy load map
LazyLoadMap();
$(window).scroll(function () {
    LazyLoadMap();
});
function LazyLoadMap() {
    if ((document.documentElement.scrollTop + window.innerHeight) >= $("#thisMap").offset().top) {
        $.get("/Home/Map",
            function (res) {
                $("#thisMap").html(res);
                $(window).off('scroll');
            });
    }
    $("body").css("overflow-x", "hidden");
    console.clear();
}
/// end lazy load map
setInterval(function () {
    DateNow();
}, 1000);
function DateNow() {
    var date = new Date();
    var hour = ("0" + date.getHours()).slice(-2);
    var minute = ("0" + date.getMinutes()).slice(-2);
    var second = ("0" + date.getSeconds()).slice(-2);
    var time = hour + ":" + minute + ":" + second;
    $("#DateNow span").text(time);
};
$(".scroll-top").click(function () {
    $("html,body").delay(0).animate({ scrollTop: $("body").offset().top }, 700);
});
$(document).ready(function () {
    $("div.loading").css("display", "none");
    $("body").css("overflow", "auto");
    $("body").css("overflow-x", "hidden");
    console.clear();
});