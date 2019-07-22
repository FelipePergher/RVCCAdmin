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

}
