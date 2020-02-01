$(function () {
    initPage();
});

function initPage() {
    $(".eyePassword").click(function () {
        let icon = $(this);
        if (icon.hasClass("fa-eye")) {
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
            $("#password").attr("type", "password");
        } else {
            icon.addClass("fa-eye").removeClass("fa-eye-slash");
            $("#password").attr("type", "text");
        }
    });
}
