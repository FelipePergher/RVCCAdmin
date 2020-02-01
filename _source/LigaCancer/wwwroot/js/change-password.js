$(function () {
    initPage();
});

function initPage() {
    $("#changePasswordForm").submit(function (e) {
        e.preventDefault();
        let form = $(this);
        if (form.valid()) {
            $("#submitSpinner").show();
            $.post("ChangePassword", form.serialize())
                .done(function (data) {
                    swalWithBootstrapButtons.fire("Sucesso", "Senha alterada com sucesso.", "success");
                })
                .fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                })
                .always(function () {
                    $("#submitSpinner").hide();
                });
        }
    });

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
