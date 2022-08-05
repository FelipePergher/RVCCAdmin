"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initResetForm();
    }

    function initResetForm() {
        $.validator.unobtrusive.parse("#resetPasswordForm");

        global.eyePassword();

        $("#resetPasswordForm").off("submit").submit(function () {
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
            }
        });
    }

}());