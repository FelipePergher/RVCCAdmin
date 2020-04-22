"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initResendEmailForm();
    }

    function initResendEmailForm() {
        $.validator.unobtrusive.parse("#resendEmailForm");

        global.eyePassword();

        $("#resendEmailForm").submit(function (e) {
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
            }
        });
    }

}());