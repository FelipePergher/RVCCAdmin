$(function () {
    initPage();
});

function initPage() {
    $(".eyePassword").click(function () {
        let icon = $(this);
        let inputPassword = icon.siblings("input");
        if (icon.hasClass("fa-eye")) {
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
            inputPassword.attr("type", "password");
        } else {
            icon.addClass("fa-eye").removeClass("fa-eye-slash");
            inputPassword.attr("type", "text");
        }
    });
}
