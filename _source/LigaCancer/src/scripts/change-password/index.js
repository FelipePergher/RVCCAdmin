"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initChangePasswordForm();
    }

    function initChangePasswordForm() {
        $.validator.unobtrusive.parse("#changePasswordForm");

        global.eyePassword();

        $("#changePasswordForm").off("submit").submit(function (e) {
            e.preventDefault();
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
                $.post("ChangePassword", form.serialize())
                    .done(function (data) {
                        global.swalWithBootstrapButtons.fire("Sucesso", "Senha alterada com sucesso.", "success");
                    })
                    .fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

}());

